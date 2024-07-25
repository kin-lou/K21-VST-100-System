using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportAttributes
{
    public class SaaReportResult : SaaReport
    {
        /// <summary>
        /// 回覆碼
        /// </summary>
        public string ReturnCode { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        public string ReturnMessage { get; set; }
    }
}
