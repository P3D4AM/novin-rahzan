using System.ComponentModel;
using System.Data;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiContainerInfoV2 : StiComponentInfo
{
	internal int DataBandPosition { get; set; } = -1;


	internal DataRow DataSourceRow { get; set; }

	internal object BusinessObjectCurrent { get; set; }

	internal bool IsAutoRendered { get; set; }

	internal bool IgnoreResetPageNumber { get; set; }

	internal bool IsColumns { get; set; }

	internal int RenderStep { get; set; } = -1;


	[Browsable(false)]
	public int SetSegmentPerWidth { get; set; } = -1;


	internal StiBand ParentBand { get; set; }
}
