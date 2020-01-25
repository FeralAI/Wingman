using System;

namespace NETStandardLibrary.Linq
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class SearchFieldAttribute : Attribute
	{
		public string Name { get; protected set; }
		public WhereClauseType WhereClauseType { get; protected set; }

		public SearchFieldAttribute(WhereClauseType whereClauseType, string name = null)
		{
			Name = name;
			WhereClauseType = whereClauseType;
		}
	}
}
