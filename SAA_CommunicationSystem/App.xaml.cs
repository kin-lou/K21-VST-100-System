using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SAA_CommunicationSystem
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Log行數
        /// </summary>
        public static readonly int DisplayLogLineCount = 200;

        /// <summary>
        /// 錯誤
        /// </summary>
        public static string Error = "錯誤";

        #region [===版本資訊===]
        /// <summary>
        /// 版本資訊
        /// </summary>
        /// <returns></returns>
        public static Version GetEdition()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
        #endregion

        #region [===更新UI畫面===]
        /// <summary>
        /// 更新UI畫面
        /// </summary>
        /// <param name="callback"></param>
        public static void UpdateUi(Action callback)
        {
            if (Current != null)
            {
                Current.Dispatcher.Invoke(DispatcherPriority.Send, callback);
            }
        }
        #endregion

        #region [===防止程式開啟第二次===]
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            if (ps != null && ps.Length > 1)
            {
                MessageBox.Show("【SAA_通訊系統】執行中！", "提示訊息", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(1);
            }
        }
        #endregion

        #region [===主頁面按鈕名稱===]
        public enum BtnName
        {
            /// <summary>
            /// 選單
            /// </summary>
            BtnMenu,

            /// <summary>
            /// LOG 資訊
            /// </summary>
            BtnLog,

            /// <summary>
            /// 手動傳送
            /// </summary>
            BtnSend,

            /// <summary>
            /// 資料匯入
            /// </summary>
            BtnImport,

            /// <summary>
            /// 指令歷史紀錄
            /// </summary>
            BtnHistor,

            /// <summary>
            /// 機台名稱修改
            /// </summary>
            BtnStorageInfo,

            /// <summary>
            /// REJECR明細
            /// </summary>
            BtnRejectList,

            /// <summary>
            /// REJECT歷史紀錄
            /// </summary>
            BtnRejectHistory,

            /// <summary>
            /// 系統操作紀錄
            /// </summary>
            BtnOperationRecord,

            /// <summary>
            /// 程式使用者權限編輯
            /// </summary>
            BtnUserPermissions,

            /// <summary>
            /// 參數設定
            /// </summary>
            BtnSetUp,
        } 
        #endregion
    }
}
