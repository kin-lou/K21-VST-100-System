using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_MsSql
{
    public class MsSql
    {
        private SqlConnection myConnWrite;
        private SqlConnection myConnQuery;
        /// <summary>
        /// 自動開啟連線,執行SQL命令,關閉連線
        /// </summary>
        private readonly object _Sqlwrite = new object();

        /// <summary>
        /// 自動開啟連線,執行SQL查詢命令,關閉連線
        /// </summary>
        private readonly object _Sqlread = new object();

        public MsSql(string database, string ip, string userid, string password)
        {
            myConnWrite = new SqlConnection(@"Data Source=" + ip + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userid + ";Password=" + password + "");
            myConnQuery = new SqlConnection(@"Data Source=" + ip + ";Initial Catalog=" + database + ";Persist Security Info=True;User ID=" + userid + ";Password=" + password + "");
        }

        /// <summary>
        /// 寫入SQL Server方法
        /// </summary>
        /// <param name="SqlCommand">SQL語法</param>
        public void WriteSqlByAutoOpen(string SqlCommand)
        {
            lock (_Sqlwrite)
            {
                string msg = string.Empty;
                for (int i = 1; i < 3; i++)
                {
                    try { myConnWrite.Open(); }
                    catch (Exception ex) { throw new Exception(ex.Message); }

                    try
                    {
                        SqlCommand mycmd = new SqlCommand(SqlCommand, myConnWrite);
                        mycmd.ExecuteNonQuery();
                        myConnWrite.Close();
                        msg = string.Empty;
                        break;
                    }
                    catch (Exception ex) { msg = ex.Message; }
                    finally { myConnWrite.Close(); }
                };
                if (msg != string.Empty) { throw new Exception(msg); }
            }
        }

        /// <summary>
        /// 讀取SQL Server方法
        /// </summary>
        /// <param name="strSql">SQL語法</param>
        /// <returns></returns>
        public DataSet QuerySqlByAutoOpen(string strSql)
        {
            lock (_Sqlread)
            {
                DataSet myDataSet = null;
                string msg = string.Empty;
                for (int i = 1; i < 3; i++)
                {
                    try { myConnQuery.Open(); }
                    catch (Exception ex) { throw new Exception(ex.Message); }

                    try
                    {
                        SqlDataAdapter myDataAdapter = new SqlDataAdapter(strSql, myConnQuery);
                        myDataAdapter.SelectCommand.CommandTimeout = 1200;
                        myDataSet = new DataSet();
                        myDataAdapter.Fill(myDataSet);
                        myConnQuery.Close();
                        msg = string.Empty;
                        break;
                    }
                    catch (Exception ex) { msg = ex.Message; }
                    finally { myConnQuery.Close(); }
                };
                if (msg != string.Empty) { throw new Exception(msg); }
                else { return myDataSet; }
            }
        }
    }
}
