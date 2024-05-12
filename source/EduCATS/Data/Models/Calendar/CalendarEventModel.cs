using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Data.Models.Calendar
{
	public class CalendarEventModelTest
	{
		[JsonProperty("Notes")]
		public List<Event> Event { get; set; }

		public CalendarEventModelTest()
		{
			Event = new List<Event>();
		}
	}

	public class Event
	{
		[JsonProperty("Color")]
		public string Color { get; set; }

		[JsonProperty("EndTime")]
		public string End { get; set; }

		[JsonProperty("Text")]
		public string Name { get; set; }

		[JsonProperty("StartTime")]
		public string Start { get; set; }

		/*
		[JsonProperty("Notes")]
		public string[] Notes { get; set; }
		*/
	}

}
