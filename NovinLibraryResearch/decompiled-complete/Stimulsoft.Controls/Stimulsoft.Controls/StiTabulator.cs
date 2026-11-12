using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiTabulatorDesigner))]
public class StiTabulator : ContainerControl
{
	internal StringFormat sfTabs;

	private bool locked;

	private StiTabTitlePosition tabTitlePosition = StiTabTitlePosition.TopHorizontal;

	private StiTabulatorMode mode;

	private StiControlStyle controlStyle;

	private bool showTitle = true;

	private IStiTabulatorStyle style;

	private int titleWidth;

	private Color titleBackColor = Color.FromArgb(255, 250, 250, 250);

	private int titleHeight;

	private StiTabulatorPage selectedTab;

	private ImageList imageList;

	[CompilerGenerated]
	private MouseEventHandler m_TitleMouseUp;

	[CompilerGenerated]
	private MouseEventHandler m_TitleMouseMove;

	[CompilerGenerated]
	private MouseEventHandler m_TitleMouseDown;

	private Rectangle rightRect;

	private Rectangle leftRect;

	private bool visibleRightScrollButton;

	private bool visibleLeftScrollButton;

	private bool stateRight;

	private bool stateLeft;

	private bool ScrollButtonVisible;

	private int countPage;

	private Timer timer1 = new Timer();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	internal bool IsDesignMode => ((Component)this).DesignMode;

	[DefaultValue(StiTabTitlePosition.TopHorizontal)]
	public StiTabTitlePosition TabTitlePosition
	{
		get
		{
			return tabTitlePosition;
		}
		set
		{
			tabTitlePosition = value;
			switch (value)
			{
			case StiTabTitlePosition.LeftHorizontal:
				Mode = new StiLeftHorizontalMode(this);
				break;
			case StiTabTitlePosition.TopHorizontal:
				Mode = new StiTopHorizontalMode(this);
				break;
			case StiTabTitlePosition.RightHorizontal:
				Mode = new StiRightHorizontalMode(this);
				break;
			}
		}
	}

	[Browsable(false)]
	public StiTabulatorMode Mode
	{
		get
		{
			return mode;
		}
		set
		{
			mode = value;
			UpdateTitle();
			((Control)this).Invalidate();
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
			switch (value)
			{
			case StiControlStyle.Flat:
				Style = new StiTabulatorStyle(this);
				break;
			case StiControlStyle.Office2013Blue:
				Style = new StiTabulatorStyleOffice2013Blue(this);
				break;
			}
		}
	}

	[DefaultValue(true)]
	public bool ShowTitle
	{
		get
		{
			return showTitle;
		}
		set
		{
			showTitle = value;
			UpdateTitle();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public IStiTabulatorStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
			UpdateTitle();
			((Control)this).Invalidate();
		}
	}

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

	[Browsable(false)]
	internal int MaxTitleWidth { get; set; }

	[Browsable(false)]
	internal int MaxTitleHeight { get; set; }

