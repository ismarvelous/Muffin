using Umbraco.Web;
using Umbraco.Web.Models;

namespace Muffin.Core.Models
{
    public class CroppedImageModel : IImageModel, INullModel
    {
        private readonly dynamic _source;
        private readonly string _json;
       
        public CroppedImageModel(string json)
        {
            _source = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
            _json = json;
        }

        public dynamic Crops //TODO: add support for manually defined crops.. like an array? Model.Image.Crops["alias"]
        {
            get { return _source.crops; }
        }

        public dynamic FocalPoint
        {
            get { return _source.focalPoint; }
        }

        public string Url
        {
            get { return _source.src; }
        }

        public override string ToString()
        {
            return this.Url;
        }

        public IUrlModel this[int width, int height]
        {
            get
            {
                return new UrlModel
                {
                    Url = Url.GetCropUrl(imageCropperValue: _json, height:height, width:width, imageCropMode:ImageCropMode.Max)
                };
            }
        }

        public IUrlModel this[string alias]
        {
            get
            {
                return new UrlModel
                {
                    Url = Url.GetCropUrl(imageCropperValue: _json, cropAlias: alias, useCropDimensions: true)
                };
            }
        }


        public string ToHtmlString()
        {
            return ToString();
        }

        public bool IsNull()
        {
            //TODO: implement IsNull()
            throw new System.NotImplementedException();
        }
    }
}
