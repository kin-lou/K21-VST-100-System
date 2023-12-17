using NLog.LayoutRenderers;
using SAA_CommunicationSystem_Lib;
using SAA_CommunicationSystem_Lib.DataTableAttributes;
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
using static System.Collections.Specialized.BitVector32;

namespace SAA_CommunicationSystem.UI.SaaCommunicationReject
{
    /// <summary>
    /// ucSaaCommunicationRejectDetailsHistory.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationRejectDetailsHistory : UserControl
    {
        public ucSaaCommunicationRejectDetailsHistory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CmdEquipmentName.IsEnabled = false;
                TxtLotId.IsEnabled = false;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmdEquipmentName_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                CmdEquipmentName.Items.Clear();
                var data = SAA_Database.SaaSql.GetScEquipmentZone();
                foreach ( DataRow dr in data.Rows )
                {
                    CmdEquipmentName.Items.Add(dr[SAA_DatabaseEnum.SC_EQUIPMENT_ZONE.MODEL_NAME.ToString()].ToString());
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRejectInquire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DprStartDay.SelectedDate != null&& DprStoptDay.SelectedDate != null)
                {
                    string daystrat = DprStartDay.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00.000");
                    string daystope = DprStoptDay.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59.997");
                    SaaScRrejectHistory scrrejecthistory = new SaaScRrejectHistory
                    {
                        MODEL_NAME = CmdEquipmentName.Text,
                        CARRIERID = TxtLotId.Text,
                    };
                    daystrat = DprStartDay.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00.000");
                    daystope = DprStoptDay.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59.997");
                    RadioButton radiobutton = (RadioButton)sender;
                    if (radiobutton.IsChecked == true)
                    {
                        DataTable data = null;
                        switch ((RadName)Enum.Parse(typeof(RadName), radiobutton.Name))
                        {
                            case RadName.RadDateTime:
                                if (string.IsNullOrEmpty(daystrat) && string.IsNullOrEmpty(daystope))
                                {
                                    SAA_Database.LogMessage($"查詢REJECT資料日期不可空白，請重新選擇", SAA_Database.LogType.Error);
                                    MessageBox.Show($"查詢REJECT資料日期不可空白，請重新選擇", "查詢REJECT", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                }
                                data = SAA_Database.SaaSql.GetScRejectHistory(daystrat, daystope);
                                break;
                            case RadName.RadModel:
                                if (string.IsNullOrEmpty(daystrat) && string.IsNullOrEmpty(daystope) && string.IsNullOrEmpty(scrrejecthistory.MODEL_NAME))
                                {
                                    SAA_Database.LogMessage($"查詢REJECT資料日期或機型不可空白，請重新選擇", SAA_Database.LogType.Error);
                                    MessageBox.Show($"查詢REJECT資料日期或機型不可空白，請重新選擇", "查詢REJECT", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                }
                                data = SAA_Database.SaaSql.GetScRejectHistory(scrrejecthistory, daystrat, daystope);
                                break;
                            case RadName.RadCarrierId:
                                if (string.IsNullOrEmpty(daystrat) && string.IsNullOrEmpty(daystope) && string.IsNullOrEmpty(scrrejecthistory.CARRIERID))
                                {
                                    SAA_Database.LogMessage($"查詢REJECT資料日期或機型不可空白，請重新選擇", SAA_Database.LogType.Error);
                                    MessageBox.Show($"查詢REJECT資料日期或機型不可空白，請重新選擇", "查詢REJECT", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                }
                                data = SAA_Database.SaaSql.GetScRejectIDHistory(scrrejecthistory, daystrat, daystope);
                                break;
                            case RadName.RadModelCarrierId:
                                if (string.IsNullOrEmpty(daystrat) && string.IsNullOrEmpty(daystope) && string.IsNullOrEmpty(scrrejecthistory.CARRIERID))
                                {
                                    SAA_Database.LogMessage($"查詢REJECT資料日期或機型或卡匣ID不可空白，請重新選擇", SAA_Database.LogType.Error);
                                    MessageBox.Show($"查詢REJECT資料日期或機型不可空白，請重新選擇", "查詢REJECT", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                }
                                data = SAA_Database.SaaSql.GetScRejectCarrierIdHistory(scrrejecthistory, daystrat, daystope);
                                break;
                            default:
                                break;
                        }
                        if (data != null)
                        {
                            App.UpdateUi(() => { DgRejectListHistory.ItemsSource = GetRejectHistory(data); });
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"查詢REJECT資料日期不可空白，請重新選擇", "查詢REJECT", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<SaaScRrejectHistory> GetRejectHistory(DataTable db)
        {
            try
            {
                List<SaaScRrejectHistory> scrrejecthistory = new List<SaaScRrejectHistory>();
                foreach (DataRow dr in db.Rows)
                {
                    SaaScRrejectHistory rejectHistory = new SaaScRrejectHistory
                    {
                        SETNO = int.Parse(dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.SETNO.ToString()].ToString()),
                        REJECT_TIME = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.REJECT_TIME.ToString()].ToString(),
                        MODEL_NAME = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.MODEL_NAME.ToString()].ToString(),
                        STATION = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.STATION.ToString()].ToString(),
                        CARRIERID = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.CARRIERID.ToString()].ToString(),
                        PARTNO = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.PARTNO.ToString()].ToString(),
                        LOCAL_REJECT_CODE = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.LOCAL_REJECT_CODE.ToString()].ToString(),
                        LOCAL_REJECT_MSG = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.LOCAL_REJECT_MSG.ToString()].ToString(),
                        REMOTE_REJECT_CODE = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.REMOTE_REJECT_CODE.ToString()].ToString(),
                        REMOTE_REJECT_MSG = dr[SAA_DatabaseEnum.SC_REJECT_HISTORY.REMOTE_REJECT_MSG.ToString()].ToString(),
                    };
                    scrrejecthistory.Add(rejectHistory);
                }
                return scrrejecthistory;
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void BtnUpdRejectList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rejectdata = SAA_Database.SaaSql.GetScRejectHistory();
                if (rejectdata != null)
                {
                    var rejectHistory = GetRejectHistory(rejectdata);
                    if (rejectHistory != null)
                    {
                        App.UpdateUi(() => { DgRejectList.ItemsSource = rejectHistory; });
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RadDateTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radiobutton = (RadioButton)sender;
                if (radiobutton.IsChecked == true)
                {
                    switch ((RadName)Enum.Parse(typeof(RadName), radiobutton.Name))
                    {
                        case RadName.RadDateTime:
                            CmdEquipmentName.IsEnabled = false;
                            TxtLotId.IsEnabled = false;
                            break;
                        case RadName.RadModel:
                            CmdEquipmentName.IsEnabled = true;
                            TxtLotId.IsEnabled = false;
                            break;
                        case RadName.RadCarrierId:
                            CmdEquipmentName.IsEnabled = false;
                            TxtLotId.IsEnabled = true;
                            break;
                        case RadName.RadModelCarrierId:
                            CmdEquipmentName.IsEnabled = true;
                            TxtLotId.IsEnabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}-{ex.StackTrace}", SAA_Database.LogType.Error);
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private enum RadName
        {
            /// <summary>
            /// 時間查詢
            /// </summary>
            RadDateTime,

            /// <summary>
            /// 機型查詢
            /// </summary>
            RadModel,

            /// <summary>
            /// 卡匣查詢
            /// </summary>
            RadCarrierId,

            /// <summary>
            /// 卡匣+機型查詢
            /// </summary>
            RadModelCarrierId,
        }

    }
}
