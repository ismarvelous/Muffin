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
        ModelBase FindById(int id);
        ModelBase FindByUrl(string urlpath);
		IContent FindContentById(int id);

        IEnumerable<ModelBase> Find(string query);
		IEnumerable<ModelBase> FindAll();

		/// <summary>
		/// Returns a friendly url with domainname
		/// </summary>
		/// <param name="id">Content / Node Id</param>
		/// <returns></returns>
		string FriendlyUrl(int id);
		string FriendlyUrl(IPublishedContent content);

        object GetPropertyValue(MacroPropertyModel property);
        object GetPropertyValue(Control gridControl);
    }
}
