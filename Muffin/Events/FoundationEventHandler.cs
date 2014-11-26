using System;
using Muffin.Controllers;
using Muffin.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Models;
using DevTrends.MvcDonutCaching;
using Umbraco.Core.Logging;

namespace Muffin.Events
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

			IDependencyResolver resolver;
			InitializeAtStartup(umbracoApplication, applicationContext,
				out resolver);

			DependencyResolver.SetResolver(resolver);

			RegisterBaseRoutes();

			//todo: for flexibility it's maybe better to move this into the project / implementation itself.
			DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DynamicBaseController));

            //theme engine as default view engine!
            ViewEngines.Engines.Insert(0, new ThemeViewEngine());
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