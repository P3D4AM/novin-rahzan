using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiTabControl.bmp")]
[Designer(typeof(StiTabControlDesigner))]
public class StiTabControl : Panel
{
	private int[] widthPages;

	private StringFormat sfTabs;

	private int titleHeight = 22;

	private bool locked;

	private bool positionAtBottom;

	private ImageList imageList;

	private StiTabPage selectedTab;

	private StiControlStyle controlStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	public Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(false)]
	public Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).BorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).BorderStyle = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public virtual bool PositionAtBottom
	{
		get
		{
			return positionAtBottom;
		}
		set
		{
			positionAtBottom = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(null)]
	public virtual ImageList ImageList
	{
		get
		{
			return imageList;
		}
		set
		{
			imageList = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiTabPage SelectedTab
	{
		get
		{
			return selectedTab;
		}
		set
		{
			foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (item == value)
				{
					item.Dock = (DockStyle)5;
					((Control)item).Show();
				}
			}
			foreach (StiTabPage item2 in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (item2 != value)
				{
					((Control)item2).Hide();
				}
			}
			selectedTab = value;
			((Control)this).Invalidate();
			InvokeSelectedIndexChanged(this, EventArgs.Empty);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			return ((Control)this).Controls.IndexOf((Control)(object)SelectedTab);
		}
		set
		{
			if (value >= 0 && value < ((ArrangedElementCollection)((Control)this).Controls).Count)
			{
				SelectedTab = ((Control)this).Controls[value] as StiTabPage;
			}
		}
	}

	[DefaultValue(StiControlStyle.Flat)]
	public StiControlStyle ControlStyle
	{
		get
		{
			return controlStyle;
		}
		set
		{
			controlStyle = value;
		}
	}

	[Category("Behavior")]
	public event EventHandler TitleClick;

	[Category("Behavior")]
	public event EventHandler SelectedIndexChanged;

	protected virtual void OnTitleClick(EventArgs e)
	{
	}

	public virtual void InvokeTitleClick(object sender, EventArgs e)
	{
		try
		{
			OnTitleClick(EventArgs.Empty);
			if (this.TitleClick != null)
			{
				this.TitleClick(null, EventArgs.Empty);
			}
		}
		catch
		{
		}
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
	}

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			OnSelectedIndexChanged(EventArgs.Empty);
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(sender, e);
			}
		}
		catch
		{
		}
	}

	public void LockRefresh()
	{
		locked = true;
	}

	public void UnlockRefresh()
	{
		locked = false;
	}

	private void CalculatePagesWidth(Graphics g)
	{
		widthPages = new int[((ArrangedElementCollection)((Control)this).Controls).Count];
		int num = 0;
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			widthPages[num++] = GetPageWidth(g, item);
		}
	}

	private int GetPageWidth(Graphics g, StiTabPage page)
	{
		int num = 0;
		if (ImageList != null && ImageList.Images.Count > ((Control)this).Controls.IndexOf((Control)(object)page))
		{
			num = ImageList.ImageSize.Width;
		}
		if (page.Image != null)
		{
			num = page.Image.Width;
		}
		return (int)g.MeasureString(((Control)page).Text, ((Control)this).Font, 1000, sfTabs).Width + 10 + num;
	}

	private int GetPagesWidth(Graphics g)
	{
		int num = 0;
		int num2 = 0;
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (!item.Invisible || ((Component)this).DesignMode)
			{
				num += widthPages[num2];
			}
			num2++;
		}
		return num;
	}

	private int GetPanelStartPos()
	{
		return 5;
	}

	private Rectangle GetPageRectangle(Graphics g, StiTabPage page)
	{
		int num = GetPanelStartPos();
		int width = 10;
		int pagesWidth = GetPagesWidth(g);
		double num2 = 1.0;
		if (pagesWidth + num * 2 + 5 > ((Control)this).Width)
		{
			num2 = (double)((Control)this).Width / ((double)pagesWidth + (double)num * 2.0 + 5.0);
		}
		int num3 = 0;
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (!item.Invisible || ((Component)this).DesignMode)
			{
				if (item == page)
				{
					width = (int)((double)widthPages[num3] * num2);
					break;
				}
				num += (int)((double)widthPages[num3] * num2);
			}
			num3++;
		}
		Rectangle empty = Rectangle.Empty;
		return (!PositionAtBottom) ? new Rectangle(num, 2, width, titleHeight - 2) : new Rectangle(num, ((Control)this).Height - titleHeight - 1, width, titleHeight - 2);
	}

	public StiTabPage GetTabPageAtPoint(Point p)
	{
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if ((!item.Invisible || ((Component)this).DesignMode) && GetPageRectangle(val, item).Contains(p))
				{
					return item;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return null;
	}

	protected virtual void SetLayout()
	{
		DockPaddingEdges dockPadding = DockPadding;
		int left = (DockPadding.Right = 0);
		dockPadding.Left = left;
		if (PositionAtBottom)
		{
			DockPadding.Top = 0;
			DockPadding.Bottom = titleHeight;
		}
		else
		{
			DockPadding.Top = titleHeight;
			DockPadding.Bottom = 0;
		}
	}

	private void DrawPage(Graphics g, StiTabPage page)
	{
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Expected O, but got Unknown
		if (page.Invisible && !((Component)this).DesignMode)
		{
			return;
		}
		Rectangle pageRectangle = GetPageRectangle(g, page);
		if (pageRectangle.Width == 0 || pageRectangle.Height == 0)
		{
			return;
		}
		Point[] array = new Point[0];
		array = ((ControlStyle == StiControlStyle.Office2013Blue) ? new Point[4]
		{
			new Point(pageRectangle.X, pageRectangle.Y),
			new Point(pageRectangle.X, pageRectangle.Bottom),
			new Point(pageRectangle.Right, pageRectangle.Bottom),
			new Point(pageRectangle.Right, pageRectangle.Y)
		} : (PositionAtBottom ? new Point[4]
		{
			new Point(pageRectangle.X, pageRectangle.Y),
			new Point(pageRectangle.X, pageRectangle.Bottom),
			new Point(pageRectangle.Right - 5, pageRectangle.Bottom),
			new Point(pageRectangle.Right + 5, pageRectangle.Y)
		} : new Point[4]
		{
			new Point(pageRectangle.X, pageRectangle.Y),
			new Point(pageRectangle.X, pageRectangle.Bottom),
			new Point(pageRectangle.Right + 5, pageRectangle.Bottom),
			new Point(pageRectangle.Right - 5, pageRectangle.Y)
		}));
		GraphicsPath val = new GraphicsPath((FillMode)0);
		try
		{
			val.AddPolygon(array);
			if (ControlStyle == StiControlStyle.Office2013Blue)
			{
				if (SelectedTab == page)
				{
					GraphicsPath val2 = new GraphicsPath();
					try
					{
						val2.AddPolygon(new Point[4]
						{
							new Point(array[0].X - 1, array[0].Y - 1),
							new Point(array[1].X - 1, array[1].Y),
							new Point(array[2].X + 1, array[2].Y),
							new Point(array[3].X + 1, array[3].Y)
						});
						SolidBrush val3 = new SolidBrush(Color.White);
						try
						{
							g.FillPath((Brush)(object)val3, val2);
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
				else
				{
					SolidBrush val4 = new SolidBrush(Color.White);
					try
					{
						g.FillPath((Brush)(object)val4, val);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
			else if (SelectedTab == page)
			{
				StiTabPage tabPageAtPoint = GetTabPageAtPoint(((Control)this).PointToClient(Cursor.Position));
				if (tabPageAtPoint == page)
				{
					Brush controlLightBrush = StiBrushes.GetControlLightBrush(pageRectangle, 90f);
					try
					{
						g.FillPath(controlLightBrush, val);
					}
					finally
					{
						((IDisposable)controlLightBrush)?.Dispose();
					}
				}
				else
				{
					Brush controlBrush = StiBrushes.GetControlBrush(pageRectangle, 90f);
					try
					{
						g.FillPath(controlBrush, val);
					}
					finally
					{
						((IDisposable)controlBrush)?.Dispose();
					}
				}
			}
			else
			{
				StiTabPage tabPageAtPoint2 = GetTabPageAtPoint(((Control)this).PointToClient(Cursor.Position));
				if (tabPageAtPoint2 == page)
				{
					SolidBrush val5 = new SolidBrush(StiColorUtils.Light(StiColors.Content, 15));
					try
					{
						g.FillPath((Brush)(object)val5, val);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				else
				{
					g.FillPath(StiBrushes.Content, val);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Color color = ((Control)this).ForeColor;
		if (page != SelectedTab)
		{
			color = SystemColors.ControlDark;
		}
		SizeF sizeF = SizeF.Empty;
		if (ImageList != null || page.Image != null)
		{
			int num = ((Control)this).Controls.IndexOf((Control)(object)page);
			if (ImageList != null || page.Image != null)
			{
				sizeF = ((page.Image == null) ? ((SizeF)ImageList.ImageSize) : ((SizeF)page.Image.Size));
				Rectangle rectangle = new Rectangle(pageRectangle.X + 2, (int)((float)pageRectangle.Y + ((float)pageRectangle.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height);
				if (sizeF.Width != 0f)
				{
					if (imageList != null && ImageList.Images.Count > num)
					{
						ImageList.Draw(g, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, num);
					}
					else if (page.Image != null)
					{
						g.DrawImage(page.Image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
					}
				}
			}
		}
		Rectangle rectangle2 = new Rectangle(pageRectangle.X + (int)sizeF.Width, pageRectangle.Y, pageRectangle.Width - (int)sizeF.Width, pageRectangle.Height);
		SolidBrush val6 = new SolidBrush(color);
		try
		{
			g.DrawString(((Control)page).Text, ((Control)this).Font, (Brush)(object)val6, (RectangleF)rectangle2, sfTabs);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		if (ControlStyle == StiControlStyle.Office2013Blue)
		{
			Pen val7 = new Pen(StiStyleOffice2013Blue.ColorLineGray);
			try
			{
				if (SelectedTab == page)
				{
					if (!PositionAtBottom)
					{
						g.DrawLine(val7, new Point(array[0].X - 1, array[0].Y - 1), new Point(array[1].X - 1, array[1].Y));
						g.DrawLine(val7, new Point(array[0].X - 1, array[0].Y - 1), new Point(array[3].X + 1, array[3].Y - 1));
						g.DrawLine(val7, new Point(array[2].X + 1, array[2].Y), new Point(array[3].X + 1, array[3].Y));
					}
					else
					{
						g.DrawLine(val7, array[1], array[2]);
						g.DrawLine(val7, array[2], array[3]);
					}
				}
				else if (PositionAtBottom)
				{
					g.DrawLine(val7, array[0], array[1]);
					g.DrawLine(val7, array[1], array[2]);
					g.DrawLine(val7, array[2], array[3]);
				}
				else
				{
					g.DrawLine(val7, array[0], array[1]);
					g.DrawLine(val7, array[0], array[3]);
					g.DrawLine(val7, array[2], array[3]);
				}
				return;
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (SelectedTab == page)
		{
			g.DrawLine(SystemPens.ControlLightLight, array[0], array[1]);
			if (!PositionAtBottom)
			{
				LinearGradientBrush val8 = new LinearGradientBrush(new Rectangle(array[3].X, array[3].Y, array[2].X - array[3].X, array[2].Y - array[3].Y - 5), SystemColors.Control, SystemColors.ControlDark, 0f);
				try
				{
					Pen val9 = new Pen((Brush)(object)val8);
					try
					{
						g.DrawLine(val9, array[2], array[3]);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
				g.DrawLine(SystemPens.ControlLightLight, array[0], array[3]);
			}
			else
			{
				g.DrawLine(SystemPens.ControlDark, array[1], array[2]);
				g.DrawLine(SystemPens.ControlDark, array[2], array[3]);
			}
			return;
		}
		Pen val10 = new Pen(StiColorUtils.Dark(SystemColors.Control, 40));
		try
		{
			if (PositionAtBottom)
			{
				g.DrawLine(val10, array[0], array[1]);
				g.DrawLine(val10, array[1], array[2]);
				g.DrawLine(val10, array[2], array[3]);
			}
			else
			{
				g.DrawLine(val10, array[0], array[1]);
				g.DrawLine(val10, array[0], array[3]);
				g.DrawLine(val10, array[2], array[3]);
			}
		}
		finally
		{
			((IDisposable)val10)?.Dispose();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override bool ProcessMnemonic(char key)
	{
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (Control.IsMnemonic(key, ((Control)item).Text))
			{
				SelectedTab = item;
				return true;
			}
		}
		return false;
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		((Control)this).OnPaint(p);
		if (locked || ((Control)this).Width == 0 || ((Control)this).Height == 0)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		Rectangle empty = Rectangle.Empty;
		Rectangle empty2 = Rectangle.Empty;
		if (PositionAtBottom)
		{
			empty = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height - titleHeight);
			empty2 = new Rectangle(0, ((Control)this).Height - titleHeight, ((Control)this).Width, titleHeight);
		}
		else
		{
			empty = new Rectangle(0, titleHeight, ((Control)this).Width, ((Control)this).Height - titleHeight);
			empty2 = new Rectangle(0, 0, ((Control)this).Width, titleHeight);
		}
		CalculatePagesWidth(graphics);
		if (ControlStyle == StiControlStyle.Office2013Blue)
		{
			SolidBrush val = new SolidBrush(Color.White);
			try
			{
				graphics.FillRectangle((Brush)(object)val, empty2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			graphics.FillRectangle(StiBrushes.ContentDark, empty2);
		}
		int num = 0;
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (!item.Invisible)
			{
				num++;
			}
		}
		if (((Component)this).DesignMode)
		{
			num = ((ArrangedElementCollection)((Control)this).Controls).Count;
		}
		if (num <= 0)
		{
			return;
		}
		if (SelectedTab == null)
		{
			SelectedTab = ((Control)this).Controls[0] as StiTabPage;
		}
		int num2 = 0;
		for (int num3 = ((ArrangedElementCollection)((Control)this).Controls).Count - 1; num3 >= 0; num3--)
		{
			StiTabPage stiTabPage2 = ((Control)this).Controls[num3] as StiTabPage;
			if (stiTabPage2 != SelectedTab && (!stiTabPage2.Invisible || ((Component)this).DesignMode))
			{
				DrawPage(graphics, stiTabPage2);
			}
			num2++;
		}
		DrawPage(graphics, SelectedTab);
		Rectangle pageRectangle = GetPageRectangle(graphics, SelectedTab);
		if (PositionAtBottom)
		{
			if (ControlStyle == StiControlStyle.Office2013Blue)
			{
				Pen val2 = new Pen(StiStyleOffice2013Blue.ColorLineGray);
				try
				{
					graphics.DrawLine(SystemPens.ControlDark, 0, ((Control)this).Height - titleHeight, pageRectangle.X - 1, ((Control)this).Height - titleHeight);
					graphics.DrawLine(val2, pageRectangle.Right + 1, ((Control)this).Height - titleHeight, ((Control)this).Width + 1, ((Control)this).Height - titleHeight);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			graphics.DrawLine(SystemPens.ControlDark, 0, ((Control)this).Height - titleHeight, pageRectangle.X - 1, ((Control)this).Height - titleHeight);
			graphics.DrawLine(SystemPens.ControlDark, pageRectangle.Right + 5, ((Control)this).Height - titleHeight, ((Control)this).Width + 1, ((Control)this).Height - titleHeight);
			return;
		}
		if (ControlStyle == StiControlStyle.Office2013Blue)
		{
			Pen val3 = new Pen(StiStyleOffice2013Blue.ColorLineGray);
			try
			{
				graphics.DrawLine(val3, 0, titleHeight - 1, pageRectangle.X - 1, titleHeight - 1);
				graphics.DrawLine(val3, pageRectangle.Right + 1, titleHeight - 1, ((Control)this).Width + 1, titleHeight - 1);
				return;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		graphics.DrawLine(SystemPens.ControlLightLight, 0, titleHeight - 1, pageRectangle.X - 1, titleHeight - 1);
		graphics.DrawLine(SystemPens.ControlDark, pageRectangle.Right + 5, titleHeight - 1, ((Control)this).Width + 1, titleHeight - 1);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		Point p = new Point(e.X, e.Y);
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			CalculatePagesWidth(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StiTabPage tabPageAtPoint = GetTabPageAtPoint(p);
		if (tabPageAtPoint != null && ((Control)tabPageAtPoint).Enabled)
		{
			SelectedTab = tabPageAtPoint;
			InvokeTitleClick(this, (EventArgs)(object)e);
		}
		((Control)this).OnMouseDown(e);
	}

	private void PageAdded(object sender, ControlEventArgs e)
	{
		e.Control.Dock = (DockStyle)5;
		e.Control.CreateGraphics();
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 1)
		{
			SelectedTab = ((Control)this).Controls[0] as StiTabPage;
		}
		((Control)this).Invalidate();
	}

	private void PageRemoved(object sender, ControlEventArgs e)
	{
		foreach (StiTabPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (item == SelectedTab)
			{
				((Control)this).Invalidate();
				return;
			}
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			SelectedTab = ((Control)this).Controls[0] as StiTabPage;
			((Control)this).Invalidate();
		}
		SelectedTab = null;
		((Control)this).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			sfTabs.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	public StiTabControl()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		sfTabs = new StringFormat
		{
			HotkeyPrefix = (HotkeyPrefix)1,
			LineAlignment = (StringAlignment)1,
			Alignment = (StringAlignment)1,
			FormatFlags = (StringFormatFlags)4096,
			Trimming = (StringTrimming)3
		};
		if ((int)((Control)this).RightToLeft == 1)
		{
			StringFormat obj = sfTabs;
			obj.FormatFlags = (StringFormatFlags)(obj.FormatFlags | 1);
		}
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).ControlAdded += new ControlEventHandler(PageAdded);
		((Control)this).ControlRemoved += new ControlEventHandler(PageRemoved);
		SetLayout();
	}
}
