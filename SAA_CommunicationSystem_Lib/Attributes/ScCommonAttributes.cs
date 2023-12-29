using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class ScCommonAttributes
    {
        /// <summary>
        /// 讀取ID錯誤顯示值
        /// </summary>
        public string ReaderError { get; set; }

        /// <summary>
        /// 空值
        /// </summary>
        public string Empty { get; set; }

        /// <summary>
        /// 空值
        /// </summary>
        public string NA { get; set; }

        /// <summary>
        ///手臂名稱
        /// </summary>
        public string CRANE { get; set; }

        /// <summary>
        /// 上報手臂名稱
        /// </summary>
        public string ReportCraneName { get; set; }

        /// <summary>
        /// 詢問卡匣
        /// </summary>
        public string AskCarrier { get; set; }

        /// <summary>
        /// 詢問卡匣結果不存在
        /// </summary>
        public string AskResultNo { get; set; }

        /// <summary>
        /// 詢問卡匣結果存在
        /// </summary>
        public string AskResultYes { get; set; }

        /// <summary>
        ///  LCS 離線
        /// </summary>
        public int LCS_STS_OFFLINE { get; set; }

        /// <summary>
        /// LCS 在線上，但不收入、出料命令
        /// </summary>
        public int LCS_STS_LOCAL_ONLINE { get; set; }

        /// <summary>
        /// LCS 在線上，收入、出料命令
        /// </summary>
        public int LCS_STS_REMOTE_ONLINE { get; set; }

        /// <summary>
        /// LCS 為入出料模式
        /// </summary>
        public int LCS_MODE_InOut { get; set; }

        /// <summary>
        /// LCS 為入料模式
        /// </summary>
        public int LCS_MODE_In { get; set; }

        /// <summary>
        /// LCS 為出料模式
        /// </summary>
        public int LCS_MODE_Out { get; set; }

        /// <summary>
        /// 1:啟動-運轉中/EXECUTING
        /// </summary>
        public int DeviceSts_1 { get; set; }

        /// <summary>
        /// 2:停止-允許啟動/IDLE
        /// </summary>
        public int DeviceSts_2 { get; set; }

        /// <summary>
        /// 3:手動/SETUP
        /// </summary>
        public int DeviceSts_3 { get; set; }

        /// <summary>
        /// 4:停止-無法啟動/INIT
        /// </summary>
        public int DeviceSts_4 { get; set; }

        /// <summary>
        /// 5:啟動-閒置中/READY
        /// </summary>
        public int DeviceSts_5 { get; set; }

        /// <summary>
        /// 天車平台1為入料模式
        /// </summary>
        public int RGV_1_MODE_In { get; set; }

        /// <summary>
        /// 天車平台1為出料模式
        /// </summary>
        public int RGV_1_MODE_Out { get; set; }

        /// <summary>
        /// 天車平台2為入料模式
        /// </summary>
        public int RGV_2_MODE_In { get; set; }

        /// <summary>
        /// 天車平台2為出料模式
        /// </summary>
        public int RGV_2_MODE_Out { get; set; }

        /// <summary>
        /// 天車平台1為啟用狀態
        /// </summary>
        public int RGV_1_STS_ON { get; set; }

        /// <summary>
        /// 天車平台1為關閉狀態
        /// </summary>
        public int RGV_1_STS_OFF { get; set; }

        /// <summary>
        /// 天車平台2為啟用狀態
        /// </summary>
        public int RGV_2_STS_ON { get; set; }

        /// <summary>
        /// 天車平台2為關閉狀態
        /// </summary>
        public int RGV_2_STS_OFF { get; set; }

        /// <summary>
        /// 碼頭平台1名稱
        /// </summary>
        public string Pire1Name { get; set; }

        /// <summary>
        /// 碼頭平台2名稱
        /// </summary>
        public string Pire2Name { get; set; }

        /// <summary>
        /// 碼頭平台入倉模式
        /// </summary>
        public string PireModeIn { get; set; }

        /// <summary>
        /// 碼頭平台出倉模式
        /// </summary>
        public string PireModOut { get; set; }

        /// <summary>
        /// 碼頭平台狀態開啟
        /// </summary>
        public string PireStatusOn { get; set; }

        /// <summary>
        /// 碼頭平台狀態關閉
        /// </summary>
        public string PireStatusOff { get; set; }
    }
}
