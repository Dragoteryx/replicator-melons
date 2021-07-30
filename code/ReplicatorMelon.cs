using Sandbox;

namespace ReplicatorMelons {

  [Library("ent_replicatormelon", Title = "Replicator Melon", Spawnable = true )]
  public class ReplicatorMelon : BasePhysics {

    public override void Spawn() {
      base.Spawn();

      SetModel("models/sbox_props/watermelon/watermelon.vmdl");
    }

    [Event.Tick.Server]
    public void Tick() {
      
    }
  }

}