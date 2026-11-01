using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceUI.Salary.UC;

namespace TimeAttendanceUI.Salary.Shift;

public class FrmCrtShiftCircle1 : FormExtention
{
	public ShiftDet.FriDayType FriDayType = ShiftDet.FriDayType.Elzami;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel7;

	internal DateBoxFa TxtFinishDate;

	internal DateBoxFa TXTStartDate;

	private LabelX labelX1;

	private TextBoxX TxtDesc;

	private GroupPanel groupPanelSelTaradod;

	private LabelX labelX2;

	private LabelX labelX39;

	private NumericUpDown Hour1;

	private LabelX labelX3;

	private LabelX labelX4;

	private FlowLayoutPanel flowLayoutPanel1;

	private UCHelpTypeShift ucHelpTypeShift3;

	private CheckBoxX ChbSTTatili;

	private LabelX labelX6;

	private RadioButton RbMustWork;

	private RadioButton RbOptionalNotKarkerd;

	private RadioButton RbOptionalKarkerd;

	public string Desc => Text.FixString(((Control)TxtDesc).Text);

	public DateTime DStartDate => Utilities.CalendarToMiladi(((Control)TXTStartDate).Text);

	public DateTime DFinishDate => Utilities.CalendarToMiladi(((Control)TxtFinishDate).Text);

	public int DayofCircle => Utilities.ParseToInt(Hour1.Value.ToString(), 0);

	public bool WorkOnHaliday => ChbSTTatili.Checked;

	public FrmCrtShiftCircle1(string contextGuid)
		: base(contextGuid, true, true)
	{
		string textTr = "شیفت چرخشی";
		Task.Run(async delegate
		{
			textTr = Utilities.GetTranslate("[شیفت چرخشی]");
		});
		InitializeComponent();
		try
		{
			DateTime now = DateTime.Now;
			string text = Utilities.YearOfDateCalendar(Calendar.MiladiToCalture(now)) + "/01/01";
			((Control)TXTStartDate).Text = text;
			now = Utilities.CalendarToMiladi(text);
			((Control)TxtFinishDate).Text = Calendar.MiladiToCalture(now.AddYears(1));
			((Control)TXTStartDate).Focus();
			((Control)TxtDesc).Text = textTr;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle1.cs", 41);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((BaseForm)this).NoCheckFormContentsChanged = true;
			((Control)base.statusStrip1).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle1.cs", 54);
		}
	}

