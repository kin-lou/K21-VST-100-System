using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.GuiAttributes;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using SAA_MsSql;
using System.ComponentModel.Design;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Http.Results;
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem_Lib.SqlData
{
    public class MsSqlData
    {
        private MsSql SaaSql = new MsSql(SAA_Database.configattributes.SaaDataBase, SAA_Database.configattributes.SaaDataBaseIP, SAA_Database.configattributes.SaaDataBaseName, SAA_Database.configattributes.SaaDataBasePassword);

        /*===============================新增=======================================*/

        #region [===新增使用者===]
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="guiuser">使用者屬性</param>
        public void SetGuiUser(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_USER Values('{guiuser.USERID}', '{guiuser.PWD}', '{guiuser.GROUPID}','{guiuser.USERNAME}','{guiuser.LAST_LOGIN_TIME}')");
        }
        #endregion

        #region [===新增操作紀錄===]
        /// <summary>
        /// 新增操作紀錄
        /// </summary>
        /// <param name="guiopetation">操作紀錄屬性</param>
        public void SetGuiOpetationHistory(GuiOpetationHistoryAttributes guiopetation)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_OPETATION_HISTORY Values('{guiopetation.PROJECT_ITEM}', '{guiopetation.OPERATE_DATETIME}', '{guiopetation.USERNAME}', '{guiopetation.OPERATE_CONTENT}')");
        }
        #endregion

        #region [===新增使用者登入===]
        /// <summary>
        /// 新增使用者登入
        /// </summary>
        /// <param name="guiuser"></param>
        public void SetGuiLoginstatus(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into GUI_LOGINSTATUS Values('{guiuser.PROGRAMNAME}' ,'{guiuser.USERID}', '{guiuser.USERNAME}','{guiuser.GROUPID}','{guiuser.LAST_LOGIN_TIME}')");
        }
        #endregion

        #region [===新增RejectList===]
        /// <summary>
        /// 新增RejectList
        /// </summary>
        /// <param name="rejectList"></param>
        public void SetScRejectList(SaaScRejectList rejectList)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REJECT_LIST Values('{rejectList.SETNO}', '{rejectList.MODEL_NAME}', '{rejectList.LOCAL_REJECT_CODE}', '{rejectList.LOCAL_REJECT_MSG}', '{rejectList.REMOTE_REJECT_CODE}', '{rejectList.REMOTE_REJECT_MSG}')");
        }
        #endregion

        #region [===新增Reject歷史紀錄===]
        /// <summary>
        /// 新增Reject歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        public void SetScRejectHistory(SaaScRrejectHistory rejecthistory)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REJECT_HISTORY Values('{rejecthistory.SETNO}', '{rejecthistory.REJECT_TIME}', '{rejecthistory.MODEL_NAME}', '{rejecthistory.STATION}', '{rejecthistory.CARRIERID}', '{rejecthistory.PARTNO}', '{rejecthistory.LOCAL_REJECT_CODE}', '{rejecthistory.LOCAL_REJECT_MSG}', '{rejecthistory.REMOTE_REJECT_CODE}', '{rejecthistory.REMOTE_REJECT_MSG}')");
        }
        #endregion

        #region [===新增上報指令===]
        /// <summary>
        /// 新增上報指令
        /// </summary>
        /// <param name="reportcommand"></param>
        public void SetScReportCpommand(SaaScReportCommand reportcommand)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_REPORT_COMMAND Values('{reportcommand.SETNO}', '{reportcommand.MODEL_NAME}',  '{reportcommand.LCS_COMMAND_NAME}', '{reportcommand.LCS_COMMAND_NOTE}', '{reportcommand.GROUP_NO}', '{reportcommand.REPORT_COMMAND}', '{reportcommand.REPORT_COMMAND_NOTE}')");
        }
        #endregion

        #region [===新增傳送指令===]
        /// <summary>
        /// 新增傳送指令
        /// </summary>
        /// <param name="scdirective"></param>
        public void SetScDirective(SaaScDirective scdirective)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_DIRECTIVE(TASKDATETIME, SETNO, COMMANDON, STATION_NAME, CARRIERID, COMMANDID, COMMANDTEXT, SOURCE) Values('{scdirective.TASKDATETIME}', '{scdirective.SETNO}',  '{scdirective.COMMANDON}', '{scdirective.STATION_NAME}', '{scdirective.CARRIERID}', '{scdirective.COMMANDID}', '{scdirective.COMMANDTEXT}', '{scdirective.SOURCE}')");
        }
        #endregion

        #region [===新增傳送指令===]
        /// <summary>
        /// 新增傳送指令
        /// </summary>
        /// <param name="scdirective"></param>
        public void SetScDirectiveHistory(SaaScDirective scdirective, string updatetime)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_DIRECTIVE_HISTORY(TASKDATETIME, SETNO, COMMANDON, STATION_NAME, CARRIERID, COMMANDID, COMMANDTEXT, SOURCE, SENDFLAG, UPDATEDATETIME) Values('{scdirective.TASKDATETIME}', '{scdirective.SETNO}',  '{scdirective.COMMANDON}', '{scdirective.STATION_NAME}', '{scdirective.CARRIERID}', '{scdirective.COMMANDID}', '{scdirective.COMMANDTEXT}', '{scdirective.SOURCE}', '{scdirective.SENDFLAG}','{updatetime}')");
        }
        #endregion

        #region [===新增LIFT任務===]
        /// <summary>
        /// 新增LIFT任務
        /// </summary>
        /// <param name="sclifttask"></param>
        public void SetScLiftTask(SaaScLiftTask sclifttask)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_LIFT_TASK(TASKDATETIME, STATION_NAME, COMMANDID, CARRIERID, BEGINSTATION, ENDINSTATION) Values('{sclifttask.TASKDATETIME}','{sclifttask.STATION_NAME}','{sclifttask.COMMANDID}', '{sclifttask.CARRIERID}', '{sclifttask.BEGINSTATION}', '{sclifttask.ENDSTATION}')");
        }
        #endregion

        public void SetScLiftE84Pc(string taskdatetime, string station_name, string shuttleid, string commandid, string carrierid, Handshake handshake)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_LIFT_E84PC(TASKDATETIME, STATION_NAME, SHUTTLEID, COMMANDID, CARRIERID, Mode, VALID, CS_0, CS_1, TR_REQ, L_REQ, U_REQ, READY, BUSY, COMPT, CONT, HO_AVBL, ES, VA, AM_AVBL, VS_0, VS_1) Values('{taskdatetime}', '{station_name}', '{shuttleid}', '{commandid}', '{carrierid}', '{handshake.Mode}', '{handshake.VALID}', '{handshake.CS_0}', '{handshake.CS_1}', '{handshake.TR_REQ}'," +
                $" '{handshake.L_REQ}', '{handshake.U_REQ}', '{handshake.READY}', '{handshake.BUSY}', '{handshake.COMPT}', '{handshake.CONT}', '{handshake.HO_AVBL}', '{handshake.ES}', '{handshake.VA}', '{handshake.AM_AVBL}', '{handshake.VS_0}', '{handshake.VS_1}')");
        }

        public void SetScCommandTask(SaaScCommandTask commandtask)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_COMMAND_TASK(TASKDATETIME, SETNO, MODEL_NAME, STATION_NAME, COMMANDID, CARRIERID, LOCATIONTAKE, LOCATIONPUT) Values('{commandtask.TASKDATETIME}', '{commandtask.SETNO}', '{commandtask.MODEL_NAME}', '{commandtask.STATION_NAME}','{commandtask.COMMANDID}', '{commandtask.CARRIERID}', '{commandtask.LOCATIONTAKE}', '{commandtask.LOCATIONPUT}')");
        }

        public void SetScEquipmnetHardwareInfo(SaaScEquipmnetHardwareInfo hardwareinfo)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_EQUIPMNET_HARDWARE_INFO(SETNO, MODEL_NAME, STATION_NAME, EQUIPMNET_TIME, EQUIPMNET_TEID) Values('{hardwareinfo.SETNO}','{hardwareinfo.MODEL_NAME}','{hardwareinfo.STATION_NAME}','{hardwareinfo.EQUIPMNET_TIME}','{hardwareinfo.EQUIPMNET_TEID}')");
        }

        public void SetScLiftE84PlcHandshakeInfo(SaaScLiftE84PlcHandshakeInfo e84plchandshakeinfo)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_E84PLC_HANDSHAKE_INFO(SETNO, MODEL_NAME, STATION_NAME, EQUIPMNET_TIME, EQUIPMNET_TEID) Values('{e84plchandshakeinfo.SETNO}','{e84plchandshakeinfo.MODEL_NAME}','{e84plchandshakeinfo.STATION_NAME}','{e84plchandshakeinfo.EQUIPMNET_TIME}','{e84plchandshakeinfo.EQUIPMNET_TEID}')");
        }

        public void SetScEquipmentCarrierInfo(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_EQUIPMENT_CARRIER_INFO(SETNO, MODEL_NAME, STATIOM_NAME, CARRIERID, PARTNO , CARRIERTYOE , ROTFLAG, FLIPFLAG, OPER, RECIPE, ORIGIN, DESTINATION, QTIME, CYCLETIME, REJECT_CODE, REJECT_MESSAGE, CARRIERSTATE, DESTINATIONTYPE) " +
                $"Values('{equipmentcarrierinfo.SETNO}', '{equipmentcarrierinfo.MODEL_NAME}', '{equipmentcarrierinfo.STATIOM_NAME}', '{equipmentcarrierinfo.CARRIERID}','{equipmentcarrierinfo.PARTNO}', '{equipmentcarrierinfo.CARRIERTYOE}', '{equipmentcarrierinfo.ROTFLAG}', '{equipmentcarrierinfo.FLIPFLAG}', '{equipmentcarrierinfo.OPER}', '{equipmentcarrierinfo.RECIPE}', '{equipmentcarrierinfo.ORIGIN}', '{equipmentcarrierinfo.DESTINATION}', '{equipmentcarrierinfo.QTIME}', '{equipmentcarrierinfo.CYCLETIME}', '{equipmentcarrierinfo.REJECT_CODE}', '{equipmentcarrierinfo.REJECT_MESSAGE}', '{equipmentcarrierinfo.CARRIERSTATE}', '{equipmentcarrierinfo.DESTINATIONTYPE}')");
        }

        public void SetScLiftCarrierInfo(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_CARRIER_INFO(SETNO, MODEL_NAME, STATION_NAME, CARRIERID, CARRIERTYPE) Values('{LiftCarrierInfo.SETNO}','{LiftCarrierInfo.MODEL_NAME}','{LiftCarrierInfo.STATION_NAME}','{LiftCarrierInfo.CARRIERID}', '{LiftCarrierInfo.CARRIERTYPE}')");
        }

        public void SetScLiftAmount(SaaScLiftAmount LiftAmount)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_AMOUNT(SETNO, MODEL_NAME, STATION_NAME, TASKDATETIME) Values('{LiftAmount.SETNO}','{LiftAmount.MODEL_NAME}','{LiftAmount.STATION_NAME}','{LiftAmount.TASKDATETIME}')");
        }

        public void SetScLiftCarrierInfoEmpty(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_CARRIER_INFO_EMPTY(TASKDATETIME, SETNO, MODEL_NAME, STATION_NAME, CARRIERID, DEVICETYPE, QTIME, CYCLETIME) Values('{ScLiftCarrierInfoEmpty.TASKDATETIME}','{ScLiftCarrierInfoEmpty.SETNO}','{ScLiftCarrierInfoEmpty.MODEL_NAME}','{ScLiftCarrierInfoEmpty.STATION_NAME}','{ScLiftCarrierInfoEmpty.CARRIERID}', '{ScLiftCarrierInfoEmpty.DEVICETYPE}', '{ScLiftCarrierInfoEmpty.QTIME}', '{ScLiftCarrierInfoEmpty.CYCLETIME}')");
        }

        public void SetScLiftCarrierInfoMaterial(SaaScLiftCarrierInfoMaterial ScLiftCarrierInfoMaterial)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_CARRIER_INFO_MATERIAL(TASKDATETIME, SETNO, MODEL_NAME, STATION_NAME, CARRIERID, DEVICETYPE, QTIME, CYCLETIME) Values('{ScLiftCarrierInfoMaterial.TASKDATETIME}','{ScLiftCarrierInfoMaterial.SETNO}','{ScLiftCarrierInfoMaterial.MODEL_NAME}','{ScLiftCarrierInfoMaterial.STATION_NAME}','{ScLiftCarrierInfoMaterial.CARRIERID}', '{ScLiftCarrierInfoMaterial.DEVICETYPE}', '{ScLiftCarrierInfoMaterial.QTIME}', '{ScLiftCarrierInfoMaterial.CYCLETIME}')");
        }

        public void SetScTransportrEquirementMaterial(SaaScTransportrEquirementMaterial TransportrEquirement)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_TRANSPORTR_EQUIREMENT_MATERIAL(SETNO, MODEL_NAME, STATION_NAME, REPORTID, REPORT_TIME , REPORT_STATION , REQUIREMENT_TYPE, CARRIERID, BEGIN_STATION, END_STATION, REQUIREMENT_RESULT) " +
                $"Values('{TransportrEquirement.SETNO}', '{TransportrEquirement.MODEL_NAME}', '{TransportrEquirement.STATION_NAME}', '{TransportrEquirement.REPORTID}','{TransportrEquirement.REPORT_TIME}', '{TransportrEquirement.REPORT_STATION}', '{TransportrEquirement.REQUIREMENT_TYPE}', '{TransportrEquirement.CARRIERID}','{TransportrEquirement.BEGIN_STATION}', '{TransportrEquirement.END_STATION}', '{TransportrEquirement.REQUIREMENT_RESULT}')");
        }

        public void SetScAlarmHistory(SaaScAlarmHistory AlarmHistory)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_ALARM_HISTORY(SETNO, TRN_TIME, MODEL_NAME, STATION_NAME, ALARM_CODE , ALARM_MAG , ALARM_TYPE, ALARM_STATUS, REPORT_STATUS, START_TIME, END_TIME) " +
                $"Values('{AlarmHistory.SETNO}', '{AlarmHistory.TRN_TIME}', '{AlarmHistory.MODEL_NAME}', '{AlarmHistory.STATION_NAME}','{AlarmHistory.ALARM_CODE}', '{AlarmHistory.ALARM_MAG}', '{AlarmHistory.ALARM_TYPE}', '{AlarmHistory.ALARM_STATUS}','{AlarmHistory.REPORT_STATUS}', '{AlarmHistory.START_TIME}', '{AlarmHistory.END_TIME}')");
        }

        public void SetSaaScLiftE84History(SaaScLiftE84History lifte84history)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_LIFT_E84_HISTORY Values('{lifte84history.TASKDATETIME}', '{lifte84history.STATION_NAME}',  '{lifte84history.SHUTTLEID}', '{lifte84history.COMMANDID}', '{lifte84history.CARRIERID}', '{lifte84history.HO_AVBL}', '{lifte84history.ES}', '{lifte84history.L_REQ}', " +
                $"'{lifte84history.UL_REQ}', '{lifte84history.READY}', '{lifte84history.GO}', '{lifte84history.VA}', '{lifte84history.VS_0}', '{lifte84history.VS_1}', '{lifte84history.AM_AVBL}', '{lifte84history.CS_0}', '{lifte84history.VALID}', '{lifte84history.TR_REQ}', '{lifte84history.BUSY}', '{lifte84history.COMPLETE}', '{lifte84history.CONTINUE}', '{lifte84history.SELECT}', '{lifte84history.SOURCE}')");
        }

        public void SetEquipmentReportHistory(SaaScEquipmentReportHistory EquipmentReport)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_EQUIPMENT_REPORT_HISTORY(TASKDATETIME, SETNO, MODEL_NAME, STATION_NAME, CARRIERID, REPORE_DATATRACK, REPORE_DATAREMOTE, REPORE_DATALOCAL, SENDFLAG, UPDATETASKDATETIME) Values('{EquipmentReport.TASKDATETIME}','{EquipmentReport.SETNO}', '{EquipmentReport.MODEL_NAME}', '{EquipmentReport.STATION_NAME}', '{EquipmentReport.CARRIERID}', '{EquipmentReport.REPORE_DATATRACK}', '{EquipmentReport.REPORE_DATAREMOTE}', '{EquipmentReport.REPORE_DATALOCAL}', '{EquipmentReport.SENDFLAG}', '{EquipmentReport.UPDATETASKDATETIME}')");
        }

        public void SetScTransportrEquirement(SaaScTransportrEquirement TransportrEquirement)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_TRANSPORTR_EQUIREMENT(SETNO, MODEL_NAME, STATION_NAME, REPORTID, REPORT_TIME , REPORT_STATION , REQUIREMENT_TYPE, CARRIERID, BEGIN_STATION, END_STATION) " +
            $"Values('{TransportrEquirement.SETNO}', '{TransportrEquirement.MODEL_NAME}', '{TransportrEquirement.STATION_NAME}', '{TransportrEquirement.REPORTID}','{TransportrEquirement.REPORT_TIME}', '{TransportrEquirement.REPORT_STATION}', '{TransportrEquirement.REQUIREMENT_TYPE}', '{TransportrEquirement.CARRIERID}','{TransportrEquirement.BEGIN_STATION}', '{TransportrEquirement.END_STATION}')");
        }

        public void SetScLifte84PlcHistory(SaaScLiftE84Plc lifte84plc)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_E84PLC_HISTORY(TASKDATETIME, STATION_NAME, SHUTTLEID, COMMANDID, CARRIERID, CS_0, Valid, TR_REQ, Busy, Complete, [Continue], [SELECT], AM_AVBL) Values('{lifte84plc.TASKDATETIME}','{lifte84plc.STATION_NAME}','{lifte84plc.SHUTTLEID}','{lifte84plc.COMMANDID}','{lifte84plc.CARRIERID}','{lifte84plc.CS_0}','{lifte84plc.Valid}','{lifte84plc.TR_REQ}','{lifte84plc.Busy}','{lifte84plc.Complete}','{lifte84plc.Continue}','{lifte84plc.SELECT}','{lifte84plc.AM_AVBL}')");
        }

        public void SetScLiftE84Pc_History(string taskdatetime, string station_name, string shuttleid, string commandid, string carrierid, Handshake handshake)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert Into SC_LIFT_E84PC_HISTORY(TASKDATETIME, STATION_NAME, SHUTTLEID, COMMANDID, CARRIERID, Mode, VALID, CS_0, CS_1, TR_REQ, L_REQ, U_REQ, READY, BUSY, COMPT, CONT, HO_AVBL, ES, VA, AM_AVBL, VS_0, VS_1) Values('{taskdatetime}', '{station_name}', '{shuttleid}', '{commandid}', '{carrierid}', '{handshake.Mode}', '{handshake.VALID}', '{handshake.CS_0}', '{handshake.CS_1}', '{handshake.TR_REQ}'," +
                $" '{handshake.L_REQ}', '{handshake.U_REQ}', '{handshake.READY}', '{handshake.BUSY}', '{handshake.COMPT}', '{handshake.CONT}', '{handshake.HO_AVBL}', '{handshake.ES}', '{handshake.VA}', '{handshake.AM_AVBL}', '{handshake.VS_0}', '{handshake.VS_1}')");
        }

        public void SetScLiftCarrierInfoReject(SaaScLiftCarrierInfoReject ScLiftCarrierInfoReject)
        {
            SaaSql.WriteSqlByAutoOpen($"Insert into SC_LIFT_CARRIER_INFO_REJECT(TASKDATETIME, SETNO, MODEL_NAME, STATION_NAME, CARRIERID, DEVICETYPE, QTIME, CYCLETIME) Values('{ScLiftCarrierInfoReject.TASKDATETIME}','{ScLiftCarrierInfoReject.SETNO}','{ScLiftCarrierInfoReject.MODEL_NAME}','{ScLiftCarrierInfoReject.STATION_NAME}','{ScLiftCarrierInfoReject.CARRIERID}', '{ScLiftCarrierInfoReject.DEVICETYPE}', '{ScLiftCarrierInfoReject.QTIME}', '{ScLiftCarrierInfoReject.CYCLETIME}')");
        }

        /*===============================刪除=======================================*/

        #region [===刪除使用者===]
        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="userid">帳號</param>
        public void DelGuiUser(string userid)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From GUI_USER Where USERID = '" + userid + "'");
        }
        #endregion

        #region [===刪除使用者登入 ===]
        /// <summary>
        /// 刪除使用者登入
        /// </summary>
        /// <param name="programname">程式名稱</param>
        public void DelGuiLoginStatus(string programname)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From GUI_LOGINSTATUS Where PROGRAMNAME = '" + programname + "'");
        }
        #endregion

        #region [===刪除RejectList===]
        /// <summary>
        /// 刪除RejectList
        /// </summary>
        /// <param name="screjectlis"></param>
        public void DelRejectList(SaaScRejectList screjectlis)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_REJECT_LIST Where SETNO = '" + screjectlis.SETNO + "' And MODEL_NAME = '" + screjectlis.MODEL_NAME + "' And LOCAL_REJECT_CODE = '" + screjectlis.LOCAL_REJECT_CODE + "'");
        }
        #endregion

        public void DelEquipmentReport(SaaScEquipmentReportHistory EquipmentReport)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_EQUIPMENT_REPORT Where STATION_NAME = '" + EquipmentReport.STATION_NAME + "' And CARRIERID = '" + EquipmentReport.CARRIERID + "' And SENDFLAG = '" + EquipmentReport.SENDFLAG + "'");
        }

        public void DelEquipmentCarrierInfo(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_EQUIPMENT_CARRIER_INFO Where SETNO = '" + equipmentcarrierinfo.SETNO + "' And MODEL_NAME = '" + equipmentcarrierinfo.MODEL_NAME + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "' And CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "'");
        }

        public void DelLiftCarrierInfo(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_CARRIER_INFO Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATION_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERID = '" + LiftCarrierInfo.CARRIERID + "'");
            SAA_Database.LogMessage("Delete From SC_LIFT_CARRIER_INFO Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATION_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERID = '" + LiftCarrierInfo.CARRIERID + "'");
        }

        public void DelLiftAmount(SaaScLiftAmount LiftAmount)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_AMOUNT Where SETNO = '" + LiftAmount.SETNO + "' And MODEL_NAME = '" + LiftAmount.MODEL_NAME + "' And STATION_NAME = '" + LiftAmount.STATION_NAME + "'");
        }

        public void DelLiftAmount(string stationname)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_AMOUNT Where STATION_NAME = '" + stationname + "'");
        }

        public void DelScDirective(SaaScDirective scdirective, string sendflag)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_DIRECTIVE Where SETNO = '" + scdirective.SETNO + "' And STATION_NAME = '" + scdirective.STATION_NAME + "' And SENDFLAG = '" + sendflag + "'");
        }

        public void DelLiftCarrierInfoEmptyCarrier(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_LIFT_CARRIER_INFO_EMPTY Where SETNO = '" + ScLiftCarrierInfoEmpty.SETNO + "' And MODEL_NAME = '" + ScLiftCarrierInfoEmpty.MODEL_NAME + "' And STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "' And CARRIERID = '" + ScLiftCarrierInfoEmpty.CARRIERID + "'");
        }

        public void DelLiftCarrierInfoMaterial(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_LIFT_CARRIER_INFO_MATERIAL Where SETNO = '" + ScLiftCarrierInfoEmpty.SETNO + "' And MODEL_NAME = '" + ScLiftCarrierInfoEmpty.MODEL_NAME + "' And STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "' And CARRIERID = '" + ScLiftCarrierInfoEmpty.CARRIERID + "'");
        }

        public void DelLiftCarrierInfoReject(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_LIFT_CARRIER_INFO_REJECT Where SETNO = '" + ScLiftCarrierInfoEmpty.SETNO + "' And MODEL_NAME = '" + ScLiftCarrierInfoEmpty.MODEL_NAME + "' And STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "' And CARRIERID = '" + ScLiftCarrierInfoEmpty.CARRIERID + "'");
        }

        public void DelScCommandTask(string station_name, string carrierid)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_COMMAND_TASK Where STATION_NAME = '" + station_name + "' And CARRIERID = '" + carrierid + "'");
        }

        public void DelScTransportrEquirement(string station_name, string carrierid)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_TRANSPORTR_EQUIREMENT Where STATION_NAME = '" + station_name + "' And CARRIERID = '" + carrierid + "'");
        }

        public void DelScTransportrEquirementMaterial(string station_name, string carrierid)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_TRANSPORTR_EQUIREMENT_MATERIAL Where STATION_NAME = '" + station_name + "' And CARRIERID = '" + carrierid + "'");
        }

        public void DelScLiftAmount(string station_name)
        {
            SaaSql.WriteSqlByAutoOpen($"Delete From SC_LIFT_AMOUNT Where STATION_NAME = '" + station_name + "'");
        }

        public void DelScAlarmCurrent(SaaScAlarmCurrent AlarmCurrent)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_ALARM_CURRENT Where STATION_NAME = '" + AlarmCurrent.STATION_NAME + "' And ALARM_CODE = '" + AlarmCurrent.ALARM_CODE + "' And REPORT_STATUS = 'Y' And START_TIME is not null and END_TIME is not null");
        }

        public void DelScLifte84Plc(string result)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_E84PLC Where RESULT = '" + result + "'");
        }

        public void DelScE84Pc(string station_name, string result)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_E84PC Where STATION_NAME = '" + station_name + "' And RESULT = '" + result + "'");
        }

        public void DelScE84Plc(string station_name, string result)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_E84PLC Where STATION_NAME = '" + station_name + "' And RESULT = '" + result + "'");
        }

        public void DelScLiftE84PlcHandshakeInfo(string sendflag)
        {
            SaaSql.WriteSqlByAutoOpen("Delete From SC_LIFT_E84PLC_HANDSHAKE_INFO Where SENDFLAG = '" + sendflag + "'");
        }

        /*===============================更新=======================================*/

        #region [===更新使用者===]
        /// <summary>
        /// 更新使用者
        /// </summary>
        /// <param name="guiuser">使用者屬性</param>
        public void UpdGuiUser(GuiUserAttributes guiuser)
        {
            SaaSql.WriteSqlByAutoOpen("Update GUI_USER Set PWD = '" + guiuser.PWD + "', GROUPID = '" + guiuser.GROUPID + "', USERNAME = '" + guiuser.USERNAME + "' Where USERID = '" + guiuser.USERID + "'");
        }
        #endregion

        #region [===更新使用者登入時間===]
        /// <summary>
        /// 更新使用者登入時間
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <param name="lastlogintime">登入時間(yyyy-MM-dd HH:mm:ss.fff)</param>
        public void UpdGuiUserLoginTime(string userid, string lastlogintime)
        {
            SaaSql.WriteSqlByAutoOpen("Update GUI_USER Set LAST_LOGIN_TIME = '" + lastlogintime + "' Where USERID = '" + userid + "'");
        }
        #endregion

        #region [===更新上報Inedex===]
        /// <summary>
        /// 更新上報Inedex
        /// </summary>
        /// <param name="reportinadx"></param>
        public void UpdScReportIndex(SaaScReportInadx reportinadx)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_REPORT_INDEX Set REPORT_INDEX = '" + reportinadx.REPORT_INDEX + "' Where SETNO = '" + reportinadx.SETNO + "' And MODEL_NAME = '" + reportinadx.MODEL_NAME + "' And REPORT_NAME = '" + reportinadx.REPORT_NAME + "'");
        }
        #endregion

        #region [===更新設備狀態===]
        /// <summary>
        /// 更新設備狀態
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentStatus(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set EQUIPMENT_STATUS = '" + equipmentstatus.EQUIPMENT_STATUS + "', EQUIPMENT_STATUS_CODE = '" + equipmentstatus.EQUIPMENT_STATUS_CODE + "', STATUS_UPDATE_TIME = '" + equipmentstatus.STATUS_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        }
        #endregion

        #region [===更新設備模式===]
        /// <summary>
        /// 更新設備模式
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentModel(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set EQUIPMENT_MODEL = '" + equipmentstatus.EQUIPMENT_MODEL_CODE + "', EQUIPMENT_MODEL_CODE = '" + equipmentstatus.EQUIPMENT_STATUS_CODE + "', MODEL_UPDATE_TIME = '" + equipmentstatus.MODEL_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        }
        #endregion

        #region [===更新LCS狀態===]
        /// <summary>
        /// 更新LCS狀態
        /// </summary>
        /// <param name="equipmentstatus"></param>
        public void UpdSaaEquipmentLcsStatus(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_STATUS Set EQUIPMENT_STATUS = '" + equipmentstatus.LCS_STATUS + "', EQUIPMENT_STATUS_CODE = '" + equipmentstatus.LCS_STATUS_CODE + "', STATUS_UPDATE_TIME = '" + equipmentstatus.LCS_UPDATE_TIME + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "'");
        }
        #endregion

        #region [===更新平台狀態===]
        /// <summary>
        /// 更新平台狀態
        /// </summary>
        /// <param name="saalocationsetting"></param>
        public void UpdScLocationSettingStatus(SaaScLocationSetting saalocationsetting)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LOCATIONSETTING Set LOCATIONSTATUS = '" + saalocationsetting.LOCATIONSTATUS + "' Where SETNO = '" + saalocationsetting.SETNO + "' And MODEL_NAME = '" + saalocationsetting.MODEL_NAME + "' And HOSTID = '" + saalocationsetting.HOSTID + "'");
        }
        #endregion

        #region [===更新平台模式===]
        /// <summary>
        /// 更新平台模式
        /// </summary>
        /// <param name="saalocationsetting"></param>
        public void UpdScLocationSettingMode(SaaScLocationSetting saalocationsetting)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LOCATIONSETTING Set LOCATIONMODE = '" + saalocationsetting.LOCATIONMODE + "' Where SETNO = '" + saalocationsetting.SETNO + "' And MODEL_NAME = '" + saalocationsetting.MODEL_NAME + "' And HOSTID = '" + saalocationsetting.HOSTID + "'");
        }
        #endregion

        #region [===更新傳送指令資料===]
        /// <summary>
        /// 更新傳送指令資料
        /// </summary>
        /// <param name="directive"></param>
        public void UpdScDirective(SaaScDirective directive)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_DIRECTIVE Set SENDFLAG = '" + directive.SENDFLAG + "' Where TASKDATETIME = '" + directive.TASKDATETIME + "' And STATION_NAME = '" + directive.STATION_NAME + "' And COMMANDID = '" + directive.COMMANDID + "' And SOURCE = '" + directive.SOURCE + "' And CARRIERID = '" + directive.CARRIERID + "'");
        }
        #endregion

        public void UpdScLifttE84Plc(SaaScLiftE84Plc LiftE84Plc)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_E84PLC Set RESULT = '" + LiftE84Plc.RESULT + "' Where TASKDATETIME = '" + LiftE84Plc.TASKDATETIME + "' And STATION_NAME = '" + LiftE84Plc.STATION_NAME + "' And COMMANDID = '" + LiftE84Plc.COMMANDID + "'");
        }

        public void UpdateScLiftE84PcStatus(string taskdatetime, string station_name, string shuttleid, string commandid, string carrierid, Handshake handshake)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_E84PC_STATSUS Set TASKDATETIME = '" + taskdatetime + "', SHUTTLEID = '" + shuttleid + "', COMMANDID = '" + commandid + "', CARRIERID = '" + carrierid + "', Mode = '" + handshake.Mode + "', L_REQ = '" + handshake.L_REQ + "', U_REQ = '" + handshake.U_REQ + "', READY = '" + handshake.READY + "', HO_AVBL = '" + handshake.HO_AVBL + "', ES = '" + handshake.ES + "', VA = '" + handshake.VA + "', VS_0 = '" + handshake.VS_0 + "', VS_1 = '" + handshake.VS_1 + "' Where STATION_NAME = '" + station_name + "'");
        }

        public void UpdateScEquipmentStatusReply(SaaScEquipmentStatus equipmentstatus)
        {
            SaaSql.WriteSqlByAutoOpen($"Update SC_EQUIPMENT_STATUS Set READREPLY = '" + equipmentstatus.READREPLY + "' Where SETNO = '" + equipmentstatus.SETNO + "' And MODEL_NAME = '" + equipmentstatus.MODEL_NAME + "' And STATION_NAME = '" + equipmentstatus.STATION_NAME + "'");
        }

        public void UpdScEquipmentReport(SaaScEquipmentReport equipmentreport)
        {
            SaaSql.WriteSqlByAutoOpen($"Update SC_EQUIPMENT_REPORT Set SENDFLAG = '" + equipmentreport.SENDFLAG + "' Where TASKDATETIME = '" + equipmentreport.TASKDATETIME + "' And SETNO = '" + equipmentreport.SETNO + "' And MODEL_NAME = '" + equipmentreport.MODEL_NAME + "' And STATION_NAME ='" + equipmentreport.STATION_NAME + "' And CARRIERID ='" + equipmentreport.CARRIERID + "' And REPORE_DATATRACK ='" + equipmentreport.REPORE_DATATRACK + "' And REPORE_DATAREMOTE = '" + equipmentreport.REPORE_DATAREMOTE + "' And REPORE_DATALOCAL='" + equipmentreport.REPORE_DATALOCAL + "'");
        }

        public void UpdScEquipmentReportJumpDie(SaaScEquipmentReport equipmentreport)
        {
            SaaSql.WriteSqlByAutoOpen($"Update SC_EQUIPMENT_REPORT Set SENDFLAG = '" + equipmentreport.SENDFLAG + "' Where TASKDATETIME = '" + equipmentreport.TASKDATETIME + "' And SETNO = '" + equipmentreport.SETNO + "' And MODEL_NAME = '" + equipmentreport.MODEL_NAME + "' And STATION_NAME ='" + equipmentreport.STATION_NAME + "' And CARRIERID ='" + equipmentreport.CARRIERID + "' And REPORE_DATATRACK ='" + equipmentreport.REPORE_DATATRACK + "'");
        }

        public void UpdScEquipmnetHardwareInfo(SaaScEquipmnetHardwareInfo hardwareinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMNET_HARDWARE_INFO Set SENDFLAG = '" + hardwareinfo.SENDFLAG + "' Where STATION_NAME = '" + hardwareinfo.STATION_NAME + "' And EQUIPMNET_TIME = '" + hardwareinfo.EQUIPMNET_TIME + "' And EQUIPMNET_TEID = '" + hardwareinfo.EQUIPMNET_TEID + "' ");
        }

        public void UpdScEquipmnetPlcHandshakeInfo(SaaScLiftE84PlcHandshakeInfo plchandshakeinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_E84PLC_HANDSHAKE_INFO Set SENDFLAG = '" + plchandshakeinfo.SENDFLAG + "' Where STATION_NAME = '" + plchandshakeinfo.STATION_NAME + "' And EQUIPMNET_TIME = '" + plchandshakeinfo.EQUIPMNET_TIME + "' And EQUIPMNET_TEID = '" + plchandshakeinfo.EQUIPMNET_TEID + "' ");
        }

        public void UpdScScPurchase(int setno, string statiomname, string carrierid, string replyresult)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_PURCHASE Set REPLYRESULT = '" + replyresult + "' Where SETNO = '" + setno + "' And STATION_NAME = '" + statiomname + "' And CARRIERID = '" + carrierid + "'");
            SAA_Database.LogMessage($"【SQL語法】【UpdScScPurchase】Update SC_PURCHASE Set REPLYRESULT = '" + replyresult + "' Where SETNO = '" + setno + "' And STATION_NAME = '" + statiomname + "' And CARRIERID = '" + carrierid + "'");
        }

        public void UpdEquipmentCarrierInfo(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_CARRIER_INFO Set PARTNO = '" + equipmentcarrierinfo.PARTNO + "', CARRIERTYOE = '" + equipmentcarrierinfo.CARRIERTYOE + "', ROTFLAG = '" + equipmentcarrierinfo.ROTFLAG + "', FLIPFLAG = '" + equipmentcarrierinfo.FLIPFLAG + "', CARRIERSTATE = '" + equipmentcarrierinfo.CARRIERSTATE + "', DESTINATIONTYPE = '" + equipmentcarrierinfo.DESTINATIONTYPE + "', REJECT_CODE = '" + equipmentcarrierinfo.REJECT_CODE + "', REJECT_MESSAGE = '" + equipmentcarrierinfo.REJECT_MESSAGE + "' Where SETNO = '" + equipmentcarrierinfo.SETNO + "' And MODEL_NAME = '" + equipmentcarrierinfo.MODEL_NAME + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "' And CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "'");
        }

        public void UpdEquipmentCarrierInfoNotPartno(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_CARRIER_INFO Set CARRIERTYOE = '" + equipmentcarrierinfo.CARRIERTYOE + "', ROTFLAG = '" + equipmentcarrierinfo.ROTFLAG + "', FLIPFLAG = '" + equipmentcarrierinfo.FLIPFLAG + "', CARRIERSTATE = '" + equipmentcarrierinfo.CARRIERSTATE + "', DESTINATIONTYPE = '" + equipmentcarrierinfo.DESTINATIONTYPE + "', REJECT_CODE = '" + equipmentcarrierinfo.REJECT_CODE + "', REJECT_MESSAGE = '" + equipmentcarrierinfo.REJECT_MESSAGE + "' Where SETNO = '" + equipmentcarrierinfo.SETNO + "' And MODEL_NAME = '" + equipmentcarrierinfo.MODEL_NAME + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "' And CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "'");
        }

        //public void UpdScLiftCarrierInfo(SaaScLiftCarrierInfo LiftCarrierInfo)
        //{
        //    SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_CARRIER_INFO Set PARTNO = '" + LiftCarrierInfo.PARTNO + "', CARRIERTYPE = '" + LiftCarrierInfo.CARRIERTYPE + "', ROTFLAG = '" + equipmentcarrierinfo.ROTFLAG + "', FLIPFLAG = '" + equipmentcarrierinfo.FLIPFLAG + "' Where SETNO = '" + equipmentcarrierinfo.SETNO + "' And MODEL_NAME = '" + equipmentcarrierinfo.MODEL_NAME + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "' And CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "'");
        //}
        public void UpdScLiftCarrierInfoCarrieridUpdate(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_CARRIER_INFO Set CARRIERID_UPDATE = '" + LiftCarrierInfo.CARRIERID_UPDATE + "' Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATIOM_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERID = '" + LiftCarrierInfo.CARRIERID + "'");
        }

        public void UpdScLiftCarrierInfoCallShuttle(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_CARRIER_INFO Set CALL_SHUTTLE = '" + LiftCarrierInfo.CALL_SHUTTLE + "' Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATIOM_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERID = '" + LiftCarrierInfo.CARRIERID + "'");
        }

        public void UpdScTransportrEquirement(string carrier, string requirement_result)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_TRANSPORTR_EQUIREMENT Set REQUIREMENT_RESULT ='" + requirement_result + "' Where CARRIERID = '" + carrier + "'");
            SAA_Database.LogMessage("Update SC_TRANSPORTR_EQUIREMENT Set REQUIREMENT_RESULT ='" + requirement_result + "' Where CARRIERID = '" + carrier + "'");
        }

        public void UpdScTransportrEquirementMaterial(string carrier, string requirement_result)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_TRANSPORTR_EQUIREMENT_MATERIAL Set REQUIREMENT_RESULT ='" + requirement_result + "' Where CARRIERID = '" + carrier + "'");
            SAA_Database.LogMessage("Update SC_TRANSPORTR_EQUIREMENT_MATERIAL Set REQUIREMENT_RESULT ='" + requirement_result + "' Where CARRIERID = '" + carrier + "'");
        }

        public void UpdScLiftCarrierInfoEmpty(string stationname, string carrierid, string sendflag)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_LIFT_CARRIER_INFO_EMPTY Set SENDFLAG ='" + sendflag + "' Where CARRIERID = '" + carrierid + "' And STATION_NAME = '" + stationname + "'");
            SAA_Database.LogMessage("Update SC_LIFT_CARRIER_INFO_EMPTY Set SENDFLAG ='" + sendflag + "' Where CARRIERID = '" + carrierid + "' And STATION_NAME = '" + stationname + "'");
        }

        public void UpdScEquipmentCarrierInfo(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_CARRIER_INFO Set REJECT_CODE ='" + equipmentcarrierinfo.REJECT_CODE + "', REJECT_MESSAGE = '" + equipmentcarrierinfo.REJECT_MESSAGE + "' Where CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "'");
            SAA_Database.LogMessage("Update SC_EQUIPMENT_CARRIER_INFO Set REJECT_CODE ='" + equipmentcarrierinfo.REJECT_CODE + "', REJECT_MESSAGE = '" + equipmentcarrierinfo.REJECT_MESSAGE + "' Where CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "'");
        }

        public void UpdScEquipmentCarrierInfoSendFlag(string stationname, string carrierid, string carrierflag)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_EQUIPMENT_CARRIER_INFO Set CARRIERFLAG ='" + carrierflag + "' Where CARRIERID = '" + carrierid + "' And STATIOM_NAME = '" + stationname + "'");
            SAA_Database.LogMessage("Update SC_EQUIPMENT_CARRIER_INFO Set CARRIERFLAG ='" + carrierflag + "' Where CARRIERID = '" + carrierid + "' And STATIOM_NAME = '" + stationname + "'");
        }

        public void UpdScTransportrEquirement(string stationname)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_TRANSPORTR_EQUIREMENT Set REQUIREMENT_RESULT = Null Where STATION_NAME = '" + stationname + "'");
        }

        public void UpdScAlarmCurrent(SaaScAlarmCurrent AlarmCurrent)
        {
            SaaSql.WriteSqlByAutoOpen("Update SC_ALARM_CURRENT Set REPORT_STATUS = '" + AlarmCurrent.REPORT_STATUS + "' Where STATION_NAME = '" + AlarmCurrent.STATION_NAME + "' And ALARM_CODE = '" + AlarmCurrent.ALARM_CODE + "'");
        }

        /*===============================查詢=======================================*/

        #region [===讀取系統權限===]
        /// <summary>
        /// 讀取使用者
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <param name="pwd">密碼</param>
        /// <returns></returns>
        public DataTable GetGuiUserConfirm(string userid, string pwd)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_USER Where USERID='" + userid + "' And PWD = '" + pwd + "'").Tables[0];
        }
        #endregion

        #region [===讀取帳號===]
        /// <summary>
        /// 讀取帳號
        /// </summary>
        /// <param name="userid">帳號</param>
        /// <returns></returns>
        public bool GetUserName(string userid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_USER Where USERID='" + userid + "'").Tables[0].Rows.Count == 0;
        }
        #endregion

        #region [===讀取使用者登入===]
        /// <summary>
        /// 讀取使用者登入
        /// </summary>
        /// <returns></returns>
        public DataTable GetLoginStatus()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From GUI_LOGINSTATUS").Tables[0];
        }
        #endregion

        #region [===讀取轉譯上報參數設定===]
        /// <summary>
        /// 讀取轉譯上報參數設定
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportConveys()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_CONVEYS").Tables[0];
        }
        #endregion

        #region [===讀取Reject 明細===]
        /// <summary>
        /// 讀取Reject 明細
        /// </summary>
        /// <returns></returns>
        public DataTable GetScRejectList()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST").Tables[0];
        }
        #endregion

        #region [===讀取Reject List Code碼查詢===]
        /// <summary>
        /// 讀取REJECT LIST Code碼查詢
        /// </summary>
        /// <param name="localrejectcode"></param>
        /// <returns></returns>
        public DataTable GetScRejectList(string localrejectcode)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST Where LOCAL_REJECT_CODE = '" + localrejectcode + "'").Tables[0];
        }
        #endregion

        #region [===讀取退盒訊息===]
        /// <summary>
        /// 退盒訊息
        /// </summary>
        /// <param name="localrejectmesage"></param>
        /// <returns></returns>
        public DataTable GetScRejectMessage(string localrejectmesage)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_LIST Where LOCAL_REJECT_MSG = '" + localrejectmesage + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectCarrierIdHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And MODEL_NAME = '" + rejecthistory.MODEL_NAME + "' And CARRIERID LIKE '" + rejecthistory.CARRIERID + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And MODEL_NAME = '" + rejecthistory.MODEL_NAME + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="rejecthistory"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectIDHistory(SaaScRrejectHistory rejecthistory, string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "' And CARRIERID LIKE '" + rejecthistory.CARRIERID + "'").Tables[0];
        }
        #endregion

        #region [===查詢REJECT歷史紀錄===]
        /// <summary>
        /// 查詢REJECT歷史紀錄
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public DataTable GetScRejectHistory(string starttime, string endtime)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REJECT_HISTORY Where REJECT_TIME BETWEEN '" + starttime + "' And '" + endtime + "'").Tables[0];
        }
        #endregion

        #region [===讀取前20筆REJECT歷史資料===]
        /// <summary>
        /// 讀取前20筆REJECT歷史資料
        /// </summary>
        /// <returns></returns>
        public DataTable GetScRejectHistory()
        {
            return SaaSql.QuerySqlByAutoOpen("Select TOP(20) * From SC_REJECT_HISTORY Order By REJECT_TIME DESC").Tables[0];
        }
        #endregion

        #region [===讀取設備機台名稱===]
        public DataTable GetScEquipmentZone()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_ZONE").Tables[0];
        }
        #endregion

        #region [===讀取上報命令===]
        /// <summary>
        /// 讀取上報命令
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportCommand()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND").Tables[0];
        }
        #endregion

        #region [===讀取上報指令===]
        /// <summary>
        /// 讀取上報指令
        /// </summary>
        /// <param name="reportcommand"></param>
        /// <returns></returns>
        public DataTable GetScReportCommand(SaaScReportCommand reportcommand)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND Where SETNO = '" + reportcommand.SETNO + "' And MODEL_NAME = '" + reportcommand.MODEL_NAME + "' And LCS_COMMAND_NAME = '" + reportcommand.LCS_COMMAND_NAME + "' And REPORT_COMMAND='" + reportcommand.REPORT_COMMAND + "' ").Tables[0];
        }
        #endregion

        #region [===讀取上報指令名稱===]
        /// <summary>
        /// 讀取上報指令名稱
        /// </summary>
        /// <returns></returns>
        public DataTable GetScReportCommandLcsName()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select DISTINCT LCS_COMMAND_NAME From SC_REPORT_COMMAND").Tables[0];
        }
        #endregion

        #region [===讀取上報指令===]
        /// <summary>
        /// 讀取上報指令
        /// </summary>
        /// <param name="setno"></param>
        /// <param name="modelname"></param>
        /// <param name="lcscommandname"></param>
        /// <returns></returns>
        public DataTable GetScReportCommand(string setno, string modelname, string lcscommandname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LCS_COMMAND_NAME = '" + lcscommandname + "'").Tables[0];
        }
        #endregion

        #region [===讀取客戶上報代碼===]
        /// <summary>
        /// 讀取客戶上報代碼
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="modelname">機台型號</param>
        /// <param name="lcscommandname">LCS上報名稱</param>
        /// <returns></returns>
        public DataTable GetReportCommandName(string setno, string modelname, string lcscommandname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND_NAME Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LCS_COMMAND_NAME = '" + lcscommandname + "'").Tables[0];
        }
        #endregion

        #region [===讀取客戶上報代碼===]
        /// <summary>
        /// 讀取客戶上報代碼
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="modelname">機台型號</param>
        /// <param name="lcscommandname">LCS上報名稱</param>
        /// <returns></returns>
        public DataTable GetReportCommandName(string setno, string modelname, string lcscommandname, string locationid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_COMMAND_NAME Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LCS_COMMAND_NAME = '" + lcscommandname + "' And LOCATIONID = '" + locationid + "'").Tables[0];
        }
        #endregion

        #region [===查詢機台站點名稱===]
        /// <summary>
        /// 查詢機台站點名稱
        /// </summary>
        /// <param name="setno">機台編號</param>
        /// <param name="modelname">機台型號</param>
        /// <returns></returns>
        public DataTable GetScEquipmentZone(string setno, string modelname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_ZONE Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "'").Tables[0];
        }
        #endregion

        #region [===查詢上報資料表===]
        /// <summary>
        /// 查詢上報資料表
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="commandid">指令編碼</param>
        /// <param name="commandtext">指令內容</param>
        /// <param name="source">來源</param>
        /// <returns></returns>
        public DataTable GetScDirective(int setno, string commandid, string commandtext, string source)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DIRECTIVE Where SETNO = '" + setno + "' And COMMANDID = '" + commandid + "' And COMMANDTEXT = '" + commandtext + "' And SOURCE = '" + source + "'").Tables[0];
        }
        #endregion

        #region [===查詢上報編號===]
        /// <summary>
        /// 查詢上報編號
        /// </summary>
        /// <param name="setno">機台編碼</param>
        /// <param name="modelname">機台型號</param>
        /// <param name="reportname">資料表名稱</param>
        /// <returns></returns>
        public DataTable GetScReportIndex(int setno, string modelname, string reportname, string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_REPORT_INDEX Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And REPORT_NAME = '" + reportname + "' And STATION_NAME = '" + stationname + "'").Tables[0];
        }
        #endregion

        public DataTable GetScDeviceStation(int setno, string modelname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "'").Tables[0];
        }

        public DataTable GetScCommon(int setno, string modelname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_COMMON Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "'").Tables[0];
        }

        public DataTable GetScDevice(int setno, string modelname, string deviceid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And DEVICEID = '" + deviceid + "'").Tables[0];
        }

        public DataTable GetScDevice(int setno, string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And STATION_NAME = '" + stationname + "'").Tables[0];
        }


        public DataTable GetScDeviceModelName(int setno, string model_name)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And MODEL_NAME = '" + model_name + "'").Tables[0];
        }

        public DataTable GetScDevice(string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where STATION_NAME = '" + stationname + "'").Tables[0];
        }

        public DataTable GetScLocationsetting(int setno, string modelname, string locationid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And LOCATIONID = '" + locationid + "'").Tables[0];
        }

        public DataTable GetScScLocationsetting(int setno, string modelname, string stationname, string locationtype)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + stationname + "' And LOCATIONTYPE = '" + locationtype + "'").Tables[0];
        }

        public DataTable GetScScLocationsettingFull(int setno, string modelname, string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + stationname + "' And (CARRIERID is Null or CARRIERID = '') And LOCATIONTYPE = 'Shelf'").Tables[0];
        }

        public DataTable GetScScLocationsettingCarrierId(string statiomname, string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where STATIOM_NAME = '" + statiomname + "' And CARRIERID = '" + carrierid + "' And LOCATIONTYPE = 'Shelf'").Tables[0];
        }

        public DataTable GetScCommon(int setno, string modelname, string itemname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_COMMON Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And ITEM_NAME = '" + itemname + "'").Tables[0];
        }

        public DataTable GetScDirective(string source)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DIRECTIVE Where SOURCE = '" + source + "' And SENDFLAG is null Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetScLiftE84Plc()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_E84PLC Where RESULT is null Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetScLiftTask(SaaScLiftTask sclifttask)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_TASK Where CARRIERID  = '" + sclifttask.CARRIERID + "' And RESULT is null").Tables[0];
        }

        public DataTable GetScLiftE84PcStatsus(string station_name)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_E84PC_STATSUS Where STATION_NAME  = '" + station_name + "'").Tables[0];
        }

        public DataTable GetScEquipmentStatus(string station)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_EQUIPMENT_STATUS Where STATION_NAME = '" + station + "'").Tables[0];
        }

        public DataTable GetScLocationSetting(int setno, string modelname, string hostid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And HOSTID = '" + hostid + "'").Tables[0];
        }

        public DataTable GetScLocationSetting(int setno, string modelname, string statiomname, string locationid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + statiomname + "' And LOCATIONID = '" + locationid + "'").Tables[0];
        }

        public DataTable GetScLocationSettingInfo(int setno, string modelname, string statiomname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + statiomname + "' And LOCATIONTYPE IS NOT NULL").Tables[0];
        }

        public DataTable GetScLocationSettingInfo(int setno, string modelname, string statiomname, string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + statiomname + "' And CARRIERID  = '" + carrierid + "'").Tables[0];
        }

        public DataTable GetScLocationSettingInfoShelf(int setno, string modelname, string statiomname, string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LOCATIONSETTING Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATIOM_NAME = '" + statiomname + "' And CARRIERID  = '" + carrierid + "' And LOCATIONTYPE = 'Shelf'").Tables[0];
        }

        public DataTable GetScEquipmnetHardwareInfo()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_EQUIPMNET_HARDWARE_INFO Where SENDFLAG is Null").Tables[0];
        }

        public DataTable GetScLiftE84PlcHandshakeInfo()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_E84PLC_HANDSHAKE_INFO Where SENDFLAG is Null").Tables[0];
        }

        public DataTable GetScEquipmentReport()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_REPORT Where SENDFLAG is null Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetScEquipmentReportSuccess(string sendflag)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_REPORT Where SENDFLAG='" + sendflag + "' Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetEquipmentCarrierInfo(SaaEquipmentCarrierInfo equipmentcarrierinfo)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_EQUIPMENT_CARRIER_INFO Where SETNO = '" + equipmentcarrierinfo.SETNO + "' And MODEL_NAME = '" + equipmentcarrierinfo.MODEL_NAME + "' And STATIOM_NAME = '" + equipmentcarrierinfo.STATIOM_NAME + "' And CARRIERID = '" + equipmentcarrierinfo.CARRIERID + "'").Tables[0];
        }

        public DataTable GetLiftCarrierInfo(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATION_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERID = '" + LiftCarrierInfo.CARRIERID + "'").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmpty(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATION_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERTYPE = '" + LiftCarrierInfo.CARRIERTYPE + "' And CALL_SHUTTLE is Null And CARRIERID_UPDATE is Not Null").Tables[0];
        }

        public DataTable GetLiftCarrierInfoCount(SaaScLiftCarrierInfo LiftCarrierInfo)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO Where SETNO = '" + LiftCarrierInfo.SETNO + "' And MODEL_NAME = '" + LiftCarrierInfo.MODEL_NAME + "' And STATION_NAME = '" + LiftCarrierInfo.STATION_NAME + "' And CARRIERTYPE = '" + LiftCarrierInfo.CARRIERTYPE + "' And CARRIERID_UPDATE is Not Null").Tables[0];
        }

        public DataTable GetScScLiftAmount(int setno, string modelname, string statiomname)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_AMOUNT Where SETNO = '" + setno + "' And MODEL_NAME = '" + modelname + "' And STATION_NAME = '" + statiomname + "'").Tables[0];
        }

        public DataTable GetScScLiftAmount(string statiomname)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_AMOUNT Where STATION_NAME = '" + statiomname + "'").Tables[0];
        }

        public DataTable GetScTransportrEquirement()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_TRANSPORTR_EQUIREMENT Where REQUIREMENT_RESULT is Null Order By REPORT_TIME").Tables[0];
        }

        public DataTable GetScTransportrEquirementMaterial()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_TRANSPORTR_EQUIREMENT_MATERIAL Where (REQUIREMENT_RESULT is Null or REQUIREMENT_RESULT = '') Order By REPORT_TIME").Tables[0];
        }

        public DataTable GetScTransportrEquirement(string station_name)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_TRANSPORTR_EQUIREMENT Where STATION_NAME = '" + station_name + "' and REQUIREMENT_RESULT is Not Null Order By REPORT_TIME").Tables[0];
        }

        public DataTable GetLiftE84PcCommandId(string commandid, string station_name)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_E84PC Where COMMANDID = '" + commandid + "' And STATION_NAME = '" + station_name + "'").Tables[0];
        }

        public DataTable GetReportStargName(string station_name, string locationid)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_REPORT_STARG_NAME Where STATION_NAME = '" + station_name + "' And LOCATIONID = '" + locationid + "'").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmpty()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_EMPTY Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmpty(string station_name)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_EMPTY Where STATION_NAME = '" + station_name + "' Order By TASKDATETIME").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmptySendFlag()
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_EMPTY Where SENDFLAG is not Null").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmpty(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_EMPTY Where SETNO = '" + ScLiftCarrierInfoEmpty.SETNO + "' And MODEL_NAME = '" + ScLiftCarrierInfoEmpty.MODEL_NAME + "' And STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "'").Tables[0];
        }

        public DataTable GetLiftCarrierInfoEmptyCarrier(SaaScLiftCarrierInfoEmpty ScLiftCarrierInfoEmpty)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_EMPTY Where SETNO = '" + ScLiftCarrierInfoEmpty.SETNO + "' And MODEL_NAME = '" + ScLiftCarrierInfoEmpty.MODEL_NAME + "' And STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "' And CARRIERID = '" + ScLiftCarrierInfoEmpty.CARRIERID + "'").Tables[0];
        }

        public DataTable GetScDeviceModelName(int setno, string model_name, string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And MODEL_NAME = '" + model_name + "' And STATION_NAME = '" + stationname + "'").Tables[0];
        }

        #region [===查詢站點===]
        /// <summary>
        /// 查詢站點
        /// </summary>
        public DataTable GetScLocationsetting()
        {
            return SaaSql.QuerySqlByAutoOpen("Select DISTINCT STATIOM_NAME From SC_LOCATIONSETTING").Tables[0];
        }
        #endregion

        public DataTable GetScDevice()
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Order By STATION_NAME").Tables[0];
        }

        public DataTable GetLiftE84Status(string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_E84PLC_STATUS Where STATION_NAME = '" + stationname + "'").Tables[0];
        }

        public DataTable GetScDeviceStation(int setno, string model_name, string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_DEVICE Where SETNO = '" + setno + "' And  MODEL_NAME = '" + model_name + "' And STATION_NAME = '" + stationname + "'").Tables[0];
        }

        public DataTable GetScCommandTask(string station_name, string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select *  From SC_COMMAND_TASK Where STATION_NAME = '" + station_name + "' And CARRIERID = '" + carrierid + "'").Tables[0];
        }

        #region [===查詢站點===]
        /// <summary>
        /// 查詢站點
        /// </summary>
        public DataTable GetScDeviceStation()
        {
            return SaaSql.QuerySqlByAutoOpen("Select DISTINCT STATION_NAME, DEVICESTATUS From SC_DEVICE").Tables[0];
        }
        #endregion

        public DataTable GetScAlarmCurrent()
        {
            return SaaSql.QuerySqlByAutoOpen("SELECT * FROM SC_ALARM_CURRENT Where (REPORT_STATUS is Null or REPORT_STATUS ='') Order By START_TIME").Tables[0];
        }

        public DataTable GetScAlarmList(string station_name, string alaem_code)
        {
            return SaaSql.QuerySqlByAutoOpen("SELECT * FROM SC_ALARM_LIST Where STATION_NAME = '" + station_name + "' And ALARM_CODE = '" + alaem_code + "'").Tables[0];
        }

        public DataTable GetScLiftE84Plc(string result)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_E84PLC Where  RESULT = '" + result + "'").Tables[0];
        }

        public DataTable GetScTransportrEquirementMaterial(string stationname)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_TRANSPORTR_EQUIREMENT_MATERIAL Where  STATION_NAME = '" + stationname + "'").Tables[0];
        }

        public DataTable GetScTransportrEquirementMaterialCarrierId(string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_TRANSPORTR_EQUIREMENT_MATERIAL Where  CARRIERID = '" + carrierid + "'").Tables[0];
        }

        public DataTable GetLiftE84PcCommandSend(string taskdatetime, string station_name, string shuttleid, string commandid, string carrierid, string result)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_E84PC Where TASKDATETIME = '" + taskdatetime + "' And STATION_NAME = '" + station_name + "' And SHUTTLEID = '" + shuttleid + "' And  COMMANDID = '" + commandid + "' And CARRIERID='" + carrierid + "' And RESULT = '" + result + "'").Tables[0];
        }

        public DataTable GetLiftCarrierInfoMaterial(SaaScLiftCarrierInfoMaterial ScLiftCarrierInfoEmpty)
        {
            return SaaSql.QuerySqlByAutoOpen($"Select * From SC_LIFT_CARRIER_INFO_MATERIAL Where STATION_NAME = '" + ScLiftCarrierInfoEmpty.STATION_NAME + "' And CARRIERID = '" + ScLiftCarrierInfoEmpty.CARRIERID + "'").Tables[0];
        }

        public DataTable GetScLiftCarrierInfoReject(string carrierid)
        {
            return SaaSql.QuerySqlByAutoOpen("Select * From SC_LIFT_CARRIER_INFO_REJECT Where  CARRIERID = '" + carrierid + "'").Tables[0];
        }
    }
}
