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
    public class HandleRemoteEnterCode : IHandleMessage<RemoteEnterCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RemoteEnterCode remoteEnterCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(remoteEnterCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[remoteEnterCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(remoteEnterCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
