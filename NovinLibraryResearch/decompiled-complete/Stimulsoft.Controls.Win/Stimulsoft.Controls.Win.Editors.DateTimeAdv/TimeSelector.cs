using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

[DefaultEvent("SelectedTimeChanged")]
[DefaultProperty("SelectedTime")]
public class TimeSelector : ItemControl
{
	private TimeSelectorItem _TimeSelector;

	private Size _PreferredSize = Size.Empty;

	[DefaultValue("OK")]
	[Category("Appearance")]
	[Description("Indicates text displayed on OK button.")]
	[Localizable(true)]
	public string OkText
	{
		get
		{
			return _TimeSelector.OkText;
		}
		set
		{
			_TimeSelector.OkText = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Ok button is visible.")]
	public bool OkButtonVisible
	{
		get
		{
			return _TimeSelector.OkButtonVisible;
		}
		set
		{
			_TimeSelector.OkButtonVisible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDateTime
	{
		get
		{
			return _TimeSelector.SelectedDateTime;
		}
		set
		{
			_TimeSelector.SelectedDateTime = value;
		}
	}

	[Category("Data")]
	[Description("Indicates selected time.")]
	public TimeSpan SelectedTime
	{
		get
		{
			return _TimeSelector.SelectedTime;
		}
		set
		{
			_TimeSelector.SelectedTime = value;
		}
	}

	[DefaultValue(eTimeSelectorFormat.System)]
	[Category("Appearance")]
	[Description("Indicates time format used to present time by the selector.")]
	[Localizable(true)]
	public eTimeSelectorFormat TimeFormat
	{
		get
		{
			return _TimeSelector.TimeFormat;
		}
		set
		{
			_TimeSelector.TimeFormat = value;
			_PreferredSize = Size.Empty;
			AdjustSize();
		}
	}

	[DefaultValue("hh:mm tt")]
	[Category("Data")]
	[Description("Indicates format for the 12 Hour Time Display.")]
	[Localizable(true)]
	public string TimeFormat12H
	{
		get
		{
			return _TimeSelector.TimeFormat12H;
		}
		set
		{
			_TimeSelector.TimeFormat12H = value;
		}
	}

	[DefaultValue("HH:mm")]
	[Category("Data")]
	[Description("Indicates format for the 24 Hour Time Display.")]
	[Localizable(true)]
	public string TimeFormat24H
	{
		get
		{
			return _TimeSelector.TimeFormat24H;
		}
		set
		{
			_TimeSelector.TimeFormat24H = value;
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

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
				AdjustSize();
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

	[DefaultValue(eTimeSelectorType.TouchStyle)]
	[Category("Appearance")]
	[Description("Indicates the type of the selector used to select time.")]
	public eTimeSelectorType SelectorType
	{
		get
		{
			return _TimeSelector.SelectorType;
		}
		set
		{
			_TimeSelector.SelectorType = value;
			_PreferredSize = Size.Empty;
			AdjustSize();
		}
	}

	[DefaultValue("Clear")]
	[Category("Appearance")]
	[Description("Indicates text displayed on Clear button only when MonthCalendarStyle is used.")]
	[Localizable(true)]
	public string ClearText
	{
		get
		{
			return _TimeSelector.ClearText;
		}
		set
		{
			_TimeSelector.ClearText = value;
		}
	}

	[DefaultValue("Hour")]
	[Category("Appearance")]
	[Description("Indicates text displayed on Hour label.")]
	[Localizable(true)]
	public string HourText
	{
		get
		{
			return _TimeSelector.HourText;
		}
		set
		{
			_TimeSelector.HourText = value;
		}
	}

	[DefaultValue("Minute")]
	[Category("Appearance")]
	[Description("Indicates text displayed on Minute label.")]
	[Localizable(true)]
	public string MinuteText
	{
		get
		{
			return _TimeSelector.MinuteText;
		}
		set
		{
			_TimeSelector.MinuteText = value;
		}
	}

	[Description("Occurs after SelectedTime changes.")]
	public event EventHandler SelectedTimeChanged;

	[Description("Occurs when OK button is clicked.")]
	public event EventHandler OkClick;

	protected virtual void OnSelectedTimeChanged(EventArgs e)
	{
		this.SelectedTimeChanged?.Invoke(this, e);
	}

	protected virtual void OnOkClick(EventArgs e)
	{
		this.OkClick?.Invoke(this, e);
	}

	public TimeSelector()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_TimeSelector = new TimeSelectorItem();
		_TimeSelector.GlobalItem = false;
		_TimeSelector.ContainerControl = this;
		_TimeSelector.Stretch = false;
		_TimeSelector.Displayed = true;
		_TimeSelector.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.Style = eDotNetBarStyle.StyleManagerControlled;
		_TimeSelector.SetOwner(this);
		_TimeSelector.SelectedTimeChanged += TimeSelector_SelectedTimeChanged;
		_TimeSelector.OkClick += TimeSelector_OkClick;
		SetBaseItemContainer(_TimeSelector);
	}

	private void TimeSelector_OkClick(object sender, EventArgs e)
	{
		OnOkClick(e);
	}

	private void TimeSelector_SelectedTimeChanged(object sender, EventArgs e)
	{
		OnSelectedTimeChanged(e);
	}

	internal bool GetDesignModeInternal()
	{
		return ((Component)(object)this).DesignMode;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTime()
	{
		return _TimeSelector.ShouldSerializeSelectedTime();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTime()
	{
		_TimeSelector.ResetSelectedTime();
	}

	public void InvalidateAutoSize()
	{
		_PreferredSize = Size.Empty;
		AdjustSize();
	}

	private void AdjustSize()
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
		if (!_PreferredSize.IsEmpty)
		{
			return _PreferredSize;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		ElementStyle backgroundStyle = GetBackgroundStyle();
		_TimeSelector.RecalcSize();
		_PreferredSize = _TimeSelector.Size;
		if (backgroundStyle != null)
		{
			_PreferredSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(backgroundStyle);
			_PreferredSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
		}
		return _PreferredSize;
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
			AdjustSize();
		}
		base.OnHandleCreated(e);
	}
}
