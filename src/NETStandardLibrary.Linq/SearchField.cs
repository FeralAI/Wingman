using System;
using System.Collections.Generic;
using System.Reflection;

namespace NETStandardLibrary.Linq
{
	public class SearchField : ISearchField
	{
		public static IEnumerable<SearchField> FromObject(object model)
		{
			if (model == null)
				throw new ArgumentNullException("Model is required");

			var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var result = new List<SearchField>();

			foreach (var property in properties)
			{
				var value = property.GetValue(model);
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

				result.Add(searchField);
			}

			return result;
		}

		public string Name { get; set; }
		public object Value { get; set; }
		public object MaxValue { get; set; }
		public WhereClauseType Operator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
