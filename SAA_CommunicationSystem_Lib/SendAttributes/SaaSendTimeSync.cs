using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendTimeSync : SaaSend
    {
        /// <summary>
        /// 時間:yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        public string Data { get; set; }
    }
}
