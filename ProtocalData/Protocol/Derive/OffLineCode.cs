using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    /// <summary>
    /// 掉线消息
    /// </summary>
    [Serializable]
    public class OffLineCode : CodeBase
    {
        public OffLineCode()
        {
            this.Name = "OffLine";
        }
        public string OffLineMac { get; set; }
    }
}
