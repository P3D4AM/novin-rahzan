using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Dashboard.Viewer.Controls.DatePicker;

[ToolboxItem(false)]
public class StiDateRangeControl : UserControl
{
	private StiControlColorCollection colors;

	private IContainer components;

	private MonthCalendar calendarFrom;

	private MonthCalendar calendarTo;

	private StiListBox listBoxPredefined;

	private Label labelFrom;

	private Label labelTo;

	public static string LocDayTomorrow => Loc.Get("DatePickerRanges", "Tomorrow");

	public static string LocDayToday => Loc.Get("DatePickerRanges", "Today");

	public static string LocDayYesterday => Loc.Get("DatePickerRanges", "Yesterday");

	public static string LocLast7Days => Loc.Get("DatePickerRanges", "Last7Days");

	public static string LocLast14Days => Loc.Get("DatePickerRanges", "Last14Days");

	public static string LocLast30Days => Loc.Get("DatePickerRanges", "Last30Days");

	public static string LocWeekNext => Loc.Get("DatePickerRanges", "NextWeek");

	public static string LocWeekCurrent => Loc.Get("DatePickerRanges", "CurrentWeek");

	public static string LocWeekPrevious => Loc.Get("DatePickerRanges", "PreviousWeek");

	public static string LocMonthNext => Loc.Get("DatePickerRanges", "NextMonth");

	public static string LocMonthCurrent => Loc.Get("DatePickerRanges", "CurrentMonth");

	public static string LocMonthPrevious => Loc.Get("DatePickerRanges", "PreviousMonth");

	public static string LocQuarterNext => Loc.Get("DatePickerRanges", "NextQuarter");

	public static string LocQuarterCurrent => Loc.Get("DatePickerRanges", "CurrentQuarter");

	public static string LocQuarterPrevious => Loc.Get("DatePickerRanges", "PreviousQuarter");

	public static string LocQuarterFirst => Loc.Get("DatePickerRanges", "FirstQuarter");

	public static string LocQuarterSecond => Loc.Get("DatePickerRanges", "SecondQuarter");

	public static string LocQuarterThird => Loc.Get("DatePickerRanges", "ThirdQuarter");

	public static string LocQuarterFourth => Loc.Get("DatePickerRanges", "FourthQuarter");

	public static string LocYearNext => Loc.Get("DatePickerRanges", "NextYear");

	public static string LocYearCurrent => Loc.Get("DatePickerRanges", "CurrentYear");

	public static string LocYearPrevious => Loc.Get("DatePickerRanges", "PreviousYear");

	public static string LocDateToWeek => Loc.Get("DatePickerRanges", "WeekToDate");

	public static string LocDateToMonth => Loc.Get("DatePickerRanges", "MonthToDate");

	public static string LocDateToQuarter => Loc.Get("DatePickerRanges", "QuarterToDate");

