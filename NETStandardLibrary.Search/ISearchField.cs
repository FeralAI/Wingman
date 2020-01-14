using System;

namespace NETStandardLibrary.Search
{
	public interface ISearchField
	{
		string Name { get; set; }
		object Value { get; set; }
		SearchOperator Operator { get; set; }
		Type ValueType { get; set; }
	}
}
