using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

public class TimeSelectorItem : BaseItem
{
	private Command _HourCommand;

	private Command _MinuteCommand;

	private Command _MinuteChangeCommand;

	private Command _ClearCommand;

	private Command _AmPmCommand;

	private Command _OkCommand;

	private Font _DualButtonFont;

	private ItemContainer _InnerContainer;

	private LabelItem _AmPmLabel;

	private LabelItem _HourLabel;

	private LabelItem _MinuteLabel;

	private LabelItem _CurrentTimeLabel;

	private ButtonItem _ButtonAm;

	private ButtonItem _ButtonPm;

	private static readonly Size DefaultHourMinuteButtonSize = new Size(24, 15);

	private TimeSpan _SelectedTime = TimeSpan.Zero;

	internal const string DefaultTimeFormat24H = "HH:mm";

	private string _TimeFormat24H = "HH:mm";

	internal const string DefaultTimeFormat12H = "hh:mm tt";

	private string _TimeFormat12H = "hh:mm tt";

	private DateTime _SelectedDateTime = DateTime.MinValue;

	private string _OkText = "OK";

	private string _ClearText = "Clear";

	private eTimeSelectorFormat _TimeFormat;

	private bool _OkButtonVisible = true;

	private bool _ClearButtonVisible = true;

	private eTimeSelectorType _SelectorType = eTimeSelectorType.TouchStyle;

	private string _HourText = "Hour";

	private string _MinuteText = "Minute";

	private Size _HourMinuteButtonSize = DefaultHourMinuteButtonSize;

	private bool IsZuluTime
	{
		get
		{
			if (GetEffectiveTimeFormat() == eTimeSelectorFormat.Time24H)
			{
				return true;
			}
			return false;
		}
	}

