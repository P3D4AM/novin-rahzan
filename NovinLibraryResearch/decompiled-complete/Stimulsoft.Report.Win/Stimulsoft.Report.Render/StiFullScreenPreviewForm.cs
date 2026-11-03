using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Render;

public class StiFullScreenPreviewForm : StiFullScreenViewerForm
{
	public StiFullScreenPreviewForm()
		: this(null)
	{
	}

	public StiFullScreenPreviewForm(StiReport report)
		: base(report)
	{
	}
}
