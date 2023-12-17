using NLog;
using SAA_CommunicationSystem_Lib.Attributes;
using SAA_CommunicationSystem_Lib.SqlData;
using SAA_CommunicationSystem_Lib.WebApiSendCommand;
using SAA_CommunicationSystem_Lib.WebApiServer;
using System;
using System.Collections.Generic;
using System.Data;
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
        }
        #endregion

        public static void GetReportCommand()
        {
            var commandnamedata = SaaSql.GetScReportCommandLcsName();
            foreach (DataRow dr in commandnamedata.Rows)
            {
                string commandname = dr["LCS_COMMAND_NAME"].ToString();
                var reportcommanddata = SaaSql.GetScReportCommand(configattributes.SaaEquipmentNo, configattributes.SaaEquipmentName, commandname);
                foreach (DataRow command in reportcommanddata.Rows)
                {
                    string lcscommandname = command["LCS_COMMAND_NAME"].ToString();
                    switch ((SAA_DatabaseEnum.ReportCommand)Enum.Parse(typeof(SAA_DatabaseEnum.ReportCommand), lcscommandname))
                    {
                        case SAA_DatabaseEnum.ReportCommand.ALARM_REPORT:
                            reportcommand.DicAlarmReport.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.AlarmReportAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.ASK_CARRIER:
                            reportcommand.DicAskCarrier.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.AskCarrierAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_IN_REPORT:
                            reportcommand.DicCarryInReport.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.CarryInReportAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_OUT_REPORT:
                            reportcommand.DicCarryOutReport.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.CarryOutReportAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CARRY_REJECT:
                            reportcommand.DicCarryReject.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.CarryRejectAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.CLEAR_CACHE:
                            reportcommand.DicClearCache.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.ClearCacheAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        case SAA_DatabaseEnum.ReportCommand.IN_OUT_LOCK:
                            reportcommand.DicInOutLock.Add(command["REPORT_COMMAND"].ToString(), string.Empty);
                            reportcommand.InOutLockAry.Add(command["REPORT_COMMAND"].ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
        }

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
