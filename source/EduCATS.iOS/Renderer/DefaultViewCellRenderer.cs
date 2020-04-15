using EduCATS.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(DefaultViewCellRenderer))]
namespace EduCATS.iOS
{
	public class DefaultViewCellRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			return cell;
		}
	}
}
