﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_ReportCommand
    {

        /// <summary>
        /// ALARM上報
        /// </summary>
        public Dictionary<string, string> DicAlarmReport = new Dictionary<string, string>();

        /// <summary>
        /// 詢問上報
        /// </summary>
        public Dictionary<string, string> DicAskCarrier = new Dictionary<string, string>();

        /// <summary>
        /// 入庫上報
        /// </summary>
        public Dictionary<string, string> DicCarryInReport = new Dictionary<string, string>();

        /// <summary>
        /// 出庫上報
        /// </summary>
        public Dictionary<string, string> DicCarryOutReport = new Dictionary<string, string>();

        /// <summary>
        /// 退盒上報
        /// </summary>
        public Dictionary<string, string> DicCarryReject = new Dictionary<string, string>();

        /// <summary>
        /// 清除上報
        /// </summary>
        public Dictionary<string, string> DicClearCache = new Dictionary<string, string>();

        /// <summary>
        /// 鎖格上報
        /// </summary>
        public Dictionary<string, string> DicInOutLock = new Dictionary<string, string>();

        public List<string> AlarmReportAry= new List<string>();

        public List<string> AskCarrierAry = new List<string>();

        public List<string> CarryInReportAry = new List<string>();

        public List<string> CarryOutReportAry = new List<string>();

        public List<string> CarryRejectAry = new List<string>();

        public List<string> ClearCacheAry = new List<string>();

        public List<string> InOutLockAry = new List<string>();
    }
}