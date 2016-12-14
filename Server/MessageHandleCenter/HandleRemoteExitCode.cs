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
    public class HandleRemoteExitCode : IHandleMessage<RemoteExitCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RemoteExitCode remoteExitCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(remoteExitCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[remoteExitCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(remoteExitCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
