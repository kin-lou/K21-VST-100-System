using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.GuiAttributes
{
    public class GuiOpetationHistoryAttributes
    {
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string PROJECT_ITEM { get; set; }

        /// <summary>
        /// 操作時間
        /// </summary>
        public string OPERATE_DATETIME { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string USERNAME { get; set; }

        /// <summary>
        /// 操作內容記錄
        /// </summary>
        public string OPERATE_CONTENT { get; set; }
    }
}
