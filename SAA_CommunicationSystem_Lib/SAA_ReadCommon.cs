using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_ReadCommon
    {
        public void ReadScCommon()
        {
            try
            {
                var commondb = SAA_Database.SaaSql.GetScCommon(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName);
                foreach (DataRow dr in commondb.Rows)
                {
                    string itemname = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_NAME.ToString()].ToString();
                    switch ((SAA_DatabaseEnum.SC_COMMON_ITEM_NAME)Enum.Parse(typeof(SAA_DatabaseEnum.SC_COMMON_ITEM_NAME), itemname))
                    {
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.ReaderError:
                            SAA_Database.SaaCommon.ReaderError = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_VALUE.ToString()].ToString();
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.Empty:
                            SAA_Database.SaaCommon.Empty = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_VALUE.ToString()].ToString();
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.NA:
                            SAA_Database.SaaCommon.NA = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_VALUE.ToString()].ToString();
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.CRANE:
                            SAA_Database.SaaCommon.CRANE = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_VALUE.ToString()].ToString();
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.ReportCraneName:
                            var device = SAA_Database.SaaSql.GetScDevice(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName, SAA_Database.SaaCommon.CRANE);
                            SAA_Database.SaaCommon.ReportCraneName = device.Rows.Count != 0 ? device.Rows[0][SAA_DatabaseEnum.SC_DEVICE.HOSTDEVICEID.ToString()].ToString() : string.Empty;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }
    }
}
