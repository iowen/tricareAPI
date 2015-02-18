using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI
{
    public static class AppGlobalContext
    {
        public static string ApiUrl
        {

            get
            {
                string url = "";
                #if DEBUG
                    url = "http://teamsavagemma.com/";
                #else
                    url = "http://fertilscripts.com/";
                #endif
                    return url;
            }
        }
        public static string AppPassword
        {

            get
            {
                string pass = "";
#if DEBUG
                pass = "Tcare1234";
#else
                    pass = "100M@rch!ng";
#endif
                return pass;
            }
        }
        public static string AppUsername
        {

            get
            {
                string username = "";
#if DEBUG
                username = "TcareApp";
#else
                    username = "RXTcareApp";
#endif
                return username;
            }
        }
    }
}
