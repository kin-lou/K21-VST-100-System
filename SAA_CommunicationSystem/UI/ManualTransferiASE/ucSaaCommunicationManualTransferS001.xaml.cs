using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.ReportCommandAttributes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace SAA_CommunicationSystem.UI.ManualTransferiASE
{
    /// <summary>
    /// ucSaaCommunicationManualTransferS001.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferS001 : UserControl
    {
        public ucSaaCommunicationManualTransferS001()
        {
            InitializeComponent();
        }

        private void CmdStation_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                CmdStation.Items.Clear();
                var locationsettingdata = SAA_Database.SaaSql?.GetScLocationsetting();
                if (locationsettingdata != null)
                {
                    foreach (DataRow dr in locationsettingdata.Rows)
                    {
                        CmdStation.Items.Add(dr["STATIOM_NAME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmdstation = CmdStation.Text;
                if (!string.IsNullOrEmpty(cmdstation))
                {
                    SaaReportCommandAutpMation commandAutpMation = new SaaReportCommandAutpMation
                    {
                        CMD_NO = "S001",
                        CMD_NAME = RadStart.IsChecked == true ? "AUTOMATION_ON" : "AUTOMATION_OFF",
                        STATION = cmdstation,
                    };
                    SAA_Database.SaaSendAutoMation(commandAutpMation);
                }
                else
                {
                    MessageBox.Show("站點不可為空，請重新選擇", "選擇站點", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }
    }
}
