using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiYRadarAxisLabelGeom : StiCellGeom
{
	private StiRotationMode rotationMode;

	private PointF textPoint;

	private float angle;

	private IStiYRadarAxis axis;

	private string text;

	private StiStripLineXF stripLine;

	public StiRotationMode RotationMode => rotationMode;

	public PointF TextPoint => textPoint;

	public float Angle => angle;

	public IStiYRadarAxis Axis => axis;

	public string Text => text;

	public StiStripLineXF StripLine => stripLine;

	public override void Draw(StiContext context)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		StiStringFormatGeom stringFormatGeom = Axis.YCore.GetStringFormatGeom(context);
		StiFontGeom fontGeom = Axis.YCore.GetFontGeom(context);
		StiBrush brush = (StiBrush)new StiSolidBrush(Axis.Labels.Color);
		context.DrawRotatedString(Text, fontGeom, brush, TextPoint, stringFormatGeom, RotationMode, Angle, Axis.Labels.Antialiasing, (int)(Axis.Labels.Width * context.Options.Zoom));
	}

	public StiYRadarAxisLabelGeom(IStiYRadarAxis axis, RectangleF clientRectangle, PointF textPoint, string text, StiStripLineXF stripLine, float angle, StiRotationMode rotationMode)
		: base(clientRectangle)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		this.axis = axis;
		this.text = text;
		this.stripLine = stripLine;
		this.textPoint = textPoint;
		this.angle = angle;
		this.rotationMode = rotationMode;
	}
}
