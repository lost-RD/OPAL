using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace RD_OPAL
{
    public class Projectile_LaserBullet : Projectile
    {

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            GenSpawn.Spawn(ThingDef.Named("OPAL_LaserDeployed"), this.Position, Find.VisibleMap);
        }

    }
}
