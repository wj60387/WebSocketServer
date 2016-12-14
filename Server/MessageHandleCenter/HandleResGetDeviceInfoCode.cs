using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{
    public class HandleResGetDeviceInfoCode : IHandleMessage<ResGetDeviceInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, ResGetDeviceInfoCode resGetDeviceInfoCode)
        {
            resGetDeviceInfoCode.SrcMac=session.Cookies["MAC"];
           var sess= session.AppServer.GetSessionByID(resGetDeviceInfoCode.ToSessionID);
           var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(resGetDeviceInfoCode);
           sess.Send(bytes, 0, bytes.Length);
        }
    }
}
