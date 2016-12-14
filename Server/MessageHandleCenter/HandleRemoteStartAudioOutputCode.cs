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
    public class HandleRemoteStartAudioOutputCode : IHandleMessage<RemoteStartAudioOutputCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RemoteStartAudioOutputCode remoteStartAudioOutputCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(remoteStartAudioOutputCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[remoteStartAudioOutputCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(remoteStartAudioOutputCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
