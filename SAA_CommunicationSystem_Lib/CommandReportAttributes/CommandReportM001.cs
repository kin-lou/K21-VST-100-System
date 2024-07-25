using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.CommandReportAttributes
{
    public class CommandReportM001: CommandReport
    {
        public string CARRIER { get; set; }

        public string SCHEDULE { get; set; }

        public string OPER { get; set;}

        public string RECIPE { get; set; }

        public string ORIGIN { get; set; }

        public string DESTINATION { get; set; }

        public string QTIME { get; set; }

        public string CYCLETIME { get; set; }
    }
}
