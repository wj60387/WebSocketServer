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
    public class HandleRequestRemoteAuscultateCode : IHandleMessage<RequestRemoteAuscultateCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RequestRemoteAuscultateCode requestRemoteAuscultateCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(requestRemoteAuscultateCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[requestRemoteAuscultateCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(requestRemoteAuscultateCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }

        }
    }
}
