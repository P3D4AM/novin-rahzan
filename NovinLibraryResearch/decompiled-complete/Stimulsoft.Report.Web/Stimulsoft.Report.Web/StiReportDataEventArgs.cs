namespace Stimulsoft.Report.Web;

public class StiReportDataEventArgs : StiWebEventArgs
{
	public StiReport Report { get; set; }

	public virtual bool IsDataStoreEmpty
	{
		get
		{
			if (Report == null || Report.DataStore == null)
			{
				return true;
			}
			return Report.DataStore.Count == 0;
		}
	}

	public virtual bool IsReportRendered
	{
		get
		{
			if (Report != null)
			{
				return Report.IsRendered;
			}
			return false;
		}
	}

	public virtual bool IsReportDocument
	{
		get
		{
			if (Report != null)
			{
				return Report.IsDocument;
			}
			return false;
		}
	}

	public StiReportDataEventArgs(StiRequestParams requestParams, StiReport report)
		: base(requestParams)
	{
		Report = report;
	}
}
