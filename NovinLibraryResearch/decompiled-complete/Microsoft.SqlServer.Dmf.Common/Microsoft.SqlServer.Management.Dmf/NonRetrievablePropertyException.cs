using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class NonRetrievablePropertyException : DmfException
{
	private string propertyName;

	public string PropertyName
	{
		get
		{
			return propertyName;
		}
		internal set
		{
			propertyName = value;
		}
	}

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.NonRetrievableProperty;

	public override string Message => ExceptionTemplatesSR.NonRetrievableProperty(propertyName);

	public NonRetrievablePropertyException()
	{
	}

	public NonRetrievablePropertyException(string propertyName)
	{
		this.propertyName = propertyName;
	}

	public NonRetrievablePropertyException(string propertyName, Exception innerException)
		: base(ExceptionTemplatesSR.NonRetrievableProperty(propertyName), innerException)
	{
		PropertyName = propertyName;
	}

	private NonRetrievablePropertyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		propertyName = (string)info.GetValue("propertyName", typeof(string));
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("propertyName", propertyName);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		SetHelpContext("NonRetrievableProperty");
	}
}
