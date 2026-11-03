using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using EntityCache;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities;
using Serilog;

namespace FormCustom;

public class BaseForm : CacheBaseForm
{
	private CancellationTokenSource _formChangeToken;

	private bool _isDisposed;

	private bool _isContentChanged;

	private bool startCaptureChanges = true;

	private const string helpfile = "helpHozor.chm";

	public bool StartCaptureChanges
	{
		get
		{
			return startCaptureChanges;
		}
		set
		{
			if (!value)
			{
				IsFormContentsChanged = false;
			}
			startCaptureChanges = value;
		}
	}

	public bool IsFormContentsChanged
	{
		get
		{
			return _isContentChanged;
		}
		set
		{
			if (!value || StartCaptureChanges)
			{
				_isContentChanged = value;
				_formChangeToken?.Cancel();
				_formChangeToken = new CancellationTokenSource(60000);
				CancellationToken token = _formChangeToken.Token;
				UpdateIsFormContentsChanged(value, token);
			}
		}
	}

	public string HelpState { get; set; } = "Main";


	protected bool NoCheckFormContentsChanged { get; set; }

	public BaseFormConfig Config { get; private set; }

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

	protected BaseForm(string contextGuid, BaseFormConfig _config = null)
		: base(contextGuid)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		try
		{
			if (_config == null)
			{
				Config = new BaseFormConfig();
			}
			else
			{
				Config = _config;
			}
			if (Config.ChangeFont)
			{
				if (ClsCache.GetCache(contextGuid) != null)
				{
					ClsCache cache = ClsCache.GetCache(contextGuid);
					if (cache == null || cache.Source != ENSource.TimeAttendance)
					{
						goto IL_00a9;
					}
				}
				((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
				((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
				((Form)this).ClientSize = new Size(784, 561);
				((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
			}
			goto IL_00a9;
			IL_00a9:
			((Form)this).KeyPreview = true;
			((Control)this).RightToLeft = (RightToLeft)1;
			((Control)this).VisibleChanged += BaseForm_VisibleChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 70);
		}
	}

	public BaseForm()
		: base("")
	{
	}

	private void UpdateIsFormContentsChanged(bool value, CancellationToken token)
	{
		try
		{
			if (!NoCheckFormContentsChanged && !IsDisposed && !token.IsCancellationRequested)
			{
				ILogger val = LogCache.Instance.Value.Get("UiLogger");
				if (val != null)
				{
					val.Information($"IsFormContentsChanged from {_isContentChanged} to {value}");
				}
				UpdateUiChangedAsync(token, val);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateIsFormContentsChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 84);
		}
	}

	private async Task UpdateUiChangedAsync(CancellationToken token, ILogger logger)
	{
		try
		{
			do
			{
				await Task.Delay(500);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						ILogger obj = logger;
						if (obj != null)
						{
							obj.Information("IsFormContentsChanged Invoked. from text:" + ((Control)this).Text);
						}
						if (_isContentChanged)
						{
							if (!string.IsNullOrEmpty(((Control)this).Text?.Trim()) && !NoCheckFormContentsChanged && ((Control)this).Text.Substring(((Control)this).Text.Length - 1, 1) != "*")
							{
								((Control)this).Text = ((Control)this).Text + "*";
							}
						}
						else
						{
							((Control)this).Text = ((Control)this).Text.Replace("*", "");
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateUiChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 112);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateUiChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 115);
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
			WebErrorLog.Instance.StartLog(ex, "", "ShowHelp", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 258);
		}
	}

	private void BaseForm_VisibleChanged(object sender, EventArgs e)
	{
		try
		{
			if (LanguageAndCalendar.Instance.TranslateUi() && ((Control)this).Visible && !IsDisposed)
			{
				((Control)this).Text = LanguageAndCalendar.Instance.GetValue(((Control)this).Text);
				EventHolder events = new EventHolder
				{
					c_ControlChanged = c_ControlChanged,
					c_OnLeave = c_OnLeave,
					c_OnEnter = c_OnEnter
				};
				ControlUtilities.Transelate(this, null, events);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BaseForm_VisibleChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 276);
		}
	}

	private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		try
		{
			((Control)this).OnKeyDown(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 293);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			((Form)this).OnLoad(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 302);
		}
	}

	private void c_OnEnter(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			BaseFormConfig config = Config;
			if (config != null && config.ChangeBackColor)
			{
				((Control)sender).BackColor = Color.Yellow;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "c_OnEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 312);
		}
	}

	private void c_OnLeave(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			BaseFormConfig config = Config;
			if (config != null && config.ChangeBackColor)
			{
				((Control)sender).BackColor = Color.White;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "c_OnLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 321);
		}
	}

	private void c_ControlChanged(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		try
		{
			Type type = ((object)(Control)sender).GetType();
			if (type == typeof(ComboBox) || type == typeof(ComboBoxEx) || type == typeof(NumericUpDown) || type == typeof(MaskedTextBoxAdv) || type == typeof(CheckBox) || type == typeof(TextBoxFa) || type == typeof(DateBoxFa) || type == typeof(TextBoxBase))
			{
				IsFormContentsChanged = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "c_ControlChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 339);
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_formChangeToken?.Cancel();
				_formChangeToken?.Dispose();
				_formChangeToken = null;
				((Form)this).FormClosed -= new FormClosedEventHandler(BaseForm_FormClosed);
				((Control)this).VisibleChanged -= BaseForm_VisibleChanged;
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\BaseForm.cs", 356);
		}
	}
}
