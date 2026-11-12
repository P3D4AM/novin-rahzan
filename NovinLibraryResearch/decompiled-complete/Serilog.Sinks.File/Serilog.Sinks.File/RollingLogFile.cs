using System;

namespace Serilog.Sinks.File;

internal class RollingLogFile
{
	public string Filename { get; }

	public DateTime? DateTime { get; }

	public int? SequenceNumber { get; }

	public RollingLogFile(string filename, DateTime? dateTime, int? sequenceNumber)
	{
		Filename = filename;
		DateTime = dateTime;
		SequenceNumber = sequenceNumber;
	}
}
