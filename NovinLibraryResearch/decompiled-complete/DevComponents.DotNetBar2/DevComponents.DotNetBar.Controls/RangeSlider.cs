using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.RangeSliderDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("ValueChanged")]
[ToolboxBitmap(typeof(RangeSlider), "Controls.RangeSlider.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
public class RangeSlider : BaseItemControl
{
	private RangeSliderItem __2B60;

	private RangeSliderValueChangingEventHandler _18AB;

	private EventHandler ᗲ;

	private RangeTooltipEventHandler _24ED;

	private Rectangle __24F1 = Rectangle.Empty;

	internal RangeSliderItem _251F => __2B60;

	[Description("Indicates whether clicking the area outside of the range change buttons moves the range change button to the clicked location if possible thus allowing range change.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ClientClickRangeChangeEnabled
	{
		get
		{
			return __2B60.ClientClickRangeChangeEnabled;
		}
		set
		{
			__2B60.ClientClickRangeChangeEnabled = value;
		}
	}

	[DefaultValue(10)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	public int Maximum
	{
		get
		{
			return __2B60.Maximum;
		}
		set
		{
			__2B60.Maximum = value;
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return __2B60.Minimum;
		}
		set
		{
			__2B60.Minimum = value;
		}
	}

	[Description("Indicates image to be used as maximum range slider button instead of built-in button. Image is scaled to size set by RangeButtonSize.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image MaxRangeSliderImage
	{
		get
		{
			return __2B60.MaxRangeSliderImage;
		}
		set
		{
			__2B60.MaxRangeSliderImage = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(null)]
	[Description("Indicates image to be used as minimum range slider button instead of built-in button. Image is scaled to size set by RangeButtonSize.")]
	public Image MinRangeSliderImage
	{
		get
		{
			return __2B60.MinRangeSliderImage;
		}
		set
		{
			__2B60.MinRangeSliderImage = value;
		}
	}

	[Description("Specifies minimum absolute range that user can select. Absolute range is defined as Abs(Value.Max-Value.Min) Applies to user performed selection through mouse only.")]
	[DefaultValue(0)]
	[Category("Behavior")]
	public int MinimumAbsoluteRange
	{
		get
		{
			return __2B60.MinimumAbsoluteRange;
		}
		set
		{
			__2B60.MinimumAbsoluteRange = value;
		}
	}

	[Browsable(false)]
	public eRangeSliderPart MouseDownPart => __2B60.MouseDownPart;

	[Browsable(false)]
	public eRangeSliderPart MouseOverPart => __2B60.MouseOverPart;

	[Browsable(false)]
	public Rectangle RangeButtonMaxBounds => __2B60.RangeButtonMaxBounds;

	[Browsable(false)]
	public Rectangle RangeButtonMinBounds => __2B60.RangeButtonMinBounds;

	[Description("Indicates the size of the range change buttons.")]
	[Category("Appearance")]
	public Size RangeButtonSize
	{
		get
		{
			return __2B60.RangeButtonSize;
		}
		set
		{
			__2B60.RangeButtonSize = value;
		}
	}

	[Description("Specifies the height of the range line")]
	[DefaultValue(7)]
	[Category("Appearance")]
	public int RangeLineHeight
	{
		get
		{
			return __2B60.RangeLineHeight;
		}
		set
		{
			__2B60.RangeLineHeight = value;
		}
	}

	[DefaultValue("{0} - {1}")]
	[Description("Indicate string that is used to format the range value to be displayed while user moves the range buttons. Value set here is used in string.Format(RangeTooltipFormat, Value.Min, Value.Max).")]
	[Category("Behavior")]
	[Localizable(true)]
	public string RangeTooltipFormat
	{
		get
		{
			return __2B60.RangeTooltipFormat;
		}
		set
		{
			__2B60.RangeTooltipFormat = value;
		}
	}

	[Description("Indicates color of range value.")]
	[Category("Appearance")]
	public Color RangeValueColor
	{
		get
		{
			return __2B60.RangeValueColor;
		}
		set
		{
			__2B60.RangeValueColor = value;
		}
	}

	[Description("Specifies whether range tooltip is shown while user is changing the range")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowRangeTooltip
	{
		get
		{
			return __2B60.ShowRangeTooltip;
		}
		set
		{
			__2B60.ShowRangeTooltip = value;
		}
	}

	[Description("Indicates slider orientation.")]
	[Category("Appearance")]
	[DefaultValue(eOrientation.Horizontal)]
	public eOrientation SliderOrientation
	{
		get
		{
			return __2B60.SliderOrientation;
		}
		set
		{
			__2B60.SliderOrientation = value;
		}
	}

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Browsable(false)]
	public Rectangle TicksBounds => __2B60.TicksBounds;

	[Browsable(false)]
	public Rectangle TicksBounds2 => __2B60.TicksBounds2;

	[Description("Specifies the ticks position inside of Range Slider.")]
	[DefaultValue(eTicksPosition.Bottom)]
	[Category("Appearance")]
	public eTicksPosition TicksPosition
	{
		get
		{
			return __2B60.TicksPosition;
		}
		set
		{
			__2B60.TicksPosition = value;
		}
	}

	[Description("Indicates tick display period")]
	[DefaultValue(1)]
	[Category("Appearance")]
	public int TicksStep
	{
		get
		{
			return __2B60.TicksStep;
		}
		set
		{
			__2B60.TicksStep = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tick lines are shown")]
	public bool TicksVisible
	{
		get
		{
			return __2B60.TicksVisible;
		}
		set
		{
			__2B60.TicksVisible = value;
		}
	}

	[Category("Data")]
	[Description("Specifies range displayed by the control.")]
	public RangeValue Value
	{
		get
		{
			return __2B60.Value;
		}
		set
		{
			__2B60.Value = value;
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

	protected virtual void OnValueChanging(RangeSliderValueChangingEventArgs e)
	{
		_18AB?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	protected virtual void OnRangeTooltipText(RangeTooltipEventArgs e)
	{
		_24ED?.Invoke(this, e);
	}

	public RangeSlider()
	{
		__2B60 = new RangeSliderItem();
		__2B60.Style = eDotNetBarStyle.StyleManagerControlled;
		__2B60.ValueChanging += ⰸ;
		__2B60.ValueChanged += ⰷ;
		__2B60.RangeTooltipText += ⰶ;
		HostItem = __2B60;
	}

	private void ⰶ(object _٠, RangeTooltipEventArgs ؿ)
	{
		OnRangeTooltipText(ؿ);
	}

	private void ⰷ(object _٠, EventArgs ؿ)
	{
		OnValueChanged(ؿ);
	}

	private void ⰸ(object _٠, RangeSliderValueChangingEventArgs ؿ)
	{
		OnValueChanging(ؿ);
	}

	public override void RecalcLayout()
	{
		Rectangle itemBounds = GetItemBounds();
		__2B60.Width = itemBounds.Width;
		__2B60.Height = itemBounds.Height;
		base.RecalcLayout();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnHandleCreated(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRangeButtonSize()
	{
		return __2B60.ShouldSerializeRangeButtonSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRangeButtonSize()
	{
		__2B60.ResetRangeButtonSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeRangeValueColor()
	{
		return __2B60.ShouldSerializeRangeValueColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetRangeValueColor()
	{
		__2B60.ResetRangeValueColor();
	}
}
