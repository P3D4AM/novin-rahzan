using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using DevComponents.DotNetBar;

namespace DevComponents.Editors.DateTimeAdv;

public class TimeSelectorItem : BaseItem
{
	internal const string _265B = "HH:mm";

	internal const string _265C = "hh:mm tt";

	private Command __265D;

	private Command __265E;

	private Command __265F;

	private Command __2660;

	private Command __2661;

	private Command __2662;

	private Font __2663;

	private ItemContainer __2664;

	private LabelItem __2665;

	private LabelItem __2666;

	private LabelItem __2667;

	private EventHandler _2668;

	private EventHandler _2669;

	private LabelItem __266A;

	private ButtonItem __266B;

	private ButtonItem __266C;

	private static readonly Size _266D = new Size(24, 15);

	private TimeSpan __266E = TimeSpan.Zero;

	private string __266F = "HH:mm";

	private string __2670 = "hh:mm tt";

	private DateTime __2671 = DateTime.MinValue;

	private string __2672 = "OK";

	private string __2673 = "Clear";

	private eTimeSelectorFormat __2674;

	private bool __2675 = true;

	private bool __2587 = true;

	private eTimeSelectorType __2676 = eTimeSelectorType.TouchStyle;

	private string __2677 = "Hour";

	private string __2678 = "Minute";

	private Size __2679 = _266D;

	private bool _2659
	{
		get
		{
			if (_2688() == eTimeSelectorFormat.Time24H)
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
			return __266E;
		}
		set
		{
			if (value != __266E)
			{
				TimeSpan oldValue = __266E;
				__266E = value;
				OnSelectedTimeChanged(oldValue, value);
			}
		}
	}

	[DefaultValue("HH:mm")]
	[Localizable(true)]
	[Description("Indicates format for the 24 Hour Time Display.")]
	[Category("Data")]
	public string TimeFormat24H
	{
		get
		{
			return __266F;
		}
		set
		{
			if (value != __266F)
			{
				string oldValue = __266F;
				__266F = value;
				OnTimeFormat24HChanged(oldValue, value);
			}
		}
	}

