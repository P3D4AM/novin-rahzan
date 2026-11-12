using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class StringPropertyTooLongException : DmfException
{
	private string propertyName;

	private int maxLength;

	private int currentLength;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.StringPropertyTooLong;

	public override string Message => ExceptionTemplatesSR.StringPropertyTooLong(propertyName, maxLength, currentLength);

	public string PropertyName => propertyName;

	public int MaxLength => maxLength;

	public int CurrentLength => currentLength;

	public StringPropertyTooLongException()
	{
	}

	public StringPropertyTooLongException(string message)
		: base(message)
	{
	}

	public StringPropertyTooLongException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public StringPropertyTooLongException(string propertyName, int maxLength, int currentLength)
	{
		this.propertyName = propertyName;
		this.maxLength = maxLength;
		this.currentLength = currentLength;
		Data["HelpLink.EvtData1"] = propertyName;
		Data["HelpLink.EvtData2"] = maxLength;
		Data["HelpLink.EvtData3"] = currentLength;
	}

	private StringPropertyTooLongException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		propertyName = info.GetString("propertyName");
		maxLength = info.GetInt32("maxLength");
		currentLength = info.GetInt32("currentLength");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("propertyName", propertyName);
		info.AddValue("maxLength", maxLength);
		info.AddValue("currentLength", currentLength);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		propertyName = string.Empty;
		maxLength = 0;
		currentLength = 0;
		SetHelpContext("StringPropertyTooLongException");
	}
}
