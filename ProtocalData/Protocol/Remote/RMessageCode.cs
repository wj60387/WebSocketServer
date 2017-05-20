using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    /// <summary>
    /// 普通的消息
    /// </summary>
    [Serializable]
    public class RMessageCode : CodeBase
    {
        public RMessageCode()
        {
            this.Name = "RMessage";
        }
        public string Msg { get; set; }
    }
    [Serializable]
    public class RReadyCode : CodeBase
    {
        public RReadyCode()
        {
            this.Name = "RReady";
        }
    }
    [Serializable]
    public class RStartAudioCode : CodeBase
    {
        public RStartAudioCode()
        {
            this.Name = "RStartAudio";
        }
    }
    [Serializable]
    public class RTransAudioCode : CodeBase
    {
        public RTransAudioCode()
        {
            this.Name = "RTransAudio";
        }
        public  byte[] Bytes { get; set; }
    }
    [Serializable]
    public class RStopAudioCode : CodeBase
    {
        public RStopAudioCode()
        {
            this.Name = "RStopAudio";
        }
    }

    [Serializable]
    public class RExitCode : CodeBase
    {
        public RExitCode()
        {
            this.Name = "RExit";
        }
    }
}
