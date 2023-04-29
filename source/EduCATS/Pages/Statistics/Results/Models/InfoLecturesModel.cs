using EduCATS.Controls.RoundedListView.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EduCATS.Pages.Statistics.Results.Models
{
	public class InfoLecturesModel
	{
		[JsonProperty("Lectures")]
		public List<Lectures> Lectures { get; set; }
	}

	public class Lectures
	{
		[JsonProperty("Theme")]
		public string Theme { get; set; }

		[JsonProperty("Duration")]
		public int Duration { get; set; }
	}
}
