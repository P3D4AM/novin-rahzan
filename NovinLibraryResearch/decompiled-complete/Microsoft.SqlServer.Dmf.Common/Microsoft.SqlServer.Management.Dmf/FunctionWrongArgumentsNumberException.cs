using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class FunctionWrongArgumentsNumberException : DmfException
{
	private string functionName;

	private int receivedCount;

	private int expectedCount;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.FunctionWrongArgumentsNumber;

	public override string Message => ExceptionTemplatesSR.FunctionWrongArgumentsNumber(functionName, receivedCount, expectedCount);

	public string FunctionName => functionName;

	public int ReceivedCount => receivedCount;

	public int ExpectedCount => expectedCount;

	public FunctionWrongArgumentsNumberException()
	{
	}

	public FunctionWrongArgumentsNumberException(string message)
		: base(message)
	{
	}

	public FunctionWrongArgumentsNumberException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public FunctionWrongArgumentsNumberException(string functionName, int receivedCount, int expectedCount)
	{
		this.functionName = functionName;
		this.receivedCount = receivedCount;
		this.expectedCount = expectedCount;
		Data["HelpLink.EvtData1"] = functionName;
	}

	private FunctionWrongArgumentsNumberException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		functionName = info.GetString("functionName");
		receivedCount = info.GetInt32("receivedCount");
		expectedCount = info.GetInt32("expectedCount");
		Data["HelpLink.EvtData1"] = functionName;
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("functionName", functionName);
		info.AddValue("receivedCount", receivedCount);
		info.AddValue("expectedCount", expectedCount);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		functionName = string.Empty;
		receivedCount = 0;
		expectedCount = 0;
		SetHelpContext("FunctionWrongArgumentsNumberException");
	}
}
