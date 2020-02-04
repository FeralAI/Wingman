namespace Wingman.Linq
{
	public class OrderByClause
	{
		public string Name { get; set; }
		public OrderByDirection Direction { get; set; }

		public override string ToString() => $"{Name} {Direction}".Trim();
	}
}
