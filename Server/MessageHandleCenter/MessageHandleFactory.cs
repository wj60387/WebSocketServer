using ProtocalData.Protocol;
using Server.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Server.MessageHandleCenter
{
    public class MessageHandleFactory  
    {
        /// <summary>
        /// StetName,Mac
        /// </summary>
        public static Dictionary<string, string> Dict_Session_Token = new Dictionary<string, string>();
        public static void HandleMessage(SuperWebSocket.WebSocketSession session, CodeBase message)
        {

            var codetype = message.GetType().Name;
            string className = string.Format("Server.MessageHandleCenter.Handle{0}", codetype);
            if (!Assembly.GetExecutingAssembly().GetTypes().Contains(Type.GetType(className, false)))
            {
                //throw new Exception("没有对应的处理模块:" + className);
                return;
            }
            var obj = Activator.CreateInstance(Type.GetType(className, false));
            //var attributes = obj.GetType().GetCustomAttributes(true);
            //var authAttribute= attributes.Where(attribute=>attribute.GetType()==typeof(AuthAttribute));
            //if(authAttribute.Count()>0 && !((AuthAttribute)authAttribute.First()).isCheck)
            //{
            //    session.Close();
            //}
            //Wrire log 用户访问服务器日志
           // session.RemoteEndPoint.ToString();
            
            var methodInfo = obj.GetType().GetMethod("HandleMessage");
            try
            {
                methodInfo.Invoke(obj, new object[] { session, message });
                Common.loggerHelper.SaveMessage(new Logger.LogMessage() {
                    SN = session.Cookies!=null&&session.Cookies.ContainsKey("SN") ? session.Cookies["SN"] : string.Empty,
                    MAC = session.Cookies != null && session.Cookies.ContainsKey("MAC") ? session.Cookies["MAC"] : string.Empty,
                    EndPoint=session.RemoteEndPoint.ToString(),
                    CodeName = message.Name,
                    ExceptionString=""
                }, 1);

            }
            catch(Exception ex)
            {
                Common.loggerHelper.SaveMessage(new Logger.LogMessage()
                {
                    SN = session.Cookies != null && session.Cookies.ContainsKey("SN") ? session.Cookies["SN"] : string.Empty,
                    MAC = session.Cookies != null && session.Cookies.ContainsKey("MAC") ? session.Cookies["MAC"] : string.Empty,
                    EndPoint = session.RemoteEndPoint.ToString(),
                    CodeName = message.Name,
                    ExceptionString = ex.ToString()
                }, 3);
            }
        }

    }
}
