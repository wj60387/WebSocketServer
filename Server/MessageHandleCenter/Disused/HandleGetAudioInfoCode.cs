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
    public class HandleGetAudioInfoCode : IHandleMessage<GetAudioInfoCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, GetAudioInfoCode getAudioInfoCode)
        {
            string sql = @"SELECT    
       [GUID]
,StetSerialNumber
       ,StetName
      ,[PatientID]
      ,[PatientName]
      ,[Posture]
      ,[Part]
      ,[RecordTime]
      ,[TakeTime]
      ,[Remark]
      ,[UpLoadTime]
  FROM AuscultationInfo where [StetName]='" + getAudioInfoCode.StetName + "'";
            var dt = SqlHelper.ExecuteDataset(Setting.connectionString, CommandType.Text, sql).Tables[0];
            ResGetAudioInfoCode resPondAudioInfoCode = new ResGetAudioInfoCode();
            resPondAudioInfoCode.StetName = getAudioInfoCode.StetName;
            resPondAudioInfoCode.DTable = dt;
            var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(resPondAudioInfoCode);
            session.Send(bytes, 0, bytes.Length);
        }
    }
}
