using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RequestGetDeviceInfoCode : CodeBase
    {
        public RequestGetDeviceInfoCode()
        {
            this.Name = "RequestGetDeviceInfo";
        }
        //public string StetName { get; set; }
        public string SessionID { get; set; }

    }
}
