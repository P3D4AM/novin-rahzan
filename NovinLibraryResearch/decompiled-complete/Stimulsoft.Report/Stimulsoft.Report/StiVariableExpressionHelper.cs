using Stimulsoft.Base;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report;

public static class StiVariableExpressionHelper
{
	public static IStiAppVariable GetVariableSpecifiedAsExpression(IStiElement element, string expression)
	{
		expression = GetSimpleName(expression);
		if (element == null)
		{
			return null;
		}
		IStiReport report = ((IStiReportComponent)element).GetReport();
		if (report == null)
		{
			return null;
		}
		IStiAppDictionary dictionary = ((IStiApp)report).GetDictionary();
		if (dictionary == null)
		{
			return null;
		}
		return dictionary.GetVariableByName(expression);
	}

	public static bool IsVariableSpecifiedAsExpression(IStiElement element, string expression)
	{
		return GetVariableSpecifiedAsExpression(element, expression) != null;
	}

	public static string ExtractVariableName(string exp)
	{
		if (string.IsNullOrWhiteSpace(exp))
		{
			return exp;
		}
		exp = exp.Trim();
		if (exp.StartsWith("["))
		{
			exp = exp.Substring(1);
		}
		if (exp.EndsWith("]"))
		{
			exp = exp.Substring(0, exp.Length - 1);
		}
		return exp.Trim();
	}

	public static string GetSimpleName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return name;
		}
		name = name.ToLowerInvariant().Trim();
		name = ExtractVariableName(name);
		return name.Trim();
	}
}
