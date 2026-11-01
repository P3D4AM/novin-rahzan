using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceUI.Salary.UC;

public class UCMonth : UserControl, IHasContext
{
	private bool _isDisposed;

	private List<Company> _listData;

	private SortableBindingList<UCMonth> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private string _searchString = "";

	private string _dateSH = "";

	private Font _font;

	private List<Holiday> _pHolidays;

	private List<ShiftDetView> _pShiftDetViews;

	private bool _isFirst;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX lblMonth;

	private LabelX lblYear;

	private Panel panel1;

	private LabelX labelX8;

	private LabelX labelX6;

	private LabelX labelX5;

	private LabelX labelX4;

	private LabelX labelX3;

	private LabelX labelX2;

	private LabelX labelX1;

	public string ContextGuid { get; set; }

	public List<UCLblDay> UcLblDays { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	private int MaxDayOfMonth
	{
		get
		{
			if (_dateSH == "")
			{
				return 0;
			}
			int num = Utilities.YearOfDateCalendar(_dateSH);
			int num2 = Utilities.MonthOfDateCalendar(_dateSH);
			return LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonthInt(short.Parse(num2.ToString()), num);
		}
	}

	public UCMonth(string contextGuid, Font font, string pDateSh, List<ShiftDetView> pShiftDetViews, List<Holiday> pHolidays, bool isFirst = false)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		ContextGuid = contextGuid;
		try
		{
			_font = font;
			_pHolidays = pHolidays;
			_pShiftDetViews = pShiftDetViews;
			((Control)this).DoubleBuffered = true;
			_isFirst = isFirst;
			InitializeComponent();
			_dateSH = Calendar.MiladiToCalture(Calendar.StartDayOfPersianMonth(pDateSh));
			((Control)lblYear).Text = _dateSH.Substring(0, 4);
			int monthOfDate = LanguageAndCalendar.Instance.CaltureCalender.GetMonthOfDate(_dateSH);
			((Control)lblMonth).Text = Utilities.GetMonthName(monthOfDate);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده ها", (Control)(object)this, SetShiftOnDays(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCMonth.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCMonth.cs", 72);
		}
	}

	private async Task SetShiftOnDays(CancellationToken token)
	{
		try
		{
			UcLblDays = new List<UCLblDay>();
			int num = -7;
			DateTime curDate = Utilities.CalendarToMiladi(_dateSH);
			int position = (int)(curDate.DayOfWeek + 1);
			if (position >= 7)
			{
				position -= 7;
			}
			int num2 = 0;
			while (num2 < MaxDayOfMonth)
			{
				int x;
				if (position % 7 == 0)
				{
					x = 220;
					num += 27;
					position = 0;
				}
				else
				{
					x = 220 - position * 36;
				}
				if (num < 25 || num > 135)
				{
					num = 25;
				}
				Holiday holiday = _pHolidays.FirstOrDefault((Holiday q) => q.DateM.Equals(curDate));
				List<ShiftDetView> list = _pShiftDetViews.Where((ShiftDetView q) => curDate >= q.DateStartM && curDate <= q.DateFinishM).ToList();
				if (curDate.DayOfWeek != DayOfWeek.Friday)
				{
					list = list.Where((ShiftDetView q) => q.DayOfWeeks[position].Equals(obj: true)).ToList();
				}
				List<ShiftDetView> list2 = Json.FromJson<List<ShiftDetView>>(Json.ToStringJson((object)list, false), "SetShiftOnDays", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\UC\\UCMonth.cs", 105);
				List<ShiftCodeing> list3 = new List<ShiftCodeing>();
				foreach (ShiftDetView item in list2)
				{
					ShiftCodeing clsShiftCoding = item.ClsShiftCoding;
					if (curDate.DayOfWeek == DayOfWeek.Friday)
					{
						if (_isFirst && (item.Fritype == ShiftDet.FriDayType.EkhtiyariAndKarkard || item.Fritype == ShiftDet.FriDayType.EkhtiyariAndAdamKarkard))
						{
							continue;
						}
					}
					else if (_isFirst && holiday != null && holiday.StRasmi && !item.WorkOnHolidaysWithOutFriDay)
					{
						continue;
					}
					list3.Add(clsShiftCoding);
				}
				int num3 = num2;
				int y = num;
				DateTime dateM = curDate;
				UCLblDay uCLblDay = new UCLblDay(ContextGuid, _font);
				((Control)uCLblDay).Location = new Point(x, y);
				((Control)uCLblDay).Name = "UCLblDay" + (num3 + 1).ToString("00");
				((Control)uCLblDay).TabIndex = num3 + 1;
				uCLblDay.txtLbl = (num3 + 1).ToString("00");
				uCLblDay.DateM = dateM;
				uCLblDay.tShiftCodeings = list3;
				uCLblDay.Holiday = holiday;
				uCLblDay.workOnHolidayWithOutFriday = list2.Any((ShiftDetView p) => p.WorkOnHolidaysWithOutFriDay);
				uCLblDay.FriDayType = (list2.Any((ShiftDetView p) => p.Fritype == ShiftDet.FriDayType.Elzami) ? ShiftDet.FriDayType.Elzami : ((!list2.Any((ShiftDetView p) => p.Fritype == ShiftDet.FriDayType.EkhtiyariAndKarkard)) ? ShiftDet.FriDayType.EkhtiyariAndAdamKarkard : ShiftDet.FriDayType.EkhtiyariAndKarkard));
				UCLblDay uCLblDay2 = uCLblDay;
				uCLblDay2.SetShiftToolTip(holiday);
				UcLblDays.Add(uCLblDay2);
				curDate = curDate.AddDays(1.0);
				num2++;
				position++;
			}
			foreach (UCLblDay ucLblDay in UcLblDays)
			{
				((Control)panel1).Controls.Add((Control)(object)ucLblDay);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		panel1 = new Panel();
		labelX8 = new LabelX();
		labelX6 = new LabelX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		lblMonth = new LabelX();
		lblYear = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)panel1);
		((Control)panelEx1).Controls.Add((Control)(object)lblMonth);
		((Control)panelEx1).Controls.Add((Control)(object)lblYear);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(252, 186);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)panel1).Controls.Add((Control)(object)labelX8);
		((Control)panel1).Controls.Add((Control)(object)labelX6);
		((Control)panel1).Controls.Add((Control)(object)labelX5);
		((Control)panel1).Controls.Add((Control)(object)labelX4);
		((Control)panel1).Controls.Add((Control)(object)labelX3);
		((Control)panel1).Controls.Add((Control)(object)labelX2);
		((Control)panel1).Controls.Add((Control)(object)labelX1);
		((Control)panel1).Location = new Point(0, 26);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(251, 160);
		((Control)panel1).TabIndex = 2;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(40, 3);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(30, 16);
		((Control)labelX8).TabIndex = 1;
		((Control)labelX8).Text = "پنج";
		labelX8.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(76, 3);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(30, 16);
		((Control)labelX6).TabIndex = 1;
		((Control)labelX6).Text = "چهار";
		labelX6.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(3, 3);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(30, 16);
		((Control)labelX5).TabIndex = 1;
		((Control)labelX5).Text = "جمعه";
		labelX5.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(112, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(30, 16);
		((Control)labelX4).TabIndex = 1;
		((Control)labelX4).Text = "سه";
		labelX4.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(148, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(30, 16);
		((Control)labelX3).TabIndex = 1;
		((Control)labelX3).Text = "دو";
		labelX3.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(184, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(30, 16);
		((Control)labelX2).TabIndex = 1;
		((Control)labelX2).Text = "یک";
		labelX2.TextAlignment = (StringAlignment)1;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(220, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(30, 16);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "شنبه";
		labelX1.TextAlignment = (StringAlignment)1;
		((Control)lblMonth).Anchor = (AnchorStyles)9;
		((Control)lblMonth).BackColor = Color.Transparent;
		((BaseItemControl)lblMonth).BackgroundStyle.Class = "";
		((BaseItemControl)lblMonth).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMonth).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblMonth).ForeColor = Color.Maroon;
		((Control)lblMonth).Location = new Point(-8, 0);
		((Control)lblMonth).Name = "lblMonth";
		((Control)lblMonth).Size = new Size(160, 23);
		((Control)lblMonth).TabIndex = 1;
		((Control)lblMonth).Text = "ماه :";
		((Control)lblYear).Anchor = (AnchorStyles)9;
		((Control)lblYear).BackColor = Color.Transparent;
		((BaseItemControl)lblYear).BackgroundStyle.Class = "";
		((BaseItemControl)lblYear).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblYear).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblYear).ForeColor = Color.Maroon;
		((Control)lblYear).Location = new Point(168, 0);
		((Control)lblYear).Name = "lblYear";
		((Control)lblYear).Size = new Size(75, 23);
		((Control)lblYear).TabIndex = 0;
		((Control)lblYear).Text = "سال :";
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).Name = "UCMonth";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(252, 186);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
