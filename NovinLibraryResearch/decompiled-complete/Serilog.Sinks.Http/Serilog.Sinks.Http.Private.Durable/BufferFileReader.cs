using System.IO;
using System.Text;
using Serilog.Debugging;

namespace Serilog.Sinks.Http.Private.Durable;

public static class BufferFileReader
{
	private struct Line
	{
		public string Text;

		public string NewLine;
	}

	private const char CR = '\r';

	private const char LF = '\n';

	private static readonly string CRLFString = $"{'\r'}{'\n'}";

	private static readonly string LFString = $"{'\n'}";

	public static Batch Read(string fileName, ref long nextLineBeginsAtOffset, long? logEventLimitBytes, int? logEventsInBatchLimit, long? batchSizeLimitBytes)
	{
		using FileStream stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		return Read(stream, ref nextLineBeginsAtOffset, logEventLimitBytes, logEventsInBatchLimit, batchSizeLimitBytes);
	}

	public static Batch Read(Stream stream, ref long nextLineBeginsAtOffset, long? logEventLimitBytes, int? logEventsInBatchLimit, long? batchSizeLimitBytes)
	{
		Batch batch = new Batch();
		long num = 0L;
		while (stream.Length > nextLineBeginsAtOffset)
		{
			stream.Position = nextLineBeginsAtOffset;
			Line line = ReadLine(stream);
			if (string.IsNullOrEmpty(line.Text))
			{
				break;
			}
			long num2 = ByteSize.From(line.Text);
			bool flag = true;
			if (num2 > logEventLimitBytes)
			{
				flag = false;
				SelfLog.WriteLine("Log event exceeds the size limit of {0} bytes set for this sink and will be dropped; data: {1}", logEventLimitBytes, line.Text);
			}
			else if (num2 > batchSizeLimitBytes)
			{
				flag = false;
				SelfLog.WriteLine("Event exceeds the batch size limit of {0} bytes set for this sink and will be dropped; data: {1}", batchSizeLimitBytes, line);
			}
			else if (num + num2 > batchSizeLimitBytes)
			{
				batch.HasReachedLimit = true;
				break;
			}
			nextLineBeginsAtOffset += num2 + ByteSize.From(line.NewLine);
			if (flag)
			{
				batch.LogEvents.Add(line.Text);
				num += num2;
			}
			if (batch.LogEvents.Count == logEventsInBatchLimit)
			{
				batch.HasReachedLimit = true;
				break;
			}
		}
		return batch;
	}

	private static Line ReadLine(Stream stream)
	{
		StreamReader streamReader = new StreamReader(stream, Encoding.UTF8WithoutBom, detectEncodingFromByteOrderMarks: false, 128);
		StringBuilder stringBuilder = new StringBuilder();
		while (true)
		{
			int num = streamReader.Read();
			switch (num)
			{
			case -1:
			{
				Line result = default(Line);
				result.Text = string.Empty;
				result.NewLine = string.Empty;
				return result;
			}
			case 13:
			{
				Line result = default(Line);
				result.Text = stringBuilder.ToString();
				result.NewLine = CRLFString;
				return result;
			}
			case 10:
			{
				Line result = default(Line);
				result.Text = stringBuilder.ToString();
				result.NewLine = LFString;
				return result;
			}
			}
			stringBuilder.Append((char)num);
		}
	}
}
