using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiCloudTabulatorDesigner))]
public class StiCloudTabulator : StiScrollableControl
{
	private Color titleBackColor = Color.FromArgb(255, 250, 250, 250);

	private StiTabTitlePosition titlePosition = StiTabTitlePosition.RightHorizontal;

	private int titleWidth = 110;

	private bool titleInLine;

	private bool win11Look;

	private StiCloudTabulatorPage selectedTab;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new DockPaddingEdges DockPadding => base.DockPadding;

	internal int CloudScrollWidth { get; set; } = StiScaleUI.I(22);


	private int FullPagesHeight { get; set; }

	internal bool ShowVerticalScroll { get; set; }

	internal StiCloudMode Mode { get; set; }

	internal int MaxTitleWidth { get; set; }

	internal int MaxTitleHeight { get; set; }

	internal StiCloudTabulatorPage MouseOverPage { get; set; }

	private bool LockUpdateTitle { get; set; }

	[Browsable(false)]
	internal bool IsDesignMode => ((Component)(object)this).DesignMode;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
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

	public Color TitleBackColor
	{
		get
		{
			return titleBackColor;
		}
		set
		{
			if (titleBackColor != value)
			{
				titleBackColor = value;
				UpdateTitle();
			}
		}
	}

	[DefaultValue(StiTabTitlePosition.RightHorizontal)]
	public StiTabTitlePosition TitlePosition
	{
		get
		{
			return titlePosition;
		}
		set
		{
			if (titlePosition != value)
			{
				titlePosition = value;
				UpdateTitle();
			}
		}
	}

	[DefaultValue(110)]
	public int TitleWidth
	{
		get
		{
			return titleWidth;
		}
		set
		{
			if (titleWidth != value)
			{
				titleWidth = value;
				UpdateTitle();
			}
		}
	}

	[DefaultValue(false)]
	public bool TitleInLine
	{
		get
		{
			return titleInLine;
		}
		set
		{
			if (titleInLine != value)
			{
				titleInLine = value;
				UpdateTitle();
			}
		}
	}

