using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class CmdPowerShellTool : DependentTool<ICmdShellTool>, IPowerShellTool, ITool
{
	private static readonly XmlReaderSettings Settings = new XmlReaderSettings
	{
		ConformanceLevel = ConformanceLevel.Fragment
	};

	private const string PowerShellCommandHead = "powershell -NoLogo -NoProfile -ExecutionPolicy RemoteSigned -OutputFormat Xml -EncodedCommand ";

	private const string CommandTemplate = "$({0})";

	public CmdPowerShellTool(RunSpace runSpace)
		: base(runSpace)
	{
	}

	public async Task<IDataView> Execute(string script, IReadOnlyDictionary<string, object> parameters)
	{
		string script2 = BuildCommand(script, parameters);
		string s = string.Join(null, (await base.BaseTool.ExecuteLines(script2).ConfigureAwait(continueOnCapturedContext: false)).Where((string l) => string.CompareOrdinal("#< CLIXML", l) != 0));
		using StringReader input = new StringReader(s);
		using XmlReader xmlReader = XmlReader.Create(input, Settings);
		XDocument val = new XDocument(new object[1] { (object)new XElement(CliXml.ObjsElem) });
		xmlReader.Read();
		while (xmlReader.NodeType == XmlNodeType.Element && XName.op_Implicit(xmlReader.Name) == CliXml.ObjsElem)
		{
			xmlReader.ReadStartElement();
			while (xmlReader.NodeType != XmlNodeType.EndElement)
			{
				((XContainer)val.Root).Add((object)XNode.ReadFrom(xmlReader));
			}
			xmlReader.Read();
		}
		ThrowOnErrors(val);
		return new CliXmlView(val);
	}

	private static void ThrowOnErrors(XDocument doc)
	{
		List<XElement> list = (from e in ((XContainer)doc).Descendants()
			where e.HasAttribute(CliXml.StreamAttr, "Error")
			select e).ToList();
		if (list.Count == 0)
		{
			return;
		}
		throw BuildException(list);
	}

	private static Exception BuildException(List<XElement> errorObjects)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (XElement errorObject in errorObjects)
		{
			stringBuilder.Append(errorObject.GetCliValue());
			stringBuilder.AppendLine();
			if (errorObject.TryGetObjectValue(".InvocationInfo.PositionMessage", 0, out var value) && !string.IsNullOrEmpty((string)value))
			{
				stringBuilder.Append("    ");
				stringBuilder.AppendLine((string)value);
			}
		}
		return new SqlAssessmentEvaluationException(stringBuilder.ToString());
	}

	public static IPowerShellTool Create(RunSpace runSpace)
	{
		return new CmdPowerShellTool(runSpace);
	}

	internal static string BuildCommand(string script, IReadOnlyDictionary<string, object> parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (parameters != null)
		{
			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				if (TryConvertToPowerShellLiteral(parameter.Value, out var str))
				{
					stringBuilder.Append("$" + parameter.Key + "=" + str + ";");
					continue;
				}
				throw new SqlAssessmentProbeParameterException(string.Format(CultureInfo.InvariantCulture, Resources.Unsupported_PowerShell_parameter_type, str), parameter.Key);
			}
		}
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "$({0})", script);
		string text = Convert.ToBase64String(Encoding.Unicode.GetBytes(stringBuilder.ToString()));
		return "powershell -NoLogo -NoProfile -ExecutionPolicy RemoteSigned -OutputFormat Xml -EncodedCommand " + text;
	}

	private static bool TryConvertToPowerShellLiteral(object value, out string str)
	{
		if (!(value is ushort) && !(value is uint) && !(value is ulong) && !(value is short) && !(value is int) && !(value is long) && !(value is byte) && !(value is sbyte) && !(value is float) && !(value is double) && !(value is decimal))
		{
			if (!(value is string str2))
			{
				if (value is bool flag)
				{
					str = (flag ? "$true" : "$false");
					return true;
				}
				str = value.GetType().FullName;
				return false;
			}
			str = Utils.EscapePowerShellSingleQuoteString(str2);
			return true;
		}
		str = value.Normalize<string>();
		return true;
	}

	protected override Task<bool> IsChannelAvailable(RunSpace runSpace)
	{
		return Task.FromResult(result: true);
	}
}
