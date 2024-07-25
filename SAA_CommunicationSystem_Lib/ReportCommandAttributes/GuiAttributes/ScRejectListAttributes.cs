using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.GuiAttributes
{
    public class ScRejectListAttributes
    {
        /// <summary>
        /// 機型編號
        /// </summary>
        public int SETNO { get; set; }

        /// <summary>
        /// 機型名稱
        /// </summary>
        public string MODEL_NAME { get; set; }

        /// <summary>
        /// 本機REJECT編碼
        /// </summary>
        public string LOCAL_REJECT_CODE { get; set; }

        /// <summary>
        /// 本機REJECT訊息
        /// </summary>
        public string LOCAL_REJECT_MSG { get; set; }

        /// <summary>
        /// 遠端REJECT編碼
        /// </summary>
        public string REMOTE_REJECT_CODE_ { get; set; }

        /// <summary>
        /// 遠端REJECT訊息
        /// </summary>
        public string REMOTE_REJECT_MSG { get; set; }
    }
}
