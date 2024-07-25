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
    /// ucSaaCommunicationManualTransfer.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationManualTransfer : UserControl
    {
        private UserControl control = new UserControl();
        private readonly ucSaaCommunicationManualTransferiLis _mSaaCommunicationManualTransferiLis = new ucSaaCommunicationManualTransferiLis();
        private readonly ucSaaCommunicationManualTransferLcs _mSaaCommunicationManualTransferLcs = new ucSaaCommunicationManualTransferLcs();
        private readonly ucSaaCommunicationManualTransferAse _mSaaCommunicationManualTransferAse = new ucSaaCommunicationManualTransferAse();

        public ucSaaCommunicationManualTransfer()
        {
            InitializeComponent();

            GetGrid(_mSaaCommunicationManualTransferLcs);
        }

        private void BtnManualTransfer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch ((BtnName)Enum.Parse(typeof(BtnName), button.Name))
            {
                case BtnName.BtniList:
                    GetGrid(_mSaaCommunicationManualTransferiLis);
                    break;
                case BtnName.BtnReceiv:
                    GetGrid(_mSaaCommunicationManualTransferLcs);
                    break;
                case BtnName.BtnASE:
                    GetGrid(_mSaaCommunicationManualTransferAse);
                    break;
            }
        }


        private void GetGrid(UserControl user)
        {
            GdContent.Children.Clear();
            control.Visibility = Visibility.Hidden;
            user.Visibility = Visibility.Visible;
            GdContent.Children.Add(user);
        }

        #region [===操作按鈕列舉===]
        private enum BtnName
        {
            /// <summary>
            /// 傳送至iList 
            /// </summary>
            BtniList,

            /// <summary>
            /// 傳送至LCS
            /// </summary>
            BtnReceiv,

            /// <summary>
            /// 傳送ASE指令
            /// </summary>
            BtnASE,
        }
        #endregion
    }
}
