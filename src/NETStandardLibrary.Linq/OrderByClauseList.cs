using System;
using System.Collections.Generic;
using System.Linq;
using NETStandardLibrary.Common;

namespace NETStandardLibrary.Linq
{
	public class OrderByClauseList : List<OrderByClause>
	{
		public OrderByClauseList() : base() { }
		public OrderByClauseList(IEnumerable<OrderByClause> list) : base(list) { }
    public OrderByClauseList(string clause) : base(Parse(clause)) { }

		/// <summary>
		/// Add <c>OrderByClause</c> objects to the list by providing a SQL-like syntax using the <c>Parse</c> method.
		/// </summary>
		/// <param name="clause">The order by clause string.</param>
    public void AddByClause(string clause) => AddRange(Parse(clause));

    /// <summary>
    /// Parses a given string for SQL-like order by syntax using the <c>OrderBy</c> enum.
    /// e.g. LastName ASC, FirstName DESC, Age
    /// </summary>
    /// <param name="clause"></param>
    /// <returns>An ordered list of <c>OrderBy</c> objects.</returns>
    public static List<OrderByClause> Parse(string clause)
		{
			if (string.IsNullOrWhiteSpace(clause))
				throw new ArgumentNullException("Must not be null or empty", "clause");
;
			var parts = clause.Split(',')
				.Where(p => !string.IsNullOrWhiteSpace(p))
				.Select(p => p.Trim())
				.ToArray();

			if (parts.Length == 0)
				throw new ArgumentNullException("Must not be null or empty", "clause");

			var orderByClauses = new List<OrderByClause>();
			foreach (var part in parts)
			{
				var orderByParts = part.Trim().Split(' ');
				if (orderByParts.Length > 2)
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

		/// <summary>
		/// Creates a SQL-like order by clause string from the stored objects.
		/// e.g. LastName ASC, FirstName DESC, Age
		/// </summary>
		/// <returns>Order by clause string, null if empty set.</returns>
		public override string ToString()
		{
			var count = this.Count;
			if (count == 0)
				return null;

			if (count == 1)
				return this[0].ToString();

			var clauses = new string[count];
			for (var i = 0; i < count; i++)
			{
				clauses[i] = this[i].ToString();
			}

			return string.Join(", ", clauses);
		}
	}
}
