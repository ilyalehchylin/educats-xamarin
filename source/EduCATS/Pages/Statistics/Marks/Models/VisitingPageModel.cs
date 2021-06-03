using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCATS.Pages.Statistics.Marks.Models
{
	public class VisitingPageModel : IRoundedListType
	{
		public string Title { get; set; }
		public string Comment { get; set; }
		public string Mark { get; set; }
		public bool ShowForStud { get; set; }

		public bool IsTitle { get { return !string.IsNullOrEmpty(Title); } }
		public bool IsComment { get { return !string.IsNullOrEmpty(Comment); } }

		public VisitingPageModel(string title, string comment, string mark, bool showforStud)
		{
			Title = title;
			Comment = comment;
			Mark = mark;
			ShowForStud = showforStud;
		}

		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
