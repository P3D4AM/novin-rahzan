using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCTakDate : UserControl
{
	public class Mounth
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}

	private bool _enable = true;

	private short _month;

	private int _year;

	private int _monthDays;

	private DateTime _date1 = DateTime.Now;

	private DateTime _date2 = DateTime.Now;

	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private NumericUpDown numyear;

	private ComboBoxEx CmbMon;

	private LabelX lblMon;

	private LabelX labelX2;

	public bool IsEnabledMounts
	{
		get
		{
			return ((Control)CmbMon).Visible;
		}
		set
		{
			ComboBoxEx cmbMon = CmbMon;
			bool visible = (((Control)lblMon).Visible = value);
			((Control)cmbMon).Visible = visible;
		}
	}

	public DateTime DateM1
	{
		get
		{
			CalcDate();
			return _date1;
		}
		set
		{
			_date1 = value;
		}
	}

	public DateTime DateM2
	{
		get
		{
			CalcDate();
			return _date2;
		}
		set
		{
			_date2 = value;
		}
	}

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
				_enable = ucPanel1.Expand;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDate.cs", 134);
			}
		}
	}

	public UCTakDate()
	{
		try
		{
			InitializeComponent();
			((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
			((Control)panelEx1).Top = 0;
			string[] listOfMonth = LanguageAndCalendar.Instance.CaltureCalender.GetListOfMonth();
			((ListControl)CmbMon).DisplayMember = "Name";
			((ComboBox)CmbMon).DataSource = listOfMonth.Select((string s) => new Mounth
			{
				Name = s
			}).ToList();
			InitDate(DateTime.Now);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDate.cs", 32);
		}
	}

	public void InitDate(DateTime date1)
	{
		string text = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(date1);
		_date1 = date1;
		_month = (short)Utilities.MonthOfDateCalendar(text);
		_year = Utilities.YearOfDateCalendar(text);
		_monthDays = LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonthInt(_month, _year);
		((Control)numyear).Text = _year.ToString();
		((ListControl)CmbMon).SelectedIndex = _month - 1;
		CalcDate();
	}

	public void CalcDate()
	{
		string text = _month.ToString("00");
		try
		{
			_date1 = Utilities.CalendarToMiladi(_year + "/" + text + "/01");
			_monthDays = LanguageAndCalendar.Instance.CaltureCalender.EndDayOfMonthInt(_month, _year);
			_date2 = Utilities.CalendarToMiladi(_year + "/" + text + "/" + _monthDays).AddDays(1.0).AddSeconds(-1.0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CalcDate", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDate.cs", 72);
		}
	}

	private void numyear_ValueChanged(object sender, EventArgs e)
	{
		_year = int.Parse(numyear.Value.ToString());
		CalcDate();
	}

	private void CmbMon_SelectedIndexChanged(object sender, EventArgs e)
	{
		_month = short.Parse((((ListControl)CmbMon).SelectedIndex + 1).ToString());
		CalcDate();
	}

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "تاریخ" : "تاریخ - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDate.cs", 115);
		}
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void UCTakDate_Load(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		RefreshForm();
	}

	public void SetDate(DateTime date)
	{
		string text = Calendar.MiladiToCalture(date);
		_month = (short)Utilities.MonthOfDateCalendar(text);
		_year = Utilities.YearOfDateCalendar(text);
		((Control)numyear).Text = _year.ToString();
		((ListControl)CmbMon).SelectedIndex = _month - 1;
	}

	public void GotoMonth(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected I4, but got Unknown
		if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
		{
			string text = Calendar.MiladiToCalture(DateTime.Now);
			int num = e.KeyCode - 112 + 1;
			Utilities.YearOfDateCalendar(text);
			((ListControl)CmbMon).SelectedIndex = num - 1;
		}
	}

	private void ucPanel1_Load(object sender, EventArgs e)
	{
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
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		numyear = new NumericUpDown();
		CmbMon = new ComboBoxEx();
		lblMon = new LabelX();
		labelX2 = new LabelX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)numyear).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)numyear);
		((Control)panelEx1).Controls.Add((Control)(object)CmbMon);
		((Control)panelEx1).Controls.Add((Control)(object)lblMon);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, 30);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(600, 34);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)numyear).Anchor = (AnchorStyles)9;
		((Control)numyear).Location = new Point(479, 5);
		numyear.Maximum = new decimal(new int[4] { 2000, 0, 0, 0 });
		numyear.Minimum = new decimal(new int[4] { 1300, 0, 0, 0 });
		((Control)numyear).Name = "numyear";
		((Control)numyear).RightToLeft = (RightToLeft)1;
		((Control)numyear).Size = new Size(71, 21);
		((Control)numyear).TabIndex = 83;
		numyear.Value = new decimal(new int[4] { 1300, 0, 0, 0 });
		numyear.ValueChanged += numyear_ValueChanged;
		((Control)CmbMon).Anchor = (AnchorStyles)9;
		((ListControl)CmbMon).DisplayMember = "Text";
		((ComboBox)CmbMon).DrawMode = (DrawMode)1;
		((ComboBox)CmbMon).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbMon).FormattingEnabled = true;
		((ComboBox)CmbMon).ItemHeight = 16;
		((Control)CmbMon).Location = new Point(269, 6);
		((Control)CmbMon).Name = "CmbMon";
		((Control)CmbMon).Size = new Size(114, 22);
		CmbMon.Style = (eDotNetBarStyle)9;
		((Control)CmbMon).TabIndex = 82;
		((ComboBox)CmbMon).SelectedIndexChanged += CmbMon_SelectedIndexChanged;
		((Control)lblMon).Anchor = (AnchorStyles)9;
		((BaseItemControl)lblMon).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMon).Location = new Point(389, 5);
		((Control)lblMon).Name = "lblMon";
		((Control)lblMon).Size = new Size(58, 21);
		((Control)lblMon).TabIndex = 81;
		((Control)lblMon).Text = "[ماه]";
		lblMon.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(556, 5);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(38, 21);
		((Control)labelX2).TabIndex = 80;
		((Control)labelX2).Text = "[سال]";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)ucPanel1).Anchor = (AnchorStyles)13;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).MinimumSize = new Size(600, 10);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 60);
		((Control)ucPanel1).TabIndex = 0;
		((UserControl)ucPanel1).Load += ucPanel1_Load;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 64);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCTakDate";
		((Control)this).Size = new Size(600, 64);
		((UserControl)this).Load += UCTakDate_Load;
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)numyear).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
