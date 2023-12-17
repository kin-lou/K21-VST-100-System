using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScSquipmentStatus
    {
        /// <summary>
        /// 機型編號
        /// </summary>
        public int SETNO { get; set; }

        /// <summary>
        /// 機型名稱
        /// </summary>
        public string MODEL_NAME { get; set; }

        /// <summary>
        /// 站點
        /// </summary>
        public string STATION { get; set; }

        /// <summary>
        ///客戶機台名稱
        /// </summary>
        public string REPORT_NAME { get; set; }

        /// <summary>
        /// 設備狀態
        /// </summary>
        public string EQUIPMENT_STATUS { get; set; }

        /// <summary>
        /// 設備狀態代碼
        /// </summary>
        public string EQUIPMENT_STATUS_CODE { get; set; }

        /// <summary>
        /// 設備狀態更新時間(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public string STATUS_UPDATE_TIME { get; set; }

        /// <summary>
        /// 設備模式
        /// </summary>
        public string EQUIPMENT_MODEL { get; set; }

        /// <summary>
        /// 設備模式代碼
        /// </summary>
        public string EQUIPMENT_MODEL_CODE { get; set; }

        /// <summary>
        /// 設備模式更新時間(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public string MODEL_UPDATE_TIME { get; set; }
    }
}
