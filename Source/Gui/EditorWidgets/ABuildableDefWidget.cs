using InGameDefEditor.Gui.Dialog;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    abstract class ABuildableDefWidget<D> : AParentDefStatWidget<D> where D : BuildableDef, new()
	{
		private enum Direction { North, South, East, West }

		protected readonly List<IInputWidget> inputWidgets;
		protected readonly List<FloatInputWidget<StatModifier>> statBases = new List<FloatInputWidget<StatModifier>>();
		private readonly WindowUtil.PlusMinusArgs<StatDef> statBasesPlusMinus;

		public ABuildableDefWidget(D def, DefType type) : base(def, type)
        {
            if (base.Def.statBases == null)
                base.Def.statBases = new List<StatModifier>();

			this.inputWidgets = new List<IInputWidget>()
			{
				new EnumInputWidget<D, Traversability>(base.Def, "Passability", 200, d => d.passability, (d, v) => d.passability = v),
				new IntInputWidget<D>(base.Def, "Path Cost", d => d.pathCost, (d, v) => d.pathCost = v),
				new BoolInputWidget<D>(base.Def, "Path Cost Ignore Repeat", d => d.pathCostIgnoreRepeat, (d, v) => d.pathCostIgnoreRepeat = v),
				new FloatInputWidget<D>(base.Def, "Fertility", d => d.fertility, (d, v) => d.fertility = v),
				new IntInputWidget<D>(base.Def, "Cost Stuff Count", d => d.costStuffCount, (d, v) => d.costStuffCount = v),
				new IntInputWidget<D>(base.Def, "Placing Draggable Dimensions", d => d.placingDraggableDimensions, (d, v) => d.placingDraggableDimensions = v),
				new BoolInputWidget<D>(base.Def, "Clear Building Area", d => d.clearBuildingArea, (d, v) => d.clearBuildingArea = v),
				new FloatInputWidget<D>(base.Def, "Resources Fraction When Deconstructed", d => d.resourcesFractionWhenDeconstructed, (d, v) => d.resourcesFractionWhenDeconstructed = v),
				new IntInputWidget<D>(base.Def, "Construction Skill Prerequisite", d => d.constructionSkillPrerequisite, (d, v) => d.constructionSkillPrerequisite = v),
				new EnumInputWidget<D, TechLevel>(base.Def, "Min Tech Level To Build", 200, d => d.minTechLevelToBuild, (d, v) => d.minTechLevelToBuild = v),
				new EnumInputWidget<D, TechLevel>(base.Def, "Max Tech Level To Build", 200, d => d.maxTechLevelToBuild, (d, v) => d.maxTechLevelToBuild = v),
				new EnumInputWidget<D, AltitudeLayer>(base.Def, "Altitude Layer", 200, d => d.altitudeLayer, (d, v) => d.altitudeLayer = v),
				new BoolInputWidget<D>(base.Def, "Menu Hidden", d => d.menuHidden, (d, v) => d.menuHidden = v),
				new FloatInputWidget<D>(base.Def, "Special Display Radius", d => d.specialDisplayRadius, (d, v) => d.specialDisplayRadius = v),

				new DefInputWidget<D, TerrainAffordanceDef>(base.Def, "Terrain Affordance Needed", 200, d => d.terrainAffordanceNeeded, (d, v) => d.terrainAffordanceNeeded = v, true),
				new DefInputWidget<D, EffecterDef>(base.Def, "Repair Effect", 200, d => d.repairEffect, (d, v) => d.repairEffect = v, true),
				new DefInputWidget<D, EffecterDef>(base.Def, "Construct Effect", 200, d => d.constructEffect, (d, v) => d.constructEffect = v, true),
				new DefInputWidget<D, DesignationCategoryDef>(base.Def, "Designation Category", 200, d => d.designationCategory, (d, v) => d.designationCategory = v, true),
				new DefInputWidget<D, DesignatorDropdownGroupDef>(base.Def, "Designator Dropdown", 200, d => d.designatorDropdown, (d, v) => d.designatorDropdown = v, true),
				new DefInputWidget<D, KeyBindingDef>(base.Def, "Key Binding", 200, d => d.designationHotKey, (d, v) => d.designationHotKey = v, true),
				new DefInputWidget<D, ThingDef>(base.Def, "Blueprint", 200, d => d.blueprintDef, (d, v) => d.blueprintDef = v, true),
				new DefInputWidget<D, ThingDef>(base.Def, "Install Blueprint", 200, d => d.installBlueprintDef, (d, v) => d.installBlueprintDef = v, true),
				new DefInputWidget<D, ThingDef>(base.Def, "Frame", 200, d => d.frameDef, (d, v) => d.frameDef = v, true),

				new EnumInputWidget<D, Direction>(base.Def, "Default Placing Rotation", 200,
				d =>
				{
					switch (d.defaultPlacingRot.AsAngle)
					{
						case 0f:
							return Direction.North;
						case 90f:
							return Direction.East;
						case 180f:
							return Direction.South;
						default:
							return Direction.West;
					}
				},
				(d, v) =>
				{
					switch (v)
					{
						case Direction.North:
							d.defaultPlacingRot = Rot4.North;
							break;
						case Direction.East:
							d.defaultPlacingRot = Rot4.East;
							break;
						case Direction.South:
							d.defaultPlacingRot = Rot4.South;
							break;
						default:
							d.defaultPlacingRot = Rot4.West;
							break;
					}
				}),
			};

			this.statBasesPlusMinus = new WindowUtil.PlusMinusArgs<StatDef>()
			{
				allItems = DefDatabase<StatDef>.AllDefs,
				getDisplayName = d => Util.GetLabel(d),
				beingUsed = () =>
				{
					List<StatDef> l = new List<StatDef>(base.Def.statBases.Count);
					base.Def.statBases.ForEach(v => l.Add(v.stat));
					return l;
				},
				onAdd = v =>
				{
					StatModifier sm = new StatModifier() { stat = v, value = 0 };
					base.Def.statBases.Add(sm);
					this.statBases.Add(this.CreateFloatInput(sm));
				},
				onRemove = v =>
				{
					base.Def.statBases.RemoveAll(sm => sm.stat == v);
					this.statBases.RemoveAll(w => w.Parent.stat == v);
				},
			};
		}
		public abstract void DrawLeftInput(float x, ref float y, float width);
		public abstract void DrawMiddleInput(float x, ref float y, float width);
		public abstract void DrawRightInput(float x, ref float y, float width);

		public sealed override void DrawLeft(float x, ref float y, float width)
        {
			y += 10;
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
			
			this.DrawLeftInput(x, ref y, width);
        }
		
		public sealed override void DrawMiddle(float x, ref float y, float width)
		{
			this.DrawMiddleInput(x, ref y, width);
		}

		public sealed override void DrawRight(float x, ref float y, float width)
		{
			this.DrawStatModifiers(x, ref y, width);

			y += 10;
			this.DrawRightInput(x, ref y, width);
		}

		private void DrawStatModifiers(float x, ref float y, float width)
        {
			WindowUtil.PlusMinusLabel(x, ref y, width, "Base Modifiers", this.statBasesPlusMinus);

			x += 10;
            foreach (var w in this.statBases)
                w.Draw(x, ref y, width);
        }

        protected IEnumerable<StatDef> GetPossibleStatModifiers(IEnumerable<StatModifier> statModifiers)
        {
            HashSet<string> lookup = new HashSet<string>();
            if (statModifiers != null)
            {
                foreach (StatModifier s in statModifiers)
                    lookup.Add(s.stat.defName);
            }

            SortedDictionary<string, StatDef> sorted = new SortedDictionary<string, StatDef>();
            foreach (StatDef d in DefDatabase<StatDef>.AllDefs)
                if (!lookup.Contains(d.defName))
                    sorted[d.label] = d;

            return sorted.Values;
        }

        public override void Rebuild()
        {
            this.statBases.Clear();
			base.Def.statBases?.ForEach(v => this.statBases.Add(this.CreateFloatInput(v)));
        }

        public override void ResetBuffers()
        {
			base.ResetBuffers();
			this.inputWidgets?.ForEach(v => v.ResetBuffers());
			this.statBases?.ForEach(v => v.ResetBuffers());
        }

        protected FloatInputWidget<StatModifier> CreateFloatInput(StatModifier sm)
        {
            return new FloatInputWidget<StatModifier>(sm, sm.stat.label, (StatModifier m) => m.value, (StatModifier m, float f) => m.value = f);
        }
    }
}
