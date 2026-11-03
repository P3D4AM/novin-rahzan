using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[Browsable(false)]
[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class RangeSliderItem : BaseItem
{
	private RangeSliderValueChangingEventHandler _18AB;

	private EventHandler ᗲ;

	private int __24E8 = 4;

	private int __24E9 = 2;

	private eRangeSliderPart __24EA;

	private eRangeSliderPart __24EB;

	private ToolTip __24EC;

	private RangeTooltipEventHandler _24ED;

	private int _ᗶ;

	private int _ᗵ = 10;

	private static readonly RangeValue __24EE = new RangeValue(0, 10);

	private RangeValue _ᗸ = __24EE;

	private bool __24EF = true;

	private Rectangle __24F0 = Rectangle.Empty;

	private Rectangle __24F1 = Rectangle.Empty;

	private Rectangle __24F2 = Rectangle.Empty;

	private Rectangle __24F3 = Rectangle.Empty;

	private int __24F4 = 1;

	private eOrientation __24D8;

	private int _ች = 140;

	private int _ᶗ = 24;

	private static readonly Size _24F5 = new Size(7, 17);

	private Size __24F6 = _24F5;

	private eTicksPosition __24F7 = eTicksPosition.Bottom;

	private int __24F8;

	private string __24F9 = "Min: {0} - Max: {1}";

	private bool __24FA = true;

	private Image __24FB;

	private Image __24FC;

	private int __24FD = 7;

	private bool __24FE = true;

	private Color __24FF = Color.Empty;

	[Browsable(false)]
	public eRangeSliderPart MouseOverPart
	{
		get
		{
			return __24EA;
		}
		private set
		{
			if (__24EA != value)
			{
				__24EA = value;
				Refresh();
			}
		}
	}

	[Browsable(false)]
	public eRangeSliderPart MouseDownPart
	{
		get
		{
			return __24EB;
		}
		private set
		{
			if (__24EB != value)
			{
				__24EB = value;
				Refresh();
			}
		}
	}

	protected override bool ShowToolTips => __24EC == null;

	[Category("Behavior")]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return _ᗶ;
		}
		set
		{
			if (value != _ᗶ)
			{
				int ᗶ = _ᗶ;
				_ᗶ = value;
				OnMinimumChanged(ᗶ, value);
			}
		}
	}

	[DefaultValue(10)]
	[Category("Behavior")]
	[Description("Gets or sets the maximum value of the range of the control.")]
	public int Maximum
	{
		get
		{
			return _ᗵ;
		}
		set
		{
			if (value != _ᗵ)
			{
				int ᗵ = _ᗵ;
				_ᗵ = value;
				OnMaximumChanged(ᗵ, value);
			}
		}
	}

	[Description("Specifies range displayed by the control.")]
	[Category("Data")]
	public RangeValue Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			if (value.Min != _ᗸ.Min || value.Max != _ᗸ.Max)
			{
				SetValue(value, eEventSource.Code);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether tick lines are shown")]
	public bool TicksVisible
	{
		get
		{
			return __24EF;
		}
		set
		{
			if (value != __24EF)
			{
				bool oldValue = __24EF;
				__24EF = value;
				OnTicksVisibleChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public Rectangle TicksBounds => __24F0;

	[Browsable(false)]
	public Rectangle TicksBounds2 => __24F1;

	[Browsable(false)]
	public Rectangle RangeButtonMinBounds => __24F2;

	[Browsable(false)]
	public Rectangle RangeButtonMaxBounds => __24F3;

	[Description("Indicates tick display period")]
	[Category("Appearance")]
	[DefaultValue(1)]
	public int TicksStep
	{
		get
		{
			return __24F4;
		}
		set
		{
			if (value != __24F4)
			{
				int oldValue = __24F4;
				__24F4 = value;
				OnTicksStepChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates slider orientation.")]
	[DefaultValue(eOrientation.Horizontal)]
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

	[DefaultValue(140)]
	[Description("Indicates the width of range slider in pixels.")]
	[Category("Layout")]
	[Browsable(true)]
	public int Width
	{
		get
		{
			return _ች;
		}
		set
		{
			if (_ች != value && value > 0)
			{
				_ች = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Description("Indicates the Height of range slider in pixels.")]
	[DefaultValue(24)]
	[Browsable(true)]
	[Category("Layout")]
	public int Height
	{
		get
		{
			return _ᶗ;
		}
		set
		{
			if (_ᶗ != value && value > 0)
			{
				_ᶗ = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Description("Indicates the size of the range change buttons.")]
	[Category("Appearance")]
	public Size RangeButtonSize
	{
		get
		{
			return __24F6;
		}
		set
		{
			if (value != __24F6)
			{
				Size oldValue = __24F6;
				__24F6 = value;
				OnRangeButtonSizeChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(eTicksPosition.Bottom)]
	[Description("Specifies the ticks position inside of Range Slider.")]
	[Category("Appearance")]
	public eTicksPosition TicksPosition
	{
		get
		{
			return __24F7;
		}
		set
		{
			if (value != __24F7)
			{
				eTicksPosition oldValue = __24F7;
				__24F7 = value;
				OnTicksPositionChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
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

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Specifies minimum absolute range that user can select. Absolute range is defined as Abs(Value.Max-Value.Min) Applies to user performed selection through mouse only.")]
	public int MinimumAbsoluteRange
	{
		get
		{
			return __24F8;
		}
		set
		{
			if (value != __24F8)
			{
				int oldValue = __24F8;
				__24F8 = value;
				OnMinimumAbsoluteRangeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicate string that is used to format the range value to be displayed while user moves the range buttons. Value set here is used in string.Format(RangeTooltipFormat, Value.Min, Value.Max).")]
	[Category("Behavior")]
	[Localizable(true)]
	[DefaultValue("{0} - {1}")]
	public string RangeTooltipFormat
	{
		get
		{
			return __24F9;
		}
		set
		{
			if (value != __24F9)
			{
				string oldValue = __24F9;
				__24F9 = value;
				OnRangeTooltipFormatChanged(oldValue, value);
			}
		}
	}

	[Description("Specifies whether range tooltip is shown while user is changing the range")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowRangeTooltip
	{
		get
		{
			return __24FA;
		}
		set
		{
			__24FA = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates image to be used as minimum range slider button instead of built-in button. Image is scaled to size set by RangeButtonSize.")]
	[DefaultValue(null)]
	public Image MinRangeSliderImage
	{
		get
		{
			return __24FB;
		}
		set
		{
			if (value != __24FB)
			{
				Image oldValue = __24FB;
				__24FB = value;
				OnMinRangeSliderImageChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates image to be used as maximum range slider button instead of built-in button. Image is scaled to size set by RangeButtonSize.")]
	public Image MaxRangeSliderImage
	{
		get
		{
			return __24FC;
		}
		set
		{
			if (value != __24FC)
			{
				Image oldValue = __24FC;
				__24FC = value;
				OnMaxRangeSliderImageChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(7)]
	[Description("Specifies the height of the range line")]
	[Category("Appearance")]
	public int RangeLineHeight
	{
		get
		{
			return __24FD;
		}
		set
		{
			if (value != __24FD)
			{
				int oldValue = __24FD;
				__24FD = value;
				OnRangeLineHeightChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether clicking the area outside of the range change buttons moves the range change button to the clicked location if possible thus allowing range change.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ClientClickRangeChangeEnabled
	{
		get
		{
			return __24FE;
		}
		set
		{
			__24FE = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of range value.")]
	public Color RangeValueColor
	{
		get
		{
			return __24FF;
		}
		set
		{
			__24FF = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("Occurs before Value has changed and allow the cancellation of the change.")]
	public event RangeSliderValueChangingEventHandler ValueChanging
	{
		add
		{
			RangeSliderValueChangingEventHandler rangeSliderValueChangingEventHandler = _18AB;
			RangeSliderValueChangingEventHandler rangeSliderValueChangingEventHandler2;
			do
			{
				rangeSliderValueChangingEventHandler2 = rangeSliderValueChangingEventHandler;
				RangeSliderValueChangingEventHandler value2 = (RangeSliderValueChangingEventHandler)Delegate.Combine(rangeSliderValueChangingEventHandler2, value);
				rangeSliderValueChangingEventHandler = Interlocked.CompareExchange(ref _18AB, value2, rangeSliderValueChangingEventHandler2);
			}
			while ((object)rangeSliderValueChangingEventHandler != rangeSliderValueChangingEventHandler2);
		}
		remove
		{
			RangeSliderValueChangingEventHandler rangeSliderValueChangingEventHandler = _18AB;
			RangeSliderValueChangingEventHandler rangeSliderValueChangingEventHandler2;
			do
			{
				rangeSliderValueChangingEventHandler2 = rangeSliderValueChangingEventHandler;
				RangeSliderValueChangingEventHandler value2 = (RangeSliderValueChangingEventHandler)Delegate.Remove(rangeSliderValueChangingEventHandler2, value);
				rangeSliderValueChangingEventHandler = Interlocked.CompareExchange(ref _18AB, value2, rangeSliderValueChangingEventHandler2);
			}
			while ((object)rangeSliderValueChangingEventHandler != rangeSliderValueChangingEventHandler2);
		}
	}

	[Description("Occurs after Value property has changed.")]
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

	[Description("Occurs when control is about to display the range tooltip and it allows you to customize tooltip")]
	public event RangeTooltipEventHandler RangeTooltipText
	{
		add
		{
			RangeTooltipEventHandler rangeTooltipEventHandler = _24ED;
			RangeTooltipEventHandler rangeTooltipEventHandler2;
			do
			{
				rangeTooltipEventHandler2 = rangeTooltipEventHandler;
				RangeTooltipEventHandler value2 = (RangeTooltipEventHandler)Delegate.Combine(rangeTooltipEventHandler2, value);
				rangeTooltipEventHandler = Interlocked.CompareExchange(ref _24ED, value2, rangeTooltipEventHandler2);
			}
			while ((object)rangeTooltipEventHandler != rangeTooltipEventHandler2);
		}
		remove
		{
			RangeTooltipEventHandler rangeTooltipEventHandler = _24ED;
			RangeTooltipEventHandler rangeTooltipEventHandler2;
			do
			{
				rangeTooltipEventHandler2 = rangeTooltipEventHandler;
				RangeTooltipEventHandler value2 = (RangeTooltipEventHandler)Delegate.Remove(rangeTooltipEventHandler2, value);
				rangeTooltipEventHandler = Interlocked.CompareExchange(ref _24ED, value2, rangeTooltipEventHandler2);
			}
			while ((object)rangeTooltipEventHandler != rangeTooltipEventHandler2);
		}
	}

	public RangeSliderItem()
		: this("", "")
	{
	}

	public RangeSliderItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public RangeSliderItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		MouseUpNotification = true;
		MouseDownCapture = true;
	}

	public override BaseItem Copy()
	{
		RangeSliderItem rangeSliderItem = new RangeSliderItem(m_Name);
		CopyToItem(rangeSliderItem);
		return rangeSliderItem;
	}

	internal void _0EBA(SliderItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RangeSliderItem rangeSliderItem = copy as RangeSliderItem;
		base.CopyToItem(rangeSliderItem);
		rangeSliderItem.Maximum = Maximum;
		rangeSliderItem.Minimum = Minimum;
	}

	protected override void Dispose(bool disposing)
	{
		_250C();
		base.Dispose(disposing);
	}

	protected virtual void OnValueChanging(RangeSliderValueChangingEventArgs e)
	{
		_18AB?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			RangeSliderItemRendererEventArgs rangeSliderItemRendererEventArgs = new RangeSliderItemRendererEventArgs(this, p.Graphics);
			rangeSliderItemRendererEventArgs.က = p;
			ໝ.DrawRangeSliderItem(rangeSliderItemRendererEventArgs);
		}
		else
		{
			_24E7 _24E7 = ᣓ._1926(this);
			if (_24E7 != null)
			{
				RangeSliderItemRendererEventArgs rangeSliderItemRendererEventArgs2 = new RangeSliderItemRendererEventArgs(this, p.Graphics);
				rangeSliderItemRendererEventArgs2.က = p;
				_24E7.Paint(rangeSliderItemRendererEventArgs2);
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
		m_Rect.Size = new Size(_ች, _ᶗ);
		_2501();
		_2500();
		base.RecalcSize();
	}

	private void _2500()
	{
		Size size = Dpi.Size(__24F6);
		int num = Dpi.Height(__24E8);
		int num2 = Dpi.Height(__24E9);
		if (__24D8 == eOrientation.Horizontal)
		{
			int ᶗ = _ᶗ;
			if (__24F7 == eTicksPosition.Top)
			{
				__24F2 = new Rectangle(0, num + num2 + Math.Max(0, (ᶗ - (num + size.Height + num2)) / 2), size.Width, size.Height);
			}
			else if (__24F7 == eTicksPosition.Bottom)
			{
				__24F2 = new Rectangle(0, Math.Max(0, (ᶗ - (num + size.Height + num2)) / 2), size.Width, size.Height);
			}
			else if (__24F7 == eTicksPosition.TopAndBottom)
			{
				__24F2 = new Rectangle(0, num + num2 + (ᶗ - (num * 2 + size.Height + num2 * 2)) / 2, size.Width, size.Height);
			}
			__24F3 = __24F2;
			RangeValue rangeValue = _2508();
			RangeValue rangeValue2 = _2507();
			double num3 = _2502(__24F0.Width, rangeValue.Min, rangeValue.Max, (!__24EF) ? 1 : __24F4);
			__24F2.X = __24F0.X + (int)((double)(Math.Abs(rangeValue2.Min - rangeValue.Min) / __24F4) * num3) - size.Width;
			__24F3.X = __24F0.X + (int)((double)Math.Abs((rangeValue2.Max - rangeValue.Min) / __24F4) * num3);
			if (rangeValue2.Max == rangeValue.Max)
			{
				__24F3.X = __24F0.Right - 1;
			}
		}
		else
		{
			int ች = _ች;
			if (__24F7 == eTicksPosition.Top)
			{
				__24F2 = new Rectangle(num + num2 + Math.Max(0, (ች - (num + size.Height + num2)) / 2), 0, size.Height, size.Width);
			}
			else if (__24F7 == eTicksPosition.Bottom)
			{
				__24F2 = new Rectangle(Math.Max(0, (ች - (num + size.Height + num2)) / 2), 0, size.Height, size.Width);
			}
			else if (__24F7 == eTicksPosition.TopAndBottom)
			{
				__24F2 = new Rectangle(num + num2 + (ች - (num * 2 + size.Height + num2 * 2)) / 2, 0, size.Height, size.Width);
			}
			__24F3 = __24F2;
			RangeValue rangeValue3 = _2508();
			RangeValue rangeValue4 = _2507();
			double num4 = _2502(__24F0.Height, rangeValue3.Min, rangeValue3.Max, __24F4);
			__24F2.Y = __24F0.Y + (int)((double)(Math.Abs(rangeValue4.Min - rangeValue3.Min) / __24F4) * num4) - size.Width;
			__24F3.Y = __24F0.Y + (int)((double)Math.Abs((rangeValue4.Max - rangeValue3.Min) / __24F4) * num4);
			if (rangeValue4.Max == rangeValue3.Max)
			{
				__24F3.Y = __24F0.Bottom - 1;
			}
		}
	}

	private void _2501()
	{
		__24F0 = Rectangle.Empty;
		__24F1 = Rectangle.Empty;
		Size size = Dpi.Size(__24F6);
		int num = Dpi.Height(__24E9);
		int num2 = Dpi.Height(__24E8);
		if (__24D8 == eOrientation.Horizontal)
		{
			if (__24F7 == eTicksPosition.Top)
			{
				__24F0 = new Rectangle(size.Width, (m_Rect.Height - (num2 + size.Height + num)) / 2, m_Rect.Width - size.Width * 2, num2);
			}
			else if (__24F7 == eTicksPosition.Bottom)
			{
				__24F0 = new Rectangle(size.Width, size.Height + num + (m_Rect.Height - (num2 + size.Height + num)) / 2, m_Rect.Width - size.Width * 2, num2);
			}
			else if (__24F7 == eTicksPosition.TopAndBottom)
			{
				__24F0 = new Rectangle(size.Width, (m_Rect.Height - (num2 * 2 + size.Height + num * 2)) / 2, m_Rect.Width - size.Width * 2, num2);
				__24F1 = new Rectangle(size.Width, size.Height + num2 + num * 2 + (m_Rect.Height - (num2 * 2 + size.Height + num * 2)) / 2, m_Rect.Width - size.Width * 2, num2);
			}
		}
		else if (__24F7 == eTicksPosition.Top)
		{
			__24F0 = new Rectangle((m_Rect.Width - (num2 + size.Height + num)) / 2, size.Width, num2, m_Rect.Height - size.Width * 2);
		}
		else if (__24F7 == eTicksPosition.Bottom)
		{
			__24F0 = new Rectangle(size.Height + num + (m_Rect.Width - (num2 + size.Height + num)) / 2, size.Width, num2, m_Rect.Height - size.Width * 2);
		}
		else if (__24F7 == eTicksPosition.TopAndBottom)
		{
			__24F0 = new Rectangle((m_Rect.Width - (num2 * 2 + size.Height + num * 2)) / 2, size.Width, num2, m_Rect.Height - size.Width * 2);
			__24F1 = new Rectangle(size.Height + num2 + num * 2 + (m_Rect.Width - (num2 * 2 + size.Height + num * 2)) / 2, size.Width, num2, m_Rect.Height - size.Width * 2);
		}
	}

	internal static double _2502(int _2503, int _2504, int _2505, int _2506)
	{
		if (_2505 == _2504)
		{
			return 1.0;
		}
		return (double)_2503 / ((double)Math.Abs(_2505 - _2504) / (double)_2506);
	}

	internal RangeValue _2507()
	{
		RangeValue value = Value;
		if (value.Min < Minimum)
		{
			value.Min = Minimum;
		}
		if (value.Max > Maximum)
		{
			value.Max = Maximum;
		}
		if (value.Max < value.Min)
		{
			value.Max = value.Min;
		}
		return value;
	}

	internal RangeValue _2508()
	{
		int minimum = Minimum;
		int num = Maximum;
		if (num < minimum)
		{
			num = minimum + 1;
		}
		return new RangeValue(minimum, num);
	}

	public override void InternalMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && !DesignMode)
		{
			MouseDownPart = HitTest(e.Location);
			if (MouseDownPart == eRangeSliderPart.TickArea && __24FE)
			{
				int num = 0;
				RangeValue rangeValue = _2508();
				if (__24D8 == eOrientation.Horizontal)
				{
					int num2 = e.X - (__24F0.X + Bounds.X);
					num = rangeValue.Min + (int)((double)num2 / _2502(__24F0.Width, rangeValue.Min, rangeValue.Max, (!__24EF) ? 1 : __24F4)) * __24F4;
				}
				else
				{
					int num3 = e.Y - (__24F0.Y + Bounds.Y);
					num = rangeValue.Min + (int)((double)num3 / _2502(__24F0.Height, rangeValue.Min, rangeValue.Max, (!__24EF) ? 1 : __24F4)) * __24F4;
				}
				if (num > rangeValue.Max)
				{
					num = rangeValue.Max;
				}
				if (num < rangeValue.Min)
				{
					num = rangeValue.Min;
				}
				RangeValue value = Value;
				if (num > Value.Max)
				{
					value = new RangeValue(Value.Min, num);
				}
				else if (num < Value.Min)
				{
					value = new RangeValue(num, Value.Max);
				}
				else if (Math.Abs(num - Value.Min) < Math.Abs(num - Value.Max) && num <= Value.Max)
				{
					value = new RangeValue(num, Value.Max);
				}
				else if (num >= Value.Min)
				{
					value = new RangeValue(Value.Min, num);
				}
				if (Math.Abs(value.Max - value.Min) >= __24F8)
				{
					SetValue(value, eEventSource.Mouse);
				}
			}
		}
		base.InternalMouseDown(e);
	}

	public override void InternalMouseMove(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Button == 1048576 && !DesignMode)
		{
			if (__24EB == eRangeSliderPart.MinRangeSlider)
			{
				if (__24D8 == eOrientation.Horizontal)
				{
					int num = e.X - (__24F0.X + Bounds.X);
					RangeValue rangeValue = _2508();
					double num2 = _2502(__24F0.Width, rangeValue.Min, rangeValue.Max, __24F4);
					int num3 = rangeValue.Min + (int)((double)num / num2) * __24F4;
					if (num3 > Value.Max)
					{
						num3 = Value.Max;
					}
					if (num3 < rangeValue.Min)
					{
						num3 = rangeValue.Min;
					}
					RangeValue value = new RangeValue(num3, Value.Max);
					if (Math.Abs(value.Max - value.Min) >= __24F8)
					{
						SetValue(value, eEventSource.Mouse);
					}
				}
				else
				{
					int num4 = e.Y - (__24F0.Y + Bounds.Y);
					RangeValue rangeValue2 = _2508();
					double num5 = _2502(__24F0.Height, rangeValue2.Min, rangeValue2.Max, __24F4);
					int num6 = rangeValue2.Min + (int)((double)num4 / num5) * __24F4;
					if (num6 > Value.Max)
					{
						num6 = Value.Max;
					}
					if (num6 < rangeValue2.Min)
					{
						num6 = rangeValue2.Min;
					}
					RangeValue value2 = new RangeValue(num6, Value.Max);
					if (Math.Abs(value2.Max - value2.Min) >= __24F8)
					{
						SetValue(value2, eEventSource.Mouse);
					}
				}
			}
			else if (__24EB == eRangeSliderPart.MaxRangeSlider)
			{
				if (__24D8 == eOrientation.Horizontal)
				{
					int num7 = e.X - (__24F0.X + Bounds.X);
					RangeValue rangeValue3 = _2508();
					double num8 = _2502(__24F0.Width, rangeValue3.Min, rangeValue3.Max, __24F4);
					int num9 = rangeValue3.Min + (int)((double)num7 / num8) * __24F4;
					if (num9 < Value.Min)
					{
						num9 = Value.Min;
					}
					if (num9 > rangeValue3.Max)
					{
						num9 = rangeValue3.Max;
					}
					RangeValue value3 = new RangeValue(Value.Min, num9);
					if (Math.Abs(value3.Max - value3.Min) >= __24F8)
					{
						SetValue(value3, eEventSource.Mouse);
					}
				}
				else
				{
					int num10 = e.Y - (__24F0.Y + Bounds.Y);
					RangeValue rangeValue4 = _2508();
					int num11 = rangeValue4.Min + (int)((double)num10 / _2502(__24F0.Height, rangeValue4.Min, rangeValue4.Max, __24F4)) * __24F4;
					if (num11 < Value.Min)
					{
						num11 = Value.Min;
					}
					if (num11 > rangeValue4.Max)
					{
						num11 = rangeValue4.Max;
					}
					RangeValue value4 = new RangeValue(Value.Min, num11);
					if (Math.Abs(value4.Max - value4.Min) >= __24F8)
					{
						SetValue(value4, eEventSource.Mouse);
					}
				}
			}
		}
		else if ((int)e.Button == 0 && !DesignMode)
		{
			MouseOverPart = HitTest(e.Location);
		}
		base.InternalMouseMove(e);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		MouseDownPart = eRangeSliderPart.None;
		_250C();
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseLeave()
	{
		MouseOverPart = eRangeSliderPart.None;
		base.InternalMouseLeave();
	}

	private void _2509()
	{
		if (__24EC == null)
		{
			__24EC = new ToolTip();
			__24EC.FixedLocation = _250A();
			__24EC.ShowDropShadow = false;
			__24EC.Style = EffectiveStyle;
			__24EC.Text = _250B();
			__24EC.ShowToolTip();
		}
		else
		{
			__24EC.FixedLocation = _250A();
			__24EC.Text = _250B();
		}
	}

	private Point _250A()
	{
		Point empty = Point.Empty;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Rectangle rectangle = __24F2;
			if (__24EB == eRangeSliderPart.MaxRangeSlider)
			{
				rectangle = __24F3;
			}
			rectangle.Offset(Bounds.Location);
			Point point = val.PointToScreen(rectangle.Location);
			Point point2 = val.PointToScreen(Bounds.Location);
			if (__24D8 == eOrientation.Horizontal)
			{
				empty.X = point.X;
				if (__24F7 == eTicksPosition.Top)
				{
					empty.Y = point2.Y - 18;
				}
				else
				{
					empty.Y = point2.Y + Bounds.Height + 3;
				}
			}
			else
			{
				empty.Y = point.Y;
				empty.X = point2.X + Bounds.Width + 3;
			}
		}
		return empty;
	}

	private string _250B()
	{
		string tooltip = string.Format(__24F9, Value.Min, Value.Max);
		RangeTooltipEventArgs rangeTooltipEventArgs = new RangeTooltipEventArgs(tooltip);
		OnRangeTooltipText(rangeTooltipEventArgs);
		return rangeTooltipEventArgs.Tooltip;
	}

	protected virtual void OnRangeTooltipText(RangeTooltipEventArgs e)
	{
		_24ED?.Invoke(this, e);
	}

	private void _250C()
	{
		if (__24EC != null)
		{
			((Control)__24EC).Hide();
			((Component)(object)__24EC).Dispose();
			__24EC = null;
		}
	}

	public eRangeSliderPart HitTest(Point p)
	{
		Rectangle rectangle = __24F2;
		rectangle.Offset(Bounds.Location);
		if (rectangle.Contains(p))
		{
			return eRangeSliderPart.MinRangeSlider;
		}
		rectangle = __24F3;
		rectangle.Offset(Bounds.Location);
		if (rectangle.Contains(p))
		{
			return eRangeSliderPart.MaxRangeSlider;
		}
		rectangle = __24F0;
		rectangle.Offset(Bounds.Location);
		if (__24D8 == eOrientation.Horizontal && p.X >= rectangle.X && p.X <= rectangle.Right)
		{
			return eRangeSliderPart.TickArea;
		}
		if (__24D8 == eOrientation.Vertical && p.Y >= rectangle.Y && p.Y <= rectangle.Bottom)
		{
			return eRangeSliderPart.TickArea;
		}
		return eRangeSliderPart.None;
	}

	protected virtual void OnMinimumChanged(int oldValue, int newValue)
	{
		Refresh();
		OnAppearanceChanged();
	}

	protected virtual void OnMaximumChanged(int oldValue, int newValue)
	{
		Refresh();
		OnAppearanceChanged();
	}

	public bool SetValue(RangeValue value, eEventSource source)
	{
		RangeSliderValueChangingEventArgs rangeSliderValueChangingEventArgs = new RangeSliderValueChangingEventArgs(source, value);
		OnValueChanging(rangeSliderValueChangingEventArgs);
		if (rangeSliderValueChangingEventArgs.Cancel)
		{
			return false;
		}
		RangeValue ᗸ = _ᗸ;
		_ᗸ = value;
		_2500();
		OnValueChanged(ᗸ, value);
		OnValueChanged(new EventSourceArgs(source));
		Refresh();
		OnAppearanceChanged();
		if (__24EB != 0 && __24FA)
		{
			_2509();
		}
		return true;
	}

	protected virtual void OnValueChanged(RangeValue oldValue, RangeValue newValue)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeValue()
	{
		if (_ᗸ.Min == __24EE.Min)
		{
			return _ᗸ.Max != __24EE.Max;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetValue()
	{
		Value = __24EE;
	}

	protected virtual void OnTicksVisibleChanged(bool oldValue, bool newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnTicksStepChanged(int oldValue, int newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRangeButtonSize()
	{
		return __24F6 != _24F5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRangeButtonSize()
	{
		RangeButtonSize = _24F5;
	}

	protected virtual void OnRangeButtonSizeChanged(Size oldValue, Size newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnTicksPositionChanged(eTicksPosition oldValue, eTicksPosition newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnMinimumAbsoluteRangeChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnRangeTooltipFormatChanged(string oldValue, string newValue)
	{
	}

	protected virtual void OnMinRangeSliderImageChanged(Image oldValue, Image newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnMaxRangeSliderImageChanged(Image oldValue, Image newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnRangeLineHeightChanged(int oldValue, int newValue)
	{
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRangeValueColor()
	{
		return !__24FF.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRangeValueColor()
	{
		RangeValueColor = Color.Empty;
	}
}
