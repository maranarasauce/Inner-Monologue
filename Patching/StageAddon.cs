using System;
using System.Reflection;

namespace ProjectProphet.Patching
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class StageAddon : Attribute
    {
        public int stage;
        public string specialStageName;
        public StageAddon(int stage)
        {
            this.stage = stage;
        }

        public StageAddon(string stage)
        {
            this.specialStageName = stage;
        }

        private MethodInfo method;
        public void SetMethodInfo(MethodInfo info)
        {
            method = info;
        }

        public void RunAddon()
        {
            method.Invoke(null, null);
        }
    }
}
