using System.Collections;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiCloneInfoV1 : StiComponentInfo
{
	public Hashtable ResBottomRenderedFooters { get; set; }

	public Hashtable ResBottomRenderedGroupFooters { get; set; }

	public Hashtable ResBottomRenderedDataBands { get; set; }

	public Hashtable ResBottomRenderedHeaders { get; set; }

	public StiComponentsCollection ResBottomRenderedParentsFooters { get; set; }

	public StiComponentsCollection ResBottomRenderedParentsGroupFooters { get; set; }

	public StiComponentsCollection ResBottomRenderedParentsDataBands { get; set; }

	public StiComponentsCollection ResBottomRenderedParentsHeaders { get; set; }

	public StiComponent ResLastDataBand { get; set; }

	public Hashtable WidthHor { get; set; }

	public Hashtable LeftHor { get; set; }
}
