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
    public class HandleRemoteAudioCode : IHandleMessage<RemoteAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RemoteAudioCode remoteAudioCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(remoteAudioCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[remoteAudioCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(remoteAudioCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
