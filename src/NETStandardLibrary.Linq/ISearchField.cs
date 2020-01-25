using System;

namespace NETStandardLibrary.Linq
{
	public interface ISearchField
	{
		string Name { get; set; }
		object Value { get; set; }
		object MaxValue { get; set; }
		WhereClauseType Operator { get; set; }
		Type ValueType { get; set; }
	}
}