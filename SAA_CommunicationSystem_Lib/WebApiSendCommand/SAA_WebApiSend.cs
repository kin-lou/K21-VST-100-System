using Newtonsoft.Json;
using SAA_CommunicationSystem_Lib.CommandReportAttributes;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using SAA_CommunicationSystem_Lib.ReceivLiftAttributes;
using SAA_CommunicationSystem_Lib.ReportAttributes;
using SAA_CommunicationSystem_Lib.ReportCommandAttributes;
using SAA_CommunicationSystem_Lib.SendAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem_Lib.WebApiSendCommand
{
    public class SAA_WebApiSend
    {
        private string lcsreceive = string.Empty;
        private string returnresult = string.Empty;
        private string lcsreturnresult = string.Empty;
        private bool device = true;
        private DateTime dtStart = DateTime.Now;
        private int plcreturncode = 0;
        private List<string> devicelist = new List<string>();
        private readonly SaaReportCommandAttributes reportcommand = new SaaReportCommandAttributes();
        public async void StartWebApiSend()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        //WebApiEsReportAlive();
                        WebApiLcsReceive();
                        WebApiSendiLcs();
                        //WebApiLcsHandshakeSendiLcs();
                        WebApiEquipmentReport();
                        //WebApiSendEquipmnetHardwareInfo();
                        WebApiSndEquipmentCarrier();
                        WebApiSendAskShuttleTask();
                        //WebApiSendLiftAlarmList();
                        //WebApiSendEquipmentStatus();
                        Thread.Sleep(500);
                    }
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        #region [===心跳包===]
        public async void StartWebApiSendAlive()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        WebApiEsReportAlive();
                        Thread.Sleep(2000);
                    }
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public void WebApiEsReportAlive()
        {
            try
            {
                if (device)
                {
                    var scdevicedata = SAA_Database.SaaSql.GetScDevice();
                    foreach (DataRow dr in scdevicedata.Rows)
                    {
                        devicelist.Add(dr[SC_DEVICE.STATION_NAME.ToString()].ToString());
                    }
                    for (int i = 0; i < devicelist.Count; i++)
                    {
                        SaaReport report = new SaaReport
                        {
                            StationID = devicelist[i],
                            Time = SAA_Database.ReadTeid(),
                        };
                        report.TEID = $"{report.StationID}_{report.Time}";
                        Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                        {
                            { ES_Report_Alive.StationID.ToString(),  report.StationID},
                            { ES_Report_Alive.Time.ToString(),  report.Time},
                            { ES_Report_Alive.TEID.ToString(),  report.TEID},
                        };
                        string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                        returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Request_Handshake_CarrierTransport.ToString());
                        SaaReportResult saareportresult = WebApiReportResult(returnresult);
                    }
                    device = false;
                }
                for (int i = 0; i < devicelist.Count; i++)
                {
                    SaaReport report = new SaaReport
                    {
                        StationID = devicelist[i],
                        Time = SAA_Database.ReadTeid(),
                    };
                    report.TEID = $"{report.StationID}_{report.Time}";
                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                    {
                        { ES_Report_Alive.StationID.ToString(),  report.StationID},
                        { ES_Report_Alive.Time.ToString(),  report.Time},
                        { ES_Report_Alive.TEID.ToString(),  report.TEID},
                    };
                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                    returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_Alive.ToString());
                    SaaReportResult saareportresult = WebApiReportResult(returnresult);
                    if (saareportresult != null)
                    {
                        if (saareportresult.ReturnCode != SAA_Database.SaaCommon.Success)
                        {
                            SAA_Database.LogMessage($"【{SendWebApi.ES_Report_Alive}】【{report.StationID}】心跳傳送失敗。");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        #region [===傳送搬運命令至LCS===]
        /// <summary>
        /// 傳送搬運命令至LCS
        /// </summary>
        private void WebApiLcsReceive()
        {
            try
            {
                var directivedata = SAA_Database.SaaSql.GetScDirective(SAA_Database.LogSystmes.iLIs.ToString());
                foreach (DataRow dr in directivedata.Rows)
                {
                    SaaScDirective SaaDirective = new SaaScDirective
                    {
                        TASKDATETIME = dr[SC_DIRECTIVE.TASKDATETIME.ToString()].ToString(),
                        SETNO = dr[SC_DIRECTIVE.SETNO.ToString()].ToString(),
                        COMMANDON = dr[SC_DIRECTIVE.COMMANDON.ToString()].ToString(),
                        STATION_NAME = dr[SC_DIRECTIVE.STATION_NAME.ToString()].ToString(),
                        CARRIERID = dr[SC_DIRECTIVE.CARRIERID.ToString()].ToString(),
                        COMMANDID = dr[SC_DIRECTIVE.COMMANDID.ToString()].ToString(),
                        COMMANDTEXT = dr[SC_DIRECTIVE.COMMANDTEXT.ToString()].ToString(),
                        SOURCE = dr[SC_DIRECTIVE.SOURCE.ToString()].ToString(),
                        SENDFLAG = dr[SC_DIRECTIVE.SENDFLAG.ToString()].ToString(),
                    };
                    SAA_Database.LogMessage($"【接收】iLIS指令:{SaaDirective.COMMANDTEXT}");
                    Dictionary<string, string> dicdata = SAA_Database.ContentToDictionary(SaaDirective.COMMANDTEXT);
                    foreach (var data in dicdata)
                    {
                        var datakey = data.Key;
                        var datavalue = data.Value;
                        if (Enum.IsDefined(typeof(ReceivCommand), datakey))
                        {
                            switch ((ReceivCommand)Enum.Parse(typeof(ReceivCommand), datakey))
                            {
                                case ReceivCommand.CMD_NO:
                                    reportcommand.CMD_NO = datavalue;
                                    break;
                                case ReceivCommand.CMD_NAME:
                                    reportcommand.CMD_NAME = datavalue;
                                    break;
                                case ReceivCommand.STATION:
                                    reportcommand.STATION = datavalue;
                                    break;
                                case ReceivCommand.SCHEDULE:
                                    reportcommand.SCHEDULE = datavalue;
                                    break;
                                case ReceivCommand.WARENUMBER:
                                    reportcommand.WARENUMBER = datavalue;
                                    break;
                                case ReceivCommand.ORIGIN:
                                    reportcommand.ORIGIN = datavalue;
                                    break;
                                case ReceivCommand.DESTINATION:
                                    reportcommand.DESTINATION = datavalue;
                                    break;
                                case ReceivCommand.ROTFLAG:
                                    reportcommand.ROTFLAG = datavalue;
                                    break;
                                case ReceivCommand.FLIPFLAG:
                                    reportcommand.FLIPFLAG = datavalue;
                                    break;
                                case ReceivCommand.CARRIER:
                                    reportcommand.CARRIER = datavalue;
                                    break;
                                case ReceivCommand.FROM:
                                    reportcommand.FROM = datavalue;
                                    break;
                                case ReceivCommand.TO:
                                    reportcommand.TO = datavalue;
                                    break;
                                case ReceivCommand.QTIME:
                                    reportcommand.QTIME = datavalue;
                                    break;
                                case ReceivCommand.OPER:
                                    reportcommand.OPER = datavalue;
                                    break;
                                case ReceivCommand.CYCLETIME:
                                    reportcommand.CYCLETIME = datavalue;
                                    break;
                                case ReceivCommand.RECIPE:
                                    reportcommand.RECIPE = datavalue;
                                    break;
                                case ReceivCommand.REJECT_CODE:
                                    reportcommand.REJECT_CODE = datavalue;
                                    break;
                                case ReceivCommand.REJECT_MESSAGE:
                                    reportcommand.REJECT_MESSAGE = datavalue;
                                    break;
                                case ReceivCommand.CARRIERTYOE:
                                    reportcommand.CARRIERTYOE = datavalue;
                                    break;
                            }
                        }
                    }
                    if (reportcommand != null)
                    {
                        var scdevicedata = SAA_Database.SaaSql.GetScDevice(int.Parse(SaaDirective.SETNO), SaaDirective.STATION_NAME);
                        SaaEquipmentCarrierInfo equipmentcarrierinfo = new SaaEquipmentCarrierInfo
                        {
                            SETNO = int.Parse(SaaDirective.SETNO),
                            MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.SaaCommon.NA,
                            STATIOM_NAME = SaaDirective.STATION_NAME,
                            CARRIERID = reportcommand.CARRIER,
                            PARTNO = reportcommand.SCHEDULE,
                            CARRIERTYOE = string.IsNullOrEmpty(reportcommand.CARRIERTYOE) ? SAA_Database.SaaCommon.CarrierType : reportcommand.CARRIERTYOE,
                            ROTFLAG = reportcommand.ROTFLAG,
                            FLIPFLAG = reportcommand.FLIPFLAG,
                            OPER = reportcommand.OPER,
                            RECIPE = reportcommand.RECIPE,
                            ORIGIN = reportcommand.ORIGIN,
                            DESTINATION = reportcommand.DESTINATION,
                            QTIME = reportcommand.QTIME,
                            CYCLETIME = reportcommand.CYCLETIME,
                            REJECT_CODE = reportcommand.REJECT_CODE,
                            REJECT_MESSAGE = reportcommand.REJECT_MESSAGE,
                            CARRIERSTATE = (reportcommand.SCHEDULE != SAA_Database.SaaCommon.NA.ToString() || !string.IsNullOrEmpty(reportcommand.SCHEDULE)) ? CarrierState.Material.ToString() : CarrierState.Empty.ToString(),
                            DESTINATIONTYPE = reportcommand.DESTINATION.Contains("WIP-ZIP_INWIP") || reportcommand.DESTINATION.Contains("EQPBUFF") ? DestinationType.Buffer.ToString() : reportcommand.DESTINATION.Contains("REJECT") ? DestinationType.Reject.ToString() : DestinationType.EQP.ToString(),
                        };
                        var carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                        if (carrierinfodata.Rows.Count != 0)
                            SAA_Database.SaaSql.DelEquipmentCarrierInfo(equipmentcarrierinfo);
                        SAA_Database.SaaSql.SetScEquipmentCarrierInfo(equipmentcarrierinfo);
                        SAA_Database.LogMessage($"【接收命令資料】 機台編號:{equipmentcarrierinfo.SETNO} 站點:{equipmentcarrierinfo.STATIOM_NAME}，指令編號:{SaaDirective.COMMANDID}");
                        if (Enum.IsDefined(typeof(LcsReceive), SaaDirective.COMMANDID))
                        {
                            switch ((LcsReceive)Enum.Parse(typeof(LcsReceive), SaaDirective.COMMANDID))
                            {
                                case LcsReceive.M001:
                                case LcsReceive.M501:
                                    SAA_Database.SaaSql.UpdScScPurchase(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.Y.ToString());
                                    SAA_Database.SaaSql.UpdScEquipmentCarrierInfoSendFlag(equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.Y.ToString());
                                    SAA_Database.LogMessage($"更新資料回覆資料 機台編號:{equipmentcarrierinfo.SETNO} 站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}，更新為:{SaaSendReply.Y}(Y:進盒，N:REJECT)");
                                    if (reportcommand.ORIGIN.Contains("BUFF") || reportcommand.DESTINATION.Contains("BUFF"))
                                    {
                                        var fulldata = SAA_Database.SaaSql.GetScScLocationsettingFull(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.MODEL_NAME, equipmentcarrierinfo.STATIOM_NAME);
                                        if (fulldata.Rows.Count == 0)
                                        {
                                            #region [===LIFT儲格已滿送回ZIP===]
                                            SAA_Database.LogMessage($"倉儲已滿:站點:{equipmentcarrierinfo.STATIOM_NAME}、起點{equipmentcarrierinfo.STATIOM_NAME}、終點:{SAA_Database.SaaCommon.SaaZipStationName}、卡匣ID:{equipmentcarrierinfo.CARRIERID}");
                                            carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                                            string beginstation = carrierinfodata.Rows[0]["ORIGIN"].ToString();
                                            string endstation = SAA_Database.SaaCommon.SaaZipStationName;
                                            string station = scdevicedata.Rows[0]["STATION_NAME"].ToString();
                                            SAA_Database.LogMessage($"建立料盒搬運需求:站點:{station}、起點{beginstation}、終點:{endstation}、卡匣ID:{equipmentcarrierinfo.CARRIERID}");
                                            string dataTime = SAA_Database.ReadTime();
                                            string TEID = $"{station}_{dataTime}";
                                            List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
                                            RequirementInfo info = new RequirementInfo
                                            {
                                                RequirementType = RequirementType.Take_out_Carrier.ToString(),
                                                CarrierID = equipmentcarrierinfo.CARRIERID,
                                                BeginStation = beginstation,
                                                EndStation = endstation,
                                            };
                                            requirementinfo.Add(info);
                                            Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                                            {
                                                { ES_Report_TransportRequirement.StationID.ToString(),  station },
                                                { ES_Report_TransportRequirement.Time.ToString(), dataTime },
                                                { ES_Report_TransportRequirement.TEID.ToString(), TEID },
                                                { ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo }
                                            };
                                            string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                                            SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{equipmentcarrierinfo.STATIOM_NAME}，時間:{dataTime}，傳送編號:{TEID}傳送內容:{commandcontent}");
                                            string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_TransportRequirement.ToString());
                                            SaaReportResult saareportresult = WebApiReportResult(returnresult);
                                            SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");

                                            SaaScLiftCarrierInfo liftCarrierInfo = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = equipmentcarrierinfo.SETNO,
                                                MODEL_NAME = equipmentcarrierinfo.MODEL_NAME,
                                                STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                CARRIERID = equipmentcarrierinfo.CARRIERID,
                                            };
                                            var liftcarrierinfodata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                            string infodata = liftcarrierinfodata.Rows.Count != 0 ? liftcarrierinfodata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                            SAA_Database.LogMessage($"ORIGIN:{infodata}");
                                            CommandReportM001 commandreportm001 = new CommandReportM001
                                            {
                                                CMD_NO = AseCommandNo.M001.ToString(),
                                                CMD_NAME = AseCommandName.CARRIER_TO_MECHANISM.ToString(),
                                                CARRIER = equipmentcarrierinfo.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError) ? "ERROR" : equipmentcarrierinfo.CARRIERID,
                                                SCHEDULE = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString()) ? carrierinfodata.Rows[0]["PARTNO"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                                OPER = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["OPER"].ToString()) ? carrierinfodata.Rows[0]["OPER"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                                RECIPE = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["RECIPE"].ToString()) ? carrierinfodata.Rows[0]["RECIPE"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                                ORIGIN = infodata,
                                                DESTINATION = endstation,
                                                QTIME = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["QTIME"].ToString()) ? carrierinfodata.Rows[0]["QTIME"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                                CYCLETIME = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["CYCLETIME"].ToString()) ? carrierinfodata.Rows[0]["CYCLETIME"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                            };
                                            WebApiSendCommandM001(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, commandreportm001);
                                            #endregion
                                        }
                                        else
                                        {
                                            if (reportcommand.DESTINATION.Contains(SaaDirective.STATION_NAME))
                                            {
                                                var data = SAA_Database.SaaSql.GetScDeviceStation(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.MODEL_NAME, equipmentcarrierinfo.STATIOM_NAME);
                                                if (data.Rows.Count != 0)
                                                {
                                                    string devicetype = data.Rows[0][SC_DEVICE.DEVICETYPE.ToString()].ToString();
                                                    SaaScLiftCarrierInfo liftCarrierInfo = new SaaScLiftCarrierInfo()
                                                    {
                                                        SETNO = equipmentcarrierinfo.SETNO,
                                                        MODEL_NAME = equipmentcarrierinfo.MODEL_NAME,
                                                        STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                        CARRIERID = equipmentcarrierinfo.CARRIERID,
                                                    };
                                                    var liftcarrierinfodata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                                    string infodata = liftcarrierinfodata.Rows.Count != 0 ? liftcarrierinfodata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                                    SAA_Database.LogMessage($"【{liftCarrierInfo.STATION_NAME}】【查詢來源站點】來源站點:{infodata}，卡匣ID:{liftCarrierInfo.CARRIERID}");
                                                    if (devicetype == SAA_Database.SaaCommon.DeivertTypeUD.ToString())
                                                    {
                                                        if (infodata == "PGV-IN")
                                                        {
                                                            SaaScLiftCarrierInfoEmpty LiftCarrierInfoEmpty = new SaaScLiftCarrierInfoEmpty()
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
                                                            if (!equipmentcarrierinfo.CARRIERID.Contains(SAA_Database.configattributes.PARTICLE))
                                                            {
                                                                SAA_Database.SaaSql.SetScLiftCarrierInfoEmpty(LiftCarrierInfoEmpty);
                                                                SAA_Database.LogMessage($"【新增空盒卡匣】新增ScLiftCarrierInfoEmpty(資料表完成，站點:{LiftCarrierInfoEmpty.STATION_NAME}，卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!reportcommand.DESTINATION.Contains("BUFF"))
                                                        {
                                                            if (infodata == "PGV-IN")
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
                                                                SAA_Database.SaaSql.SetScLiftCarrierInfoMaterial(LiftCarrierInfoMaterial);
                                                                SAA_Database.LogMessage($"【新增實盒卡匣】新增ScLiftCarrierInfoMaterial資料表完成，站點:{LiftCarrierInfoMaterial.STATION_NAME}，卡匣ID:{LiftCarrierInfoMaterial.CARRIERID}");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!equipmentcarrierinfo.DESTINATION.Contains(equipmentcarrierinfo.STATIOM_NAME))
                                                {
                                                    carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                                                    SaaEsReportTransportRequirement EsReportTransport = new SaaEsReportTransportRequirement
                                                    {
                                                        STATION = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.STATION_NAME.ToString()].ToString() : string.Empty,
                                                        DATATIME = SAA_Database.ReadTime(),
                                                        CARRIERID = equipmentcarrierinfo.CARRIERID,
                                                        BEGINSTATION = reportcommand.ORIGIN,
                                                        ENDSTATION = reportcommand.DESTINATION,
                                                    };
                                                    EsReportTransport.TEID = $"{EsReportTransport.STATION}_{EsReportTransport.DATATIME}";

                                                    SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                                                    {
                                                        SETNO = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                                                        MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                                        STATION_NAME = EsReportTransport.STATION,
                                                        REPORT_STATION = EsReportTransport.STATION,
                                                        REPORT_TIME = EsReportTransport.DATATIME,
                                                        CARRIERID = EsReportTransport.CARRIERID,
                                                        REQUIREMENT_TYPE = "2",
                                                        BEGIN_STATION = EsReportTransport.BEGINSTATION,
                                                        END_STATION = EsReportTransport.ENDSTATION,
                                                    };
                                                    ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                                                    if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                                                    {
                                                        SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                                                        SAA_Database.LogMessage($"【新增資料】【天車】新增料盒搬運需求:站點:{EsReportTransport.STATION}、起點{EsReportTransport.BEGINSTATION}、終點:{EsReportTransport.ENDSTATION}、卡匣ID:{EsReportTransport.CARRIERID}");
                                                    }
                                                    else
                                                    {
                                                        SAA_Database.LogMessage($"【新增資料】【天車】已有需求無法新增料盒搬運需求:站點:{EsReportTransport.STATION}、起點{EsReportTransport.BEGINSTATION}、終點:{EsReportTransport.ENDSTATION}、卡匣ID:{EsReportTransport.CARRIERID}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var lcsreceivedata = SAA_Database.SaaSql.GetScDevice(int.Parse(SaaDirective.SETNO), SaaDirective.STATION_NAME);
                                        if (lcsreceivedata.Rows.Count != 0)
                                        {
                                            if (lcsreceivedata.Rows[0][SC_DEVICE.DEVICETYPE.ToString()].ToString() == SAA_Database.SaaCommon.DeivertTypeUD.ToString())
                                            {
                                                if (!equipmentcarrierinfo.DESTINATION.Contains(equipmentcarrierinfo.STATIOM_NAME))
                                                {
                                                    SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                                                    {
                                                        SETNO = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                                                        MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                                        STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                        REPORT_STATION = equipmentcarrierinfo.STATIOM_NAME,
                                                        REPORT_TIME = SAA_Database.ReadTeid(),
                                                        CARRIERID = reportcommand.CARRIER,
                                                        REQUIREMENT_TYPE = "2",
                                                        BEGIN_STATION = reportcommand.ORIGIN,
                                                        END_STATION = reportcommand.DESTINATION,
                                                    };
                                                    ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                                                    if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                                                    {
                                                        SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                                                        SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增資料】【天車】新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                                    }
                                                    else
                                                    {
                                                        SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】【新增資料】【天車】已有需求無法新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case LcsReceive.M004:
                                case LcsReceive.M504:
                                    if (reportcommand.DESTINATION.Contains(equipmentcarrierinfo.STATIOM_NAME))
                                    {
                                        if (reportcommand.ORIGIN.Contains("BUFF"))
                                        {
                                            SaaScLiftCarrierInfoReject CarrierInfoReject = new SaaScLiftCarrierInfoReject
                                            {
                                                TASKDATETIME = SAA_Database.ReadTime(),
                                                SETNO = scdevicedata.Rows.Count != 0 ? int.Parse(scdevicedata.Rows[0][SC_DEVICE.SETNO.ToString()].ToString()) : int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                CARRIERID = reportcommand.CARRIER,
                                                DEVICETYPE = reportcommand.SCHEDULE=="NA"?"1":"2",
                                                CYCLETIME = reportcommand.CYCLETIME,
                                                QTIME = reportcommand.QTIME,
                                            };
                                            SAA_Database.SaaSql?.SetScLiftCarrierInfoReject(CarrierInfoReject);
                                            SAA_Database.LogMessage($"【{CarrierInfoReject.STATION_NAME}】【監控上報】新增退盒資料表SC_LIFT_CARRIER_INFO_REJECT 卡匣ID: ({CarrierInfoReject.CARRIERID}) ");
                                        }
                                        else
                                        {
                                            SAA_Database.SaaSql.UpdScScPurchase(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.N.ToString());
                                            SAA_Database.SaaSql.UpdScEquipmentCarrierInfoSendFlag(equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.N.ToString());
                                            SAA_Database.LogMessage($"【{equipmentcarrierinfo.STATIOM_NAME}】更新資料回覆資料 機台編號:{equipmentcarrierinfo.SETNO} 站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}，更新為:N(Y:進盒，N:REJECT)");
                                            SaaScLiftCarrierInfoReject CarrierInfoReject = new SaaScLiftCarrierInfoReject
                                            {
                                                TASKDATETIME = SAA_Database.ReadTime(),
                                                SETNO = scdevicedata.Rows.Count != 0 ? int.Parse(scdevicedata.Rows[0][SC_DEVICE.SETNO.ToString()].ToString()) : int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                CARRIERID = reportcommand.CARRIER,
                                                DEVICETYPE = reportcommand.SCHEDULE == "NA" ? "1" : "2",
                                                CYCLETIME = reportcommand.CYCLETIME,
                                                QTIME = reportcommand.QTIME,
                                            };
                                            SAA_Database.SaaSql?.SetScLiftCarrierInfoReject(CarrierInfoReject);
                                            SAA_Database.LogMessage($"【{CarrierInfoReject.STATION_NAME}】【監控上報】新增退盒資料表SC_LIFT_CARRIER_INFO_REJECT 卡匣ID: ({CarrierInfoReject.CARRIERID}) ");
                                        }
                                    }
                                    else
                                    {
                                        var lcsreceivedata = SAA_Database.SaaSql.GetScDevice(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME);
                                        if (lcsreceivedata.Rows.Count != 0)
                                        {
                                            //if (lcsreceivedata.Rows[0][SC_DEVICE.DEVICETYPE.ToString()].ToString() == "2")
                                            //{
                                            SAA_Database.SaaSql.UpdScScPurchase(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.Y.ToString());
                                            SAA_Database.SaaSql.UpdScEquipmentCarrierInfoSendFlag(equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SaaSendReply.Y.ToString());
                                            SAA_Database.LogMessage($"更新資料回覆資料 機台編號:{equipmentcarrierinfo.SETNO} 站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}，更新為:Y(Y:進盒，N:REJECT)");
                                            if (!equipmentcarrierinfo.DESTINATION.Contains(equipmentcarrierinfo.STATIOM_NAME))
                                            {
                                                SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                                                {
                                                    SETNO = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.SETNO.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                                                    MODEL_NAME = scdevicedata.Rows.Count != 0 ? scdevicedata.Rows[0][SC_DEVICE.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                                    STATION_NAME = equipmentcarrierinfo.STATIOM_NAME,
                                                    REPORT_STATION = equipmentcarrierinfo.STATIOM_NAME,
                                                    REPORT_TIME = SAA_Database.ReadTeid(),
                                                    CARRIERID = reportcommand.CARRIER,
                                                    REQUIREMENT_TYPE = "2",
                                                    BEGIN_STATION = reportcommand.ORIGIN,
                                                    END_STATION = reportcommand.DESTINATION,
                                                };
                                                ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                                                if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                                                {
                                                    SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                                                    SAA_Database.LogMessage($"【{ScTransportrEquirement.STATION_NAME}】【新增資料【天車】】新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                                }
                                                else
                                                {
                                                    SAA_Database.LogMessage($"【{ScTransportrEquirement.STATION_NAME}】【新增資料【天車】】已有需求無法新增料盒搬運需求:站點:{ScTransportrEquirement.STATION_NAME}、起點{ScTransportrEquirement.BEGIN_STATION}、終點:{ScTransportrEquirement.END_STATION}、卡匣ID:{ScTransportrEquirement.CARRIERID}");
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            SaaDirective.SENDFLAG = SendFlag.Y.ToString();
                            SAA_Database.SaaSql.UpdScDirective(SaaDirective);
                            SAA_Database.SaaSql.SetScDirectiveHistory(SaaDirective, SAA_Database.ReadTime());
                        }
                    }
                }

                #region [===UD水位要盒需求===]
                var devicedata = SAA_Database.SaaSql.GetScDevice();
                if (devicedata.Rows.Count != 0)
                {
                    foreach (DataRow item in devicedata.Rows)
                    {
                        if (item[SC_DEVICE.DEVICETYPE.ToString()].ToString() == "2")
                        {
                            SaaScLiftCarrierInfo equipmentcarrierinfo = new SaaScLiftCarrierInfo
                            {
                                SETNO = int.Parse(item[SC_DEVICE.SETNO.ToString()].ToString()),
                                MODEL_NAME = item[SC_DEVICE.MODEL_NAME.ToString()].ToString(),
                                CARRIERTYPE = item[SC_DEVICE.DEVICETYPE.ToString()].ToString(),
                                STATION_NAME = item[SC_DEVICE.STATION_NAME.ToString()].ToString()
                            };

                            var carrierinfoemptydata = SAA_Database.SaaSql.GetLiftCarrierInfoEmpty(equipmentcarrierinfo.STATION_NAME);
                            if (carrierinfoemptydata.Rows.Count < SAA_Database.SaaCommon.LiftCarrierInfoEmptyCount)
                            {
                                if (SAA_Database.SaaSql.GetScScLiftAmount(equipmentcarrierinfo.STATION_NAME).Rows.Count == 0)
                                {
                                    string station = equipmentcarrierinfo.STATION_NAME;
                                    SAA_Database.LogMessage($"建立空盒需求:站點:{station}");
                                    string dataTime = SAA_Database.ReadTime();
                                    string TEID = $"{station}_{dataTime}";
                                    List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
                                    RequirementInfo info = new RequirementInfo
                                    {
                                        RequirementType = RequirementType.Take_In_EmptyCarrier.ToString(),
                                        CarrierID = string.Empty,
                                        BeginStation = string.Empty,
                                        EndStation = station,
                                    };
                                    requirementinfo.Add(info);

                                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                                    {
                                        { ES_Report_TransportRequirement.StationID.ToString(),  station },
                                        { ES_Report_TransportRequirement.Time.ToString(), dataTime },
                                        { ES_Report_TransportRequirement.TEID.ToString(), TEID },
                                        { ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo }
                                    };
                                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                                    SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{station}，時間:{dataTime}，傳送編號:{TEID}傳送內容:{commandcontent}");
                                    string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_TransportRequirement.ToString());
                                    SaaReportResult saareportresult = WebApiReportResult(returnresult);
                                    SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                                    if (saareportresult != null)
                                    {
                                        if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success || saareportresult.ReturnCode == "1")
                                        {
                                            SaaScLiftAmount ScLiftAmount = new SaaScLiftAmount
                                            {
                                                SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = station,
                                                TASKDATETIME = dataTime,
                                            };
                                            SAA_Database.SaaSql.SetScLiftAmount(ScLiftAmount);
                                            SAA_Database.LogMessage($"【新增LiftAmount】新增{ScLiftAmount.STATION_NAME}資料完成");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region [===LD出空盒卡匣邏輯===]
                var TransportrEquirementdata = SAA_Database.SaaSql.GetScTransportrEquirement();
                if (TransportrEquirementdata.Rows.Count != 0)
                {
                    foreach (DataRow dr in TransportrEquirementdata.Rows)
                    {
                        var rejectdata = SAA_Database.SaaSql.GetScLiftCarrierInfoReject(dr["CARRIERID"].ToString());
                        if (rejectdata.Rows.Count == 0)
                        {
                            var carrierinfodata = SAA_Database.SaaSql.GetScScLocationsettingCarrierId(dr["STATION_NAME"].ToString(), dr["CARRIERID"].ToString());
                            if(carrierinfodata.Rows.Count != 0)
                            {
                                string station = dr["STATION_NAME"].ToString();
                                SAA_Database.LogMessage($"傳送空盒送出:站點:{station}");
                                string dataTime = SAA_Database.ReadTime();
                                string TEID = $"{station}_{dataTime}";
                                List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
                                RequirementInfo info = new RequirementInfo
                                {
                                    RequirementType = RequirementType.Take_out_EmptyCarrier.ToString(),
                                    CarrierID = dr["CARRIERID"].ToString(),
                                    BeginStation = station,
                                    EndStation = string.Empty,
                                };
                                requirementinfo.Add(info);

                                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                                 {
                                     { ES_Report_TransportRequirement.StationID.ToString(),  station },
                                     { ES_Report_TransportRequirement.Time.ToString(), dataTime },
                                     { ES_Report_TransportRequirement.TEID.ToString(), TEID },
                                     { ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo }
                                 };
                                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                                SAA_Database.LogMessage($"【LCS->iLIS】【Take_out_ EmptyCarrier】【通訊傳送】站點:{station}，時間:{dataTime}，傳送編號:{TEID}傳送內容:{commandcontent}");
                                string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_TransportRequirement.ToString());
                                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                                if (saareportresult != null)
                                {
                                    SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                                    //if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
                                    //{
                                    SAA_Database.SaaSql.UpdScTransportrEquirement(info.CarrierID, SendFlag.Y.ToString());
                                    //}
                                }
                            }
                        }
                    }
                }
                #endregion

                var TransportrEquirementMaterial = SAA_Database.SaaSql.GetScTransportrEquirementMaterial();
                if (TransportrEquirementMaterial.Rows.Count != 0)
                {
                    foreach (DataRow dr in TransportrEquirementMaterial.Rows)
                    {
                        var rejectdata = SAA_Database.SaaSql.GetScLiftCarrierInfoReject(dr["CARRIERID"].ToString());
                        if (rejectdata.Rows.Count == 0)
                        {
                            var carrierinfodata = SAA_Database.SaaSql.GetScScLocationsettingCarrierId(dr["STATION_NAME"].ToString(), dr["CARRIERID"].ToString());
                            if (carrierinfodata.Rows.Count != 0)
                            {
                                SaaEsReportTransportRequirement EsReportTransport = new SaaEsReportTransportRequirement
                                {
                                    STATION = dr["STATION_NAME"].ToString(),
                                    DATATIME = SAA_Database.ReadTeid(),
                                    CARRIERID = dr["CARRIERID"].ToString(),
                                    BEGINSTATION = dr["BEGIN_STATION"].ToString(),
                                    ENDSTATION = dr["END_STATION"].ToString(),
                                };
                                EsReportTransport.TEID = $"{EsReportTransport.STATION}_{EsReportTransport.DATATIME}";
                                if (EsReportTransport.STATION != string.Empty)
                                    WebApiSendEsReportTransportRequirement(EsReportTransport);
                                SAA_Database.SaaSql.UpdScTransportrEquirementMaterial(EsReportTransport.CARRIERID, SendFlag.Y.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.iLIs);
            }
        }
        #endregion

        #region [===上報搬運指令===]
        public void WebApiEquipmentReport()
        {
            try
            {
                var equipmentreportdata = SAA_Database.SaaSql.GetScEquipmentReport();
                if (equipmentreportdata != null)
                {
                    foreach (DataRow dr in equipmentreportdata.Rows)
                    {
                        SaaScEquipmentReport equipmentreportt = new SaaScEquipmentReport
                        {
                            TASKDATETIME = dr[SC_EQUIPMENT_REPORT.TASKDATETIME.ToString()].ToString(),
                            SETNO = dr[SC_EQUIPMENT_REPORT.SETNO.ToString()].ToString(),
                            MODEL_NAME = dr[SC_EQUIPMENT_REPORT.MODEL_NAME.ToString()].ToString(),
                            STATION_NAME = dr[SC_EQUIPMENT_REPORT.STATION_NAME.ToString()].ToString(),
                            CARRIERID = dr[SC_EQUIPMENT_REPORT.CARRIERID.ToString()].ToString(),
                            REPORE_DATATRACK = dr[SC_EQUIPMENT_REPORT.REPORE_DATATRACK.ToString()].ToString(),
                            REPORE_DATAREMOTE = dr[SC_EQUIPMENT_REPORT.REPORE_DATAREMOTE.ToString()].ToString(),
                            REPORE_DATALOCAL = dr[SC_EQUIPMENT_REPORT.REPORE_DATALOCAL.ToString()].ToString(),
                        };
                        SaaEquipmentCarrierInfo equipmentcarrierinfo = new SaaEquipmentCarrierInfo
                        {
                            SETNO = int.Parse(equipmentreportt.SETNO),
                            MODEL_NAME = equipmentreportt.MODEL_NAME,
                            STATIOM_NAME = equipmentreportt.STATION_NAME,
                            CARRIERID = equipmentreportt.CARRIERID,
                        };
                        if (equipmentcarrierinfo.STATIOM_NAME != SAA_Database.configattributes.SaaVST101StationName)
                        {
                            var carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                            if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Move)
                            {
                                if (equipmentreportt.REPORE_DATAREMOTE == "RGV" && equipmentreportt.REPORE_DATALOCAL == "CRANE")
                                {
                                    SAA_Database.SaaSql.DelLiftAmount(equipmentcarrierinfo.STATIOM_NAME);
                                    SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_AMOUNT 資料站點名稱:{equipmentcarrierinfo.STATIOM_NAME}:，資訊刪除完成");
                                }

                                if (equipmentreportt.REPORE_DATAREMOTE != "Stage" && equipmentreportt.REPORE_DATALOCAL != "Stage")
                                {
                                    if (equipmentreportt.REPORE_DATAREMOTE != "PGV-IN")
                                    {
                                        if (equipmentreportt.REPORE_DATALOCAL != "PGV-OUT")
                                        {
                                            if (equipmentreportt.REPORE_DATAREMOTE != "DK-OUT")
                                            {
                                                #region [===正常搬運上報===]
                                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                                var shuttledata = SAA_Database.SaaSql.GetScLiftE84PcStatsus(equipmentreportt.STATION_NAME);
                                                string shuttleid = shuttledata.Rows.Count != 0 ? shuttledata.Rows[0]["SHUTTLEID"].ToString() : "0";
                                                Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                                {
                                                    { "CMD_NO", "S002" },
                                                    { "CMD_NAME", "CARRIER_INTO_MECHANISM" },
                                                    { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                                    { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                    { "ORIGIN",hostidremote.Rows.Count!=0?hostidremote.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidremote.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidremote.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                    { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidlocal.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                };
                                                string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                                SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S002", commandcontent, ReportSource.LCS);
                                                #endregion
                                            }
                                        }

                                        if (equipmentreportt.REPORE_DATALOCAL == "PGV-OUT")
                                        {
                                            #region [===退盒上報===]
                                            var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                            var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                            var shuttledata = SAA_Database.SaaSql.GetScLiftE84PcStatsus(equipmentreportt.STATION_NAME);
                                            string shuttleid = shuttledata.Rows.Count != 0 ? shuttledata.Rows[0]["SHUTTLEID"].ToString() : "0";
                                            string reject_code = string.Empty;
                                            string reject_message = string.Empty;
                                            SaaScLiftCarrierInfo CarrierInfo = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = int.Parse(equipmentreportt.SETNO),
                                                MODEL_NAME = equipmentreportt.MODEL_NAME,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                            };

                                            var SaaCarrierInfo = SAA_Database.SaaSql.GetLiftCarrierInfo(CarrierInfo);
                                            string carrierinfo = string.Empty;
                                            if (SaaCarrierInfo.Rows.Count != 0)
                                            {
                                                carrierinfo = SaaCarrierInfo.Rows[0]["READPLC"].ToString();
                                                if (SaaCarrierInfo.Rows[0]["READPLC"].ToString() == "99")
                                                {
                                                    Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                                    {
                                                        { "CMD_NO", "M004" },
                                                        { "CMD_NAME", "CARRIER_REJECT" },
                                                        { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                                        { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                        { "ORIGIN",hostidremote.Rows.Count!=0?hostidremote.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidremote.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidremote.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                        { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                        { "REJECT_CODE", "RS0004"},
                                                        { "REJECT_MESSAGE","Residual_substrate"}
                                                    };
                                                    string commandcontent1 = JsonConvert.SerializeObject(CarrierReject);
                                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "M004", commandcontent1, ReportSource.LCS);
                                                    equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                                    SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                                }
                                            }

                                            var fulldata = SAA_Database.SaaSql.GetScScLocationsettingFull(CarrierInfo.SETNO, CarrierInfo.MODEL_NAME, CarrierInfo.STATION_NAME);
                                            if (equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError))
                                            {
                                                reject_code = "RS0002";
                                                reject_message = "Carrier_1D_fail";
                                            }
                                            else if (carrierinfo == "99")
                                            {
                                                reject_code = "RS0004";
                                                reject_message = "Residual_substrate";
                                            }
                                            else if (fulldata.Rows.Count == 0)
                                            {
                                                reject_code = "RS0006";
                                                reject_message = "WIP_full";
                                            }
                                            else
                                            {
                                                reject_code = carrierinfodata.Rows.Count != 0 ? carrierinfodata.Rows[0]["REJECT_CODE"].ToString() : SAA_Database.SaaCommon.NA;
                                                reject_message = carrierinfodata.Rows.Count != 0 ? carrierinfodata.Rows[0]["REJECT_MESSAGE"].ToString() : SAA_Database.SaaCommon.NA;
                                            }
                                            SaaScRrejectHistory RrejectHistory = new SaaScRrejectHistory
                                            {
                                                REJECT_TIME = SAA_Database.ReadTime(),
                                                SETNO = int.Parse(equipmentreportt.SETNO),
                                                MODEL_NAME = equipmentreportt.MODEL_NAME,
                                                STATION = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError) ? "ERROR" : equipmentreportt.CARRIERID,
                                                PARTNO = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString()) ? carrierinfodata.Rows[0]["PARTNO"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                                REMOTE_REJECT_CODE = reject_code,
                                                REMOTE_REJECT_MSG = reject_message,
                                            };
                                            SAA_Database.SaaSql.SetScRejectHistory(RrejectHistory);

                                            Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                            {
                                                { "CMD_NO", "S004" },
                                                { "CMD_NAME", "CARRIER_REJECT" },
                                                { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                                { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                { "ORIGIN",hostidremote.Rows.Count!=0?hostidremote.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidremote.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidremote.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidlocal.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                                { "REJECT_CODE", reject_code},
                                                { "REJECT_MESSAGE", reject_message}
                                            };
                                            string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                            SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S004", commandcontent, ReportSource.LCS);
                                            SaaScLiftCarrierInfo saaScLiftCarrier = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                            };
                                            SAA_Database.SaaSql.DelLiftCarrierInfo(saaScLiftCarrier);
                                            SAA_Database.LogMessage($"【{CarrierInfo.STATION_NAME}】【刪除資訊】刪除SC_LIFT_CARRIER_INFO卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                            SAA_Database.SaaSql.DelScTransportrEquirement(CarrierInfo.STATION_NAME, CarrierInfo.CARRIERID);
                                            SAA_Database.LogMessage($"【{CarrierInfo.STATION_NAME}】【刪除資訊】刪除SC_TRANSPORTR_EQUIREMENT(空盒叫車資料表)卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                            SAA_Database.SaaSql.DelScTransportrEquirementMaterial(CarrierInfo.STATION_NAME, CarrierInfo.CARRIERID);
                                            SAA_Database.LogMessage($"【{CarrierInfo.STATION_NAME}】【刪除資訊】刪除SC_TRANSPORTR_EQUIREMENT_MATERIAL(實盒叫車資料表)卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                            #endregion
                                        }

                                        if (equipmentreportt.REPORE_DATAREMOTE == "CRANE" && equipmentreportt.REPORE_DATALOCAL == "RGV")
                                        {
                                            SaaScLiftCarrierInfo saaScLiftCarrier = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                            };
                                            SAA_Database.SaaSql.DelLiftCarrierInfo(saaScLiftCarrier);
                                            SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                        }

                                        #region [===放入製程機上報===]
                                        if (equipmentreportt.REPORE_DATALOCAL == "DK-IN")
                                        {
                                            var scdevicedata = SAA_Database.SaaSql.GetScDevice(int.Parse(equipmentreportt.SETNO), equipmentreportt.STATION_NAME);
                                            if (scdevicedata.Rows.Count != 0)
                                            {
                                                if (scdevicedata.Rows[0]["DEVICETYPE"].ToString() == "2")
                                                {
                                                    var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                                    var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                                    Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                                    {
                                                        { "CMD_NO", "S003" },
                                                        { "CMD_NAME", "CARRIER_OUT_OF_MECHANISM" },
                                                        { "CARRIER", equipmentreportt.CARRIERID },
                                                        { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                        { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料" },
                                                    };
                                                    string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S003", commandcontent, ReportSource.LCS);
                                                }
                                            }
                                            else
                                            {
                                                SAA_Database.LogMessage($"【查無站點】查無該站點{equipmentreportt.STATION_NAME}無法上報S003刪除指令", SAA_Database.LogType.Error);
                                            }
                                            equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                            SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                        }
                                        #endregion

                                        if (equipmentreportt.REPORE_DATAREMOTE == "CRANE" && equipmentreportt.REPORE_DATALOCAL == "DK-OUT")
                                        {
                                            SaaScLiftCarrierInfo saaScLiftCarrier = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                            };
                                            SAA_Database.SaaSql.DelLiftCarrierInfo(saaScLiftCarrier);
                                            SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                        }
                                    }
                                }
                                else
                                {
                                    if (equipmentreportt.REPORE_DATAREMOTE == "Stage" && equipmentreportt.REPORE_DATALOCAL == "CRANE")
                                    {
                                        SaaScLiftCarrierInfo liftCarrierInfo = new SaaScLiftCarrierInfo()
                                        {
                                            SETNO = int.Parse(equipmentreportt.SETNO),
                                            MODEL_NAME = equipmentreportt.MODEL_NAME,
                                            STATION_NAME = equipmentreportt.STATION_NAME,
                                            CARRIERID = equipmentreportt.CARRIERID,
                                        };
                                        var liftcarrierinfodata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                        string infodata = liftcarrierinfodata.Rows.Count != 0 ? liftcarrierinfodata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                        SAA_Database.LogMessage($"【查詢來源站點】來源站點:{infodata}");
                                        if ((infodata != "RGV" && infodata != string.Empty) || liftCarrierInfo.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError))
                                        {
                                            #region [===正常搬運上報===]
                                            var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                            var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                            var shuttledata = SAA_Database.SaaSql.GetScLiftE84PcStatsus(equipmentreportt.STATION_NAME);
                                            string shuttleid = shuttledata.Rows.Count != 0 ? shuttledata.Rows[0]["SHUTTLEID"].ToString() : "0";
                                            string order = string.Empty;
                                            SaaScLiftCarrierInfo CarrierInfo = new SaaScLiftCarrierInfo()
                                            {
                                                SETNO = int.Parse(equipmentreportt.SETNO),
                                                MODEL_NAME = equipmentreportt.MODEL_NAME,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                            };
                                            var carrierorigindata = SAA_Database.SaaSql.GetLiftCarrierInfo(CarrierInfo);
                                            string origindata = carrierorigindata.Rows.Count != 0 ? carrierorigindata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                            SAA_Database.LogMessage($"【詢問】【查詢來源站點】來源站點:{infodata}");
                                            var ReportStargData = SAA_Database.SaaSql.GetReportStargName(CarrierInfo.STATION_NAME, infodata);
                                            if (ReportStargData.Rows.Count != 0)
                                                order = ReportStargData.Rows[0]["HOSTID"].ToString();
                                            else
                                                order = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                            SAA_Database.LogMessage($"【查詢起點】查詢起點名稱:{order}");
                                            Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                            {
                                                { "CMD_NO", "S002" },
                                                { "CMD_NAME", "CARRIER_INTO_MECHANISM" },
                                                { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID },
                                                { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                { "ORIGIN",order},
                                                { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString().Contains("RGV")?$"{hostidlocal.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                            };
                                            string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                            SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S003", commandcontent, ReportSource.LCS);
                                            #endregion
                                        }
                                        var devicemodel = SAA_Database.SaaSql?.GetScDeviceModelName(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME);
                                        if (devicemodel.Rows.Count != 0)
                                        {
                                            if (devicemodel?.Rows[0]["DEVICETYPE"].ToString() == "2")//1:LD 2:ULD
                                            {
                                                if (infodata == "PGV-IN" || infodata == "RGV")
                                                {
                                                    var carrierdata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                                                    SaaScLiftCarrierInfoEmpty CarrierInfoEmpty = new SaaScLiftCarrierInfoEmpty
                                                    {
                                                        TASKDATETIME = SAA_Database.ReadTime(),
                                                        SETNO = int.Parse(equipmentreportt.SETNO),
                                                        MODEL_NAME = equipmentreportt.MODEL_NAME,
                                                        STATION_NAME = equipmentreportt.STATION_NAME,
                                                        CARRIERID = equipmentreportt.CARRIERID,
                                                        DEVICETYPE = "2",
                                                        CYCLETIME = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["CYCLETIME"].ToString() : string.Empty,
                                                        QTIME = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["QTIME"].ToString() : string.Empty,
                                                    };
                                                    var emptycarriedata = SAA_Database.SaaSql.GetLiftCarrierInfoEmptyCarrier(CarrierInfoEmpty);
                                                    if (emptycarriedata.Rows.Count != 0)
                                                    {
                                                        SAA_Database.SaaSql.DelLiftCarrierInfoEmptyCarrier(CarrierInfoEmpty);
                                                        SAA_Database.LogMessage($"【空盒卡匣】有相同卡匣資料，刪除空盒卡匣資料，站點:{CarrierInfoEmpty.STATION_NAME}，卡匣ID:{CarrierInfoEmpty.CARRIERID}");
                                                    }
                                                    string carrierstate = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["CARRIERSTATE"].ToString() : string.Empty;
                                                    string destinationtype = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["DESTINATIONTYPE"].ToString() : string.Empty;
                                                    SAA_Database.LogMessage($"【空盒卡匣】空盒卡匣ID:{equipmentreportt.CARRIERID}，CarrierState:{carrierstate}");
                                                    if (!equipmentreportt.CARRIERID.Contains(SAA_Database.configattributes.PARTICLE))
                                                    {
                                                        SAA_Database.SaaSql.SetScLiftCarrierInfoEmpty(CarrierInfoEmpty);
                                                        SAA_Database.LogMessage($"【空盒卡匣】新增卡匣資料，站點:{CarrierInfoEmpty.STATION_NAME}，卡匣ID:{CarrierInfoEmpty.CARRIERID}，CYCLETIME:{CarrierInfoEmpty.CYCLETIME}，QTIME:{CarrierInfoEmpty.QTIME}");
                                                    }
                                                    else
                                                    {
                                                        SAA_Database.LogMessage($"【空盒卡匣】空盒卡匣ID:{equipmentreportt.CARRIERID}，CarrierState:{carrierstate}不為Empty無法新增空盒卡匣資料，或卡匣ID有:{SAA_Database.configattributes.PARTICLE}字串，不可新增空盒卡匣(卡匣ID:{equipmentreportt.CARRIERID})");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var carrierdata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                                                SaaScLiftCarrierInfoEmpty CarrierInfoEmpty = new SaaScLiftCarrierInfoEmpty
                                                {
                                                    TASKDATETIME = SAA_Database.ReadTime(),
                                                    SETNO = int.Parse(equipmentreportt.SETNO),
                                                    MODEL_NAME = equipmentreportt.MODEL_NAME,
                                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                                    CARRIERID = equipmentreportt.CARRIERID,
                                                    DEVICETYPE = "1",
                                                    CYCLETIME = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["CYCLETIME"].ToString() : string.Empty,
                                                    QTIME = carrierdata.Rows.Count != 0 ? carrierdata.Rows[0]["QTIME"].ToString() : string.Empty,
                                                };
                                            }
                                        }
                                    }
                                }
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Clear)
                            {
                                #region [===人員取走上報===]
                                if (equipmentreportt.REPORE_DATALOCAL != "RGV" && equipmentreportt.REPORE_DATALOCAL != "DK-OUT" && equipmentreportt.REPORE_DATALOCAL != "PGV-OUT")
                                {
                                    var scdevicedata = SAA_Database.SaaSql.GetScDevice(int.Parse(equipmentreportt.SETNO), equipmentreportt.STATION_NAME);
                                    if (scdevicedata.Rows.Count != 0)
                                    {
                                        if (equipmentreportt.REPORE_DATALOCAL == "DK-IN")
                                        {
                                            if (scdevicedata.Rows[0]["DEVICETYPE"].ToString() == "2")
                                            {
                                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                                Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                                {
                                                    { "CMD_NO", "S003" },
                                                    { "CMD_NAME", "CARRIER_OUT_OF_MECHANISM" },
                                                    { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                                    { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                                    { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料" },
                                                };
                                                string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                                SaaScReportInadx reportInadx = new SaaScReportInadx()
                                                {
                                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                                    REPORT_NAME = IndexTableName.SC_DIRECTIVE.ToString(),
                                                };
                                                SaaScDirective directive = new SaaScDirective()
                                                {
                                                    TASKDATETIME = SAA_Database.ReadTime(),
                                                    SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                                                    COMMANDON = SAA_Database.ReadRequorIndex(reportInadx).ToString(),
                                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                                    CARRIERID = equipmentreportt.CARRIERID,
                                                    COMMANDID = "S003",
                                                    COMMANDTEXT = commandcontent,
                                                    SOURCE = ReportSource.LCS.ToString(),
                                                };
                                                SAA_Database.SaaSql.SetScDirective(directive);
                                                SAA_Database.LogMessage($"【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_Id:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                                                SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
                                            }
                                        }
                                        else
                                        {
                                            var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                            Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                            {
                                                { "CMD_NO", "D001" },
                                                { "CMD_NAME", "DELETE_CARRIER" },
                                                { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                                { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料" },
                                            };
                                            string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                            SaaScReportInadx reportInadx = new SaaScReportInadx()
                                            {
                                                SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                REPORT_NAME = IndexTableName.SC_DIRECTIVE.ToString(),
                                            };
                                            SaaScDirective directive = new SaaScDirective()
                                            {
                                                TASKDATETIME = SAA_Database.ReadTime(),
                                                SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                                                COMMANDON = SAA_Database.ReadRequorIndex(reportInadx).ToString(),
                                                STATION_NAME = equipmentreportt.STATION_NAME,
                                                CARRIERID = equipmentreportt.CARRIERID,
                                                COMMANDID = "D001",
                                                COMMANDTEXT = commandcontent,
                                                SOURCE = ReportSource.LCS.ToString(),
                                            };
                                            SAA_Database.SaaSql.SetScDirective(directive);
                                            SAA_Database.LogMessage($"【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_Id:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                                            SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
                                        }
                                    }
                                }
                                else if (equipmentreportt.REPORE_DATALOCAL == "PGV-OUT")
                                {
                                    var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                    var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                    Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                        {
                                            { "CMD_NO", "S003" },
                                            { "CMD_NAME", "CARRIER_OUT_OF_MECHANISM" },
                                            { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                            { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                            { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料" },
                                        };
                                    string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                    SaaScReportInadx reportInadx = new SaaScReportInadx()
                                    {
                                        SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                        MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                        STATION_NAME = equipmentreportt.STATION_NAME,
                                        REPORT_NAME = IndexTableName.SC_DIRECTIVE.ToString(),
                                    };
                                    SaaScDirective directive = new SaaScDirective()
                                    {
                                        TASKDATETIME = SAA_Database.ReadTime(),
                                        SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                                        COMMANDON = SAA_Database.ReadRequorIndex(reportInadx).ToString(),
                                        STATION_NAME = equipmentreportt.STATION_NAME,
                                        CARRIERID = equipmentreportt.CARRIERID,
                                        COMMANDID = "S003",
                                        COMMANDTEXT = commandcontent,
                                        SOURCE = ReportSource.LCS.ToString(),
                                    };
                                    SAA_Database.SaaSql.SetScDirective(directive);
                                    SAA_Database.LogMessage($"【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_Id:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                                    SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
                                }
                                #endregion
                                if (equipmentreportt.REPORE_DATALOCAL == "PGV-IN")
                                {
                                    SaaScLiftCarrierInfo saascliftcarrier = new SaaScLiftCarrierInfo()
                                    {
                                        SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                        MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                        STATION_NAME = equipmentreportt.STATION_NAME,
                                        CARRIERID = equipmentreportt.CARRIERID,
                                    };
                                    SAA_Database.SaaSql.DelLiftCarrierInfo(saascliftcarrier);
                                    SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO卡匣ID:{saascliftcarrier.CARRIERID}:，資訊刪除完成");

                                }
                                SaaScLiftCarrierInfoEmpty LiftCarrierInfoEmpty = new SaaScLiftCarrierInfoEmpty()
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                    CARRIERID = equipmentreportt.CARRIERID,
                                };

                                if (equipmentreportt.REPORE_DATALOCAL != "RGV")
                                {
                                    SAA_Database.SaaSql.DelLiftCarrierInfoEmptyCarrier(LiftCarrierInfoEmpty);
                                    SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO_EMPTY卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}:，資訊刪除完成");
                                    SAA_Database.SaaSql.DelLiftCarrierInfoMaterial(LiftCarrierInfoEmpty);
                                    SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO_MATERIAL卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}:，資訊刪除完成");
                                    SAA_Database.SaaSql.DelLiftCarrierInfoReject(LiftCarrierInfoEmpty);
                                    SAA_Database.LogMessage($"【刪除資訊】刪除SC_LIFT_CARRIER_INFO_REJECT卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}:，資訊刪除完成");
                                }
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                SAA_Database.SaaSql.DelScCommandTask(LiftCarrierInfoEmpty.STATION_NAME, LiftCarrierInfoEmpty.CARRIERID);
                                SAA_Database.LogMessage($"【刪除資訊】刪除SC_COMMAND_TASK卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}，站點:{LiftCarrierInfoEmpty.STATION_NAME}，資訊刪除完成");
                                SAA_Database.SaaSql.DelScTransportrEquirement(LiftCarrierInfoEmpty.STATION_NAME, LiftCarrierInfoEmpty.CARRIERID);
                                SAA_Database.LogMessage($"【刪除資訊】刪除SC_TRANSPORTR_EQUIREMENT卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}:，資訊刪除完成");
                                SAA_Database.SaaSql.DelScTransportrEquirementMaterial(LiftCarrierInfoEmpty.STATION_NAME, LiftCarrierInfoEmpty.CARRIERID);
                                SAA_Database.LogMessage($"【刪除資訊】刪除SC_TRANSPORTR_EQUIREMENT_MATERIAL卡匣ID:{LiftCarrierInfoEmpty.CARRIERID}:，資訊刪除完成");
                                SaaScLiftCarrierInfo saaScLiftCarrier = new SaaScLiftCarrierInfo()
                                {
                                    SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                    MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName,
                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                    CARRIERID = equipmentreportt.CARRIERID,
                                };
                                SAA_Database.SaaSql?.DelLiftCarrierInfo(saaScLiftCarrier);
                                SAA_Database.LogMessage($"【{saaScLiftCarrier.STATION_NAME}】【刪除資訊】刪除SC_LIFT_CARRIER_INFO卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                                SAA_Database.SaaSql.DelScTransportrEquirement(saaScLiftCarrier.STATION_NAME, saaScLiftCarrier.CARRIERID);
                                SAA_Database.LogMessage($"【{saaScLiftCarrier.STATION_NAME}】【刪除資訊】刪除SC_TRANSPORTR_EQUIREMENT(空盒叫車資料表)卡匣ID:{saaScLiftCarrier.CARRIERID}:，資訊刪除完成");
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Ask)
                            {
                                #region [===詢問上報===]
                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                string cmdname = string.Empty;
                                string station = string.Empty;
                                SaaScLiftCarrierInfo liftCarrierInfo = new SaaScLiftCarrierInfo()
                                {
                                    SETNO = int.Parse(equipmentreportt.SETNO),
                                    MODEL_NAME = equipmentreportt.MODEL_NAME,
                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                    CARRIERID = equipmentreportt.CARRIERID,
                                };
                                var liftcarrierinfodata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                string infodata = liftcarrierinfodata.Rows.Count != 0 ? liftcarrierinfodata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                SAA_Database.LogMessage($"【詢問】【查詢來源站點】來源站點:{infodata}");
                                var ReportStargData = SAA_Database.SaaSql.GetReportStargName(liftCarrierInfo.STATION_NAME, infodata);
                                if (ReportStargData.Rows.Count != 0)
                                {
                                    station = ReportStargData.Rows[0]["HOSTID"].ToString();
                                    cmdname = ReportStargData.Rows[0]["COMMAND_NAME"].ToString();
                                }
                                else
                                {
                                    station = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                    cmdname = "查無此站點";
                                }
                                SAA_Database.LogMessage($"【{liftCarrierInfo.STATION_NAME}】【查詢起點】查詢起點名稱:{station}");
                                SAA_Database.LogMessage($"【{liftCarrierInfo.STATION_NAME}】【查詢詢問】{infodata}查詢命令名稱:{cmdname}");

                                if (infodata == "PGV-IN")
                                {
                                    var fulldata = SAA_Database.SaaSql.GetScScLocationsettingFull(liftCarrierInfo.SETNO, liftCarrierInfo.MODEL_NAME, liftCarrierInfo.STATION_NAME);
                                    if (fulldata.Rows.Count == 0)
                                    {
                                        SAA_Database.SaaSql.UpdScScPurchase(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, SendFlag.N.ToString());
                                        SAA_Database.LogMessage($"更新資料回覆資料 機台編號:{equipmentcarrierinfo.SETNO} 站點:{equipmentcarrierinfo.STATIOM_NAME}，卡匣ID:{equipmentcarrierinfo.CARRIERID}，更新為:{SendFlag.N}(Y:進盒，N:REJECT)");
                                        SAA_Database.LogMessage($"【倉儲已滿】倉儲已滿無法放料退盒上報M004");
                                        hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "PGV-OUT");
                                        Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                        {
                                            { "CMD_NO", "M004" },
                                            { "CMD_NAME", "CARRIER_REJECT" },
                                            { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                            { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                            { "ORIGIN",station},
                                            { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                            { "REJECT_CODE", "RS0006"},
                                            { "REJECT_MESSAGE","WIP_full"}
                                        };
                                        string commandcontent1 = JsonConvert.SerializeObject(CarrierReject);
                                        SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "M004", commandcontent1, ReportSource.LCS);
                                        equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                        SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                    }
                                    else
                                    {
                                        CommandReportQ001 reportQ001 = new CommandReportQ001
                                        {
                                            CMD_NO = AseCommandNo.Q001.ToString(),
                                            CMD_NAME = cmdname,
                                            CARRIER = equipmentreportt.CARRIERID,
                                            STATION = station,
                                        };
                                        WebApiSendCommandQ001(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, reportQ001);
                                        equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                        SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                    }
                                }
                                else
                                {
                                    CommandReportQ001 reportQ001 = new CommandReportQ001
                                    {
                                        CMD_NO = AseCommandNo.Q001.ToString(),
                                        CMD_NAME = cmdname,
                                        CARRIER = equipmentreportt.CARRIERID,
                                        STATION = station,
                                    };
                                    WebApiSendCommandQ001(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, reportQ001);
                                    equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                    SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                                }
                                #endregion
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Update)
                            {
                                if (equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError))
                                {
                                    var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "PGV-OUT");
                                    var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "Stage");
                                    string order = string.Empty;
                                    SaaScLiftCarrierInfo liftCarrierInfErr = new SaaScLiftCarrierInfo()
                                    {
                                        SETNO = int.Parse(equipmentreportt.SETNO),
                                        MODEL_NAME = equipmentreportt.MODEL_NAME,
                                        STATION_NAME = equipmentreportt.STATION_NAME,
                                        CARRIERID = equipmentreportt.CARRIERID,
                                    };
                                    var liftcarrierinfoErrdata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfErr);
                                    string infoerrdata = liftcarrierinfoErrdata.Rows.Count != 0 ? liftcarrierinfoErrdata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                    SAA_Database.LogMessage($"【查詢來源站點】來源站點:{infoerrdata}");
                                    var ReportStargData = SAA_Database.SaaSql.GetReportStargName(liftCarrierInfErr.STATION_NAME, infoerrdata);
                                    if (ReportStargData.Rows.Count != 0)
                                        order = ReportStargData.Rows[0]["HOSTID"].ToString();
                                    else
                                        order = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                    SAA_Database.LogMessage($"【查詢起點】查詢起點名稱:{order}");

                                    Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                     {
                                         { "CMD_NO", "M004" },
                                         { "CMD_NAME", "CARRIER_REJECT" },
                                         { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                         { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                         { "ORIGIN",order},
                                         { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                         { "REJECT_CODE", "RS0002"},
                                         { "REJECT_MESSAGE","Carrier_1D_fail"}
                                     };
                                    string commandcontent = JsonConvert.SerializeObject(CarrierReject);
                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "M004", commandcontent, ReportSource.LCS);
                                }
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == "99")
                            {
                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "Stage");
                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "PGV-OUT");

                                string order = string.Empty;
                                SaaScLiftCarrierInfo liftCarrierInfErr = new SaaScLiftCarrierInfo()
                                {
                                    SETNO = int.Parse(equipmentreportt.SETNO),
                                    MODEL_NAME = equipmentreportt.MODEL_NAME,
                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                    CARRIERID = equipmentreportt.CARRIERID,
                                };
                                var liftcarrierinfoErrdata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfErr);
                                string infoerrdata = liftcarrierinfoErrdata.Rows.Count != 0 ? liftcarrierinfoErrdata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                SAA_Database.LogMessage($"【查詢來源站點】來源站點:{infoerrdata}");
                                var ReportStargData = SAA_Database.SaaSql.GetReportStargName(liftCarrierInfErr.STATION_NAME, infoerrdata);
                                if (ReportStargData.Rows.Count != 0)
                                    order = ReportStargData.Rows[0]["HOSTID"].ToString();
                                else
                                    order = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                SAA_Database.LogMessage($"【查詢起點】查詢起點名稱:{order}");


                                Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                    {
                                        { "CMD_NO", "M004" },
                                        { "CMD_NAME", "CARRIER_REJECT" },
                                        { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                        { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                        { "ORIGIN",order},
                                        { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                        { "REJECT_CODE", "RS0004"},
                                        { "REJECT_MESSAGE","Residual_substrate"}
                                    };
                                string commandcontent = JsonConvert.SerializeObject(CarrierReject);
                                SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "M004", commandcontent, ReportSource.LCS);
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == "105")
                            {
                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                CommandReportM001 commandreportm001 = new CommandReportM001
                                {
                                    CMD_NO = AseCommandNo.M001.ToString(),
                                    CMD_NAME = AseCommandName.CARRIER_TO_MECHANISM.ToString(),
                                    CARRIER = equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError) ? "ERROR" : equipmentreportt.CARRIERID,
                                    SCHEDULE = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString()) ? carrierinfodata.Rows[0]["PARTNO"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                    OPER = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["OPER"].ToString()) ? carrierinfodata.Rows[0]["OPER"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                    RECIPE = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["RECIPE"].ToString()) ? carrierinfodata.Rows[0]["RECIPE"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                    ORIGIN = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料",
                                    DESTINATION = hostidlocal.Rows.Count != 0 ? hostidlocal.Rows[0]["HOSTID"].ToString() : "查無資料",
                                    QTIME = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["QTIME"].ToString()) ? carrierinfodata.Rows[0]["QTIME"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                    CYCLETIME = carrierinfodata.Rows.Count != 0 ? !string.IsNullOrEmpty(carrierinfodata.Rows[0]["CYCLETIME"].ToString()) ? carrierinfodata.Rows[0]["CYCLETIME"].ToString() : SAA_Database.SaaCommon.NA : SAA_Database.SaaCommon.NA,
                                };
                                WebApiSendCommandM001(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, commandreportm001);
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == "405")
                            {

                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                SaaScLiftCarrierInfo liftCarrierInfo = new SaaScLiftCarrierInfo()
                                {
                                    SETNO = int.Parse(equipmentreportt.SETNO),
                                    MODEL_NAME = equipmentreportt.MODEL_NAME,
                                    STATION_NAME = equipmentreportt.STATION_NAME,
                                    CARRIERID = equipmentreportt.CARRIERID,
                                };
                                var liftcarrierinfodata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                string infodata = liftcarrierinfodata.Rows.Count != 0 ? liftcarrierinfodata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                string order = string.Empty;
                                var liftcarrierinfoErrdata = SAA_Database.SaaSql.GetLiftCarrierInfo(liftCarrierInfo);
                                string infoerrdata = liftcarrierinfoErrdata.Rows.Count != 0 ? liftcarrierinfoErrdata.Rows[0]["REMOTE"].ToString() : string.Empty;
                                SAA_Database.LogMessage($"【查詢來源站點】來源站點:{infoerrdata}");
                                var ReportStargData = SAA_Database.SaaSql.GetReportStargName(liftCarrierInfo.STATION_NAME, infoerrdata);
                                if (equipmentreportt.REPORE_DATAREMOTE == "Stage" || equipmentreportt.REPORE_DATALOCAL == "Stage")
                                {
                                    if (ReportStargData.Rows.Count != 0)
                                        order = ReportStargData.Rows[0]["HOSTID"].ToString();
                                    else
                                        order = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                }
                                else
                                {
                                    order = hostidremote.Rows.Count != 0 ? hostidremote.Rows[0]["HOSTID"].ToString() : "查無資料";
                                }
                                SAA_Database.LogMessage($"【查詢起點】查詢起點名稱:{order}");
                                SaaEquipmentCarrierInfo equipmentcarrierinforeject = new SaaEquipmentCarrierInfo
                                {
                                    SETNO = liftCarrierInfo.SETNO,
                                    MODEL_NAME = liftCarrierInfo.MODEL_NAME,
                                    STATIOM_NAME = liftCarrierInfo.STATION_NAME,
                                    CARRIERID = liftCarrierInfo.CARRIERID,
                                };
                                var carrierdata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinforeject);
                                Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                {
                                    { "CMD_NO", "M004" },
                                    { "CMD_NAME", "CARRIER_REJECT" },
                                    { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                    { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                    { "ORIGIN",order},
                                    { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                    { "REJECT_CODE", carrierdata.Rows.Count!=0?!string.IsNullOrEmpty(carrierdata.Rows[0]["REJECT_CODE"].ToString())?carrierdata.Rows[0]["REJECT_CODE"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                    { "REJECT_MESSAGE",carrierdata.Rows.Count!=0?!string.IsNullOrEmpty(carrierdata.Rows[0]["REJECT_MESSAGE"].ToString())?carrierdata.Rows[0]["REJECT_MESSAGE"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA}
                                };
                                string commandcontent = JsonConvert.SerializeObject(CarrierReject);
                                SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "M004", commandcontent, ReportSource.LCS);
                                equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            }
                        }
                        else
                        {
                            #region [===VST-101專用===]
                            var carrierinfodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(equipmentcarrierinfo);
                            if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Move)
                            {
                                if (equipmentreportt.REPORE_DATAREMOTE == "RGV-IN" && equipmentreportt.REPORE_DATALOCAL == "CRANE")
                                {
                                    #region [===正常搬運上報===]
                                    var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATALOCAL);
                                    var hostidremote = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, equipmentreportt.REPORE_DATAREMOTE);
                                    var shuttledata = SAA_Database.SaaSql.GetScLiftE84PcStatsus(equipmentreportt.STATION_NAME);
                                    string shuttleid = shuttledata.Rows.Count != 0 ? shuttledata.Rows[0]["SHUTTLEID"].ToString() : "0";
                                    Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                   {
                                       { "CMD_NO", "S002" },
                                       { "CMD_NAME", "CARRIER_INTO_MECHANISM" },
                                       { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID },
                                       { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                       { "ORIGIN",hostidremote.Rows.Count!=0?hostidremote.Rows[0]["HOSTID"].ToString().Contains("RGV_RGV")?$"{hostidremote.Rows[0]["HOSTID"]}{shuttleid.PadLeft(3, '0')}":hostidremote.Rows[0]["HOSTID"].ToString():"查無資料"},
                                       { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                   };
                                    string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S002", commandcontent, ReportSource.LCS);
                                    #endregion
                                }
                            }
                            else if (equipmentreportt.REPORE_DATATRACK == SAA_Database.SaaCommon.Clear)
                            {
                                var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(int.Parse(equipmentreportt.SETNO), equipmentreportt.MODEL_NAME, equipmentreportt.STATION_NAME, "CRANE");
                                if (equipmentreportt.REPORE_DATALOCAL == "RGV-OUT")
                                {
                                    Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                    {
                                        { "CMD_NO", "S003" },
                                        { "CMD_NAME", "CARRIER_OUT_OF_MECHANISM" },
                                        { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                        { "SCHEDULE", carrierinfodata.Rows.Count!=0?!string.IsNullOrEmpty(carrierinfodata.Rows[0]["PARTNO"].ToString())?carrierinfodata.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                        { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料" },
                                    };
                                    string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "S003", commandcontent, ReportSource.LCS);
                                }
                                else if (equipmentreportt.REPORE_DATALOCAL != "RGV-IN")
                                {
                                    Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
                                    {
                                        { "CMD_NO", "D001" },
                                        { "CMD_NAME", "DELETE_CARRIER" },
                                        { "CARRIER", equipmentreportt.CARRIERID.Contains(SAA_Database.SaaCommon.ReaderError)?"ERROR": equipmentreportt.CARRIERID},
                                        { "STATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():SAA_Database.configattributes.SaaVST101StationName },
                                    };
                                    string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
                                    SAA_Database.SetSaaDirective(equipmentcarrierinfo.SETNO, equipmentcarrierinfo.STATIOM_NAME, equipmentcarrierinfo.CARRIERID, "D001", commandcontent, ReportSource.LCS);
                                }
                            }
                            equipmentreportt.SENDFLAG = SendFlag.Y.ToString();
                            SAA_Database.SaaSql.UpdScEquipmentReport(equipmentreportt);
                            #endregion
                        }
                        WebApiEquipmentReportHistory();
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        public void WebApiSendiLcs()
        {
            try
            {
                var directivedata = SAA_Database.SaaSql.GetScDirective(SAA_Database.LogSystmes.LCS.ToString());
                foreach (DataRow dr in directivedata.Rows)
                {
                    SaaScDirective SaaDirective = new SaaScDirective
                    {
                        TASKDATETIME = dr[SC_DIRECTIVE.TASKDATETIME.ToString()].ToString(),
                        SETNO = dr[SC_DIRECTIVE.SETNO.ToString()].ToString(),
                        COMMANDON = dr[SC_DIRECTIVE.COMMANDON.ToString()].ToString(),
                        STATION_NAME = dr[SC_DIRECTIVE.STATION_NAME.ToString()].ToString(),
                        CARRIERID = dr[SC_DIRECTIVE.CARRIERID.ToString()].ToString(),
                        COMMANDID = dr[SC_DIRECTIVE.COMMANDID.ToString()].ToString(),
                        COMMANDTEXT = dr[SC_DIRECTIVE.COMMANDTEXT.ToString()].ToString(),
                        SOURCE = dr[SC_DIRECTIVE.SOURCE.ToString()].ToString(),
                        SENDFLAG = dr[SC_DIRECTIVE.SENDFLAG.ToString()].ToString(),
                    };

                    SaaRequestDataTransport SaaEsDataTransport = new SaaRequestDataTransport
                    {
                        StationID = SaaDirective.STATION_NAME,
                        Time = SAA_Database.ReadTeid(),
                        Content = SaaDirective.COMMANDTEXT,
                    };
                    SaaEsDataTransport.TEID = $"{SaaDirective.STATION_NAME}_{SaaEsDataTransport.Time}";
                    Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                    {
                        { ES_DataTransport.StationID.ToString(), SaaEsDataTransport.StationID },
                        { ES_DataTransport.Time.ToString(), SaaEsDataTransport.Time },
                        { ES_DataTransport.TEID.ToString(), SaaEsDataTransport.TEID },
                        { ES_DataTransport.Content.ToString(), SaaEsDataTransport.Content },
                    };

                    string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                    SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{SaaEsDataTransport.StationID}，時間:{SaaEsDataTransport.Time}，傳送編號:{SaaEsDataTransport.TEID}傳送內容:{commandcontent}");
                    returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_DataTransport.ToString());
                    SaaReportResult saareportresult = WebApiReportResult(returnresult);
                    if (saareportresult != null)
                    {
                        SAA_Database.LogMessage($"【LCS->iLIS】【ES_DataTransport】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                        if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
                        {
                            SaaDirective.SENDFLAG = SendFlag.Y.ToString();
                            SAA_Database.SaaSql.UpdScDirective(SaaDirective);
                            SAA_Database.SaaSql.SetScDirectiveHistory(SaaDirective, SAA_Database.ReadTime());
                            SAA_Database.LogMessage("【新增歷史資料】SC_DIRECTIVE_HISTORY 歷史資訊新增資料完成");
                            SAA_Database.SaaSql.DelScDirective(SaaDirective, SendFlag.Y.ToString());
                            SAA_Database.LogMessage("【刪除上報資料】SC_DIRECTIVE 已完成上報，資訊刪除資料");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public void WebApiLcsHandshakeSendiLcs()
        {
            try
            {
                var plcdata = SAA_Database.SaaSql.GetScLiftE84Plc();
                if (plcdata.Rows.Count != 0)
                {
                    foreach (DataRow dr in plcdata.Rows)
                    {
                        SaaScLiftE84Plc lifte84plc = new SaaScLiftE84Plc
                        {
                            TASKDATETIME = dr[SC_LIFT_E84PLC.TASKDATETIME.ToString()].ToString(),
                            STATION_NAME = dr[SC_LIFT_E84PLC.STATION_NAME.ToString()].ToString(),
                            SHUTTLEID = dr[SC_LIFT_E84PLC.SHUTTLEID.ToString()].ToString(),
                            COMMANDID = dr[SC_LIFT_E84PLC.COMMANDID.ToString()].ToString(),
                            CARRIERID = dr[SC_LIFT_E84PLC.CARRIERID.ToString()].ToString(),
                            CS_0 = int.Parse(dr[SC_LIFT_E84PLC.CS_0.ToString()].ToString()),
                            Valid = int.Parse(dr[SC_LIFT_E84PLC.Valid.ToString()].ToString()),
                            TR_REQ = int.Parse(dr[SC_LIFT_E84PLC.TR_REQ.ToString()].ToString()),
                            Busy = int.Parse(dr[SC_LIFT_E84PLC.Busy.ToString()].ToString()),
                            Complete = int.Parse(dr[SC_LIFT_E84PLC.Complete.ToString()].ToString()),
                            Continue = int.Parse(dr[SC_LIFT_E84PLC.Continue.ToString()].ToString()),
                            SELECT = int.Parse(dr[SC_LIFT_E84PLC.SELECT.ToString()].ToString()),
                            AM_AVBL = int.Parse(dr[SC_LIFT_E84PLC.AM_AVBL.ToString()].ToString()),
                        };

                        Handshake PlcHandshake = new Handshake
                        {
                            Mode = Mode.ActiveEquipment.ToString(),
                            VALID = lifte84plc.Valid == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            CS_0 = lifte84plc.CS_0 == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            CS_1 = E84Handshake.False.ToString(),
                            TR_REQ = lifte84plc.TR_REQ == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            L_REQ = E84Handshake.False.ToString(),
                            U_REQ = E84Handshake.False.ToString(),
                            READY = E84Handshake.False.ToString(),
                            BUSY = lifte84plc.Busy == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            COMPT = lifte84plc.Complete == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            CONT = lifte84plc.Continue == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            HO_AVBL = E84Handshake.False.ToString(),
                            ES = lifte84plc.SELECT == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                            AM_AVBL = lifte84plc.AM_AVBL == 1 ? E84Handshake.True.ToString() : E84Handshake.False.ToString(),
                        };

                        SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo()
                        {
                            SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo.ToString()),
                            MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                            STATIOM_NAME = lifte84plc.STATION_NAME,
                            CARRIERID = lifte84plc.CARRIERID,
                        };

                        var infodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(EquipmentCarrierInfo);
                        if (infodata.Rows.Count != 0)
                        {
                            EquipmentCarrierInfo.PARTNO = infodata.Rows[0]["PARTNO"].ToString() != string.Empty ? infodata.Rows[0]["PARTNO"].ToString() : SAA_Database.SaaCommon.NA;
                        }
                        else
                        {
                            EquipmentCarrierInfo.PARTNO = SAA_Database.SaaCommon.NA;
                        }

                        CarrierInfo carrierInfo = new CarrierInfo
                        {
                            CarrierID = lifte84plc.CARRIERID,
                            CarrierType = CarrierType.Normal.ToString(),
                            Schedule = EquipmentCarrierInfo.PARTNO,
                            Flip = "0",
                            Rotation = "0",
                            CarrierState = infodata.Rows.Count != 0 ? infodata.Rows[0]["CARRIERSTATE"].ToString() : CarrierState.Unknow.ToString(),
                            DestinationType = infodata.Rows.Count != 0 ? infodata.Rows[0]["DESTINATIONTYPE"].ToString() : DestinationType.Unknow.ToString(),
                            Qtime = infodata.Rows.Count != 0 ? infodata.Rows[0]["QTIME"].ToString() : string.Empty,
                            Cycletime = infodata.Rows.Count != 0 ? infodata.Rows[0]["CYCLETIME"].ToString() : string.Empty,
                            Oper = infodata.Rows.Count != 0 ? infodata.Rows[0]["OPER"].ToString() : string.Empty,
                            Recipe = infodata.Rows.Count != 0 ? infodata.Rows[0]["RECIPE"].ToString() : string.Empty,
                            RejectCode = infodata.Rows.Count != 0 ? infodata.Rows[0]["REJECT_CODE"].ToString() : string.Empty,
                            RejectMessage = infodata.Rows.Count != 0 ? infodata.Rows[0]["REJECT_MESSAGE"].ToString() : string.Empty,
                        };

                        SaaReportHandshakeCarrierTransport ReportHandshakeCarrierTransport = new SaaReportHandshakeCarrierTransport
                        {
                            CarrierInfo = carrierInfo,
                            Handshake = PlcHandshake,
                            Time = SAA_Database.ReadTime(),
                            HandsHakeType = HandshakeType.Normal.ToString(),
                            ShuttleID = lifte84plc.SHUTTLEID,
                            MissionID = lifte84plc.COMMANDID,
                            StationID = lifte84plc.STATION_NAME,
                        };
                        ReportHandshakeCarrierTransport.TEID = $"{ReportHandshakeCarrierTransport.StationID}_{ReportHandshakeCarrierTransport.Time}";
                        Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                        {
                            { ES_Handshake_CarrierTransport.StationID.ToString(), ReportHandshakeCarrierTransport.StationID },
                            { ES_Handshake_CarrierTransport.Time.ToString(), ReportHandshakeCarrierTransport.Time },
                            { ES_Handshake_CarrierTransport.TEID.ToString(), ReportHandshakeCarrierTransport.TEID },
                            { ES_Handshake_CarrierTransport.ShuttleID.ToString(), ReportHandshakeCarrierTransport.ShuttleID },
                            { ES_Handshake_CarrierTransport.MissionID.ToString(), ReportHandshakeCarrierTransport.MissionID },
                            { ES_Handshake_CarrierTransport.HandsHakeType.ToString(), ReportHandshakeCarrierTransport.HandsHakeType },
                            { ES_Handshake_CarrierTransport.Handshake.ToString(), ReportHandshakeCarrierTransport.Handshake },
                            { ES_Handshake_CarrierTransport.CarrierInfo.ToString(), ReportHandshakeCarrierTransport.CarrierInfo }
                        };
                        string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                        SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{ReportHandshakeCarrierTransport.StationID}，時間:{ReportHandshakeCarrierTransport.Time}，傳送編號:{ReportHandshakeCarrierTransport.TEID}傳送內容:{commandcontent}");
                        returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Handshake_CarrierTransport.ToString());//ES_Handshake_CarrierTransport
                        SaaReportResult saareportresult = WebApiReportResult(returnresult);
                        if (saareportresult != null)
                        {
                            SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                            if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
                            {
                                lifte84plc.RESULT = SendFlag.Y.ToString();
                                SAA_Database.SaaSql.UpdScLifttE84Plc(lifte84plc);
                                //SAA_Database.SaaSql.SetScLifte84PlcHistory(lifte84plc);
                                //SAA_Database.LogMessage($"【{lifte84plc.STATION_NAME}】【新增資料】已新增E84 PLC交握歷史紀錄資料完成，【命令編號:{lifte84plc.COMMANDID}】【卡匣ID:{lifte84plc.CARRIERID}】");
                                SAA_Database.SaaSql.DelScE84Plc(lifte84plc.STATION_NAME, lifte84plc.RESULT);
                                SAA_Database.LogMessage($"【{lifte84plc.STATION_NAME}】【刪除資料】已刪除E84 PLC交握資料完成，【命令編號:{lifte84plc.COMMANDID}】【卡匣ID:{lifte84plc.CARRIERID}】");
                            }
                            else
                            {
                                if (plcreturncode > 3)
                                {
                                    lifte84plc.RESULT = SendFlag.Y.ToString();
                                    SAA_Database.SaaSql.UpdScLifttE84Plc(lifte84plc);
                                    SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})，傳送{plcreturncode}次失敗不予傳送", SAA_Database.LogType.Error);
                                    plcreturncode = 0;
                                }
                                else
                                {
                                    plcreturncode = plcreturncode + 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public void WebApiSndEquipmentCarrier()
        {
            try
            {
                var liftcarrierinfoemptydata = SAA_Database.SaaSql.GetLiftCarrierInfoEmpty();
                foreach (DataRow dr in liftcarrierinfoemptydata.Rows)
                {
                    if (!string.IsNullOrEmpty(dr[SC_LIFT_CARRIER_INFO_EMPTY.CYCLETIME.ToString()].ToString()))
                    {
                        if (dr[SC_LIFT_CARRIER_INFO_EMPTY.CYCLETIME.ToString()].ToString().Length == 12)
                        {
                            DateTime aTime = DateTime.ParseExact(dr[SC_LIFT_CARRIER_INFO_EMPTY.CYCLETIME.ToString()].ToString(), "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                            int intTotal = (int)(DateTime.Now - aTime).TotalSeconds;
                            if (intTotal > (3600 * 10))
                            {
                                if (SAA_Database.SaaSql.GetLiftCarrierInfoEmptySendFlag().Rows.Count == 0)
                                {
                                    var dataShelf = SAA_Database.SaaSql.GetScLocationSettingInfoShelf(int.Parse(dr[SC_LIFT_CARRIER_INFO_EMPTY.SETNO.ToString()].ToString()), dr[SC_LIFT_CARRIER_INFO_EMPTY.MODEL_NAME.ToString()].ToString(), dr[SC_LIFT_CARRIER_INFO_EMPTY.STATION_NAME.ToString()].ToString(), dr[SC_LIFT_CARRIER_INFO_EMPTY.CARRIERID.ToString()].ToString());
                                    if (dataShelf.Rows.Count != 0)
                                    {
                                        SaaScLiftCarrierInfoReject CarrierInfoReject = new SaaScLiftCarrierInfoReject
                                        {
                                            TASKDATETIME = SAA_Database.ReadTime(),
                                            SETNO = liftcarrierinfoemptydata.Rows.Count != 0 ? int.Parse(liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.SETNO.ToString()].ToString()) : int.Parse(SAA_Database.configattributes.SaaEquipmentNo),
                                            MODEL_NAME = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.MODEL_NAME.ToString()].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                                            STATION_NAME = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.STATION_NAME.ToString()].ToString() : string.Empty,
                                            CARRIERID = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.CARRIERID.ToString()].ToString() : string.Empty,
                                            DEVICETYPE = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.DEVICETYPE.ToString()].ToString() : string.Empty,
                                            CYCLETIME = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.CYCLETIME.ToString()].ToString() : string.Empty,
                                            QTIME = liftcarrierinfoemptydata.Rows.Count != 0 ? liftcarrierinfoemptydata.Rows[0][SC_LIFT_CARRIER_INFO_EMPTY.QTIME.ToString()].ToString() : string.Empty,
                                        };
                                        SAA_Database.SaaSql?.SetScLiftCarrierInfoReject(CarrierInfoReject);
                                        SAA_Database.LogMessage($"【{CarrierInfoReject.STATION_NAME}】【監控上報】新增退盒資料表SC_LIFT_CARRIER_INFO_REJECT 卡匣ID: ({CarrierInfoReject.CARRIERID}) ");

                                        SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo
                                        {
                                            STATIOM_NAME = dr["STATION_NAME"].ToString(),
                                            CARRIERID = dr["CARRIERID"].ToString(),
                                            REJECT_CODE = "RS0008",
                                            REJECT_MESSAGE = "Carrier_life_cycle_time_exceeded",
                                        };
                                        SAA_Database.SaaSql.UpdScEquipmentCarrierInfo(EquipmentCarrierInfo);
                                        var hostidlocal = SAA_Database.SaaSql.GetScLocationSetting(CarrierInfoReject.SETNO, CarrierInfoReject.MODEL_NAME, CarrierInfoReject.STATION_NAME, "PGV-OUT");
                                        Dictionary<string, string> CarrierReject = new Dictionary<string, string>
                                       {
                                           { "CMD_NO", "M004" },
                                           { "CMD_NAME", "CARRIER_REJECT" },
                                           { "CARRIER", CarrierInfoReject.CARRIERID},
                                           { "SCHEDULE", dataShelf.Rows.Count!=0?!string.IsNullOrEmpty(dataShelf.Rows[0]["PARTNO"].ToString())?dataShelf.Rows[0]["PARTNO"].ToString():SAA_Database.SaaCommon.NA:SAA_Database.SaaCommon.NA},
                                           { "ORIGIN",dataShelf.Rows.Count!=0?dataShelf.Rows[0]["HOSTID"].ToString():"查無資料"},
                                           { "DESTINATION",hostidlocal.Rows.Count!=0?hostidlocal.Rows[0]["HOSTID"].ToString():"查無資料"},
                                           { "REJECT_CODE", EquipmentCarrierInfo.REJECT_CODE},
                                           { "REJECT_MESSAGE",EquipmentCarrierInfo.REJECT_MESSAGE}
                                       };
                                        string commandcontent1 = JsonConvert.SerializeObject(CarrierReject);
                                        SaaScReportInadx reportInadx1 = new SaaScReportInadx()
                                        {
                                            SETNO = CarrierInfoReject.SETNO,
                                            MODEL_NAME = CarrierInfoReject.MODEL_NAME,
                                            STATION_NAME = CarrierInfoReject.STATION_NAME,
                                            REPORT_NAME = IndexTableName.SC_DIRECTIVE.ToString(),
                                        };
                                        SaaScDirective directive1 = new SaaScDirective()
                                        {
                                            TASKDATETIME = SAA_Database.ReadTime(),
                                            SETNO = SAA_Database.configattributes.SaaEquipmentNo,
                                            COMMANDON = SAA_Database.ReadRequorIndex(reportInadx1).ToString(),
                                            STATION_NAME = CarrierInfoReject.STATION_NAME,
                                            CARRIERID = CarrierInfoReject.CARRIERID,
                                            COMMANDID = "M004",
                                            COMMANDTEXT = commandcontent1,
                                            SOURCE = ReportSource.LCS.ToString(),
                                        };
                                        SAA_Database.SaaSql.SetScDirective(directive1);
                                        SAA_Database.LogMessage($"【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive1.COMMANDON} Command_Id:{directive1.COMMANDID} Command_Text:{directive1.COMMANDTEXT}。");
                                        SAA_Database.LogMessage($"【新增指令】新增Directive表，指令新增完成");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
            //GetLiftCarrierInfoEmpty
        }

        public void WebApiSendEquipmnetHardwareInfo()
        {
            try
            {
                var hardwareinfodata = SAA_Database.SaaSql.GetScEquipmnetHardwareInfo();
                if (hardwareinfodata != null)
                {
                    if (hardwareinfodata.Rows.Count != 0)
                    {
                        string StationID = hardwareinfodata.Rows[0]["STATION_NAME"].ToString();
                        string Time = hardwareinfodata.Rows[0]["EQUIPMNET_TIME"].ToString();
                        string TEID = hardwareinfodata.Rows[0]["EQUIPMNET_TEID"].ToString();
                        List<HardwareInfo> requirementinfo = new List<HardwareInfo>();
                        List<CarrierInfo> CarrierInfolist = new List<CarrierInfo>();
                        var settinginfodata = SAA_Database.SaaSql.GetScLocationSettingInfo(int.Parse(SAA_Database.configattributes.SaaEquipmentNo), SAA_Database.configattributes.SaaEquipmentName, StationID);
                        foreach (DataRow dr in settinginfodata.Rows)
                        {
                            SaaEquipmentCarrierInfo EquipmentCarrierInfo = new SaaEquipmentCarrierInfo()
                            {
                                SETNO = int.Parse(SAA_Database.configattributes.SaaEquipmentNo.ToString()),
                                MODEL_NAME = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                                STATIOM_NAME = StationID,
                                CARRIERID = dr["CARRIERID"].ToString(),
                            };
                            var infodata = SAA_Database.SaaSql.GetEquipmentCarrierInfo(EquipmentCarrierInfo);
                            HardwareInfo info = new HardwareInfo
                            {
                                HardwareID = dr["HOSTID"].ToString(),
                                CarrierID = dr["CARRIERID"].ToString(),
                                HardwareType = dr["LOCATIONTYPE"].ToString(),
                                UsingFlag = "True"
                            };
                            requirementinfo.Add(info);

                            CarrierInfo carrierinfo = new CarrierInfo
                            {
                                CarrierID = dr["CARRIERID"].ToString(),
                                CarrierType = "Normal",
                                Schedule = dr["PARTNO"].ToString(),
                                Rotation = "0",
                                Flip = "0",
                                CarrierState = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0]["CARRIERSTATE"].ToString()) ? CarrierState.Unknow.ToString() : infodata.Rows[0]["CARRIERSTATE"].ToString() : CarrierState.Unknow.ToString(),
                                DestinationType = infodata.Rows.Count != 0 ? string.IsNullOrEmpty(infodata.Rows[0]["DESTINATIONTYPE"].ToString()) ? DestinationType.Buffer.ToString() : infodata.Rows[0]["DESTINATIONTYPE"].ToString() : DestinationType.Buffer.ToString(),
                                Qtime = infodata.Rows.Count != 0 ? infodata.Rows[0]["QTIME"].ToString() : string.Empty,
                                Cycletime = infodata.Rows.Count != 0 ? infodata.Rows[0]["CYCLETIME"].ToString() : string.Empty,//CARRIERSTATE DESTINATIONTYPE
                                Oper = infodata.Rows.Count != 0 ? infodata.Rows[0]["OPER"].ToString() : string.Empty,
                                Recipe = infodata.Rows.Count != 0 ? infodata.Rows[0]["RECIPE"].ToString() : string.Empty,
                                RejectCode = infodata.Rows.Count != 0 ? infodata.Rows[0]["REJECT_CODE"].ToString() : string.Empty,
                                RejectMessage = infodata.Rows.Count != 0 ? infodata.Rows[0]["REJECT_MESSAGE"].ToString() : string.Empty,
                            };
                            CarrierInfolist.Add(carrierinfo);
                        }
                        WebApiSendTransportEquipmentHardwareInfo(StationID, Time, TEID, requirementinfo, CarrierInfolist);
                        SaaScEquipmnetHardwareInfo equipmnethardwareinfo = new SaaScEquipmnetHardwareInfo
                        {
                            STATION_NAME = StationID,
                            EQUIPMNET_TEID = TEID,
                            EQUIPMNET_TIME = Time,
                            SENDFLAG = SendFlag.Y.ToString(),
                        };
                        SAA_Database.SaaSql.UpdScEquipmnetHardwareInfo(equipmnethardwareinfo);
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public void WebApiSendAskShuttleTask()
        {
            try
            {
                #region [===重新詢問天車任務===]
                TimeSpan WaitTime = new TimeSpan(DateTime.Now.Ticks - dtStart.Ticks);
                if (WaitTime.TotalSeconds > SAA_Database.SaaCommon.AskShuttleTaskTime)
                {
                    var devicedata = SAA_Database.SaaSql.GetScDeviceStation();
                    if (devicedata.Rows.Count != 0)
                    {
                        foreach (DataRow row in devicedata.Rows)
                        {
                            if (row[SC_DEVICE.DEVICESTATUS.ToString()].ToString() == DEVICESTATUS.Y.ToString())
                            {
                                SaaEsReportTransportRequirement TransportRequirement = new SaaEsReportTransportRequirement
                                {
                                    STATION = row[SC_DEVICE.STATION_NAME.ToString()].ToString(),
                                    DATATIME = SAA_Database.ReadTeid(),
                                };
                                TransportRequirement.TEID = $"{TransportRequirement.STATION}_{TransportRequirement.DATATIME}";
                                returnresult = WebApiSendTransportRequirementInfo(TransportRequirement.STATION, TransportRequirement.DATATIME, TransportRequirement.TEID);
                                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                                SAA_Database.LogMessage($"【{TransportRequirement.STATION}】【LCS->iLIS】【{SendWebApi.ES_Request_TransportRequirementInfo}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                                if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
                                {
                                    dtStart = DateTime.Now;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        #region [===上報ALARMLIST===]
        public void WebApiSendLiftAlarmList()
        {
            try
            {
                var alarmdata = SAA_Database.SaaSql.GetScAlarmCurrent();
                if (alarmdata.Rows.Count != 0)
                {
                    foreach (DataRow dr in alarmdata.Rows)
                    {
                        var alarmlistdata = SAA_Database.SaaSql.GetScAlarmList(dr["MODEL_NAME"].ToString(), dr["ALARM_CODE"].ToString());
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
                            Dictionary<string, string> Alarmlist = new Dictionary<string, string>
                           {
                               { "CMD_NO", cmd_no },
                               { "CMD_NAME", cmd_name },
                               { "STATION", dr["STATION_NAME"].ToString()},
                               { "ALARM_CODE", dr["ALARM_CODE"].ToString()},
                               { "ALARM_MESSAGE",alarmlistdata.Rows[0]["ALARM_MSG"].ToString()},
                           };
                            string commandcontent = JsonConvert.SerializeObject(Alarmlist);
                            SAA_Database.SetSaaDirective(int.Parse(dr["SETNO"].ToString()), dr["STATION_NAME"].ToString(), string.Empty, cmd_no, commandcontent, ReportSource.LCS);
                            SaaScAlarmCurrent scalarmcurrent = new SaaScAlarmCurrent
                            {
                                REPORT_STATUS = SendFlag.Y.ToString(),
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
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        /*================================================方法=======================================================*/

        #region [===讀取HostId資料===]
        public string GetLocationHostId(int setno, string modelname, string locationid)
        {
            try
            {
                var locationdata = SAA_Database.SaaSql.GetScLocationsetting(setno, modelname, locationid);
                return locationdata.Rows.Count != 0 ? locationdata.Rows[0][SC_LOCATIONSETTING.HOSTID.ToString()].ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                return string.Empty;
            }
        }
        #endregion

        #region [===新增搬運天車需求===]
        /// <summary>
        /// 新增搬運天車需求
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="Time"></param>
        /// <param name="TEID"></param>
        /// <param name="requirementinfo"></param>
        public void WebApiSendTransportRequirement(string StationID, string Time, string TEID, List<RequirementInfo> requirementinfo)
        {
            try
            {
                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                {
                    { ES_Report_TransportRequirement.StationID.ToString(), StationID },
                    { ES_Report_TransportRequirement.Time.ToString(), Time },
                    { ES_Report_TransportRequirement.TEID.ToString(), TEID },
                    { ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo }
                };
                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                SAA_Database.LogMessage($"【{StationID}】【LCS->iLIS】【{SendWebApi.ES_Report_TransportRequirement}】【通訊傳送】站點:{StationID}，時間:{Time}，傳送編號:{TEID}傳送內容:{commandcontent}");
                string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_TransportRequirement.ToString());
                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                SAA_Database.LogMessage($"【{StationID}】【LCS->iLIS】【{SendWebApi.ES_Report_TransportRequirement}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        #region [===料盒搬運需求===]
        /// <summary>
        /// 料盒搬運需求
        /// </summary>
        /// <param name="ReportTransportRequirement"></param>
        public void WebApiSendEsReportTransportRequirement(SaaEsReportTransportRequirement ReportTransportRequirement)
        {
            List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
            RequirementInfo info = new RequirementInfo
            {
                RequirementType = RequirementType.Take_out_Carrier.ToString(),
                CarrierID = ReportTransportRequirement.CARRIERID,
                BeginStation = ReportTransportRequirement.BEGINSTATION,
                EndStation = ReportTransportRequirement.ENDSTATION,
            };
            requirementinfo.Add(info);
            Dictionary<string, object> dicstatusb = new Dictionary<string, object>
            {
                 { ES_Report_TransportRequirement.StationID.ToString(),  ReportTransportRequirement.STATION },
                { ES_Report_TransportRequirement.Time.ToString(), ReportTransportRequirement.DATATIME },
                { ES_Report_TransportRequirement.TEID.ToString(), ReportTransportRequirement.TEID },
                { ES_Report_TransportRequirement.ListRequirementInfo.ToString(), requirementinfo },
            };
            string commandcontent = JsonConvert.SerializeObject(dicstatusb);
            SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{ReportTransportRequirement.STATION}，時間:{ReportTransportRequirement.DATATIME}，傳送編號:{ReportTransportRequirement.TEID}傳送內容:{commandcontent}");
            string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_TransportRequirement.ToString());
            SaaReportResult saareportresult = WebApiReportResult(returnresult);
            SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
            if (saareportresult.ReturnCode == SAA_Database.SaaCommon.Success)
            {
                SAA_Database.LogMessage($"建立料盒搬運需求:站點:{ReportTransportRequirement.STATION}、起點{ReportTransportRequirement.BEGINSTATION}、終點:{ReportTransportRequirement.ENDSTATION}、卡匣ID:{ReportTransportRequirement.CARRIERID}");
                var devicedata = SAA_Database.SaaSql.GetScDevice(ReportTransportRequirement.STATION);
                SaaScTransportrEquirementMaterial ScTransportrEquirement = new SaaScTransportrEquirementMaterial
                {
                    SETNO = devicedata.Rows.Count != 0 ? devicedata.Rows[0]["SETNO"].ToString() : SAA_Database.configattributes.SaaEquipmentNo,
                    MODEL_NAME = devicedata.Rows.Count != 0 ? devicedata.Rows[0]["MODEL_NAME"].ToString() : SAA_Database.configattributes.SaaEquipmentName,
                    STATION_NAME = ReportTransportRequirement.STATION,
                    REPORT_STATION = ReportTransportRequirement.STATION,
                    REPORT_TIME = ReportTransportRequirement.DATATIME,
                    CARRIERID = ReportTransportRequirement.CARRIERID,
                    REQUIREMENT_TYPE = "2",
                    BEGIN_STATION = ReportTransportRequirement.BEGINSTATION,
                    END_STATION = ReportTransportRequirement.ENDSTATION,
                    REQUIREMENT_RESULT = SendFlag.Y.ToString()
                };
                ScTransportrEquirement.REPORTID = $"{ScTransportrEquirement.REPORT_STATION}_{ScTransportrEquirement.REPORT_TIME}";
                if (SAA_Database.SaaSql.GetScTransportrEquirementMaterialCarrierId(ScTransportrEquirement.CARRIERID).Rows.Count == 0)
                {
                    SAA_Database.SaaSql.SetScTransportrEquirementMaterial(ScTransportrEquirement);
                    SAA_Database.LogMessage($"【新增資料】新增料盒搬運需求:站點:{ReportTransportRequirement.STATION}、起點{ReportTransportRequirement.BEGINSTATION}、終點:{ReportTransportRequirement.ENDSTATION}、卡匣ID:{ReportTransportRequirement.CARRIERID}");
                }
                else
                {
                    SAA_Database.LogMessage($"【新增資料】已有需求無法新增料盒搬運需求:站點:{ReportTransportRequirement.STATION}、起點{ReportTransportRequirement.BEGINSTATION}、終點:{ReportTransportRequirement.ENDSTATION}、卡匣ID:{ReportTransportRequirement.CARRIERID}");
                }
            }
        }
        #endregion

        public void WebApiSendTransportEquipmentHardwareInfo(string StationID, string Time, string TEID, List<HardwareInfo> requihardwareinfo, List<CarrierInfo> requicarrierinfo)
        {
            try
            {
                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                {
                    { ES_Report_EquipmentHardwareInfo.StationID.ToString(), StationID },
                    { ES_Report_EquipmentHardwareInfo.Time.ToString(), Time },
                    { ES_Report_EquipmentHardwareInfo.TEID.ToString(), TEID },
                    { ES_Report_EquipmentHardwareInfo.ListHardwareInfo.ToString(), requihardwareinfo },
                    { ES_Report_EquipmentHardwareInfo.ListCarrierInfo.ToString(), requicarrierinfo }
                };
                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                SAA_Database.LogMessage($"【LCS->iLIS】【{SendWebApi.ES_Report_EquipmentHardwareInfo}】【通訊傳送】站點:{StationID}，時間:{Time}，傳送編號:{TEID}傳送內容:{commandcontent}");
                string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Report_EquipmentHardwareInfo.ToString());
                SaaReportResult saareportresult = WebApiReportResult(returnresult);
                SAA_Database.LogMessage($"【{StationID}】【LCS->iLIS】【{SendWebApi.ES_Report_EquipmentHardwareInfo}】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }

        public string WebApiSendTransportRequirementInfo(string StationID, string Time, string TEID)
        {
            try
            {
                Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                {
                    { ES_Request_TransportRequirementInfo.StationID.ToString(), StationID },
                    { ES_Request_TransportRequirementInfo.Time.ToString(), Time },
                    { ES_Request_TransportRequirementInfo.TEID.ToString(), TEID },
                };
                string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                SAA_Database.LogMessage($"【LCS->iLIS】【{SendWebApi.ES_Request_TransportRequirementInfo}】【通訊傳送】站點:{StationID}，時間:{Time}，傳送編號:{TEID}傳送內容:{commandcontent}");
                return SAA_Database.SaaSendCommandiLis(commandcontent, SendWebApi.ES_Request_TransportRequirementInfo.ToString());
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
                return string.Empty;
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
                    if (Enum.IsDefined(typeof(WebApiReceive), datakey))
                    {
                        switch ((WebApiReceive)Enum.Parse(typeof(WebApiReceive), datakey))
                        {
                            case WebApiReceive.StationID:
                                saareportresult.StationID = datavalue;
                                break;
                            case WebApiReceive.Time:
                                saareportresult.Time = datavalue;
                                break;
                            case WebApiReceive.TEID:
                                saareportresult.TEID = datavalue;
                                break;
                            case WebApiReceive.ReturnCode:
                                saareportresult.ReturnCode = datavalue;
                                break;
                            case WebApiReceive.ReturnMessage:
                                saareportresult.ReturnMessage = datavalue;
                                break;
                        }
                    }
                }
                return saareportresult;
            }
            return null;
        }

        #region [===上報M001指令===]
        /// <summary>
        /// 上報M001指令
        /// </summary>
        /// <param name="SETNO">設備編號</param>
        /// <param name="STATIOM_NAME">設備站點</param>
        /// <param name="CommandReport"></param>
        public void WebApiSendCommandM001(int SETNO, string STATIOM_NAME, CommandReportM001 CommandReport)
        {
            try
            {
                Dictionary<string, string> SaaCommand = new Dictionary<string, string>
                {
                    { CommandM001.CMD_NO.ToString(), CommandReport.CMD_NO },
                    { CommandM001.CMD_NAME.ToString(), CommandReport.CMD_NAME },
                    { CommandM001.CARRIER.ToString(), CommandReport.CARRIER},
                    { CommandM001.SCHEDULE.ToString(), CommandReport.SCHEDULE},
                    { CommandM001.OPER.ToString(), CommandReport.OPER},
                    { CommandM001.RECIPE.ToString(), CommandReport.RECIPE},
                    { CommandM001.ORIGIN.ToString(),CommandReport.ORIGIN},
                    { CommandM001.DESTINATION.ToString(),CommandReport.DESTINATION},
                    { CommandM001.QTIME.ToString(), CommandReport.QTIME},
                    { CommandM001.CYCLETIME.ToString(),CommandReport.CYCLETIME}
                };
                string commandcontent = JsonConvert.SerializeObject(SaaCommand);
                SAA_Database.SetSaaDirective(SETNO, STATIOM_NAME, CommandReport.CARRIER, CommandReport.CMD_NO, commandcontent, ReportSource.LCS);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        #region [=== 上報M004指令===]
        /// <summary>
        /// 上報M004指令
        /// </summary>
        /// <param name="SETNO">設備編號</param>
        /// <param name="STATIOM_NAME">設備站點</param>
        /// <param name="CommandReport"></param>
        public void WebApiSendCommandM004(int SETNO, string STATIOM_NAME, CommandReportM004 CommandReport)
        {
            try
            {
                Dictionary<string, string> SaaCommand = new Dictionary<string, string>
                {
                    { CommandM004.CMD_NO.ToString(), CommandReport.CMD_NO },
                    { CommandM004.CMD_NAME.ToString(), CommandReport.CMD_NAME },
                    { CommandM004.CARRIER.ToString(), CommandReport.CARRIER},
                    { CommandM004.SCHEDULE.ToString(), CommandReport.SCHEDULE},
                    { CommandM004.ORIGIN.ToString(),CommandReport.ORIGIN},
                    { CommandM004.DESTINATION.ToString(),CommandReport.DESTINATION},
                    { CommandM004.REJECT_CODE.ToString(), CommandReport.REJECT_CODE},
                    { CommandM004.REJECT_MESSAGE.ToString(),CommandReport.REJECT_MESSAGE}
                };
                string commandcontent = JsonConvert.SerializeObject(SaaCommand);
                SAA_Database.SetSaaDirective(SETNO, STATIOM_NAME, CommandReport.CARRIER, CommandReport.CMD_NO, commandcontent, ReportSource.LCS);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        #region [===詢問卡匣出處===]
        /// <summary>
        /// 詢問卡匣出處
        /// </summary>
        /// <param name="SETNO"></param>
        /// <param name="STATIOM_NAME"></param>
        /// <param name="CommandReport"></param>
        public void WebApiSendCommandQ001(int SETNO, string STATIOM_NAME, CommandReportQ001 CommandReport)
        {
            try
            {
                Dictionary<string, string> SaaCommand = new Dictionary<string, string>
                {
                    { CommandQ001.CMD_NO.ToString(), CommandReport.CMD_NO },
                    { CommandQ001.CMD_NAME.ToString(), CommandReport.CMD_NAME },
                    { CommandQ001.CARRIER.ToString(), CommandReport.CARRIER},
                    { CommandQ001.STATION.ToString(), CommandReport.STATION},
                };
                string commandcontent = JsonConvert.SerializeObject(SaaCommand);
                SAA_Database.SetSaaDirective(SETNO, STATIOM_NAME, CommandReport.CARRIER, CommandReport.CMD_NO, commandcontent, ReportSource.LCS);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
        #endregion

        public void WebApiEquipmentReportHistory()
        {
            try
            {
                var reportdata = SAA_Database.SaaSql.GetScEquipmentReportSuccess(SendFlag.Y.ToString());
                if (reportdata.Rows.Count != 0)
                {
                    foreach (DataRow dr in reportdata.Rows)
                    {
                        SaaScEquipmentReportHistory EquipmentReportHistory = new SaaScEquipmentReportHistory
                        {
                            TASKDATETIME = dr[SC_EQUIPMENT_REPORT.TASKDATETIME.ToString()].ToString(),
                            SETNO = dr[SC_EQUIPMENT_REPORT.SETNO.ToString()].ToString(),
                            MODEL_NAME = dr[SC_EQUIPMENT_REPORT.MODEL_NAME.ToString()].ToString(),
                            STATION_NAME = dr[SC_EQUIPMENT_REPORT.STATION_NAME.ToString()].ToString(),
                            CARRIERID = dr[SC_EQUIPMENT_REPORT.CARRIERID.ToString()].ToString(),
                            REPORE_DATATRACK = dr[SC_EQUIPMENT_REPORT.REPORE_DATATRACK.ToString()].ToString(),
                            REPORE_DATAREMOTE = dr[SC_EQUIPMENT_REPORT.REPORE_DATAREMOTE.ToString()].ToString(),
                            REPORE_DATALOCAL = dr[SC_EQUIPMENT_REPORT.REPORE_DATALOCAL.ToString()].ToString(),
                            SENDFLAG = dr[SC_EQUIPMENT_REPORT.SENDFLAG.ToString()].ToString(),
                            UPDATETASKDATETIME = SAA_Database.ReadTime(),
                        };
                        SAA_Database.SaaSql.SetEquipmentReportHistory(EquipmentReportHistory);
                        SAA_Database.LogMessage("【新增歷史資料】SC_EQUIPMENT_REPORT_HISTORYE 歷史資訊新增資料完成");
                        SAA_Database.SaaSql.DelEquipmentReport(EquipmentReportHistory);
                        SAA_Database.LogMessage("【刪除上報資料】SC_EQUIPMENT_REPORT，資訊刪除資料");
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error, SAA_Database.LogSystmes.LCS);
            }
        }
    }
}
