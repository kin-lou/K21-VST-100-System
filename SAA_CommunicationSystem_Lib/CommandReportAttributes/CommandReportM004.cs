using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.CommandReportAttributes
{
    public class CommandReportM004 : CommandReport
    {
        public string CARRIER { get; set; }

        public string SCHEDULE { get; set; }


        public string ORIGIN { get; set; }

        public string DESTINATION { get; set; }

        public string REJECT_CODE { get; set; }

        public string REJECT_MESSAGE { get; set; }
    }
}
