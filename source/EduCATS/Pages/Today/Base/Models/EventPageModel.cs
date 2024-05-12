using EduCATS.Controls.RoundedListView.Enums;
using EduCATS.Controls.RoundedListView.Interfaces;
using EduCATS.Data.Models.Calendar;
using Nyxbull.Plugins.CrossLocalization;

namespace EduCATS.Pages.Today.Base.Models
{
	public class EventPageModel : IRoundedListType
	{	
		public string Address { get; set; }
		public string Color { get; set; }
		public string Name { get; set; }

		public string Note { get; set; }

		public string Date { get; set; }
		public string Type { get; set; }

		public EventPageModel(Event @event)
		{
			setDefaultProps(@event);
		}

		void setDefaultProps(Event @event)
		{
			if (@event != null)
			{
				Color = "#FFA500";
				Date = @event.Start + "-" + @event.End;
				Name = @event.Name;
				/*for (int i = 0; i < schedule.Notes.Length; i++)
				{
					Note += schedule.Notes[i];
				}*/

			}
		} 


		public RoundedListTypeEnum GetListType()
		{
			return RoundedListTypeEnum.Navigation;
		}
	}
}
