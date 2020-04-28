using Verse;

namespace InGameDefEditor.Gui.Dialog
{
    public class Dialog_Name : Dialog_Rename
    {
        public delegate void OnAcceptName(string name);
        public delegate AcceptanceReport IsValid(string name);

        private readonly OnAcceptName onAcceptName;
        private readonly IsValid isValid;

        public Dialog_Name(string text, OnAcceptName onAcceptName, IsValid isValid)
        {
            base.curName = text;
            this.onAcceptName = onAcceptName;
            this.isValid = isValid;
        }
        
        protected override AcceptanceReport NameIsValid(string name)
        {
            if (name == null || name.Trim().Length == 0)
            {
                return "Name must be given.";
            }
            if (this.isValid != null)
                return this.isValid(name);
            return true;
        }

        protected override void SetName(string name)
        {
            this.onAcceptName?.Invoke(name.Trim());
        }
    }
}
