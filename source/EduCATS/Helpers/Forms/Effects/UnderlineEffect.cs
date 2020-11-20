using EduCATS.Constants;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Effects
{
	public class UnderlineEffect : RoutingEffect
	{
		const string _underlineEffect = GlobalConsts.RunNamespace + ".UnderlineEffect";

		/// <summary>
		/// Constructor.
		/// </summary>
		public UnderlineEffect() : base(_underlineEffect) { }
	}
}
