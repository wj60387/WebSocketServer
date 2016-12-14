using Microsoft.ApplicationBlocks.Data;
using Server.Filter;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Linq;
public class RecordInvoker : IOperationInvoker
{

    private readonly IOperationInvoker _mOldInvoker;
    private readonly string _operationName;
    private readonly RecordType _recordType;

    public bool Check()
    {
        int index1 = OperationContext.Current.IncomingMessageHeaders.FindHeader("SN", "http://tempuri.org");
        int index2 = OperationContext.Current.IncomingMessageHeaders.FindHeader("MAC", "http://tempuri.org");
        if (index1 < 0 || index2 < 0)
            return false;
        // username = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index).ToString();
        // index = OperationContext.Current.IncomingMessageHeaders.FindHeader("MAC", "http://tempuri.org");
        //if (index >= 0)
        //{
        //    pwd = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index).ToString();
        //}
        return true;
    }
    public string GetEndPoint()
    {
        //获取传进的消息属性
        MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
        //获取消息发送的远程终结点IP和端口
        RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
        return string.Format("{0}:{1}", endpoint.Address, endpoint.Port);

    }
    private void WriteLog(object[] inputs)
    {
        int index1 = OperationContext.Current.IncomingMessageHeaders.FindHeader("SN", "http://tempuri.org");
        int index2 = OperationContext.Current.IncomingMessageHeaders.FindHeader("MAC", "http://tempuri.org");
        var sn =index1 < 0?"" :OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index1).ToString();
        var mac =index2 < 0?"": OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index2).ToString();
        var message = "参数为:";
        foreach (var item in inputs)
        {
            if (item is object[])
            {
                var array=item as object[];
                message += string.Join(",", array.Select(a => a+"").ToArray());  
            }
            else
            message += item+""+",";
        }
        string sql = "insert into ServerRunLog(SN, MAC, FuncName, EndPoint, Message) select {0},{1},{2},{3},{4}";
         AuscultationService.AuscultationService.sqlHelper.ExecuteNonQuery(sql, sn, mac, _operationName, GetEndPoint(), message);
    }
    protected bool PreInvoke(object instance, object[] inputs)
    {
        WriteLog(  inputs);
        if (_recordType == (Server.Filter.RecordType.Right | Server.Filter.RecordType.Log | Server.Filter.RecordType.Exception))
        {
           
            return Check();
        }
        return true;
         
        //if (_recordType == RecordType.Log || _recordType == RecordType.LogAndException)
        //{
        //    var r = GetEndPoint();
        //    //记录日志  

        //}

    }

    protected void PostInvoke(object instance, object returnedValue, object[] outputs, Exception err)
    {

        if (err != null)    //如果有异常  
        {
            //记录异常  
        }

    }

    public RecordInvoker(IOperationInvoker oldInvoker, RecordType recordType, string operationName)
    {
        _mOldInvoker = oldInvoker;
        _operationName = operationName;
        _recordType = recordType;
    }
    
    public object[] AllocateInputs()
    {
        return _mOldInvoker.AllocateInputs();
    }

    public object Invoke(object instance, object[] inputs, out object[] outputs)
    {

       if(! PreInvoke(instance, inputs))
       {
           outputs = null;
           return null;
       }
        object returnedValue = null;
        var outputParams = new object[] { };
        Exception exception = null;
        try
        {
            returnedValue = _mOldInvoker.Invoke(instance, inputs, out outputParams);
            outputs = outputParams;
            return returnedValue;
        }
        catch (Exception err)
        {
            outputs = null;
            exception = err;
            return null;
        }
        finally
        {
            PostInvoke(instance, returnedValue, outputParams, exception);
        }

    }

    public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
    {
        PreInvoke(instance, inputs);
        return _mOldInvoker.InvokeBegin(instance, inputs, callback, state);
    }

    public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
    {
        object returnedValue = null;
        object[] outputParams = { };
        Exception exception = null;
        try
        {
            returnedValue = _mOldInvoker.InvokeEnd(instance, out outputs, result);
            outputs = outputParams;
            return returnedValue;
        }
        catch (Exception err)
        {
            outputs = null;
            exception = err;
            return null;
        }
        finally
        {
            PostInvoke(instance, returnedValue, outputParams, exception);
        }
    }

    public bool IsSynchronous
    {
        get
        {
            return _mOldInvoker.IsSynchronous;
        }
    }
}