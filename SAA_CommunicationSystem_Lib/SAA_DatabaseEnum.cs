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
    }
}
