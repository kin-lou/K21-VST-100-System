using MaterialDesignThemes.Wpf;
using SAA_CommunicationSystem.UI.ManualTransferLCS;
using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.SendAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAA_CommunicationSystem.UI
{
    /// <summary>
    /// ucSaaCommunicationManualTransferLcs.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferLcs : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferLcs()
        {
            InitializeComponent();
        }

        #region [===清除===]
        private void SaaClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendClear sendclear = new SaaSendClear
                    {
                        CMD = SaaSend.CSMName.ClearStorage.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        Loc = "014001",
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferClearStorage(sendclear);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===出庫===]
        private void SaaStockOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendStockOut sendstockout = new SaaSendStockOut
                    {
                        CMD = SaaSend.CSMName.StockOut.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        No = "12354",
                        From = "014001",
                        To = "010617",
                        Direction = "",
                        RGV_ID = "16",
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferStockOut(sendstockout);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===入庫===]
        private void SaaStockIn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendStockIn sendstockin = new SaaSendStockIn
                    {
                        CMD = SaaSend.CSMName.StockIn.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        No = "12354",
                        Memo = "",
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferStockIn(sendstockin);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===出庫退Reject===]
        private void SaaStockOutToReject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendStockOutToReject sendstockouttoreject = new SaaSendStockOutToReject
                    {
                        CMD = SaaSend.CSMName.StockOutToReject.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        No = "12354",
                        From = string.Empty,
                        To = string.Empty,
                        Direction = string.Empty
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferStockOutToReject(sendstockouttoreject);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===入庫退Reject===]
        private void SaaReject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendReject sendreject = new SaaSendReject
                    {
                        CMD = SaaSend.CSMName.Reject.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        No = "12354",
                        To = string.Empty,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferReject(sendreject);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===時間同步===]
        private void SaaTimeSync_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SaaSendTimeSync sendtimesync = new SaaSendTimeSync
                {
                    CMD = SaaSend.CSMName.TimeSync.ToString(),
                    Data = SAA_Database.ReadTime()
                };
                command = $"{sendtimesync.CMD},{sendtimesync.Data}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送時間同步指令完成。");
                MessageBox.Show($"手動傳送入時間同步指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===測試通訊===]
        private void SaaAreYouOK_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SaaSend saasendareyouok = new SaaSend
                {
                    CMD = SaaSend.CSMName.AreYouOK.ToString(),
                };
                command = $"{saasendareyouok.CMD}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送HOST 測試通訊是否斷掉指令完成。");
                MessageBox.Show($"手動傳送HOST 測試通訊是否斷掉指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===詢問儲格資訊===]
        private void SaaStorageInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendStorageInfo sendstorageinfo = new SaaSendStorageInfo
                    {
                        CMD = string.Empty,
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        From = string.Empty,
                        To = string.Empty,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferStorageInfo(sendstorageinfo);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===卡匣查詢===]
        private void SaaInquire_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSend sendInquire = new SaaSend
                    {
                        CMD = SaaSend.CSMName.IsCarrierInWip.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = string.Empty,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferInquire(sendInquire);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===詢問LCS狀態===]
        private void SaaLCS_Sts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SaaSend saasendstatus = new SaaSend
                {
                    CMD = SaaSend.CSMName.AreYouOK.ToString(),
                    Station = SAA_Database.configattributes.SaaEquipmentName.ToString(),
                };
                command = $"{saasendstatus.CMD},{saasendstatus.Station}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送詢問 LCS 狀態指令完成。");
                MessageBox.Show($"手動傳送詢問 LCS 狀態指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===切換LCS狀態===]
        private void SaaStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSend saastatus = new SaaSend
                    {
                        CMD = string.Empty,
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferLcsSwitch(saastatus);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===切換LCS模式===]
        private void SaaLcsMode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSend saastatus = new SaaSend
                    {
                        CMD = string.Empty,
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferLcsSwitch(saastatus);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===出庫至Buffer===]
        private void SaaStockOutToBuffer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendStockOutToBuffer saasendstockouttobuffer = new SaaSendStockOutToBuffer
                    {
                        CMD = SaaSend.CSMName.StockOutToBuffer.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        ID = "MA01234IN",
                        No = "12354",
                        From = "014001",
                        To = "010617",
                        Direction = string.Empty,
                        Type = string.Empty,
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferStockOutToBuffer(saasendstockouttobuffer);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===暫存區數量===]
        private void SaaBufferCount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SaaSend saasendareyouok = new SaaSend
                {
                    CMD = SaaSend.CSMName.BufferCount.ToString(),
                    Station = SAA_Database.configattributes.SaaEquipmentName,
                };
                command = $"{saasendareyouok.CMD},{saasendareyouok.Station}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送Buffer可用數量指令完成。");
                MessageBox.Show($"手動傳送Buffer可用數量指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===詢問機構===]
        private void SaaQueryPortInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    SaaSendQueryPortInfo saasendqueryportinfo = new SaaSendQueryPortInfo
                    {
                        CMD = SaaSend.CSMName.QueryPortInfo.ToString(),
                        Station = SAA_Database.configattributes.SaaEquipmentName,
                        Port = string.Empty
                    };
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferQueryPortInfo(saasendqueryportinfo);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventargs)
        {
            Console.WriteLine(eventargs);
        }
    }
}
