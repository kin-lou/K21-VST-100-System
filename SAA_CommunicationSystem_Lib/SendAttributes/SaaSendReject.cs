using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendReject : SaaSend
    {
        /// <summary>
        /// 退料機構編號
        /// </summary>
        public string To { get; set; }
    }
}
