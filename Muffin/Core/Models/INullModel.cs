namespace Muffin.Core.Models
{
	public interface INullModel
	{
		/// <summary>
		/// available for all dynamic objects, using this construction prevents 
		/// nullreference exceptions on non existing properties
		/// </summary>
		/// <returns></returns>
		bool IsNull();
	}
}
