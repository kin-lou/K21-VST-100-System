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

        /// <summary>
        /// 讀取條碼上報站點名稱
        /// </summary>
        public string ReaderSataion { get; set; }

        /// <summary>
        /// 平台上報位置
        /// </summary>
        public string ReadStatge { get; set; }

        /// <summary>
        /// 回覆iLIS接收成功
        /// </summary>
        public string Success { get; set; }

        /// <summary>
        /// 回覆iLIS接收失敗
        /// </summary>
        public string Fail { get; set; }

        /// <summary>
        /// REJECT平台編號
        /// </summary>
        public string RejectStage { get; set; }

        /// <summary>
        /// 平台上報位置名稱
        /// </summary>
        public string ReadStatgeName { get; set; }

        /// <summary>
        /// LIFT物料追蹤-搬移
        /// </summary>
        public string Move { get; set; }=string.Empty;

        /// <summary>
        /// LIFT物料追蹤-建立
        /// </summary>
        public string Establish { get; set; } = string.Empty;

        /// <summary>
        /// LIFT物料追蹤-清除
        /// </summary>
        public string Clear { get; set; } = string.Empty;

        /// <summary>
        /// LIFT物料追蹤-詢問
        /// </summary>
        public string Ask { get; set; } = string.Empty;

        /// <summary>
        /// LIFT物料追蹤-更新
        /// </summary>
        public string Update { get; set; } = string.Empty;

        /// <summary>
        /// LIFT物料追蹤-回覆
        /// </summary>
        public string Reply { get; set; } = string.Empty;

        /// <summary>
        /// LIFT資料同步-有帳
        /// </summary>
        public string Have { get; set; } = string.Empty;

        /// <summary>
        /// LIFT資料同步-無帳
        /// </summary>
        public string None { get; set; } = string.Empty;

        /// <summary>
        /// 卡匣屬性為空值時顯示文字
        /// </summary>
        public string CarrierType { get; set; } = string.Empty;

        /// <summary>
        /// UD管控空盒數量
        /// </summary>
        public int LiftCarrierInfoEmptyCount { get; set; } = 0;

        /// <summary>
        /// LIFT LD代號
        /// </summary>
        public int DevicetTypeLD { get; set; } = 1;

        /// <summary>
        /// LIFT UD代號
        /// </summary>
        public int DeivertTypeUD { get; set; } = 2;

        /// <summary>
        /// LIFT卡匣回倉儲編號
        /// </summary>
        public string SaaZipStationName { get; set; } = string.Empty;

        /// <summary>
        /// 多少秒詢問一次iLis天車任務
        /// </summary>
        public int AskShuttleTaskTime { get; set; } = 0;

        /// <summary>
        /// LD空框檢知
        /// </summary>
        public string DetectionLD { get; set; } = string.Empty;

        /// <summary>
        /// UD空框檢知
        /// </summary>
        public string DetectionUD { get; set; } = string.Empty;

        /// <summary>
        /// Cycle_Time
        /// </summary>
        public int CycleTimeTask { get; set; } = 600;

        /// <summary>
        /// UD水位要盒時間
        /// </summary>
        public int BoxRequirements { get; set; } = 60;

        public int EmptyBoxTime { get; set; } = 60;
    }
}
