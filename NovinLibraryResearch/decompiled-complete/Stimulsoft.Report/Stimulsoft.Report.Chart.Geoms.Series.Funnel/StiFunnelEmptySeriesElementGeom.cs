using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart.Geoms.Series.Funnel;

public class StiFunnelEmptySeriesElementGeom : StiCellGeom
{
	public List<StiSegmentGeom> Path { get; }

	public override void Draw(StiContext context)
	{
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(Color.FromArgb(50, Color.LightGray), Path, base.ClientRectangle);
		context.DrawPath(new StiPenGeom(Color.LightGray), Path, base.ClientRectangle);
		context.PopSmoothingMode();
	}

	public StiFunnelEmptySeriesElementGeom(RectangleF clientRectangle, List<StiSegmentGeom> path)
		: base(clientRectangle)
	{
		Path = path;
	}
}
