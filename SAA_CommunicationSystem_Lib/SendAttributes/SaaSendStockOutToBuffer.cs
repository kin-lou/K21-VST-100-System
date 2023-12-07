using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendStockOutToBuffer : SaaSendStockOutToReject
    {
        /// <summary>
        /// 盒子種類
        /// </summary>
        public string Type { get; set; }
    }
}
