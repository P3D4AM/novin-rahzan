using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public class StiIconSetGdiIndicatorTypePainter : StiGdiIndicatorTypePainter
{
	public override RectangleD Paint(object context, StiComponent component, RectangleD rect)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Invalid comparison between Unknown and I4
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected I4, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Invalid comparison between Unknown and I4
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Invalid comparison between Unknown and I4
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)context;
		StiText stiText = (StiText)component;
		float num = (float)(component.Report.Info.Zoom * StiScale.Factor);
		RectangleD val2 = rect;
		if (num > 1f)
		{
			((RectangleD)(ref val2)).Inflate((double)(-1f * num), (double)(-1f * num));
		}
		else
		{
			((RectangleD)(ref val2)).Inflate(-1.0, -1.0);
		}
		if (stiText.Indicator is StiIconSetIndicator stiIconSetIndicator && (stiIconSetIndicator.Icon != 0 || stiIconSetIndicator.CustomIcon != null))
		{
			Image icon = StiIconSetHelper.GetIcon(stiIconSetIndicator);
			try
			{
				float num2 = (float)icon.Width * num;
				float num3 = (float)icon.Height * num;
				RectangleF rectangleF = ((RectangleD)(ref val2)).ToRectangleF();
				ContentAlignment alignment = stiIconSetIndicator.Alignment;
				if ((int)alignment <= 32)
				{
					switch (alignment - 1)
					{
					default:
						if ((int)alignment != 16)
						{
							if ((int)alignment == 32)
							{
								rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num2) / 2f, rectangleF.Y + (rectangleF.Height - num3) / 2f, num2, num3);
							}
						}
						else
						{
							rectangleF = new RectangleF(rectangleF.X, rectangleF.Y + (rectangleF.Height - num3) / 2f, num2, num3);
							((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + (double)(rectangleF.Width + 1f);
							((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
						}
						break;
					case 0:
						rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, num2, num3);
						((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + (double)(rectangleF.Width + 1f);
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
						break;
					case 1:
						rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num2) / 2f, rectangleF.Y, num2, num3);
						break;
					case 3:
						rectangleF = new RectangleF(rectangleF.Right - num2, rectangleF.Y, num2, num3);
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
						break;
					case 2:
						break;
					}
				}
				else if ((int)alignment <= 256)
				{
					if ((int)alignment != 64)
					{
						if ((int)alignment == 256)
						{
							rectangleF = new RectangleF(rectangleF.X, rectangleF.Bottom - num3, num2, num3);
							((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + (double)(rectangleF.Width + 1f);
							((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
						}
					}
					else
					{
						rectangleF = new RectangleF(rectangleF.Right - num2, rectangleF.Y + (rectangleF.Height - num3) / 2f, num2, num3);
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
					}
				}
				else if ((int)alignment != 512)
				{
					if ((int)alignment == 1024)
					{
						rectangleF = new RectangleF(rectangleF.Right - num2, rectangleF.Bottom - num3, num2, num3);
						((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (double)(rectangleF.Width + 1f);
					}
				}
				else
				{
					rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num2) / 2f, rectangleF.Bottom - num3, num2, num3);
				}
				GraphicsState val3 = val.Save();
				val.SmoothingMode = (SmoothingMode)3;
				val.DrawImage(icon, rectangleF);
				val.Restore(val3);
			}
			finally
			{
				((IDisposable)icon)?.Dispose();
			}
		}
		return rect;
	}
}
