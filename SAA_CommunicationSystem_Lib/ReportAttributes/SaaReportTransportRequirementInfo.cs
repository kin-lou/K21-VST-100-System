using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReportAttributes
{
    public class SaaReportTransportRequirementInfo : SaaReport
    {
        public List<RequirementInfo> ListRequirementInfo = new List<RequirementInfo>();
    }
}
