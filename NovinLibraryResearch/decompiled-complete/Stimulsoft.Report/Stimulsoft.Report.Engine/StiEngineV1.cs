using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiEngineV1
{
	internal decimal LatestProgressValue { get; set; }

	public bool RequreResetPageNumber { get; set; }

	public StiPage PageInProgress { get; set; }

	public int RealPageNumber { get; set; }

	public bool IsUsedResetPageNumber { get; set; }

	public Hashtable ReportPageNumbers { get; set; }

	public Hashtable ReportTotalPageCounts { get; set; }

	public StiReport Report { get; }

	internal StiProgressHelperV1 ProgressHelper { get; }

	public void ResetPageNumber()
	{
		RequreResetPageNumber = true;
	}

	public void ProcessResetPageNumber(StiReport report)
	{
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			if (renderedPage.PageInfoV1.TotalPageCount == -1)
			{
				renderedPage.PageInfoV1.TotalPageCount = report.TotalPageCount;
			}
		}
		report.PageNumber = 1;
		report.TotalPageCount = 0;
	}

	public StiEngineV1(StiReport report)
	{
		Report = report;
		ProgressHelper = new StiProgressHelperV1(this);
	}
}
