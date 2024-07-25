using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivLiftAttributes
{
    public class SaaEsReportTransportRequirement
    {
        /// <summary>
        /// 站點
        /// </summary>
        public string STATION { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        public string DATATIME { get; set; }

        /// <summary>
        /// 命令編號
        /// </summary>
        public string TEID { get; set; }

        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string CARRIERID { get; set; }

        /// <summary>
        /// 起始站點
        /// </summary>
        public string BEGINSTATION { get; set; }

        /// <summary>
        /// 終點站點
        /// </summary>
        public string ENDSTATION { get; set; }
    }
}
