using System;
using System.Collections.Generic;
using Muffin.Core.Models;

namespace Muffin.Core.Models
{
	/// <summary>
	/// Interface for models that need to show paged result sets.
	/// </summary>
    public interface IPager
	{
		/// <summary>
		/// Add a custom implementation for the paged results..
		/// </summary>
		Func<IEnumerable<DynamicModel>> PagedResults { get; set; }

		/// <summary>
		/// returns all results, or when PagedResults is set; it retuns a selection of the results.
		/// </summary>
		IEnumerable<DynamicModel> Results { get; }

		/// <summary>
		/// Total count of all results.
		/// Not the same as Results.Count, because Results contains the paged results.
		/// </summary>
		int TotalResults { get; }

		/// <summary>
		/// Set by the controller
		/// </summary>
		int CurrentPage { get; set; }

		/// <summary>
		/// Set by the controller
		/// </summary>
		int PageSize { get; set; }

		int TotalPages { get; }
	}
}
