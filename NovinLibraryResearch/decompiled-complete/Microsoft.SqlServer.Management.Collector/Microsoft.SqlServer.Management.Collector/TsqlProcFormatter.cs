using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Collector;

internal class TsqlProcFormatter
{
	public class SprocArg
	{
		public string argName;

		public string property;

		public bool required;

		public bool output;

		public string outputParamName;

		public SprocArg(string name, string property, bool required, bool output)
		{
			argName = name;
			this.property = property;
			this.required = required;
			this.output = output;
			outputParamName = name;
		}

		public SprocArg(string name, bool required)
		{
			argName = name;
			property = string.Empty;
			this.required = required;
			output = false;
			outputParamName = name;
		}
	}

	public struct RuntimeArg
	{
		public Type type;

		public object value;

		public bool isScript;

		public RuntimeArg(Type type, object value)
		{
			this.type = type;
			this.value = value;
			isScript = false;
		}

		public RuntimeArg(Type type, object value, bool isScript)
		{
			this.type = type;
			this.value = value;
			this.isScript = isScript;
		}
	}

	public struct ScriptTypeArg
	{
		public string argName;

		public Type type;

		public string script;

		public ScriptTypeArg(string name, Type type, string script)
		{
			argName = name;
			this.type = type;
			this.script = script;
		}
	}

	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.TsqlProcFormatter");

	private List<SprocArg> arguments;

	private string procedure;

	private bool hasOutput;

	internal static int sequence = 1;

	public string Procedure
	{
		get
		{
			return procedure;
		}
		set
		{
			procedure = value;
		}
	}

	public List<SprocArg> Arguments => arguments;

	public bool HasOutput => hasOutput;

	public TsqlProcFormatter()
	{
		arguments = new List<SprocArg>();
	}

	public string GenerateScript(SfcInstance sfcObject)
	{
		return GenerateScript(sfcObject, null, null);
	}

	public string GenerateScript(SfcInstance sfcObject, IEnumerable<RuntimeArg> runtimeArgs)
	{
		return GenerateScript(sfcObject, runtimeArgs, null);
	}

