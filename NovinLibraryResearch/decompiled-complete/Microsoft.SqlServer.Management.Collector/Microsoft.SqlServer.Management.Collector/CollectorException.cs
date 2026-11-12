using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Collector;

[Serializable]
public class CollectorException : SqlServerManagementException
{
	private static TraceContext tc;

	private static string prodVer;

	protected static string ProdVer => prodVer;

	public override string HelpLink
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
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

	public CollectorException()
	{
		TraceExceptionDetails();
		Init();
	}

	public CollectorException(string message)
		: base(message)
	{
		TraceExceptionDetails();
		Init();
	}

	public CollectorException(string message, Exception innerException)
		: base(message, innerException)
	{
		TraceExceptionDetails();
		Init();
	}

	public CollectorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Init();
	}

	private void TraceExceptionDetails()
	{
		tc.TraceError("New CollectorException: {0}", ToString());
		if (base.InnerException != null)
		{
			tc.TraceError("InnerException: {0}", base.InnerException.ToString());
		}
	}

	private void Init()
	{
		Data.Add("HelpLink.ProdVer", ProdVer);
	}

	static CollectorException()
	{
		tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectorException");
		prodVer = string.Empty;
		object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(inherit: true);
		if (customAttributes == null)
		{
			return;
		}
		object[] array = customAttributes;
		foreach (object obj in array)
		{
			if (obj is AssemblyFileVersionAttribute)
			{
				prodVer = ((AssemblyFileVersionAttribute)obj).Version;
				break;
			}
		}
	}

	protected internal CollectorException SetHelpContext(string resource)
	{
		Data["HelpLink.EvtSrc"] = resource;
		return this;
	}
}
