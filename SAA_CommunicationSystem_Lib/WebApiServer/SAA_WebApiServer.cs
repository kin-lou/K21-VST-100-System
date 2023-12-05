using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http;

namespace SAA_CommunicationSystem_Lib.WebApiServer
{
    public class SAA_WebApiServer
    {
        private string serverip = string.Empty;
        private static readonly string _routeName = "Log";
        private HttpSelfHostServer selfhostserver;

        /// <summary>
        /// WebAPI服務啟動
        /// </summary>
        public void WebAPIServerSatrt()
        {
            serverip = SAA_Database.configattributes.WebApiServerIP;
            var selfhost = new HttpSelfHostConfiguration(serverip);
            selfhost.MapHttpAttributeRoutes();
            selfhost.Routes.MapHttpRoute(_routeName, "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            selfhostserver = new HttpSelfHostServer(selfhost);
            selfhostserver.OpenAsync();
        }
    }
}
