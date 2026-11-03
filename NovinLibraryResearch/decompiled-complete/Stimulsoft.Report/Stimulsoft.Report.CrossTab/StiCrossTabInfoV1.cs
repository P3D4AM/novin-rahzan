using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossTabInfoV1 : StiComponentInfo
{
	public int StartRow { get; set; }

	public int StartCol { get; set; }

	public RectangleD RenderRect { get; set; } = RectangleD.Empty;


	public bool FinishRender { get; set; }
}
