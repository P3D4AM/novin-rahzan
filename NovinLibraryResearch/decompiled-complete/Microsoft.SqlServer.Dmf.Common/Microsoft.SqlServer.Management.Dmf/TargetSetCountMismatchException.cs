using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class TargetSetCountMismatchException : DmfException
{
	private string objectSetName;

	private string facetName;

	public override string Message => ExceptionTemplatesSR.TargetSetCountMismatch(objectSetName, facetName);

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.TargetSetCountMismatch;

	public string ObjectSetName => objectSetName;

	public string FacetName => facetName;

	public TargetSetCountMismatchException()
	{
	}

	public TargetSetCountMismatchException(string message)
		: base(message)
	{
	}

	public TargetSetCountMismatchException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public TargetSetCountMismatchException(string objectSetName, string facetName)
	{
		this.objectSetName = objectSetName;
		this.facetName = facetName;
		Data["HelpLink.EvtData1"] = objectSetName;
		Data["HelpLink.EvtData2"] = facetName;
	}

	private TargetSetCountMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		objectSetName = info.GetString("objectSetName");
		facetName = info.GetString("facetName");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("objectSetName", objectSetName);
		info.AddValue("facetName", facetName);
		base.GetObjectData(info, context);
	}
}