	public static string LocDateToYear => Loc.Get("DatePickerRanges", "YearToDate");

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionStart
	{
		get
		{
			return calendarFrom.SelectionStart;
		}
		set
		{
			calendarFrom.SelectionStart = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public DateTime SelectionEnd
	{
		get
		{
			return calendarTo.SelectionEnd;
		}
		set
		{
			calendarTo.SelectionEnd = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
				listBoxPredefined.Colors = value;
			}
		}
	}

	public event EventHandler DateChanged;

	public StiDateRangeControl()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		InitializeComponent();
		((ListBox)listBoxPredefined).ItemHeight = StiScale.YYI(16.0);
		calendarFrom.DateChanged += (DateRangeEventHandler)delegate
		{
			this.DateChanged?.Invoke(this, EventArgs.Empty);
		};
		calendarTo.DateChanged += (DateRangeEventHandler)delegate
		{
			this.DateChanged?.Invoke(this, EventArgs.Empty);
		};
		Localize();
		((ListBox)listBoxPredefined).Items.Add((object)LocDayTomorrow);
		((ListBox)listBoxPredefined).Items.Add((object)LocDayToday);
		((ListBox)listBoxPredefined).Items.Add((object)LocDayYesterday);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocLast7Days);
		((ListBox)listBoxPredefined).Items.Add((object)LocLast14Days);
		((ListBox)listBoxPredefined).Items.Add((object)LocLast30Days);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocWeekNext);
		((ListBox)listBoxPredefined).Items.Add((object)LocWeekCurrent);
		((ListBox)listBoxPredefined).Items.Add((object)LocWeekPrevious);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocMonthNext);
		((ListBox)listBoxPredefined).Items.Add((object)LocMonthCurrent);
		((ListBox)listBoxPredefined).Items.Add((object)LocMonthPrevious);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterNext);
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterCurrent);
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterPrevious);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocYearNext);
		((ListBox)listBoxPredefined).Items.Add((object)LocYearCurrent);
		((ListBox)listBoxPredefined).Items.Add((object)LocYearPrevious);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterFirst);
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterSecond);
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterThird);
		((ListBox)listBoxPredefined).Items.Add((object)LocQuarterFourth);
		((ListBox)listBoxPredefined).Items.Add((object)"-");
		((ListBox)listBoxPredefined).Items.Add((object)LocDateToWeek);
		((ListBox)listBoxPredefined).Items.Add((object)LocDateToMonth);
		((ListBox)listBoxPredefined).Items.Add((object)LocDateToQuarter);
		((ListBox)listBoxPredefined).Items.Add((object)LocDateToYear);
	}

	private void Localize()
	{
		((Control)labelFrom).Text = Loc.Get("PropertyMain", "From");
		((Control)labelTo).Text = Loc.Get("PropertyMain", "To");
	}

	public void RefreshSize()
	{
		Label obj = labelFrom;
		int left = (((Control)calendarFrom).Left = StiScale.I12);
		((Control)obj).Left = left;
		Label obj2 = labelTo;
		left = (((Control)calendarTo).Left = StiScale.I12 + ((Control)calendarFrom).Right);
		((Control)obj2).Left = left;
		((Control)listBoxPredefined).Top = StiScale.I12;
		((Control)listBoxPredefined).Left = StiScale.XXI(24.0) + ((Control)calendarTo).Right;
		((Control)listBoxPredefined).Height = ((Control)calendarTo).Bottom - ((Control)listBoxPredefined).Top;
		((Control)labelFrom).Width = ((Control)calendarFrom).Width;
		((Control)labelTo).Width = ((Control)calendarTo).Width;
		((Control)this).Width = ((Control)listBoxPredefined).Right + StiScale.I12;
		((Control)this).Height = ((Control)calendarTo).Bottom + StiScale.I12;
	}

	private void SetRange(DateTime start, DateTime end)
	{
		SelectionStart = start;
		SelectionEnd = end;
	}

	private void RefreshColors()
	{
		((Control)labelFrom).ForeColor = ((Control)this).ForeColor;
		((Control)labelTo).ForeColor = ((Control)this).ForeColor;
		((Control)listBoxPredefined).ForeColor = ((Control)this).ForeColor;
		((Control)listBoxPredefined).BackColor = ((Control)this).BackColor;
		calendarFrom.TitleBackColor = ((Control)this).ForeColor;
		calendarFrom.TitleForeColor = ((Control)this).BackColor;
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)this).OnForeColorChanged(e);
		RefreshColors();
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		RefreshColors();
	}

	private void Calendar_SizeChanged(object sender, EventArgs e)
	{
		RefreshSize();
	}

	private void ListBoxPredefined_Click(object sender, EventArgs e)
	{
		string text = ((ListBox)listBoxPredefined).SelectedItem?.ToString();
		if (text != null)
		{
			DateTime now = DateTime.Now;
			if (text == LocDayTomorrow)
			{
				SetRange(now.AddDays(1.0), now.AddDays(1.0));
			}
			else if (text == LocDayToday)
			{
				SetRange(now, now);
			}
			else if (text == LocDayYesterday)
			{
				SetRange(now.AddDays(-1.0), now.AddDays(-1.0));
			}
			if (text == LocLast7Days)
			{
				SetRange(now.AddDays(-7.0), now);
			}
			else if (text == LocLast14Days)
			{
				SetRange(now.AddDays(-14.0), now);
			}
			else if (text == LocLast30Days)
			{
				SetRange(now.AddDays(-30.0), now);
			}
			else if (text == LocWeekNext)
			{
				SetRange(now.AddDays(7.0).FirstDayOfWeek(), now.AddDays(7.0).LastDayOfWeek());
			}
			else if (text == LocWeekCurrent)
			{
				SetRange(now.FirstDayOfWeek(), now.LastDayOfWeek());
			}
			else if (text == LocWeekPrevious)
			{
				SetRange(now.AddDays(-7.0).FirstDayOfWeek(), now.AddDays(-7.0).LastDayOfWeek());
			}
			else if (text == LocMonthNext)
			{
				SetRange(now.AddMonths(1).FirstDayOfMonth(), now.AddMonths(1).LastDayOfMonth());
			}
			else if (text == LocMonthCurrent)
			{
				SetRange(now.FirstDayOfMonth(), now.LastDayOfMonth());
			}
			else if (text == LocMonthPrevious)
			{
				SetRange(now.AddMonths(-1).FirstDayOfMonth(), now.AddMonths(-1).LastDayOfMonth());
			}
			else if (text == LocQuarterNext)
			{
				SetRange(now.AddMonths(3).FirstDayOfQuarter(), now.AddMonths(3).LastDayOfQuarter());
			}
			else if (text == LocQuarterCurrent)
			{
				SetRange(now.FirstDayOfQuarter(), now.LastDayOfQuarter());
			}
			else if (text == LocQuarterPrevious)
			{
				SetRange(now.AddMonths(-3).FirstDayOfQuarter(), now.AddMonths(-3).LastDayOfQuarter());
			}
			else if (text == LocQuarterFirst)
			{
				SetRange(now.FirstDayOfFirthQuarter(), now.LastDayOfFirthQuarter());
			}
			else if (text == LocQuarterSecond)
			{
				SetRange(now.FirstDayOfSecondQuarter(), now.LastDayOfSecondQuarter());
			}
			else if (text == LocQuarterThird)
			{
				SetRange(now.FirstDayOfThirdQuarter(), now.LastDayOfThirdQuarter());
			}
			else if (text == LocQuarterFourth)
			{
				SetRange(now.FirstDayOfFourthQuarter(), now.LastDayOfFourthQuarter());
			}
			else if (text == LocYearNext)
			{
				SetRange(now.AddYears(1).FirstDayOfYear(), now.AddYears(1).LastDayOfYear());
			}
			else if (text == LocYearCurrent)
			{
				SetRange(now.FirstDayOfYear(), now.LastDayOfYear());
			}
			else if (text == LocYearPrevious)
			{
				SetRange(now.AddYears(-1).FirstDayOfYear(), now.AddYears(-1).LastDayOfYear());
			}
			else if (text == LocDateToMonth)
			{
				SetRange(now.FirstDayOfMonth(), now);
			}
			else if (text == LocDateToQuarter)
			{
				SetRange(now.FirstDayOfQuarter(), now);
			}
			else if (text == LocDateToWeek)
			{
				SetRange(now.FirstDayOfWeek(), now);
			}
			else if (text == LocDateToYear)
			{
				SetRange(now.FirstDayOfYear(), now);
			}
		}
	}

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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		calendarFrom = new MonthCalendar();
		calendarTo = new MonthCalendar();
		labelFrom = new Label();
		labelTo = new Label();
		listBoxPredefined = new StiListBox();
		((Control)this).SuspendLayout();
		((Control)calendarFrom).Location = new Point(25, 56);
		calendarFrom.MaxSelectionCount = 1;
		((Control)calendarFrom).Name = "calendarFrom";
		((Control)calendarFrom).TabIndex = 0;
		((Control)calendarFrom).SizeChanged += Calendar_SizeChanged;
		((Control)calendarTo).Location = new Point(338, 56);
		calendarTo.MaxSelectionCount = 1;
		((Control)calendarTo).Name = "calendarTo";
		calendarTo.ShowTodayCircle = false;
		((Control)calendarTo).TabIndex = 0;
		((Control)labelFrom).ForeColor = Color.DimGray;
		((Control)labelFrom).Location = new Point(25, 0);
		((Control)labelFrom).Name = "labelFrom";
		((Control)labelFrom).Size = new Size(290, 47);
		((Control)labelFrom).TabIndex = 2;
		((Control)labelFrom).Text = "From";
		labelFrom.TextAlign = (ContentAlignment)32;
		((Control)labelTo).ForeColor = Color.DimGray;
		((Control)labelTo).Location = new Point(338, 0);
		((Control)labelTo).Name = "labelTo";
		((Control)labelTo).Size = new Size(290, 47);
		((Control)labelTo).TabIndex = 2;
		((Control)labelTo).Text = "To";
		labelTo.TextAlign = (ContentAlignment)32;
		((Control)listBoxPredefined).BackColor = Color.White;
		((Control)listBoxPredefined).Font = new Font("Microsoft Sans Serif", 7.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)listBoxPredefined).ForeColor = Color.DimGray;
		((ListControl)listBoxPredefined).FormattingEnabled = true;
		((Control)listBoxPredefined).Location = new Point(669, 18);
		((Control)listBoxPredefined).Name = "listBoxPredefined";
		((Control)listBoxPredefined).Size = new Size(223, 352);
		((Control)listBoxPredefined).TabIndex = 1;
		((ListBox)listBoxPredefined).Click += ListBoxPredefined_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)labelTo);
		((Control)this).Controls.Add((Control)(object)labelFrom);
		((Control)this).Controls.Add((Control)(object)listBoxPredefined);
		((Control)this).Controls.Add((Control)(object)calendarTo);
		((Control)this).Controls.Add((Control)(object)calendarFrom);
		((Control)this).Name = "StiDateRangeControl";
		((Control)this).Size = new Size(912, 373);
		((Control)this).ResumeLayout(false);
	}
}
