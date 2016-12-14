using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocalData.Protocol.Derive
{
    [Serializable]
    public class AudioInfoUpLoadCode : CodeBase
    {
        public AudioInfoUpLoadCode()
        {
            this.Name = "AudioInfoUpLoad";
        }
        public string Guid { get; set; }
        public string StetSerialNumber { get; set; }
        public string StetName { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string Posture { get; set; }
        public string Part { get; set; }
        public float TakeTime { get; set; }
        public string Remark { get; set; }
        public DateTime RecordTime { get; set; }
        public string DocName { get; set; }
        public string DocDiagnose { get; set; }
        /// <summary>
        /// Base64String的转化
        /// </summary>
        //public string AudioBytes { get; set; }
        //public string GetPath()
        //{
        //    return StetSerialNumber + "\\" + RecordTime.Year + "\\" + RecordTime.Month + "\\" + RecordTime.Day + "\\" + Guid + ".MP3";
        //}
    }
}
