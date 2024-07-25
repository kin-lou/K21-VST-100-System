using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib
{
    public class SAA_IniFiles
    {
        private int chars = 256;
        private string sDefault = string.Empty;
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        public extern static int GetPrivateProfileString(string segName, string keyName, string sDefault, byte[] buffer, int iLen, string fileName); // ANSI版本

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", SetLastError = true)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr retVal, uint size, string filePath);

        private string _filename;

        public SAA_IniFiles(string filename)
        {
            if (File.Exists(filename))
                _filename = filename;
            else
                throw new Exception("specify ini file name error!");
        }

        /// <summary>
        /// 寫入數字Ini檔方法
        /// </summary>
        /// <param name="section">節</param>
        /// <param name="key">鍵</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool WriteInteger(string section, string key, int value)
        {
            return WriteIniValue(section, key, value.ToString());
        }

        /// <summary>
        /// 讀取Ini檔方法
        /// </summary>
        /// <param name="section">節</param>
        /// <param name="key">鍵</param>
        /// <returns></returns>
        public string ReadString(string section, string key)
        {
            return GetIniValue(section, key);
        }

        /// <summary>
        /// 寫入文字Ini檔方法
        /// </summary>
        /// <param name="section">節</param>
        /// <param name="key">鍵</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool WriteString(string section, string key, string value)
        {
            return WriteIniValue(section, key, value);
        }

        /// <summary>
        /// 讀取Ini檔的值的方法
        /// </summary>
        /// <param name="section">節</param>
        /// <param name="key">鍵</param>
        /// <returns></returns>
        private string GetIniValue(string section, string key)
        {
            StringBuilder buffer = new StringBuilder(chars);
            return GetPrivateProfileString(section, key, sDefault, buffer, chars, _filename) != 0 ? buffer.ToString() : null;
        }

        /// <summary>
        /// 寫入Ini檔
        /// </summary>
        /// <param name="section">節</param>
        /// <param name="key">鍵</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private bool WriteIniValue(string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, _filename);
        }

        public void WriteValueFromIniFile(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _filename);
        }

        /// <summary>
        /// 讀取節裡面的key值
        /// </summary>
        /// <param name="sectionName">節</param>
        /// <returns></returns>
        public ArrayList ReadKeys(string sectionName)
        {

            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileString(sectionName, null, "", buffer, buffer.GetUpperBound(0), _filename);

            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = Encoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }

        /// <summary>
        /// 讀取所有節點
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public string[] GetSectionNames(string path)
        {
            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            return IntPtrToStringArray(pReturnedString, bytesReturned);
        }

        //指標資料轉字串陣列
        private string[] IntPtrToStringArray(IntPtr pReturnedString, uint bytesReturned)
        {
            //use of Substring below removes terminating null for split
            if (bytesReturned == 0)
            {
                Marshal.FreeCoTaskMem(pReturnedString);
                return null;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            return local.Substring(0, local.Length - 1).Split('\0');
        }
    }
}
