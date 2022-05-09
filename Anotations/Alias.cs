using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Alias : Attribute
    {
        private string name;
        public Alias(string value)
        {
            this.name = value;
        }

        public virtual string Name
        {
            get { return name; }
        }
    }
}