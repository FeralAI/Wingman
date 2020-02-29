using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using static Bogus.DataSets.Name;

namespace WingmanTests
{
	public class TestPerson
	{
		public static IQueryable<TestPerson> GenerateData(int seed = 5882300)
		{
			var testPeople = new List<TestPerson>();

			var fakePeople = new Faker<TestPerson>()
				.UseSeed(seed)
				.StrictMode(true)
				.RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
				.RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName(p.Gender))
				.RuleFor(p => p.LastName, (f, p) => f.Name.LastName(p.Gender))
				.RuleFor(p => p.Age, f => f.PickRandom<int>(Enumerable.Range(4, 10).Select(n => n * 5)))
				.RuleFor(p => p.Weight, f => f.PickRandom<int?>(new List<int?> { null, 125, 150, 175, 200, 225, 250 }))
				.RuleFor(p => p.Created, f => f.Date.Past(1, new DateTime(1999, 1, 1)))
				.RuleFor(p => p.Updated, f => f.PickRandom<DateTime?>(new List<DateTime?> {
					null,
					new DateTime(1999, 12, 31),
					new DateTime(2000, 1, 1),
					new DateTime(2000, 6, 15)
				}));

			fakePeople.RuleFor(p => p.Friend, f =>
			{
				var fakeFriend = default(TestPerson);
				if (f.IndexFaker % 2 == 0)
				{
					fakeFriend = fakePeople.Generate(1).First();
					testPeople.Add(fakeFriend);
				}

				return fakeFriend;
			});

			testPeople.AddRange(fakePeople.Generate(50));

			// Use this to write out the data if needed:
			// using (var stream = new FileStream("data.json", FileMode.OpenOrCreate))
			// using (var writer = new StreamWriter(stream))
			// {
			// 	writer.WriteLine("[");
			// 	foreach (var testPerson in testPeople)
			// 	{
			// 		writer.WriteLine(JsonConvert.SerializeObject(testPerson) + ",");
			// 	}
			// 	writer.WriteLine("]");
			// }

			return testPeople.AsQueryable();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Gender Gender { get; set; }
		public int Age { get; set; }
		public int? Weight { get; set; }
		public TestPerson Friend { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
	}
}
