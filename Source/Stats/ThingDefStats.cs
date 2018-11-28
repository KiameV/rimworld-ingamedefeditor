using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats
{
    public class ThingDefStats : DefStat<ThingDef>, IParentStat
	{
		//public GraphicStats interactionCellGraphic;

		public List<FloatValueDefStat<StatDef>> StatModifiers = null;
        public List<VerbStats> VerbStats = null;
        public List<ToolStats> Tools = null;
        public List<FloatValueDefStat<StatDef>> EquippedStatOffsets = null;
		
		public bool destroyable;
		public bool rotatable;
		public bool smallVolume;
		public bool useHitPoints;
		public bool receivesSignals;
		public bool smeltable;
		public bool randomizeRotationOnSpawn;
		public bool isTechHediff;
		public bool isUnfinishedThing;
		public bool leaveResourcesWhenKilled;
		public bool isFrameInt;
		public bool hasInteractionCell;
		public bool interactionCellIconReverse;
		public bool forceDebugSpawnable;
		public bool intricate;
		public bool scatterableOnMapGen;
		public float deepCommonality;
		public int deepCountPerCell;
		public int deepCountPerPortion;
		public float generateCommonality;
		public float generateAllowChance;
		public bool canOverlapZones;
		public bool alwaysFlee;
		public bool drawOffscreen;
		public float hideAtSnowDepth;
		public bool drawDamagedOverlay;
		public bool castEdgeShadows;
		public float staticSunShadowHeight;
		public bool selectable;
		public bool neverMultiSelect;
		public bool isAutoAttackableMapObject;
		public bool hasTooltip;
		public bool seeThroughFog;
		public bool drawGUIOverlay;
		public ResourceCountPriority resourceReadoutPriority;
		public bool resourceReadoutAlwaysShow;
		public bool drawPlaceWorkersWhileSelected;
		public float uiIconScale;
		public bool alwaysHaulable;
		public bool designateHaulable;
		public bool mineable;
		public bool socialPropernessMatters;
		public bool stealable;
		public bool saveCompressible;
		public bool isSaveable;
		public bool holdsRoof;
		public float fillPercent;
		public bool coversFloor;
		public bool neverOverlapFloors;
		public SurfaceType surfaceType;
		public bool blockPlants;
		public bool blockLight;
		public bool blockWind;
		public Tradeability tradeability;
		public bool tradeNeverStack;
		public float equippedAngleOffset;
		public EquipmentType equipmentType;
		public TechLevel techLevel;
		public bool destroyOnDrop;
		public DrawerType drawerType;

		public DefStat<ThingDef> slagDef;
		public DefStat<ThingDef> interactionCellIcon;
		public DefStat<ThingDef> filthLeaving;
		public DefStat<ThingDef> minifiedDef;
		public DefStat<SoundDef> soundDrop;
		public DefStat<SoundDef> soundPickup;
		public DefStat<SoundDef> soundInteract;
		public DefStat<SoundDef> soundImpactDefault;
		public DefStat<Def> entityDefToBuild;

		public ApparelPropertiesStats apparel;
		public MinMaxIntStats deepLumpSizeRange;
		public IntVec3Stats interactionCellOffset;
		public MinMaxFloatStats startingHpRange;
		public GraphicDataStats graphicData;
		public IngestiblePropertiesStats ingestible;
		public FilthPropertiesStats filth;
		public GasPropertiesStats gas;
		public BuildingPropertiesStats building;
		public RacePropertiesStats race;
		public MotePropertiesStats mote;
		public PlantPropertiesStats plant;
		public StuffPropertiesStats stuffProps;
		public SkyfallerPropertiesStats skyfaller;

		public List<DefStat<ThingCategoryDef>> thingCategories;
		public List<IntValueDefStat<ThingDef>> killedLeavings;
		public List<IntValueDefStat<ThingDef>> butcherProducts;
		public List<IntValueDefStat<ThingDef>> smeltProducts;
		public List<FloatValueDefStat<DamageDef>> damageMultipliers;
		public List<DefStat<StuffCategoryDef>> stuffCategories;
		public List<string> thingSetMakers;
		public List<string> comps;
		public List<string> tradeTags;
		public List<string> weaponTags;
		public List<string> techHediffsTags;
		
		public bool IsApparel => base.Def.IsApparel;
        public bool IsWeapon => base.Def.IsWeapon;

        public ThingDefStats() { }
		public ThingDefStats(ThingDef d) : base(d)
		{
			Util.Populate(out this.StatModifiers, Def.statBases, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			Util.Populate(out this.VerbStats, Def.Verbs, (v) =>
			{
				try
				{
					if (v != null)
						return new VerbStats(v);
				}
				catch (Exception e) { Log.Warning(e.GetType().Name + " -- " + e.Message); }
				Log.Warning("Poorly formatted VerbProperties in " + this.Def.defName);
				return null;
			});
			Util.Populate(out this.Tools, Def.tools, (v) => new ToolStats(v));
			Util.Populate(out this.EquippedStatOffsets, Def.equippedStatOffsets, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			
			this.destroyable = d.destroyable;
			this.rotatable = d.rotatable;
			this.smallVolume = d.smallVolume;
			this.useHitPoints = d.useHitPoints;
			this.receivesSignals = d.receivesSignals;
			this.smeltable = d.smeltable;
			this.randomizeRotationOnSpawn = d.randomizeRotationOnSpawn;
			this.isTechHediff = d.isTechHediff;
			this.isUnfinishedThing = d.isUnfinishedThing;
			this.leaveResourcesWhenKilled = d.leaveResourcesWhenKilled;
			this.isFrameInt = d.isFrameInt;
			this.hasInteractionCell = d.hasInteractionCell;
			this.interactionCellIconReverse = d.interactionCellIconReverse;
			this.forceDebugSpawnable = d.forceDebugSpawnable;
			this.intricate = d.intricate;
			this.scatterableOnMapGen = d.scatterableOnMapGen;
			this.deepCommonality = d.deepCommonality;
			this.deepCountPerCell = d.deepCountPerCell;
			this.deepCountPerPortion = d.deepCountPerPortion;
			this.generateCommonality = d.generateCommonality;
			this.generateAllowChance = d.generateAllowChance;
			this.canOverlapZones = GetCanOverlapZones(d);
			this.alwaysFlee = d.alwaysFlee;
			this.drawOffscreen = d.drawOffscreen;
			this.hideAtSnowDepth = d.hideAtSnowDepth;
			this.drawDamagedOverlay = d.drawDamagedOverlay;
			this.castEdgeShadows = d.castEdgeShadows;
			this.staticSunShadowHeight = d.staticSunShadowHeight;
			this.selectable = d.selectable;
			this.neverMultiSelect = d.neverMultiSelect;
			this.isAutoAttackableMapObject = d.isAutoAttackableMapObject;
			this.hasTooltip = d.hasTooltip;
			this.seeThroughFog = d.seeThroughFog;
			this.drawGUIOverlay = d.drawGUIOverlay;
			this.resourceReadoutPriority = d.resourceReadoutPriority;
			this.resourceReadoutAlwaysShow = d.resourceReadoutAlwaysShow;
			this.drawPlaceWorkersWhileSelected = d.drawPlaceWorkersWhileSelected;
			this.uiIconScale = d.uiIconScale;
			this.alwaysHaulable = d.alwaysHaulable;
			this.designateHaulable = d.designateHaulable;
			this.mineable = d.mineable;
			this.socialPropernessMatters = d.socialPropernessMatters;
			this.stealable = d.stealable;
			this.saveCompressible = d.saveCompressible;
			this.isSaveable = d.isSaveable;
			this.holdsRoof = d.holdsRoof;
			this.fillPercent = d.fillPercent;
			this.coversFloor = d.coversFloor;
			this.neverOverlapFloors = d.neverOverlapFloors;
			this.surfaceType = d.surfaceType;
			this.blockPlants = d.blockPlants;
			this.blockLight = d.blockLight;
			this.blockWind = d.blockWind;
			this.tradeability = d.tradeability;
			this.tradeNeverStack = d.tradeNeverStack;
			this.equippedAngleOffset = d.equippedAngleOffset;
			this.equipmentType = d.equipmentType;
			this.techLevel = d.techLevel;
			this.destroyOnDrop = d.destroyOnDrop;
			this.drawerType = d.drawerType;

			Util.AssignDefStat(d.slagDef, out this.slagDef);
			Util.AssignDefStat(d.interactionCellIcon, out this.interactionCellIcon);
			Util.AssignDefStat(d.filthLeaving, out this.filthLeaving);
			Util.AssignDefStat(d.minifiedDef, out this.minifiedDef);
			Util.AssignDefStat(d.soundDrop, out this.soundDrop);
			Util.AssignDefStat(d.soundPickup, out this.soundPickup);
			Util.AssignDefStat(d.soundInteract, out this.soundInteract);
			Util.AssignDefStat(d.soundImpactDefault, out this.soundImpactDefault);
			Util.AssignDefStat(d.entityDefToBuild, out this.entityDefToBuild);

			if (d.apparel != null)
				this.apparel = new ApparelPropertiesStats(Def.apparel);
			if (d.deepLumpSizeRange != null)
				this.deepLumpSizeRange = new MinMaxIntStats(Def.deepLumpSizeRange);
			if (d.interactionCellOffset != null)
				this.interactionCellOffset = new IntVec3Stats(Def.interactionCellOffset);
			if (d.startingHpRange != null)
				this.startingHpRange = new MinMaxFloatStats(Def.startingHpRange);
			if (d.graphicData != null)
				this.graphicData = new GraphicDataStats(Def.graphicData);
			if (d.ingestible != null)
				this.ingestible = new IngestiblePropertiesStats(Def.ingestible);
			if (d.filth != null)
				this.filth = new FilthPropertiesStats(Def.filth);
			if (d.gas != null)
				this.gas = new GasPropertiesStats(Def.gas);
			if (d.building != null)
				this.building = new BuildingPropertiesStats(Def.building);
			if (d.race != null)
				this.race = new RacePropertiesStats(Def.race);
			if (d.mote != null)
				this.mote = new MotePropertiesStats(Def.mote);
			if (d.plant != null)
				this.plant = new PlantPropertiesStats(Def.plant);
			if (d.stuffProps != null)
				this.stuffProps = new StuffPropertiesStats(Def.stuffProps);
			if (d.skyfaller != null)
				this.skyfaller = new SkyfallerPropertiesStats(Def.skyfaller);

			Util.Populate(out this.thingCategories, Def.thingCategories, (v) => new DefStat<ThingCategoryDef>(v));
			Util.Populate(out this.killedLeavings, Def.killedLeavings, (v) => new IntValueDefStat<ThingDef>(v.thingDef, v.count));
			Util.Populate(out this.butcherProducts, Def.butcherProducts, (v) => new IntValueDefStat<ThingDef>(v.thingDef, v.count));
			Util.Populate(out this.smeltProducts, Def.smeltProducts, (v) => new IntValueDefStat<ThingDef>(v.thingDef, v.count));
			Util.Populate(out this.damageMultipliers, Def.damageMultipliers, (v) => new FloatValueDefStat<DamageDef>(v.damageDef, v.multiplier));
			Util.Populate(out this.stuffCategories, Def.stuffCategories, (v) => new DefStat<StuffCategoryDef>(v));
			Util.Populate(out this.thingSetMakers, Def.thingSetMakerTags);
			Util.Populate(out this.comps, Def.comps, (v) => v.compClass.FullName);
			Util.Populate(out this.tradeTags, Def.tradeTags);
			Util.Populate(out this.weaponTags, Def.weaponTags);
			Util.Populate(out this.techHediffsTags, Def.techHediffsTags);
		}

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

			if (this.StatModifiers != null)
				this.StatModifiers.ForEach(v => Util.InitializeDefStat(v));

			if (this.VerbStats != null)
				this.VerbStats.ForEach(v => Util.InitializeDefStat(v));

			if (this.Tools != null)
				this.Tools.ForEach(v => Util.InitializeDefStat(v));

			if (this.EquippedStatOffsets != null)
				this.EquippedStatOffsets.ForEach(v => Util.InitializeDefStat(v));

			if (!base.Initialize())
				return false;

			foreach (var v in this.stuffCategories)
				v.Initialize();

			this.apparel?.Initialize();

			return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) &&
                obj is ThingDefStats stats)
            {
                if (Util.AreEqual(this.StatModifiers, stats.StatModifiers) &&
                    Util.AreEqual(this.EquippedStatOffsets, stats.EquippedStatOffsets) &&
                    Util.AreEqual(this.VerbStats, stats.VerbStats, null) &&
                    Util.AreEqual(this.Tools, stats.Tools, null))
                {
                    return true;
                }
            }
            return false;
        }

#region Apply____
		public virtual void ApplyStats(Def to)
        {
#if DEBUG_THINGDEF
            Log.Warning("ApplyStats for " + this.defName);
#endif
            if (to is ThingDef t)
            {
                try
                {
                    this.ApplyStatModifiers(t);
                    this.ApplyVerbStats(t);
                    this.ApplyTools(t);
                    this.ApplyEquipmentStatOffsets(t);
                }
                catch (Exception e)
                {
                    Log.Warning("Failed to apply stats [" + t.defName + "]\n" + e.Message);
                }
				
				if (t.thingSetMakerTags == null && !Util.IsNullEmpty(this.thingSetMakers))
					t.thingSetMakerTags = new List<string>();

				if (t.thingSetMakerTags != null)
				{
					t.thingSetMakerTags.Clear();
					Util.Populate(t.thingSetMakerTags, this.thingSetMakers);
				}

				if (t.stuffCategories == null && !Util.IsNullEmpty(this.stuffCategories))
					t.stuffCategories = new List<StuffCategoryDef>();
				if (t.stuffCategories != null)
				{
					t.stuffCategories.Clear();
					Util.Populate(t.stuffCategories, this.stuffCategories, (d) => d.Def);
				}

				if (this.apparel != null)
				{
					this.apparel.ApplyStats(t.apparel);
				}
#if DEBUG_THINGDEF
            Log.Warning("ApplyStats Done");
#endif
			}
            else
                Log.Error("ThingDefStat passed none ThingDef!");
        }

        private void ApplyEquipmentStatOffsets(ThingDef d)
        {
            if (d.equippedStatOffsets != null)
                d.equippedStatOffsets.Clear();

            if (this.EquippedStatOffsets == null || this.EquippedStatOffsets.Count == 0)
                return;

            if (d.equippedStatOffsets == null)
                d.equippedStatOffsets = new List<StatModifier>(this.EquippedStatOffsets.Count);

            Dictionary<string, StatModifier> lookup = new Dictionary<string, StatModifier>();
            foreach (StatModifier to in d.equippedStatOffsets)
                lookup.Add(to.stat.defName, to);
            
            foreach (var from in this.EquippedStatOffsets)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = ((FloatValueDefStat<StatDef>)from).value;
                }
                else
                {
                    d.equippedStatOffsets.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = ((FloatValueDefStat<StatDef>)from).value
                    });
                }
            }
        }

        private void ApplyStatModifiers(ThingDef d)
        {
            if (d.statBases != null)
                d.statBases.Clear();

            if (this.StatModifiers == null || this.StatModifiers.Count == 0)
                return;

            if (d.statBases == null)
                d.statBases = new List<StatModifier>(this.StatModifiers.Count);

            Dictionary<string, StatModifier> lookup = new Dictionary<string, StatModifier>();
            foreach (StatModifier m in d.statBases)
            {
                lookup.Add(m.stat.ToString(), m);
            }

            foreach (var from in this.StatModifiers)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = ((FloatValueDefStat<StatDef>)from).value;
                }
                else
                {
                    d.statBases.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = ((FloatValueDefStat<StatDef>)from).value
                    });
                }
            }
            lookup.Clear();
            lookup = null;
        }

        private void ApplyVerbStats(ThingDef d)
        {
            if (d.Verbs == null || d.Verbs.Count == 0)
                return;

            if (this.VerbStats == null || this.VerbStats.Count == 0)
            {
                Log.Error("Null or Empty verbs " + this.Def.defName);
                return;
            }

            Dictionary<string, VerbProperties> lookup = new Dictionary<string, VerbProperties>();
            foreach (VerbProperties v in d.Verbs)
            {
                lookup.Add(v.label, v);
            }
            
            foreach (VerbStats from in this.VerbStats)
            {
                if (lookup.TryGetValue(from.label, out VerbProperties to))
                {
                    from.ApplyStats(to);
                }
            }
            lookup.Clear();
            lookup = null;
        }

        private void ApplyTools(ThingDef d)
        {
            if (d.tools != null)
                d.tools.Clear();

            if (this.Tools == null || this.Tools.Count == 0)
                return;

            if (d.tools == null)
                d.tools = new List<Tool>(this.Tools.Count);

            Dictionary<string, Tool> lookup = new Dictionary<string, Tool>();
            foreach (Tool to in d.tools)
            {
                lookup.Add(to.label, to);
            }

            foreach (ToolStats from in this.Tools)
            {
                if (lookup.TryGetValue(from.label, out Tool to))
                {
                    from.ApplyStats(to);
                }
                else
                {
                    Tool t = new Tool();
                    from.ApplyStats(t);
                    d.tools.Add(t);
                }
            }
        }
#endregion
		public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Stats:");
            sb.Append(Environment.NewLine);
            sb.AppendLine("    DefName: " + this.defName);
            sb.AppendLine("    StatModifiers: " + ((this.StatModifiers == null) ? "null" : ""));
            if (this.StatModifiers != null)
            {
                foreach (var s in this.StatModifiers)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    VerbStats: " + ((this.VerbStats == null) ? "null" : ""));
            if (this.VerbStats != null)
            {
                foreach (VerbStats s in this.VerbStats)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    Tools: " + ((this.Tools == null) ? "null" : ""));
            if (this.Tools != null)
            {
                foreach (ToolStats s in this.Tools)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    EquippedStatOffsets: " + ((this.EquippedStatOffsets == null) ? "null" : ""));
            if (this.EquippedStatOffsets != null)
            {
                foreach (var s in this.EquippedStatOffsets)
                    sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }

		public static bool GetCanOverlapZones(ThingDef d)
		{
			return (bool)typeof(ThingDef).GetField("canOverlapZones", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(d);
		}

		public static void SetCanOverlapZones(ThingDef d, bool b)
		{
			typeof(ThingDef).GetField("canOverlapZones", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(d, b);
		}
	}
}
