﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem.Attributes
{
    public class GuiUserAttributes
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string USERID { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// 權限群組
        /// </summary>
        public string GROUPID { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string USERNAME { get; set; }
    }
}