using Microsoft.ApplicationBlocks.Data;
using ProtocalData.Protocol.Derive;
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
    public class HandleAudioFileUpLoadCode : IHandleMessage<AudioFileUpLoadCode>
    {
        public void HandleMessage(SuperWebSocket.WebSocketSession session, AudioFileUpLoadCode audioFileUpLoadCode)
        {
            var rPath = audioFileUpLoadCode.GetPath();
            var filePath = Path.Combine(Setting.rootPath, rPath);
            byte[] data = audioFileUpLoadCode.AudioBytes;
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (audioFileUpLoadCode.Offset == 0) session.Send("文件" + audioFileUpLoadCode.Guid+".MP3"+ "开始上传!");
            if (audioFileUpLoadCode.Offset + audioFileUpLoadCode.AudioBytes.Length == audioFileUpLoadCode.FileSize)
                session.Send("文件" + audioFileUpLoadCode.Guid + ".MP3" + "上传成功!");
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite))
            {
                stream.Seek(audioFileUpLoadCode.Offset, SeekOrigin.Begin);
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
        }
    }
}
