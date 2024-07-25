using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.CommandReportAttributes
{
    public class CommandReportQ001 : CommandReport
    {
        public string CARRIER { get; set; }

        public string STATION { get; set; }
    }
}
