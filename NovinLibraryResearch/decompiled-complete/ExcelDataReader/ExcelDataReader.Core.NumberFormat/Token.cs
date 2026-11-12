using System;

namespace ExcelDataReader.Core.NumberFormat;

internal static class Token
{
	public static bool IsExponent(string token)
	{
		if (string.Compare(token, "e+", StringComparison.OrdinalIgnoreCase) != 0)
		{
			return string.Compare(token, "e-", StringComparison.OrdinalIgnoreCase) == 0;
		}
		return true;
	}

	public static bool IsLiteral(string token)
	{
		if (!token.StartsWith("_") && !token.StartsWith("\\") && !token.StartsWith("\"") && !token.StartsWith("*"))
		{
			switch (token)
			{
			default:
				return token == " ";
			case ",":
			case "!":
			case "&":
			case "%":
			case "+":
			case "-":
			case "$":
			case "€":
			case "£":
			case "1":
			case "2":
			case "3":
			case "4":
			case "5":
			case "6":
			case "7":
			case "8":
			case "9":
			case "{":
			case "}":
			case "(":
			case ")":
				break;
			}
		}
		return true;
	}

	public static bool IsNumberLiteral(string token)
	{
		if (!IsPlaceholder(token) && !IsLiteral(token))
		{
			return token == ".";
		}
		return true;
	}

	public static bool IsPlaceholder(string token)
	{
		if (!(token == "0") && !(token == "#"))
		{
			return token == "?";
		}
		return true;
	}

	public static bool IsGeneral(string token)
	{
		return string.Compare(token, "general", StringComparison.OrdinalIgnoreCase) == 0;
	}

	public static bool IsDatePart(string token)
	{
		if (!token.StartsWith("y", StringComparison.OrdinalIgnoreCase) && !token.StartsWith("m", StringComparison.OrdinalIgnoreCase) && !token.StartsWith("d", StringComparison.OrdinalIgnoreCase) && !token.StartsWith("s", StringComparison.OrdinalIgnoreCase) && !token.StartsWith("h", StringComparison.OrdinalIgnoreCase) && (!token.StartsWith("g", StringComparison.OrdinalIgnoreCase) || IsGeneral(token)) && string.Compare(token, "am/pm", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(token, "a/p", StringComparison.OrdinalIgnoreCase) != 0)
		{
			return IsDurationPart(token);
		}
		return true;
	}

	public static bool IsDurationPart(string token)
	{
		if (!token.StartsWith("[h", StringComparison.OrdinalIgnoreCase) && !token.StartsWith("[m", StringComparison.OrdinalIgnoreCase))
		{
			return token.StartsWith("[s", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static bool IsDigit09(string token)
	{
		if (!(token == "0"))
		{
			return IsDigit19(token);
		}
		return true;
	}

	public static bool IsDigit19(string token)
	{
		switch (token)
		{
		case "1":
		case "2":
		case "3":
		case "4":
		case "5":
		case "6":
		case "7":
		case "8":
		case "9":
			return true;
		default:
			return false;
		}
	}
}
