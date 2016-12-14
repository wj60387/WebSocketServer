using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RemoteExitCode:CodeBase
    {
        public RemoteExitCode()
        {
            this.Name = "RemoteExit";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string DestMac { get; set; }
        public string ExitMac { get; set; }

    }
}
