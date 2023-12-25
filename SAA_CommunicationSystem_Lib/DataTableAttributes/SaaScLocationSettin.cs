using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLocationSettin
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
        /// 位置編號
        /// </summary>
        public string LOCATIONID { get; set; }

        /// <summary>
        /// 上報編號
        /// </summary>
        public string HOSTID { get; set; }

        /// <summary>
        /// 卡匣號碼
        /// </summary>
        public string CARRIERID { get; set; }

        /// <summary>
        /// 卡匣批號
        /// </summary>
        public string PARTNO { get; set; }

        /// <summary>
        /// BANK
        /// </summary>
        public string BANK { get; set; }

        /// <summary>
        /// BAY
        /// </summary>
        public string BAY { get; set; }

        /// <summary>
        /// LV
        /// </summary>
        public string LV { get; set; }

        /// <summary>
        /// 位置狀態
        /// </summary>
        public string LOCATIONSTATUS { get; set; }

        /// <summary>
        /// 位置模式
        /// </summary>
        public string LOCATIONMODE { get; set; }

        /// <summary>
        /// 位置種類
        /// </summary>
        public string LOCATIONTYPE { get; set; }

        /// <summary>
        /// 是否有卡匣(0:無卡匣 1:有卡匣)
        /// </summary>
        public int INVENTORYFULL { get; set; }

        /// <summary>
        /// 區域名稱
        /// </summary>
        public string ZONEID { get; set; }

        /// <summary>
        /// 位置排序
        /// </summary>
        public int LOCATIONPRIORITIZ { get; set; }
    }
}
