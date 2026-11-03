using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiSeriesGeom : StiCellGeom
{
	private IStiSeries series;

	private List<StiSeriesInteractionData> interactions;

	private StiAreaGeom areaGeom;

	public IStiSeries Series => series;

	public List<StiSeriesInteractionData> Interactions
	{
		get
		{
			return interactions;
		}
		set
		{
			interactions = value;
		}
	}

	public StiAreaGeom AreaGeom
	{
		get
		{
			return areaGeom;
		}
		set
		{
			areaGeom = value;
		}
	}

	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
	}

	public StiSeriesGeom(StiAreaGeom areaGeom, IStiSeries series, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.areaGeom = areaGeom;
		this.series = series;
	}
}
