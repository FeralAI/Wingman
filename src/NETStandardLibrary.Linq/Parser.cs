using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibrary.Linq
{
	public sealed class Parser
	{
		/// <summary>
		/// Joins a group of <c>OrderByClause</c> using a SQL-like syntax.
		/// </summary>
		/// <param name="clauses"></param>
		/// <returns>A string composed of the order by clauses.</returns>
		public static string JoinOrderBys(IEnumerable<OrderByClause> clauses)
		{
			if (clauses == null || clauses.Count() == 0)
				return null;

			var clause = string.Join(", ", clauses.Select(c => c?.ToString().Trim()));
			return clause;
		}

		/// <summary>
		/// Parses a given string for SQL-like order by syntax using the <c>OrderBy</c> enum.
		/// e.g. LastName ASC, FirstName DESC, Age
		/// </summary>
		/// <param name="clause"></param>
		/// <param name="separator"></param>
		/// <returns>A ordered list of <c>OrderBy</c> objects.</returns>
		public static List<OrderByClause> ParseOrderBys(string clause)
		{
			if (string.IsNullOrWhiteSpace(clause))
				throw new ArgumentNullException("Must not be null or empty", "clause");

			var parts = clause.Split(',');
			parts.ToList().ForEach(p => p.Trim());
			parts = parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
			if (parts.Length == 0)
				throw new ArgumentNullException("Must not be null or empty", "clause");

			var orderByClauses = new List<OrderByClause>();
			foreach(var part in parts)
			{
				var orderByParts = part?.Trim().Split(' ');
				if (orderByParts.Length < 1 || orderByParts.Length > 2)
					throw new ArgumentException("Order by clauses must be in the \"FieldName DIR\" format, (e.g. LastName DESC): " + string.Join(" ", orderByParts), "clause");

				var name = orderByParts[0];
				var direction = orderByParts.Length == 1
					? OrderByDirection.ASC
					: (OrderByDirection)Enum.Parse(typeof(OrderByDirection), orderByParts[1]);

				var orderByClause = new OrderByClause
				{
					Name = name,
					Direction = direction,
				};

				orderByClauses.Add(orderByClause);
			}

			return orderByClauses;
		}
	}
}
