using InGameDefEditor.Stats.DefStat;
using RimWorld;
using Verse;

namespace InGameDefEditor.Stats
{
	public interface IParentStat
	{
		void ApplyStats(object to);
	}

	public interface IInitializable
	{
		string Label { get; }
		bool Initialize();
	}
/*
	public abstract class ADefStat<D> : IParentStat<D> where D : Def
	{
		protected readonly D def;

		public D Def => def;

		protected ADefStat(D def)
		{
			this.def = def;
		}

		public abstract void ApplyStats(object to);
	}

	public abstract class ABackstroyStat : IParentStat<Backstory>
	{
		protected readonly Backstory backstory;

		public Backstory Def => backstory;

		protected ABackstroyStat(Backstory backstory)
		{
			this.backstory = backstory;
		}

		public abstract void ApplyStats(object to);
	}*/
}
