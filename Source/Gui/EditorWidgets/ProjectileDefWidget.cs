using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ProjectileDefWidget : AParentStatWidget<ThingDef>, IDefEditorWidget
    {
        public readonly ThingDef ProjectileDef;

        private readonly List<IInputWidget> inputWidgets;

        public ProjectileDefWidget(ThingDef d, DefType type) : base(d, type)
        {
            this.ProjectileDef = d;

            this.inputWidgets = new List<IInputWidget>()
            {
                new IntInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Base Damage", (ProjectileProperties p) => ProjectileDefStats.GetDamage(p), (ProjectileProperties p, int i) => ProjectileDefStats.SetDamage(p, i)),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Stopping Power", (ProjectileProperties p) => p.stoppingPower, (ProjectileProperties p, float f) => this.ProjectileDef.projectile.stoppingPower = f),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Armor Penetration", (ProjectileProperties p) => ProjectileDefStats.GetArmorPenetration(p), (ProjectileProperties p, float f) => ProjectileDefStats.SetArmorPenetration(this.ProjectileDef.projectile, f)),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Speed", (ProjectileProperties p) => p.speed, (ProjectileProperties p, float f) => p.speed = f)
            };

            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            foreach (var w in this.inputWidgets)
                w.Draw(x, ref y, width);
        }

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var w in this.inputWidgets)
				w.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{

		}

		public override void DrawRight(float x, ref float y, float width)
		{

		}

		public override void Rebuild()
		{
			foreach (var v in this.inputWidgets)
				v.ResetBuffers();
		}
		
		public override void ResetBuffers()
		{
			foreach (var w in this.inputWidgets)
				w.ResetBuffers();
		}
	}
}
