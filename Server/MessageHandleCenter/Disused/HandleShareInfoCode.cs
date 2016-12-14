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
    public class HandleShareInfoCode : IHandleMessage<ShareInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, ShareInfoCode shareInfoCode)
        {
            string sqlTxt = "insert into AudioShare(GUID,SrcStetName,ToStetName) select '{0}','{1}','{2}'";
            foreach (var toName in shareInfoCode.ToName)
            {
                string sql = string.Format(sqlTxt, shareInfoCode.Guid, shareInfoCode.SrcName, toName);
                int count=  SqlHelper.ExecuteNonQuery(Setting.connectionString, CommandType.Text, sql);
                
            }
            
        }
    }
}
