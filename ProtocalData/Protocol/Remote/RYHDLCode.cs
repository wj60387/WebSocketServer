using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [Serializable]
    public class RYHDLCode : CodeBase
    {
        public RYHDLCode()
        {
            this.Name = "RYHDL";
        }
        /// <summary>
        /// 登录人
        /// </summary>
        public string DLR { get; set; }
    }
    /// <summary>
    /// 用户下线
    /// </summary>
    [Serializable]
    public class RYHXXCode : CodeBase
    {
        public RYHXXCode()
        {
            this.Name = "RYHXX";
        }
         /// <summary>
        /// 下线人
        /// </summary>
        public string XXR { get; set; }
    }
}
