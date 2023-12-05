using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivPurpose: SaaReceiv
    {
        /// <summary>
        /// 現在料在哪
        /// </summary>
        public string WhereCarrier { get; set; }

        /// <summary>
        /// 盒號
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string NO { get; set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 結束位置
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 左右方向+上下方向
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 可用儲格數-總儲格數
        /// </summary>
        public string WareCount { get; set; }

        /// <summary>
        /// 退盒原因
        /// </summary>
        public string RejectInfo { get; set; }
    }
}
