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
using static System.Net.Mime.MediaTypeNames;

namespace SAA_CommunicationSystem.UI.ManualTransferLCS
{
    /// <summary>
    /// ucSaaCommunicationManualTransferStorageInfo.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferStorageInfo : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferStorageInfo(SaaSendStorageInfo sendstorageinfo)
        {
            InitializeComponent();

            TextCmd.Text = sendstorageinfo.CMD;
            TextStation.Text = sendstorageinfo.Station;
            TextFrom.Text = sendstorageinfo.From;
            TextTo.Text = sendstorageinfo.To;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSendStorageInfo sendstorageinfo = new SaaSendStorageInfo
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                    From = TextFrom.Text,
                    To = TextTo.Text,
                };
                command = $"{sendstorageinfo.CMD},{sendstorageinfo.Station},{sendstorageinfo.From},{sendstorageinfo.To}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送詢問儲格資訊指令完成。");
                MessageBox.Show($"手動傳送詢問儲格資訊指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
