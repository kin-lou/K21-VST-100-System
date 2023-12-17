using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScEquipmentZone
    {
        /// <summary>
        /// 機型編號
        /// </summary>
        public string SETNO { get; set; }

        /// <summary>
        /// 機型名稱
        /// </summary>
        public string MODEL_NAME { get; set; }

        /// <summary>
        /// 站點編號
        /// </summary>
        public string STATION_NAME { get; set; }

        /// <summary>
        /// 區域名稱
        /// </summary>
        public string ZONE_NAME { get; set; }

        /// <summary>
        /// 客戶機台名稱
        /// </summary>
        public string REPORT_NAME { get; set; }
    }
}
