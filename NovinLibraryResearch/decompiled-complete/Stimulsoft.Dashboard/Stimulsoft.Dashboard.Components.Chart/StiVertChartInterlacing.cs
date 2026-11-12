using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiVertChartInterlacing : StiChartInterlacing
{
	public StiVertChartInterlacing()
	{
	}

	[StiUniversalConstructor("InterlacingVert")]
	public StiVertChartInterlacing(Color color, bool visible)
		: base(color, visible)
	{
	}
}
