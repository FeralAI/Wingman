using System;

namespace NETStandardLibrary.Search
{
	public class SearchField : ISearchField
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public SearchOperator Operator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
