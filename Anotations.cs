using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Fk : Attribute
    {
        public Fk()
        {
        }
    }
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

    public class Ignore : Attribute
    {
        public Ignore()
        {
        }
    }

    public class ListObjectLocal : Attribute
    {
        public ListObjectLocal()
        {
        }
    }

    public class ObjectLocal : Attribute
    {
        public ObjectLocal()
        {
        }
    }

    public class OneToOne : Attribute
    {
        public OneToOne()
        {
        }
    }
}
