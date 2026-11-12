using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Dmf;

[Serializable]
[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
[StrongNameIdentityPermission(SecurityAction.InheritanceDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100272736ad6e5f9586bac2d531eabc3acc666c2f8ec879fa94f8f7b0327d2ff2ed523448f83c3d5c5dd2dfc7bc99c5286b2c125117bf5cbe242b9d41750732b2bdffe649c6efb8e5526d526fdd130095ecdb7bf210809c6cdad8824faa9ac0310ac3cba2aa0523567b2dfa7fe250b30facbd62d4ec99b94ac47c7d3b28f1f6e4c8")]
public class DmfException : SqlServerManagementException
{
	private const int INIT_BUFFER_SIZE = 1024;

	private static readonly string prodVer = ((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), inherit: true)[0]).Version;

	protected static string ProdVer => prodVer;

	public virtual DmfExceptionType DmfExceptionType => DmfExceptionType.DmfException;

	public override string HelpLink
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append(Data["HelpLink.BaseHelpUrl"] as string);
			stringBuilder.Append("?");
			stringBuilder.AppendFormat("ProdName={0}", Data["HelpLink.ProdName"] as string);
			if (Data.Contains("HelpLink.ProdVer"))
			{
				stringBuilder.AppendFormat("&ProdVer={0}", Data["HelpLink.ProdVer"] as string);
			}
			if (Data.Contains("HelpLink.EvtSrc"))
			{
				stringBuilder.AppendFormat("&EvtSrc={0}", Data["HelpLink.EvtSrc"] as string);
			}
			if (Data.Contains("HelpLink.EvtData1"))
			{
				stringBuilder.AppendFormat("&EvtID={0}", Data["HelpLink.EvtData1"] as string);
				for (int i = 2; i < 10; i++)
				{
					if (!Data.Contains("HelpLink.EvtData" + i))
					{
						break;
					}
					stringBuilder.Append("+");
					stringBuilder.Append(Data["HelpLink.EvtData" + i] as string);
				}
			}
			stringBuilder.AppendFormat("&LinkId={0}", Data["HelpLink.LinkId"] as string);
			return stringBuilder.ToString().Replace(' ', '+');
		}
	}

	public DmfException()
	{
		Init();
	}

	public DmfException(string message)
		: base(message)
	{
		Init();
	}

	public DmfException(string message, Exception innerException)
		: base(message, innerException)
	{
		Init();
	}

	protected DmfException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	protected virtual void Init()
	{
		Data.Add("HelpLink.ProdVer", ProdVer);
	}

	protected internal DmfException SetHelpContext(string resource)
	{
		Data["HelpLink.EvtSrc"] = resource;
		return this;
	}
}
