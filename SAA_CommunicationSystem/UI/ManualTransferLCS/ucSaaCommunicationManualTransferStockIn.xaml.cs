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
    /// ucSaaCommunicationManualTransferStockIn.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferStockIn : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferStockIn(SaaSendStockIn saasendstockin)
        {
            InitializeComponent();

            TextCmd.Text = saasendstockin.CMD;
            TextStation.Text = saasendstockin.Station;
            TextID.Text = saasendstockin.ID;
            TextNo.Text = saasendstockin.No;
            TextMemo.Text = saasendstockin.Memo;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSendStockIn sendstockin = new SaaSendStockIn
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                    ID = TextID.Text,
                    No = TextNo.Text,
                    Memo = TextMemo.Text,
                };
                command = $"{sendstockin.CMD},{sendstockin.Station},{sendstockin.ID},{sendstockin.No},{sendstockin.Memo}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送入庫指令完成。");
                MessageBox.Show($"手動傳送入庫指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
