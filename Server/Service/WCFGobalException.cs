using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
  
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Publib.WCFException
{
    /// <summary> 
    /// WCF服务端异常处理器 
    /// </summary> 
    public class WCF_ExceptionHandler : IErrorHandler 
    {
        #region IErrorHandler Members
        string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

        /// <summary> 
        /// HandleError 
        /// </summary> 
        /// <param name="ex">ex</param> 
        /// <returns>true</returns> 
        public bool HandleError(Exception ex)
        {
            return true;
        }

        /// <summary> 
        /// ProvideFault 
        /// </summary> 
        /// <param name="ex">ex</param> 
        /// <param name="version">version</param> 
        /// <param name="msg">msg</param> 
        public void ProvideFault(Exception ex, MessageVersion version, ref System.ServiceModel.Channels.Message msg)
        { 
            
            //在这里处理服务端的消息，将消息写入服务端的日志 
            System.Reflection.MethodBase method = ex.TargetSite;
           
            string methodName = method.Name;
            string exeName = ex.Source;
            string stack = ex.StackTrace;
            string err = "异常发生时间:"+DateTime.Now.ToString() + "\r\n" + ex.Message + exeName + "." + methodName + "\r\n" + stack + "\r\n" 
                + "******************************************************************************\r\n";
             
            var newEx = new FaultException(err);
            MessageFault msgFault = newEx.CreateMessageFault();
            msg = Message.CreateMessage(version, msgFault, newEx.Action);
            if (!Directory.Exists(path+"\\Error"))
            {
                Directory.CreateDirectory(path + "\\Error");
            }
             
            File.AppendAllText(path+@"\Error\"+DateTime.Now.ToString("yy-MM-dd")+".txt", err);
        }
        #endregion
    }

    

    /// <summary> 
    /// WCF服务类的特性 
    /// </summary> 
    /// 

    public class WCF_ExceptionBehaviourAttribute : Attribute, IServiceBehavior, IOperationBehavior
    {
        private readonly Type _errorHandlerType;
        public WCF_ExceptionBehaviourAttribute(Type errorHandlerType)
        {
            _errorHandlerType = errorHandlerType;
        }
        #region IServiceBehavior Members

        public void Validate(ServiceDescription description,
        ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription description,
        ServiceHostBase serviceHostBase,
        Collection<ServiceEndpoint> endpoints,
        BindingParameterCollection parameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription description,
        ServiceHostBase serviceHostBase)
        {
            var handler =
            (IErrorHandler)Activator.CreateInstance(_errorHandlerType);

            foreach (ChannelDispatcherBase dispatcherBase in
            serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = dispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    channelDispatcher.ErrorHandlers.Add(handler);
                }
            }
        }

        #endregion




        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            throw new NotImplementedException();
        }

        public void Validate(OperationDescription operationDescription)
        {
           
        }
    }
}
 
