using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary.DB
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public class SqlServerDBHelper : IDisposable
    {
        string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
        SqlConnection Conn;
        SqlTransaction Transaction;
        SqlCommand comm;
        public void BeginTransaction()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
                comm = Conn.CreateCommand();
            }
            comm.Transaction = Transaction = Transaction ?? Conn.BeginTransaction();
        }

        public void Commit()
        {
            Transaction?.Commit();
        }

        public void RollBack()
        {
            Transaction?.Rollback();
        }
        public SqlServerDBHelper(string connStr = "")
        {
            if (!string.IsNullOrWhiteSpace(connStr))
            {
                ConnStr = connStr;
            }
            Conn = GetConnection();
        }

        /// <summary>
        /// 获取一个SqlConnection
        /// </summary>
        /// <param name="connStr">连接字符串,不传则返回自动生成的.传则new一个新的</param>
        /// <returns>新生成的连接未开启</returns>
        public SqlConnection GetConnection(string connStr = "")
        {
            SqlConnection conn;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                conn = Conn ?? new SqlConnection(this.ConnStr);
            }
            else
            {
                conn = new SqlConnection(connStr);
            }
            return conn;
        }

        public DataSet GetDateSet(string commText, IEnumerable<SqlParameter> Pars = null, CommandType ct = CommandType.Text)
        {
            CheckConn();

            DataSet ds = new DataSet();
            comm.CommandText = commText;
            if (Pars != null)
            {
                comm.Parameters.AddRange(Pars.ToArray());

            }
            comm.CommandType = ct;
            using (SqlDataAdapter sad = new SqlDataAdapter(comm))
            {
                sad.Fill(ds);
            }
            comm.Dispose();
            return ds;
        }


        public DataTable GetDataTable(string commText, IEnumerable<SqlParameter> Pars = null, CommandType ct = CommandType.Text)
        {
            return GetDateSet(commText, Pars, ct).Tables[0];
        }

        public T ExecuteScalar<T>(string commText, IEnumerable<SqlParameter> Pars = null, CommandType ct = CommandType.Text)
        {
            CheckConn();

            comm.CommandText = commText;
            if (Pars != null)
            {
                comm.Parameters.AddRange(Pars.ToArray());

            }
            comm.CommandType = ct;
            T t = (T)Convert.ChangeType(comm.ExecuteScalar(), typeof(T));
            comm.Dispose();
            return t;

        }
        public void ExecuteNonQuery(string commText, IEnumerable<SqlParameter> Pars = null, CommandType ct = CommandType.Text)
        {
            CheckConn();
            comm.CommandText = commText;
            if (Pars != null)
            {
                comm.Parameters.AddRange(Pars.ToArray());

            }
            comm.CommandType = ct;
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        /// <summary>
        /// 批量插入数据到数据库
        /// </summary>
        /// <param name="dt">需要插入的数据源</param>
        /// <param name="map">映射(key=上面dt里的列, value=数据表的列)</param>
        /// <param name="opt">插入选项</param>
        /// <param name="tra">事务(默认会使用当前连接的)</param>
        public void BulkCopy(string DBName, DataTable dt, Dictionary<string, string> map = null, SqlBulkCopyOptions opt = SqlBulkCopyOptions.Default, SqlTransaction tra = null)
        {
            if (map == null)
            {
                map = new Dictionary<string, string>();
                foreach (DataColumn item in dt.Columns)
                {
                    map.Add(item.ColumnName, item.ColumnName);
                }
            }


            tra = tra ?? Transaction;
            using (SqlBulkCopy sdc = new SqlBulkCopy(Conn, opt, tra))
            {
                foreach (var item in map)
                {
                    sdc.ColumnMappings.Add(item.Key, item.Value);
                }
                sdc.DestinationTableName = DBName;
                sdc.WriteToServer(dt);
            }

        }

        public void CheckConn()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
                comm = Conn.CreateCommand();
            }
        }

        public void Dispose()
        {
            Conn?.Dispose();
        }
    }
}
