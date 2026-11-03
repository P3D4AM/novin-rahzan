namespace Stimulsoft.Report.Engine;

public class StiRenderState
{
	internal decimal LatestProgressValue { get; set; }

	public int FromPage { get; }

	public int ToPage { get; }

	public bool ShowProgress { get; set; }

	internal bool IsSubReportMode { get; set; }

	public bool DestroyPagesWhichNotInRange { get; }

	public bool RenderOnlyPagesFromRange { get; }

	public StiRenderState(bool showProgress)
		: this(-1, -1, showProgress)
	{
	}

	public StiRenderState(int fromPage, int toPage, bool showProgress)
		: this(fromPage, toPage, showProgress, destroyPagesWhichNotInRange: true, renderOnlyPagesFromRange: false)
	{
	}

	public StiRenderState(int fromPage, int toPage, bool showProgress, bool destroyPagesWhichNotInRange, bool renderOnlyPagesFromRange)
	{
		FromPage = fromPage;
		ToPage = toPage;
		if (fromPage == -1 && toPage == -1)
		{
			destroyPagesWhichNotInRange = false;
		}
		ShowProgress = showProgress;
		DestroyPagesWhichNotInRange = destroyPagesWhichNotInRange;
		RenderOnlyPagesFromRange = renderOnlyPagesFromRange;
	}
}
