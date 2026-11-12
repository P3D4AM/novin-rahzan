using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Microsoft.SqlServer.Management.SqlParser.Diagnostics;

public static class Timers
{
	public enum EventType : byte
	{
		Invalid,
		Start,
		End
	}

	private struct Record
	{
		public EventType eventType;

		public TimerId timerId;

		public long timestamp;

		public bool Matches(ref Record startRecord)
		{
			if (timerId == startRecord.timerId && eventType == EventType.End)
			{
				return startRecord.eventType == EventType.Start;
			}
			return false;
		}
	}

	private abstract class Timer
	{
		protected const int MaxIndentLevel = 64;

		protected const int IndentSize = 2;

		protected const string TextPattern = ". . . . . . . . . . . . . . . . . . . . . . . .: ";

		protected static readonly string[] Paddings = CreatePaddings();

		protected readonly ComplexTimer parent;

		protected readonly TimerId id;

		protected long ticks;

		public string Name => id.ToString();

		public long Milliseconds => ticks / (Stopwatch.Frequency / 1000);

		public long Ticks => ticks;

		public ComplexTimer Parent => parent;

		public abstract int ChildCount { get; }

		public abstract IEnumerable<Timer> ChildTimers { get; }

		public int Depth
		{
			get
			{
				int num = 0;
				foreach (Timer childTimer in ChildTimers)
				{
					int depth = childTimer.Depth;
					if (depth > num)
					{
						num = depth;
					}
				}
				return num + 1;
			}
		}

		private static string[] CreatePaddings()
		{
			string[] array = new string[64];
			for (int i = 0; i < 64; i++)
			{
				array[i] = string.Empty.PadRight(i * 2);
			}
			return array;
		}

		public Timer(TimerId timerId, ComplexTimer parent)
		{
			id = timerId;
			this.parent = parent;
		}

		public void WriteAsXml(TextWriter writer)
		{
			WriteAsXml(writer, 0);
		}

		protected void WriteAsXml(TextWriter writer, int level)
		{
			string text = Paddings[level % 64];
			writer.Write("{0}<timer name=\"{1}\" clock-cycles=\"{2}\" milliseconds=\"{3}\"", text, Name, Ticks, Milliseconds);
			if (ChildCount == 0)
			{
				writer.WriteLine("/>");
				return;
			}
			writer.WriteLine(">");
			foreach (Timer childTimer in ChildTimers)
			{
				childTimer.WriteAsXml(writer, level + 1);
			}
			writer.WriteLine("{0}</timer>", text);
		}

		public void WriteAsText(TextWriter writer)
		{
			int num = ". . . . . . . . . . . . . . . . . . . . . . . .: ".Length - 1;
			int num2 = 14;
			int num3 = 12;
			string format = string.Format(CultureInfo.CurrentCulture, "{{0,-{0}}} {{1, -{1}}} {{2, -{2}}}", num, num2, num3);
			writer.WriteLine(format, "Timer Name", "Clock Cycles", "Milliseconds");
			writer.WriteLine("{0} {1} {2}", string.Empty.PadRight(num, '-'), string.Empty.PadRight(num2, '-'), string.Empty.PadRight(num3, '-'));
			WriteAsText(writer, 0);
		}

		protected void WriteAsText(TextWriter writer, int level)
		{
			string value = Paddings[level % 64];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Length = 0;
			stringBuilder.Append(value);
			stringBuilder.Append(Name);
			if (stringBuilder.Length < ". . . . . . . . . . . . . . . . . . . . . . . .: ".Length)
			{
				stringBuilder.Append(". . . . . . . . . . . . . . . . . . . . . . . .: ".Substring(stringBuilder.Length));
			}
			stringBuilder.AppendFormat("{0,14:N0} {1,12:N0}", ticks, Milliseconds);
			writer.WriteLine(stringBuilder.ToString());
			foreach (Timer childTimer in ChildTimers)
			{
				childTimer.WriteAsText(writer, level + 1);
			}
		}

		public void WriteAsCsv(TextWriter writer)
		{
			int depth = Depth;
			writer.WriteLine("Timer Name{0}Clock Cycles,Milliseconds", string.Empty.PadRight(depth, ','));
			WriteAsCsv(writer, 0, depth);
		}

