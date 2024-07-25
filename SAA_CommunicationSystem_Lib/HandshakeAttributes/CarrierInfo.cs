using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem_Lib.HandshakeAttributes
{
    public class CarrierInfo
    {
        /// <summary>
        /// 載體ID
        /// </summary>
        public string CarrierID { get; set; } = string.Empty;

        /// <summary>
        /// 載體類型
        /// </summary>
        public string CarrierType { get; set; } = string.Empty;

        /// <summary>
        /// 批號
        /// </summary>
        public string Schedule { get; set; } = string.Empty;

        /// <summary>
        /// 旋轉資訊
        /// </summary>
        public string Rotation { get; set; } = string.Empty;

        /// <summary>
        /// 翻轉資訊
        /// </summary>
        public string Flip { get; set; } = string.Empty;

        /// <summary>
        /// 載體狀態
        /// </summary>
        public string CarrierState { get; set; } = string.Empty;

        /// <summary>
        /// 目的地類型
        /// </summary>
        public string DestinationType { get; set; } = string.Empty;

        /// <summary>
        /// Qtime
        /// </summary>
        public string Qtime { get; set; } = string.Empty;

        /// <summary>
        /// Cycletime
        /// </summary>
        public string Cycletime { get; set; } = string.Empty;

        public string Oper { get; set; }

        public string Recipe { get; set; }

        public string RejectCode { get; set; }

        public string RejectMessage { get; set; }
    }
}
