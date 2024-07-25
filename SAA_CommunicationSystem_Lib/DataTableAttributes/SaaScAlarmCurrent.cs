using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScAlarmCurrent
    {
        public string SETNO { get; set; } = string.Empty;
        public string MODELNAME { get; set; } = string.Empty;
        public string STATION_NAME { get; set; } = string.Empty;
        public string ALARM_CODE { get; set; } = string.Empty;
        public string ALARM_STATUS { get; set; } = string.Empty;
        public string REPORT_STATUS { get; set; } = string.Empty;
        public string START_TIME { get; set; } = string.Empty;
        public string END_TME { get; set; } = string.Empty;
    }
}
