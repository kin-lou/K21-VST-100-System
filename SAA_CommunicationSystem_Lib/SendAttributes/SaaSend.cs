using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSend
    {
        /// <summary>
        /// 指令
        /// </summary>
        public string CMD { get; set; }

        /// <summary>
        /// 站點
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// 盒號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string No { get; set; }

        public enum CSMName
        {
            /// <summary>
            /// 清除貨批在籍 (單格)
            /// </summary>
            ClearStorage,

            /// <summary>
            /// 指定搬運起始/目的 (派貨用)
            /// </summary>
            StockOut,

            /// <summary>
            /// 通知進入機構
            /// </summary>
            StockIn,

            /// <summary>
            /// 通知離開機構
            /// </summary>
            StockOutToReject,

            /// <summary>
            /// 通知 Reject (未進入機構)
            /// </summary>
            Reject,

            /// <summary>
            /// 通知經過倉儲 (未進入機構
            /// </summary>
            ThroughWip,

            /// <summary>
            /// 詢問儲格水位
            /// </summary>
            WareCount,

            /// <summary>
            /// 時間同步
            /// </summary>
            TimeSync,

            /// <summary>
            /// 詢問Qtime到期可用的
            /// </summary>
            QtimeCloseToExpiration,

            /// <summary>
            /// 詢問儲格資訊
            /// </summary>
            ZipStorageInfo,

            /// <summary>
            /// 詢問儲格資訊
            /// </summary>
            WipStorageInfo,

            /// <summary>
            /// 詢問儲格資訊
            /// </summary>
            IsCarrierInWip,

            /// <summary>
            /// 詢問 LCS 狀態
            /// </summary>
            LCS_Sts,

            /// <summary>
            /// 離線
            /// </summary>
            SetLCS_STS_OFFLINE,

            /// <summary>
            /// 本基端上線
            /// </summary>
            SetLCS_STS_LOCAL_ONLINE,

            /// <summary>
            /// 遠端上線
            /// </summary>
            SetLCS_STS_REMOTE_ONLINE,

            /// <summary>
            /// 可進可出
            /// </summary>
            SetLCS_MODE_InOut,

            /// <summary>
            /// 只進不出
            /// </summary>
            SetLCS_MODE_In,

            /// <summary>
            /// 只出不進
            /// </summary>
            SetLCS_MODE_Out,

            /// <summary>
            /// 通知移庫至Buffer
            /// </summary>
            StockOutToBuffer,

            /// <summary>
            /// 詢問Buffer可用數量
            /// </summary>
            BufferCount,

            /// <summary>
            /// 詢問機構資訊
            /// </summary>
            QueryPortInfo,

            /// <summary>
            /// HOST 測試通訊是否斷掉
            /// </summary>
            AreYouOK,
        }
    }
}
