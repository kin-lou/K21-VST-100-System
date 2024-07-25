using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLiftE84Plc
    {
        public string TASKDATETIME { get; set; }

        public string STATION_NAME { get; set; }

        public string SHUTTLEID { get; set; }

        public string COMMANDID { get; set; }

        public string CARRIERID { get; set; }

        public int CS_0 { get; set; }

        public int Valid { get; set; }

        public int TR_REQ { get; set; }

        public int Busy { get; set; }

        public int Complete { get; set; }

        public int Continue { get; set; }

        public int SELECT { get; set; }

        public int AM_AVBL { get; set; }

        public string RESULT { get; set; }
    }
}
