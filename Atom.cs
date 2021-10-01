using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.Json;

namespace AtomFrameworkCs
{
    public class Atom
    {
        
        // Start Postgres
        public static Object InsertAll(Object obj,NpgsqlConnection con)
        {
            NpgsqlCommand command = null;
            command = ConstructorCommand(obj, con,1);
            return new Object();
        }
        

        public static int InsertedOne(Object obj, NpgsqlConnection con)
        {
            NpgsqlCommand command = null;
            command = ConstructorCommand(obj, con, 1);
            int id = (int)command.ExecuteScalar();
            return id;
        }

        

        public static Boolean EditingOne(Object obj, NpgsqlConnection con)
        {
            Boolean ret = false;
            NpgsqlCommand command = null;
            command = ConstructorCommand(obj, con, 2);
            command.ExecuteNonQuery();
            ret = true;
            return ret;
        }

        public static Boolean Deleted(Object obj, NpgsqlConnection con)
        {
            Boolean ret = false;
            NpgsqlCommand command = null;
            command = ConstructorCommand(obj, con, 3);
            command.ExecuteNonQuery();
            ret = true;
            return ret;
        }

        public static Object GetOne(NpgsqlConnection con, String sql)
        {
            Object obj =  ConstructorCommand(con, sql,1);
            return obj;
        }

        public static T GetOne<T>(NpgsqlConnection con, String sql)
        {
            Object ret = new();

            ret = ConstructorCommand(con, sql, 1);
            var json = JsonConvert.SerializeObject(ret);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static Object GetAll(NpgsqlConnection con, String sql)
        {
            Object obj = ConstructorCommand(con, sql,2);
            return obj;
        }

        public static Collection<T> GetAll<T>(NpgsqlConnection con, String sql)
        {

            Object ret = new();
            ret = ConstructorCommand(con, sql, 2);
            var json = JsonConvert.SerializeObject(ret);

            Collection<T> obj = JsonConvert.DeserializeObject<Collection<T>>(json);

            return obj;
        }

        private static NpgsqlCommand ConstructorCommand(Object obj, NpgsqlConnection con, int type)
        {
            NpgsqlCommand command = new();
            Type clazz = obj.GetType();
            Alias tbl = (Alias)Attribute.GetCustomAttribute(clazz, typeof(Alias));
            string table = tbl.Name;

            string fields = "";
            string values = "";
            string idcolumn = "";
            string sql = "";
            var idvalue = new Object();

            if (type == 1)
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
                        if (vl != null)
                            command.Parameters.AddWithValue("@" + propriedades.Name, vl);
                        else
                            command.Parameters.AddWithValue("@" + propriedades.Name, DBNull.Value);
                        var i = 0;
                    }
                    if (id != null)
                    {
                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;
                    }

                }
                fields = fields.Substring(0, fields.Length - 1);
                values = values.Substring(0, values.Length - 1);

