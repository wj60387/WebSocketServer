using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class StetInfoCode:CodeBase
    {
        public StetInfoCode()
        {
            this.Name = "StetInfo";
        }
        public string StetName { get; set; }
        public string SN { get; set; }
        public string MAC { get; set; }
        public string PCName { get; set; }
        public string StetChineseName { get; set; }
        public string Owner { get; set; }
        public string FuncDescript { get; set; }
        public string ReMark { get; set; }
        public DateTime RecordTime { get; set; }
        public int StetType { get; set; } 
    }
}
