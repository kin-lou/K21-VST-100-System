using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLiftTask
    {
        /// <summary>
        /// 任務時間(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public string TASKDATETIME { get; set; }

        /// <summary>
        /// 站點編號
        /// </summary>
        public string STATION_NAME { get; set; }

        /// <summary>
        /// 命令編號
        /// </summary>
        public string COMMANDID { get; set; }

        /// <summary>
        /// 卡匣號碼
        /// </summary>
        public string CARRIERID { get; set; }

        /// <summary>
        /// 起點
        /// </summary>
        public string BEGINSTATION { get; set; }

        /// <summary>
        /// 終點
        /// </summary>
        public string ENDSTATION { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public string RESULT { get; set; }
    }
}
