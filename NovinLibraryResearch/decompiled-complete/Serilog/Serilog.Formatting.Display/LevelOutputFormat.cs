using System;
using Serilog.Events;
using Serilog.Rendering;

namespace Serilog.Formatting.Display;

internal static class LevelOutputFormat
{
	private static readonly string[][] _titleCaseLevelMap = new string[6][]
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

	private static readonly string[][] _lowerCaseLevelMap = new string[6][]
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

	private static readonly string[][] _upperCaseLevelMap = new string[6][]
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
		if ((value < LogEventLevel.Verbose || value > LogEventLevel.Fatal) ? true : false)
		{
			return Casing.Format(value.ToString(), format);
		}
		if (format == null || (format.Length != 2 && format.Length != 3))
		{
			return Casing.Format(GetLevelMoniker(_titleCaseLevelMap, value), format);
		}
		int num = format[1] - 48;
		if (format.Length == 3)
		{
			num *= 10;
			num += format[2] - 48;
		}
		if (num < 1)
		{
			return string.Empty;
		}
		return format[0] switch
		{
			'w' => GetLevelMoniker(_lowerCaseLevelMap, value, num), 
			'u' => GetLevelMoniker(_upperCaseLevelMap, value, num), 
			't' => GetLevelMoniker(_titleCaseLevelMap, value, num), 
			_ => Casing.Format(GetLevelMoniker(_titleCaseLevelMap, value), format), 
		};
	}

	private static string GetLevelMoniker(string[][] caseLevelMap, LogEventLevel level, int width)
	{
		string[] array = caseLevelMap[(int)level];
		return array[Math.Min(width, array.Length) - 1];
	}

	private static string GetLevelMoniker(string[][] caseLevelMap, LogEventLevel level)
	{
		return caseLevelMap[(int)level][^1];
	}
}
