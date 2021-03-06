﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Dynamics;

namespace Muffin.Core.Models
{
    /// <summary>
    /// Typed GridModel is based on the normal grid model, but the controls are using the IConverter Interface
    /// The Dynamic behaviour is used to allow compatibility with the dynamic json objects used by Umbraco.
    /// </summary>
    public class GridModel : DynamicObject
    {
        private string _json; //for performance reasons, do not constantly serialize and deserialize
        private string Json
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_json))
                    _json = JsonConvert.SerializeObject(this);

                return _json;
            }
        }

        private dynamic _source; //for performance reasons, do not constantly serialize and deserialize
        private dynamic Source
        {
            get { return _source ?? (_source = JsonConvert.DeserializeObject(Json)); }
        }

        public static GridModel Create(string json) //allow to have a parameterless constructor, to take advantage of the JsonConvert('ers)
        {
            var ret = JsonConvert.DeserializeObject<GridModel>(json);
            ret._json = json; //for performance reasons, set what we already have
            
            return ret;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) //support default umbraco way of working. (lower case, based on json source)
        {
            JObject obj = Source;
            JToken token;
            if (obj.TryGetValue(Char.ToLowerInvariant(binder.Name[0]) + binder.Name.Substring(1), out token)) //start with either lower or upper case.
                result = token;
            else
            {
                result = DynamicNull.Null;
            }
            
            return true;
        }

        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("sections")]
        public virtual ICollection<Section> Sections { get; set; }

        public override string ToString()
        {
            return Json;
        }
    }

    public class Section
    {
        [JsonProperty("grid")]
        public virtual int Grid { get; set; }

        [JsonProperty("rows")]
        public virtual ICollection<Row> Rows { get; set; }
    }

    public class Row
    {
        [JsonProperty("id")]
        public virtual string Id { get; set; }

        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("areas")]
        public virtual ICollection<Area> Areas { get; set; }
    }

    public class Area
    {
        [JsonProperty("grid")]
        public virtual int Grid { get; set; }

        [JsonProperty("controls")]
        public virtual ICollection<Control> Controls {get; set; }
    }

    public class Control : DynamicObject, IHtmlString
    {
        public ISiteRepository Repository { get; private set; }

        public Control()
            : this(DependencyResolver.Current.GetService<ISiteRepository>())
        {
        }

        public Control(ISiteRepository repository)
        {
            Repository = repository;
        }

        [JsonProperty("value")]
        public virtual dynamic SourceValue { get; set; }

        public dynamic Value
        {
            get { return Repository.GetPropertyValue(this); }
        }

        [JsonProperty("editor")]
        public virtual Editor Editor { get; set; }

        public string Alias
        {
            get { return Editor.Alias; }
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : SourceValue.ToString();
        }

        public string ToHtmlString()
        {
            return ToString();
        }
    }

    public class Editor
    {
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("alias")]
        public virtual string Alias { get; set; }

        [JsonProperty("view")]
        public virtual string View { get; set; }

        [JsonProperty("icon")]
        public virtual string Icon { get; set; }
    }
}
