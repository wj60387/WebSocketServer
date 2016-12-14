using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtocalData.Protocol;
using SuperWebSocket;
using Server.Filter;

namespace Server.MessageHandleCenter
{
    /// <summary>
    /// 泛型消息处理接口
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// 
    [AuthAttribute(isCheck=true)]
    public  interface IHandleMessage<M>  
        where M : CodeBase
    {
       void HandleMessage(WebSocketSession session,M message);
    }
     
}
