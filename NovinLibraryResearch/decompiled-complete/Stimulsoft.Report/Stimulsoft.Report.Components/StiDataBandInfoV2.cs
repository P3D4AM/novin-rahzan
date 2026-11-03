using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiDataBandInfoV2 : StiComponentInfo
{
	public StiComponentsCollection GroupHeaders { get; set; }

	public StiComponentsCollection GroupFooters { get; set; }

	public StiComponentsCollection DetailDataBands { get; set; }

	public StiComponentsCollection Details { get; set; }

	public StiComponentsCollection SubReports { get; set; }

	public StiComponentsCollection EmptyBands { get; set; }

	public StiComponentsCollection Headers { get; set; }

	public StiComponentsCollection HierarchicalHeaders { get; set; }

	public StiComponentsCollection FootersOnAllPages { get; set; }

	public StiComponentsCollection FootersOnLastPage { get; set; }

	public StiComponentsCollection HierarchicalFooters { get; set; }

	public bool[] GroupHeaderResults { get; set; }

	public bool[][] GroupHeaderCachedResults { get; set; }

	public bool[] GroupFooterResults { get; set; }

	public bool[][] GroupFooterCachedResults { get; set; }

	public List<StiReportTitleBand> ReportTitles { get; set; }

	public List<StiReportSummaryBand> ReportSummaries { get; set; }

	internal Hashtable DetailDataBandsFromSubReports { get; set; }

	public StiBookmark StoredParentPointer { get; set; }

	public StiBookmark StoredParentBookmark { get; set; }
}
