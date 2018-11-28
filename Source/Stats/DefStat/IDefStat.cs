using System;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	public interface IStat<T>
	{
		void ApplyStats(T to);
	}

	public interface IDefStat
    {
        Def BaseDef { get; }
    }
}
