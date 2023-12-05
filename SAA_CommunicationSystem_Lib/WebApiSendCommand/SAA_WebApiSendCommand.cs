using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SAA_CommunicationSystem_Lib.WebApiSendCommand
{
    public class SAA_WebApiSendCommand
    {
        public string Post(string url, string paraKey, string jsonParas)
        {
            string hostURL = url;

            //建立一個HTTP請求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hostURL);
            //Post請求方式
            request.Method = "POST";
            //內容型別
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";

            //request.

            //設定引數，並進行URL編碼
            //雖然我們需要傳遞給伺服器端的實際引數是JsonParas(格式：[{\"UserID\":\"0206001\",\"UserName\":\"ceshi\"}])，
            //但是需要將該字串引數構造成鍵值對的形式（注："paramaters=[{\"UserID\":\"0206001\",\"UserName\":\"ceshi\"}]"），
            //其中鍵paramaters為WebService介面函式的引數名，值為經過序列化的Json資料字串
            //最後將字串引數進行Url編碼

            string paraUrlCoded = Uri.EscapeDataString(paraKey);
            paraUrlCoded += "=" + Uri.EscapeDataString(jsonParas);

            SAA_Database.LogMessage($"【Client->Server】【傳送】{jsonParas}");
            byte[] payload;
            //將Json字串轉化為位元組
            //payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            //MyMessage myMessage = new MyMessage();
            //myMessage.Message = paraUrlCoded;
            //payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(myMessage));

            payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            //設定請求的ContentLength 
            request.ContentLength = payload.Length;
            //傳送請求，獲得請求流

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//獲取用於寫入請求資料的Stream物件
            }
            catch (Exception)
            {
                writer = null;
                SAA_Database.LogMessage($"連線伺服器失敗", SAA_Database.LogType.Error);
            }
            //將請求引數寫入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//關閉請求流

            string strValue = "";//strValue為http響應所返回的字元流
            HttpWebResponse response;
            try
            {
                //獲得響應流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                SAA_Database.LogMessage($"{ex.Message}", SAA_Database.LogType.Error);
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();

            //伺服器端返回的是一個XML格式的字串，XML的Content才是我們所需要的Json資料
            try
            {
                //StreamReader sr = new StreamReader(s);
                //var str = sr.ReadToEnd();
                //strValue = str;
                XmlTextReader Reader = new XmlTextReader(s);
                Reader.MoveToContent();
                strValue = Reader.ReadInnerXml();//取出Content中的Json資料
                Reader.Close();
                s.Close();
                SAA_Database.LogMessage($"【Server->Client】【接收】{strValue}");
            }
            catch (Exception ex)
            {
                SAA_Database.LogMessage($"{ex.Message}", SAA_Database.LogType.Error);
            }

            return strValue;//返回Json資料
        }

        public string Post(string url, string jsonParas)
        {
            string hostURL = url;

            //建立一個HTTP請求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hostURL);
            //Post請求方式
            request.Method = "POST";
            //內容型別
            request.ContentType = "application/json";

            //設定引數，並進行URL編碼
            //雖然我們需要傳遞給伺服器端的實際引數是JsonParas(格式：[{\"UserID\":\"0206001\",\"UserName\":\"ceshi\"}])，
            //但是需要將該字串引數構造成鍵值對的形式（注："paramaters=[{\"UserID\":\"0206001\",\"UserName\":\"ceshi\"}]"），
            //其中鍵paramaters為WebService介面函式的引數名，值為經過序列化的Json資料字串
            //最後將字串引數進行Url編碼

            byte[] payload;
            //將Json字串轉化為位元組
            payload = Encoding.UTF8.GetBytes(jsonParas);
            //設定請求的ContentLength 
            request.ContentLength = payload.Length;
            //傳送請求，獲得請求流

            Stream writer;
            try
            {
                //不認證SSL
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                writer = request.GetRequestStream();//獲取用於寫入請求資料的Stream物件
            }
            catch (Exception)
            {
                writer = null;
                SAA_Database.LogMessage("連線伺服器失敗!", SAA_Database.LogType.Error);
            }
            if (writer != null)
            {
                //將請求引數寫入流
                writer.Write(payload, 0, payload.Length);
                writer.Close();//關閉請求流

                String strValue = "";//strValue為http響應所返回的字元流
                HttpWebResponse response;
                try
                {
                    //獲得響應流
                    response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine(response);
                }
                catch (WebException ex)
                {
                    response = ex.Response as HttpWebResponse;
                    return "";
                }

                Stream s = response.GetResponseStream();

                //伺服器端返回的是一個XML格式的字串，XML的Content才是我們所需要的Json資料
                try
                {
                    StreamReader sr = new StreamReader(s);
                    return sr.ReadToEnd();
                }
                catch { }

                return strValue;//返回Json資料
            }
            return null;
        }
    }
}
