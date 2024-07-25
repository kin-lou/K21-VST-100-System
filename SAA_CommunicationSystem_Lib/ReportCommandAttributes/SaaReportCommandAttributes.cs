using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportCommandAttributes
{
    public class SaaReportCommandAttributes
    {
        /// <summary>
        /// 指令編碼
        /// </summary>
        public string CMD_NO { get; set; }

        /// <summary>
        /// 指令名稱
        /// </summary>
        public string CMD_NAME { get; set; }

        /// <summary>
        ///站點
        /// </summary>
        public string STATION { get; set; }

        /// <summary>
        /// 批號
        /// </summary>
        public string SCHEDULE { get; set; }

        /// <summary>
        /// 儲格位置
        /// </summary>
        public string WARENUMBER { get; set; }

        /// <summary>
        /// 起點
        /// </summary>
        public string ORIGIN { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string DESTINATION { get; set; }

        /// <summary>
        /// 是否翻轉
        /// </summary>
        public string ROTFLAG { get; set; }

        /// <summary>
        /// 盒子上下方向
        /// </summary>
        public string FLIPFLAG { get; set; }

        /// <summary>
        /// 盒號
        /// </summary>
        public string CARRIER { get; set; }

        /// <summary>
        /// 起始點
        /// </summary>
        public string FROM { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string TO { get; set; }

        /// <summary>
        /// 卡匣生命期限
        /// </summary>
        public string QTIME { get; set; }

        /// <summary>
        /// 貨批站點
        /// </summary>
        public string OPER{ get; set; }

        /// <summary>
        /// 盒效期
        /// </summary>
        public string CYCLETIME { get; set; }

        /// <summary>
        /// 貨批參數
        /// </summary>
        public string RECIPE{ get; set; }

        /// <summary>
        /// 退盒編碼
        /// </summary>
        public string REJECT_CODE { get; set; }

        /// <summary>
        /// 退盒資訊
        /// </summary>
        public string REJECT_MESSAGE { get; set; }

        /// <summary>
        /// 卡匣屬性
        /// </summary>
        public string CARRIERTYOE { get; set; }
    }
}
