﻿using Newtonsoft.Json;
using NLog;
using SAA_CommunicationSystem_Lib.Attributes;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.ReportCommandAttributes;
using SAA_CommunicationSystem_Lib.SqlData;
using SAA_CommunicationSystem_Lib.WebApiSendCommand;
using SAA_CommunicationSystem_Lib.WebApiServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_Database
    {
        /// <summary>
        /// Log訊息
        /// </summary>
        /// <param name="message">訊息</param>
        public delegate void DelLogMessage(string message, LogType logtype, LogSystmes logsystmes);
        public static event DelLogMessage OnLogMessage;

        /// <summary>
        /// 路徑名稱
        /// </summary>
        public static readonly string Config = "Config";

        /// <summary>
        /// Config檔名稱
        /// </summary>
        public static readonly string SystemSetting = "SystemSetting.config";

        /// <summary>
        /// Config屬性
        /// </summary>
        public static ConfigAttributes configattributes;

        /// <summary>
        /// 讀取設定參數
        /// </summary>
        public static SAA_ReadCommon readcommon;

        /// <summary>
        /// Web Api啟動
        /// </summary>
        public static SAA_WebApiServer webapiserver;

        /// <summary>
        /// 讀取SQL方法
        /// </summary>
        public static MsSqlData SaaSql;

        /// <summary>
        /// NLog方法
        /// </summary>
        public static Logger SaaLog;

        /// <summary>
        /// 傳送Web API方法
        /// </summary>
        public static SAA_WebApiSendCommand webapisendcommand = new SAA_WebApiSendCommand();

        /// <summary>
        /// 上報命令
        /// </summary>
        public static SAA_ReportCommand reportcommand = new SAA_ReportCommand();

        /// <summary>
        /// 設定檔參數
        /// </summary>
        public static ScCommonAttributes SaaCommon = new ScCommonAttributes();

        #region [===寫入Log訊息===]
        /// <summary>
        ///寫入Log訊息
        /// </summary>
        /// <param name="message">訊息</param>
        /// <param name="logtype">log分類</param>
        public static void LogMessage(string message, LogType logtype = LogType.Normal, LogSystmes logsystmes = LogSystmes.LCS)
        {
            try
            {
                string Message = $"【{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}】{message}";
                switch ((LogType)Enum.Parse(typeof(LogType), logtype.ToString()))
                {
                    case LogType.Normal:
                        SaaLog.Info(Message);
                        break;
                    case LogType.Warnning:
                        SaaLog.Warn(Message);
                        break;
                    case LogType.Error:
                        SaaLog.Error(Message);
                        break;
                }
                OnLogMessage?.Invoke(Message, logtype, logsystmes);
            }
            catch (Exception ex)
            {
                SaaLog.Error($"{ex.Message}-{ex.StackTrace}");
            }
        }
        #endregion

        #region [===讀取當前時間===]
        /// <summary>
        /// 讀取當前時間
        /// </summary>
        /// <returns></returns>
        public static string ReadTime()
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
        }
        #endregion

        #region [===讀取Report時間===]
        /// <summary>
        /// 讀取Report時間
        /// </summary>
        /// <returns></returns>
        public static string ReadTeid()
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff}";
        }
        #endregion

        #region [===LCS命令名稱列舉===]
        /// <summary>
        /// LCS命令名稱
        /// </summary>
        public enum CommandName
        {
            /// <summary>
            /// 異常發生
            /// </summary>
            ErrorHappen,

            /// <summary>
            /// 異常結束
            /// </summary>
            ErrorEnd,

            /// <summary>
            /// 警告發生
            /// </summary>
            WarningHappen,

            /// <summary>
            /// 警告結束
            /// </summary>
            WarningEnd,

            /// <summary>
            /// 清除貨批在籍 (單格)
            /// </summary>
            ClearStorage,

            /// <summary>
            /// 詢問料要去哪
            /// </summary>
            GoWhere,

            /// <summary>
            /// 通知設備狀態(啟動)
            /// </summary>
            DeviceSts_1,

            /// <summary>
            /// 通知設備狀態(停止-允許啟動/IDLE)
            /// </summary>
            DeviceSts_2,

            /// <summary>
            /// 通知設備狀態(手動/SETUP)
            /// </summary>
            DeviceSts_3,

            /// <summary>
            /// 通知設備狀態(無法啟動/INIT)
            /// </summary>
            DeviceSts_4,

            /// <summary>
            /// 通知設備狀態(啟動-閒置中/READY)
            /// </summary>
            DeviceSts_5,

            /// <summary>
            /// 機構載入
            /// </summary>
            DeviceLoadIn,

            /// <summary>
            /// 儲格載入
            /// </summary>
            StorageLoadIn,

            /// <summary>
            /// 儲格載出
            /// </summary>
            StorageLoadOut,

            /// <summary>
            /// 料到 Reject 區
            /// </summary>
            RejectDown,

            /// <summary>
            /// Port 口載出
            /// </summary>
            PortLoadOut,

            /// <summary>
            /// 儲格狀態通知
            /// </summary>
            StorageInfo,

            /// <summary>
            /// 任務取消
            /// </summary>
            CmdCancel,

            /// <summary>
            /// 設備交握異常取消任務
            /// </summary>
            EqpCmdCancel,

            /// <summary>
            /// LCS 離線
            /// </summary>
            LCS_STS_OFFLINE,

            /// <summary>
            /// LCS 在線上，但不收入、出料命令
            /// </summary>
            LCS_STS_LOCAL_ONLINE,

            /// <summary>
            /// LCS 在線上，收入、出料命令
            /// </summary>
            LCS_STS_REMOTE_ONLINE,

            /// <summary>
            /// LCS 為入出料模式
            /// </summary>
            LCS_MODE_InOut,

            /// <summary>
            /// LCS 為入料模式
            /// </summary>
            LCS_MODE_In,

            /// <summary>
            /// LCS 為出料模式
            /// </summary>
            LCS_MODE_Out,

            /// <summary>
            /// 天車平台1為入料模式
            /// </summary>
            RGV_1_MODE_In,

            /// <summary>
            /// 天車平台1為出料模式
            /// </summary>
            RGV_1_MODE_Out,

            /// <summary>
            /// 天車平台2為入料模式
            /// </summary>
            RGV_2_MODE_In,

            /// <summary>
            /// 天車平台2為出料模式
            /// </summary>
            RGV_2_MODE_Out,

            /// <summary>
            /// 天車平台1為啟用狀態
            /// </summary>
            RGV_1_STS_ON,

            /// <summary>
            /// 天車平台1為關閉狀態
            /// </summary>
            RGV_1_STS_OFF,

            /// <summary>
            /// 天車平台2為啟用狀態
            /// </summary>
            RGV_2_STS_ON,

            /// <summary>
            /// 天車平台2為關閉狀態
            /// </summary>
            RGV_2_STS_OFF,

            /// <summary>
            /// 卡匣到達出料平台
            /// </summary>
            CarrierArrivedPlatform,

            RGV_1,

            Carrier_1D_Fail,

            Carrier_Repeat,

            WIP_Full,
        }
        #endregion

        public static void GetReportCommand()
        {
            var commandnamedata = SaaSql.GetScReportCommandLcsName();
            foreach (DataRow dr in commandnamedata.Rows)
            {
                string commandname = dr[SAA_DatabaseEnum.SC_REPORT_COMMAND.LCS_COMMAND_NAME.ToString()].ToString();
                var reportcommanddata = SaaSql.GetScReportCommand(configattributes.SaaEquipmentNo, configattributes.SaaEquipmentName, commandname);
                foreach (DataRow command in reportcommanddata.Rows)
                {
                    string lcscommandname = command[SAA_DatabaseEnum.SC_REPORT_COMMAND.LCS_COMMAND_NAME.ToString()].ToString();
                    switch ((SAA_DatabaseEnum.ReportCommand)Enum.Parse(typeof(SAA_DatabaseEnum.ReportCommand), lcscommandname))
                    {
                        case SAA_DatabaseEnum.ReportCommand.ALARM_REPORT:
                            reportcommand.DicAlarmReport.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.AlarmReportAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.ASK_CARRIER:
                            reportcommand.DicAskCarrier.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.AskCarrierAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_IN_REPORT:
                            reportcommand.DicCarryInReport.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.CarryInReportAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_OUT_REPORT:
                            reportcommand.DicCarryOutReport.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.CarryOutReportAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_REJECT:
                            reportcommand.DicCarryReject.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.CarryRejectAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CLEAR_CACHE:
                            reportcommand.DicClearCache.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.ClearCacheAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.IN_OUT_LOCK:
                            reportcommand.DicInOutLock.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(), string.Empty);
                            reportcommand.InOutLockAry.Add(command[SAA_DatabaseEnum.SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #region [===讀取命令編號===]
        /// <summary>
        /// 讀取命令編號
        /// </summary>
        /// <param name="reportInadx"></param>
        /// <returns></returns>
        public static int ReadRequorIndex(SaaScReportInadx reportInadx)
        {
            var indexdata = SaaSql.GetScReportIndex(reportInadx.SETNO, reportInadx.MODEL_NAME, reportInadx.REPORT_NAME, reportInadx.STATION_NAME);
            if (indexdata.Rows.Count != 0)
            {
                reportInadx.REPORT_MAX = int.Parse(indexdata.Rows[0][SAA_DatabaseEnum.SC_REPORT_INDEX.REPORT_MAX.ToString()].ToString());
                reportInadx.REPORT_INDEX = int.Parse(indexdata.Rows[0][SAA_DatabaseEnum.SC_REPORT_INDEX.REPORT_INDEX.ToString()].ToString());
                reportInadx.REPORT_INDEX = reportInadx.REPORT_INDEX + 1;
                if (reportInadx.REPORT_INDEX > reportInadx.REPORT_MAX)
                {
                    reportInadx.REPORT_INDEX = 1;
                    LogMessage($"【{reportInadx.STATION_NAME}】【更新資料】Index大於最大值:{reportInadx.REPORT_MAX}更新為:{reportInadx.REPORT_INDEX}");
                }
                LogMessage($"【{reportInadx.STATION_NAME}】【回傳資料】回傳Index值:{reportInadx.REPORT_INDEX}");
                SaaSql.UpdScReportIndex(reportInadx);
                return reportInadx.REPORT_INDEX;
            }
            LogMessage($"【{reportInadx.STATION_NAME}】【無此資料】查無此Index資料回傳時間數直為Index", LogType.Error);
            return int.Parse($"{DateTime.Now:ssfff}");
        }
        #endregion

        #region [===傳送指令至LCS===]
        /// <summary>
        /// 傳送指令至LCS
        /// </summary>
        /// <param name="commanddata">指令內容</param>
        /// <returns></returns>
        public static string SaaSendCommandLcs(string commanddata)
        {
            try
            {
                return webapisendcommand.Post(configattributes.StorageWebApiServerIP, configattributes.ParaKey, commanddata);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                return string.Empty;
            }
        }
        #endregion

        public static string SaaSendCommandiLis(string commanddata, string reportevent)
        {
            try
            {
                return webapisendcommand.Post(configattributes.iLISWebApiServerIP + "LCS/" + reportevent, commanddata);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                return string.Empty;
            }
        }

        public static string SaaSendCommandLift(string commanddata, string reportevent)
        {
            try
            {
                return webapisendcommand.Post($"{configattributes.LiftWebApiServerIP}/" + reportevent, commanddata);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                return string.Empty;
            }
        }

        public static string SaaSendCommandJumpDie(string commanddata, string reportevent)
        {
            try
            {
                return webapisendcommand.Post($"{configattributes.JumpDieApiServerIP}/" + reportevent, commanddata);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                return string.Empty;
            }
        }

        public static void SaaSendAutoMation(SaaReportCommandAutpMation autpmation)
        {
            var commondb = SaaSql.GetScDevice(autpmation.STATION);
            Dictionary<string, string> CarrierGoTo = new Dictionary<string, string>
            {
               { "CMD_NO", autpmation.CMD_NO },
               { "CMD_NAME",autpmation.CMD_NAME},
               { "STATION",$"{autpmation.STATION}_ROBOT"},
            };
            string commandcontent = JsonConvert.SerializeObject(CarrierGoTo);
            int saaequipmentno = commondb.Rows.Count != 0 ? int.Parse(commondb.Rows[0]["SETNO"].ToString()) : 0;
            string stationid = commondb.Rows.Count != 0 ? commondb.Rows[0]["STATION_NAME"].ToString() : string.Empty;
            SetSaaDirective(saaequipmentno, stationid, string.Empty, autpmation.CMD_NO, commandcontent, SAA_DatabaseEnum.ReportSource.LCS);
        }

        #region [===新增Directive資料===]
        /// <summary>
        /// 新增Directive資料
        /// </summary>
        /// <param name="saaequipmentno">設備編號</param>
        /// <param name="commandstation">站點</param>
        /// <param name="carrierid">卡匣ID</param>
        /// <param name="commandno">指令編號</param>
        /// <param name="commandcontent">指令內容</param>
        /// <param name="reportsource">來源(LCS or iLIS)</param>
        public static void SetSaaDirective(int saaequipmentno, string commandstation, string carrierid, string commandno, string commandcontent, SAA_DatabaseEnum.ReportSource reportsource = SAA_DatabaseEnum.ReportSource.LCS)
        {
            if (ReadScDirectiveCount(saaequipmentno, commandno, commandcontent, reportsource))
            {
                SaaScReportInadx reportInadx = new SaaScReportInadx()
                {
                    SETNO = saaequipmentno,
                    MODEL_NAME = configattributes.SaaEquipmentName,
                    STATION_NAME = commandstation,
                    REPORT_NAME = SAA_DatabaseEnum.IndexTableName.SC_DIRECTIVE.ToString(),
                };
                SaaScDirective directive = new SaaScDirective()
                {
                    TASKDATETIME = ReadTime(),
                    SETNO = saaequipmentno.ToString(),
                    COMMANDON = ReadRequorIndex(reportInadx).ToString(),
                    STATION_NAME = commandstation,
                    CARRIERID = carrierid,
                    COMMANDID = commandno,
                    COMMANDTEXT = commandcontent,
                    SOURCE = reportsource.ToString(),
                };
                SaaSql.SetScDirective(directive);
                LogMessage($"【{commandstation}】【新增指令】新增資料至SC_DIRECTIVE=>Command_ON:{directive.COMMANDON} Command_ID:{directive.COMMANDID} Command_Text:{directive.COMMANDTEXT}。");
                LogMessage($"【{commandstation}】【新增指令】新增Directive表，指令新增完成");
            }
            else
            {
                LogMessage($"【{commandstation}】【指令相同】已有相同指令無法新增。", LogType.Error);
            }
        }
        #endregion

        public static string SaaSendCommandSystems(string commanddata, string reportevent)
        {
            try
            {
                return webapisendcommand.Post($"{configattributes.WebApiServerIP}/" + reportevent, commanddata);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                return string.Empty;
            }
        }

        #region [===讀取是否有相同資料===]
        public static bool ReadScDirectiveCount(int saaequipmentno, string commandid, string commandtext, SAA_DatabaseEnum.ReportSource reportSource)
        {
            if (SaaSql.GetScDirective(saaequipmentno, commandid, commandtext, reportSource.ToString()).Rows.Count == 0)
                return true;
            return false;
        }
        #endregion

        #region [===字串轉Dictionary===]
        /// <summary>
        /// 字串轉Dictionary
        /// </summary>
        /// <param name="commandtext"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ContentToDictionary(string commandtext)
        {
            try
            {
                string messagetemp = commandtext.Replace("\r\n", "").Replace("\"", "").Replace("{", "").Replace("}", "");
                Dictionary<string, string> mydictionary = new Dictionary<string, string>();
                string[] aftercontent = messagetemp.Split(',');
                foreach (var datas in aftercontent)
                {
                    var data = datas.Split(':');
                    mydictionary.Add(data[0], data[1]);
                }
                return mydictionary;
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}");
                return null;
            }
        }
        #endregion

        public enum LogType
        {
            /// <summary>
            /// 成功
            /// </summary>
            Normal = 0,

            /// <summary>
            /// 警告
            /// </summary>
            Warnning = 1,

            /// <summary>
            /// 失敗
            /// </summary>
            Error = 2
        }

        public enum LogSystmes
        {
            iLIs,

            LCS
        }
    }
}
