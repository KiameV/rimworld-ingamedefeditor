using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class AddedBodyPartPropsStats
	{
		public float partEfficiency;
		public bool solid;
		public bool isGoodWeapon;
		public bool betterThanNatural;

		public AddedBodyPartPropsStats() { }
		public AddedBodyPartPropsStats(AddedBodyPartProps p)
		{
			this.partEfficiency = p.partEfficiency;
			this.solid = p.solid;
			this.isGoodWeapon = p.isGoodWeapon;
			this.betterThanNatural = p.betterThanNatural;
		}

		public AddedBodyPartProps ToAddedBodyPartProps()
		{
			return new AddedBodyPartProps()
			{
				partEfficiency = this.partEfficiency,
				solid = this.solid,
				isGoodWeapon = this.isGoodWeapon,
				betterThanNatural = this.betterThanNatural,
			};
		}

		public override bool Equals(object obj)
		{
			if (obj is AddedBodyPartProps p)
			{
				return
					this.partEfficiency == p.partEfficiency &&
					this.solid == p.solid &&
					this.isGoodWeapon == p.isGoodWeapon &&
					this.betterThanNatural == p.betterThanNatural;
			}
			return false;
		}
	}
}
