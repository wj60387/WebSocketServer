using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class RegistCode : CodeBase
    {
        public RegistCode()
        {
            this.Name = "Regist";
        }
        public int GroupID { get; set; }
        public string SN { get; set; }
        public string Mac { get; set; }
        public string License { get; set; }
        public bool isLegal { get; set; }
    }
}
