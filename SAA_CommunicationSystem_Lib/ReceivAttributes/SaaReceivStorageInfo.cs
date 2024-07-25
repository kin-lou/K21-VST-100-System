using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivAttributes
{
    public class SaaReceivStorageInfo : SaaReceiv
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public string Sts { get; set; }

        /// <summary>
        /// 儲格編號
        /// </summary>
        public string Loc { get; set; }

        /// <summary>
        ///儲格總數
        /// </summary>
        public string WareCount { get; set; }

        public string CarrideID { get; set; } = string.Empty;
    }
}
