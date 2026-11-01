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

public class FrmClubContractMonthly : FormExtention, IClubForms
{
	private readonly TimeAttendanceDAL.Dal.FLRClub.Club _club;

	private IClubForms _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX8;

	internal LabelX labelX10;

	internal DateBoxFa TxtEndDate;

	internal DateBoxFa TXTStartDate;

	public FrmClubContractMonthly(string contextGuid, TimeAttendanceDAL.Dal.FLRClub.Club club)
		: base(contextGuid, true, true)
	{
		try
		{
			InitializeComponent();
			_club = club;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 21);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 34);
		}
	}

	public void FillFrmFields()
	{
		try
		{
			((Control)TXTStartDate).Text = Calendar.MiladiToCalture(_club.ContractStartDate);
			((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_club.ContractEndDate);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFrmFields", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 49);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NextPage", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 61);
		}
		return null;
	}

	private void TXTStartDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_club.ContractStartDate = TXTStartDate.MiladiDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTStartDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 74);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtEndDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Club\\FrmClubContractMonthly.cs", 86);
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
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		labelX8 = new LabelX();
		labelX10 = new LabelX();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
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
		((Control)groupPanel4).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX10);
		((Control)groupPanel4).Controls.Add((Control)(object)TxtEndDate);
		((Control)groupPanel4).Controls.Add((Control)(object)TXTStartDate);
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
		((Control)groupPanel4).Text = "قرارداد ماهیانه";
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(429, 101);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(138, 23);
		((Control)labelX8).TabIndex = 10;
		((Control)labelX8).Text = "تاریخ پایان قرارداد:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(429, 58);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(138, 23);
		((Control)labelX10).TabIndex = 8;
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
		((Control)TxtEndDate).Location = new Point(272, 100);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2022, 6, 27, 3, 44, 51, 18);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		TxtEndDate.ShowNameOfDayOnToolTip = false;
		TxtEndDate.ShowToolTipText = false;
		((Control)TxtEndDate).Size = new Size(151, 20);
		((Control)TxtEndDate).TabIndex = 11;
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
		((Control)TXTStartDate).Location = new Point(272, 62);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2022, 6, 27, 3, 44, 51, 18);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(151, 20);
		((Control)TXTStartDate).TabIndex = 9;
		((Control)TXTStartDate).Text = "26/06/2022";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		((Control)TXTStartDate).TextChanged += TXTStartDate_TextChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmClubContractMonthly";
		((Control)this).Text = "FrmCheckOutVacation";
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
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
