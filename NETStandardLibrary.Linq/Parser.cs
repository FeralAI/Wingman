using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibrary.Linq
{
	public sealed class Parser
	{
		/// <summary>
		/// Parses a given string for SQL-like order by syntax using the <c>OrderBy</c> enum.
		/// e.g. LastName ASC, FirstName DESC, Age
		/// </summary>
		/// <param name="clause"></param>
		/// <returns>A ordered list of <c>OrderBy</c> objects.</returns>
		public static List<OrderByClause> ParseOrderBys(string clause)
		{
			if (string.IsNullOrWhiteSpace(clause))
				return null;

			var parts = clause.Split(',');
			parts.ToList().ForEach(p => p.Trim());

			var orderByClauses = new List<OrderByClause>();
			foreach(var part in parts)
			{
				var orderByParts = part?.Trim().Split(' ');
				if (orderByParts.Length == 0 || orderByParts.Length > 2)
					throw new ArgumentException("Order by clauses must be in the \"FieldName DIR\" format, (e.g. LastName DESC): " + string.Join(" ", orderByParts));

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
