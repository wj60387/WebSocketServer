using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server
{
    public class Setting
    {
        public static string connectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
            }
        }
        public static string rootPath
        {
            get
            {
                var root= Application.StartupPath+"\\AudioFile";
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);
                return root;
                //return System.Configuration.ConfigurationManager.AppSettings["AudioSaveRootPath"];
            }
        }
        public static string StartupPath
        {
            get
            {
                var root = Application.StartupPath ;
                if (!Directory.Exists(root))
                    Directory.CreateDirectory(root);
                return root;
                //return System.Configuration.ConfigurationManager.AppSettings["AudioSaveRootPath"];
            }
        }
    }
}
