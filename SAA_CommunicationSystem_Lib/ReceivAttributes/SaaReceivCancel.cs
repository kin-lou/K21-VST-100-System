using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivCancel: SaaReceiv
    {
        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string NO { get; set; }

        /// <summary>
        /// 起點
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 終點
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 儲格數量
        /// </summary>
        public string WareCount { get; set; }
    }
}
