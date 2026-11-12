using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class UnsupportedTypeException : DmfException
{
	private string node;

	private string typeName;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.UnsupportedType;

	public override string Message => ExceptionTemplatesSR.UnsupportedConstantType(node, typeName);

	public string NodeType => node;

	public string TypeName => typeName;

	public UnsupportedTypeException()
	{
	}

	public UnsupportedTypeException(string message)
		: base(message)
	{
	}

	public UnsupportedTypeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private UnsupportedTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		node = (string)info.GetValue("node", typeof(string));
		typeName = (string)info.GetValue("typeName", typeof(string));
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("node", node);
		info.AddValue("typeName", typeName);
		base.GetObjectData(info, context);
	}

	public UnsupportedTypeException(string node, string typeName)
	{
		this.node = node;
		this.typeName = typeName;
	}

	protected override void Init()
	{
		base.Init();
		typeName = string.Empty;
		SetHelpContext("UnsupportedTypeException");
	}
}
