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
        private int dicval;
        public void ReadScCommon()
        {
            try
            {
                var commondb = SAA_Database.SaaSql.GetScCommon(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName);
                foreach (DataRow dr in commondb.Rows)
                {
                    string itemname = dr[SAA_DatabaseEnum.SC_COMMON.ITEM_NAME.ToString()].ToString();
                    string itemvalue= dr[SAA_DatabaseEnum.SC_COMMON.ITEM_VALUE.ToString()].ToString();
                    switch ((SAA_DatabaseEnum.SC_COMMON_ITEM_NAME)Enum.Parse(typeof(SAA_DatabaseEnum.SC_COMMON_ITEM_NAME), itemname))
                    {
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.ReaderError:
                            SAA_Database.SaaCommon.ReaderError = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.Empty:
                            SAA_Database.SaaCommon.Empty = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.NA:
                            SAA_Database.SaaCommon.NA = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.CRANE:
                            SAA_Database.SaaCommon.CRANE = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.ReportCraneName:
                            var device = SAA_Database.SaaSql.GetScDevice(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName, SAA_Database.SaaCommon.CRANE);
                            SAA_Database.SaaCommon.ReportCraneName = device.Rows.Count != 0 ? device.Rows[0][SAA_DatabaseEnum.SC_DEVICE.HOSTDEVICEID.ToString()].ToString() : string.Empty;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.AskCarrier:
                            SAA_Database.SaaCommon.AskCarrier = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.AskResultNo:
                            SAA_Database.SaaCommon.AskResultNo = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.AskResultYes:
                            SAA_Database.SaaCommon.AskResultYes = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_STS_OFFLINE:
                            SAA_Database.SaaCommon.LCS_STS_OFFLINE = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_STS_OFFLINE);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_STS_LOCAL_ONLINE:
                            SAA_Database.SaaCommon.LCS_STS_LOCAL_ONLINE = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_STS_LOCAL_ONLINE);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_STS_REMOTE_ONLINE:
                            SAA_Database.SaaCommon.LCS_STS_REMOTE_ONLINE = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_STS_REMOTE_ONLINE);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_MODE_InOut:
                            SAA_Database.SaaCommon.LCS_MODE_InOut = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_MODE_InOut);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_MODE_In:
                            SAA_Database.SaaCommon.LCS_MODE_In = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_MODE_In);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.LCS_MODE_Out:
                            SAA_Database.SaaCommon.LCS_MODE_Out = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.LCS_MODE_Out);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.DeviceSts_1:
                            SAA_Database.SaaCommon.DeviceSts_1 = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.DeviceSts_1);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.DeviceSts_2:
                            SAA_Database.SaaCommon.DeviceSts_2 = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.DeviceSts_2);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.DeviceSts_3:
                            SAA_Database.SaaCommon.DeviceSts_3 = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.DeviceSts_3);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.DeviceSts_4:
                            SAA_Database.SaaCommon.DeviceSts_4 = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.DeviceSts_4);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.DeviceSts_5:
                            SAA_Database.SaaCommon.DeviceSts_5 = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.DeviceSts_5);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_1_MODE_In:
                            SAA_Database.SaaCommon.RGV_1_MODE_In = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_1_MODE_In);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_1_MODE_Out:
                            SAA_Database.SaaCommon.RGV_1_MODE_Out = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_1_MODE_Out);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_2_MODE_In:
                            SAA_Database.SaaCommon.RGV_2_MODE_In = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_2_MODE_In);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_2_MODE_Out:
                            SAA_Database.SaaCommon.RGV_2_MODE_Out = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_2_MODE_Out);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_1_STS_ON:
                            SAA_Database.SaaCommon.RGV_1_STS_ON = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_1_STS_ON);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_1_STS_OFF:
                            SAA_Database.SaaCommon.RGV_1_STS_OFF = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_1_STS_OFF);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_2_STS_ON:
                            SAA_Database.SaaCommon.RGV_2_STS_ON = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_2_STS_ON);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.RGV_2_STS_OFF:
                            SAA_Database.SaaCommon.RGV_2_STS_OFF = int.Parse(itemvalue);
                            SetDictionary(itemname, SAA_Database.SaaCommon.RGV_2_STS_OFF);
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.Pire1Name:
                            SAA_Database.SaaCommon.Pire1Name = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.Pire2Name:
                            SAA_Database.SaaCommon.Pire2Name = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.PireModeIn:
                            SAA_Database.SaaCommon.PireModeIn = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.PireModOut:
                            SAA_Database.SaaCommon.PireModOut = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.PireStatusOn:
                            SAA_Database.SaaCommon.PireStatusOn = itemvalue;
                            break;
                        case SAA_DatabaseEnum.SC_COMMON_ITEM_NAME.PireStatusOff:
                            SAA_Database.SaaCommon.PireStatusOff = itemvalue;
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

        public void SetDictionary(string key, int value)
        {
            if (SAA_Database.reportcommand.DicCommon.TryGetValue(key, out dicval))
                SAA_Database.reportcommand.DicCommon[key] = value;
            else
                SAA_Database.reportcommand.DicCommon.Add(key, value);
        }
    }
}
