using SAA_CommunicationSystem_Lib.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_Config
    {
        private Configuration ConfigFile;

        public void ConfigReadStatr()
        {
            try
            {
                string ConfigFileRoute = Path.Combine(Directory.GetCurrentDirectory(), SAA_Database.Config, SAA_Database.SystemSetting);
                if (File.Exists(ConfigFileRoute))
                {
                    ExeConfigurationFileMap configMap = new ExeConfigurationFileMap
                    {
                        ExeConfigFilename = ConfigFileRoute
                    };
                    ConfigFile = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                    SAA_Database.configattributes = new ConfigAttributes
                    {
                        WebApiServerIP = ConfigFile.AppSettings.Settings[ConfigName.WebApiServerIP.ToString()].Value.ToString(),
                        SaaDataBaseIP = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBaseIP.ToString()].Value.ToString(),
                        SaaDataBase = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBase.ToString()].Value.ToString(),
                        SaaDataBaseName = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBaseName.ToString()].Value.ToString(),
                        SaaDataBasePassword = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBasePassword.ToString()].Value.ToString(),
                        SaaLogName = ConfigFile.AppSettings.Settings[ConfigName.SaaLogName.ToString()].Value.ToString(),
                        WebApiResultOK = ConfigFile.AppSettings.Settings[ConfigName.WebApiResultOK.ToString()].Value.ToString(),
                        WebApiResultFAIL = ConfigFile.AppSettings.Settings[ConfigName.WebApiResultFAIL.ToString()].Value.ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}-{ex.StackTrace}");
            }
        }

        private enum ConfigName
        {
            /// <summary>
            /// Web Api伺服器IP位置
            /// </summary>
            WebApiServerIP,

            /// <summary>
            /// 資料庫名稱
            /// </summary>
            SaaDataBase,

            /// <summary>
            /// 資料庫IP位置
            /// </summary>
            SaaDataBaseIP,

            /// <summary>
            /// 資料庫帳號
            /// </summary>
            SaaDataBaseName,

            /// <summary>
            /// 資料庫密碼
            /// </summary>
            SaaDataBasePassword,

            /// <summary>
            /// NLog Config檔案名稱
            /// </summary>
            SaaLogName,

            /// <summary>
            /// 回覆LCS接受結果完成
            /// </summary>
            WebApiResultOK,

            /// <summary>
            /// 回覆LCS接受結果失敗
            /// </summary>
            WebApiResultFAIL,
        }
    }
}
