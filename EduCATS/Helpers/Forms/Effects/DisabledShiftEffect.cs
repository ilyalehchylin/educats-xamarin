using EduCATS.Constants;
using Xamarin.Forms;

namespace EduCATS.Helpers.Forms.Effects
{
	/// <summary>
	/// Effect for disabling <c>TabbedPage</c> shift effect.
	/// </summary>
	public class DisabledShiftEffect : RoutingEffect
	{
		/// <summary>
		/// Namespace for effect.
		/// </summary>
		const string _disabledShiftEffect = GlobalConsts.RunNamespace + ".DisabledShiftEffect";

		/// <summary>
		/// Constructor.
		/// </summary>
		public DisabledShiftEffect() : base(_disabledShiftEffect) { }
	}
}
