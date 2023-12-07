using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendQtimeCloseToExpiration:SaaSend
    {
        /// <summary>
        /// 排除卡匣ID
        /// </summary>
        public string ExcludedIDs { get; set; }
    }
}
