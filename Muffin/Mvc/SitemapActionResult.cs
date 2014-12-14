using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using Muffin.Core;
using Muffin.Core.Models;

namespace Muffin.Mvc
{
	public class SitemapActionResult : ActionResult
	{
		public ISiteRepository Repository { get; private set; }
		private readonly IEnumerable<ModelBase> Pages;

		public SitemapActionResult(ISiteRepository repository) // IEnumerable<DynamicModel> pages, UmbracoHelper helper)
		{
			Repository = repository;
			Pages = repository.FindAll();
		}

		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "application/rss+xml";

			using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
			{
				writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

				if (Pages != null)
				{
					foreach (var page in Pages)
					{
						writer.WriteStartElement("url");
						writer.WriteElementString("loc", page.Url);

						writer.WriteElementString("lastmod", page.UpdateDate.ToString("yyyy-MM-dd"));

						writer.WriteElementString("changefreq", "daily"); //todo: set changefreq
						writer.WriteElementString("priority", "0.5"); //todo: set priority
						writer.WriteEndElement();
					}
				}

				writer.WriteEndElement();

				writer.Flush();
				writer.Close();
			}
		}
	}
}
