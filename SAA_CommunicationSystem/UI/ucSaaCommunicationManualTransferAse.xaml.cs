using MaterialDesignThemes.Wpf;
using SAA_CommunicationSystem_Lib.ReportCommandAttributes;
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
using SAA_CommunicationSystem.UI.ManualTransferiASE;

namespace SAA_CommunicationSystem.UI
{
    /// <summary>
    /// ucSaaCommunicationManualTransferAse.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransferAse : UserControl
    {
        public ucSaaCommunicationManualTransferAse()
        {
            InitializeComponent();
        }

        private void ReportEquipmentCommand_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    var sampleMessageDialog = new ucSaaCommunicationManualTransferS001();
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventargs)
        {
            Console.WriteLine(eventargs);
        }
    }
}
