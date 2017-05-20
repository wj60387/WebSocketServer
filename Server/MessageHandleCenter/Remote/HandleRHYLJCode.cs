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
    public class HandleRHYLJCode : IHandleMessage<RHYLJCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RHYLJCode rHYLJCode)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            //如果其他人未启动程序则返回
            bool hasClient = false;
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(rHYLJCode);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d=>d!=session.SessionID).ToArray() ;
            foreach (var key in keys)
            {
                    var sess = session.AppServer.GetSessionByID(key);
                    sess.Send(bytes, 0, bytes.Length);
                    hasClient = true;
                    session.Send("你的小伙伴即将开始远程,骚等...");
            }
            if (!hasClient)
                session.Send("你的小伙伴好像跑路了...");
        }
    }
}
