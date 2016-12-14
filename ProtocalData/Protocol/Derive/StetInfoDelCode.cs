using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class StetInfoDelCode:CodeBase
    {
        public StetInfoDelCode()
        {
            this.Name = "StetInfoDel";
        }
        public string StetName { get; set; }
        public string MAC { get; set; }
    }
}
