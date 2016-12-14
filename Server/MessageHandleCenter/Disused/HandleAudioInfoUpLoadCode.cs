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
    public class HandleAudioInfoUpLoadCode : IHandleMessage<AudioInfoUpLoadCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, AudioInfoUpLoadCode audioInfoUpLoadCode)
        {
            string querySql = "select 1 from AuscultationInfo where GUID='" + audioInfoUpLoadCode.Guid + "'";
            var isExist = SqlHelper.ExecuteScalar(Setting.connectionString, CommandType.Text, querySql);
            if (isExist != null && isExist.ToString() == "1")//已经上传过
            {
                var resAudioInfoUpLoadCode = new ResAudioInfoUpLoadCode()
                {
                    Guid = audioInfoUpLoadCode.Guid
                    ,
                    isUploaded = true
                    ,
                    RecordTime = audioInfoUpLoadCode.RecordTime
                        ,
                    StetName = audioInfoUpLoadCode.StetName

                };
                var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(resAudioInfoUpLoadCode);
                session.Send(bytes, 0, bytes.Length);
                session.Send("文件已经上传了");
            }
            else
            {
                string insertSql = @"insert into AuscultationInfo (GUID,StetSerialNumber,StetName,PatientID,PatientName,Posture
  ,Part,RecordTime,TakeTime,Remark)
  select '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}','{9}'";
                int i = SqlHelper.ExecuteNonQuery(Setting.connectionString, CommandType.Text, string.Format(insertSql
                    , audioInfoUpLoadCode.Guid
                    , audioInfoUpLoadCode.StetSerialNumber
                             , audioInfoUpLoadCode.StetName
                             , audioInfoUpLoadCode.PatientID
                             , audioInfoUpLoadCode.PatientName
                             , audioInfoUpLoadCode.Posture
                             , audioInfoUpLoadCode.Part
                             , audioInfoUpLoadCode.RecordTime
                             , audioInfoUpLoadCode.TakeTime
                             , audioInfoUpLoadCode.Remark));
                if (i > 0)
                {
                    session.Send("音频文件信息写入成功!");
                    var resAudioInfoUpLoadCode = new ResAudioInfoUpLoadCode()
                    {
                        Guid = audioInfoUpLoadCode.Guid
                        ,
                        isUploaded = false
                        ,
                        RecordTime = audioInfoUpLoadCode.RecordTime
                        ,
                        StetName = audioInfoUpLoadCode.StetName

                    };
                    var bytes = ProtocalData.Utilities.SerializaHelper.Serialize(resAudioInfoUpLoadCode);
                    session.Send(bytes, 0, bytes.Length);
                }
                else
                {
                    session.Send("音频文件信息写入失败!");
                }
            }
           
        }
    }
}
