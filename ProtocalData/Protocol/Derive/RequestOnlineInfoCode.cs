using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RequestOnlineInfoCode : CodeBase
    {
        public RequestOnlineInfoCode()
        {
            this.Name = "RequestOnlineInfo";
        }
        public DataTable DT { get; set; }
    }
}
