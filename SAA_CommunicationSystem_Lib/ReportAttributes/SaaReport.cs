using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportAttributes
{
    public class SaaReport
    {
        /// <summary>
        /// 站點
        /// </summary>
        public string StationID { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 命令編號
        /// </summary>
        public string TEID { get; set; }
    }
}
