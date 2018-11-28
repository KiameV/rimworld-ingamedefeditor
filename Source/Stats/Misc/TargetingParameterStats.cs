using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class TargetingParameterStats
	{
		public bool canTargetLocations;
		public bool canTargetSelf;
		public bool canTargetPawns;
		public bool canTargetFires;
		public bool canTargetBuildings;
		public bool canTargetItems;
		//public List<Faction> onlyTargetFactions;
		//public Predicate<TargetInfo> validator;
		public bool onlyTargetFlammables;
		//public Thing targetSpecificThing;
		public bool mustBeSelectable;
		public bool neverTargetDoors;
		public bool neverTargetIncapacitated;
		public bool onlyTargetThingsAffectingRegions;
		public bool onlyTargetDamagedThings;
		public bool mapObjectTargetsMustBeAutoAttackable;
		public bool onlyTargetIncapacitatedPawns;

		public TargetingParameterStats() { }
		public TargetingParameterStats(TargetingParameters p)
		{
			this.canTargetLocations = p.canTargetLocations;
			this.canTargetSelf = p.canTargetSelf;
			this.canTargetPawns = p.canTargetPawns;
			this.canTargetFires = p.canTargetFires;
			this.canTargetBuildings = p.canTargetBuildings;
			this.canTargetItems = p.canTargetItems;
			this.onlyTargetFlammables = p.onlyTargetFlammables;
			this.mustBeSelectable = p.mustBeSelectable;
			this.neverTargetDoors = p.neverTargetDoors;
			this.neverTargetIncapacitated = p.neverTargetIncapacitated;
			this.onlyTargetThingsAffectingRegions = p.onlyTargetThingsAffectingRegions;
			this.onlyTargetDamagedThings = p.onlyTargetDamagedThings;
			this.mapObjectTargetsMustBeAutoAttackable = p.mapObjectTargetsMustBeAutoAttackable;
			this.onlyTargetIncapacitatedPawns = p.onlyTargetIncapacitatedPawns;
		}
	}
}
