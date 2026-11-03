using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;

namespace FormCustom;

public class FrmBase : CacheBaseForm
{
	private const string helpfile = "helpHozor.chm";

	protected bool IsFormContentsChanged;

	protected bool NoCheckFormContentsChanged;

	private bool _isDisposed;

	private IContainer components;

	public string HelpState { get; set; } = "Main";


	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FrmBase(string context)
		: base(context)
	{
		InitializeComponent();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		try
		{
			((Control)this).OnKeyDown(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmBase.cs", 43);
		}
	}

	public void ShowHelp(Control sender, string pState)
	{
		try
		{
			string text = "";
			if (!File.Exists("helpHozor.chm"))
			{
				return;
			}
			string name = sender.Name;
			if (pState == "Main")
			{
				switch (name)
				{
				case "FRMUserMain":
				case "FRMUsers":
					text = AccessLevelClass.Instance.Users.GetExtentionHelpURLAttr("UsersMain").Text;
					break;
				case "FRMDeviceMain":
				case "FRMDevice":
					text = AccessLevelClass.Instance.Device.GetExtentionHelpURLAttr("DeviceMain").Text;
					break;
				case "FRMCompanyMain":
				case "FRMCompany":
					text = AccessLevelClass.Instance.Company.GetExtentionHelpURLAttr("CompanyMain").Text;
					break;
				case "FRMHolidayMain":
				case "FRMHoliday":
					text = AccessLevelClass.Instance.Vacation.GetExtentionHelpURLAttr("VacationHolidays").Text;
					break;
				case "FRMCrtShift1":
				case "FRMShiftMain":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftMain").Text;
					break;
				case "FRMCrtShiftNormal":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftNormal").Text;
					break;
				case "FRMCrtShiftShenavar":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftShenavar").Text;
					break;
				case "FrmCreateShift":
				case "FRMCrtShiftCircle1":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftCircle").Text;
					break;
				case "FRMCrtShiftCircle2":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftCirclePage2").Text;
					break;
				case "FRMShift":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftUpdate").Text;
					break;
				case "FrmShiftProgrammable":
					text = AccessLevelClass.Instance.Shift.GetExtentionHelpURLAttr("ShiftProgrammable").Text;
					break;
				case "DefMorakhasiTypeMain":
				case "DefMorakhasiType":
					text = AccessLevelClass.Instance.Vacation.GetExtentionHelpURLAttr("VacationDefine").Text;
					break;
				case "FRMPersonel":
				case "FRMSelectPersonel":
					text = AccessLevelClass.Instance.Personnel.GetExtentionHelpURLAttr("PersonnelMain").Text;
					break;
				case "FRMHardwareCode":
					text = AccessLevelClass.Instance.Personnel.GetExtentionHelpURLAttr("PersonnelHardWareCodeCust").Text;
					break;
				case "FrmHesab":
				case "FrmHesabList":
					text = AccessLevelClass.Instance.CodeingHesab.GetExtentionHelpURLAttr("CodeingHesabMain").Text;
					break;
				case "FRMGharardad":
				case "FrmGharardadList":
				case "FRMGharardadDet":
					text = AccessLevelClass.Instance.Gharardad.GetExtentionHelpURLAttr("GharardadMain").Text;
					break;
				case "FrmBaseYearsSalary":
				case "FrmBaseYearsSalaryList":
					text = AccessLevelClass.Instance.BaseYearsSalary.GetExtentionHelpURLAttr("BaseYearsSalaryMain").Text;
					break;
				case "RPTTaradod_Full":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodMain").Text;
					break;
				case "FRMATT_TaradodDet":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodEditManagement").Text;
					break;
				case "FrmGetDecision":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodChangeTypeTaradod").Text;
					break;
				case "RPTTaradod_Full_All":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodReportTaradodPersonnelOnADay").Text;
					break;
				case "RPTManage":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodReportAmarPersonnel").Text;
					break;
				case "FRMMorakhasiDaily":
				case "FRMMorakhasiDailyMain":
					text = AccessLevelClass.Instance.Vacation.GetExtentionHelpURLAttr("VacationMain").Text;
					break;
				case "FrmTypeTaradod":
				case "FrmTypeTaradodList":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodDefine").Text;
					break;
				case "FrmReportMorakhasi":
					text = AccessLevelClass.Instance.Vacation.GetExtentionHelpURLAttr("VacationReport").Text;
					break;
				case "FrmFishHoghogh":
					text = AccessLevelClass.Instance.BillPrice.GetExtentionHelpURLAttr("BillPriceMain").Text;
					break;
				case "FrmFishList":
					text = AccessLevelClass.Instance.BillPrice.GetExtentionHelpURLAttr("BillPriceReport").Text;
					break;
				case "FrmSanad":
				case "FRMSanadMain":
					text = AccessLevelClass.Instance.Accounting.GetExtentionHelpURLAttr("AccountingMain").Text;
					break;
				case "FRMRPTGenMain":
				case "FRMRPTGen":
					text = AccessLevelClass.Instance.Reports.GetExtentionHelpURLAttr("ReportsGozareshSazPoya").Text;
					break;
				case "FrmLogUserList":
				case "FrmLogUser":
					text = AccessLevelClass.Instance.Users.GetExtentionHelpURLAttr("UsersReportLogs").Text;
					break;
				case "FrmRecieveData":
					text = AccessLevelClass.Instance.Taradod.GetExtentionHelpURLAttr("TaradodReciveDataOffLine").Text;
					break;
				case "FRMSetPass":
					text = AccessLevelClass.Instance.SoftwareSetting.GetExtentionHelpURLAttr("ChangePassword").Text;
					break;
				case "FRMSetting":
					text = AccessLevelClass.Instance.SoftwareSetting.GetExtentionHelpURLAttr("SoftwareSettingMain").Text;
					break;
				}
			}
			Help.ShowHelp(sender, "helpHozor.chm", (HelpNavigator)(-2147483647), (object)text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowHelp", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmBase.cs", 186);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		AddControlChanged(((Control)this).Controls);
	}

	protected void AddControlChanged(ControlCollection pobj)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)pobj)
		{
			Control val = item;
			CheckBox val2 = (CheckBox)(object)((val is CheckBox) ? val : null);
			if (val2 != null)
			{
				val2.CheckedChanged += c_ControlChanged;
			}
			else
			{
				val.TextChanged += c_ControlChanged;
			}
			Type type = ((object)val).GetType();
			if (type == typeof(TextBox) || type == typeof(TextBoxX) || type == typeof(ComboBox) || type == typeof(ComboBoxEx) || type == typeof(NumericUpDown) || type == typeof(MaskedTextBoxAdv) || type == typeof(CheckBox))
			{
				val.Leave -= c_OnLeave;
				val.Leave += c_OnLeave;
				val.Enter -= c_OnEnter;
				val.Enter += c_OnEnter;
			}
			AddControlChanged(val.Controls);
		}
	}

	private void c_OnEnter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)sender).BackColor = Color.Yellow;
	}

	private void c_OnLeave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)sender).BackColor = Color.White;
	}

	private void c_ControlChanged(object sender, EventArgs e)
	{
		IsFormContentsChanged = true;
		if (!NoCheckFormContentsChanged && ((Control)this).Text.Substring(((Control)this).Text.Length - 1, 1) != "*")
		{
			((Control)this).Text = ((Control)this).Text + "*";
		}
	}

	private void FrmBase_Load(object sender, EventArgs e)
	{
	}

	private void FrmBase_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\FrmBase.cs", 282);
		}
	}

	private void InitializeComponent()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "FrmBase";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Text = "FrmBase";
		((Form)this).FormClosed += new FormClosedEventHandler(FrmBase_FormClosed);
		((Form)this).Load += FrmBase_Load;
		((Control)this).ResumeLayout(false);
	}
}
