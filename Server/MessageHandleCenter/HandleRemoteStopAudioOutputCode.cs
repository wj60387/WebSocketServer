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
    public class HandleRemoteStopAudioOutputCode : IHandleMessage<RemoteStopAudioOutputCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RemoteStopAudioOutputCode remoteStopAudioOutputCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(remoteStopAudioOutputCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[remoteStopAudioOutputCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(remoteStopAudioOutputCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
