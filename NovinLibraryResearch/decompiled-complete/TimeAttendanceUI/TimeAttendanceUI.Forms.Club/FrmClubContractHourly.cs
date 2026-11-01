using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRClub;

namespace TimeAttendanceUI.Forms.Club;

public class FrmClubContractHourly : FormExtention, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private NumericUpDown numericUpDown1;

	internal LabelX labelX1;

	internal LabelX labelX8;

	internal LabelX labelX10;

	internal DateBoxFa TXTStartDate;

	internal DateBoxFa TxtEndDate;

	public FrmClubContractHourly(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_club = club;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 21);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((FormExtention)this).OnLoad(e);
			((Control)base.statusStrip1).Visible = false;
			FillFrmFields();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 34);
		}
	}

	public void FillFrmFields()
	{
		try
		{
			((Control)TXTStartDate).Text = Calendar.MiladiToCalture(_club.ContractStartDate);
			((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_club.ContractEndDate);
			if (_club.ContractHour <= 0m)
			{
				_club.ContractHour = 1m;
			}
			if (_club.ContractHour > 999m)
			{
				_club.ContractHour = 999m;
			}
			numericUpDown1.Value = _club.ContractHour;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFrmFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 51);
		}
	}

	public IClubForms NextPage()
	{
		try
		{
			return _nextPage ?? (_nextPage = new FrmClubAccountInfo(((CacheBaseForm)this).ContextGuid, _club));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 63);
		}
		return null;
	}

	private void numericUpDown1_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractHour = numericUpDown1.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "numericUpDown1_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 77);
		}
	}

	private void TXTStartDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractStartDate = TXTStartDate.MiladiDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTStartDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 88);
		}
	}

	private void TxtEndDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractEndDate = TxtEndDate.MiladiDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtEndDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractHourly.cs", 100);
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
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Expected O, but got Unknown
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		labelX8 = new LabelX();
		labelX10 = new LabelX();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		labelX1 = new LabelX();
		numericUpDown1 = new NumericUpDown();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((ISupportInitialize)numericUpDown1).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 500);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)numericUpDown1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX10);
		((Control)groupPanel4).Controls.Add((Control)(object)TXTStartDate);
		((Control)groupPanel4).Controls.Add((Control)(object)TxtEndDate);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 500);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "قرارداد ساعتی";
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(389, 93);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(138, 23);
		((Control)labelX8).TabIndex = 14;
		((Control)labelX8).Text = "تاریخ پایان قرارداد:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(389, 48);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(138, 23);
		((Control)labelX10).TabIndex = 12;
		((Control)labelX10).Text = "تاریخ شروع قرارداد:";
		labelX10.TextAlignment = (StringAlignment)2;
		TxtEndDate.AllowNull = false;
		TxtEndDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtEndDate).Border.CornerType = (eCornerType)1;
		TxtEndDate.EnterToTab = true;
		TxtEndDate.EscToClose = true;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtEndDate.FocusBackColor = Color.Yellow;
		TxtEndDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtEndDate.FocusForeColor = Color.Blue;
		TxtEndDate.FocusTextSelect = true;
		((Control)TxtEndDate).Font = new Font("Times New Roman", 12f);
		((Control)TxtEndDate).Location = new Point(232, 91);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2022, 6, 26, 17, 36, 26, 93);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		TxtEndDate.ShowNameOfDayOnToolTip = false;
		TxtEndDate.ShowToolTipText = false;
		((Control)TxtEndDate).Size = new Size(151, 20);
		((Control)TxtEndDate).TabIndex = 15;
		((Control)TxtEndDate).Text = "26/06/2022";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		((Control)TxtEndDate).TextChanged += TxtEndDate_TextChanged;
		TXTStartDate.AllowNull = false;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(232, 52);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2022, 6, 26, 17, 36, 26, 93);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(151, 20);
		((Control)TXTStartDate).TabIndex = 13;
		((Control)TXTStartDate).Text = "26/06/2022";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)TXTStartDate).TextChanged += TXTStartDate_TextChanged;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(389, 135);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(138, 23);
		((Control)labelX1).TabIndex = 16;
		((Control)labelX1).Text = "تعداد ساعت:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)numericUpDown1).Location = new Point(233, 130);
		numericUpDown1.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		numericUpDown1.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numericUpDown1).Name = "numericUpDown1";
		((Control)numericUpDown1).Size = new Size(150, 31);
		((Control)numericUpDown1).TabIndex = 17;
		numericUpDown1.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubContractHourly";
		((Control)this).Text = "FrmCheckOutVacation";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((ISupportInitialize)numericUpDown1).EndInit();
		((Control)this).ResumeLayout(false);
	}

	bool IClubForms.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void IClubForms.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void IClubForms.Show()
	{
		((Control)this).Show();
	}
}
