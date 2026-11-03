using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[Browsable(false)]
public class PageNavigatorItem : BaseItem
{
	private EventHandler Ⱂ;

	private EventHandler Ⱃ;

	private EventHandler Ⱄ;

	private ePageNavigatorPart __24EA;

	private ePageNavigatorPart __24EB;

	private Rectangle _Ⱌ;

	private Rectangle _Ⱍ;

	private Rectangle _Ⱎ;

	private string _Ⱏ = "";

	private string _Ⱐ = "";

	private string _Ⱑ = "";

	private Bitmap _Ⱒ;

	private Bitmap _Ⱓ;

	private Bitmap _Ⱔ;

	private Timer _ᶜ;

	private int _Ⱕ;

	private int __268E = SystemInformation.VerticalScrollBarWidth;

	private int _Ⱖ = SystemInformation.VerticalScrollBarWidth * 3;

	internal ePageNavigatorPart ᘅ
	{
		get
		{
			return __24EA;
		}
		set
		{
			if (__24EA != value)
			{
				__24EA = value;
				_24DC();
				Refresh();
			}
		}
	}

	internal ePageNavigatorPart Ⱋ
	{
		get
		{
			return __24EB;
		}
		set
		{
			if (__24EB != value)
			{
				__24EB = value;
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
				ⰴ(ref _Ⱔ);
				ⰴ(ref _Ⱒ);
				Size = ((value == eOrientation.Horizontal) ? new Size(Dpi.Width(_Ⱖ), Dpi.Height(__268E)) : new Size(Dpi.Width(__268E), Dpi.Height(_Ⱖ)));
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets whether Click event will be auto repeated when the mouse button is kept pressed over the item.")]
	[DevCoBrowsable(false)]
	[Browsable(false)]
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

	[Description("Gets or sets the auto-repeat interval for the click event when the mouse button is kept pressed over the item.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[DefaultValue(200)]
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

	[DefaultValue("")]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the PreviousPage button of the control.")]
	[Localizable(true)]
	public string PreviousPageTooltip
	{
		get
		{
			return _Ⱏ;
		}
		set
		{
			_Ⱏ = value;
			_24DC();
		}
	}

	[Description("Indicates tooltip for the TodayPage button of the control.")]
	[Browsable(true)]
	[DefaultValue("")]
	[Category("Appearance")]
	[Localizable(true)]
	public string TodayTooltip
	{
		get
		{
			return _Ⱐ;
		}
		set
		{
			_Ⱐ = value;
			_24DC();
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates tooltip for the NextPage button of the control.")]
	[DefaultValue("")]
	[Localizable(true)]
	public string NextPageTooltip
	{
		get
		{
			return _Ⱑ;
		}
		set
		{
			_Ⱑ = value;
			_24DC();
		}
	}

	[Description("Occurs when NavigateNextPage button is clicked.")]
	public event EventHandler NavigateNextPage
	{
		add
		{
			EventHandler eventHandler = Ⱂ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱂ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱂ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱂ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when NavigateToday button is clicked.")]
	public event EventHandler NavigateToday
	{
		add
		{
			EventHandler eventHandler = Ⱃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when NavigatePreviousPage button is clicked.")]
	public event EventHandler NavigatePreviousPage
	{
		add
		{
			EventHandler eventHandler = Ⱄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		Size = new Size(Dpi.Width(_Ⱖ), Dpi.Height(__268E));
	}

	protected override void ScaleItem(SizeF factor)
	{
		ⰴ(ref _Ⱒ);
		ⰴ(ref _Ⱓ);
		ⰴ(ref _Ⱔ);
		Size = ((Orientation == eOrientation.Horizontal) ? new Size(Dpi.Width(_Ⱖ), Dpi.Height(__268E)) : new Size(Dpi.Width(__268E), Dpi.Height(_Ⱖ)));
		base.ScaleItem(factor);
	}

	public override void RecalcSize()
	{
		ᘥ();
		base.RecalcSize();
	}

	protected override void OnExternalSizeChange()
	{
		ᘥ();
		base.OnExternalSizeChange();
	}

	private void ᘥ()
	{
		Rectangle bounds = Bounds;
		if (Orientation == eOrientation.Horizontal)
		{
			int num = bounds.Width / 3;
			_Ⱌ = new Rectangle(bounds.Left, bounds.Top, num, bounds.Height);
			_Ⱎ = new Rectangle(bounds.Right - num, bounds.Top, num, bounds.Height);
			_Ⱍ = new Rectangle(bounds.Left + num, bounds.Top, bounds.Width - num * 2, bounds.Height);
		}
		else
		{
			int num2 = bounds.Height / 3;
			_Ⱌ = new Rectangle(bounds.Left, bounds.Top, bounds.Width, num2);
			_Ⱎ = new Rectangle(bounds.Left, bounds.Bottom - num2, bounds.Width, num2);
			_Ⱍ = new Rectangle(bounds.Left, bounds.Y + num2, bounds.Width, bounds.Height - num2 * 2);
		}
	}

	public override void Paint(ItemPaintArgs e)
	{
		Graphics graphics = e.Graphics;
		Office2007ScrollBarColorTable scrollBar = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.ScrollBar;
		Ⱗ(graphics, scrollBar);
		Ⱛ(graphics, scrollBar);
		Ⱞ(graphics, scrollBar);
	}

	private void Ⱗ(Graphics _٥, Office2007ScrollBarColorTable ዅ)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ⰱ(ዅ, ePageNavigatorPart.PreviousPage);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_Ⱌ, office2007ScrollBarStateColorTable.Background.Start, office2007ScrollBarStateColorTable.Background.End, num);
		try
		{
			if (office2007ScrollBarStateColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = office2007ScrollBarStateColorTable.TrackBackground.GetColorBlend();
			}
			_٥.FillRectangle((Brush)(object)val, _Ⱌ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.DrawImageUnscaled((Image)(object)Ⱘ(_٥, office2007ScrollBarStateColorTable), ⰲ(_Ⱌ));
	}

	private Bitmap Ⱘ(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		if (_Ⱒ == null)
		{
			_Ⱒ = Ⱚ(_٥, Ⱙ);
		}
		return _Ⱒ;
	}

	private Bitmap Ⱚ(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width10, Dpi.Height10, _٥);
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
						new Point(Dpi.Width8, 0),
						new Point(Dpi.Width4, Dpi.Height4),
						new Point(Dpi.Width8, Dpi.Height8)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(Dpi.Width4, 0),
						new Point(0, Dpi.Height4),
						new Point(Dpi.Width4, Dpi.Height8)
					});
					val3.CloseFigure();
				}
				else
				{
					num = 0f;
					val3.AddLines(new Point[3]
					{
						new Point(0, Dpi.Height5),
						new Point(Dpi.Width4, 0),
						new Point(Dpi.Width8, Dpi.Height5)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(0, Dpi.Height9),
						new Point(Dpi.Width4, Dpi.Height4),
						new Point(Dpi.Width8, Dpi.Height9)
					});
					val3.CloseFigure();
				}
				Rectangle rectangle = new Rectangle(0, 0, Dpi.Width10, Dpi.Height10);
				LinearGradientBrush val4 = new LinearGradientBrush(rectangle, Ⱙ.ThumbSignBackground.Start, Ⱙ.ThumbSignBackground.End, num);
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

	private void Ⱛ(Graphics _٥, Office2007ScrollBarColorTable ዅ)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ⰱ(ዅ, ePageNavigatorPart.Today);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_Ⱍ, office2007ScrollBarStateColorTable.Background.Start, office2007ScrollBarStateColorTable.Background.End, num);
		try
		{
			if (office2007ScrollBarStateColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = office2007ScrollBarStateColorTable.TrackBackground.GetColorBlend();
			}
			_٥.FillRectangle((Brush)(object)val, _Ⱍ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.DrawImageUnscaled((Image)(object)Ⱜ(_٥, office2007ScrollBarStateColorTable), ⰲ(_Ⱍ));
	}

	private Bitmap Ⱜ(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		if (_Ⱓ == null)
		{
			_Ⱓ = Ⱝ(_٥, Ⱙ);
		}
		return _Ⱓ;
	}

	private Bitmap Ⱝ(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width10, Dpi.Height10, _٥);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Rectangle rectangle = new Rectangle(Dpi.Width1, Dpi.Height1, Dpi.Width6, Dpi.Height6);
			val2.SmoothingMode = (SmoothingMode)2;
			Color color = ControlPaint.LightLight(Ⱙ.ThumbSignBackground.Start);
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val2.FillEllipse((Brush)(object)val3, rectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(Ⱙ.ThumbSignBackground.End, (float)Dpi.Width1);
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

	private void Ⱞ(Graphics _٥, Office2007ScrollBarColorTable ዅ)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		Office2007ScrollBarStateColorTable office2007ScrollBarStateColorTable = ⰱ(ዅ, ePageNavigatorPart.NextPage);
		float num = ((Orientation == eOrientation.Horizontal) ? 90f : 0f);
		LinearGradientBrush val = new LinearGradientBrush(_Ⱎ, office2007ScrollBarStateColorTable.Background.Start, office2007ScrollBarStateColorTable.Background.End, num);
		try
		{
			if (office2007ScrollBarStateColorTable.TrackBackground.Count > 0)
			{
				val.InterpolationColors = office2007ScrollBarStateColorTable.TrackBackground.GetColorBlend();
			}
			_٥.FillRectangle((Brush)(object)val, _Ⱎ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		_٥.DrawImageUnscaled((Image)(object)_2C2F(_٥, office2007ScrollBarStateColorTable), ⰲ(_Ⱎ));
	}

	private Bitmap _2C2F(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		if (_Ⱔ == null)
		{
			_Ⱔ = ⰰ(_٥, Ⱙ);
		}
		return _Ⱔ;
	}

	private Bitmap ⰰ(Graphics _٥, Office2007ScrollBarStateColorTable Ⱙ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		Bitmap val = new Bitmap(Dpi.Width10, Dpi.Height10, _٥);
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
						new Point(Dpi.Width1, 0),
						new Point(Dpi.Width5, Dpi.Height4),
						new Point(Dpi.Width1, Dpi.Height8)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(Dpi.Width5, 0),
						new Point(Dpi.Width9, Dpi.Height4),
						new Point(Dpi.Width5, Dpi.Height8)
					});
					val3.CloseFigure();
				}
				else
				{
					val3.AddLines(new Point[3]
					{
						new Point(Dpi.Width1, 0),
						new Point(Dpi.Width8, 0),
						new Point(Dpi.Width4, Dpi.Height4)
					});
					val3.CloseFigure();
					val3.AddLines(new Point[3]
					{
						new Point(Dpi.Width1, Dpi.Height4),
						new Point(Dpi.Width8, Dpi.Height4),
						new Point(Dpi.Width4, Dpi.Height8)
					});
					val3.CloseFigure();
				}
				new Rectangle(0, 0, Dpi.Width10, Dpi.Height10);
				DisplayHelp.FillPath(val2, val3, Ⱙ.ThumbSignBackground);
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

	private Office2007ScrollBarStateColorTable ⰱ(Office2007ScrollBarColorTable ዅ, ePageNavigatorPart _070B)
	{
		if (!GetEnabled())
		{
			return ዅ.Disabled;
		}
		if (Ⱋ == _070B)
		{
			return ዅ.Pressed;
		}
		if (ᘅ == _070B)
		{
			return ዅ.MouseOver;
		}
		return ዅ.Default;
	}

	private Rectangle ⰲ(Rectangle ल)
	{
		ल.X += (ल.Width - 10) / 2 + 1;
		ल.Y += (ल.Height - 10) / 2 + 1;
		if (ल.X < 0)
		{
			ल.X = 0;
		}
		if (ल.Y < 0)
		{
			ल.Y = 0;
		}
		return ल;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (GetEnabled() && !DesignMode && Ⱋ == ePageNavigatorPart.None)
		{
			ᘅ = HitTest(objArg.Location);
		}
		base.InternalMouseMove(objArg);
	}

	public ePageNavigatorPart HitTest(Point pt)
	{
		if (_Ⱌ.Contains(pt))
		{
			return ePageNavigatorPart.PreviousPage;
		}
		if (_Ⱎ.Contains(pt))
		{
			return ePageNavigatorPart.NextPage;
		}
		if (_Ⱍ.Contains(pt))
		{
			return ePageNavigatorPart.Today;
		}
		return ePageNavigatorPart.None;
	}

	public override void InternalMouseLeave()
	{
		_24D9();
		ᘅ = ePageNavigatorPart.None;
		Ⱋ = ePageNavigatorPart.None;
		base.InternalMouseLeave();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && GetEnabled() && !DesignMode)
		{
			Ⱋ = HitTest(objArg.Location);
			if (Ⱋ != 0)
			{
				switch (Ⱋ)
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
		_24D9();
		Ⱋ = ePageNavigatorPart.None;
		base.InternalMouseUp(objArg);
	}

	protected virtual void OnNavigatePreviousPage()
	{
		if (Ⱄ != null)
		{
			Ⱄ(this, EventArgs.Empty);
		}
		ⰳ();
	}

	protected virtual void OnNavigateToday()
	{
		if (Ⱃ != null)
		{
			Ⱃ(this, EventArgs.Empty);
		}
	}

	protected virtual void OnNavigateNextPage()
	{
		if (Ⱂ != null)
		{
			Ⱂ(this, EventArgs.Empty);
		}
		ⰳ();
	}

	private void _0A55(object _٠, EventArgs ؿ)
	{
		switch (Ⱋ)
		{
		case ePageNavigatorPart.PreviousPage:
			OnNavigatePreviousPage();
			break;
		case ePageNavigatorPart.NextPage:
			OnNavigateNextPage();
			break;
		}
		if (_ᶜ != null)
		{
			_Ⱕ++;
			if (_Ⱕ > 4 && _ᶜ.Interval > 20)
			{
				Timer ᶜ = _ᶜ;
				ᶜ.Interval -= 10;
			}
		}
	}

	private void ⰳ()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (ClickRepeatInterval > 0 && _ᶜ == null)
		{
			_ᶜ = new Timer();
			_ᶜ.Interval = ClickRepeatInterval;
			_ᶜ.Tick += _0A55;
			_ᶜ.Start();
			_Ⱕ = 0;
		}
	}

	private void _24D9()
	{
		if (_ᶜ != null)
		{
			_ᶜ.Tick -= _0A55;
			_ᶜ.Stop();
			((Component)(object)_ᶜ).Dispose();
			_ᶜ = null;
		}
	}

	protected override void OnTooltipChanged()
	{
		_24DC();
		base.OnTooltipChanged();
	}

	private void _24DC()
	{
		if (DesignMode)
		{
			return;
		}
		string text = "";
		switch (ᘅ)
		{
		case ePageNavigatorPart.PreviousPage:
			text = _Ⱏ;
			break;
		case ePageNavigatorPart.Today:
			text = _Ⱐ;
			break;
		case ePageNavigatorPart.NextPage:
			text = _Ⱑ;
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
			pageNavigatorItem.PreviousPageTooltip = _Ⱏ;
			pageNavigatorItem.TodayTooltip = _Ⱐ;
			pageNavigatorItem.NextPageTooltip = _Ⱑ;
		}
	}

	protected override void Dispose(bool disposing)
	{
		_24D9();
		ⰴ(ref _Ⱒ);
		ⰴ(ref _Ⱓ);
		ⰴ(ref _Ⱔ);
		base.Dispose(disposing);
	}

	private void ⰴ(ref Bitmap _1DD1)
	{
		if (_1DD1 != null)
		{
			((Image)_1DD1).Dispose();
			_1DD1 = null;
		}
	}
}
