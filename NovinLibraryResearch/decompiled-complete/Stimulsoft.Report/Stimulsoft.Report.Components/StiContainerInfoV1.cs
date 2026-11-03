using System.Collections;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiContainerInfoV1 : StiComponentInfo
{
	public bool ForceBreak { get; set; }

	public bool IsColumn { get; set; }

	public int CurrentColumn { get; set; }

	public StiComponent CurrentComponent { get; set; }

	public StiComponentsCollection CloneComponents { get; set; }

	public Hashtable BottomRenderedFooters { get; set; }

	public Hashtable BottomRenderedGroupFooters { get; set; }

	public Hashtable BottomRenderedDataBands { get; set; }

	public Hashtable BottomRenderedHeaders { get; set; }

	public StiComponentsCollection BottomRenderedParentsFooters { get; set; }

	public StiComponentsCollection BottomRenderedParentsGroupFooters { get; set; }

	public StiComponentsCollection BottomRenderedParentsDataBands { get; set; }

	public StiComponentsCollection BottomRenderedParentsHeaders { get; set; }

	public StiComponent LastDataBand { get; set; }
}
