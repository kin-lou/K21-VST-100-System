using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScPurchase
    {
        /// <summary>
        /// 任務時間
        /// </summary>
        public string TASKDATETIME { get; set; } = string.Empty;

        /// <summary>
        /// 機型編號
        /// </summary>
        public int SETNO { get; set; } = 0;

        /// <summary>
        /// 機型名稱
        /// </summary>
        public string MODEL_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 站點
        /// </summary>
        public string STATION { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string CARRIERID { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string REPLY { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string LOCAL { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string COMMANDID { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string REPLYRESULT { get; set; } = string.Empty;
    }
}
