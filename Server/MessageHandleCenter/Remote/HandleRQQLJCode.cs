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
    [AuthAttribute(isCheck=false)]
    public class HandleRQQLJCode : IHandleMessage<RQQLJCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RQQLJCode rQQLJCode)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            //如果其他人未启动程序则返回
            bool hasClient = false;
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(rQQLJCode);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d=>d!=session.SessionID).ToArray() ;
            foreach (var key in keys)
            {
                    var sess = session.AppServer.GetSessionByID(key);
                    sess.Send(bytes, 0, bytes.Length);
                    hasClient = true;
                    session.Send("发送听诊请求,等待对方响应...");
            }
            if (!hasClient)
                session.Send("目前没有客户端加入此次远程听诊...");
        }
    }
}
