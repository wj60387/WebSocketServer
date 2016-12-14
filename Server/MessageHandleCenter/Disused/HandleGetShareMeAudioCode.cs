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
    public class HandleGetShareMeAudioCode : IHandleMessage<GetShareMeAudioCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, GetShareMeAudioCode getShareMeAudioCode)
        {
             var ds = SqlHelper.ExecuteDataset(Setting.connectionString, "P_GetShareAuido", getShareMeAudioCode.StetName);
             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             {
                 getShareMeAudioCode.DTable = ds.Tables[0];
             }
             var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(getShareMeAudioCode);
             session.Send(bytes, 0, bytes.Length);
        }
    }
}
