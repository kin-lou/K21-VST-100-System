using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_DatabaseEnum
    {
        #region [===權限帳號密碼===]
        /// <summary>
        /// 權限帳號密碼列舉
        /// </summary>
        public enum GUI_USER
        {
            /// <summary>
            /// 帳號
            /// </summary>
            USERID,

            /// <summary>
            /// 密碼
            /// </summary>
            PWD,

            /// <summary>
            /// 權限群組
            /// </summary>
            GROUPID,

            /// <summary>
            /// 使用者名稱
            /// </summary>
            USERNAME,

            /// <summary>
            /// 登入系統時間(yyyy-MM-dd HH:mm:ss.fff)
            /// </summary>
            LAST_LOGIN_TIME,
        }
        #endregion

        #region [===REJECT 明細===]
        /// <summary>
        /// REJECT 明細
        /// </summary>
        public enum SC_REJECT_LIST
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 本機REJECT編碼
            /// </summary>
            LOCAL_REJECT_CODE,

            /// <summary>
            /// 本機REJECT訊息
            /// </summary>
            LOCAL_REJECT_MSG,

            /// <summary>
            /// 遠端REJECT編碼
            /// </summary>
            REMOTE_REJECT_CODE,

            /// <summary>
            /// 遠端REJECT訊息
            /// </summary>
            REMOTE_REJECT_MSG,
        }
        #endregion

        #region  [===REJECT歷史紀錄===]
        /// <summary>
        /// REJECT歷史紀錄
        /// </summary>
        public enum SC_REJECT_HISTORY
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// REJECT時間(yyyy-MM-dd HH:mm:ss.fff)
            /// </summary>
            REJECT_TIME,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點
            /// </summary>
            STATION,

            /// <summary>
            /// 卡匣號碼
            /// </summary>
            CARRIERID,

            /// <summary>
            /// 卡匣批號
            /// </summary>
            PARTNO,

            /// <summary>
            /// 本機REJECT編碼
            /// </summary>
            LOCAL_REJECT_CODE,

            /// <summary>
            /// 本機REJECT訊息
            /// </summary>
            LOCAL_REJECT_MSG,

            /// <summary>
            /// 遠端REJECT編碼
            /// </summary>
            REMOTE_REJECT_CODE,

            /// <summary>
            /// 遠端REJECT訊息
            /// </summary>
            REMOTE_REJECT_MSG,
        }
        #endregion 

        #region [===機台區域===]
        /// <summary>
        /// 機台區域
        /// </summary>
        public enum SC_EQUIPMENT_ZONE
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點編號
            /// </summary>
            STATION_NAME,

            /// <summary>
            /// 區域名稱
            /// </summary>
            ZONE_NAME,

            /// <summary>
            /// 客戶機台名稱
            /// </summary>
            REPORT_NAME,
        }
        #endregion

        #region [===上報命令===]
        public enum ReportCommand
        {
            /// <summary>
            /// Alarm上報
            /// </summary>
            ALARM_REPORT,

            /// <summary>
            /// 詢問上報
            /// </summary>
            ASK_CARRIER,

            /// <summary>
            /// 入庫上報
            /// </summary>
            CARRY_IN_REPORT,

            /// <summary>
            /// 出庫上報
            /// </summary>
            CARRY_OUT_REPORT,

            /// <summary>
            /// 退盒上報
            /// </summary>
            CARRY_REJECT,

            /// <summary>
            /// 清除上報
            /// </summary>
            CLEAR_CACHE,

            /// <summary>
            /// 鎖格上報
            /// </summary>
            IN_OUT_LOCK,
        }
        #endregion

        #region [===上報指令資料===]
        public enum SC_REPORT_COMMAND
        {
            /// <summary>
            /// 
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// LCS上報名稱
            /// </summary>
            LCS_COMMAND_NAME,

            /// <summary>
            /// 上報註解內容
            /// </summary>
            LCS_COMMAND_NOTE,

            /// <summary>
            /// 上報
            /// </summary>
            GROUP_NO,

            /// <summary>
            /// 群組編號
            /// </summary>
            REPORT_COMMAND_NO,

            /// <summary>
            /// 上報名稱
            /// </summary>
            REPORT_COMMAND,

            /// <summary>
            /// 上報名稱註解內容
            /// </summary>
            REPORT_COMMAND_NOTE
        }
        #endregion

        #region [===上報指令名稱===]
        /// <summary>
        /// 上報指令名稱
        /// </summary>
        public enum SC_REPORT_COMMAND_NAME
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// LCS上報名稱
            /// </summary>
            LCS_COMMAND_NAME,

            /// <summary>
            /// 上報指令編號
            /// </summary>
            REPORT_COMMAND_NO,

            /// <summary>
            /// 上報指令名稱
            /// </summary>
            REPORT_COMMAND_NAME,

            /// <summary>
            /// 機構位置
            /// </summary>
            LOCATIONID,
        }
        #endregion

        public enum ReportSource
        {
            /// <summary>
            /// 設備端口
            /// </summary>
            LCS,

            /// <summary>
            /// 上位端口
            /// </summary>
            iLIS,
        }

        #region [===命令Index===]
        public enum SC_REPORT_INDEX
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 上報名稱
            /// </summary>
            REPORT_NAME,

            /// <summary>
            /// 目前Index值
            /// </summary>
            REPORT_INDEX,

            /// <summary>
            /// Inex最大值
            /// </summary>
            REPORT_MAX
        }
        #endregion

        public enum IndexTableName
        {
            SC_DIRECTIVE,
        }

        #region [===資料參數===]
        public enum SC_COMMON
        {
            SETNO,

            MODEL_NAME,

            ITEM_NAME,

            ITEM_VALUE,

            VALUECOMMENT,
        }
        #endregion

        #region [===資料參數名稱===]
        public enum SC_COMMON_ITEM_NAME
        {
            ReaderError,

            Empty,

            NA,

            CRANE,

            ReportCraneName,

            AskCarrier,

            AskResultNo,

            AskResultYes,

            LCS_STS_OFFLINE,

            LCS_STS_LOCAL_ONLINE,

            LCS_STS_REMOTE_ONLINE,

            LCS_MODE_InOut,

            LCS_MODE_In,

            LCS_MODE_Out,

            DeviceSts_1,

            DeviceSts_2,

            DeviceSts_3,

            DeviceSts_4,

            DeviceSts_5,

            RGV_1_MODE_In,

            RGV_1_MODE_Out,

            RGV_2_MODE_In,

            RGV_2_MODE_Out,

            RGV_1_STS_ON,

            RGV_1_STS_OFF,

            RGV_2_STS_ON,

            RGV_2_STS_OFF,

            Pire1Name,

            Pire2Name,

            PireModeIn,

            PireModOut,

            PireStatusOn,

            PireStatusOff,

            ReaderSataion,

            ReadStatge,

            Success,

            Fail,

            RejectStage,

            ReadStatgeName,

            /// <summary>
            /// LIFT物料追蹤-搬移
            /// </summary>
            Move,

            /// <summary>
            /// LIFT物料追蹤-更新
            /// </summary>
            Update,

            /// <summary>
            /// LIFT物料追蹤-建立
            /// </summary>
            Establish,

            /// <summary>
            /// LIFT物料追蹤-清除
            /// </summary>
            Clear,

            /// <summary>
            /// LIFT物料追蹤-詢問
            /// </summary>
            Ask,

            /// <summary>
            /// LIFT物料追蹤-回覆
            /// </summary>
            Reply,

            /// <summary>
            /// LIFT資料同步-有帳
            /// </summary>
            Have,

            /// <summary>
            /// LIFT資料同步-無帳
            /// </summary>
            None,

            /// <summary>
            /// 卡匣屬性為空值時顯示文字
            /// </summary>
            CarrierType,

            /// <summary>
            /// UD管控空盒數量
            /// </summary>
            LiftCarrierInfoEmptyCount,

            /// <summary>
            /// LIFT LD代號
            /// </summary>
            DevicetTypeLD,

            /// <summary>
            /// LIFTUD代號
            /// </summary>
            DeivertTypeUD,

            /// <summary>
            /// LIFT卡匣回倉儲站點名稱
            /// </summary>
            SaaZipStationName,

            /// <summary>
            /// 多少秒詢問一次iLis天車任務
            /// </summary>
            AskShuttleTaskTime,
        }
        #endregion

        public enum SC_DEVICE
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點編號
            /// </summary>
            STATION_NAME,

            /// <summary>
            /// 機台手臂編號
            /// </summary>
            DEVICENO,

            /// <summary> 
            /// 機台手臂名稱
            /// </summary>
            DEVICEID,

            /// <summary>
            /// 客戶機台手臂名稱
            /// </summary>
            HOSTDEVICEID,

            /// <summary>
            /// 機台種類
            /// </summary>
            DEVICETYPE,

            /// <summary>
            /// 機台狀態
            /// </summary>
            DEVICESTATUS,
        }

        public enum SC_LOCATIONSETTING
        {
            /// <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 位置編號
            /// </summary>
            LOCATIONID,

            /// <summary>
            /// 上報編號
            /// </summary>
            HOSTID,

            /// <summary>
            /// 卡匣號碼
            /// </summary>
            CARRIERID,

            /// <summary>
            /// 卡匣批號
            /// </summary>
            PARTNO,

            /// <summary>
            /// BANK
            /// </summary>
            BANK,

            /// <summary>
            /// BAY
            /// </summary>
            BAY,

            /// <summary>
            /// LV
            /// </summary>
            LV,

            /// <summary>
            /// 位置狀態
            /// </summary>
            LOCATIONSTATUS,

            /// <summary>
            /// 位置模式
            /// </summary>
            LOCATIONMODE,

            /// <summary>
            /// 位置種類
            /// </summary>
            LOCATIONTYPE,

            /// <summary>
            /// 是否有卡匣(0:無卡匣 1:有卡匣)
            /// </summary>
            INVENTORYFULL,

            /// <summary>
            /// 區域名稱
            /// </summary>
            ZONEID,

            /// <summary>
            /// 位置排序
            /// </summary>
            LOCATIONPRIORITIZ,
        }

        public enum SC_DIRECTIVE
        {
            TASKDATETIME,

            SETNO,

            COMMANDON,

            STATION_NAME,

            CARRIERID,

            COMMANDID,

            COMMANDTEXT,

            SOURCE,

            SENDFLAG,
        }

        public enum SC_EQUIPMENT_REPORT
        {
            TASKDATETIME,

            SETNO,

            MODEL_NAME,

            STATION_NAME,

            CARRIERID,

            REPORE_DATATRACK,

            REPORE_DATAREMOTE,

            REPORE_DATALOCAL,

            SENDFLAG
        }

        #region [===指令名稱===]
        /// <summary>
        /// 指令名稱
        /// </summary>
        public enum CommandName
        {
            /// <summary>
            /// 命令編號
            /// </summary>
            CMD_NO,

            /// <summary>
            /// 命令名稱
            /// </summary>
            CMD_NAME,

            /// <summary>
            /// 盒號
            /// </summary>
            CARRIER,

            /// <summary>
            /// 站點
            /// </summary>
            STATION,

            /// <summary>
            /// 回絕碼
            /// </summary>
            REJECT_CODE,

            /// <summary>
            /// 回絕訊息
            /// </summary>
            REJECT_MESSAGE,
        }
        #endregion

        public enum E84Handshake
        {
            Unknow,
            True,
            False,
        }

        public enum HandshakeType
        {
            Normal,

            Report,
        }

        public enum HardwareType
        {

            Unknow,

            Crane,

            Port,

            Shelf,
        }

        public enum UsingFlag
        {
            Unknow,

            True,

            False,
        }

        public enum CarrierType
        {
            Normal,

            Report,
        }

        public enum DestinationType
        {
            /// <summary>
            /// 未知
            /// </summary>
            Unknow,

            /// <summary>
            /// 製程區
            /// </summary>
            EQP,

            /// <summary>
            ///暫存區 
            /// </summary>
            Buffer,

            /// <summary>
            /// 退盒區
            /// </summary>
            Reject,
        }

        public enum RequirementType
        {
            Unknow,
            Take_out_Carrier,
            Take_In_EmptyCarrier,
            Take_out_EmptyCarrier,
        }

        public enum Mode
        {
            Unknow,
            ActiveVehicle,
            PassiveVehicle,
            ActiveEquipment,
            PassiveEquipment,
        }

        #region [===接收上位命令===]
        public enum ReceivCommand
        {
            /// <summary>
            /// 指令編碼
            /// </summary>
            CMD_NO,

            /// <summary>
            /// 指令名稱
            /// </summary>
            CMD_NAME,

            /// <summary>
            ///站點
            /// </summary>
            STATION,

            /// <summary>
            /// 批號
            /// </summary>
            SCHEDULE,

            /// <summary>
            /// 儲格位置
            /// </summary>
            WARENUMBER,

            /// <summary>
            /// 起點
            /// </summary>
            ORIGIN,

            /// <summary>
            /// 目的地
            /// </summary>
            DESTINATION,

            /// <summary>
            /// 是否翻轉
            /// </summary>
            ROTFLAG,

            /// <summary>
            /// 盒子上下方向
            /// </summary>
            FLIPFLAG,

            /// <summary>
            /// 盒號
            /// </summary>
            CARRIER,

            /// <summary>
            /// 起始點
            /// </summary>
            FROM,

            /// <summary>
            /// 目的地
            /// </summary>
            TO,

            /// <summary>
            /// 貨批站點
            /// </summary>
            OPER,

            /// <summary>
            /// 卡匣生命期限
            /// </summary>
            QTIME,

            /// <summary>
            /// 盒效期
            /// </summary>
            CYCLETIME,

            /// <summary>
            /// 貨批參數
            /// </summary>
            RECIPE,

            /// <summary>
            /// 退盒編碼
            /// </summary>
            REJECT_CODE,

            /// <summary>
            /// 退盒資訊
            /// </summary>
            REJECT_MESSAGE,

            /// <summary>
            /// 卡匣屬性
            /// </summary>
            CARRIERTYOE,
        }
        #endregion

        public enum WebApiReceive
        {
            /// <summary>
            /// 
            /// </summary>
            StationID,

            /// <summary>
            /// 
            /// </summary>
            Time,

            /// <summary>
            /// 
            /// </summary>
            TEID,

            /// <summary>
            /// 
            /// </summary>
            ReturnCode,

            /// <summary>
            /// 
            /// </summary>
            ReturnMessage,
        }

        public enum LcsReceive
        {
            /// <summary>
            /// 入庫或出庫
            /// </summary>
            M501,

            /// <summary>
            /// 退REJECT
            /// </summary>
            M504,

            /// <summary>
            /// 入庫或出庫
            /// </summary>
            M001,

            /// <summary>
            /// 退REJECT
            /// </summary>
            M004,
        }

        public enum LcsCmd
        {
            /// <summary>
            /// 清除貨批在籍
            /// </summary>
            ClearStorage,

            /// <summary>
            /// 指定搬運起始/目的 (派貨用)
            /// </summary>
            StockOut,

            /// <summary>
            /// 通知進入機構
            /// </summary>
            StockIn,

            /// <summary>
            /// 通知離開機構
            /// </summary>
            StockOutToReject,

            /// <summary>
            /// 通知 Reject (未進入機構)
            /// </summary>
            Reject,

            /// <summary>
            /// 詢問儲格資訊
            /// </summary>
            StorageInfo,

            /// <summary>
            /// 空值
            /// </summary>
            EMPTY,
        }

        public enum ES_DataTransport
        {
            /// <summary>
            /// 站點
            /// </summary>
            StationID,

            /// <summary>
            /// 時間
            /// </summary>
            Time,

            /// <summary>
            /// 指令編號
            /// </summary>
            TEID,

            /// <summary>
            /// 內容
            /// </summary>
            Content,
        }

        public enum ES_Report_Alive
        {
            ES_Report_Alive,

            /// <summary>
            /// 站點
            /// </summary>
            StationID,

            /// <summary>
            /// 時間
            /// </summary>
            Time,

            /// <summary>
            /// 指令編號
            /// </summary>
            TEID,
        }

        #region [===是否傳送列舉===]
        /// <summary>
        /// 是否傳送列舉
        /// </summary>
        public enum SendFlag
        {
            /// <summary>
            /// 傳送
            /// </summary>
            Y,

            /// <summary>
            /// 未傳送
            /// </summary>
            N
        }
        #endregion

        public enum SaaSendReply
        {
            /// <summary>
            /// 入庫
            /// </summary>
            Y,

            /// <summary>
            /// 退REJECT
            /// </summary>
            N,
        }

        public enum SaaLiftReport
        {
            // <summary>
            /// 機型編號
            /// </summary>
            SETNO,

            /// <summary>
            /// 機型名稱
            /// </summary>
            MODEL_NAME,

            /// <summary>
            /// 站點名稱
            /// </summary>
            STATION_NAME,

            /// <summary>
            //追中模式狀態
            /// </summary>
            TRACK,

            /// <summary>
            /// 卡匣ID
            /// </summary>
            CARRIERID,

            /// <summary>
            /// 起始位置
            /// </summary>
            ORIGIN,

            /// <summary>
            /// 終點位置
            /// </summary>
            DESTINATION,
        }

        public enum SC_LIFT_E84PLC
        {
            TASKDATETIME,

            STATION_NAME,

            SHUTTLEID,

            COMMANDID,

            CARRIERID,

            CS_0,

            Valid,

            TR_REQ,

            Busy,

            Complete,

            Continue,

            SELECT,

            AM_AVBL
        }

        public enum SC_LIFT_CARRIER_INFO_EMPTY
        {
            TASKDATETIME,
            SETNO,
            MODEL_NAME,
            STATION_NAME,
            CARRIERID,
            DEVICETYPE,
            QTIME,
            CYCLETIME,
        }

        public enum CarrierState
        {
            Unknow,
            Empty,
            Material,
        }

        public enum SendWebApi
        {
            ES_Report_Alive,
            ES_Report_TransportRequirement,
            ES_DataTransport,
            ES_Handshake_CarrierTransport,
            ES_Request_TransportRequirementInfo,
            ES_Request_Handshake_CarrierTransport,
            ES_Report_EquipmentStatus,
            ES_Report_EquipmentHardwareInfo,
        }

        public enum DEVICESTATUS
        {
            /// <summary>
            ///自動
            /// </summary>
            Y,

            /// <summary>
            /// 手動
            /// </summary>
            N,
        }

        public enum ES_Report_TransportRequirement
        {
            StationID,
            Time,
            TEID,
            ListRequirementInfo,
        }

        public enum SE_Request_EquipmentStatus
        {
            StationID,
            Time,
            TEID,
            EqpStatus
        }

        public enum ES_Request_TransportRequirementInfo
        {
            StationID,
            Time,
            TEID,
        }

        public enum CommandM001
        {
            CMD_NO,

            CMD_NAME,

            CARRIER,

            SCHEDULE,

            OPER,

            RECIPE,

            ORIGIN,

            DESTINATION,

            QTIME,

            CYCLETIME,
        }

        public enum CommandM004
        {
            CMD_NO,

            CMD_NAME,

            CARRIER,

            SCHEDULE,

            ORIGIN,

            DESTINATION,

            REJECT_CODE,

            REJECT_MESSAGE,
        }

        public enum AseCommandNo
        {
            M001,

            Q001,
        }

        public enum AseCommandName
        {
            CARRIER_TO_MECHANISM,
        }

        public enum CommandQ001
        {
            CMD_NO,

            CMD_NAME,

            CARRIER,

            STATION,
        }

        public enum EqpStatus
        {
            Unknow,
            Manual,
            Auto,
            Alarm,
        }

        public enum LiftCommandName
        {
            EquipmentStatus,
            EquipmnetHardwareInfo,
            EquipmentLiftE84PlcHandshakeInfo,
            EquipmentLiftAlarmList,
        }

        public enum ES_Report_EquipmentHardwareInfo
        {
            StationID,

            Time,

            TEID,

            ListHardwareInfo,

            ListCarrierInfo
        }

        public enum ES_Handshake_CarrierTransport
        {
            StationID,

            Time,

            TEID,

            ShuttleID,

            MissionID,

            HandsHakeType,

            Handshake,

            CarrierInfo,
        }

        public enum EquipmentStatusCommand
        {
            Statiom_Name,

            CommandName,
        }

        public enum SendWebApiCommandName
        {
            GetLiftMessage,

            SaaEquipmentMonitorE84PcSendStart,

            SaaEquipmentMonitorE84PlcRendStart
        }

        public enum SC_TRANSPORTR_EQUIREMENT_MATERIAL
        {
            SETNO,
            MODEL_NAME,
            STATION_NAME,
            REPORTID,
            REPORT_TIME,
            REPORT_STATION,
            REQUIREMENT_TYPE,
            CARRIERID,
            BEGIN_STATION,
            END_STATION,
            REQUIREMENT_RESULT,
        }

        public enum SaaLiftCommandName
        {
            /// <summary>
            /// 設備狀態
            /// </summary>
            EquipmentStatus,

            EquipmnetHardwareInfo,
            EquipmentLiftE84PlcHandshakeInfo,
            EquipmentLiftAlarmList,
        }

        public enum SendLiftE84iLisPc
        {
            TASKDATETIME,
            STATION_NAME,
            SHUTTLEID,
            COMMANDID,
            CARRIERID,
            Mode,
            VALID,
            CS_0,
            CS_1,
            TR_REQ,
            L_REQ,
            U_REQ,
            READY,
            BUSY,
            COMPT,
            CONT,
            HOA_VBL,
            ES,
            VA,
            AM_AVBL,
            VS_0,
            VS_1,
        }

        public enum SendLiftE84Plc
        {
            TASKDATETIME,
            STATION_NAME,
            SHUTTLEID,
            COMMANDID,
            CARRIERID,
            CS_0,
            AM_AVBL,
            TR_REQ,
            BUSY,
            COMPT,
            CONT,
            CS_1,
            READY,
            VALID,
            HOA_VBL,
            L_REQ,
            Mode,
            U_REQ,
            VS_0,
            VS_1,
            SELECT,
            RESULT,
            ES
        }
    }
}
