using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    /// <summary>
    /// 开启音频
    /// </summary>
    [Serializable]
    public class RKQYPCode : CodeBase
    {
        public RKQYPCode()
        {
            this.Name = "RKQYP";
        }
    }
    /// <summary>
    /// 关闭音频
    /// </summary>
    [Serializable]
    public class RGBYPCode : CodeBase
    {
        public RGBYPCode()
        {
            this.Name = "RGBYP";
        }
    }
    /// <summary>
    /// 传输音频
    /// </summary>
    [Serializable]
    public class RCSYPCode : CodeBase
    {
        public RCSYPCode()
        {
            this.Name = "RCSYP";
        }
        public byte[] bytes { get; set; }
    }
}
