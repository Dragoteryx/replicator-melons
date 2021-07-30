using Sandbox;

namespace ReplicatorMelons {

	[Library("replicator-melons")]
	public partial class Game : Sandbox.Game {
		public Game() {
			if (IsServer) {
				Log.Info("Serverside ok");
				new Hud();
			}

			if (IsClient) {
				Log.Info("Clientside ok");
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
