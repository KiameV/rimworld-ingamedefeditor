using InGameDefEditor.Stats.DefStat;
using Verse;

namespace InGameDefEditor.Stats
{
	public interface IParentStat : IDefStat
    {
        void ApplyStats(Def to);
	}

	public interface IInitializable
	{
		string Label { get; }
		bool Initialize();
	}
}
