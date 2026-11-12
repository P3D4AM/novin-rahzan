using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiGroupLine), "Toolbox.StiGroupLine.bmp")]
public class StiGroupLine : ScrollableControl
{
	[Browsable(true)]
	private StiLineStyle lineStyle = StiLineStyle.Line;

	[Browsable(true)]
	private StiLinePosition linePosition;

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
			((Control)this).Invalidate();
		}
	}

	public StiLineStyle LineStyle
	{
		get
		{
			return lineStyle;
		}
		set
		{
			lineStyle = value;
			((Control)this).Invalidate();
		}
	}

	public StiLinePosition LinePosition
	{
		get
		{
			return linePosition;
		}
		set
		{
			linePosition = value;
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Invalid comparison between Unknown and I4
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Invalid comparison between Unknown and I4
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		int i = StiScaleUI.I5;
		Graphics graphics = p.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Color color = (StiUX.IsDark ? StiUX.Separator : StiColorUtils.GetDisabledColor((Control)(object)this));
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, clientRectangle);
			SizeF sizeF = graphics.MeasureString(((Control)this).Text, ((Control)this).Font);
			if (sizeF.Width > (float)(clientRectangle.Width - StiScaleUI.I10 - i))
			{
				sizeF.Width = clientRectangle.Width - StiScaleUI.I10 - i;
			}
			int x = clientRectangle.X + i + StiScaleUI.I4;
			int num = clientRectangle.X + i + (int)sizeF.Width + StiScaleUI.I8;
			if (((Control)this).Text.Length == 0)
			{
				x = num;
			}
			if ((int)((Control)this).RightToLeft == 1)
			{
				x = clientRectangle.Right - i - (int)sizeF.Width - StiScaleUI.I8;
				num = clientRectangle.Right - i - StiScaleUI.I4;
			}
			int y = clientRectangle.Y + (int)(sizeF.Height / 2f);
			if (linePosition != StiLinePosition.None)
			{
				Color color2 = (StiUX.IsDark ? color : ControlPaint.Light(color, 1f));
				Color color3 = (StiUX.IsDark ? color : ControlPaint.Dark(color, 0f));
				Pen val2 = new Pen(color2);
				try
				{
					Pen val3 = new Pen(color3);
					try
					{
						if (linePosition == StiLinePosition.Top)
						{
							if (lineStyle == StiLineStyle.Dot)
							{
								if (!StiUX.IsDark)
								{
									val3.DashPattern = new float[2] { 1f, 2f };
								}
								Point point = new Point(clientRectangle.X, y);
								Point point2 = new Point(x, y);
								Point point3 = new Point(num, y);
								Point point4 = new Point(clientRectangle.Right, y);
								if (string.IsNullOrEmpty(((Control)this).Text))
								{
									graphics.DrawLine(val3, point, point4);
								}
								else
								{
									graphics.DrawLine(val3, point, point2);
									graphics.DrawLine(val3, point3, point4);
								}
							}
							else
							{
								Point point5 = new Point(clientRectangle.X, y);
								Point point6 = new Point(x, y);
								Point point7 = new Point(num, y);
								Point point8 = new Point(clientRectangle.Right, y);
								graphics.DrawLine(val3, point5, point6);
								graphics.DrawLine(val3, point7, point8);
								point5.Y++;
								point6.Y++;
								point7.Y++;
								point8.Y++;
								graphics.DrawLine(val2, point5, point6);
								graphics.DrawLine(val2, point7, point8);
							}
						}
						else if (linePosition == StiLinePosition.Bottom)
						{
							if (lineStyle == StiLineStyle.Dot)
							{
								if (!StiUX.IsDark)
								{
									val3.DashPattern = new float[2] { 1f, 2f };
								}
								Point point9 = new Point(clientRectangle.X, clientRectangle.Bottom - 1);
								Point point10 = new Point(clientRectangle.Right, clientRectangle.Bottom - 1);
								graphics.DrawLine(val3, point9, point10);
							}
						}
						else if (linePosition == StiLinePosition.Center)
						{
							if (lineStyle == StiLineStyle.Dot)
							{
								if (!StiUX.IsDark)
								{
									val3.DashPattern = new float[2] { 1f, 2f };
								}
								Point point11 = new Point(clientRectangle.X, clientRectangle.Y + clientRectangle.Height / 2 - 1);
								Point point12 = new Point(clientRectangle.Right, clientRectangle.Y + clientRectangle.Height / 2 - 1);
								graphics.DrawLine(val3, point11, point12);
							}
						}
						else if (linePosition == StiLinePosition.Right && lineStyle == StiLineStyle.Dot)
						{
							if (!StiUX.IsDark)
							{
								val3.DashPattern = new float[2] { 1f, 2f };
							}
							Point point13 = new Point(clientRectangle.Right - 1, clientRectangle.Y);
							Point point14 = new Point(clientRectangle.Right - 1, clientRectangle.Bottom - 1);
							graphics.DrawLine(val3, point13, point14);
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
			if (((Control)this).Text.Length <= 0)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(x, 0, (int)sizeF.Width + 5, 16);
			StringFormat val4 = new StringFormat();
			try
			{
				val4.FormatFlags = (StringFormatFlags)4096;
				val4.Trimming = (StringTrimming)3;
				val4.Alignment = (StringAlignment)1;
				val4.HotkeyPrefix = (HotkeyPrefix)1;
				if ((int)((Control)this).RightToLeft == 1)
				{
					val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 1);
				}
				if (((Control)this).Enabled)
				{
					SolidBrush val5 = new SolidBrush(((Control)this).ForeColor);
					try
					{
						graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val5, (RectangleF)rectangle, val4);
						return;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				ControlPaint.DrawStringDisabled(graphics, ((Control)this).Text, ((Control)this).Font, color, (RectangleF)rectangle, val4);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiGroupLine()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
	}
}
