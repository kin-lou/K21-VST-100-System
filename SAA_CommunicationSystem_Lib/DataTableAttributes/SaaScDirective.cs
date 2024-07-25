using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScDirective
    {
        /// <summary>
        /// 任務時間(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public string TASKDATETIME { get; set; }

        /// <summary>
        /// 機型編號
        /// </summary>
        public string SETNO { get; set; }

        /// <summary>
        /// 命令編號
        /// </summary>
        public string COMMANDON { get; set; }

        /// <summary>
        /// 站點
        /// </summary>
        public string STATION_NAME { get; set; }

        /// <summary>
        /// 卡匣號碼
        /// </summary>
        public string CARRIERID { get; set; }

        /// <summary>
        /// 指令編號
        /// </summary>
        public string COMMANDID { get; set; }

        /// <summary>
        /// 指令內容
        /// </summary>
        public string COMMANDTEXT { get; set; }

        /// <summary>
        /// 指令來源
        /// </summary>
        public string SOURCE { get; set; }

        /// <summary>
        /// 傳送結果
        /// </summary>
        public string SENDFLAG { get; set; }
    }
}
