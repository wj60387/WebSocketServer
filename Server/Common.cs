using Server.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Common
    {
        public static LoggerHelper loggerHelper = new LoggerHelper();


        public static Dictionary<string, string> OnlineUser = new Dictionary<string, string>();

     }
}
