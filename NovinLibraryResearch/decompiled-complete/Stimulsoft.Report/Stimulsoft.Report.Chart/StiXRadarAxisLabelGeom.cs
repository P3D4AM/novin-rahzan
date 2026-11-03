using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiXRadarAxisLabelGeom : StiCellGeom
{
	public Color BorderColor { get; }

	public StiBrush LabelBrush { get; }

	public string Text { get; }

	public float Angle { get; }

	public PointF Point { get; set; }

	public RectangleF LabelRect { get; }

	public IStiXRadarAxis Axis { get; }

	public override void Draw(StiContext context)
	{
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rect = Rectangle.Round(base.ClientRectangle);
		StiPenGeom pen = new StiPenGeom(BorderColor);
		StiFontGeom font = StiFontGeom.ChangeFontSize(Axis.Labels.Font, Axis.Labels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		if (!Axis.Labels.WordWrap)
		{
			genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		}
		if (!Axis.Labels.RotationLabels)
		{
			if (Axis.Labels.Antialiasing)
			{
				context.PushSmoothingModeToAntiAlias();
			}
			if (!rect.IsEmpty)
			{
				context.FillRectangle(Axis.Labels.Brush, GetDrawRectangle());
				if (Axis.Labels.DrawBorder)
				{
					context.DrawRectangle(pen, GetDrawRectangle());
				}
			}
			if (Axis.Labels.Antialiasing)
			{
				context.PopSmoothingMode();
			}
			context.DrawRotatedString(Text, font, LabelBrush, GetDrawRectangle(), genericStringFormat, (StiRotationMode)4, 0f, Axis.Labels.Antialiasing, (int)(Axis.Labels.Width * context.Options.Zoom));
			return;
		}
		int num = (int)Math.Round(LabelRect.X + LabelRect.Width / 2f, 0);
		int num2 = (int)Math.Round(LabelRect.Y + LabelRect.Height / 2f, 0);
		if (Axis.Labels.Antialiasing)
		{
			context.PushSmoothingModeToAntiAlias();
		}
		context.PushTranslateTransform(num, num2);
		context.PushRotateTransform(Angle);
		if (!rect.IsEmpty)
		{
			context.FillRectangle(Axis.Labels.Brush, rect);
			if (Axis.Labels.DrawBorder)
			{
				context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
			}
		}
		context.PopTransform();
		context.PopTransform();
		if (Axis.Labels.Antialiasing)
		{
			context.PopSmoothingMode();
		}
		StiRotationMode mode = (StiRotationMode)5;
		float angle = Angle;
		if (Angle >= 90f && Angle <= 270f)
		{
			angle = Angle + 180f;
			mode = (StiRotationMode)3;
		}
		context.DrawRotatedString(Text, font, LabelBrush, Point, genericStringFormat, mode, angle, Axis.Labels.Antialiasing, (int)(Axis.Labels.Width * context.Options.Zoom));
	}

	public Rectangle GetDrawRectangle()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected I4, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		RectangleF value = new RectangleF(Point.X, Point.Y, base.ClientRectangle.Width, base.ClientRectangle.Height);
		Rectangle result = Rectangle.Round(value);
		StiRotationMode val = (StiRotationMode)5;
		if (!Axis.Labels.RotationLabels)
		{
			if (Angle > 0f && Angle < 180f)
			{
				val = (StiRotationMode)1;
			}
			else if (Angle < 360f && Angle > 180f)
			{
				val = (StiRotationMode)7;
			}
			else if (Angle == 0f)
			{
				val = (StiRotationMode)5;
			}
			else if (Angle == 180f)
			{
				val = (StiRotationMode)3;
			}
		}
		switch ((int)val)
		{
		case 5:
			result = new Rectangle(result.X - result.Width / 2, result.Y - result.Height, result.Width, result.Height);
			break;
		case 4:
			result = new Rectangle(result.X - result.Width / 2, result.Y - result.Height / 2, result.Width, result.Height);
			break;
		case 3:
			result = new Rectangle(result.X - result.Width / 2, result.Y, result.Width, result.Height);
			break;
		case 2:
			result = new Rectangle(result.X, result.Y - result.Height, result.Width, result.Height);
			break;
		case 1:
			result = new Rectangle(result.X, result.Y - result.Height / 2, result.Width, result.Height);
			break;
		case 8:
			result = new Rectangle(result.X - result.Width, result.Y - result.Height, result.Width, result.Height);
			break;
		case 7:
			result = new Rectangle(result.X - result.Width, result.Y - result.Height / 2, result.Width, result.Height);
			break;
		case 6:
			result = new Rectangle(result.X - result.Width, result.Y, result.Width, result.Height);
			break;
		}
		return result;
	}

	public StiXRadarAxisLabelGeom(IStiXRadarAxis axis, string text, StiBrush labelBrush, Color borderColor, float angle, RectangleF clientRectangle, RectangleF labelRect, PointF point)
		: base(clientRectangle)
	{
		Axis = axis;
		LabelRect = labelRect;
		Text = text;
		Angle = angle;
		Point = point;
		LabelBrush = labelBrush;
		BorderColor = borderColor;
	}
}
