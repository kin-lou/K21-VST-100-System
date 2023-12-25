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
    }
}
