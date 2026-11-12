using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiButton), "Toolbox.StiPanel.bmp")]
public class StiPanel : Panel
{
	private StiBorderStyle borderStyle;

	[Category("Appearance")]
	[DefaultValue(StiBorderStyle.None)]
	public virtual StiBorderStyle BorderStyle
	{
		get
		{
			return borderStyle;
		}
		set
		{
			borderStyle = value;
			((Control)this).Invalidate();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (BorderStyle != 0)
		{
			ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)BorderStyle);
		}
	}

	public StiPanel()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)8192, false);
	}
}
