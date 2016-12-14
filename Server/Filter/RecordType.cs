using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Filter
{
    public enum RecordType
    {

        None = 1,

        /// <summary>  
        /// 记录日志  
        /// </summary>  
        Log = 2,

        /// <summary>  
        /// 记录异常  
        /// </summary>  
        Exception = 4,
        /// <summary>
        /// 检查权限
        /// </summary>
       Right=8

    }  
}
