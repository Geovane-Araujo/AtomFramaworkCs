using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Union : Attribute//3746
    {
        public Union()
        {
        }
    }
}