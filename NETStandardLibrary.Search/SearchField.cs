using System;
using NETStandardLibrary.Linq;

namespace NETStandardLibrary.Search
{
	public class SearchField : ISearchField
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public WhereClauseType Operator { get; set; }
		public virtual Type ValueType { get; set; }
	}
}
