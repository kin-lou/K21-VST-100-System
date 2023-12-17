using SAA_CommunicationSystem.UI.SaaCommunicationReject;
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

namespace SAA_CommunicationSystem.UI
{
    /// <summary>
    /// ucSaaCommunicationElectric.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationElectric : UserControl
    {
        private UserControl control = new UserControl();
        private readonly ucSaaCommunicationHome _mSaaCommunicationHome = new ucSaaCommunicationHome();
        private readonly ucSaaCommunicationLogDisplay _mSaaCommunicationLogDisplay = new ucSaaCommunicationLogDisplay();
        private readonly ucSaaCommunicationRejectDetails _mSaaCommunicationRejectList = new ucSaaCommunicationRejectDetails();
        private readonly ucSaaCommunicationManualTransfer _mSaaCommunicationManualTransfer = new ucSaaCommunicationManualTransfer();
        private readonly ucSaaCommunicationParameterImport _mSaaCommunicationParameterImport = new ucSaaCommunicationParameterImport();
        private readonly ucSaaCommunicationRejectDetailsHistory _mSaaCommunicationRejectDetailsHistory = new ucSaaCommunicationRejectDetailsHistory();

        public ucSaaCommunicationElectric()
        {
            InitializeComponent();

            foreach (UIElement child in SplButton.Children)
            {
                child.IsEnabled = true;
            }
            BtnMenu.IsEnabled = false;
            GetGrid(_mSaaCommunicationHome);
            _mSaaCommunicationHome.OnDataHome += _mSaaCommunicationHome_OnDataHome
                ;
        }

        private void _mSaaCommunicationHome_OnDataHome(App.BtnName home)
        {
            UIInfo(home.ToString());
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                UIInfo(button.Name);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }

        private void UIInfo(string uiname)
        {
            try
            {
                foreach (UIElement child in SplButton.Children)
                {
                    child.IsEnabled = true;
                }

                switch ((App.BtnName)Enum.Parse(typeof(App.BtnName), uiname))
                {
                    case App.BtnName.BtnMenu:
                        GetGrid(_mSaaCommunicationHome);
                        BtnMenu.IsEnabled = false;
                        break;
                    case App.BtnName.BtnLog:
                        GetGrid(_mSaaCommunicationLogDisplay);
                        BtnLog.IsEnabled = false;
                        break;
                    case App.BtnName.BtnSend:
                        GetGrid(_mSaaCommunicationManualTransfer);
                        BtnSend.IsEnabled = false;
                        break;
                    case App.BtnName.BtnImport:
                        GetGrid(_mSaaCommunicationParameterImport);
                        BtnImport.IsEnabled = false;
                        break;
                    case App.BtnName.BtnHistory:
                        BtnHistory.IsEnabled = false;
                        break;
                    case App.BtnName.BtnStorageInfo:
                        BtnStorageInfo.IsEnabled = false;
                        break;
                    case App.BtnName.BtnRejectList:
                        GetGrid(_mSaaCommunicationRejectList);
                        BtnRejectList.IsEnabled = false;
                        break;
                    case App.BtnName.BtnRejectHistory:
                        GetGrid(_mSaaCommunicationRejectDetailsHistory);
                        BtnRejectHistory.IsEnabled = false;
                        break;
                    case App.BtnName.BtnOperationRecord:
                        BtnOperationRecord.IsEnabled = false;
                        break;
                    case App.BtnName.BtnUserPermissions:
                        BtnUserPermissions.IsEnabled = false;
                        break;
                    case App.BtnName.BtnSetUp:
                        BtnSetUp.IsEnabled = false;
                        break;
                    
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetGrid(UserControl user)
        {
            GdContent.Children.Clear();
            control.Visibility = Visibility.Hidden;
            user.Visibility = Visibility.Visible;
            GdContent.Children.Add(user);
        }
    }
}
