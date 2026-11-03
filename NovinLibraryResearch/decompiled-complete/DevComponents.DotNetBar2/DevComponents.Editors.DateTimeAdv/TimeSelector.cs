using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

[ToolboxBitmap(typeof(DotNetBarManager), "TimeSelector.ico")]
[DefaultEvent("SelectedTimeChanged")]
[DefaultProperty("SelectedTime")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.TimeSelectorDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TimeSelector : ItemControl
{
	private TimeSelectorItem _២;

	private EventHandler _2668;

	private EventHandler _2669;

	private Size __21D2 = Size.Empty;

	[Description("Indicates text displayed on OK button.")]
	[Category("Appearance")]
	[DefaultValue("OK")]
	[Localizable(true)]
	public string OkText
	{
		get
		{
			return _២.OkText;
		}
		set
		{
			_២.OkText = value;
		}
	}

	[Description("Indicates whether Ok button is visible.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool OkButtonVisible
	{
		get
		{
			return _២.OkButtonVisible;
		}
		set
		{
			_២.OkButtonVisible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDateTime
	{
		get
		{
			return _២.SelectedDateTime;
		}
		set
		{
			_២.SelectedDateTime = value;
		}
	}

	[Description("Indicates selected time.")]
	[Category("Data")]
	public TimeSpan SelectedTime
	{
		get
		{
			return _២.SelectedTime;
		}
		set
		{
			_២.SelectedTime = value;
		}
	}

	[Description("Indicates time format used to present time by the selector.")]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue(eTimeSelectorFormat.System)]
	public eTimeSelectorFormat TimeFormat
	{
		get
		{
			return _២.TimeFormat;
		}
		set
		{
			_២.TimeFormat = value;
			__21D2 = Size.Empty;
			_21DF();
		}
	}

	[Description("Indicates format for the 12 Hour Time Display.")]
	[DefaultValue("hh:mm tt")]
	[Category("Data")]
	[Localizable(true)]
	public string TimeFormat12H
	{
		get
		{
			return _២.TimeFormat12H;
		}
		set
		{
			_២.TimeFormat12H = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates format for the 24 Hour Time Display.")]
	[DefaultValue("HH:mm")]
	[Category("Data")]
	public string TimeFormat24H
	{
		get
		{
			return _២.TimeFormat24H;
		}
		set
		{
			_២.TimeFormat24H = value;
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

	[EditorBrowsable(EditorBrowsableState.Always)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return base.AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				base.AutoSize = value;
				_21DF();
			}
		}
	}

	[Browsable(false)]
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

	[Description("Indicates the type of the selector used to select time.")]
	[Category("Appearance")]
	[DefaultValue(eTimeSelectorType.TouchStyle)]
	public eTimeSelectorType SelectorType
	{
		get
		{
			return _២.SelectorType;
		}
		set
		{
			_២.SelectorType = value;
			__21D2 = Size.Empty;
			_21DF();
		}
	}

	[Description("Indicates text displayed on Clear button only when MonthCalendarStyle is used.")]
	[Localizable(true)]
	[DefaultValue("Clear")]
	[Category("Appearance")]
	public string ClearText
	{
		get
		{
			return _២.ClearText;
		}
		set
		{
			_២.ClearText = value;
		}
	}

	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates text displayed on Hour label.")]
	[DefaultValue("Hour")]
	public string HourText
	{
		get
		{
			return _២.HourText;
		}
		set
		{
			_២.HourText = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates text displayed on Minute label.")]
	[DefaultValue("Minute")]
	[Category("Appearance")]
	public string MinuteText
	{
		get
		{
			return _២.MinuteText;
		}
		set
		{
			_២.MinuteText = value;
		}
	}

	[Description("Occurs after SelectedTime changes.")]
	public event EventHandler SelectedTimeChanged
	{
		add
		{
			EventHandler eventHandler = _2668;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2668, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2668;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2668, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when OK button is clicked.")]
	public event EventHandler OkClick
	{
		add
		{
			EventHandler eventHandler = _2669;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2669, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2669;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2669, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnSelectedTimeChanged(EventArgs e)
	{
		_2668?.Invoke(this, e);
	}

	protected virtual void OnOkClick(EventArgs e)
	{
		_2669?.Invoke(this, e);
	}

	public TimeSelector()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_២ = new TimeSelectorItem();
		_២.GlobalItem = false;
		_២.ContainerControl = this;
		_២.Stretch = false;
		_២.Displayed = true;
		_២.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		_២.SetOwner(this);
		_២.SelectedTimeChanged += ⲟ;
		_២.OkClick += Ⲟ;
		SetBaseItemContainer(_២);
	}

	private void Ⲟ(object _٠, EventArgs ؿ)
	{
		OnOkClick(ؿ);
	}

	private void ⲟ(object _٠, EventArgs ؿ)
	{
		OnSelectedTimeChanged(ؿ);
	}

	internal bool _2658()
	{
		return ((Component)(object)this).DesignMode;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTime()
	{
		return _២.ShouldSerializeSelectedTime();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTime()
	{
		_២.ResetSelectedTime();
	}

	public void InvalidateAutoSize()
	{
		__21D2 = Size.Empty;
		_21DF();
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		InvalidateAutoSize();
		base.OnFontChanged(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!__21D2.IsEmpty)
		{
			return __21D2;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		ElementStyle backgroundStyle = GetBackgroundStyle();
		_២.RecalcSize();
		__21D2 = _២.Size;
		if (backgroundStyle != null)
		{
			__21D2.Width += ᝈ.ᝍ(backgroundStyle);
			__21D2.Height += ᝈ.ᝏ(backgroundStyle);
		}
		return __21D2;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			width = preferredSize.Width;
			height = preferredSize.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
		base.OnHandleCreated(e);
	}
}
