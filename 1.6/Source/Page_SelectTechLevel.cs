using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ProgressionScenarios
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class HotSwappableAttribute : Attribute
	{
	}

	[HotSwappable]
	[StaticConstructorOnStartup]
	public class Page_SelectTechLevel : Page
	{
		public override string PageTitle => "PS_ChooseTechLevel".Translate();
		public override Vector2 InitialSize
		{
			get
			{
				var size = base.InitialSize;
				return new Vector2(size.x, size.y - 75);
			}
		}
		public override void PreOpen()
		{
			base.PreOpen();
			ScenarioLister.MarkDirty();
		}

		public override void DoWindowContents(Rect rect)
		{
			DrawPageTitle(rect);
			var mainRect = GetMainRect(rect);
			Widgets.BeginGroup(mainRect);

			var boxWidth = 310f;
			var boxHeight = 140f;
			var spacingX = 20f;
			var spacingY = 20f;

			var totalWidth = 3f * boxWidth + 2f * spacingX;
			var startX = (mainRect.width - totalWidth) / 2f;
			var startY = 20f;

			var animalRect = new Rect((mainRect.width - boxWidth) / 2f, startY, boxWidth, boxHeight);
			DrawTechLevelBox(animalRect, TechLevel.Animal);

			var row2Y = startY + boxHeight + spacingY;
			DrawTechLevelBox(new Rect(startX, row2Y, boxWidth, boxHeight), TechLevel.Neolithic);
			DrawTechLevelBox(new Rect(startX + boxWidth + spacingX, row2Y, boxWidth, boxHeight), TechLevel.Medieval);
			DrawTechLevelBox(new Rect(startX + 2f * (boxWidth + spacingX), row2Y, boxWidth, boxHeight), TechLevel.Industrial);

			var row3Y = row2Y + boxHeight + spacingY;
			DrawTechLevelBox(new Rect(startX, row3Y, boxWidth, boxHeight), TechLevel.Spacer);
			DrawTechLevelBox(new Rect(startX + boxWidth + spacingX, row3Y, boxWidth, boxHeight), TechLevel.Ultra);
			DrawTechLevelBox(new Rect(startX + 2f * (boxWidth + spacingX), row3Y, boxWidth, boxHeight), TechLevel.Archotech);

			Widgets.EndGroup();

			var y = rect.y + rect.height - 38f;
			var label = "Back".Translate();
			var backSize = 200f;
			if ((Widgets.ButtonText(new Rect((rect.width / 2f) - (backSize / 2f), y, backSize, BottomButSize.y), label) || KeyBindingDefOf.Cancel.KeyDownEvent) && CanDoBack())
			{
				DoBack();
			}
		}

		private void DrawTechLevelBox(Rect boxRect, TechLevel techLevel)
		{
			Widgets.DrawBox(boxRect, 1);
			Widgets.DrawHighlightIfMouseover(boxRect);
			if (Widgets.ButtonInvisible(boxRect))
			{
				ScenarioLister_ScenariosInCategory_Patch.selectedLevel = techLevel;
				if (ScenarioLister.ScenariosInCategory(ScenarioCategory.FromDef).Any() ||
				    ScenarioLister.ScenariosInCategory(ScenarioCategory.CustomLocal).Any() ||
				    ScenarioLister.ScenariosInCategory(ScenarioCategory.SteamWorkshop).Any())
				{
					this.Close();
					var page = new Page_SelectScenario();
					page.prev = this;
					Find.WindowStack.Add(page);
				}
				else
				{
					ScenarioLister_ScenariosInCategory_Patch.selectedLevel = TechLevel.Undefined;
					Messages.Message("PS_NoScenariosForTechLevel".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				}
			}

			var iconSize = boxRect.height * 0.8f;
			var iconRect = new Rect(boxRect.x + 15f, boxRect.y + (boxRect.height - iconSize) / 2f, iconSize, iconSize);
			var tex = ContentFinder<Texture2D>.Get("UI/Icons/" + techLevel.ToString(), false);
			if (tex != null) GUI.DrawTexture(iconRect, tex);

			var textRect = new Rect(iconRect.xMax + 15f, boxRect.y, boxRect.width - iconRect.width - 45f, boxRect.height);
			var centerY = textRect.height / 2f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(new Rect(textRect.x, textRect.y + centerY - 25f, textRect.width, 30f), GetTechLevelName(techLevel));
			Text.Font = GameFont.Tiny;
			var descHeight = Text.CalcHeight(("PS_TechLevelDesc_" + techLevel.ToString()).Translate(), textRect.width);
			Widgets.Label(new Rect(textRect.x, textRect.y + centerY + 2f, textRect.width, descHeight), ("PS_TechLevelDesc_" + techLevel.ToString()).Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		private string GetTechLevelName(TechLevel techLevel)
		{
			if (techLevel == TechLevel.Ultra) return "PS_Ultratech".Translate();
			return techLevel.ToStringHuman().CapitalizeFirst();
		}
	}
}
