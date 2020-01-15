using System;
using NETStandardLibrary.Linq;

namespace NETStandardLibrary.Search
{
	public interface ISearchField
	{
		string Name { get; set; }
		object Value { get; set; }
		WhereClauseType Operator { get; set; }
		Type ValueType { get; set; }
	}
}
