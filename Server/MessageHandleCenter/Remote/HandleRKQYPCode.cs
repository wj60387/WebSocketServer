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
    
    [AuthAttribute(isCheck=false)]
    public class HandleRKQYPCode : IHandleMessage<RKQYPCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RKQYPCode rKQYPCode)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(rKQYPCode);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d=>d!=session.SessionID).ToArray() ;
            foreach (var key in keys)
            {
                    var sess = session.AppServer.GetSessionByID(key);
                    sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    public class HandleRGBYPCode : IHandleMessage<RGBYPCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RGBYPCode rGBYPCode)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(rGBYPCode);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    public class HandleRCSYPCode : IHandleMessage<RCSYPCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RCSYPCode rCSYPCode)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(rCSYPCode);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
}
