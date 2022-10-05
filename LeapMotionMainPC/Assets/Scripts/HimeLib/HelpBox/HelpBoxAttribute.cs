using UnityEngine;

namespace HimeLib {
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly int type;

        public HelpBoxAttribute(int type = 1)
        {
            this.type = type;
        }
    }
}