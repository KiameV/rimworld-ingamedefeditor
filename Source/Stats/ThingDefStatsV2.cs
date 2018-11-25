using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats
{
    internal class ThingDefStatsV2 : ThingDefStats
    {
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
        public DefStat<BuildableDef> entityDefToBuild;

        public ApparelPropertiesStats apparel = null;
        public IntRangeStat sdeepLumpSizeRange;
        public IntVec3Stats interactionCellOffset;
        public FloatRangeStats startingHpRange;
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
        public GraphicStats interactionCellGraphic;

        public List<DefStat<ThingCategoryDef>> thingCategories;
        public List<IngredientCountStats> killedLeavings;
        public List<IngredientCountStats> butcherProducts;
        public List<IngredientCountStats> smeltProducts;
        public List<DamageMultiplierStats> damageMultipliers;
        public List<DefStat<ThingSetMakerDef>> thingSetMakers;
        public List<DefStat<StuffCategoryDef>> stuffCategories;
        public List<string> comps;
        public List<string> tradeTags;
        public List<string> weaponTags;
        public List<string> techHediffsTags;

        //public List<Type> inspectorTabs;
        //private string descriptionDetailedCached;
        //private List<Verse.RecipeDef> allRecipesCached;
        //private Dictionary<ThingDef, Thing> concreteExamplesInt;
        
        public ThingDefStatsV2() : base() { }
        public ThingDefStatsV2(ThingDef d) : base(d)
        {
            this.thingSetMakers = new List<DefStat<ThingSetMakerDef>>();
            if (d.thingSetMakerTags != null)
                foreach (var v in d.thingSetMakerTags)
                    this.thingSetMakers.Add(new DefStat<ThingSetMakerDef>(v));

            this.comps = new List<string>();
            if (d.comps != null)
                foreach (var v in d.comps)
                    this.comps.Add(v.compClass.FullName);

            this.stuffCategories = new List<DefStat<StuffCategoryDef>>();
            if (d.stuffCategories != null)
                foreach (var v in d.stuffCategories)
                    this.stuffCategories.Add(new DefStat<StuffCategoryDef>(v));

            if (d.apparel != null)
                this.apparel = new ApparelPropertiesStats(d.apparel);
        }

        public override void ApplyStats(Def to)
        {
            base.ApplyStats(to);

            if (to is ThingDef t)
            {
                if (t.thingSetMakerTags == null && !Util.IsNullEmpty(this.thingSetMakers))
                    t.thingSetMakerTags = new List<string>();
                if (t.thingSetMakerTags != null)
                {
                    t.thingSetMakerTags.Clear();
                    Util.Populate(t.thingSetMakerTags, this.thingSetMakers, (d) => d.defName);
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
            }
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            foreach (var v in this.thingSetMakers)
                v.Initialize();

            foreach (var v in this.stuffCategories)
                v.Initialize();

            this.apparel?.Initialize();

            return true;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && 
                obj is ThingDefStatsV2 t)
            {

            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
