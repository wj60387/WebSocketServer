using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class ResAudioInfoUpLoadCode : CodeBase
    {
        public ResAudioInfoUpLoadCode()
        {
            this.Name = "ResAudioInfoUpLoad";
        }
        public string Guid { get; set; }
        public bool isUploaded { get; set; }
        public DateTime RecordTime { get; set; }
        public string StetName { get; set; }

    }
}
