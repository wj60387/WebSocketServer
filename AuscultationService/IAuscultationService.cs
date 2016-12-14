using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;

namespace AuscultationService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IAuscultationService
    {
        [PrincipalPermission(SecurityAction.Demand, Name ="Foo")]
        [OperationContract]
        string AccountCredentials(string MAC, string SN);
        [OperationContract]
        bool UpdateStetInfo(string stetInfo);
        [OperationContract]
        string GetFilePath(string StetName, DateTime RecordTime, string Guid);
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        DataSet ExecuteDataset(string sql,string[] Args);

        [OperationContract]
        int ExecuteNonQuery(string sqlText,   string[] Args);

        [OperationContract]
        string ExecuteScalar(string sqlText,   string[] Args);
        // TODO: 在此添加您的服务操作

        #region 文件操作
        [OperationContract]
        void DeleteFile(string FileName);
        [OperationContract]
        bool isExistFolder(string FolderPath);
        [OperationContract]
        void CreateFolder(string FolderPath);

        [OperationContract]
        string[] GetFolderFiles(string FolderPath, string fileFilter = "", bool isCurLevel = true);
        [OperationContract]
        long GetFileLength(string FilePath);
        [OperationContract]
        bool isExistFile(string FilePath);
        [OperationContract]
        void UpLoadFile(string FileName, long offset, byte[] bytes);
        [OperationContract]
        void DownLoadFile(out byte[] bytes, string FileName, long offset, int len);
        [OperationContract]
        Dictionary<string, long> GetFilesInfo(out long totalLength, string FolderName);
        #endregion
    }

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
