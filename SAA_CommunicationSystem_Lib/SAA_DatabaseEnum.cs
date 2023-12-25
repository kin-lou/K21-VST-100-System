using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_DatabaseEnum
    {
        #region [===權限帳號密碼===]
        /// <summary>
        /// 權限帳號密碼列舉
        /// </summary>
        public enum GUI_USER
        {
            /// <summary>
            /// 帳號
            /// </summary>
            USERID,

            /// <summary>
            /// 密碼
            /// </summary>
            PWD,

            /// <summary>
            /// 權限群組
            /// </summary>
            GROUPID,

            /// <summary>
            /// 使用者名稱
            /// </summary>
            USERNAME,

            /// <summary>
            /// 登入系統時間(yyyy-MM-dd HH:mm:ss.fff)
            /// </summary>
            LAST_LOGIN_TIME,
        }
        #endregion

        #region [===REJECT 明細===]
        /// <summary>
        /// REJECT 明細
        /// </summary>
        public enum SC_REJECT_LIST
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 本機REJECT編碼
            /// </summary>
            LOCAL_REJECT_CODE,

            /// <summary>
            /// 本機REJECT訊息
            /// </summary>
            LOCAL_REJECT_MSG,

            /// <summary>
            /// 遠端REJECT編碼
            /// </summary>
            REMOTE_REJECT_CODE,

            /// <summary>
            /// 遠端REJECT訊息
            /// </summary>
            REMOTE_REJECT_MSG,
        }
        #endregion

        #region  [===REJECT歷史紀錄===]
        /// <summary>
        /// REJECT歷史紀錄
        /// </summary>
        public enum SC_REJECT_HISTORY
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// REJECT時間(yyyy-MM-dd HH:mm:ss.fff)
            /// </summary>
            REJECT_TIME,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點
            /// </summary>
            STATION,

            /// <summary>
            /// 卡匣號碼
            /// </summary>
            CARRIERID,

            /// <summary>
            /// 卡匣批號
            /// </summary>
            PARTNO,

            /// <summary>
            /// 本機REJECT編碼
            /// </summary>
            LOCAL_REJECT_CODE,

            /// <summary>
            /// 本機REJECT訊息
            /// </summary>
            LOCAL_REJECT_MSG,

            /// <summary>
            /// 遠端REJECT編碼
            /// </summary>
            REMOTE_REJECT_CODE,

            /// <summary>
            /// 遠端REJECT訊息
            /// </summary>
            REMOTE_REJECT_MSG,
        }
        #endregion

        #region [===機台區域===]
        /// <summary>
        /// 機台區域
        /// </summary>
        public enum SC_EQUIPMENT_ZONE
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點編號
            /// </summary>
            STATION_NAME,

            /// <summary>
            /// 區域名稱
            /// </summary>
            ZONE_NAME,

            /// <summary>
            /// 客戶機台名稱
            /// </summary>
            REPORT_NAME,
        }
        #endregion

        #region [===上報命令===]
        public enum ReportCommand
        {
            /// <summary>
            /// Alarm上報
            /// </summary>
            ALARM_REPORT,

            /// <summary>
            /// 詢問上報
            /// </summary>
            ASK_CARRIER,

            /// <summary>
            /// 入庫上報
            /// </summary>
            CARRY_IN_REPORT,

            /// <summary>
            /// 出庫上報
            /// </summary>
            CARRY_OUT_REPORT,

            /// <summary>
            /// 退盒上報
            /// </summary>
            CARRY_REJECT,

            /// <summary>
            /// 清除上報
            /// </summary>
            CLEAR_CACHE,

            /// <summary>
            /// 鎖格上報
            /// </summary>
            IN_OUT_LOCK,
        }
        #endregion

        public enum SC_REPORT_COMMAND
        {
            /// <summary>
            /// 
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// LCS上報名稱
            /// </summary>
            LCS_COMMAND_NAME,

            /// <summary>
            /// 上報註解內容
            /// </summary>
            LCS_COMMAND_NOTE,

            /// <summary>
            /// 上報
            /// </summary>
            GROUP_NO,

            /// <summary>
            /// 群組編號
            /// </summary>
            REPORT_COMMAND_NO,

            /// <summary>
            /// 上報名稱
            /// </summary>
            REPORT_COMMAND,

            /// <summary>
            /// 上報名稱註解內容
            /// </summary>
            REPORT_COMMAND_NOTE
        }

        #region [===上報指令名稱===]
        /// <summary>
        /// 上報指令名稱
        /// </summary>
        public enum SC_REPORT_COMMAND_NAME
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// LCS上報名稱
            /// </summary>
            LCS_COMMAND_NAME,

            /// <summary>
            /// 上報指令編號
            /// </summary>
            REPORT_COMMAND_NO,

            /// <summary>
            /// 上報指令名稱
            /// </summary>
            REPORT_COMMAND_NAME,

            /// <summary>
            /// 機構位置
            /// </summary>
            LOCATIONID,
        }
        #endregion

        public enum ReportSource
        {
            /// <summary>
            /// 設備端口
            /// </summary>
            LCS,

            /// <summary>
            /// 上位端口
            /// </summary>
            iLIS,
        }

        public enum SC_REPORT_INDEX
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 上報名稱
            /// </summary>
            REPORT_NAME,

            /// <summary>
            /// 目前Index值
            /// </summary>
            REPORT_INDEX,

            /// <summary>
            /// Inex最大值
            /// </summary>
            REPORT_MAX
        }

        public enum IndexTableName
        {
            SC_DIRECTIVE,
        }

        public enum SC_COMMON
        {
            SETNO,

            MODEL_NAME,

            ITEM_NAME,

            ITEM_VALUE,

            VALUECOMMENT,
        }

        public enum SC_COMMON_ITEM_NAME
        {
            ReaderError,

            Empty,

            NA,

            CRANE,

            ReportCraneName,
        }

        public enum SC_DEVICE
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點編號
            /// </summary>
            STATION_NAME,

            /// <summary>
            /// 機台手臂編號
            /// </summary>
            DEVICENO,

            /// <summary> 
            /// 機台手臂名稱
            /// </summary>
            DEVICEID,

            /// <summary>
            /// 客戶機台手臂名稱
            /// </summary>
            HOSTDEVICEID,

            /// <summary>
            /// 機台種類
            /// </summary>
            DEVICETYPE,
        }

        public enum SC_LOCATIONSETTING
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 位置編號
            /// </summary>
            LOCATIONID,

            /// <summary>
            /// 上報編號
            /// </summary>
            HOSTID,

            /// <summary>
            /// 卡匣號碼
            /// </summary>
            CARRIERID,

            /// <summary>
            /// 卡匣批號
            /// </summary>
            PARTNO,

            /// <summary>
            /// BANK
            /// </summary>
            BANK,

            /// <summary>
            /// BAY
            /// </summary>
            BAY,

            /// <summary>
            /// LV
            /// </summary>
            LV,

            /// <summary>
            /// 位置狀態
            /// </summary>
            LOCATIONSTATUS,

            /// <summary>
            /// 位置模式
            /// </summary>
            LOCATIONMODE,

            /// <summary>
            /// 位置種類
            /// </summary>
            LOCATIONTYPE,

            /// <summary>
            /// 是否有卡匣(0:無卡匣 1:有卡匣)
            /// </summary>
            INVENTORYFULL,

            /// <summary>
            /// 區域名稱
            /// </summary>
            ZONEID,

            /// <summary>
            /// 位置排序
            /// </summary>
            LOCATIONPRIORITIZ,
        }
    }
}
