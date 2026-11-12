using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public class StiTabulatorStyleOffice2013Blue : IStiTabulatorStyle
{
	protected StiTabulator tabulator;

	private void DrawPageTitleLeftHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, Rectangle rect, Rectangle contentRect)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		if (page != tabulator.SelectedTab)
		{
			int num = 4;
			rect.X += num;
			rect.Width -= num;
			contentRect.X += num;
			contentRect.Width -= num;
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

	private void DrawPageTitleTopHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, Rectangle rect, Rectangle contentRect)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		Point[] array = new Point[4]
		{
			new Point(rect.X, rect.Y),
			new Point(rect.X, rect.Bottom),
			new Point(rect.Right + 5, rect.Bottom + 1),
			new Point(rect.Right - 5, rect.Y)
		};
		GraphicsPath val = new GraphicsPath((FillMode)0);
		try
		{
			val.AddPolygon(array);
			if (tabulator.SelectedTab == page)
			{
				SolidBrush val2 = new SolidBrush(((Control)tabulator).BackColor);
				try
				{
					g.FillPath((Brush)(object)val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (page.IsMouseOver)
			{
				SolidBrush val3 = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0));
				try
				{
					g.FillPath((Brush)(object)val3, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			else
			{
				SolidBrush val4 = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 10));
				try
				{
					g.FillPath((Brush)(object)val4, val);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			if (tabulator.SelectedTab == page)
			{
				g.DrawLine(penDark, array[0], array[1]);
				g.DrawLine(penLight, array[0].X + 1, array[0].Y + 1, array[1].X + 1, array[1].Y + 1);
				g.DrawLine(penDark, array[2], array[3]);
				g.DrawLine(penDark, array[0], array[3]);
				g.DrawLine(penLight, array[0].X + 1, array[0].Y + 1, array[3].X - 1, array[3].Y + 1);
			}
			else
			{
				g.DrawLine(penDark, array[0], array[1]);
				g.DrawLine(penDark, array[0], array[3]);
				g.DrawLine(penDark, array[2], array[3]);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		tabulator.Mode.DrawTitleText(g, rect, page, page == tabulator.SelectedTab);
		tabulator.Mode.DrawTitleImage(g, rect, page);
	}

	private void DrawPageTitleRightHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, Rectangle rect, Rectangle contentRect)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (page != tabulator.SelectedTab)
		{
			if (page.IsMouseOver)
			{
				SolidBrush val = new SolidBrush(StiStyleOffice2013Blue.ColorButtonButtonMouseOver);
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
				SolidBrush val2 = new SolidBrush(Color.White);
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
			SolidBrush val3 = new SolidBrush(StiStyleOffice2013Blue.ColorButtonChecked);
			try
			{
				g.FillRectangle((Brush)(object)val3, contentRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		tabulator.Mode.DrawTitleText(g, rect, page, page == tabulator.SelectedTab);
		tabulator.Mode.DrawTitleImage(g, rect, page);
	}

	public void DrawPageTitle(Graphics g, StiTabTitlePosition position, StiTabulatorPage page)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		Rectangle titlePageRectangle = tabulator.GetTitlePageRectangle(g, page);
		if (titlePageRectangle.Width == 0 || titlePageRectangle.Height == 0)
		{
			return;
		}
		Pen val = new Pen(StiColorUtils.Light(SystemColors.Control, 30));
		try
		{
			Pen val2 = new Pen(StiColorUtils.Dark(SystemColors.Control, 50));
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
				{
					Rectangle contentRect = new Rectangle(titlePageRectangle.X + 1, titlePageRectangle.Y + 1, titlePageRectangle.Width - 1, titlePageRectangle.Height - 1);
					DrawPageTitleTopHorizontal(g, val, val2, page, titlePageRectangle, contentRect);
					break;
				}
				case StiTabTitlePosition.RightHorizontal:
				{
					Rectangle contentRect = new Rectangle(titlePageRectangle.X, titlePageRectangle.Y + 3, titlePageRectangle.Width, titlePageRectangle.Height);
					DrawPageTitleRightHorizontal(g, val, val2, page, titlePageRectangle, contentRect);
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
		g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
		g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.Right - 2, rect.Y + 1);
		g.DrawLine(penDark, rect.Right, rect.Top + 1, rect.Right, rect.Bottom - 1);
		g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
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
		g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
		g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 2);
		g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
		g.DrawLine(penDark, rect.Right, rect.Y + 1, rect.Right, rect.Bottom - 1);
		if (((StiTabulator)(object)((Control)page).Parent).ShowTitle)
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, pageRect.X - 1, rect.Y + 1);
			g.DrawLine(penDark, rect.X + 1, rect.Y, pageRect.X, rect.Y);
			g.DrawLine(penLight, pageRect.X + pageRect.Width + 5, rect.Y + 1, rect.Right - 2, rect.Y + 1);
			g.DrawLine(penDark, pageRect.X + pageRect.Width + 5, rect.Y, rect.Right - 1, rect.Y);
		}
		else
		{
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.Right - 2, rect.Y + 1);
			g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
		}
	}

	private void DrawPageRightHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark, Rectangle rect, Rectangle pageRect)
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		Rectangle rect = new Rectangle(0, 0, ((Control)page).Width - 1, ((Control)page).Height - 1);
		Rectangle titlePageRectangle = ((StiTabulator)(object)((Control)page).Parent).GetTitlePageRectangle(g, page);
		Pen val = new Pen(StiColorUtils.Light(SystemColors.Control, 30));
		try
		{
			Pen val2 = new Pen(StiColorUtils.Dark(SystemColors.Control, 50));
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
					DrawPageRightHorizontal(g, page, val, val2, rect, titlePageRectangle);
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

	public StiTabulatorStyleOffice2013Blue(StiTabulator tabulator)
	{
		this.tabulator = tabulator;
	}
}
