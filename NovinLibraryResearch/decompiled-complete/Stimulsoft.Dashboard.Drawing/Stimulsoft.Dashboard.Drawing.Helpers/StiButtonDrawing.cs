using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

public static class StiButtonDrawing
{
	public static void PaintIcon(Graphics g, RectangleF rect, StiButtonVisualState state, StiFontIcons icon, bool isEnabled, float scale)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected I4, but got Unknown
		StiBrush brush = (isEnabled ? state.IconBrush : new StiSolidBrush(Color.LightGray));
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		if (!isEnabled)
		{
			brush = new StiSolidBrush(Color.LightGray);
		}
		Font val = new Font(fontFamilyIcons, (float)((double)(8f * scale) * StiScale.Factor));
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					string content = StiFontIconsHelper.GetContent(icon);
					SizeF sizeF = g.MeasureString(content, val);
					float num = Math.Min(sizeF.Width, sizeF.Height);
					float num2 = Math.Min(rect.Width, rect.Height);
					float num3 = ((num > num2) ? (num2 / num) : 1f) * 3f;
					val3.AddString(content, fontFamilyIcons, (int)val.Style, (float)((double)(8f * scale) * StiScale.Factor * (double)num3), new RectangleF(0f, 0f, rect.Width, rect.Height), val2);
					GraphicsState val4 = g.Save();
					try
					{
						g.SmoothingMode = (SmoothingMode)4;
						RectangleF bounds = val3.GetBounds();
						g.TranslateTransform(rect.X - bounds.X + (rect.Width - bounds.Width) / 2f, rect.Y - bounds.Y + (rect.Height - bounds.Height) / 2f);
						Brush brush2 = StiBrush.GetBrush(brush, bounds);
						try
						{
							g.FillPath(brush2, val3);
						}
						finally
						{
							((IDisposable)brush2)?.Dispose();
						}
					}
					finally
					{
						g.Restore(val4);
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void PaintText(Graphics g, RectangleF rect, StiButtonVisualState state, StiButtonElement button, float scale)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		string text = StiReportParser.Parse(button.Text, button, allowCache: false);
		float newFontSize = state.Font.Size * scale;
		bool flag = button.HorAlignment == StiTextHorAlignment.Width;
		bool flag2 = flag || button.WordWrap;
		Font val = StiFontUtils.ChangeFontSize(state.Font, newFontSize);
		try
		{
			StringFormat val2 = new StringFormat(StringFormat.GenericTypographic);
			try
			{
				Brush brush = StiBrush.GetBrush(state.TextBrush, rect);
				try
				{
					val2.Alignment = GetHorAlignment(button.HorAlignment);
					val2.LineAlignment = GetVertAlignment(button.VertAlignment);
					val2.FormatFlags = (StringFormatFlags)((!flag2) ? 4096 : 0);
					val2.Trimming = (StringTrimming)((!flag) ? 3 : 0);
					SizeF sizeF = g.MeasureString(text, val);
					if (sizeF.Width / 2f <= rect.Width && sizeF.Height <= rect.Height)
					{
						if (flag)
						{
							StiTextDrawing.DrawStringWidth(g, text, val, brush, RectangleD.CreateFromRectangle(rect), val2);
						}
						else
						{
							g.DrawString(text, val, brush, rect, val2);
						}
					}
				}
				finally
				{
					((IDisposable)brush)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static StringAlignment GetHorAlignment(StiTextHorAlignment horAlignment)
	{
		return (StringAlignment)(horAlignment switch
		{
			StiTextHorAlignment.Left => 0, 
			StiTextHorAlignment.Right => 2, 
			_ => 1, 
		});
	}

	private static StringAlignment GetVertAlignment(StiVertAlignment vertAlignment)
	{
		return (StringAlignment)(vertAlignment switch
		{
			StiVertAlignment.Top => 0, 
			StiVertAlignment.Bottom => 2, 
			_ => 1, 
		});
	}
}
