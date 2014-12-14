using System;
using System.Web.Mvc;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using Muffin.Controllers;
using Muffin.Core.Models;
using Muffin.Mvc;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.Routing;

namespace Muffin.Infrastructure
{
	public abstract class FoundationEventHandler : IApplicationEventHandler
	{
		public virtual void RegisterBaseRoutes()
		{
			//todo: add route for json and rss.
			RouteTable.Routes.MapRoute(
			"Sitemap",
			"sitemap",
			new
			{
				controller = "DynamicBase",
				action = "sitemap",
				id = UrlParameter.Optional
			});

            RouteTable.Routes.MapRoute(
            "Json",
            "json/{*path}",
            new
            {
                controller = "DynamicBase",
                action = "json",
                id = UrlParameter.Optional
            });

            RouteTable.Routes.MapRoute(
            "Rss",
            "rss/{*path}",
            new
            {
                controller = "DynamicBase",
                action = "rss",
                id = UrlParameter.Optional
            }); 
		}

		public virtual void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			//nothing
		}

		public virtual void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			//nothing
		}

		public virtual void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			ContentService.Published += ContentPublished;
			ContentService.UnPublished += ContentPublished;
			ContentService.Moved += ContentMoved;
			ContentService.Trashed += ContentMoved;
			ContentService.Deleted += ContentDeleted;
			MediaService.Saved += MediaSaved;

            //excecuting this region here allows developers to override defaults in InitializeAtStartup
            #region set defaults
            //User dynamic basecontroller as the default controller, if you like to use your own, you can change it here..
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DynamicBaseController));
            //theme engine as default view engine!
            ViewEngines.Engines.Insert(0, new ThemeViewEngine());

            var types = PluginManager.Current.ResolveTypes<ModelBase>();
            var factory = new PublishedContentModelFactory(types);
            PublishedContentModelFactoryResolver.Current.SetFactory(factory);

            #endregion

            IDependencyResolver resolver;
			InitializeAtStartup(umbracoApplication, applicationContext,
				out resolver);

			DependencyResolver.SetResolver(resolver);

			RegisterBaseRoutes();
		}

		public abstract void InitializeAtStartup(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext,
			out IDependencyResolver resolver);

		#region Cache management
        
		private void ContentPublished(global::Umbraco.Core.Publishing.IPublishingStrategy sender, global::Umbraco.Core.Events.PublishEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void ContentDeleted(IContentService sender, global::Umbraco.Core.Events.DeleteEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void ContentMoved(IContentService sender, global::Umbraco.Core.Events.MoveEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void MediaSaved(IMediaService sender, global::Umbraco.Core.Events.SaveEventArgs<IMedia> e)
		{
			ClearCache();
		}

		/// <summary>
		/// Remove all items from the cache.
		/// </summary>
		private void ClearCache()
		{
			try
			{
				var cm = new OutputCacheManager();
				cm.RemoveItems();
			}
			catch (Exception ex)
			{
				LogHelper.Error(typeof(FoundationEventHandler), string.Format("Unhandled exception while clearing the cache"), ex);
			}
		}

		#endregion
	}
}