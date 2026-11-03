using System;

namespace Stimulsoft.Report.Events;

public class StiSaveDocumentEventArgs : EventArgs
{
	public string FilePath { get; set; }

	public string FileName { get; set; }

	public StiReport Report { get; }

	public StiSaveDocumentEventArgs(StiReport report, string filePath, string fileName)
	{
		FilePath = filePath;
		FileName = fileName;
		Report = report;
	}
}
