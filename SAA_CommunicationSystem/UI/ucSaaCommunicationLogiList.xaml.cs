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
    /// ucSaaCommunicationLogiList.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationLogiList : UserControl
    {
        public ucSaaCommunicationLogiList()
        {
            InitializeComponent();

            OnLogMessage += SAA_Database_OnLogMessage;
        }

        private void SAA_Database_OnLogMessage(string message, LogType logtype, LogSystmes logsystmes)
        {
            if (logsystmes == LogSystmes.iLIs)
            {
                App.UpdateUi(() =>
                {
                    if (logtype == LogType.Error)
                    {
                        LogMessage.Children.Insert(0, new TextBlock
                        {
                            Text = message,
                            Foreground = Brushes.Red,
                            Style = (Style)Application.Current.TryFindResource("SelectRecord")
                        });
                    }
                    else if (logtype == LogType.Warnning)
                    {
                        LogMessage.Children.Insert(0, new TextBlock
                        {
                            Text = message,
                            Foreground = Brushes.Pink,
                            Style = (Style)Application.Current.TryFindResource("SelectRecord")
                        });
                    }
                    else
                    {
                        LogMessage.Children.Insert(0, new TextBlock
                        {
                            Text = message,
                            Style = (Style)Application.Current.TryFindResource("SelectRecord")
                        });
                    }

                    if (LogMessage.Children.Count > App.DisplayLogLineCount)
                    {
                        LogMessage.Children.RemoveAt(LogMessage.Children.Count - 1);
                    }
                });
            }
        }
    }
}
