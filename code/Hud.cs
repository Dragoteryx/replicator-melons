using Sandbox.UI;
using Sandbox;

namespace ReplicatorMelons {
	
	public partial class Hud : HudEntity<RootPanel> {

		public string MelonsCount {
			get {
				var count = ReplicatorMelon.AllMelons.Count;
				return count == 1 ? "1 melon " : $"{count} melons";
			}
		}
		
		public Hud() {
			if (IsClient) {
				RootPanel.SetTemplate("/hud.html");
			}
		}
	}

}
