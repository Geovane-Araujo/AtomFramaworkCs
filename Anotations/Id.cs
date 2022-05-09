using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Id : Attribute
    {
        private string name = null;
        public Id()
        {
        }
        
        public Id(string value)
        {
            this.name = value;
        }

        public virtual string Name
        {
            get { return name; }
        }
    }
}