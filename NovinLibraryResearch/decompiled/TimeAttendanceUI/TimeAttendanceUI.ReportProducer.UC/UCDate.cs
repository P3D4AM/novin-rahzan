using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCDate : UserControl
{
	public enum FilterType : short
	{
		None,
		All,
		Between,
		Today,
		InDate
	}

	public enum DurationDate
	{
		BeginToEndOfMonth,
		BeginToCurrentDayOfMonth
	}

	private readonly DateTime _minDateM = DateTime.Now.AddYears(-10);

	private bool _enable = true;

	private DateTime _dateM;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx PanelDateMain;

	private Panel PanelDateBetween;

	private Label Label1;

	private Label Label2;

	private RadioButton RBToday;

	private RadioButton RBInDate;

	private Label LBLToday;

	private RadioButton RBAllGardesh;

	private RadioButton RBTarikhBetween;

	private DateBoxFa TXTInDate;

	private DateBoxFa TXTDate2;

	private DateBoxFa TXTDate1;

	public bool EnalbleControl
	{
		get
		{
			_enable = ucPanel1.Expand;
			return _enable;
		}
		set
		{
			try
			{
				_enable = value;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 37);
			}
		}
	}

	public DateTime DateM1
	{
		get
		{
			if (RBTarikhBetween.Checked)
			{
				return TXTDate1.MiladiDate.Date;
			}
			if (RBInDate.Checked)
			{
				return TXTInDate.MiladiDate.Date;
			}
			if (!RBToday.Checked)
			{
				return _minDateM;
			}
			return DateTime.Now.Date;
		}
	}

	public DateTime DateM2
	{
		get
		{
			if (RBTarikhBetween.Checked)
			{
				return TXTDate2.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
			}
			if (RBInDate.Checked)
			{
				return TXTInDate.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);
			}
			if (!RBToday.Checked)
			{
				return DateTime.Now.AddYears(10);
			}
			return DateTime.Now.Date.AddDays(1.0).AddSeconds(-1.0);
		}
	}

	public string DateSh1 => ((Control)TXTDate1).Text;

	public string DateSh2 => ((Control)TXTDate2).Text;

	public FilterType SelectionType
	{
		get
		{
			if (RBAllGardesh.Checked)
			{
				return FilterType.All;
			}
			if (RBInDate.Checked)
			{
				return FilterType.InDate;
			}
			if (!RBToday.Checked)
			{
				return FilterType.Between;
			}
			return FilterType.Today;
		}
	}

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "تاریخ" : "تاریخ - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 50);
		}
	}

	public UCDate()
	{
		try
		{
			InitializeComponent();
			_dateM = DateTime.Now;
			((Control)ucPanel1.panelMain).Controls.Add((Control)(object)PanelDateMain);
			((Control)PanelDateMain).Top = 0;
			InitPanels();
			InitDate(DateTime.Now);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 67);
		}
	}

	public void InitDate(DateTime date1, DurationDate durationDate = DurationDate.BeginToCurrentDayOfMonth)
	{
		_dateM = date1;
		string text = Calendar.MiladiToCalture(_dateM);
		((Control)TXTDate1).Text = Calendar.MiladiToCalture(LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth(text, "00:00:00"));
		((Control)TXTDate2).Text = Calendar.MiladiToCalture(LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonth(text, "23:59:59"));
		((Control)LBLToday).Text = LanguageAndCalendar.Instance.CaltureCalender.Today();
	}

	public void GotoMonth(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected I4, but got Unknown
		try
		{
			if (!(((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123)))
			{
				return;
			}
			string text = Calendar.MiladiToCalture(_dateM);
			int num = e.KeyCode - 112 + 1;
			int num2 = Utilities.YearOfDateCalendar(text);
			Calendar.GetDayOfDateSh(text);
			string text2 = Calendar.MiladiToCalture(DateTime.Now);
			int num3 = Utilities.MonthOfDateCalendar(text2);
			int num4 = Utilities.YearOfDateCalendar(text2);
			if (RBInDate.Checked && ((Control)TXTInDate).Text.Length == 10)
			{
				((Control)TXTInDate).Text = $"{num2}/{num:00}/{((Control)TXTInDate).Text.Substring(8, 2)}";
			}
			else if (RBTarikhBetween.Checked)
			{
				((Control)TXTDate1).Text = $"{num2}/{num:00}/01";
				if (num2 == num4 && num == num3)
				{
					((Control)TXTDate2).Text = text2;
				}
				else
				{
					((Control)TXTDate2).Text = Calendar.MiladiToCalture(Utilities.EndDayOfMonthtCalendar(((Control)TXTDate1).Text, "23:59:59"));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GotoMonth", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 151);
		}
	}

	private void UCDate_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Size = ((Control)ucPanel1).Size;
			TXTInDate.MiladiDate = _dateM;
			RBTarikhBetween.Checked = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UCDate_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 164);
		}
	}

	public void VisibleRBAllGardesh(bool status)
	{
		try
		{
			((Control)RBAllGardesh).Visible = status;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "VisibleRBAllGardesh", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 175);
		}
	}

	public void SetDateSh1(string dateSh)
	{
		try
		{
			if (SelectionType == FilterType.InDate)
			{
				((Control)TXTInDate).Text = dateSh;
			}
			else
			{
				((Control)TXTDate1).Text = dateSh;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetDateSh1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 190);
		}
	}

	public void SetDateSh2(string dateSh)
	{
		((Control)TXTDate2).Text = dateSh;
	}

	private void RadioButtons_CheckedChanged(object sender, EventArgs e)
	{
		InitPanels();
	}

	private void InitPanels()
	{
		try
		{
			DateBoxFa tXTInDate = TXTInDate;
			bool enabled = (((Control)PanelDateBetween).Enabled = false);
			((Control)tXTInDate).Enabled = enabled;
			if (RBInDate.Checked)
			{
				((Control)TXTInDate).Enabled = true;
			}
			else if (RBTarikhBetween != null && RBTarikhBetween.Checked)
			{
				((Control)PanelDateBetween).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitPanels", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 214);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Size = ((Control)ucPanel1).Size;
			EnalbleControl = EnalbleControl;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucPanel1_Resize", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCDate.cs", 226);
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Expected O, but got Unknown
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbd: Expected O, but got Unknown
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Expected O, but got Unknown
		PanelDateMain = new PanelEx();
		TXTInDate = new DateBoxFa();
		PanelDateBetween = new Panel();
		TXTDate2 = new DateBoxFa();
		TXTDate1 = new DateBoxFa();
		Label1 = new Label();
		Label2 = new Label();
		RBToday = new RadioButton();
		RBInDate = new RadioButton();
		LBLToday = new Label();
		RBAllGardesh = new RadioButton();
		RBTarikhBetween = new RadioButton();
		ucPanel1 = new UCPanel();
		((Control)PanelDateMain).SuspendLayout();
		((Control)PanelDateBetween).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)PanelDateMain).Anchor = (AnchorStyles)15;
		PanelDateMain.CanvasColor = SystemColors.Control;
		PanelDateMain.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelDateMain).Controls.Add((Control)(object)TXTInDate);
		((Control)PanelDateMain).Controls.Add((Control)(object)PanelDateBetween);
		((Control)PanelDateMain).Controls.Add((Control)(object)RBToday);
		((Control)PanelDateMain).Controls.Add((Control)(object)RBInDate);
		((Control)PanelDateMain).Controls.Add((Control)(object)LBLToday);
		((Control)PanelDateMain).Controls.Add((Control)(object)RBAllGardesh);
		((Control)PanelDateMain).Controls.Add((Control)(object)RBTarikhBetween);
		PanelDateMain.DisabledBackColor = Color.Empty;
		((Control)PanelDateMain).Location = new Point(1, 30);
		((Control)PanelDateMain).Name = "PanelDateMain";
		((Control)PanelDateMain).Size = new Size(599, 170);
		PanelDateMain.Style.Alignment = (StringAlignment)1;
		PanelDateMain.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelDateMain.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelDateMain.Style.Border = (eBorderType)1;
		PanelDateMain.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelDateMain.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelDateMain.Style.GradientAngle = 90;
		((Control)PanelDateMain).TabIndex = 26;
		TXTInDate.AllowNull = false;
		((Control)TXTInDate).Anchor = (AnchorStyles)9;
		TXTInDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTInDate).Border.Class = "TextBoxBorder";
		((TextBoxX)TXTInDate).Border.CornerType = (eCornerType)1;
		TXTInDate.EnterToTab = true;
		TXTInDate.EscToClose = true;
		TXTInDate.FarsyDateStyle = (ENFarsyDateStyle)0;
		TXTInDate.FocusBackColor = Color.Yellow;
		TXTInDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTInDate.FocusForeColor = Color.Blue;
		TXTInDate.FocusTextSelect = true;
		((Control)TXTInDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTInDate).Location = new Point(247, 48);
		TXTInDate.MessageEmptyShowDialog = false;
		TXTInDate.MessegeEmpty = "";
		TXTInDate.MessegeEmptyInFormRight = true;
		TXTInDate.MessegeEmptyShowInForm = false;
		TXTInDate.MiladiDate = new DateTime(2024, 7, 31, 8, 22, 54, 928);
		((Control)TXTInDate).Name = "TXTInDate";
		((Control)TXTInDate).RightToLeft = (RightToLeft)0;
		TXTInDate.ShowNameOfDayOnToolTip = false;
		TXTInDate.ShowToolTipText = false;
		((Control)TXTInDate).Size = new Size(116, 26);
		((Control)TXTInDate).TabIndex = 27;
		((Control)TXTInDate).Text = "30/07/2024";
		((TextBox)TXTInDate).TextAlign = (HorizontalAlignment)2;
		TXTInDate.TootTipDelay = 500;
		((Control)PanelDateBetween).Anchor = (AnchorStyles)9;
		((Control)PanelDateBetween).BackColor = Color.Transparent;
		((Control)PanelDateBetween).Controls.Add((Control)(object)TXTDate2);
		((Control)PanelDateBetween).Controls.Add((Control)(object)TXTDate1);
		((Control)PanelDateBetween).Controls.Add((Control)(object)Label1);
		((Control)PanelDateBetween).Controls.Add((Control)(object)Label2);
		((Control)PanelDateBetween).Enabled = false;
		((Control)PanelDateBetween).Location = new Point(24, 78);
		((Control)PanelDateBetween).Name = "PanelDateBetween";
		((Control)PanelDateBetween).Size = new Size(421, 28);
		((Control)PanelDateBetween).TabIndex = 23;
		TXTDate2.AllowNull = false;
		TXTDate2.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTDate2).Border.Class = "TextBoxBorder";
		((TextBoxX)TXTDate2).Border.CornerType = (eCornerType)1;
		TXTDate2.EnterToTab = true;
		TXTDate2.EscToClose = true;
		TXTDate2.FarsyDateStyle = (ENFarsyDateStyle)0;
		TXTDate2.FocusBackColor = Color.Yellow;
		TXTDate2.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDate2.FocusForeColor = Color.Blue;
		TXTDate2.FocusTextSelect = true;
		((Control)TXTDate2).Font = new Font("Times New Roman", 12f);
		((Control)TXTDate2).Location = new Point(6, 1);
		TXTDate2.MessageEmptyShowDialog = false;
		TXTDate2.MessegeEmpty = "";
		TXTDate2.MessegeEmptyInFormRight = true;
		TXTDate2.MessegeEmptyShowInForm = false;
		TXTDate2.MiladiDate = new DateTime(2024, 7, 31, 8, 22, 54, 932);
		((Control)TXTDate2).Name = "TXTDate2";
		((Control)TXTDate2).RightToLeft = (RightToLeft)0;
		TXTDate2.ShowNameOfDayOnToolTip = false;
		TXTDate2.ShowToolTipText = false;
		((Control)TXTDate2).Size = new Size(116, 26);
		((Control)TXTDate2).TabIndex = 26;
		((Control)TXTDate2).Text = "30/07/2024";
		((TextBox)TXTDate2).TextAlign = (HorizontalAlignment)2;
		TXTDate2.TootTipDelay = 500;
		TXTDate1.AllowNull = false;
		TXTDate1.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTDate1).Border.Class = "TextBoxBorder";
		((TextBoxX)TXTDate1).Border.CornerType = (eCornerType)1;
		TXTDate1.EnterToTab = true;
		TXTDate1.EscToClose = true;
		TXTDate1.FarsyDateStyle = (ENFarsyDateStyle)0;
		TXTDate1.FocusBackColor = Color.Yellow;
		TXTDate1.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDate1.FocusForeColor = Color.Blue;
		TXTDate1.FocusTextSelect = true;
		((Control)TXTDate1).Font = new Font("Times New Roman", 12f);
		((Control)TXTDate1).Location = new Point(224, 1);
		TXTDate1.MessageEmptyShowDialog = false;
		TXTDate1.MessegeEmpty = "";
		TXTDate1.MessegeEmptyInFormRight = true;
		TXTDate1.MessegeEmptyShowInForm = false;
		TXTDate1.MiladiDate = new DateTime(2024, 7, 31, 8, 22, 54, 934);
		((Control)TXTDate1).Name = "TXTDate1";
		((Control)TXTDate1).RightToLeft = (RightToLeft)0;
		TXTDate1.ShowNameOfDayOnToolTip = false;
		TXTDate1.ShowToolTipText = false;
		((Control)TXTDate1).Size = new Size(116, 26);
		((Control)TXTDate1).TabIndex = 25;
		((Control)TXTDate1).Text = "30/07/2024";
		((TextBox)TXTDate1).TextAlign = (HorizontalAlignment)2;
		TXTDate1.TootTipDelay = 500;
		((Control)Label1).Location = new Point(345, 3);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(58, 22);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "از:";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)Label2).Location = new Point(128, 3);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(54, 22);
		((Control)Label2).TabIndex = 2;
		((Control)Label2).Text = "تا:";
		Label2.TextAlign = (ContentAlignment)64;
		((Control)RBToday).Anchor = (AnchorStyles)9;
		((Control)RBToday).BackColor = Color.Transparent;
		((Control)RBToday).ForeColor = Color.Black;
		((Control)RBToday).Location = new Point(470, 15);
		((Control)RBToday).Name = "RBToday";
		((Control)RBToday).Size = new Size(110, 28);
		((Control)RBToday).TabIndex = 17;
		((Control)RBToday).Text = "[امروز]";
		((ButtonBase)RBToday).UseVisualStyleBackColor = false;
		RBToday.CheckedChanged += RadioButtons_CheckedChanged;
		((Control)RBInDate).Anchor = (AnchorStyles)9;
		((Control)RBInDate).BackColor = Color.Transparent;
		((Control)RBInDate).ForeColor = Color.Black;
		((Control)RBInDate).Location = new Point(470, 47);
		((Control)RBInDate).Name = "RBInDate";
		((Control)RBInDate).Size = new Size(110, 28);
		((Control)RBInDate).TabIndex = 19;
		((Control)RBInDate).Text = "[تاریخ]";
		((ButtonBase)RBInDate).UseVisualStyleBackColor = false;
		RBInDate.CheckedChanged += RadioButtons_CheckedChanged;
		((Control)LBLToday).Anchor = (AnchorStyles)9;
		((Control)LBLToday).BackColor = Color.Transparent;
		((Control)LBLToday).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLToday).ForeColor = Color.FromArgb(192, 0, 0);
		((Control)LBLToday).Location = new Point(24, 18);
		((Control)LBLToday).Name = "LBLToday";
		((Control)LBLToday).Size = new Size(449, 22);
		((Control)LBLToday).TabIndex = 24;
		((Control)LBLToday).Text = "------";
		LBLToday.TextAlign = (ContentAlignment)32;
		((Control)RBAllGardesh).Anchor = (AnchorStyles)9;
		((Control)RBAllGardesh).BackColor = Color.Transparent;
		((Control)RBAllGardesh).ForeColor = Color.Black;
		((Control)RBAllGardesh).Location = new Point(441, 107);
		((Control)RBAllGardesh).Name = "RBAllGardesh";
		((Control)RBAllGardesh).Size = new Size(139, 28);
		((Control)RBAllGardesh).TabIndex = 21;
		((Control)RBAllGardesh).Text = "[همه تاریخها]";
		((ButtonBase)RBAllGardesh).UseVisualStyleBackColor = false;
		RBAllGardesh.CheckedChanged += RadioButtons_CheckedChanged;
		((Control)RBTarikhBetween).Anchor = (AnchorStyles)9;
		((Control)RBTarikhBetween).BackColor = Color.Transparent;
		RBTarikhBetween.Checked = true;
		((Control)RBTarikhBetween).ForeColor = Color.Black;
		((Control)RBTarikhBetween).Location = new Point(470, 78);
		((Control)RBTarikhBetween).Name = "RBTarikhBetween";
		((Control)RBTarikhBetween).Size = new Size(110, 28);
		((Control)RBTarikhBetween).TabIndex = 20;
		RBTarikhBetween.TabStop = true;
		((Control)RBTarikhBetween).Text = "[بین دو تاریخ]";
		((ButtonBase)RBTarikhBetween).UseVisualStyleBackColor = false;
		RBTarikhBetween.CheckedChanged += RadioButtons_CheckedChanged;
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).MaximumSize = new Size(600, 400);
		((Control)ucPanel1).MinimumSize = new Size(600, 10);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 200);
		((Control)ucPanel1).TabIndex = 25;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelDateMain);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 200);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCDate";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(600, 200);
		((UserControl)this).Load += UCDate_Load;
		((Control)PanelDateMain).ResumeLayout(false);
		((Control)PanelDateBetween).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
