using Newtonsoft.Json;
using SAA_CommunicationSystem_Lib.ReceivAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SAA_CommunicationSystem_Lib.Controllers
{
    public class WebApiController : ApiController
    {
        private string commandcontent = string.Empty;
        private SaaReceiv saareceivsts = new SaaReceiv();
        private SaaReceiv saareceivmode = new SaaReceiv();
        private SaaReceivClear receivclear = new SaaReceivClear();
        private SaaReceivAlarm receivalarm = new SaaReceivAlarm();
        private SaaReceivGoWhere receivgowhere = new SaaReceivGoWhere();
        private SaaReceivDeviceSts receivdevicests = new SaaReceivDeviceSts();
        private SaaReceivStorageInfo receivstorageinfo = new SaaReceivStorageInfo();

        private SaaReceivPurpose receivdeviceloadin;
        private SaaReceivPurpose receivstorageloadin;
        private SaaReceivPurpose receivstorageloadout;
        private SaaReceivPurpose receivrejectdown;
        private SaaReceivPurpose receivportloadout;
        private SaaReceivCancel receivcmdcancel;
        private SaaReceivCancel receiveqpcmdcancel;

        #region [===接受LCS上報訊息===]
        [Route("GetStorageMessage")]
        [HttpPost]
        public string GetStorageMessage([FromBody] StorageMessage data)
        {
            try
            {
                SAA_Database.LogMessage($"【接收】接收到LCS上報{data.Message}");
                string[] commandmessage = data.Message.Split(new char[] { '=', '\r', '\n', '{', '}', }, StringSplitOptions.RemoveEmptyEntries);
                string[] dataAry = commandmessage[0].Trim().Split(new string[] { "," }, StringSplitOptions.None);
                if (dataAry.Length > 0)
                {
                    string cmdname = dataAry[0];
                    if (Enum.IsDefined(typeof(SAA_Database.CommandName), cmdname))
                    {
                        switch ((SAA_Database.CommandName)Enum.Parse(typeof(SAA_Database.CommandName), cmdname))
                        {
                            case SAA_Database.CommandName.ErrorHappen:
                            case SAA_Database.CommandName.ErrorEnd:
                            case SAA_Database.CommandName.WarningHappen:
                            case SAA_Database.CommandName.WarningEnd:
                                #region [===異常/警告上報===]
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            receivalarm.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            receivalarm.Station = dataAry[i];
                                            break;
                                        case 2:
                                            receivalarm.EQP = dataAry[i];
                                            break;
                                        case 3:
                                            receivalarm.Code = dataAry[i];
                                            break;
                                        case 4:
                                            receivalarm.Msg = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                var alarmcommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivalarm.Station, receivalarm.CMD);
                                if (alarmcommanddata.Rows.Count!=0)
                                {
                                    SaaReceivCommandName receivcommand = new SaaReceivCommandName
                                    {
                                        CommandNo = alarmcommanddata.Rows[0][SAA_DatabaseEnum.SC_REPORT_COMMAND_NAME.REPORT_COMMAND_NO.ToString()].ToString(),
                                        CommandName = alarmcommanddata.Rows[0][SAA_DatabaseEnum.SC_REPORT_COMMAND_NAME.REPORT_COMMAND_NAME.ToString()].ToString(),
                                    };
                                    var equipmentzonedata = SAA_Database.SaaSql.GetScEquipmentZone(SAA_Database.configattributes.SaaEquipmentNo, receivalarm.Station);
                                    receivcommand.CommandStation = equipmentzonedata.Rows.Count != 0 ? equipmentzonedata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_ZONE.REPORT_NAME.ToString()].ToString() : string.Empty;
                                    for (int i = 0; i < SAA_Database.reportcommand.AlarmReportAry.Count; i++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandNo;
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandName;
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandStation;
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivalarm.Code;
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivalarm.Msg;
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicAlarmReport);
                                }
                                else
                                {
                                    SAA_Database.LogMessage($"【查無定義指令】查無定義指令無法接收。", SAA_Database.LogType.Error);
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.ClearStorage:
                                #region [===清除上報===]
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            receivclear.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            receivclear.Station = dataAry[i];
                                            break;
                                        case 2:
                                            receivclear.ID = dataAry[i];
                                            break;
                                        case 3:
                                            receivclear.Loc = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }


                                #endregion
                                break;
                            case SAA_Database.CommandName.GoWhere:
                                #region [===卡匣詢問上報===]
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            receivgowhere.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            receivgowhere.Station = dataAry[i];
                                            break;
                                        case 2:
                                            receivgowhere.ID = dataAry[i];
                                            break;
                                        case 3:
                                            receivgowhere.From = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.DeviceSts_1:
                            case SAA_Database.CommandName.DeviceSts_2:
                            case SAA_Database.CommandName.DeviceSts_3:
                            case SAA_Database.CommandName.DeviceSts_4:
                            case SAA_Database.CommandName.DeviceSts_5:
                                #region [===機台狀態上報===]
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            receivdevicests.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            receivdevicests.Station = dataAry[i];
                                            break;
                                        case 2:
                                            receivdevicests.Status = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.DeviceLoadIn:
                                receivdeviceloadin = GetReceivPurpose(dataAry);
                                break;
                            case SAA_Database.CommandName.StorageLoadIn:
                                receivstorageloadin = GetReceivPurpose(dataAry);
                                break;
                            case SAA_Database.CommandName.StorageLoadOut:
                                receivstorageloadout = GetReceivPurpose(dataAry);
                                break;
                            case SAA_Database.CommandName.RejectDown:
                                receivrejectdown = GetReceivPurpose(dataAry);
                                break;
                            case SAA_Database.CommandName.PortLoadOut:
                                receivportloadout = GetReceivPurpose(dataAry);
                                break;
                            case SAA_Database.CommandName.StorageInfo:
                                #region [===儲格狀態===]
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            receivstorageinfo.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            receivstorageinfo.Station = dataAry[i];
                                            break;
                                        case 2:
                                            receivstorageinfo.Sts = dataAry[i];
                                            break;
                                        case 3:
                                            receivstorageinfo.Loc = dataAry[i];
                                            break;
                                        case 4:
                                            receivstorageinfo.WareCount = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.CmdCancel:
                                receivcmdcancel = GetReceivCancel(dataAry);
                                break;
                            case SAA_Database.CommandName.EqpCmdCancel:
                                receiveqpcmdcancel = GetReceivCancel(dataAry);
                                break;
                            case SAA_Database.CommandName.LCS_STS_OFFLINE:
                            case SAA_Database.CommandName.LCS_STS_LOCAL_ONLINE:
                            case SAA_Database.CommandName.LCS_STS_REMOTE_ONLINE:
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            saareceivsts.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            saareceivsts.Station = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case SAA_Database.CommandName.LCS_MODE_InOut:
                            case SAA_Database.CommandName.LCS_MODE_In:
                            case SAA_Database.CommandName.LCS_MODE_Out:
                                for (int i = 0; i < dataAry.Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            saareceivmode.CMD = dataAry[i];
                                            break;
                                        case 1:
                                            saareceivmode.Station = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case SAA_Database.CommandName.RGV_1_MODE_In:
                            case SAA_Database.CommandName.RGV_1_MODE_Out:
                                break;
                            case SAA_Database.CommandName.RGV_2_MODE_In:
                            case SAA_Database.CommandName.RGV_2_MODE_Out:
                                break;
                            case SAA_Database.CommandName.RGV_1_STS_ON:
                            case SAA_Database.CommandName.RGV_1_STS_OFF:
                                break; ;
                            case SAA_Database.CommandName.RGV_2_STS_ON:
                            case SAA_Database.CommandName.RGV_2_STS_OFF:
                                break;
                            case SAA_Database.CommandName.CarrierArrivedPlatform:
                                break;
                        }
                    }
                }
                return SAA_Database.configattributes.WebApiResultOK;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return SAA_Database.configattributes.WebApiResultFAIL;
            }
        }
        #endregion

        private SaaReceivPurpose GetReceivPurpose(string[] dataAry)
        {
            try
            {
                SaaReceivPurpose saareceivpurpose = new SaaReceivPurpose();
                for (int i = 0; i < dataAry.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            saareceivpurpose.CMD = dataAry[i];
                            break;
                        case 1:
                            saareceivpurpose.Station = dataAry[i];
                            break;
                        case 2:
                            saareceivpurpose.WhereCarrier = dataAry[i];
                            break;
                        case 3:
                            //saareceivpurpose.ID = dataAry[i].Contains(SAA_Database.configattributes.ReaderError) ? SAA_Database.configattributes.ReaderError : dataAry[i];
                            break;
                        case 4:
                            //saareceivpurpose.NO = (dataAry[i] == SAA_Database.configattributes.Empty || dataAry[i] == string.Empty) ? SAA_Database.configattributes.NA : dataAry[i];
                            break;
                        case 5:
                            saareceivpurpose.From = dataAry[i];
                            break;
                        case 6:
                            saareceivpurpose.To = dataAry[i];
                            break;
                        case 7:
                            saareceivpurpose.Direction = dataAry[i];
                            break;
                        case 8:
                            saareceivpurpose.WareCount = dataAry[i];
                            break;
                        case 9:
                            saareceivpurpose.RejectInfo = dataAry[i];
                            break;
                        default:
                            break;
                    }
                }
                return saareceivpurpose;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }

        private SaaReceivCancel GetReceivCancel(string[] dataAry)
        {
            try
            {
                SaaReceivCancel saareceivcancel = new SaaReceivCancel();
                for (int i = 0; i < dataAry.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            saareceivcancel.CMD = dataAry[i];
                            break;
                        case 1:
                            saareceivcancel.Station = dataAry[i];
                            break;
                        case 2:
                            saareceivcancel.ID = dataAry[i];
                            break;
                        case 3:
                            saareceivcancel.NO = dataAry[i];
                            break;
                        case 4:
                            saareceivcancel.From = dataAry[i];
                            break;
                        case 5:
                            saareceivcancel.To = dataAry[i];
                            break;
                        case 6:
                            saareceivcancel.WareCount = dataAry[i];
                            break;
                        default:
                            break;
                    }
                }
                return saareceivcancel;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }

        private string GetReportCommands(Dictionary<string, string> dicContent)
        {
            try
            {
                return JsonConvert.SerializeObject(dicContent).ToString();
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return string.Empty;
            }
        }
    }
}