	public string GenerateScript(SfcInstance sfcObject, IEnumerable<RuntimeArg> runtimeArgs, IEnumerable<ScriptTypeArg> scriptArgs)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GenerateScript");
		methodTraceContext.TraceVerbose("Generating script for proc {0}", procedure);
		string text = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		hasOutput = false;
		List<RuntimeArg> list = ((runtimeArgs == null) ? new List<RuntimeArg>() : new List<RuntimeArg>(runtimeArgs));
		if (scriptArgs != null)
		{
			IEnumerator<ScriptTypeArg> enumerator = scriptArgs.GetEnumerator();
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				ScriptTypeArg current = enumerator.Current;
				string arg = SqlString($"{current.argName}_{sequence++}");
				string text2 = null;
				if (typeof(int) == current.type)
				{
					text2 = "int";
				}
				else
				{
					if (!(typeof(Guid) == current.type))
					{
						throw new ArgumentException("GenerateScript", "scriptArgs");
					}
					text2 = "uniqueidentifier";
				}
				stringBuilder.AppendFormat("Declare @{0} {1}", arg, text2);
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Format(current.script, arg));
				stringBuilder.AppendLine(";");
				RuntimeArg item = new RuntimeArg(current.type, $"@{arg}", isScript: true);
				list.Add(item);
				arguments.Add(new SprocArg(current.argName, required: true));
			}
		}
		IEnumerator<RuntimeArg> enumerator2 = null;
		if (list != null)
		{
			enumerator2 = list.GetEnumerator();
			enumerator2.Reset();
			enumerator2.MoveNext();
		}
		IEnumerator<SprocArg> enumerator3 = Arguments.GetEnumerator();
		enumerator3.Reset();
		while (enumerator3.MoveNext())
		{
			SprocArg current2 = enumerator3.Current;
			if (!current2.output)
			{
				continue;
			}
			SfcProperty sfcProperty = sfcObject.Properties[current2.property];
			current2.outputParamName = $"{current2.outputParamName}_{sequence++}";
			if (sfcProperty.Type == typeof(int))
			{
				stringBuilder.AppendFormat("Declare @{0} {1}", current2.outputParamName, "int");
			}
			else
			{
				if (!(sfcProperty.Type == typeof(Guid)))
				{
					throw new ArgumentException("GenerateScript", "Arguments");
				}
				stringBuilder.AppendFormat("Declare @{0} {1}", current2.outputParamName, "uniqueidentifier");
			}
			stringBuilder.AppendLine();
			if (hasOutput)
			{
				text = $"{text}, @{current2.outputParamName}";
				continue;
			}
			text = $"Select @{current2.outputParamName}";
			hasOutput = true;
		}
		stringBuilder.Append("EXEC ");
		stringBuilder.Append(Procedure);
		int num = 0;
		foreach (SprocArg argument in Arguments)
		{
			Type type = null;
			object obj = null;
			bool flag = false;
			if (string.IsNullOrEmpty(argument.property))
			{
				RuntimeArg current4 = enumerator2.Current;
				enumerator2.MoveNext();
				type = current4.type;
				obj = current4.value;
				flag = current4.isScript;
			}
			else
			{
				SfcProperty sfcProperty2 = sfcObject.Properties[argument.property];
				if (argument.required && null == sfcProperty2)
				{
					methodTraceContext.TraceError("Throwing SfcPropertyNotSetException('{0}')", argument.property);
					throw new SfcPropertyNotSetException(argument.property);
				}
				type = sfcProperty2.Type;
				obj = sfcProperty2.Value;
				methodTraceContext.TraceVerbose("prop.Type={0}, prop.Value={1}", type.Name, obj);
			}
			if (num > 0)
			{
				stringBuilder.Append(",");
			}
			if (argument.output)
			{
				stringBuilder.AppendFormat(" @{0}=@{1} OUTPUT", argument.argName, argument.outputParamName);
			}
			else
			{
				stringBuilder.AppendFormat(" @{0}=", argument.argName);
				if (flag)
				{
					stringBuilder.Append(obj);
				}
				else if (type == typeof(string))
				{
					stringBuilder.Append(MakeSqlString(obj as string));
				}
				else if (type == typeof(DateTime))
				{
					stringBuilder.Append(MakeSqlString(((DateTime)obj).ToString("s", CultureInfo.InvariantCulture)));
				}
				else if (type == typeof(Guid) || type == typeof(SfcQueryExpression))
				{
					stringBuilder.Append(MakeSqlString(obj.ToString()));
				}
				else if (type.IsEnum)
				{
					stringBuilder.Append(Convert.ToInt32(obj));
				}
				else
				{
					stringBuilder.Append(obj);
				}
			}
			num++;
		}
		if (hasOutput)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(text);
		}
		string text3 = stringBuilder.ToString();
		methodTraceContext.TraceParameterOut("ret", text3);
		return text3;
	}

	internal static string EscapeString(string s, char cEsc)
	{
		if (s == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in s)
		{
			stringBuilder.Append(c);
			if (cEsc == c)
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	internal static string SqlString(string s)
	{
		return EscapeString(s, '\'');
	}

	internal static string MakeSqlString(string s)
	{
		return string.Format(CultureInfo.InvariantCulture, "N'{0}'", EscapeString(s, '\''));
	}

	internal static string SqlBraket(string s)
	{
		return EscapeString(s, ']');
	}

	internal static string MakeSqlBraket(string s)
	{
		return string.Format(CultureInfo.InvariantCulture, "[{0}]", EscapeString(s, ']'));
	}

	internal static string SqlStringBraket(string s)
	{
		return SqlBraket(SqlString(s));
	}
}
