using InGameDefEditor.Stats;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ProjectileDefWidget : IDefEditorWidget
    {
        public readonly ThingDef ProjectileDef;

        private string[] buffer = new string[4];

        public ProjectileDefWidget(ThingDef d)
        {
            this.ProjectileDef = d;
            this.ResetBuffers();
        }

        public string DisplayLabel => this.ProjectileDef.label;

        public void Draw(float x, ref float y, float width)
        {
            int i = ProjectileStats.GetDamage(this.ProjectileDef.projectile);
            this.buffer[0] = WindowUtil.DrawInput(x, ref y, "Base Damage", ref i, this.buffer[0]);
            ProjectileStats.SetDamage(this.ProjectileDef.projectile, i);

            this.buffer[1] = WindowUtil.DrawInput(x, ref y, "Stopping Power", ref this.ProjectileDef.projectile.stoppingPower, this.buffer[1]);

            float f = ProjectileStats.GetArmorPenetration(this.ProjectileDef.projectile);
            this.buffer[2] = WindowUtil.DrawInput(x, ref y, "Armor Penetration", ref f, this.buffer[2]);
            ProjectileStats.SetArmorPenetration(this.ProjectileDef.projectile, f);

            this.buffer[3] = WindowUtil.DrawInput(x, ref y, "Speed", ref this.ProjectileDef.projectile.speed, this.buffer[3]);
        }

        public void ResetBuffers()
        {
            this.buffer[0] = ProjectileStats.GetDamage(this.ProjectileDef.projectile).ToString();
            this.buffer[1] = this.ProjectileDef.projectile.stoppingPower.ToString();
            this.buffer[2] = ProjectileStats.GetArmorPenetration(this.ProjectileDef.projectile).ToString();
            this.buffer[3] = this.ProjectileDef.projectile.speed.ToString();
        }
    }
}