	[DefaultValue(false)]
	public bool Win11Look
	{
		get
		{
			return win11Look;
		}
		set
		{
			if (win11Look != value)
			{
				win11Look = value;
				UpdateTitle();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiCloudTabulatorPage SelectedTab
	{
		get
		{
			return selectedTab;
		}
		set
		{
			MouseOverPage = null;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage && stiCloudTabulatorPage == value)
				{
					try
					{
						stiCloudTabulatorPage.Dock = (DockStyle)5;
						((Control)stiCloudTabulatorPage).Show();
					}
					catch
					{
						continue;
					}
					break;
				}
			}
			for (int j = 0; j < ((ArrangedElementCollection)((Control)this).Controls).Count; j++)
			{
				if (((Control)this).Controls[j] is StiCloudTabulatorPage stiCloudTabulatorPage2 && stiCloudTabulatorPage2 != value)
				{
					((Control)stiCloudTabulatorPage2).Hide();
				}
			}
			if (selectedTab != value)
			{
				selectedTab = value;
				((Control)this).Invalidate();
				InvokeSelectedIndexChanged(this, EventArgs.Empty);
			}
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			int num = 0;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage)
				{
					if (stiCloudTabulatorPage == selectedTab)
					{
						return num;
					}
					num++;
				}
			}
			return ((Control)this).Controls.IndexOf((Control)(object)SelectedTab);
		}
		set
		{
			if (value < 0 || value >= ((ArrangedElementCollection)((Control)this).Controls).Count)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage)
				{
					if (num == value)
					{
						SelectedTab = stiCloudTabulatorPage;
						break;
					}
					num++;
				}
			}
		}
	}

	[Category("Behavior")]
	public event EventHandler SelectedIndexChanged;

	private bool ShouldSerializeTitleBackColor()
	{
		return TitleBackColor != Color.FromArgb(255, 250, 250, 250);
	}

	private void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(sender, e);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (e.Delta > 0)
		{
			base.VScrollBar.DoUp();
		}
		else
		{
			base.VScrollBar.DoDown();
		}
	}

	public StiCloudTabulatorPage GetTabPageAtPoint(Point p)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage && (!stiCloudTabulatorPage.Invisible || ((Component)(object)this).DesignMode))
			{
				Rectangle rectangle = new Rectangle(stiCloudTabulatorPage.TitleRect.X, stiCloudTabulatorPage.TitleRect.Y, stiCloudTabulatorPage.TitleRect.Width, stiCloudTabulatorPage.TitleRect.Height);
				if (ShowVerticalScroll)
				{
					rectangle.Y -= ((ScrollBar)base.VScrollBar).Value;
				}
				if (rectangle.Contains(p))
				{
					return stiCloudTabulatorPage;
				}
			}
		}
		return null;
	}

	private Size GetMaxPageTitleSize()
	{
		int val = 0;
		int num = 0;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage { TitleSize: var titleSize })
			{
				val = Math.Max(titleSize.Width, val);
				num = Math.Max(titleSize.Height, num);
			}
		}
		val = Math.Max(TitleWidth, val);
		return new Size(val, num);
	}

	private int GetCountVisiblePages()
	{
		int num = 0;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage { Invisible: false })
			{
				num++;
			}
		}
		return num;
	}

	internal void UpdateTitle()
	{
		if (!LockUpdateTitle)
		{
			MeasureTitles();
			Size maxPageTitleSize = GetMaxPageTitleSize();
			MaxTitleWidth = maxPageTitleSize.Width;
			MaxTitleHeight = maxPageTitleSize.Height;
			CheckScrollBar();
			((Control)this).Invalidate();
		}
	}

	private void MeasureTitles()
	{
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			return;
		}
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage)
				{
					stiCloudTabulatorPage.TitleSize = Mode.MeasureTitle(val, stiCloudTabulatorPage);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ApplyTitleSize()
	{
		int num = ((TitlePosition != 0) ? (((Control)this).Width - MaxTitleWidth) : 0);
		int num2 = MaxTitleWidth;
		int num3 = 0;
		if (ShowVerticalScroll)
		{
			num -= CloudScrollWidth + StiScaleUI.I6;
			num2 += CloudScrollWidth;
		}
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage { Invisible: false } stiCloudTabulatorPage)
			{
				stiCloudTabulatorPage.TitleRect = new Rectangle(num, num3, num2, MaxTitleHeight);
				num3 += MaxTitleHeight;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (!(((Control)this).Controls[i] is StiCloudTabulatorPage { Invisible: false } stiCloudTabulatorPage))
			{
				continue;
			}
			Rectangle rectangle = new Rectangle(stiCloudTabulatorPage.TitleRect.X, stiCloudTabulatorPage.TitleRect.Y, stiCloudTabulatorPage.TitleRect.Width, stiCloudTabulatorPage.TitleRect.Height);
			if (ShowVerticalScroll)
			{
				rectangle.Y -= ((ScrollBar)base.VScrollBar).Value;
			}
			if (rectangle.Contains(pt))
			{
				if (MouseOverPage != stiCloudTabulatorPage)
				{
					MouseOverPage = stiCloudTabulatorPage;
					((Control)this).Invalidate();
				}
				return;
			}
		}
		if (MouseOverPage != null)
		{
			MouseOverPage = null;
			((Control)this).Invalidate();
		}
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

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		UpdateTitle();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
		((Control)this).BackColor = StiColorUtils.Light(SystemColors.Control, 10);
	}

	protected override bool ProcessMnemonic(char key)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage && Control.IsMnemonic(key, ((Control)stiCloudTabulatorPage).Text))
			{
				SelectedTab = stiCloudTabulatorPage;
				return true;
			}
		}
		return false;
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
		if (selectedTab != null)
		{
			((Control)selectedTab).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		if (LockUpdateTitle)
		{
			LockUpdateTitle = false;
			UpdateTitle();
		}
		Graphics graphics = p.Graphics;
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			int num = ((TitlePosition != 0) ? (((Control)this).Width - MaxTitleWidth - 2 * Mode.titleSpace) : 0);
			int num2 = MaxTitleWidth + 2 * Mode.titleSpace;
			if (ShowVerticalScroll)
			{
				num -= CloudScrollWidth + StiScaleUI.I4;
				num2 += CloudScrollWidth + StiScaleUI.I4;
			}
			Rectangle rect = new Rectangle(num, 0, num2, ((Control)this).Height);
			if (((Control)this).Width == 0 || ((Control)this).Height == 0)
			{
				return;
			}
			StiDrawing.FillRectangle(graphics, StiUX.BackgroundDark, rect);
			if (selectedTab == null)
			{
				for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
				{
					if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage)
					{
						SelectedTab = stiCloudTabulatorPage;
						return;
					}
				}
			}
			((Control)SelectedTab).Show();
			for (int j = 0; j < ((ArrangedElementCollection)((Control)this).Controls).Count; j++)
			{
				if (((Control)this).Controls[j] is StiCloudTabulatorPage stiCloudTabulatorPage2 && (!stiCloudTabulatorPage2.Invisible || ((Component)(object)this).DesignMode))
				{
					DrawPageTitle(graphics, stiCloudTabulatorPage2);
				}
			}
		}
		else
		{
			Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			if (((Control)this).Width != 0 && ((Control)this).Height != 0)
			{
				StiDrawing.FillRectangle(graphics, StiUX.BackgroundDark, rect);
			}
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		Point p = new Point(e.X, e.Y);
		StiCloudTabulatorPage tabPageAtPoint = GetTabPageAtPoint(p);
		if (tabPageAtPoint != null)
		{
			if ((e.Button & 0x100000) > 0)
			{
				SelectedTab = tabPageAtPoint;
			}
			tabPageAtPoint.InvokeMouseClick(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);
		if (Mode != null)
		{
			UpdateTitle();
		}
	}

	private void DrawPageTitle(Graphics g, StiCloudTabulatorPage page)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(page.TitleRect.X, page.TitleRect.Y, page.TitleRect.Width, page.TitleRect.Height);
		if (ShowVerticalScroll)
		{
			rectangle.Y -= ((ScrollBar)base.VScrollBar).Value;
		}
		if (rectangle.Width == 0 || rectangle.Height == 0)
		{
			return;
		}
		Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y + StiScaleUI.I3, rectangle.Width, rectangle.Height);
		if (page != SelectedTab || !page.ShowSelected)
		{
			if (page == MouseOverPage)
			{
				SolidBrush val = new SolidBrush(StiUX.IsDark ? StiUX.ItemBackgroundHover : StiStyleOffice2013Blue.ColorButtonButtonMouseOver);
				try
				{
					g.FillRectangle((Brush)(object)val, rectangle2);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(StiUX.BackgroundDark);
				try
				{
					g.FillRectangle((Brush)(object)val2, rectangle2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else
		{
			SolidBrush val3 = new SolidBrush(StiUX.IsDark ? StiUX.ItemBackgroundSelected : StiStyleOffice2013Blue.ColorButtonChecked);
			try
			{
				g.FillRectangle((Brush)(object)val3, rectangle2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Mode.DrawTitleText(g, rectangle2, page);
		Mode.DrawTitleImage(g, rectangle2, page);
		Mode.DrawMarker(g, rectangle2, page);
	}

	internal void CheckScrollBar()
	{
		if (!LockUpdateTitle)
		{
			FullPagesHeight = MaxTitleHeight * GetCountVisiblePages();
			if (FullPagesHeight > ((Control)this).Height)
			{
				ScrollHeight = FullPagesHeight;
				ShowVerticalScroll = true;
			}
			else
			{
				ScrollHeight = 0;
				ShowVerticalScroll = false;
			}
			int num = MaxTitleWidth + Mode.titleSpace;
			if (ShowVerticalScroll)
			{
				num += CloudScrollWidth + 6;
			}
			if (TitlePosition == StiTabTitlePosition.LeftHorizontal)
			{
				DockPadding.Right = 0;
				DockPadding.Left = num;
			}
			else
			{
				DockPadding.Left = 0;
				DockPadding.Right = num;
			}
			ApplyTitleSize();
			((Control)this).Invalidate();
			if (SelectedTab != null)
			{
				((Control)SelectedTab).Invalidate();
			}
		}
	}

	private void PageAdded(object sender, ControlEventArgs e)
	{
		e.Control.Dock = (DockStyle)5;
		e.Control.CreateGraphics();
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 4)
		{
			SelectedTab = ((Control)this).Controls[3] as StiCloudTabulatorPage;
		}
		UpdateTitle();
	}

	private void PageRemoved(object sender, ControlEventArgs e)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			if (((Control)this).Controls[i] is StiCloudTabulatorPage stiCloudTabulatorPage && stiCloudTabulatorPage == SelectedTab)
			{
				((Control)this).Invalidate();
				return;
			}
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 3)
		{
			SelectedTab = ((Control)this).Controls[3] as StiCloudTabulatorPage;
			((Control)this).Invalidate();
		}
		SelectedTab = null;
		UpdateTitle();
	}

	public StiCloudTabulator()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		Mode = new StiCloudMode(this);
		DockPadding.Left = 0;
		DockPadding.Top = 0;
		DockPadding.Bottom = 0;
		DockPadding.Right = StiScaleUI.XXI(110.0);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).ControlAdded += new ControlEventHandler(PageAdded);
		((Control)this).ControlRemoved += new ControlEventHandler(PageRemoved);
		LockUpdateTitle = true;
	}
}
