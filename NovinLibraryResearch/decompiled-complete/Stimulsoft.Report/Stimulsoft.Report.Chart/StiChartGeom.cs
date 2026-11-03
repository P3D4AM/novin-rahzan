using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiChartGeom : StiCellGeom
{
	public override void Draw(StiContext context)
	{
	}

	public StiChartGeom(RectangleF clientRectangle)
		: base(clientRectangle)
	{
	}
}
