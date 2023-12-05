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
    /// ucSaaCommunicationLogin.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationLogin : UserControl
    {
        public delegate void DelSaaCommunicationLogin(string loginname);
        public static event DelSaaCommunicationLogin OnSaaCommunicationLogin;
        public ucSaaCommunicationLogin()
        {
            InitializeComponent();
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetLogInConfirm(AccountBox.Text, PasswordBox.Password);
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }

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
            }
        }

        private void GetLogInConfirm(string accesscontro, string password)
        {
            try
            {

            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
            }
        }

    }
}
