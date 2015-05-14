using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Muffin.Core;
using Muffin.Core.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Muffin.Infrastructure
{
    /// <summary>
    /// Muffin Content Factory based on The Ditto published content model factory for creating strong typed models.
    /// But with support and optimized for muffin specific senarios
    /// </summary>
    public class MuffinPublishedContentModelFactory : IPublishedContentModelFactory
    {
        protected IMapper Mapper
        {
            get { return DependencyResolver.Current.GetService<IMapper>(); }
        }

        /// <summary>
        /// The type converter cache.
        /// </summary>
        private readonly Dictionary<string, Func<IPublishedContent, IPublishedContent>> _converterCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MuffinPublishedContentModelFactory"/> class.
        /// </summary>
        /// <param name="types">
        /// The <see cref="IEnumerable{Type}"/> to register for creation.
        /// </param>
        public MuffinPublishedContentModelFactory(IEnumerable<Type> types)
        {
            var converters = new Dictionary<string, Func<IPublishedContent, IPublishedContent>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var type in types.Where(x => typeof(IPublishedContent).IsAssignableFrom(x)))
            {
                // Fixes possible compiler issues caused by accessing closure in loop.
                //var innerType = type;
                //Func<IPublishedContent, IPublishedContent> func = x => x.As(innerType) as IPublishedContent;

                //var attribute = type.GetCustomAttribute<PublishedContentModelAttribute>(false);
                //var typeName = attribute == null ? type.Name : attribute.ContentTypeAlias;

                //if (!converters.ContainsKey(typeName))
                //{
                //    converters.Add(typeName, func);
                //}
            }

            _converterCache = converters.Count > 0 ? converters : null;
        }

        /// <summary>
        /// Creates a strongly-typed model representing a published content.
        /// </summary>
        /// <param name="content">The original published content.</param>
        /// <returns>
        /// The strongly-typed model representing the published content, or the published content
        /// itself it the factory has no model for that content type.
        /// </returns>
        public IPublishedContent CreateModel(IPublishedContent content)
        {
            if (content is IModel) //JW: Don't convert a content item that is already a typed model.
                return content;

            // HACK: [LK:2014-12-04] It appears that when a Save & Publish is performed in the back-office, the model-factory's `CreateModel` is called.
            // This can cause a null-reference exception in specific cases, as the `UmbracoContext.PublishedContentRequest` might be null.
            // Ref: https://github.com/leekelleher/umbraco-ditto/issues/14
            if (UmbracoContext.Current == null || UmbracoContext.Current.PublishedContentRequest == null)
            {
                return content;
            }

            if (_converterCache == null)
            {
                return content;
            }

            var contentTypeAlias = content.DocumentTypeAlias;
            Func<IPublishedContent, IPublishedContent> converter;

            return !_converterCache.TryGetValue(contentTypeAlias, out converter) ? 
                content :
                converter(content);
        }
    }

    /// <summary>
    /// Encapsulates extension methods for <see cref="PublishedContentModelFactoryResolver"/>.
    /// </summary>
    public static class PublishedContentModelFactoryResolverExtensions
    {
        /// <summary>
        /// Sets the factory resolver to resolve the given types using the <see cref="MuffinPublishedContentModelFactory"/>.
        /// </summary>
        /// <param name="resolver">
        /// The <see cref="PublishedContentModelFactoryResolver"/> this method extends.
        /// </param>
        /// <typeparam name="T">
        /// The base <see cref="Type"/> to retrieve classes that inherit from.
        /// </typeparam>
        public static void SetFactory<T>(this PublishedContentModelFactoryResolver resolver)
            where T : IPublishedContent
        {
            var types = PluginManager.Current.ResolveTypes<T>();
            var factory = new MuffinPublishedContentModelFactory(types);

            resolver.SetFactory(factory);
        }
    }
}