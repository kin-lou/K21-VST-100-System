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
    /// ucSaaCommunicationLogMessageComplete.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationLogMessageComplete : UserControl
    {
        private readonly ucSaaCommunicationLogiList _mSaaCommunicationLogiList= new ucSaaCommunicationLogiList();
        private readonly ucSaaCommunicationLogReceiv _mSaaCommunicationLogReceiv = new ucSaaCommunicationLogReceiv();
        public ucSaaCommunicationLogMessageComplete()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.UpdateUi(() =>
            {
                GdiList.Children.Clear();
                GdReceiv.Children.Clear();
                GdiList.Children.Add(_mSaaCommunicationLogiList);
                GdReceiv.Children.Add(_mSaaCommunicationLogReceiv);
            });
        }
    }
}
