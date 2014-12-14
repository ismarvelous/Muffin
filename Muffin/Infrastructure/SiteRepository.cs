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
using Umbraco.Web;
using Umbraco.Core.Dynamics;
using System.Reflection;

namespace Muffin.Infrastructure
{
    public class SiteRepository : ISiteRepository
    {
        //protected IMediaService MediaService;
        protected IContentService Service;
        protected IMacroService MacroService;
        protected UmbracoContext CurrentContext;

        protected UmbracoHelper Helper;
        public string SearchProvidername { get; private set; }

        public SiteRepository(IContentService service, IMacroService macroService, UmbracoContext ctx)
			: this(service, macroService, ctx, "ExternalSearcher") //use umbraco default searcher.
		{
		}

        public SiteRepository(IContentService service, IMacroService macroService, UmbracoContext ctx, string searchProvidername)
        {
            Service = service;
            MacroService = macroService;
            SearchProvidername = searchProvidername;
            Helper = new UmbracoHelper(ctx);
            CurrentContext = ctx;
        }

        /// <summary>
        /// Find a dynamic model based on the given query, when mainbody exist it is used for highlighting, otherwise nothing is highlighted.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<ModelBase> Find(string query)
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

				var content = FindById(item.Id); //returns dynamic null if this is not a content item (for example when this is a media item.

				if (content != null)
				{
					var ret = new SearchResultItem(content) {HighlightText = searchHiglight};

				    yield return ret;
				}
            }
        }

		public IEnumerable<ModelBase> FindAll()
		{
			var roots = Helper.ContentAtRoot() as IEnumerable<IPublishedContent>;
		    return roots != null ? FindAll(roots.Select(n => new ModelBase(n))) : null;
		}


        public IEnumerable<ModelBase> FindAll(IEnumerable<ModelBase> rootNodes)
		{
            var result = new List<ModelBase>();
            foreach (var node in rootNodes)
            {
                result.Add(node);
                result.AddRange(node.Children().Select(n => new ModelBase(n)));
            }

            return result;
		}

		public ModelBase FindById(int id)
		{
		    var content = Helper.TypedContent(id);
		    return content != null ? new ModelBase(content) : null;
		}

        public ModelBase FindByUrl(string urlpath)
        {
            var content = CurrentContext.ContentCache.GetByRoute(urlpath);
            return content != null ? new ModelBase(content) : null;
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
			return Helper.NiceUrlWithDomain(id);
		}

        public object GetPropertyValue(MacroPropertyModel property) 
        {
            if (property != null)
            {
                var assembly = typeof(IConverter).Assembly;
                var types = assembly.GetTypes().Where(type => type != typeof(IConverter) && typeof(IConverter).IsAssignableFrom(type)).ToList();

                foreach (var type in types)
                {
                    var instance = (IConverter) Activator.CreateInstance(type);
                    if (instance.IsConverter(property.Type))
                    {
                        return instance.ConvertDataToSource(property.Value);
                    }
                }

                return property.Value ?? DynamicNull.Null;
            }
            else
                return DynamicNull.Null;
        }


        public object GetPropertyValue(Control gridControl)
        {
            var assembly = typeof(IConverter).Assembly;
            var types = assembly.GetTypes().Where(type => type != typeof(IConverter) && typeof(IConverter).IsAssignableFrom(type)).ToList();

            foreach (var type in types)
            {
                var instance = (IConverter)Activator.CreateInstance(type);
                if (instance.IsConverter(gridControl.Editor.Alias))
                {
                    return instance.ConvertDataToSource(gridControl.SourceValue);
                }
            }

            return gridControl.SourceValue;
        }
    }
}