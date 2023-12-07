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
    /// ucSaaCommunicationManualTransferLcsSwitch.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferLcsSwitch : UserControl
    {
        private string command = string.Empty;
        private string returnresult = string.Empty;
        public ucSaaCommunicationManualTransferLcsSwitch(SaaSend lcsswitch)
        {
            InitializeComponent();

            TextCmd.Text = lcsswitch.CMD;
            TextStation.Text = lcsswitch.Station;
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaaSend lcsswitch = new SaaSend
                {
                    CMD = TextCmd.Text,
                    Station = TextStation.Text,
                };
                command = $"{lcsswitch.CMD},{lcsswitch.Station}";
                returnresult = SAA_Database.webapisendcommand.Post(SAA_Database.configattributes.StorageWebApiServerIP, SAA_Database.configattributes.ParaKey, command);
                SAA_Database.LogMessage($"【LCS接收】結果:{returnresult}");
                SAA_Database.LogMessage($"手動傳送LCS切換指令完成。");
                MessageBox.Show($"手動傳送LCS切換指令完成。");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
