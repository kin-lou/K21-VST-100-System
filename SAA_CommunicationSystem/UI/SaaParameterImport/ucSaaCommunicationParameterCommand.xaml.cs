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
using static SAA_CommunicationSystem_Lib.SAA_DatabaseEnum;

namespace SAA_CommunicationSystem.UI.SaaParameterImport
{
    /// <summary>
    /// ucSaaCommunicationParameterCommand.xaml 的互動邏輯
    /// </summary>
    public partial class ucSaaCommunicationParameterCommand : UserControl
    {
        public ucSaaCommunicationParameterCommand()
        {
            InitializeComponent();
        }

        private void ReadReportCommand(DataTable Data)
        {
            try
            {
                List<SaaReportCommand> ReportCommandList = new List<SaaReportCommand>();
                foreach (DataRow dr in Data.Rows)
                {
                    SaaReportCommand ReportCommand = new SaaReportCommand
                    {
                        SETNO = dr[SC_REPORT_COMMAND.SETNO.ToString()].ToString(),
                        MODEL_NAME = dr[SC_REPORT_COMMAND.MODEL_NAME.ToString()].ToString(),
                        LCS_COMMAND_NAME = dr[SC_REPORT_COMMAND.LCS_COMMAND_NAME.ToString()].ToString(),
                        LCS_COMMAND_NOTE = dr[SC_REPORT_COMMAND.LCS_COMMAND_NOTE.ToString()].ToString(),
                        GROUP_NO = dr[SC_REPORT_COMMAND.GROUP_NO.ToString()].ToString(),
                        REPORT_COMMAND_NO = dr[SC_REPORT_COMMAND.REPORT_COMMAND_NO.ToString()].ToString(),
                        REPORT_COMMAND = dr[SC_REPORT_COMMAND.REPORT_COMMAND.ToString()].ToString(),
                        REPORT_COMMAND_NOTE = dr[SC_REPORT_COMMAND.REPORT_COMMAND_NOTE.ToString()].ToString(),
                    };
                    ReportCommandList.Add(ReportCommand);
                }
                App.UpdateUi(() => { DgParameterCommandList.ItemsSource = ReportCommandList; });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", App.Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