	[Category("Data")]
	[Description("Indicates selected time.")]
	public TimeSpan SelectedTime
	{
		get
		{
			return _SelectedTime;
		}
		set
		{
			if (value != _SelectedTime)
			{
				TimeSpan selectedTime = _SelectedTime;
				_SelectedTime = value;
				OnSelectedTimeChanged(selectedTime, value);
			}
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
			return _TimeFormat24H;
		}
		set
		{
			if (value != _TimeFormat24H)
			{
				string timeFormat24H = _TimeFormat24H;
				_TimeFormat24H = value;
				OnTimeFormat24HChanged(timeFormat24H, value);
			}
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
			return _TimeFormat12H;
		}
		set
		{
			if (value != _TimeFormat12H)
			{
				string timeFormat12H = _TimeFormat12H;
				_TimeFormat12H = value;
				OnTimeFormat12HChanged(timeFormat12H, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDateTime
	{
		get
		{
			if (_SelectedTime == TimeSpan.Zero)
			{
				return DateTime.MinValue;
			}
			return new DateTime(_SelectedDateTime.Year, _SelectedDateTime.Month, _SelectedDateTime.Day, _SelectedTime.Hours, _SelectedTime.Minutes, _SelectedTime.Seconds);
		}
		set
		{
			if (value != _SelectedDateTime)
			{
				DateTime selectedDateTime = _SelectedDateTime;
				_SelectedDateTime = value;
				OnSelectedDateTimeChanged(selectedDateTime, value);
			}
		}
	}

	[DefaultValue("OK")]
	[Category("Appearance")]
	[Description("Indicates text displayed on OK button.")]
	[Localizable(true)]
	public string OkText
	{
		get
		{
			return _OkText;
		}
		set
		{
			if (value != _OkText)
			{
				string okText = _OkText;
				_OkText = value;
				OnOkTextChanged(okText, value);
			}
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
			return _ClearText;
		}
		set
		{
			if (value != _ClearText)
			{
				string clearText = _ClearText;
				_ClearText = value;
				OnClearTextChanged(clearText, value);
			}
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
			return _TimeFormat;
		}
		set
		{
			if (value != _TimeFormat)
			{
				eTimeSelectorFormat timeFormat = _TimeFormat;
				_TimeFormat = value;
				OnTimeFormatChanged(timeFormat, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Ok button is visible.")]
	public bool OkButtonVisible
	{
		get
		{
			return _OkButtonVisible;
		}
		set
		{
			if (value != _OkButtonVisible)
			{
				bool okButtonVisible = _OkButtonVisible;
				_OkButtonVisible = value;
				OnOkButtonVisibleChanged(okButtonVisible, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Ok button is visible.")]
	public bool ClearButtonVisible
	{
		get
		{
			return _ClearButtonVisible;
		}
		set
		{
			if (value != _ClearButtonVisible)
			{
				bool clearButtonVisible = _ClearButtonVisible;
				_ClearButtonVisible = value;
				OnClearButtonVisibleChanged(clearButtonVisible, value);
			}
		}
	}

	[DefaultValue(eTimeSelectorType.TouchStyle)]
	[Category("Appearance")]
	[Description("Indicates the type of the selector used to select time.")]
	public eTimeSelectorType SelectorType
	{
		get
		{
			return _SelectorType;
		}
		set
		{
			if (value != _SelectorType)
			{
				eTimeSelectorType selectorType = _SelectorType;
				_SelectorType = value;
				OnSelectorTypeChanged(selectorType, value);
			}
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
			return _HourText;
		}
		set
		{
			if (value != _HourText)
			{
				string hourText = _HourText;
				_HourText = value;
				OnHourTextChanged(hourText, value);
			}
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
			return _MinuteText;
		}
		set
		{
			if (value != _MinuteText)
			{
				string minuteText = _MinuteText;
				_MinuteText = value;
				OnMinuteTextChanged(minuteText, value);
			}
		}
	}

	internal Size HourMinuteButtonSize
	{
		get
		{
			return _HourMinuteButtonSize;
		}
		set
		{
			if (_HourMinuteButtonSize != value)
			{
				_HourMinuteButtonSize = value;
				RecreateItems();
			}
		}
	}

	[Description("Occurs after SelectedTime changes.")]
	public event EventHandler SelectedTimeChanged;

	[Description("Occurs when OK button is clicked.")]
	public event EventHandler OkClick;

	public TimeSelectorItem()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = false;
		_InnerContainer = new ItemContainer();
		_InnerContainer.ItemSpacing = 3;
		_InnerContainer.LayoutOrientation = eOrientation.Vertical;
		_InnerContainer.AutoCollapseOnClick = false;
		SubItems.Add(_InnerContainer);
		_HourCommand = new Command();
		_HourCommand.Executed += HourCommandExecuted;
		_MinuteCommand = new Command();
		_MinuteCommand.Executed += MinuteCommandExecuted;
		_MinuteChangeCommand = new Command();
		_MinuteChangeCommand.Executed += MinuteChangeCommandExecuted;
		_ClearCommand = new Command();
		_ClearCommand.Executed += ClearCommandExecuted;
		_AmPmCommand = new Command();
		_AmPmCommand.Executed += AmPmCommandExecuted;
		_OkCommand = new Command();
		_OkCommand.Executed += OkCommandExecuted;
		RecreateItems();
	}

	protected override void Dispose(bool disposing)
	{
		if (_HourCommand != null)
		{
			_HourCommand.Dispose();
			_HourCommand = null;
		}
		if (_MinuteCommand != null)
		{
			_MinuteCommand.Dispose();
			_MinuteCommand = null;
		}
		if (_MinuteChangeCommand != null)
		{
			_MinuteChangeCommand.Dispose();
			_MinuteChangeCommand = null;
		}
		if (_ClearCommand != null)
		{
			_ClearCommand.Dispose();
			_ClearCommand = null;
		}
		if (_AmPmCommand != null)
		{
			_AmPmCommand.Dispose();
			_AmPmCommand = null;
		}
		if (_OkCommand != null)
		{
			_OkCommand.Dispose();
			_OkCommand = null;
		}
		if (_DualButtonFont != null)
		{
			_DualButtonFont.Dispose();
			_DualButtonFont = null;
		}
		base.Dispose(disposing);
	}

	protected virtual void OnSelectedTimeChanged(EventArgs e)
	{
		this.SelectedTimeChanged?.Invoke(this, e);
	}

	protected virtual void OnOkClick(EventArgs e)
	{
		this.OkClick?.Invoke(this, e);
	}

	public override void RecalcSize()
	{
		_InnerContainer.SetDisplayRectangle(new Rectangle(m_Rect.X, m_Rect.Y, 300, 300));
		_ = _SelectorType;
		_InnerContainer.Displayed = true;
		_InnerContainer.RecalcSize();
		m_Rect.Width = _InnerContainer.WidthInternal;
		m_Rect.Height = _InnerContainer.HeightInternal;
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		_InnerContainer.Paint(p);
	}

	public override BaseItem Copy()
	{
		throw new Exception("The method or operation is not implemented.");
	}

	private void OkCommandExecuted(object sender, EventArgs e)
	{
		OnOkClick(EventArgs.Empty);
	}

	private void AmPmCommandExecuted(object sender, EventArgs e)
	{
		bool flag = false;
		if (_SelectorType == eTimeSelectorType.TouchStyle)
		{
			if (sender == _ButtonAm)
			{
				if (_ButtonPm != null)
				{
					_ButtonPm.Checked = false;
				}
				_ButtonAm.Checked = true;
				flag = true;
			}
			else
			{
				if (_ButtonAm != null)
				{
					_ButtonAm.Checked = false;
				}
				_ButtonPm.Checked = true;
			}
		}
		else if (_SelectedTime != TimeSpan.Zero && _SelectedTime.Hours >= 12)
		{
			flag = true;
		}
		if (flag)
		{
			if (_SelectedTime != TimeSpan.Zero && _SelectedTime.Hours >= 12)
			{
				int num = _SelectedTime.Hours - 12;
				int minutes = _SelectedTime.Minutes;
				int num2 = _SelectedTime.Seconds;
				if (num == 0 && minutes == 0 && num2 == 0)
				{
					num2 = 1;
				}
				SelectedTime = new TimeSpan(num, minutes, num2);
			}
		}
		else if (_SelectedTime != TimeSpan.Zero && _SelectedTime.Hours < 12)
		{
			SelectedTime = new TimeSpan(_SelectedTime.Hours + 12, _SelectedTime.Minutes, _SelectedTime.Seconds);
		}
	}

	private void ClearCommandExecuted(object sender, EventArgs e)
	{
		SelectedTime = TimeSpan.Zero;
	}

	private void MinuteChangeCommandExecuted(object sender, EventArgs e)
	{
		if (!(_SelectedTime == TimeSpan.Zero))
		{
			int num = (int)((ICommandSource)sender).CommandParameter;
			SelectedTime = new TimeSpan(_SelectedTime.Hours, _SelectedTime.Minutes + num, _SelectedTime.Seconds);
		}
	}

	private void HourCommandExecuted(object sender, EventArgs e)
	{
		int num = (int)((ICommandSource)sender).CommandParameter;
		if (!IsZuluTime)
		{
			bool flag = false;
			if (_SelectorType == eTimeSelectorType.TouchStyle)
			{
				flag = _ButtonAm.Checked;
			}
			else if (_SelectedTime != TimeSpan.Zero && _SelectedTime.Hours < 12)
			{
				flag = true;
			}
			if (!flag && num < 12)
			{
				num += 12;
			}
			else if (flag && num == 12)
			{
				num = 0;
			}
		}
		int num2 = ((_SelectedTime == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Minute : _SelectedTime.Minutes);
		int num3 = ((_SelectedTime == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Second : _SelectedTime.Seconds);
		if (num == 0 && num2 == 0 && num3 == 0)
		{
			num3 = 1;
		}
		SelectedTime = new TimeSpan(num, num2, num3);
	}

	private void MinuteCommandExecuted(object sender, EventArgs e)
	{
		int num = (int)((ICommandSource)sender).CommandParameter;
		int num2 = ((_SelectedTime == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Hour : _SelectedTime.Hours);
		int num3 = ((_SelectedTime == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Second : _SelectedTime.Seconds);
		if (num2 == 0 && num == 0 && num3 == 0)
		{
			num3 = 1;
		}
		SelectedTime = new TimeSpan(num2, num, num3);
	}

	private void RecreateItems()
	{
		_AmPmLabel = null;
		_HourLabel = null;
		_MinuteLabel = null;
		if (_InnerContainer.SubItems.Count > 0)
		{
			BaseItem[] array = new BaseItem[_InnerContainer.SubItems.Count];
			_InnerContainer.SubItems.CopyTo(array, 0);
			_InnerContainer.SubItems.Clear();
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				baseItem.Dispose();
			}
		}
		if (_SelectorType == eTimeSelectorType.MonthCalendarStyle)
		{
			RecreateItemsMonthCalendarStyle();
		}
		else
		{
			if (_SelectorType != eTimeSelectorType.TouchStyle)
			{
				throw new NotImplementedException("Selector type '" + _SelectorType.ToString() + "' not implemented");
			}
			RecreateItemsTouchStyle();
		}
		UpdateSelectedTimeText();
	}

	private void RecreateItemsTouchStyle()
	{
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Expected O, but got Unknown
		if (_InnerContainer.SubItems.Count > 0)
		{
			BaseItem[] array = new BaseItem[_InnerContainer.SubItems.Count];
			_InnerContainer.SubItems.CopyTo(array, 0);
			_InnerContainer.SubItems.Clear();
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				baseItem.Dispose();
			}
		}
		_InnerContainer.ResizeItemsToFit = false;
		_CurrentTimeLabel = new LabelItem();
		ItemContainer itemContainer = new ItemContainer();
		ItemContainer itemContainer2 = new ItemContainer();
		ItemContainer itemContainer3 = new ItemContainer();
		ButtonItem buttonItem = new ButtonItem();
		ButtonItem buttonItem2 = new ButtonItem();
		LabelItem labelItem = new LabelItem();
		ButtonItem buttonItem3 = new ButtonItem();
		ItemContainer itemContainer4 = new ItemContainer();
		ItemContainer itemContainer5 = new ItemContainer();
		LabelItem labelItem2 = new LabelItem();
		ItemContainer itemContainer6 = new ItemContainer();
		ItemContainer itemContainer7 = new ItemContainer();
		ItemContainer itemContainer8 = new ItemContainer();
		ItemContainer itemContainer9 = new ItemContainer();
		ItemContainer itemContainer10 = new ItemContainer();
		ItemContainer itemContainer11 = new ItemContainer();
		LabelItem labelItem3 = new LabelItem();
		ItemContainer itemContainer12 = new ItemContainer();
		ItemContainer itemContainer13 = new ItemContainer();
		ItemContainer itemContainer14 = new ItemContainer();
		ItemContainer itemContainer15 = new ItemContainer();
		ItemContainer itemContainer16 = new ItemContainer();
		LabelItem labelItem4 = new LabelItem();
		_ButtonAm = new ButtonItem();
		_ButtonPm = new ButtonItem();
		LabelItem labelItem5 = new LabelItem();
		ButtonItem buttonItem4 = new ButtonItem();
		_InnerContainer.SubItems.AddRange(new BaseItem[3] { itemContainer, itemContainer4, itemContainer16 });
		_InnerContainer.ItemSpacing = 3;
		_InnerContainer.LayoutOrientation = eOrientation.Vertical;
		itemContainer.ItemSpacing = 3;
		itemContainer.Name = "itemContainer1";
		itemContainer.AutoCollapseOnClick = false;
		itemContainer.SubItems.AddRange(new BaseItem[1] { itemContainer2 });
		itemContainer2.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer2.ItemSpacing = 4;
		itemContainer2.Name = "itemContainer2";
		itemContainer2.AutoCollapseOnClick = false;
		itemContainer2.SubItems.AddRange(new BaseItem[4] { _CurrentTimeLabel, itemContainer3, labelItem, buttonItem3 });
		_CurrentTimeLabel.Font = new Font("Segoe UI", 18f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		_CurrentTimeLabel.Name = "labelCurrentTime";
		_CurrentTimeLabel.TextAlignment = (StringAlignment)2;
		_CurrentTimeLabel.Width = 150;
		_CurrentTimeLabel.AutoCollapseOnClick = false;
		itemContainer3.LayoutOrientation = eOrientation.Vertical;
		itemContainer3.Name = "itemContainer6";
		itemContainer3.AutoCollapseOnClick = false;
		itemContainer3.SubItems.AddRange(new BaseItem[2] { buttonItem, buttonItem2 });
		buttonItem.ButtonStyle = eButtonStyle.TextOnlyAlways;
		buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem.FixedSize = new Size(24, 14);
		buttonItem.Name = "buttonMinuteUp";
		buttonItem.Text = "<div width=\"20\" align=\"center\"><expand direction=\"top\"/></div>";
		buttonItem.Command = _MinuteChangeCommand;
		buttonItem.CommandParameter = 1;
		buttonItem.ClickAutoRepeat = true;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem2.ButtonStyle = eButtonStyle.TextOnlyAlways;
		buttonItem2.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem2.FixedSize = new Size(24, 14);
		buttonItem2.Name = "buttonMinuteDown";
		buttonItem2.Text = "<div width=\"20\" align=\"center\"><expand direction=\"bottom\"/></div>";
		buttonItem2.Command = _MinuteChangeCommand;
		buttonItem2.CommandParameter = -1;
		buttonItem2.ClickAutoRepeat = true;
		buttonItem2.AutoCollapseOnClick = false;
		labelItem.Name = "labelSpacerTop";
		labelItem.Width = 4;
		labelItem.AutoCollapseOnClick = false;
		buttonItem3.Name = "buttonClearTime";
		buttonItem3.Text = "<symbol/>";
		buttonItem3.Command = _ClearCommand;
		buttonItem3.Visible = _ClearButtonVisible;
		buttonItem3.AutoCollapseOnClick = false;
		itemContainer4.ItemSpacing = 8;
		itemContainer4.Name = "itemContainer3";
		itemContainer4.AutoCollapseOnClick = false;
		itemContainer4.SubItems.AddRange(new BaseItem[2] { itemContainer5, itemContainer10 });
		itemContainer5.ItemSpacing = 3;
		itemContainer5.LayoutOrientation = eOrientation.Vertical;
		itemContainer5.MultiLine = true;
		itemContainer5.Name = "itemContainer4";
		itemContainer5.AutoCollapseOnClick = false;
		itemContainer5.SubItems.AddRange(new BaseItem[5] { labelItem2, itemContainer6, itemContainer7, itemContainer8, itemContainer9 });
		labelItem2.Font = new Font("Segoe UI", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		labelItem2.Name = "labelHour";
		labelItem2.Text = GetHourText();
		labelItem2.TextAlignment = (StringAlignment)1;
		labelItem2.AutoCollapseOnClick = false;
		_HourLabel = labelItem2;
		itemContainer6.ItemSpacing = 3;
		itemContainer6.Name = "hourRow1";
		itemContainer6.AutoCollapseOnClick = false;
		itemContainer6.SubItems.AddRange(new BaseItem[3]
		{
			CreateHourItem(1, eTimeSelectorType.TouchStyle),
			CreateHourItem(2, eTimeSelectorType.TouchStyle),
			CreateHourItem(3, eTimeSelectorType.TouchStyle)
		});
		itemContainer7.ItemSpacing = 3;
		itemContainer7.Name = "hourRow2";
		itemContainer7.AutoCollapseOnClick = false;
		itemContainer7.SubItems.AddRange(new BaseItem[3]
		{
			CreateHourItem(4, eTimeSelectorType.TouchStyle),
			CreateHourItem(5, eTimeSelectorType.TouchStyle),
			CreateHourItem(6, eTimeSelectorType.TouchStyle)
		});
		itemContainer8.ItemSpacing = 3;
		itemContainer8.Name = "hourRow3";
		itemContainer8.AutoCollapseOnClick = false;
		itemContainer8.SubItems.AddRange(new BaseItem[3]
		{
			CreateHourItem(7, eTimeSelectorType.TouchStyle),
			CreateHourItem(8, eTimeSelectorType.TouchStyle),
			CreateHourItem(9, eTimeSelectorType.TouchStyle)
		});
		itemContainer9.ItemSpacing = 3;
		itemContainer9.Name = "hourRow4";
		itemContainer9.AutoCollapseOnClick = false;
		itemContainer9.SubItems.AddRange(new BaseItem[3]
		{
			CreateHourItem(10, eTimeSelectorType.TouchStyle),
			CreateHourItem(11, eTimeSelectorType.TouchStyle),
			CreateHourItem(12, eTimeSelectorType.TouchStyle)
		});
		itemContainer10.LayoutOrientation = eOrientation.Vertical;
		itemContainer10.Name = "itemContainer5";
		itemContainer10.AutoCollapseOnClick = false;
		itemContainer10.SubItems.AddRange(new BaseItem[1] { itemContainer11 });
		itemContainer11.ItemSpacing = 3;
		itemContainer11.LayoutOrientation = eOrientation.Vertical;
		itemContainer11.MultiLine = true;
		itemContainer11.Name = "itemContainer10";
		itemContainer11.AutoCollapseOnClick = false;
		itemContainer11.SubItems.AddRange(new BaseItem[5] { labelItem3, itemContainer12, itemContainer13, itemContainer14, itemContainer15 });
		labelItem3.Font = new Font("Segoe UI", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		labelItem3.Name = "labelMinute";
		labelItem3.Text = GetMinuteText();
		labelItem3.TextAlignment = (StringAlignment)1;
		labelItem3.AutoCollapseOnClick = false;
		_MinuteLabel = labelItem3;
		itemContainer12.ItemSpacing = 3;
		itemContainer12.Name = "minuteRow1";
		itemContainer12.AutoCollapseOnClick = false;
		itemContainer12.SubItems.AddRange(new BaseItem[3]
		{
			CreateMinuteItem(0, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(5, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(10, eTimeSelectorType.TouchStyle)
		});
		itemContainer13.ItemSpacing = 3;
		itemContainer13.Name = "minuteRow2";
		itemContainer13.AutoCollapseOnClick = false;
		itemContainer13.SubItems.AddRange(new BaseItem[3]
		{
			CreateMinuteItem(15, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(20, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(25, eTimeSelectorType.TouchStyle)
		});
		itemContainer14.ItemSpacing = 3;
		itemContainer14.Name = "minuteRow3";
		itemContainer14.AutoCollapseOnClick = false;
		itemContainer14.SubItems.AddRange(new BaseItem[3]
		{
			CreateMinuteItem(30, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(35, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(40, eTimeSelectorType.TouchStyle)
		});
		itemContainer15.ItemSpacing = 3;
		itemContainer15.Name = "minuteRow4";
		itemContainer15.AutoCollapseOnClick = false;
		itemContainer15.SubItems.AddRange(new BaseItem[3]
		{
			CreateMinuteItem(45, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(50, eTimeSelectorType.TouchStyle),
			CreateMinuteItem(55, eTimeSelectorType.TouchStyle)
		});
		itemContainer16.Name = "itemContainer15";
		itemContainer16.AutoCollapseOnClick = false;
		itemContainer16.SubItems.AddRange(new BaseItem[5] { labelItem4, _ButtonAm, _ButtonPm, labelItem5, buttonItem4 });
		labelItem4.Name = "labelSpacerAm";
		labelItem4.Width = (IsZuluTime ? 84 : 36);
		labelItem4.AutoCollapseOnClick = false;
		_ButtonAm.Checked = true;
		_ButtonAm.Name = "buttonAm";
		_ButtonAm.Text = "AM";
		_ButtonAm.Visible = !IsZuluTime;
		_ButtonAm.Command = _AmPmCommand;
		_ButtonAm.CommandParameter = "AM";
		_ButtonAm.AutoCollapseOnClick = false;
		_ButtonPm.Name = "buttonPm";
		_ButtonPm.Text = "PM";
		_ButtonPm.Visible = !IsZuluTime;
		_ButtonPm.Command = _AmPmCommand;
		_ButtonPm.CommandParameter = "PM";
		_ButtonPm.AutoCollapseOnClick = false;
		if (_SelectedTime != TimeSpan.Zero)
		{
			_ButtonAm.Checked = _SelectedTime.Hours <= 12;
			_ButtonPm.Checked = _SelectedTime.Hours > 12;
		}
		labelItem5.Name = "labelSpacerBottom";
		labelItem5.Width = 124;
		labelItem5.AutoCollapseOnClick = false;
		buttonItem4.ColorTable = eButtonColor.BlueWithBackground;
		buttonItem4.Name = "buttonOk";
		buttonItem4.Text = "<div align=\"center\" width=\"32\"><b>" + GetOkText() + "</b></div>";
		buttonItem4.Visible = _OkButtonVisible;
		buttonItem4.Command = _OkCommand;
	}

	private void RecreateItemsMonthCalendarStyle()
	{
		Size fixedSize = new Size(13, 18);
		bool isZuluTime = IsZuluTime;
		ItemContainer itemContainer = new ItemContainer();
		ItemContainer itemContainer2 = new ItemContainer();
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.FixedSize = fixedSize;
		buttonItem.Name = "buttonMinuteDown";
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._FixedSizeCenterText = true;
		buttonItem.ClickAutoRepeat = true;
		buttonItem.Command = _MinuteChangeCommand;
		buttonItem.CommandParameter = -1;
		_CurrentTimeLabel = new LabelItem();
		_CurrentTimeLabel.Name = "_CurrentTimeLabel";
		_CurrentTimeLabel.Text = "03:45";
		_CurrentTimeLabel.TextAlignment = (StringAlignment)1;
		_CurrentTimeLabel.Width = 64;
		_CurrentTimeLabel.AutoCollapseOnClick = false;
		ButtonItem buttonItem2 = new ButtonItem();
		buttonItem2.FixedSize = fixedSize;
		buttonItem2.Name = "buttonMinuteUp";
		buttonItem2.Text = "<expand direction=\"right\"/>";
		buttonItem2.AutoCollapseOnClick = false;
		buttonItem2._FixedSizeCenterText = true;
		buttonItem2.ClickAutoRepeat = true;
		buttonItem2.Command = _MinuteChangeCommand;
		buttonItem2.CommandParameter = 1;
		LabelItem labelItem = new LabelItem();
		labelItem.Name = "labelSpacerLine1";
		labelItem.Width = 10;
		labelItem.AutoCollapseOnClick = false;
		ButtonItem buttonItem3 = new ButtonItem();
		buttonItem3.FixedSize = fixedSize;
		buttonItem3.Name = "_ButtonAm";
		buttonItem3.Text = "<expand direction=\"left\"/>";
		buttonItem3.AutoCollapseOnClick = false;
		buttonItem3._FixedSizeCenterText = true;
		buttonItem3.Visible = !isZuluTime;
		buttonItem3.Command = _AmPmCommand;
		buttonItem3.CommandParameter = "AM";
		_AmPmLabel = new LabelItem();
		_AmPmLabel.Name = "labelItem2";
		_AmPmLabel.Text = "PM";
		_AmPmLabel.TextAlignment = (StringAlignment)1;
		_AmPmLabel.Width = 28;
		_AmPmLabel.AutoCollapseOnClick = false;
		_AmPmLabel.Visible = !isZuluTime;
		_ButtonPm = new ButtonItem();
		_ButtonPm.FixedSize = fixedSize;
		_ButtonPm.Name = "_ButtonPm";
		_ButtonPm.Text = "<expand direction=\"right\"/>";
		_ButtonPm.AutoCollapseOnClick = false;
		_ButtonPm._FixedSizeCenterText = true;
		_ButtonPm.Command = _AmPmCommand;
		_ButtonPm.CommandParameter = "PM";
		_ButtonPm.Visible = !isZuluTime;
		itemContainer.AutoCollapseOnClick = false;
		itemContainer.BackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		itemContainer.BackgroundStyle.BackColorGradientAngle = 90;
		itemContainer.BackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
		itemContainer.BackgroundStyle.Class = "";
		itemContainer.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer.Name = "itemContainer2";
		itemContainer.MinimumSize = new Size(0, fixedSize.Height + 3);
		itemContainer.SubItems.AddRange(new BaseItem[7] { buttonItem, _CurrentTimeLabel, buttonItem2, labelItem, buttonItem3, _AmPmLabel, _ButtonPm });
		LabelItem labelItem2 = new LabelItem();
		labelItem2.Name = "labelHour";
		labelItem2.Text = GetHourText();
		labelItem2.ForeColorColorSchemePart = eColorSchemePart.ItemText;
		labelItem2.TextAlignment = (StringAlignment)1;
		labelItem2.Width = (isZuluTime ? (_HourMinuteButtonSize.Width * 4 + 3) : (_HourMinuteButtonSize.Width * 3 + 2));
		labelItem2.Height = _HourMinuteButtonSize.Height - 3;
		labelItem2.AutoCollapseOnClick = false;
		labelItem2.PaddingBottom = 1;
		_HourLabel = labelItem2;
		LabelItem labelItem3 = new LabelItem();
		labelItem3.Name = "labelMinute";
		labelItem3.Text = GetMinuteText();
		labelItem3.ForeColorColorSchemePart = eColorSchemePart.ItemText;
		labelItem3.TextAlignment = (StringAlignment)1;
		labelItem3.Width = _HourMinuteButtonSize.Width * 3 + 2;
		labelItem3.Height = _HourMinuteButtonSize.Height - 3;
		labelItem3.AutoCollapseOnClick = false;
		labelItem3.PaddingBottom = 1;
		_MinuteLabel = labelItem3;
		LabelItem labelItem4 = new LabelItem();
		labelItem4.Height = _HourMinuteButtonSize.Height - 3;
		labelItem4.Width = (isZuluTime ? 2 : 18);
		ItemContainer itemContainer3 = new ItemContainer();
		itemContainer3.AutoCollapseOnClick = false;
		itemContainer3.BackgroundStyle.BorderBottom = eStyleBorderType.Solid;
		itemContainer3.BackgroundStyle.BorderBottomColorSchemePart = eColorSchemePart.PanelBorder;
		itemContainer3.BackgroundStyle.BorderBottomWidth = 1;
		itemContainer3.BackgroundStyle.Class = "";
		itemContainer3.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer3.Name = "hourMinLabelRow";
		itemContainer3.BackgroundStyle.MarginBottom = 2;
		itemContainer3.SubItems.AddRange(new BaseItem[3] { labelItem2, labelItem4, labelItem3 });
		LabelItem labelItem5 = new LabelItem();
		labelItem5.Name = "labelSpacerLine3";
		labelItem5.Width = 18;
		labelItem5.AutoCollapseOnClick = false;
		ItemContainer itemContainer4 = new ItemContainer();
		itemContainer4.AutoCollapseOnClick = false;
		itemContainer4.BackgroundStyle.Class = "";
		itemContainer4.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer4.Name = "hourMinRow1";
		if (isZuluTime)
		{
			labelItem5.Width -= 12;
			itemContainer4.SubItems.AddRange(new BaseItem[8]
			{
				CreateHourItem(0, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(1, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(2, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(3, eTimeSelectorType.MonthCalendarStyle),
				labelItem5,
				CreateMinuteItem(0, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(5, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(10, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer4.SubItems.AddRange(new BaseItem[7]
			{
				CreateHourItem(1, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(2, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(3, eTimeSelectorType.MonthCalendarStyle),
				labelItem5,
				CreateMinuteItem(0, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(5, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(10, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		LabelItem labelItem6 = new LabelItem();
		labelItem6.Name = "labelItem5";
		labelItem6.Width = 18;
		labelItem6.AutoCollapseOnClick = false;
		ItemContainer itemContainer5 = new ItemContainer();
		itemContainer5.AutoCollapseOnClick = false;
		itemContainer5.BackgroundStyle.Class = "";
		itemContainer5.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer5.Name = "hourMinRow2";
		if (isZuluTime)
		{
			labelItem6.Width -= 12;
			itemContainer5.SubItems.AddRange(new BaseItem[8]
			{
				CreateHourItem(4, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(5, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(6, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(7, eTimeSelectorType.MonthCalendarStyle),
				labelItem6,
				CreateMinuteItem(15, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(20, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(25, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer5.SubItems.AddRange(new BaseItem[7]
			{
				CreateHourItem(4, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(5, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(6, eTimeSelectorType.MonthCalendarStyle),
				labelItem6,
				CreateMinuteItem(15, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(20, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(25, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		LabelItem labelItem7 = new LabelItem();
		labelItem7.Name = "labelItem6";
		labelItem7.Width = 18;
		labelItem7.AutoCollapseOnClick = false;
		ItemContainer itemContainer6 = new ItemContainer();
		itemContainer6.AutoCollapseOnClick = false;
		itemContainer6.BackgroundStyle.Class = "";
		itemContainer6.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer6.Name = "hourMinRow3";
		if (isZuluTime)
		{
			labelItem7.Width -= 12;
			itemContainer6.SubItems.AddRange(new BaseItem[8]
			{
				CreateHourItem(8, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(9, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(10, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(11, eTimeSelectorType.MonthCalendarStyle),
				labelItem7,
				CreateMinuteItem(30, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(35, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(40, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer6.SubItems.AddRange(new BaseItem[7]
			{
				CreateHourItem(7, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(8, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(9, eTimeSelectorType.MonthCalendarStyle),
				labelItem7,
				CreateMinuteItem(30, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(35, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(40, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		LabelItem labelItem8 = new LabelItem();
		labelItem8.Name = "labelItem7";
		labelItem8.Width = 18;
		labelItem8.AutoCollapseOnClick = false;
		ItemContainer itemContainer7 = new ItemContainer();
		itemContainer7.AutoCollapseOnClick = false;
		itemContainer7.BackgroundStyle.Class = "";
		itemContainer7.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer7.Name = "hourMinRow4";
		if (isZuluTime)
		{
			labelItem8.Width -= 12;
			itemContainer7.SubItems.AddRange(new BaseItem[8]
			{
				CreateHourItem(12, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(13, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(14, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(15, eTimeSelectorType.MonthCalendarStyle),
				labelItem8,
				CreateMinuteItem(45, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(50, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(55, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer7.SubItems.AddRange(new BaseItem[7]
			{
				CreateHourItem(10, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(11, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(12, eTimeSelectorType.MonthCalendarStyle),
				labelItem8,
				CreateMinuteItem(45, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(50, eTimeSelectorType.MonthCalendarStyle),
				CreateMinuteItem(55, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		ItemContainer itemContainer8 = null;
		ItemContainer itemContainer9 = null;
		if (isZuluTime)
		{
			itemContainer8 = new ItemContainer();
			itemContainer8.AutoCollapseOnClick = false;
			itemContainer8.BackgroundStyle.Class = "";
			itemContainer8.BackgroundStyle.CornerType = eCornerType.Square;
			itemContainer8.Name = "hourMinRow5";
			itemContainer8.SubItems.AddRange(new BaseItem[4]
			{
				CreateHourItem(16, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(17, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(18, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(19, eTimeSelectorType.MonthCalendarStyle)
			});
			itemContainer9 = new ItemContainer();
			itemContainer9.AutoCollapseOnClick = false;
			itemContainer9.BackgroundStyle.Class = "";
			itemContainer9.BackgroundStyle.CornerType = eCornerType.Square;
			itemContainer9.Name = "hourMinRow6";
			itemContainer9.SubItems.AddRange(new BaseItem[4]
			{
				CreateHourItem(20, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(21, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(22, eTimeSelectorType.MonthCalendarStyle),
				CreateHourItem(23, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		itemContainer2.AutoCollapseOnClick = false;
		itemContainer2.BackgroundStyle.Class = "";
		itemContainer2.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer2.LayoutOrientation = eOrientation.Vertical;
		itemContainer2.Name = "itemContainer3";
		itemContainer2.ItemSpacing = 0;
		itemContainer2.SubItems.AddRange(new BaseItem[5] { itemContainer3, itemContainer4, itemContainer5, itemContainer6, itemContainer7 });
		if (itemContainer8 != null)
		{
			itemContainer2.SubItems.Add(itemContainer8);
		}
		if (itemContainer9 != null)
		{
			itemContainer2.SubItems.Add(itemContainer9);
		}
		ButtonItem buttonItem4 = new ButtonItem("buttonClearTime");
		buttonItem4.Text = "<div align=\"center\" width=\"32\">" + GetClearText() + "</div>";
		buttonItem4.Command = _ClearCommand;
		buttonItem4.Visible = _ClearButtonVisible;
		buttonItem4.AutoCollapseOnClick = false;
		ButtonItem buttonItem5 = new ButtonItem("buttonOk");
		buttonItem5.Text = "<div align=\"center\" width=\"32\">" + GetOkText() + "</div>";
		buttonItem5.Visible = _OkButtonVisible;
		buttonItem5.Command = _OkCommand;
		ItemContainer itemContainer10 = new ItemContainer();
		itemContainer10.AutoCollapseOnClick = false;
		ElementStyle backgroundStyle = itemContainer10.BackgroundStyle;
		int marginTop;
		if (!isZuluTime)
		{
			int height = _HourMinuteButtonSize.Height;
			Size defaultHourMinuteButtonSize = DefaultHourMinuteButtonSize;
			marginTop = 35 + 2 * (height - defaultHourMinuteButtonSize.Height);
		}
		else
		{
			marginTop = 2;
		}
		backgroundStyle.MarginTop = marginTop;
		itemContainer10.BackgroundStyle.Border = eStyleBorderType.Solid;
		itemContainer10.BackgroundStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		itemContainer10.BackgroundStyle.BorderTopWidth = 1;
		itemContainer10.BackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
		itemContainer10.BackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		itemContainer10.BackgroundStyle.BackColorGradientAngle = 90;
		itemContainer10.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer10.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Center;
		itemContainer10.Name = "commandRow";
		itemContainer10.ItemSpacing = 5;
		itemContainer10.SubItems.AddRange(new BaseItem[2] { buttonItem4, buttonItem5 });
		_InnerContainer.ResizeItemsToFit = true;
		_InnerContainer.SubItems.AddRange(new BaseItem[3] { itemContainer, itemContainer2, itemContainer10 });
	}

	private BaseItem CreateMinuteItem(int minute, eTimeSelectorType style)
	{
		BaseItem baseItem = null;
		if (style == eTimeSelectorType.TouchStyle)
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
			buttonItem.Text = "<div align=\"center\" width=\"32\"><font face=\"Segoe UI\" size=\"10\"><b>" + minute.ToString("##00") + "</b></font></div>";
			buttonItem.Command = _MinuteCommand;
			buttonItem.CommandParameter = minute;
			baseItem = buttonItem;
		}
		else
		{
			ButtonItem buttonItem2 = new ButtonItem();
			buttonItem2.Shape = new RoundRectangleShapeDescriptor();
			buttonItem2.Text = minute.ToString("##00");
			buttonItem2.Command = _MinuteCommand;
			buttonItem2.CommandParameter = minute;
			buttonItem2.FixedSize = HourMinuteButtonSize;
			buttonItem2._FixedSizeCenterText = true;
			buttonItem2.ForeColorColorSchemePart = eColorSchemePart.ItemText;
			baseItem = buttonItem2;
		}
		baseItem.AutoCollapseOnClick = false;
		return baseItem;
	}

	private BaseItem CreateHourItem(int hour, eTimeSelectorType style)
	{
		BaseItem baseItem = null;
		if (style == eTimeSelectorType.TouchStyle)
		{
			if (IsZuluTime)
			{
				DualButton dualButton = new DualButton();
				dualButton.Text = hour.ToString();
				if (hour + 12 == 24)
				{
					dualButton.Text2 = "0";
				}
				else
				{
					dualButton.Text2 = (hour + 12).ToString();
				}
				dualButton.Command = _HourCommand;
				dualButton.CommandParameter = hour;
				dualButton.Command2 = _HourCommand;
				dualButton.Command2Parameter = hour + 12;
				baseItem = dualButton;
				if (_DualButtonFont == null)
				{
					CreateDualButtonFont();
				}
				dualButton.Font = _DualButtonFont;
			}
			else
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
				buttonItem.Text = "<div align=\"center\" width=\"32\"><font face=\"Segoe UI\" size=\"10\"><b>" + hour + "</b></font></div>";
				buttonItem.Command = _HourCommand;
				buttonItem.CommandParameter = hour;
				baseItem = buttonItem;
			}
		}
		else
		{
			ButtonItem buttonItem2 = new ButtonItem();
			buttonItem2.Shape = new RoundRectangleShapeDescriptor();
			buttonItem2.Text = hour.ToString("##00");
			buttonItem2.Command = _HourCommand;
			buttonItem2.CommandParameter = hour;
			buttonItem2.FixedSize = HourMinuteButtonSize;
			buttonItem2._FixedSizeCenterText = true;
			buttonItem2.ForeColorColorSchemePart = eColorSchemePart.ItemText;
			baseItem = buttonItem2;
		}
		baseItem.AutoCollapseOnClick = false;
		return baseItem;
	}

	private void CreateDualButtonFont()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (_DualButtonFont != null)
		{
			_DualButtonFont.Dispose();
		}
		_DualButtonFont = new Font("Segoe UI", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
	}

	private eTimeSelectorFormat GetEffectiveTimeFormat()
	{
		eTimeSelectorFormat timeFormat = _TimeFormat;
		if (timeFormat == eTimeSelectorFormat.System)
		{
			if (CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("H"))
			{
				return eTimeSelectorFormat.Time24H;
			}
			return eTimeSelectorFormat.Time12H;
		}
		return timeFormat;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTime()
	{
		return !_SelectedTime.Equals(TimeSpan.Zero);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTime()
	{
		SelectedTime = TimeSpan.Zero;
	}

	protected virtual void OnSelectedTimeChanged(TimeSpan oldValue, TimeSpan newValue)
	{
		UpdateSelectedTimeText();
		OnSelectedTimeChanged(EventArgs.Empty);
	}

	protected virtual void OnTimeFormat24HChanged(string oldValue, string newValue)
	{
		UpdateSelectedTimeText();
	}

	protected virtual void OnTimeFormat12HChanged(string oldValue, string newValue)
	{
		UpdateSelectedTimeText();
	}

	private void UpdateSelectedTimeText()
	{
		if (_SelectedTime == TimeSpan.Zero)
		{
			_CurrentTimeLabel.Text = "--:--";
			if (_AmPmLabel != null)
			{
				_AmPmLabel.Text = "--";
			}
			return;
		}
		if (IsZuluTime)
		{
			_CurrentTimeLabel.Text = SelectedDateTime.ToString(_TimeFormat24H);
		}
		else
		{
			_CurrentTimeLabel.Text = SelectedDateTime.ToString(_TimeFormat12H);
		}
		if (_SelectorType == eTimeSelectorType.TouchStyle)
		{
			_ButtonAm.Checked = _SelectedTime.Hours < 12;
			_ButtonPm.Checked = _SelectedTime.Hours >= 12;
		}
		else
		{
			_ButtonAm.Checked = false;
			_ButtonPm.Checked = false;
		}
		if (_AmPmLabel != null)
		{
			_AmPmLabel.Text = ((_SelectedTime.Hours < 12) ? "AM" : "PM");
		}
	}

	protected virtual void OnSelectedDateTimeChanged(DateTime oldValue, DateTime newValue)
	{
		if (newValue == DateTime.MinValue)
		{
			SelectedTime = TimeSpan.Zero;
		}
		else
		{
			SelectedTime = newValue.TimeOfDay;
		}
	}

	protected virtual void OnOkTextChanged(string oldValue, string newValue)
	{
		if (_OkCommand != null)
		{
			_OkCommand.Text = GetOkText();
		}
	}

	private string GetOkText()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorOkButton);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return _OkText;
	}

	protected virtual void OnClearTextChanged(string oldValue, string newValue)
	{
		if (_ClearCommand != null)
		{
			_ClearCommand.Text = GetClearText();
		}
	}

	private string GetClearText()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorClearButton);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return _ClearText;
	}

	protected virtual void OnTimeFormatChanged(eTimeSelectorFormat oldValue, eTimeSelectorFormat newValue)
	{
		RecreateItems();
	}

	protected virtual void OnOkButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (_OkCommand != null)
		{
			_OkCommand.Visible = newValue;
		}
	}

	protected virtual void OnClearButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (_ClearCommand != null)
		{
			_ClearCommand.Visible = newValue;
		}
	}

	protected virtual void OnSelectorTypeChanged(eTimeSelectorType oldValue, eTimeSelectorType newValue)
	{
		RecreateItems();
	}

	protected virtual void OnHourTextChanged(string oldValue, string newValue)
	{
		if (_HourLabel != null)
		{
			_HourLabel.Text = GetHourText();
		}
	}

	private string GetHourText()
	{
		using (new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string text = StiLocalization.Get("A_WebViewer", "Hours");
			if (text != "")
			{
				return text;
			}
		}
		return _HourText;
	}

	protected virtual void OnMinuteTextChanged(string oldValue, string newValue)
	{
		if (_MinuteLabel != null)
		{
			_MinuteLabel.Text = GetMinuteText();
		}
	}

	private string GetMinuteText()
	{
		using (new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string text = StiLocalization.Get("A_WebViewer", "Minutes");
			if (text != "")
			{
				return text;
			}
		}
		return _MinuteText;
	}
}