	private void TxtDesc_Enter(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(((Control)TxtDesc).Text))
			{
				((Control)TxtDesc).Text = Utilities.GetTranslate("[شیفت چرخشی دوره چرخش] " + ((Control)Hour1).Text + " [روزه] : ");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtDesc_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle1.cs", 67);
		}
	}

	private void RbFriDayType_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbOptionalKarkerd.Checked)
			{
				FriDayType = ShiftDet.FriDayType.EkhtiyariAndKarkard;
			}
			else if (RbOptionalNotKarkerd.Checked)
			{
				FriDayType = ShiftDet.FriDayType.EkhtiyariAndAdamKarkard;
			}
			else if (RbMustWork.Checked)
			{
				FriDayType = ShiftDet.FriDayType.Elzami;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbFriDayType_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Shift\\FRMCrtShiftCircle1.cs", 84);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Expected O, but got Unknown
		//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e90: Expected O, but got Unknown
		//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdf: Expected O, but got Unknown
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1015: Expected O, but got Unknown
		//IL_1299: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a3: Expected O, but got Unknown
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		PanelEx1 = new PanelEx();
		groupPanel7 = new GroupPanel();
		labelX6 = new LabelX();
		RbMustWork = new RadioButton();
		RbOptionalNotKarkerd = new RadioButton();
		RbOptionalKarkerd = new RadioButton();
		ChbSTTatili = new CheckBoxX();
		flowLayoutPanel1 = new FlowLayoutPanel();
		labelX1 = new LabelX();
		TxtDesc = new TextBoxX();
		groupPanelSelTaradod = new GroupPanel();
		labelX3 = new LabelX();
		labelX4 = new LabelX();
		TxtFinishDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		Hour1 = new NumericUpDown();
		labelX2 = new LabelX();
		labelX39 = new LabelX();
		ucHelpTypeShift3 = new UCHelpTypeShift();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((Control)flowLayoutPanel1).SuspendLayout();
		((Control)groupPanelSelTaradod).SuspendLayout();
		((ISupportInitialize)Hour1).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel7);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 478);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 2;
		((Control)groupPanel7).Anchor = (AnchorStyles)15;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel7).Controls.Add((Control)(object)RbMustWork);
		((Control)groupPanel7).Controls.Add((Control)(object)RbOptionalNotKarkerd);
		((Control)groupPanel7).Controls.Add((Control)(object)RbOptionalKarkerd);
		((Control)groupPanel7).Controls.Add((Control)(object)ChbSTTatili);
		((Control)groupPanel7).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtDesc);
		((Control)groupPanel7).Controls.Add((Control)(object)groupPanelSelTaradod);
		((Control)groupPanel7).Location = new Point(14, 12);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(554, 454);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.Class = "";
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.Class = "";
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.Class = "";
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 1;
		((Control)labelX6).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).ForeColor = Color.DarkRed;
		((Control)labelX6).Location = new Point(429, 323);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(68, 24);
		((Control)labelX6).TabIndex = 19;
		((Control)labelX6).Text = "[[روزهای جمعه]] :";
		((Control)RbMustWork).Anchor = (AnchorStyles)10;
		((Control)RbMustWork).ForeColor = SystemColors.HotTrack;
		((Control)RbMustWork).Location = new Point(83, 354);
		((Control)RbMustWork).Name = "RbMustWork";
		((Control)RbMustWork).Size = new Size(332, 24);
		((Control)RbMustWork).TabIndex = 22;
		((Control)RbMustWork).Text = "[حضور الزامی است و در صورت عدم حضور غیبت محسوب میشود]";
		((ButtonBase)RbMustWork).UseVisualStyleBackColor = true;
		RbMustWork.CheckedChanged += RbFriDayType_CheckedChanged;
		((Control)RbOptionalNotKarkerd).Anchor = (AnchorStyles)10;
		((Control)RbOptionalNotKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalNotKarkerd).Location = new Point(87, 323);
		((Control)RbOptionalNotKarkerd).Name = "RbOptionalNotKarkerd";
		((Control)RbOptionalNotKarkerd).Size = new Size(328, 24);
		((Control)RbOptionalNotKarkerd).TabIndex = 21;
		((Control)RbOptionalNotKarkerd).Text = "[حضور اختیاری در جمعه ها و عدم محاسبه کارکرد]";
		((ButtonBase)RbOptionalNotKarkerd).UseVisualStyleBackColor = true;
		RbOptionalNotKarkerd.CheckedChanged += RbFriDayType_CheckedChanged;
		((Control)RbOptionalKarkerd).Anchor = (AnchorStyles)10;
		RbOptionalKarkerd.Checked = true;
		((Control)RbOptionalKarkerd).ForeColor = SystemColors.HotTrack;
		((Control)RbOptionalKarkerd).Location = new Point(71, 294);
		((Control)RbOptionalKarkerd).Name = "RbOptionalKarkerd";
		((Control)RbOptionalKarkerd).Size = new Size(344, 24);
		((Control)RbOptionalKarkerd).TabIndex = 20;
		RbOptionalKarkerd.TabStop = true;
		((Control)RbOptionalKarkerd).Text = "[حضور اختیاری در جمعه ها و در صورت عدم حضور کارکرد محاسبه میشود]";
		((ButtonBase)RbOptionalKarkerd).UseVisualStyleBackColor = true;
		RbOptionalKarkerd.CheckedChanged += RbFriDayType_CheckedChanged;
		((Control)ChbSTTatili).Anchor = (AnchorStyles)10;
		((BaseItemControl)ChbSTTatili).BackgroundStyle.Class = "";
		((BaseItemControl)ChbSTTatili).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSTTatili).Location = new Point(125, 248);
		((Control)ChbSTTatili).Name = "ChbSTTatili";
		((Control)ChbSTTatili).Size = new Size(293, 33);
		((BaseItemControl)ChbSTTatili).Style = (eDotNetBarStyle)9;
		((Control)ChbSTTatili).TabIndex = 17;
		((Control)ChbSTTatili).Text = "[در نظر گرفتن تعطیلات رسمی بعنوان روزکاری]";
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)14;
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucHelpTypeShift3);
		((Control)flowLayoutPanel1).Location = new Point(23, 324);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).RightToLeft = (RightToLeft)0;
		((Control)flowLayoutPanel1).Size = new Size(54, 114);
		((Control)flowLayoutPanel1).TabIndex = 16;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(491, 127);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(37, 31);
		((Control)labelX1).TabIndex = 9;
		((Control)labelX1).Text = "[شرح] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(23, 127);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(461, 115);
		((Control)TxtDesc).TabIndex = 10;
		((Control)TxtDesc).Enter += TxtDesc_Enter;
		((Control)groupPanelSelTaradod).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanelSelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelSelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX3);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX4);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)TxtFinishDate);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)TXTStartDate);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)Hour1);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX2);
		((Control)groupPanelSelTaradod).Controls.Add((Control)(object)labelX39);
		((Control)groupPanelSelTaradod).Location = new Point(23, 3);
		((Control)groupPanelSelTaradod).Name = "groupPanelSelTaradod";
		((Control)groupPanelSelTaradod).Size = new Size(505, 121);
		((PanelControl)groupPanelSelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelSelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelSelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelSelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelSelTaradod).Style.Class = "";
		((PanelControl)groupPanelSelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelSelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelSelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelSelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelSelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelSelTaradod).StyleMouseDown.Class = "";
		((PanelControl)groupPanelSelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelSelTaradod).StyleMouseOver.Class = "";
		((PanelControl)groupPanelSelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelSelTaradod).TabIndex = 0;
		((Control)groupPanelSelTaradod).Text = "[دوره چرخش را انتخاب نمائید]";
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(136, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(83, 31);
		((Control)labelX3).TabIndex = 36;
		((Control)labelX3).Text = "[تاریخ پایان]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(401, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(70, 31);
		((Control)labelX4).TabIndex = 35;
		((Control)labelX4).Text = "[تاریخ شروع] :";
		labelX4.TextAlignment = (StringAlignment)2;
		TxtFinishDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtFinishDate).Border.Class = "";
		((TextBoxX)TxtFinishDate).Border.CornerType = (eCornerType)1;
		TxtFinishDate.EnterToTab = true;
		TxtFinishDate.EscToClose = true;
		TxtFinishDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtFinishDate.FocusBackColor = Color.Yellow;
		TxtFinishDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtFinishDate.FocusForeColor = Color.Blue;
		TxtFinishDate.FocusTextSelect = true;
		((Control)TxtFinishDate).Font = new Font("Times New Roman", 12f);
		((Control)TxtFinishDate).Location = new Point(14, 3);
		TxtFinishDate.MessageEmptyShowDialog = false;
		TxtFinishDate.MessegeEmpty = "";
		TxtFinishDate.MessegeEmptyInFormRight = true;
		TxtFinishDate.MessegeEmptyShowInForm = false;
		TxtFinishDate.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TxtFinishDate).Name = "TxtFinishDate";
		((Control)TxtFinishDate).RightToLeft = (RightToLeft)0;
		((Control)TxtFinishDate).Size = new Size(115, 20);
		((Control)TxtFinishDate).TabIndex = 0;
		((Control)TxtFinishDate).Text = "1391/12/12";
		((TextBox)TxtFinishDate).TextAlign = (HorizontalAlignment)2;
		TxtFinishDate.TootTipDelay = 500;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.Class = "";
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(279, 3);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2013, 3, 2, 0, 0, 0, 0);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		((Control)TXTStartDate).Size = new Size(115, 20);
		((Control)TXTStartDate).TabIndex = 0;
		((Control)TXTStartDate).Text = "1391/12/12";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)Hour1).Location = new Point(279, 40);
		Hour1.Maximum = new decimal(new int[4] { 93, 0, 0, 0 });
		((Control)Hour1).Name = "Hour1";
		((Control)Hour1).Size = new Size(115, 31);
		((Control)Hour1).TabIndex = 34;
		((UpDownBase)Hour1).TextAlign = (HorizontalAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(202, 40);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(70, 31);
		((Control)labelX2).TabIndex = 33;
		((Control)labelX2).Tag = "";
		((Control)labelX2).Text = "[روز است]";
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.Class = "";
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(401, 40);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(70, 31);
		((Control)labelX39).TabIndex = 33;
		((Control)labelX39).Text = "[دوره چرخش]";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)ucHelpTypeShift3).Font = new Font("B Homa", 9.75f);
		((Control)ucHelpTypeShift3).Location = new Point(5, 5);
		((Control)ucHelpTypeShift3).Margin = new Padding(5);
		ucHelpTypeShift3.Model = (TypeShiftEnumType)100;
		((Control)ucHelpTypeShift3).Name = "ucHelpTypeShift3";
		((Control)ucHelpTypeShift3).RightToLeft = (RightToLeft)1;
		((Control)ucHelpTypeShift3).Size = new Size(51, 109);
		((Control)ucHelpTypeShift3).TabIndex = 0;
		((Control)ucHelpTypeShift3).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCrtShiftCircle1";
		((Control)this).Text = "FRMCrtShift1";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((Control)flowLayoutPanel1).ResumeLayout(false);
		((Control)groupPanelSelTaradod).ResumeLayout(false);
		((ISupportInitialize)Hour1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
