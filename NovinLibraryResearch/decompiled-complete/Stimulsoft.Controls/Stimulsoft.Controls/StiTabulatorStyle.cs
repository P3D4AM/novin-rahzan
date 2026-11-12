using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public class StiTabulatorStyle : IStiTabulatorStyle
{
	protected StiTabulator tabulator;

	private void DrawPageTitleLeftHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, Rectangle rect, Rectangle contentRect)
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		if (page != tabulator.SelectedTab)
		{
			rect.X += 4;
			rect.Width -= 4;
			contentRect.X += 4;
			contentRect.Width -= 4;
		}
		g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right, rect.Y);
		g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
		g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right, rect.Bottom);
		if (page != tabulator.SelectedTab)
		{
			if (page.IsMouseOver)
			{
				SolidBrush val = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0));
				try
				{
					g.FillRectangle((Brush)(object)val, contentRect);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 10));
				try
				{
					g.FillRectangle((Brush)(object)val2, contentRect);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val3 = new SolidBrush(((Control)tabulator).BackColor);
			try
			{
				g.FillRectangle((Brush)(object)val3, contentRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			g.DrawLine(penLight, rect.X + 2, rect.Y + 1, rect.Right, rect.Y + 1);
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 1);
		}
		tabulator.Mode.DrawTitleText(g, rect, page, page == tabulator.SelectedTab);
		tabulator.Mode.DrawTitleImage(g, rect, page);
	}

	private void DrawPageTitleTopHorizontal(Graphics g, Pen penDark, StiTabulatorPage page, Rectangle rect)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		if (tabulator.SelectedTab == page)
		{
			g.DrawLine(penDark, new Point(rect.X, rect.Y), new Point(rect.X, rect.Bottom));
			g.DrawLine(penDark, new Point(rect.Right, rect.Y), new Point(rect.Right, rect.Bottom));
			SolidBrush val = new SolidBrush(Color.FromArgb(43, 86, 154));
			try
			{
				g.FillRectangle((Brush)(object)val, rect.X, rect.Y, rect.Width + 1, StiScaleUI.I2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		tabulator.Mode.DrawTitleText(g, rect, page, page == tabulator.SelectedTab);
		tabulator.Mode.DrawTitleImage(g, rect, page);
	}

	private void DrawPageTitleRightHorizontal(Graphics g, StiTabulatorPage page, Rectangle rect, Rectangle contentRect)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (page != tabulator.SelectedTab)
		{
			if (page.IsMouseOver)
			{
				SolidBrush val = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0));
				try
				{
					g.FillRectangle((Brush)(object)val, contentRect);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val2 = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0));
			try
			{
				g.FillRectangle((Brush)(object)val2, contentRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		tabulator.Mode.DrawTitleText(g, rect, page, page == tabulator.SelectedTab);
		tabulator.Mode.DrawTitleImage(g, rect, page);
	}

	public void DrawPageTitle(Graphics g, StiTabTitlePosition position, StiTabulatorPage page)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		Rectangle titlePageRectangle = tabulator.GetTitlePageRectangle(g, page);
		if (titlePageRectangle.Width == 0 || titlePageRectangle.Height == 0)
		{
			return;
		}
		Pen val = new Pen(StiUX.IsDark ? StiUX.Separator : StiColorUtils.Light(SystemColors.Control, 30));
		try
		{
			Pen val2 = new Pen(StiUX.IsDark ? StiUX.Separator : StiColorUtils.Dark(SystemColors.Control, 50));
			try
			{
				switch (position)
				{
				case StiTabTitlePosition.LeftHorizontal:
				{
					Rectangle contentRect = new Rectangle(titlePageRectangle.X + 1, titlePageRectangle.Y + 1, titlePageRectangle.Width - 1, titlePageRectangle.Height - 1);
					DrawPageTitleLeftHorizontal(g, val, val2, page, titlePageRectangle, contentRect);
					break;
				}
				case StiTabTitlePosition.TopHorizontal:
					DrawPageTitleTopHorizontal(g, val2, page, titlePageRectangle);
					break;
				case StiTabTitlePosition.RightHorizontal:
				{
					Rectangle contentRect = new Rectangle(titlePageRectangle.X, titlePageRectangle.Y + 3, titlePageRectangle.Width, titlePageRectangle.Height);
					DrawPageTitleRightHorizontal(g, page, titlePageRectangle, contentRect);
					break;
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
	}

	private void DrawPageLeftHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark, Rectangle rect, Rectangle pageRect)
	{
		g.DrawLine(penDark, rect.X, rect.Y, rect.Right, rect.Y);
		g.DrawLine(penDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
		g.DrawLine(penDark, rect.X, rect.Bottom, rect.Right, rect.Bottom);
		if (((StiTabulator)(object)((Control)page).Parent).ShowTitle)
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, pageRect.Y - 1);
			g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, pageRect.Y);
			g.DrawLine(penLight, rect.X + 1, pageRect.Y + pageRect.Height, rect.X + 1, rect.Bottom - 2);
			g.DrawLine(penDark, rect.X, pageRect.Y + pageRect.Height, rect.X, rect.Bottom - 1);
		}
		else
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 2);
			g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
		}
	}

	private void DrawPageTopHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark, Rectangle rect, Rectangle pageRect)
	{
		g.DrawLine(penDark, rect.X, rect.Y, rect.X, rect.Bottom);
		g.DrawLine(penDark, rect.X, rect.Bottom, rect.Right, rect.Bottom);
		g.DrawLine(penDark, rect.Right, rect.Y, rect.Right, rect.Bottom);
		if (((StiTabulator)(object)((Control)page).Parent).ShowTitle)
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, pageRect.X - 1, rect.Y + 1);
			g.DrawLine(penDark, rect.X + 1, rect.Y, pageRect.X, rect.Y);
			g.DrawLine(penLight, pageRect.X + pageRect.Width, rect.Y + 1, rect.Right - 2, rect.Y + 1);
			g.DrawLine(penDark, pageRect.X + pageRect.Width, rect.Y, rect.Right - 1, rect.Y);
		}
		else
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.Right - 2, rect.Y + 1);
			g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
		}
	}

	private void DrawPageRightHorizontal(Graphics g, StiTabulatorPage page, Pen penDark, Rectangle rect)
	{
		if (((StiTabulator)(object)((Control)page).Parent).ShowTitle)
		{
			penDark.Color = Color.FromArgb(198, 198, 198);
			penDark.DashPattern = new float[2] { 1f, 2f };
			g.DrawLine(penDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
		}
	}

	public void DrawPage(Graphics g, StiTabTitlePosition position, StiTabulatorPage page)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		Rectangle rect = new Rectangle(0, 0, ((Control)page).Width - 1, ((Control)page).Height - 1);
		Rectangle titlePageRectangle = ((StiTabulator)(object)((Control)page).Parent).GetTitlePageRectangle(g, page);
		Pen val = new Pen(StiUX.IsDark ? StiUX.Separator : StiColorUtils.Light(SystemColors.Control, 30));
		try
		{
			Pen val2 = new Pen(StiUX.IsDark ? StiUX.Separator : StiColorUtils.Dark(SystemColors.Control, 50));
			try
			{
				DrawDot(g, 0, 0);
				DrawDot(g, rect.Right, 0);
				DrawDot(g, rect.Right, rect.Bottom);
				DrawDot(g, 0, rect.Bottom);
				switch (position)
				{
				case StiTabTitlePosition.LeftHorizontal:
					DrawPageLeftHorizontal(g, page, val, val2, rect, titlePageRectangle);
					break;
				case StiTabTitlePosition.TopHorizontal:
					DrawPageTopHorizontal(g, page, val, val2, rect, titlePageRectangle);
					break;
				case StiTabTitlePosition.RightHorizontal:
					DrawPageRightHorizontal(g, page, val2, rect);
					break;
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

	internal void DrawDot(Graphics g, int x, int y)
	{
		g.FillRectangle(SystemBrushes.Control, x, y, 1, 1);
	}

	public StiTabulatorStyle(StiTabulator tabulator)
	{
		this.tabulator = tabulator;
	}
}
