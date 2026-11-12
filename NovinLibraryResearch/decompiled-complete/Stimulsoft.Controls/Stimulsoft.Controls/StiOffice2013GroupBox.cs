using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiOffice2013GroupBox), "Toolbox.StiGroupLine.bmp")]
public class StiOffice2013GroupBox : ScrollableControl
{
	private int headerHeight = 24;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return Color.White;
			}
			if (!IsDarkerBackground)
			{
				return StiUX.Background;
			}
			return StiUX.BackgroundDark;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

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

	[Browsable(true)]
	public int HeaderHeight
	{
		get
		{
			return headerHeight;
		}
		set
		{
			headerHeight = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	public bool DrawBorder { get; set; }

	[DefaultValue(false)]
	public bool SolidBorder { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDarkerBackground { get; set; }

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(5.0 * StiScaleUI.X);
		Graphics graphics = p.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Font val = new Font("Microsoft Sans Serif", 10f);
		try
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, clientRectangle);
				SizeF sizeF = graphics.MeasureString(((Control)this).Text, val);
				if (sizeF.Width > (float)(clientRectangle.Width - (int)(10.0 * StiScaleUI.X) - num))
				{
					sizeF.Width = clientRectangle.Width - (int)(10.0 * StiScaleUI.X) - num;
				}
				Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, (int)((double)headerHeight * StiScaleUI.X));
				SolidBrush val3 = new SolidBrush(StiUX.BackgroundLight);
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				if (((Control)this).Text.Length > 0)
				{
					rectangle.X += (int)(12.0 * StiScaleUI.X);
					rectangle.Width += (int)(5.0 * StiScaleUI.X);
					StringFormat val4 = new StringFormat();
					try
					{
						val4.FormatFlags = (StringFormatFlags)4096;
						val4.Trimming = (StringTrimming)3;
						val4.Alignment = (StringAlignment)0;
						val4.LineAlignment = (StringAlignment)1;
						val4.HotkeyPrefix = (HotkeyPrefix)1;
						if ((int)((Control)this).RightToLeft == 1)
						{
							val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 1);
						}
						SolidBrush val5 = new SolidBrush(StiUX.Foreground);
						try
						{
							graphics.DrawString(((Control)this).Text, val, (Brush)(object)val5, (RectangleF)rectangle, val4);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
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
		if (!DrawBorder)
		{
			return;
		}
		Pen val6 = new Pen(StiUX.ButtonBorder);
		try
		{
			if (!SolidBorder)
			{
				val6.DashPattern = new float[2] { 1f, 2f };
			}
			graphics.DrawLine(val6, 0, HeaderHeight, 0, ((Control)this).Height - 1);
			graphics.DrawLine(val6, ((Control)this).Width - 1, HeaderHeight, ((Control)this).Width - 1, ((Control)this).Height - 1);
			graphics.DrawLine(val6, 0, ((Control)this).Height - 1, ((Control)this).Width - 1, ((Control)this).Height - 1);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
	}

	public StiOffice2013GroupBox()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
	}
}
