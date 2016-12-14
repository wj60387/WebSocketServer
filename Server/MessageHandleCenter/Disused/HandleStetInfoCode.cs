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
    public class HandleStetInfoCode : IHandleMessage<StetInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, StetInfoCode stetInfoCode)
        {
            stetInfoCode.RecordTime = DateTime.Now;
            var count = SqlHelper.ExecuteNonQuery(Setting.connectionString, "P_LoadStetInfo", stetInfoCode.StetName, stetInfoCode.SN, stetInfoCode.MAC, stetInfoCode.StetChineseName,stetInfoCode.Owner, stetInfoCode.FuncDescript, stetInfoCode.ReMark);
            if (count > 0)
            {
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(stetInfoCode);
                session.Send(bytes, 0, bytes.Length);
            }
            
        }
    }
}
