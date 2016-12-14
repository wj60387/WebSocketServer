using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using Server.Filter;
using Server.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{
    /// <summary>
    /// 处理AudioInfoUpLoadCode消息
    /// </summary>
    [AuthAttribute(isCheck=false)]
    public class HandleRegistCode : IHandleMessage<RegistCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RegistCode registCode)
        {
            var code = SecurityHelper.GetRegistCode(registCode.SN, registCode.Mac);
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            session.Send(bytes, 0, bytes.Length);
            
        }
    }
}
