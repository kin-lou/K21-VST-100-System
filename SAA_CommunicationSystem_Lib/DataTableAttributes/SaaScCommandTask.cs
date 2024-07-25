using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScCommandTask
    {
        public string TASKDATETIME { get; set; }

        public int SETNO { get; set; }

        public string MODEL_NAME { get; set; }

        public string STATION_NAME { get; set; }

        public string COMMANDID { get; set; }

        public string CARRIERID { get; set; }

        public string LOCATIONTAKE { get; set; }

        public string LOCATIONPUT { get; set; }

        public string RESULT { get; set; }
    }
}
