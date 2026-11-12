using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickerItem;

[ToolboxItem(false)]
internal class ColorSelectionPreviewControl : UserControl
{
	private Color m_CurrentColor = Color.Black;

	private Color m_NewColor = Color.Empty;

	public Color NewColor
	{
		get
		{
			return m_NewColor;
		}
		set
		{
			m_NewColor = value;
			((Control)this).Invalidate();
		}
	}

	public Color CurrentColor
	{
		get
		{
			return m_CurrentColor;
		}
		set
		{
			m_CurrentColor = value;
			((Control)this).Invalidate();
		}
	}

	public ColorSelectionPreviewControl()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)16, true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		if (((Control)this).BackColor == Color.Transparent)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ColorSelectionPreviewControl));
		Image val = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
		TextureBrush val2 = new TextureBrush(val);
		graphics.FillRectangle((Brush)(object)val2, clientRectangle);
		if (!m_NewColor.IsEmpty)
		{
			SolidBrush val3 = new SolidBrush(m_NewColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val3, clientRectangle.X, clientRectangle.Y, clientRectangle.Width / 2, clientRectangle.Height);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (!m_CurrentColor.IsEmpty)
		{
			SolidBrush val4 = new SolidBrush(m_CurrentColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, clientRectangle.X + clientRectangle.Width / 2, clientRectangle.Y, clientRectangle.Width / 2, clientRectangle.Height);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		Pen val5 = new Pen(Color.Black);
		try
		{
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(val5, clientRectangle);
			graphics.DrawLine(val5, clientRectangle.X, clientRectangle.Y, clientRectangle.X + clientRectangle.Width / 2, clientRectangle.Y);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}
}
