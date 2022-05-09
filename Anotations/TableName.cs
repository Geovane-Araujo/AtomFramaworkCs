using System;

namespace AtomFrameworkCs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute
    {
        private string value;
        public TableName(string value)
        {
            this.value = value;
        }

        public virtual string Value
        {
            get { return value; }
        }
    }
}