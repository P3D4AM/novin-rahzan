using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SqlServer.Management.Dmf.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public sealed class ConflictingPropertyValuesException : DmfException
{
	private string type1;

	private string name1;

	private string type2;

	private string name2;

	private string mode;

	public override DmfExceptionType DmfExceptionType => DmfExceptionType.ConflictingPropertyValues;

	public override string Message => base.Message + "\n" + ExceptionTemplatesSR.NotSupported(type1, name1, type2, name2);

	public string Mode => mode;

	public string Type1 => type1;

	public string Name1 => name1;

	public string Type2 => type2;

	public string Name2 => name2;

	public ConflictingPropertyValuesException()
	{
	}

	public ConflictingPropertyValuesException(string message)
		: base(message)
	{
	}

	public ConflictingPropertyValuesException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ConflictingPropertyValuesException(string mode, string type1, string name1, string type2, string name2)
	{
		this.mode = mode;
		this.type1 = type1;
		this.name1 = name1;
		this.type2 = type2;
		this.name2 = name2;
		Data["HelpLink.EvtData2"] = type1;
		Data["HelpLink.EvtData3"] = type2;
	}

	private ConflictingPropertyValuesException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		mode = info.GetString("mode");
		name1 = info.GetString("type1");
		type2 = info.GetString("name1");
		name2 = info.GetString("type2");
		name2 = info.GetString("name2");
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("mode", mode);
		info.AddValue("type1", type1);
		info.AddValue("name1", name1);
		info.AddValue("type2", type2);
		info.AddValue("name2", name2);
		base.GetObjectData(info, context);
	}

	protected override void Init()
	{
		base.Init();
		type1 = string.Empty;
		name1 = string.Empty;
		type2 = string.Empty;
		name2 = string.Empty;
		SetHelpContext("ConflictingPropertyValuesException");
	}
}
