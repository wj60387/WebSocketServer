using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RemoteEnterCode:CodeBase
    {
        public RemoteEnterCode()
        {
            this.Name = "RemoteEnter";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string DestMac { get; set; }

    }
}
