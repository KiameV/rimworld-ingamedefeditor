using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	public class Rot4Stats
	{
		public byte rotInt;

		public Rot4Stats() {}
		public Rot4Stats(Rot4 r)
		{
			this.rotInt = GetRotInt(r);
		}
		public Rot4 ToRot4()
		{
			return new Rot4(this.rotInt);
		}

		public static byte GetRotInt(Rot4 r)
		{
			return (byte)typeof(Rot4).GetField("rotInt", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(r);
		}

		public static void SetRotInt(Rot4 r, byte v)
		{
			typeof(Rot4).GetField("rotInt", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(r, v);
		}
	}
}
