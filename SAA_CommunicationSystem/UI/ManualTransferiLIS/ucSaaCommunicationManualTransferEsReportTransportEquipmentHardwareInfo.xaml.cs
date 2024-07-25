using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using SAA_CommunicationSystem_Lib;
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
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem.UI.ManualTransferiLIS
{
    /// <summary>
    /// ucSaaCommunicationManualTransferEsReportTransportEquipmentHardwareInfo.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferEsReportTransportEquipmentHardwareInfo : UserControl
    {
        public ucSaaCommunicationManualTransferEsReportTransportEquipmentHardwareInfo()
        {
            InitializeComponent();
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            List<HardwareInfo> requirementinfo = new List<HardwareInfo>();
            HardwareInfo info = new HardwareInfo
            {
                HardwareID = "1",
                CarrierID = "E888",
                HardwareType = "Port",
                UsingFlag = "True"
            };
            requirementinfo.Add(info);
            List<CarrierInfo> CarrierInfolist = new List<CarrierInfo>();
            CarrierInfo carrierinfo = new CarrierInfo
            {
                CarrierID = "E888",
                CarrierType = "Normal",
                Schedule="597",
                Rotation = "N",
                Flip = "N",
            };
            CarrierInfolist.Add(carrierinfo);
            App.SaaWebApiSend.WebApiSendTransportEquipmentHardwareInfo(TextStationID.Text, SAA_Database.ReadTime(), TextTEID.Text, requirementinfo, CarrierInfolist);
        }
    }
}
