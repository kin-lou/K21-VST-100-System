using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivAlarm: SaaReceiv
    {
        /// <summary>
        /// 警告發生設備
        /// </summary>
        public string EQP { get; set; }

        /// <summary>
        /// 警告碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 警告訊息
        /// </summary>
        public string Msg { get; set; }
    }
}
