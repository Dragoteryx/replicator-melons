using Sandbox;

namespace ReplicatorMelons {

  [Library("ent_replicatormelon", Title = "Replicator Melon", Spawnable = true )]
  public class ReplicatorMelon : BasePhysics {

    public override void Spawn() {
      base.Spawn();
      SetModel("models/sbox_props/watermelon/watermelon.vmdl");
    }

    public Entity FindTarget() {
      Entity closest = null;
      foreach (var ent in Entity.All) {
        if (!(ent is Player || ent is Prop)) continue;
        if (closest != null && closest is Player && ent is Prop) continue;
        if (closest == null || this.Position.Distance(closest.Position) > this.Position.Distance(ent.Position)) {
          closest = ent;
        }
      }
      return closest;
    }

    [Event.Tick.Server]
    public void Tick() {
      var target = FindTarget();
      if (target != null) {
        this.PhysicsBody.ApplyForce((target.Position - this.Position).Normal*3000);
      }
    }
  }

}