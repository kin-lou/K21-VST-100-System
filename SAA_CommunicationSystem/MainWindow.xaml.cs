using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using NLog;
using SAA_CommunicationSystem.UI;
using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib.HandshakeAttributes;
using SAA_CommunicationSystem_Lib.ReceivLiftAttributes;
using SAA_CommunicationSystem_Lib.ReportAttributes;
using SAA_CommunicationSystem_Lib.WebApiSendCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                SAA_CommunicationSystem.Title = $"SAA VST-100 傳送通訊指令 版本{App.GetEdition()} 更新日期:{new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime:yyyy-MM-dd HH:mm:ss}";
                if (SAA_Database.SaaLog == null)
                    SAA_Database.SaaLog = LogManager.Setup().LoadConfigurationFromFile(SAA_Database.configattributes.SaaLogName).GetCurrentClassLogger();

                if (SAA_Database.SaaSql == null)
                    SAA_Database.SaaSql = new SAA_CommunicationSystem_Lib.SqlData.MsSqlData();

                if(SAA_Database.readcommon==null)
                    SAA_Database.readcommon = new SAA_ReadCommon();

                if (SAA_Database.webapiserver == null)
                    SAA_Database.webapiserver = new SAA_CommunicationSystem_Lib.WebApiServer.SAA_WebApiServer();

                SAA_Database.readcommon.ReadScCommon();
                SAA_Database.GetReportCommand();
                TexEquipment.Text = SAA_Database.configattributes.SaaEquipmentName;
                SAA_Database.webapiserver.WebAPIServerSatrt();
                App.SaaWebApiSend.StartWebApiSend();
                App.SaaWebApiSend.StartWebApiSendAlive();
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
                    SAA_Database.SaaSql.DelGuiLoginStatus(SAA_Database.configattributes.SaaSystemsName);
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
                //if (LogInStatus())
                //{
                //    SAA_Database.SaaSql.DelGuiLoginStatus(SAA_Database.configattributes.SaaSystemsName);
                //    BtnLogIn.Content = "系統登入";
                //    TexAccount.Text = "----";
                //}
                //else 
                //{
                //    App.UpdateUi(async () =>
                //    {
                //        var sampleMessageDialog = new ucSaaCommunicationLogin();
                //        object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                //        if (LogInStatus())
                //        {
                //            BtnLogIn.Content = "系統登出";
                //        }
                //    });
                //}
                //SAA_IniFiles saainifile = new SAA_IniFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), SAA_Database.configattributes.SaaDestinationIniName));
                //ArrayList saadatatabledetailsarry = saainifile.ReadKeys(SAA_Database.configattributes.SaaIniParaKeyStation);
                //if (saadatatabledetailsarry != null)
                //{
                //    for (int i = 0; i < saadatatabledetailsarry.Count; i++)
                //    {
                //        string station = saadatatabledetailsarry[i].ToString();
                //        SAA_Database.LogMessage($"建立空盒需求:站點:{station}");
                //        string dataTime = SAA_Database.ReadTime();
                //        string TEID = $"{station}_{dataTime}";
                //        List<RequirementInfo> requirementinfo = new List<RequirementInfo>();
                //        RequirementInfo info = new RequirementInfo
                //        {
                //            RequirementType = "Take_In_EmptyCarrier",
                //            CarrierID = string.Empty,
                //            BeginStation = string.Empty,
                //            EndStation = station,
                //        };
                //        requirementinfo.Add(info);

                //        Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                //        {
                //            { "StationID",  station },
                //            { "Time", dataTime },
                //            { "TEID", TEID },
                //            { "ListRequirementInfo", requirementinfo }
                //        };
                //        string commandcontent = JsonConvert.SerializeObject(dicstatusb);
                //        SAA_Database.LogMessage($"【LCS->iLIS】【通訊傳送】站點:{station}，時間:{dataTime}，傳送編號:{TEID}傳送內容:{commandcontent}");
                //        string returnresult = SAA_Database.SaaSendCommandiLis(commandcontent, "ES_Report_TransportRequirement");
                //        Dictionary<string, string> returndic = JsonConvert.DeserializeObject<Dictionary<string, string>>(returnresult);
                //        if (returndic != null)
                //        {
                //            SaaReportResult saareportresult = new SaaReportResult();
                //            foreach (var result in returndic)
                //            {
                //                var datakey = result.Key;
                //                var datavalue = result.Value;
                //                if (Enum.IsDefined(typeof(WebApiReceive), datakey))
                //                {
                //                    switch ((WebApiReceive)Enum.Parse(typeof(WebApiReceive), datakey))
                //                    {
                //                        case WebApiReceive.StationID:
                //                            saareportresult.StationID = datavalue;
                //                            break;
                //                        case WebApiReceive.Time:
                //                            saareportresult.Time = datavalue;
                //                            break;
                //                        case WebApiReceive.TEID:
                //                            saareportresult.TEID = datavalue;
                //                            break;
                //                        case WebApiReceive.ReturnCode:
                //                            saareportresult.ReturnCode = datavalue;
                //                            break;
                //                        case WebApiReceive.ReturnMessage:
                //                            saareportresult.ReturnMessage = datavalue;
                //                            break;
                //                    }
                //                }
                //            }
                //            SAA_Database.LogMessage($"【LCS->iLIS】【iLIS接收】StationID:{saareportresult.StationID}，Time:{saareportresult.Time}，TEID:{saareportresult.TEID}，ReturnCode:{saareportresult.ReturnCode}，ReturnMessage:{saareportresult.ReturnMessage}，(結果:{saareportresult.ReturnCode})");
                //        }
                //    }
                //}
                //MessageBox.Show("手動發送空盒需求完成");


                //SaaLiftReceive saaLift = new SaaLiftReceive
                //{
                //    Statiom_Name = "K21-8F_ASEF1-2890-A04",
                //    CommandName = SAA_DatabaseEnum.LiftCommandName.EquipmentLiftE84PlcHandshakeInfo.ToString(),
                //};
                //Dictionary<string, object> dicstatusb = new Dictionary<string, object>
                //        {
                //            { SAA_DatabaseEnum.EquipmentStatusCommand.Statiom_Name.ToString(),  saaLift.Statiom_Name},
                //            { SAA_DatabaseEnum.EquipmentStatusCommand.CommandName.ToString(), saaLift.CommandName}
                //        };
                //string commandcontent = JsonConvert.SerializeObject(saaLift);
                //string ReportMessage = SAA_Database.SaaSendCommandSystems(commandcontent, SAA_DatabaseEnum.SendWebApiCommandName.GetLiftMessage.ToString());
                //SAA_Database.LogMessage($"【傳送設備】【{saaLift.Statiom_Name}】【轉譯程式】自行接收結果:{ReportMessage}");
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
