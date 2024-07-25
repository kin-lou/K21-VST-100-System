using MaterialDesignThemes.Wpf;
using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.GuiAttributes;
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
    /// ucSaaCommunicationLogin.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationLogin : UserControl
    {
        public delegate void DelSaaCommunicationLogin(GuiUserAttributes guiuser);
        public static event DelSaaCommunicationLogin OnSaaCommunicationLogin;
        public ucSaaCommunicationLogin()
        {
            InitializeComponent();
        }

        #region [===登入確認按鈕===]
        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetLogInConfirm(AccountBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void UsernameTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    GetLogInConfirm(AccountBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PasswordBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    GetLogInConfirm(AccountBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetLogInConfirm(string accesscontro, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(accesscontro) && !string.IsNullOrEmpty(password))
                {
                    var guiuserdata = SAA_Database.SaaSql.GetGuiUserConfirm(accesscontro, password);
                    if (guiuserdata.Rows.Count != 0)
                    {
                        GuiUserAttributes guiuser = new GuiUserAttributes
                        {
                            PROGRAMNAME = SAA_Database.configattributes.SaaSystemsName,
                            USERID = guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.USERID.ToString()].ToString(),
                            USERNAME = guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.USERNAME.ToString()].ToString(),
                            GROUPID = guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.GROUPID.ToString()].ToString(),
                            LAST_LOGIN_TIME = SAA_Database.ReadTime(),
                        };
                        SAA_Database.SaaSql.SetGuiLoginstatus(guiuser);
                        GuiOpetationHistoryAttributes opetationhistory = new GuiOpetationHistoryAttributes
                        {
                            PROJECT_ITEM = SAA_Database.configattributes.SaaSystemsName,
                            OPERATE_DATETIME = SAA_Database.ReadTime(),
                            USERNAME = guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.USERNAME.ToString()].ToString(),
                            OPERATE_CONTENT = $"系統登入成功，帳號:{guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.USERID.ToString()]}，權限:{guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.GROUPID.ToString()]}，使用者名稱{guiuserdata.Rows[0][SAA_DatabaseEnum.GUI_USER.USERNAME.ToString()]}",
                        };
                        SAA_Database.SaaSql.SetGuiOpetationHistory(opetationhistory);
                        SAA_Database.LogMessage($"{opetationhistory.OPERATE_CONTENT}");
                        OnSaaCommunicationLogin?.Invoke(guiuser);
                        Btnenter.Command = DialogHost.CloseDialogCommand;
                    }
                    else
                    {
                        GuiOpetationHistoryAttributes opetationhistory = new GuiOpetationHistoryAttributes
                        {
                            PROJECT_ITEM = SAA_Database.configattributes.SaaSystemsName,
                            OPERATE_DATETIME = SAA_Database.ReadTime(),
                            USERNAME = "尚未登入使用者",
                            OPERATE_CONTENT = "查無此帳號密碼，請重新輸入帳號密碼",
                        };
                        SAA_Database.SaaSql.SetGuiOpetationHistory(opetationhistory);
                        SAA_Database.LogMessage($"{opetationhistory.OPERATE_CONTENT}", SAA_Database.LogType.Error);
                        MessageBox.Show($"{opetationhistory.OPERATE_CONTENT}", "系統登入", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    GuiOpetationHistoryAttributes opetationhistory = new GuiOpetationHistoryAttributes
                    {
                        PROJECT_ITEM = SAA_Database.configattributes.SaaSystemsName,
                        OPERATE_DATETIME = SAA_Database.ReadTime(),
                        USERNAME = "尚未登入使用者",
                        OPERATE_CONTENT = "帳號密碼欄位不可為空值，請重新輸入帳號密碼",
                    };
                    SAA_Database.SaaSql.SetGuiOpetationHistory(opetationhistory);
                    SAA_Database.LogMessage($"{opetationhistory.OPERATE_CONTENT}", SAA_Database.LogType.Error);
                    MessageBox.Show($"{opetationhistory.OPERATE_CONTENT}", "系統登入", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
