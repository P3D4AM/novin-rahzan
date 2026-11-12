using System;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

internal static class Utils
{
	private static readonly char[] CharactersToEscape = new char[12]
	{
		'\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', '`', '#',
		'\'', '"'
	};

	private static readonly char[] Translation = new char[12]
	{
		'0', 'a', 'b', 'f', 'n', 'r', 't', 'v', '`', '#',
		'\'', '"'
	};

	public static T Normalize<T>(this object value)
	{
		return (T)value.Normalize(typeof(T));
	}

	public static object Normalize(this object value, Type type)
	{
		if (type == typeof(bool))
		{
			if (value == null || value is DBNull)
			{
				return false;
			}
			if (value is string text)
			{
				if (bool.TryParse(text, out var _))
				{
					return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
				}
				return text.Length > 0;
			}
			if (value is IEnumerable source)
			{
				return source.Cast<object>().Any();
			}
		}
		try
		{
			return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw new SqlAssessmentTypeMismatchException(type, value?.GetType() ?? typeof(object), value ?? "<null>", innerException);
		}
		catch (FormatException innerException2)
		{
			throw new SqlAssessmentFormatException(type, value.ToString(), innerException2);
		}
		catch (OverflowException innerException3)
		{
			throw new SqlAssessmentOverflowException(type, innerException3);
		}
	}

	public static object Normalize(this object value)
	{
		if (value.IsNumeric())
		{
			return value.Normalize<decimal>();
		}
		return value;
	}

	public static bool AreEqual(object arg1, object arg2, StringComparison comparison = StringComparison.Ordinal)
	{
		if (!NormalizeParams<bool>() && !NormalizeNumericParams() && NormalizeParams<string>())
		{
			return string.Equals((string)arg1, (string)arg2, comparison);
		}
		return object.Equals(arg1, arg2);
		bool NormalizeNumericParams()
		{
			try
			{
				if (arg1.IsNumeric() || arg2.IsNumeric())
				{
					arg1 = arg1.Normalize<decimal>();
					arg2 = arg2.Normalize<decimal>();
					return true;
				}
			}
			catch (SqlAssessmentEvaluationException)
			{
			}
			return false;
		}
		bool NormalizeParams<T>()
		{
			try
			{
				if (arg1 is T)
				{
					arg2 = arg2.Normalize<T>();
					return true;
				}
				if (arg2 is T)
				{
					arg1 = arg1.Normalize<T>();
					return true;
				}
			}
			catch (SqlAssessmentEvaluationException)
			{
			}
			return false;
		}
	}

	public static bool IsNumeric(this object arg)
	{
		if (arg is byte || arg is decimal || arg is double || arg is short || arg is int || arg is long || arg is sbyte || arg is float || arg is ushort || arg is uint || arg is ulong)
		{
			return true;
		}
		return false;
	}

	public static DbParameter AddParameter(this DbCommand command, string name, object value)
	{
		DbParameter dbParameter = command.CreateParameter();
		dbParameter.ParameterName = name;
		dbParameter.Value = value;
		command.Parameters.Add(dbParameter);
		return dbParameter;
	}

	public static string EscapePowerShellDoubleQuoteString(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str.Length + 7);
		stringBuilder.Append('"');
		int num = 0;
		for (int num2 = str.IndexOfAny(CharactersToEscape); num2 >= 0; num2 = str.IndexOfAny(CharactersToEscape, num))
		{
			stringBuilder.Append(str, num, num2 - num);
			stringBuilder.Append('`');
			stringBuilder.Append(Translation[Array.IndexOf(CharactersToEscape, str[num2])]);
			num = num2 + 1;
		}
		stringBuilder.Append(str, num, str.Length - num);
		stringBuilder.Append('"');
		return stringBuilder.ToString();
	}

	public static string EscapePowerShellSingleQuoteString(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str.Length + 7);
		stringBuilder.Append('\'');
		stringBuilder.Append(str);
		stringBuilder.Replace("'", "''", 1, stringBuilder.Length - 1);
		stringBuilder.Append('\'');
		return stringBuilder.ToString();
	}

	public static Expression ToLiteral(object value)
	{
		if (value != null)
		{
			if (!(value is string value2))
			{
				if (!(value is bool value3))
				{
					if (value is DateTimeOffset value4)
					{
						return new Literal<DateTimeOffset>(value4);
					}
					return value.Normalize<decimal>();
				}
				return new Literal<bool>(value3);
			}
			return new Literal<string>(value2);
		}
		return NullLiteralExpression.Instance;
	}
}
