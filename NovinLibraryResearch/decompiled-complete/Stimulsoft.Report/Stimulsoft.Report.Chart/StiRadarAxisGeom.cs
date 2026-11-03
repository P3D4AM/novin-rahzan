using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiRadarAxisGeom : StiCellGeom
{
	private IStiYRadarAxis axis;

	public IStiYRadarAxis Axis => axis;

	private void DrawAxisLine(StiContext context, RectangleF rect)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		float right = rect.Right;
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		context.DrawLine(stiPenGeom, right, rect.Y, right, rect.Bottom);
	}

	private void DrawMinorTicks(StiContext context, StiPenGeom pen, float posX, float posY1, float posY2, IStiAxisTicks ticks)
	{
		float num = posY2 - posY1;
		float num2 = num / (float)(ticks.MinorCount + 1);
		float num3 = ticks.MinorLength * context.Options.Zoom;
		for (int i = 1; i <= ticks.MinorCount; i++)
		{
			float num4 = posY1 + num2 * (float)i;
			float x = posX - num3;
			context.DrawLine(pen, posX, num4, x, num4);
		}
	}

	private void DrawTicks(StiContext context, RectangleF rect, IStiAxisTicks ticks, StiPenGeom penLine)
	{
		if (!ticks.Visible)
		{
			return;
		}
		float num = ticks.Length * context.Options.Zoom;
		float right = rect.Right;
		float x = right - num;
		int num2 = 0;
		foreach (StiStripPositionXF item in Axis.Info.TicksCollection)
		{
			float position = item.Position;
			context.DrawLine(penLine, right, position, x, position);
			if (ticks.MinorVisible && num2 != Axis.Info.TicksCollection.Count - 1)
			{
				float position2 = Axis.Info.TicksCollection[num2 + 1].Position;
				DrawMinorTicks(context, penLine, right, position, position2, ticks);
			}
			num2++;
		}
	}

	private void DrawAxis(StiContext context, RectangleF rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		StiPenGeom stiPenGeom = new StiPenGeom(Axis.LineColor, Axis.LineWidth);
		stiPenGeom.PenStyle = Axis.LineStyle;
		DrawTicks(context, rect, Axis.Area.YAxis.Ticks, stiPenGeom);
		DrawAxisLine(context, rect);
	}

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (!(rect.Width <= 0f) && !(rect.Height <= 0f))
		{
			DrawAxis(context, rect);
		}
	}

	public StiRadarAxisGeom(IStiYRadarAxis axis, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.axis = axis;
	}
}
