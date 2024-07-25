using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SAA_CommunicationSystem.UI.ManualTransferiLIS
{
    /// <summary>
    /// ucSaaCommunicationManualTransferEsReportTransportRquirenent.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferEsReportTransportRquirenent : UserControl
    {
        public ucSaaCommunicationManualTransferEsReportTransportRquirenent()
        {
            InitializeComponent();
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
            RequirementInfo info = new RequirementInfo
            {
                RequirementType = TextRequirementType.Text,
                CarrierID = TextCarrierID.Text,
                BeginStation = TextBeginStation.Text,
                EndStation = TextEndStation.Text,
            };
            requirementinfo.Add(info);
            App.SaaWebApiSend.WebApiSendTransportRequirement(TextStationID.Text, SAA_Database.ReadTime(), TextTEID.Text, requirementinfo);
        }
    }
}
