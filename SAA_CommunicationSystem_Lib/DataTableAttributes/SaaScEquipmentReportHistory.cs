using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScEquipmentReportHistory: SaaScEquipmentReport
    {
        /// <summary>
        /// 新增上報歷史紀錄時間
        /// </summary>
        public string UPDATETASKDATETIME { get; set; }
    }
}
