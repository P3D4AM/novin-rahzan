namespace Stimulsoft.Report.Check;

public class StiReportRenderingMessageCheck : StiReportCheck
{
	private string longMessage;

	public override string ShortMessage => string.Empty;

	public override string LongMessage => longMessage;

	public override StiCheckStatus Status => StiCheckStatus.ReportRenderingMessage;

	public void SetMessage(string message)
	{
		longMessage = message;
	}

	public override object ProcessCheck(StiReport report, object msg)
	{
		return null;
	}
}
