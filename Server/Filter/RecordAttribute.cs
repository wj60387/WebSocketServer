using Server.Filter;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RecordAttribute : Attribute, IOperationBehavior
{

    //private readonly string _operationName;
    private readonly RecordType _recordType;

    public RecordAttribute( RecordType recordType)
    {
        //_operationName = methodname;
        _recordType = recordType;
    }

    protected RecordInvoker CreateInvoker(IOperationInvoker oldInvoker, string operationName)
    {
        return new RecordInvoker(oldInvoker, _recordType,operationName);
    }
     
    public void Validate(OperationDescription operationDescription)
    {

    }

    public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
    {
        IOperationInvoker oldInvoker = dispatchOperation.Invoker;
        dispatchOperation.Invoker = CreateInvoker(oldInvoker, dispatchOperation.Name);
    }

    public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
    {

    }

    public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
    {

    }
}