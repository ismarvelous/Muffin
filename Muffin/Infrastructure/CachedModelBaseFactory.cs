//using System.Collections.Generic;
//using Our.Umbraco.Ditto;
//using Umbraco.Core.Models;
//using Umbraco.Core.Models.PublishedContent;

//namespace Muffin.Infrastructure
//{
//    /// <summary>
//    /// Wrapper for the DittoPublishedContentModelFactory, using a cached objects.
//    /// </summary>
//    public class CachedModelBaseFactory : IPublishedContentModelFactory
//    {
//        private readonly Dictionary<int, object> _cachedItems;
//        private readonly DittoPublishedContentModelFactory _publishedContentModelFactory;

//        public CachedModelBaseFactory(DittoPublishedContentModelFactory publishedContentModelFactory)
//        {
//            this._publishedContentModelFactory = publishedContentModelFactory;
//            _cachedItems = new Dictionary<int, object>();
//        }

//        public IPublishedContent CreateModel(IPublishedContent content)
//        {
//            if (!_cachedItems.ContainsKey(content.Id))
//            {
//                _cachedItems.Add(content.Id, _publishedContentModelFactory.CreateModel(content)); //HACK: to avoid overflow..
//                _cachedItems.Add(content.Id, _publishedContentModelFactory.CreateModel(content));
//            }

//            return _cachedItems[content.Id] as IPublishedContent;
//        }
//    }
//}
