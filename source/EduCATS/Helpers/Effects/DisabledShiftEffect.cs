using EduCATS.Constants;
using Xamarin.Forms;

namespace EduCATS.Helpers.Effects
{
	public class DisabledShiftEffect : RoutingEffect
	{
		const string _disabledShiftEffect = GlobalConsts.RunNamespace + ".DisabledShiftEffect";

		public DisabledShiftEffect() : base(_disabledShiftEffect) { }
	}
}
