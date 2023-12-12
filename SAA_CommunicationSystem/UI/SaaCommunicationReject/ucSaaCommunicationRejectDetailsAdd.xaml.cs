using SAA_CommunicationSystem_Lib.DataTableAttributes;
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

namespace SAA_CommunicationSystem.UI.SaaCommunicationReject
{
    /// <summary>
    /// ucSaaCommunicationRejectDetailsAdd.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationRejectDetailsAdd : UserControl
    {
        public ucSaaCommunicationRejectDetailsAdd()
        {
            InitializeComponent();
        }

        private void Btnenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TextSetNo.Text))
                {
                    SaaScRejectList screjectlist = new SaaScRejectList
                    {
                        SETNO = int.Parse(TextSetNo.Text),
                        MODEL_NAME = TextModelName.Text,
                        LOCAL_REJECT_CODE = TextLocalRejecTCode.Text,
                        LOCAL_REJECT_MSG = TextLocalRejectMsg.Text,
                        REMOTE_REJECT_CODE = TextRemoteRejectCode.Text,
                        REMOTE_REJECT_MSG = TextRemoteRejectMsg.Text,
                    };
                    if (!string.IsNullOrEmpty(screjectlist.SETNO.ToString()) && !string.IsNullOrEmpty(screjectlist.MODEL_NAME.ToString()) && !string.IsNullOrEmpty(screjectlist.LOCAL_REJECT_CODE.ToString()) && !string.IsNullOrEmpty(screjectlist.LOCAL_REJECT_MSG.ToString()) && !string.IsNullOrEmpty(screjectlist.REMOTE_REJECT_CODE.ToString()) && !string.IsNullOrEmpty(screjectlist.REMOTE_REJECT_MSG.ToString()))
                    {
                        if (SAA_Database.SaaSql.GetScRejectList(screjectlist.LOCAL_REJECT_CODE).Rows.Count == 0)
                        {
                            SAA_Database.SaaSql.SetScRejectList(screjectlist);
                            SAA_Database.LogMessage($"新增REJECT CODE碼:{screjectlist.LOCAL_REJECT_CODE}，已完成新增");
                            MessageBox.Show($"新增REJECT CODE碼:{screjectlist.LOCAL_REJECT_CODE}，已完成新增", "新增REJECT", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        else
                        {
                            SAA_Database.LogMessage($"已有相同的REJECT CODE碼:{screjectlist.LOCAL_REJECT_CODE}，請重新確認");
                            MessageBox.Show($"已有相同的REJECT CODE碼:{screjectlist.LOCAL_REJECT_CODE}，請重新確認", "新增REJECT", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                    }
                    else
                    {
                        SAA_Database.LogMessage("新增REJECT欄位不可空白，請重新確認");
                        MessageBox.Show("新增REJECT欄位不可空白，請重新確認", "新增REJECT", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
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
