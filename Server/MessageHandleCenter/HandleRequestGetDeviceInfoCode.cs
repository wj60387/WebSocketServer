using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{
    public class HandleRequestGetDeviceInfoCode : IHandleMessage<RequestGetDeviceInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RequestGetDeviceInfoCode requsetGetDeviceInfoCode)
        {
            requsetGetDeviceInfoCode.SessionID = session.SessionID;
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(requsetGetDeviceInfoCode);
            // session.Send(bytes,0,bytes.Length);
            var sessions = session.AppServer.GetAllSessions();
            foreach (var item in sessions)
            {
                if (!Common.RemoteSession.ContainsKey(item.SessionID))
                //if (item != session)
                item.Send(bytes, 0, bytes.Length);
            }
        }
    }
}
