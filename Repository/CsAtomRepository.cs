using System;
using System.Data;

namespace AtomFrameworkCs.Repository
{
    public interface CsAtomRepository
    {

        public void ExecuteQuery(String sql, String database);
        
        public void ExecuteQuery(String sql);
        
        public void Save(Object obj);

        public void Save(Object obj, String database);
        
        public T Get<T>(IDbConnection con, String sql);
        
        public T GeById<T>(IDbConnection con, String sql);
        
        
    }
}