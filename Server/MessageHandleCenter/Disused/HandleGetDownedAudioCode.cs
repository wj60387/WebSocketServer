using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Server.MessageHandleCenter
{
    /// <summary>
    /// 处理AudioInfoUpLoadCode消息
    /// </summary>
    public class HandleGetDownedAudioCode : IHandleMessage<GetDownedAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, GetDownedAudioCode getDownedAudioCode)
        {
            string sql = "select * from View_RecentDownFileHis where Downer='" + getDownedAudioCode.Downer + "'";
            var ds = SqlHelper.ExecuteDataset(Setting.connectionString, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                getDownedAudioCode.DTable = ds.Tables[0];
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(getDownedAudioCode);
                session.Send(bytes, 0, bytes.Length);
            }
        }
    }
}
