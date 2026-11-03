using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiAxisLabelGeom : StiCellGeom
{
	public StiRotationMode RotationMode { get; }

	public PointF TextPoint { get; }

	public float Angle { get; }

	public float Width { get; }

	public IStiAxis Axis { get; }

	public string Text { get; }

	public StiStripLineXF StripLine { get; }

	public bool WordWrap { get; }

	public override void Draw(StiContext context)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		StiStringFormatGeom stringFormatGeom = Axis.Core.GetStringFormatGeom(context, WordWrap);
		StiFontGeom fontGeom = Axis.Core.GetFontGeom(context);
		StiSolidBrush brush = new StiSolidBrush(Axis.Labels.Color);
		context.DrawRotatedString(Text, fontGeom, brush, TextPoint, stringFormatGeom, RotationMode, Angle, Axis.Labels.Antialiasing, (int)(Width * context.Options.Zoom));
	}

	public StiAxisLabelGeom(IStiAxis axis, RectangleF clientRectangle, PointF textPoint, string text, StiStripLineXF stripLine, float angle, float width, StiRotationMode rotationMode, bool wordWrap)
		: base(clientRectangle)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Axis = axis;
		Text = text;
		StripLine = stripLine;
		TextPoint = textPoint;
		Angle = angle;
		Width = width;
		WordWrap = wordWrap;
		RotationMode = rotationMode;
	}
}
