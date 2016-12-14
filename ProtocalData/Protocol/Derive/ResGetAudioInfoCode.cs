using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class ResGetAudioInfoCode : CodeBase
    {
        public ResGetAudioInfoCode()
        {
            this.Name = "ResGetAudioInfo";
        }
       // public int InfoType { get; set; }
        public string StetName { get; set; }
        public DataTable DTable { get; set; }
         
    }
}
