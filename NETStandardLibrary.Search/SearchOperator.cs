namespace NETStandardLibrary.Search
{
	public enum SearchOperator
	{
		/// <summary>
		/// Contains operator. Only applies to <c>string</c> values.
		/// </summary>
		Contains,

		/// <summary>
		/// Equals (=) operator.
		/// </summary>
		Equals,

		/// <summary>
		/// Greater than (>) operator.
		/// </summary>
		GreaterThan,

		/// <summary>
		/// Greater than or equal (>=) operator.
		/// </summary>
		GreaterThanEqual,

		/// <summary>
		/// Less than (<) operator.
		/// </summary>
		LessThan,

		/// <summary>
		/// Less than or equal (<=) operator.
		/// </summary>
		LessThanEqual
	}
}
