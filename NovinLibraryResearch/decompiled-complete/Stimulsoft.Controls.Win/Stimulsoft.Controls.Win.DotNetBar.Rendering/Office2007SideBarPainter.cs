using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007SideBarPainter : SideBarPainter, IOffice2007Painter
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

	public override void PaintSideBar(SideBarRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		SideBar sideBar = e.SideBar;
		Office2007SideBarColorTable sideBar2 = m_ColorTable.SideBar;
		LinearGradientColorTable background = sideBar2.Background;
		Rectangle r = new Rectangle(Point.Empty, ((Control)sideBar).Size);
		DisplayHelp.FillRectangle(graphics, r, background);
		Color border = sideBar2.Border;
		DisplayHelp.DrawRectangle(graphics, border, r);
		base.PaintSideBar(e);
	}

	public override void PaintSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		Graphics graphics = e.Graphics;
		SideBarPanelItem sideBarPanelItem = e.SideBarPanelItem;
		Office2007SideBarColorTable sideBar = m_ColorTable.SideBar;
		GradientColorTable gradientColorTable = (sideBarPanelItem.Expanded ? sideBar.SideBarPanelItemExpanded : sideBar.SideBarPanelItemDefault);
		if (sideBarPanelItem.IsMouseDown)
		{
			gradientColorTable = sideBar.SideBarPanelItemPressed;
		}
		else if (sideBarPanelItem.IsMouseOver)
		{
			gradientColorTable = sideBar.SideBarPanelItemMouseOver;
		}
		if (!sideBarPanelItem.BackgroundStyle.BackColor1.IsEmpty || sideBarPanelItem.BackgroundStyle.BackgroundImage != null)
		{
			sideBarPanelItem.BackgroundStyle.Paint(graphics, sideBarPanelItem.DisplayRectangle);
		}
		Rectangle panelRect = sideBarPanelItem.PanelRect;
		Rectangle displayRectangle = sideBarPanelItem.DisplayRectangle;
		panelRect.Offset(displayRectangle.Location);
		if (gradientColorTable != null)
		{
			Brush val = DisplayHelp.CreateBrush(panelRect, gradientColorTable);
			try
			{
				graphics.FillRectangle(val, panelRect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (sideBarPanelItem.Expanded)
		{
			DisplayHelp.DrawLine(graphics, panelRect.X, panelRect.Bottom, panelRect.Right, panelRect.Bottom, sideBar.Border, 1);
		}
		DisplayHelp.DrawLine(graphics, displayRectangle.X, displayRectangle.Bottom, displayRectangle.Right, displayRectangle.Bottom, sideBar.Border, 1);
		Font font = sideBarPanelItem.GetFont();
		CompositeImage image = sideBarPanelItem.GetImage();
		if (image != null)
		{
			panelRect.X += 2;
			panelRect.Width -= 2;
			image.DrawImage(graphics, new Rectangle(panelRect.X, panelRect.Y + (panelRect.Height - image.Height) / 2, image.Width, image.Height));
			panelRect.X += image.Width + 4;
			panelRect.Width -= image.Width + 8;
		}
		Rectangle rectangle = Rectangle.Empty;
		if (sideBarPanelItem.Text.Length > 0)
		{
			Color color = sideBar.SideBarPanelItemText;
			if (!sideBarPanelItem.ForeColor.IsEmpty)
			{
				color = sideBarPanelItem.ForeColor;
			}
			if (sideBarPanelItem.IsMouseOver && !sideBarPanelItem.HotForeColor.IsEmpty)
			{
				color = sideBarPanelItem.HotForeColor;
			}
			TextDrawing.DrawString(graphics, sideBarPanelItem.Text, font, color, panelRect, sideBarPanelItem.GetStringFormat());
			rectangle = panelRect;
		}
		if (sideBarPanelItem.Focused)
		{
			if (sideBarPanelItem.DesignMode)
			{
				Rectangle panelRect2 = sideBarPanelItem.PanelRect;
				panelRect2.Offset(sideBarPanelItem.DisplayRectangle.Location);
				panelRect2.Inflate(-2, -2);
				DesignTime.DrawDesignTimeSelection(graphics, panelRect2, e.ItemPaintArgs.Colors.ItemDesignTimeBorder);
			}
			else if (!rectangle.IsEmpty)
			{
				ControlPaint.DrawFocusRectangle(graphics, rectangle);
			}
		}
		base.PaintSideBarPanelItem(e);
	}
}
