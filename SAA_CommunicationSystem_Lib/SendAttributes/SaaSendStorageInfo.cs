using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendStorageInfo:SaaSend
    {
        /// <summary>
        /// 開始位置
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 終點位置
        /// </summary>
        public string To { get; set; }
    }
}
