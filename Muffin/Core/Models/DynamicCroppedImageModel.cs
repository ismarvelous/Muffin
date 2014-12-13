using System;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Dynamics;
using Umbraco.Web;

namespace Muffin.Core.Models
{
    /// <summary>
    /// Typed CroppedImage model
    /// The Dynamic behaviour is used to allow compatibility with the dynamic json objects used by Umbraco.
    /// </summary>
    public class DynamicCroppedImageModel : DynamicObject, ICropImageModel, INullModel
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

        public static DynamicCroppedImageModel Create(string json) //allow to have a parameterless constructor, to take advantage of the JsonConvert('ers)
        {
            var ret = JsonConvert.DeserializeObject<DynamicCroppedImageModel>(json);
            return ret;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) //backwards compatibility support default umbraco way of working. (lower case, based on json source)
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

        [JsonProperty("crops")]
        public virtual dynamic Crops { get; set; }

        [JsonProperty("focalPoint")]
        public virtual dynamic FocalPoint { get; set; }

        private string _url;
        [JsonProperty("src")]
        public virtual string Url
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                    _url = Image; //hack: one of the 2 is used. (grid vs rest of the system)

                return _url;
            } 
            set { _url = value; }
        }

        private string _image;
        [JsonProperty("image")]
        public virtual string Image { //hack: grid is using image as the src, croppedimage is using src.
            get
            {
                if (string.IsNullOrEmpty(_image))
                    _image = Url;

                return _image;
            } 
            set { _image = value; } 
        }

        [JsonProperty("id")]
        public virtual int Id { get; set; } //only filled for grid images, otherwise not filled.

        public override string ToString()
        {
            return Url;
        }

        public virtual IUrlModel this[int width, int height]
        {
            get
            {
                return new UrlModel
                {
                    Url = Url.GetCropUrl(imageCropperValue: Json, height:height, width:width, preferFocalPoint:true)
                };
            }
        }

        public virtual IUrlModel this[string alias]
        {
            get
            {
                return new UrlModel
                {
                    Url = Url.GetCropUrl(imageCropperValue: Json, cropAlias: alias, useCropDimensions: true)
                };
            }
        }


        public virtual string ToHtmlString()
        {
            return ToString();
        }

        public virtual bool IsNull()
        {
            return false;
        }
    }
}
