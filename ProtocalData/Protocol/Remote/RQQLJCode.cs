using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ProtocalData.Protocol.Derive
{
    /// <summary>
    /// 请求连接
    /// </summary>
    [Serializable]
    public class RQQLJCode : CodeBase
    {
        public RQQLJCode()
        {
            this.Name = "RQQLJ";
        }
        /// <summary>
        /// 请求人
        /// </summary>
        public string QQR { get; set; }
    }
    /// <summary>
    /// 回应连接
    /// </summary>
    [Serializable]
    public class RHYLJCode : CodeBase
    {
        public RHYLJCode()
        {
            this.Name = "RHYLJ";
            this.isOK = false;
        }
        /// <summary>
        /// 请求人
        /// </summary>
        public string HYR { get; set; }
        public bool isOK { get; set; }
    }
}
