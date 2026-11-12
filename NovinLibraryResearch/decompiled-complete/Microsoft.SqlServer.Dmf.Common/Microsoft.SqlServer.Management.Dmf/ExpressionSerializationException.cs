using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class ExpressionSerializationException : DmfException
{
	public enum ReaderActionType
	{
		Undefined,
		Move,
		Read
	}

	private ReaderActionType actionType;

	private string typeExpected;

	private string nameExpected;

	private string typeRead;

	private string nameRead;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.ExpressionSerialization;

	public override string Message
	{
		get
		{
			switch (actionType)
			{
			case ReaderActionType.Move:
				if (string.IsNullOrEmpty(nameRead))
				{
					return ExceptionTemplatesSR.CannotMoveToElement;
				}
				return ExceptionTemplatesSR.UnexpectedElement(nameRead, nameExpected);
			case ReaderActionType.Read:
				if (string.IsNullOrEmpty(nameExpected))
				{
					return ExceptionTemplatesSR.UnexpectedType(typeRead, nameRead, typeExpected);
				}
				return ExceptionTemplatesSR.UnexpectedName(typeRead, nameRead, typeExpected, nameExpected);
			default:
				return base.Message;
			}
		}
	}

	public ReaderActionType ActionType => actionType;

	public string TypeExpected => typeExpected;

	public string NameExpected => nameExpected;

	public string TypeRead => TypeRead;

	public string NameRead => NameRead;

	public ExpressionSerializationException()
	{
	}

	public ExpressionSerializationException(string message)
		: base(message)
	{
	}

	public ExpressionSerializationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private ExpressionSerializationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		actionType = (ReaderActionType)info.GetValue("actionType", typeof(ReaderActionType));
		typeRead = (string)info.GetValue("typeRead", typeof(string));
		nameRead = (string)info.GetValue("nameRead", typeof(string));
		typeExpected = (string)info.GetValue("typeExpected", typeof(string));
		nameExpected = (string)info.GetValue("nameExpected", typeof(string));
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("actionType", actionType);
		info.AddValue("typeRead", typeRead);
		info.AddValue("nameRead", nameRead);
		info.AddValue("typeExpected", typeExpected);
		info.AddValue("nameExpected", nameExpected);
		base.GetObjectData(info, context);
	}

	public ExpressionSerializationException(string nameRead, string nameExpected)
	{
		actionType = ReaderActionType.Move;
		this.nameRead = nameRead;
		this.nameExpected = nameExpected;
	}

	public ExpressionSerializationException(string typeRead, string nameRead, string typeExpected, string nameExpected)
	{
		actionType = ReaderActionType.Move;
		this.typeRead = typeRead;
		this.typeExpected = typeExpected;
		this.nameRead = nameRead;
		this.nameExpected = nameExpected;
	}

	protected override void Init()
	{
		base.Init();
		actionType = ReaderActionType.Undefined;
		typeExpected = string.Empty;
		nameExpected = string.Empty;
		typeRead = string.Empty;
		nameRead = string.Empty;
		SetHelpContext("ExpressionSerializationException");
	}
}
