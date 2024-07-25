using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.DataTableAttributes
{
    public class SaaScLiftE84iLisPlc
    {
        public string TASKDATETIME { get; set; } = string.Empty;
        public string STATION_NAME { get; set; } = string.Empty;
        public string SHUTTLEID { get; set; } = string.Empty;
        public string COMMANDID { get; set; } = string.Empty;
        public string CARRIERID { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public int VALID { get; set; }
        public int CS_0 { get; set; }
        public int CS_1 { get; set; }
        public int TR_REQ { get; set; }
        public int L_REQ { get; set; }
        public int U_REQ { get; set; }
        public int READY { get; set; }
        public int BUSY { get; set; }
        public int COMPT { get; set; }
        public int CONT { get; set; }
        public int HOA_VBL { get; set; }
        public int ES { get; set; }
        public int VA { get; set; }
        public int AM_AVBL { get; set; }
        public int VS_0 { get; set; }
        public int VS_1 { get; set; }

        public int SELECT { get; set; }

        public string RESULT { get; set; } = string.Empty;
    }
}
