using System;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Rendering;

namespace Serilog.Sinks.SystemConsole.Output;

internal static class LevelOutputFormat
{
	private static readonly string[][] TitleCaseLevelMap = new string[6][]
	{
		new string[7] { "V", "Vb", "Vrb", "Verb", "Verbo", "Verbos", "Verbose" },
		new string[5] { "D", "De", "Dbg", "Dbug", "Debug" },
		new string[11]
		{
			"I", "In", "Inf", "Info", "Infor", "Inform", "Informa", "Informat", "Informati", "Informatio",
			"Information"
		},
		new string[7] { "W", "Wn", "Wrn", "Warn", "Warni", "Warnin", "Warning" },
		new string[5] { "E", "Er", "Err", "Eror", "Error" },
		new string[5] { "F", "Fa", "Ftl", "Fatl", "Fatal" }
	};

	private static readonly string[][] LowerCaseLevelMap = new string[6][]
	{
		new string[7] { "v", "vb", "vrb", "verb", "verbo", "verbos", "verbose" },
		new string[5] { "d", "de", "dbg", "dbug", "debug" },
		new string[11]
		{
			"i", "in", "inf", "info", "infor", "inform", "informa", "informat", "informati", "informatio",
			"information"
		},
		new string[7] { "w", "wn", "wrn", "warn", "warni", "warnin", "warning" },
		new string[5] { "e", "er", "err", "eror", "error" },
		new string[5] { "f", "fa", "ftl", "fatl", "fatal" }
	};

	private static readonly string[][] UpperCaseLevelMap = new string[6][]
	{
		new string[7] { "V", "VB", "VRB", "VERB", "VERBO", "VERBOS", "VERBOSE" },
		new string[5] { "D", "DE", "DBG", "DBUG", "DEBUG" },
		new string[11]
		{
			"I", "IN", "INF", "INFO", "INFOR", "INFORM", "INFORMA", "INFORMAT", "INFORMATI", "INFORMATIO",
			"INFORMATION"
		},
		new string[7] { "W", "WN", "WRN", "WARN", "WARNI", "WARNIN", "WARNING" },
		new string[5] { "E", "ER", "ERR", "EROR", "ERROR" },
		new string[5] { "F", "FA", "FTL", "FATL", "FATAL" }
	};

	public static string GetLevelMoniker(LogEventLevel value, string? format = null)
	{
		int num = (int)value;
		if ((num < 0 || num > 5) ? true : false)
		{
			return Casing.Format(value.ToString(), format);
		}
		if (format == null || (format.Length != 2 && format.Length != 3))
		{
			return Casing.Format(GetLevelMoniker(TitleCaseLevelMap, num), format);
		}
		int num2 = format[1] - 48;
		if (format.Length == 3)
		{
			num2 *= 10;
			num2 += format[2] - 48;
		}
		if (num2 < 1)
		{
			return string.Empty;
		}
		return format[0] switch
		{
			'w' => GetLevelMoniker(LowerCaseLevelMap, num, num2), 
			'u' => GetLevelMoniker(UpperCaseLevelMap, num, num2), 
			't' => GetLevelMoniker(TitleCaseLevelMap, num, num2), 
			_ => Casing.Format(GetLevelMoniker(TitleCaseLevelMap, num), format), 
		};
	}

	private static string GetLevelMoniker(string[][] caseLevelMap, int index, int width)
	{
		string[] array = caseLevelMap[index];
		return array[Math.Min(width, array.Length) - 1];
	}

	private static string GetLevelMoniker(string[][] caseLevelMap, int index)
	{
		return caseLevelMap[index][^1];
	}
}
