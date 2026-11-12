using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ParameterizedText
{
	public static readonly ParameterizedText Empty = new ParameterizedText();

	private const char StringSubstitutionSign = '#';

	private static readonly Regex VariablePattern = new Regex('@'.ToString(CultureInfo.InvariantCulture) + "\\{(?<key>(\\w|\\.|::)+)(:(?<format>.*?))?\\}", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

	public string Template { get; set; } = string.Empty;


	public static implicit operator ParameterizedText(string template)
	{
		return new ParameterizedText
		{
			Template = template
		};
	}

	public override string ToString()
	{
		return Template;
	}

	public virtual string ToString(IEvaluationContext context)
	{
		return VariablePattern.Replace(Template, (Match match) => GetVariableString(context, match.Groups["key"].Value, match.Groups["format"].Success ? match.Groups["format"].Value : null));
	}

	private static string GetVariableString(IEvaluationContext data, string variableName, string format)
	{
		if (data.TryGetData(variableName, out var value))
		{
			if (value is Expression expression)
			{
				value = expression.Evaluate(data);
			}
			try
			{
				return FormatVariableValueString(format, value);
			}
			catch (FormatException)
			{
			}
		}
		return PassVariableAsIs(variableName, format);
	}

	private static string PassVariableAsIs(string variableName, string format)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Clear();
		stringBuilder.Append('@');
		stringBuilder.Append('{');
		stringBuilder.Append(variableName);
		if (!string.IsNullOrEmpty(format))
		{
			stringBuilder.Append(':');
			stringBuilder.Append(format);
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}

	private static string FormatVariableValueString(string format, object value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!(value is string value2))
		{
			if (value is IEnumerable source)
			{
				return string.Join(", ", from object i in source
					select i.ToString());
			}
			stringBuilder.Append("{0:");
			if (!string.IsNullOrWhiteSpace(format))
			{
				stringBuilder.Append(format);
			}
			stringBuilder.Append('}');
			return string.Format(CultureInfo.CurrentCulture, stringBuilder.ToString(), value);
		}
		return GetVariableValueString(format, value2, stringBuilder);
	}

	private static string GetVariableValueString(string format, string value, StringBuilder sb)
	{
		if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(format))
		{
			return value;
		}
		bool flag = false;
		for (int i = 0; i < format.Length; i++)
		{
			char c = format[i];
			if (c == '#')
			{
				if (ShouldSubstitute(format, i))
				{
					sb.Append(value);
					flag = true;
					continue;
				}
				i++;
			}
			sb.Append(c);
		}
		if (!flag)
		{
			return value;
		}
		return sb.ToString();
	}

	private static bool ShouldSubstitute(string format, int i)
	{
		if (i != format.Length - 1)
		{
			return format[i + 1] != '#';
		}
		return true;
	}
}
