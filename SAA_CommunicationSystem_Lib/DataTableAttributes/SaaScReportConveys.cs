﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScReportConveys
    {
        /// <summary>
        /// 機型編號
        /// </summary>
        public int SETNO { get; set; }

        /// <summary>
        /// 機型名稱
        /// </summary>
        public string MODEL_NAME { get; set;}

        /// <summary>
        /// LCS上報名稱
        /// </summary>
        public string LCS_COMMAND_NAME { get; set;}

        /// <summary>
        /// 上報註解內容
        /// </summary>
        public string LCS_COMMAND_NOTE { get; set; }

        /// <summary>
        /// 上報名稱編號
        /// </summary>
        public string REPORT_COMMAND_NO { get; set; }

        /// <summary>
        /// 上報名稱
        /// </summary>
        public string REPORT_COMMAND { get; set; }

        /// <summary>
        /// 上報名稱註解內容
        /// </summary>
        public string REPORT_COMMAND_NOTE { get; set; }

        /// <summary>
        /// 機構名稱代碼
        /// </summary>
        public int DEVICE_TYPE { get; set; }
    }
}
