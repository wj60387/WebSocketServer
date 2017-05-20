using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{
    public class HandleRequestOnlineInfoCode : IHandleMessage<RequestOnlineInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, RequestOnlineInfoCode requestOnlineInfoCode)
        {
            requestOnlineInfoCode.DT = new System.Data.DataTable();
            requestOnlineInfoCode.DT.Columns.Add("EndPoint");
            requestOnlineInfoCode.DT.Columns.Add("SN");
            requestOnlineInfoCode.DT.Columns.Add("Mac");
            foreach (var sess in session.AppServer.GetAllSessions())
            {
                if (!Common.RemoteSession.ContainsKey(sess.SessionID))
                requestOnlineInfoCode.DT.Rows.Add(sess.RemoteEndPoint.ToString(), sess.Cookies["SN"], sess.Cookies["MAC"]);
            }
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(requestOnlineInfoCode);
            session.Send(bytes, 0, bytes.Length);
        }
    }
}
