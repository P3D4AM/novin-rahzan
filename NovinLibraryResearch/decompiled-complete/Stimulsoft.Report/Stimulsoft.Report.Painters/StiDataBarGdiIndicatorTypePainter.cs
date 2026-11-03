using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public class StiDataBarGdiIndicatorTypePainter : StiGdiIndicatorTypePainter
{
	public override RectangleD Paint(object context, StiComponent component, RectangleD rect)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Expected O, but got Unknown
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)context;
		StiText stiText = (StiText)component;
		float num = (float)component.Report.Info.Zoom;
		RectangleD val2 = rect;
		if (num > 1f)
		{
			((RectangleD)(ref val2)).Inflate((double)(-2f * num), (double)(-2f * num));
		}
		else
		{
			((RectangleD)(ref val2)).Inflate(-2.0, -2.0);
		}
		if (!(stiText.Indicator is StiDataBarIndicator { Value: not 0f } stiDataBarIndicator))
		{
			return rect;
		}
		float num2 = stiDataBarIndicator.Maximum + Math.Abs(stiDataBarIndicator.Minimum);
		RectangleF rectangleF = ((RectangleD)(ref val2)).ToRectangleF();
		float num3 = rectangleF.Width * Math.Abs(stiDataBarIndicator.Minimum) / num2;
		float num4 = rectangleF.Width * stiDataBarIndicator.Maximum / num2;
		float num5 = rectangleF.Width * Math.Abs(stiDataBarIndicator.Value) / num2;
		StiDataBarDirection stiDataBarDirection = stiDataBarIndicator.Direction;
		if (stiDataBarDirection == StiDataBarDirection.Default)
		{
			stiDataBarDirection = ((stiText.TextOptions == null || !stiText.TextOptions.RightToLeft) ? StiDataBarDirection.LeftToRight : StiDataBarDirection.RighToLeft);
		}
		float num6 = 0f;
		if (stiDataBarDirection == StiDataBarDirection.LeftToRight)
		{
			if (stiDataBarIndicator.Value < 0f)
			{
				rectangleF.X += num3 - num5;
			}
			else
			{
				rectangleF.X += num3;
			}
			rectangleF.Width = num5;
		}
		else
		{
			num6 = 180f;
			if (stiDataBarIndicator.Value < 0f)
			{
				rectangleF.X = rectangleF.Right - num3;
			}
			else
			{
				rectangleF.X = rectangleF.Right - num3 - num5;
			}
			rectangleF.Width = num5;
		}
		if (stiDataBarIndicator.Value < 0f)
		{
			num6 += 180f;
		}
		if (rectangleF.Width > 0f && rectangleF.Height > 0f)
		{
			if (stiDataBarIndicator.BrushType == Stimulsoft.Report.Components.StiBrushType.Gradient)
			{
				RectangleD val3 = default(RectangleD);
				if (stiDataBarDirection == StiDataBarDirection.LeftToRight)
				{
					if (stiDataBarIndicator.Value > 0f)
					{
						((RectangleD)(ref val3))._002Ector(((RectangleD)(ref rect)).Left + (double)num3, ((RectangleD)(ref rect)).Top, (double)num4, ((RectangleD)(ref rect)).Height);
					}
					else
					{
						((RectangleD)(ref val3))._002Ector(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, (double)num3, ((RectangleD)(ref rect)).Height);
					}
				}
				else if (stiDataBarIndicator.Value < 0f)
				{
					((RectangleD)(ref val3))._002Ector(((RectangleD)(ref rect)).Left + (double)num4, ((RectangleD)(ref rect)).Top, (double)num3, ((RectangleD)(ref rect)).Height);
				}
				else
				{
					((RectangleD)(ref val3))._002Ector(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, (double)num4, ((RectangleD)(ref rect)).Height);
				}
				if (rectangleF.Width > 0f && rectangleF.Width < 1f)
				{
					rectangleF.Width = 1f;
				}
				if (((RectangleD)(ref val3)).Width > 0.0 && ((RectangleD)(ref val3)).Width < 1.0)
				{
					((RectangleD)(ref val3)).Width = 1.0;
				}
				if (((RectangleD)(ref val3)).Width > 0.0 && rectangleF.Width > 0f)
				{
					Color color = ((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeColor : stiDataBarIndicator.PositiveColor);
					Color color2 = StiColorUtils.Light(color, (byte)200);
					((RectangleD)(ref val3)).X = ((RectangleD)(ref val3)).X - ((RectangleD)(ref val3)).Width * 0.1;
					((RectangleD)(ref val3)).Width = ((RectangleD)(ref val3)).Width + ((RectangleD)(ref val3)).Width * 0.2;
					LinearGradientBrush val4 = new LinearGradientBrush(((RectangleD)(ref val3)).ToRectangleF(), color, color2, num6);
					try
					{
						val.FillRectangle((Brush)(object)val4, rectangleF);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
			else
			{
				SolidBrush val5 = new SolidBrush((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeColor : stiDataBarIndicator.PositiveColor);
				try
				{
					val.FillRectangle((Brush)(object)val5, rectangleF);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			if (stiDataBarIndicator.ShowBorder)
			{
				Pen val6 = new Pen((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeBorderColor : stiDataBarIndicator.PositiveBorderColor);
				try
				{
					if (num > 1f)
					{
						val6.Width *= num;
					}
					val.DrawRectangle(val6, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
		}
		return rect;
	}
}
