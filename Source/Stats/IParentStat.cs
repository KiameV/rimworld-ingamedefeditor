using InGameDefEditor.Stats.Misc;
using Verse;

namespace InGameDefEditor.Stats
{
    interface IParentStat : IDefStat
    {
        void ApplyStats(Def def);
    }
}
