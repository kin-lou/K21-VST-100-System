﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SendAttributes
{
    public class SaaSendStockOutToReject:SaaSend
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 終點位置
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public string Direction { get; set; }
    }
}