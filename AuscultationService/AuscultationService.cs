using Helper;
using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace AuscultationService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class AuscultationService : IAuscultationService
    {
        private static Helper.SqlCommon sqlHelper = new Helper.SqlCommon(ConfigurationManager.ConnectionStrings["sql"].ConnectionString);
        public static string connectionString = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
        public static string AudioSaveRootPath = ConfigurationManager.AppSettings["AudioSaveRootPath"];


        private   string GetMD5(string myString)
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
        public string AccountCredentials(string MAC, string SN)
        {
            var registCode = new ProtocalData.Protocol.Derive.RegistCode()
            {
                SN = SN,
                Mac = MAC,
            };
            var ds = SqlHelper.ExecuteDataset(connectionString, "P_CreateAuth", SN, MAC);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var mac = MAC;
                var sn = SN;
                var st = (DateTime)ds.Tables[0].Rows[0]["AuthStartTime"];
                //允许客户端时间误差范围为10分钟吧
                var lut = ((DateTime)ds.Tables[0].Rows[0]["LastUseTime"]).AddMinutes(-10);
                var days = float.Parse(ds.Tables[0].Rows[0]["AuthDays"].ToString());
                var content = mac + sn + st.ToString("yyyyMMddhhmmss") + lut.ToString("yyyyMMddhhmmss") + days + "wanjian";
                var md5 = GetMD5(content);
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
            return  Newtonsoft.Json.JsonConvert.SerializeObject(registCode);
        }

        public bool UpdateStetInfo(string stetInfo)
        {

            var dict = OperationContext.Current.IncomingMessageHeaders;

            var stetInfoCode=Newtonsoft.Json.JsonConvert.DeserializeObject<StetInfoCode>(stetInfo);
             stetInfoCode.RecordTime = DateTime.Now;
            var count = SqlHelper.ExecuteNonQuery(connectionString, "P_LoadStetInfo", stetInfoCode.StetName, stetInfoCode.SN, stetInfoCode.MAC, stetInfoCode.StetChineseName,stetInfoCode.Owner, stetInfoCode.FuncDescript, stetInfoCode.ReMark);
            if (count > 0)
                return true;
            return false;
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        public DataSet ExecuteDataset(string sqlText, string[] Args)
        {
            return sqlHelper.ExecuteDataset(sqlText, Args);
        }
        public int ExecuteNonQuery(string sqlText,   string[] Args)
        {
            return sqlHelper.ExecuteNonQuery(sqlText, Args);
        }
        public string ExecuteScalar(string sqlText,   string[] Args)
        {
            return sqlHelper.ExecuteScalar(sqlText, Args);
        }


        #region 文件操作
        public void DeleteFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }
        public bool isExistFolder(string FolderPath)
        {
            return Directory.Exists(FolderPath);
        }
        public void CreateFolder(string FolderPath)
        {
            if (!isExistFolder(FolderPath))
                Directory.CreateDirectory(FolderPath);
        }
        public string[] GetFolderFiles(string FolderPath, string fileFilter = "", bool isCurLevel = true)
        {
            if (Directory.Exists(FolderPath))
            {
                return Directory.GetFiles(FolderPath, fileFilter, isCurLevel ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            }
            return null;

        }
        public long GetFileLength(string FilePath)
        {
            if (isExistFile(FilePath))
            {
                FileInfo info = new FileInfo(FilePath);
                return info.Length;
            }
            return -1;
        }
        public bool isExistFile(string FilePath)
        {
            return File.Exists(FilePath);
        }
        public string GetFilePath(string StetName,DateTime RecordTime,string Guid)
        {
            return Path.Combine(AudioSaveRootPath, StetName + "\\" + RecordTime.Year + "\\" + RecordTime.Month + "\\" + RecordTime.Day + "\\" + Guid + ".MP3");
        }
        public void UpLoadFile(string FileName, long offset, byte[] bytes)
        {
            var directory = Path.GetDirectoryName(FileName);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

        }
        public void DownLoadFile(out byte[] bytes, string FileName, long offset, int len)
        {
            bytes = new byte[len];
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Seek(offset, SeekOrigin.Begin);
                int read = fs.Read(bytes, 0, len);
                bytes = bytes.Take(read).ToArray();
                fs.Close();
            }

        }
        public Dictionary<string, long> GetFilesInfo(out long totalLength, string FolderName)
        {
            long tolalLen = 0;
            Dictionary<string, long> dict_FileInfo = new Dictionary<string, long>();
            DirectoryInfo dictoryInfo = new DirectoryInfo(FolderName);
            FileInfo[] fiArr = dictoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var file in fiArr)
            {
                dict_FileInfo.Add(file.FullName, file.Length);
                tolalLen += file.Length;
            }
            totalLength = tolalLen;
            return dict_FileInfo;
        }
        #endregion
    }
}
