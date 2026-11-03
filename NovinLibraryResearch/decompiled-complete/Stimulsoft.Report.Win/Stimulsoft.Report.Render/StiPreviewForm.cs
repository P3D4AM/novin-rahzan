using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Render;

public class StiPreviewForm : StiViewerForm
{
	public StiPreviewForm()
		: this(null)
	{
	}

	public StiPreviewForm(StiReport report)
		: base(report)
	{
	}
}
