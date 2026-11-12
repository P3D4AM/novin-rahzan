using System.ComponentModel;

namespace Stimulsoft.Report.Web;

public class StiSaveReportEventArgs : StiReportDataEventArgs
{
	public bool IsAutoSave => base.RequestParams.Designer.IsAutoSave;

	public bool IsNewReport => base.RequestParams.Designer.IsNewReport;

	public bool SendReportToClient { get; set; }

	public string FileName => base.RequestParams.Designer.FileName;

	public string Password => base.RequestParams.Designer.Password;

	public int ErrorCode { get; set; }

	public string ErrorString { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsDataStoreEmpty => base.IsDataStoreEmpty;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsReportDocument => base.IsReportDocument;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool IsReportRendered => base.IsReportRendered;

	public StiSaveReportEventArgs(StiRequestParams requestParams, StiReport report)
		: base(requestParams, report)
	{
		SendReportToClient = false;
		ErrorCode = -1;
		ErrorString = string.Empty;
	}
}
