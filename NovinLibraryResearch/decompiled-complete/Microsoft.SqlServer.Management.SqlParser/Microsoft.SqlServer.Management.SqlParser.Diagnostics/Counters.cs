using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.SqlServer.Management.SqlParser.Diagnostics;

public static class Counters
{
	private const int CounterLo = 1;

	private const int CounterHi = 0;

	private const int CounterIdCount = 0;

	private static PerfWriterCollection perfWriters;

	private static Format format;

	private static uint[] idCounters;

	private static Dictionary<Type, uint> ctorCounters;

	private static bool isInitialized;

	private const string TextPattern = ". . . . . . . . . . . . . . . . . . . . . . . . . . . . .: ";

	public static Format OutputFormat
	{
		get
		{
			return format;
		}
		set
		{
			format = value;
		}
	}

	public static IList<TextWriter> PerfWriters
	{
		get
		{
			if (perfWriters == null)
			{
				perfWriters = new PerfWriterCollection();
			}
			return perfWriters;
		}
	}

	public static void Begin()
	{
	}

	public static void Finish()
	{
	}

	[Conditional("INCLUDE_COUNTERS")]
	internal static void Increment(CounterId counterId)
	{
		if (isInitialized)
		{
			idCounters[(uint)counterId]++;
		}
	}

	[Conditional("INCLUDE_COUNTERS")]
	internal static void Increment(object instance)
	{
		_ = isInitialized;
	}

	[Conditional("INCLUDE_COUNTERS")]
	internal static void Increment(Type type)
	{
		if (isInitialized)
		{
			uint value = 0u;
			ctorCounters.TryGetValue(type, out value);
			ctorCounters[type] = value + 1;
		}
	}

	static Counters()
	{
		idCounters = new uint[0];
		ctorCounters = new Dictionary<Type, uint>(32);
		isInitialized = false;
	}

	[Conditional("INCLUDE_COUNTERS")]
	private static void Reset()
	{
		for (int i = 1; i <= 0; i++)
		{
			idCounters[i] = 0u;
		}
		ctorCounters.Clear();
		isInitialized = false;
	}

	[Conditional("INCLUDE_COUNTERS")]
	private static void WriteAsXml(TextWriter writer, IDictionary<string, uint> results)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<counters>");
		foreach (KeyValuePair<string, uint> result in results)
		{
			stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, "  <counter name=\"{0}\" value=\"{1}\"/>", result.Key, result.Value));
		}
		stringBuilder.AppendLine("</counters>");
		writer.Write(stringBuilder.ToString());
	}

	[Conditional("INCLUDE_COUNTERS")]
	private static void WriteAsText(TextWriter writer, IDictionary<string, uint> results)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = ". . . . . . . . . . . . . . . . . . . . . . . . . . . . .: ".Length - 1;
		int num2 = 8;
		string text = string.Format(CultureInfo.CurrentCulture, "{{0,-{0}}} {{1, -{1}}}", num, num2);
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, text, "Counter Name", "Value");
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0} {1}", string.Empty.PadRight(num, '-'), string.Empty.PadRight(num2, '-'));
		stringBuilder.AppendLine();
		foreach (KeyValuePair<string, uint> result in results)
		{
			string key = result.Key;
			stringBuilder.Append(key);
			if (key.Length < num)
			{
				stringBuilder.Append(". . . . . . . . . . . . . . . . . . . . . . . . . . . . .: ".Substring(key.Length));
			}
			stringBuilder.AppendFormat("{0,8:N0}", result.Value);
			stringBuilder.AppendLine();
		}
		writer.Write(stringBuilder.ToString());
	}

	[Conditional("INCLUDE_COUNTERS")]
	private static void WriteAsCsv(TextWriter writer, IDictionary<string, uint> results)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Counter,Value");
		foreach (KeyValuePair<string, uint> result in results)
		{
			stringBuilder.Append(result.Key);
			stringBuilder.Append(",");
			stringBuilder.AppendLine(result.Value.ToString(CultureInfo.CurrentCulture));
		}
		writer.Write(stringBuilder.ToString());
	}

	private static IDictionary<string, uint> MergeCounters()
	{
		SortedList<string, uint> sortedList = new SortedList<string, uint>(ctorCounters.Count);
		for (int i = 1; i <= 0; i++)
		{
			sortedList.Add(((CounterId)i).ToString(), idCounters[i]);
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<Type, uint> ctorCounter in ctorCounters)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append("ctor_");
			Type key = ctorCounter.Key;
			stringBuilder.Append(key.Name);
			if (key.GetIsGenericType())
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				stringBuilder.Append("<");
				Type[] genericArguments = key.GetGenericArguments();
				foreach (Type type in genericArguments)
				{
					stringBuilder.Append(type.Name);
					stringBuilder.Append(",");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append(">");
			}
			sortedList.Add(stringBuilder.ToString(), ctorCounter.Value);
		}
		return sortedList;
	}
}
