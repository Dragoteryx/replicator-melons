using Sandbox.UI;

namespace ReplicatorMelons {
	
	public partial class Hud : Sandbox.HudEntity<RootPanel> {
		public Hud() {
			if (IsClient) {
				RootPanel.SetTemplate("/hud.html");
			}
		}
	}

}
