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
                        SaaEquipmentNo = ConfigFile.AppSettings.Settings[ConfigName.SaaEquipmentNo.ToString()].Value.ToString(),
                        SaaEquipmentName = ConfigFile.AppSettings.Settings[ConfigName.SaaEquipmentName.ToString()].Value.ToString(),
                        StorageWebApiServerIP =ConfigFile.AppSettings.Settings[ConfigName.StorageWebApiServerIP.ToString()].Value.ToString(),
                        iLISWebApiServerIP = ConfigFile.AppSettings.Settings[ConfigName.iLISWebApiServerIP.ToString()].Value.ToString(),
                        WebApiServerIP = ConfigFile.AppSettings.Settings[ConfigName.WebApiServerIP.ToString()].Value.ToString(),
                        SaaDataBaseIP = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBaseIP.ToString()].Value.ToString(),
                        SaaDataBase = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBase.ToString()].Value.ToString(),
                        SaaDataBaseName = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBaseName.ToString()].Value.ToString(),
                        SaaDataBasePassword = ConfigFile.AppSettings.Settings[ConfigName.SaaDataBasePassword.ToString()].Value.ToString(),
                        SaaLogName = ConfigFile.AppSettings.Settings[ConfigName.SaaLogName.ToString()].Value.ToString(),
                        SaaSystemsName = ConfigFile.AppSettings.Settings[ConfigName.SaaSystemsName.ToString()].Value.ToString(),
                        WebApiResultOK = ConfigFile.AppSettings.Settings[ConfigName.WebApiResultOK.ToString()].Value.ToString(),
                        WebApiResultFAIL = ConfigFile.AppSettings.Settings[ConfigName.WebApiResultFAIL.ToString()].Value.ToString(),
                        ParaKey = ConfigFile.AppSettings.Settings[ConfigName.ParaKey.ToString()].Value.ToString(),
                        SaaVST101StationName = ConfigFile.AppSettings.Settings[ConfigName.SaaVST101StationName.ToString()].Value.ToString(),
                        SaaDestinationIniName = ConfigFile.AppSettings.Settings[ConfigName.SaaDestinationIniName.ToString()].Value.ToString(),
                        SaaIniParaKey = ConfigFile.AppSettings.Settings[ConfigName.SaaIniParaKey.ToString()].Value.ToString(),
                        SaaIniParaKeyStation = ConfigFile.AppSettings.Settings[ConfigName.SaaIniParaKeyStation.ToString()].Value.ToString(),
                        PARTICLE = ConfigFile.AppSettings.Settings[ConfigName.PARTICLE.ToString()].Value.ToString(),
                        LiftWebApiServerIP =ConfigFile.AppSettings.Settings[ConfigName.LiftWebApiServerIP.ToString()].Value.ToString(),
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
            /// 設備編號
            /// </summary>
           SaaEquipmentNo,

            /// <summary>
            /// 設備機型
            /// </summary>
            SaaEquipmentName,

            /// <summary>
            /// LCS Web Api伺服器IP位置
            /// </summary>
            StorageWebApiServerIP,

            /// <summary>
            /// iLIS Web Api伺服器IP位置
            /// </summary>
            iLISWebApiServerIP,

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
            /// 系統名稱
            /// </summary>
            SaaSystemsName,

            /// <summary>
            /// 回覆LCS接受結果完成
            /// </summary>
            WebApiResultOK,

            /// <summary>
            /// 回覆LCS接受結果失敗
            /// </summary>
            WebApiResultFAIL,

            /// <summary>
            /// 傳送指令至LCS Web Api伺服器關鍵字
            /// </summary>
            ParaKey,

            SaaVST101StationName,


            SaaDestinationIniName,

            SaaIniParaKey,

            SaaIniParaKeyStation,

            PARTICLE,

            LiftWebApiServerIP
        }
    }
}
