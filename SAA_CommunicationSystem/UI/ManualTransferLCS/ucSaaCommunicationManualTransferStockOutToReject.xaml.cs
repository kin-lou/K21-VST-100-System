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

namespace SAA_CommunicationSystem.UI.ManualTransferLCS
{
    /// <summary>
    /// ucSaaCommunicationManualTransferStockOutToReject.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferStockOutToReject : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferStockOutToReject(SaaSendStockOutToReject sendstockouttoreject)
        {
            InitializeComponent();

            TextCmd.Text = sendstockouttoreject.CMD;
            TextStation.Text = sendstockouttoreject.Station;
            TextID.Text = sendstockouttoreject.ID;
            TextNo.Text = sendstockouttoreject.No;
            TextFrom.Text = sendstockouttoreject.From;
            TextTo.Text = sendstockouttoreject.To;
            TextDirection.Text = sendstockouttoreject.Direction;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSendStockOutToReject sendstockouttoreject = new SaaSendStockOutToReject
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                    ID = TextID.Text,
                    No = TextNo.Text,
                    From = TextFrom.Text,
                    To = TextTo.Text,
                    Direction = TextDirection.Text,
                };
                command = $"{sendstockouttoreject.CMD},{sendstockouttoreject.Station},{sendstockouttoreject.ID},{sendstockouttoreject.No},{sendstockouttoreject.From},{sendstockouttoreject.To},{sendstockouttoreject.Direction}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送出庫Reject指令完成。");
                MessageBox.Show($"手動傳送出庫Reject指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
