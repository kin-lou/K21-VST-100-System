using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLiftTaskHistory: SaaScLiftTask
    {
        /// <summary>
        /// 更新資料時間(yyyy-MM-dd HH:mm:ss.fff)
        /// </summary>
        public string UPDATEDATATIME { get; set; }
    }
}
