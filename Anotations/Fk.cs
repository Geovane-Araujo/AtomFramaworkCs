using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Fk : Attribute
    {
        private string value = null;
        public Fk()
        {
        }
        
        public Fk(string value)
        {
            this.value = value;
        }

        public virtual string Value
        {
            get { return value; }
        }
    }
}