		protected void WriteAsCsv(TextWriter writer, int level, int maxLevel)
		{
			writer.WriteLine("{3}{0}{4}{1},{2}", Name, ticks, Milliseconds, string.Empty.PadRight(level, ','), string.Empty.PadRight(maxLevel - level, ','));
			foreach (Timer childTimer in ChildTimers)
			{
				childTimer.WriteAsCsv(writer, level + 1, maxLevel);
			}
		}
	}

	private class SimpleTimer : Timer
	{
		public override int ChildCount => 0;

		public override IEnumerable<Timer> ChildTimers
		{
			get
			{
				yield break;
			}
		}

		public SimpleTimer(ref Record startRecord, ref Record endRecord, ComplexTimer parent)
			: base(startRecord.timerId, parent)
		{
			ticks = endRecord.timestamp - startRecord.timestamp;
		}
	}

	private class ComplexTimer : Timer
	{
		protected List<Timer> childTimers;

		protected bool isClosed;

		public override int ChildCount => childTimers.Count;

		public override IEnumerable<Timer> ChildTimers
		{
			get
			{
				foreach (Timer childTimer in childTimers)
				{
					yield return childTimer;
				}
			}
		}

		public bool IsClosed => isClosed;

		protected ComplexTimer(TimerId timerId, ComplexTimer parent)
			: base(timerId, parent)
		{
			childTimers = new List<Timer>(8);
		}

		public ComplexTimer(ref Record startRecord, ComplexTimer parent)
			: this(startRecord.timerId, parent)
		{
			ticks = startRecord.timestamp;
			isClosed = false;
		}

		public bool SetEndRecord(ref Record endRecord)
		{
			if (endRecord.eventType == EventType.End && endRecord.timerId == id)
			{
				ticks = endRecord.timestamp - ticks;
				isClosed = true;
			}
			return isClosed;
		}

		public void AddChildTimer(Timer childTimer)
		{
			childTimers.Add(childTimer);
		}
	}

	private class RootTimer : ComplexTimer
	{
		public RootTimer(long startTimestamp, long endTimestamp)
			: base(TimerId.Root, null)
		{
			ticks = endTimestamp - startTimestamp;
			isClosed = true;
		}
	}

	private static PerfWriterCollection perfWriters;

	private static Format format;

	private static int maxRecordCount;

	private static int currentRecordCount;

	private static Record[] records;

	private static long startTimestamp;

	private static long endTimestamp;

	private static bool isInitialized;

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

	public static void Begin(int recordCount)
	{
	}

	public static void Finish()
	{
	}

	[Conditional("INCLUDE_TIMERS")]
	internal static void Start(TimerId timerId)
	{
		if (isInitialized && currentRecordCount < maxRecordCount)
		{
			records[currentRecordCount].eventType = EventType.Start;
			records[currentRecordCount].timerId = timerId;
			records[currentRecordCount].timestamp = Stopwatch.GetTimestamp();
		}
		currentRecordCount++;
	}

	[Conditional("INCLUDE_TIMERS")]
	internal static void Stop(TimerId timerId)
	{
		if (isInitialized && currentRecordCount < maxRecordCount)
		{
			records[currentRecordCount].eventType = EventType.End;
			records[currentRecordCount].timerId = timerId;
			records[currentRecordCount].timestamp = Stopwatch.GetTimestamp();
		}
		currentRecordCount++;
	}

	[Conditional("INCLUDE_TIMERS")]
	private static void Reset()
	{
		maxRecordCount = 0;
		currentRecordCount = 0;
		records = null;
		startTimestamp = 0L;
		endTimestamp = 0L;
		isInitialized = false;
	}

	private static Timer GenerateTimers(int recordCount)
	{
		RootTimer rootTimer = new RootTimer(startTimestamp, endTimestamp);
		int num = 0;
		ComplexTimer complexTimer = rootTimer;
		while (num < recordCount)
		{
			if (num < recordCount - 1 && records[num + 1].Matches(ref records[num]))
			{
				SimpleTimer childTimer = new SimpleTimer(ref records[num], ref records[num + 1], complexTimer);
				complexTimer.AddChildTimer(childTimer);
				num += 2;
				continue;
			}
			if (records[num].eventType == EventType.Start)
			{
				ComplexTimer complexTimer2 = new ComplexTimer(ref records[num], complexTimer);
				complexTimer.AddChildTimer(complexTimer2);
				complexTimer = complexTimer2;
			}
			else if (complexTimer.SetEndRecord(ref records[num]))
			{
				complexTimer = complexTimer.Parent;
			}
			num++;
		}
		return rootTimer;
	}
}
