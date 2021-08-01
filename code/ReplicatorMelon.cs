using System.Collections.Generic;
using System;
using Sandbox;

namespace ReplicatorMelons {

  [Library("ent_replicatormelon", Title = "Replicator Melon", Spawnable = true )]
  public class ReplicatorMelon : BasePhysics {

    // lifetime

    internal static HashSet<ReplicatorMelon> AllMelons = new HashSet<ReplicatorMelon>();
    //internal static HashSet<Entity> Targets = new HashSet<Entity>();

    public override void Spawn() {
      base.Spawn();

      SetModel("models/sbox_props/watermelon/watermelon.vmdl");

      AllMelons.Add(this);
    }

    protected override void OnDestroy() {
      base.OnDestroy();

      AllMelons.Remove(this);
    }

    // target

    private Entity Target {get; set;}
    private DateTime LastTargetRefresh {get; set;}

    private Entity FindTarget() {
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
      if (!ent.IsValid()) return false;
      if (ent is Player && ent.Health <= 0) return false;
      if (ent is Prop) {
        if (!CanCreateMelon()) return false;
        var tr = Trace.Ray(this.Position, ent.Position).WorldOnly().Run();
        if (tr.Entity != null && tr.Entity.IsWorld) return false;
      }
      return ent is Player || ent is Prop;
    }

    // tick

    [Event.Tick.Server]
    private void Tick() {
      if (LastTargetRefresh == null || (DateTime.Now - LastTargetRefresh).Seconds >= 1) {
        Target = FindTarget();
        LastTargetRefresh = DateTime.Now;
      }
      if (IsValidTarget(Target)) {
        PhysicsBody.ApplyForceAt(Position + new Vector3 {z = 10}, (Target.Position - this.Position).Normal*2000);
      }
    }

    // create melons

    public static Boolean CanCreateMelon() {
      return AllMelons.Count < Game.MaxMelons;
    }

    public static ReplicatorMelon CreateMelon(Vector3 pos) {
      return new ReplicatorMelon {
          Position = pos
      };
    }

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
        if (CanCreateMelon()) {
          CreateMelon(pos  + new Vector3 {z = 10});
        }
      } else if (CanCreateMelon()) {
        var pos = ent.Position;
        ent.Delete();
        CreateMelon(pos);
      }
    }
  }

}