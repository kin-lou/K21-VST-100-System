using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLiftCarrierInfoReject
    {
        public string TASKDATETIME { get; set; } = string.Empty;

        public int SETNO { get; set; }

        public string MODEL_NAME { get; set; } = string.Empty;

        public string STATION_NAME { get; set; } = string.Empty;

        public string CARRIERID { get; set; } = string.Empty;

        public string DEVICETYPE { get; set; } = string.Empty;

        public string QTIME { get; set; } = string.Empty;

        public string CYCLETIME { get; set; } = string.Empty;
    }
}