                sql = "INSERT INTO " + table + " (" + fields + ") VALUES (" + values + " ) returning " + idcolumn + ";";
            }
            else if (type == 2)
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
                        values += propriedades.Name + "=@" + propriedades.Name + ",";
                        var vl = propriedades.GetValue(obj);
                        if (vl != null)
                            command.Parameters.AddWithValue("@" + propriedades.Name, vl);
                        else
                            command.Parameters.AddWithValue("@" + propriedades.Name, DBNull.Value);
                        var i = 0;
                    }

                    if (id != null)
                    {

                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;

                        idvalue = propriedades.GetValue(obj);
                    }

                }
                values = values.Substring(0, values.Length - 1);

                sql = "UPDATE " + table + " set " + values + " WHERE " + idcolumn + " = " + idvalue;
            }
            else if (type == 3)
            {
                foreach (var propriedades in obj.GetType().GetRuntimeProperties())
                {
                    Type a = propriedades.GetType();
                    Id id = (Id)Attribute.GetCustomAttribute(propriedades, typeof(Id));
                    if (id != null)
                    {

                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;

                        idvalue = propriedades.GetValue(obj);
                        break;
                    }

                }


                sql = "DELETE FROM " + table + " where " + idcolumn + " = " + idvalue;
            }

            command.Connection = con;
            command.CommandText = sql;

            return command;
        }

        private static Object ConstructorCommand(NpgsqlConnection con, string sql,int type)
        {
            NpgsqlCommand command = new NpgsqlCommand(sql,con);
            NpgsqlDataReader dr = command.ExecuteReader();

            Hashtable table = new();
            List<Hashtable> tbl = new List<Hashtable>();
           
            if(type == 1)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        table.Add(dr.GetName(i), (Object)dr.GetValue(dr.GetOrdinal(dr.GetName(i))));
                    }
                }
                command.Dispose();
                dr.Close();
                return table;
            }
            else if(type == 2)
            {
                while (dr.Read())
                {
                    table = new();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        table.Add(dr.GetName(i), (Object)dr.GetValue(dr.GetOrdinal(dr.GetName(i))));
                    }
                    tbl.Add(table);

                }
                command.Dispose();
                dr.Close();
                return tbl;
            }
            return false;
        }


        // End Postgres
        //Start SqlServer

        public static Object InsertAll(Object obj, SqlConnection con)
        {
            SqlCommand command = null;
            command = ConstructorCommand(obj, con, 1);
            return new Object();
        }


        public static int InsertedOne(Object obj, SqlConnection con)
        {
            SqlCommand command = null;
            command = ConstructorCommand(obj, con, 1);
            int id = Convert.ToInt32(command.ExecuteScalar());
            return id;
        }

        public static Boolean EditingOne(Object obj, SqlConnection con)
        {
            Boolean ret = false;
            SqlCommand command = null;
            command = ConstructorCommand(obj, con, 2);
            command.ExecuteNonQuery();
            ret = true;
            return ret;
        }

        public static Boolean Deleted(Object obj, SqlConnection con)
        {
            Boolean ret = false;
            SqlCommand command = null;
            command = ConstructorCommand(obj, con, 3);
            command.ExecuteNonQuery();
            ret = true;
            return ret;
        }

        public static Object GetOne(SqlConnection con, String sql)
        {
            Object obj = ConstructorCommand(con, sql, 1);
            return obj;
        }

        public static T GetOne<T>(SqlConnection con, String sql)
        {
            Object ret = new();

            ret = ConstructorCommand(con, sql, 1);
            var json = JsonConvert.SerializeObject(ret);
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static Object GetAll(SqlConnection con, String sql)
        {
            Object obj = ConstructorCommand(con, sql, 2);
            return obj;
        }

        public static Collection<T> GetAll<T>(SqlConnection con, String sql)
        {

            Object ret = new();
            ret = ConstructorCommand(con, sql, 2);
            var json = JsonConvert.SerializeObject(ret);

            Collection<T> obj = JsonConvert.DeserializeObject<Collection<T>>(json);

            return obj;
        }

        private static SqlCommand ConstructorCommand(Object obj, SqlConnection con, int type)
        {
            SqlCommand command = new();
            Type clazz = obj.GetType();
            Alias tbl = (Alias)Attribute.GetCustomAttribute(clazz, typeof(Alias));
            string table = tbl.Name;

            string fields = "";
            string values = "";
            string idcolumn = "";
            string sql = "";
            var idvalue = new Object();

            if (type == 1)
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
                        if (vl != null)
                            command.Parameters.AddWithValue("@" + propriedades.Name, vl);
                        else
                            command.Parameters.AddWithValue("@" + propriedades.Name, DBNull.Value);
                        var i = 0;
                    }
                    if (id != null)
                    {
                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;
                    }

                }
                fields = fields.Substring(0, fields.Length - 1);
                values = values.Substring(0, values.Length - 1);

                sql = "INSERT INTO " + table + " (" + fields + ") VALUES (" + values + " ) ;SELECT SCOPE_IDENTITY();";
            }
            else if (type == 2)
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
                        values += propriedades.Name + "=@" + propriedades.Name + ",";
                        var vl = propriedades.GetValue(obj);
                        if (vl != null)
                            command.Parameters.AddWithValue("@" + propriedades.Name, vl);
                        else
                            command.Parameters.AddWithValue("@" + propriedades.Name, DBNull.Value);
                        var i = 0;
                    }

                    if (id != null)
                    {

                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;

                        idvalue = propriedades.GetValue(obj);
                    }

                }
                values = values.Substring(0, values.Length - 1);

                sql = "UPDATE " + table + " set " + values + " WHERE " + idcolumn + " = " + idvalue;
            }
            else if (type == 3)
            {
                foreach (var propriedades in obj.GetType().GetRuntimeProperties())
                {
                    Type a = propriedades.GetType();
                    Id id = (Id)Attribute.GetCustomAttribute(propriedades, typeof(Id));
                    if (id != null)
                    {

                        if (id.Name != null)
                            idcolumn = id.Name;
                        else
                            idcolumn = propriedades.Name;

                        idvalue = propriedades.GetValue(obj);
                        break;
                    }

                }


                sql = "DELETE FROM " + table + " where " + idcolumn + " = " + idvalue;
            }

            command.Connection = con;
            command.CommandText = sql;

            return command;
        }

        private static Object ConstructorCommand(SqlConnection con, string sql, int type)
        {
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader dr = command.ExecuteReader();

            Hashtable table = new();
            List<Hashtable> tbl = new List<Hashtable>();

            if (type == 1)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        table.Add(dr.GetName(i), (Object)dr.GetValue(dr.GetOrdinal(dr.GetName(i))));
                    }
                }
                command.Dispose();
                dr.Close();
                return table;
            }
            else if (type == 2)
            {
                while (dr.Read())
                {
                    table = new();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        table.Add(dr.GetName(i), (Object)dr.GetValue(dr.GetOrdinal(dr.GetName(i))));
                    }
                    tbl.Add(table);

                }
                command.Dispose();
                dr.Close();
                return tbl;
            }
            return false;
        }

        //End SqlServer

    }
}
