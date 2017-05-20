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
        /// <summary>
        /// 远程听诊用户（那个独立的程序）SessionID,RemoteID
        /// </summary>
        public static Dictionary<string, string> RemoteSession = new Dictionary<string, string>();

     }
}
