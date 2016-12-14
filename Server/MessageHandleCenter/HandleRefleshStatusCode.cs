using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{

     //,[StetName]
     // ,[SN]
     // ,[MAC]
     // ,[UserName]
     // ,[FuncDescript]
     // ,[ReMark]
     // ,[RecordTime]
    public class  HandleRefleshStatusCode : IHandleMessage<RefleshStatusCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RefleshStatusCode refleshStatusCode)
        {
            if (MessageHandleFactory.Dict_Session_Token.ContainsKey(refleshStatusCode.DestMac))
            {
                var sess = session.AppServer.GetSessionByID(MessageHandleFactory.Dict_Session_Token[refleshStatusCode.DestMac]);
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(refleshStatusCode);
                sess.Send(bytes, 0, bytes.Length);
                return;
            }
        }
    }
}
