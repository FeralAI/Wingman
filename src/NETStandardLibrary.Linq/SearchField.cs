using System;

namespace NETStandardLibrary.Linq
{
	public class SearchField : ISearchField
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public object MaxValue { get; set; }
		public WhereClauseType Operator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
