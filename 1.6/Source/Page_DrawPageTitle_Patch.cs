using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionScenarios
{
	[HarmonyPatch(typeof(Page), "DrawPageTitle")]
	public static class Page_DrawPageTitle_Patch
	{
		public static bool Prefix(Rect rect, Page __instance)
		{
			if (__instance is not Page_SelectScenario) return true;

			if (ScenarioLister_ScenariosInCategory_Patch.selectedLevel == TechLevel.Undefined) return true;

			var techLevelIcon = ContentFinder<Texture2D>.Get("UI/Icons/" + ScenarioLister_ScenariosInCategory_Patch.selectedLevel.ToString(), false);
			if (techLevelIcon == null) return true;

			var offsetRect = rect;
			offsetRect.xMin += 48f;
			Text.Font = GameFont.Medium;
			Widgets.Label(offsetRect, __instance.PageTitle);
			GUI.DrawTexture(new Rect(rect.xMin + 5f, 0f, 36f, 36f), techLevelIcon);
			return false;
		}
	}
}
