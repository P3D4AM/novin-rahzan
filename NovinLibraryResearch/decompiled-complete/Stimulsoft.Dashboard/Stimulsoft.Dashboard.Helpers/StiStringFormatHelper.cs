using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiStringFormatHelper
{
	internal static StiStringFormatGeom GetStringFormatGeom(StiContext context)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		genericStringFormat.FormatFlags = (StringFormatFlags)(genericStringFormat.FormatFlags | 0x1800);
		genericStringFormat.Alignment = (StringAlignment)1;
		genericStringFormat.LineAlignment = (StringAlignment)1;
		return genericStringFormat;
	}

	internal static void MeasureAlignmentParameters(StiHorAlignment alignment, RectangleF elementRectText, ref StiRotationMode rotationMode, ref PointF point)
	{
		switch (alignment)
		{
		case StiHorAlignment.Left:
			rotationMode = StiRotationMode.LeftCenter;
			point.X = elementRectText.X;
			point.Y = elementRectText.Y + elementRectText.Height / 2f;
			break;
		case StiHorAlignment.Center:
			rotationMode = StiRotationMode.CenterCenter;
			point.X = elementRectText.X + elementRectText.Width / 2f;
			point.Y = elementRectText.Y + elementRectText.Height / 2f;
			break;
		case StiHorAlignment.Right:
			rotationMode = StiRotationMode.RightCenter;
			point.X = elementRectText.X + elementRectText.Width;
			point.Y = elementRectText.Y + elementRectText.Height / 2f;
			break;
		}
	}

	internal static float GetFontSize(StiContext context, RectangleF rect, string text, Font font)
	{
		SizeF sizeF = context.MeasureString(text, new StiFontGeom(font));
		if (sizeF.Width == 0f || sizeF.Height == 0f)
		{
			return 0f;
		}
		float num = rect.Width / sizeF.Width;
		float num2 = rect.Height / sizeF.Height;
		return (num > num2) ? num2 : num;
	}

	internal static float GetFontSize(StiContext context, Font font, RectangleF rect, string text)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		Font font2 = new Font(font.FontFamily, 1f);
		SizeF sizeF = context.MeasureString(text, new StiFontGeom(font2));
		if (sizeF.Width == 0f || sizeF.Height == 0f)
		{
			return 0f;
		}
		float num = rect.Width / sizeF.Width;
		float num2 = rect.Height / sizeF.Height;
		return (num > num2) ? num2 : num;
	}
}
