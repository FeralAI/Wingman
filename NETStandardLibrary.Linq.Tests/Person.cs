using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibrary.Linq.Tests
{
	public class Person
	{
		public static IQueryable<Person> Data
		{
			get => new List<Person> {
				new Person { FirstName = "Bob", LastName = "Smith", Age = 20 },
				new Person { FirstName = "John", LastName = "Johnson", Age = 25 },
			}.AsQueryable();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
	}
}
