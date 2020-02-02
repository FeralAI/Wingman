using System;

namespace NETStandardLibrary.Linq
{
	public class SearchField : ISearchField
	{
		public SearchField() { }
		public SearchField(string name, object value, WhereOperator whereOperator, object maxValue = null)
		{
			Name = name;
			Value = value;
			ValueType = value?.GetType();
			MaxValue = maxValue;
			WhereOperator = whereOperator;
		}
		public SearchField(string name, object value, Type valueType, WhereOperator whereOperator, object maxValue = null)
			: this(name, value, whereOperator, maxValue)
		{
			ValueType = valueType;
		}

		public string Name { get; set; }
		public object Value { get; set; }
		public object MaxValue { get; set; }
		public WhereOperator WhereOperator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
