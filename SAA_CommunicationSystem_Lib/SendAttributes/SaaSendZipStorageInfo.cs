using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendZipStorageInfo : SaaSend
    {
        /// <summary>
        /// PLC 儲格編號
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// PLC 儲格編號
        /// </summary>
        public string To { get; set; }
    }
}
