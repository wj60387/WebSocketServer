using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class ResRemoteAuscultateCode:CodeBase
    {
        public ResRemoteAuscultateCode()
        {
            this.Name = "ResRemoteAuscultate";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string SrcPCName { get; set; }
        //public string SrcStetName { get; set; }
        public string DestMac { get; set; }
        public bool isAccept { get; set; }
        public string Comment { get; set; }
    }
}