	[Localizable(true)]
	[DefaultValue("hh:mm tt")]
	[Description("Indicates format for the 12 Hour Time Display.")]
	[Category("Data")]
	public string TimeFormat12H
	{
		get
		{
			return __2670;
		}
		set
		{
			if (value != __2670)
			{
				string oldValue = __2670;
				__2670 = value;
				OnTimeFormat12HChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DateTime SelectedDateTime
	{
		get
		{
			if (__266E == TimeSpan.Zero)
			{
				return DateTime.MinValue;
			}
			return new DateTime(__2671.Year, __2671.Month, __2671.Day, __266E.Hours, __266E.Minutes, __266E.Seconds);
		}
		set
		{
			if (value != __2671)
			{
				DateTime oldValue = __2671;
				__2671 = value;
				OnSelectedDateTimeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates text displayed on OK button.")]
	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue("OK")]
	public string OkText
	{
		get
		{
			return __2672;
		}
		set
		{
			if (value != __2672)
			{
				string oldValue = __2672;
				__2672 = value;
				OnOkTextChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates text displayed on Clear button only when MonthCalendarStyle is used.")]
	[Category("Appearance")]
	[DefaultValue("Clear")]
	[Localizable(true)]
	public string ClearText
	{
		get
		{
			return __2673;
		}
		set
		{
			if (value != __2673)
			{
				string oldValue = __2673;
				__2673 = value;
				OnClearTextChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Localizable(true)]
	[Description("Indicates time format used to present time by the selector.")]
	[DefaultValue(eTimeSelectorFormat.System)]
	public eTimeSelectorFormat TimeFormat
	{
		get
		{
			return __2674;
		}
		set
		{
			if (value != __2674)
			{
				eTimeSelectorFormat oldValue = __2674;
				__2674 = value;
				OnTimeFormatChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether Ok button is visible.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool OkButtonVisible
	{
		get
		{
			return __2675;
		}
		set
		{
			if (value != __2675)
			{
				bool oldValue = __2675;
				__2675 = value;
				OnOkButtonVisibleChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether Ok button is visible.")]
	[DefaultValue(true)]
	[Category("Appearance")]
	public bool ClearButtonVisible
	{
		get
		{
			return __2587;
		}
		set
		{
			if (value != __2587)
			{
				bool oldValue = __2587;
				__2587 = value;
				OnClearButtonVisibleChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(eTimeSelectorType.TouchStyle)]
	[Description("Indicates the type of the selector used to select time.")]
	public eTimeSelectorType SelectorType
	{
		get
		{
			return __2676;
		}
		set
		{
			if (value != __2676)
			{
				eTimeSelectorType oldValue = __2676;
				__2676 = value;
				OnSelectorTypeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates text displayed on Hour label.")]
	[Localizable(true)]
	[Category("Appearance")]
	[DefaultValue("Hour")]
	public string HourText
	{
		get
		{
			return __2677;
		}
		set
		{
			if (value != __2677)
			{
				string oldValue = __2677;
				__2677 = value;
				OnHourTextChanged(oldValue, value);
			}
		}
	}

	[DefaultValue("Minute")]
	[Description("Indicates text displayed on Minute label.")]
	[Category("Appearance")]
	[Localizable(true)]
	public string MinuteText
	{
		get
		{
			return __2678;
		}
		set
		{
			if (value != __2678)
			{
				string oldValue = __2678;
				__2678 = value;
				OnMinuteTextChanged(oldValue, value);
			}
		}
	}

	internal Size _265A
	{
		get
		{
			return __2679;
		}
		set
		{
			if (__2679 != value)
			{
				__2679 = value;
				_2680();
			}
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

	public TimeSelectorItem()
	{
		m_IsContainer = true;
		AutoCollapseOnClick = false;
		__2664 = new ItemContainer();
		__2664.ItemSpacing = 3;
		__2664.LayoutOrientation = eOrientation.Vertical;
		__2664.AutoCollapseOnClick = false;
		SubItems.Add(__2664);
		__265D = new Command();
		__265D.Executed += _267E;
		__265E = new Command();
		__265E.Executed += _267F;
		__265F = new Command();
		__265F.Executed += _267D;
		__2660 = new Command();
		__2660.Executed += _267C;
		__2661 = new Command();
		__2661.Executed += _267B;
		__2662 = new Command();
		__2662.Executed += _267A;
		_2680();
	}

	protected override void Dispose(bool disposing)
	{
		if (__265D != null)
		{
			__265D.Dispose();
			__265D = null;
		}
		if (__265E != null)
		{
			__265E.Dispose();
			__265E = null;
		}
		if (__265F != null)
		{
			__265F.Dispose();
			__265F = null;
		}
		if (__2660 != null)
		{
			__2660.Dispose();
			__2660 = null;
		}
		if (__2661 != null)
		{
			__2661.Dispose();
			__2661 = null;
		}
		if (__2662 != null)
		{
			__2662.Dispose();
			__2662 = null;
		}
		if (__2663 != null)
		{
			__2663.Dispose();
			__2663 = null;
		}
		base.Dispose(disposing);
	}

	protected virtual void OnSelectedTimeChanged(EventArgs e)
	{
		_2668?.Invoke(this, e);
	}

	protected virtual void OnOkClick(EventArgs e)
	{
		_2669?.Invoke(this, e);
	}

	internal void _258C()
	{
	}

	public override void RecalcSize()
	{
		__2664.SetDisplayRectangle(new Rectangle(m_Rect.X, m_Rect.Y, 300, 300));
		_ = __2676;
		__2664.Displayed = true;
		__2664.RecalcSize();
		m_Rect.Width = __2664.WidthInternal;
		m_Rect.Height = __2664.HeightInternal;
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs p)
	{
		__2664.Paint(p);
	}

	public override BaseItem Copy()
	{
		throw new Exception("The method or operation is not implemented.");
	}

	private void _267A(object _٠, EventArgs ؿ)
	{
		OnOkClick(EventArgs.Empty);
	}

	private void _267B(object _٠, EventArgs ؿ)
	{
		bool flag = false;
		if (__2676 == eTimeSelectorType.TouchStyle)
		{
			if (_٠ == __266B)
			{
				if (__266C != null)
				{
					__266C.Checked = false;
				}
				__266B.Checked = true;
				flag = true;
			}
			else
			{
				if (__266B != null)
				{
					__266B.Checked = false;
				}
				__266C.Checked = true;
			}
		}
		else if (__266E != TimeSpan.Zero && __266E.Hours >= 12)
		{
			flag = true;
		}
		if (flag)
		{
			if (__266E != TimeSpan.Zero && __266E.Hours >= 12)
			{
				int num = __266E.Hours - 12;
				int minutes = __266E.Minutes;
				int num2 = __266E.Seconds;
				if (num == 0 && minutes == 0 && num2 == 0)
				{
					num2 = 1;
				}
				SelectedTime = new TimeSpan(num, minutes, num2);
			}
		}
		else if (__266E != TimeSpan.Zero && __266E.Hours < 12)
		{
			SelectedTime = new TimeSpan(__266E.Hours + 12, __266E.Minutes, __266E.Seconds);
		}
	}

	private void _267C(object _٠, EventArgs ؿ)
	{
		SelectedTime = TimeSpan.Zero;
	}

	private void _267D(object _٠, EventArgs ؿ)
	{
		if (!(__266E == TimeSpan.Zero))
		{
			int num = (int)((ICommandSource)_٠).CommandParameter;
			if ((num < 0 && (__266E.Hours > 0 || __266E.Minutes > 0)) || num > 0)
			{
				SelectedTime = new TimeSpan(__266E.Hours, __266E.Minutes + num, __266E.Seconds);
			}
		}
	}

	private void _267E(object _٠, EventArgs ؿ)
	{
		int num = (int)((ICommandSource)_٠).CommandParameter;
		if (!_2659)
		{
			bool flag = false;
			if (__2676 == eTimeSelectorType.TouchStyle)
			{
				flag = __266B.Checked;
			}
			else if (__266E != TimeSpan.Zero && __266E.Hours < 12)
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
		int num2 = ((__266E == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Minute : __266E.Minutes);
		int num3 = ((__266E == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Second : __266E.Seconds);
		if (num == 0 && num2 == 0 && num3 == 0)
		{
			num3 = 1;
		}
		SelectedTime = new TimeSpan(num, num2, num3);
	}

	private void _267F(object _٠, EventArgs ؿ)
	{
		int num = (int)((ICommandSource)_٠).CommandParameter;
		int num2 = ((__266E == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Hour : __266E.Hours);
		int num3 = ((__266E == TimeSpan.Zero) ? DateTimeInput.DateTimeDefaults.Second : __266E.Seconds);
		if (num2 == 0 && num == 0 && num3 == 0)
		{
			num3 = 1;
		}
		SelectedTime = new TimeSpan(num2, num, num3);
	}

	private void _2680()
	{
		__2665 = null;
		__2666 = null;
		__2667 = null;
		if (__2664.SubItems.Count > 0)
		{
			BaseItem[] array = new BaseItem[__2664.SubItems.Count];
			__2664.SubItems.CopyTo(array, 0);
			__2664.SubItems.Clear();
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				baseItem.Dispose();
			}
		}
		if (__2676 == eTimeSelectorType.MonthCalendarStyle)
		{
			_2682();
		}
		else
		{
			if (__2676 != eTimeSelectorType.TouchStyle)
			{
				throw new NotImplementedException("Selector type '" + __2676.ToString() + "' not implemented");
			}
			_2681();
		}
		_2689();
	}

	private void _2681()
	{
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Expected O, but got Unknown
		if (__2664.SubItems.Count > 0)
		{
			BaseItem[] array = new BaseItem[__2664.SubItems.Count];
			__2664.SubItems.CopyTo(array, 0);
			__2664.SubItems.Clear();
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				baseItem.Dispose();
			}
		}
		__2664.ResizeItemsToFit = false;
		__266A = new LabelItem();
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
		__266B = new ButtonItem();
		__266C = new ButtonItem();
		LabelItem labelItem5 = new LabelItem();
		ButtonItem buttonItem4 = new ButtonItem();
		__2664.SubItems.AddRange(new BaseItem[3] { itemContainer, itemContainer4, itemContainer16 });
		__2664.ItemSpacing = 3;
		__2664.LayoutOrientation = eOrientation.Vertical;
		itemContainer.ItemSpacing = 3;
		itemContainer.Name = "itemContainer1";
		itemContainer.AutoCollapseOnClick = false;
		itemContainer.SubItems.AddRange(new BaseItem[1] { itemContainer2 });
		itemContainer2.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer2.ItemSpacing = 4;
		itemContainer2.Name = "itemContainer2";
		itemContainer2.AutoCollapseOnClick = false;
		itemContainer2.SubItems.AddRange(new BaseItem[4] { __266A, itemContainer3, labelItem, buttonItem3 });
		__266A.Font = new Font("Segoe UI", 18f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		__266A.Name = "labelCurrentTime";
		__266A.TextAlignment = (StringAlignment)2;
		__266A.Width = 150;
		__266A.AutoCollapseOnClick = false;
		itemContainer3.LayoutOrientation = eOrientation.Vertical;
		itemContainer3.Name = "itemContainer6";
		itemContainer3.AutoCollapseOnClick = false;
		itemContainer3.SubItems.AddRange(new BaseItem[2] { buttonItem, buttonItem2 });
		buttonItem.ButtonStyle = eButtonStyle.TextOnlyAlways;
		buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem.FixedSize = new Size(24, 14);
		buttonItem.Name = "buttonMinuteUp";
		buttonItem.Text = "<div width=\"20\" align=\"center\"><expand direction=\"top\"/></div>";
		buttonItem.Command = __265F;
		buttonItem.CommandParameter = 1;
		buttonItem.ClickAutoRepeat = true;
		buttonItem.AutoCollapseOnClick = false;
		buttonItem2.ButtonStyle = eButtonStyle.TextOnlyAlways;
		buttonItem2.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem2.FixedSize = new Size(24, 14);
		buttonItem2.Name = "buttonMinuteDown";
		buttonItem2.Text = "<div width=\"20\" align=\"center\"><expand direction=\"bottom\"/></div>";
		buttonItem2.Command = __265F;
		buttonItem2.CommandParameter = -1;
		buttonItem2.ClickAutoRepeat = true;
		buttonItem2.AutoCollapseOnClick = false;
		labelItem.Name = "labelSpacerTop";
		labelItem.Width = 4;
		labelItem.AutoCollapseOnClick = false;
		buttonItem3.Name = "buttonClearTime";
		buttonItem3.Symbol = "\ue5c9";
		buttonItem3.SymbolSet = eSymbolSet.Material;
		buttonItem3.Command = __2660;
		buttonItem3.Visible = __2587;
		buttonItem3.AutoCollapseOnClick = false;
		itemContainer4.ItemSpacing = 8;
		itemContainer4.Name = "itemContainer3";
		itemContainer4.AutoCollapseOnClick = false;
		itemContainer4.SubItems.AddRange(new BaseItem[2] { itemContainer5, itemContainer10 });
		itemContainer5.ItemSpacing = 3;
		itemContainer5.LayoutOrientation = eOrientation.Vertical;
		itemContainer5.MultiLine = false;
		itemContainer5.Name = "itemContainer4";
		itemContainer5.AutoCollapseOnClick = false;
		itemContainer5.SubItems.AddRange(new BaseItem[5] { labelItem2, itemContainer6, itemContainer7, itemContainer8, itemContainer9 });
		labelItem2.Font = new Font("Segoe UI", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		labelItem2.Name = "labelHour";
		labelItem2.Text = _268C();
		labelItem2.TextAlignment = (StringAlignment)1;
		labelItem2.AutoCollapseOnClick = false;
		__2666 = labelItem2;
		itemContainer6.ItemSpacing = 3;
		itemContainer6.Name = "hourRow1";
		itemContainer6.AutoCollapseOnClick = false;
		itemContainer6.SubItems.AddRange(new BaseItem[3]
		{
			_2685(1, eTimeSelectorType.TouchStyle),
			_2685(2, eTimeSelectorType.TouchStyle),
			_2685(3, eTimeSelectorType.TouchStyle)
		});
		itemContainer7.ItemSpacing = 3;
		itemContainer7.Name = "hourRow2";
		itemContainer7.AutoCollapseOnClick = false;
		itemContainer7.SubItems.AddRange(new BaseItem[3]
		{
			_2685(4, eTimeSelectorType.TouchStyle),
			_2685(5, eTimeSelectorType.TouchStyle),
			_2685(6, eTimeSelectorType.TouchStyle)
		});
		itemContainer8.ItemSpacing = 3;
		itemContainer8.Name = "hourRow3";
		itemContainer8.AutoCollapseOnClick = false;
		itemContainer8.SubItems.AddRange(new BaseItem[3]
		{
			_2685(7, eTimeSelectorType.TouchStyle),
			_2685(8, eTimeSelectorType.TouchStyle),
			_2685(9, eTimeSelectorType.TouchStyle)
		});
		itemContainer9.ItemSpacing = 3;
		itemContainer9.Name = "hourRow4";
		itemContainer9.AutoCollapseOnClick = false;
		itemContainer9.SubItems.AddRange(new BaseItem[3]
		{
			_2685(10, eTimeSelectorType.TouchStyle),
			_2685(11, eTimeSelectorType.TouchStyle),
			_2685(12, eTimeSelectorType.TouchStyle)
		});
		itemContainer10.LayoutOrientation = eOrientation.Vertical;
		itemContainer10.Name = "itemContainer5";
		itemContainer10.AutoCollapseOnClick = false;
		itemContainer10.SubItems.AddRange(new BaseItem[1] { itemContainer11 });
		itemContainer11.ItemSpacing = 3;
		itemContainer11.LayoutOrientation = eOrientation.Vertical;
		itemContainer11.MultiLine = false;
		itemContainer11.Name = "itemContainer10";
		itemContainer11.AutoCollapseOnClick = false;
		itemContainer11.SubItems.AddRange(new BaseItem[5] { labelItem3, itemContainer12, itemContainer13, itemContainer14, itemContainer15 });
		labelItem3.Font = new Font("Segoe UI", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		labelItem3.Name = "labelMinute";
		labelItem3.Text = _268D();
		labelItem3.TextAlignment = (StringAlignment)1;
		labelItem3.AutoCollapseOnClick = false;
		__2667 = labelItem3;
		itemContainer12.ItemSpacing = 3;
		itemContainer12.Name = "minuteRow1";
		itemContainer12.AutoCollapseOnClick = false;
		itemContainer12.SubItems.AddRange(new BaseItem[3]
		{
			_2683(0, eTimeSelectorType.TouchStyle),
			_2683(5, eTimeSelectorType.TouchStyle),
			_2683(10, eTimeSelectorType.TouchStyle)
		});
		itemContainer13.ItemSpacing = 3;
		itemContainer13.Name = "minuteRow2";
		itemContainer13.AutoCollapseOnClick = false;
		itemContainer13.SubItems.AddRange(new BaseItem[3]
		{
			_2683(15, eTimeSelectorType.TouchStyle),
			_2683(20, eTimeSelectorType.TouchStyle),
			_2683(25, eTimeSelectorType.TouchStyle)
		});
		itemContainer14.ItemSpacing = 3;
		itemContainer14.Name = "minuteRow3";
		itemContainer14.AutoCollapseOnClick = false;
		itemContainer14.SubItems.AddRange(new BaseItem[3]
		{
			_2683(30, eTimeSelectorType.TouchStyle),
			_2683(35, eTimeSelectorType.TouchStyle),
			_2683(40, eTimeSelectorType.TouchStyle)
		});
		itemContainer15.ItemSpacing = 3;
		itemContainer15.Name = "minuteRow4";
		itemContainer15.AutoCollapseOnClick = false;
		itemContainer15.SubItems.AddRange(new BaseItem[3]
		{
			_2683(45, eTimeSelectorType.TouchStyle),
			_2683(50, eTimeSelectorType.TouchStyle),
			_2683(55, eTimeSelectorType.TouchStyle)
		});
		itemContainer16.Name = "itemContainer15";
		itemContainer16.AutoCollapseOnClick = false;
		itemContainer16.SubItems.AddRange(new BaseItem[5] { labelItem4, __266B, __266C, labelItem5, buttonItem4 });
		labelItem4.Name = "labelSpacerAm";
		labelItem4.Width = (_2659 ? 84 : 36);
		labelItem4.AutoCollapseOnClick = false;
		__266B.Checked = true;
		__266B.Name = "buttonAm";
		__266B.Text = "AM";
		__266B.Visible = !_2659;
		__266B.Command = __2661;
		__266B.CommandParameter = "AM";
		__266B.AutoCollapseOnClick = false;
		__266C.Name = "buttonPm";
		__266C.Text = "PM";
		__266C.Visible = !_2659;
		__266C.Command = __2661;
		__266C.CommandParameter = "PM";
		__266C.AutoCollapseOnClick = false;
		if (__266E != TimeSpan.Zero)
		{
			__266B.Checked = __266E.Hours <= 12;
			__266C.Checked = __266E.Hours > 12;
		}
		labelItem5.Name = "labelSpacerBottom";
		labelItem5.Width = 124;
		labelItem5.AutoCollapseOnClick = false;
		buttonItem4.ColorTable = eButtonColor.BlueWithBackground;
		buttonItem4.Name = "buttonOk";
		buttonItem4.Text = "<div align=\"center\" width=\"32\"><b>" + _268A() + "</b></div>";
		buttonItem4.Visible = __2675;
		buttonItem4.Command = __2662;
	}

	private void _2682()
	{
		Size fixedSize = new Size(13, 18);
		bool flag = _2659;
		ItemContainer itemContainer = new ItemContainer();
		ItemContainer itemContainer2 = new ItemContainer();
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.FixedSize = fixedSize;
		buttonItem.Name = "buttonMinuteDown";
		buttonItem.Text = "<expand direction=\"left\"/>";
		buttonItem.AutoCollapseOnClick = false;
		buttonItem._ບ = true;
		buttonItem.ClickAutoRepeat = true;
		buttonItem.Command = __265F;
		buttonItem.CommandParameter = -1;
		__266A = new LabelItem();
		__266A.Name = "_CurrentTimeLabel";
		__266A.Text = "03:45";
		__266A.TextAlignment = (StringAlignment)1;
		__266A.Width = 64;
		__266A.AutoCollapseOnClick = false;
		ButtonItem buttonItem2 = new ButtonItem();
		buttonItem2.FixedSize = fixedSize;
		buttonItem2.Name = "buttonMinuteUp";
		buttonItem2.Text = "<expand direction=\"right\"/>";
		buttonItem2.AutoCollapseOnClick = false;
		buttonItem2._ບ = true;
		buttonItem2.ClickAutoRepeat = true;
		buttonItem2.Command = __265F;
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
		buttonItem3._ບ = true;
		buttonItem3.Visible = !flag;
		buttonItem3.Command = __2661;
		buttonItem3.CommandParameter = "AM";
		__2665 = new LabelItem();
		__2665.Name = "labelItem2";
		__2665.Text = "PM";
		__2665.TextAlignment = (StringAlignment)1;
		__2665.Width = 28;
		__2665.AutoCollapseOnClick = false;
		__2665.Visible = !flag;
		__266C = new ButtonItem();
		__266C.FixedSize = fixedSize;
		__266C.Name = "_ButtonPm";
		__266C.Text = "<expand direction=\"right\"/>";
		__266C.AutoCollapseOnClick = false;
		__266C._ບ = true;
		__266C.Command = __2661;
		__266C.CommandParameter = "PM";
		__266C.Visible = !flag;
		itemContainer.AutoCollapseOnClick = false;
		itemContainer.BackgroundStyle.BackColor2SchemePart = eColorSchemePart.BarBackground2;
		itemContainer.BackgroundStyle.BackColorGradientAngle = 90;
		itemContainer.BackgroundStyle.BackColorSchemePart = eColorSchemePart.BarBackground;
		itemContainer.BackgroundStyle.Class = "";
		itemContainer.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer.Name = "itemContainer2";
		itemContainer.MinimumSize = new Size(0, fixedSize.Height + 3);
		itemContainer.SubItems.AddRange(new BaseItem[7] { buttonItem, __266A, buttonItem2, labelItem, buttonItem3, __2665, __266C });
		LabelItem labelItem2 = new LabelItem();
		labelItem2.Name = "labelHour";
		labelItem2.Text = _268C();
		labelItem2._0E61 = eColorSchemePart.ItemText;
		labelItem2.TextAlignment = (StringAlignment)1;
		labelItem2.Width = (flag ? (__2679.Width * 4 + 3) : (__2679.Width * 3 + 2));
		labelItem2.Height = __2679.Height - 3;
		labelItem2.AutoCollapseOnClick = false;
		labelItem2.PaddingBottom = 1;
		__2666 = labelItem2;
		LabelItem labelItem3 = new LabelItem();
		labelItem3.Name = "labelMinute";
		labelItem3.Text = _268D();
		labelItem3._0E61 = eColorSchemePart.ItemText;
		labelItem3.TextAlignment = (StringAlignment)1;
		labelItem3.Width = __2679.Width * 3 + 2;
		labelItem3.Height = __2679.Height - 3;
		labelItem3.AutoCollapseOnClick = false;
		labelItem3.PaddingBottom = 1;
		__2667 = labelItem3;
		LabelItem labelItem4 = new LabelItem();
		labelItem4.Height = __2679.Height - 3;
		labelItem4.Width = (flag ? 2 : 18);
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
		if (flag)
		{
			labelItem5.Width -= 12;
			itemContainer4.SubItems.AddRange(new BaseItem[8]
			{
				_2685(0, eTimeSelectorType.MonthCalendarStyle),
				_2685(1, eTimeSelectorType.MonthCalendarStyle),
				_2685(2, eTimeSelectorType.MonthCalendarStyle),
				_2685(3, eTimeSelectorType.MonthCalendarStyle),
				labelItem5,
				_2683(0, eTimeSelectorType.MonthCalendarStyle),
				_2683(5, eTimeSelectorType.MonthCalendarStyle),
				_2683(10, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer4.SubItems.AddRange(new BaseItem[7]
			{
				_2685(1, eTimeSelectorType.MonthCalendarStyle),
				_2685(2, eTimeSelectorType.MonthCalendarStyle),
				_2685(3, eTimeSelectorType.MonthCalendarStyle),
				labelItem5,
				_2683(0, eTimeSelectorType.MonthCalendarStyle),
				_2683(5, eTimeSelectorType.MonthCalendarStyle),
				_2683(10, eTimeSelectorType.MonthCalendarStyle)
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
		if (flag)
		{
			labelItem6.Width -= 12;
			itemContainer5.SubItems.AddRange(new BaseItem[8]
			{
				_2685(4, eTimeSelectorType.MonthCalendarStyle),
				_2685(5, eTimeSelectorType.MonthCalendarStyle),
				_2685(6, eTimeSelectorType.MonthCalendarStyle),
				_2685(7, eTimeSelectorType.MonthCalendarStyle),
				labelItem6,
				_2683(15, eTimeSelectorType.MonthCalendarStyle),
				_2683(20, eTimeSelectorType.MonthCalendarStyle),
				_2683(25, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer5.SubItems.AddRange(new BaseItem[7]
			{
				_2685(4, eTimeSelectorType.MonthCalendarStyle),
				_2685(5, eTimeSelectorType.MonthCalendarStyle),
				_2685(6, eTimeSelectorType.MonthCalendarStyle),
				labelItem6,
				_2683(15, eTimeSelectorType.MonthCalendarStyle),
				_2683(20, eTimeSelectorType.MonthCalendarStyle),
				_2683(25, eTimeSelectorType.MonthCalendarStyle)
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
		if (flag)
		{
			labelItem7.Width -= 12;
			itemContainer6.SubItems.AddRange(new BaseItem[8]
			{
				_2685(8, eTimeSelectorType.MonthCalendarStyle),
				_2685(9, eTimeSelectorType.MonthCalendarStyle),
				_2685(10, eTimeSelectorType.MonthCalendarStyle),
				_2685(11, eTimeSelectorType.MonthCalendarStyle),
				labelItem7,
				_2683(30, eTimeSelectorType.MonthCalendarStyle),
				_2683(35, eTimeSelectorType.MonthCalendarStyle),
				_2683(40, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer6.SubItems.AddRange(new BaseItem[7]
			{
				_2685(7, eTimeSelectorType.MonthCalendarStyle),
				_2685(8, eTimeSelectorType.MonthCalendarStyle),
				_2685(9, eTimeSelectorType.MonthCalendarStyle),
				labelItem7,
				_2683(30, eTimeSelectorType.MonthCalendarStyle),
				_2683(35, eTimeSelectorType.MonthCalendarStyle),
				_2683(40, eTimeSelectorType.MonthCalendarStyle)
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
		if (flag)
		{
			labelItem8.Width -= 12;
			itemContainer7.SubItems.AddRange(new BaseItem[8]
			{
				_2685(12, eTimeSelectorType.MonthCalendarStyle),
				_2685(13, eTimeSelectorType.MonthCalendarStyle),
				_2685(14, eTimeSelectorType.MonthCalendarStyle),
				_2685(15, eTimeSelectorType.MonthCalendarStyle),
				labelItem8,
				_2683(45, eTimeSelectorType.MonthCalendarStyle),
				_2683(50, eTimeSelectorType.MonthCalendarStyle),
				_2683(55, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		else
		{
			itemContainer7.SubItems.AddRange(new BaseItem[7]
			{
				_2685(10, eTimeSelectorType.MonthCalendarStyle),
				_2685(11, eTimeSelectorType.MonthCalendarStyle),
				_2685(12, eTimeSelectorType.MonthCalendarStyle),
				labelItem8,
				_2683(45, eTimeSelectorType.MonthCalendarStyle),
				_2683(50, eTimeSelectorType.MonthCalendarStyle),
				_2683(55, eTimeSelectorType.MonthCalendarStyle)
			});
		}
		ItemContainer itemContainer8 = null;
		ItemContainer itemContainer9 = null;
		if (flag)
		{
			itemContainer8 = new ItemContainer();
			itemContainer8.AutoCollapseOnClick = false;
			itemContainer8.BackgroundStyle.Class = "";
			itemContainer8.BackgroundStyle.CornerType = eCornerType.Square;
			itemContainer8.Name = "hourMinRow5";
			itemContainer8.SubItems.AddRange(new BaseItem[4]
			{
				_2685(16, eTimeSelectorType.MonthCalendarStyle),
				_2685(17, eTimeSelectorType.MonthCalendarStyle),
				_2685(18, eTimeSelectorType.MonthCalendarStyle),
				_2685(19, eTimeSelectorType.MonthCalendarStyle)
			});
			itemContainer9 = new ItemContainer();
			itemContainer9.AutoCollapseOnClick = false;
			itemContainer9.BackgroundStyle.Class = "";
			itemContainer9.BackgroundStyle.CornerType = eCornerType.Square;
			itemContainer9.Name = "hourMinRow6";
			itemContainer9.SubItems.AddRange(new BaseItem[4]
			{
				_2685(20, eTimeSelectorType.MonthCalendarStyle),
				_2685(21, eTimeSelectorType.MonthCalendarStyle),
				_2685(22, eTimeSelectorType.MonthCalendarStyle),
				_2685(23, eTimeSelectorType.MonthCalendarStyle)
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
		buttonItem4.Text = "<div align=\"center\" width=\"32\">" + _268B() + "</div>";
		buttonItem4.Command = __2660;
		buttonItem4.Visible = __2587;
		buttonItem4.AutoCollapseOnClick = false;
		ButtonItem buttonItem5 = new ButtonItem("buttonOk");
		buttonItem5.Text = "<div align=\"center\" width=\"32\">" + _268A() + "</div>";
		buttonItem5.Visible = __2675;
		buttonItem5.Command = __2662;
		ItemContainer itemContainer10 = new ItemContainer();
		itemContainer10.AutoCollapseOnClick = false;
		ElementStyle backgroundStyle = itemContainer10.BackgroundStyle;
		int marginTop;
		if (!flag)
		{
			int height = __2679.Height;
			Size size = _266D;
			marginTop = 35 + 2 * (height - size.Height);
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
		itemContainer10.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
		itemContainer10.Name = "commandRow";
		itemContainer10.ItemSpacing = 5;
		itemContainer10.SubItems.AddRange(new BaseItem[2] { buttonItem4, buttonItem5 });
		__2664.ResizeItemsToFit = true;
		__2664.SubItems.AddRange(new BaseItem[3] { itemContainer, itemContainer2, itemContainer10 });
	}

	private BaseItem _2683(int _2684, eTimeSelectorType _0657)
	{
		BaseItem baseItem = null;
		if (_0657 == eTimeSelectorType.TouchStyle)
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
			buttonItem.Text = "<div align=\"center\" width=\"32\"><font face=\"Segoe UI\" size=\"10\"><b>" + _2684.ToString("##00") + "</b></font></div>";
			buttonItem.Command = __265E;
			buttonItem.CommandParameter = _2684;
			baseItem = buttonItem;
		}
		else
		{
			ButtonItem buttonItem2 = new ButtonItem();
			buttonItem2.Shape = new RoundRectangleShapeDescriptor();
			buttonItem2.Text = _2684.ToString("##00");
			buttonItem2.Command = __265E;
			buttonItem2.CommandParameter = _2684;
			buttonItem2.FixedSize = _265A;
			buttonItem2._ບ = true;
			buttonItem2._0E61 = eColorSchemePart.ItemText;
			baseItem = buttonItem2;
		}
		baseItem.AutoCollapseOnClick = false;
		return baseItem;
	}

	private BaseItem _2685(int _2686, eTimeSelectorType _0657)
	{
		BaseItem baseItem = null;
		if (_0657 == eTimeSelectorType.TouchStyle)
		{
			if (_2659)
			{
				DualButton dualButton = new DualButton();
				dualButton.Text = _2686.ToString();
				if (_2686 + 12 == 24)
				{
					dualButton.Text2 = "0";
				}
				else
				{
					dualButton.Text2 = (_2686 + 12).ToString();
				}
				dualButton.Command = __265D;
				dualButton.CommandParameter = _2686;
				dualButton.Command2 = __265D;
				dualButton.Command2Parameter = _2686 + 12;
				baseItem = dualButton;
				if (__2663 == null)
				{
					_2687();
				}
				dualButton.Font = __2663;
			}
			else
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.ColorTable = eButtonColor.OrangeWithBackground;
				buttonItem.Text = "<div align=\"center\" width=\"32\"><font face=\"Segoe UI\" size=\"10\"><b>" + _2686 + "</b></font></div>";
				buttonItem.Command = __265D;
				buttonItem.CommandParameter = _2686;
				baseItem = buttonItem;
			}
		}
		else
		{
			ButtonItem buttonItem2 = new ButtonItem();
			buttonItem2.Shape = new RoundRectangleShapeDescriptor();
			buttonItem2.Text = _2686.ToString("##00");
			buttonItem2.Command = __265D;
			buttonItem2.CommandParameter = _2686;
			buttonItem2.FixedSize = _265A;
			buttonItem2._ບ = true;
			buttonItem2._0E61 = eColorSchemePart.ItemText;
			baseItem = buttonItem2;
		}
		baseItem.AutoCollapseOnClick = false;
		return baseItem;
	}

	private void _2687()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (__2663 != null)
		{
			__2663.Dispose();
		}
		__2663 = new Font("Segoe UI", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
	}

	private eTimeSelectorFormat _2688()
	{
		eTimeSelectorFormat eTimeSelectorFormat2 = __2674;
		if (eTimeSelectorFormat2 == eTimeSelectorFormat.System)
		{
			if (CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Contains("H"))
			{
				return eTimeSelectorFormat.Time24H;
			}
			return eTimeSelectorFormat.Time12H;
		}
		return eTimeSelectorFormat2;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedTime()
	{
		return !__266E.Equals(TimeSpan.Zero);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTime()
	{
		SelectedTime = TimeSpan.Zero;
	}

	protected virtual void OnSelectedTimeChanged(TimeSpan oldValue, TimeSpan newValue)
	{
		_2689();
		OnSelectedTimeChanged(EventArgs.Empty);
	}

	protected virtual void OnTimeFormat24HChanged(string oldValue, string newValue)
	{
		_2689();
	}

	protected virtual void OnTimeFormat12HChanged(string oldValue, string newValue)
	{
		_2689();
	}

	private void _2689()
	{
		if (__266E == TimeSpan.Zero)
		{
			__266A.Text = "--:--";
			if (__2665 != null)
			{
				__2665.Text = "--";
			}
			return;
		}
		if (_2659)
		{
			__266A.Text = SelectedDateTime.ToString(__266F);
		}
		else
		{
			__266A.Text = SelectedDateTime.ToString(__2670);
		}
		if (__2676 == eTimeSelectorType.TouchStyle)
		{
			__266B.Checked = __266E.Hours < 12;
			__266C.Checked = __266E.Hours >= 12;
		}
		else
		{
			__266B.Checked = false;
			__266C.Checked = false;
		}
		if (__2665 != null)
		{
			__2665.Text = ((__266E.Hours < 12) ? "AM" : "PM");
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
		if (__2662 != null)
		{
			__2662.Text = _268A();
		}
	}

	private string _268A()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorOkButton);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return __2672;
	}

	protected virtual void OnClearTextChanged(string oldValue, string newValue)
	{
		if (__2660 != null)
		{
			__2660.Text = _268B();
		}
	}

	private string _268B()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorClearButton);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return __2673;
	}

	protected virtual void OnTimeFormatChanged(eTimeSelectorFormat oldValue, eTimeSelectorFormat newValue)
	{
		_2680();
	}

	protected virtual void OnOkButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (__2662 != null)
		{
			__2662.Visible = newValue;
		}
	}

	protected virtual void OnClearButtonVisibleChanged(bool oldValue, bool newValue)
	{
		if (__2660 != null)
		{
			__2660.Visible = newValue;
		}
	}

	protected virtual void OnSelectorTypeChanged(eTimeSelectorType oldValue, eTimeSelectorType newValue)
	{
		_2680();
	}

	protected virtual void OnHourTextChanged(string oldValue, string newValue)
	{
		if (__2666 != null)
		{
			__2666.Text = _268C();
		}
	}

	private string _268C()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorHourLabel);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return __2677;
	}

	protected virtual void OnMinuteTextChanged(string oldValue, string newValue)
	{
		if (__2667 != null)
		{
			__2667.Text = _268D();
		}
	}

	private string _268D()
	{
		using (LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize))
		{
			string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.TimeSelectorMinuteLabel);
			if (localizedString != "")
			{
				return localizedString;
			}
		}
		return __2678;
	}
}
