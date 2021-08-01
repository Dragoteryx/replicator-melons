using Sandbox;

namespace ReplicatorMelons {

	[Library("replicatormelons")]
	public partial class Game : Sandbox.Game {

		[ConVar.Replicated("replicatormelons_max")]
		public static int MaxMelons {get; set;} = 150;

		public Game() {			
			if (IsServer) {
				Log.Info("Serverside ok");
				new Hud();
			}
		}

		public override void ClientJoined(Client client) {
			base.ClientJoined( client );
			var player = new Player();
			client.Pawn = player;
			player.Respawn();
		}
	}

}
