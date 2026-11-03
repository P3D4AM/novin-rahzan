using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[Browsable(false)]
public class SliderItem : BaseItem
{
	private int ᘘ = 100;

	private int ᘗ;

	private int ᘛ;

	private int Ṕ = 1;

	private bool _24C7 = true;

	private int ܘ = 136;

	private int _24C8 = 38;

	private eSliderLabelPosition _24C9;

	private eSliderPart ᘋ;

	private eSliderPart _24CA;

	private int _24CB = 18;

	private Size _24CC = new Size(11, 15);

	private bool _24CD = true;

	private Color ޗ = Color.Empty;

	private string _24CE = "";

	private string _24CF = "";

	private string _24D0 = "";

	private EventHandler ᗲ;

	private CancelIntValueEventHandler _18AB;

	private EventHandler _24D1;

	private EventHandler _24D2;

	private Timer _৮;

	private Rectangle _24D3 = Rectangle.Empty;

	private Rectangle _24D4 = Rectangle.Empty;

	private Rectangle _24D5 = Rectangle.Empty;

	private Rectangle _24D6 = Rectangle.Empty;

	private Rectangle _24D7 = Rectangle.Empty;

	private bool __0EB9 = true;

	private eOrientation __24D8;

	private Office2007SliderColorTable __137C;

