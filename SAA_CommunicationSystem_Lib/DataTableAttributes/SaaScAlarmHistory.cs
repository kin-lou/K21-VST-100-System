using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScAlarmHistory
    {
        public string SETNO { get; set; }

        public string TRN_TIME { get; set; }

        public string MODEL_NAME { get; set; }

        public string STATION_NAME { get; set; }

        public string ALARM_CODE { get; set; }

        public string ALARM_MAG { get; set; }

        public string ALARM_TYPE { get; set; }

        public string ALARM_STATUS { get; set; }

        public string REPORT_STATUS { get; set; }

        public string START_TIME { get; set; }

        public string END_TIME { get; set; }
    }
}
