using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class ShareInfoCode:CodeBase
    {
        public ShareInfoCode()
        {
            this.Name = "ShareInfo";
        }
        public string Guid { get; set; }
        public string SrcName { get; set; }
        public string[] ToName { get; set; }
        public DateTime ShareTime { get; set; }
    }
}
