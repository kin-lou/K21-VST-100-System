using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSend3190StockIn:SaaSend
    {
        /// <summary>
        /// 水洗次數
        /// </summary>
        public string ProcessCount { get; set; }
    }
}
