using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class FunctionWrongArgumentTypeException : DmfException
{
	private string functionName;

	private string receivedType;

	private string expectedType;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.FunctionWrongArgumentType;

	public override string Message => ExceptionTemplatesSR.FunctionWrongArgumentType(functionName, receivedType, expectedType);

	public string FunctionName => functionName;

	public string ReceivedType => receivedType;

	public string ExpectedType => expectedType;

	public FunctionWrongArgumentTypeException()
	{
	}

	public FunctionWrongArgumentTypeException(string message)
		: base(message)
	{
	}

	public FunctionWrongArgumentTypeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public FunctionWrongArgumentTypeException(string functionName, string receivedType, string expectedType)
	{
		this.functionName = functionName;
		this.receivedType = receivedType;
		this.expectedType = expectedType;
		Data["HelpLink.EvtData1"] = functionName;
	}

	private FunctionWrongArgumentTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		functionName = info.GetString("functionName");
		receivedType = info.GetString("receivedType");
		expectedType = info.GetString("expectedType");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("functionName", functionName);
		info.AddValue("receivedType", receivedType);
		info.AddValue("expectedType", expectedType);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		functionName = string.Empty;
		receivedType = string.Empty;
		expectedType = string.Empty;
		SetHelpContext("FunctionWrongArgumentTypeException");
	}
}
