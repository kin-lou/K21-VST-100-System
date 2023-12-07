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
    /// ucSaaCommunicationManualTransferStockOut.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferStockOut : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferStockOut(SaaSendStockOut saasendstockout)
        {
            InitializeComponent();

            TextCmd.Text = saasendstockout.CMD;
            TextStation.Text = saasendstockout.Station;
            TextID.Text = saasendstockout.ID;
            TextNo.Text = saasendstockout.No;
            TextFrom.Text = saasendstockout.From;
            TextTo.Text = saasendstockout.To;
            TextDirection.Text = saasendstockout.Direction;
            TextRGV_ID.Text = saasendstockout.RGV_ID;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSendStockOut sendstockout = new SaaSendStockOut
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                    ID = TextID.Text,
                    No = TextNo.Text,
                    From = TextFrom.Text,
                    To = TextTo.Text,
                    Direction = TextDirection.Text,
                    RGV_ID = TextRGV_ID.Text,
                };
                command = $"{sendstockout.CMD},{sendstockout.Station},{sendstockout.ID},{sendstockout.No},{sendstockout.From},{sendstockout.To},{sendstockout.Direction},{sendstockout.RGV_ID}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送出庫指令完成。");
                MessageBox.Show($"手動傳送出庫指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
