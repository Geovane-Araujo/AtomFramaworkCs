using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SimpleObject : Attribute
    {
        public SimpleObject()
        {
        }
    }
}