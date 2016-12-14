using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RemoteAudioCode:CodeBase
    {
        public RemoteAudioCode()
        {
            this.Name = "RemoteAudio";
        }
        public byte[] AudioData { get; set; }
        public string Guid { get; set; }
        public string SrcMac { get; set; }
        public string SrctetName { get; set; }
        public string DestMac { get; set; }
        public string DestStetName { get; set; }
    }
}
