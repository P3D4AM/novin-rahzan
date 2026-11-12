using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[Browsable(false)]
public class PageNavigatorItem : BaseItem
{
	private ePageNavigatorPart _MouseOverPart;

	private ePageNavigatorPart _MouseDownPart;

	private Rectangle _PreviousPageBounds;

	private Rectangle _TodayBounds;

	private Rectangle _NextPageBounds;

	private string _PreviousPageTooltip = "";

	private string _TodayTooltip = "";

	private string _NextPageTooltip = "";

	private Bitmap _PreviousPageBitmap;

	private Bitmap _TodayBitmap;

	private Bitmap _NextPageBitmap;

	private Timer _ClickTimer;

	private int _AutoClickCount;

	private int _ButtonSize = SystemInformation.VerticalScrollBarWidth;

	private int _MinWidth = SystemInformation.VerticalScrollBarWidth * 3;

	internal ePageNavigatorPart MouseOverPart
	{
		get
		{
			return _MouseOverPart;
		}
		set
		{
			if (_MouseOverPart != value)
			{
				_MouseOverPart = value;
				UpdateTooltip();
				Refresh();
			}
		}
	}

	internal ePageNavigatorPart MouseDownPart
	{
		get
		{
			return _MouseDownPart;
		}
		set
		{
			if (_MouseDownPart != value)
			{
				_MouseDownPart = value;
				Refresh();
			}
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates PageNavigator orientation.")]
	public override eOrientation Orientation
	{
		get
		{
			return base.Orientation;
		}
		set
		{
			if (base.Orientation != value)
			{
				base.Orientation = value;
				ReleaseBitmap(ref _NextPageBitmap);
				ReleaseBitmap(ref _PreviousPageBitmap);
				Size = ((value == eOrientation.Horizontal) ? new Size(_MinWidth, _ButtonSize) : new Size(_ButtonSize, _MinWidth));
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when the mouse button is kept pressed over the item.")]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(200)]
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when the mouse button is kept pressed over the item.")]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the PreviousPage button of the control.")]
	public string PreviousPageTooltip
	{
		get
		{
			return _PreviousPageTooltip;
		}
		set
		{
			_PreviousPageTooltip = value;
			UpdateTooltip();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the TodayPage button of the control.")]
	public string TodayTooltip
	{
		get
		{
			return _TodayTooltip;
		}
		set
		{
			_TodayTooltip = value;
			UpdateTooltip();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the NextPage button of the control.")]
	public string NextPageTooltip
	{
		get
		{
			return _NextPageTooltip;
		}
		set
		{
			_NextPageTooltip = value;
			UpdateTooltip();
		}
	}

	[Description("Occurs when NavigateNextPage button is clicked.")]
	public event EventHandler NavigateNextPage;

	[Description("Occurs when NavigateToday button is clicked.")]
	public event EventHandler NavigateToday;

	[Description("Occurs when NavigatePreviousPage button is clicked.")]
	public event EventHandler NavigatePreviousPage;

	public PageNavigatorItem()
		: this("")
	{
	}

	public PageNavigatorItem(string itemName)
	{
		Name = itemName;
		ClickRepeatInterval = 200;
		MouseUpNotification = true;
		MouseDownCapture = true;
		Size = new Size(_MinWidth, _ButtonSize);
	}

	public override void RecalcSize()
	{
		UpdateLayout();
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		UpdateLayout();
		base.OnExternalSizeChange();
	}

	private void UpdateLayout()
	{
		Rectangle bounds = Bounds;
		if (Orientation == eOrientation.Horizontal)
		{
			int num = bounds.Width / 3;
			_PreviousPageBounds = new Rectangle(bounds.Left, bounds.Top, num, bounds.Height);
			_NextPageBounds = new Rectangle(bounds.Right - num, bounds.Top, num, bounds.Height);
			_TodayBounds = new Rectangle(bounds.Left + num, bounds.Top, bounds.Width - num * 2, bounds.Height);
		}
		else
		{
			int num2 = bounds.Height / 3;
			_PreviousPageBounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, num2);
			_NextPageBounds = new Rectangle(bounds.Left, bounds.Bottom - num2, bounds.Width, num2);
			_TodayBounds = new Rectangle(bounds.Left, bounds.Y + num2, bounds.Width, bounds.Height - num2 * 2);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		Graphics graphics = e.Graphics;
		Office2007ScrollBarColorTable scrollBar = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ScrollBar;
		DrawPreviousPage(graphics, scrollBar);
		DrawToday(graphics, scrollBar);
		DrawNextPage(graphics, scrollBar);
	}

	private void DrawPreviousPage(Graphics g, Office2007ScrollBarColorTable ct)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable pageColorTable = GetPageColorTable(ct, ePageNavigatorPart.PreviousPage);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_PreviousPageBounds, pageColorTable.Background.Start, pageColorTable.Background.End, num);
		try
		{
			if (pageColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = pageColorTable.TrackBackground.GetColorBlend();
			}
			g.FillRectangle((Brush)(object)val, _PreviousPageBounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.DrawImageUnscaled((Image)(object)GetPreviousPageBitmap(g, pageColorTable), CenterRect(_PreviousPageBounds));
	}

	private Bitmap GetPreviousPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		if (_PreviousPageBitmap == null)
		{
			_PreviousPageBitmap = CreatePreviousPageBitmap(g, cst);
		}
		return _PreviousPageBitmap;
	}

	private Bitmap CreatePreviousPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				float num;
				if (Orientation == eOrientation.Horizontal)
				{
					num = 90f;
					val3.AddLines(new Point[3]
					{
						new Point(8, 0),
						new Point(4, 4),
						new Point(8, 8)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(4, 0),
						new Point(0, 4),
						new Point(4, 8)
					});
					val3.CloseFigure();
				}
				else
				{
					num = 0f;
					val3.AddLines(new Point[3]
					{
						new Point(0, 5),
						new Point(4, 0),
						new Point(8, 5)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(0, 9),
						new Point(4, 4),
						new Point(8, 9)
					});
					val3.CloseFigure();
				}
				Rectangle rectangle = new Rectangle(0, 0, 10, 10);
				LinearGradientBrush val4 = new LinearGradientBrush(rectangle, cst.ThumbSignBackground.Start, cst.ThumbSignBackground.End, num);
				try
				{
					val2.FillPath((Brush)(object)val4, val3);
					return val;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
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

	private void DrawToday(Graphics g, Office2007ScrollBarColorTable ct)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable pageColorTable = GetPageColorTable(ct, ePageNavigatorPart.Today);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_TodayBounds, pageColorTable.Background.Start, pageColorTable.Background.End, num);
		try
		{
			if (pageColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = pageColorTable.TrackBackground.GetColorBlend();
			}
			g.FillRectangle((Brush)(object)val, _TodayBounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.DrawImageUnscaled((Image)(object)GetTodayBitmap(g, pageColorTable), CenterRect(_TodayBounds));
	}

	private Bitmap GetTodayBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		if (_TodayBitmap == null)
		{
			_TodayBitmap = CreateTodayBitmap(g, cst);
		}
		return _TodayBitmap;
	}

	private Bitmap CreateTodayBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Rectangle rectangle = new Rectangle(1, 1, 6, 6);
			val2.SmoothingMode = (SmoothingMode)2;
			Color color = ControlPaint.LightLight(cst.ThumbSignBackground.Start);
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val2.FillEllipse((Brush)(object)val3, rectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(cst.ThumbSignBackground.End);
			try
			{
				val2.DrawEllipse(val4, rectangle);
				return val;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void DrawNextPage(Graphics g, Office2007ScrollBarColorTable ct)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable pageColorTable = GetPageColorTable(ct, ePageNavigatorPart.NextPage);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_NextPageBounds, pageColorTable.Background.Start, pageColorTable.Background.End, num);
		try
		{
			if (pageColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = pageColorTable.TrackBackground.GetColorBlend();
			}
			g.FillRectangle((Brush)(object)val, _NextPageBounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.DrawImageUnscaled((Image)(object)GetNextPageBitmap(g, pageColorTable), CenterRect(_NextPageBounds));
	}

	private Bitmap GetNextPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		if (_NextPageBitmap == null)
		{
			_NextPageBitmap = CreateNextPageBitmap(g, cst);
		}
		return _NextPageBitmap;
	}

	private Bitmap CreateNextPageBitmap(Graphics g, Office2007ScrollBarStateColorTable cst)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		Bitmap val = new Bitmap(10, 10, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				if (Orientation == eOrientation.Horizontal)
				{
					val3.AddLines(new Point[3]
					{
						new Point(1, 0),
						new Point(5, 4),
						new Point(1, 8)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(5, 0),
						new Point(9, 4),
						new Point(5, 8)
					});
					val3.CloseFigure();
				}
				else
				{
					val3.AddLines(new Point[3]
					{
						new Point(1, 0),
						new Point(8, 0),
						new Point(4, 4)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(1, 4),
						new Point(8, 4),
						new Point(4, 8)
					});
					val3.CloseFigure();
				}
				DisplayHelp.FillPath(val2, val3, cst.ThumbSignBackground);
				return val;
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

	private Office2007ScrollBarStateColorTable GetPageColorTable(Office2007ScrollBarColorTable ct, ePageNavigatorPart part)
	{
		if (!GetEnabled())
		{
			return ct.Disabled;
		}
		if (MouseDownPart == part)
		{
			return ct.Pressed;
		}
		if (MouseOverPart == part)
		{
			return ct.MouseOver;
		}
		return ct.Default;
	}

	private Rectangle CenterRect(Rectangle r)
	{
		r.X += (r.Width - 10) / 2 + 1;
		r.Y += (r.Height - 10) / 2 + 1;
		if (r.X < 0)
		{
			r.X = 0;
		}
		if (r.Y < 0)
		{
			r.Y = 0;
		}
		return r;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (GetEnabled() && !DesignMode && MouseDownPart == ePageNavigatorPart.None)
		{
			MouseOverPart = HitTest(objArg.Location);
		}
		base.InternalMouseMove(objArg);
	}

	public ePageNavigatorPart HitTest(Point pt)
	{
		if (_PreviousPageBounds.Contains(pt))
		{
			return ePageNavigatorPart.PreviousPage;
		}
		if (_NextPageBounds.Contains(pt))
		{
			return ePageNavigatorPart.NextPage;
		}
		if (_TodayBounds.Contains(pt))
		{
			return ePageNavigatorPart.Today;
		}
		return ePageNavigatorPart.None;
	}

	public override void InternalMouseLeave()
	{
		DisposeClickTimer();
		MouseOverPart = ePageNavigatorPart.None;
		MouseDownPart = ePageNavigatorPart.None;
		base.InternalMouseLeave();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && GetEnabled() && !DesignMode)
		{
			MouseDownPart = HitTest(objArg.Location);
			if (MouseDownPart != 0)
			{
				switch (MouseDownPart)
				{
				case ePageNavigatorPart.PreviousPage:
					OnNavigatePreviousPage();
					break;
				case ePageNavigatorPart.NextPage:
					OnNavigateNextPage();
					break;
				default:
					OnNavigateToday();
					break;
				}
			}
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		DisposeClickTimer();
		MouseDownPart = ePageNavigatorPart.None;
		base.InternalMouseUp(objArg);
	}

	protected virtual void OnNavigatePreviousPage()
	{
		if (this.NavigatePreviousPage != null)
		{
			this.NavigatePreviousPage(this, EventArgs.Empty);
		}
		EnableClickTimer();
	}

	protected virtual void OnNavigateToday()
	{
		if (this.NavigateToday != null)
		{
			this.NavigateToday(this, EventArgs.Empty);
		}
	}

	protected virtual void OnNavigateNextPage()
	{
		if (this.NavigateNextPage != null)
		{
			this.NavigateNextPage(this, EventArgs.Empty);
		}
		EnableClickTimer();
	}

	private void ClickTimerTick(object sender, EventArgs e)
	{
		switch (MouseDownPart)
		{
		case ePageNavigatorPart.PreviousPage:
			OnNavigatePreviousPage();
			break;
		case ePageNavigatorPart.NextPage:
			OnNavigateNextPage();
			break;
		}
		if (_ClickTimer != null)
		{
			_AutoClickCount++;
			if (_AutoClickCount > 4 && _ClickTimer.Interval > 20)
			{
				Timer clickTimer = _ClickTimer;
				clickTimer.Interval -= 10;
			}
		}
	}

	private void EnableClickTimer()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (ClickRepeatInterval > 0 && _ClickTimer == null)
		{
			_ClickTimer = new Timer();
			_ClickTimer.Interval = ClickRepeatInterval;
			_ClickTimer.Tick += ClickTimerTick;
			_ClickTimer.Start();
			_AutoClickCount = 0;
		}
	}

	private void DisposeClickTimer()
	{
		if (_ClickTimer != null)
		{
			_ClickTimer.Tick -= ClickTimerTick;
			_ClickTimer.Stop();
			((Component)(object)_ClickTimer).Dispose();
			_ClickTimer = null;
		}
	}

	protected override void OnTooltipChanged()
	{
		UpdateTooltip();
		base.OnTooltipChanged();
	}

	private void UpdateTooltip()
	{
		if (DesignMode)
		{
			return;
		}
		string text = "";
		switch (MouseOverPart)
		{
		case ePageNavigatorPart.PreviousPage:
			text = _PreviousPageTooltip;
			break;
		case ePageNavigatorPart.Today:
			text = _TodayTooltip;
			break;
		case ePageNavigatorPart.NextPage:
			text = _NextPageTooltip;
			break;
		}
		if (!text.Equals("") && m_Tooltip != text)
		{
			m_Tooltip = text;
			if (base.ToolTipVisible)
			{
				ShowToolTip();
			}
		}
	}

	public override BaseItem Copy()
	{
		PageNavigatorItem pageNavigatorItem = new PageNavigatorItem(Name);
		CopyToItem(pageNavigatorItem);
		return pageNavigatorItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is PageNavigatorItem pageNavigatorItem)
		{
			base.CopyToItem(pageNavigatorItem);
			pageNavigatorItem.PreviousPageTooltip = _PreviousPageTooltip;
			pageNavigatorItem.TodayTooltip = _TodayTooltip;
			pageNavigatorItem.NextPageTooltip = _NextPageTooltip;
		}
	}

	protected override void Dispose(bool disposing)
	{
		DisposeClickTimer();
		ReleaseBitmap(ref _PreviousPageBitmap);
		ReleaseBitmap(ref _TodayBitmap);
		ReleaseBitmap(ref _NextPageBitmap);
		base.Dispose(disposing);
	}

	private void ReleaseBitmap(ref Bitmap bmp)
	{
		if (bmp != null)
		{
			((Image)bmp).Dispose();
			bmp = null;
		}
	}
}
