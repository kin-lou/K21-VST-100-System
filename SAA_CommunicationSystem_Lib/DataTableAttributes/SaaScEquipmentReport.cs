using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScEquipmentReport
    {
        public string TASKDATETIME { get; set; }

        public string SETNO { get; set; }

        public string MODEL_NAME { get; set; }

        public string STATION_NAME { get; set; }


        public string CARRIERID { get; set; }

        public string REPORE_DATATRACK { get; set; }

        public string REPORE_DATAREMOTE { get; set; }
        
        public string REPORE_DATALOCAL { get; set; }
        
        public string SENDFLAG { get; set; }
    }
}
