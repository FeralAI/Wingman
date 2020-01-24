using System;
using System.Collections.Generic;
using System.Linq;
using NETStandardLibrary.Common;
using NETStandardLibrary.Linq;

namespace NETStandardLibrary.Search
{
	public class SearchField : ISearchField
	{
		public static IEnumerable<SearchField> FromObject(object model)
		{
			if (model == null)
				throw new ArgumentNullException("Model is required");

			var values = model.GetType().GetPropertyValues(model);

			// TODO: Make something more robust
			var result = values
				.Where(kv => kv.Value != null)
				.Select(kv => new SearchField
				{
					Name = kv.Key,
					Value = kv.Value,
					ValueType = kv.Value.GetType(),
					Operator = kv.Value.GetType() == typeof(string)
						? WhereClauseType.Contains
						: WhereClauseType.Equal,
				});

			return result;
		}

		public string Name { get; set; }
		public object Value { get; set; }
		public object MaxValue { get; set; }
		public WhereClauseType Operator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
