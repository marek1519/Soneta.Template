using Soneta.Business;
using Soneta.Business.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Soneta.Template
{
    /// <summary>
    /// Obsluga bazy danych poprzez SQL
    /// </summary>
    public static class DataBase
    {

        /// <summary>
        /// Pobranie słownika z bazy danych wg podanego zapytania
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetStringListBySql(ISessionable session, string sql)
        {
            var _list = new System.Collections.Generic.List<string>();

            var dbs = DevUczelnie.DataBase.SqlExecuteReader(session, sql);
            foreach (DataRow item in dbs.Rows)
            {
                _list.Add(item[0].ToString());
            }
            return _list.ToArray();
        }


        /// <summary>
        /// Wykonanie komendy MSSQL bez zwrócenia tabeli
        /// </summary>
        /// <param name="sqlQuery">Query</param>
        /// <returns></returns>
        public static int SqlExecuteNonQuery(ISessionable session, string sqlQuery)
        {

            SqlCommand SelectCommand;
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConnectionString(session);
                SelectCommand = con.CreateCommand();
                SelectCommand.CommandText = sqlQuery;
                con.Open();
                var res = SelectCommand.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception exc)
            {
                throw new Exception("QUERY: " + sqlQuery + "\n" + exc.Message);
            }
        }

        /// <summary>
        /// Wykonanie komendy MSSQL
        /// </summary>
        /// <param name="sqlQuery">TSQL Query</param>
        /// <param name="session">Sesja</param>
        /// <returns></returns>
        public static DataTable SqlExecuteReader(ISessionable session, string sqlQuery)
        {
            SqlCommand SelectCommand;
            SqlDataReader reader;// = new SqlDataReader();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConnectionString(session);
                SelectCommand = con.CreateCommand();
                SelectCommand.CommandText = sqlQuery;
                con.Open();
                reader = SelectCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new System.Exception("QUERY: " + sqlQuery + "\n" + ex.Message);
            }
        }

        public static int SqlGetInt(ISessionable session, string query, int _dflt = -1)
        {
            try
            {
                var dtb = SqlExecuteReader(session, query);
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    return (int)dtb.Rows[0][0];
                }
            }
            catch { }

            return _dflt;
        }



        /// <summary>
        /// Pobranie pierwszej wartości z pierwszej komórki w tabeli
        /// <para>Funkcja zwraca null przy pustej tabeli</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="sql"></param>
        /// <param name="deflt"></param>
        /// <returns></returns>
        public static T GetFirstValue<T>(ISessionable session, string sql, T deflt)
        {
            var adr = DataBase.SqlExecuteReader(session, sql);
            foreach (System.Data.DataRow row in adr.Rows)
            {
                object value = row[0];

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFrom(value);
                }
            }

            return deflt;
        }

        private static string ConnectionString(ISessionable session)
        {
            MsSqlDatabase msdb = (MsSqlDatabase)session.Session.Login.Database;
            string server = msdb.Server;
            string password = msdb.GetPassword();
            string baza = msdb.DatabaseName;
            string user = msdb.User;

            string con = "Server = " + server + " ;";
            con += "Database = " + baza + " ;";
            con += "User ID = " + user + " ;";
            con += "Password = " + password + " ;";
            con += "trusted_connection = false;";

            return con;
        }

        /// <summary>
        /// Sprawdzenie czy dane polecenie zwróci dane
        /// </summary>
        /// <param name="sqlQuery">TSQL Query</param>
        /// <returns></returns>
        public static bool SqlHasRows(this ISessionable session, string sqlQuery)
        {
            SqlCommand SelectCommand;
            SqlDataReader reader;// = new SqlDataReader();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConnectionString(session);
                SelectCommand = con.CreateCommand();
                SelectCommand.CommandText = sqlQuery;
                con.Open();
                reader = SelectCommand.ExecuteReader();
                int iloscKol = reader.FieldCount;
                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                con.Close();

                if (dt.Rows.Count > 0) return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new System.Exception("QUERY: " + sqlQuery + "\n" + ex.Message);
            }
        }
    }
}
