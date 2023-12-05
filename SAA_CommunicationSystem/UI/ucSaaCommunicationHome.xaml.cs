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
using static SAA_CommunicationSystem_Lib.SAA_Database;

namespace SAA_CommunicationSystem.UI
{
    /// <summary>
    /// ucSaaCommunicationHome.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationHome : UserControl
    {
        public delegate void DelDataHome(App.BtnName home);
        public event DelDataHome OnDataHome;
        public ucSaaCommunicationHome()
        {
            InitializeComponent();
        }

        private void SaaLog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnLog);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaSend_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnSend);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaImport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnImport);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaHistor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnHistor);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaStorageInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnStorageInfo);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaRejectList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnRejectList);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaRejectHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnRejectHistory);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaOperationRecord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnOperationRecord);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SaaUserPermissions_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnUserPermissions);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaaSetUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnDataHome?.Invoke(App.BtnName.BtnSetUp);
            }
            catch (Exception ex)
            {
                LogMessage($"{ex.Message}-{ex.StackTrace}", LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
