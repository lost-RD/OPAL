using Verse;
using RimWorld;
using UnityEngine;
using System;

namespace RD_OPAL
{
    public class Building_LaserDeployed : Building
    {
        public int waitticksRemaining = 0;
        public int waittickAmountToGen = 0;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            Messages.Message("OPAL_LaserRequested".Translate(), MessageSound.Standard);
            this.waittickAmountToGen = this.randomWaitticks();
            this.waitticksRemaining = this.waittickAmountToGen;
        }

        public int randomWaitticks()
        {
			return Rand.Range(60*5, 60*10);
			//return 60;
        }

        public override void Tick()
        {
            base.Tick();
            --this.waitticksRemaining;
            
            if (waitticksRemaining == Math.Floor(waittickAmountToGen*0.75))
            {
                Messages.Message("OPAL_LaserApproaching".Translate(), MessageSound.Standard);
            }
            if (waitticksRemaining == Math.Floor(waittickAmountToGen * 0.25))
            {
                Messages.Message("OPAL_LaserInitiating".Translate(), MessageSound.SeriousAlert);
            }
            if (waitticksRemaining == 0)
            {
                GenSpawn.Spawn(ThingDef.Named("OPAL_LaserAction"), this.Position, base.Map);
                ((Thing)this).Destroy((DestroyMode)0);
            }
        }
    }
}