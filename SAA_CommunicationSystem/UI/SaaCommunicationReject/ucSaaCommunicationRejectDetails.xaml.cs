using SAA_CommunicationSystem_Lib.DataTableAttributes;
using SAA_CommunicationSystem_Lib;
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
using MaterialDesignThemes.Wpf;

namespace SAA_CommunicationSystem.UI.SaaCommunicationReject
{
    /// <summary>
    /// ucSaaCommunicationRejectDetails.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationRejectDetails : UserControl
    {
        public ucSaaCommunicationRejectDetails()
        {
            InitializeComponent();
        }

        private void SaaCommunicationRejectList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetRejectDetails();
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddRejectList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.UpdateUi(async () =>
                {
                    var sampleMessageDialog = new ucSaaCommunicationRejectDetailsAdd();
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdRejectList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetRejectDetails();
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtEedit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button chk = (Button)sender;
                DataGridRow row = FindAncestor<DataGridRow>(chk);
                var rv = (SaaScRejectList)row.Item;
                SaaScRejectList saaScRejectList = new SaaScRejectList
                {
                    SETNO = rv.SETNO,
                    MODEL_NAME = rv.MODEL_NAME,
                    LOCAL_REJECT_CODE = rv.LOCAL_REJECT_CODE,
                    LOCAL_REJECT_MSG = rv.LOCAL_REJECT_MSG,
                    REMOTE_REJECT_CODE = rv.REMOTE_REJECT_CODE,
                    REMOTE_REJECT_MSG = rv.REMOTE_REJECT_MSG
                };
                App.UpdateUi(async () =>
                {
                    var sampleMessageDialog = new ucSaaCommunicationRejectDetailsUpdate(saaScRejectList);
                    object x = await DialogHost.Show(sampleMessageDialog, "RootDialog", ClosingEventHandler);
                });
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GetRejectDetails()
        {
            try
            {
                List<SaaScRejectList> screportconveyslist = new List<SaaScRejectList>();
                var reportconveysdata = SAA_Database.SaaSql.GetScRejectList();
                foreach (DataRow dr in reportconveysdata.Rows)
                {
                    SaaScRejectList saascrejectlist = new SaaScRejectList
                    {
                        SETNO = int.Parse(dr[SAA_DatabaseEnum.SC_REJECT_LIST.SETNO.ToString()].ToString()),
                        MODEL_NAME = dr[SAA_DatabaseEnum.SC_REJECT_LIST.MODEL_NAME.ToString()].ToString(),
                        LOCAL_REJECT_CODE = dr[SAA_DatabaseEnum.SC_REJECT_LIST.LOCAL_REJECT_CODE.ToString()].ToString(),
                        LOCAL_REJECT_MSG = dr[SAA_DatabaseEnum.SC_REJECT_LIST.LOCAL_REJECT_MSG.ToString()].ToString(),
                        REMOTE_REJECT_CODE = dr[SAA_DatabaseEnum.SC_REJECT_LIST.REMOTE_REJECT_CODE.ToString()].ToString(),
                        REMOTE_REJECT_MSG = dr[SAA_DatabaseEnum.SC_REJECT_LIST.REMOTE_REJECT_MSG.ToString()].ToString()
                    };
                    screportconveyslist.Add(saascrejectlist);
                }
                App.UpdateUi(() => { DgAlarmList.ItemsSource = screportconveyslist; });
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

        public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            current = VisualTreeHelper.GetParent(current);
            while (current != null)
            {
                if (current is T)
                    return (T)current;
                current = VisualTreeHelper.GetParent(current);
            };
            return null;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button chk = (Button)sender;
                DataGridRow row = FindAncestor<DataGridRow>(chk);
                var rv = (SaaScRejectList)row.Item;
                SaaScRejectList saaScRejectList = new SaaScRejectList
                {
                    SETNO = rv.SETNO,
                    MODEL_NAME = rv.MODEL_NAME,
                    LOCAL_REJECT_CODE = rv.LOCAL_REJECT_CODE,
                    LOCAL_REJECT_MSG = rv.LOCAL_REJECT_MSG,
                    REMOTE_REJECT_CODE = rv.REMOTE_REJECT_CODE,
                    REMOTE_REJECT_MSG = rv.REMOTE_REJECT_MSG
                };
                if (MessageBox.Show("是否要確定刪除REJECT資料?", "刪除REJECT資料", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SAA_Database.SaaSql.DelRejectList( saaScRejectList );
                    GetRejectDetails();
                    SAA_Database.LogMessage($"已刪除REJECT CODE碼，CODE:{saaScRejectList.LOCAL_REJECT_CODE}，已完成刪除", SAA_Database.LogType.Error);
                    MessageBox.Show($"已刪除REJECT CODE碼，CODE:{saaScRejectList.LOCAL_REJECT_CODE}，已完成刪除", "刪除REJECT", MessageBoxButton.OK, MessageBoxImage.Error);
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
