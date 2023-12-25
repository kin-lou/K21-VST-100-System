using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScReportInadx
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
        /// 上報名稱
        /// </summary>
        public string REPORT_NAME { get; set; }

        /// <summary>
        /// 目前Index值
        /// </summary>
        public int REPORT_INDEX { get; set; }

        /// <summary>
        /// Inex最大值
        /// </summary>
        public int REPORT_MAX { get; set; }
    }
}
