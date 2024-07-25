using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportAttributes
{
    public class SaaRequestEquipmentTransport: SaaReport
    {
        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string CarrierID { get; set; }

        /// <summary>
        /// 起點
        /// </summary>
        public string BeginStation { get; set; }

        /// <summary>
        /// 終點
        /// </summary>
        public string EndStation { get; set; }
    }
}
