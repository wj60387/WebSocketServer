using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Server.Security
{
    public class SecurityHelper
    {
       
        public static string GetMD5(string myString)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Unicode.GetBytes(myString);
            byte[] buffer2 = md.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder(0x2a);
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("X2"));
            }
            return builder.ToString();
        }
        public static RegistCode GetRegistCode(string SN,string MAC)
        {
            var registCode = new ProtocalData.Protocol.Derive.RegistCode()
            {
                SN = SN,
                Mac = MAC,
            };
            var ds = SqlHelper.ExecuteDataset(Setting.connectionString, "P_CreateAuth", registCode.SN, registCode.Mac);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var mac = MAC;
                var sn = SN;
                var st = (DateTime)ds.Tables[0].Rows[0]["AuthStartTime"];
                var lut = (DateTime)ds.Tables[0].Rows[0]["LastUseTime"];
                var days = float.Parse(ds.Tables[0].Rows[0]["AuthDays"].ToString());
                var content = mac + sn + st.ToString("yyyyMMddhhmmss") + lut.ToString("yyyyMMddhhmmss") + days + "wanjian";
                var md5 = SecurityHelper.GetMD5(content);
                registCode.isLegal = (st.AddDays(days) > DateTime.Now && st < DateTime.Now);
                var obj = new
                {
                    AuthorizationNum = sn,
                    MachineCode = mac,
                    AuthStartTime = st,
                    LastUseTime = lut,
                    AuthDays = days,
                    HashCode = md5
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                registCode.License = json;
            }
            return registCode;
        }
    }
    
}
