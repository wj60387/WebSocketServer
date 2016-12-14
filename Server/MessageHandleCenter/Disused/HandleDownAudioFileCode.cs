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
    /// 处理DownAudioFileCode消息
    /// </summary>
    [AuthAttribute(isCheck=false)]
    public class HandleDownAudioFileCode : IHandleMessage<DownAudioFileCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, DownAudioFileCode downAudioFileCode)
        {
            var filePath = Setting.rootPath + "\\" + downAudioFileCode.StetName + "\\" + downAudioFileCode.RecordTime.Year
                + "\\" + downAudioFileCode.RecordTime.Month + "\\" + downAudioFileCode.RecordTime.Day + "\\" + downAudioFileCode.GUID + ".MP3";
            if (!File.Exists(filePath))
            {
                return;
            }
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                while (true)
                {
                    AudioFileUpLoadCode audioFileUpLoadCode = new ProtocalData.Protocol.Derive.AudioFileUpLoadCode()
                    {
                        Guid = downAudioFileCode.GUID,
                        StetName = downAudioFileCode.StetName,
                        RecordTime = downAudioFileCode.RecordTime,
                        FileSize = stream.Length,
                    };
                    audioFileUpLoadCode.Offset = stream.Position;
                    var readBytes = new byte[1900];
                    var readed = stream.Read(readBytes, 0, readBytes.Length);
                    if (readed <= 0) break;

                    audioFileUpLoadCode.AudioBytes = readBytes.Take(readed).ToArray();
                    var _bytes = ProtocalData.Utilities.SerializaHelper.Serialize(audioFileUpLoadCode);
                    session.Send(_bytes,0,_bytes.Length);
                }
                stream.Close();
            }
            string sqlTxt = "insert into AudioDownLoadInfo(GUID,Downer) select '{0}','{1}'";
            var sql=string.Format(sqlTxt,downAudioFileCode.GUID,downAudioFileCode.Downer);
            int count=SqlHelper.ExecuteNonQuery(Setting.connectionString, CommandType.Text, sql);
            
        }
    }
}