	internal eSliderPart ᘅ
	{
		get
		{
			return ᘋ;
		}
		set
		{
			ᘋ = value;
			_24DC();
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSliderPart MouseDownPart
	{
		get
		{
			return _24CA;
		}
		set
		{
			_24CA = value;
			Refresh();
		}
	}

	internal Rectangle _24C2 => _24D3;

	internal Rectangle _24C3 => _24D4;

	internal Rectangle _24C4 => _24D5;

	internal Rectangle _24C5 => _24D6;

	internal Rectangle _24C6
	{
		get
		{
			return _24D7;
		}
		set
		{
			_24D7 = value;
		}
	}

	[Localizable(true)]
	[DefaultValue("")]
	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DevCoBrowsable(true)]
	[Description("The text contained in the item.")]
	[Category("Appearance")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Description("Gets or sets the maximum value of the range of the control.")]
	[DefaultValue(100)]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int Maximum
	{
		get
		{
			return ᘘ;
		}
		set
		{
			ᘘ = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[Description("Gets or sets the minimum value of the range of the control.")]
	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Behavior")]
	[DevCoBrowsable(true)]
	public int Minimum
	{
		get
		{
			return ᘗ;
		}
		set
		{
			ᘗ = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets the current position of the slider.")]
	[Browsable(true)]
	public int Value
	{
		get
		{
			return ᘛ;
		}
		set
		{
			if (ᘛ != value)
			{
				ᘩ(value, eEventSource.Code);
			}
		}
	}

	[DefaultValue(1)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	[DevCoBrowsable(true)]
	public int Step
	{
		get
		{
			return Ṕ;
		}
		set
		{
			if (Ṕ > 0)
			{
				Ṕ = value;
			}
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Gets or sets whether the label text is displayed.")]
	public bool LabelVisible
	{
		get
		{
			return _24C7;
		}
		set
		{
			_24C7 = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[DefaultValue(136)]
	[Category("Layout")]
	[Description("Indicates the width of the slider part of the item in pixels.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public int Width
	{
		get
		{
			return ܘ;
		}
		set
		{
			if (ܘ != value && value > 0)
			{
				ܘ = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[DefaultValue(38)]
	[Description("Indicates width of the label part of the item in pixels.")]
	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int LabelWidth
	{
		get
		{
			return _24C8;
		}
		set
		{
			if (_24C8 != value && value > 0)
			{
				_24C8 = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Category("Layout")]
	[DefaultValue(eSliderLabelPosition.Left)]
	[Description("Indicates text label position in relationship to the slider")]
	[Browsable(true)]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return _24C9;
		}
		set
		{
			_24C9 = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Category("Behavior")]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Browsable(false)]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
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

	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
	[DevCoBrowsable(true)]
	[DefaultValue(200)]
	[Category("Behavior")]
	[Browsable(true)]
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

	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public virtual bool TrackMarker
	{
		get
		{
			return _24CD;
		}
		set
		{
			_24CD = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the label text.")]
	[Browsable(true)]
	public Color TextColor
	{
		get
		{
			return ޗ;
		}
		set
		{
			ޗ = value;
			Refresh();
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Browsable(false)]
	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[Description("Indicates list of shortcut keys for this item.")]
	[Editor("DevComponents.DotNetBar.Design.ShortcutsDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Design")]
	[TypeConverter("DevComponents.DotNetBar.Design.ShortcutsConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
	[Browsable(false)]
	[DevCoBrowsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[DefaultValue(true)]
	[DevCoBrowsable(false)]
	[Description("Determines whether sub-items are displayed.")]
	[Category("Behavior")]
	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Browsable(false)]
	[Category("Appearance")]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[DevCoBrowsable(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
	[DefaultValue(false)]
	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(true)]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	public string IncreaseTooltip
	{
		get
		{
			return _24CF;
		}
		set
		{
			_24CF = value;
			_24DC();
		}
	}

	[DefaultValue("")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	[Browsable(true)]
	[Localizable(true)]
	[Category("Appearance")]
	public string DecreaseTooltip
	{
		get
		{
			return _24CE;
		}
		set
		{
			_24CE = value;
			_24DC();
		}
	}

	[Description("Indicates slider orientation.")]
	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	public eOrientation SliderOrientation
	{
		get
		{
			return __24D8;
		}
		set
		{
			if (__24D8 != value)
			{
				__24D8 = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Office2007SliderColorTable ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
			Refresh();
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelIntValueEventHandler ValueChanging
	{
		add
		{
			CancelIntValueEventHandler cancelIntValueEventHandler = _18AB;
			CancelIntValueEventHandler cancelIntValueEventHandler2;
			do
			{
				cancelIntValueEventHandler2 = cancelIntValueEventHandler;
				CancelIntValueEventHandler value2 = (CancelIntValueEventHandler)Delegate.Combine(cancelIntValueEventHandler2, value);
				cancelIntValueEventHandler = Interlocked.CompareExchange(ref _18AB, value2, cancelIntValueEventHandler2);
			}
			while ((object)cancelIntValueEventHandler != cancelIntValueEventHandler2);
		}
		remove
		{
			CancelIntValueEventHandler cancelIntValueEventHandler = _18AB;
			CancelIntValueEventHandler cancelIntValueEventHandler2;
			do
			{
				cancelIntValueEventHandler2 = cancelIntValueEventHandler;
				CancelIntValueEventHandler value2 = (CancelIntValueEventHandler)Delegate.Remove(cancelIntValueEventHandler2, value);
				cancelIntValueEventHandler = Interlocked.CompareExchange(ref _18AB, value2, cancelIntValueEventHandler2);
			}
			while ((object)cancelIntValueEventHandler != cancelIntValueEventHandler2);
		}
	}

	public event EventHandler IncreaseButtonClick
	{
		add
		{
			EventHandler eventHandler = _24D1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _24D1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler DecreaseButtonClick
	{
		add
		{
			EventHandler eventHandler = _24D2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _24D2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SliderItem()
		: this("", "")
	{
	}

	public SliderItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public SliderItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		ClickRepeatInterval = 200;
		MouseUpNotification = true;
		MouseDownCapture = true;
	}

	public override BaseItem Copy()
	{
		SliderItem sliderItem = new SliderItem(m_Name);
		CopyToItem(sliderItem);
		return sliderItem;
	}

	internal void _0EBA(SliderItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SliderItem sliderItem = copy as SliderItem;
		base.CopyToItem(sliderItem);
		sliderItem.Maximum = Maximum;
		sliderItem.Minimum = Minimum;
		sliderItem.Value = Value;
		sliderItem.Text = Text;
		sliderItem.LabelWidth = LabelWidth;
		sliderItem.LabelVisible = LabelVisible;
		sliderItem.LabelPosition = LabelPosition;
		sliderItem.Width = Width;
		sliderItem._24D1 = _24D1;
		sliderItem._24D2 = _24D2;
		sliderItem.ᗲ = ᗲ;
		sliderItem._18AB = _18AB;
	}

	protected override void Dispose(bool disposing)
	{
		_24D9();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			SliderItemRendererEventArgs sliderItemRendererEventArgs = new SliderItemRendererEventArgs(this, p.Graphics);
			sliderItemRendererEventArgs.က = p;
			ໝ.DrawSliderItem(sliderItemRendererEventArgs);
		}
		else
		{
			_24C1 _24DB = ᣓ._1924(this);
			if (_24DB != null)
			{
				SliderItemRendererEventArgs sliderItemRendererEventArgs2 = new SliderItemRendererEventArgs(this, p.Graphics);
				sliderItemRendererEventArgs2.က = p;
				_24DB.Paint(sliderItemRendererEventArgs2);
			}
		}
		if (DesignMode && Focused)
		{
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, rect, p.Colors.ItemDesignTimeBorder);
		}
	}

	public override void RecalcSize()
	{
		Font val = _໘(null);
		int num = Dpi.Height(_24CB);
		Size size = new Size(Dpi.Width(ܘ), num);
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 == null || val2.Disposing || val2.IsDisposed)
		{
			return;
		}
		Graphics val3 = BarFunctions.CreateGraphics(val2);
		if (val3 == null)
		{
			return;
		}
		try
		{
			if (_24C7)
			{
				Size size2 = _20B1._20B2(this, val3, size.Width, val, eTextFormat.Default, IsRightToLeft);
				if (_24C9 == eSliderLabelPosition.Left || _24C9 == eSliderLabelPosition.Right)
				{
					size.Height = Math.Max(num, Math.Max(size2.Height, val.Height));
					size.Width += Dpi.Width(_24C8);
				}
				else
				{
					size.Height = num + Math.Max(size2.Height, val.Height);
				}
			}
			if (__24D8 == eOrientation.Vertical)
			{
				size = new Size(size.Height, size.Width);
			}
			m_Rect.Size = size;
			ᘥ();
			base.RecalcSize();
		}
		finally
		{
			val3.Dispose();
		}
	}

	public eSliderPart HitTest(int x, int y)
	{
		eSliderPart result = eSliderPart.None;
		Rectangle rectangle = _24C3;
		rectangle.Offset(Bounds.Location);
		if (rectangle.Contains(x, y))
		{
			result = eSliderPart.DecreaseButton;
		}
		else
		{
			rectangle = _24C4;
			rectangle.Offset(Bounds.Location);
			if (rectangle.Contains(x, y))
			{
				result = eSliderPart.IncreaseButton;
			}
			else if (Bounds.Contains(x, y))
			{
				result = eSliderPart.TrackArea;
			}
		}
		return result;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (GetEnabled() && !DesignMode)
		{
			if (MouseDownPart == eSliderPart.None)
			{
				eSliderPart eSliderPart2 = HitTest(objArg.X, objArg.Y);
				if (ᘅ != eSliderPart2)
				{
					ᘅ = eSliderPart2;
				}
			}
			else if (MouseDownPart == eSliderPart.TrackArea)
			{
				if (SliderOrientation == eOrientation.Horizontal)
				{
					ᘩ(_24DA(objArg.X), eEventSource.Mouse);
				}
				else
				{
					ᘩ(_24DB(objArg.Y), eEventSource.Mouse);
				}
			}
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseLeave()
	{
		if (ᘋ != 0)
		{
			ᘅ = eSliderPart.None;
		}
		_24D9();
		if (MouseDownPart != 0)
		{
			MouseDownPart = eSliderPart.None;
		}
		base.InternalMouseLeave();
	}

	private void _24D9()
	{
		if (_৮ != null)
		{
			_৮.Stop();
			((Component)(object)_৮).Dispose();
			_৮ = null;
		}
	}

	protected virtual void OnIncreaseButtonClick(EventArgs e)
	{
		_24D1?.Invoke(this, e);
	}

	protected virtual void OnDecreaseButtonClick(EventArgs e)
	{
		_24D2?.Invoke(this, e);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		if (GetEnabled() && !DesignMode && (int)objArg.Button == 1048576)
		{
			eSliderPart eSliderPart3 = (MouseDownPart = HitTest(objArg.X, objArg.Y));
			if (MouseDownPart == eSliderPart.DecreaseButton)
			{
				ᘩ(Value - Step, eEventSource.Mouse);
				OnDecreaseButtonClick(EventArgs.Empty);
			}
			else if (MouseDownPart == eSliderPart.IncreaseButton)
			{
				PerformStep(eEventSource.Mouse);
				OnIncreaseButtonClick(EventArgs.Empty);
			}
			else if (eSliderPart3 == eSliderPart.TrackArea && !_24D7.Contains(objArg.X, objArg.Y))
			{
				if (SliderOrientation == eOrientation.Horizontal)
				{
					ᘩ(_24DA(objArg.X), eEventSource.Mouse);
				}
				else
				{
					ᘩ(_24DB(objArg.Y), eEventSource.Mouse);
				}
			}
			if (ClickRepeatInterval > 0 && (eSliderPart3 == eSliderPart.IncreaseButton || eSliderPart3 == eSliderPart.DecreaseButton))
			{
				if (_৮ == null)
				{
					_৮ = new Timer();
				}
				_৮.Interval = ClickRepeatInterval;
				_৮.Tick += _0A55;
				_৮.Start();
			}
		}
		base.InternalMouseDown(objArg);
	}

	private int _24DA(int و)
	{
		و -= (_24CC.Width - 2) / 2;
		Rectangle rectangle = _24C5;
		rectangle.Width -= _24CC.Width;
		rectangle.Offset(DisplayRectangle.Location);
		if (و <= rectangle.X)
		{
			if (IsRightToLeft)
			{
				return Maximum;
			}
			return Minimum;
		}
		if (و >= rectangle.Right)
		{
			if (IsRightToLeft)
			{
				return Minimum;
			}
			return Maximum;
		}
		float num = (float)(و - rectangle.X) / (float)(rectangle.Right - rectangle.X);
		if (IsRightToLeft)
		{
			num = 1f - num;
		}
		int num2 = Minimum + (int)((float)(Maximum - Minimum) * num);
		if (Step > 1 && Math.Ceiling((float)(num2 - Minimum) / (float)Step) * (double)Step != (double)(num2 - Minimum))
		{
			num2 = Minimum + (int)Math.Ceiling((float)(num2 - Minimum) / (float)Step) * Step;
		}
		return num2;
	}

	private int _24DB(int ى)
	{
		ى -= (_24CC.Width - 2) / 2;
		Rectangle rectangle = _24C5;
		rectangle.Height -= _24CC.Width;
		rectangle.Offset(DisplayRectangle.Location);
		if (ى <= rectangle.Y)
		{
			return Maximum;
		}
		if (ى >= rectangle.Bottom)
		{
			return Minimum;
		}
		float num = 1f - (float)(ى - rectangle.Y) / (float)(rectangle.Bottom - rectangle.Y);
		int num2 = Minimum + (int)((float)(Maximum - Minimum) * num);
		if (Step > 1 && Math.Ceiling((float)(num2 - Minimum) / (float)Step) * (double)Step != (double)(num2 - Minimum))
		{
			num2 = Minimum + (int)Math.Ceiling((float)(num2 - Minimum) / (float)Step) * Step;
		}
		return num2;
	}

	private void _0A55(object _٠, EventArgs ؿ)
	{
		if (MouseDownPart == eSliderPart.DecreaseButton)
		{
			ᘩ(Value - Step, eEventSource.Mouse);
			OnDecreaseButtonClick(EventArgs.Empty);
		}
		else if (MouseDownPart == eSliderPart.IncreaseButton)
		{
			PerformStep(eEventSource.Mouse);
			OnIncreaseButtonClick(EventArgs.Empty);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		_24D9();
		if (MouseDownPart != 0)
		{
			MouseDownPart = eSliderPart.None;
		}
		base.InternalMouseUp(objArg);
	}

	protected override void OnExternalSizeChange()
	{
		ᘥ();
		base.OnExternalSizeChange();
	}

	private void ᘥ()
	{
		bool isRightToLeft = IsRightToLeft;
		Rectangle rectangle = new Rectangle(Point.Empty, DisplayRectangle.Size);
		int num = Dpi.Width(_24CB);
		if (_24C7)
		{
			int num2 = Dpi.Width(_24C8);
			if ((_24C9 == eSliderLabelPosition.Left && (!isRightToLeft || __24D8 == eOrientation.Vertical)) || (_24C9 == eSliderLabelPosition.Right && isRightToLeft))
			{
				if (__24D8 == eOrientation.Vertical)
				{
					_24D3 = new Rectangle(0, 2, rectangle.Width, num2 - 2);
					rectangle.Height -= num2;
					rectangle.Y += num2;
				}
				else
				{
					_24D3 = new Rectangle(2, 0, num2 - 2, rectangle.Height);
					rectangle.Width -= num2;
					rectangle.X += num2;
				}
			}
			else if ((_24C9 == eSliderLabelPosition.Left && isRightToLeft) || (_24C9 == eSliderLabelPosition.Right && (!isRightToLeft || __24D8 == eOrientation.Vertical)))
			{
				if (__24D8 == eOrientation.Vertical)
				{
					_24D3 = new Rectangle(0, rectangle.Height - num2 + 2, rectangle.Width, num2 - 2);
					rectangle.Height -= num2;
				}
				else
				{
					_24D3 = new Rectangle(rectangle.Width - num2 + 2, 0, num2 - 2, rectangle.Height);
					rectangle.Width -= num2;
				}
			}
			else if (_24C9 == eSliderLabelPosition.Top)
			{
				Font val = _໘(null);
				int num3 = ((val != null) ? val.Height : 14);
				if (base.TextMarkupBody != null)
				{
					num3 = base.TextMarkupBody.Bounds.Height;
				}
				int num4 = num + num3;
				if (__24D8 == eOrientation.Vertical)
				{
					_24D3 = new Rectangle((rectangle.Width - num4) / 2, 0, num3, rectangle.Height);
					rectangle.X = _24D3.Right;
					rectangle.Width = num;
				}
				else
				{
					_24D3 = new Rectangle(0, (rectangle.Height - num4) / 2, rectangle.Width, num3);
					rectangle.Y = _24D3.Bottom;
					rectangle.Height = num;
				}
			}
			else if (_24C9 == eSliderLabelPosition.Bottom)
			{
				Font val2 = _໘(null);
				int num5 = ((val2 != null) ? val2.Height : 14);
				if (base.TextMarkupBody != null)
				{
					num5 = base.TextMarkupBody.Bounds.Height;
				}
				int num6 = num + num5;
				if (__24D8 == eOrientation.Vertical)
				{
					_24D3 = new Rectangle((rectangle.Width - num6) / 2 + num, 0, num5, rectangle.Height);
					rectangle.X = (rectangle.Width - num6) / 2;
					rectangle.Width = num;
				}
				else
				{
					_24D3 = new Rectangle(0, (rectangle.Height - num6) / 2 + num, rectangle.Width, num5);
					rectangle.Y = (rectangle.Height - num6) / 2;
					rectangle.Height = num;
				}
			}
		}
		else
		{
			_24D3 = Rectangle.Empty;
		}
		if (isRightToLeft && __24D8 == eOrientation.Horizontal)
		{
			_24D4 = new Rectangle(rectangle.Right - num, rectangle.Y + (rectangle.Height - num) / 2, num, num);
			_24D5 = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - num) / 2, num, num);
		}
		else if (__24D8 == eOrientation.Horizontal)
		{
			_24D4 = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - num) / 2, num, num);
			_24D5 = new Rectangle(rectangle.Right - num, rectangle.Y + (rectangle.Height - num) / 2, num, num);
		}
		else
		{
			_24D4 = new Rectangle(rectangle.X + (rectangle.Width - num) / 2, rectangle.Bottom - num, num, num);
			_24D5 = new Rectangle(rectangle.X + (rectangle.Width - num) / 2, rectangle.Y, num, num);
		}
		if (__24D8 == eOrientation.Horizontal)
		{
			_24D6 = new Rectangle(rectangle.X + num, rectangle.Y, rectangle.Width - num * 2, rectangle.Height);
		}
		else
		{
			_24D6 = new Rectangle(rectangle.X, rectangle.Y + num, rectangle.Width, rectangle.Height - num * 2);
		}
	}

	private Font _໘(ItemPaintArgs _0BD2)
	{
		Font val = null;
		if (_0BD2 != null)
		{
			val = _0BD2.Font;
		}
		if (val == null)
		{
			Control val2 = null;
			if (_0BD2 != null)
			{
				val2 = _0BD2.ContainerControl;
			}
			if (val2 == null)
			{
				object containerControl = ContainerControl;
				val2 = (Control)((containerControl is Control) ? containerControl : null);
			}
			val = ((val2 == null || val2.Font == null) ? SystemFonts.DefaultFont : val2.Font);
		}
		return val;
	}

	private void ᘩ(int ԝ, eEventSource ᎂ)
	{
		if (ԝ == ᘛ)
		{
			return;
		}
		CancelIntValueEventArgs cancelIntValueEventArgs = new CancelIntValueEventArgs();
		cancelIntValueEventArgs.NewValue = ԝ;
		cancelIntValueEventArgs.EventSource = ᎂ;
		OnValueChanging(cancelIntValueEventArgs);
		if (!cancelIntValueEventArgs.Cancel)
		{
			if (ԝ < ᘗ)
			{
				ᘛ = ᘗ;
			}
			else if (ԝ > ᘘ)
			{
				ᘛ = ᘘ;
			}
			else
			{
				ᘛ = ԝ;
			}
			OnValueChanged(cancelIntValueEventArgs);
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "Value");
			}
			Refresh();
			OnAppearanceChanged();
			ExecuteCommand();
		}
	}

	public virtual void PerformStep()
	{
		PerformStep(eEventSource.Code);
	}

	public virtual void PerformStep(eEventSource eventSource)
	{
		ᘩ(Value + Ṕ, eventSource);
	}

	public virtual void Increment(int value)
	{
		Increment(value, eEventSource.Code);
	}

	public virtual void Increment(int value, eEventSource eventSource)
	{
		ᘩ(Value + value, eventSource);
	}

	protected virtual void OnValueChanged()
	{
		OnValueChanged(EventArgs.Empty);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (ᗲ != null)
		{
			ᗲ(this, e);
		}
	}

	protected virtual void OnValueChanging(CancelIntValueEventArgs e)
	{
		if (_18AB != null)
		{
			_18AB(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !ޗ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected override void OnTooltipChanged()
	{
		_24D0 = Tooltip;
		_24DC();
		base.OnTooltipChanged();
	}

	private void _24DC()
	{
		if (!DesignMode)
		{
			if (ᘋ == eSliderPart.DecreaseButton && _24CE != "")
			{
				_24DD(_24CE);
			}
			else if (ᘋ == eSliderPart.IncreaseButton && _24CF != "")
			{
				_24DD(_24CF);
			}
			else if (m_Tooltip != _24D0)
			{
				_24DD(_24D0);
			}
		}
	}

	private void _24DD(string ዲ)
	{
		if (m_Tooltip != ዲ)
		{
			m_Tooltip = ዲ;
			if (base.ToolTipVisible)
			{
				ShowToolTip();
			}
		}
	}
}
