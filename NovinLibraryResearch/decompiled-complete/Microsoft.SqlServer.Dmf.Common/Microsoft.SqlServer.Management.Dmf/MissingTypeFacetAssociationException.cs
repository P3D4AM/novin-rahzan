using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class MissingTypeFacetAssociationException : DmfException
{
	private string typeName;

	private string facet;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.MissingTypeFacetAssociation;

	public override string Message => ExceptionTemplatesSR.MissingTypeFacetAssociation(typeName, facet);

	public string TypeName => typeName;

	public string Facet => facet;

	public MissingTypeFacetAssociationException()
	{
	}

	public MissingTypeFacetAssociationException(string message)
		: base(message)
	{
	}

	public MissingTypeFacetAssociationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private MissingTypeFacetAssociationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		typeName = (string)info.GetValue("typeName", typeof(string));
		facet = (string)info.GetValue("facet", typeof(string));
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("typeName", typeName);
		info.AddValue("facet", facet);
		base.GetObjectData(info, context);
	}

	public MissingTypeFacetAssociationException(string typeName, string facet)
	{
		this.typeName = typeName;
		this.facet = facet;
	}

	protected override void Init()
	{
		base.Init();
		typeName = string.Empty;
		facet = string.Empty;
		SetHelpContext("MissingTypeFacetAssociationException");
	}
}
