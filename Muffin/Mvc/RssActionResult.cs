using System;
using System.Web.Mvc;
using System.Xml;
using Muffin.Core.Models;
using Umbraco.Web;

namespace Muffin.Mvc
{
    public class RssActionResult : ActionResult
    {
        protected IModel Content;

        public RssActionResult(IModel content)
        {
            Content = content;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.HttpContext.Request.Url != null)
            { 
                var settings = new XmlWriterSettings { Indent = true, NewLineHandling = NewLineHandling.Entitize };

                context.HttpContext.Response.ContentType = "text/xml";

                using (var writer = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
                {
                    // Begin structure
                    writer.WriteStartElement("rss");
                    writer.WriteAttributeString("version", "2.0");
                    writer.WriteStartElement("channel");

                    writer.WriteElementString("title", Content.HasProperty("MetaTitle") ? Content.GetPropertyValue<string>("MetaTitle") : Content.Name);
                    writer.WriteElementString("description", Content.HasProperty("MetaDescription") ? Content.GetPropertyValue<string>("MetaDescription") : Content.Name);
                    writer.WriteElementString("link", context.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));

                    // Individual item
                    foreach (var x in Content.Children)
                    {
                        writer.WriteStartElement("item");
                        writer.WriteElementString("title", x.HasProperty("MetaTitle") ? x.GetPropertyValue<string>("MetaTitle") : x.Name);

                        writer.WriteStartElement("description");
                        writer.WriteCData(x.HasProperty("MetaDescription") ? x.GetPropertyValue<string>("MetaDescription") : x.Name);
                        writer.WriteEndElement();
                        //http://msdn.microsoft.com/en-us/library/system.globalization.datetimeformatinfo.rfc1123pattern.aspx used for rss feed
                        writer.WriteElementString("pubDate", x.PublishDate.ToString("r"));

                        writer.WriteElementString("link", x.Url);
                        writer.WriteEndElement();
                    }

                    // End structure
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
        }
    }
}