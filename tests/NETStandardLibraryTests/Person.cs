﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NETStandardLibraryTests
{
	public class Person
	{
		public static IQueryable<Person> Data
		{
			get => new List<Person> {
				Mom,
				Dad,
				new Person { FirstName = "James", LastName = "Brown", Age = 30, Mother = Mom, Father = Dad },
				new Person { FirstName = "Bob", LastName = "Smith", Age = 20, Mother = Mom, Weight = 175 },
				new Person { FirstName = "Jackie", LastName = "Johnson", Age = 25, Father = Dad },
				new Person { FirstName = "Keith", LastName = "Myers", Age = 28, Weight = 220, Date = new DateTime(2000, 1, 1) },
				new Person { FirstName = "Bob", LastName = "Johnson", Age = 22, Father = Dad, Date = new DateTime(2000, 6, 30) },
			}.AsQueryable();
		}

		public static Person Mom = new Person { FirstName = "Mary", LastName = "Henderson", Age = 51, Weight = 125 };
		public static Person Dad = new Person { FirstName = "Steven", LastName = "Jackson", Age = 48, Date = new DateTime(1990, 3, 15) };

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Date { get; set; }
		public int Age { get; set; }
		public int? Weight { get; set; }
		public Person Mother { get; set; }
		public Person Father { get; set; }
	}
}
