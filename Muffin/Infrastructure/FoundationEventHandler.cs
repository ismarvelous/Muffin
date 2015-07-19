using System;
using System.Web.Mvc;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using Muffin.Controllers;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

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
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(BaseController));
            //theme engine as default view engine!
            //ViewEngines.Engines.Insert(0, new ThemeViewEngine());

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
        
		private void ContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void ContentDeleted(IContentService sender, DeleteEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void ContentMoved(IContentService sender, MoveEventArgs<IContent> e)
		{
			ClearCache();
		}

		private void MediaSaved(IMediaService sender, SaveEventArgs<IMedia> e)
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