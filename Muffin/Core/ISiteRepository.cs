using System.Collections.Generic;
using Muffin.Core.Models;
using umbraco.cms.businesslogic.macro;
using Umbraco.Core.Models;

namespace Muffin.Core
{
    public interface ISiteRepository
    {
		string Translate(string key);

        DynamicMacroModel FindMacroByAlias(string alias, int pageId, IDictionary<string, object> values);
		MediaModel FindMediaById(int id);

        IModel FindById(int id);
        TM FindById<TM>(int id) where TM : class, IModel;

        IModel FindByUrl(string urlPath);
        TM FindByUrl<TM>(string urlpath) where TM : class, IModel;

		IContent FindContentById(int id);

        IEnumerable<IModel> Find(string query);
        //IEnumerable<TM> Find<TM>(string query) where TM : class, IModel; 

        IEnumerable<TM> FindAll<TM>() where TM : class, IModel;

		/// <summary>
		/// Returns a friendly url with domainname
		/// </summary>
		/// <param name="id">Content / Node Id</param>
		/// <returns></returns>
		string FriendlyUrl(int id);
		string FriendlyUrl(IPublishedContent content);

        object GetPropertyValue(MacroPropertyModel property);
        object GetPropertyValue(Control gridControl);

        object ConvertPropertyValue(string editoralias, object value);

        T GetObject<T>(object key) where T : new();
        bool SaveObject<T>(T obj) where T : new();
        IEnumerable<T> GetObjects<T>(int page, int pageSize) where T : new();
    }
}
