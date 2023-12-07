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
    /// ucSaaCommunicationManualTransferStockOutToBuffer.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferStockOutToBuffer : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferStockOutToBuffer(SaaSendStockOutToBuffer saasendstockouttobuffer)
        {
            InitializeComponent();

            TextCmd.Text = saasendstockouttobuffer.CMD;
            TextStation.Text = saasendstockouttobuffer.Station;
            TextID.Text = saasendstockouttobuffer.ID;
            TextNo.Text = saasendstockouttobuffer.No;
            TextFrom.Text = saasendstockouttobuffer.From;
            TextTo.Text = saasendstockouttobuffer.To;
            TextDirection.Text = saasendstockouttobuffer.Direction;
            TextType.Text = saasendstockouttobuffer.Type;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSendStockOutToBuffer sendstockouttobuffer = new SaaSendStockOutToBuffer
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                    ID = TextID.Text,
                    No = TextNo.Text,
                    From = TextFrom.Text,
                    To = TextTo.Text,
                    Direction = TextDirection.Text,
                    Type = TextType.Text,
                };
                command = $"{sendstockouttobuffer.CMD},{sendstockouttobuffer.Station},{sendstockouttobuffer.ID},{sendstockouttobuffer.No},{sendstockouttobuffer.From},{sendstockouttobuffer.To},{sendstockouttobuffer.Direction},{sendstockouttobuffer.Type}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送出庫至Buffer指令完成。");
                MessageBox.Show($"手動傳送出庫至Buffer指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
