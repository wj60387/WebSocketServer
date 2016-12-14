using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
     [Serializable]
    public class RemoteStopAudioOutputCode:CodeBase
    {
        public RemoteStopAudioOutputCode()
        {
            this.Name = "RemoteStopAudioOutput";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string SrctetName { get; set; }
        public string DestMac { get; set; }
        public string DestStetName { get; set; }
    }
}
