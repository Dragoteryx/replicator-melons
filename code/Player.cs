using Sandbox;

namespace ReplicatorMelons {

	partial class Player : Sandbox.Player {

		public override void Respawn() {
			SetModel("models/citizen/citizen.vmdl");
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera ??= new FirstPersonCamera();
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			Dress();
			base.Respawn();
		}

		public override void Simulate(Client cl) {
			base.Simulate(cl);
			SimulateActiveChild(cl, ActiveChild);

			if (Input.Pressed(InputButton.View)) {
				if (Camera is FirstPersonCamera) {
					Camera = new ThirdPersonCamera();
				} else {
					Camera = new FirstPersonCamera();
				}
			}

			if (IsServer) {
				if (Input.Pressed(InputButton.Reload)) {
					foreach (var ent in Entity.All) {
						if (ent is ReplicatorMelon) {
							ent.Delete();
						}
					}
				} else if (ReplicatorMelon.CanCreateMelon() && (Input.Pressed(InputButton.Attack1) || Input.Down(InputButton.Attack2))) {
					var melon = new ReplicatorMelon();
					var tr = Trace.Ray(EyePos, EyePos + EyeRot.Forward*5000)
						.WorldOnly()
						.Run();
					melon.Position = tr.EndPos - EyeRot.Forward*25;
				}
			}
		}

		public override void OnKilled() {
			base.OnKilled();
			EnableDrawing = false;
		}
	}
}
