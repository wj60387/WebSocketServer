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
    public class HandleRMessageCode : IHandleMessage<RMessageCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RMessageCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d=>d!=session.SessionID).ToArray() ;
            foreach (var key in keys)
            {
                    var sess = session.AppServer.GetSessionByID(key);
                    sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    [AuthAttribute(isCheck = false)]
    public class HandleRReadyCode : IHandleMessage<RReadyCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RReadyCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }

    [AuthAttribute(isCheck = false)]
    public class HandleRStartAudioCode : IHandleMessage<RStartAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RStartAudioCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    [AuthAttribute(isCheck = false)]
    public class HandleRTransAudioCode : IHandleMessage<RTransAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RTransAudioCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    [AuthAttribute(isCheck = false)]
    public class HandleRStopAudioCode : IHandleMessage<RStopAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RStopAudioCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
            //线程安全考虑 不遍历字典 而是遍历字典的key
            var keys = Common.RemoteSession.Keys.Where(d => d != session.SessionID).ToArray();
            foreach (var key in keys)
            {
                var sess = session.AppServer.GetSessionByID(key);
                sess.Send(bytes, 0, bytes.Length);
            }
        }
    }
    [AuthAttribute(isCheck = false)]
    public class HandleRExitCode : IHandleMessage<RExitCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RExitCode code)
        {
            //远程的id
            var remoteID = Common.RemoteSession[session.SessionID];
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(code);
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
