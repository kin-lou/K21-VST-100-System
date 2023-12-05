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
    }
}
