using System;
using System.Diagnostics.Tracing;
using System.IO;

namespace Microsoft.IdentityModel.Logging;

public class TextWriterEventListener : EventListener
{
	private StreamWriter _streamWriter;

	private bool _disposeStreamWriter = true;

	public static readonly string DefaultLogFileName = "IdentityModelLogs.txt";

	public TextWriterEventListener()
	{
		try
		{
			Stream stream = new FileStream(DefaultLogFileName, FileMode.OpenOrCreate, FileAccess.Write);
			_streamWriter = new StreamWriter(stream);
			_streamWriter.AutoFlush = true;
		}
		catch (Exception innerException)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException("MIML10001: Cannot create the fileStream or StreamWriter to write logs. See inner exception.", innerException));
			throw;
		}
	}

	public TextWriterEventListener(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			throw LogHelper.LogArgumentNullException("filePath");
		}
		try
		{
			Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
			_streamWriter = new StreamWriter(stream);
			_streamWriter.AutoFlush = true;
		}
		catch (Exception innerException)
		{
			LogHelper.LogExceptionMessage(new InvalidOperationException("MIML10001: Cannot create the fileStream or StreamWriter to write logs. See inner exception.", innerException));
			throw;
		}
	}

	public TextWriterEventListener(StreamWriter streamWriter)
	{
		if (streamWriter == null)
		{
			throw LogHelper.LogArgumentNullException("streamWriter");
		}
		_streamWriter = streamWriter;
		_disposeStreamWriter = false;
	}

	protected override void OnEventWritten(EventWrittenEventArgs eventData)
	{
		if (eventData == null)
		{
			throw LogHelper.LogArgumentNullException("eventData");
		}
		if (eventData.Payload == null || eventData.Payload.Count <= 0)
		{
			LogHelper.LogInformation("MIML10000: eventData.Payload is null or empty. Not logging any messages.");
			return;
		}
		for (int i = 0; i < eventData.Payload.Count; i++)
		{
			_streamWriter.WriteLine(eventData.Payload[i].ToString());
		}
	}

	public override void Dispose()
	{
		if (_disposeStreamWriter && _streamWriter != null)
		{
			_streamWriter.Flush();
			_streamWriter.Dispose();
		}
		GC.SuppressFinalize(this);
		base.Dispose();
	}
}
