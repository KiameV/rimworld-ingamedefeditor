using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class ApparelPropertiesWidget : IInputWidget
	{
		private readonly ApparelProperties apparelProperties;

		private readonly List<IInputWidget> inputWidgets;

		private WindowUtil.PlusMinusArgs<BodyPartGroupDef> bodyPartGroupArgs;
		private WindowUtil.PlusMinusArgs<ApparelLayerDef> apparelLayerArgs;

		public ApparelPropertiesWidget(ApparelProperties apparelProperties)
		{
			this.apparelProperties = apparelProperties;

			this.inputWidgets = new List<IInputWidget>()
			{
                // this.useWornGraphicMask == s.useWornGraphicMask &&
                //    this.canBeGeneratedToSatisfyWarmth == s.canBeGeneratedToSatisfyWarmth &&
                //    this.gender == s.gender &&
                new FloatInputWidget<ApparelProperties>(this.apparelProperties, "Wear Per Day", s => s.wearPerDay, (s, v) => s.wearPerDay = v),
				new BoolInputWidget<ApparelProperties>(this.apparelProperties, "Care If Worn By Corpse", s => s.careIfWornByCorpse, (s, v) => s.careIfWornByCorpse = v),
				new BoolInputWidget<ApparelProperties>(this.apparelProperties, "Hat Rendered Front OfFace", s => s.hatRenderedFrontOfFace, (s, v) => s.hatRenderedFrontOfFace = v),
				new BoolInputWidget<ApparelProperties>(this.apparelProperties, "Use Deflect Metal Effect", s => s.useDeflectMetalEffect, (s, v) => s.useDeflectMetalEffect = v),
                new BoolInputWidget<ApparelProperties>(this.apparelProperties, "Use Worn Graphic Mask", s => s.useWornGraphicMask, (s, b) => s.useWornGraphicMask = b),
                new BoolInputWidget<ApparelProperties>(this.apparelProperties, "Can Be Generated for Warmth", s => s.canBeGeneratedToSatisfyWarmth, (s, b) => s.canBeGeneratedToSatisfyWarmth = b),
                new EnumInputWidget<ApparelProperties, Gender>(this.apparelProperties, "Gender", 100f, s => s.gender, (s, v) => s.gender = v),
				new TextPlusMinusInputWidget("Tags", 100f, this.apparelProperties.tags)
            };

			this.bodyPartGroupArgs = new WindowUtil.PlusMinusArgs<BodyPartGroupDef>()
			{
				allItems = DefDatabase<BodyPartGroupDef>.AllDefs,
				beingUsed = () => this.apparelProperties?.bodyPartGroups,
				onAdd = v => this.apparelProperties.bodyPartGroups.Add(v),
				onRemove = v => this.apparelProperties.bodyPartGroups.Remove(v),
				getDisplayName = v => Util.GetLabel(v),
			};

			this.apparelLayerArgs = new WindowUtil.PlusMinusArgs<ApparelLayerDef>()
			{
				allItems = DefDatabase<ApparelLayerDef>.AllDefs,
				beingUsed = () => this.apparelProperties?.layers,
				onAdd = v => this.apparelProperties.layers.Add(v),
				onRemove = v => this.apparelProperties.layers.Remove(v),
				getDisplayName = v => Util.GetLabel(v),
			};
		}

		public string DisplayLabel => "Apprel Properties";

		public void Draw(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Body Part Groups", this.bodyPartGroupArgs);
			WindowUtil.DrawList(x, ref y, width, this.apparelProperties.bodyPartGroups);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Apparel Layers", this.apparelLayerArgs);
			WindowUtil.DrawList(x, ref y, width, this.apparelProperties.layers);
		}

		public void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
		}

	}
}
