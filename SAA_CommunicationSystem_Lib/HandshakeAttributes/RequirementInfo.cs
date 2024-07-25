using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem_Lib.HandshakeAttributes
{
    public class RequirementInfo
    {
        /// <summary>
        /// 需求類型
        /// </summary>
        public string RequirementType { get; set; }

        /// <summary>
        /// 載體ID
        /// </summary>
        public string CarrierID { get; set; }

        /// <summary>
        /// 起點站名
        /// </summary>
        public string BeginStation { get; set; }

        /// <summary>
        /// 終點站名
        /// </summary>
        public string EndStation { get; set; }
    }
}
