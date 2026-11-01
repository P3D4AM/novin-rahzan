using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Entities;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCTakDay : UserControl
{
	private IContainer components;

	private UCPanel ucPanel1;

	private PanelEx panelEx1;

	private LabelX labelX2;

	private Label LBLToday;

	private DateBoxFa TXTInDate;

	public bool EnalbleControl
	{
		get
		{
			return ucPanel1.Expand;
		}
		set
		{
			try
			{
				ucPanel1.Expand = value;
				RefreshForm();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnalbleControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDay.cs", 47);
			}
		}
	}

	public DateTime DateM => TXTInDate.MiladiDate.Date.AddDays(1.0).AddSeconds(-1.0);

	public UCTakDay()
	{
		InitializeComponent();
		((Control)ucPanel1.panelMain).Controls.Add((Control)(object)panelEx1);
		((Control)panelEx1).Top = 0;
		InitDate(DateTime.Now);
	}

	public void InitDate(DateTime date1)
	{
		TXTInDate.MiladiDate = date1;
		Label lBLToday = LBLToday;
		LanguageAndCalendar instance = LanguageAndCalendar.Instance;
		object text;
		if (instance == null)
		{
			text = null;
		}
		else
		{
			ICalendar caltureCalender = instance.CaltureCalender;
			text = ((caltureCalender != null) ? caltureCalender.MiladitoCalenderText(date1) : null);
		}
		((Control)lBLToday).Text = (string)text;
	}

	private void UCTakDay_Load(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		RefreshForm();
	}

	private void ucPanel1_Resize(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ucPanel1).Size;
		EnalbleControl = EnalbleControl;
	}

	private void RefreshForm()
	{
		try
		{
			ucPanel1.HeaderText2 = (EnalbleControl ? "تاریخ" : "تاریخ - غیر فعال");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\ReportProducer\\UC\\UCTakDay.cs", 60);
		}
	}

	public void SetDate(DateTime Date)
	{
		((Control)TXTInDate).Text = Calendar.MiladiToCalture(Date);
	}

	public void GotoMonth(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (((int)e.KeyCode >= 112) & ((int)e.KeyCode <= 123))
		{
			string text = Calendar.MiladiToCalture(DateTime.Now);
			short num = (short)(e.KeyCode - 112 + 1);
			int yearOfDate = LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(text);
			((Control)TXTInDate).Text = Calendar.MiladiToCalture(LanguageAndCalendar.Instance.CaltureCalender.StartDayOfMonth(num, yearOfDate, "23:59:59"));
		}
	}

	private void TXTInDate_TextChanged_1(object sender, EventArgs e)
	{
		((Control)LBLToday).Text = LanguageAndCalendar.Instance.CaltureCalender.MiladitoCalenderText(TXTInDate.MiladiDate);
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
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		TXTInDate = new DateBoxFa();
		LBLToday = new Label();
		labelX2 = new LabelX();
		ucPanel1 = new UCPanel();
		((Control)panelEx1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)15;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)TXTInDate);
		((Control)panelEx1).Controls.Add((Control)(object)LBLToday);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, 29);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).RightToLeft = (RightToLeft)1;
		((Control)panelEx1).Size = new Size(600, 37);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 2;
		TXTInDate.AllowNull = false;
		((Control)TXTInDate).Anchor = (AnchorStyles)9;
		TXTInDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTInDate).Border.Class = "TextBoxBorder";
		((TextBoxX)TXTInDate).Border.CornerType = (eCornerType)1;
		TXTInDate.EnterToTab = true;
		TXTInDate.EscToClose = true;
		TXTInDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTInDate.FocusBackColor = Color.Yellow;
		TXTInDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTInDate.FocusForeColor = Color.Blue;
		TXTInDate.FocusTextSelect = true;
		((Control)TXTInDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTInDate).Location = new Point(359, 6);
		TXTInDate.MessageEmptyShowDialog = false;
		TXTInDate.MessegeEmpty = "";
		TXTInDate.MessegeEmptyInFormRight = true;
		TXTInDate.MessegeEmptyShowInForm = false;
		TXTInDate.MiladiDate = new DateTime(2024, 1, 12, 0, 23, 11, 84);
		((Control)TXTInDate).Name = "TXTInDate";
		((Control)TXTInDate).RightToLeft = (RightToLeft)0;
		TXTInDate.ShowNameOfDayOnToolTip = false;
		TXTInDate.ShowToolTipText = false;
		((Control)TXTInDate).Size = new Size(100, 26);
		((Control)TXTInDate).TabIndex = 83;
		((Control)TXTInDate).Text = "11/01/2024";
		((TextBox)TXTInDate).TextAlign = (HorizontalAlignment)2;
		TXTInDate.TootTipDelay = 500;
		((Control)TXTInDate).TextChanged += TXTInDate_TextChanged_1;
		((Control)LBLToday).Anchor = (AnchorStyles)13;
		((Control)LBLToday).BackColor = Color.Transparent;
		((Control)LBLToday).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLToday).ForeColor = Color.FromArgb(192, 0, 0);
		((Control)LBLToday).Location = new Point(3, 8);
		((Control)LBLToday).Name = "LBLToday";
		((Control)LBLToday).Size = new Size(350, 21);
		((Control)LBLToday).TabIndex = 82;
		((Control)LBLToday).Text = "-----";
		LBLToday.TextAlign = (ContentAlignment)32;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(524, 8);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(60, 21);
		((Control)labelX2).TabIndex = 81;
		((Control)labelX2).Text = "[تاریخ]";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)ucPanel1).Anchor = (AnchorStyles)15;
		ucPanel1.Expand = true;
		((Control)ucPanel1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		ucPanel1.HeaderText = "";
		ucPanel1.HeaderText2 = "";
		((Control)ucPanel1).Location = new Point(0, 0);
		((Control)ucPanel1).Name = "ucPanel1";
		((Control)ucPanel1).Size = new Size(600, 66);
		((Control)ucPanel1).TabIndex = 0;
		((Control)ucPanel1).Resize += ucPanel1_Resize;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Controls.Add((Control)(object)ucPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 66);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCTakDay";
		((Control)this).Size = new Size(600, 66);
		((UserControl)this).Load += UCTakDay_Load;
		((Control)panelEx1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
