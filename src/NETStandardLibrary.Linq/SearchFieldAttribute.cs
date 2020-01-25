using System;

namespace NETStandardLibrary.Linq
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class SearchFieldAttribute : Attribute
	{
		public string Name { get; protected set; }
		public Type ValueType { get; protected set; }
		public WhereClauseType WhereClauseType { get; protected set; }

		public SearchFieldAttribute(WhereClauseType whereClauseType, Type valueType, string name = null)
		{
			Name = name;
			ValueType = valueType;
			WhereClauseType = whereClauseType;
		}
	}
}
