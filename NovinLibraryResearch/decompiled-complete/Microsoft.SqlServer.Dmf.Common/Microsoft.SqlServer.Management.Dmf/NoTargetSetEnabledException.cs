using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class NoTargetSetEnabledException : DmfException
{
	private string objectSetName;

	public override string Message => ExceptionTemplatesSR.NoTargetSetEnabled(objectSetName);

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.NoTargetSetEnabled;

	public string ObjectSetName => objectSetName;

	public NoTargetSetEnabledException()
	{
	}

	public NoTargetSetEnabledException(string objectSetName)
	{
		this.objectSetName = objectSetName;
		Data["HelpLink.EvtData1"] = objectSetName;
	}

	public NoTargetSetEnabledException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private NoTargetSetEnabledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		objectSetName = info.GetString("objectSetName");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("objectSetName", objectSetName);
		base.GetObjectData(info, context);
	}
}
