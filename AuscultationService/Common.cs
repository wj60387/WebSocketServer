using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

namespace Helper
{
    public class ConfigHelper
    {
        public static string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            }
        }
    }
    public class SqlCommon
    {
        private string _connectionString;
        public SqlCommon(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public   string connectionString
        {
            get
            {
                return _connectionString;
            }
        }
        public void Insert(string tableName, DataTable dataTable, Dictionary<string, string> ColumnMapping)
        {
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = tableName;//数据库中的表名
            foreach (var item in ColumnMapping)
            {
                sqlbulkcopy.ColumnMappings.Add(item.Key, item.Value);
            }
            sqlbulkcopy.WriteToServer(dataTable);
        }
        /// <summary>
        /// 无列映射，默认为列名相同
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataTable"></param>
        public void Insert(string tableName, DataTable dataTable)
        {
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = tableName;//数据库中的表名
            foreach (DataColumn item in dataTable.Columns)
            {
                sqlbulkcopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);
            }
            sqlbulkcopy.WriteToServer(dataTable);
          
        }
        public void Update(string sqlText, DataTable dataTable)
        {
            if (dataTable == null) return;
            SqlDataAdapter adapter = new SqlDataAdapter (sqlText, connectionString);
            
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dt.Merge(dataTable);
            DataTable updates = dt.GetChanges(DataRowState.Modified);
            if(updates!=null)
            adapter.Update(updates);
            DataTable deletes = dt.GetChanges(DataRowState.Deleted);
            if(deletes!=null)
            adapter.Update(deletes);
            DataTable inserts = dt.GetChanges(DataRowState.Added);
            if (inserts != null)
            adapter.Update(inserts);
        }
        #region 非事务
        public int ExecuteNonQuery(string sqlText, params string[] dictParams)
        {
            if (dictParams == null || dictParams.Length == 0)
            {
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sqlText);
            }
            else
            {
                List<string> listStrParams = new List<string>();
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < dictParams.Length; i++)
                {
                    object value = dictParams[i];
                    if (string.IsNullOrEmpty(dictParams[i]))
                        value = DBNull.Value;
                    string strPar = "@SqlParameter" + i;
                    SqlParameter param = new SqlParameter(strPar, value);
                    listParam.Add(param);
                    listStrParams.Add(strPar);
                }
                string sql = string.Format(sqlText, listStrParams.ToArray());
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, listParam.ToArray());
            }

        }
        public DataSet ExecuteDataset(string sqlText, params string[] dictParams)
        {

            if (dictParams == null || dictParams.Length == 0)
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlText );
            }
            else
            {
                List<string> listStrParams = new List<string>();
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < dictParams.Length; i++)
                {
                    object value = dictParams[i];
                    if (string.IsNullOrEmpty(dictParams[i]))
                        value = DBNull.Value;
                    string strPar = "@SqlParameter" + i;
                    SqlParameter param = new SqlParameter(strPar, value);
                    listParam.Add(param);
                    listStrParams.Add(strPar);
                }
                string sql = string.Format(sqlText, listStrParams.ToArray());
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, listParam.ToArray());
            }
           
        }
        public string ExecuteScalar(string sqlText, params string[] dictParams)
        {
            if (dictParams == null || dictParams.Length == 0)
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlText).ToString();
            }
            else
            {
                List<string> listStrParams = new List<string>();
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < dictParams.Length; i++)
                {
                    object value = dictParams[i];
                    if (string.IsNullOrEmpty(dictParams[i]))
                        value = DBNull.Value;
                    string strPar = "@SqlParameter" + i;
                    SqlParameter param = new SqlParameter(strPar, value);
                    listParam.Add(param);
                    listStrParams.Add(strPar);
                }
                string sql = string.Format(sqlText, listStrParams.ToArray());
                object r= SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql, listParam.ToArray());
                if (r == null)
                    return string.Empty;
                return r.ToString();
            }

        }
        #endregion
        #region 事务
        public int _ExecuteNonQuery(string sqlText, params string[] dictParams)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction()) 
                {
                    try
                    {
                        if (dictParams == null || dictParams.Length == 0)
                        {
                            int rows = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlText);
                            trans.Commit();
                            conn.Close();
                            return rows;
                        }
                        else
                        {
                            List<string> listStrParams = new List<string>();
                            List<SqlParameter> listParam = new List<SqlParameter>();
                            for (int i = 0; i < dictParams.Length; i++)
                            {
                                string strPar = "@SqlParameter" + i;
                                SqlParameter param = new SqlParameter(strPar, dictParams[i]);
                                listParam.Add(param);
                                listStrParams.Add(strPar);
                            }
                            string sql = string.Format(sqlText, listStrParams.ToArray());
                            int rows = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, listParam.ToArray());
                            trans.Commit();
                            conn.Close();
                            return rows;
                        }
                      
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
                conn.Close();
            }
            return -1;

        }
        public DataSet _ExecuteDataset(string sqlText, params string[] dictParams)
        {
            if (dictParams == null || dictParams.Length == 0)
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sqlText);
            }
            else
            {
                List<string> listStrParams = new List<string>();
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < dictParams.Length; i++)
                {
                    string strPar = "@SqlParameter" + i;
                    SqlParameter param = new SqlParameter(strPar, dictParams[i]);
                    listParam.Add(param);
                    listStrParams.Add(strPar);
                }
                string sql = string.Format(sqlText, listStrParams.ToArray());
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, listParam.ToArray());
            }

        }
        public string _ExecuteScalar(string sqlText, params string[] dictParams)
        {
            if (dictParams == null || dictParams.Length == 0)
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlText).ToString();
            }
            else
            {
                List<string> listStrParams = new List<string>();
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < dictParams.Length; i++)
                {
                    string strPar = "@SqlParameter" + i;
                    SqlParameter param = new SqlParameter(strPar, dictParams[i]);
                    listParam.Add(param);
                    listStrParams.Add(strPar);
                }
                string sql = string.Format(sqlText, listStrParams.ToArray());
                return SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql, listParam.ToArray()).ToString();
            }

        }
        #endregion

    }
}
