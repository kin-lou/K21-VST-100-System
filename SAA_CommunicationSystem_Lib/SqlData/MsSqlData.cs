using SAA_CommunicationSystem_Lib.GuiAttributes;
using SAA_MsSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SqlData
{
    public class MsSqlData
    {
        private MsSql SaaSql = new MsSql(SAA_Database.configattributes.SaaDataBase, SAA_Database.configattributes.SaaDataBaseIP, SAA_Database.configattributes.SaaDataBaseName, SAA_Database.configattributes.SaaDataBasePassword);

        /*===============================新增=======================================*/

        #region [===新增使用者===]
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="guiuser">使用者屬性</param>
        public void SetGuiUser(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_USER Values('{guiuser.USERID}', '{guiuser.PWD}', '{guiuser.GROUPID}','{guiuser.USERNAME}','{guiuser.LAST_LOGIN_TIME}')");
        }
        #endregion

        #region [===新增操作紀錄===]
        /// <summary>
        /// 新增操作紀錄
        /// </summary>
        /// <param name="guiopetation">操作紀錄屬性</param>
        public void SetGuiOpetationHistory(GuiOpetationHistoryAttributes guiopetation)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_OPETATION_HISTORY Values('{guiopetation.PROJECT_ITEM}', '{guiopetation.OPERATE_DATETIME}', '{guiopetation.USERNAME}', '{guiopetation.OPERATE_CONTENT}')");
        }
        #endregion

        #region [===新增使用者登入===]
        /// <summary>
        /// 新增使用者登入
        /// </summary>
        /// <param name="guiuser"></param>
        public void SetGuiLoginstatus(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_LOGINSTATUS Values('{guiuser.USERID}', '{guiuser.USERNAME}','{guiuser.GROUPID}','{guiuser.LAST_LOGIN_TIME}')");
        } 
        #endregion

        /*===============================刪除=======================================*/

        #region [===刪除使用者===]
        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="userid">帳號</param>
        public void DelGuiUser(string userid)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From GUI_USER Where USERID = '" + userid + "'");
        }
        #endregion

        #region [===刪除使用者登入 ===]
        /// <summary>
        ///刪除使用者登入 
        /// </summary>
        public void DelGuiLoginStatus()
        {
            SaaSql.WriteSqlByAutoOpen("Delete From GUI_LOGINSTATUS");
        } 
        #endregion

        /*===============================更新=======================================*/

        #region [===更新使用者===]
        /// <summary>
        /// 更新使用者
        /// </summary>
        /// <param name="guiuser">使用者屬性</param>
        public void UpdGuiUser(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen("Update GUI_USER Set PWD = '" + guiuser.PWD + "', GROUPID = '" + guiuser.GROUPID + "', USERNAME = '" + guiuser.USERNAME + "' Where USERID = '" + guiuser.USERID + "'");
        }
        #endregion

        #region [===更新使用者登入時間===]
        /// <summary>
        /// 更新使用者登入時間
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <param name="lastlogintime">登入時間(yyyy-MM-dd HH:mm:ss.fff)</param>
        public void UpdGuiUserLoginTime(string userid, string lastlogintime)
        {
            SaaSql.WriteSqlByAutoOpen("Update GUI_USER Set LAST_LOGIN_TIME = '" + lastlogintime + "' Where USERID = '" + userid + "'");
        } 
        #endregion

        /*===============================查詢=======================================*/

        #region [===讀取系統權限===]
        /// <summary>
        /// 讀取使用者
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <param name="pwd">密碼</param>
        /// <returns></returns>
        public DataTable GetGuiUserConfirm(string userid, string pwd)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_USER Where USERID='" + userid + "' And PWD = '" + pwd + "'").Tables[0];
        } 
        #endregion

        #region [===讀取帳號===]
        /// <summary>
        /// 讀取帳號
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <returns></returns>
        public bool GetUserName(string userid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_USER Where USERID='" + userid + "'").Tables[0].Rows.Count == 0;
        }
        #endregion

        #region [===讀取使用者登入===]
        /// <summary>
        /// 讀取使用者登入
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoginStatus()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_LOGINSTATUS").Tables[0];
        } 
        #endregion
    }
}
