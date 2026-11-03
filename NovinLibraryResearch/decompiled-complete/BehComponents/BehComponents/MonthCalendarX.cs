using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace BehComponents;

[ToolboxItem(true)]
[Description("Persian/English Month Calendar v2")]
[ToolboxBitmap(typeof(MonthCalendarX), "MonthCalendarX.bmp")]
public class MonthCalendarX : UserControl
{
	public delegate void OnSelectedDateChanged(DateTime selectedDateTime, PersianDateTime selectedPersianDateTime);

	public delegate void OnTodayDateChanged(DateTime todayDateTime, PersianDateTime todayPersianDateTime);

	public delegate void OnCalendarTypeChanged(CalendarTypes newType);

	public delegate void OnDayMouseClick(DayMouseEventArgs e);

	public delegate void OnDayMouseEnter(DayEventArgs e);

	public delegate void OnDayMouseHover(DayEventArgs e);

	public delegate void OnDayMouseLeave(DayEventArgs e);

	public delegate void OnDayMouseMove(DayMouseEventArgs e);

	public delegate void OnDayMouseUp(DayMouseEventArgs e);

	public delegate void OnDayKeyDown(KeyEventArgs e);

	public delegate void OnDayKeyUp(KeyEventArgs e);

	public delegate void OnDayKeyPress(KeyPressEventArgs e);

	public class DayMouseEventArgs
	{
		public MouseButtons Button;

		public int Clicks;

		public int Delta;

		public Point Location;

		public bool IsTrailingDay;

		public object ValueOfDay;

		public int X_InCurrentDay;

		public int Y_InCurrentDay;

		public int X_InCalendar;

		public int Y_InCalendar;
	}

	public class DayEventArgs
	{
		public bool IsTrailingDay;

		public object ValueOfDay;
	}

	private enum enmBoldedDates
	{
		Annually,
		Monthly,
		Single
	}

	internal enum Command
	{
		None,
		NextMonth,
		PrevMonth,
		Year,
		Month,
		Days,
		GotoToday
	}

	public enum WeekDays
	{
		Saturday,
		Sunday,
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday
	}

	public enum DayOfWeekForHoliday
	{
		None,
		Friday,
		Monday,
		Saturday,
		Sunday,
		Thursday,
		Tuesday,
		Wednesday
	}

	private IContainer components;

	private Panel pnlTitles;

	private Panel pnlWeekDays;

	private Panel pnlToday;

	private Panel pnlDays;

	private ButtonX btnPreMonth;

	private ButtonX btnNextMonth;

	private ButtonX btnMonth;

	private ButtonX btnYear;

	private Label lblSat;

	private Label lblSun;

	private Label lblMon;

	private Label lblTue;

	private Label lblWed;

	private Label lblThu;

	private Label lblFri;

	private ButtonX btn42;

	private ButtonX btn30;

	private ButtonX btn4;

	private ButtonX btn41;

	private ButtonX btn3;

	private ButtonX btn2;

	private ButtonX btn40;

	private ButtonX btn1;

	private ButtonX btn5;

	private ButtonX btn39;

	private ButtonX btn6;

	private ButtonX btn7;

	private ButtonX btn38;

	private ButtonX btn8;

	private ButtonX btn11;

	private ButtonX btn37;

	private ButtonX btn10;

	private ButtonX btn36;

	private ButtonX btn9;

	private ButtonX btn35;

	private ButtonX btn12;

	private ButtonX btn34;

	private ButtonX btn13;

	private ButtonX btn33;

	private ButtonX btn14;

	private ButtonX btn32;

	private ButtonX btn15;

	private ButtonX btn31;

	private ButtonX btn16;

	private ButtonX btn17;

	private ButtonX btn18;

	private ButtonX btn29;

	private ButtonX btn19;

	private ButtonX btn28;

	private ButtonX btn20;

	private ButtonX btn27;

	private ButtonX btn21;

	private ButtonX btn26;

	private ButtonX btn22;

	private ButtonX btn25;

	private ButtonX btn23;

	private ButtonX btn24;

	private Label lblLineWeek;

	private ButtonX btnTodayGoto;

	private Label lblTodayText;

	private ContextMenuStrip cmsMonthNames;

	private NumericUpDown nudYear;

	private ButtonX btnDayClicked;

	private ToolTip toolTip1;

	private TextBox txtTemp;

	private int _iComponentMargin;

	private bool _bFormLoaded;

	private bool _bTxtTempFocused;

	private bool _bNudYearFocused;

	private bool _bBtnXFocused;

	private DateTime _SelectedDate;

	private DateTime _TodayDate;

	private CalendarTypes _CalendarType;

	private Font _TitleFont;

	private Color _TitleForeColor;

	private Color _TitleBackColor;

	private Color _WeekDaysForeColor;

	private Color _WeekDaysBackColor;

	private Font _WeekDaysFont;

	private Color _LineWeekColor;

	private Font _DaysFont;

	private Color _DaysForeColor;

	private Color _DaysBackColor;

	private Font _TodayFont;

	private Color _TodayForeColor;

	private Color _TodayBackColor;

	private Color _BoldedDayForeColor;

	private Color _TrailingForeColor;

	private bool _ShowTrailing;

	private Color _TodayRectColor;

	private bool _ShowTodayRect;

	private bool _ShowToday;

	private float _TodayRectTickness;

	private float _DayRectTickness;

	private WeekDays _WeekStartsOn;

	private Color _HolidayForeColor;

	private DayOfWeekForHoliday _HolidayWeekly;

	private Color _BorderColor;

	private List<DateTime> _BoldedDates_English;

	private List<DateTime> _MonthlyBoldedDates_English;

	private List<DateTime> _AnnuallyBoldedDates_English;

	private List<DateTime> _HolidayDates_English;

	private List<PersianDateTime> _BoldedDates_Persian;

	private List<PersianDateTime> _MonthlyBoldedDates_Persian;

	private List<PersianDateTime> _AnnuallyBoldedDates_Persian;

	private List<PersianDateTime> _HolidayDates_Persian;

	private bool _ShowToolTips;

	[Browsable(false)]
	public string Programmer => "Saeed Mohammadi";

	[Browsable(false)]
	public string ProgrammerWebSite => "http://BehSoft.blogfa.com/";

	public CalendarTypes CalendarType
	{
		get
		{
			return _CalendarType;
		}
		set
		{
			if (_CalendarType != value)
			{
				_CalendarType = value;
				LoadMonthCalendarX();
				_CalendarTypeChanged();
			}
		}
	}

	public Font TitleFont
	{
		get
		{
			return _TitleFont;
		}
		set
		{
			if (_TitleFont != value)
			{
				_TitleFont = value;
				ChangeStyle_Titles();
			}
		}
	}

	public Color TitleForeColor
	{
		get
		{
			return _TitleForeColor;
		}
		set
		{
			if (!(_TitleForeColor == value))
			{
				_TitleForeColor = value;
				ChangeStyle_Titles();
			}
		}
	}

	public Color TitleBackColor
	{
		get
		{
			return _TitleBackColor;
		}
		set
		{
			if (!(_TitleBackColor == value))
			{
				_TitleBackColor = value;
				ChangeStyle_Titles();
			}
		}
	}

	public Color WeekDaysForeColor
	{
		get
		{
			return _WeekDaysForeColor;
		}
		set
		{
			if (!(_WeekDaysForeColor == value))
			{
				_WeekDaysForeColor = value;
				ChangeStyle_WeekDays();
			}
		}
	}

	public Color WeekDaysBackColor
	{
		get
		{
			return _WeekDaysBackColor;
		}
		set
		{
			if (!(_WeekDaysBackColor == value))
			{
				_WeekDaysBackColor = value;
				ChangeStyle_WeekDays();
			}
		}
	}

	public Font WeekDaysFont
	{
		get
		{
			return _WeekDaysFont;
		}
		set
		{
			if (_WeekDaysFont != value)
			{
				_WeekDaysFont = value;
				ChangeStyle_WeekDays();
			}
		}
	}

	public Color LineWeekColor
	{
		get
		{
			return _LineWeekColor;
		}
		set
		{
			if (!(_LineWeekColor == value))
			{
				_LineWeekColor = value;
				ChangeStyle_LineWeek();
			}
		}
	}

	public Font DaysFont
	{
		get
		{
			return _DaysFont;
		}
		set
		{
			if (_DaysFont != value)
			{
				_DaysFont = value;
				ChangeStyle_Days();
				ChangeStyle_TrailingDay();
			}
		}
	}

	public Color DaysForeColor
	{
		get
		{
			return _DaysForeColor;
		}
		set
		{
			if (!(_DaysForeColor == value))
			{
				_DaysForeColor = value;
				ChangeStyle_Days();
			}
		}
	}

	public Color DaysBackColor
	{
		get
		{
			return _DaysBackColor;
		}
		set
		{
			if (!(_DaysBackColor == value))
			{
				_DaysBackColor = value;
				ChangeStyle_Days();
			}
		}
	}

	public float DayRectTickness
	{
		get
		{
			return _DayRectTickness;
		}
		set
		{
			if (_DayRectTickness != value)
			{
				_DayRectTickness = value;
				ChangeStyle_Days();
			}
		}
	}

	public Font TodayFont
	{
		get
		{
			return _TodayFont;
		}
		set
		{
			if (_TodayFont != value)
			{
				_TodayFont = value;
				ChangeStyle_Today();
			}
		}
	}

	public Color TodayForeColor
	{
		get
		{
			return _TodayForeColor;
		}
		set
		{
			if (!(_TodayForeColor == value))
			{
				_TodayForeColor = value;
				ChangeStyle_Today();
			}
		}
	}

	public Color TodayBackColor
	{
		get
		{
			return _TodayBackColor;
		}
		set
		{
			if (!(_TodayBackColor == value))
			{
				_TodayBackColor = value;
				ChangeStyle_Today();
			}
		}
	}

	public Color BoldedDayForeColor
	{
		get
		{
			return _BoldedDayForeColor;
		}
		set
		{
			if (!(_BoldedDayForeColor == value))
			{
				_BoldedDayForeColor = value;
				ChangeStyle_BoldedDay();
			}
		}
	}

	public Color TrailingForeColor
	{
		get
		{
			return _TrailingForeColor;
		}
		set
		{
			if (!(_TrailingForeColor == value))
			{
				_TrailingForeColor = value;
				ChangeStyle_TrailingDay();
			}
		}
	}

	public bool ShowTrailing
	{
		get
		{
			return _ShowTrailing;
		}
		set
		{
			if (_ShowTrailing != value)
			{
				_ShowTrailing = value;
				ChangeStyle_TrailingDay();
			}
		}
	}

	public Color TodayRectColor
	{
		get
		{
			return _TodayRectColor;
		}
		set
		{
			if (!(_TodayRectColor == value))
			{
				_TodayRectColor = value;
				ChangeStyle_TodayRect();
			}
		}
	}

	public bool ShowToday
	{
		get
		{
			return _ShowToday;
		}
		set
		{
			if (_ShowToday != value)
			{
				_ShowToday = value;
				ChangeStyle_Today();
			}
		}
	}

	public bool ShowTodayRect
	{
		get
		{
			return _ShowTodayRect;
		}
		set
		{
			if (_ShowTodayRect != value)
			{
				_ShowTodayRect = value;
				SetTextForComponents();
				ChangeStyle_TodayRect();
			}
		}
	}

	public float TodayRectTickness
	{
		get
		{
			return _TodayRectTickness;
		}
		set
		{
			if (_TodayRectTickness != value)
			{
				_TodayRectTickness = value;
				ChangeStyle_TodayRect();
			}
		}
	}

	public WeekDays WeekStartsOn
	{
		get
		{
			return _WeekStartsOn;
		}
		set
		{
			if (_WeekStartsOn != value)
			{
				_WeekStartsOn = value;
				LoadMonthCalendarX();
			}
		}
	}

	public Color HolidayForeColor
	{
		get
		{
			return _HolidayForeColor;
		}
		set
		{
			if (!(_HolidayForeColor == value))
			{
				_HolidayForeColor = value;
				ChangeStyle_Holiday();
			}
		}
	}

	public DayOfWeekForHoliday HolidayWeekly
	{
		get
		{
			return _HolidayWeekly;
		}
		set
		{
			if (_HolidayWeekly != value)
			{
				_HolidayWeekly = value;
				LoadMonthCalendarX();
			}
		}
	}

	public Color BorderColor
	{
		get
		{
			return _BorderColor;
		}
		set
		{
			if (!(_BorderColor == value))
			{
				_BorderColor = value;
				RenderFocused();
			}
		}
	}

