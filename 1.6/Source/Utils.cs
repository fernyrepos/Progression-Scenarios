using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionScenarios
{
	public static class Utils
	{
		private static Dictionary<Scenario, ScenarioDef> scenarioDefCache = new Dictionary<Scenario, ScenarioDef>();
		private static Dictionary<Scenario, Texture2D> scenarioIconCache = new Dictionary<Scenario, Texture2D>();

		public static ScenarioDef GetDef(this Scenario scen)
		{
			if (scenarioDefCache.TryGetValue(scen, out var cached)) return cached;
			var def = DefDatabase<ScenarioDef>.AllDefsListForReading.FirstOrDefault(d => d.scenario == scen);
			scenarioDefCache[scen] = def;
			return def;
		}

		public static Texture2D GetIcon(this Scenario scen)
		{
			if (scenarioIconCache.TryGetValue(scen, out var cached)) return cached;
			Texture2D icon = null;
			var def = scen.GetDef();
			if (def != null)
			{
				var ext = def.GetModExtension<ScenarioExtension>();
				if (ext != null && !ext.iconPath.NullOrEmpty()) icon = ContentFinder<Texture2D>.Get(ext.iconPath, false);
				if (icon == null && def.modContentPack != null)
				{
					foreach (var expansionDef in DefDatabase<ExpansionDef>.AllDefs)
					{
						if (expansionDef.linkedMod == def.modContentPack.PackageId && !expansionDef.iconPath.NullOrEmpty())
						{
							icon = ContentFinder<Texture2D>.Get(expansionDef.iconPath, false);
							break;
						}
					}
					if (icon == null)
					{
						icon = ModLister.GetModWithIdentifier(def.modContentPack.PackageId)?.Icon;
					}
				}
			}
			scenarioIconCache[scen] = icon;
			return icon;
		}
	}
}
