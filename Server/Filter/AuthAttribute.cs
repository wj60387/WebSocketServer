using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class AuthAttribute:Attribute
    {
        public bool isCheck { get; set; }
        public AuthAttribute()
        {

        }
         
    }
}
