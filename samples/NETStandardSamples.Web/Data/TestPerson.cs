using System;
using System.Text.Json.Serialization;

namespace NETStandardSamples.Web.Data
{
	public class TestPerson
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? Date { get; set; }
		public int Age { get; set; }
		public int? Weight { get; set; }
		[JsonIgnore]
		public TestPerson Mother { get; set; }
		[JsonIgnore]
		public TestPerson Father { get; set; }
	}
}
