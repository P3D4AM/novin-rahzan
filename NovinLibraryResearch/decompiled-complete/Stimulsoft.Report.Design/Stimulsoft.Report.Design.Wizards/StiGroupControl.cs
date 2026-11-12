using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiGroupControl : Control
{
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		StringFormat val = new StringFormat();
		try
		{
			val.LineAlignment = (StringAlignment)1;
			e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, Brushes.Black, (RectangleF)new Rectangle(((Control)this).ClientRectangle.X + 10, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height), val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Y = clientRectangle.Height - 1;
		clientRectangle.Height = 1;
		clientRectangle.Width = Math.Min(300, clientRectangle.Width);
		LinearGradientBrush val2 = new LinearGradientBrush(clientRectangle, Color.FromArgb(166, 202, 240), ((Control)this).BackColor, 0f);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val2, clientRectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public StiGroupControl()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)8192, false);
	}
}
