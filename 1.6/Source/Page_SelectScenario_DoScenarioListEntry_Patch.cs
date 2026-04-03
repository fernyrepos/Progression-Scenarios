using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionScenarios
{
	[HarmonyPatch(typeof(Page_SelectScenario), "DoScenarioListEntry")]
	public static class Page_SelectScenario_DoScenarioListEntry_Patch
	{
		public static void Prefix(ref Rect rect, Scenario scen)
		{
			var icon = scen.GetIcon();
			if (icon == null) return;

			var iconRect = new Rect(rect.x, rect.y + (rect.height - 40f) / 2f, 40f, 40f);
			GUI.DrawTexture(iconRect, icon);
			rect.xMin += 48f;
		}
	}
}
