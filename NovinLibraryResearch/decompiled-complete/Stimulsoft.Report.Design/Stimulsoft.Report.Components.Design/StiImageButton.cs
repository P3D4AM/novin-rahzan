using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiImageButton : CheckBox
{
	private bool isMouseOver;

	private bool isPressed;

	[DefaultValue(true)]
	public bool ShowFooter { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Color color;
		Color color2;
		Color color3;
		if (isPressed)
		{
			color = (StiUX.IsDark ? StiUX.ItemBackgroundPressed : StiColorUtils.Dark(SystemColors.MenuHighlight, 50));
			color2 = (StiUX.IsDark ? StiUX.ItemBackgroundPressed : StiStyleOffice2013Blue.ColorButtonPressed);
			color3 = (StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.MenuHighlight);
		}
		else if (isMouseOver)
		{
			color = (StiUX.IsDark ? StiUX.ItemBackgroundHover : SystemColors.MenuHighlight);
			color2 = (StiUX.IsDark ? StiUX.ItemBackgroundHover : StiStyleOffice2013Blue.ColorButtonButtonMouseOver);
			color3 = (StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.MenuHighlight);
		}
		else if (((CheckBox)this).Checked)
		{
			color = (StiUX.IsDark ? StiUX.ItemBackgroundSelected : StiColorUtils.Dark(SystemColors.MenuHighlight, 25));
			color2 = (StiUX.IsDark ? StiUX.ItemBackgroundSelected : StiStyleOffice2013Blue.ColorButtonChecked);
			color3 = (StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.MenuHighlight);
		}
		else
		{
			color = (StiUX.IsDark ? StiUX.ItemBackground : SystemColors.ControlDark);
			color2 = (StiUX.IsDark ? StiUX.ItemBackground : SystemColors.ControlLight);
			color3 = (StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark);
		}
		SolidBrush val = new SolidBrush(color2);
		try
		{
			Pen val2 = new Pen(color3);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, 0, 0, rect.Width - 1, rect.Height - ((!ShowFooter) ? 1 : StiScale.YYI(20.0)));
				e.Graphics.DrawRectangle(val2, 0, 0, rect.Width - 1, rect.Height - 1);
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
		Rectangle rectangle = new Rectangle(1, rect.Height - StiScale.YYI(20.0), rect.Width - 2, StiScale.YYI(20.0) - 1);
		if (ShowFooter)
		{
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val3, rectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Font val4 = new Font("Arial", 8f);
			try
			{
				StringFormat val5 = new StringFormat
				{
					Alignment = (StringAlignment)1,
					LineAlignment = (StringAlignment)1,
					Trimming = (StringTrimming)3,
					FormatFlags = (StringFormatFlags)4096
				};
				try
				{
					rectangle.Inflate(-StiScale.I3, 0);
					e.Graphics.DrawString(((Control)this).Text, val4, SystemBrushes.Control, (RectangleF)rectangle, val5);
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
			rect.Height -= StiScale.YYI(20.0);
		}
		if (((ButtonBase)this).Image != null)
		{
			StiImageDialogPainter.DrawImage(e.Graphics, ((ButtonBase)this).Image, rect);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ButtonBase)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseDown(mevent);
		isPressed = true;
	}

	protected override void OnMouseUp(MouseEventArgs mevent)
	{
		((CheckBox)this).OnMouseUp(mevent);
		isPressed = false;
		((CheckBox)this).Checked = true;
	}

	public StiImageButton()
	{
		((CheckBox)this).Appearance = (Appearance)1;
		((Control)this).SetStyle((ControlStyles)4352, true);
		ShowFooter = true;
	}
}
