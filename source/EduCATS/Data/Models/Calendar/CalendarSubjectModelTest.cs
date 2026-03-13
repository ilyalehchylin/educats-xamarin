using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models.Calendar
{
	public class CalendarSubjectModelTest
	{
		[JsonProperty("Schedule")]
		public List<Schedule> Schedule { get; set; }

		public CalendarSubjectModelTest()
		{
			Schedule = new List<Schedule>();
		}
	}

	public class Schedule
	{
		[JsonProperty("Audience")]
		public string Audience { get; set; }

		[JsonProperty("Building")]
		public string Building { get; set; }

		[JsonProperty("Color")]
		public string Color { get; set; }

		[JsonProperty("End")]
		public string End { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Start")]
		public string Start { get; set; }

		[JsonProperty("Teacher")]
		public Teacher Teacher { get; set; }

		[JsonProperty("Type")]
		public int Type { get; set; }

	}

	public class Teacher
	{
		[JsonProperty("FullName")]
		public string FullName { get; set; }
	}
}
