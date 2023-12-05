using SAA_MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAA_CommunicationSystem_Lib.SqlData
{
    public class MsSqlData
    {
        private MsSql SaaSql = new MsSql(SAA_Database.configattributes.SaaDataBase, SAA_Database.configattributes.SaaDataBaseIP, SAA_Database.configattributes.SaaDataBaseName, SAA_Database.configattributes.SaaDataBasePassword);
    }
}
