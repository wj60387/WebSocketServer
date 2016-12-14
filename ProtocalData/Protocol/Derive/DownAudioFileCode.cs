using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class DownAudioFileCode : CodeBase
    {
        public DownAudioFileCode()
        {
            this.Name = "DownAudioFile";
        }
        public string GUID { get; set; }
        public string StetName { get; set; }
        public DateTime RecordTime { get; set; }

        public string Downer { get; set; }
    }
}
