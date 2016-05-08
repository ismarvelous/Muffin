namespace Muffin.Core.Models
{
    public interface IUrlModel
    {
        string Url { get; }
    }

    public class UrlModel : IUrlModel
    {
        public string Url { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }

    public class LinkModel : UrlModel
    {
        public string Title { get; set; }
        public bool NewWindow { get; set; }
    }
}

