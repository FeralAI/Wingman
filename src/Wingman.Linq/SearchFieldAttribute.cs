using System;

namespace Wingman.Linq
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class SearchFieldAttribute : Attribute
	{
		public string Name { get; protected set; }
		public Type ValueType { get; protected set; }
		public WhereOperator WhereClauseType { get; protected set; }

		public SearchFieldAttribute(WhereOperator whereClauseType, Type valueType = null, string name = null)
		{
			Name = name;
			ValueType = valueType;
			WhereClauseType = whereClauseType;
		}
	}
}
