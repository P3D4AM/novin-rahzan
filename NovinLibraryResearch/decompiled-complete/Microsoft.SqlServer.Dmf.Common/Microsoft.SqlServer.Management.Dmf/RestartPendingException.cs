using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
[StrongNameIdentityPermission(SecurityAction.InheritanceDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public class RestartPendingException : DmfException
{
	public RestartPendingException()
	{
	}

	public RestartPendingException(string message)
		: base(message)
	{
	}

	public RestartPendingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}

	protected RestartPendingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class RestartPendingException<T> : RestartPendingException
{
	private T configValue;

	private T runValue;

	private string propertyName;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.RestartPending;

	public override string Message => ExceptionTemplatesSR.ConfigValueMismatch(propertyName, configValue.ToString(), runValue.ToString());

	public T ConfigValue
	{
		get
		{
			return configValue;
		}
		set
		{
			configValue = value;
		}
	}

	public T RunValue
	{
		get
		{
			return runValue;
		}
		set
		{
			runValue = value;
		}
	}

	public string PropertyName => propertyName;

	public RestartPendingException()
	{
	}

	public RestartPendingException(string message)
		: base(message)
	{
	}

	public RestartPendingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public RestartPendingException(string propertyName, T configValue, T runValue)
	{
		this.propertyName = propertyName;
		this.configValue = configValue;
		this.runValue = runValue;
		Data["HelpLink.EvtData1"] = propertyName;
		Data["HelpLink.EvtData2"] = configValue;
		Data["HelpLink.EvtData3"] = runValue;
	}

	private RestartPendingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		propertyName = info.GetString("propertyName");
		configValue = (T)info.GetValue("configValue", typeof(T));
		runValue = (T)info.GetValue("runValue", typeof(T));
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("propertyName", propertyName);
		info.AddValue("configValue", configValue);
		info.AddValue("runValue", runValue);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		propertyName = string.Empty;
		SetHelpContext("StringPropertyTooLongException");
	}
}
