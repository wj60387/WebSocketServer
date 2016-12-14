using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{


    public class HandleStetInfoDelCode : IHandleMessage<StetInfoDelCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, StetInfoDelCode stetInfoDelCode)
        {
            string sql = "update StethoscopeManager set IfDel=1  where StetName='{0}'  and MAC='{1}' and IfDel=0";
            var count = SqlHelper.ExecuteNonQuery(Setting.connectionString, System.Data.CommandType.Text, string.Format(sql, stetInfoDelCode.StetName, stetInfoDelCode.MAC));
            if (count > 0)
            {
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(stetInfoDelCode);
                session.Send(bytes, 0, bytes.Length);
            }
            
        }
    }
}
