using System;

namespace Wingman.Linq
{
	public interface ISearchField
	{
		string Name { get; set; }
		object Value { get; set; }
		object MaxValue { get; set; }
		WhereOperator WhereOperator { get; set; }
		Type ValueType { get; set; }
	}
}
