using System.Collections;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossTabInfo : StiComponentInfo
{
	public int DefaultWidth { get; set; } = 60;


	public int DefaultHeight { get; set; } = 14;


	public Hashtable HidedCells { get; } = new Hashtable();


	public StiCross Cross { get; set; }

	public RectangleD RenderRect { get; set; } = RectangleD.Empty;


	public bool FinishRender { get; set; }
}
