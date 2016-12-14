using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class GetShareMeAudioCode:CodeBase
    {
        public GetShareMeAudioCode()
        {
            this.Name = "GetShareMeAudio";
        }
        public string StetName { get; set; }
        public DataTable DTable { get; set; }
    }
}
