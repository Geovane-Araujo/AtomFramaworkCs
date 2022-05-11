using System;
using System.Data;
using AtomFrameworkCs.Connection;

namespace AtomFrameworkCs.Model
{
    public class GlobalVariables
    {
        protected IDbConnection _con = null;

        protected Boolean _collect = false;

        protected IDbCommand _command = null;

        protected IDataReader _dataReader = null;

        protected CsConnection _csConnection = new();
    }
}