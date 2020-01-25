using System;
using System.Collections.Generic;
using System.Reflection;

namespace NETStandardLibrary.Linq
{
	public class SearchFieldList : List<SearchField>
	{
		public SearchFieldList() : base() { }
		public SearchFieldList(IEnumerable<SearchField> searchFields, WhereClauseOperator? whereOperator = null)
			: base(searchFields)
		{
			WhereOperator = whereOperator ?? WhereOperator;
		}

		/// <summary>
		/// Converts an object into a list of <c>SearchField</c> by scanning each public property of the object.
		/// </summary>
		/// <param name="model">The object to analyze.</param>
		/// <param name="ignoreNulls">Ignore properties with a value of <c>null</c>.</param>
		/// <param name="whereOperator">The where clause operator.</param>
		/// <returns>A <c>SearchFieldList</c>.</returns>
		public static SearchFieldList FromObject(object model, bool ignoreNulls = false, WhereClauseOperator whereOperator =  WhereClauseOperator.AND)
		{
			if (model == null)
				throw new ArgumentNullException("Model is required");

			var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var searchFields = new List<SearchField>();

			foreach (var property in properties)
			{
				var value = property.GetValue(model);
				if (ignoreNulls && value == null)
					continue;

				var attribute = (SearchFieldAttribute)property.GetCustomAttribute(typeof(SearchFieldAttribute));
				var valueType = (Type)null;

				try
				{
					valueType = attribute?.ValueType ?? value.GetType();
				}
				catch (NullReferenceException e)
				{
					throw new NullReferenceException($"A ValueType must be provided via a non-null value or the SearchField attribute for property {property.Name}", e);
				}

				var searchField = new SearchField
				{
					Name = attribute?.Name ?? property.Name,
					Operator = attribute?.WhereClauseType ?? WhereClauseType.Equal,
					Value = value,
					ValueType = valueType,
				};

				searchFields.Add(searchField);
			}

			var result = new SearchFieldList(searchFields, whereOperator);
			return result;
		}

		/// <summary>
		/// Subclauses to be joined with the main clause.
		/// </summary>
		public SearchFieldList Subclauses { get; set; }

		/// <summary>
		/// The operator to join the where clauses generated from the search fields.
		/// </summary>
		public WhereClauseOperator? SubclauseWhereOperator { get; set; } = WhereClauseOperator.AND;

		/// <summary>
		/// The operator to join the where clauses generated from the search fields.
		/// </summary>
		public WhereClauseOperator WhereOperator { get; set; } = WhereClauseOperator.AND;
	}
}
