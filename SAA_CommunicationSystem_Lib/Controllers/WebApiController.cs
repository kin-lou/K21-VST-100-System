using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Layouts;
using SAA_CommunicationSystem_Lib.Attributes;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using SAA_CommunicationSystem_Lib.ReceivAttributes;
using SAA_CommunicationSystem_Lib.ReceivLiftAttributes;
using SAA_CommunicationSystem_Lib.ReportAttributes;
using SAA_CommunicationSystem_Lib.ReportCommandAttributes;
using SAA_CommunicationSystem_Lib.SendAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace SAA_CommunicationSystem_Lib.Controllers
{
    public class WebApiController : ApiController
    {
        private string commandcontent = string.Empty;
        private SaaReportResult saareportresult = new SaaReportResult();
        private ReportCommandNameAttributes reportcommandname = new ReportCommandNameAttributes();
        private SaaEquipmentRequirementType equipmentrequirementtype = new SaaEquipmentRequirementType();
        public SaaReceivStorageInfo receivstorageinfo = new SaaReceivStorageInfo();

        /*===============================Web Api接收地方=======================================*/
        #region [===接收LIFT上報訊息===]
        [Route("GetLiftMessage")]
        [HttpPost]
        public string GetLiftMessage([FromBody] SaaLiftReceive data)
        {
            try
            {
                if (data != null)
                {
                    SAA_Database.LogMessage($"【{data.Statiom_Name}】【接收】LIFT上報站點:{data.Statiom_Name}指令名稱:{data.CommandName}");
                    if (data.CommandName != string.Empty)
                    {
                        if (Enum.IsDefined(typeof(SAA_DatabaseEnum.LiftCommandName), data.CommandName))
                        {
                            switch ((SAA_DatabaseEnum.LiftCommandName)Enum.Parse(typeof(SAA_DatabaseEnum.LiftCommandName), data.CommandName))
                            {
                                case SAA_DatabaseEnum.LiftCommandName.EquipmentStatus:
                                    SaaRequestEquipmentStatus RequestEquipmentStatus = new SaaRequestEquipmentStatus
                                    {
                                        StationID = data.Statiom_Name,
                                        Time = SAA_Database.ReadTeid(),
                                    };
                                    RequestEquipmentStatus.TEID = $"{RequestEquipmentStatus.StationID}_{RequestEquipmentStatus.Time}";
                                    SendWebApiEquipmentStatusiLIS(RequestEquipmentStatus);
                                    break;
                                case SAA_DatabaseEnum.LiftCommandName.EquipmnetHardwareInfo:
                                    var hardwareinfodata = SAA_Database.SaaSql.GetScEquipmnetHardwareInfo();
                                    if (hardwareinfodata != null)
                                    {
                                        if (hardwareinfodata.Rows.Count != 0)
                                        {
                                            string StationID = hardwareinfodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMNET_HARDWARE_INFO.STATION_NAME.ToString()].ToString();
                                            string Time = hardwareinfodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMNET_HARDWARE_INFO.EQUIPMNET_TIME.ToString()].ToString();
                                            string TEID = hardwareinfodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMNET_HARDWARE_INFO.EQUIPMNET_TEID.ToString()].ToString();
                                            List<HardwareInfo> requirementinfo = new List<HardwareInfo>();
                                            List<CarrierInfo> CarrierInfolist = new List<CarrierInfo>();
                                            var settinginfodata = SAA_Database.SaaSql.GetScLocationSettingInfoiLIS(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName, StationID);
                                            foreach (DataRow dr in settinginfodata.Rows)
                                            {
                                                SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo()
                                                {
                                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo.ToString()),
                                                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                                                    STATIOM_NAME = StationID,
                                                    CARRIERID = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.CARRIERID.ToString()].ToString(),
                                                };
                                                HardwareInfo info = new HardwareInfo
                                                {
                                                    HardwareID = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.HOSTID.ToString()].ToString(),
                                                    CarrierID = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.CARRIERID.ToString()].ToString(),
                                                    HardwareType = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.LOCATIONTYPE.ToString()].ToString(),
                                                    UsingFlag = "True"
                                                };
                                                requirementinfo.Add(info);

                                                var infodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(EquipmentCarrierInfo);
                                                CarrierInfo carrierinfo = new CarrierInfo
                                                {
                                                    CarrierID = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.CARRIERID.ToString()].ToString(),
                                                    CarrierType = "Normal",
                                                    Schedule = dr[SAA_DatabaseEnum.SC_LOCATIONSETTING.PARTNO.ToString()].ToString(),
                                                    Rotation = "0",
                                                    Flip = "0",
                                                    CarrierState = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.CARRIERSTATE.ToString()].ToString()) ? SAA_DatabaseEnum.CarrierState.Unknow.ToString() : infodata.Rows[0]["CARRIERSTATE"].ToString() : SAA_DatabaseEnum.CarrierState.Unknow.ToString(),
                                                    DestinationType = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.DESTINATIONTYPE.ToString()].ToString()) ? SAA_DatabaseEnum.DestinationType.Buffer.ToString() : infodata.Rows[0]["DESTINATIONTYPE"].ToString() : SAA_DatabaseEnum.DestinationType.Buffer.ToString(),
                                                    Qtime = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.QTIME.ToString()].ToString() : string.Empty,
                                                    Cycletime = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.CYCLETIME.ToString()].ToString() : string.Empty,//CARRIERSTATE DESTINATIONTYPE
                                                    Oper = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.OPER.ToString()].ToString() : string.Empty,
                                                    Recipe = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.RECIPE.ToString()].ToString() : string.Empty,
                                                    RejectCode = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.REJECT_CODE.ToString()].ToString() : string.Empty,
                                                    RejectMessage = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.REJECT_MESSAGE.ToString()].ToString() : string.Empty,
                                                };
                                                CarrierInfolist.Add(carrierinfo);
                                            }
                                            WebApiSendTransportEquipmentHardwareInfo(StationID, Time, TEID, requirementinfo, CarrierInfolist);
                                            SaaScEquipmnetHardwareInfo equipmnethardwareinfo = new SaaScEquipmnetHardwareInfo
                                            {
                                                STATION_NAME = StationID,
                                                EQUIPMNET_TEID = TEID,
                                                EQUIPMNET_TIME = Time,
                                                SENDFLAG = SAA_DatabaseEnum.SendFlag.Y.ToString(),
                                            };
                                            SAA_Database.SaaSql.UpdScEquipmnetHardwareInfo(equipmnethardwareinfo);
                                        }
                                    }
                                    break;
                                case SAA_DatabaseEnum.LiftCommandName.EquipmentLiftE84PlcHandshakeInfo:
                                    var lifte84plchandshakeinfodata = SAA_Database.SaaSql.GetScLiftE84PlcHandshakeInfo();
                                    if (lifte84plchandshakeinfodata.Rows.Count != 0)
                                    {
                                        foreach (DataRow item in lifte84plchandshakeinfodata.Rows)
                                        {
                                            string StationID = item["STATION_NAME"].ToString();
                                            string Time = item["EQUIPMNET_TIME"].ToString();
                                            string TEID = item["EQUIPMNET_TEID"].ToString();
                                            SaaScLiftE84iLisPlc LiftE84Plc = new SaaScLiftE84iLisPlc
                                            {
                                                TASKDATETIME = SAA_Database.ReadTime(),
                                                STATION_NAME = StationID,
                                                SHUTTLEID = string.Empty,
                                                COMMANDID = TEID,
                                                CARRIERID = string.Empty,
                                                CS_0 = 0,
                                                TR_REQ = 0,
                                                AM_AVBL = 0,
                                                BUSY = 0,
                                                COMPT = 0,
                                                CONT = 0,
                                                CS_1 = 0,
                                                READY = 0,
                                                VALID = 0,
                                                ES = 0,
                                                HOA_VBL = 0,
                                                L_REQ = 0,
                                                Mode = string.Empty,
                                                U_REQ = 0,
                                                VA = 0,
                                                VS_0 = 0,
                                                VS_1 = 0,
                                                SELECT = 0,
                                                RESULT = SAA_DatabaseEnum.SendFlag.Y.ToString(),
                                            };
                                            Dictionary<string, object> lifte84dic = new Dictionary<string, object>
                                       {
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.TASKDATETIME.ToString(), LiftE84Plc.TASKDATETIME},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.STATION_NAME.ToString(), LiftE84Plc.STATION_NAME},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.SHUTTLEID.ToString(), LiftE84Plc.SHUTTLEID},
                                            { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.CARRIERID.ToString(), LiftE84Plc.CARRIERID},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.COMMANDID.ToString(), LiftE84Plc.COMMANDID},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.CS_0.ToString(), LiftE84Plc.CS_0},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.AM_AVBL.ToString(), LiftE84Plc.AM_AVBL},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.TR_REQ.ToString(), LiftE84Plc.TR_REQ},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.BUSY.ToString(), LiftE84Plc.BUSY},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.COMPT.ToString(), LiftE84Plc.COMPT},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.CONT.ToString(), LiftE84Plc.CONT},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.CS_1.ToString(), LiftE84Plc.CS_1},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.READY.ToString(), LiftE84Plc.READY},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.VALID.ToString(), LiftE84Plc.VALID},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.ES.ToString(), LiftE84Plc.ES},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.HOA_VBL.ToString(), LiftE84Plc.HOA_VBL},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.L_REQ.ToString(), LiftE84Plc.L_REQ},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.Mode.ToString(), LiftE84Plc.Mode},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.U_REQ.ToString(), LiftE84Plc.U_REQ},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.VS_0.ToString(), LiftE84Plc.VS_0},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.VS_1.ToString(), LiftE84Plc.VS_1},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.SELECT.ToString(), LiftE84Plc.SELECT},
                                           { SAA_DatabaseEnum.SC_LIFT_E84PC_STATSUS.RESULT.ToString(), LiftE84Plc.RESULT},
                                       };
                                            string lifte84commandcontent = JsonConvert.SerializeObject(lifte84dic);
                                            while (true)
                                            {
                                                SAA_Database.LogMessage($"【{data.Statiom_Name}】【{SAA_DatabaseEnum.SendWebApiCommandName.SaaEquipmentMonitorE84PlcRendStart}】【傳送E84訊號】傳送E84訊號至LIFT，內容:{lifte84commandcontent}");
                                                string result = SAA_Database.SaaSendCommandLift(lifte84commandcontent, SAA_DatabaseEnum.SendWebApiCommandName.SaaEquipmentMonitorE84PlcRendStart.ToString());
                                                SAA_Database.LogMessage($"【{data.Statiom_Name}】LIFT回傳結果:{result}");
                                                if (result == SAA_Database.configattributes.WebApiResultOK)
                                                    break;
                                                else
                                                    SAA_Database.LogMessage("LIFT回傳結果不為OK，重新傳送");
                                                Thread.Sleep(100);
                                            }


                                            var plcdata = SAA_Database.SaaSql.GetLiftE84Status(item["STATION_NAME"].ToString());
                                            if (plcdata.Rows.Count != 0)
                                            {
                                                foreach (DataRow dr in plcdata.Rows)
                                                {
                                                    SaaScLiftE84Plc lifte84plc = new SaaScLiftE84Plc
                                                    {
                                                        TASKDATETIME = dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.TASKDATETIME.ToString()].ToString(),
                                                        STATION_NAME = dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.STATION_NAME.ToString()].ToString(),
                                                        SHUTTLEID = dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.SHUTTLEID.ToString()].ToString(),
                                                        COMMANDID = dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.COMMANDID.ToString()].ToString(),
                                                        CARRIERID = dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.CARRIERID.ToString()].ToString(),
                                                        CS_0 = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.CS_0.ToString()].ToString()),
                                                        Valid = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.Valid.ToString()].ToString()),
                                                        TR_REQ = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.TR_REQ.ToString()].ToString()),
                                                        Busy = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.Busy.ToString()].ToString()),
                                                        Complete = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.Complete.ToString()].ToString()),
                                                        Continue = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.Continue.ToString()].ToString()),
                                                        SELECT = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.SELECT.ToString()].ToString()),
                                                        AM_AVBL = int.Parse(dr[SAA_DatabaseEnum.SC_LIFT_E84PLC.AM_AVBL.ToString()].ToString()),
                                                    };

                                                    Handshake PlcHandshake = new Handshake
                                                    {
                                                        Mode = SAA_DatabaseEnum.Mode.ActiveEquipment.ToString(),
                                                        VALID = lifte84plc.Valid == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        CS_0 = lifte84plc.CS_0 == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        CS_1 = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        TR_REQ = lifte84plc.TR_REQ == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        L_REQ = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        U_REQ = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        READY = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        BUSY = lifte84plc.Busy == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        COMPT = lifte84plc.Complete == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        CONT = lifte84plc.Continue == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        HO_AVBL = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        ES = lifte84plc.SELECT == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                        AM_AVBL = lifte84plc.AM_AVBL == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                                                    };

                                                    SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo()
                                                    {
                                                        SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo.ToString()),
                                                        MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                                                        STATIOM_NAME = lifte84plc.STATION_NAME,
                                                        CARRIERID = lifte84plc.CARRIERID,
                                                    };

                                                    CarrierInfo carrierInfo = new CarrierInfo
                                                    {
                                                        CarrierID = string.Empty,
                                                        CarrierType = SAA_DatabaseEnum.CarrierType.Normal.ToString(),
                                                        Schedule = string.Empty,
                                                        Flip = "0",
                                                        Rotation = "0",
                                                        CarrierState = SAA_DatabaseEnum.CarrierState.Unknow.ToString(),
                                                        DestinationType = SAA_DatabaseEnum.DestinationType.Unknow.ToString(),
                                                        Qtime = string.Empty,
                                                        Cycletime = string.Empty,
                                                        Oper = string.Empty,
                                                        Recipe = string.Empty,
                                                        RejectCode = string.Empty,
                                                        RejectMessage = string.Empty,
                                                    };

                                                    SaaReportHandshakeCarrierTransport ReportHandshakeCarrierTransport = new SaaReportHandshakeCarrierTransport
                                                    {
                                                        CarrierInfo = carrierInfo,
                                                        Handshake = PlcHandshake,
                                                        Time = SAA_Database.ReadTime(),
                                                        HandsHakeType = SAA_DatabaseEnum.HandshakeType.Report.ToString(),
                                                        ShuttleID = lifte84plc.SHUTTLEID,
                                                        MissionID = lifte84plc.COMMANDID,
                                                        StationID = lifte84plc.STATION_NAME,
                                                    };
                                                    ReportHandshakeCarrierTransport.TEID = $"{ReportHandshakeCarrierTransport.StationID}_{ReportHandshakeCarrierTransport.Time}";
                                                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                                                {
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.StationID.ToString(), ReportHandshakeCarrierTransport.StationID },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.Time.ToString(), ReportHandshakeCarrierTransport.Time },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.TEID.ToString(), ReportHandshakeCarrierTransport.TEID },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.ShuttleID.ToString(), ReportHandshakeCarrierTransport.ShuttleID },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.MissionID.ToString(), ReportHandshakeCarrierTransport.MissionID },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.HandsHakeType.ToString(), ReportHandshakeCarrierTransport.HandsHakeType },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.Handshake.ToString(), ReportHandshakeCarrierTransport.Handshake },
                                                    {SAA_DatabaseEnum.ES_Handshake_CarrierTransport.CarrierInfo.ToString(), ReportHandshakeCarrierTransport.CarrierInfo }
                                                };
                                                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                                                    SAA_Database.LogMessage($"【{ReportHandshakeCarrierTransport.StationID}】【LCS->iLIS】【詢問回覆E84訊號】【通訊傳送】站點:{ReportHandshakeCarrierTransport.StationID}，時間:{ReportHandshakeCarrierTransport.Time}，傳送編號:{ReportHandshakeCarrierTransport.TEID}傳送內容:{commandcontent}");
                                                    string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SAA_DatabaseEnum.SendWebApi.ES_Handshake_CarrierTransport.ToString());//ES_Handshake_CarrierTransport
                                                    SaaReportResult saareportresult = WebApiReportResult(returnresult);
                                                    if (saareportresult != null)
                                                    {
                                                        SAA_Database.LogMessage($"【{ReportHandshakeCarrierTransport.StationID}】【LCS->iLIS】【詢問回覆E84訊號】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                                                        if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
                                                        {
                                                            SaaScLiftE84PlcHandshakeInfo e84plchandshakeinfo = new SaaScLiftE84PlcHandshakeInfo
                                                            {
                                                                STATION_NAME = StationID,
                                                                EQUIPMNET_TEID = TEID,
                                                                EQUIPMNET_TIME = Time,
                                                                SENDFLAG = SAA_DatabaseEnum.SendFlag.Y.ToString(),
                                                            };
                                                            SAA_Database.SaaSql.UpdScEquipmnetPlcHandshakeInfo(e84plchandshakeinfo);
                                                            SAA_Database.SaaSql.DelScLiftE84PlcHandshakeInfo(e84plchandshakeinfo.SENDFLAG);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case SAA_DatabaseEnum.LiftCommandName.EquipmentLiftAlarmList:
                                    var alarmdata = SAA_Database.SaaSql.GetScAlarmCurrent();
                                    if (alarmdata.Rows.Count != 0)
                                    {
                                        foreach (DataRow dr in alarmdata.Rows)
                                        {
                                            var alarmlistdata = SAA_Database.SaaSql.GetScAlarmList(dr["STATION_NAME"].ToString(), dr["ALARM_CODE"].ToString());
                                            if (alarmlistdata.Rows.Count != 0)
                                            {
                                                string cmd_no = string.Empty;
                                                string cmd_name = string.Empty;
                                                if (dr["ALARM_STATUS"].ToString() == "1")
                                                {
                                                    cmd_no = alarmlistdata.Rows[0]["ALARM_TYPE"].ToString() == "ALARM" ? "A001" : "A003";
                                                    cmd_name = alarmlistdata.Rows[0]["ALARM_TYPE"].ToString() == "ALARM" ? "MECHANISM_STOP" : "WARNING_HAPPEN";
                                                }
                                                else
                                                {
                                                    cmd_no = alarmlistdata.Rows[0]["ALARM_TYPE"].ToString() == "ALARM" ? "A002" : "A004";
                                                    cmd_name = alarmlistdata.Rows[0]["ALARM_TYPE"].ToString() == "ALARM" ? "MECHANISM_START" : "WARNING_CLEAR";
                                                }
                                                var stationdata = SAA_Database.SaaSql.GetScDevice(dr["STATION_NAME"].ToString());
                                                string station = stationdata.Rows.Count != 0 ? stationdata.Rows[0]["HOSTDEVICEID"].ToString() : string.Empty;
                                                string devicestatus = stationdata.Rows.Count != 0 ? stationdata.Rows[0]["DEVICESTATUS"].ToString() : string.Empty;//新增判斷設備自動才上報 Michael.Lin新增
                                                if (station != string.Empty)
                                                {
                                                    if (devicestatus == SAA_DatabaseEnum.DEVICESTATUS.Y.ToString())//新增判斷設備自動才上報 Michael.Lin新增
                                                    {
                                                        Dictionary<string, string> Alarmlist = new Dictionary<string, string>
                                                       {
                                                           { "CMD_NO", cmd_no },
                                                           { "CMD_NAME", cmd_name },
                                                           { "STATION", station},
                                                           { "ALARM_CODE", $"{alarmlistdata.Rows[0]["MODEL_NAME"]}{dr["ALARM_CODE"]}"},//MODEL_NAME
                                                           { "ALARM_MESSAGE",alarmlistdata.Rows[0]["ALARM_MSG"].ToString()},
                                                       };
                                                        string commandcontent = JsonConvert.SerializeObject(Alarmlist);
                                                        SaaScDirective directive = new SaaScDirective()
                                                        {
                                                            TASKDATETIME = SAA_Database.ReadTime(),
                                                            SETNO = dr["SETNO"].ToString(),
                                                            COMMANDON = $"{DateTime.Now:ssfff}",
                                                            STATION_NAME = dr["STATION_NAME"].ToString(),
                                                            CARRIERID = string.Empty,
                                                            COMMANDID = cmd_no,
                                                            COMMANDTEXT = commandcontent,
                                                            SOURCE = SAA_DatabaseEnum.ReportSource.LCS.ToString(),
                                                        };
                                                        SAA_Database.SaaSql.SetScDirective(directive);
                                                        SAA_Database.LogMessage($"【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_Id:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                                                        SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
                                                    }

                                                    SaaScAlarmCurrent scalarmcurrent = new SaaScAlarmCurrent
                                                    {
                                                        REPORT_STATUS = SAA_DatabaseEnum.SendFlag.Y.ToString(),
                                                        STATION_NAME = dr["STATION_NAME"].ToString(),
                                                        ALARM_CODE = dr["ALARM_CODE"].ToString(),
                                                    };
                                                    SAA_Database.SaaSql.UpdScAlarmCurrent(scalarmcurrent);
                                                    SaaScAlarmHistory alarmHistory = new SaaScAlarmHistory
                                                    {
                                                        SETNO = dr["SETNO"].ToString(),
                                                        TRN_TIME = SAA_Database.ReadTime(),
                                                        MODEL_NAME = dr["MODEL_NAME"].ToString(),
                                                        STATION_NAME = scalarmcurrent.STATION_NAME,
                                                        REPORT_STATUS = scalarmcurrent.REPORT_STATUS,
                                                        ALARM_CODE = scalarmcurrent.ALARM_CODE,
                                                        ALARM_MAG = alarmlistdata.Rows[0]["ALARM_MSG"].ToString(),
                                                        ALARM_STATUS = dr["ALARM_STATUS"].ToString(),
                                                        ALARM_TYPE = alarmlistdata.Rows[0]["ALARM_TYPE"].ToString(),
                                                        START_TIME = dr["START_TIME"].ToString(),
                                                        END_TIME = dr["END_TIME"].ToString(),
                                                    };
                                                    if (!string.IsNullOrEmpty(alarmHistory.STATION_NAME) && !string.IsNullOrEmpty(alarmHistory.END_TIME))
                                                    {
                                                        SAA_Database.SaaSql.SetScAlarmHistory(alarmHistory);
                                                        SAA_Database.SaaSql.DelScAlarmCurrent(scalarmcurrent);
                                                    }
                                                }
                                                else
                                                {
                                                    SAA_Database.LogMessage($"【{dr["STATION_NAME"]}】Alarm站點為空無法上報");
                                                }
                                            }
                                            else
                                            {
                                                SAA_Database.LogMessage($"【{dr["STATION_NAME"]}】查無此AlarmCode:{dr["ALARM_CODE"].ToString()}");
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        return SAA_Database.configattributes.WebApiResultOK;
                    }
                    return SAA_Database.configattributes.WebApiResultFAIL;
                }
                return SAA_Database.configattributes.WebApiResultFAIL;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return SAA_Database.configattributes.WebApiResultFAIL;
            }
        }
        #endregion

        [Route("SaaEquipmentMonitorE84PlcSendStart")]
        [HttpPost]
        public string SaaEquipmentMonitorE84PlcSendStart([FromBody] SaaScLiftE84iLisPlc data)
        {
            try
            {
                SaaScLiftE84Plc lifte84plc = new SaaScLiftE84Plc
                {
                    TASKDATETIME = data.TASKDATETIME,
                    STATION_NAME = data.STATION_NAME,
                    SHUTTLEID = data.SHUTTLEID,
                    COMMANDID = data.COMMANDID,
                    CARRIERID = data.CARRIERID,
                    CS_0 = data.CS_0,
                    Valid = data.VALID,
                    TR_REQ = data.TR_REQ,
                    Busy = data.BUSY,
                    Complete = data.COMPT,
                    Continue = data.CONT,
                    SELECT = data.SELECT,
                    AM_AVBL = data.AM_AVBL,
                };

                Handshake PlcHandshake = new Handshake
                {
                    Mode = SAA_DatabaseEnum.Mode.ActiveEquipment.ToString(),//,
                    VALID = data.VALID == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    CS_0 = data.CS_0 == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    CS_1 = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    TR_REQ = data.TR_REQ == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    L_REQ = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    U_REQ = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    READY = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    BUSY = data.BUSY == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    COMPT = data.COMPT == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    CONT = data.CONT == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    HO_AVBL = SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    ES = data.SELECT == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                    AM_AVBL = data.AM_AVBL == 1 ? SAA_DatabaseEnum.E84Handshake.True.ToString() : SAA_DatabaseEnum.E84Handshake.False.ToString(),
                };

                SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo()
                {
                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo.ToString()),
                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                    STATIOM_NAME = data.STATION_NAME.ToString(),
                    CARRIERID = data.CARRIERID,
                };

                var infodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(EquipmentCarrierInfo);
                if (infodata.Rows.Count != 0)
                {
                    EquipmentCarrierInfo.PARTNO = infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.PARTNO.ToString()].ToString() != string.Empty ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.PARTNO.ToString()].ToString() : SAA_Database.SaaCommon.NA;
                }
                else
                {
                    EquipmentCarrierInfo.PARTNO = SAA_Database.SaaCommon.NA;
                }

                CarrierInfo carrierInfo = new CarrierInfo
                {
                    CarrierID = data.CARRIERID,
                    CarrierType = SAA_DatabaseEnum.CarrierType.Normal.ToString(),
                    Schedule = EquipmentCarrierInfo.PARTNO,
                    Flip = "0",
                    Rotation = "0",
                    CarrierState = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.CARRIERSTATE.ToString()].ToString()) ? SAA_DatabaseEnum.CarrierState.Unknow.ToString() : infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.CARRIERSTATE.ToString()].ToString() : SAA_DatabaseEnum.CarrierState.Unknow.ToString(),
                    DestinationType = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.DESTINATIONTYPE.ToString()].ToString()) ? SAA_DatabaseEnum.DestinationType.Unknow.ToString() : infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.DESTINATIONTYPE.ToString()].ToString() : SAA_DatabaseEnum.DestinationType.Unknow.ToString(),
                    Qtime = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.QTIME.ToString()].ToString() : string.Empty,
                    Cycletime = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.CYCLETIME.ToString()].ToString() : string.Empty,//CARRIERSTATE DESTINATIONTYPE
                    Oper = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.OPER.ToString()].ToString() : string.Empty,
                    Recipe = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.RECIPE.ToString()].ToString() : string.Empty,
                    RejectCode = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.REJECT_CODE.ToString()].ToString() : string.Empty,
                    RejectMessage = infodata.Rows.Count != 0 ? infodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.REJECT_MESSAGE.ToString()].ToString() : string.Empty,
                };

                SaaReportHandshakeCarrierTransport ReportHandshakeCarrierTransport = new SaaReportHandshakeCarrierTransport
                {
                    CarrierInfo = carrierInfo,
                    Handshake = PlcHandshake,
                    Time = SAA_Database.ReadTime(),
                    HandsHakeType = SAA_DatabaseEnum.HandshakeType.Normal.ToString(),
                    ShuttleID = data.SHUTTLEID,
                    MissionID = data.COMMANDID,
                    StationID = data.STATION_NAME,
                };
                ReportHandshakeCarrierTransport.TEID = $"{ReportHandshakeCarrierTransport.StationID}_{ReportHandshakeCarrierTransport.Time}";
                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                {
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.StationID.ToString(), ReportHandshakeCarrierTransport.StationID },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.Time.ToString(), ReportHandshakeCarrierTransport.Time },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.TEID.ToString(), ReportHandshakeCarrierTransport.TEID },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.ShuttleID.ToString(), ReportHandshakeCarrierTransport.ShuttleID },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.MissionID.ToString(), ReportHandshakeCarrierTransport.MissionID },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.HandsHakeType.ToString(), ReportHandshakeCarrierTransport.HandsHakeType },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.Handshake.ToString(), ReportHandshakeCarrierTransport.Handshake },
                    { SAA_DatabaseEnum.ES_Handshake_CarrierTransport.CarrierInfo.ToString(), ReportHandshakeCarrierTransport.CarrierInfo }
                };
                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                SAA_Database.LogMessage($"【{data.STATION_NAME}】【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Handshake_CarrierTransport}】【通訊傳送】站點:{ReportHandshakeCarrierTransport.StationID}，時間:{ReportHandshakeCarrierTransport.Time}，傳送編號:{ReportHandshakeCarrierTransport.TEID}傳送內容:{commandcontent}");
                string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SAA_DatabaseEnum.SendWebApi.ES_Handshake_CarrierTransport.ToString());//ES_Handshake_CarrierTransport
                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                SAA_Database.LogMessage($"【{data.STATION_NAME}】【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Handshake_CarrierTransport}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                if (saareportresult != null)
                    return SAA_Database.configattributes.WebApiResultOK;
                else
                    return SAA_Database.configattributes.WebApiResultFAIL;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }

        #region [===接收iLIS心跳包===]
        [Route("SE_Report_Alive")]
        [HttpPost]
        public SaaReportResult SE_Report_Alive([FromBody] SaaReportAlive reportalive)
        {
            try
            {
                SaaReportResult saareport = new SaaReportResult
                {
                    StationID = reportalive.StationID,
                    TEID = reportalive.TEID,
                    Time = reportalive.Time,
                    ReturnCode = SAA_Database.SaaCommon.Success,
                    ReturnMessage = string.Empty,
                };
                return saareport;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS轉拋指令===]
        [Route("SE_DataTransport")]
        [HttpPost]
        public SaaReportResult SE_DataTransport([FromBody] SaaRequestDataTransport requestdatatransport)
        {
            try
            {
                if (requestdatatransport != null)
                {
                    SAA_Database.LogMessage($"【{requestdatatransport.StationID}】【接收iLIS】轉拋指令資料:{requestdatatransport.Content}，站點:{requestdatatransport.StationID}");
                    if (IsJsonFormat(requestdatatransport.Content))
                    {
                        if (requestdatatransport.Content != string.Empty)
                        {
                            var devicedata = SAA_Database.SaaSql.GetScDevice(requestdatatransport.StationID);
                            string saaequipmentno = devicedata.Rows.Count != 0 ? devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo;
                            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestdatatransport.Content);
                            foreach (var command in dic)
                            {
                                string mykey = command.Key;
                                if (Enum.IsDefined(typeof(SAA_DatabaseEnum.CommandName), mykey))
                                {
                                    switch ((SAA_DatabaseEnum.CommandName)Enum.Parse(typeof(SAA_DatabaseEnum.CommandName), mykey))
                                    {
                                        case SAA_DatabaseEnum.CommandName.CMD_NO:
                                            reportcommandname.CMD_NO = command.Value;
                                            break;
                                        case SAA_DatabaseEnum.CommandName.CMD_NAME:
                                            reportcommandname.CMD_NAME = command.Value;
                                            break;
                                        case SAA_DatabaseEnum.CommandName.CARRIER:
                                            reportcommandname.CARRIER = command.Value;
                                            break;
                                        case SAA_DatabaseEnum.CommandName.STATION:
                                            reportcommandname.STATION = command.Value;
                                            break;
                                        case SAA_DatabaseEnum.CommandName.REJECT_CODE:
                                            reportcommandname.REJECT_CODE = command.Value;
                                            break;
                                        case SAA_DatabaseEnum.CommandName.REJECT_MESSAGE:
                                            reportcommandname.REJECT_MESSAGE = command.Value;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            SAA_Database.SetSaaDirective(int.Parse(saaequipmentno), requestdatatransport.StationID, reportcommandname.CARRIER, reportcommandname.CMD_NO, requestdatatransport.Content, SAA_DatabaseEnum.ReportSource.iLIS);
                            saareportresult = ReadReportResult(requestdatatransport.StationID, requestdatatransport.TEID, requestdatatransport.Time, SAA_Database.SaaCommon.Success, string.Empty);
                        }
                        else
                        {
                            saareportresult = ReadReportResult(requestdatatransport.StationID, requestdatatransport.TEID, requestdatatransport.Time, SAA_Database.SaaCommon.Fail, "轉拋指令資料為空值不可接收");
                            SAA_Database.LogMessage($"【{requestdatatransport.StationID}】【接收iLIS】轉拋指令資料為空值不可接收", SAA_Database.LogType.Error);
                        }
                    }
                    else
                    {
                        saareportresult = ReadReportResult(requestdatatransport.StationID, requestdatatransport.TEID, requestdatatransport.Time, SAA_Database.SaaCommon.Fail, "轉拋指令資料判斷非Json格式，不可接收");
                        SAA_Database.LogMessage($"【{requestdatatransport.StationID}】【接收iLIS】轉拋指令資料判斷非Json格式，不可接收", SAA_Database.LogType.Error);
                    }
                }
                else
                {
                    saareportresult = ReadReportResult(requestdatatransport.StationID, requestdatatransport.TEID, requestdatatransport.Time, SAA_Database.SaaCommon.Fail, "轉拋指令資料為Null不可接收");
                    SAA_Database.LogMessage($"【{requestdatatransport.StationID}】【接收iLIS】轉拋指令資料為Null不可接收", SAA_Database.LogType.Error);
                }
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS詢問設備狀態===]
        [Route("SE_Request_EquipmentStatus")]
        [HttpPost]
        public SaaReportResult SE_Request_EquipmentStatus([FromBody] SaaRequestEquipmentStatus equipmentstatus)
        {
            try
            {
                if (equipmentstatus != null)
                {
                    saareportresult = ReadReportResult(equipmentstatus.StationID, equipmentstatus.TEID, equipmentstatus.Time, SAA_Database.SaaCommon.Success, string.Empty);
                    SAA_Database.LogMessage($"【{equipmentstatus.StationID}】【接收iLIS】詢問設備狀態->站點{saareportresult.StationID} 時間:{saareportresult.Time} 指令編號:{saareportresult.TEID}");
                    SendWebApiEquipmentStatusiLIS(equipmentstatus);
                }
                else
                {
                    saareportresult = ReadReportResult(equipmentstatus.StationID, equipmentstatus.TEID, equipmentstatus.Time, SAA_Database.SaaCommon.Fail, "詢問設備狀態資料為Null不可接收");
                    SAA_Database.LogMessage($"【{equipmentstatus.StationID}】【接收iLIS】詢問設備狀態資料為Null不可接收", SAA_Database.LogType.Error);
                }
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS詢問設備上所有物料資訊===]
        [Route("SE_Request_EquipmentHardwareInfo")]
        [HttpPost]
        public SaaReportResult SE_Request_EquipmentHardwareInfo([FromBody] SaaRequestEquipmentHardwareInfo requestequipmenthardwareinfo)
        {
            try
            {
                SAA_Database.LogMessage($"【{requestequipmenthardwareinfo.StationID}】【接收iLIST】詢問帳料資訊站點:{requestequipmenthardwareinfo.StationID}");
                var devicedata = SAA_Database.SaaSql.GetScDevice(requestequipmenthardwareinfo.StationID);
                if (devicedata != null)
                {
                    if (devicedata.Rows.Count != 0)
                    {
                        SaaScEquipmnetHardwareInfo HardwareInfo = new SaaScEquipmnetHardwareInfo
                        {
                            SETNO = int.Parse(devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.SETNO.ToString()].ToString()),
                            MODEL_NAME = devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.MODEL_NAME.ToString()].ToString(),
                            STATION_NAME = requestequipmenthardwareinfo.StationID,
                            EQUIPMNET_TIME = requestequipmenthardwareinfo.Time,
                            EQUIPMNET_TEID = requestequipmenthardwareinfo.TEID,
                        };
                        SAA_Database.SaaSql.SetScEquipmnetHardwareInfo(HardwareInfo);
                        saareportresult = ReadReportResult(requestequipmenthardwareinfo.StationID, requestequipmenthardwareinfo.TEID, requestequipmenthardwareinfo.Time, SAA_Database.SaaCommon.Success, string.Empty);

                        SaaLiftReceive saaLift = new SaaLiftReceive
                        {
                            Statiom_Name = requestequipmenthardwareinfo.StationID,
                            CommandName = SAA_DatabaseEnum.LiftCommandName.EquipmnetHardwareInfo.ToString(),
                        };
                        Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                        {
                            { SAA_DatabaseEnum.EquipmentStatusCommand.Statiom_Name.ToString(),  saaLift.Statiom_Name},
                            { SAA_DatabaseEnum.EquipmentStatusCommand.CommandName.ToString(), saaLift.CommandName}
                        };
                        string commandcontent = JsonConvert.SerializeObject(saaLift);
                        string ReportMessage = SAA_Database.SaaSendCommandSystems(commandcontent, SAA_DatabaseEnum.SendWebApiCommandName.GetLiftMessage.ToString());
                        SAA_Database.LogMessage($"【{requestequipmenthardwareinfo.StationID}】【傳送設備】【轉譯程式】自行接收結果:{ReportMessage}");
                    }
                    else
                    {
                        SAA_Database.LogMessage($"【{requestequipmenthardwareinfo.StationID}】站點{requestequipmenthardwareinfo.StationID}資料為空", SAA_Database.LogType.Error);
                    }
                }
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS新增天車卡匣搬運===]
        [Route("SE_Request_EquipmentTransport")]
        [HttpPost]
        public SaaReportResult SE_Request_EquipmentTransport([FromBody] SaaRequestEquipmentTransport equipmenttransport)
        {
            try
            {
                SAA_Database.LogMessage($"【{equipmenttransport.StationID}】【接收iLIS】新增天車卡匣搬運->站點{equipmenttransport.StationID} 時間:{equipmenttransport.Time} 卡匣ID:{equipmenttransport.CarrierID}");
                saareportresult = ReadReportResult(equipmenttransport.StationID, equipmenttransport.TEID, equipmenttransport.Time, SAA_Database.SaaCommon.Success, string.Empty);
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS取得上下貨交握訊號===]
        [Route("SE_Request_Handshake_CarrierTransport")]
        [HttpPost]
        public SaaReportResult SE_Request_Handshake_CarrierTransport([FromBody] SaaRequestHandshakeCarrierTransport requesthandshakecarriertransport)
        {
            try
            {
                SAA_Database.LogMessage($"【{requesthandshakecarriertransport.StationID}】【接收iLIST】詢問E84資訊站點:{requesthandshakecarriertransport.StationID}");
                var devicedata = SAA_Database.SaaSql.GetScDevice(requesthandshakecarriertransport.StationID);
                if (devicedata != null)
                {
                    if (devicedata.Rows.Count != 0)
                    {
                        SaaScLiftE84PlcHandshakeInfo PlcHandshakeInfo = new SaaScLiftE84PlcHandshakeInfo
                        {
                            SETNO = int.Parse(devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.SETNO.ToString()].ToString()),
                            MODEL_NAME = devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.MODEL_NAME.ToString()].ToString(),
                            STATION_NAME = requesthandshakecarriertransport.StationID,
                            EQUIPMNET_TIME = requesthandshakecarriertransport.Time,
                            EQUIPMNET_TEID = requesthandshakecarriertransport.TEID,
                        };
                        //SAA_Database.SaaSql.SetScLiftE84PlcHandshakeInfo(PlcHandshakeInfo);
                        saareportresult = ReadReportResult(requesthandshakecarriertransport.StationID, requesthandshakecarriertransport.TEID, requesthandshakecarriertransport.Time, SAA_Database.SaaCommon.Success, string.Empty);

                        //SaaLiftReceive saaLift = new SaaLiftReceive
                        //{
                        //    Statiom_Name = requesthandshakecarriertransport.StationID,
                        //    CommandName = SAA_DatabaseEnum.LiftCommandName.EquipmentLiftE84PlcHandshakeInfo.ToString(),
                        //};
                        //Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                        //{
                        //    { SAA_DatabaseEnum.EquipmentStatusCommand.Statiom_Name.ToString(),  saaLift.Statiom_Name},
                        //    { SAA_DatabaseEnum.EquipmentStatusCommand.CommandName.ToString(), saaLift.CommandName}
                        //};
                        //string commandcontent = JsonConvert.SerializeObject(saaLift);
                        //string ReportMessage = SAA_Database.SaaSendCommandSystems(commandcontent, SAA_DatabaseEnum.SendWebApiCommandName.GetLiftMessage.ToString());
                        //SAA_Database.LogMessage($"【傳送設備】【{saaLift.Statiom_Name}】【轉譯程式】自行接收結果:{ReportMessage}");
                    }
                    else
                    {
                        SAA_Database.LogMessage($"【{requesthandshakecarriertransport.StationID}】站點{requesthandshakecarriertransport.StationID}資料為空", SAA_Database.LogType.Error);
                    }
                }
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS E84 上下貨交握資訊===]
        [Route("SE_Handshake_CarrierTransport")]
        [HttpPost]
        public SaaReportResult SE_Handshake_CarrierTransport([FromBody] SaaReportHandshakeCarrierTransport handshakecarriertransport)
        {
            try
            {
                SaaReportHandshakeCarrierTransport reporthandshake = new SaaReportHandshakeCarrierTransport
                {
                    StationID = handshakecarriertransport.StationID,
                    Time = handshakecarriertransport.Time,
                    TEID = handshakecarriertransport.TEID,
                    ShuttleID = handshakecarriertransport.ShuttleID,
                    MissionID = handshakecarriertransport.MissionID,
                    HandsHakeType = handshakecarriertransport.HandsHakeType,
                    Handshake = handshakecarriertransport.Handshake,
                    CarrierInfo = handshakecarriertransport.CarrierInfo,
                };
                SAA_Database.LogMessage($"【{reporthandshake.StationID}】【接收iLIST】【{reporthandshake.HandsHakeType}】天車交握:車號{reporthandshake.ShuttleID} CarrierID:{reporthandshake.CarrierInfo.CarrierID}，L_REQ:{reporthandshake.Handshake.L_REQ}，U_REQ:{reporthandshake.Handshake.U_REQ}，Ready:{reporthandshake.Handshake.READY}，HO_AVBL:{reporthandshake.Handshake.HO_AVBL}，ES:{reporthandshake.Handshake.ES}，VA:{reporthandshake.Handshake.VA}，VS_0:{reporthandshake.Handshake.VS_0}，VS_1:{reporthandshake.Handshake.VS_1}");
                SAA_Database.LogMessage($"【{reporthandshake.StationID}】【接收iLIST】【{reporthandshake.HandsHakeType}】載體資訊 CarrierID:{reporthandshake.CarrierInfo.CarrierID}，批號:{reporthandshake.CarrierInfo.Schedule}，CarrierState:{reporthandshake.CarrierInfo.CarrierState}，DestinationType:{reporthandshake.CarrierInfo.DestinationType}");
                if (handshakecarriertransport.CarrierInfo.CarrierID != string.Empty || handshakecarriertransport.HandsHakeType == SAA_DatabaseEnum.HandshakeType.Report.ToString())
                {
                    if (reporthandshake.Handshake.U_REQ == SAA_DatabaseEnum.E84Handshake.True.ToString())
                    {
                        if (reporthandshake.HandsHakeType != SAA_DatabaseEnum.HandshakeType.Report.ToString())
                        {
                            if (receivstorageinfo.CarrideID != handshakecarriertransport.CarrierInfo.CarrierID)
                            {
                                SAA_Database.LogMessage($"【{reporthandshake.StationID}】【轉譯程式】【載體資訊】記憶體ID:{receivstorageinfo.CarrideID}，卡匣ID:{reporthandshake.CarrierInfo.CarrierID}");
                                SaaEquipmentCarrierInfo equipmentcarrierinfo = new SaaEquipmentCarrierInfo
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                    STATIOM_NAME = reporthandshake.StationID,
                                    CARRIERID = reporthandshake.CarrierInfo.CarrierID,
                                    PARTNO = reporthandshake.CarrierInfo.Schedule,
                                    CARRIERTYPE = reporthandshake.CarrierInfo.CarrierType,
                                    ROTFLAG = reporthandshake.CarrierInfo.Rotation,
                                    FLIPFLAG = reporthandshake.CarrierInfo.Flip,
                                    DESTINATIONTYPE = reporthandshake.CarrierInfo.DestinationType,
                                    CARRIERSTATE = reporthandshake.CarrierInfo.CarrierState,
                                    QTIME = reporthandshake.CarrierInfo.Qtime,
                                    CYCLETIME = reporthandshake.CarrierInfo.Cycletime,
                                    OPER = reporthandshake.CarrierInfo.Oper,
                                    RECIPE = reporthandshake.CarrierInfo.Recipe,
                                    REJECT_CODE = reporthandshake.CarrierInfo.RejectCode,
                                    REJECT_MESSAGE = reporthandshake.CarrierInfo.RejectMessage,
                                };
                                var data = SAA_Database.SaaSql.GetScDevice(equipmentcarrierinfo.STATIOM_NAME);
                                if (data.Rows.Count != 0)
                                {
                                    var carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                                    if (carrierinfodata.Rows.Count != 0)
                                    {
                                        SAA_Database.SaaSql.UpdEquipmentCarrierInfo(equipmentcarrierinfo);
                                        SAA_Database.LogMessage($"【{reporthandshake.StationID}】【更新資料】【有更新批號】更新設備資訊完成");
                                    }
                                    else
                                    {
                                        SAA_Database.SaaSql.SetScEquipmentCarrierInfo(equipmentcarrierinfo);
                                        SAA_Database.LogMessage($"【{reporthandshake.StationID}】【新增資料】新增設備資訊完成");
                                    }

                                    string devicetype = data.Rows[0][SAA_DatabaseEnum.SC_DEVICE.DEVICETYPE.ToString()].ToString();
                                    if (devicetype == "1")
                                    {
                                        SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增實盒卡匣】站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}，載體狀態:{equipmentcarrierinfo.DESTINATIONTYPE}，載體資訊:{equipmentcarrierinfo.CARRIERSTATE}");
                                        if ((equipmentcarrierinfo.DESTINATIONTYPE == SAA_DatabaseEnum.DestinationType.EQP.ToString() && equipmentcarrierinfo.CARRIERSTATE == SAA_DatabaseEnum.CarrierState.Material.ToString()))
                                        {
                                            SaaScLiftCarrierInfoMaterial LiftCarrierInfoMaterial = new SaaScLiftCarrierInfoMaterial()
                                            {
                                                TASKDATETIME = SAA_Database.ReadTime(),
                                                SETNO = equipmentcarrierinfo.SETNO,
                                                MODEL_NAME = equipmentcarrierinfo.MODEL_NAME,
                                                STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                CARRIERID = equipmentcarrierinfo.CARRIERID,
                                                DEVICETYPE = devicetype == "2" ? "1" : "2",//1:盒貨 2:空盒
                                                QTIME = equipmentcarrierinfo.QTIME,
                                                CYCLETIME = equipmentcarrierinfo.CYCLETIME,
                                            };
                                            var carrierinfomaterialdata = SAA_Database.SaaSql.GetLiftCarrierInfoMaterial(LiftCarrierInfoMaterial);
                                            if (carrierinfomaterialdata.Rows.Count == 0)
                                            {
                                                if (!equipmentcarrierinfo.CARRIERID.Contains(SAA_Database.configattributes.PARTICLE))
                                                {
                                                    SAA_Database.SaaSql.SetScLiftCarrierInfoMaterial(LiftCarrierInfoMaterial);
                                                    SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增實盒卡匣】新增ScLiftCarrierInfoMaterial資料表完成，站點:{LiftCarrierInfoMaterial.STATION_NAME}，卡匣ID:{LiftCarrierInfoMaterial.CARRIERID}");
                                                }
                                            }
                                            else
                                            {
                                                SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增實盒卡匣】新增ScLiftCarrierInfoMaterial資料，站點:{LiftCarrierInfoMaterial.STATION_NAME}，卡匣ID:{LiftCarrierInfoMaterial.CARRIERID}，已有相同卡匣ID不可新增");
                                            }
                                        }
                                        else if (equipmentcarrierinfo.DESTINATIONTYPE == SAA_DatabaseEnum.DestinationType.Buffer.ToString() && equipmentcarrierinfo.CARRIERSTATE == SAA_DatabaseEnum.CarrierState.Empty.ToString())
                                        {
                                            SaaScTransportrEquirement ScTransportrEquirement = new SaaScTransportrEquirement
                                            {
                                                SETNO = equipmentcarrierinfo.SETNO,
                                                MODEL_NAME = equipmentcarrierinfo.MODEL_NAME,
                                                STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                REPORT_STATION = equipmentcarrierinfo.STATIOM_NAME,
                                                REPORT_TIME = SAA_Database.ReadTime(),
                                                CARRIERID = equipmentcarrierinfo.CARRIERID,
                                                REQUIREMENT_TYPE = "2",
                                                BEGIN_STATION = equipmentcarrierinfo.STATIOM_NAME,
                                                END_STATION = SAA_Database.SaaCommon.NA
                                            };
                                            SAA_Database.SaaSql.SetScTransportrEquirement(ScTransportrEquirement);
                                        }
                                    }
                                }
                                else
                                {
                                    SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增實盒卡匣】查無此站點，站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}");
                                }
                            }
                        }
                    }

                    //if (reporthandshake.CarrierInfo.CarrierID != reporthandshake.CarrierInfo.CarrierID)
                    SAA_Database.SaaSql.UpdateScLiftE84PcStatus(reporthandshake.Time, reporthandshake.StationID, reporthandshake.ShuttleID, reporthandshake.MissionID, reporthandshake.CarrierInfo.CarrierID, reporthandshake.Handshake);
                    reporthandshake.CarrierInfo.CarrierID = reporthandshake.CarrierInfo.CarrierID;
                    SAA_Database.LogMessage($"【{reporthandshake.StationID}】【轉譯程式】【載體資訊】變更記憶體ID:{reporthandshake.CarrierInfo.CarrierID}，卡匣ID:{reporthandshake.CarrierInfo.CarrierID}");

                    SaaScLiftE84iLisPc LiftE84Pc = new SaaScLiftE84iLisPc
                    {
                        TASKDATETIME = reporthandshake.Time,
                        STATION_NAME = reporthandshake.StationID,
                        SHUTTLEID = reporthandshake.ShuttleID,
                        COMMANDID = reporthandshake.TEID,
                        CARRIERID = reporthandshake.CarrierInfo.CarrierID,
                        Mode = reporthandshake.HandsHakeType,
                        VALID = reporthandshake.Handshake.VALID,
                        CS_0 = reporthandshake.Handshake.CS_0,
                        CS_1 = reporthandshake.Handshake.CS_1,
                        TR_REQ = reporthandshake.Handshake.TR_REQ,
                        L_REQ = reporthandshake.Handshake.L_REQ,
                        U_REQ = reporthandshake.Handshake.U_REQ,
                        READY = reporthandshake.Handshake.READY,
                        BUSY = reporthandshake.Handshake.BUSY,
                        COMPT = reporthandshake.Handshake.COMPT,
                        CONT = reporthandshake.Handshake.CONT,
                        HOA_VBL = reporthandshake.Handshake.HO_AVBL,
                        ES = reporthandshake.Handshake.ES,
                        VA = reporthandshake.Handshake.VA,
                        AM_AVBL = reporthandshake.Handshake.AM_AVBL,
                        VS_0 = reporthandshake.Handshake.VS_0,
                        VS_1 = reporthandshake.Handshake.VS_1,
                    };
                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                     {
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.TASKDATETIME.ToString(), LiftE84Pc.TASKDATETIME},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.STATION_NAME.ToString(), LiftE84Pc.STATION_NAME},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.SHUTTLEID.ToString(), LiftE84Pc.SHUTTLEID},
                          { SAA_DatabaseEnum.SendLiftE84iLisPc.CARRIERID.ToString(), LiftE84Pc.CARRIERID},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.COMMANDID.ToString(), LiftE84Pc.COMMANDID},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.Mode.ToString(), LiftE84Pc.Mode},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.VALID.ToString(), LiftE84Pc.VALID},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.CS_0.ToString(), LiftE84Pc.CS_0},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.CS_1.ToString(), LiftE84Pc.CS_1},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.TR_REQ.ToString(), LiftE84Pc.TR_REQ},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.L_REQ.ToString(), LiftE84Pc.L_REQ},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.U_REQ.ToString(), LiftE84Pc.U_REQ},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.READY.ToString(), LiftE84Pc.READY},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.BUSY.ToString(), LiftE84Pc.BUSY},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.COMPT.ToString(), LiftE84Pc.COMPT},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.CONT.ToString(), LiftE84Pc.CONT},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.HOA_VBL.ToString(), LiftE84Pc.HOA_VBL},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.ES.ToString(), LiftE84Pc.ES},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.VA.ToString(), LiftE84Pc.VA},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.AM_AVBL.ToString(), LiftE84Pc.AM_AVBL},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.VS_0.ToString(), LiftE84Pc.VS_0},
                         { SAA_DatabaseEnum.SendLiftE84iLisPc.VS_1.ToString(), LiftE84Pc.VS_1},
                     };
                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                    while (true)
                    {
                        SAA_Database.LogMessage($"【{reporthandshake.StationID}】【{SAA_DatabaseEnum.SendWebApiCommandName.SaaEquipmentMonitorE84PcSendStart}】【傳送E84訊號】傳送E84訊號至LIFT，內容:{commandcontent}");
                        string result = SAA_Database.SaaSendCommandLift(commandcontent, SAA_DatabaseEnum.SendWebApiCommandName.SaaEquipmentMonitorE84PcSendStart.ToString());
                        SAA_Database.LogMessage($"【{reporthandshake.StationID}】LIFT回傳結果:{result}");
                        if (result == SAA_Database.configattributes.WebApiResultOK)
                        {
                            saareportresult = ReadReportResult(reporthandshake.StationID, reporthandshake.TEID, handshakecarriertransport.Time, SAA_Database.SaaCommon.Success, string.Empty);
                            break;
                        }
                        else
                        {
                            SAA_Database.LogMessage($"【{reporthandshake.StationID}】LIFT回傳結果不為OK，重新傳送");
                        }
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    saareportresult = ReadReportResult(reporthandshake.StationID, reporthandshake.TEID, handshakecarriertransport.Time, SAA_Database.SaaCommon.Fail, "CarrierID不可為空值");
                }
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        #region [===接收iLIS 搬運需求資訊===]
        [Route("SE_Report_TransportRequirementInfo")]
        [HttpPost]
        public SaaReportResult SE_Report_TransportRequirementInfo([FromBody] SaaReportTransportRequirementInfo reporttransportrequirementinfo)
        {
            try
            {
                SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【接收iLIST】接收回覆需求搬運資訊，Time:{reporttransportrequirementinfo.Time}，Command ID:{reporttransportrequirementinfo.TEID}，站點:{reporttransportrequirementinfo.StationID})，數量:{reporttransportrequirementinfo.ListRequirementInfo.Count}", SAA_Database.LogType.Normal, SAA_Database.LogSystmes.iLIs);
                for (int i = 0; i < reporttransportrequirementinfo.ListRequirementInfo.Count; i++)
                {
                    RequirementInfo requirementinfo = new RequirementInfo
                    {
                        CarrierID = reporttransportrequirementinfo.ListRequirementInfo[i].CarrierID,
                        RequirementType = reporttransportrequirementinfo.ListRequirementInfo[i].RequirementType,
                        BeginStation = reporttransportrequirementinfo.ListRequirementInfo[i].BeginStation,
                        EndStation = reporttransportrequirementinfo.ListRequirementInfo[i].EndStation,
                        Oper = reporttransportrequirementinfo.ListRequirementInfo[i].Oper,
                    };

                    if (Enum.IsDefined(typeof(SAA_DatabaseEnum.RequirementType), requirementinfo.RequirementType))
                    {
                        switch ((SAA_DatabaseEnum.RequirementType)Enum.Parse(typeof(SAA_DatabaseEnum.RequirementType), requirementinfo.RequirementType))
                        {
                            case SAA_DatabaseEnum.RequirementType.Unknow:
                                break;
                            case SAA_DatabaseEnum.RequirementType.Take_out_Carrier:
                                equipmentrequirementtype.Take_out_Carrier.Add(requirementinfo.CarrierID);
                                break;
                            case SAA_DatabaseEnum.RequirementType.Take_In_EmptyCarrier:
                                equipmentrequirementtype.Take_In_EmptyCarrier.Add(requirementinfo.CarrierID);
                                break;
                            case SAA_DatabaseEnum.RequirementType.Take_out_EmptyCarrier:
                                equipmentrequirementtype.Take_out_EmptyCarrier.Add(requirementinfo.CarrierID);
                                break;
                        }
                    }
                }

                if (reporttransportrequirementinfo.ListRequirementInfo.Count == 0)
                {
                    var devicedata = SAA_Database.SaaSql.GetScDevice(reporttransportrequirementinfo.StationID);
                    if (devicedata.Rows.Count != 0)
                    {
                        string devicetype = devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.DEVICETYPE.ToString()].ToString();
                        if (devicetype == SAA_Database.SaaCommon.DeivertTypeUD.ToString())
                        {
                            SAA_Database.SaaSql.DelScLiftAmount(reporttransportrequirementinfo.StationID);
                            SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【接收iLIST】接收回覆需求搬運資訊，需求類型:空盒需求，終點站名:{reporttransportrequirementinfo.StationID}，iLIS已無空盒需求，LiftAmount刪除需求站點:{reporttransportrequirementinfo.StationID}", SAA_Database.LogType.Normal, SAA_Database.LogSystmes.iLIs);

                            var equirementmaterialdata = SAA_Database.SaaSql.GetScTransportrEquirementMaterial(reporttransportrequirementinfo.StationID);
                            if (equirementmaterialdata.Rows.Count != 0)
                            {
                                foreach (DataRow dr in equirementmaterialdata.Rows)
                                {
                                    SaaEsReportTransportRequirement EsReportTransport = new SaaEsReportTransportRequirement
                                    {
                                        STATION = reporttransportrequirementinfo.StationID,
                                        DATATIME = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.REPORT_TIME.ToString()].ToString(),
                                        TEID = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.REPORTID.ToString()].ToString(),
                                        CARRIERID = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.CARRIERID.ToString()].ToString(),
                                        BEGINSTATION = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.BEGIN_STATION.ToString()].ToString(),
                                        ENDSTATION = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.END_STATION.ToString()].ToString(),
                                    };
                                    SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                                    {
                                        SETNO = devicedata.Rows.Count != 0 ? devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                                        MODEL_NAME = devicedata.Rows.Count != 0 ? devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                        STATION_NAME = reporttransportrequirementinfo.StationID,
                                        REPORT_STATION = reporttransportrequirementinfo.StationID,
                                        REPORT_TIME = SAA_Database.ReadTeid(),
                                        CARRIERID = EsReportTransport.CARRIERID,
                                        REQUIREMENT_TYPE = "2",
                                        BEGIN_STATION = EsReportTransport.BEGINSTATION,
                                        END_STATION = EsReportTransport.ENDSTATION,
                                    };
                                    ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                                    if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                                    {
                                        SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                                        SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【新增資料】新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                    }
                                    else
                                    {
                                        SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【新增資料】已有需求無法新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                    }
                                    WebApiSendEsReportTransportRequirement(EsReportTransport);
                                }
                            }
                        }
                        else if (devicetype == SAA_Database.SaaCommon.DevicetTypeLD.ToString())
                        {
                            var TransportrEquirementdata = SAA_Database.SaaSql.GetScTransportrEquirement(reporttransportrequirementinfo.StationID);
                            if (TransportrEquirementdata.Rows.Count != 0)
                            {
                                SAA_Database.SaaSql.UpdScTransportrEquirement(reporttransportrequirementinfo.StationID);
                            }
                        }
                    }
                }
                else
                {
                    var equirementmaterial = SAA_Database.SaaSql.GetScTransportrEquirementMaterial(reporttransportrequirementinfo.StationID);
                    foreach (DataRow dr in equirementmaterial.Rows)
                    {
                        if (equipmentrequirementtype.Take_out_Carrier.IndexOf(dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.CARRIERID.ToString()].ToString()) == -1)
                        {


                            SaaEsReportTransportRequirement EsReportTransport = new SaaEsReportTransportRequirement
                            {
                                STATION = reporttransportrequirementinfo.StationID,
                                DATATIME = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.REPORT_TIME.ToString()].ToString(),
                                TEID = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.REPORTID.ToString()].ToString(),
                                CARRIERID = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.CARRIERID.ToString()].ToString(),
                                BEGINSTATION = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.BEGIN_STATION.ToString()].ToString(),
                                ENDSTATION = dr[SAA_DatabaseEnum.SC_TRANSPORTR_EQUIREMENT_MATERIAL.END_STATION.ToString()].ToString(),
                            };
                            var devicedata = SAA_Database.SaaSql.GetScDevice(reporttransportrequirementinfo.StationID);
                            SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                            {
                                SETNO = devicedata.Rows.Count != 0 ? devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                                MODEL_NAME = devicedata.Rows.Count != 0 ? devicedata.Rows[0][SAA_DatabaseEnum.SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                STATION_NAME = reporttransportrequirementinfo.StationID,
                                REPORT_STATION = reporttransportrequirementinfo.StationID,
                                REPORT_TIME = SAA_Database.ReadTeid(),
                                CARRIERID = EsReportTransport.CARRIERID,
                                REQUIREMENT_TYPE = "2",
                                BEGIN_STATION = EsReportTransport.BEGINSTATION,
                                END_STATION = EsReportTransport.ENDSTATION,
                            };
                            ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                            if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                            {
                                SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                                SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【新增資料】新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                            }
                            else
                            {
                                SAA_Database.LogMessage($"【{reporttransportrequirementinfo.StationID}】【新增資料】已有需求無法新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                            }
                            WebApiSendEsReportTransportRequirement(EsReportTransport);
                        }
                    }
                }
                if (equipmentrequirementtype.Take_out_Carrier.Count != 0 || equipmentrequirementtype.Take_In_EmptyCarrier.Count != 0 || equipmentrequirementtype.Take_out_EmptyCarrier.Count != 0)
                {
                    equipmentrequirementtype.Take_out_Carrier.Clear();
                    equipmentrequirementtype.Take_In_EmptyCarrier.Clear();
                    equipmentrequirementtype.Take_out_EmptyCarrier.Clear();
                }
                saareportresult = ReadReportResult(reporttransportrequirementinfo.StationID, reporttransportrequirementinfo.TEID, reporttransportrequirementinfo.Time, SAA_Database.SaaCommon.Success, string.Empty);
                return saareportresult;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }
        #endregion

        /*===============================方法===============================================*/

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
                            saareceiv.Station = "VA-700C";//dataAry[i];
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

        #region [===字典轉成Json格式===]
        /// <summary>
        /// 字典轉成Json格式
        /// </summary>
        /// <param name="dicContent"></param>
        /// <returns></returns>
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
        #endregion

        #region [===查詢指令名稱===]
        /// <summary>
        /// 查詢指令名稱
        /// </summary>
        /// <param name="db">資料表</param>
        /// <param name="station">站點</param>
        /// <returns></returns>
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

        #region [===傳送設備狀態至iLIS===]
        /// <summary>
        /// 傳送設備狀態至iLIS
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void SendWebApiEquipmentStatusiLIS(SaaRequestEquipmentStatus equipmentstatus)
        {
            try
            {
                var Status = SAA_Database.SaaSql.GetScDevice(equipmentstatus.StationID);
                if (Status.Rows.Count != 0)
                {
                    string eqpstatus = Status.Rows.Count != 0 ? Status.Rows[0][SAA_DatabaseEnum.SC_DEVICE.DEVICESTATUS.ToString()].ToString() : SAA_DatabaseEnum.EqpStatus.Unknow.ToString();
                    string status = eqpstatus == SAA_DatabaseEnum.DEVICESTATUS.Y.ToString() ? SAA_DatabaseEnum.EqpStatus.Auto.ToString() : SAA_DatabaseEnum.EqpStatus.Manual.ToString();
                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                   {
                       { SAA_DatabaseEnum.SE_Request_EquipmentStatus.StationID.ToString(), equipmentstatus.StationID },
                       { SAA_DatabaseEnum.SE_Request_EquipmentStatus.Time.ToString(), equipmentstatus.Time },
                       { SAA_DatabaseEnum.SE_Request_EquipmentStatus.TEID.ToString(), equipmentstatus.TEID },
                       { SAA_DatabaseEnum.SE_Request_EquipmentStatus.EqpStatus.ToString(), status},
                   };
                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                    SAA_Database.LogMessage($"【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentStatus}】【通訊傳送】站點:{equipmentstatus.StationID}，時間:{equipmentstatus.Time}，傳送編號:{equipmentstatus.TEID}傳送內容:{commandcontent}");
                    string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentStatus.ToString());
                    SaaReportResult saareportresult = WebApiReportResult(returnresult);
                    SAA_Database.LogMessage($"【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentStatus}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");

                    SaaReportCommandAutpMation commandAutpMation = new SaaReportCommandAutpMation
                    {
                        CMD_NO = "S001",
                        CMD_NAME = eqpstatus == SAA_DatabaseEnum.DEVICESTATUS.Y.ToString() ? "AUTOMATION_ON" : "AUTOMATION_OFF",
                        STATION = equipmentstatus.StationID,
                    };
                    SAA_Database.SaaSendAutoMation(commandAutpMation);
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }
        #endregion


        public SaaReportResult ReadReportResult(string stationid, string teid, string reporttime, string returncode, string returnmessage)
        {
            try
            {
                SaaReportResult saareport = new SaaReportResult
                {
                    StationID = stationid,
                    TEID = teid,
                    Time = reporttime,
                    ReturnCode = returncode,
                    ReturnMessage = returnmessage,
                };
                SAA_Database.LogMessage($"【{saareport.StationID}】【接收iLIS】回傳接收完成，StationID:{saareport.StationID}，TEID:{saareport.TEID}，Time:{saareport.Time}，ReturnCode:{saareport.ReturnCode}，ReturnMessage:{saareport.ReturnMessage}");
                return saareport;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return null;
            }
        }

        public void WebApiSendTransportEquipmentHardwareInfo(string StationID, string Time, string TEID, List<HardwareInfo> requihardwareinfo, List<CarrierInfo> requicarrierinfo)
        {
            try
            {
                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                {
                    { SAA_DatabaseEnum.ES_Report_EquipmentHardwareInfo.StationID.ToString(), StationID },
                    { SAA_DatabaseEnum.ES_Report_EquipmentHardwareInfo.Time.ToString(), Time },
                    { SAA_DatabaseEnum.ES_Report_EquipmentHardwareInfo.TEID.ToString(), TEID },
                    { SAA_DatabaseEnum.ES_Report_EquipmentHardwareInfo.ListHardwareInfo.ToString(), requihardwareinfo },
                    { SAA_DatabaseEnum.ES_Report_EquipmentHardwareInfo.ListCarrierInfo.ToString(), requicarrierinfo }
                };
                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                SAA_Database.LogMessage($"【{StationID}】【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentHardwareInfo}】【通訊傳送】站點:{StationID}，時間:{Time}，傳送編號:{TEID}傳送內容:{commandcontent}");
                string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentHardwareInfo.ToString());
                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                SAA_Database.LogMessage($"【{StationID}】【LCS->iLIS】【{SAA_DatabaseEnum.SendWebApi.ES_Report_EquipmentHardwareInfo}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public SaaReportResult WebApiReportResult(string returnresult)
        {
            Dictionary<string, string> returndic = JsonConvert.DeserializeObject<Dictionary<string, string>>(returnresult);
            SaaReportResult saareportresult = new SaaReportResult();
            if (returndic != null)
            {
                foreach (var data in returndic)
                {
                    var datakey = data.Key;
                    var datavalue = data.Value;
                    if (Enum.IsDefined(typeof(SAA_DatabaseEnum.WebApiReceive), datakey))
                    {
                        switch ((SAA_DatabaseEnum.WebApiReceive)Enum.Parse(typeof(SAA_DatabaseEnum.WebApiReceive), datakey))
                        {
                            case SAA_DatabaseEnum.WebApiReceive.StationID:
                                saareportresult.StationID = datavalue;
                                break;
                            case SAA_DatabaseEnum.WebApiReceive.Time:
                                saareportresult.Time = datavalue;
                                break;
                            case SAA_DatabaseEnum.WebApiReceive.TEID:
                                saareportresult.TEID = datavalue;
                                break;
                            case SAA_DatabaseEnum.WebApiReceive.ReturnCode:
                                saareportresult.ReturnCode = datavalue;
                                break;
                            case SAA_DatabaseEnum.WebApiReceive.ReturnMessage:
                                saareportresult.ReturnMessage = datavalue;
                                break;
                        }
                    }
                }
                return saareportresult;
            }
            return null;
        }

        private bool IsJsonFormat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            if ((value.StartsWith("{") && value.EndsWith("}")) ||
                (value.StartsWith("[") && value.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }
            return false;
        }

        #region [===料盒搬運需求===]
        /// <summary>
        /// 料盒搬運需求
        /// </summary>
        /// <param name="ReportTransportRequirement"></param>
        public void WebApiSendEsReportTransportRequirement(SaaEsReportTransportRequirement ReportTransportRequirement)
        {
            var carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(ReportTransportRequirement.CARRIERID);
            List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
            RequirementInfo info = new RequirementInfo
            {
                RequirementType = SAA_DatabaseEnum.RequirementType.Take_out_Carrier.ToString(),
                CarrierID = ReportTransportRequirement.CARRIERID,
                BeginStation = ReportTransportRequirement.BEGINSTATION,
                EndStation = ReportTransportRequirement.ENDSTATION,
                Oper = carrierinfodata.Rows.Count != 0 ? carrierinfodata.Rows[0][SAA_DatabaseEnum.SC_EQUIPMENT_CARRIER_INFO.OPER.ToString()].ToString() : string.Empty,
            };
            requirementinfo.Add(info);
            Dictionary<string, object> dicstatusb = new Dictionary<string, object>
            {
                 { SAA_DatabaseEnum.ES_Report_TransportRequirement.StationID.ToString(),  ReportTransportRequirement.STATION },
                { SAA_DatabaseEnum.ES_Report_TransportRequirement.Time.ToString(), ReportTransportRequirement.DATATIME },
                { SAA_DatabaseEnum.ES_Report_TransportRequirement.TEID.ToString(), ReportTransportRequirement.TEID },
                { SAA_DatabaseEnum.ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo },
            };
            string commandcontent = JsonConvert.SerializeObject(dicstatusb);
            SAA_Database.LogMessage($"【{ReportTransportRequirement.STATION}】【LCS->iLIS】【ES_Report_TransportRequirement】【通訊傳送】站點:{ReportTransportRequirement.STATION}，時間:{ReportTransportRequirement.DATATIME}，傳送編號:{ReportTransportRequirement.TEID}傳送內容:{commandcontent}");
            string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SAA_DatabaseEnum.SendWebApi.ES_Report_TransportRequirement.ToString());
            SaaReportResult saareportresult = WebApiReportResult(returnresult);
            SAA_Database.LogMessage($"【{ReportTransportRequirement.STATION}】【LCS->iLIS】【ES_Report_TransportRequirement】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
            if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
            {
                SAA_Database.LogMessage($"【{ReportTransportRequirement.STATION}】【LCS->iLIS】【ES_Report_TransportRequirement】重新傳送料盒搬運需求:站點:{ReportTransportRequirement.STATION}、起點{ReportTransportRequirement.BEGINSTATION}、終點:{ReportTransportRequirement.ENDSTATION}、卡匣ID:{ReportTransportRequirement.CARRIERID}");
            }
        }
        #endregion
    }
}
