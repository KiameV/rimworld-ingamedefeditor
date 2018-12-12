using InGameDefEditor.Stats.DefStat;
using Verse;

namespace InGameDefEditor.Stats
{
	public interface IParentStat
	{
		string UniqueKey { get; }
		void ApplyStats(object to);
	}

	public interface IInitializable
	{
		string Label { get; }
		bool Initialize();
	}
}
