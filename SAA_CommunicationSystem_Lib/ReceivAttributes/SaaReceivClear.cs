using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivClear : SaaReceiv
    {
        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 儲格編號
        /// </summary>
        public string Loc { get; set; }
    }
}
