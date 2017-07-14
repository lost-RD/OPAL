using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;   // Always needed    // Material/Graphics handling functions are found here
using RimWorld;      // RimWorld specific functions are found here
using Verse;         // RimWorld universal objects are here
//using Verse.AI;    // Needed when you do something with the AI
using Verse.Sound;   // Needed when you do something with the Sound

namespace RD_OPAL
{
    public class Building_LaserAction : Building
    {
        private int Burnticks = 4;
        private int Burnticks2 = 4;
        public int drillticksRemaining = 0;
        public int drilltickAmountToGen = 0;
        public Laser_Beam skydrillerEffect;
        private static readonly SoundDef PlasmaDrill = SoundDef.Named("OPAL_Sound");
        private static readonly SoundDef PlasmaDrillFire = SoundDef.Named("OPAL_Fire");

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.drilltickAmountToGen = this.randomDrillticks();
            this.drillticksRemaining = this.drilltickAmountToGen;
        }

        public int randomDrillticks()
        {
            return Rand.Range(60*10, 60*20);
        }

        public override void Tick()
        {
            base.Tick();
            Burnticks--;
            Burnticks2--;
            --this.drillticksRemaining;
           
            if (Burnticks == 0)
            {
                skydrillerEffect = new Laser_Beam(base.Map, this.Position);
                base.Map.weatherManager.eventHandler.AddEvent(skydrillerEffect);
                Burnticks = 4;
            }

            if (Burnticks2 == 0)
            {
				float radius = Laser_Beam_MeshMaker.MeshWidth * 0.66f + (0.5f + Mathf.Sin((float)this.drillticksRemaining / 150)) * Laser_Beam_MeshMaker.MeshWidth * 0.33f;
				MoteMaker.MakeStaticMote(base.Position, base.Map, ThingDefOf.Mote_ShotFlash, 9f);
				GenExplosion.DoExplosion(base.Position, base.Map, radius, DamageDefOf.Flame, (Thing)null, PlasmaDrill, (ThingDef)null);
				var worker = new DamageWorker();
				IEnumerable<IntVec3> cells = worker.ExplosionCellsToHit(base.Position, base.Map, radius);
				foreach(IntVec3 cell in cells)
				{
					TerrainDef current = base.Map.terrainGrid.TerrainAt(cell);
					if (!(current.defName.Contains("_Scorched")) & (current.changeable) & !(current.layerable))
					{
						if (current.driesTo != null)
						{
							base.Map.terrainGrid.SetTerrain(cell, current.driesTo);
							current = current.driesTo;
						}
						TerrainDef scorched = DefDatabase<TerrainDef>.GetNamed(current.defName+"_Scorched", true);
						base.Map.terrainGrid.SetTerrain(cell, scorched);
					}
				}
				//GenTemperature.PushHeat(base.Position, base.Map, 100000f);
				Burnticks2 = 4;
            }

            if (drillticksRemaining == 0)
            {
                Messages.Message("OPAL_LaserComplete".Translate(), MessageSound.Standard);
                //GenSpawn.Spawn(ThingDef.Named("Ind_MiningHole"), this.Position, base.Map);
                SoundStarter.PlayOneShot(Building_LaserAction.PlasmaDrillFire, new TargetInfo(base.Position, base.Map, false));
                ((Thing)this).Destroy((DestroyMode)0);
            }
        }
    }
}