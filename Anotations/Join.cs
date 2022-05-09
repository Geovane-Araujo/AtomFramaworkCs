using System;
using System.Data.Common;

namespace AtomFrameworkCs
{
    public class Join : Attribute
    {
        private Type reference;
        private string columnName = "";
        private string columnReference = "";


        public Join(Type reference, string columnName, string columnReference)
        {
            this.reference = reference;
            this.columnName = columnName;
            this.columnReference = columnReference;
        }
        
        public virtual Type Reference
        {
            get { return Reference; }
        }
        
        public virtual string ColumnName
        {
            get { return ColumnName; }
        }
        
        public virtual string ColumnReference
        {
            get { return ColumnReference; }
        }
    }
}