	[DefaultValue(0)]
	public int TitleWidth
	{
		get
		{
			return titleWidth;
		}
		set
		{
			titleWidth = value;
			UpdateTitle();
			((Control)this).Invalidate();
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
			titleBackColor = value;
			UpdateTitle();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0)]
	public int TitleHeight
	{
		get
		{
			return titleHeight;
		}
		set
		{
			titleHeight = value;
			UpdateTitle();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiTabulatorPage SelectedTab
	{
		get
		{
			return selectedTab;
		}
		set
		{
			foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (item == value)
				{
					try
					{
						item.Dock = (DockStyle)5;
						((Control)item).Show();
					}
					catch
					{
						continue;
					}
					break;
				}
			}
			foreach (StiTabulatorPage item2 in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (item2 != value)
				{
					((Control)item2).Hide();
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
				SelectedTab = ((Control)this).Controls[value] as StiTabulatorPage;
			}
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
			UpdateTitle();
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	public event MouseEventHandler TitleMouseUp
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseUp, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseUp;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseUp, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Behavior")]
	public event MouseEventHandler TitleMouseMove
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseMove, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseMove;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseMove, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Behavior")]
	public event MouseEventHandler TitleMouseDown
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseDown, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			MouseEventHandler val = this.m_TitleMouseDown;
			MouseEventHandler val2;
			do
			{
				val2 = val;
				MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_TitleMouseDown, value2, val2);
			}
			while (val != val2);
		}
	}

	[Category("Behavior")]
	public event EventHandler SelectedIndexChanged;

	protected virtual void OnTitleMouseUp(MouseEventArgs e)
	{
	}

	public virtual void InvokeTitleMouseUp(object sender, MouseEventArgs e)
	{
		try
		{
			OnTitleMouseUp(e);
			MouseEventHandler titleMouseUp = this.TitleMouseUp;
			if (titleMouseUp != null)
			{
				titleMouseUp.Invoke(sender, e);
			}
		}
		catch
		{
		}
	}

	protected virtual void OnTitleMouseMove(MouseEventArgs e)
	{
	}

	public virtual void InvokeTitleMouseMove(object sender, MouseEventArgs e)
	{
		try
		{
			OnTitleMouseMove(e);
			MouseEventHandler titleMouseMove = this.TitleMouseMove;
			if (titleMouseMove != null)
			{
				titleMouseMove.Invoke(sender, e);
			}
		}
		catch
		{
		}
	}

	protected virtual void OnTitleMouseDown(MouseEventArgs e)
	{
	}

	public virtual void InvokeTitleMouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			OnTitleMouseDown(e);
			MouseEventHandler titleMouseDown = this.TitleMouseDown;
			if (titleMouseDown != null)
			{
				titleMouseDown.Invoke(sender, e);
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
		OnSelectedIndexChanged(EventArgs.Empty);
		this.SelectedIndexChanged?.Invoke(sender, e);
	}

	public void LockRefresh()
	{
		locked = true;
	}

	public void UnlockRefresh()
	{
		locked = false;
	}

	internal Rectangle GetTitlePageRectangle(Graphics g, StiTabulatorPage page)
	{
		return Mode.GetTitlePageRectangle(page);
	}

	public StiTabulatorPage GetTabPageAtPoint(Point p)
	{
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if ((!item.Invisible || ((Component)this).DesignMode) && GetTitlePageRectangle(val, item).Contains(p))
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

	private Size GetMaxPageTitleSize()
	{
		int num = 0;
		int num2 = 0;
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Size titleSize = item.TitleSize;
			num = Math.Max(titleSize.Width, num);
			num2 = Math.Max(titleSize.Height, num2);
		}
		return new Size(num, num2);
	}

	internal void UpdateTitle()
	{
		MeasureTitles();
		Size maxPageTitleSize = GetMaxPageTitleSize();
		MaxTitleWidth = maxPageTitleSize.Width;
		MaxTitleHeight = maxPageTitleSize.Height;
		SetLayout();
		((Control)this).Invalidate();
		if (SelectedTab != null)
		{
			((Control)SelectedTab).Invalidate();
		}
	}

	private void MeasureTitles()
	{
		Mode.MeasureTitles();
	}

	private void SetLayout()
	{
		Mode.SetLayout();
	}

	private void DrawPageTitle(Graphics g, StiTabulatorPage page)
	{
		if (!page.Invisible || ((Component)this).DesignMode)
		{
			Mode.DrawPageTitle(g, page);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		Point p = new Point(e.X, e.Y);
		StiTabulatorPage tabPageAtPoint = GetTabPageAtPoint(p);
		if (tabPageAtPoint != null)
		{
			InvokeTitleMouseMove(this, e);
		}
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

	protected override void OnFontChanged(EventArgs e)
	{
		((ContainerControl)this).OnFontChanged(e);
		MeasureTitles();
		UpdateTitle();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
		((Control)this).BackColor = (StiUX.IsDark ? StiUX.Background : StiColorUtils.Light(SystemColors.Control, 10));
	}

	protected override bool ProcessMnemonic(char key)
	{
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (Control.IsMnemonic(key, ((Control)item).Text))
			{
				SelectedTab = item;
				return true;
			}
		}
		return false;
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		if (locked)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		Rectangle titleRectangle = Mode.GetTitleRectangle();
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Background, titleRectangle);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (!item.Invisible)
				{
					num++;
				}
				if (!item.Invisible)
				{
					num2 += item.TitleSize.Width;
				}
				else
				{
					num3++;
				}
			}
			if (tabTitlePosition == StiTabTitlePosition.TopHorizontal)
			{
				if (num2 < ((Control)this).Width && num3 > 0)
				{
					visibleRightScrollButton = false;
					visibleLeftScrollButton = true;
					mode.startPosTitle = StiScaleUI.XXI(18.0);
					ScrollButtonVisible = true;
					ResizeScrollButton();
				}
				else if (num2 > ((Control)this).Width && num3 == 0)
				{
					visibleRightScrollButton = true;
					visibleLeftScrollButton = false;
					mode.startPosTitle = StiScaleUI.I8;
					ScrollButtonVisible = true;
					ResizeScrollButton();
				}
				else if (num2 < ((Control)this).Width)
				{
					mode.startPosTitle = StiScaleUI.I8;
					ScrollButtonVisible = false;
					VisiblePage();
				}
				else
				{
					visibleRightScrollButton = true;
					visibleLeftScrollButton = true;
					mode.startPosTitle = StiScaleUI.XXI(18.0);
					ScrollButtonVisible = true;
					ResizeScrollButton();
				}
			}
			if (((Component)this).DesignMode)
			{
				num = ((ArrangedElementCollection)((Control)this).Controls).Count;
			}
			if (num > 0)
			{
				if (selectedTab == null)
				{
					SelectedTab = ((Control)this).Controls[0] as StiTabulatorPage;
				}
				for (int num4 = ((ArrangedElementCollection)((Control)this).Controls).Count - 1; num4 >= 0; num4--)
				{
					StiTabulatorPage stiTabulatorPage2 = (StiTabulatorPage)(object)((Control)this).Controls[num4];
					if (stiTabulatorPage2 != selectedTab && (!stiTabulatorPage2.Invisible || ((Component)this).DesignMode))
					{
						DrawPageTitle(graphics, stiTabulatorPage2);
					}
				}
				DrawPageTitle(graphics, selectedTab);
			}
		}
		if (tabTitlePosition != StiTabTitlePosition.TopHorizontal || !ScrollButtonVisible)
		{
			return;
		}
		if (visibleRightScrollButton)
		{
			if (stateRight)
			{
				ControlPaint.DrawScrollButton(graphics, rightRect, (ScrollButton)3, (ButtonState)1024);
			}
			else
			{
				ControlPaint.DrawScrollButton(graphics, rightRect, (ScrollButton)3, (ButtonState)0);
			}
		}
		if (visibleLeftScrollButton)
		{
			if (stateLeft)
			{
				ControlPaint.DrawScrollButton(graphics, leftRect, (ScrollButton)2, (ButtonState)1024);
			}
			else
			{
				ControlPaint.DrawScrollButton(graphics, leftRect, (ScrollButton)2, (ButtonState)0);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		UpScrollButton();
		Point p = new Point(e.X, e.Y);
		StiTabulatorPage tabPageAtPoint = GetTabPageAtPoint(p);
		if (tabPageAtPoint != null)
		{
			InvokeTitleMouseUp(this, e);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		Point p = new Point(e.X, e.Y);
		if (!GetScrollButtonAtPoint(p))
		{
			StiTabulatorPage tabPageAtPoint = GetTabPageAtPoint(p);
			if (tabPageAtPoint != null)
			{
				InvokeTitleMouseDown(this, e);
				if ((e.Button & 0x100000) > 0)
				{
					SelectedTab = tabPageAtPoint;
				}
			}
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		MeasureTitles();
		UpdateTitle();
	}

	private void InitializeButtonSize()
	{
		timer1.Interval = 150;
		timer1.Tick += Timer1_Tick;
		rightRect = new Rectangle(((Control)this).Width - StiScaleUI.XXI(18.0), StiScaleUI.XXI(3.0), StiScaleUI.XXI(16.0), StiScaleUI.XXI(23.0));
		leftRect = new Rectangle(0, StiScaleUI.XXI(3.0), StiScaleUI.XXI(16.0), StiScaleUI.XXI(23.0));
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		if (stateRight && visibleRightScrollButton)
		{
			RightButton_Click();
		}
		if (stateLeft)
		{
			LeftButton_Click();
		}
	}

	private void ResizeScrollButton()
	{
		rightRect.X = ((Control)this).Width - StiScaleUI.XXI(16.0);
	}

	private bool GetScrollButtonAtPoint(Point p)
	{
		if (visibleRightScrollButton && rightRect.Contains(p))
		{
			stateRight = true;
			RightButton_Click();
			timer1.Enabled = true;
			return true;
		}
		if (visibleLeftScrollButton && leftRect.Contains(p))
		{
			stateLeft = true;
			LeftButton_Click();
			timer1.Enabled = true;
			return true;
		}
		return false;
	}

	private void UpScrollButton()
	{
		timer1.Enabled = false;
		stateLeft = false;
		stateRight = false;
		timer1.Enabled = false;
	}

	private void RightButton_Click()
	{
		if (countPage + 1 < ((ArrangedElementCollection)((Control)this).Controls).Count)
		{
			countPage++;
			if (countPage != 0)
			{
				for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
				{
					if (i < countPage)
					{
						((StiTabulatorPage)(object)((Control)this).Controls[i]).Invisible = true;
					}
					else
					{
						((StiTabulatorPage)(object)((Control)this).Controls[i]).Invisible = false;
					}
				}
			}
		}
		((Control)selectedTab).Invalidate();
	}

	private void LeftButton_Click()
	{
		if (countPage != 0)
		{
			countPage--;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				if (i < countPage)
				{
					((StiTabulatorPage)(object)((Control)this).Controls[i]).Invisible = true;
				}
				else
				{
					((StiTabulatorPage)(object)((Control)this).Controls[i]).Invisible = false;
				}
			}
		}
		((Control)selectedTab).Invalidate();
	}

	private void VisiblePage()
	{
		if (countPage > 0)
		{
			countPage = 0;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
			{
				((StiTabulatorPage)(object)((Control)this).Controls[i]).Invisible = false;
			}
		}
	}

	private void PageAdded(object sender, ControlEventArgs e)
	{
		e.Control.Dock = (DockStyle)5;
		e.Control.CreateGraphics();
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 1)
		{
			SelectedTab = ((Control)this).Controls[0] as StiTabulatorPage;
		}
		MeasureTitles();
		UpdateTitle();
	}

	private void PageRemoved(object sender, ControlEventArgs e)
	{
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (item == SelectedTab)
			{
				((Control)this).Invalidate();
				return;
			}
		}
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			SelectedTab = ((Control)this).Controls[0] as StiTabulatorPage;
			((Control)this).Invalidate();
		}
		SelectedTab = null;
		MeasureTitles();
		UpdateTitle();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			sfTabs.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	public StiTabulator()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		mode = new StiTopHorizontalMode(this);
		style = new StiTabulatorStyle(this);
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
		InitializeButtonSize();
		SetLayout();
	}
}
