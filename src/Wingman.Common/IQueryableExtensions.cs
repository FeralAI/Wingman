using System;
using System.Linq;

namespace Wingman.Common
{
	public static class IQueryableExtensions
	{
		/// <summary>
		/// Filters an <c>IQueryable&lt;&gt;</c> using <c>IQueryable.Skip</c> and <c> IQueryable.Take</c>.
		/// </summary>
		/// <param name="this">The queryable object.</param>
		/// <param name="page">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <returns>A <c>IQueryable&lt;&gt;</c> object.</returns>
		public static IQueryable<T> GetPage<T>(this IQueryable<T> @this, int page, int pageSize)
		{
			return @this
				.Skip(pageSize * (Math.Max(page, 1) - 1))
				.Take(pageSize);
		}
	}
}
