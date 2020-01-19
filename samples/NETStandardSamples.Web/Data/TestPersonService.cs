using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardSamples.Web.Data
{
	public class TestPersonService
	{
		public static TestPerson Mom = new TestPerson { FirstName = "Mary", LastName = "Henderson", Age = 51, Weight = 125 };
		public static TestPerson Dad = new TestPerson { FirstName = "Steven", LastName = "Jackson", Age = 48, Date = new DateTime(1990, 3, 15) };

		public IQueryable<TestPerson> GetData()
		{
			return new List<TestPerson> {
				Mom,
				Dad,
				new TestPerson { FirstName = "James", LastName = "Brown", Age = 30, Mother = Mom, Father = Dad },
				new TestPerson { FirstName = "Bob", LastName = "Smith", Age = 20, Mother = Mom, Weight = 175 },
				new TestPerson { FirstName = "Jackie", LastName = "Johnson", Age = 25, Father = Dad },
				new TestPerson { FirstName = "Keith", LastName = "Myers", Age = 28, Weight = 220, Date = new DateTime(2000, 1, 1) },
				new TestPerson { FirstName = "Bob", LastName = "Johnson", Age = 22, Father = Dad, Date = new DateTime(2000, 6, 30) },
			}.AsQueryable();
		}
	}
}
