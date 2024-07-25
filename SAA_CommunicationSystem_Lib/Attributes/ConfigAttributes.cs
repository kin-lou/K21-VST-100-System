using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class ConfigAttributes
    {
        /// <summary>
        /// 設備編號
        /// </summary>
        public string SaaEquipmentNo { get; set; }

        /// <summary>
        /// 設備機型
        /// </summary>
        public string SaaEquipmentName { get; set; }

        /// <summary>
        ///LCS Web Api伺服器IP位置
        /// </summary>
        public string StorageWebApiServerIP { get; set; }

        /// <summary>
        /// iLIS Web Api伺服器IP位置
        /// </summary>
        public string iLISWebApiServerIP { get; set; }

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

        /// <summary>
        /// 傳送指令至LCS Web Api伺服器關鍵字
        /// </summary>
        public string ParaKey { get; set; }

        public string SaaVST101StationName { get; set; }

        public string SaaDestinationIniName { get; set; }

        public string SaaIniParaKey { get; set; }

        public string SaaIniParaKeyStation { get; set; }

        public string PARTICLE { get; set; }

        public string LiftWebApiServerIP { get; set; }
    }
}
