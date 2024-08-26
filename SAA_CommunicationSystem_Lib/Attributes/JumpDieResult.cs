using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class JumpDieResult
    {
        /// <summary>
        /// ASE 回傳資訊
        /// </summary>
        public AseResponseAttribute Data { get; set; } = new AseResponseAttribute();

        /// <summary>
        /// 回傳狀態
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
