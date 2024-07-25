using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivGoWhere: SaaReceiv
    {
        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 卡匣位置
        /// </summary>
        public string From { get; set; }

        public string Type { get; set; }

        public string Direction { get; set; }

    }
}
