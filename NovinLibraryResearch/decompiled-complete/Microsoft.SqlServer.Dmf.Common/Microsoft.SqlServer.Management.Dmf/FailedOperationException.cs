using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class FailedOperationException : DmfException
{
	private string operation = string.Empty;

	private string failedObjectName = string.Empty;

	private string failedObjectType = string.Empty;

	public string Operation
	{
		get
		{
			return operation;
		}
		set
		{
			operation = value;
		}
	}

	public string FailedObjectName
	{
		get
		{
			return failedObjectName;
		}
		set
		{
			failedObjectName = value;
		}
	}

	public string FailedObjectType
	{
		get
		{
			return failedObjectType;
		}
		set
		{
			failedObjectType = value;
		}
	}

	public override string Message
	{
		get
		{
			if (base.Message.Length <= 0 && operation != null && operation.Length > 0 && failedObjectName.Length > 0)
			{
				return ExceptionTemplatesSR.FailedOperation(operation, failedObjectType, failedObjectName);
			}
			return base.Message;
		}
	}

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.FailedOperation;

	public FailedOperationException()
	{
		SetHelpContext("FailedOperationExceptionText");
	}

	public FailedOperationException(string message)
		: base(message)
	{
		SetHelpContext("FailedOperationExceptionText");
	}

	public FailedOperationException(string message, Exception innerException)
		: base(message, innerException)
	{
		SetHelpContext("FailedOperationExceptionText");
	}

	private FailedOperationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		operation = info.GetString("operation");
		failedObjectName = info.GetString("failedObjectName");
		failedObjectType = info.GetString("failedObjectType");
		SetHelpContext("FailedOperationExceptionText");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("operation", operation);
		info.AddValue("failedObjectName", failedObjectName);
		info.AddValue("failedObjectType", failedObjectType);
		base.GetObjectData(info, context);
	}

	public FailedOperationException(string operation, string failedObjectName, string failedObjectType, Exception innerException)
		: base("", innerException)
	{
		this.operation = operation;
		this.failedObjectName = failedObjectName;
		this.failedObjectType = failedObjectType;
		SetHelpContext("FailedOperationExceptionText");
		Data.Add("HelpLink.EvtData1", operation);
		if (failedObjectType != null)
		{
			Data.Add("HelpLink.EvtData2", failedObjectType);
		}
	}
}
