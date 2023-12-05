using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class ConfigAttributes
    {
        /// <summary>
        /// 本機端SERVER
        /// </summary>
        public string WebApiServerIP { get; set; }

        /// <summary>
        /// 資料庫名稱
        /// </summary>
        public string SaaDataBase { get; set; }

        /// <summary>
        /// 資料庫IP
        /// </summary>
        public string SaaDataBaseIP { get; set; }

        /// <summary>
        /// 資料庫使用者名稱
        /// </summary>
        public string SaaDataBaseName { get; set; }

        /// <summary>
        /// 資料庫使用者密碼
        /// </summary>
        public string SaaDataBasePassword { get; set; }

        /// <summary>
        /// NLog Config檔案名稱
        /// </summary>
        public string SaaLogName { get; set; }

        /// <summary>
        /// 系統名稱
        /// </summary>
        public string SaaSystemsName { get; set; }

        /// <summary>
        /// 回覆LCS接受結果完成
        /// </summary>
        public string WebApiResultOK { get; set; }

        /// <summary>
        /// 回覆LCS接受結果失敗
        /// </summary>
        public string WebApiResultFAIL { get; set; }
    }
}
