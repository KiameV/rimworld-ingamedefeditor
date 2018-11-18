using InGameDefEditor.Stats.DefStat;
using Verse;

namespace InGameDefEditor.Stats
{
    interface IParentStat : IDefStat
    {
        void ApplyStats(Def to);
    }
}
