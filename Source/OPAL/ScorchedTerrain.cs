using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace RD_OPAL
{
	[StaticConstructorOnStartup]
	public class ScorchedTerrain : Def
	{
		public static bool loaded = false;

		static void MakeDefs()
		{
			if (!loaded)
			{
				TerrainDef rich = DefDatabase<TerrainDef>.GetNamed("SoilRich", true);
				rich.driesTo = DefDatabase<TerrainDef>.GetNamed("Soil", true);

				List<TerrainDef> deflist = new List<TerrainDef>();
				foreach (TerrainDef olddef in DefDatabase<TerrainDef>.AllDefs.Where(def => def.changeable && !def.layerable))
				{
					deflist.Add(olddef);
				}

				foreach (TerrainDef olddef in deflist)
				{
					TerrainDef newdef = new TerrainDef();
					newdef.defName = olddef.defName + "_Scorched";
					newdef.label = "scorched " + olddef.label;
					newdef.acceptFilth = olddef.acceptFilth;
					newdef.acceptTerrainSourceFilth = olddef.acceptTerrainSourceFilth;
					newdef.affordances = olddef.affordances;
					newdef.avoidWander = olddef.avoidWander;
					newdef.changeable = olddef.changeable;
					newdef.color = olddef.color;
					newdef.description = olddef.description;
					newdef.driesTo = olddef.driesTo;
					newdef.edgeType = olddef.edgeType;
					newdef.fertility = 0;
					newdef.graphic = olddef.graphic;
					newdef.holdSnow = olddef.holdSnow;
					newdef.layerable = olddef.layerable;
					newdef.passability = olddef.passability;
					newdef.pathCost = olddef.pathCost + 2;
					newdef.renderPrecedence = olddef.renderPrecedence;
					newdef.scatterType = olddef.scatterType;
					newdef.smoothedTerrain = olddef.smoothedTerrain;
					newdef.statBases = olddef.statBases;
					StatUtility.SetStatValueInList(ref newdef.statBases, StatDefOf.Beauty, -5);
					newdef.takeFootprints = olddef.takeFootprints;
					newdef.terrainFilthDef = olddef.terrainFilthDef;
					newdef.texturePath = olddef.texturePath;
					newdef.uiIcon = olddef.uiIcon;
					newdef.uiIconPath = olddef.uiIconPath;
					newdef.ResolveReferences();
					newdef.PostLoad();
					DefDatabase<TerrainDef>.Add(newdef);
				}
				loaded = true;
			}
		}

		public override void ResolveReferences()
		{
			MakeDefs();
		}
	}
}
