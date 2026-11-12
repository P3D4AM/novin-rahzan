using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiHorChartInterlacing : StiChartInterlacing
{
	public StiHorChartInterlacing()
	{
	}

	[StiUniversalConstructor("InterlacingHor")]
	public StiHorChartInterlacing(Color color, bool visible)
		: base(color, visible)
	{
	}
}
