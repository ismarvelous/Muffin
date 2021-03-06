﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Web;

namespace Muffin
{
	public class Application : UmbracoApplication
	{
		public override string GetVaryByCustomString(HttpContext context, string custom) //Create cache key..
        {
            var keys = custom.ToLower().Split(new char[] { ',', ';' });
            var ret = string.Empty;

            foreach (var key in keys)
            {

                if (key == "url")
                    ret = string.Format("url={0};{1}", context.Request.Url.AbsoluteUri, ret);
                
                //any future keys...
            }

            return !string.IsNullOrWhiteSpace(ret) ? ret : base.GetVaryByCustomString(context, custom);
        }
	}
}
