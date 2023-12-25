using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScDevice
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
        /// 站點編號
        /// </summary>
        public string STATION_NAME { get; set; }

        /// <summary>
        /// 機台手臂編號
        /// </summary>
        public string DEVICENO { get; set; }

        /// <summary>
        /// 機台手臂名稱
        /// </summary>
        public string DEVICEID { get; set; }

        /// <summary>
        /// 客戶機台手臂名稱
        /// </summary>
        public string HOSTDEVICEID { get; set; }

        /// <summary>
        /// 機台種類
        /// </summary>
        public string DEVICETYPE { get; set; }
    }
}
