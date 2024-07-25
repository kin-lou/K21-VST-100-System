using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem_Lib.HandshakeAttributes
{
    public class Handshake
    {
        /// <summary>
        /// 設備類型
        /// </summary>
        public HardwareType HardwareType { get; set; }

        /// <summary>
        /// 是否可被使用
        /// </summary>
        public string UsingFlag { get; set; } = string.Empty;

        /// <summary>
        /// 模式
        /// </summary>
        public string Mode { get; set; } = string.Empty;

        /// <summary>
        /// 是否可交握
        /// </summary>
        public string VALID { get; set; } = string.Empty;

        /// <summary>
        /// 指定傳輸Port1
        /// </summary>
        public string CS_0 { get; set; } = string.Empty;

        /// <summary>
        /// 指定傳輸Port2
        /// </summary>
        public string CS_1 { get; set; } = string.Empty;

        /// <summary>
        /// 傳輸請求
        /// </summary>
        public string TR_REQ { get; set; } = string.Empty;

        /// <summary>
        /// 送貨請求
        /// </summary>
        public string L_REQ { get; set; } = string.Empty;

        /// <summary>
        /// 取貨請求
        /// </summary>
        public string U_REQ { get; set; } = string.Empty;

        /// <summary>
        /// 可進行傳輸
        /// </summary>
        public string READY { get; set; } = string.Empty;

        /// <summary>
        /// 正在進行傳輸
        /// </summary>
        public string BUSY { get; set; } = string.Empty;

        /// <summary>
        /// 完成傳輸
        /// </summary>
        public string COMPT { get; set; } = string.Empty;

        /// <summary>
        /// 繼續傳輸
        /// </summary>
        public string CONT { get; set; } = string.Empty;

        /// <summary>
        /// 可進行傳輸 : On 時可傳輸
        /// </summary>
        public string HO_AVBL { get; set; } = string.Empty;

        /// <summary>
        /// 緊急停止: On 時可傳輸
        /// </summary>
        public string ES { get; set; } = string.Empty;

        /// <summary>
        /// 車子抵達port
        /// </summary>
        public string VA { get; set; } = string.Empty;

        /// <summary>
        /// 運輸(搬運)設備可使用
        /// </summary>
        public string AM_AVBL { get; set; } = string.Empty;

        /// <summary>
        /// 指定傳輸Port1(車子)
        /// </summary>
        public string VS_0 { get; set; } = string.Empty;

        /// <summary>
        /// 指定傳輸Port2(車子)
        /// </summary>
        public string VS_1 { get; set; } = string.Empty;
    }
}
