using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ProjectileDefWidget : IDefEditorWidget
    {
        public readonly ThingDef ProjectileDef;

        private readonly List<IInputWidget> inputWidgets;

        public ProjectileDefWidget(ThingDef d)
        {
            this.ProjectileDef = d;

            this.inputWidgets = new List<IInputWidget>()
            {
                new IntInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Base Damage", (ProjectileProperties p) => ProjectileStats.GetDamage(p), (ProjectileProperties p, int i) => ProjectileStats.SetDamage(p, i)),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Stopping Power", (ProjectileProperties p) => p.stoppingPower, (ProjectileProperties p, float f) => this.ProjectileDef.projectile.stoppingPower = f),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Armor Penetration", (ProjectileProperties p) => ProjectileStats.GetArmorPenetration(p), (ProjectileProperties p, float f) => ProjectileStats.SetArmorPenetration(this.ProjectileDef.projectile, f)),
                new FloatInputWidget<ProjectileProperties>(this.ProjectileDef.projectile, "Speed", (ProjectileProperties p) => p.speed, (ProjectileProperties p, float f) => p.speed = f)
            };

            this.ResetBuffers();
        }

        public string DisplayLabel => this.ProjectileDef.label;

        public void Draw(float x, ref float y, float width)
        {
            foreach (var w in this.inputWidgets)
                w.Draw(x, ref y, width);
        }

        public void ResetBuffers()
        {
            foreach (var w in this.inputWidgets)
                w.ResetBuffers();
        }
    }
}
