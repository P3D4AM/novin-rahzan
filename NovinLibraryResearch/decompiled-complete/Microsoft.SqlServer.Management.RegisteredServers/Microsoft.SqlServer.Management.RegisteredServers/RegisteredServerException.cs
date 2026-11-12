using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[Serializable]
public class RegisteredServerException : SqlServerManagementException
{
	private const int INIT_BUFFER_SIZE = 1024;

	private static readonly string prodVer = ((AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), inherit: true)[0]).Version;

	protected static string ProdVer => prodVer;

	public virtual RegisteredServerExceptionType RegisteredServerExceptionType => RegisteredServerExceptionType.RegisteredServerException;

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

	public RegisteredServerException()
	{
		Init();
	}

	public RegisteredServerException(string message)
		: base(message)
	{
		Init();
	}

	public RegisteredServerException(string message, Exception innerException)
		: base(message, innerException)
	{
		Init();
	}

	protected RegisteredServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	protected virtual void Init()
	{
		Data.Add("HelpLink.ProdVer", ProdVer);
	}

	protected internal RegisteredServerException SetHelpContext(string resource)
	{
		Data["HelpLink.EvtSrc"] = resource;
		return this;
	}
}
