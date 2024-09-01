namespace LemurRhino
{
    public class LemurRhinoPlugIn : Rhino.PlugIns.PlugIn
    {
        public LemurRhinoPlugIn()
        {
            Instance = this;
        }

        public static LemurRhinoPlugIn Instance
        {
            get;
            private set;
        }
    }
}

