using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class GetAudioInfoCode : CodeBase
    {
        public GetAudioInfoCode()
        {
            this.Name = "GetAudioInfo";
        }
        public string StetName { get; set; }
       // public int InfoType { get; set; }
    }
}
