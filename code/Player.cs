using Sandbox;

namespace ReplicatorMelons {
	partial class Player : Sandbox.Player {
		public override void Respawn() {
			SetModel("models/citizen/citizen.vmdl");
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate(Client cl) {
			base.Simulate(cl);

			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			SimulateActiveChild(cl, ActiveChild);

			if (IsServer) {
				if (Input.Pressed(InputButton.Reload)) {
					Log.Info("trying to delete all melons");
					foreach (var melon in Entity.FindAllByName("ent_replicatormelon")) {
						Log.Info("killing a melon");
						melon.Delete();
					}
				} else if (Input.Pressed(InputButton.Attack1) || Input.Down(InputButton.Attack2)) {
					var melon = new ReplicatorMelon();
					var tr = Trace.Ray(EyePos, EyePos + EyeRot.Forward*1000)
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
