namespace NETStandardLibrary.Linq
{
	/// <summary>
	/// Enum which represents the supported factory methods from <c>System.Linq.Expressions.Expression</c>.
	/// </summary>
	public enum IQueryableExpressionType
	{
		/// <summary>
		/// Contains operator. Only applies to <c>string</c> values.
		/// </summary>
		Contains,

		/// <summary>
		/// Equals (=) operator.
		/// </summary>
		Equal,

		/// <summary>
		/// Greater than (>) operator.
		/// </summary>
		GreaterThan,

		/// <summary>
		/// Greater than or equal (>=) operator.
		/// </summary>
		GreaterThanOrEqual,

		/// <summary>
		/// Less than (<) operator.
		/// </summary>
		LessThan,

		/// <summary>
		/// Less than or equal (<=) operator.
		/// </summary>
		LessThanOrEqual
	}
}
