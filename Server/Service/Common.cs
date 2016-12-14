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
        public   DataTable Merge(DataTable sourceDataTable, DataTable targetDataTable, string primaryKey)
        {
            if (sourceDataTable != null || targetDataTable != null || !sourceDataTable.Equals(targetDataTable))
            {
                sourceDataTable.PrimaryKey = new DataColumn[] { sourceDataTable.Columns[primaryKey] };
                DataTable dt = targetDataTable.Copy();
                foreach (DataRow tRow in dt.Rows)
                {
                    //拒绝自上次调用 System.Data.DataRow.AcceptChanges() 以来对该行进行的所有更改。
                    //因为行状态为DataRowState.Deleted时无法访问ItemArray的值
                    tRow.RejectChanges();
                    //在加载数据时关闭通知、索引维护和约束。
                    sourceDataTable.BeginLoadData();
                    //查找和更新特定行。如果找不到任何匹配行，则使用给定值创建新行。
                    DataRow temp = sourceDataTable.LoadDataRow(tRow.ItemArray, true);
                    sourceDataTable.EndLoadData();
                    sourceDataTable.Rows.Remove(temp);
                }
            }
            sourceDataTable.AcceptChanges();
            return sourceDataTable;
        }
        public void Update(string sqlText, DataTable dataTable)
        {
            if (dataTable == null) return;
            DataTable dt = new DataTable(dataTable.TableName);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connectionString);
            adapter.Fill(dt);
            var r = Merge(dataTable, dt, "GUID");
            adapter.Update(r);
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
