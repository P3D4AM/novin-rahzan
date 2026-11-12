using System;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007NavigationPanePainter : NavigationPanePainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void PaintButtonBackground(NavPaneRenderEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Office2007NavigationPaneColorTable navigationPane = m_ColorTable.NavigationPane;
		Brush val = DisplayHelp.CreateBrush(bounds, navigationPane.ButtonBackground);
		try
		{
			graphics.FillRectangle(val, bounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