	public DateTime[] EnglishBoldedDates
	{
		get
		{
			return _BoldedDates_English.ToArray();
		}
		set
		{
			_BoldedDates_English.Clear();
			foreach (DateTime item in value)
			{
				_BoldedDates_English.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public DateTime[] EnglishMonthlyBoldedDates
	{
		get
		{
			return _MonthlyBoldedDates_English.ToArray();
		}
		set
		{
			_MonthlyBoldedDates_English.Clear();
			foreach (DateTime item in value)
			{
				_MonthlyBoldedDates_English.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public DateTime[] EnglishAnnuallyBoldedDates
	{
		get
		{
			return _AnnuallyBoldedDates_English.ToArray();
		}
		set
		{
			_AnnuallyBoldedDates_English.Clear();
			foreach (DateTime item in value)
			{
				_AnnuallyBoldedDates_English.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public DateTime[] EnglishHolidayDates
	{
		get
		{
			return _HolidayDates_English.ToArray();
		}
		set
		{
			_HolidayDates_English.Clear();
			foreach (DateTime item in value)
			{
				_HolidayDates_English.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public PersianDateTime[] PersianBoldedDates
	{
		get
		{
			return _BoldedDates_Persian.ToArray();
		}
		set
		{
			_BoldedDates_Persian.Clear();
			foreach (PersianDateTime item in value)
			{
				_BoldedDates_Persian.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public PersianDateTime[] PersianMonthlyBoldedDates
	{
		get
		{
			return _MonthlyBoldedDates_Persian.ToArray();
		}
		set
		{
			_MonthlyBoldedDates_Persian.Clear();
			foreach (PersianDateTime item in value)
			{
				_MonthlyBoldedDates_Persian.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public PersianDateTime[] PersianAnnuallyBoldedDates
	{
		get
		{
			return _AnnuallyBoldedDates_Persian.ToArray();
		}
		set
		{
			_AnnuallyBoldedDates_Persian.Clear();
			foreach (PersianDateTime item in value)
			{
				_AnnuallyBoldedDates_Persian.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public PersianDateTime[] PersianHolidayDates
	{
		get
		{
			return _HolidayDates_Persian.ToArray();
		}
		set
		{
			_HolidayDates_Persian.Clear();
			foreach (PersianDateTime item in value)
			{
				_HolidayDates_Persian.Add(item);
			}
			LoadMonthCalendarX();
		}
	}

	public ButtonX.ButtonStyles Style_NextMonthButton
	{
		get
		{
			return btnNextMonth.Style;
		}
		set
		{
			if (btnNextMonth.Style != value)
			{
				btnNextMonth.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					btnNextMonth.ShowFirstBorder = false;
				}
				else
				{
					btnNextMonth.ShowFirstBorder = true;
				}
				if (value == ButtonX.ButtonStyles.System)
				{
					((Control)btnNextMonth).BackColor = Color.Transparent;
				}
			}
		}
	}

	public ButtonX.ButtonStyles Style_PreviousMonthButton
	{
		get
		{
			return btnPreMonth.Style;
		}
		set
		{
			if (btnPreMonth.Style != value)
			{
				btnPreMonth.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					btnPreMonth.ShowFirstBorder = false;
				}
				else
				{
					btnPreMonth.ShowFirstBorder = true;
				}
				if (value == ButtonX.ButtonStyles.System)
				{
					((Control)btnPreMonth).BackColor = Color.Transparent;
				}
			}
		}
	}

	public ButtonX.ButtonStyles Style_MonthButton
	{
		get
		{
			return btnMonth.Style;
		}
		set
		{
			if (btnMonth.Style != value)
			{
				btnMonth.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					btnMonth.ShowFirstBorder = false;
				}
				else
				{
					btnMonth.ShowFirstBorder = true;
				}
				if (value == ButtonX.ButtonStyles.System)
				{
					((Control)btnMonth).BackColor = Color.Transparent;
				}
			}
		}
	}

	public ButtonX.ButtonStyles Style_YearButton
	{
		get
		{
			return btnYear.Style;
		}
		set
		{
			if (btnYear.Style != value)
			{
				btnYear.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					btnYear.ShowFirstBorder = false;
				}
				else
				{
					btnYear.ShowFirstBorder = true;
				}
				if (value == ButtonX.ButtonStyles.System)
				{
					((Control)btnYear).BackColor = Color.Transparent;
				}
			}
		}
	}

	public ButtonX.ButtonStyles Style_DaysButton
	{
		get
		{
			return btn1.Style;
		}
		set
		{
			if (btn1.Style == value)
			{
				return;
			}
			if (btn1.Style == ButtonX.ButtonStyles.System)
			{
				btn1.Style = ButtonX.ButtonStyles.Simple;
			}
			if (value == ButtonX.ButtonStyles.System)
			{
				value = btn1.Style;
			}
			for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
			{
				ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
				buttonX.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					buttonX.ShowFirstBorder = false;
				}
				else
				{
					buttonX.ShowFirstBorder = true;
				}
			}
		}
	}

	public ButtonX.ButtonStyles Style_GotoTodayButton
	{
		get
		{
			return btnTodayGoto.Style;
		}
		set
		{
			if (btnTodayGoto.Style != value)
			{
				btnTodayGoto.Style = value;
				if (value == ButtonX.ButtonStyles.Simple)
				{
					btnTodayGoto.ShowFirstBorder = false;
				}
				else
				{
					btnTodayGoto.ShowFirstBorder = true;
				}
				if (value == ButtonX.ButtonStyles.System)
				{
					((Control)btnTodayGoto).BackColor = Color.Transparent;
				}
			}
		}
	}

	public bool ShowToolTips
	{
		get
		{
			return _ShowToolTips;
		}
		set
		{
			if (_ShowToolTips != value)
			{
				_ShowToolTips = value;
				ChangeStyle_ToolTips();
			}
		}
	}

	public event OnSelectedDateChanged SelectedDateChanged;

	public event OnTodayDateChanged TodayDateChanged;

	public event OnCalendarTypeChanged CalendarTypeChanged;

	public event OnDayMouseClick DayMouseClick;

	public event OnDayMouseEnter DayMouseEnter;

	public event OnDayMouseHover DayMouseHover;

	public event OnDayMouseLeave DayMouseLeave;

	public event OnDayMouseMove DayMouseMove;

	public event OnDayMouseUp DayMouseUp;

	public event OnDayKeyDown DayKeyDown;

	public event OnDayKeyUp DayKeyUp;

	public event OnDayKeyPress DayKeyPress;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b22: Expected O, but got Unknown
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Expected O, but got Unknown
		//IL_0cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d05: Expected O, but got Unknown
		//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Expected O, but got Unknown
		//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4f: Expected O, but got Unknown
		//IL_0eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Expected O, but got Unknown
		//IL_0f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f99: Expected O, but got Unknown
		//IL_1037: Unknown result type (might be due to invalid IL or missing references)
		//IL_1041: Expected O, but got Unknown
		//IL_18c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d0: Expected O, but got Unknown
		//IL_190b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1915: Expected O, but got Unknown
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_195a: Expected O, but got Unknown
		//IL_1acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad5: Expected O, but got Unknown
		//IL_1b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1a: Expected O, but got Unknown
		//IL_1b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5f: Expected O, but got Unknown
		//IL_1ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd6: Expected O, but got Unknown
		//IL_1d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1b: Expected O, but got Unknown
		//IL_1d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d60: Expected O, but got Unknown
		//IL_1ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed8: Expected O, but got Unknown
		//IL_1f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1d: Expected O, but got Unknown
		//IL_1f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f62: Expected O, but got Unknown
		//IL_20cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d9: Expected O, but got Unknown
		//IL_2114: Unknown result type (might be due to invalid IL or missing references)
		//IL_211e: Expected O, but got Unknown
		//IL_2159: Unknown result type (might be due to invalid IL or missing references)
		//IL_2163: Expected O, but got Unknown
		//IL_22d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22dd: Expected O, but got Unknown
		//IL_2318: Unknown result type (might be due to invalid IL or missing references)
		//IL_2322: Expected O, but got Unknown
		//IL_235d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2367: Expected O, but got Unknown
		//IL_24d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24df: Expected O, but got Unknown
		//IL_251a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2524: Expected O, but got Unknown
		//IL_255f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2569: Expected O, but got Unknown
		//IL_26d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e3: Expected O, but got Unknown
		//IL_271e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2728: Expected O, but got Unknown
		//IL_2763: Unknown result type (might be due to invalid IL or missing references)
		//IL_276d: Expected O, but got Unknown
		//IL_28da: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e4: Expected O, but got Unknown
		//IL_291f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2929: Expected O, but got Unknown
		//IL_2964: Unknown result type (might be due to invalid IL or missing references)
		//IL_296e: Expected O, but got Unknown
		//IL_2adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae6: Expected O, but got Unknown
		//IL_2b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2b: Expected O, but got Unknown
		//IL_2b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b70: Expected O, but got Unknown
		//IL_2cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce7: Expected O, but got Unknown
		//IL_2d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2c: Expected O, but got Unknown
		//IL_2d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d71: Expected O, but got Unknown
		//IL_2edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee7: Expected O, but got Unknown
		//IL_2f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2c: Expected O, but got Unknown
		//IL_2f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f71: Expected O, but got Unknown
		//IL_30df: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e9: Expected O, but got Unknown
		//IL_3124: Unknown result type (might be due to invalid IL or missing references)
		//IL_312e: Expected O, but got Unknown
		//IL_3169: Unknown result type (might be due to invalid IL or missing references)
		//IL_3173: Expected O, but got Unknown
		//IL_32e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ee: Expected O, but got Unknown
		//IL_3329: Unknown result type (might be due to invalid IL or missing references)
		//IL_3333: Expected O, but got Unknown
		//IL_336e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3378: Expected O, but got Unknown
		//IL_34e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_34f0: Expected O, but got Unknown
		//IL_352b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3535: Expected O, but got Unknown
		//IL_3570: Unknown result type (might be due to invalid IL or missing references)
		//IL_357a: Expected O, but got Unknown
		//IL_36eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f5: Expected O, but got Unknown
		//IL_3730: Unknown result type (might be due to invalid IL or missing references)
		//IL_373a: Expected O, but got Unknown
		//IL_3775: Unknown result type (might be due to invalid IL or missing references)
		//IL_377f: Expected O, but got Unknown
		//IL_38ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f7: Expected O, but got Unknown
		//IL_3932: Unknown result type (might be due to invalid IL or missing references)
		//IL_393c: Expected O, but got Unknown
		//IL_3977: Unknown result type (might be due to invalid IL or missing references)
		//IL_3981: Expected O, but got Unknown
		//IL_3af2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3afc: Expected O, but got Unknown
		//IL_3b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b41: Expected O, but got Unknown
		//IL_3b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b86: Expected O, but got Unknown
		//IL_3cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d01: Expected O, but got Unknown
		//IL_3d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d46: Expected O, but got Unknown
		//IL_3d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8b: Expected O, but got Unknown
		//IL_3ef8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f02: Expected O, but got Unknown
		//IL_3f3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f47: Expected O, but got Unknown
		//IL_3f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f8c: Expected O, but got Unknown
		//IL_40fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4104: Expected O, but got Unknown
		//IL_413f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4149: Expected O, but got Unknown
		//IL_4184: Unknown result type (might be due to invalid IL or missing references)
		//IL_418e: Expected O, but got Unknown
		//IL_42fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4306: Expected O, but got Unknown
		//IL_4341: Unknown result type (might be due to invalid IL or missing references)
		//IL_434b: Expected O, but got Unknown
		//IL_4386: Unknown result type (might be due to invalid IL or missing references)
		//IL_4390: Expected O, but got Unknown
		//IL_44fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4508: Expected O, but got Unknown
		//IL_4543: Unknown result type (might be due to invalid IL or missing references)
		//IL_454d: Expected O, but got Unknown
		//IL_4588: Unknown result type (might be due to invalid IL or missing references)
		//IL_4592: Expected O, but got Unknown
		//IL_4700: Unknown result type (might be due to invalid IL or missing references)
		//IL_470a: Expected O, but got Unknown
		//IL_4745: Unknown result type (might be due to invalid IL or missing references)
		//IL_474f: Expected O, but got Unknown
		//IL_478a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4794: Expected O, but got Unknown
		//IL_4901: Unknown result type (might be due to invalid IL or missing references)
		//IL_490b: Expected O, but got Unknown
		//IL_4946: Unknown result type (might be due to invalid IL or missing references)
		//IL_4950: Expected O, but got Unknown
		//IL_498b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4995: Expected O, but got Unknown
		//IL_4b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b0d: Expected O, but got Unknown
		//IL_4b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b52: Expected O, but got Unknown
		//IL_4b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b97: Expected O, but got Unknown
		//IL_4d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d12: Expected O, but got Unknown
		//IL_4d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d57: Expected O, but got Unknown
		//IL_4d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d9c: Expected O, but got Unknown
		//IL_4f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f14: Expected O, but got Unknown
		//IL_4f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f59: Expected O, but got Unknown
		//IL_4f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f9e: Expected O, but got Unknown
		//IL_510f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5119: Expected O, but got Unknown
		//IL_5154: Unknown result type (might be due to invalid IL or missing references)
		//IL_515e: Expected O, but got Unknown
		//IL_5199: Unknown result type (might be due to invalid IL or missing references)
		//IL_51a3: Expected O, but got Unknown
		//IL_5311: Unknown result type (might be due to invalid IL or missing references)
		//IL_531b: Expected O, but got Unknown
		//IL_5356: Unknown result type (might be due to invalid IL or missing references)
		//IL_5360: Expected O, but got Unknown
		//IL_539b: Unknown result type (might be due to invalid IL or missing references)
		//IL_53a5: Expected O, but got Unknown
		//IL_5513: Unknown result type (might be due to invalid IL or missing references)
		//IL_551d: Expected O, but got Unknown
		//IL_5558: Unknown result type (might be due to invalid IL or missing references)
		//IL_5562: Expected O, but got Unknown
		//IL_559d: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a7: Expected O, but got Unknown
		//IL_5718: Unknown result type (might be due to invalid IL or missing references)
		//IL_5722: Expected O, but got Unknown
		//IL_575d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5767: Expected O, but got Unknown
		//IL_57a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ac: Expected O, but got Unknown
		//IL_591a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5924: Expected O, but got Unknown
		//IL_595f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5969: Expected O, but got Unknown
		//IL_59a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ae: Expected O, but got Unknown
		//IL_5b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b25: Expected O, but got Unknown
		//IL_5b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b6a: Expected O, but got Unknown
		//IL_5ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5baf: Expected O, but got Unknown
		//IL_5d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d27: Expected O, but got Unknown
		//IL_5d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d6c: Expected O, but got Unknown
		//IL_5da7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db1: Expected O, but got Unknown
		//IL_5f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f29: Expected O, but got Unknown
		//IL_5f64: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f6e: Expected O, but got Unknown
		//IL_5fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fb3: Expected O, but got Unknown
		//IL_6120: Unknown result type (might be due to invalid IL or missing references)
		//IL_612a: Expected O, but got Unknown
		//IL_6165: Unknown result type (might be due to invalid IL or missing references)
		//IL_616f: Expected O, but got Unknown
		//IL_61aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_61b4: Expected O, but got Unknown
		//IL_6322: Unknown result type (might be due to invalid IL or missing references)
		//IL_632c: Expected O, but got Unknown
		//IL_6367: Unknown result type (might be due to invalid IL or missing references)
		//IL_6371: Expected O, but got Unknown
		//IL_63ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b6: Expected O, but got Unknown
		//IL_6527: Unknown result type (might be due to invalid IL or missing references)
		//IL_6531: Expected O, but got Unknown
		//IL_656c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6576: Expected O, but got Unknown
		//IL_65b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_65bb: Expected O, but got Unknown
		//IL_6729: Unknown result type (might be due to invalid IL or missing references)
		//IL_6733: Expected O, but got Unknown
		//IL_676e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6778: Expected O, but got Unknown
		//IL_67b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_67bd: Expected O, but got Unknown
		//IL_692e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6938: Expected O, but got Unknown
		//IL_6973: Unknown result type (might be due to invalid IL or missing references)
		//IL_697d: Expected O, but got Unknown
		//IL_69b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c2: Expected O, but got Unknown
		//IL_6b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b3a: Expected O, but got Unknown
		//IL_6b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b7f: Expected O, but got Unknown
		//IL_6bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bc4: Expected O, but got Unknown
		//IL_6d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d3b: Expected O, but got Unknown
		//IL_6ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_6df7: Expected O, but got Unknown
		components = new Container();
		pnlTitles = new Panel();
		nudYear = new NumericUpDown();
		btnMonth = new ButtonX();
		btnYear = new ButtonX();
		btnPreMonth = new ButtonX();
		btnNextMonth = new ButtonX();
		pnlWeekDays = new Panel();
		lblSat = new Label();
		lblSun = new Label();
		lblMon = new Label();
		lblTue = new Label();
		lblWed = new Label();
		lblThu = new Label();
		lblFri = new Label();
		pnlToday = new Panel();
		btnTodayGoto = new ButtonX();
		lblTodayText = new Label();
		pnlDays = new Panel();
		btn42 = new ButtonX();
		btn30 = new ButtonX();
		btn4 = new ButtonX();
		btn41 = new ButtonX();
		btn3 = new ButtonX();
		btn2 = new ButtonX();
		btn40 = new ButtonX();
		btn1 = new ButtonX();
		btn5 = new ButtonX();
		btn39 = new ButtonX();
		btn6 = new ButtonX();
		btn7 = new ButtonX();
		btn38 = new ButtonX();
		btn8 = new ButtonX();
		btn11 = new ButtonX();
		btn37 = new ButtonX();
		btn10 = new ButtonX();
		btn36 = new ButtonX();
		btn9 = new ButtonX();
		btn35 = new ButtonX();
		btn12 = new ButtonX();
		btn34 = new ButtonX();
		btn13 = new ButtonX();
		btn33 = new ButtonX();
		btn14 = new ButtonX();
		btn32 = new ButtonX();
		btn15 = new ButtonX();
		btn31 = new ButtonX();
		btn16 = new ButtonX();
		btn17 = new ButtonX();
		btn18 = new ButtonX();
		btn29 = new ButtonX();
		btn19 = new ButtonX();
		btn28 = new ButtonX();
		btn20 = new ButtonX();
		btn27 = new ButtonX();
		btn21 = new ButtonX();
		btn26 = new ButtonX();
		btn22 = new ButtonX();
		btn25 = new ButtonX();
		btn23 = new ButtonX();
		btn24 = new ButtonX();
		btnDayClicked = new ButtonX();
		lblLineWeek = new Label();
		cmsMonthNames = new ContextMenuStrip(components);
		toolTip1 = new ToolTip(components);
		((Control)pnlTitles).SuspendLayout();
		((ISupportInitialize)nudYear).BeginInit();
		((Control)pnlWeekDays).SuspendLayout();
		((Control)pnlToday).SuspendLayout();
		((Control)pnlDays).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pnlTitles).BackColor = SystemColors.ControlLight;
		((Control)pnlTitles).Controls.Add((Control)(object)nudYear);
		((Control)pnlTitles).Controls.Add((Control)(object)btnMonth);
		((Control)pnlTitles).Controls.Add((Control)(object)btnYear);
		((Control)pnlTitles).Controls.Add((Control)(object)btnPreMonth);
		((Control)pnlTitles).Controls.Add((Control)(object)btnNextMonth);
		((Control)pnlTitles).Location = new Point(0, 0);
		((Control)pnlTitles).Name = "pnlTitles";
		((Control)pnlTitles).Size = new Size(200, 30);
		((Control)pnlTitles).TabIndex = 0;
		((Control)pnlTitles).Click += AllControls_Click;
		((Control)nudYear).Location = new Point(121, 0);
		nudYear.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		nudYear.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)nudYear).Name = "nudYear";
		((Control)nudYear).Size = new Size(49, 20);
		((Control)nudYear).TabIndex = 5;
		((Control)nudYear).TabStop = false;
		((UpDownBase)nudYear).TextAlign = (HorizontalAlignment)2;
		nudYear.Value = new decimal(new int[4] { 2011, 0, 0, 0 });
		nudYear.ValueChanged += nudYear_ValueChanged;
		((Control)nudYear).Leave += nudYear_Leave;
		((Control)nudYear).Enter += nudYear_Enter;
		btnMonth.BoldedForeColor = Color.Blue;
		btnMonth.HolidayForeColor = Color.Red;
		btnMonth.ImageFixedSize = new Size(0, 0);
		btnMonth.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnMonth.IsBolded = false;
		btnMonth.IsHoliday = false;
		btnMonth.IsTrailing = false;
		((Control)btnMonth).Location = new Point(37, 5);
		((Control)btnMonth).Name = "btnMonth";
		btnMonth.PushedAlways = false;
		btnMonth.SecondBorderColor = Color.Red;
		btnMonth.SecondBorderDistanceToEdge = 3f;
		btnMonth.SecondBorderWidth = 3f;
		btnMonth.ShowFirstBorder = true;
		btnMonth.ShowSecondBorder = false;
		((Control)btnMonth).Size = new Size(74, 21);
		btnMonth.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnMonth).TabIndex = 4;
		((Control)btnMonth).TabStop = false;
		((Control)btnMonth).Text = "August";
		btnMonth.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnMonth).UseVisualStyleBackColor = true;
		((Control)btnMonth).Click += AllControls_Click;
		((Control)btnMonth).Leave += BtnX_Leave;
		((Control)btnMonth).MouseClick += new MouseEventHandler(btnMonth_MouseClick);
		((Control)btnMonth).Enter += BtnX_Enter;
		btnYear.BoldedForeColor = Color.Blue;
		btnYear.HolidayForeColor = Color.Red;
		btnYear.ImageFixedSize = new Size(0, 0);
		btnYear.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnYear.IsBolded = false;
		btnYear.IsHoliday = false;
		btnYear.IsTrailing = false;
		((Control)btnYear).Location = new Point(116, 5);
		((Control)btnYear).Name = "btnYear";
		btnYear.PushedAlways = false;
		btnYear.SecondBorderColor = Color.Red;
		btnYear.SecondBorderDistanceToEdge = 3f;
		btnYear.SecondBorderWidth = 3f;
		btnYear.ShowFirstBorder = true;
		btnYear.ShowSecondBorder = false;
		((Control)btnYear).Size = new Size(49, 21);
		btnYear.Style = ButtonX.ButtonStyles.Blue;
		((Control)btnYear).TabIndex = 4;
		((Control)btnYear).TabStop = false;
		((Control)btnYear).Text = "2011";
		btnYear.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnYear).UseVisualStyleBackColor = true;
		((Control)btnYear).Click += btnYear_Click;
		((Control)btnYear).Leave += BtnX_Leave;
		((Control)btnYear).Enter += BtnX_Enter;
		btnPreMonth.BoldedForeColor = Color.Blue;
		btnPreMonth.HolidayForeColor = Color.Red;
		btnPreMonth.ImageFixedSize = new Size(0, 0);
		btnPreMonth.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnPreMonth.IsBolded = false;
		btnPreMonth.IsHoliday = false;
		btnPreMonth.IsTrailing = false;
		((Control)btnPreMonth).Location = new Point(7, 5);
		((Control)btnPreMonth).Name = "btnPreMonth";
		btnPreMonth.PushedAlways = false;
		btnPreMonth.SecondBorderColor = Color.Red;
		btnPreMonth.SecondBorderDistanceToEdge = 3f;
		btnPreMonth.SecondBorderWidth = 3f;
		btnPreMonth.ShowFirstBorder = true;
		btnPreMonth.ShowSecondBorder = false;
		((Control)btnPreMonth).Size = new Size(19, 21);
		btnPreMonth.Style = ButtonX.ButtonStyles.Green;
		((Control)btnPreMonth).TabIndex = 4;
		((Control)btnPreMonth).TabStop = false;
		((Control)btnPreMonth).Text = "<";
		btnPreMonth.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnPreMonth).UseVisualStyleBackColor = true;
		((Control)btnPreMonth).Click += AllControls_Click;
		((Control)btnPreMonth).Leave += BtnX_Leave;
		((Control)btnPreMonth).MouseDown += new MouseEventHandler(btnPreMonth_MouseDown);
		((Control)btnPreMonth).Enter += BtnX_Enter;
		btnNextMonth.BoldedForeColor = Color.Blue;
		btnNextMonth.HolidayForeColor = Color.Red;
		btnNextMonth.ImageFixedSize = new Size(0, 0);
		btnNextMonth.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnNextMonth.IsBolded = false;
		btnNextMonth.IsHoliday = false;
		btnNextMonth.IsTrailing = false;
		((Control)btnNextMonth).Location = new Point(171, 5);
		((Control)btnNextMonth).Name = "btnNextMonth";
		btnNextMonth.PushedAlways = false;
		btnNextMonth.SecondBorderColor = Color.Red;
		btnNextMonth.SecondBorderDistanceToEdge = 3f;
		btnNextMonth.SecondBorderWidth = 3f;
		btnNextMonth.ShowFirstBorder = true;
		btnNextMonth.ShowSecondBorder = false;
		((Control)btnNextMonth).Size = new Size(25, 21);
		btnNextMonth.Style = ButtonX.ButtonStyles.Green;
		((Control)btnNextMonth).TabIndex = 4;
		((Control)btnNextMonth).TabStop = false;
		((Control)btnNextMonth).Text = ">";
		btnNextMonth.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnNextMonth).UseVisualStyleBackColor = true;
		((Control)btnNextMonth).Click += AllControls_Click;
		((Control)btnNextMonth).Leave += BtnX_Leave;
		((Control)btnNextMonth).MouseDown += new MouseEventHandler(btnNextMonth_MouseDown);
		((Control)btnNextMonth).Enter += BtnX_Enter;
		((Control)pnlWeekDays).BackColor = Color.Cornsilk;
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblSat);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblSun);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblMon);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblTue);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblWed);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblThu);
		((Control)pnlWeekDays).Controls.Add((Control)(object)lblFri);
		((Control)pnlWeekDays).Location = new Point(0, 30);
		((Control)pnlWeekDays).Name = "pnlWeekDays";
		((Control)pnlWeekDays).Size = new Size(200, 20);
		((Control)pnlWeekDays).TabIndex = 1;
		((Control)pnlWeekDays).Click += AllControls_Click;
		((Control)lblSat).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblSat).Location = new Point(3, 4);
		((Control)lblSat).Name = "lblSat";
		((Control)lblSat).Size = new Size(21, 14);
		((Control)lblSat).TabIndex = 14;
		((Control)lblSat).Text = "Sa";
		lblSat.TextAlign = (ContentAlignment)32;
		lblSat.UseCompatibleTextRendering = true;
		((Control)lblSat).Click += AllControls_Click;
		((Control)lblSun).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblSun).Location = new Point(27, 4);
		((Control)lblSun).Name = "lblSun";
		((Control)lblSun).Size = new Size(21, 14);
		((Control)lblSun).TabIndex = 15;
		((Control)lblSun).Text = "Su";
		lblSun.TextAlign = (ContentAlignment)32;
		lblSun.UseCompatibleTextRendering = true;
		((Control)lblSun).Click += AllControls_Click;
		((Control)lblMon).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMon).Location = new Point(55, 4);
		((Control)lblMon).Name = "lblMon";
		((Control)lblMon).Size = new Size(21, 14);
		((Control)lblMon).TabIndex = 16;
		((Control)lblMon).Text = "Mo";
		lblMon.TextAlign = (ContentAlignment)32;
		lblMon.UseCompatibleTextRendering = true;
		((Control)lblMon).Click += AllControls_Click;
		((Control)lblTue).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTue).Location = new Point(79, 4);
		((Control)lblTue).Name = "lblTue";
		((Control)lblTue).Size = new Size(21, 14);
		((Control)lblTue).TabIndex = 17;
		((Control)lblTue).Text = "Tu";
		lblTue.TextAlign = (ContentAlignment)32;
		lblTue.UseCompatibleTextRendering = true;
		((Control)lblTue).Click += AllControls_Click;
		((Control)lblWed).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblWed).Location = new Point(110, 4);
		((Control)lblWed).Name = "lblWed";
		((Control)lblWed).Size = new Size(21, 14);
		((Control)lblWed).TabIndex = 18;
		((Control)lblWed).Text = "We";
		lblWed.TextAlign = (ContentAlignment)32;
		lblWed.UseCompatibleTextRendering = true;
		((Control)lblWed).Click += AllControls_Click;
		((Control)lblThu).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblThu).Location = new Point(139, 4);
		((Control)lblThu).Name = "lblThu";
		((Control)lblThu).Size = new Size(21, 14);
		((Control)lblThu).TabIndex = 19;
		((Control)lblThu).Text = "Th";
		lblThu.TextAlign = (ContentAlignment)32;
		lblThu.UseCompatibleTextRendering = true;
		((Control)lblThu).Click += AllControls_Click;
		((Control)lblFri).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblFri).Location = new Point(170, 4);
		((Control)lblFri).Name = "lblFri";
		((Control)lblFri).Size = new Size(26, 14);
		((Control)lblFri).TabIndex = 20;
		((Control)lblFri).Text = "Fr";
		lblFri.TextAlign = (ContentAlignment)32;
		lblFri.UseCompatibleTextRendering = true;
		((Control)lblFri).Click += AllControls_Click;
		((Control)pnlToday).BackColor = SystemColors.ControlLight;
		((Control)pnlToday).Controls.Add((Control)(object)btnTodayGoto);
		((Control)pnlToday).Controls.Add((Control)(object)lblTodayText);
		((Control)pnlToday).Location = new Point(0, 173);
		((Control)pnlToday).Name = "pnlToday";
		((Control)pnlToday).Size = new Size(200, 27);
		((Control)pnlToday).TabIndex = 2;
		((Control)pnlToday).Click += AllControls_Click;
		btnTodayGoto.BoldedForeColor = Color.Blue;
		btnTodayGoto.HolidayForeColor = Color.Red;
		btnTodayGoto.ImageFixedSize = new Size(0, 0);
		btnTodayGoto.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnTodayGoto.IsBolded = false;
		btnTodayGoto.IsHoliday = false;
		btnTodayGoto.IsTrailing = false;
		((Control)btnTodayGoto).Location = new Point(3, 3);
		((Control)btnTodayGoto).Name = "btnTodayGoto";
		btnTodayGoto.PushedAlways = false;
		btnTodayGoto.SecondBorderColor = Color.Red;
		btnTodayGoto.SecondBorderDistanceToEdge = 0f;
		btnTodayGoto.SecondBorderWidth = 2f;
		btnTodayGoto.ShowFirstBorder = true;
		btnTodayGoto.ShowSecondBorder = true;
		((Control)btnTodayGoto).Size = new Size(19, 21);
		btnTodayGoto.Style = ButtonX.ButtonStyles.Green;
		((Control)btnTodayGoto).TabIndex = 98;
		((Control)btnTodayGoto).TabStop = false;
		btnTodayGoto.TrailingForeColor = Color.LightGray;
		((ButtonBase)btnTodayGoto).UseVisualStyleBackColor = true;
		((Control)btnTodayGoto).Click += btnTodayGoto_Click;
		((Control)btnTodayGoto).Leave += BtnX_Leave;
		((Control)btnTodayGoto).Enter += BtnX_Enter;
		((Control)lblTodayText).Cursor = Cursors.Default;
		((Control)lblTodayText).Location = new Point(45, 7);
		((Control)lblTodayText).Name = "lblTodayText";
		((Control)lblTodayText).Size = new Size(43, 13);
		((Control)lblTodayText).TabIndex = 96;
		((Control)lblTodayText).Text = "Today :";
		((Control)lblTodayText).Click += AllControls_Click;
		((Control)pnlDays).BackColor = Color.DarkKhaki;
		((Control)pnlDays).Controls.Add((Control)(object)btn42);
		((Control)pnlDays).Controls.Add((Control)(object)btn30);
		((Control)pnlDays).Controls.Add((Control)(object)btn4);
		((Control)pnlDays).Controls.Add((Control)(object)btn41);
		((Control)pnlDays).Controls.Add((Control)(object)btn3);
		((Control)pnlDays).Controls.Add((Control)(object)btn2);
		((Control)pnlDays).Controls.Add((Control)(object)btn40);
		((Control)pnlDays).Controls.Add((Control)(object)btn1);
		((Control)pnlDays).Controls.Add((Control)(object)btn5);
		((Control)pnlDays).Controls.Add((Control)(object)btn39);
		((Control)pnlDays).Controls.Add((Control)(object)btn6);
		((Control)pnlDays).Controls.Add((Control)(object)btn7);
		((Control)pnlDays).Controls.Add((Control)(object)btn38);
		((Control)pnlDays).Controls.Add((Control)(object)btn8);
		((Control)pnlDays).Controls.Add((Control)(object)btn11);
		((Control)pnlDays).Controls.Add((Control)(object)btn37);
		((Control)pnlDays).Controls.Add((Control)(object)btn10);
		((Control)pnlDays).Controls.Add((Control)(object)btn36);
		((Control)pnlDays).Controls.Add((Control)(object)btn9);
		((Control)pnlDays).Controls.Add((Control)(object)btn35);
		((Control)pnlDays).Controls.Add((Control)(object)btn12);
		((Control)pnlDays).Controls.Add((Control)(object)btn34);
		((Control)pnlDays).Controls.Add((Control)(object)btn13);
		((Control)pnlDays).Controls.Add((Control)(object)btn33);
		((Control)pnlDays).Controls.Add((Control)(object)btn14);
		((Control)pnlDays).Controls.Add((Control)(object)btn32);
		((Control)pnlDays).Controls.Add((Control)(object)btn15);
		((Control)pnlDays).Controls.Add((Control)(object)btn31);
		((Control)pnlDays).Controls.Add((Control)(object)btn16);
		((Control)pnlDays).Controls.Add((Control)(object)btn17);
		((Control)pnlDays).Controls.Add((Control)(object)btn18);
		((Control)pnlDays).Controls.Add((Control)(object)btn29);
		((Control)pnlDays).Controls.Add((Control)(object)btn19);
		((Control)pnlDays).Controls.Add((Control)(object)btn28);
		((Control)pnlDays).Controls.Add((Control)(object)btn20);
		((Control)pnlDays).Controls.Add((Control)(object)btn27);
		((Control)pnlDays).Controls.Add((Control)(object)btn21);
		((Control)pnlDays).Controls.Add((Control)(object)btn26);
		((Control)pnlDays).Controls.Add((Control)(object)btn22);
		((Control)pnlDays).Controls.Add((Control)(object)btn25);
		((Control)pnlDays).Controls.Add((Control)(object)btn23);
		((Control)pnlDays).Controls.Add((Control)(object)btn24);
		((Control)pnlDays).Controls.Add((Control)(object)btnDayClicked);
		((Control)pnlDays).Location = new Point(0, 54);
		((Control)pnlDays).Name = "pnlDays";
		((Control)pnlDays).Size = new Size(200, 118);
		((Control)pnlDays).TabIndex = 3;
		((Control)pnlDays).Click += AllControls_Click;
		btn42.BoldedForeColor = Color.Blue;
		btn42.HolidayForeColor = Color.Red;
		btn42.ImageFixedSize = new Size(0, 0);
		btn42.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn42.IsBolded = false;
		btn42.IsHoliday = false;
		btn42.IsTrailing = false;
		((Control)btn42).Location = new Point(7, 97);
		((Control)btn42).Name = "btn42";
		btn42.PushedAlways = false;
		btn42.SecondBorderColor = Color.Red;
		btn42.SecondBorderDistanceToEdge = 0f;
		btn42.SecondBorderWidth = 1f;
		btn42.ShowFirstBorder = false;
		btn42.ShowSecondBorder = false;
		((Control)btn42).Size = new Size(24, 15);
		btn42.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn42).TabIndex = 83;
		((Control)btn42).TabStop = false;
		((Control)btn42).Text = "42";
		btn42.TrailingForeColor = Color.LightGray;
		((Control)btn42).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn42).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn42).Click += Days_Click;
		((Control)btn42).Leave += BtnX_Leave;
		((Control)btn42).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn42).Enter += BtnX_Enter;
		((Control)btn42).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn42).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn42).MouseEnter += DaysOfMonth_MouseEnter;
		btn30.BoldedForeColor = Color.Blue;
		btn30.HolidayForeColor = Color.Red;
		btn30.ImageFixedSize = new Size(0, 0);
		btn30.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn30.IsBolded = false;
		btn30.IsHoliday = false;
		btn30.IsTrailing = false;
		((Control)btn30).Location = new Point(149, 76);
		((Control)btn30).Name = "btn30";
		btn30.PushedAlways = false;
		btn30.SecondBorderColor = Color.Red;
		btn30.SecondBorderDistanceToEdge = 0f;
		btn30.SecondBorderWidth = 1f;
		btn30.ShowFirstBorder = false;
		btn30.ShowSecondBorder = false;
		((Control)btn30).Size = new Size(24, 15);
		btn30.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn30).TabIndex = 71;
		((Control)btn30).TabStop = false;
		((Control)btn30).Text = "30";
		btn30.TrailingForeColor = Color.LightGray;
		((Control)btn30).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn30).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn30).Click += Days_Click;
		((Control)btn30).Leave += BtnX_Leave;
		((Control)btn30).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn30).Enter += BtnX_Enter;
		((Control)btn30).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn30).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn30).MouseEnter += DaysOfMonth_MouseEnter;
		btn4.BoldedForeColor = Color.Blue;
		btn4.HolidayForeColor = Color.Red;
		btn4.ImageFixedSize = new Size(0, 0);
		btn4.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn4.IsBolded = false;
		btn4.IsHoliday = false;
		btn4.IsTrailing = false;
		((Control)btn4).Location = new Point(94, 6);
		((Control)btn4).Name = "btn4";
		btn4.PushedAlways = false;
		btn4.SecondBorderColor = Color.Red;
		btn4.SecondBorderDistanceToEdge = 0f;
		btn4.SecondBorderWidth = 1f;
		btn4.ShowFirstBorder = false;
		btn4.ShowSecondBorder = false;
		((Control)btn4).Size = new Size(24, 15);
		btn4.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn4).TabIndex = 45;
		((Control)btn4).TabStop = false;
		((Control)btn4).Text = "4";
		btn4.TrailingForeColor = Color.LightGray;
		((Control)btn4).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn4).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn4).Click += Days_Click;
		((Control)btn4).Leave += BtnX_Leave;
		((Control)btn4).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn4).Enter += BtnX_Enter;
		((Control)btn4).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn4).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn4).MouseEnter += DaysOfMonth_MouseEnter;
		btn41.BoldedForeColor = Color.Blue;
		btn41.HolidayForeColor = Color.Red;
		btn41.ImageFixedSize = new Size(0, 0);
		btn41.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn41.IsBolded = false;
		btn41.IsHoliday = false;
		btn41.IsTrailing = false;
		((Control)btn41).Location = new Point(33, 97);
		((Control)btn41).Name = "btn41";
		btn41.PushedAlways = false;
		btn41.SecondBorderColor = Color.Red;
		btn41.SecondBorderDistanceToEdge = 0f;
		btn41.SecondBorderWidth = 1f;
		btn41.ShowFirstBorder = false;
		btn41.ShowSecondBorder = false;
		((Control)btn41).Size = new Size(24, 15);
		btn41.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn41).TabIndex = 82;
		((Control)btn41).TabStop = false;
		((Control)btn41).Text = "41";
		btn41.TrailingForeColor = Color.LightGray;
		((Control)btn41).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn41).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn41).Click += Days_Click;
		((Control)btn41).Leave += BtnX_Leave;
		((Control)btn41).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn41).Enter += BtnX_Enter;
		((Control)btn41).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn41).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn41).MouseEnter += DaysOfMonth_MouseEnter;
		btn3.BoldedForeColor = Color.Blue;
		btn3.HolidayForeColor = Color.Red;
		btn3.ImageFixedSize = new Size(0, 0);
		btn3.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn3.IsBolded = false;
		btn3.IsHoliday = false;
		btn3.IsTrailing = false;
		((Control)btn3).Location = new Point(121, 6);
		((Control)btn3).Name = "btn3";
		btn3.PushedAlways = false;
		btn3.SecondBorderColor = Color.Red;
		btn3.SecondBorderDistanceToEdge = 0f;
		btn3.SecondBorderWidth = 1f;
		btn3.ShowFirstBorder = false;
		btn3.ShowSecondBorder = false;
		((Control)btn3).Size = new Size(24, 15);
		btn3.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn3).TabIndex = 44;
		((Control)btn3).TabStop = false;
		((Control)btn3).Text = "3";
		btn3.TrailingForeColor = Color.LightGray;
		((Control)btn3).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn3).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn3).Click += Days_Click;
		((Control)btn3).Leave += BtnX_Leave;
		((Control)btn3).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn3).Enter += BtnX_Enter;
		((Control)btn3).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn3).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn3).MouseEnter += DaysOfMonth_MouseEnter;
		btn2.BoldedForeColor = Color.Blue;
		btn2.HolidayForeColor = Color.Red;
		btn2.ImageFixedSize = new Size(0, 0);
		btn2.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn2.IsBolded = false;
		btn2.IsHoliday = false;
		btn2.IsTrailing = false;
		((Control)btn2).Location = new Point(149, 6);
		((Control)btn2).Name = "btn2";
		btn2.PushedAlways = false;
		btn2.SecondBorderColor = Color.Red;
		btn2.SecondBorderDistanceToEdge = 0f;
		btn2.SecondBorderWidth = 1f;
		btn2.ShowFirstBorder = false;
		btn2.ShowSecondBorder = false;
		((Control)btn2).Size = new Size(24, 15);
		btn2.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn2).TabIndex = 43;
		((Control)btn2).TabStop = false;
		((Control)btn2).Text = "2";
		btn2.TrailingForeColor = Color.LightGray;
		((Control)btn2).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn2).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn2).Click += Days_Click;
		((Control)btn2).Leave += BtnX_Leave;
		((Control)btn2).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn2).Enter += BtnX_Enter;
		((Control)btn2).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn2).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn2).MouseEnter += DaysOfMonth_MouseEnter;
		btn40.BoldedForeColor = Color.Blue;
		btn40.HolidayForeColor = Color.Red;
		btn40.ImageFixedSize = new Size(0, 0);
		btn40.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn40.IsBolded = false;
		btn40.IsHoliday = false;
		btn40.IsTrailing = false;
		((Control)btn40).Location = new Point(64, 97);
		((Control)btn40).Name = "btn40";
		btn40.PushedAlways = false;
		btn40.SecondBorderColor = Color.Red;
		btn40.SecondBorderDistanceToEdge = 0f;
		btn40.SecondBorderWidth = 1f;
		btn40.ShowFirstBorder = false;
		btn40.ShowSecondBorder = false;
		((Control)btn40).Size = new Size(24, 15);
		btn40.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn40).TabIndex = 81;
		((Control)btn40).TabStop = false;
		((Control)btn40).Text = "40";
		btn40.TrailingForeColor = Color.LightGray;
		((Control)btn40).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn40).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn40).Click += Days_Click;
		((Control)btn40).Leave += BtnX_Leave;
		((Control)btn40).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn40).Enter += BtnX_Enter;
		((Control)btn40).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn40).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn40).MouseEnter += DaysOfMonth_MouseEnter;
		btn1.BoldedForeColor = Color.Blue;
		btn1.HolidayForeColor = Color.Red;
		btn1.ImageFixedSize = new Size(0, 0);
		btn1.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn1.IsBolded = false;
		btn1.IsHoliday = false;
		btn1.IsTrailing = false;
		((Control)btn1).Location = new Point(175, 6);
		((Control)btn1).Name = "btn1";
		btn1.PushedAlways = false;
		btn1.SecondBorderColor = Color.Red;
		btn1.SecondBorderDistanceToEdge = 0f;
		btn1.SecondBorderWidth = 1f;
		btn1.ShowFirstBorder = false;
		btn1.ShowSecondBorder = false;
		((Control)btn1).Size = new Size(24, 15);
		btn1.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn1).TabIndex = 42;
		((Control)btn1).TabStop = false;
		((Control)btn1).Text = "1";
		btn1.TrailingForeColor = Color.LightGray;
		((Control)btn1).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn1).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn1).Click += Days_Click;
		((Control)btn1).Leave += BtnX_Leave;
		((Control)btn1).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn1).Enter += BtnX_Enter;
		((Control)btn1).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn1).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn1).MouseEnter += DaysOfMonth_MouseEnter;
		btn5.BoldedForeColor = Color.Blue;
		btn5.HolidayForeColor = Color.Red;
		btn5.ImageFixedSize = new Size(0, 0);
		btn5.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn5.IsBolded = false;
		btn5.IsHoliday = false;
		btn5.IsTrailing = false;
		((Control)btn5).Location = new Point(63, 6);
		((Control)btn5).Name = "btn5";
		btn5.PushedAlways = false;
		btn5.SecondBorderColor = Color.Red;
		btn5.SecondBorderDistanceToEdge = 0f;
		btn5.SecondBorderWidth = 1f;
		btn5.ShowFirstBorder = false;
		btn5.ShowSecondBorder = false;
		((Control)btn5).Size = new Size(24, 15);
		btn5.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn5).TabIndex = 46;
		((Control)btn5).TabStop = false;
		((Control)btn5).Text = "5";
		btn5.TrailingForeColor = Color.LightGray;
		((Control)btn5).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn5).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn5).Click += Days_Click;
		((Control)btn5).Leave += BtnX_Leave;
		((Control)btn5).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn5).Enter += BtnX_Enter;
		((Control)btn5).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn5).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn5).MouseEnter += DaysOfMonth_MouseEnter;
		btn39.BoldedForeColor = Color.Blue;
		btn39.HolidayForeColor = Color.Red;
		btn39.ImageFixedSize = new Size(0, 0);
		btn39.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn39.IsBolded = false;
		btn39.IsHoliday = false;
		btn39.IsTrailing = false;
		((Control)btn39).Location = new Point(94, 97);
		((Control)btn39).Name = "btn39";
		btn39.PushedAlways = false;
		btn39.SecondBorderColor = Color.Red;
		btn39.SecondBorderDistanceToEdge = 0f;
		btn39.SecondBorderWidth = 1f;
		btn39.ShowFirstBorder = false;
		btn39.ShowSecondBorder = false;
		((Control)btn39).Size = new Size(24, 15);
		btn39.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn39).TabIndex = 80;
		((Control)btn39).TabStop = false;
		((Control)btn39).Text = "39";
		btn39.TrailingForeColor = Color.LightGray;
		((Control)btn39).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn39).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn39).Click += Days_Click;
		((Control)btn39).Leave += BtnX_Leave;
		((Control)btn39).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn39).Enter += BtnX_Enter;
		((Control)btn39).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn39).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn39).MouseEnter += DaysOfMonth_MouseEnter;
		btn6.BoldedForeColor = Color.Blue;
		btn6.HolidayForeColor = Color.Red;
		btn6.ImageFixedSize = new Size(0, 0);
		btn6.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn6.IsBolded = false;
		btn6.IsHoliday = false;
		btn6.IsTrailing = false;
		((Control)btn6).Location = new Point(33, 6);
		((Control)btn6).Name = "btn6";
		btn6.PushedAlways = false;
		btn6.SecondBorderColor = Color.Red;
		btn6.SecondBorderDistanceToEdge = 0f;
		btn6.SecondBorderWidth = 1f;
		btn6.ShowFirstBorder = false;
		btn6.ShowSecondBorder = false;
		((Control)btn6).Size = new Size(24, 15);
		btn6.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn6).TabIndex = 47;
		((Control)btn6).TabStop = false;
		((Control)btn6).Text = "6";
		btn6.TrailingForeColor = Color.LightGray;
		((Control)btn6).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn6).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn6).Click += Days_Click;
		((Control)btn6).Leave += BtnX_Leave;
		((Control)btn6).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn6).Enter += BtnX_Enter;
		((Control)btn6).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn6).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn6).MouseEnter += DaysOfMonth_MouseEnter;
		btn7.BoldedForeColor = Color.Blue;
		btn7.HolidayForeColor = Color.Red;
		btn7.ImageFixedSize = new Size(0, 0);
		btn7.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn7.IsBolded = false;
		btn7.IsHoliday = false;
		btn7.IsTrailing = false;
		((Control)btn7).Location = new Point(7, 6);
		((Control)btn7).Name = "btn7";
		btn7.PushedAlways = false;
		btn7.SecondBorderColor = Color.Red;
		btn7.SecondBorderDistanceToEdge = 0f;
		btn7.SecondBorderWidth = 1f;
		btn7.ShowFirstBorder = false;
		btn7.ShowSecondBorder = false;
		((Control)btn7).Size = new Size(24, 15);
		btn7.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn7).TabIndex = 48;
		((Control)btn7).TabStop = false;
		((Control)btn7).Text = "7";
		btn7.TrailingForeColor = Color.LightGray;
		((Control)btn7).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn7).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn7).Click += Days_Click;
		((Control)btn7).Leave += BtnX_Leave;
		((Control)btn7).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn7).Enter += BtnX_Enter;
		((Control)btn7).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn7).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn7).MouseEnter += DaysOfMonth_MouseEnter;
		btn38.BoldedForeColor = Color.Blue;
		btn38.HolidayForeColor = Color.Red;
		btn38.ImageFixedSize = new Size(0, 0);
		btn38.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn38.IsBolded = false;
		btn38.IsHoliday = false;
		btn38.IsTrailing = false;
		((Control)btn38).Location = new Point(119, 97);
		((Control)btn38).Name = "btn38";
		btn38.PushedAlways = false;
		btn38.SecondBorderColor = Color.Red;
		btn38.SecondBorderDistanceToEdge = 0f;
		btn38.SecondBorderWidth = 1f;
		btn38.ShowFirstBorder = false;
		btn38.ShowSecondBorder = false;
		((Control)btn38).Size = new Size(24, 15);
		btn38.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn38).TabIndex = 79;
		((Control)btn38).TabStop = false;
		((Control)btn38).Text = "38";
		btn38.TrailingForeColor = Color.LightGray;
		((Control)btn38).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn38).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn38).Click += Days_Click;
		((Control)btn38).Leave += BtnX_Leave;
		((Control)btn38).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn38).Enter += BtnX_Enter;
		((Control)btn38).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn38).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn38).MouseEnter += DaysOfMonth_MouseEnter;
		btn8.BoldedForeColor = Color.Blue;
		btn8.HolidayForeColor = Color.Red;
		btn8.ImageFixedSize = new Size(0, 0);
		btn8.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn8.IsBolded = false;
		btn8.IsHoliday = false;
		btn8.IsTrailing = false;
		((Control)btn8).Location = new Point(174, 23);
		((Control)btn8).Name = "btn8";
		btn8.PushedAlways = false;
		btn8.SecondBorderColor = Color.Red;
		btn8.SecondBorderDistanceToEdge = 0f;
		btn8.SecondBorderWidth = 1f;
		btn8.ShowFirstBorder = false;
		btn8.ShowSecondBorder = false;
		((Control)btn8).Size = new Size(24, 15);
		btn8.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn8).TabIndex = 49;
		((Control)btn8).TabStop = false;
		((Control)btn8).Text = "8";
		btn8.TrailingForeColor = Color.LightGray;
		((Control)btn8).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn8).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn8).Click += Days_Click;
		((Control)btn8).Leave += BtnX_Leave;
		((Control)btn8).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn8).Enter += BtnX_Enter;
		((Control)btn8).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn8).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn8).MouseEnter += DaysOfMonth_MouseEnter;
		btn11.BoldedForeColor = Color.Blue;
		btn11.HolidayForeColor = Color.Red;
		btn11.ImageFixedSize = new Size(0, 0);
		btn11.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn11.IsBolded = false;
		btn11.IsHoliday = false;
		btn11.IsTrailing = false;
		((Control)btn11).Location = new Point(94, 23);
		((Control)btn11).Name = "btn11";
		btn11.PushedAlways = false;
		btn11.SecondBorderColor = Color.Red;
		btn11.SecondBorderDistanceToEdge = 0f;
		btn11.SecondBorderWidth = 1f;
		btn11.ShowFirstBorder = false;
		btn11.ShowSecondBorder = false;
		((Control)btn11).Size = new Size(24, 15);
		btn11.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn11).TabIndex = 52;
		((Control)btn11).TabStop = false;
		((Control)btn11).Text = "11";
		btn11.TrailingForeColor = Color.LightGray;
		((Control)btn11).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn11).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn11).Click += Days_Click;
		((Control)btn11).Leave += BtnX_Leave;
		((Control)btn11).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn11).Enter += BtnX_Enter;
		((Control)btn11).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn11).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn11).MouseEnter += DaysOfMonth_MouseEnter;
		btn37.BoldedForeColor = Color.Blue;
		btn37.HolidayForeColor = Color.Red;
		btn37.ImageFixedSize = new Size(0, 0);
		btn37.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn37.IsBolded = false;
		btn37.IsHoliday = false;
		btn37.IsTrailing = false;
		((Control)btn37).Location = new Point(148, 97);
		((Control)btn37).Name = "btn37";
		btn37.PushedAlways = false;
		btn37.SecondBorderColor = Color.Red;
		btn37.SecondBorderDistanceToEdge = 0f;
		btn37.SecondBorderWidth = 1f;
		btn37.ShowFirstBorder = false;
		btn37.ShowSecondBorder = false;
		((Control)btn37).Size = new Size(24, 15);
		btn37.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn37).TabIndex = 78;
		((Control)btn37).TabStop = false;
		((Control)btn37).Text = "37";
		btn37.TrailingForeColor = Color.LightGray;
		((Control)btn37).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn37).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn37).Click += Days_Click;
		((Control)btn37).Leave += BtnX_Leave;
		((Control)btn37).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn37).Enter += BtnX_Enter;
		((Control)btn37).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn37).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn37).MouseEnter += DaysOfMonth_MouseEnter;
		btn10.BoldedForeColor = Color.Blue;
		btn10.HolidayForeColor = Color.Red;
		btn10.ImageFixedSize = new Size(0, 0);
		btn10.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn10.IsBolded = false;
		btn10.IsHoliday = false;
		btn10.IsTrailing = false;
		((Control)btn10).Location = new Point(120, 23);
		((Control)btn10).Name = "btn10";
		btn10.PushedAlways = false;
		btn10.SecondBorderColor = Color.Red;
		btn10.SecondBorderDistanceToEdge = 0f;
		btn10.SecondBorderWidth = 1f;
		btn10.ShowFirstBorder = false;
		btn10.ShowSecondBorder = false;
		((Control)btn10).Size = new Size(24, 15);
		btn10.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn10).TabIndex = 51;
		((Control)btn10).TabStop = false;
		((Control)btn10).Text = "10";
		btn10.TrailingForeColor = Color.LightGray;
		((Control)btn10).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn10).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn10).Click += Days_Click;
		((Control)btn10).Leave += BtnX_Leave;
		((Control)btn10).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn10).Enter += BtnX_Enter;
		((Control)btn10).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn10).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn10).MouseEnter += DaysOfMonth_MouseEnter;
		btn36.BoldedForeColor = Color.Blue;
		btn36.HolidayForeColor = Color.Red;
		btn36.ImageFixedSize = new Size(0, 0);
		btn36.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn36.IsBolded = false;
		btn36.IsHoliday = false;
		btn36.IsTrailing = false;
		((Control)btn36).Location = new Point(174, 97);
		((Control)btn36).Name = "btn36";
		btn36.PushedAlways = false;
		btn36.SecondBorderColor = Color.Red;
		btn36.SecondBorderDistanceToEdge = 0f;
		btn36.SecondBorderWidth = 1f;
		btn36.ShowFirstBorder = false;
		btn36.ShowSecondBorder = false;
		((Control)btn36).Size = new Size(24, 15);
		btn36.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn36).TabIndex = 77;
		((Control)btn36).TabStop = false;
		((Control)btn36).Text = "36";
		btn36.TrailingForeColor = Color.LightGray;
		((Control)btn36).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn36).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn36).Click += Days_Click;
		((Control)btn36).Leave += BtnX_Leave;
		((Control)btn36).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn36).Enter += BtnX_Enter;
		((Control)btn36).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn36).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn36).MouseEnter += DaysOfMonth_MouseEnter;
		btn9.BoldedForeColor = Color.Blue;
		btn9.HolidayForeColor = Color.Red;
		btn9.ImageFixedSize = new Size(0, 0);
		btn9.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn9.IsBolded = false;
		btn9.IsHoliday = false;
		btn9.IsTrailing = false;
		((Control)btn9).Location = new Point(148, 23);
		((Control)btn9).Name = "btn9";
		btn9.PushedAlways = false;
		btn9.SecondBorderColor = Color.Red;
		btn9.SecondBorderDistanceToEdge = 0f;
		btn9.SecondBorderWidth = 1f;
		btn9.ShowFirstBorder = false;
		btn9.ShowSecondBorder = false;
		((Control)btn9).Size = new Size(24, 15);
		btn9.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn9).TabIndex = 50;
		((Control)btn9).TabStop = false;
		((Control)btn9).Text = "9";
		btn9.TrailingForeColor = Color.LightGray;
		((Control)btn9).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn9).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn9).Click += Days_Click;
		((Control)btn9).Leave += BtnX_Leave;
		((Control)btn9).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn9).Enter += BtnX_Enter;
		((Control)btn9).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn9).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn9).MouseEnter += DaysOfMonth_MouseEnter;
		btn35.BoldedForeColor = Color.Blue;
		btn35.HolidayForeColor = Color.Red;
		btn35.ImageFixedSize = new Size(0, 0);
		btn35.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn35.IsBolded = false;
		btn35.IsHoliday = false;
		btn35.IsTrailing = false;
		((Control)btn35).Location = new Point(8, 76);
		((Control)btn35).Name = "btn35";
		btn35.PushedAlways = false;
		btn35.SecondBorderColor = Color.Red;
		btn35.SecondBorderDistanceToEdge = 0f;
		btn35.SecondBorderWidth = 1f;
		btn35.ShowFirstBorder = false;
		btn35.ShowSecondBorder = false;
		((Control)btn35).Size = new Size(24, 15);
		btn35.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn35).TabIndex = 76;
		((Control)btn35).TabStop = false;
		((Control)btn35).Text = "35";
		btn35.TrailingForeColor = Color.LightGray;
		((Control)btn35).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn35).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn35).Click += Days_Click;
		((Control)btn35).Leave += BtnX_Leave;
		((Control)btn35).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn35).Enter += BtnX_Enter;
		((Control)btn35).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn35).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn35).MouseEnter += DaysOfMonth_MouseEnter;
		btn12.BoldedForeColor = Color.Blue;
		btn12.HolidayForeColor = Color.Red;
		btn12.ImageFixedSize = new Size(0, 0);
		btn12.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn12.IsBolded = false;
		btn12.IsHoliday = false;
		btn12.IsTrailing = false;
		((Control)btn12).Location = new Point(63, 23);
		((Control)btn12).Name = "btn12";
		btn12.PushedAlways = false;
		btn12.SecondBorderColor = Color.Red;
		btn12.SecondBorderDistanceToEdge = 0f;
		btn12.SecondBorderWidth = 1f;
		btn12.ShowFirstBorder = false;
		btn12.ShowSecondBorder = false;
		((Control)btn12).Size = new Size(24, 15);
		btn12.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn12).TabIndex = 53;
		((Control)btn12).TabStop = false;
		((Control)btn12).Text = "12";
		btn12.TrailingForeColor = Color.LightGray;
		((Control)btn12).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn12).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn12).Click += Days_Click;
		((Control)btn12).Leave += BtnX_Leave;
		((Control)btn12).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn12).Enter += BtnX_Enter;
		((Control)btn12).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn12).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn12).MouseEnter += DaysOfMonth_MouseEnter;
		btn34.BoldedForeColor = Color.Blue;
		btn34.HolidayForeColor = Color.Red;
		btn34.ImageFixedSize = new Size(0, 0);
		btn34.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn34.IsBolded = false;
		btn34.IsHoliday = false;
		btn34.IsTrailing = false;
		((Control)btn34).Location = new Point(33, 76);
		((Control)btn34).Name = "btn34";
		btn34.PushedAlways = false;
		btn34.SecondBorderColor = Color.Red;
		btn34.SecondBorderDistanceToEdge = 0f;
		btn34.SecondBorderWidth = 1f;
		btn34.ShowFirstBorder = false;
		btn34.ShowSecondBorder = false;
		((Control)btn34).Size = new Size(24, 15);
		btn34.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn34).TabIndex = 75;
		((Control)btn34).TabStop = false;
		((Control)btn34).Text = "34";
		btn34.TrailingForeColor = Color.LightGray;
		((Control)btn34).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn34).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn34).Click += Days_Click;
		((Control)btn34).Leave += BtnX_Leave;
		((Control)btn34).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn34).Enter += BtnX_Enter;
		((Control)btn34).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn34).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn34).MouseEnter += DaysOfMonth_MouseEnter;
		btn13.BoldedForeColor = Color.Blue;
		btn13.HolidayForeColor = Color.Red;
		btn13.ImageFixedSize = new Size(0, 0);
		btn13.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn13.IsBolded = false;
		btn13.IsHoliday = false;
		btn13.IsTrailing = false;
		((Control)btn13).Location = new Point(33, 23);
		((Control)btn13).Name = "btn13";
		btn13.PushedAlways = false;
		btn13.SecondBorderColor = Color.Red;
		btn13.SecondBorderDistanceToEdge = 0f;
		btn13.SecondBorderWidth = 1f;
		btn13.ShowFirstBorder = false;
		btn13.ShowSecondBorder = false;
		((Control)btn13).Size = new Size(24, 15);
		btn13.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn13).TabIndex = 54;
		((Control)btn13).TabStop = false;
		((Control)btn13).Text = "13";
		btn13.TrailingForeColor = Color.LightGray;
		((Control)btn13).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn13).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn13).Click += Days_Click;
		((Control)btn13).Leave += BtnX_Leave;
		((Control)btn13).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn13).Enter += BtnX_Enter;
		((Control)btn13).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn13).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn13).MouseEnter += DaysOfMonth_MouseEnter;
		btn33.BoldedForeColor = Color.Blue;
		btn33.HolidayForeColor = Color.Red;
		btn33.ImageFixedSize = new Size(0, 0);
		btn33.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn33.IsBolded = false;
		btn33.IsHoliday = false;
		btn33.IsTrailing = false;
		((Control)btn33).Location = new Point(63, 76);
		((Control)btn33).Name = "btn33";
		btn33.PushedAlways = false;
		btn33.SecondBorderColor = Color.Red;
		btn33.SecondBorderDistanceToEdge = 0f;
		btn33.SecondBorderWidth = 1f;
		btn33.ShowFirstBorder = false;
		btn33.ShowSecondBorder = false;
		((Control)btn33).Size = new Size(24, 15);
		btn33.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn33).TabIndex = 74;
		((Control)btn33).TabStop = false;
		((Control)btn33).Text = "33";
		btn33.TrailingForeColor = Color.LightGray;
		((Control)btn33).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn33).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn33).Click += Days_Click;
		((Control)btn33).Leave += BtnX_Leave;
		((Control)btn33).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn33).Enter += BtnX_Enter;
		((Control)btn33).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn33).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn33).MouseEnter += DaysOfMonth_MouseEnter;
		btn14.BoldedForeColor = Color.Blue;
		btn14.HolidayForeColor = Color.Red;
		btn14.ImageFixedSize = new Size(0, 0);
		btn14.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn14.IsBolded = false;
		btn14.IsHoliday = false;
		btn14.IsTrailing = false;
		((Control)btn14).Location = new Point(6, 23);
		((Control)btn14).Name = "btn14";
		btn14.PushedAlways = false;
		btn14.SecondBorderColor = Color.Red;
		btn14.SecondBorderDistanceToEdge = 0f;
		btn14.SecondBorderWidth = 1f;
		btn14.ShowFirstBorder = false;
		btn14.ShowSecondBorder = false;
		((Control)btn14).Size = new Size(24, 15);
		btn14.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn14).TabIndex = 55;
		((Control)btn14).TabStop = false;
		((Control)btn14).Text = "14";
		btn14.TrailingForeColor = Color.LightGray;
		((Control)btn14).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn14).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn14).Click += Days_Click;
		((Control)btn14).Leave += BtnX_Leave;
		((Control)btn14).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn14).Enter += BtnX_Enter;
		((Control)btn14).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn14).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn14).MouseEnter += DaysOfMonth_MouseEnter;
		btn32.BoldedForeColor = Color.Blue;
		btn32.HolidayForeColor = Color.Red;
		btn32.ImageFixedSize = new Size(0, 0);
		btn32.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn32.IsBolded = false;
		btn32.IsHoliday = false;
		btn32.IsTrailing = false;
		((Control)btn32).Location = new Point(94, 76);
		((Control)btn32).Name = "btn32";
		btn32.PushedAlways = false;
		btn32.SecondBorderColor = Color.Red;
		btn32.SecondBorderDistanceToEdge = 0f;
		btn32.SecondBorderWidth = 1f;
		btn32.ShowFirstBorder = false;
		btn32.ShowSecondBorder = false;
		((Control)btn32).Size = new Size(24, 15);
		btn32.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn32).TabIndex = 73;
		((Control)btn32).TabStop = false;
		((Control)btn32).Text = "32";
		btn32.TrailingForeColor = Color.LightGray;
		((Control)btn32).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn32).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn32).Click += Days_Click;
		((Control)btn32).Leave += BtnX_Leave;
		((Control)btn32).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn32).Enter += BtnX_Enter;
		((Control)btn32).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn32).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn32).MouseEnter += DaysOfMonth_MouseEnter;
		btn15.BoldedForeColor = Color.Blue;
		btn15.HolidayForeColor = Color.Red;
		btn15.ImageFixedSize = new Size(0, 0);
		btn15.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn15.IsBolded = false;
		btn15.IsHoliday = false;
		btn15.IsTrailing = false;
		((Control)btn15).Location = new Point(174, 41);
		((Control)btn15).Name = "btn15";
		btn15.PushedAlways = false;
		btn15.SecondBorderColor = Color.Red;
		btn15.SecondBorderDistanceToEdge = 0f;
		btn15.SecondBorderWidth = 1f;
		btn15.ShowFirstBorder = false;
		btn15.ShowSecondBorder = false;
		((Control)btn15).Size = new Size(24, 15);
		btn15.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn15).TabIndex = 56;
		((Control)btn15).TabStop = false;
		((Control)btn15).Text = "15";
		btn15.TrailingForeColor = Color.LightGray;
		((Control)btn15).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn15).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn15).Click += Days_Click;
		((Control)btn15).Leave += BtnX_Leave;
		((Control)btn15).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn15).Enter += BtnX_Enter;
		((Control)btn15).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn15).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn15).MouseEnter += DaysOfMonth_MouseEnter;
		btn31.BoldedForeColor = Color.Blue;
		btn31.HolidayForeColor = Color.Red;
		btn31.ImageFixedSize = new Size(0, 0);
		btn31.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn31.IsBolded = false;
		btn31.IsHoliday = false;
		btn31.IsTrailing = false;
		((Control)btn31).Location = new Point(120, 76);
		((Control)btn31).Name = "btn31";
		btn31.PushedAlways = false;
		btn31.SecondBorderColor = Color.Red;
		btn31.SecondBorderDistanceToEdge = 0f;
		btn31.SecondBorderWidth = 1f;
		btn31.ShowFirstBorder = false;
		btn31.ShowSecondBorder = false;
		((Control)btn31).Size = new Size(24, 15);
		btn31.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn31).TabIndex = 72;
		((Control)btn31).TabStop = false;
		((Control)btn31).Text = "31";
		btn31.TrailingForeColor = Color.LightGray;
		((Control)btn31).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn31).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn31).Click += Days_Click;
		((Control)btn31).Leave += BtnX_Leave;
		((Control)btn31).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn31).Enter += BtnX_Enter;
		((Control)btn31).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn31).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn31).MouseEnter += DaysOfMonth_MouseEnter;
		btn16.BoldedForeColor = Color.Blue;
		btn16.HolidayForeColor = Color.Red;
		btn16.ImageFixedSize = new Size(0, 0);
		btn16.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn16.IsBolded = false;
		btn16.IsHoliday = false;
		btn16.IsTrailing = false;
		((Control)btn16).Location = new Point(148, 41);
		((Control)btn16).Name = "btn16";
		btn16.PushedAlways = false;
		btn16.SecondBorderColor = Color.Red;
		btn16.SecondBorderDistanceToEdge = 0f;
		btn16.SecondBorderWidth = 1f;
		btn16.ShowFirstBorder = false;
		btn16.ShowSecondBorder = false;
		((Control)btn16).Size = new Size(24, 15);
		btn16.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn16).TabIndex = 57;
		((Control)btn16).TabStop = false;
		((Control)btn16).Text = "16";
		btn16.TrailingForeColor = Color.LightGray;
		((Control)btn16).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn16).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn16).Click += Days_Click;
		((Control)btn16).Leave += BtnX_Leave;
		((Control)btn16).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn16).Enter += BtnX_Enter;
		((Control)btn16).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn16).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn16).MouseEnter += DaysOfMonth_MouseEnter;
		btn17.BoldedForeColor = Color.Blue;
		btn17.HolidayForeColor = Color.Red;
		btn17.ImageFixedSize = new Size(0, 0);
		btn17.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn17.IsBolded = false;
		btn17.IsHoliday = false;
		btn17.IsTrailing = false;
		((Control)btn17).Location = new Point(121, 41);
		((Control)btn17).Name = "btn17";
		btn17.PushedAlways = false;
		btn17.SecondBorderColor = Color.Red;
		btn17.SecondBorderDistanceToEdge = 0f;
		btn17.SecondBorderWidth = 1f;
		btn17.ShowFirstBorder = false;
		btn17.ShowSecondBorder = false;
		((Control)btn17).Size = new Size(24, 15);
		btn17.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn17).TabIndex = 58;
		((Control)btn17).TabStop = false;
		((Control)btn17).Text = "17";
		btn17.TrailingForeColor = Color.LightGray;
		((Control)btn17).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn17).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn17).Click += Days_Click;
		((Control)btn17).Leave += BtnX_Leave;
		((Control)btn17).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn17).Enter += BtnX_Enter;
		((Control)btn17).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn17).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn17).MouseEnter += DaysOfMonth_MouseEnter;
		btn18.BoldedForeColor = Color.Blue;
		btn18.HolidayForeColor = Color.Red;
		btn18.ImageFixedSize = new Size(0, 0);
		btn18.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn18.IsBolded = false;
		btn18.IsHoliday = false;
		btn18.IsTrailing = false;
		((Control)btn18).Location = new Point(94, 41);
		((Control)btn18).Name = "btn18";
		btn18.PushedAlways = false;
		btn18.SecondBorderColor = Color.Red;
		btn18.SecondBorderDistanceToEdge = 0f;
		btn18.SecondBorderWidth = 1f;
		btn18.ShowFirstBorder = false;
		btn18.ShowSecondBorder = false;
		((Control)btn18).Size = new Size(24, 15);
		btn18.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn18).TabIndex = 59;
		((Control)btn18).TabStop = false;
		((Control)btn18).Text = "18";
		btn18.TrailingForeColor = Color.LightGray;
		((Control)btn18).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn18).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn18).Click += Days_Click;
		((Control)btn18).Leave += BtnX_Leave;
		((Control)btn18).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn18).Enter += BtnX_Enter;
		((Control)btn18).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn18).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn18).MouseEnter += DaysOfMonth_MouseEnter;
		btn29.BoldedForeColor = Color.Blue;
		btn29.HolidayForeColor = Color.Red;
		btn29.ImageFixedSize = new Size(0, 0);
		btn29.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn29.IsBolded = false;
		btn29.IsHoliday = false;
		btn29.IsTrailing = false;
		((Control)btn29).Location = new Point(174, 76);
		((Control)btn29).Name = "btn29";
		btn29.PushedAlways = false;
		btn29.SecondBorderColor = Color.Red;
		btn29.SecondBorderDistanceToEdge = 0f;
		btn29.SecondBorderWidth = 1f;
		btn29.ShowFirstBorder = false;
		btn29.ShowSecondBorder = false;
		((Control)btn29).Size = new Size(24, 15);
		btn29.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn29).TabIndex = 70;
		((Control)btn29).TabStop = false;
		((Control)btn29).Text = "29";
		btn29.TrailingForeColor = Color.LightGray;
		((Control)btn29).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn29).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn29).Click += Days_Click;
		((Control)btn29).Leave += BtnX_Leave;
		((Control)btn29).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn29).Enter += BtnX_Enter;
		((Control)btn29).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn29).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn29).MouseEnter += DaysOfMonth_MouseEnter;
		btn19.BoldedForeColor = Color.Blue;
		btn19.HolidayForeColor = Color.Red;
		btn19.ImageFixedSize = new Size(0, 0);
		btn19.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn19.IsBolded = false;
		btn19.IsHoliday = false;
		btn19.IsTrailing = false;
		((Control)btn19).Location = new Point(63, 41);
		((Control)btn19).Name = "btn19";
		btn19.PushedAlways = false;
		btn19.SecondBorderColor = Color.Red;
		btn19.SecondBorderDistanceToEdge = 0f;
		btn19.SecondBorderWidth = 1f;
		btn19.ShowFirstBorder = false;
		btn19.ShowSecondBorder = false;
		((Control)btn19).Size = new Size(24, 15);
		btn19.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn19).TabIndex = 60;
		((Control)btn19).TabStop = false;
		((Control)btn19).Text = "19";
		btn19.TrailingForeColor = Color.LightGray;
		((Control)btn19).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn19).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn19).Click += Days_Click;
		((Control)btn19).Leave += BtnX_Leave;
		((Control)btn19).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn19).Enter += BtnX_Enter;
		((Control)btn19).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn19).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn19).MouseEnter += DaysOfMonth_MouseEnter;
		btn28.BoldedForeColor = Color.Blue;
		btn28.HolidayForeColor = Color.Red;
		btn28.ImageFixedSize = new Size(0, 0);
		btn28.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn28.IsBolded = false;
		btn28.IsHoliday = false;
		btn28.IsTrailing = false;
		((Control)btn28).Location = new Point(6, 58);
		((Control)btn28).Name = "btn28";
		btn28.PushedAlways = false;
		btn28.SecondBorderColor = Color.Red;
		btn28.SecondBorderDistanceToEdge = 0f;
		btn28.SecondBorderWidth = 1f;
		btn28.ShowFirstBorder = false;
		btn28.ShowSecondBorder = false;
		((Control)btn28).Size = new Size(24, 15);
		btn28.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn28).TabIndex = 69;
		((Control)btn28).TabStop = false;
		((Control)btn28).Text = "28";
		btn28.TrailingForeColor = Color.LightGray;
		((Control)btn28).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn28).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn28).Click += Days_Click;
		((Control)btn28).Leave += BtnX_Leave;
		((Control)btn28).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn28).Enter += BtnX_Enter;
		((Control)btn28).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn28).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn28).MouseEnter += DaysOfMonth_MouseEnter;
		btn20.BoldedForeColor = Color.Blue;
		btn20.HolidayForeColor = Color.Red;
		btn20.ImageFixedSize = new Size(0, 0);
		btn20.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn20.IsBolded = false;
		btn20.IsHoliday = false;
		btn20.IsTrailing = false;
		((Control)btn20).Location = new Point(33, 41);
		((Control)btn20).Name = "btn20";
		btn20.PushedAlways = false;
		btn20.SecondBorderColor = Color.Red;
		btn20.SecondBorderDistanceToEdge = 0f;
		btn20.SecondBorderWidth = 1f;
		btn20.ShowFirstBorder = false;
		btn20.ShowSecondBorder = false;
		((Control)btn20).Size = new Size(24, 15);
		btn20.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn20).TabIndex = 61;
		((Control)btn20).TabStop = false;
		((Control)btn20).Text = "20";
		btn20.TrailingForeColor = Color.LightGray;
		((Control)btn20).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn20).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn20).Click += Days_Click;
		((Control)btn20).Leave += BtnX_Leave;
		((Control)btn20).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn20).Enter += BtnX_Enter;
		((Control)btn20).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn20).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn20).MouseEnter += DaysOfMonth_MouseEnter;
		btn27.BoldedForeColor = Color.Blue;
		btn27.HolidayForeColor = Color.Red;
		btn27.ImageFixedSize = new Size(0, 0);
		btn27.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn27.IsBolded = false;
		btn27.IsHoliday = false;
		btn27.IsTrailing = false;
		((Control)btn27).Location = new Point(33, 58);
		((Control)btn27).Name = "btn27";
		btn27.PushedAlways = false;
		btn27.SecondBorderColor = Color.Red;
		btn27.SecondBorderDistanceToEdge = 0f;
		btn27.SecondBorderWidth = 1f;
		btn27.ShowFirstBorder = false;
		btn27.ShowSecondBorder = false;
		((Control)btn27).Size = new Size(24, 15);
		btn27.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn27).TabIndex = 68;
		((Control)btn27).TabStop = false;
		((Control)btn27).Text = "27";
		btn27.TrailingForeColor = Color.LightGray;
		((Control)btn27).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn27).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn27).Click += Days_Click;
		((Control)btn27).Leave += BtnX_Leave;
		((Control)btn27).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn27).Enter += BtnX_Enter;
		((Control)btn27).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn27).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn27).MouseEnter += DaysOfMonth_MouseEnter;
		btn21.BoldedForeColor = Color.Blue;
		btn21.HolidayForeColor = Color.Red;
		btn21.ImageFixedSize = new Size(0, 0);
		btn21.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn21.IsBolded = false;
		btn21.IsHoliday = false;
		btn21.IsTrailing = false;
		((Control)btn21).Location = new Point(7, 41);
		((Control)btn21).Name = "btn21";
		btn21.PushedAlways = false;
		btn21.SecondBorderColor = Color.Red;
		btn21.SecondBorderDistanceToEdge = 0f;
		btn21.SecondBorderWidth = 1f;
		btn21.ShowFirstBorder = false;
		btn21.ShowSecondBorder = false;
		((Control)btn21).Size = new Size(24, 15);
		btn21.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn21).TabIndex = 62;
		((Control)btn21).TabStop = false;
		((Control)btn21).Text = "21";
		btn21.TrailingForeColor = Color.LightGray;
		((Control)btn21).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn21).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn21).Click += Days_Click;
		((Control)btn21).Leave += BtnX_Leave;
		((Control)btn21).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn21).Enter += BtnX_Enter;
		((Control)btn21).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn21).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn21).MouseEnter += DaysOfMonth_MouseEnter;
		btn26.BoldedForeColor = Color.Blue;
		btn26.HolidayForeColor = Color.Red;
		btn26.ImageFixedSize = new Size(0, 0);
		btn26.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn26.IsBolded = false;
		btn26.IsHoliday = false;
		btn26.IsTrailing = false;
		((Control)btn26).Location = new Point(63, 58);
		((Control)btn26).Name = "btn26";
		btn26.PushedAlways = false;
		btn26.SecondBorderColor = Color.Red;
		btn26.SecondBorderDistanceToEdge = 0f;
		btn26.SecondBorderWidth = 1f;
		btn26.ShowFirstBorder = false;
		btn26.ShowSecondBorder = false;
		((Control)btn26).Size = new Size(24, 15);
		btn26.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn26).TabIndex = 67;
		((Control)btn26).TabStop = false;
		((Control)btn26).Text = "26";
		btn26.TrailingForeColor = Color.LightGray;
		((Control)btn26).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn26).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn26).Click += Days_Click;
		((Control)btn26).Leave += BtnX_Leave;
		((Control)btn26).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn26).Enter += BtnX_Enter;
		((Control)btn26).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn26).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn26).MouseEnter += DaysOfMonth_MouseEnter;
		btn22.BoldedForeColor = Color.Blue;
		btn22.HolidayForeColor = Color.Red;
		btn22.ImageFixedSize = new Size(0, 0);
		btn22.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn22.IsBolded = false;
		btn22.IsHoliday = false;
		btn22.IsTrailing = false;
		((Control)btn22).Location = new Point(174, 58);
		((Control)btn22).Name = "btn22";
		btn22.PushedAlways = false;
		btn22.SecondBorderColor = Color.Red;
		btn22.SecondBorderDistanceToEdge = 0f;
		btn22.SecondBorderWidth = 1f;
		btn22.ShowFirstBorder = false;
		btn22.ShowSecondBorder = false;
		((Control)btn22).Size = new Size(24, 15);
		btn22.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn22).TabIndex = 63;
		((Control)btn22).TabStop = false;
		((Control)btn22).Text = "22";
		btn22.TrailingForeColor = Color.LightGray;
		((Control)btn22).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn22).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn22).Click += Days_Click;
		((Control)btn22).Leave += BtnX_Leave;
		((Control)btn22).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn22).Enter += BtnX_Enter;
		((Control)btn22).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn22).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn22).MouseEnter += DaysOfMonth_MouseEnter;
		btn25.BoldedForeColor = Color.Blue;
		btn25.HolidayForeColor = Color.Red;
		btn25.ImageFixedSize = new Size(0, 0);
		btn25.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn25.IsBolded = false;
		btn25.IsHoliday = false;
		btn25.IsTrailing = false;
		((Control)btn25).Location = new Point(94, 58);
		((Control)btn25).Name = "btn25";
		btn25.PushedAlways = false;
		btn25.SecondBorderColor = Color.Red;
		btn25.SecondBorderDistanceToEdge = 0f;
		btn25.SecondBorderWidth = 1f;
		btn25.ShowFirstBorder = false;
		btn25.ShowSecondBorder = false;
		((Control)btn25).Size = new Size(24, 15);
		btn25.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn25).TabIndex = 66;
		((Control)btn25).TabStop = false;
		((Control)btn25).Text = "25";
		btn25.TrailingForeColor = Color.LightGray;
		((Control)btn25).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn25).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn25).Click += Days_Click;
		((Control)btn25).Leave += BtnX_Leave;
		((Control)btn25).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn25).Enter += BtnX_Enter;
		((Control)btn25).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn25).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn25).MouseEnter += DaysOfMonth_MouseEnter;
		btn23.BoldedForeColor = Color.Blue;
		btn23.HolidayForeColor = Color.Red;
		btn23.ImageFixedSize = new Size(0, 0);
		btn23.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn23.IsBolded = false;
		btn23.IsHoliday = false;
		btn23.IsTrailing = false;
		((Control)btn23).Location = new Point(149, 58);
		((Control)btn23).Name = "btn23";
		btn23.PushedAlways = false;
		btn23.SecondBorderColor = Color.Red;
		btn23.SecondBorderDistanceToEdge = 0f;
		btn23.SecondBorderWidth = 1f;
		btn23.ShowFirstBorder = false;
		btn23.ShowSecondBorder = false;
		((Control)btn23).Size = new Size(24, 15);
		btn23.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn23).TabIndex = 64;
		((Control)btn23).TabStop = false;
		((Control)btn23).Text = "23";
		btn23.TrailingForeColor = Color.LightGray;
		((Control)btn23).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn23).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn23).Click += Days_Click;
		((Control)btn23).Leave += BtnX_Leave;
		((Control)btn23).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn23).Enter += BtnX_Enter;
		((Control)btn23).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn23).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn23).MouseEnter += DaysOfMonth_MouseEnter;
		btn24.BoldedForeColor = Color.Blue;
		btn24.HolidayForeColor = Color.Red;
		btn24.ImageFixedSize = new Size(0, 0);
		btn24.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btn24.IsBolded = false;
		btn24.IsHoliday = false;
		btn24.IsTrailing = false;
		((Control)btn24).Location = new Point(121, 58);
		((Control)btn24).Name = "btn24";
		btn24.PushedAlways = false;
		btn24.SecondBorderColor = Color.Red;
		btn24.SecondBorderDistanceToEdge = 0f;
		btn24.SecondBorderWidth = 1f;
		btn24.ShowFirstBorder = false;
		btn24.ShowSecondBorder = false;
		((Control)btn24).Size = new Size(24, 15);
		btn24.Style = ButtonX.ButtonStyles.Simple;
		((Control)btn24).TabIndex = 65;
		((Control)btn24).TabStop = false;
		((Control)btn24).Text = "24";
		btn24.TrailingForeColor = Color.LightGray;
		((Control)btn24).MouseLeave += DaysOfMonth_MouseLeave;
		((Control)btn24).MouseMove += new MouseEventHandler(DaysOfMonth_MouseMove);
		((Control)btn24).Click += Days_Click;
		((Control)btn24).Leave += BtnX_Leave;
		((Control)btn24).MouseClick += new MouseEventHandler(DaysOfMonth_MouseClick);
		((Control)btn24).Enter += BtnX_Enter;
		((Control)btn24).MouseHover += DaysOfMonth_MouseHover;
		((Control)btn24).MouseUp += new MouseEventHandler(DaysOfMonth_MouseUp);
		((Control)btn24).MouseEnter += DaysOfMonth_MouseEnter;
		btnDayClicked.BoldedForeColor = Color.Blue;
		btnDayClicked.HolidayForeColor = Color.Red;
		btnDayClicked.ImageFixedSize = new Size(0, 0);
		btnDayClicked.ImageSizeMode = ButtonX.ImageSizeModes.Normal;
		btnDayClicked.IsBolded = false;
		btnDayClicked.IsHoliday = false;
		btnDayClicked.IsTrailing = false;
		((Control)btnDayClicked).Location = new Point(79, 55);
		((Control)btnDayClicked).Name = "btnDayClicked";
		btnDayClicked.PushedAlways = false;
		btnDayClicked.SecondBorderColor = Color.Red;
		btnDayClicked.SecondBorderDistanceToEdge = 0f;
		btnDayClicked.SecondBorderWidth = 1f;
		btnDayClicked.ShowFirstBorder = false;
		btnDayClicked.ShowSecondBorder = false;
		((Control)btnDayClicked).Size = new Size(24, 15);
		btnDayClicked.Style = ButtonX.ButtonStyles.Simple;
		((Control)btnDayClicked).TabIndex = 84;
		((Control)btnDayClicked).TabStop = false;
		btnDayClicked.TrailingForeColor = Color.LightGray;
		((Control)btnDayClicked).Visible = false;
		((Control)lblLineWeek).BackColor = Color.Black;
		((Control)lblLineWeek).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblLineWeek).Location = new Point(0, 50);
		((Control)lblLineWeek).Name = "lblLineWeek";
		((Control)lblLineWeek).Size = new Size(200, 4);
		((Control)lblLineWeek).TabIndex = 21;
		lblLineWeek.TextAlign = (ContentAlignment)32;
		lblLineWeek.UseCompatibleTextRendering = true;
		((Control)lblLineWeek).Click += AllControls_Click;
		((Control)cmsMonthNames).Name = "cmsMonthNames";
		((ToolStripDropDownMenu)cmsMonthNames).ShowImageMargin = false;
		((Control)cmsMonthNames).Size = new Size(36, 4);
		((ToolStrip)cmsMonthNames).ItemClicked += new ToolStripItemClickedEventHandler(cmsMonthNames_ItemClicked);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)lblLineWeek);
		((Control)this).Controls.Add((Control)(object)pnlDays);
		((Control)this).Controls.Add((Control)(object)pnlToday);
		((Control)this).Controls.Add((Control)(object)pnlWeekDays);
		((Control)this).Controls.Add((Control)(object)pnlTitles);
		((Control)this).Name = "MonthCalendarX";
		((Control)this).Size = new Size(200, 200);
		((Control)pnlTitles).ResumeLayout(false);
		((ISupportInitialize)nudYear).EndInit();
		((Control)pnlWeekDays).ResumeLayout(false);
		((Control)pnlToday).ResumeLayout(false);
		((Control)pnlDays).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void SetTextForComponents()
	{
		if (_CalendarType == CalendarTypes.Persian)
		{
			setTextForContextMenu_Persian();
			setTextForTitles_Persian();
			setTextForWeekDays_Persian();
			setTextForDays_Persian();
			setTextForToday_Persian();
		}
		else
		{
			setTextForContextMenu_English();
			setTextForTitles_English();
			setTextForWeekDays_English();
			setTextForDays_English();
			setTextForToday_English();
		}
	}

	private void setTextForContextMenu_Persian()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (((ArrangedElementCollection)((ToolStrip)cmsMonthNames).Items).Count != 12)
		{
			((ToolStrip)cmsMonthNames).Items.Clear();
			for (int i = 0; i < 12; i++)
			{
				((ToolStrip)cmsMonthNames).Items.Add("");
			}
		}
		try
		{
			((Control)cmsMonthNames).Font = new Font("Tahoma", ((Control)this).Font.Size);
		}
		catch
		{
		}
		((Control)cmsMonthNames).RightToLeft = (RightToLeft)1;
		((ToolStrip)cmsMonthNames).Items[0].Text = "فروردین";
		((ToolStrip)cmsMonthNames).Items[1].Text = "اردیبهشت";
		((ToolStrip)cmsMonthNames).Items[2].Text = "خرداد";
		((ToolStrip)cmsMonthNames).Items[3].Text = "تیر";
		((ToolStrip)cmsMonthNames).Items[4].Text = "مرداد";
		((ToolStrip)cmsMonthNames).Items[5].Text = "شهریور";
		((ToolStrip)cmsMonthNames).Items[6].Text = "مهر";
		((ToolStrip)cmsMonthNames).Items[7].Text = "آبان";
		((ToolStrip)cmsMonthNames).Items[8].Text = "آذر";
		((ToolStrip)cmsMonthNames).Items[9].Text = "دی";
		((ToolStrip)cmsMonthNames).Items[10].Text = "بهمن";
		((ToolStrip)cmsMonthNames).Items[11].Text = "اسفند";
	}

	private void setTextForContextMenu_English()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		if (((ArrangedElementCollection)((ToolStrip)cmsMonthNames).Items).Count != 12)
		{
			((ToolStrip)cmsMonthNames).Items.Clear();
			for (int i = 0; i < 12; i++)
			{
				((ToolStrip)cmsMonthNames).Items.Add("");
			}
		}
		try
		{
			((Control)cmsMonthNames).Font = new Font(FontFamily.GenericSansSerif, ((Control)this).Font.Size);
		}
		catch
		{
		}
		((Control)cmsMonthNames).RightToLeft = (RightToLeft)0;
		((ToolStrip)cmsMonthNames).Items[0].Text = "January";
		((ToolStrip)cmsMonthNames).Items[1].Text = "February";
		((ToolStrip)cmsMonthNames).Items[2].Text = "March";
		((ToolStrip)cmsMonthNames).Items[3].Text = "April";
		((ToolStrip)cmsMonthNames).Items[4].Text = "May";
		((ToolStrip)cmsMonthNames).Items[5].Text = "June";
		((ToolStrip)cmsMonthNames).Items[6].Text = "July";
		((ToolStrip)cmsMonthNames).Items[7].Text = "August";
		((ToolStrip)cmsMonthNames).Items[8].Text = "September";
		((ToolStrip)cmsMonthNames).Items[9].Text = "October";
		((ToolStrip)cmsMonthNames).Items[10].Text = "November";
		((ToolStrip)cmsMonthNames).Items[11].Text = "December";
	}

	private void setTextForTitles_Persian()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		string text;
		string text2;
		if ((int)((Control)this).RightToLeft == 1)
		{
			text = ">";
			text2 = "<";
		}
		else
		{
			text = "<";
			text2 = ">";
		}
		PersianDateTime persianDateTime = new PersianDateTime(_SelectedDate);
		string text3 = clsCommonFunctions.MonthOfYear(_CalendarType, persianDateTime.Month);
		nudYear.Value = persianDateTime.Year;
		string text4 = persianDateTime.Year.ToString();
		((Control)btnNextMonth).Text = text;
		((Control)btnYear).Text = text4;
		((Control)btnMonth).Text = text3;
		((Control)btnPreMonth).Text = text2;
	}

	private void setTextForTitles_English()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		string text;
		string text2;
		if ((int)((Control)this).RightToLeft == 1)
		{
			text = "<";
			text2 = ">";
		}
		else
		{
			text = ">";
			text2 = "<";
		}
		DateTime selectedDate = _SelectedDate;
		string text3 = clsCommonFunctions.MonthOfYear(_CalendarType, selectedDate.Month);
		nudYear.Value = selectedDate.Year;
		string text4 = selectedDate.Year.ToString();
		((Control)btnNextMonth).Text = text;
		((Control)btnYear).Text = text4;
		((Control)btnMonth).Text = text3;
		((Control)btnPreMonth).Text = text2;
	}

	private void setTextForWeekDays_Persian()
	{
		string[] array = clsCommonFunctions.NameOfWeekDays(CalendarTypes.Persian, ((Control)lblSat).Width, ((Control)this).Font);
		((Control)lblSat).Text = array[0];
		((Control)lblSun).Text = array[1];
		((Control)lblMon).Text = array[2];
		((Control)lblTue).Text = array[3];
		((Control)lblWed).Text = array[4];
		((Control)lblThu).Text = array[5];
		((Control)lblFri).Text = array[6];
	}

	private void setTextForWeekDays_English()
	{
		string[] array = clsCommonFunctions.NameOfWeekDays(CalendarTypes.English, ((Control)lblSat).Width, ((Control)this).Font);
		((Control)lblSat).Text = array[0];
		((Control)lblSun).Text = array[1];
		((Control)lblMon).Text = array[2];
		((Control)lblTue).Text = array[3];
		((Control)lblWed).Text = array[4];
		((Control)lblThu).Text = array[5];
		((Control)lblFri).Text = array[6];
	}

	private void setTextForDays_Persian()
	{
		PersianDateTime selectedDate = new PersianDateTime(_SelectedDate);
		int weekStartsOn = (int)_WeekStartsOn;
		int startIndexDayOfWeek = clsCommonFunctions.GetStartIndexDayOfWeek(CalendarTypes.Persian, _SelectedDate);
		int daysInMonth = PersianDateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
		PersianDateTime prevMonth = selectedDate.AddMonths(-1);
		int daysInMonthPrev = PersianDateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
		startIndexDayOfWeek -= weekStartsOn;
		if (startIndexDayOfWeek < 0)
		{
			startIndexDayOfWeek += 7;
		}
		int days = -startIndexDayOfWeek + 1;
		_setTextForDay_Persian(btn1, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn2, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn3, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn4, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn5, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn6, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn7, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn8, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn9, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn10, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn11, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn12, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn13, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn14, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn15, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn16, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn17, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn18, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn19, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn20, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn21, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn22, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn23, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn24, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn25, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn26, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn27, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn28, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn29, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn30, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn31, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn32, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn33, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn34, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn35, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn36, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn37, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn38, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn39, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn40, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn41, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_Persian(btn42, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
	}

	private void _setTextForDay_Persian(ButtonX btn, ref int days, int daysInMonthPrev, int daysInMonth, PersianDateTime selectedDate, PersianDateTime prevMonth)
	{
		if (days <= 0 || days > daysInMonth)
		{
			btn.IsTrailing = true;
			btn.IsHoliday = false;
			btn.ShowSecondBorder = false;
			btn.PushedAlways = false;
			int day = ((days > 0) ? (days - daysInMonth) : (daysInMonthPrev + days));
			PersianDateTime date = new PersianDateTime(prevMonth.Year, prevMonth.Month, day);
			if (isBoldedDay(date))
			{
				btn.IsBolded = true;
			}
			else
			{
				btn.IsBolded = false;
			}
			((Control)btn).Text = day.ToString();
			((Control)btn).Visible = _ShowTrailing;
		}
		else if (days >= 1 && days <= daysInMonth)
		{
			((Control)btn).Visible = true;
			btn.IsTrailing = false;
			((Control)btn).Text = days.ToString();
			PersianDateTime date2 = new PersianDateTime(selectedDate.Year, selectedDate.Month, days);
			if (isBoldedDay(date2))
			{
				btn.IsBolded = true;
			}
			else
			{
				btn.IsBolded = false;
			}
			if (selectedDate.Day == days)
			{
				btn.PushedAlways = true;
			}
			else
			{
				btn.PushedAlways = false;
			}
			if (_ShowTodayRect)
			{
				if (isToday(CalendarTypes.Persian, selectedDate.Year, selectedDate.Month, days))
				{
					btn.ShowSecondBorder = true;
				}
				else
				{
					btn.ShowSecondBorder = false;
				}
			}
			else
			{
				btn.ShowSecondBorder = false;
			}
			if (isHoliday(CalendarTypes.Persian, selectedDate.Year, selectedDate.Month, days))
			{
				btn.IsHoliday = true;
			}
			else
			{
				btn.IsHoliday = false;
			}
		}
		days++;
	}

	private void setTextForDays_English()
	{
		DateTime selectedDate = _SelectedDate;
		int weekStartsOn = (int)_WeekStartsOn;
		int startIndexDayOfWeek = clsCommonFunctions.GetStartIndexDayOfWeek(CalendarTypes.English, selectedDate);
		int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
		DateTime prevMonth = selectedDate.AddMonths(-1);
		int daysInMonthPrev = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
		startIndexDayOfWeek -= weekStartsOn;
		if (startIndexDayOfWeek < 0)
		{
			startIndexDayOfWeek += 7;
		}
		int days = -startIndexDayOfWeek + 1;
		_setTextForDay_English(btn1, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn2, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn3, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn4, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn5, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn6, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn7, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn8, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn9, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn10, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn11, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn12, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn13, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn14, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn15, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn16, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn17, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn18, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn19, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn20, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn21, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn22, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn23, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn24, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn25, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn26, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn27, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn28, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn29, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn30, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn31, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn32, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn33, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn34, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn35, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn36, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn37, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn38, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn39, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn40, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn41, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
		_setTextForDay_English(btn42, ref days, daysInMonthPrev, daysInMonth, selectedDate, prevMonth);
	}

	private void _setTextForDay_English(ButtonX btn, ref int days, int daysInMonthPrev, int daysInMonth, DateTime selectedDate, DateTime prevMonth)
	{
		if (days <= 0 || days > daysInMonth)
		{
			btn.IsTrailing = true;
			btn.IsHoliday = false;
			btn.ShowSecondBorder = false;
			btn.PushedAlways = false;
			int day = ((days > 0) ? (days - daysInMonth) : (daysInMonthPrev + days));
			DateTime date = new DateTime(prevMonth.Year, prevMonth.Month, day);
			if (isBoldedDay(date))
			{
				btn.IsBolded = true;
			}
			else
			{
				btn.IsBolded = false;
			}
			((Control)btn).Text = day.ToString();
			((Control)btn).Visible = _ShowTrailing;
		}
		else if (days >= 1 && days <= daysInMonth)
		{
			((Control)btn).Visible = true;
			btn.IsTrailing = false;
			((Control)btn).Text = days.ToString();
			DateTime date2 = new DateTime(selectedDate.Year, selectedDate.Month, days);
			if (isBoldedDay(date2))
			{
				btn.IsBolded = true;
			}
			else
			{
				btn.IsBolded = false;
			}
			if (selectedDate.Day == days)
			{
				btn.PushedAlways = true;
			}
			else
			{
				btn.PushedAlways = false;
			}
			if (_ShowTodayRect)
			{
				if (isToday(CalendarTypes.English, selectedDate.Year, selectedDate.Month, days))
				{
					btn.ShowSecondBorder = true;
				}
				else
				{
					btn.ShowSecondBorder = false;
				}
			}
			else
			{
				btn.ShowSecondBorder = false;
			}
			if (isHoliday(CalendarTypes.English, selectedDate.Year, selectedDate.Month, days))
			{
				btn.IsHoliday = true;
			}
			else
			{
				btn.IsHoliday = false;
			}
		}
		days++;
	}

	private bool isHoliday(CalendarTypes type, int year, int month, int day)
	{
		bool flag = false;
		DayOfWeek dayOfWeek;
		if (type == CalendarTypes.Persian)
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			dayOfWeek = persianDateTime.DayOfWeek;
			foreach (PersianDateTime item in _HolidayDates_Persian)
			{
				if (item.Year == persianDateTime.Year && item.Month == persianDateTime.Month && item.Day == persianDateTime.Day)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			DateTime dateTime = new DateTime(year, month, day);
			dayOfWeek = dateTime.DayOfWeek;
			foreach (DateTime item2 in _HolidayDates_English)
			{
				if (item2.Year == dateTime.Year && item2.Month == dateTime.Month && item2.Day == dateTime.Day)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag && _HolidayWeekly != 0)
		{
			switch (dayOfWeek)
			{
			case DayOfWeek.Saturday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Saturday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Sunday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Sunday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Monday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Monday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Tuesday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Tuesday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Wednesday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Wednesday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Thursday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Thursday)
				{
					flag = true;
				}
				break;
			case DayOfWeek.Friday:
				if (_HolidayWeekly == DayOfWeekForHoliday.Friday)
				{
					flag = true;
				}
				break;
			}
		}
		return flag;
	}

	private bool isToday(CalendarTypes type, int year, int month, int day)
	{
		DateTime dateTime;
		if (type == CalendarTypes.Persian)
		{
			PersianDateTime date = new PersianDateTime(year, month, day);
			dateTime = PersianDateTime.ToDateTime(date);
		}
		else
		{
			dateTime = new DateTime(year, month, day);
		}
		if (dateTime.Year == _TodayDate.Year && dateTime.Month == _TodayDate.Month)
		{
			return dateTime.Day == _TodayDate.Day;
		}
		return false;
	}

	private bool isBoldedDay(PersianDateTime date)
	{
		PersianDateTime.ToDateTime(date);
		return _isBoldedDay(CalendarTypes.Persian, date.Year, date.Month, date.Day);
	}

	private bool isBoldedDay(DateTime date)
	{
		return _isBoldedDay(CalendarTypes.English, date.Year, date.Month, date.Day);
	}

	private bool _isBoldedDay(CalendarTypes type, int year, int month, int day)
	{
		bool flag = false;
		if (type == CalendarTypes.Persian)
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			foreach (PersianDateTime item in _BoldedDates_Persian)
			{
				if (item.Year == persianDateTime.Year && item.Month == persianDateTime.Month && item.Day == persianDateTime.Day)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
			foreach (PersianDateTime item2 in _AnnuallyBoldedDates_Persian)
			{
				if (item2.Month == persianDateTime.Month && item2.Day == persianDateTime.Day)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
			foreach (PersianDateTime item3 in _MonthlyBoldedDates_Persian)
			{
				if (item3.Day == persianDateTime.Day)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			DateTime dateTime = new DateTime(year, month, day);
			foreach (DateTime item4 in _BoldedDates_English)
			{
				if (item4.Year == dateTime.Year && item4.Month == dateTime.Month && item4.Day == dateTime.Day)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
			foreach (DateTime item5 in _AnnuallyBoldedDates_English)
			{
				if (item5.Month == dateTime.Month && item5.Day == dateTime.Day)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
			foreach (DateTime item6 in _MonthlyBoldedDates_English)
			{
				if (item6.Day == dateTime.Day)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	private void setTextForToday_Persian()
	{
		PersianDateTime persianDateTime = new PersianDateTime(_TodayDate);
		string text = "امروز : ";
		text += persianDateTime.ToShortDateString();
		((Control)lblTodayText).Text = text;
		lblTodayText.TextAlign = (ContentAlignment)64;
	}

	private void setTextForToday_English()
	{
		DateTime todayDate = _TodayDate;
		string text = "Today : ";
		text += todayDate.ToShortDateString();
		((Control)lblTodayText).Text = text;
		lblTodayText.TextAlign = (ContentAlignment)16;
	}

	private void LoadMonthCalendarX()
	{
		ArrangeComponents();
		SetTextForComponents();
		ChangeStyle_ToolTips();
		RenderFocused();
		_bFormLoaded = true;
	}

	private void ChangeSelectedDate(PersianDateTime date)
	{
		ChangeSelectedDate(CalendarTypes.Persian, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
	}

	private void ChangeSelectedDate(DateTime date)
	{
		ChangeSelectedDate(CalendarTypes.English, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
	}

	private void ChangeSelectedDate(CalendarTypes type, int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		bool flag = false;
		DateTime dateTime;
		if (type == CalendarTypes.Persian)
		{
			int num = PersianDateTime.DaysInMonth(year, month);
			if (day < 1)
			{
				day = 1;
			}
			if (day > num)
			{
				day = num;
			}
			dateTime = PersianDateTime.ToDateTime(year, month, day, hour, minute, second, millisecond);
		}
		else
		{
			int num2 = DateTime.DaysInMonth(year, month);
			if (day < 1)
			{
				day = 1;
			}
			if (day > num2)
			{
				day = num2;
			}
			dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
		}
		if (dateTime.Year != _SelectedDate.Year || dateTime.Month != _SelectedDate.Month || dateTime.Day != _SelectedDate.Day)
		{
			flag = true;
			_SelectedDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
		}
		if (flag)
		{
			SetTextForComponents();
			SelectedDateTimeChanged();
		}
	}

	private void SelectedDateTimeChanged()
	{
		if (this.SelectedDateChanged != null)
		{
			PersianDateTime selectedPersianDateTime = new PersianDateTime(_SelectedDate);
			this.SelectedDateChanged(_SelectedDate, selectedPersianDateTime);
		}
	}

	private void ChangeTodayDate(PersianDateTime date)
	{
		ChangeTodayDate(CalendarTypes.Persian, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
	}

	private void ChangeTodayDate(DateTime date)
	{
		ChangeTodayDate(CalendarTypes.English, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
	}

	private void ChangeTodayDate(CalendarTypes type, int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		bool flag = false;
		DateTime dateTime;
		if (type == CalendarTypes.Persian)
		{
			int num = PersianDateTime.DaysInMonth(year, month);
			if (day < 1)
			{
				day = 1;
			}
			if (day > num)
			{
				day = num;
			}
			dateTime = PersianDateTime.ToDateTime(year, month, day, hour, minute, second, millisecond);
		}
		else
		{
			int num2 = DateTime.DaysInMonth(year, month);
			if (day < 1)
			{
				day = 1;
			}
			if (day > num2)
			{
				day = num2;
			}
			dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
		}
		if (dateTime.Year != _TodayDate.Year || dateTime.Month != _TodayDate.Month || dateTime.Day != _TodayDate.Day)
		{
			flag = true;
			_TodayDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
		}
		if (flag)
		{
			SetTextForComponents();
			TodayDateTimeChanged();
		}
	}

	private void TodayDateTimeChanged()
	{
		if (this.TodayDateChanged != null)
		{
			PersianDateTime todayPersianDateTime = new PersianDateTime(_TodayDate);
			this.TodayDateChanged(_TodayDate, todayPersianDateTime);
		}
	}

	private void cmsMonthNames_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		ToolStripItem clickedItem = e.ClickedItem;
		int num = ((ToolStrip)cmsMonthNames).Items.IndexOf(clickedItem);
		if (num != -1)
		{
			if (_CalendarType == CalendarTypes.Persian)
			{
				PersianDateTime persianDateTime = new PersianDateTime(_SelectedDate);
				int month = num + 1;
				ChangeSelectedDate(CalendarTypes.Persian, persianDateTime.Year, month, persianDateTime.Day, persianDateTime.Hour, persianDateTime.Minute, persianDateTime.Second, persianDateTime.Millisecond);
			}
			else if (_CalendarType == CalendarTypes.English)
			{
				DateTime selectedDate = _SelectedDate;
				int month2 = num + 1;
				ChangeSelectedDate(CalendarTypes.English, selectedDate.Year, month2, selectedDate.Day, selectedDate.Hour, selectedDate.Minute, selectedDate.Second, selectedDate.Millisecond);
			}
		}
	}

	private void RenderFocused()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		float num = 0f;
		float num2 = 0f;
		float num3 = ((Control)this).Width - _iComponentMargin;
		float num4 = ((Control)this).Height - _iComponentMargin;
		Graphics val = ((Control)this).CreateGraphics();
		if (_bTxtTempFocused || _bNudYearFocused || _bBtnXFocused)
		{
			Pen val2 = new Pen(Color.FromArgb(225, 227, 120), 2f);
			val.DrawRectangle(val2, num, num2, num3, num4);
			float[] dashPattern = new float[2] { 1f, 1f };
			Pen val3 = new Pen(Brushes.Black, (float)_iComponentMargin);
			val3.DashPattern = dashPattern;
			val.DrawRectangle(val3, num, num2, num3, num4);
		}
		else
		{
			Pen val2 = new Pen(_BorderColor, (float)_iComponentMargin);
			val.DrawRectangle(val2, num, num2, num3, num4);
		}
	}

	private void txtTempForKeys_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 38)
		{
			DateTime selectedDate = _SelectedDate;
			selectedDate = selectedDate.AddDays(-7.0);
			ChangeSelectedDate(selectedDate);
		}
		else if ((int)e.KeyCode == 40)
		{
			DateTime selectedDate2 = _SelectedDate;
			selectedDate2 = selectedDate2.AddDays(7.0);
			ChangeSelectedDate(selectedDate2);
		}
		else if ((int)e.KeyCode == 37)
		{
			if (_CalendarType == CalendarTypes.Persian)
			{
				DateTime selectedDate3 = _SelectedDate;
				selectedDate3 = selectedDate3.AddDays(1.0);
				ChangeSelectedDate(selectedDate3);
			}
			else if (_CalendarType == CalendarTypes.English)
			{
				DateTime selectedDate4 = _SelectedDate;
				selectedDate4 = selectedDate4.AddDays(-1.0);
				ChangeSelectedDate(selectedDate4);
			}
		}
		else if ((int)e.KeyCode == 39)
		{
			if (_CalendarType == CalendarTypes.Persian)
			{
				DateTime selectedDate5 = _SelectedDate;
				selectedDate5 = selectedDate5.AddDays(-1.0);
				ChangeSelectedDate(selectedDate5);
			}
			else if (_CalendarType == CalendarTypes.English)
			{
				DateTime selectedDate6 = _SelectedDate;
				selectedDate6 = selectedDate6.AddDays(1.0);
				ChangeSelectedDate(selectedDate6);
			}
		}
		if (this.DayKeyDown != null)
		{
			this.DayKeyDown(e);
		}
	}

	private void txtTempForKeys_KeyUp(object sender, KeyEventArgs e)
	{
		if (this.DayKeyUp != null)
		{
			this.DayKeyUp(e);
		}
	}

	private void txtTempForKeys_GotFocus(object sender, EventArgs e)
	{
		_bTxtTempFocused = true;
		RenderFocused();
	}

	private void txtTempForKeys_LostFocus(object sender, EventArgs e)
	{
		_bTxtTempFocused = false;
		RenderFocused();
	}

	private void Labels_Click(object sender, EventArgs e)
	{
		HideNudYear();
	}

	private void HideNudYear()
	{
		((Control)nudYear).Enabled = false;
		((Control)nudYear).Visible = false;
		((Control)btnYear).Visible = true;
		((Control)txtTemp).Focus();
	}

	private void AllControlsClick()
	{
		if (((Control)nudYear).Visible)
		{
			HideNudYear();
		}
		((Control)txtTemp).Text = "";
		((Control)txtTemp).Focus();
	}

	private void _CalendarTypeChanged()
	{
		if (this.CalendarTypeChanged != null)
		{
			this.CalendarTypeChanged(_CalendarType);
		}
	}

	public void SetTodayDate(CalendarTypes type, int year, int month, int day)
	{
		DateTime date = default(DateTime);
		switch (type)
		{
		case CalendarTypes.Persian:
			date = PersianDateTime.ToDateTime(year, month, day, 0, 0, 0, 0);
			break;
		case CalendarTypes.English:
			date = new DateTime(year, month, day);
			break;
		}
		ChangeTodayDate(date);
	}

	public void SetTodayDate(CalendarTypes type, int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		DateTime date = default(DateTime);
		switch (type)
		{
		case CalendarTypes.Persian:
			date = PersianDateTime.ToDateTime(year, month, day, hour, minute, second, millisecond);
			break;
		case CalendarTypes.English:
			date = new DateTime(year, month, day);
			break;
		}
		ChangeTodayDate(date);
	}

	public void SetTodayDate(PersianDateTime date)
	{
		DateTime date2 = PersianDateTime.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
		ChangeTodayDate(date2);
	}

	public void SetTodayDate(DateTime date)
	{
		ChangeTodayDate(date);
	}

	public DateTime GetTodayDateInDateTime()
	{
		return _TodayDate;
	}

	public PersianDateTime GetTodayDateInPersianDateTime()
	{
		return new PersianDateTime(_TodayDate);
	}

	public void SetSelectedDate(CalendarTypes type, int year, int month, int day)
	{
		DateTime date = default(DateTime);
		switch (type)
		{
		case CalendarTypes.Persian:
			date = PersianDateTime.ToDateTime(year, month, day, 0, 0, 0, 0);
			break;
		case CalendarTypes.English:
			date = new DateTime(year, month, day);
			break;
		}
		ChangeSelectedDate(date);
	}

	public void SetSelectedDate(CalendarTypes type, int year, int month, int day, int hour, int minute, int second, int millisecond)
	{
		DateTime date = default(DateTime);
		switch (type)
		{
		case CalendarTypes.Persian:
			date = PersianDateTime.ToDateTime(year, month, day, hour, minute, second, millisecond);
			break;
		case CalendarTypes.English:
			date = new DateTime(year, month, day);
			break;
		}
		ChangeSelectedDate(date);
	}

	public void SetSelectedDate(PersianDateTime date)
	{
		DateTime dateTime = default(DateTime);
		dateTime = PersianDateTime.ToDateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond);
		ChangeSelectedDate(dateTime);
	}

	public void SetSelectedDate(DateTime date)
	{
		ChangeSelectedDate(date);
	}

	public DateTime GetSelectedDateInDateTime()
	{
		return _SelectedDate;
	}

	public PersianDateTime GetSelectedDateInPersianDateTime()
	{
		return new PersianDateTime(_SelectedDate);
	}

	public MonthCalendarX()
	{
		InitializeComponent();
		InitializeVariables();
	}

	public MonthCalendarX(MonthCalendarX monthCalendarX)
	{
		InitializeComponent();
		InitializeVariables();
		InitializeNewVariables(ref monthCalendarX);
		LoadMonthCalendarX();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (_bFormLoaded)
		{
			RenderFocused();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((UserControl)this).OnLoad(e);
		_TodayDate = DateTime.Now;
		HideNudYear();
		LoadMonthCalendarX();
		ChangeStyle_Titles();
		ChangeStyle_WeekDays();
		ChangeStyle_Days();
		ChangeStyle_BoldedDay();
		ChangeStyle_Holiday();
		ChangeStyle_TrailingDay();
		ChangeStyle_Today();
		ChangeStyle_TodayRect();
		ChangeStyle_ToolTips();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		if (_bFormLoaded)
		{
			LoadMonthCalendarX();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((ContainerControl)this).OnFontChanged(e);
		if (_bFormLoaded)
		{
			LoadMonthCalendarX();
		}
	}

	private void AllControls_Click(object sender, EventArgs e)
	{
		AllControlsClick();
	}

	private void btnPreMonth_MouseDown(object sender, MouseEventArgs e)
	{
		PreviousMonth();
	}

	private void PreviousMonth()
	{
		DateTime date = default(DateTime);
		if (_CalendarType == CalendarTypes.Persian)
		{
			PersianDateTime date2 = new PersianDateTime(_SelectedDate).AddMonths(-1);
			date = PersianDateTime.ToDateTime(date2);
		}
		else if (_CalendarType == CalendarTypes.English)
		{
			date = _SelectedDate.AddMonths(-1);
		}
		ChangeSelectedDate(date);
	}

	private void btnNextMonth_MouseDown(object sender, MouseEventArgs e)
	{
		NextMonth();
	}

	private void NextMonth()
	{
		DateTime date = default(DateTime);
		if (_CalendarType == CalendarTypes.Persian)
		{
			PersianDateTime date2 = new PersianDateTime(_SelectedDate).AddMonths(1);
			date = PersianDateTime.ToDateTime(date2);
		}
		else if (_CalendarType == CalendarTypes.English)
		{
			date = _SelectedDate.AddMonths(1);
		}
		ChangeSelectedDate(date);
	}

	private void btnMonth_MouseClick(object sender, MouseEventArgs e)
	{
		AllControlsClick();
		Point point = ((Control)this).PointToScreen(new Point(e.X, e.Y));
		int x = point.X;
		int y = point.Y;
		((ToolStripDropDown)cmsMonthNames).Show(x, y);
	}

	private void btnYear_Click(object sender, EventArgs e)
	{
		AllControlsClick();
		ShowNumericYear();
	}

	private void ShowNumericYear()
	{
		int x = ((Control)btnYear).Location.X;
		int y = ((Control)btnYear).Location.Y;
		((Control)nudYear).Location = new Point(x, y);
		((Control)nudYear).Width = ((Control)btnYear).Width;
		int result = 0;
		int.TryParse(((Control)btnYear).Text, out result);
		nudYear.Value = result;
		((Control)btnYear).Visible = false;
		((Control)nudYear).Visible = true;
		((Control)nudYear).Enabled = true;
		((Control)nudYear).Focus();
		((UpDownBase)nudYear).Select(0, nudYear.Value.ToString().Length);
	}

	private void Days_Click(object sender, EventArgs e)
	{
		AllControlsClick();
		SelectDayOnClickDays(ref sender);
		((Control)txtTemp).Focus();
	}

	private void SelectDayOnClickDays(ref object sender)
	{
		if (sender == null || !(sender is ButtonX))
		{
			return;
		}
		ButtonX buttonX = sender as ButtonX;
		((Control)btnDayClicked).Location = ((Control)buttonX).Location;
		((Control)btnDayClicked).Text = ((Control)buttonX).Text;
		btnDayClicked.IsTrailing = buttonX.IsTrailing;
		btnDayClicked.IsBolded = buttonX.IsBolded;
		btnDayClicked.IsHoliday = buttonX.IsHoliday;
		((Control)btnDayClicked).Visible = false;
		int result = 0;
		int.TryParse(((Control)buttonX).Text, out result);
		DateTime date;
		if (buttonX.IsTrailing)
		{
			if (_CalendarType == CalendarTypes.Persian)
			{
				if (result < 15)
				{
					PersianDateTime persianDateTime = new PersianDateTime(_SelectedDate).AddMonths(1);
					persianDateTime = new PersianDateTime(persianDateTime.Year, persianDateTime.Month, result, persianDateTime.Hour, persianDateTime.Minute, persianDateTime.Second, persianDateTime.Millisecond);
					date = PersianDateTime.ToDateTime(persianDateTime);
				}
				else
				{
					PersianDateTime persianDateTime2 = new PersianDateTime(_SelectedDate).AddMonths(-1);
					persianDateTime2 = new PersianDateTime(persianDateTime2.Year, persianDateTime2.Month, result, persianDateTime2.Hour, persianDateTime2.Minute, persianDateTime2.Second, persianDateTime2.Millisecond);
					date = PersianDateTime.ToDateTime(persianDateTime2);
				}
			}
			else if (result < 15)
			{
				date = _SelectedDate;
				date = date.AddMonths(1);
				date = new DateTime(date.Year, date.Month, result, date.Hour, date.Minute, date.Second, date.Millisecond);
			}
			else
			{
				date = _SelectedDate;
				date = date.AddMonths(-1);
				date = new DateTime(date.Year, date.Month, result, date.Hour, date.Minute, date.Second, date.Millisecond);
			}
		}
		else if (_CalendarType == CalendarTypes.Persian)
		{
			PersianDateTime persianDateTime3 = new PersianDateTime(_SelectedDate);
			persianDateTime3 = new PersianDateTime(persianDateTime3.Year, persianDateTime3.Month, result, persianDateTime3.Hour, persianDateTime3.Minute, persianDateTime3.Second, persianDateTime3.Millisecond);
			date = PersianDateTime.ToDateTime(persianDateTime3);
		}
		else
		{
			date = _SelectedDate;
			date = new DateTime(date.Year, date.Month, result, date.Hour, date.Minute, date.Second, date.Millisecond);
		}
		ChangeSelectedDate(date);
	}

	private void btnTodayGoto_Click(object sender, EventArgs e)
	{
		AllControlsClick();
		ChangeSelectedDate(CalendarTypes.English, _TodayDate.Year, _TodayDate.Month, _TodayDate.Day, _SelectedDate.Hour, _SelectedDate.Minute, _SelectedDate.Second, _SelectedDate.Millisecond);
		((Control)txtTemp).Focus();
	}

	private void nudYear_ValueChanged(object sender, EventArgs e)
	{
		if (((Control)nudYear).Visible)
		{
			int month;
			int day;
			if (_CalendarType == CalendarTypes.Persian)
			{
				PersianDateTime persianDateTime = new PersianDateTime(_SelectedDate);
				month = persianDateTime.Month;
				day = persianDateTime.Day;
			}
			else
			{
				month = _SelectedDate.Month;
				day = _SelectedDate.Day;
			}
			ChangeSelectedDate(_CalendarType, (int)nudYear.Value, month, day, _SelectedDate.Hour, _SelectedDate.Minute, _SelectedDate.Second, _SelectedDate.Millisecond);
		}
	}

	private void nudYear_Enter(object sender, EventArgs e)
	{
		_bNudYearFocused = true;
		RenderFocused();
	}

	private void nudYear_Leave(object sender, EventArgs e)
	{
		HideNudYear();
		_bNudYearFocused = false;
		RenderFocused();
	}

	private void BtnX_Enter(object sender, EventArgs e)
	{
		_bBtnXFocused = true;
		RenderFocused();
	}

	private void BtnX_Leave(object sender, EventArgs e)
	{
		_bBtnXFocused = false;
		RenderFocused();
	}

	private DayMouseEventArgs GetDaysMouseEventArgs(object sender, MouseEventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		DayMouseEventArgs dayMouseEventArgs = new DayMouseEventArgs();
		if (!(sender is ButtonX))
		{
			return dayMouseEventArgs;
		}
		ButtonX buttonX = sender as ButtonX;
		Point point = ((Control)this).PointToScreen(new Point(0, 0));
		Point point2 = ((Control)buttonX).PointToScreen(new Point(0, 0));
		int num = point2.X - point.X;
		int num2 = point2.Y - point.Y;
		dayMouseEventArgs.Button = e.Button;
		dayMouseEventArgs.Clicks = e.Clicks;
		dayMouseEventArgs.Delta = e.Delta;
		dayMouseEventArgs.Location = new Point(num, num2);
		dayMouseEventArgs.X_InCalendar = num + e.X;
		dayMouseEventArgs.Y_InCalendar = num2 + e.Y;
		dayMouseEventArgs.X_InCurrentDay = e.X;
		dayMouseEventArgs.Y_InCurrentDay = e.Y;
		int result = 0;
		int.TryParse(((Control)buttonX).Text, out result);
		if (buttonX.IsTrailing)
		{
			dayMouseEventArgs.IsTrailingDay = true;
		}
		else
		{
			dayMouseEventArgs.IsTrailingDay = false;
		}
		if (result <= 0)
		{
			dayMouseEventArgs.ValueOfDay = null;
		}
		else
		{
			dayMouseEventArgs.ValueOfDay = result;
		}
		return dayMouseEventArgs;
	}

	private DayEventArgs GetDaysEventArgs(object sender, EventArgs e)
	{
		DayEventArgs dayEventArgs = new DayEventArgs();
		if (!(sender is ButtonX))
		{
			return dayEventArgs;
		}
		ButtonX buttonX = sender as ButtonX;
		int result = 0;
		int.TryParse(((Control)buttonX).Text, out result);
		if (buttonX.IsTrailing)
		{
			dayEventArgs.IsTrailingDay = true;
		}
		else
		{
			dayEventArgs.IsTrailingDay = false;
		}
		if (result <= 0)
		{
			dayEventArgs.ValueOfDay = null;
		}
		else
		{
			dayEventArgs.ValueOfDay = result;
		}
		return dayEventArgs;
	}

	private void DaysOfMonth_MouseClick(object sender, MouseEventArgs e)
	{
		if (this.DayMouseClick != null)
		{
			DayMouseEventArgs daysMouseEventArgs = GetDaysMouseEventArgs(btnDayClicked, e);
			this.DayMouseClick(daysMouseEventArgs);
		}
	}

	private void DaysOfMonth_MouseEnter(object sender, EventArgs e)
	{
		if (this.DayMouseEnter != null)
		{
			DayEventArgs daysEventArgs = GetDaysEventArgs(sender, e);
			this.DayMouseEnter(daysEventArgs);
		}
	}

	private void DaysOfMonth_MouseHover(object sender, EventArgs e)
	{
		if (this.DayMouseHover != null)
		{
			DayEventArgs daysEventArgs = GetDaysEventArgs(sender, e);
			this.DayMouseHover(daysEventArgs);
		}
	}

	private void DaysOfMonth_MouseLeave(object sender, EventArgs e)
	{
		if (this.DayMouseLeave != null)
		{
			DayEventArgs daysEventArgs = GetDaysEventArgs(sender, e);
			this.DayMouseLeave(daysEventArgs);
		}
	}

	private void DaysOfMonth_MouseMove(object sender, MouseEventArgs e)
	{
		if (this.DayMouseMove != null)
		{
			DayMouseEventArgs daysMouseEventArgs = GetDaysMouseEventArgs(sender, e);
			this.DayMouseMove(daysMouseEventArgs);
		}
	}

	private void DaysOfMonth_MouseUp(object sender, MouseEventArgs e)
	{
		if (this.DayMouseUp != null)
		{
			DayMouseEventArgs daysMouseEventArgs = GetDaysMouseEventArgs(sender, e);
			this.DayMouseUp(daysMouseEventArgs);
		}
	}

	private void txtTemp_KeyUp(object sender, KeyEventArgs e)
	{
		if (this.DayKeyUp != null)
		{
			this.DayKeyUp(e);
		}
	}

	private void txtTemp_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (this.DayKeyPress != null)
		{
			this.DayKeyPress(e);
		}
	}

	private void ArrangeComponents()
	{
		SetLocation_Panels();
		setLocations_Titles();
		setLocations_WeekDays();
		setLocations_Days();
		setLocations_Today();
	}

	private void SetLocation_Panels()
	{
		double num;
		double num2;
		double num3;
		if (_ShowToday)
		{
			num = 0.15;
			num2 = 0.1;
			num3 = 0.1;
		}
		else
		{
			num = 0.15;
			num2 = 0.1;
			num3 = 0.0;
		}
		int num4 = ((Control)this).Height - 2 * _iComponentMargin;
		int num5 = ((Control)this).Width - 2 * _iComponentMargin;
		int iComponentMargin = _iComponentMargin;
		int width = num5;
		int height = (int)((double)num4 * num);
		((Control)pnlTitles).Width = width;
		((Control)pnlTitles).Height = height;
		int iComponentMargin2 = _iComponentMargin;
		((Control)pnlTitles).Location = new Point(iComponentMargin, iComponentMargin2);
		height = (int)((double)num4 * num2);
		((Control)pnlWeekDays).Width = width;
		((Control)pnlWeekDays).Height = height;
		iComponentMargin2 += ((Control)pnlTitles).Height;
		((Control)pnlWeekDays).Location = new Point(iComponentMargin, iComponentMargin2);
		height = 2;
		((Control)lblLineWeek).Width = width;
		((Control)lblLineWeek).Height = height;
		iComponentMargin2 += ((Control)pnlWeekDays).Height;
		((Control)lblLineWeek).Location = new Point(iComponentMargin, iComponentMargin2);
		if (num3 == 0.0)
		{
			((Control)pnlToday).Height = 0;
			((Control)pnlToday).Visible = false;
		}
		else
		{
			((Control)pnlToday).Visible = true;
			height = (int)(num3 * (double)((Control)this).Height);
			((Control)pnlToday).Width = width;
			((Control)pnlToday).Height = height;
			iComponentMargin2 = ((Control)this).Height - ((Control)pnlToday).Height - _iComponentMargin;
			((Control)pnlToday).Location = new Point(iComponentMargin, iComponentMargin2);
		}
		height = num4 - (((Control)pnlTitles).Height + ((Control)pnlWeekDays).Height + ((Control)lblLineWeek).Height + ((Control)pnlToday).Height);
		((Control)pnlDays).Width = width;
		((Control)pnlDays).Height = height;
		iComponentMargin2 = _iComponentMargin + ((Control)pnlTitles).Height + ((Control)pnlWeekDays).Height + ((Control)lblLineWeek).Height;
		((Control)pnlDays).Location = new Point(iComponentMargin, iComponentMargin2);
	}

	private void setLocations_Titles()
	{
		int num = 8;
		int num2 = 4;
		int num3 = 3;
		int num4 = ((Control)pnlTitles).Width - 3 * num - 2 * num3;
		int height = ((Control)pnlTitles).Height - 2 * num2;
		int num5 = (int)((float)num4 * 0.15f);
		int num6 = (int)((float)num4 * 0.15f);
		int num7 = (int)((float)num4 * 0.4f);
		int num8 = (int)((float)num4 * 0.3f);
		if (_CalendarType == CalendarTypes.Persian)
		{
			int x = num3;
			int y = num2;
			((Control)btnNextMonth).Size = new Size(num5, height);
			((Control)btnNextMonth).Location = new Point(x, y);
			x = num3 + num5 + num;
			((Control)btnYear).Size = new Size(num8, height);
			((Control)btnYear).Location = new Point(x, y);
			x = num3 + num5 + num + num8 + num;
			((Control)btnMonth).Size = new Size(num7, height);
			((Control)btnMonth).Location = new Point(x, y);
			x = num3 + num5 + num + num8 + num + num7 + num;
			((Control)btnPreMonth).Size = new Size(num6, height);
			((Control)btnPreMonth).Location = new Point(x, y);
		}
		else
		{
			int x = num3;
			int y = num2;
			((Control)btnPreMonth).Size = new Size(num6, height);
			((Control)btnPreMonth).Location = new Point(x, y);
			x = num3 + num6 + num;
			((Control)btnMonth).Size = new Size(num7, height);
			((Control)btnMonth).Location = new Point(x, y);
			x = num3 + num6 + num + num7 + num;
			((Control)btnYear).Size = new Size(num8, height);
			((Control)btnYear).Location = new Point(x, y);
			x = num3 + num6 + num + num7 + num + num8 + num;
			((Control)btnNextMonth).Size = new Size(num5, height);
			((Control)btnNextMonth).Location = new Point(x, y);
		}
	}

	private void setLocations_WeekDays()
	{
		int num = ((Control)pnlWeekDays).Width / 7;
		int y = 0;
		int height = ((Control)pnlWeekDays).Height;
		clsCommonFunctions.NameOfWeekDays(_CalendarType, num, ((Control)this).Font);
		int x;
		if (_CalendarType == CalendarTypes.Persian)
		{
			int weekStartsOn = (int)_WeekStartsOn;
			int num2 = weekStartsOn;
			x = num2 * num;
			((Control)lblFri).Size = new Size(num, height);
			((Control)lblFri).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblThu).Size = new Size(num, height);
			((Control)lblThu).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblWed).Size = new Size(num, height);
			((Control)lblWed).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblTue).Size = new Size(num, height);
			((Control)lblTue).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblMon).Size = new Size(num, height);
			((Control)lblMon).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblSun).Size = new Size(num, height);
			((Control)lblSun).Location = new Point(x, y);
			num2++;
			if (num2 >= 7)
			{
				num2 -= 7;
			}
			x = num2 * num;
			((Control)lblSat).Size = new Size(num, height);
			((Control)lblSat).Location = new Point(x, y);
			return;
		}
		int weekStartsOn2 = (int)_WeekStartsOn;
		int num3 = weekStartsOn2;
		num3 = 7 - num3;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblSat).Size = new Size(num, height);
		((Control)lblSat).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblSun).Size = new Size(num, height);
		((Control)lblSun).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblMon).Size = new Size(num, height);
		((Control)lblMon).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblTue).Size = new Size(num, height);
		((Control)lblTue).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblWed).Size = new Size(num, height);
		((Control)lblWed).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblThu).Size = new Size(num, height);
		((Control)lblThu).Location = new Point(x, y);
		num3++;
		if (num3 >= 7)
		{
			num3 -= 7;
		}
		x = num3 * num;
		((Control)lblFri).Size = new Size(num, height);
		((Control)lblFri).Location = new Point(x, y);
	}

	private void setLocations_Days()
	{
		int col = 0;
		int row = 0;
		int margin = 0;
		DaysOfMonth_LocationAndSize(ref btn1, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn2, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn3, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn4, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn5, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn6, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn7, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn8, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn9, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn10, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn11, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn12, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn13, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn14, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn15, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn16, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn17, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn18, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn19, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn20, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn21, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn22, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn23, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn24, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn25, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn26, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn27, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn28, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn29, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn30, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn31, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn32, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn33, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn34, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn35, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn36, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn37, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn38, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn39, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn40, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn41, ref margin, ref row, ref col);
		DaysOfMonth_LocationAndSize(ref btn42, ref margin, ref row, ref col);
	}

	private void DaysOfMonth_LocationAndSize(ref ButtonX btn, ref int margin, ref int row, ref int col)
	{
		int num = (((Control)pnlDays).Width - 2 * margin) / 7;
		int num2 = ((Control)pnlDays).Height / 6;
		((Control)btn).Width = num;
		((Control)btn).Height = num2;
		int x = ((_CalendarType != CalendarTypes.Persian) ? (margin + col * num) : (margin + (6 - col) * num));
		int y = row * num2;
		col++;
		if (col == 7)
		{
			col = 0;
			row++;
		}
		((Control)btn).Location = new Point(x, y);
	}

	private void setLocations_Today()
	{
		if (((Control)pnlToday).Visible)
		{
			int num = 2;
			int num2 = ((Control)pnlToday).Height - 2 * num;
			int num3 = ((((Control)pnlToday).Height >= ((Control)pnlToday).Width / 2) ? ((int)((float)((Control)pnlToday).Width * 0.3f)) : num2);
			int num4 = ((Control)pnlToday).Width - 3 * num - num3;
			if (_CalendarType == CalendarTypes.Persian)
			{
				int num5 = num;
				int y = num;
				((Control)lblTodayText).Size = new Size(num4, num2);
				((Control)lblTodayText).Location = new Point(num5, y);
				num5 += num4 + num;
				((Control)btnTodayGoto).Size = new Size(num3, num2);
				((Control)btnTodayGoto).Location = new Point(num5, y);
			}
			else
			{
				int num5 = num;
				int y = num;
				((Control)btnTodayGoto).Size = new Size(num3, num2);
				((Control)btnTodayGoto).Location = new Point(num5, y);
				num5 += num3 + num;
				((Control)lblTodayText).Size = new Size(num4, num2);
				((Control)lblTodayText).Location = new Point(num5, y);
			}
		}
	}

	public void PersianHolidayDates_Add(int year, int month, int day)
	{
		AddToHolidayDates(CalendarTypes.Persian, year, month, day);
	}

	public void PersianHolidayDates_Add(PersianDateTime persianDate)
	{
		AddToHolidayDates(CalendarTypes.Persian, persianDate.Year, persianDate.Month, persianDate.Day);
	}

	public void EnglishHolidayDates_Add(int year, int month, int day)
	{
		AddToHolidayDates(CalendarTypes.English, year, month, day);
	}

	public void EnglishHolidayDates_Add(DateTime date)
	{
		AddToHolidayDates(CalendarTypes.English, date.Year, date.Month, date.Day);
	}

	private void AddToHolidayDates(CalendarTypes type, int year, int month, int day)
	{
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime item2 = new PersianDateTime(year, month, day);
			_HolidayDates_Persian.Add(item2);
			break;
		}
		case CalendarTypes.English:
		{
			DateTime item = new DateTime(year, month, day);
			_HolidayDates_English.Add(item);
			break;
		}
		}
		SetTextForComponents();
	}

	public void PersianHolidayDates_RemoveAll()
	{
		_HolidayDates_Persian.Clear();
	}

	public void EnglishHolidayDates_RemoveAll()
	{
		_HolidayDates_English.Clear();
		SetTextForComponents();
	}

	public void PersianHolidayDates_Remove(int year, int month, int day)
	{
		RemoveFromHolidayDates(CalendarTypes.Persian, year, month, day);
	}

	public void PersianHolidayDates_Remove(PersianDateTime date)
	{
		RemoveFromHolidayDates(CalendarTypes.Persian, date.Year, date.Month, date.Day);
	}

	public void EnglishHolidayDates_Remove(int year, int month, int day)
	{
		RemoveFromHolidayDates(CalendarTypes.English, year, month, day);
	}

	public void EnglishHolidayDates_Remove(DateTime date)
	{
		RemoveFromHolidayDates(CalendarTypes.English, date.Year, date.Month, date.Day);
	}

	private void RemoveFromHolidayDates(CalendarTypes type, int year, int month, int day)
	{
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			for (int j = 0; j < _HolidayDates_Persian.Count; j++)
			{
				PersianDateTime persianDateTime2 = _HolidayDates_Persian[j];
				if (persianDateTime2.Year == persianDateTime.Year && persianDateTime2.Month == persianDateTime.Month && persianDateTime2.Day == persianDateTime.Day)
				{
					_HolidayDates_Persian.RemoveAt(j);
				}
			}
			break;
		}
		case CalendarTypes.English:
		{
			DateTime dateTime = new DateTime(year, month, day);
			for (int i = 0; i < _HolidayDates_English.Count; i++)
			{
				DateTime dateTime2 = _HolidayDates_English[i];
				if (dateTime2.Year == dateTime.Year && dateTime2.Month == dateTime.Month && dateTime2.Day == dateTime.Day)
				{
					_HolidayDates_English.RemoveAt(i);
				}
			}
			break;
		}
		}
		SetTextForComponents();
	}

	public bool PersianHolidayDates_Contains(int year, int month, int day)
	{
		return HolidayDates_IsContains(CalendarTypes.Persian, year, month, day);
	}

	public bool EnglishHolidayDates_Contains(int year, int month, int day)
	{
		return HolidayDates_IsContains(CalendarTypes.English, year, month, day);
	}

	private bool HolidayDates_IsContains(CalendarTypes type, int year, int month, int day)
	{
		bool result = false;
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			for (int j = 0; j < _HolidayDates_Persian.Count; j++)
			{
				PersianDateTime persianDateTime2 = _HolidayDates_Persian[j];
				if (persianDateTime2.Year == persianDateTime.Year && persianDateTime2.Month == persianDateTime.Month && persianDateTime2.Day == persianDateTime.Day)
				{
					result = true;
					break;
				}
			}
			break;
		}
		case CalendarTypes.English:
		{
			DateTime dateTime = new DateTime(year, month, day);
			for (int i = 0; i < _HolidayDates_English.Count; i++)
			{
				DateTime dateTime2 = _HolidayDates_English[i];
				if (dateTime2.Year == dateTime.Year && dateTime2.Month == dateTime.Month && dateTime2.Day == dateTime.Day)
				{
					result = true;
					break;
				}
			}
			break;
		}
		}
		return result;
	}

	public void PersianBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Single, year, month, day);
	}

	public void PersianBoldedDates_Add(PersianDateTime date)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Single, date.Year, date.Month, date.Day);
	}

	public void EnglishBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Single, year, month, day);
	}

	public void EnglishBoldedDates_Add(DateTime date)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Single, date.Year, date.Month, date.Day);
	}

	public void PersianAnnuallyBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Annually, year, month, day);
	}

	public void PersianAnnuallyBoldedDates_Add(PersianDateTime date)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Annually, date.Year, date.Month, date.Day);
	}

	public void EnglishAnnuallyBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Annually, year, month, day);
	}

	public void EnglishAnnuallyBoldedDates_Add(DateTime date)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Annually, date.Year, date.Month, date.Day);
	}

	public void PersianMonthlyBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Monthly, year, month, day);
	}

	public void PersianMonthlyBoldedDates_Add(PersianDateTime date)
	{
		AddToBoldedDates(CalendarTypes.Persian, enmBoldedDates.Monthly, date.Year, date.Month, date.Day);
	}

	public void EnglishMonthlyBoldedDates_Add(int year, int month, int day)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Monthly, year, month, day);
	}

	public void EnglishMonthlyBoldedDates_Add(DateTime date)
	{
		AddToBoldedDates(CalendarTypes.English, enmBoldedDates.Monthly, date.Year, date.Month, date.Day);
	}

	private void AddToBoldedDates(CalendarTypes type, enmBoldedDates enmBolded, int year, int month, int day)
	{
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime item2 = new PersianDateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
				_AnnuallyBoldedDates_Persian.Add(item2);
				break;
			case enmBoldedDates.Monthly:
				_MonthlyBoldedDates_Persian.Add(item2);
				break;
			case enmBoldedDates.Single:
				_BoldedDates_Persian.Add(item2);
				break;
			}
			break;
		}
		case CalendarTypes.English:
		{
			DateTime item = new DateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
				_AnnuallyBoldedDates_English.Add(item);
				break;
			case enmBoldedDates.Monthly:
				_MonthlyBoldedDates_English.Add(item);
				break;
			case enmBoldedDates.Single:
				_BoldedDates_English.Add(item);
				break;
			}
			break;
		}
		}
		SetTextForComponents();
	}

	public void PersianBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Single, year, month, day);
	}

	public void PersianBoldedDates_Remove(PersianDateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Single, date.Year, date.Month, date.Day);
	}

	public void EnglishBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Single, year, month, day);
	}

	public void EnglishBoldedDates_Remove(DateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Single, date.Year, date.Month, date.Day);
	}

	public void PersianBoldedDates_RemoveAll()
	{
		_BoldedDates_Persian.Clear();
		SetTextForComponents();
	}

	public void EnglishBoldedDates_RemoveAll()
	{
		_BoldedDates_English.Clear();
		SetTextForComponents();
	}

	public void PersianAnnuallyBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Annually, year, month, day);
	}

	public void PersianAnnuallyBoldedDates_Remove(PersianDateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Annually, date.Year, date.Month, date.Day);
	}

	public void EnglishAnnuallyBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Annually, year, month, day);
	}

	public void EnglishAnnuallyBoldedDates_Remove(DateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Annually, date.Year, date.Month, date.Day);
	}

	public void PersianAnnuallyBoldedDates_RemoveAll()
	{
		_AnnuallyBoldedDates_Persian.Clear();
		SetTextForComponents();
	}

	public void EnglishAnnuallyBoldedDates_RemoveAll()
	{
		_AnnuallyBoldedDates_English.Clear();
		SetTextForComponents();
	}

	public void PersianMonthlyBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Single, year, month, day);
	}

	public void PersianMonthlyBoldedDates_Remove(PersianDateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.Persian, enmBoldedDates.Monthly, date.Year, date.Month, date.Day);
	}

	public void EnglishMonthlyBoldedDates_Remove(int year, int month, int day)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Single, year, month, day);
	}

	public void EnglishMonthlyBoldedDates_Remove(DateTime date)
	{
		RemoveFromBoldedDates(CalendarTypes.English, enmBoldedDates.Monthly, date.Year, date.Month, date.Day);
	}

	public void PersianMonthlyBoldedDates_RemoveAll()
	{
		_MonthlyBoldedDates_Persian.Clear();
		SetTextForComponents();
	}

	public void EnglishMonthlyBoldedDates_RemoveAll()
	{
		_MonthlyBoldedDates_English.Clear();
		SetTextForComponents();
	}

	private void RemoveFromBoldedDates(CalendarTypes type, enmBoldedDates enmBolded, int year, int month, int day)
	{
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
			{
				for (int m = 0; m < _AnnuallyBoldedDates_Persian.Count; m++)
				{
					PersianDateTime persianDateTime3 = _AnnuallyBoldedDates_Persian[m];
					if (persianDateTime3.Month == persianDateTime.Month && persianDateTime3.Day == persianDateTime.Day)
					{
						_AnnuallyBoldedDates_Persian.RemoveAt(m);
						m--;
					}
				}
				break;
			}
			case enmBoldedDates.Monthly:
			{
				for (int n = 0; n < _MonthlyBoldedDates_Persian.Count; n++)
				{
					if (_MonthlyBoldedDates_Persian[n].Day == persianDateTime.Day)
					{
						_MonthlyBoldedDates_Persian.RemoveAt(n);
						n--;
					}
				}
				break;
			}
			case enmBoldedDates.Single:
			{
				for (int l = 0; l < _BoldedDates_Persian.Count; l++)
				{
					PersianDateTime persianDateTime2 = _BoldedDates_Persian[l];
					if (persianDateTime2.Year == persianDateTime.Year && persianDateTime2.Month == persianDateTime.Month && persianDateTime2.Day == persianDateTime.Day)
					{
						_BoldedDates_Persian.RemoveAt(l);
						l--;
					}
				}
				break;
			}
			}
			break;
		}
		case CalendarTypes.English:
		{
			DateTime dateTime = new DateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
			{
				for (int j = 0; j < _AnnuallyBoldedDates_English.Count; j++)
				{
					DateTime dateTime3 = _AnnuallyBoldedDates_English[j];
					if (dateTime3.Month == dateTime.Month && dateTime3.Day == dateTime.Day)
					{
						_AnnuallyBoldedDates_English.RemoveAt(j);
						j--;
					}
				}
				break;
			}
			case enmBoldedDates.Monthly:
			{
				for (int k = 0; k < _MonthlyBoldedDates_English.Count; k++)
				{
					if (_MonthlyBoldedDates_English[k].Day == dateTime.Day)
					{
						_MonthlyBoldedDates_English.RemoveAt(k);
						k--;
					}
				}
				break;
			}
			case enmBoldedDates.Single:
			{
				for (int i = 0; i < _BoldedDates_English.Count; i++)
				{
					DateTime dateTime2 = _BoldedDates_English[i];
					if (dateTime2.Year == dateTime.Year && dateTime2.Month == dateTime.Month && dateTime2.Day == dateTime.Day)
					{
						_BoldedDates_English.RemoveAt(i);
						i--;
					}
				}
				break;
			}
			}
			break;
		}
		}
		SetTextForComponents();
	}

	public bool PersianBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.Persian, enmBoldedDates.Single, year, month, day);
	}

	public bool EnglishBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.English, enmBoldedDates.Single, year, month, day);
	}

	public bool PersianAnnuallyBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.Persian, enmBoldedDates.Annually, year, month, day);
	}

	public bool EnglishAnnuallyBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.English, enmBoldedDates.Annually, year, month, day);
	}

	public bool PersianMonthlyBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.Persian, enmBoldedDates.Monthly, year, month, day);
	}

	public bool EnglishMonthlyBoldedDates_Contains(int year, int month, int day)
	{
		return boldedDates_Contains(CalendarTypes.English, enmBoldedDates.Monthly, year, month, day);
	}

	private bool boldedDates_Contains(CalendarTypes type, enmBoldedDates enmBolded, int year, int month, int day)
	{
		bool result = false;
		switch (type)
		{
		case CalendarTypes.Persian:
		{
			PersianDateTime persianDateTime = new PersianDateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
			{
				for (int m = 0; m < _AnnuallyBoldedDates_Persian.Count; m++)
				{
					PersianDateTime persianDateTime3 = _AnnuallyBoldedDates_Persian[m];
					if (persianDateTime3.Month == persianDateTime.Month && persianDateTime3.Day == persianDateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			case enmBoldedDates.Monthly:
			{
				for (int n = 0; n < _MonthlyBoldedDates_Persian.Count; n++)
				{
					if (_MonthlyBoldedDates_Persian[n].Day == persianDateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			case enmBoldedDates.Single:
			{
				for (int l = 0; l < _BoldedDates_Persian.Count; l++)
				{
					PersianDateTime persianDateTime2 = _BoldedDates_Persian[l];
					if (persianDateTime2.Year == persianDateTime.Year && persianDateTime2.Month == persianDateTime.Month && persianDateTime2.Day == persianDateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			}
			break;
		}
		case CalendarTypes.English:
		{
			DateTime dateTime = new DateTime(year, month, day);
			switch (enmBolded)
			{
			case enmBoldedDates.Annually:
			{
				for (int j = 0; j < _AnnuallyBoldedDates_English.Count; j++)
				{
					DateTime dateTime3 = _AnnuallyBoldedDates_English[j];
					if (dateTime3.Month == dateTime.Month && dateTime3.Day == dateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			case enmBoldedDates.Monthly:
			{
				for (int k = 0; k < _MonthlyBoldedDates_English.Count; k++)
				{
					if (_MonthlyBoldedDates_English[k].Day == dateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			case enmBoldedDates.Single:
			{
				for (int i = 0; i < _BoldedDates_English.Count; i++)
				{
					DateTime dateTime2 = _BoldedDates_English[i];
					if (dateTime2.Year == dateTime.Year && dateTime2.Month == dateTime.Month && dateTime2.Day == dateTime.Day)
					{
						result = true;
						break;
					}
				}
				break;
			}
			}
			break;
		}
		}
		return result;
	}

	private void ChangeStyle_Titles()
	{
		((Control)pnlTitles).BackColor = _TitleBackColor;
		((Control)pnlTitles).Font = _TitleFont;
		((Control)pnlTitles).ForeColor = _TitleForeColor;
		if (btnNextMonth.Style != 0)
		{
			((Control)btnNextMonth).BackColor = _TitleBackColor;
		}
		((Control)btnNextMonth).Font = _TitleFont;
		((Control)btnNextMonth).ForeColor = _TitleForeColor;
		if (btnPreMonth.Style != 0)
		{
			((Control)btnPreMonth).BackColor = _TitleBackColor;
		}
		((Control)btnPreMonth).Font = _TitleFont;
		((Control)btnPreMonth).ForeColor = _TitleForeColor;
		if (btnMonth.Style != 0)
		{
			((Control)btnMonth).BackColor = _TitleBackColor;
		}
		((Control)btnMonth).Font = _TitleFont;
		((Control)btnMonth).ForeColor = _TitleForeColor;
		if (btnYear.Style != 0)
		{
			((Control)btnYear).BackColor = _TitleBackColor;
		}
		((Control)btnYear).Font = _TitleFont;
		((Control)btnYear).ForeColor = _TitleForeColor;
	}

	private void ChangeStyle_WeekDays()
	{
		((Control)pnlWeekDays).ForeColor = _WeekDaysForeColor;
		((Control)pnlWeekDays).BackColor = _WeekDaysBackColor;
		((Control)pnlWeekDays).Font = _WeekDaysFont;
		((Control)lblFri).ForeColor = _WeekDaysForeColor;
		((Control)lblFri).BackColor = _WeekDaysBackColor;
		((Control)lblFri).Font = _WeekDaysFont;
		((Control)lblMon).ForeColor = _WeekDaysForeColor;
		((Control)lblMon).BackColor = _WeekDaysBackColor;
		((Control)lblMon).Font = _WeekDaysFont;
		((Control)lblSat).ForeColor = _WeekDaysForeColor;
		((Control)lblSat).BackColor = _WeekDaysBackColor;
		((Control)lblSat).Font = _WeekDaysFont;
		((Control)lblSun).ForeColor = _WeekDaysForeColor;
		((Control)lblSun).BackColor = _WeekDaysBackColor;
		((Control)lblSun).Font = _WeekDaysFont;
		((Control)lblThu).ForeColor = _WeekDaysForeColor;
		((Control)lblThu).BackColor = _WeekDaysBackColor;
		((Control)lblThu).Font = _WeekDaysFont;
		((Control)lblTue).ForeColor = _WeekDaysForeColor;
		((Control)lblTue).BackColor = _WeekDaysBackColor;
		((Control)lblTue).Font = _WeekDaysFont;
		((Control)lblWed).ForeColor = _WeekDaysForeColor;
		((Control)lblWed).BackColor = _WeekDaysBackColor;
		((Control)lblWed).Font = _WeekDaysFont;
	}

	private void ChangeStyle_LineWeek()
	{
		((Control)lblLineWeek).BackColor = _LineWeekColor;
	}

	private void ChangeStyle_Days()
	{
		((Control)pnlDays).BackColor = _DaysBackColor;
		((Control)pnlDays).ForeColor = _DaysForeColor;
		((Control)pnlDays).Font = _DaysFont;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
		{
			ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
			((Control)buttonX).BackColor = _DaysBackColor;
			((Control)buttonX).Font = _DaysFont;
			((Control)buttonX).ForeColor = _DaysForeColor;
			buttonX.SecondBorderWidth = _DayRectTickness;
		}
	}

	private void ChangeStyle_BoldedDay()
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
		{
			ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
			buttonX.BoldedForeColor = _BoldedDayForeColor;
		}
	}

	private void ChangeStyle_Holiday()
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
		{
			ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
			buttonX.HolidayForeColor = _HolidayForeColor;
		}
	}

	private void ChangeStyle_TrailingDay()
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
		{
			ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
			buttonX.TrailingForeColor = _TrailingForeColor;
		}
	}

	private void ChangeStyle_Today()
	{
		ArrangeComponents();
		if (((Control)pnlToday).Visible)
		{
			((Control)pnlToday).BackColor = _TodayBackColor;
			((Control)pnlToday).Font = _TodayFont;
			((Control)pnlToday).ForeColor = _TodayForeColor;
			((Control)lblTodayText).ForeColor = _TodayForeColor;
			((Control)lblTodayText).BackColor = _TodayBackColor;
			((Control)lblTodayText).Font = _TodayFont;
			((Control)lblTodayText).Visible = _ShowToday;
			if (_ShowTodayRect)
			{
				((Control)btnTodayGoto).Visible = _ShowToday;
			}
		}
	}

	private void ChangeStyle_TodayRect()
	{
		if (btnTodayGoto.Style != 0)
		{
			((Control)btnTodayGoto).BackColor = _TodayBackColor;
		}
		((Control)btnTodayGoto).Visible = _ShowTodayRect;
		btnTodayGoto.ShowSecondBorder = _ShowTodayRect;
		btnTodayGoto.SecondBorderColor = _TodayRectColor;
		if (btn1.SecondBorderColor != _TodayRectColor)
		{
			for (int i = 0; i < ((ArrangedElementCollection)((Control)pnlDays).Controls).Count; i++)
			{
				ButtonX buttonX = ((Control)pnlDays).Controls[i] as ButtonX;
				buttonX.SecondBorderColor = _TodayRectColor;
			}
		}
		btnTodayGoto.SecondBorderWidth = _TodayRectTickness;
	}

	private void ChangeStyle_ToolTips()
	{
		toolTip1.Active = _ShowToolTips;
		if (_CalendarType == CalendarTypes.Persian)
		{
			toolTip1.SetToolTip((Control)(object)btnNextMonth, "ماه بعد");
			toolTip1.SetToolTip((Control)(object)btnPreMonth, "ماه قبل");
			toolTip1.SetToolTip((Control)(object)btnMonth, "انتخاب ماه");
			toolTip1.SetToolTip((Control)(object)btnYear, "انتخاب سال");
			toolTip1.SetToolTip((Control)(object)btnTodayGoto, "برو به امروز");
		}
		else
		{
			toolTip1.SetToolTip((Control)(object)btnNextMonth, "Next Month");
			toolTip1.SetToolTip((Control)(object)btnPreMonth, "Previous Month");
			toolTip1.SetToolTip((Control)(object)btnMonth, "Select Month");
			toolTip1.SetToolTip((Control)(object)btnYear, "Select Year");
			toolTip1.SetToolTip((Control)(object)btnTodayGoto, "Go To Today");
		}
	}

	private void InitializeVariables()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		txtTemp = new TextBox();
		((Control)txtTemp).KeyDown += new KeyEventHandler(txtTempForKeys_KeyDown);
		((Control)txtTemp).KeyUp += new KeyEventHandler(txtTempForKeys_KeyUp);
		((Control)txtTemp).KeyPress += new KeyPressEventHandler(txtTemp_KeyPress);
		((Control)txtTemp).GotFocus += txtTempForKeys_GotFocus;
		((Control)txtTemp).LostFocus += txtTempForKeys_LostFocus;
		((Control)txtTemp).Width = 0;
		((Control)this).Controls.Add((Control)(object)txtTemp);
		_iComponentMargin = 1;
		_bFormLoaded = false;
		_bTxtTempFocused = false;
		_bNudYearFocused = false;
		_bBtnXFocused = false;
		_SelectedDate = DateTime.Now;
		_TodayDate = DateTime.Now;
		_CalendarType = CalendarTypes.Persian;
		try
		{
			_TitleFont = new Font("Tahoma", ((Control)this).Font.Size);
		}
		catch
		{
			_TitleFont = ((Control)this).Font;
		}
		_TitleForeColor = Color.Black;
		_TitleBackColor = Color.Wheat;
		_WeekDaysForeColor = Color.OrangeRed;
		_WeekDaysBackColor = Color.Wheat;
		try
		{
			_WeekDaysFont = new Font("Tahoma", ((Control)this).Font.Size);
		}
		catch
		{
			_WeekDaysFont = ((Control)this).Font;
		}
		_LineWeekColor = Color.Black;
		_DaysFont = ((Control)this).Font;
		_DaysForeColor = Color.DodgerBlue;
		_DaysBackColor = Color.LightGray;
		try
		{
			_TodayFont = new Font("Tahoma", ((Control)this).Font.Size, (FontStyle)1);
		}
		catch
		{
			_TodayFont = ((Control)this).Font;
		}
		_TodayForeColor = Color.Black;
		_TodayBackColor = Color.Wheat;
		_BoldedDayForeColor = Color.Blue;
		_TrailingForeColor = Color.DarkGray;
		_ShowTrailing = true;
		_TodayRectColor = Color.Coral;
		_ShowTodayRect = true;
		_ShowToday = true;
		_TodayRectTickness = 2f;
		_DayRectTickness = 2f;
		_WeekStartsOn = WeekDays.Saturday;
		_HolidayForeColor = Color.Red;
		_HolidayWeekly = DayOfWeekForHoliday.Friday;
		_BorderColor = Color.CadetBlue;
		_BoldedDates_English = new List<DateTime>();
		_MonthlyBoldedDates_English = new List<DateTime>();
		_AnnuallyBoldedDates_English = new List<DateTime>();
		_HolidayDates_English = new List<DateTime>();
		_BoldedDates_Persian = new List<PersianDateTime>();
		_MonthlyBoldedDates_Persian = new List<PersianDateTime>();
		_AnnuallyBoldedDates_Persian = new List<PersianDateTime>();
		_HolidayDates_Persian = new List<PersianDateTime>();
		_ShowToolTips = false;
	}

	private void InitializeNewVariables(ref MonthCalendarX monthCalendarX)
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		_AnnuallyBoldedDates_English = monthCalendarX._AnnuallyBoldedDates_English;
		_BoldedDates_English = monthCalendarX._BoldedDates_English;
		_AnnuallyBoldedDates_Persian = monthCalendarX._AnnuallyBoldedDates_Persian;
		_BoldedDates_Persian = monthCalendarX._BoldedDates_Persian;
		_BoldedDayForeColor = monthCalendarX._BoldedDayForeColor;
		_BorderColor = monthCalendarX._BorderColor;
		_CalendarType = monthCalendarX._CalendarType;
		_DaysBackColor = monthCalendarX._DaysBackColor;
		_DaysFont = monthCalendarX._DaysFont;
		_DaysForeColor = monthCalendarX._DaysForeColor;
		_HolidayDates_English = monthCalendarX._HolidayDates_English;
		_HolidayDates_Persian = monthCalendarX._HolidayDates_Persian;
		_HolidayForeColor = monthCalendarX._HolidayForeColor;
		_HolidayWeekly = monthCalendarX._HolidayWeekly;
		_LineWeekColor = monthCalendarX._LineWeekColor;
		_MonthlyBoldedDates_English = monthCalendarX._MonthlyBoldedDates_English;
		_MonthlyBoldedDates_Persian = monthCalendarX._MonthlyBoldedDates_Persian;
		_SelectedDate = monthCalendarX._SelectedDate;
		_ShowToday = monthCalendarX._ShowToday;
		_ShowTodayRect = monthCalendarX._ShowTodayRect;
		_ShowTrailing = monthCalendarX._ShowTrailing;
		_TitleBackColor = monthCalendarX._TitleBackColor;
		_TitleFont = monthCalendarX._TitleFont;
		_TitleForeColor = monthCalendarX._TitleForeColor;
		_TodayBackColor = monthCalendarX._TodayBackColor;
		_TodayDate = monthCalendarX._TodayDate;
		_TodayFont = monthCalendarX._TodayFont;
		_TodayForeColor = monthCalendarX._TodayForeColor;
		_TodayRectColor = monthCalendarX._TodayRectColor;
		_TodayRectTickness = monthCalendarX._TodayRectTickness;
		_TrailingForeColor = monthCalendarX._TrailingForeColor;
		_WeekDaysBackColor = monthCalendarX._WeekDaysBackColor;
		_WeekDaysFont = monthCalendarX._WeekDaysFont;
		_WeekDaysForeColor = monthCalendarX._WeekDaysForeColor;
		_WeekStartsOn = monthCalendarX._WeekStartsOn;
		_ShowToolTips = monthCalendarX._ShowToolTips;
		((Control)this).BackColor = ((Control)monthCalendarX).BackColor;
		((Control)this).Font = ((Control)monthCalendarX).Font;
		((Control)this).ForeColor = ((Control)monthCalendarX).ForeColor;
		((Control)this).RightToLeft = ((Control)monthCalendarX).RightToLeft;
		LoadMonthCalendarX();
	}
}
