using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.ReceivLiftAttributes
{
    public class SaaEquipmentRequirementType
    {
        /// <summary>
        /// 實要車需求
        /// </summary>
        public List<string> Take_out_Carrier { get; set; }= new List<string>();

        /// <summary>
        /// 要空盒需求
        /// </summary>
        public List<string> Take_In_EmptyCarrier { get; set; } = new List<string>();

        /// <summary>
        /// 出空盒需求
        /// </summary>
        public List<string> Take_out_EmptyCarrier { get; set; } = new List<string>();
    }
}
