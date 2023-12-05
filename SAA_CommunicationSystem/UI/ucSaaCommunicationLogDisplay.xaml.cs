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
    /// ucSaaCommunicationLogDisplay.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationLogDisplay : UserControl
    {
        private UserControl control = new UserControl();
        private readonly ucSaaCommunicationLogiList _mSaaCommunicationLogiList = new ucSaaCommunicationLogiList();
        private readonly ucSaaCommunicationLogReceiv _mSaaCommunicationLogReceiv = new ucSaaCommunicationLogReceiv();
        private readonly ucSaaCommunicationLogMessageComplete _mSaaCommunicationLogMessageComplete = new ucSaaCommunicationLogMessageComplete();
        
        public ucSaaCommunicationLogDisplay()
        {
            InitializeComponent();

            GetGrid(_mSaaCommunicationLogMessageComplete);
        }

        private void BtnLogAll_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch ((BtnName)Enum.Parse(typeof(BtnName), button.Name))
            {
                case BtnName.BtnLogAll:
                    GetGrid(_mSaaCommunicationLogMessageComplete);
                    break;
                case BtnName.BtniList:
                    GetGrid(_mSaaCommunicationLogiList);
                    break;
                case BtnName.BtnReceiv:
                    GetGrid(_mSaaCommunicationLogReceiv);
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
            /// 顯示全部LOG
            /// </summary>
            BtnLogAll,

            /// <summary>
            /// 顯示iList Log
            /// </summary>
            BtniList,

            /// <summary>
            /// 顯示接收LCS Log
            /// </summary>
            BtnReceiv,
        }
        #endregion
    }
}
