using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.CommandReportAttributes
{
    public class CommandReportS002: CommandReport
    {
        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string CARRIER { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string SCHEDULE { get; set; }

        /// <summary>
        /// 起點
        /// </summary>
        public string ORIGIN { get; set; }

        /// <summary>
        /// 終點
        /// </summary>
        public string DESTINATION { get; set; }
    }
}
