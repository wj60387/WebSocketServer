using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class GetDownedAudioCode:CodeBase
    {
        public GetDownedAudioCode()
        {
            this.Name = "GetDownedAudioCode";
        }
        public string Downer { get; set; }
        public DataTable DTable { get; set; }
    }
}
