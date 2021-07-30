using System;
using Sandbox;

namespace ReplicatorMelons {

  [Library("ent_replicatormelon", Title = "Replicator Melon", Spawnable = true )]
  public class ReplicatorMelon : BasePhysics {

    public override void Spawn() {
      base.Spawn();
      SetModel("models/sbox_props/watermelon/watermelon.vmdl");
    }

    // target

    private Entity Target {get; set;}
    private DateTime LastTargetRefresh {get; set;}

    protected Entity FindTarget() {
      Entity closest = null;
      foreach (var ent in Entity.All) {
        if (!IsValidTarget(ent)) continue;
        if (closest == null || this.Position.Distance(closest.Position) > this.Position.Distance(ent.Position)) {
          closest = ent;
        }
      }
      return closest;
    }

    protected bool IsValidTarget(Entity ent) {
      if (ent == null) return false;
      if (ent is Player && ent.Health <= 0) return false;
      return ent is Player || ent is Prop;
    }

    // ai

    [Event.Tick.Server]
    private void Tick() {
      if (LastTargetRefresh == null || (DateTime.Now - LastTargetRefresh).Seconds >= 1 || !IsValidTarget(Target)) {
        Target = FindTarget();
        LastTargetRefresh = DateTime.Now;
      }
      if (IsValidTarget(Target)) {
        PhysicsBody.ApplyForceAt(Position + new Vector3 {z = 10}, (Target.Position - this.Position).Normal*2000);
      }
    }

    // collisions

    protected override void OnPhysicsCollision(CollisionEventData eventData) {
      base.OnPhysicsCollision(eventData);

      var ent = eventData.Entity;
      if (!IsValidTarget(ent)) return;

      if (ent is Player) {
        var pos = ent.Position;
        ent.TakeDamage(new DamageInfo {
          Damage = ent.Health,
          Attacker = this
        });
        new ReplicatorMelon {
          Position = pos + new Vector3 {z = 10}
        };
      } else {
        var pos = ent.Position;
        ent.Delete();
        new ReplicatorMelon {
          Position = pos
        };
      }
    }
  }

}