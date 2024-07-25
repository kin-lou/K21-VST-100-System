using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.Attributes
{
    public class ReportCommandNameAttributes
    {
        public string CMD_NO { get; set; }

        public string CMD_NAME { get; set; }

        public string STATION { get; set; }

        public string CARRIER { get; set; }

        public string REJECT_CODE { get; set; }

        public string REJECT_MESSAGE { get; set; }
    }
}
