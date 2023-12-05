using MaterialDesignThemes.Wpf;
using NLog;
using SAA_CommunicationSystem.UI;
using SAA_CommunicationSystem_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAA_CommunicationSystem
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 讀取SAA Config資料
        /// </summary>
        private readonly SAA_Config saaconfig = new SAA_Config();
        private readonly ucSaaCommunicationElectric _mSaaCommunicationElectric = new ucSaaCommunicationElectric();
        public MainWindow()
        {
            InitializeComponent();

            ucSaaCommunicationLogin.OnSaaCommunicationLogin += UcSaaCommunicationLogin_OnSaaCommunicationLogin;
        }

        private void SAA_CommunicationSystem_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                saaconfig.ConfigReadStatr();
                GdContent.Children.Add(_mSaaCommunicationElectric);
                if (SAA_Database.SaaLog == null)
                    SAA_Database.SaaLog = LogManager.Setup().LoadConfigurationFromFile(SAA_Database.configattributes.SaaLogName).GetCurrentClassLogger();

                if (SAA_Database.SaaSql == null)
                    SAA_Database.SaaSql = new SAA_CommunicationSystem_Lib.SqlData.MsSqlData();

                if (SAA_Database.webapiserver == null)
                    SAA_Database.webapiserver = new SAA_CommunicationSystem_Lib.WebApiServer.SAA_WebApiServer();

                SAA_Database.webapiserver.WebAPIServerSatrt();
                SAA_Database.LogMessage($"設備Web Api Server已啟動，Server IP位置:{SAA_Database.configattributes.WebApiServerIP}");
                SAA_Database.LogMessage("設備資料傳送系統準備開始");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region [===關閉程式===]
        private void SAA_CommunicationSystem_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否要關閉程式", "關閉程式", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SAA_Database.SaaSql.DelGuiLoginStatus();
                    SAA_Database.LogMessage("程式關閉，刪除登入資料");
                    SAA_Database.LogMessage("設備資料傳送系統關閉");
                    e.Cancel = false;
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region [===切換背景顏色===]
        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyTheme(theme => theme.SetBaseTheme(DarkModeToggleButton.IsChecked == true ? Theme.Dark : Theme.Light));
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }
        #endregion

        #region [===登入狀態顯示===]
        private void UcSaaCommunicationLogin_OnSaaCommunicationLogin(SAA_CommunicationSystem_Lib.GuiAttributes.GuiUserAttributes guiuser)
        {
            try
            {
                App.UpdateUi(() =>
                {
                    TexAccount.Text = guiuser.USERNAME;
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        #endregion

        private void BtnLogIn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LogInStatus())
                {
                    SAA_Database.SaaSql.DelGuiLoginStatus();
                    BtnLogIn.Content = "系統登入";
                    TexAccount.Text = "----";
                }
                else 
                {
                    App.UpdateUi(async () =>
                    {
                        var sampleMessageDialog = new ucSaaCommunicationLogin();
                        object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                        if (LogInStatus())
                        {
                            BtnLogIn.Content = "系統登出";
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool LogInStatus()
        {
            return SAA_Database.SaaSql.GetLoginStatus().Rows.Count > 0;
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventargs)
        {
            Console.WriteLine(eventargs);
        }
    }
}
