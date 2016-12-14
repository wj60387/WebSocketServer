using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RequestRemoteAuscultateCode:CodeBase
    {
        public RequestRemoteAuscultateCode()
        {
            this.Name = "RequestRemoteAuscultate";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string SrcPCName { get; set; }
        public string SrcStetOwner { get; set; }
        public string SrcStetName { get; set; }
        public string DestMac { get; set; }
        public string []DestStetNames { get; set; }
        /// <summary>
        /// 邀请的列表
        /// </summary>
        public DataTable InvestList { get; set; }
        //public string DestOwner { get; set; }
        //public string DestStetName { get; set; }
       
 
    }
}
