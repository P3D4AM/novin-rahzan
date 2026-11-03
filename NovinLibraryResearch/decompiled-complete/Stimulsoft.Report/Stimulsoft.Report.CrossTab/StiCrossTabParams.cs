using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossTabParams
{
	public int StartRow { get; set; }

	public int StartColumn { get; set; }

	public bool RenderingIsFinished { get; set; }

	public bool AllowRendering { get; set; } = true;


	public RectangleM DestinationRectangle { get; set; } = RectangleM.Empty;


	public StiContainer DestinationContainer { get; set; }

	public decimal ShiftX { get; set; }

	public bool ForceNoBreak { get; set; }
}
