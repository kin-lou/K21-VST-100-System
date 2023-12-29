using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.GuiAttributes;
using SAA_MsSql;
using System.Data;
using System.Runtime.InteropServices;

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

        #region [===新增RejectList===]
        /// <summary>
        /// 新增RejectList
        /// </summary>
        /// <param name="rejectList"></param>
        public void SetScRejectList(SaaScRejectList rejectList)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REJECT_LIST Values('{rejectList.SETNO}', '{rejectList.MODEL_NAME}', '{rejectList.LOCAL_REJECT_CODE}', '{rejectList.LOCAL_REJECT_MSG}', '{rejectList.REMOTE_REJECT_CODE}', '{rejectList.REMOTE_REJECT_MSG}')");
        }
        #endregion

        #region [===新增Reject歷史紀錄===]
        /// <summary>
        /// 新增Reject歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        public void SetScRejectHistory(SaaScRrejectHistory rejecthistory)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REJECT_HISTORY Values('{rejecthistory.SETNO}', '{rejecthistory.REJECT_TIME}', '{rejecthistory.MODEL_NAME}', '{rejecthistory.STATION}', '{rejecthistory.CARRIERID}', '{rejecthistory.PARTNO}', '{rejecthistory.LOCAL_REJECT_CODE}', '{rejecthistory.LOCAL_REJECT_MSG}', '{rejecthistory.REMOTE_REJECT_CODE}', '{rejecthistory.REMOTE_REJECT_MSG}')");
        }
        #endregion

        #region [===新增上報指令===]
        /// <summary>
        /// 新增上報指令
        /// </summary>
        /// <param name="reportcommand"></param>
        public void SetScReportCpommand(SaaScReportCommand reportcommand)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REPORT_COMMAND Values('{reportcommand.SETNO}', '{reportcommand.MODEL_NAME}',  '{reportcommand.LCS_COMMAND_NAME}', '{reportcommand.LCS_COMMAND_NOTE}', '{reportcommand.GROUP_NO}', '{reportcommand.REPORT_COMMAND}', '{reportcommand.REPORT_COMMAND_NOTE}')");
        }
        #endregion

        #region [===新增傳送指令===]
        /// <summary>
        /// 新增傳送指令
        /// </summary>
        /// <param name="scdirective"></param>
        public void SetScDirective(SaaScDirective scdirective)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_DIRECTIVE(TASKDATETIME, SETNO, COMMANDON, STATION, CARRIERID, COMMANDID, COMMANDTEXT, SOURCE) Values('{scdirective.TASKDATETIME}', '{scdirective.SETNO}',  '{scdirective.COMMANDON}', '{scdirective.STATION}', '{scdirective.CARRIERID}', '{scdirective.COMMANDID}', '{scdirective.COMMANDTEXT}', '{scdirective.SOURCE}')");
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

        #region [===刪除RejectList===]
        /// <summary>
        /// 刪除RejectList
        /// </summary>
        /// <param name="screjectlis"></param>
        public void DelRejectList(SaaScRejectList screjectlis)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_REJECT_LIST Where SETNO = '" + screjectlis.SETNO + "' And MODEL_NAME = '" + screjectlis.MODEL_NAME + "' And LOCAL_REJECT_CODE = '" + screjectlis.LOCAL_REJECT_CODE + "'");
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

        #region [===更新上報Inedex===]
        /// <summary>
        /// 更新上報Inedex
        /// </summary>
        /// <param name="reportinadx"></param>
        public void UpdScReportIndex(SaaScReportInadx reportinadx)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_REPORT_INDEX Set REPORT_INDEX = '" + reportinadx.REPORT_INDEX + "' Where SETNO = '" + reportinadx.SETNO + "' And MODEL_NAME = '" + reportinadx.MODEL_NAME + "' And REPORT_NAME = '" + reportinadx.REPORT_NAME + "'");
        }
        #endregion

        #region [===更新設備狀態===]
        /// <summary>
        /// 更新設備狀態
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentStatus(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set EQUIPMENT_STATUS = '" + equipmentstatus.EQUIPMENT_STATUS + "', EQUIPMENT_STATUS_CODE = '" + equipmentstatus.EQUIPMENT_STATUS_CODE + "', STATUS_UPDATE_TIME = '" + equipmentstatus.STATUS_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        } 
        #endregion

        #region [===更新設備模式===]
        /// <summary>
        /// 更新設備模式
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentModel(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set EQUIPMENT_MODEL = '" + equipmentstatus.EQUIPMENT_MODEL_CODE + "', EQUIPMENT_MODEL_CODE = '" + equipmentstatus.EQUIPMENT_STATUS_CODE + "', MODEL_UPDATE_TIME = '" + equipmentstatus.MODEL_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        }
        #endregion

        #region [===更新LCS狀態===]
        /// <summary>
        /// 更新LCS狀態
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentLcsStatus(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set LCS_STATUS = '" + equipmentstatus.LCS_STATUS + "', LCS_STATUS_CODE = '" + equipmentstatus.LCS_STATUS_CODE + "', LCS_UPDATE_TIME = '" + equipmentstatus.LCS_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        }
        #endregion

        #region [===更新平台狀態===]
        /// <summary>
        /// 更新平台狀態
        /// </summary>
        /// <param name="saalocationsetting"></param>
        public void UpdScLocationSettingStatus(SaaScLocationSetting saalocationsetting)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LOCATIONSETTING Set LOCATIONSTATUS = '" + saalocationsetting.LOCATIONSTATUS + "' Where SETNO = '" + saalocationsetting.SETNO + "' And MODEL_NAME = '" + saalocationsetting.MODEL_NAME + "' And HOSTID = '" + saalocationsetting.HOSTID + "'");
        } 
        #endregion

        #region [===更新平台模式===]
        /// <summary>
        /// 更新平台模式
        /// </summary>
        /// <param name="saalocationsetting"></param>
        public void UpdScLocationSettingMode(SaaScLocationSetting saalocationsetting)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LOCATIONSETTING Set LOCATIONMODE = '" + saalocationsetting.LOCATIONMODE + "' Where SETNO = '" + saalocationsetting.SETNO + "' And MODEL_NAME = '" + saalocationsetting.MODEL_NAME + "' And HOSTID = '" + saalocationsetting.HOSTID + "'");
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

        #region [===讀取轉譯上報參數設定===]
        /// <summary>
        /// 讀取轉譯上報參數設定
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportConveys()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_CONVEYS").Tables[0];
        }
        #endregion

        #region [===讀取Reject 明細===]
        /// <summary>
        /// 讀取Reject 明細
        /// </summary>
        /// <returns></returns>
        public DataTable GetScRejectList()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST").Tables[0];
        }
        #endregion

        #region [===讀取Reject List Code碼查詢===]
        /// <summary>
        /// 讀取REJECT LIST Code碼查詢
        /// </summary>
        /// <param name="localrejectcode"></param>
        /// <returns></returns>
        public DataTable GetScRejectList(string localrejectcode)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST Where LOCAL_REJECT_CODE = '" + localrejectcode + "'").Tables[0];
        }
        #endregion

        public DataTable GetScRejectMessage(string localrejectmesage)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST Where LOCAL_REJECT_MSG = '" + localrejectmesage + "'").Tables[0];
        }

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectCarrierIdHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And MODEL_NAME = '" + rejecthistory.MODEL_NAME + "' And CARRIERID LIKE '" + rejecthistory.CARRIERID + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And MODEL_NAME = '" + rejecthistory.MODEL_NAME + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectIDHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And CARRIERID LIKE '" + rejecthistory.CARRIERID + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectHistory(string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "'").Tables[0];
        }
        #endregion

        #region [===讀取前20筆REJECT歷史資料===]
        /// <summary>
        /// 讀取前20筆REJECT歷史資料
        /// </summary>
        /// <returns></returns>
        public DataTable GetScRejectHistory()
        {
            return SaaSql.QuerySqlByAutoOpen("Select TOP(20) * From SC_REJECT_HISTORY Order By REJECT_TIME DESC").Tables[0];
        }
        #endregion

        #region [===讀取設備機台名稱===]
        public DataTable GetScEquipmentZone()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_ZONE").Tables[0];
        }
        #endregion

        #region [===讀取上報命令===]
        /// <summary>
        /// 讀取上報命令
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportCommand()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND").Tables[0];
        }
        #endregion

        #region [===讀取上報指令===]
        /// <summary>
        /// 讀取上報指令
        /// </summary>
        /// <param name="reportcommand"></param>
        /// <returns></returns>
        public DataTable GetScReportCommand(SaaScReportCommand reportcommand)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND Where SETNO = '" + reportcommand.SETNO + "' And MODEL_NAME = '" + reportcommand.MODEL_NAME + "' And LCS_COMMAND_NAME = '" + reportcommand.LCS_COMMAND_NAME + "' And REPORT_COMMAND='" + reportcommand.REPORT_COMMAND + "' ").Tables[0];
        } 
        #endregion

        #region [===讀取上報指令名稱===]
        /// <summary>
        /// 讀取上報指令名稱
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportCommandLcsName()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select DISTINCT LCS_COMMAND_NAME From SC_REPORT_COMMAND").Tables[0];
        } 
        #endregion

        #region [===讀取上報指令===]
        /// <summary>
        /// 讀取上報指令
        /// </summary>
        /// <param name="setno"></param>
        /// <param name="modelname"></param>
        /// <param name="lcscommandname"></param>
        /// <returns></returns>
        public DataTable GetScReportCommand(string setno, string modelname, string lcscommandname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LCS_COMMAND_NAME = '" + lcscommandname + "'").Tables[0];
        } 
        #endregion

        #region [===讀取客戶上報代碼===]
        /// <summary>
        /// 讀取客戶上報代碼
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="modelname">機台型號</param>
        /// <param name="lcscommandname">LCS上報名稱</param>
        /// <returns></returns>
        public DataTable GetReportCommandName(string setno, string modelname, string lcscommandname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND_NAME Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LCS_COMMAND_NAME = '" + lcscommandname + "'").Tables[0];
        }
        #endregion

        #region [===查詢機台站點名稱===]
        /// <summary>
        /// 查詢機台站點名稱
        /// </summary>
        /// <param name="setno">機台編號</param>
        /// <param name="modelname">機台型號</param>
        /// <returns></returns>
        public DataTable GetScEquipmentZone(string setno, string modelname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_ZONE Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "'").Tables[0];
        }
        #endregion

        #region [===查詢上報資料表===]
        /// <summary>
        /// 查詢上報資料表
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="commandid">指令編碼</param>
        /// <param name="commandtext">指令內容</param>
        /// <param name="source">來源</param>
        /// <returns></returns>
        public DataTable GetScDirective(string setno, string commandid, string commandtext, string source)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DIRECTIVE Where SETNO = '" + setno + "' And COMMANDID = '" + commandid + "' And COMMANDTEXT = '" + commandtext + "' And SOURCE = '" + source + "'").Tables[0];
        }
        #endregion

        #region [===查詢上報編號===]
        /// <summary>
        /// 查詢上報編號
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="modelname">機台型號</param>
        /// <param name="reportname">資料表名稱</param>
        /// <returns></returns>
        public DataTable GetScReportIndex(int setno, string modelname, string reportname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_INDEX Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And REPORT_NAME = '" + reportname + "'").Tables[0];
        } 
        #endregion

        public DataTable GetScCommon(int setno,string modelname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_COMMON Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "'").Tables[0];
        }

        public DataTable GetScDevice(int setno, string modelname, string deviceid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And DEVICEID = '" + deviceid + "'").Tables[0];
        }

        public DataTable GetScLocationsetting(int setno, string modelname, string locationid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LOCATIONID = '" + locationid + "'").Tables[0];
        }

        public DataTable GetScCommon(int setno, string modelname, string itemname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_COMMON Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And ITEM_NAME = '" + itemname + "'").Tables[0];
        }
    }
}
