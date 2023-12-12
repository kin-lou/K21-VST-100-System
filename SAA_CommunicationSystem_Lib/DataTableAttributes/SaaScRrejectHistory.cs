using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScRrejectHistory : SaaScRejectList
    {
        /// <summary>
        /// Reject時間
        /// </summary>
        public string REJECT_TIME { get; set; }

        /// <summary>
        /// 站點
        /// </summary>
        public string STATION { get; set; }

        /// <summary>
        ///卡匣號碼 
        /// </summary>
        public string CARRIERID { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string PARTNO { get; set; }
    }
}
