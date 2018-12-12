using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats
{
	public abstract class ABuildableDefStat<D> : DefStat<D>, IParentStat where D : BuildableDef, new()
	{
		//public List<Type> placeWorkers;
		//public Graphic graphic = BaseContent.BadGraphic;
		//public Texture2D uiIcon = BaseContent.BadTex;
		//public float uiIconAngle;
		//public int uiIconForStackCount = -1;
		//public Vector2Stats uiIconOffset;
		//public string uiIconPath;
		//public ColorStats uiIconColor = Color.white;
		//private List<PlaceWorker> placeWorkersInstantiatedInt;

		public Traversability passability;
		public int pathCost;
		public bool pathCostIgnoreRepeat;
		public float fertility;
		public int costStuffCount;
		public int placingDraggableDimensions;
		public bool clearBuildingArea;
		public Rot4Stats defaultPlacingRot;
		public float resourcesFractionWhenDeconstructed;
		public int constructionSkillPrerequisite;
		public TechLevel minTechLevelToBuild;
		public TechLevel maxTechLevelToBuild;
		public AltitudeLayer altitudeLayer;
		public bool menuHidden;
		public float specialDisplayRadius;

		public DefStat<TerrainAffordanceDef> terrainAffordanceNeeded;
		public DefStat<EffecterDef> repairEffect;
		public DefStat<EffecterDef> constructEffect;
		public DefStat<DesignationCategoryDef> designationCategory;
		public DefStat<DesignatorDropdownGroupDef> designatorDropdown;
		public DefStat<KeyBindingDef> designationHotKey;
		public DefStat<ThingDef> blueprintDef;
		public DefStat<ThingDef> installBlueprintDef;
		public DefStat<ThingDef> frameDef;

		public List<DefStat<StuffCategoryDef>> stuffCategories;
		public List<DefStat<ThingDef>> buildingPrerequisites;
		public List<DefStat<ResearchProjectDef>> researchPrerequisites;
		public List<FloatValueDefStat<StatDef>> statBases = null;
		public List<IntValueDefStat<ThingDef>> costList;

		public string UniqueKey => base.defName;

		public ABuildableDefStat() { }
		public ABuildableDefStat(D d) : base(d)
		{
			this.passability = d.passability;
			this.pathCost = d.pathCost;
			this.pathCostIgnoreRepeat = d.pathCostIgnoreRepeat;
			this.fertility = d.fertility;
			this.costStuffCount = d.costStuffCount;
			this.placingDraggableDimensions = d.placingDraggableDimensions;
			this.clearBuildingArea = d.clearBuildingArea;
			this.defaultPlacingRot = new Rot4Stats(d.defaultPlacingRot);
			this.resourcesFractionWhenDeconstructed = d.resourcesFractionWhenDeconstructed;
			this.constructionSkillPrerequisite = d.constructionSkillPrerequisite;
			this.minTechLevelToBuild = d.minTechLevelToBuild;
			this.maxTechLevelToBuild = d.maxTechLevelToBuild;
			this.altitudeLayer = d.altitudeLayer;
			this.menuHidden = d.menuHidden;
			this.specialDisplayRadius = d.specialDisplayRadius;

			Util.AssignDefStat(d.terrainAffordanceNeeded, out this.terrainAffordanceNeeded);
			Util.AssignDefStat(d.repairEffect, out this.repairEffect);
			Util.AssignDefStat(d.constructEffect, out this.constructEffect);
			Util.AssignDefStat(d.designationCategory, out this.designationCategory);
			Util.AssignDefStat(d.designatorDropdown, out this.designatorDropdown);
			Util.AssignDefStat(d.designationHotKey, out this.designationHotKey);
			Util.AssignDefStat(d.blueprintDef, out this.blueprintDef);
			Util.AssignDefStat(d.installBlueprintDef, out this.installBlueprintDef);
			Util.AssignDefStat(d.frameDef, out this.frameDef);

			Util.Populate(out this.stuffCategories, d.stuffCategories, v => new DefStat<StuffCategoryDef>(v));
			Util.Populate(out this.buildingPrerequisites, d.buildingPrerequisites, v => new DefStat<ThingDef>(v));
			Util.Populate(out this.researchPrerequisites, d.researchPrerequisites, v => new DefStat<ResearchProjectDef>(v));
			Util.Populate(out this.statBases, d.statBases, v => new FloatValueDefStat<StatDef>(v.stat, v.value));
			Util.Populate(out this.costList, d.costList, v => new IntValueDefStat<ThingDef>(v.thingDef, v.count));
		}

		public virtual void ApplyStats(object t)
		{
			if (t is BuildableDef to)
			{
				to.passability = this.passability;
				to.pathCost = this.pathCost;
				to.pathCostIgnoreRepeat = this.pathCostIgnoreRepeat;
				to.fertility = this.fertility;
				to.costStuffCount = this.costStuffCount;
				to.placingDraggableDimensions = this.placingDraggableDimensions;
				to.clearBuildingArea = this.clearBuildingArea;
				to.defaultPlacingRot = this.defaultPlacingRot.ToRot4();
				to.resourcesFractionWhenDeconstructed = this.resourcesFractionWhenDeconstructed;
				to.constructionSkillPrerequisite = this.constructionSkillPrerequisite;
				to.minTechLevelToBuild = this.minTechLevelToBuild;
				to.maxTechLevelToBuild = this.maxTechLevelToBuild;
				to.altitudeLayer = this.altitudeLayer;
				to.menuHidden = this.menuHidden;
				to.specialDisplayRadius = this.specialDisplayRadius;

				Util.AssignDef(this.terrainAffordanceNeeded, out to.terrainAffordanceNeeded);
				Util.AssignDef(this.repairEffect, out to.repairEffect);
				Util.AssignDef(this.constructEffect, out to.constructEffect);
				Util.AssignDef(this.designationCategory, out to.designationCategory);
				Util.AssignDef(this.designatorDropdown, out to.designatorDropdown);
				Util.AssignDef(this.designationHotKey, out to.designationHotKey);
				Util.AssignDef(this.blueprintDef, out to.blueprintDef);
				Util.AssignDef(this.installBlueprintDef, out to.installBlueprintDef);
				Util.AssignDef(this.frameDef, out to.frameDef);

				Util.Populate(out to.stuffCategories, this.stuffCategories, v => v.Def);
				Util.Populate(out to.buildingPrerequisites, this.buildingPrerequisites, v => v.Def);
				Util.Populate(out to.researchPrerequisites, this.researchPrerequisites, v => v.Def);
				Util.Populate(out to.statBases, this.statBases, v => new StatModifier() { stat = v.Def, value = v.value });
				Util.Populate(out to.costList, this.costList, v => new ThingDefCountClass(v.Def, v.value));
			}
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;

			Util.InitializeDefStat(this.terrainAffordanceNeeded);
			Util.InitializeDefStat(this.repairEffect);
			Util.InitializeDefStat(this.constructEffect);
			Util.InitializeDefStat(this.designationCategory);
			Util.InitializeDefStat(this.designatorDropdown);
			Util.InitializeDefStat(this.designationHotKey);
			Util.InitializeDefStat(this.blueprintDef);
			Util.InitializeDefStat(this.installBlueprintDef);
			Util.InitializeDefStat(this.frameDef);

			this.stuffCategories?.ForEach(v => Util.InitializeDefStat(v));
			this.buildingPrerequisites?.ForEach(v => Util.InitializeDefStat(v));
			this.researchPrerequisites?.ForEach(v => Util.InitializeDefStat(v));
			this.statBases?.ForEach(v => Util.InitializeDefStat(v));
			this.costList?.ForEach(v => Util.InitializeDefStat(v));

			return true;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) && 
				obj is ABuildableDefStat<D> d)
			{
				return 
					this.passability == d.passability &&
					this.pathCost == d.pathCost &&
					this.pathCostIgnoreRepeat == d.pathCostIgnoreRepeat &&
					this.fertility == d.fertility &&
					this.costStuffCount == d.costStuffCount &&
					this.placingDraggableDimensions == d.placingDraggableDimensions &&
					this.clearBuildingArea == d.clearBuildingArea &&
					this.defaultPlacingRot.rotInt == d.defaultPlacingRot.rotInt &&
					this.resourcesFractionWhenDeconstructed == d.resourcesFractionWhenDeconstructed &&
					this.constructionSkillPrerequisite == d.constructionSkillPrerequisite &&
					this.minTechLevelToBuild == d.minTechLevelToBuild &&
					this.maxTechLevelToBuild == d.maxTechLevelToBuild &&
					this.altitudeLayer == d.altitudeLayer &&
					this.menuHidden == d.menuHidden &&
					this.specialDisplayRadius == d.specialDisplayRadius &&
					Util.AreEqual(d.terrainAffordanceNeeded, this.terrainAffordanceNeeded) && 
					Util.AreEqual(d.repairEffect, this.repairEffect) && 
					Util.AreEqual(d.constructEffect, this.constructEffect) && 
					Util.AreEqual(d.designationCategory, this.designationCategory) && 
					Util.AreEqual(d.designatorDropdown, this.designatorDropdown) && 
					Util.AreEqual(d.designationHotKey, this.designationHotKey) && 
					Util.AreEqual(d.blueprintDef, this.blueprintDef) && 
					Util.AreEqual(d.installBlueprintDef, this.installBlueprintDef) && 
					Util.AreEqual(d.frameDef, this.frameDef) && 
					Util.AreEqual(this.stuffCategories, d.stuffCategories) &&
					Util.AreEqual(this.buildingPrerequisites, d.buildingPrerequisites) &&
					Util.AreEqual(this.researchPrerequisites, d.researchPrerequisites) &&
					Util.AreEqual(this.statBases, d.statBases) &&
					Util.AreEqual(this.costList, d.costList);
			}
			return false;
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
