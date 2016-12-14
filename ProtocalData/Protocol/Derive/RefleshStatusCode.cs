using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RefleshStatusCode:CodeBase
    {
        public RefleshStatusCode()
        {
            this.Name = "RefleshStatus";
        }
        public string SrcStetName { get; set; }
        public string SrcMac { get; set; }
        public string DestMac { get; set; }
        public bool isConnect { get; set; } 
    }
}
