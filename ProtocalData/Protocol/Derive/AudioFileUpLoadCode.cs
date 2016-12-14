using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class AudioFileUpLoadCode : CodeBase
    {
        public AudioFileUpLoadCode()
        {
            this.Name = "AudioFileUpLoad";
        }
        public string Guid { get; set; }
        public string StetName { get; set; }
        public DateTime RecordTime { get; set; }
        public long Offset { get; set; }
        public long FileSize { get; set; }
        /// <summary>
        /// Base64String的转化
        /// </summary>
        public byte[] AudioBytes { get; set; }
        public string GetPath()
        {
            return StetName + "\\" + RecordTime.Year + "\\" + RecordTime.Month + "\\" + RecordTime.Day + "\\" + Guid + ".MP3";
        }
    }
}
