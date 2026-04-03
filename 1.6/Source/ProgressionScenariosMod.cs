using HarmonyLib;
using Verse;

namespace ProgressionScenarios
{
	public class ProgressionScenariosMod : Mod
	{
		public ProgressionScenariosMod(ModContentPack pack) : base(pack)
		{
			new Harmony("ProgressionScenariosMod").PatchAll();
		}
	}
}
