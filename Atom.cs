using Npgsql;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Reflection;

namespace AtomFrameworkCs
{
    public class Atom
    {
        
        public static Object InsertAll(Object obj,NpgsqlConnection con)
        {
            NpgsqlCommand command = null;
            command = ConstructorQuery(obj, con,1);
            return new Object();
        }
        

        public static int InsertedOne(Object obj, NpgsqlConnection con)
        {
            NpgsqlCommand command = null;
            command = ConstructorQuery(obj, con, 1);
            int id = (int)command.ExecuteScalar();
            return id;
        }

        public static int EditingOne(SqlConnection con, int idObject, String tableName)
        {
            return new Int32();
        }

        public static int Deleted(Object obj, SqlConnection con, int idUpdate)
        {
            return new Int32();
        }

        public static Object GetOne(SqlConnection con, String sql)
        {
            return new Int32();
        }

        public static Object GetAll(SqlConnection con, String sql)
        {
            return new Int32();
        }

        public static void ExecuteQuery(SqlConnection con, String sql)
        {

        }

        private static NpgsqlCommand ConstructorQuery(Object obj,NpgsqlConnection con, int type)
        {
            NpgsqlCommand command = new();
            Type clazz = obj.GetType();
            Alias tbl = (Alias)Attribute.GetCustomAttribute(clazz, typeof(Alias));
            string table = tbl.Name;

            string fields = "";
            string values = "";
            string idcolumn = "";
            string sql = "";

            if(type == 1)
            {
                foreach (var propriedades in obj.GetType().GetRuntimeProperties())
                {
                    Type a = propriedades.GetType();
                    Id id = (Id)Attribute.GetCustomAttribute(propriedades, typeof(Id));
                    ListObjectLocal list = (ListObjectLocal)Attribute.GetCustomAttribute(propriedades, typeof(ListObjectLocal));
                    ObjectLocal objLocal = (ObjectLocal)Attribute.GetCustomAttribute(propriedades, typeof(ObjectLocal));
                    Ignore igi = (Ignore)Attribute.GetCustomAttribute(propriedades, typeof(Ignore));

                    if (id == null && list == null && objLocal == null && igi == null)
                    {
                        fields += propriedades.Name + ",";
                        values += "@" + propriedades.Name + ",";
                        var vl = propriedades.GetValue(obj);
                        if(vl != null)
                            command.Parameters.AddWithValue("@" + propriedades.Name, vl);
                        else
                            command.Parameters.AddWithValue("@" + propriedades.Name, DBNull.Value);
                        var i = 0;
                    }
                    if(id != null)
                    {
                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;
                    }
                    
                }
                fields = fields.Substring(0, fields.Length - 1);
                values = values.Substring(0, values.Length - 1);

                sql = "INSERT INTO " + table + " (" + fields + ") VALUES (" + values + " ) returning "+ idcolumn + ";";
            }

            command.Connection = con;
            command.CommandText = sql;

            return command;
        }
    }
}
