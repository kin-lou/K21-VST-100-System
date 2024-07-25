using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaEquipmentCarrierInfo
    {
        /// <summary>
        /// 設備編號
        /// </summary>
        public int SETNO { get; set; }

        /// <summary>
        /// 設備名稱
        /// </summary>
        public string MODEL_NAME { get; set; }

        /// <summary>
        /// 設備站點
        /// </summary>
        public string STATIOM_NAME { get; set;}

        public string REMOTE { get; set; }

        /// <summary>
        /// 卡匣ID
        /// </summary>
        public string CARRIERID { get; set;}

        /// <summary>
        /// 批號
        /// </summary>
        public string PARTNO { get; set; }

        /// <summary>
        /// 載體類型
        /// </summary>
        public string CARRIERTYOE { get; set; }

        /// <summary>
        /// 旋轉資訊
        /// </summary>
        public string ROTFLAG { get; set; }

        /// <summary>
        /// 翻轉資訊
        /// </summary>
        public string FLIPFLAG { get; set; }

        /// <summary>
        /// 貨批站點
        /// </summary>
        public string OPER { get; set; }

        /// <summary>
        /// 貨批參數
        /// </summary>
        public string RECIPE { get; set; }

        /// <summary>
        /// 原始位置
        /// </summary>
        public string ORIGIN { get; set; }

        /// <summary>
        /// 現在位置
        /// </summary>
        public string DESTINATION { get; set; }

        /// <summary>
        /// 貨批效期
        /// </summary>
        public string QTIME { get; set; }

        /// <summary>
        /// 盒效期
        /// </summary>
        public string CYCLETIME { get; set; }

        /// <summary>
        /// 退盒編碼
        /// </summary>
        public string REJECT_CODE { get; set; }

        /// <summary>
        /// 退盒資訊
        /// </summary>
        public string REJECT_MESSAGE { get; set; }

        /// <summary>
        /// 載體狀態
        /// </summary>
        public string CARRIERSTATE { get; set; }

        /// <summary>
        /// 目的地類型
        /// </summary>
        public string DESTINATIONTYPE { get; set; }

        /// <summary>
        /// 卡匣結果
        /// </summary>
        public string CARRIERFLAG { get; set; }

        public string CARRIERTYPE { get; set; }

    }
}
