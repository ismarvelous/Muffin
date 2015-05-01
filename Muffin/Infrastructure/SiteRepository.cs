using Examine;
using Examine.LuceneEngine.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using Muffin.Core;
using Muffin.Core.Models;
using Muffin.Infrastructure.Converters;
using umbraco.cms.businesslogic.macro;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Dynamics;
using System.Reflection;
using Examine.LuceneEngine.SearchCriteria;
using Muffin.Infrastructure.Models;
using Our.Umbraco.Ditto;
using Umbraco.Core;
using Umbraco.Web;

namespace Muffin.Infrastructure
{
    public class SiteRepository : ISiteRepository
    {
        //protected IMediaService MediaService;
        protected IContentService Service;
        protected IMacroService MacroService;
        protected Umbraco.Web.UmbracoContext CurrentContext;

        protected Umbraco.Web.UmbracoHelper Helper; //todo: try to avoid using the helper
        public string SearchProvidername { get; private set; }

        public SiteRepository(IContentService service, IMacroService macroService, Umbraco.Web.UmbracoContext ctx)
			: this(service, macroService, ctx, "ExternalSearcher") //use umbraco default searcher.
		{
		}

        public SiteRepository(IContentService service, IMacroService macroService, Umbraco.Web.UmbracoContext ctx, string searchProvidername)
        {
            Service = service;
            MacroService = macroService;
            SearchProvidername = searchProvidername;
            Helper = new Umbraco.Web.UmbracoHelper(ctx);
            CurrentContext = ctx;
        }

        /// <summary>
        /// Find a dynamic model based on the given query, when mainbody exist it is used for highlighting, otherwise nothing is highlighted.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<IModel> Find(string query)
        {
			//todo: check for PublishedContentExtensions Search and SearchChildren extensions..
            var searcher = ExamineManager.Instance.SearchProviderCollection[SearchProvidername];
            var criteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);
            var searchCriteria = criteria.RawQuery(query);
            var results = searcher.Search(searchCriteria);

            foreach (var item in results)
            {
                var fields = item.Fields.Where(f => f.Value.ToUpper()
                    .Contains(query.ToUpper())
                    || f.Value.ToUpper().Split(new[] { ' ' })
                        .Any(val => query.Split(new[] { ' ' })
                            .Contains(val))).ToDictionary(x => x.Key, x => x.Value);

                var field = fields.Keys.Contains("mainbody", StringComparer.InvariantCultureIgnoreCase) ? 
                    fields.FirstOrDefault(f => f.Key.ToLower() == "mainbody") : fields.FirstOrDefault();

                var searchHiglight = !String.IsNullOrEmpty(field.Key) ?
                    LuceneHighlightHelper.Instance.GetHighlight(field.Value, field.Key, ((LuceneSearcher)searcher).GetSearcher(), query) : String.Empty;

				var content = FindById<ModelBase>(item.Id); //returns dynamic null if this is not a content item (for example when this is a media item.

				if (content != null)
				{
					var ret = new DynamicSearchResultItem(content) {HighlightText = searchHiglight};

				    yield return ret;
				}
            }
        }

        public IEnumerable<TM> FindAll<TM>() where TM : class, IModel
        {
            var roots = Helper.ContentAtRoot() as IEnumerable<IPublishedContent>;
            return roots != null ? FindAll(roots.Select(n => n is IModel ? n as TM : n.As<TM>())) : null;
        }

        protected IEnumerable<TM> FindAll<TM>(IEnumerable<TM> rootNodes) where TM : class, IModel
		{
            var result = new List<TM>();
            foreach (var node in rootNodes)
            {
                result.Add(node);
                if (node.Children.Any()) result.AddRange(FindAll(node.Children().Select(n => n is IModel ? n as TM : n.As<TM>()))); //recursive..
            }

            return result;
		}

        ///<summary>
        ///Returns IModel as a Dynamic Proxy.
        ///</summary>
        ///<param name="id"></param>
        ///<returns></returns>
        public IModel FindById(int id)
        {
            return FindById<ModelBase>(id).AsDynamic();
        }

		public TM FindById<TM>(int id) where TM : class, IModel
		{
		    //var content = Helper.TypedContent(id);
		    var content = CurrentContext.ContentCache.GetById(id);

		    if (content is IModel)
		        return content as TM;

		    return content != null ? content.As<TM>() : null;
		}

        public IModel FindByUrl(string urlPath)
        {
            return FindByUrl<ModelBase>(urlPath).AsDynamic();
        }

        public TM FindByUrl<TM>(string urlpath) where TM : class, IModel
        {
            var content = CurrentContext.ContentCache.GetByRoute(urlpath);

            if (content is IModel)
                return content as TM;

            return content != null ? content.As<TM>() : null;
        }

        public IContent FindContentById(int id)
		{
			return Service.GetById(id);
		}

        public MediaModel FindMediaById(int id)
        {
            var media = Helper.TypedMedia(id);
            return media != null ? new MediaModel(media) : null;
        }

        public string Translate(string key)
		{
			return Helper.GetDictionaryValue(key);
		}
		
		public DynamicMacroModel FindMacroByAlias(string alias, int pageId, IDictionary<string, object> values)
		{
		    var macro = MacroService.GetByAlias(alias);

            var propertyValues = values.Join(macro.Properties,
                val => val.Key, prop => prop.Alias,
                (val, prop) => new MacroPropertyModel(val.Key, val.Value.ToString(), prop.EditorAlias, null));

			return new DynamicMacroModel(macro, propertyValues, this);
		}

		public string FriendlyUrl(IPublishedContent content)
		{
			return FriendlyUrl(content.Id);
		}

		public string FriendlyUrl(int id)
		{
		    return CurrentContext.UrlProvider.GetUrl(id, true);
		}

        #region Converted values

        public object GetPropertyValue(MacroPropertyModel property)
        {
            if (property != null && property.Value != null)
            {
                return ConvertPropertyValue(property.Type, property.Value);
            }

            return DynamicNull.Null;
        }


        public object GetPropertyValue(Control gridControl)
        {
            return ConvertPropertyValue(gridControl.Editor.Alias, gridControl.SourceValue);
        }

        public object ConvertPropertyValue(string editoralias, object value)
        {
            var assembly = typeof(IConverter).Assembly;
            var types = assembly.GetTypes().Where(type => type != typeof(IConverter) && typeof(IConverter).IsAssignableFrom(type)).ToList();

            foreach (var type in types)
            {
                var instance = (IConverter)Activator.CreateInstance(type);
                if (instance.IsConverter(editoralias))
                {
                    return instance.ConvertDataToSource(value);
                }
            }

            return value;
        }

        #endregion

    }
}