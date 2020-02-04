using System;
using System.Collections.Generic;
using System.Linq;

namespace WingmanTests
{
	public class TestPerson
	{
		public static IQueryable<TestPerson> Data
		{
			get => new List<TestPerson> {
				new TestPerson { FirstName = "James", LastName = "Brown", Age = 30, Mother = Mom, Father = Dad },
				new TestPerson { FirstName = "Bob", LastName = "Smith", Age = 20, Mother = Mom, Weight = 175 },
				new TestPerson { FirstName = "Jackie", LastName = "Johnson", Age = 25, Father = Dad },
				new TestPerson { FirstName = "Keith", LastName = "Myers", Age = 28, Weight = 220, Date = new DateTime(2000, 1, 1) },
				new TestPerson { FirstName = "Bob", LastName = "Johnson", Age = 22, Father = Dad, Date = new DateTime(2000, 6, 30) },
				new TestPerson { FirstName = "Chris", LastName = "Nelson", Age = 20, Weight = 220, Date = new DateTime(2000, 1, 1) },
				Mom,
				Dad,
			}.AsQueryable();
		}

		public static TestPerson Mom = new TestPerson { FirstName = "Mary", LastName = "Henderson", Age = 51, Weight = 125 };
		public static TestPerson Dad = new TestPerson { FirstName = "Steven", LastName = "Jackson", Age = 48, Date = new DateTime(1990, 3, 15) };

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Date { get; set; }
		public int Age { get; set; }
		public int? Weight { get; set; }
		public TestPerson Mother { get; set; }
		public TestPerson Father { get; set; }
	}
}
