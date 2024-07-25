using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportAttributes
{
    public class SaaReportHandshakeCarrierTransport: SaaReport
    {
        /// <summary>
        /// 天車序號
        /// </summary>
        public string ShuttleID { get; set; }

        /// <summary>
        /// 任務編號
        /// </summary>
        public string MissionID { get; set; }

        /// <summary>
        /// 交握類型
        /// </summary>
        public string HandsHakeType { get;set; }

        /// <summary>
        ///交握訊號
        /// </summary>
        public Handshake Handshake { get; set; } = new Handshake();

        /// <summary>
        /// 載體資訊
        /// </summary>
        public CarrierInfo CarrierInfo { get; set; } = new CarrierInfo();
    }
}
