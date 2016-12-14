using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class ResGetDeviceInfoCode : CodeBase
    {
        public ResGetDeviceInfoCode()
        {
            this.Name = "ResGetDeviceInfoCode";
        }
        public string SrcMac { get; set; }
        public string PCName { get; set; }
        public string[] StetNames { get; set; }
        
        public bool[] isConnected { get; set; }
        public string[] StetChineseNames { get; set; }
        public string[] StetOwners { get; set; }
        public string ToSessionID { get; set; }
    }
}
