using System;

namespace Stimulsoft.Report.Events;

public class StiSendEMailEventArgs : EventArgs
{
	public string Recipient { get; set; }

	public string Subject { get; set; }

	public string Body { get; set; }

	public string FilePath { get; set; }

	public StiReport Report { get; set; }

	public bool CallStandardHandler { get; set; } = true;


	public StiSendEMailEventArgs(StiReport report, string recipient, string subject, string body, string filePath)
	{
		Report = report;
		Recipient = recipient;
		Subject = subject;
		Body = body;
		FilePath = filePath;
	}
}
