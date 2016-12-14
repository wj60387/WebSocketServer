using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
     [Serializable]
    public class RemoteStartAudioOutputCode:CodeBase
    {
        public RemoteStartAudioOutputCode()
        {
            this.Name = "RemoteStartAudioOutput";
        }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string SrcStetName { get; set; }
        public string DestMac { get; set; }
        public string DestStetName { get; set; }
    }
}
