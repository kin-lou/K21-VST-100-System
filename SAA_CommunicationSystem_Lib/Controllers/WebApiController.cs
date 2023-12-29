using Newtonsoft.Json;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.ReceivAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace SAA_CommunicationSystem_Lib.Controllers
{
    public class WebApiController : ApiController
    {
        private string commandcontent = string.Empty;
        
        private SaaReceivClear receivclear = new SaaReceivClear();
        private SaaReceivAlarm receivalarm = new SaaReceivAlarm();
        private SaaReceivGoWhere receivgowhere = new SaaReceivGoWhere();
        private SaaReceivDeviceSts receivdevicests = new SaaReceivDeviceSts();
        private SaaReceivStorageInfo receivstorageinfo = new SaaReceivStorageInfo();

        private SaaReceiv saareceivsts;
        private SaaReceiv saareceivmode;
        private SaaReceiv saasettingmode;
        private SaaReceiv saasettingstatus;
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
                                if (alarmcommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(alarmcommanddata, receivalarm.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.AlarmReportAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo} 】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandStation;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandStation}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivalarm.Code;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivalarm.Code}");
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivalarm.Msg;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.InOutLockAry[i]}={receivalarm.Msg}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicAlarmReport);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, string.Empty, receivcommand.CommandNo, commandcontent);
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
                                            receivclear.Loc = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivclear.Station, dataAry[i]);//需轉換為客戶編碼
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                var clearcommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivclear.Station, receivclear.CMD);
                                if (clearcommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(clearcommanddata, receivalarm.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.ClearCacheAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivclear.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivclear.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivclear.Loc;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivclear.Loc}");
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicClearCache);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, string.Empty, receivcommand.CommandNo, commandcontent);
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
                                            receivgowhere.From = dataAry[i];//需轉換為客戶編碼
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                var gowherecommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivgowhere.Station, receivgowhere.CMD);
                                if (gowherecommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(gowherecommanddata, receivgowhere.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.AskCarrierAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicAskCarrier[SAA_Database.reportcommand.AskCarrierAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.AskCarrierAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicAskCarrier[SAA_Database.reportcommand.AskCarrierAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.AskCarrierAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicAskCarrier[SAA_Database.reportcommand.AskCarrierAry[i]] = receivgowhere.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.AskCarrierAry[i]}={receivgowhere.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicAskCarrier[SAA_Database.reportcommand.AskCarrierAry[i]] = receivgowhere.From;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.AskCarrierAry[i]}={receivgowhere.From}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicAskCarrier);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivgowhere.ID, receivcommand.CommandNo, commandcontent);
                                }
                                else
                                {
                                    SAA_Database.LogMessage($"【查無定義指令】查無定義指令無法接收。", SAA_Database.LogType.Error);
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
                                            receivdevicests.Model_Name = dataAry[i];
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                SaaScEquipmentStatus equipmentstatus = new SaaScEquipmentStatus
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = receivdevicests.Station,
                                    EQUIPMENT_STATUS = cmdname,
                                    EQUIPMENT_STATUS_CODE = SAA_Database.reportcommand.DicCommon[cmdname],
                                    STATUS_UPDATE_TIME = SAA_Database.ReadTime(),
                                };
                                SAA_Database.SaaSql.UpdSaaEquipmentStatus(equipmentstatus);
                                #endregion
                                break;
                            case SAA_Database.CommandName.DeviceLoadIn:
                                #region [===平台至手臂===]
                                receivdeviceloadin = GetReceivPurpose(dataAry);
                                if (receivdeviceloadin.WhereCarrier == SAA_Database.SaaCommon.ReportCraneName)
                                {
                                    var deviceloadincommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivdeviceloadin.Station, receivdeviceloadin.CMD);
                                    if (deviceloadincommanddata.Rows.Count != 0)
                                    {
                                        SaaReceivCommandName receivcommand = GetReceivCommandName(deviceloadincommanddata, receivdeviceloadin.Station);
                                        for (int i = 0; i < SAA_Database.reportcommand.CarryInReportAry.Count; i++)
                                        {
                                            int ReportNo = i + 1;
                                            switch (i)
                                            {
                                                case 0:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandNo;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandNo}");
                                                    break;
                                                case 1:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandName;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandName}");
                                                    break;
                                                case 2:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivdeviceloadin.ID;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivdeviceloadin.ID}");
                                                    break;
                                                case 3:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivdeviceloadin.NO;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivdeviceloadin.NO}");
                                                    break;
                                                case 4:
                                                    string hostid = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivdeviceloadin.Station, receivdeviceloadin.From);
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = hostid;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={hostid}");
                                                    break;
                                                case 5:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = SAA_Database.SaaCommon.ReportCraneName;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={SAA_Database.SaaCommon.ReportCraneName}");
                                                    break;
                                                case 6:
                                                    SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivdeviceloadin.WareCount;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivdeviceloadin.WareCount}");
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        commandcontent = GetReportCommands(SAA_Database.reportcommand.DicCarryInReport);
                                        SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivdeviceloadin.ID, receivcommand.CommandNo, commandcontent);
                                    }
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.StorageLoadIn:
                                #region [===手臂至儲格===]
                                receivstorageloadin = GetReceivPurpose(dataAry);
                                var storageloadincommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivstorageloadin.Station, receivstorageloadin.CMD);
                                if (storageloadincommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(storageloadincommanddata, receivstorageloadin.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.CarryInReportAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadin.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadin.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadin.NO;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadin.NO}");
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = SAA_Database.SaaCommon.ReportCraneName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={SAA_Database.SaaCommon.ReportCraneName}");
                                                break;
                                            case 5:
                                                string hostid = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivstorageloadin.Station, receivstorageloadin.To);
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = hostid;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={hostid}");
                                                break;
                                            case 6:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadin.WareCount;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadin.WareCount}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicCarryInReport);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivstorageloadin.ID, receivcommand.CommandNo, commandcontent);
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.StorageLoadOut:
                                #region [===儲格至手臂===]
                                receivstorageloadout = GetReceivPurpose(dataAry);
                                var storageloadoutcommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivstorageloadout.Station, receivstorageloadout.CMD);
                                if (storageloadoutcommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(storageloadoutcommanddata, receivstorageloadout.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.CarryInReportAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadout.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadout.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadout.NO;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadout.NO}");
                                                break;
                                            case 4:
                                                string hostid = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivstorageloadout.Station, receivstorageloadout.From);
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = hostid;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={hostid}");
                                                break;
                                            case 5:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = SAA_Database.SaaCommon.ReportCraneName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={SAA_Database.SaaCommon.ReportCraneName}");
                                                break;
                                            case 6:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivstorageloadout.WareCount;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivstorageloadout.WareCount}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicCarryInReport);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivstorageloadout.ID, receivcommand.CommandNo, commandcontent);
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.PortLoadOut:
                                #region [===手臂至平台===]
                                receivportloadout = GetReceivPurpose(dataAry);
                                var portloadoutcommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivportloadout.Station, receivportloadout.CMD);
                                if (portloadoutcommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(portloadoutcommanddata, receivportloadout.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.CarryInReportAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivportloadout.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivportloadout.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivportloadout.NO;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivportloadout.NO}");
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = SAA_Database.SaaCommon.ReportCraneName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={SAA_Database.SaaCommon.ReportCraneName}");
                                                break;
                                            case 5:
                                                string hostid = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivportloadout.Station, receivportloadout.To);
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = hostid;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={hostid}");
                                                break;
                                            case 6:
                                                SAA_Database.reportcommand.DicCarryInReport[SAA_Database.reportcommand.CarryInReportAry[i]] = receivportloadout.WareCount;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryInReportAry[i]}={receivportloadout.WareCount}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicCarryInReport);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivportloadout.ID, receivcommand.CommandNo, commandcontent);
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.RejectDown:
                                #region [===搬運至Reject區===]
                                receivrejectdown = GetReceivPurpose(dataAry);
                                var rejectdowncommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivrejectdown.Station, receivrejectdown.CMD);
                                if (rejectdowncommanddata.Rows.Count != 0)
                                {
                                    SaaScRejectList saareject = new SaaScRejectList();
                                    var rejecrdata = SAA_Database.SaaSql.GetScRejectMessage(receivrejectdown.RejectInfo);
                                    if (rejecrdata.Rows.Count != 0)
                                    {
                                        saareject.REMOTE_REJECT_CODE = rejecrdata.Rows[0][SAA_DatabaseEnum.SC_REJECT_LIST.REMOTE_REJECT_CODE.ToString()].ToString();
                                        saareject.REMOTE_REJECT_MSG = rejecrdata.Rows[0][SAA_DatabaseEnum.SC_REJECT_LIST.REMOTE_REJECT_MSG.ToString()].ToString();
                                    }
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(rejectdowncommanddata, receivrejectdown.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.CarryRejectAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={receivcommand.CommandName}");
                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = receivrejectdown.ID;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={receivrejectdown.ID}");
                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = receivrejectdown.NO;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={receivrejectdown.NO}");
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = SAA_Database.SaaCommon.ReportCraneName;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={SAA_Database.SaaCommon.ReportCraneName}");
                                                break;
                                            case 5:
                                                string hostid = GetLocationHostId(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), receivrejectdown.Station, receivrejectdown.To);
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = hostid;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={hostid}");
                                                break;
                                            case 6:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = saareject.REMOTE_REJECT_CODE;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={saareject.REMOTE_REJECT_CODE}");
                                                break;
                                            case 7:
                                                SAA_Database.reportcommand.DicCarryReject[SAA_Database.reportcommand.CarryRejectAry[i]] = saareject.REMOTE_REJECT_MSG;
                                                SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.CarryRejectAry[i]}={saareject.REMOTE_REJECT_MSG}");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicCarryReject);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivrejectdown.ID, receivcommand.CommandNo, commandcontent);
                                }
                                #endregion
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
                                var storageInfocommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivstorageinfo.Station, receivstorageinfo.CMD);
                                if (storageInfocommanddata.Rows.Count != 0)
                                {
                                    SaaReceivCommandName receivcommand = GetReceivCommandName(storageInfocommanddata, receivstorageinfo.Station);
                                    for (int i = 0; i < SAA_Database.reportcommand.InOutLockAry.Count; i++)
                                    {
                                        int ReportNo = i + 1;
                                        switch (i)
                                        {
                                            case 0:
                                                SAA_Database.reportcommand.DicInOutLock[SAA_Database.reportcommand.InOutLockAry[i]] = receivcommand.CommandNo;
                                                SAA_Database.LogMessage($"【上報指令】{SAA_Database.reportcommand.InOutLockAry[i]}={receivcommand.CommandNo}");
                                                break;
                                            case 1:
                                                SAA_Database.reportcommand.DicAlarmReport[SAA_Database.reportcommand.AlarmReportAry[i]] = receivcommand.CommandName;
                                                SAA_Database.LogMessage($"【上報指令】{SAA_Database.reportcommand.InOutLockAry[i]}={receivcommand.CommandName}");

                                                break;
                                            case 2:
                                                SAA_Database.reportcommand.DicInOutLock[SAA_Database.reportcommand.InOutLockAry[i]] = receivstorageinfo.Sts;
                                                SAA_Database.LogMessage($"【上報指令】{SAA_Database.reportcommand.InOutLockAry[i]}={receivstorageinfo.Sts}");

                                                break;
                                            case 3:
                                                SAA_Database.reportcommand.DicInOutLock[SAA_Database.reportcommand.InOutLockAry[i]] = receivstorageinfo.Loc;
                                                SAA_Database.LogMessage($"【上報指令】{SAA_Database.reportcommand.InOutLockAry[i]}={receivstorageinfo.Loc}");
                                                break;
                                            case 4:
                                                SAA_Database.reportcommand.DicInOutLock[SAA_Database.reportcommand.InOutLockAry[i]] = receivstorageinfo.WareCount;
                                                SAA_Database.LogMessage($"【上報指令】{SAA_Database.reportcommand.InOutLockAry[i]}={receivstorageinfo.WareCount}");
                                                break;
                                        }
                                    }
                                    commandcontent = GetReportCommands(SAA_Database.reportcommand.DicInOutLock);
                                    SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, string.Empty, receivcommand.CommandNo, commandcontent);
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.CmdCancel:
                                #region [===刪除任務指令===]
                                receivcmdcancel = GetReceivCancel(dataAry);
                                string commanddata = $"{SAA_Database.SaaCommon.AskCarrier},{SAA_Database.configattributes.SaaEquipmentName},{receivcmdcancel.ID}";
                                string returnresultdata = SAA_Database.SaaSendCommandLcs(commanddata);
                                SAA_Database.LogMessage($"詢問ZIP卡匣盒號:{receivcmdcancel.ID}，詢問結果:{returnresultdata}");
                                if (returnresultdata == SAA_Database.SaaCommon.AskResultNo)
                                {
                                    var cmdclearcommanddata = SAA_Database.SaaSql.GetReportCommandName(SAA_Database.configattributes.SaaEquipmentNo, receivcmdcancel.Station, receivcmdcancel.CMD);
                                    if (cmdclearcommanddata.Rows.Count != 0)
                                    {
                                        SaaReceivCommandName receivcommand = GetReceivCommandName(cmdclearcommanddata, receivcmdcancel.Station);
                                        for (int i = 0; i < SAA_Database.reportcommand.ClearCacheAry.Count; i++)
                                        {
                                            int ReportNo = i + 1;
                                            switch (i)
                                            {
                                                case 0:
                                                    SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcommand.CommandNo;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandNo}");
                                                    break;
                                                case 1:
                                                    SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcommand.CommandName;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcommand.CommandName}");
                                                    break;
                                                case 2:
                                                    SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcmdcancel.ID;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcmdcancel.ID}");
                                                    break;
                                                case 3:
                                                    SAA_Database.reportcommand.DicClearCache[SAA_Database.reportcommand.ClearCacheAry[i]] = receivcmdcancel.From;
                                                    SAA_Database.LogMessage($"【上報指令】【{ReportNo}】{SAA_Database.reportcommand.ClearCacheAry[i]}={receivcmdcancel.From}");
                                                    break;
                                            }
                                        }
                                        commandcontent = GetReportCommands(SAA_Database.reportcommand.DicClearCache);
                                        SetSaaDirective(receivcommand.CommandName, receivcommand.CommandStation, receivcmdcancel.ID, receivcommand.CommandNo, commandcontent);
                                    }
                                }
                                else
                                {
                                    SAA_Database.LogMessage($"【上報D001】卡匣:{receivcmdcancel.ID}尚未離開倉儲，不上報D001指令");
                                }
                                #endregion
                                break;
                            case SAA_Database.CommandName.EqpCmdCancel:
                                #region [===設備交握異常===]
                                receiveqpcmdcancel = GetReceivCancel(dataAry);
                                SAA_Database.LogMessage($"【異常取消】卡匣:{receiveqpcmdcancel.ID}，設備交握異常取消任務。", SAA_Database.LogType.Error);
                                #endregion
                                break;
                            case SAA_Database.CommandName.LCS_STS_OFFLINE:
                            case SAA_Database.CommandName.LCS_STS_LOCAL_ONLINE:
                            case SAA_Database.CommandName.LCS_STS_REMOTE_ONLINE:
                                #region [===LCS狀態===]
                                saareceivsts = GetReceiv(dataAry);
                                SaaScEquipmentStatus lcsstatus = new SaaScEquipmentStatus
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = saareceivsts.Station,
                                    LCS_STATUS = cmdname,
                                    LCS_STATUS_CODE = SAA_Database.reportcommand.DicCommon[cmdname],
                                    LCS_UPDATE_TIME = SAA_Database.ReadTime(),
                                };
                                SAA_Database.SaaSql.UpdSaaEquipmentLcsStatus(lcsstatus);
                                #endregion
                                break;
                            case SAA_Database.CommandName.LCS_MODE_InOut:
                            case SAA_Database.CommandName.LCS_MODE_In:
                            case SAA_Database.CommandName.LCS_MODE_Out:
                                #region [===設備模式===]
                                saareceivmode = GetReceiv(dataAry);
                                SaaScEquipmentStatus equipmentmodel = new SaaScEquipmentStatus
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = saareceivmode.Station,
                                    EQUIPMENT_MODEL = cmdname,
                                    EQUIPMENT_MODEL_CODE = SAA_Database.reportcommand.DicCommon[cmdname],
                                    STATUS_UPDATE_TIME = SAA_Database.ReadTime(),
                                };
                                SAA_Database.SaaSql.UpdSaaEquipmentModel(equipmentmodel);
                                #endregion
                                break;
                            case SAA_Database.CommandName.RGV_1_MODE_In:
                            case SAA_Database.CommandName.RGV_1_MODE_Out:
                            case SAA_Database.CommandName.RGV_2_MODE_In:
                            case SAA_Database.CommandName.RGV_2_MODE_Out:
                                #region [===更換碼頭機構模式===]
                                saasettingmode = GetReceiv(dataAry);
                                SaaScLocationSetting LocationSettingMode = new SaaScLocationSetting
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = saasettingmode.Station,
                                    HOSTID = cmdname.Contains(SAA_Database.SaaCommon.Pire1Name) ? SAA_Database.SaaCommon.Pire1Name : SAA_Database.SaaCommon.Pire2Name,
                                    LOCATIONMODE = SAA_Database.reportcommand.DicCommon[cmdname],
                                };
                                SAA_Database.SaaSql.UpdScLocationSettingMode(LocationSettingMode); 
                                #endregion
                                break;
                            case SAA_Database.CommandName.RGV_1_STS_ON:
                            case SAA_Database.CommandName.RGV_2_STS_ON:
                            case SAA_Database.CommandName.RGV_1_STS_OFF:
                            case SAA_Database.CommandName.RGV_2_STS_OFF:
                                #region [===更換碼頭機構狀態===]
                                saasettingstatus = GetReceiv(dataAry);
                                SaaScLocationSetting LocationSettingStatus = new SaaScLocationSetting
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = saasettingstatus.Station,
                                    HOSTID = cmdname.Contains(SAA_Database.SaaCommon.Pire1Name) ? SAA_Database.SaaCommon.Pire1Name : SAA_Database.SaaCommon.Pire2Name,
                                    LOCATIONSTATUS = SAA_Database.reportcommand.DicCommon[cmdname],
                                };
                                SAA_Database.SaaSql.UpdScLocationSettingStatus(LocationSettingStatus); 
                                #endregion
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

        #region [===手臂搬運===]
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
                            saareceivpurpose.ID = dataAry[i].Contains(SAA_Database.SaaCommon.ReaderError) ? SAA_Database.SaaCommon.ReaderError : dataAry[i];
                            break;
                        case 4:
                            saareceivpurpose.NO = (dataAry[i] == SAA_Database.SaaCommon.Empty || dataAry[i] == string.Empty) ? SAA_Database.SaaCommon.NA : dataAry[i];
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
        #endregion

        #region [===清除上報===]
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
        #endregion

        #region [===讀取LCS設備狀態===]
        public SaaReceiv GetReceiv(string[] dataAry)
        {
            try
            {
                SaaReceiv saareceiv = new SaaReceiv();
                for (int i = 0; i < dataAry.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            saareceiv.CMD = dataAry[i];
                            break;
                        case 1:
                            saareceiv.Station = dataAry[i];
                            break;
                    }
                }
                return saareceiv;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        } 
        #endregion

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

        #region [===查詢指令名稱===]
        public SaaReceivCommandName GetReceivCommandName(DataTable db, string station)
        {
            lock (this)
            {
                SaaReceivCommandName receivcommand = new SaaReceivCommandName
                {
                    CommandNo = db.Rows[0][SAA_DatabaseEnum.SC_REPORT_COMMAND_NAME.REPORT_COMMAND_NO.ToString()].ToString(),
                    CommandName = db.Rows[0][SAA_DatabaseEnum.SC_REPORT_COMMAND_NAME.REPORT_COMMAND_NAME.ToString()].ToString(),
                };
                var equipmentzonedata = SAA_Database.SaaSql.GetScEquipmentZone(SAA_Database.configattributes.SaaEquipmentNo, station);
                receivcommand.CommandStation = equipmentzonedata.Rows.Count != 0 ? equipmentzonedata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_ZONE.REPORT_NAME.ToString()].ToString() : string.Empty;
                return receivcommand;
            }
        }
        #endregion

        public bool ScDirectiveCount(string saaequipmentno, string commandname, string commandtext, SAA_DatabaseEnum.ReportSource reportSource)
        {
            if (SAA_Database.SaaSql.GetScDirective(saaequipmentno, commandname, commandtext, reportSource.ToString()).Rows.Count == 0)
                return true;
            return false;
        }

        #region [===新增Directive資料===]
        /// <summary>
        /// 新增Directive資料
        /// </summary>
        /// <param name="commandname"></param>
        /// <param name="commandstation"></param>
        /// <param name="carrierid"></param>
        /// <param name="commandno"></param>
        public void SetSaaDirective(string commandname, string commandstation, string carrierid, string commandno, string commandcontent, SAA_DatabaseEnum.ReportSource reportsource = SAA_DatabaseEnum.ReportSource.LCS)
        {
            if (ScDirectiveCount(SAA_Database.configattributes.SaaEquipmentNo, commandname, commandcontent, reportsource))
            {
                SaaScReportInadx reportInadx = new SaaScReportInadx()
                {
                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                    MODEL_NAME = commandstation,
                    REPORT_NAME = SAA_DatabaseEnum.IndexTableName.SC_DIRECTIVE.ToString(),
                };
                SaaScDirective directive = new SaaScDirective()
                {
                    TASKDATETIME = SAA_Database.ReadTime(),
                    SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                    COMMANDON = SAA_Database.ReadRequorIndex(reportInadx).ToString(),
                    STATION = reportInadx.MODEL_NAME,
                    CARRIERID = carrierid,
                    COMMANDID = commandno,
                    COMMANDTEXT = commandcontent,
                    SOURCE = reportsource.ToString(),
                };
                SAA_Database.SaaSql.SetScDirective(directive);
                SAA_Database.LogMessage($"【新增指令】新增資料至SC_SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_Id:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
            }
            else
            {
                SAA_Database.LogMessage($"【指令相同】已有相同指令無法新增。", SAA_Database.LogType.Error);
            }
        } 
        #endregion

        public string GetLocationHostId(int setno, string modelname, string locationid)
        {
            try
            {
                var locationdata = SAA_Database.SaaSql.GetScLocationsetting(setno, modelname, locationid);
                return locationdata.Rows.Count != 0 ? locationdata.Rows[0][SAA_DatabaseEnum.SC_LOCATIONSETTING.HOSTID.ToString()].ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return string.Empty;
            }
        }
    }
}
