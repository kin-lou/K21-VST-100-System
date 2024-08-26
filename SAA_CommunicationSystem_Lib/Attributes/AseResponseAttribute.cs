using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class AseResponseAttribute
    {
        /// <summary>
        /// 是否收到取圖通知
        /// </summary>
        public string RESULT { get; set; } = string.Empty;

        /// <summary>
        /// 異常碼
        /// </summary>
        public string ERRORCODE { get; set; } = string.Empty;

        /// <summary>
        /// 當下ASE回覆時間點
        /// </summary>
        public string DATATIME { get; set; } = string.Empty;

        /// <summary>
        /// 當下時間點
        /// </summary>
        public string TIMESTAMP { get; set; } = string.Empty;

        /// <summary>
        /// 回傳狀態碼
        /// </summary>
        public bool ISSUCCESS { get; set; } = false;

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string MESSAGE { get; set; } = string.Empty;
    }
}
