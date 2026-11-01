using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.ServiceManagement;
using FormCustom.Waiting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Win32;
using NPResourceManagement;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Attendance.Access;
using PacketParser.Attendance.LaborLaw;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PacketParser.Settings.Accounting;
using PacketParser.Settings.Attendance;
using PayamakUi.Classes;
using PayamakUi.Classes.Service;
using PayamakUi.Users;
using Serilog;
using TimeAttendance.ValidatorAttribute.Payamak;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.DeviceSerive;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDAL.Utility.Settings;
using TimeAttendanceUI.Forms.Service;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms.Codeing;

namespace TimeAttendanceUI.Forms.Other;

public class FrmSetting : BaseForm
{
	private bool _isDisposed;

	private TextBox _whichTextBoxClicked;

	private int _tmpAddition1;

	private int _tmpFraction1;

	private int _tmpFraction2;

	private int _tmpFraction3;

	private string _descSaleKari = "";

	private string _descSms = "";

	private string _descBackup = "";

	private string _descSalary = "";

	private string _descAccounting = "";

	private string _descExtraSetting = "";

	private List<GharardadStatus> _gharardadStatuses;

	private CancellationTokenSource _loadDatatokenSetting;

	private CancellationTokenSource _signalRToken;

	private PayamakClientHubInstances _payamakService;

	private Font _font;

	private ServiceManager _serviceManager;

	private DeviceServiceManager _deviceServiceManager;

	private IContainer components;

	private PanelEx panelEx1;

	private FontDialog FD;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private TabItem tabItem1;

	private TabControlPanel tabControlPanel5;

	private TabItem tabItem5;

	private PictureBox pictureBox5;

	private FolderBrowserDialog FBD;

	private TabControlPanel tabControlPanel6;

	private TabItem tabItem6;

	private PictureBox pictureBox6;

	internal CheckBoxX ChbSMSVorodValed;

	internal CheckBoxX ChbSmsVManage;

	internal GroupPanel groupPanel7;

	internal CheckBoxX CHEnableAutoBackupAllPartions;

	internal GroupPanel groupPanel8;

	internal LabelX labelX6;

	internal LabelX labelX7;

	internal Button BackupBTNFindPath;

	internal LabelX labelX8;

	internal LinkLabel TPBackup_DefaultPath;

	internal NumericUpDown TbxBackupTimeBetween;

	internal CheckBoxX CHEnableAutoBackup;

	internal CheckBoxX ChbSMSKHValed;

	internal CheckBoxX ChbSmsKHManage;

	internal TextBox TxtsSmsManageV;

	internal TextBox TxtSmsValedKh;

	internal TextBox TxtsSmsManageKH;

	private TextBoxX TxtAdminMobs;

	internal LabelX labelX15;

	private TabControlPanel tabControlPanel7;

	private TabItem tabItem7;

	private TextBoxX TxtCompany;

	private LabelX labelX1;

	private PictureBox pictureBox7;

	internal GroupPanel groupPanel12;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TextBoxX TxtCodeEkhtesasi;

	private LabelX labelX20;

	private TextBoxX TxtTel;

	private LabelX labelX19;

	private TextBoxX TxtADD;

	private LabelX labelX18;

	private TextBoxX TxtFax;

	private LabelX labelX16;

	private TextBoxX TxtShrh;

	private LabelX labelX21;

	internal Label Label42;

	internal GroupPanel groupPanel14;

	internal ButtonX BtnPerName;

	internal TextBox TxtSmsValedV;

	internal ButtonX BtnTime;

	internal ButtonX BtnEK;

	internal ButtonX BtntypeTaradod;

	internal ButtonX BtnCodeMoein;

	internal ButtonX btnCodeT;

	internal ButtonX BtnDate;

	internal ButtonX BtnMoreSms;

	private TextBoxX TxtSamaneSMS;

	internal LabelX labelX9;

	internal ButtonX BtnDeleteAcc;

	private TabControlPanel tabControlPanel3;

	private TabItem tabItem3;

	private TextBoxX TxtSabt;

	private LabelX labelX14;

	internal GroupPanel groupPanel2;

	internal LabelX labelX3;

	internal NumericUpDown NumSendTaradodSmsMaxMinutes;

	internal TextBox TxtSmsAbsentPerson;

	internal TextBox TxtSmsAbsentManager;

	internal CheckBoxX ChbSMSAbsentPerson;

	internal CheckBoxX ChbSMSAbsentManager;

	internal LabelX labelX4;

	internal NumericUpDown NumSendAbsentSmsMinMinutesAfterShift;

	internal LabelX labelX17;

	private TabControlPanel tabControlPanel2;

	internal GroupPanel groupPanel3;

	internal ButtonX btnSearchFileName;

	internal CheckBoxX chBxGetFileName;

	private TextBox txtFileName;

	internal LabelX labelX22;

	private TabItem tabItem2;

	private OpenFileDialog openFileDialog1;

	internal ButtonX BtnKK;

	internal TextBox TxtSmsIncompletePerson;

	internal CheckBoxX chkBxIncompletePerson;

	internal TextBox TxtSmsIncompleteManager;

	internal CheckBoxX chkBxIncompleteManager;

	private TabControlPanel tabControlPanel4;

	private TabItem tabItem4;

	private TextBox TxtTafsilTaxGov;

	private TextBox TxtMoeinTaxGov;

	private TextBox TxtTafsilTaminGov;

	private TextBox TxtMoeinTaminGov;

	private TextBox TxtTafsilLeaveTime;

	private TextBox TxtMoeinLeaveTime;

	private TextBox TxtTafsilUnEmployMent;

	private TextBox TxtMoeinUnEmployMent;

	private TextBox TxtTafsilBaseSalary;

	private TextBox TxtMoeinBaseSalary;

	internal LabelX labelX32;

	internal LabelX labelX33;

	internal LabelX lblMoeinTaxGov;

	internal LabelX labelX35;

	internal LabelX labelX39;

	internal LabelX labelX40;

	internal LabelX labelX41;

	private TextBox TxtTafsilEmployer;

	private TextBox TxtTafsilVacation;

	private TextBox TxtMoeinEmployer;

	private TextBox TxtMoeinVacation;

	internal LabelX labelX28;

	internal LabelX labelX27;

	private TextBox TxtTafsilNightTime;

	private TextBox TxtTafsilHolidayTime;

	private TextBox TxtTafsilShiftTime;

	private TextBox TxtTafsilOverTime;

	private TextBox TxtMoeinNightTime;

	private TextBox TxtMoeinHolidayTime;

	private TextBox TxtMoeinShiftTime;

	private TextBox TxtMoeinOverTime;

	internal LabelX labelX26;

	internal LabelX labelX25;

	internal LabelX lblMoeinShiftTime;

	internal LabelX lblMoeinOverTime;

	internal LabelX labelX13;

	private CheckBoxX ChBxTaxBaseSalary;

	private CheckBoxX ChBxInsuranceBaseSalary;

	internal LabelX labelX36;

	private CheckBoxX ChBxTaxVacation;

	private CheckBoxX ChBxTaxLeaveTime;

	private CheckBoxX ChBxTaxNightTime;

	private CheckBoxX ChBxInsuranceVacation;

	private CheckBoxX ChBxTaxHolidayTime;

	private CheckBoxX ChBxInsuranceNightTime;

	private CheckBoxX ChBxTaxShiftTime;

	private CheckBoxX ChBxInsuranceHolidayTime;

	private CheckBoxX ChBxTaxOverTime;

	private CheckBoxX ChBxInsuranceShiftTime;

	private CheckBoxX ChBxInsuranceOverTime;

	internal GroupPanel groupPanel1;

	private TextBoxX TxtFont;

	private ButtonX BtnMore;

	private LabelX labelX2;

	private PictureBox pictureBox3;

	internal Label LBLPreviwe;

	internal CheckBoxX ChBxInsuranceLeaveTime;

	private CheckBoxX ChBxTaxAbsentTime;

	internal CheckBoxX ChBxInsuranceAbsentTime;

	internal LabelX labelX38;

	private TextBox TxtTafsilAbsentTime;

	private TextBox TxtMoeinAbsentTime;

	internal GroupPanel groupPanel5;

	internal CheckBoxX CBRunInStart;

	internal CheckBoxX ChkNotShowUi;

	private TabControlPanel tabControlPanel8;

	private TabItem tabItem8;

	internal LabelX labelX52;

	internal LabelX labelX54;

	internal CheckBoxX ChkViewEndedGharardad;

	internal NumericUpDown NUMGharardadEndedAgo;

	internal LabelX labelX48;

	internal LabelX labelX49;

	internal NumericUpDown NUMGharardadEndedNext;

	internal CheckBoxX ChkFromDateEndedGharardad;

	internal LabelX labelX50;

	private FlowLayoutPanel flowLayoutPanel1;

	internal LabelX labelX53;

	internal NumericUpDown NumLockTime;

	internal CheckBoxX CBGetPassAfterMaximize;

	internal LabelX labelX55;

	private TextBox ZaribTatil;

	internal LabelX labelX56;

	internal LabelX labelX45;

	internal LabelX labelX12;

	private TextBox ZaribFriday;

	internal LabelX labelX47;

	internal LabelX labelX46;

	internal LabelX labelX11;

	private TextBox ZaribEzafeDay;

	private TextBoxFa txtHaghSpouse;

	internal LabelX labelX43;

	private TextBox MINEzafeDay;

	private TextBoxFa txtHaghMaskan;

	internal LabelX labelX10;

	internal LabelX labelX44;

	private ButtonX btnGetLabor;

	private TextBoxFa txtBon;

	internal LabelX labelX42;

	private TextBoxFa txtDailyWage;

	internal LabelX labelX37;

	internal CheckBoxX CHAutoEzafe;

	private GroupPanel groupPanel4;

	private ButtonX btnFraction3;

	private ButtonX btnFraction2;

	private ButtonX btnFraction1;

	private ButtonX btnAdditions1;

	internal LabelX labelX31;

	internal LabelX labelX34;

	internal LabelX labelX23;

	internal LabelX labelX24;

	private TextBox txtFraction3Title;

	private TextBox txtFraction2Title;

	private TextBox txtFraction1Title;

	private TextBox txtAdditions1Title;

	private TextBox txtFraction3Tafsil;

	private TextBox txtFraction3Moein;

	private TextBox txtFraction2Tafsil;

	private TextBox txtFraction2Moein;

	private TextBox txtFraction1Tafsil;

	private TextBox txtFraction1Moein;

	private TextBox txtAdditions1Tafsil;

	private TextBox txtAdditions1Moein;

	internal LabelX lblAdditions1;

	internal LabelX labelX29;

	internal LabelX labelX30;

	internal NumericUpDown txtMaxTedadOlaad;

	internal LabelX labelX5;

	internal GroupPanel groupPanel6;

	private TextBoxX TbxManualEditTaradod;

	internal LabelX labelX58;

	private TextBoxX TbxManualAddTaradod;

	internal LabelX labelX57;

	internal GroupPanel groupPanel9;

	internal LabelX labelX59;

	private ComboBox CmbCalendarType;

	private CheckBoxX ChBxTaxJarime;

	internal CheckBoxX ChBxInsuranceJarime;

	internal LabelX labelX62;

	private TextBox TxtTafsilJarime;

	private TextBox TxtMoeinJarime;

	private CheckBoxX ChBxTaxTatilKari;

	internal LabelX labelX61;

	private CheckBoxX ChBxInsuranceTatilKari;

	private TextBox TxtMoeinTatilKari;

	private TextBox TxtTafsilTatilKari;

	private CheckBoxX ChBxTaxMission;

	internal LabelX labelX60;

	private TextBox TxtMoeinMission;

	private CheckBoxX ChBxInsuranceMission;

	private TextBox TxtTafsilMission;

	internal LabelX labelX63;

	private TextBox TxtRoundBelow;

	internal LabelX labelX64;

	internal LabelX labelX65;

	private TextBox TxtTafsilRoundFish;

	private TextBox TxtMoeinRoundFish;

	private TabControlPanel tabControlPanel9;

	private TabItem WebServiceTab;

	private TextBox TxtNpPass;

	private TextBox TxtNpUser;

	internal GroupPanel GrpWebservice;

	private LabelX labelX66;

	private LabelX labelX67;

	private CheckBoxX ChkWebService;

	internal LinkLabel lbllinkwebservice;

	private LabelX lblURL;

	internal CheckBoxX ChkViewGridLastTaradods;

	internal LabelX labelX68;

	internal NumericUpDown TbxCountLastTaradods;

	internal CheckBoxX ChkViewGridReportDevices;

	internal LabelX labelX69;

	internal NumericUpDown TbxCountReportDevices;

	private TabControlPanel tabControlPanel10;

	private TabItem tabItemSettingTaradod;

	internal CheckBoxX ChkAutomaticCalcMoveToNextDay;

	internal CheckBoxX ChkNotKarkardWhenNoGharardad;

	internal CheckBoxX ChkCalcEzafeOutOfShift;

	internal CheckBoxX ChkCalcHoliday;

	internal CheckBoxX ChkCalcEkWhenHasVacationOrMission;

	internal CheckBoxX ChkCalcEkInSourceDate;

	internal LabelX labelX71;

	private TextBox ZaribMinesBimeOfTax;

	internal LabelX labelX72;

	private ComboBox CmbURL;

	internal CheckBoxX ChkCalcEK_KK;

	internal LabelX labelX51;

	internal LabelX labelX70;

	internal NumericUpDown NumSendIncompleteSmsToClock;

	internal NumericUpDown NumSendIncompleteSmsFromClock;

	internal ButtonX BtnPerFamily;

	internal LabelX labelX73;

	internal NumericUpDown NumSendSmsToClock;

	internal NumericUpDown NumSendSmsFromClock;

	internal LabelX labelX74;

	private TabControlPanel tabControlPanel11;

	private TabItem tabItem9;

	private Panel pnlPayamakServiceSettings;

	private CheckBox CBPayamakEnable;

	private UcServiceManager ucServiceManager1;

	private LabelX labelX75;

	internal NumericUpDown NumGetTaradodFormCountDaysAgo;

	private TextBoxFa TxtHaghTahol;

	internal LabelX labelX76;

	internal LabelX labelX77;

	internal NumericUpDown NumSendAbsentSmsMaxMinutesAfterShift;

	internal LabelX labelX78;

	private LabelX labelX79;

	internal CheckBoxX ChbSendSmsVacationManager;

	private TabControlPanel tabControlPanel12;

	private TabItem tabControlDeviceService;

	private UcServiceManager ucDeviceService;

	internal CheckBoxX chbHandleDeviceService;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmSetting(string contextGuid, Font font, int selectedTabcontrol = 0)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			_font = font;
			InitializeComponent();
			_payamakService = PayamakClientHubInstances.GetInstance(contextGuid);
			_serviceManager = new ServiceManager(contextGuid);
			_deviceServiceManager = new DeviceServiceManager(contextGuid);
			tabControl1.SelectedTabIndex = ((selectedTabcontrol >= 0 && selectedTabcontrol <= ((CollectionBase)(object)tabControl1.Tabs).Count) ? selectedTabcontrol : 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 73);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 8;
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.SoftwareSetting.SoftwareSettingPayamak)
			{
				((Control)tabControlPanel6).Enabled = false;
			}
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			UserAttSetting userSetting = UserAttendance.SelectedUser.UserSetting;
			CBRunInStart.Checked = setting.RunInStart;
			CBGetPassAfterMaximize.Checked = userSetting.ShowFrmEnter;
			NumLockTime.Value = userSetting.LockTime;
			((Control)TbxManualAddTaradod).Text = userSetting.ManualAddTaradodCharacter;
			((Control)TbxManualEditTaradod).Text = userSetting.ManualEditTaradodCharacter;
			((ListControl)CmbCalendarType).SelectedValue = (int)Utility.GetLanguage();
			((Control)TxtCompany).Text = setting.SaleKari.TitleSalekari;
			((Control)TxtTel).Text = setting.SaleKari.Tel;
			((Control)TxtADD).Text = setting.SaleKari.Address;
			((Control)TxtFax).Text = setting.SaleKari.Fax;
			((Control)TxtShrh).Text = setting.SaleKari.Desc;
			((Control)TxtCodeEkhtesasi).Text = setting.SaleKari.CodeEkhtesasi;
			((Control)TxtSabt).Text = setting.SaleKari.SabtNo;
			((Control)TxtNpUser).Text = setting.WebServiceKey.NPACCUserName;
			((Control)TxtNpPass).Text = setting.WebServiceKey?.NPACCPassword;
			((Control)CmbURL).Text = setting.WebServiceKey?.URL;
			if (userSetting != null)
			{
				ChkWebService.Checked = UserAttendance.SelectedUser.UserSetting.EnableCloudSignalR;
			}
			((Control)GrpWebservice).Enabled = ChkWebService.Checked;
			((Control)LBLPreviwe).Font = setting.FontProgram;
			((Control)TxtFont).Font = setting.FontProgram;
			((Control)TxtFont).Text = setting.FontProgram.Name;
			FD.Font = setting.FontProgram;
			if (string.IsNullOrEmpty(setting.Payamak.ConnectionStringPayamak))
			{
				setting.Payamak.ConnectionStringPayamak = Extensions.GetSqlConnectionString(((CacheBaseForm)this).ContextGuid);
			}
			CBPayamakEnable.Checked = ((AttDepartmentAdminSms)setting.Payamak).EnablePayamak;
			if (((AttDepartmentAdminSms)setting.Payamak).EnablePayamak)
			{
				if (((AttDepartmentAdminSms)setting.Payamak).IdSamane == 0)
				{
					((AttDepartmentAdminSms)setting.Payamak).IdSamane = 10000;
				}
				try
				{
					ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> val = await _payamakService.SmsNetAccount.GetByIdAsync(((AttDepartmentAdminSms)setting.Payamak).IdSamane);
					if (((ReturnedSaveFuncInfo)val).HasError)
					{
						Log.Error(((ReturnedSaveFuncInfo)val).ErrorMessage);
						Log.Error(((ReturnedSaveFuncInfo)val).Messages);
						Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, ((ReturnedSaveFuncInfo)val).ErrorMessage, "", 3);
					}
					else
					{
						((Control)TxtSamaneSMS).Text = val.value.Title ?? "";
					}
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
				}
			}
			ChbSmsVManage.Checked = ((AttDepartmentAdminSms)setting.Payamak).SendVorodManage;
			ChbSmsKHManage.Checked = ((AttDepartmentAdminSms)setting.Payamak).SendKhorojManage;
			ChbSMSVorodValed.Checked = setting.Payamak.SendVorodValed;
			ChbSMSKHValed.Checked = setting.Payamak.SendKhorojValed;
			((Control)TxtsSmsManageV).Text = ((AttDepartmentAdminSms)setting.Payamak).PayamakManageVorod?.Replace("[", "<").Replace("]", ">");
			((Control)TxtsSmsManageKH).Text = ((AttDepartmentAdminSms)setting.Payamak).PayamakManageKhoroj?.Replace("[", "<").Replace("]", ">");
			((Control)TxtSmsValedV).Text = setting.Payamak.PayamakValedVorod?.Replace("[", "<").Replace("]", ">");
			((Control)TxtSmsValedKh).Text = setting.Payamak.PayamakValedKhoroj?.Replace("[", "<").Replace("]", ">");
			ChbSMSAbsentManager.Checked = ((AttDepartmentAdminSms)setting.Payamak).IsSendSmsAbsentManager;
			chkBxIncompleteManager.Checked = ((AttDepartmentAdminSms)setting.Payamak).IsSendSmsIncompleteManager;
			ChbSMSAbsentPerson.Checked = setting.Payamak.IsSendSmsAbsentPerson;
			ChbSendSmsVacationManager.Checked = setting.Payamak.IsSendSmsVacationManager;
			chkBxIncompletePerson.Checked = setting.Payamak.IsSendSmsIncompletePerson;
			((Control)TxtSmsAbsentManager).Text = ((AttDepartmentAdminSms)setting.Payamak).SmsAbsentManager?.Replace("[", "<").Replace("]", ">");
			((Control)TxtSmsAbsentPerson).Text = setting.Payamak.SmsAbsentPerson?.Replace("[", "<").Replace("]", ">");
			((Control)TxtSmsIncompleteManager).Text = ((AttDepartmentAdminSms)setting.Payamak).SmsIncompleteManager?.Replace("[", "<").Replace("]", ">");
			((Control)TxtSmsIncompletePerson).Text = setting.Payamak.SmsIncompletePerson?.Replace("[", "<").Replace("]", ">");
			((Control)TxtAdminMobs).Text = ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs;
			NumSendSmsFromClock.Value = ((AttDepartmentAdminSms)setting.Payamak).SendSmsFromClockOfDay;
			NumSendSmsToClock.Value = ((AttDepartmentAdminSms)setting.Payamak).SendSmsToClockOfDay;
			NumSendIncompleteSmsFromClock.Value = ((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsFromClock;
			NumSendIncompleteSmsToClock.Value = ((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsToClock;
			if (((AttDepartmentAdminSms)setting.Payamak).SendTaradodSmsMaxMinutes < 1m)
			{
				((AttDepartmentAdminSms)setting.Payamak).SendTaradodSmsMaxMinutes = 1m;
			}
			NumSendTaradodSmsMaxMinutes.Value = (int)((AttDepartmentAdminSms)setting.Payamak).SendTaradodSmsMaxMinutes;
			if (((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMinMinutesAfterShift > 0m)
			{
				NumSendAbsentSmsMinMinutesAfterShift.Value = ((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMinMinutesAfterShift;
			}
			if (((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMaxMinutesAfterShift > 0m)
			{
				NumSendAbsentSmsMaxMinutesAfterShift.Value = ((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMaxMinutesAfterShift;
			}
			AccGlobalSettings localSetting = await LocalSettingBusiness.GetSettingAsync(((CacheBaseForm)this).ContextGuid, default(CancellationToken), "");
			if (localSetting != null)
			{
				ServiceManagerConfig val2 = new ServiceManagerConfig
				{
					Manager = (ServiceBaseManager)(object)_serviceManager,
					ConnectionString = localSetting.Payamak.ServerConnectionString,
					Ip = localSetting.Payamak.BaseHubIp,
					Port = localSetting.Payamak.BaseHubPort
				};
				await ucServiceManager1.InitAsync(val2, default(CancellationToken), (Font)null);
				CHEnableAutoBackup.Checked = localSetting.Others.BackUpSettings.EnableAutoBackup;
				decimal.TryParse(localSetting.Others.BackUpSettings.BackupTimeBetween.ToString(), out var result);
				if (result <= 10m)
				{
					result = 10m;
				}
				TbxBackupTimeBetween.Value = result;
			}
			CHEnableAutoBackupAllPartions.Checked = BackupSetting.EnableAutoBackupAllPartions;
			((Control)TPBackup_DefaultPath).Text = BackupSetting.DefaultPath;
			CHAutoEzafe.Checked = setting.AutoEzafe;
			bool @checked;
			if ((int)AccessLevelClass.Instance.LevelHozor != 5)
			{
				CheckBoxX chkCalcEkInSourceDate = ChkCalcEkInSourceDate;
				@checked = (ChkAutomaticCalcMoveToNextDay.Checked = false);
				chkCalcEkInSourceDate.Checked = @checked;
				CheckBoxX chkCalcEkInSourceDate2 = ChkCalcEkInSourceDate;
				@checked = (((Control)ChkAutomaticCalcMoveToNextDay).Enabled = false);
				((Control)chkCalcEkInSourceDate2).Enabled = @checked;
				CheckBoxX chkAutomaticCalcMoveToNextDay = ChkAutomaticCalcMoveToNextDay;
				@checked = (((Control)ChkAutomaticCalcMoveToNextDay).Enabled = false);
				chkAutomaticCalcMoveToNextDay.Checked = @checked;
				CheckBoxX chkCalcHoliday = ChkCalcHoliday;
				@checked = (ChkCalcHoliday.Checked = false);
				((Control)chkCalcHoliday).Enabled = @checked;
			}
			else
			{
				ChkCalcEzafeOutOfShift.Checked = setting.CalcEzafeOutOfShift;
				ChkAutomaticCalcMoveToNextDay.Checked = setting.AutomaticCalcMovetoNextDay;
				ChkCalcEkInSourceDate.Checked = setting.CalcEkInSourceDate;
				ChkCalcHoliday.Checked = setting.CodeingAccounting.IsCalcHoliday;
			}
			ChkCalcEkWhenHasVacationOrMission.Checked = setting.CalcEkWhenHasVacationOrMission;
			ChkCalcEK_KK.Checked = setting.NotCalkEK_KK;
			ChkNotKarkardWhenNoGharardad.Checked = setting.NotKarkardWhenNoGharardad;
			((Control)TxtRoundBelow).Text = setting.RoundBelowPrice.ToString("N0");
			txtMaxTedadOlaad.Value = setting.MaxTedadOlaadInSalary;
			((Control)ZaribEzafeDay).Text = setting.ZaribEzafeDay.ToString();
			((Control)ZaribFriday).Text = setting.ZaribFriday.ToString();
			((Control)ZaribMinesBimeOfTax).Text = setting.ZaribMinesBimeOfTax.ToString();
			((Control)ZaribTatil).Text = setting.ZaribTatil.ToString();
			((Control)txtDailyWage).Text = setting.LaborLaw.DailyWage.ToString("N0");
			((Control)txtBon).Text = setting.LaborLaw.Bon.ToString("N0");
			((Control)TxtHaghTahol).Text = setting.LaborLaw.HaghTahol.ToString("N0");
			((Control)txtHaghMaskan).Text = setting.LaborLaw.HaghMaskan.ToString("N0");
			((Control)txtHaghSpouse).Text = setting.LaborLaw.HaghSpouse.ToString("N0");
			_tmpAddition1 = setting.CodeingAccounting.Additions1;
			_tmpFraction1 = setting.CodeingAccounting.Fraction1;
			_tmpFraction2 = setting.CodeingAccounting.Fraction2;
			_tmpFraction3 = setting.CodeingAccounting.Fraction3;
			List<FishHesab> source = await FishHesab.SelectAllAsync(((CacheBaseForm)this).ContextGuid, getAllField: true, token);
			FishHesab fishHesab = source.FirstOrDefault((FishHesab q) => q.Id.Equals(_tmpAddition1));
			if (fishHesab != null)
			{
				((Control)txtAdditions1Moein).Text = fishHesab.CodeMoein.ToString();
				((Control)txtAdditions1Tafsil).Text = fishHesab.CodeTafsil.ToString();
				((Control)txtAdditions1Title).Text = fishHesab.Name;
			}
			FishHesab fishHesab2 = source.FirstOrDefault((FishHesab q) => q.Id.Equals(_tmpFraction1));
			if (fishHesab2 != null)
			{
				((Control)txtFraction1Moein).Text = fishHesab2.CodeMoein.ToString();
				((Control)txtFraction1Tafsil).Text = fishHesab2.CodeTafsil.ToString();
				((Control)txtFraction1Title).Text = fishHesab2.Name;
			}
			FishHesab fishHesab3 = source.FirstOrDefault((FishHesab q) => q.Id.Equals(_tmpFraction2));
			if (fishHesab3 != null)
			{
				((Control)txtFraction2Moein).Text = fishHesab3.CodeMoein.ToString();
				((Control)txtFraction2Tafsil).Text = fishHesab3.CodeTafsil.ToString();
				((Control)txtFraction2Title).Text = fishHesab3.Name;
			}
			FishHesab fishHesab4 = source.FirstOrDefault((FishHesab q) => q.Id.Equals(_tmpFraction3));
			if (fishHesab4 != null)
			{
				((Control)txtFraction3Moein).Text = fishHesab4.CodeMoein.ToString();
				((Control)txtFraction3Tafsil).Text = fishHesab4.CodeTafsil.ToString();
				((Control)txtFraction3Title).Text = fishHesab4.Name;
			}
			CodeingAccounting codeingAccounting = setting.CodeingAccounting;
			((Control)TxtMoeinBaseSalary).Text = ((!codeingAccounting.MoeinBaseSalary.HasValue) ? "0" : codeingAccounting.MoeinBaseSalary.ToString());
			((Control)TxtTafsilBaseSalary).Text = ((!codeingAccounting.TafsilBaseSalary.HasValue) ? "0" : codeingAccounting.TafsilBaseSalary.ToString());
			ChBxInsuranceBaseSalary.Checked = codeingAccounting.InsuranceBaseSalary;
			ChBxTaxBaseSalary.Checked = codeingAccounting.TaxBaseSalary;
			((Control)TxtMoeinMission).Text = ((!codeingAccounting.MoeinMission.HasValue) ? "0" : codeingAccounting.MoeinMission.ToString());
			((Control)TxtTafsilMission).Text = ((!codeingAccounting.TafsilMission.HasValue) ? "0" : codeingAccounting.TafsilMission.ToString());
			ChBxInsuranceMission.Checked = codeingAccounting.InsuranceMission;
			ChBxTaxMission.Checked = codeingAccounting.TaxMission;
			((Control)TxtMoeinOverTime).Text = ((!codeingAccounting.MoeinOverTime.HasValue) ? "0" : codeingAccounting.MoeinOverTime.ToString());
			((Control)TxtTafsilOverTime).Text = ((!codeingAccounting.TafsilOverTime.HasValue) ? "0" : codeingAccounting.TafsilOverTime.ToString());
			ChBxInsuranceOverTime.Checked = codeingAccounting.InsuranceOverTime;
			ChBxTaxOverTime.Checked = codeingAccounting.TaxOverTime;
			((Control)TxtMoeinShiftTime).Text = ((!codeingAccounting.MoeinShiftTime.HasValue) ? "0" : codeingAccounting.MoeinShiftTime.ToString());
			((Control)TxtTafsilShiftTime).Text = ((!codeingAccounting.TafsilShiftTime.HasValue) ? "0" : codeingAccounting.TafsilShiftTime.ToString());
			ChBxInsuranceShiftTime.Checked = codeingAccounting.InsuranceShiftTime;
			ChBxTaxShiftTime.Checked = codeingAccounting.TaxShiftTime;
			((Control)TxtMoeinHolidayTime).Text = ((!codeingAccounting.MoeinHolidayTime.HasValue) ? "0" : codeingAccounting.MoeinHolidayTime.ToString());
			((Control)TxtTafsilHolidayTime).Text = ((!codeingAccounting.TafsilHolidayTime.HasValue) ? "0" : codeingAccounting.TafsilHolidayTime.ToString());
			ChBxInsuranceHolidayTime.Checked = codeingAccounting.InsuranceHolidayTime;
			ChBxTaxHolidayTime.Checked = codeingAccounting.TaxHolidayTime;
			((Control)TxtMoeinTatilKari).Text = ((!codeingAccounting.MoeinTatilKari.HasValue) ? "0" : codeingAccounting.MoeinTatilKari.ToString());
			((Control)TxtTafsilTatilKari).Text = ((!codeingAccounting.TafsilTatilKari.HasValue) ? "0" : codeingAccounting.TafsilTatilKari.ToString());
			ChBxInsuranceTatilKari.Checked = codeingAccounting.InsuranceTatilKari;
			ChBxTaxTatilKari.Checked = codeingAccounting.TaxTatilKari;
			((Control)TxtMoeinNightTime).Text = ((!codeingAccounting.MoeinNightTime.HasValue) ? "0" : codeingAccounting.MoeinNightTime.ToString());
			((Control)TxtTafsilNightTime).Text = ((!codeingAccounting.TafsilNightTime.HasValue) ? "0" : codeingAccounting.TafsilNightTime.ToString());
			ChBxInsuranceNightTime.Checked = codeingAccounting.InsuranceNightTime;
			ChBxTaxNightTime.Checked = codeingAccounting.TaxNightTime;
			((Control)TxtMoeinVacation).Text = ((!codeingAccounting.MoeinVacation.HasValue) ? "0" : codeingAccounting.MoeinVacation.ToString());
			((Control)TxtTafsilVacation).Text = ((!codeingAccounting.TafsilVacation.HasValue) ? "0" : codeingAccounting.TafsilVacation.ToString());
			ChBxInsuranceVacation.Checked = codeingAccounting.InsuranceVacation;
			ChBxTaxVacation.Checked = codeingAccounting.TaxVacation;
			((Control)TxtMoeinLeaveTime).Text = ((!codeingAccounting.MoeinLeaveTime.HasValue) ? "0" : codeingAccounting.MoeinLeaveTime.ToString());
			((Control)TxtTafsilLeaveTime).Text = ((!codeingAccounting.TafsilLeaveTime.HasValue) ? "0" : codeingAccounting.TafsilLeaveTime.ToString());
			ChBxInsuranceLeaveTime.Checked = codeingAccounting.InsuranceLeaveTime;
			ChBxTaxLeaveTime.Checked = codeingAccounting.TaxLeaveTime;
			((Control)TxtMoeinJarime).Text = ((!codeingAccounting.MoeinJarime.HasValue) ? "0" : codeingAccounting.MoeinJarime.ToString());
			((Control)TxtTafsilJarime).Text = ((!codeingAccounting.TafsilJarime.HasValue) ? "0" : codeingAccounting.TafsilJarime.ToString());
			ChBxInsuranceJarime.Checked = codeingAccounting.InsuranceJarime;
			ChBxTaxJarime.Checked = codeingAccounting.TaxJarime;
			((Control)TxtMoeinAbsentTime).Text = ((!codeingAccounting.MoeinAbsentTime.HasValue) ? "0" : codeingAccounting.MoeinAbsentTime.ToString());
			((Control)TxtTafsilAbsentTime).Text = ((!codeingAccounting.TafsilAbsentTime.HasValue) ? "0" : codeingAccounting.TafsilAbsentTime.ToString());
			ChBxInsuranceAbsentTime.Checked = codeingAccounting.InsuranceAbsentTime;
			ChBxTaxAbsentTime.Checked = codeingAccounting.TaxAbsentTime;
			((Control)TxtMoeinEmployer).Text = ((!codeingAccounting.MoeinEmployer.HasValue) ? "0" : codeingAccounting.MoeinEmployer.ToString());
			((Control)TxtTafsilEmployer).Text = ((!codeingAccounting.TafsilEmployer.HasValue) ? "0" : codeingAccounting.TafsilEmployer.ToString());
			((Control)TxtMoeinUnEmployMent).Text = ((!codeingAccounting.MoeinUnEmployMent.HasValue) ? "0" : codeingAccounting.MoeinUnEmployMent.ToString());
			((Control)TxtTafsilUnEmployMent).Text = ((!codeingAccounting.TafsilUnEmployMent.HasValue) ? "0" : codeingAccounting.TafsilUnEmployMent.ToString());
			((Control)TxtMoeinTaminGov).Text = ((!codeingAccounting.MoeinTaminGov.HasValue) ? "0" : codeingAccounting.MoeinTaminGov.ToString());
			((Control)TxtTafsilTaminGov).Text = ((!codeingAccounting.TafsilTaminGov.HasValue) ? "0" : codeingAccounting.TafsilTaminGov.ToString());
			((Control)TxtMoeinTaxGov).Text = ((!codeingAccounting.MoeinTaxGov.HasValue) ? "0" : codeingAccounting.MoeinTaxGov.ToString());
			((Control)TxtTafsilTaxGov).Text = ((!codeingAccounting.TafsilTaxGov.HasValue) ? "0" : codeingAccounting.TafsilTaxGov.ToString());
			((Control)TxtMoeinRoundFish).Text = ((!codeingAccounting.MoeinRoundFish.HasValue) ? "0" : codeingAccounting.MoeinRoundFish.ToString());
			((Control)TxtTafsilRoundFish).Text = ((!codeingAccounting.TafsilRoundFish.HasValue) ? "0" : codeingAccounting.TafsilRoundFish.ToString());
			((Control)txtFileName).Text = setting.AppRunAfterLogOnPerson;
			TextBox obj = txtFileName;
			ButtonX obj2 = btnSearchFileName;
			bool flag5 = (chBxGetFileName.Checked = setting.IsRunAppAfterLogOnPerson);
			@checked = (((Control)obj2).Enabled = flag5);
			((Control)obj).Enabled = @checked;
			object obj3 = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin\\service", "NotShowUi", 0) ?? ((object)0);
			ChkNotShowUi.Checked = obj3.ToString() != "0";
			ChkViewGridLastTaradods.Checked = userSetting.viewGridLastTaradods;
			TbxCountLastTaradods.Value = userSetting.CountLastTaradods;
			ChkViewGridReportDevices.Checked = userSetting.viewGridReportDevices;
			TbxCountReportDevices.Value = userSetting.CountReportDevices;
			NumGetTaradodFormCountDaysAgo.Value = setting.GetTaradodFormCountDaysAgo;
			NUMGharardadEndedAgo.Value = setting.GharardadEndedAgo;
			NUMGharardadEndedNext.Value = setting.GharardadEndedNext;
			ChkViewEndedGharardad.Checked = setting.ViewEndedGharardad;
			ChkFromDateEndedGharardad.Checked = setting.FromDateEndedGharardad;
			await LoadGharardadStatusItemsAsync(token);
			await LogDataAsync();
			DeviceServiceSettings val3 = await LocalSettingBusiness.GetDeviceServiceSettingsAsync(((CacheBaseForm)this).ContextGuid, token, true);
			CheckBoxX obj4 = chbHandleDeviceService;
			@checked = (((Control)ucDeviceService).Enabled = val3.HandleService);
			obj4.Checked = @checked;
			ServiceManagerConfig val4 = new ServiceManagerConfig
			{
				Manager = (ServiceBaseManager)(object)_deviceServiceManager,
				Ip = val3.BaseHubIp,
				Port = val3.BaseHubPort,
				HandleConnectionString = false,
				SetUpIp = val3.SetupIp
			};
			await ucDeviceService.InitAsync(val4, default(CancellationToken), (Font)null);
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 361);
		}
	}

	private async void FRMSetting_Load(object sender, EventArgs e)
	{
		try
		{
			Dictionary<string, int> enumPersianName = Utility.GetEnumPersianName(typeof(LanguageTypes));
			enumPersianName.Remove("English");
			enumPersianName.Remove("Spanish");
			enumPersianName.Remove("Turkish");
			enumPersianName.Remove("InterNational");
			CmbCalendarType.DataSource = (object)new BindingSource((object)enumPersianName, (string)null);
			((ListControl)CmbCalendarType).DisplayMember = "Key";
			((ListControl)CmbCalendarType).ValueMember = "Value";
			_loadDatatokenSetting?.Cancel();
			_loadDatatokenSetting = new CancellationTokenSource();
			CancellationToken token = _loadDatatokenSetting.Token;
			await LoadDataAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSetting_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 382);
		}
	}

	private static Dictionary<string, int> GetEnumDescriptions(Type enumType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		try
		{
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Only enum type is expected.");
			}
			foreach (object value2 in Enum.GetValues(enumType))
			{
				int value = (int)value2;
				string text = value2.ToString();
				object obj = value2.GetType().GetField(text)?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).First();
				if (obj != null)
				{
					text = (obj as DescriptionAttribute)?.Description;
				}
				if (text != null)
				{
					dictionary.Add(text, value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetEnumDescriptions", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 405);
		}
		return dictionary;
	}

	private void TxtMobs_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtMobs_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 421);
		}
	}

	private void TxtMobs_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtMobs_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 433);
		}
	}

	private void FRMSetting_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 116)
					{
						e.Handled = true;
						((PopupItemControl)BtnAccept).PerformClick();
					}
				}
				else
				{
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSetting_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 458);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		int lin = 0;
		try
		{
			((Control)BtnAccept).Enabled = false;
			UserAttSetting userSetting = UserAttendance.SelectedUser.UserSetting;
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			lin = 1;
			setting.RunInStart = CBRunInStart.Checked;
			userSetting.LockTime = (int)NumLockTime.Value;
			userSetting.ManualAddTaradodCharacter = ((Control)TbxManualAddTaradod).Text;
			userSetting.ManualEditTaradodCharacter = ((Control)TbxManualEditTaradod).Text;
			userSetting.viewGridLastTaradods = ChkViewGridLastTaradods.Checked;
			userSetting.CountLastTaradods = int.Parse(TbxCountLastTaradods.Value.ToString());
			userSetting.viewGridReportDevices = ChkViewGridReportDevices.Checked;
			userSetting.CountReportDevices = int.Parse(TbxCountReportDevices.Value.ToString());
			Utility.SetLanguage((LanguageTypes)short.Parse(((ListControl)CmbCalendarType).SelectedValue.ToString()));
			LanguageTypes language = Utility.GetLanguage();
			if ((int)language != 4)
			{
				LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageFarsiTimeAttendance, true, (ENSource)5);
			}
			else
			{
				LanguageAndCalendar.Instance.ChangeLanguage(language, TextResourceManagement.TextLanguageArabicTimeAttendance, true, (ENSource)5);
			}
			userSetting.ShowFrmEnter = CBGetPassAfterMaximize.Checked;
			await UserAttendance.SelectedUser.SaveAsync();
			lin = 2;
			setting.SaleKari.TitleSalekari = ((Control)TxtCompany).Text;
			lin = 3;
			setting.SaleKari.Tel = ((Control)TxtTel).Text;
			lin = 4;
			setting.SaleKari.Address = ((Control)TxtADD).Text;
			lin = 5;
			setting.SaleKari.Fax = ((Control)TxtFax).Text;
			lin = 6;
			setting.SaleKari.Desc = ((Control)TxtShrh).Text;
			lin = 7;
			setting.SaleKari.CodeEkhtesasi = ((Control)TxtCodeEkhtesasi).Text;
			lin = 8;
			setting.SaleKari.SabtNo = ((Control)TxtSabt).Text;
			((AttDepartmentAdminSms)setting.Payamak).EnablePayamak = CBPayamakEnable.Checked;
			((AttDepartmentAdminSms)setting.Payamak).SendVorodManage = ChbSmsVManage.Checked;
			((AttDepartmentAdminSms)setting.Payamak).SendKhorojManage = ChbSmsKHManage.Checked;
			setting.Payamak.SendVorodValed = ChbSMSVorodValed.Checked;
			setting.Payamak.SendKhorojValed = ChbSMSKHValed.Checked;
			((AttDepartmentAdminSms)setting.Payamak).PayamakManageVorod = ((Control)TxtsSmsManageV).Text;
			((AttDepartmentAdminSms)setting.Payamak).PayamakManageKhoroj = ((Control)TxtsSmsManageKH).Text;
			setting.Payamak.PayamakValedVorod = ((Control)TxtSmsValedV).Text;
			setting.Payamak.PayamakValedKhoroj = ((Control)TxtSmsValedKh).Text;
			((AttDepartmentAdminSms)setting.Payamak).IsSendSmsAbsentManager = ChbSMSAbsentManager.Checked;
			setting.Payamak.IsSendSmsAbsentPerson = ChbSMSAbsentPerson.Checked;
			setting.Payamak.IsSendSmsVacationManager = ChbSendSmsVacationManager.Checked;
			((AttDepartmentAdminSms)setting.Payamak).IsSendSmsIncompleteManager = chkBxIncompleteManager.Checked;
			setting.Payamak.IsSendSmsIncompletePerson = chkBxIncompletePerson.Checked;
			((AttDepartmentAdminSms)setting.Payamak).SmsAbsentManager = ((Control)TxtSmsAbsentManager).Text;
			setting.Payamak.SmsAbsentPerson = ((Control)TxtSmsAbsentPerson).Text;
			((AttDepartmentAdminSms)setting.Payamak).SmsIncompleteManager = ((Control)TxtSmsIncompleteManager).Text;
			setting.Payamak.SmsIncompletePerson = ((Control)TxtSmsIncompletePerson).Text;
			((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs = ((Control)TxtAdminMobs).Text;
			((AttDepartmentAdminSms)setting.Payamak).SendSmsFromClockOfDay = NumSendSmsFromClock.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendSmsToClockOfDay = NumSendSmsToClock.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsFromClock = NumSendIncompleteSmsFromClock.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsToClock = NumSendIncompleteSmsToClock.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendTaradodSmsMaxMinutes = NumSendTaradodSmsMaxMinutes.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMinMinutesAfterShift = NumSendAbsentSmsMinMinutesAfterShift.Value;
			((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMaxMinutesAfterShift = NumSendAbsentSmsMaxMinutesAfterShift.Value;
			if ((TbxBackupTimeBetween.Value < 10m) & CHEnableAutoBackup.Checked)
			{
				lin = 32;
				Utility.MessageBoxShowError("[حداقل زمان پشتیبان گیری باید 10 دقیقه باشد]", ((CacheBaseForm)this).ContextGuid);
				lin = 33;
				((Control)TbxBackupTimeBetween).Focus();
				lin = 34;
				TbxBackupTimeBetween.Value = 10m;
				lin = 35;
				return;
			}
			lin = 36;
			AccGlobalSettings val = await LocalSettingBusiness.GetSettingAsync(((CacheBaseForm)this).ContextGuid, default(CancellationToken), "");
			if (val != null)
			{
				val.Others.BackUpSettings.EnableAutoBackup = CHEnableAutoBackup.Checked;
				int num = (int)TbxBackupTimeBetween.Value;
				if (num < 10)
				{
					num = 10;
				}
				val.Others.BackUpSettings.BackupTimeBetween = num;
				bool flag = false;
				if (ucServiceManager1 != null && ucServiceManager1.Config != null)
				{
					if (val.Payamak.BaseHubIp != ucServiceManager1.Config.Ip || val.Payamak.BaseHubPort != ucServiceManager1.Config.Port || val.Payamak.ServerConnectionString != ucServiceManager1.ConnectionString)
					{
						val.Payamak.BaseHubIp = ucServiceManager1.Config.Ip;
						val.Payamak.BaseHubPort = ucServiceManager1.Config.Port;
						val.Payamak.ServerConnectionString = ucServiceManager1.ConnectionString;
						flag = true;
					}
					new Waiter("ذخیره اطلاعات سرویس پیامک", (Form)(object)this, (Task)ExtensionMethods.SaveAsync(val, ((CacheBaseForm)this).ContextGuid, true, (short)10, default(CancellationToken), ""), false, (EnWaiterType)0, (Control)null, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs");
				}
				if (flag)
				{
					await PayamakServiceContainter.InitializeServiceAsync(((CacheBaseForm)this).ContextGuid, (CacheBaseForm)(object)this);
					Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "تنظیمات جدید سرویس پیامک اعمال شد", "", 3);
				}
			}
			lin = 37;
			BackupSetting.DefaultPath = ((Control)TPBackup_DefaultPath).Text;
			lin = 39;
			BackupSetting.EnableAutoBackupAllPartions = CHEnableAutoBackupAllPartions.Checked;
			lin = 40;
			setting.MaxTedadOlaadInSalary = (int)txtMaxTedadOlaad.Value;
			lin = 43;
			setting.ZaribEzafeDay = Utilities.ParseToInt(((Control)ZaribEzafeDay).Text, 0);
			setting.ZaribFriday = Utilities.ParseToInt(((Control)ZaribFriday).Text, 0);
			setting.ZaribMinesBimeOfTax = Utilities.ParseToInt(((Control)ZaribMinesBimeOfTax).Text, 0);
			setting.ZaribTatil = Utilities.ParseToInt(((Control)ZaribTatil).Text, 0);
			lin = 44;
			setting.AutoEzafe = CHAutoEzafe.Checked;
			setting.CodeingAccounting.IsCalcHoliday = ChkCalcHoliday.Checked;
			setting.AutomaticCalcMovetoNextDay = ChkAutomaticCalcMoveToNextDay.Checked;
			setting.CalcEkInSourceDate = ChkCalcEkInSourceDate.Checked;
			setting.CalcEkWhenHasVacationOrMission = ChkCalcEkWhenHasVacationOrMission.Checked;
			setting.NotCalkEK_KK = ChkCalcEK_KK.Checked;
			if (string.IsNullOrEmpty(NumGetTaradodFormCountDaysAgo.Value.ToString().Trim()))
			{
				setting.GetTaradodFormCountDaysAgo = 365;
				NumGetTaradodFormCountDaysAgo.Value = 365m;
			}
			else
			{
				setting.GetTaradodFormCountDaysAgo = int.Parse(NumGetTaradodFormCountDaysAgo.Value.ToString());
			}
			setting.CalcEzafeOutOfShift = ChkCalcEzafeOutOfShift.Checked;
			string text = Text.RemoveNoNumbers(((Control)TxtRoundBelow).Text, "");
			setting.RoundBelowPrice = int.Parse(string.IsNullOrEmpty(text) ? "1" : text);
			setting.NotKarkardWhenNoGharardad = ChkNotKarkardWhenNoGharardad.Checked;
			lin = 45;
			setting.LaborLaw.DailyWage = Utilities.ParseToDecimal(((Control)txtDailyWage).Text, 0m, 0);
			setting.LaborLaw.HaghMaskan = Utilities.ParseToDecimal(((Control)txtHaghMaskan).Text, 0m, 0);
			setting.LaborLaw.HaghSpouse = Utilities.ParseToDecimal(((Control)txtHaghSpouse).Text, 0m, 0);
			lin = 46;
			setting.LaborLaw.Bon = Utilities.ParseToDecimal(((Control)txtBon).Text, 0m, 0);
			setting.LaborLaw.HaghTahol = Utilities.ParseToDecimal(((Control)TxtHaghTahol).Text, 0m, 0);
			setting.LaborLaw.OvertimeCoefficient = (Utilities.ParseToDecimal(((Control)ZaribEzafeDay).Text, 0m, 0) + 100m) / 100m;
			lin = 47;
			setting.CodeingAccounting.Additions1 = _tmpAddition1;
			lin = 48;
			setting.CodeingAccounting.Fraction1 = _tmpFraction1;
			lin = 49;
			setting.CodeingAccounting.Fraction2 = _tmpFraction2;
			lin = 50;
			setting.CodeingAccounting.Fraction3 = _tmpFraction3;
			lin = 51;
			lin = 52;
			CodeingAccounting codeingAccounting = setting.CodeingAccounting;
			ReturnedSaveFuncInfo val2 = new ReturnedSaveFuncInfo();
			lin = 53;
			codeingAccounting.MoeinBaseSalary = (long.TryParse(((Control)TxtMoeinBaseSalary).Text, out var result) ? result : (-1));
			if (codeingAccounting.MoeinBaseSalary == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین حقوق پایه نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 655);
			}
			lin = 54;
			codeingAccounting.TafsilBaseSalary = (long.TryParse(((Control)TxtTafsilBaseSalary).Text, out var result2) ? result2 : (-1));
			if (codeingAccounting.TafsilBaseSalary == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل حقوق پایه نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 660);
			}
			lin = 55;
			codeingAccounting.InsuranceBaseSalary = ChBxInsuranceBaseSalary.Checked;
			lin = 56;
			codeingAccounting.TaxBaseSalary = ChBxTaxBaseSalary.Checked;
			codeingAccounting.MoeinMission = (long.TryParse(((Control)TxtMoeinMission).Text, out var result3) ? result3 : (-1));
			if (codeingAccounting.MoeinMission == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین حق ماموریت نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 669);
			}
			lin = 54;
			codeingAccounting.TafsilMission = (long.TryParse(((Control)TxtTafsilMission).Text, out var result4) ? result4 : (-1));
			if (codeingAccounting.TafsilMission == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل حق ماموریت نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 674);
			}
			lin = 55;
			codeingAccounting.InsuranceMission = ChBxInsuranceMission.Checked;
			lin = 56;
			codeingAccounting.TaxMission = ChBxTaxMission.Checked;
			lin = 57;
			codeingAccounting.MoeinOverTime = (long.TryParse(((Control)TxtMoeinOverTime).Text, out var result5) ? result5 : (-1));
			if (codeingAccounting.MoeinOverTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین اضافه کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 684);
			}
			lin = 58;
			codeingAccounting.TafsilOverTime = (long.TryParse(((Control)TxtTafsilOverTime).Text, out var result6) ? result6 : (-1));
			if (codeingAccounting.TafsilOverTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل اضافه کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 689);
			}
			lin = 59;
			codeingAccounting.InsuranceOverTime = ChBxInsuranceOverTime.Checked;
			lin = 60;
			codeingAccounting.TaxOverTime = ChBxTaxOverTime.Checked;
			lin = 61;
			codeingAccounting.MoeinShiftTime = (long.TryParse(((Control)TxtMoeinShiftTime).Text, out var result7) ? result7 : (-1));
			if (codeingAccounting.MoeinShiftTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین نوبت کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 699);
			}
			lin = 62;
			codeingAccounting.TafsilShiftTime = (long.TryParse(((Control)TxtTafsilShiftTime).Text, out var result8) ? result8 : (-1));
			if (codeingAccounting.TafsilShiftTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل نوبت کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 704);
			}
			lin = 63;
			codeingAccounting.InsuranceShiftTime = ChBxInsuranceShiftTime.Checked;
			lin = 64;
			codeingAccounting.TaxShiftTime = ChBxTaxShiftTime.Checked;
			lin = 65;
			codeingAccounting.MoeinHolidayTime = (long.TryParse(((Control)TxtMoeinHolidayTime).Text, out var result9) ? result9 : (-1));
			if (codeingAccounting.MoeinHolidayTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین جمعه کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 714);
			}
			lin = 66;
			codeingAccounting.TafsilHolidayTime = (long.TryParse(((Control)TxtTafsilHolidayTime).Text, out var result10) ? result10 : (-1));
			if (codeingAccounting.TafsilHolidayTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل جمعه کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 719);
			}
			codeingAccounting.InsuranceHolidayTime = ChBxInsuranceHolidayTime.Checked;
			codeingAccounting.TaxHolidayTime = ChBxTaxHolidayTime.Checked;
			codeingAccounting.MoeinTatilKari = (long.TryParse(((Control)TxtMoeinTatilKari).Text, out var result11) ? result11 : (-1));
			if (codeingAccounting.MoeinTatilKari == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین تعطیل کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 725);
			}
			codeingAccounting.TafsilTatilKari = (long.TryParse(((Control)TxtTafsilTatilKari).Text, out var result12) ? result12 : (-1));
			if (codeingAccounting.TafsilTatilKari == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل تعطیل کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 729);
			}
			codeingAccounting.InsuranceTatilKari = ChBxInsuranceTatilKari.Checked;
			codeingAccounting.TaxTatilKari = ChBxTaxTatilKari.Checked;
			codeingAccounting.MoeinNightTime = (long.TryParse(((Control)TxtMoeinNightTime).Text, out var result13) ? result13 : (-1));
			if (codeingAccounting.MoeinNightTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین شب کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 735);
			}
			lin = 70;
			codeingAccounting.TafsilNightTime = (long.TryParse(((Control)TxtTafsilNightTime).Text, out var result14) ? result14 : (-1));
			if (codeingAccounting.TafsilNightTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل شب کاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 740);
			}
			lin = 71;
			codeingAccounting.InsuranceNightTime = ChBxInsuranceNightTime.Checked;
			lin = 72;
			codeingAccounting.TaxNightTime = ChBxTaxNightTime.Checked;
			lin = 73;
			codeingAccounting.MoeinVacation = (long.TryParse(((Control)TxtMoeinVacation).Text, out var result15) ? result15 : (-1));
			if (codeingAccounting.MoeinVacation == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین مرخصی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 749);
			}
			lin = 74;
			codeingAccounting.TafsilVacation = (long.TryParse(((Control)TxtTafsilVacation).Text, out var result16) ? result16 : (-1));
			if (codeingAccounting.TafsilVacation == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل مرخصی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 754);
			}
			lin = 75;
			codeingAccounting.InsuranceVacation = ChBxInsuranceVacation.Checked;
			lin = 76;
			codeingAccounting.TaxVacation = ChBxTaxVacation.Checked;
			lin = 77;
			codeingAccounting.MoeinLeaveTime = (long.TryParse(((Control)TxtMoeinLeaveTime).Text, out var result17) ? result17 : (-1));
			if (codeingAccounting.MoeinLeaveTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[FrmSettingStr115]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 763);
			}
			lin = 78;
			codeingAccounting.TafsilLeaveTime = (long.TryParse(((Control)TxtTafsilLeaveTime).Text, out var result18) ? result18 : (-1));
			if (codeingAccounting.TafsilLeaveTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل کسر کار  نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 768);
			}
			lin = 79;
			codeingAccounting.InsuranceLeaveTime = ChBxInsuranceLeaveTime.Checked;
			lin = 80;
			codeingAccounting.TaxLeaveTime = ChBxTaxLeaveTime.Checked;
			codeingAccounting.MoeinJarime = (long.TryParse(((Control)TxtMoeinJarime).Text, out var result19) ? result19 : (-1));
			if (codeingAccounting.MoeinJarime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین جریمه تاخیر نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 776);
			}
			lin = 78;
			codeingAccounting.TafsilJarime = (long.TryParse(((Control)TxtTafsilJarime).Text, out var result20) ? result20 : (-1));
			if (codeingAccounting.TafsilJarime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل جریمه تاخیر  نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 781);
			}
			lin = 79;
			codeingAccounting.InsuranceJarime = ChBxInsuranceJarime.Checked;
			lin = 80;
			codeingAccounting.TaxJarime = ChBxTaxJarime.Checked;
			lin = 81;
			codeingAccounting.MoeinAbsentTime = (long.TryParse(((Control)TxtMoeinAbsentTime).Text, out var result21) ? result21 : (-1));
			if (codeingAccounting.MoeinAbsentTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین غیبت نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 790);
			}
			lin = 82;
			codeingAccounting.TafsilAbsentTime = (long.TryParse(((Control)TxtTafsilAbsentTime).Text, out var result22) ? result22 : (-1));
			if (codeingAccounting.TafsilAbsentTime == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل غیبت نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 795);
			}
			lin = 83;
			codeingAccounting.InsuranceAbsentTime = ChBxInsuranceAbsentTime.Checked;
			lin = 84;
			codeingAccounting.TaxAbsentTime = ChBxTaxAbsentTime.Checked;
			lin = 85;
			codeingAccounting.MoeinEmployer = (long.TryParse(((Control)TxtMoeinEmployer).Text, out var result23) ? result23 : (-1));
			if (codeingAccounting.MoeinEmployer == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین بیمه سهم کارفرما نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 805);
			}
			lin = 86;
			codeingAccounting.TafsilEmployer = (long.TryParse(((Control)TxtTafsilEmployer).Text, out var result24) ? result24 : (-1));
			if (codeingAccounting.TafsilEmployer == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل بیمه سهم کارفرما نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 810);
			}
			lin = 87;
			codeingAccounting.MoeinUnEmployMent = (long.TryParse(((Control)TxtMoeinUnEmployMent).Text, out var result25) ? result25 : (-1));
			if (codeingAccounting.MoeinUnEmployMent == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین بیمه بیکاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 816);
			}
			lin = 88;
			codeingAccounting.TafsilUnEmployMent = (long.TryParse(((Control)TxtTafsilUnEmployMent).Text, out var result26) ? result26 : (-1));
			if (codeingAccounting.TafsilUnEmployMent == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل بیمه بیکاری نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 821);
			}
			lin = 89;
			codeingAccounting.MoeinTaminGov = (long.TryParse(((Control)TxtMoeinTaminGov).Text, out var result27) ? result27 : (-1));
			if (codeingAccounting.MoeinTaminGov == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین تامین اجتماعی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 827);
			}
			lin = 90;
			codeingAccounting.TafsilTaminGov = (long.TryParse(((Control)TxtTafsilTaminGov).Text, out var result28) ? result28 : (-1));
			if (codeingAccounting.TafsilTaminGov == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل تامین اجتماعی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 832);
			}
			lin = 91;
			codeingAccounting.MoeinTaxGov = (long.TryParse(((Control)TxtMoeinTaxGov).Text, out var result29) ? result29 : (-1));
			if (codeingAccounting.MoeinTaxGov == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین اداره مالیات نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 838);
			}
			lin = 92;
			codeingAccounting.TafsilTaxGov = (long.TryParse(((Control)TxtTafsilTaxGov).Text, out var result30) ? result30 : (-1));
			if (codeingAccounting.TafsilTaxGov == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل اداره مالیات نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 843);
			}
			lin = 93;
			codeingAccounting.MoeinRoundFish = (long.TryParse(((Control)TxtMoeinRoundFish).Text, out var result31) ? result31 : (-1));
			if (codeingAccounting.MoeinRoundFish == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد معین کسر ریال فیش حقوقی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 849);
			}
			lin = 92;
			codeingAccounting.TafsilRoundFish = (long.TryParse(((Control)TxtTafsilRoundFish).Text, out var result32) ? result32 : (-1));
			if (codeingAccounting.TafsilRoundFish == -1)
			{
				val2.AddReturnedValue((ReturnedState)2, "[کد تفصیل کسر ریال فیش حقوقی نامعتبر است]", "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 854);
			}
			if (val2.HasError && (int)(await ((CacheBaseForm)new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, val2, "[خطای ورود داده]", (MessageBoxButtons)3, false, (LanguageTypes)0, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs")).ShowAsync((IWin32Window)(object)this)) != 6)
			{
				return;
			}
			setting.IsRunAppAfterLogOnPerson = chBxGetFileName.Checked;
			lin = 94;
			setting.AppRunAfterLogOnPerson = ((Control)txtFileName).Text;
			lin = 95;
			setting.FontProgram = ((Control)TxtFont).Font;
			lin = 96;
			lin = 41;
			setting.GharardadEndedAgo = (int)NUMGharardadEndedAgo.Value;
			setting.GharardadEndedNext = (int)NUMGharardadEndedNext.Value;
			setting.ViewEndedGharardad = ChkViewEndedGharardad.Checked;
			setting.FromDateEndedGharardad = ChkFromDateEndedGharardad.Checked;
			List<GharardadStatus> list = new List<GharardadStatus>();
			foreach (CheckBox item in (ArrangedElementCollection)((Control)flowLayoutPanel1).Controls)
			{
				CheckBox chkBox = item;
				if (chkBox.Checked)
				{
					list.Add(_gharardadStatuses.FirstOrDefault((GharardadStatus p) => p.Name == ((Control)chkBox).Text));
				}
			}
			setting.GharardadStatuses = list;
			string obj = await LogDataAsync();
			lin = 97;
			if (obj[1] == '1')
			{
				DialogResult val3 = await PayamakValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, setting.Payamak, checkPayamakWeb: false);
				lin = 103;
				if ((int)val3 != 6)
				{
					lin = 104;
					if ((int)val3 == 2)
					{
						lin = 105;
						((Form)this).Close();
						lin = 106;
						return;
					}
					lin = 107;
					if ((int)val3 == 4)
					{
						return;
					}
				}
				lin = 108;
			}
			lin = 109;
			FishHesab obj2 = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, 1);
			obj2.Account = setting.LaborLaw.HaghMaskan;
			await obj2.SaveAsync();
			FishHesab obj3 = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, 2);
			obj3.Account = setting.LaborLaw.HaghSpouse;
			await obj3.SaveAsync();
			FishHesab obj4 = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, 3);
			obj4.Account = setting.LaborLaw.Bon;
			await obj4.SaveAsync();
			FishHesab obj5 = await FishHesab.GetAsync(((CacheBaseForm)this).ContextGuid, 7);
			obj5.Account = setting.LaborLaw.HaghTahol;
			await obj5.SaveAsync();
			lin = 115;
			setting.WebServiceKey.URL = ((Control)CmbURL).Text.Trim();
			new Waiter("در حال ذخیره اطلاعات", (Form)(object)this, (Task)setting.SaveAsync(((CacheBaseForm)this).ContextGuid), false, (EnWaiterType)0, (Control)null, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs");
			bool serviceSettingChanged = false;
			DeviceServiceSettings val4 = await LocalSettingBusiness.GetDeviceServiceSettingsAsync(((CacheBaseForm)this).ContextGuid, default(CancellationToken), true);
			if (val4.HandleService != chbHandleDeviceService.Checked)
			{
				val4.HandleService = chbHandleDeviceService.Checked;
				serviceSettingChanged = true;
			}
			UcServiceManager obj6 = ucDeviceService;
			if (((obj6 != null) ? obj6.Config : null) != null && (val4.BaseHubIp != ucDeviceService.Config.Ip || val4.BaseHubPort != ucDeviceService.Config.Port))
			{
				val4.BaseHubIp = ucDeviceService.Config.Ip;
				val4.BaseHubPort = ucDeviceService.Config.Port;
				serviceSettingChanged = true;
			}
			if (serviceSettingChanged && !(await LocalSettingBusiness.SetDeviceServiceSettingsAsync(((CacheBaseForm)this).ContextGuid, val4)).HasError)
			{
				await DeviceServiceHandler.GetInstance(((CacheBaseForm)this).ContextGuid, (CacheBaseForm)(object)this).InitializeCommunicationDevice(VersionManagment.Version, default(CancellationToken));
			}
			string translate = Utilities.GetTranslate("[تنظیمات با موفقیت ثبت شد]");
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, translate, "", 3);
			((Form)this).DialogResult = (DialogResult)1;
			Task.Run((Func<Task?>)ReconnectToHostAsync);
		}
		catch (Exception ex)
		{
			WebErrorLog errorLogInstance = WebErrorLog.ErrorLogInstance;
			Exception ex2 = ex;
			object arg = lin;
			errorLogInstance.StartLog(ex2, $" the line no {arg}, ret is {await LogDataAsync()}", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 980);
		}
		finally
		{
			((Control)BtnAccept).Enabled = true;
		}
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void PayamakClientHubInstances_OnHubChanged(object sender, HubConnectionState e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected I4, but got Unknown
		try
		{
			switch ((int)e)
			{
			case 0:
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "ارتباط سرویس پیامک برقرار نیست", "", 3);
				break;
			case 2:
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "در حال ارتباط با سرویس پیامک", "", 3);
				break;
			case 1:
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "ارتباط با سرویس پیامک برقرار شد", "", 3);
				break;
			case 3:
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "در حال تلاش برای ارتباط مجدد با سرویس پیامک", "", 3);
				break;
			default:
				throw new NotImplementedException();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task ReconnectToHostAsync()
	{
		try
		{
			_signalRToken?.Cancel();
			_signalRToken = new CancellationTokenSource();
			CancellationToken token = _signalRToken.Token;
			await EntityCacheConfig.ConnectSignalRAsync(((CacheBaseForm)this).ContextGuid, SystemInfoClass.SystemInfo(), VersionManagment.Version, (EnConnectionType)1, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReconnectToHostAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1011);
		}
	}

	private async Task<string> LogDataAsync()
	{
		string result = "";
		try
		{
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			string text = "[اصلاح اطلاعات شرکت : ]";
			text = text + "[ اجرای اتومات در هنگام روشن شدن کامپیوتر : ]" + timeAttendanceSettings.RunInStart;
			text = text + "\n [عنوان شرکت]" + timeAttendanceSettings.SaleKari.TitleSalekari;
			text = text + "\n [شماره تلفن : ]" + timeAttendanceSettings.SaleKari.Tel;
			text = text + "\n [آدرس]:" + timeAttendanceSettings.SaleKari.Address;
			text = text + "\n [فکس : ]" + timeAttendanceSettings.SaleKari.Fax;
			text = text + "\n [شرح]:" + timeAttendanceSettings.SaleKari.Desc;
			text = text + "\n [کد اقتصادی]:" + timeAttendanceSettings.SaleKari.CodeEkhtesasi;
			text = text + "\n [شماره ثبت]:" + timeAttendanceSettings.SaleKari.SabtNo;
			result += ((text != _descSaleKari) ? "1" : "0");
			_descSaleKari = Utilities.GetTranslate(text);
			string text2 = "[اصلاح اطلاعات پیامک : ]";
			text2 = text2 + "[ فعال کردن پیامک : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).EnablePayamak;
			text2 = text2 + "\n [پیام ورود به مدیر فعال باشد : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).SendVorodManage;
			text2 = text2 + "\n [پیام خروج به مدیر فعال باشد : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).SendKhorojManage;
			text2 = text2 + "\n [پیام ورود به کارکنان/ ولی دانش آموز فعال باشد : ]" + timeAttendanceSettings.Payamak.SendVorodValed;
			text2 = text2 + "\n [پیام خروج به کارکنان/ ولی دانش آموز فعال باشد : ]" + timeAttendanceSettings.Payamak.SendKhorojValed;
			text2 = text2 + "\n [پیام ورود به مدیر : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).PayamakManageVorod?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام خروج به مدیر : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).PayamakManageKhoroj?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام ورود به کارکنان/ ولی دانش آموز : ]" + timeAttendanceSettings.Payamak.PayamakValedVorod?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام خروج به کارکنان/ ولی دانش آموز : ]" + timeAttendanceSettings.Payamak.PayamakValedKhoroj?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام غیبت به مدیر فعال باشد : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).IsSendSmsAbsentManager;
			text2 = text2 + "\n [پیام غیبت به کارکنان/ ولی دانش آموز فعال باشد : ]" + timeAttendanceSettings.Payamak.IsSendSmsAbsentPerson;
			text2 = text2 + "\n [پیام تردد ناقص به مدیر فعال باشد : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).IsSendSmsIncompleteManager;
			text2 = text2 + "\n [پیام تردد ناقص به کارکنان/ ولی دانش آموز فعال باشد : ]" + timeAttendanceSettings.Payamak.IsSendSmsIncompletePerson;
			text2 = text2 + "\n [پیام غیبت به مدیر : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).SmsAbsentManager?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام غیبت به کارکنان/ ولی دانش آموز : ]" + timeAttendanceSettings.Payamak.SmsAbsentPerson?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام تردد ناقص به مدیر : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).SmsIncompleteManager?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [پیام تردد ناقص به کارکنان/ ولی دانش آموز : ]" + timeAttendanceSettings.Payamak.SmsIncompletePerson?.Replace("[", "<").Replace("]", ">");
			text2 = text2 + "\n [شماره تلفن مدیر : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).PayamakManageAdminMobs;
			text2 = text2 + "\n [بعد از چند دقیقه پیامک غیبت ارسال شود : ]" + ((AttDepartmentAdminSms)timeAttendanceSettings.Payamak).SendAbsentSmsMinMinutesAfterShift;
			result += ((text2 != _descSms) ? "1" : "0");
			_descSms = Utilities.GetTranslate(text2);
			string text3 = "[پشتیبان گیری زمانبندی فعال باشد]:" + CHEnableAutoBackup.Checked;
			text3 = text3 + "\n [مسیر تهیه پشتیبان]:" + BackupSetting.DefaultPath;
			text3 = text3 + "\n [پشتیبان گیری پس از ... دقیقه :]" + TbxBackupTimeBetween.Value;
			text3 = text3 + "\n [پشتیبان گیری اتوماتیک هنگام خروج از نرم افزار فعال باشد ?]" + BackupSetting.EnableAutoBackupAllPartions;
			result += ((text3 != _descBackup) ? "1" : "0");
			_descBackup = Utilities.GetTranslate(text3);
			string text4 = "[اصلاح اطلاعات حقوقی: ]";
			text4 = text4 + "\n [ضریب اضافه کاری : ]" + timeAttendanceSettings.ZaribEzafeDay;
			text4 = text4 + "\n [ضریب جمعه کاری : ]" + timeAttendanceSettings.ZaribFriday;
			text4 = text4 + "\n ضریب کسر بیمه از مالیات" + timeAttendanceSettings.ZaribMinesBimeOfTax;
			text4 = text4 + "\n [ضریب تعطیل کاری : ]" + timeAttendanceSettings.ZaribTatil;
			text4 = text4 + "\n [اضافه کاری به صورت اتوماتیک لحاظ شود]:" + timeAttendanceSettings.AutoEzafe;
			text4 = text4 + "\n [حداکثر تعداد فرزندان که شخص میتواند در یک قرارداد بعنوان حق اولاد بگیرد]:" + timeAttendanceSettings.MaxTedadOlaadInSalary;
			text4 = text4 + "\n [حداقل دستمزد روزانه]:" + timeAttendanceSettings.LaborLaw.DailyWage;
			text4 = text4 + "\n [حساب انتخابی مزایا]:" + timeAttendanceSettings.CodeingAccounting.Additions1;
			text4 = text4 + "\n [حساب انتخابی کسورات 1]:" + timeAttendanceSettings.CodeingAccounting.Fraction1;
			text4 = text4 + "\n [حساب انتخابی کسورات 2]:" + timeAttendanceSettings.CodeingAccounting.Fraction2;
			text4 = text4 + "\n [حساب انتخابی کسورات 3]:" + timeAttendanceSettings.CodeingAccounting.Fraction3;
			result += ((text4 != _descSalary) ? "1" : "0");
			_descSalary = Utilities.GetTranslate(text4);
			CodeingAccounting codeingAccounting = timeAttendanceSettings.CodeingAccounting;
			string text5 = "[اصلاح اطلاعات سند اتومات حقوق : ]";
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حقوق پایه(کد معین) :]" + codeingAccounting.MoeinBaseSalary;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حقوق پایه(کد تفصیل) :]" + codeingAccounting.TafsilBaseSalary;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حق ماموریت(کد معین) :]" + codeingAccounting.MoeinMission;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حق ماموریت(کد تفصیل) :]" + codeingAccounting.TafsilMission;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حقوق پایه(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceBaseSalary;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه حقوق پایه(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxBaseSalary;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه اضافه کاری(کد معین) :]" + codeingAccounting.MoeinOverTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه اضافه کاری(کد تفصیل) :]" + codeingAccounting.TafsilOverTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه اضافه کاری(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceOverTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه اضافه کاری(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxOverTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه نوبت کاری(کد معین) :]" + codeingAccounting.MoeinShiftTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه نوبت کاری(کد تفصیل) :]" + codeingAccounting.TafsilShiftTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه نوبت کاری(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceShiftTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه نوبت کاری(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxShiftTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه جمعه کاری(کد معین) :]" + codeingAccounting.MoeinHolidayTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه جمعه کاری(کد تفصیل) :]" + codeingAccounting.TafsilHolidayTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه تعطیل کاری(کد معین) :]" + codeingAccounting.MoeinTatilKari;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه تعطیل کاری(کد تفصیل) :]" + codeingAccounting.TafsilTatilKari;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه جمعه کاری(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceHolidayTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه جمعه کاری(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxHolidayTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه شب کاری(کد معین) :]" + codeingAccounting.MoeinNightTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه شب کاری(کد تفصیل) :]" + codeingAccounting.TafsilNightTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه شب کاری(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceNightTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه شب کاری(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxNightTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه مرخصی(کد معین) :]" + codeingAccounting.MoeinVacation;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه مرخصی(کد تفصیل) :]" + codeingAccounting.TafsilVacation;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه مرخصی(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceVacation;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه مرخصی(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxVacation;
			text5 = text5 + "\n [طرف حساب بدهکار کسر کار (کد معین) :]" + codeingAccounting.MoeinLeaveTime;
			text5 = text5 + "\n [طرف حساب بدهکار جریمه تاخیر (کد تفصیل) :]" + codeingAccounting.TafsilLeaveTime;
			text5 = text5 + "\n [طرف حساب بدهکار جریمه تاخیر (کد معین) :]" + codeingAccounting.MoeinJarime;
			text5 = text5 + "\n [طرف حساب بدهکار کسر کار (کد تفصیل) :]" + codeingAccounting.TafsilJarime;
			text5 = text5 + "\n [طرف حساب بدهکار کسر کار (آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceLeaveTime;
			text5 = text5 + "\n [طرف حساب بدهکار کسر کار (آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxLeaveTime;
			text5 = text5 + "\n [طرف حساب بدهکار غیبت(کد معین) :]" + codeingAccounting.MoeinAbsentTime;
			text5 = text5 + "\n [طرف حساب بدهکار غیبت(کد تفصیل) :]" + codeingAccounting.TafsilAbsentTime;
			text5 = text5 + "\n [طرف حساب بدهکار غیبت(آیا مشمول بیمه باشد؟) :]" + codeingAccounting.InsuranceAbsentTime;
			text5 = text5 + "\n [طرف حساب بدهکار غیبت(آیا مشمول مالیات باشد؟) :]" + codeingAccounting.TaxAbsentTime;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه بیمه سهم کارفرما(کد معین) :]" + codeingAccounting.MoeinEmployer;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه بیمه سهم کارفرما(کد تفصیل) :]" + codeingAccounting.TafsilEmployer;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه بیمه بیکاری(کد معین) :]" + codeingAccounting.MoeinUnEmployMent;
			text5 = text5 + "\n [طرف حساب بدهکار هزینه بیمه بیکاری(کد تفصیل) :]" + codeingAccounting.TafsilUnEmployMent;
			text5 = text5 + "\n [طرف حساب بستانکار سازمان تامین اجتماعی(کد معین) :]" + codeingAccounting.MoeinTaminGov;
			text5 = text5 + "\n [طرف حساب بستانکار سازمان تامین اجتماعی(کد تفصیل) :]" + codeingAccounting.TafsilTaminGov;
			text5 = text5 + "\n [طرف حساب بستانکار اداره مالیات(کد معین) :]" + codeingAccounting.MoeinTaxGov;
			text5 = text5 + "\n [طرف حساب بستانکار اداره مالیات(کد تفصیل) :]" + codeingAccounting.TafsilTaxGov;
			result += ((text5 != _descAccounting) ? "1" : "0");
			_descAccounting = Utilities.GetTranslate(text5);
			string text6 = "[اصلاح اطلاعات  : ]";
			text6 = text6 + "\n [پس از دریافت برنامه اجرا شود?]" + timeAttendanceSettings.IsRunAppAfterLogOnPerson;
			text6 = text6 + "\n [نام فایل اجرایی]:" + timeAttendanceSettings.AppRunAfterLogOnPerson;
			result += ((text6 != _descExtraSetting) ? "1" : "0");
			_descExtraSetting = Utilities.GetTranslate(text6);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LogDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1263);
		}
		return result;
	}

	private void BtnMore_Click_1(object sender, EventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (!((Control)BtnMore).Enabled || !((Control)BtnMore).Visible)
		{
			return;
		}
		try
		{
			if ((int)((CommonDialog)FD).ShowDialog() == 1)
			{
				((Control)TxtFont).Font = FD.Font;
				((Control)TxtFont).Text = FD.Font.Name;
				((Control)LBLPreviwe).Font = FD.Font;
				((Control)BtnMore).Enabled = true;
			}
			((Control)BtnMore).Enabled = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMore_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1287);
		}
	}

	private async void BtnMoreSms_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)TxtSamaneSMS).Focus();
			FRMAccountMain frm = new FRMAccountMain(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				((AttDepartmentAdminSms)(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).Payamak).IdSamane = ((BaseSmsNetAccount)frm.SelectedAcount).Id;
				((Control)TxtSamaneSMS).Text = ((BaseSmsNetAccount)frm.SelectedAcount).Title;
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreSms_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1311);
		}
	}

	private async void BtnDeleteAcc_Click(object sender, EventArgs e)
	{
		try
		{
			((AttDepartmentAdminSms)(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).Payamak).IdSamane = 0;
			((Control)TxtSamaneSMS).Text = "";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDeleteAcc_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1323);
		}
	}

	private void ChbSmsVManage_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtsSmsManageV).Enabled = ChbSmsVManage.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSmsVManage_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1332);
		}
	}

	private void ChbSmsKHManage_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtsSmsManageKH).Enabled = ChbSmsKHManage.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSmsKHManage_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1341);
		}
	}

	private void ChbSMSVorodValed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsValedV).Enabled = ChbSMSVorodValed.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSMSVorodValed_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1350);
		}
	}

	private void ChbSMSKHValed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsValedKh).Enabled = ChbSMSKHValed.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSMSKHValed_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1359);
		}
	}

	private void BtnPayamak_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		try
		{
			Control val = (Control)sender;
			string text = "<" + val.Text + ">";
			if (_whichTextBoxClicked != null)
			{
				((Control)_whichTextBoxClicked).Focus();
				SendKeys.Send(text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPayamak_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1375);
		}
	}

	private void BtnCancelAll_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancelAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1388);
		}
	}

	private void chkBxIncompleteManager_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsIncompleteManager).Enabled = chkBxIncompleteManager.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chkBxIncompleteManager_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1400);
		}
	}

	private void chkBxIncompletePerson_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsIncompletePerson).Enabled = chkBxIncompletePerson.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chkBxIncompletePerson_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1412);
		}
	}

	private void TxtsSmsManageV_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			_whichTextBoxClicked = (TextBox)sender;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtsSmsManageV_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1424);
		}
	}

	private void ChbSMSAbsentPerson_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsAbsentPerson).Enabled = ChbSMSAbsentPerson.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSMSAbsentPerson_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1436);
		}
	}

	private void ChbSMSAbsentManager_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSmsAbsentManager).Enabled = ChbSMSAbsentManager.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSMSAbsentManager_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1448);
		}
	}

	private void TPBackup_DefaultPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			Process.Start(((Control)TPBackup_DefaultPath).Text);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TPBackup_DefaultPath_LinkClicked", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1464);
		}
	}

	private void BackupBTNFindPath_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		try
		{
			FolderBrowserDialog val = new FolderBrowserDialog();
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)TPBackup_DefaultPath).Text = val.SelectedPath;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BackupBTNFindPath_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1478);
		}
	}

	private async void btnAdditions1_Click(object sender, EventArgs e)
	{
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 1, _tmpAddition1.ToString());
			try
			{
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				FishHesab selectedHesab = frm.SelectedHesab;
				_tmpAddition1 = selectedHesab.Id;
				((Control)txtAdditions1Moein).Text = selectedHesab.CodeMoein.ToString();
				((Control)txtAdditions1Tafsil).Text = selectedHesab.CodeTafsil.ToString();
				((Control)txtAdditions1Title).Text = selectedHesab.Name;
			}
			finally
			{
				((IDisposable)frm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnAdditions1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1498);
		}
	}

	private async void btnFraction1_Click(object sender, EventArgs e)
	{
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2, _tmpFraction1.ToString());
			try
			{
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				FishHesab selectedHesab = frm.SelectedHesab;
				_tmpFraction1 = selectedHesab.Id;
				((Control)txtFraction1Moein).Text = selectedHesab.CodeMoein.ToString();
				((Control)txtFraction1Tafsil).Text = selectedHesab.CodeTafsil.ToString();
				((Control)txtFraction1Title).Text = selectedHesab.Name;
			}
			finally
			{
				((IDisposable)frm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnFraction1_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1513);
		}
	}

	private async void btnFraction2_Click(object sender, EventArgs e)
	{
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2, _tmpFraction2.ToString());
			try
			{
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				FishHesab selectedHesab = frm.SelectedHesab;
				_tmpFraction2 = selectedHesab.Id;
				((Control)txtFraction2Moein).Text = selectedHesab.CodeMoein.ToString();
				((Control)txtFraction2Tafsil).Text = selectedHesab.CodeTafsil.ToString();
				((Control)txtFraction2Title).Text = selectedHesab.Name;
			}
			finally
			{
				((IDisposable)frm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnFraction2_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1528);
		}
	}

	private async void btnFraction3_Click(object sender, EventArgs e)
	{
		try
		{
			FrmHesabList frm = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2, _tmpFraction3.ToString());
			try
			{
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) != 1)
				{
					return;
				}
				FishHesab selectedHesab = frm.SelectedHesab;
				_tmpFraction3 = selectedHesab.Id;
				((Control)txtFraction3Moein).Text = selectedHesab.CodeMoein.ToString();
				((Control)txtFraction3Tafsil).Text = selectedHesab.CodeTafsil.ToString();
				((Control)txtFraction3Title).Text = selectedHesab.Name;
			}
			finally
			{
				((IDisposable)frm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnFraction3_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1545);
		}
	}

	private async void btnGetLabor_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[آیا تمایل دارید مبالغ حداقل دستمزد روزانه و حق  بن خوار وبار بر اساس قانون کار سال جاری تنظیم شود؟]"), Utilities.GetTranslate("[بروزرسانی...]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Information) != 7)
			{
				UpdateLaborLawDetAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnGetLabor_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1559);
		}
	}

	private async Task UpdateLaborLawDetAsync()
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebLaborLawDet>>> val = await WebLaborLawDet.GetFromApi(default(CancellationToken));
			if (!((ReturnedSaveFuncInfo)val).HasError)
			{
				WebResponse<List<WebLaborLawDet>> value = val.value;
				if (value != null && value.HttpClietnStatusCode == HttpStatusCode.OK)
				{
					WebResponse<List<WebLaborLawDet>> value2 = val.value;
					if (value2 != null && value2.Data?.Count > 0)
					{
						WebLaborLawDet val2 = ((IEnumerable<WebLaborLawDet>)val.value.Data).FirstOrDefault((Func<WebLaborLawDet, bool>)((WebLaborLawDet p) => p.Sal == Convert.ToInt16(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 4))));
						if (val2 != null)
						{
							((Control)txtDailyWage).Text = val2.DailyWage.ToString("N0");
							((Control)txtBon).Text = val2.Bon.ToString("N0");
							((Control)txtHaghMaskan).Text = val2.HaghMaskan.ToString("N0");
							((Control)txtHaghSpouse).Text = val2.HaghSpouse.ToString("N0");
							((Control)TxtHaghTahol).Text = val2.HaghTahol.ToString("N0");
						}
						return;
					}
				}
			}
			Utility.MessageBoxShowError("[بدلیل خطای ارتباطی بروزرسانی انجام نشد.] \r\n " + ((ReturnedSaveFuncInfo)val).ErrorMessage, ((CacheBaseForm)this).ContextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "UpdateLaborLawDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1590);
		}
	}

	private async void btnSearchFileName_Click(object sender, EventArgs e)
	{
		try
		{
			((FileDialog)openFileDialog1).Filter = "Cursor Files|*.exe";
			((FileDialog)openFileDialog1).Title = Utilities.GetTranslate("[انتخاب فایل اجرایی]");
			if ((int)((CommonDialog)openFileDialog1).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((Control)txtFileName).Text = ((FileDialog)openFileDialog1).FileName;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnSearchFileName_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1610);
		}
	}

	private void chBxGetFileName_CheckValueChanged(object sender, EventArgs e)
	{
		try
		{
			TextBox obj = txtFileName;
			bool enabled = (((Control)btnSearchFileName).Enabled = chBxGetFileName.Checked);
			((Control)obj).Enabled = enabled;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chBxGetFileName_CheckValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1622);
		}
	}

	private void ChkNotShowUi_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\service", "NotShowUi", ChkNotShowUi.Checked ? 1 : 0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkNotShowUi_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1634);
		}
	}

	private async Task LoadGharardadStatusItemsAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (_gharardadStatuses == null)
			{
				return;
			}
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			_gharardadStatuses = await GharardadStatus.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
			foreach (GharardadStatus st in _gharardadStatuses)
			{
				CheckBox val = new CheckBox
				{
					Text = st.Name,
					Checked = true
				};
				val.Checked = setting.GharardadStatuses?.Find((GharardadStatus p) => p.Name == st.Name) != null;
				((Control)flowLayoutPanel1).Controls.Add((Control)(object)val);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadGharardadStatusItemsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1652);
		}
	}

	private void CBGetPassAfterMaximize_CheckedChanged(object sender, EventArgs e)
	{
		((Control)NumLockTime).Enabled = CBGetPassAfterMaximize.Checked;
	}

	private void LblLinkWebService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			Process.Start("http://novinp.ir/webservice/webservice.aspx");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LblLinkWebService_LinkClicked", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1666);
		}
	}

	private async void ChkWebService_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)GrpWebservice).Enabled = ChkWebService.Checked;
			UserAttSetting userAttSetting = UserAttendance.SelectedUser?.UserSetting;
			if (userAttSetting != null)
			{
				userAttSetting.EnableCloudSignalR = ChkWebService.Checked;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkWebService_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1678);
		}
	}

	private async void TxtNpUser_TextChanged(object sender, EventArgs e)
	{
		try
		{
			(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).WebServiceKey.NPACCUserName = ((Control)TxtNpUser).Text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void TxtNpPass_TextChanged(object sender, EventArgs e)
	{
		try
		{
			(await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid)).WebServiceKey.NPACCPassword = ((Control)TxtNpPass).Text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void ChkViewGridLastTaradods_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TbxCountLastTaradods).Enabled = ChkViewGridLastTaradods.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkViewGridLastTaradods_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1705);
		}
	}

	private void ChkViewGridReportDevices_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TbxCountReportDevices).Enabled = ChkViewGridReportDevices.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChkViewGridReportDevices_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmSetting.cs", 1713);
		}
	}

	private void ChkAutomaticCalcMoveToNextDay_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (!ChkAutomaticCalcMoveToNextDay.Checked)
			{
				ChkCalcEkInSourceDate.Checked = false;
				((Control)ChkCalcEkInSourceDate).Enabled = false;
			}
			else
			{
				((Control)ChkCalcEkInSourceDate).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void chbHandleDeviceService_CheckedChanged(object sender, EventArgs e)
	{
		((Control)ucDeviceService).Enabled = chbHandleDeviceService.Checked;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_signalRToken?.Cancel();
				_signalRToken?.Dispose();
				_signalRToken = null;
				_loadDatatokenSetting?.Cancel();
				_loadDatatokenSetting?.Dispose();
				_loadDatatokenSetting = null;
				components.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Expected O, but got Unknown
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Expected O, but got Unknown
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected O, but got Unknown
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Expected O, but got Unknown
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Expected O, but got Unknown
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Expected O, but got Unknown
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Expected O, but got Unknown
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Expected O, but got Unknown
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Expected O, but got Unknown
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Expected O, but got Unknown
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Expected O, but got Unknown
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Expected O, but got Unknown
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Expected O, but got Unknown
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Expected O, but got Unknown
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Expected O, but got Unknown
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Expected O, but got Unknown
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Expected O, but got Unknown
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Expected O, but got Unknown
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_092b: Expected O, but got Unknown
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Expected O, but got Unknown
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Expected O, but got Unknown
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Expected O, but got Unknown
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Expected O, but got Unknown
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Expected O, but got Unknown
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Expected O, but got Unknown
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Expected O, but got Unknown
		//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Expected O, but got Unknown
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Expected O, but got Unknown
		//IL_09d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Expected O, but got Unknown
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Expected O, but got Unknown
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Expected O, but got Unknown
		//IL_09f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Expected O, but got Unknown
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Expected O, but got Unknown
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Expected O, but got Unknown
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Expected O, but got Unknown
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Expected O, but got Unknown
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Expected O, but got Unknown
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Expected O, but got Unknown
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Expected O, but got Unknown
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Expected O, but got Unknown
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Expected O, but got Unknown
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Expected O, but got Unknown
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Expected O, but got Unknown
		//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0a: Expected O, but got Unknown
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Expected O, but got Unknown
		//IL_0b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Expected O, but got Unknown
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Expected O, but got Unknown
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b36: Expected O, but got Unknown
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Expected O, but got Unknown
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Expected O, but got Unknown
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Expected O, but got Unknown
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Expected O, but got Unknown
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Expected O, but got Unknown
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Expected O, but got Unknown
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Expected O, but got Unknown
		//IL_0b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Expected O, but got Unknown
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b99: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bba: Expected O, but got Unknown
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Expected O, but got Unknown
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd0: Expected O, but got Unknown
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Expected O, but got Unknown
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Expected O, but got Unknown
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Expected O, but got Unknown
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c33: Expected O, but got Unknown
		//IL_0c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3e: Expected O, but got Unknown
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Expected O, but got Unknown
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Expected O, but got Unknown
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Expected O, but got Unknown
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Expected O, but got Unknown
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		//IL_0c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Expected O, but got Unknown
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Expected O, but got Unknown
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cac: Expected O, but got Unknown
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Expected O, but got Unknown
		//IL_0cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc8: Expected O, but got Unknown
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Expected O, but got Unknown
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cde: Expected O, but got Unknown
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Expected O, but got Unknown
		//IL_0cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf4: Expected O, but got Unknown
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Expected O, but got Unknown
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0a: Expected O, but got Unknown
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Expected O, but got Unknown
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Expected O, but got Unknown
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2b: Expected O, but got Unknown
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Expected O, but got Unknown
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d41: Expected O, but got Unknown
		//IL_0d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4c: Expected O, but got Unknown
		//IL_0d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5d: Expected O, but got Unknown
		//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Expected O, but got Unknown
		//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d73: Expected O, but got Unknown
		//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7e: Expected O, but got Unknown
		//IL_1109: Unknown result type (might be due to invalid IL or missing references)
		//IL_1113: Expected O, but got Unknown
		//IL_11fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1207: Expected O, but got Unknown
		//IL_1414: Unknown result type (might be due to invalid IL or missing references)
		//IL_141e: Expected O, but got Unknown
		//IL_15f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f8: Expected O, but got Unknown
		//IL_1815: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8d: Expected O, but got Unknown
		//IL_1aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_419f: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a9: Expected O, but got Unknown
		//IL_456b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4705: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be9: Expected O, but got Unknown
		//IL_4d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d18: Expected O, but got Unknown
		//IL_4e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e85: Expected O, but got Unknown
		//IL_4fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fe5: Expected O, but got Unknown
		//IL_5147: Unknown result type (might be due to invalid IL or missing references)
		//IL_5151: Expected O, but got Unknown
		//IL_52b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_52bd: Expected O, but got Unknown
		//IL_5421: Unknown result type (might be due to invalid IL or missing references)
		//IL_542b: Expected O, but got Unknown
		//IL_557e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5588: Expected O, but got Unknown
		//IL_569e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b71: Expected O, but got Unknown
		//IL_5c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c88: Expected O, but got Unknown
		//IL_5cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd3: Expected O, but got Unknown
		//IL_5de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5def: Expected O, but got Unknown
		//IL_6013: Unknown result type (might be due to invalid IL or missing references)
		//IL_6140: Unknown result type (might be due to invalid IL or missing references)
		//IL_614a: Expected O, but got Unknown
		//IL_62a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ad: Expected O, but got Unknown
		//IL_699d: Unknown result type (might be due to invalid IL or missing references)
		//IL_70f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_867a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8684: Expected O, but got Unknown
		//IL_9366: Unknown result type (might be due to invalid IL or missing references)
		//IL_c00f: Unknown result type (might be due to invalid IL or missing references)
		//IL_c255: Unknown result type (might be due to invalid IL or missing references)
		//IL_c25f: Expected O, but got Unknown
		//IL_cc8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_cc98: Expected O, but got Unknown
		//IL_cf2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_cf37: Expected O, but got Unknown
		//IL_d0c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_d0d2: Expected O, but got Unknown
		//IL_d2e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_d2eb: Expected O, but got Unknown
		//IL_d51e: Unknown result type (might be due to invalid IL or missing references)
		//IL_d528: Expected O, but got Unknown
		//IL_e78b: Unknown result type (might be due to invalid IL or missing references)
		//IL_f085: Unknown result type (might be due to invalid IL or missing references)
		//IL_f08f: Expected O, but got Unknown
		//IL_f24b: Unknown result type (might be due to invalid IL or missing references)
		//IL_f255: Expected O, but got Unknown
		//IL_f2b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_f2c0: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmSetting));
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		tabControl1 = new TabControl();
		tabControlPanel12 = new TabControlPanel();
		chbHandleDeviceService = new CheckBoxX();
		ucDeviceService = new UcServiceManager();
		tabControlDeviceService = new TabItem(components);
		tabControlPanel11 = new TabControlPanel();
		pnlPayamakServiceSettings = new Panel();
		ucServiceManager1 = new UcServiceManager();
		tabItem9 = new TabItem(components);
		tabControlPanel6 = new TabControlPanel();
		CBPayamakEnable = new CheckBox();
		groupPanel2 = new GroupPanel();
		NumSendAbsentSmsMaxMinutesAfterShift = new NumericUpDown();
		labelX73 = new LabelX();
		NumSendSmsToClock = new NumericUpDown();
		NumSendSmsFromClock = new NumericUpDown();
		labelX74 = new LabelX();
		labelX70 = new LabelX();
		NumSendIncompleteSmsToClock = new NumericUpDown();
		NumSendIncompleteSmsFromClock = new NumericUpDown();
		labelX51 = new LabelX();
		TxtSmsIncompletePerson = new TextBox();
		chkBxIncompletePerson = new CheckBoxX();
		labelX78 = new LabelX();
		labelX77 = new LabelX();
		labelX4 = new LabelX();
		TxtSmsAbsentPerson = new TextBox();
		TxtSmsIncompleteManager = new TextBox();
		TxtSmsAbsentManager = new TextBox();
		chkBxIncompleteManager = new CheckBoxX();
		ChbSMSAbsentPerson = new CheckBoxX();
		ChbSMSAbsentManager = new CheckBoxX();
		TxtSmsValedKh = new TextBox();
		BtnDeleteAcc = new ButtonX();
		TxtSmsValedV = new TextBox();
		NumSendAbsentSmsMinMinutesAfterShift = new NumericUpDown();
		NumSendTaradodSmsMaxMinutes = new NumericUpDown();
		ChbSMSKHValed = new CheckBoxX();
		BtnMoreSms = new ButtonX();
		ChbSMSVorodValed = new CheckBoxX();
		labelX3 = new LabelX();
		TxtSamaneSMS = new TextBoxX();
		TxtsSmsManageV = new TextBox();
		labelX9 = new LabelX();
		ChbSendSmsVacationManager = new CheckBoxX();
		ChbSmsVManage = new CheckBoxX();
		TxtAdminMobs = new TextBoxX();
		ChbSmsKHManage = new CheckBoxX();
		labelX17 = new LabelX();
		labelX15 = new LabelX();
		TxtsSmsManageKH = new TextBox();
		groupPanel14 = new GroupPanel();
		BtnPerFamily = new ButtonX();
		BtnTime = new ButtonX();
		BtnKK = new ButtonX();
		BtnEK = new ButtonX();
		BtntypeTaradod = new ButtonX();
		BtnCodeMoein = new ButtonX();
		btnCodeT = new ButtonX();
		BtnDate = new ButtonX();
		BtnPerName = new ButtonX();
		pictureBox6 = new PictureBox();
		tabItem6 = new TabItem(components);
		tabControlPanel1 = new TabControlPanel();
		tabItem1 = new TabItem(components);
		tabControlPanel7 = new TabControlPanel();
		pictureBox7 = new PictureBox();
		groupPanel12 = new GroupPanel();
		labelX14 = new LabelX();
		TxtSabt = new TextBoxX();
		Label42 = new Label();
		TxtShrh = new TextBoxX();
		labelX21 = new LabelX();
		TxtCodeEkhtesasi = new TextBoxX();
		labelX20 = new LabelX();
		TxtTel = new TextBoxX();
		labelX19 = new LabelX();
		TxtADD = new TextBoxX();
		labelX18 = new LabelX();
		TxtFax = new TextBoxX();
		labelX16 = new LabelX();
		TxtCompany = new TextBoxX();
		labelX1 = new LabelX();
		tabItem7 = new TabItem(components);
		tabControlPanel9 = new TabControlPanel();
		ChkWebService = new CheckBoxX();
		GrpWebservice = new GroupPanel();
		CmbURL = new ComboBox();
		lblURL = new LabelX();
		lbllinkwebservice = new LinkLabel();
		labelX67 = new LabelX();
		TxtNpPass = new TextBox();
		labelX66 = new LabelX();
		TxtNpUser = new TextBox();
		WebServiceTab = new TabItem(components);
		tabControlPanel10 = new TabControlPanel();
		labelX79 = new LabelX();
		NumGetTaradodFormCountDaysAgo = new NumericUpDown();
		labelX75 = new LabelX();
		ChkCalcEK_KK = new CheckBoxX();
		ChkCalcEkInSourceDate = new CheckBoxX();
		ChkCalcEkWhenHasVacationOrMission = new CheckBoxX();
		ChkAutomaticCalcMoveToNextDay = new CheckBoxX();
		ChkNotKarkardWhenNoGharardad = new CheckBoxX();
		ChkCalcEzafeOutOfShift = new CheckBoxX();
		ChkCalcHoliday = new CheckBoxX();
		tabItemSettingTaradod = new TabItem(components);
		tabControlPanel8 = new TabControlPanel();
		labelX50 = new LabelX();
		flowLayoutPanel1 = new FlowLayoutPanel();
		ChkFromDateEndedGharardad = new CheckBoxX();
		labelX48 = new LabelX();
		labelX49 = new LabelX();
		NUMGharardadEndedNext = new NumericUpDown();
		labelX52 = new LabelX();
		labelX54 = new LabelX();
		ChkViewEndedGharardad = new CheckBoxX();
		NUMGharardadEndedAgo = new NumericUpDown();
		tabItem8 = new TabItem(components);
		tabControlPanel2 = new TabControlPanel();
		groupPanel9 = new GroupPanel();
		ChkViewGridReportDevices = new CheckBoxX();
		labelX69 = new LabelX();
		ChkViewGridLastTaradods = new CheckBoxX();
		TbxCountReportDevices = new NumericUpDown();
		labelX68 = new LabelX();
		TbxCountLastTaradods = new NumericUpDown();
		CmbCalendarType = new ComboBox();
		labelX59 = new LabelX();
		groupPanel6 = new GroupPanel();
		TbxManualEditTaradod = new TextBoxX();
		labelX58 = new LabelX();
		TbxManualAddTaradod = new TextBoxX();
		labelX57 = new LabelX();
		groupPanel5 = new GroupPanel();
		CBGetPassAfterMaximize = new CheckBoxX();
		CBRunInStart = new CheckBoxX();
		labelX53 = new LabelX();
		NumLockTime = new NumericUpDown();
		groupPanel1 = new GroupPanel();
		TxtFont = new TextBoxX();
		BtnMore = new ButtonX();
		pictureBox3 = new PictureBox();
		labelX2 = new LabelX();
		LBLPreviwe = new Label();
		groupPanel3 = new GroupPanel();
		ChkNotShowUi = new CheckBoxX();
		btnSearchFileName = new ButtonX();
		chBxGetFileName = new CheckBoxX();
		txtFileName = new TextBox();
		labelX22 = new LabelX();
		tabItem2 = new TabItem(components);
		tabControlPanel4 = new TabControlPanel();
		labelX65 = new LabelX();
		TxtTafsilRoundFish = new TextBox();
		TxtMoeinRoundFish = new TextBox();
		ChBxTaxJarime = new CheckBoxX();
		ChBxInsuranceJarime = new CheckBoxX();
		labelX62 = new LabelX();
		TxtTafsilJarime = new TextBox();
		TxtMoeinJarime = new TextBox();
		ChBxTaxTatilKari = new CheckBoxX();
		labelX61 = new LabelX();
		ChBxInsuranceTatilKari = new CheckBoxX();
		TxtMoeinTatilKari = new TextBox();
		TxtTafsilTatilKari = new TextBox();
		ChBxTaxMission = new CheckBoxX();
		labelX60 = new LabelX();
		TxtMoeinMission = new TextBox();
		ChBxInsuranceMission = new CheckBoxX();
		TxtTafsilMission = new TextBox();
		labelX36 = new LabelX();
		labelX13 = new LabelX();
		ChBxTaxVacation = new CheckBoxX();
		ChBxTaxAbsentTime = new CheckBoxX();
		ChBxTaxLeaveTime = new CheckBoxX();
		ChBxTaxBaseSalary = new CheckBoxX();
		ChBxInsuranceAbsentTime = new CheckBoxX();
		ChBxInsuranceLeaveTime = new CheckBoxX();
		labelX41 = new LabelX();
		labelX40 = new LabelX();
		ChBxTaxNightTime = new CheckBoxX();
		labelX39 = new LabelX();
		ChBxInsuranceVacation = new CheckBoxX();
		lblMoeinOverTime = new LabelX();
		ChBxTaxHolidayTime = new CheckBoxX();
		lblMoeinShiftTime = new LabelX();
		ChBxInsuranceNightTime = new CheckBoxX();
		labelX25 = new LabelX();
		ChBxTaxShiftTime = new CheckBoxX();
		labelX26 = new LabelX();
		ChBxInsuranceHolidayTime = new CheckBoxX();
		labelX35 = new LabelX();
		ChBxTaxOverTime = new CheckBoxX();
		lblMoeinTaxGov = new LabelX();
		ChBxInsuranceShiftTime = new CheckBoxX();
		labelX33 = new LabelX();
		labelX32 = new LabelX();
		ChBxInsuranceOverTime = new CheckBoxX();
		TxtMoeinBaseSalary = new TextBox();
		ChBxInsuranceBaseSalary = new CheckBoxX();
		TxtTafsilBaseSalary = new TextBox();
		TxtTafsilEmployer = new TextBox();
		TxtMoeinOverTime = new TextBox();
		TxtTafsilVacation = new TextBox();
		TxtMoeinShiftTime = new TextBox();
		TxtMoeinEmployer = new TextBox();
		TxtMoeinHolidayTime = new TextBox();
		TxtMoeinVacation = new TextBox();
		TxtMoeinNightTime = new TextBox();
		labelX38 = new LabelX();
		labelX28 = new LabelX();
		TxtTafsilOverTime = new TextBox();
		labelX27 = new LabelX();
		TxtTafsilShiftTime = new TextBox();
		TxtTafsilTaxGov = new TextBox();
		TxtTafsilHolidayTime = new TextBox();
		TxtMoeinTaxGov = new TextBox();
		TxtTafsilNightTime = new TextBox();
		TxtTafsilTaminGov = new TextBox();
		TxtMoeinUnEmployMent = new TextBox();
		TxtMoeinTaminGov = new TextBox();
		TxtTafsilUnEmployMent = new TextBox();
		TxtTafsilAbsentTime = new TextBox();
		TxtTafsilLeaveTime = new TextBox();
		TxtMoeinAbsentTime = new TextBox();
		TxtMoeinLeaveTime = new TextBox();
		tabItem4 = new TabItem(components);
		tabControlPanel3 = new TabControlPanel();
		TxtHaghTahol = new TextBoxFa();
		labelX76 = new LabelX();
		labelX71 = new LabelX();
		ZaribMinesBimeOfTax = new TextBox();
		labelX72 = new LabelX();
		labelX63 = new LabelX();
		TxtRoundBelow = new TextBox();
		labelX64 = new LabelX();
		labelX55 = new LabelX();
		ZaribTatil = new TextBox();
		labelX56 = new LabelX();
		labelX45 = new LabelX();
		labelX12 = new LabelX();
		ZaribFriday = new TextBox();
		labelX47 = new LabelX();
		labelX46 = new LabelX();
		labelX11 = new LabelX();
		ZaribEzafeDay = new TextBox();
		txtHaghSpouse = new TextBoxFa();
		labelX43 = new LabelX();
		MINEzafeDay = new TextBox();
		txtHaghMaskan = new TextBoxFa();
		labelX10 = new LabelX();
		labelX44 = new LabelX();
		btnGetLabor = new ButtonX();
		txtBon = new TextBoxFa();
		labelX42 = new LabelX();
		txtDailyWage = new TextBoxFa();
		labelX37 = new LabelX();
		CHAutoEzafe = new CheckBoxX();
		groupPanel4 = new GroupPanel();
		btnFraction3 = new ButtonX();
		btnFraction2 = new ButtonX();
		btnFraction1 = new ButtonX();
		btnAdditions1 = new ButtonX();
		labelX31 = new LabelX();
		labelX34 = new LabelX();
		labelX23 = new LabelX();
		labelX24 = new LabelX();
		txtFraction3Title = new TextBox();
		txtFraction2Title = new TextBox();
		txtFraction1Title = new TextBox();
		txtAdditions1Title = new TextBox();
		txtFraction3Tafsil = new TextBox();
		txtFraction3Moein = new TextBox();
		txtFraction2Tafsil = new TextBox();
		txtFraction2Moein = new TextBox();
		txtFraction1Tafsil = new TextBox();
		txtFraction1Moein = new TextBox();
		txtAdditions1Tafsil = new TextBox();
		txtAdditions1Moein = new TextBox();
		lblAdditions1 = new LabelX();
		labelX29 = new LabelX();
		labelX30 = new LabelX();
		txtMaxTedadOlaad = new NumericUpDown();
		labelX5 = new LabelX();
		tabItem3 = new TabItem(components);
		tabControlPanel5 = new TabControlPanel();
		groupPanel7 = new GroupPanel();
		CHEnableAutoBackupAllPartions = new CheckBoxX();
		pictureBox5 = new PictureBox();
		groupPanel8 = new GroupPanel();
		labelX6 = new LabelX();
		labelX7 = new LabelX();
		BackupBTNFindPath = new Button();
		labelX8 = new LabelX();
		TPBackup_DefaultPath = new LinkLabel();
		TbxBackupTimeBetween = new NumericUpDown();
		CHEnableAutoBackup = new CheckBoxX();
		tabItem5 = new TabItem(components);
		FD = new FontDialog();
		FBD = new FolderBrowserDialog();
		openFileDialog1 = new OpenFileDialog();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel12).SuspendLayout();
		((Control)tabControlPanel11).SuspendLayout();
		((Control)pnlPayamakServiceSettings).SuspendLayout();
		((Control)tabControlPanel6).SuspendLayout();
		((Control)groupPanel2).SuspendLayout();
		((ISupportInitialize)NumSendAbsentSmsMaxMinutesAfterShift).BeginInit();
		((ISupportInitialize)NumSendSmsToClock).BeginInit();
		((ISupportInitialize)NumSendSmsFromClock).BeginInit();
		((ISupportInitialize)NumSendIncompleteSmsToClock).BeginInit();
		((ISupportInitialize)NumSendIncompleteSmsFromClock).BeginInit();
		((ISupportInitialize)NumSendAbsentSmsMinMinutesAfterShift).BeginInit();
		((ISupportInitialize)NumSendTaradodSmsMaxMinutes).BeginInit();
		((Control)groupPanel14).SuspendLayout();
		((ISupportInitialize)pictureBox6).BeginInit();
		((Control)tabControlPanel7).SuspendLayout();
		((ISupportInitialize)pictureBox7).BeginInit();
		((Control)groupPanel12).SuspendLayout();
		((Control)tabControlPanel9).SuspendLayout();
		((Control)GrpWebservice).SuspendLayout();
		((Control)tabControlPanel10).SuspendLayout();
		((ISupportInitialize)NumGetTaradodFormCountDaysAgo).BeginInit();
		((Control)tabControlPanel8).SuspendLayout();
		((ISupportInitialize)NUMGharardadEndedNext).BeginInit();
		((ISupportInitialize)NUMGharardadEndedAgo).BeginInit();
		((Control)tabControlPanel2).SuspendLayout();
		((Control)groupPanel9).SuspendLayout();
		((ISupportInitialize)TbxCountReportDevices).BeginInit();
		((ISupportInitialize)TbxCountLastTaradods).BeginInit();
		((Control)groupPanel6).SuspendLayout();
		((Control)groupPanel5).SuspendLayout();
		((ISupportInitialize)NumLockTime).BeginInit();
		((Control)groupPanel1).SuspendLayout();
		((ISupportInitialize)pictureBox3).BeginInit();
		((Control)groupPanel3).SuspendLayout();
		((Control)tabControlPanel4).SuspendLayout();
		((Control)tabControlPanel3).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((ISupportInitialize)txtMaxTedadOlaad).BeginInit();
		((Control)tabControlPanel5).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)pictureBox5).BeginInit();
		((Control)groupPanel8).SuspendLayout();
		((ISupportInitialize)TbxBackupTimeBetween).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(1124, 561);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(3, 515);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(130, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancelAll_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)14;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(151, 515);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(953, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)tabControl1).Anchor = (AnchorStyles)13;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel10);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel7);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel12);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel11);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel6);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel9);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel8);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel2);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel4);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel3);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel5);
		((Control)tabControl1).Location = new Point(0, 0);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 1;
		((Control)tabControl1).Size = new Size(1124, 509);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem7);
		tabControl1.Tabs.Add(tabItem1);
		tabControl1.Tabs.Add(tabItem6);
		tabControl1.Tabs.Add(tabItem9);
		tabControl1.Tabs.Add(tabItem5);
		tabControl1.Tabs.Add(tabItem3);
		tabControl1.Tabs.Add(tabItem4);
		tabControl1.Tabs.Add(tabItem2);
		tabControl1.Tabs.Add(tabItem8);
		tabControl1.Tabs.Add(WebServiceTab);
		tabControl1.Tabs.Add(tabItemSettingTaradod);
		tabControl1.Tabs.Add(tabControlDeviceService);
		((Control)tabControl1).Text = "[مشخصات شرکت]";
		((Control)tabControlPanel12).Controls.Add((Control)(object)chbHandleDeviceService);
		((Control)tabControlPanel12).Controls.Add((Control)(object)ucDeviceService);
		((PanelEx)tabControlPanel12).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel12).Dock = (DockStyle)5;
		((Control)tabControlPanel12).Location = new Point(0, 26);
		((Control)tabControlPanel12).Name = "tabControlPanel12";
		((Control)tabControlPanel12).Padding = new Padding(1);
		((Control)tabControlPanel12).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel12).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel12).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel12).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel12).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel12).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel12).Style.GradientAngle = 90;
		((Control)tabControlPanel12).TabIndex = 56;
		tabControlPanel12.TabItem = tabControlDeviceService;
		((Control)chbHandleDeviceService).Anchor = (AnchorStyles)9;
		((Control)chbHandleDeviceService).BackColor = Color.Transparent;
		((BaseItemControl)chbHandleDeviceService).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbHandleDeviceService).Location = new Point(823, 6);
		((Control)chbHandleDeviceService).Name = "chbHandleDeviceService";
		((Control)chbHandleDeviceService).RightToLeft = (RightToLeft)1;
		((Control)chbHandleDeviceService).Size = new Size(288, 23);
		((BaseItemControl)chbHandleDeviceService).Style = (eDotNetBarStyle)9;
		((Control)chbHandleDeviceService).TabIndex = 13;
		((Control)chbHandleDeviceService).Text = "[تنظیمات سرویس ارتباط با دستگاه فعال باشد]";
		chbHandleDeviceService.CheckedChanged += chbHandleDeviceService_CheckedChanged;
		((Control)ucDeviceService).Anchor = (AnchorStyles)15;
		((ScrollableControl)ucDeviceService).AutoScroll = true;
		((Control)ucDeviceService).BackColor = Color.Transparent;
		ucDeviceService.Config = null;
		((Control)ucDeviceService).Enabled = false;
		((Control)ucDeviceService).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucDeviceService).Location = new Point(1, 33);
		((Control)ucDeviceService).Margin = new Padding(4, 5, 4, 5);
		((Control)ucDeviceService).Name = "ucDeviceService";
		((Control)ucDeviceService).RightToLeft = (RightToLeft)1;
		((Control)ucDeviceService).Size = new Size(1122, 449);
		((Control)ucDeviceService).TabIndex = 0;
		tabControlDeviceService.AttachedControl = (Control)(object)tabControlPanel12;
		tabControlDeviceService.Name = "tabControlDeviceService";
		tabControlDeviceService.Text = "[سرویس ارتباط با دستگاه]";
		((Control)tabControlPanel11).Controls.Add((Control)(object)pnlPayamakServiceSettings);
		((PanelEx)tabControlPanel11).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel11).Dock = (DockStyle)5;
		((Control)tabControlPanel11).Location = new Point(0, 26);
		((Control)tabControlPanel11).Name = "tabControlPanel11";
		((Control)tabControlPanel11).Padding = new Padding(1);
		((Control)tabControlPanel11).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel11).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel11).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel11).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel11).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel11).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel11).Style.GradientAngle = 90;
		((Control)tabControlPanel11).TabIndex = 44;
		tabControlPanel11.TabItem = tabItem9;
		((Control)pnlPayamakServiceSettings).BackColor = Color.Transparent;
		((Control)pnlPayamakServiceSettings).Controls.Add((Control)(object)ucServiceManager1);
		((Control)pnlPayamakServiceSettings).Dock = (DockStyle)5;
		((Control)pnlPayamakServiceSettings).Location = new Point(1, 1);
		((Control)pnlPayamakServiceSettings).Name = "pnlPayamakServiceSettings";
		((Control)pnlPayamakServiceSettings).Size = new Size(1122, 481);
		((Control)pnlPayamakServiceSettings).TabIndex = 0;
		((ScrollableControl)ucServiceManager1).AutoScroll = true;
		ucServiceManager1.Config = null;
		((Control)ucServiceManager1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucServiceManager1).Location = new Point(15, 7);
		((Control)ucServiceManager1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucServiceManager1).Name = "ucServiceManager1";
		((Control)ucServiceManager1).RightToLeft = (RightToLeft)1;
		((Control)ucServiceManager1).Size = new Size(1090, 468);
		((Control)ucServiceManager1).TabIndex = 0;
		tabItem9.AttachedControl = (Control)(object)tabControlPanel11;
		tabItem9.Name = "tabItem9";
		tabItem9.Text = "سرویس پیامک";
		((Control)tabControlPanel6).Controls.Add((Control)(object)CBPayamakEnable);
		((Control)tabControlPanel6).Controls.Add((Control)(object)groupPanel2);
		((Control)tabControlPanel6).Controls.Add((Control)(object)groupPanel14);
		((Control)tabControlPanel6).Controls.Add((Control)(object)pictureBox6);
		((PanelEx)tabControlPanel6).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel6).Dock = (DockStyle)5;
		((Control)tabControlPanel6).Location = new Point(0, 26);
		((Control)tabControlPanel6).Name = "tabControlPanel6";
		((Control)tabControlPanel6).Padding = new Padding(1);
		((Control)tabControlPanel6).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel6).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel6).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel6).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel6).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel6).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel6).Style.GradientAngle = 90;
		((Control)tabControlPanel6).TabIndex = 0;
		tabControlPanel6.TabItem = tabItem6;
		((Control)CBPayamakEnable).AutoSize = true;
		((Control)CBPayamakEnable).BackColor = Color.Transparent;
		((Control)CBPayamakEnable).Location = new Point(926, 17);
		((Control)CBPayamakEnable).Name = "CBPayamakEnable";
		((Control)CBPayamakEnable).Size = new Size(119, 17);
		((Control)CBPayamakEnable).TabIndex = 36;
		((Control)CBPayamakEnable).Text = "فعال سازی پیامک";
		((ButtonBase)CBPayamakEnable).UseVisualStyleBackColor = false;
		((Control)groupPanel2).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendAbsentSmsMaxMinutesAfterShift);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX73);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendSmsToClock);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendSmsFromClock);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX74);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX70);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendIncompleteSmsToClock);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendIncompleteSmsFromClock);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX51);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsIncompletePerson);
		((Control)groupPanel2).Controls.Add((Control)(object)chkBxIncompletePerson);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX78);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX77);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsAbsentPerson);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsIncompleteManager);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsAbsentManager);
		((Control)groupPanel2).Controls.Add((Control)(object)chkBxIncompleteManager);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSMSAbsentPerson);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSMSAbsentManager);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsValedKh);
		((Control)groupPanel2).Controls.Add((Control)(object)BtnDeleteAcc);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsValedV);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendAbsentSmsMinMinutesAfterShift);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendTaradodSmsMaxMinutes);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSMSKHValed);
		((Control)groupPanel2).Controls.Add((Control)(object)BtnMoreSms);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSMSVorodValed);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSamaneSMS);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtsSmsManageV);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSendSmsVacationManager);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSmsVManage);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtAdminMobs);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSmsKHManage);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX17);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX15);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtsSmsManageKH);
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(133, 47);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(980, 432);
		((PanelControl)groupPanel2).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel2).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel2).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel2).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel2).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel2).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel2).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel2).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel2).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel2).Style.CornerDiameter = 4;
		((PanelControl)groupPanel2).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel2).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel2).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel2).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel2).TabIndex = 0;
		((Control)NumSendAbsentSmsMaxMinutesAfterShift).Anchor = (AnchorStyles)9;
		((Control)NumSendAbsentSmsMaxMinutesAfterShift).Location = new Point(178, 31);
		NumSendAbsentSmsMaxMinutesAfterShift.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		NumSendAbsentSmsMaxMinutesAfterShift.Minimum = new decimal(new int[4] { 15, 0, 0, 0 });
		((Control)NumSendAbsentSmsMaxMinutesAfterShift).Name = "NumSendAbsentSmsMaxMinutesAfterShift";
		((Control)NumSendAbsentSmsMaxMinutesAfterShift).Size = new Size(55, 21);
		((Control)NumSendAbsentSmsMaxMinutesAfterShift).TabIndex = 36;
		((UpDownBase)NumSendAbsentSmsMaxMinutesAfterShift).TextAlign = (HorizontalAlignment)2;
		NumSendAbsentSmsMaxMinutesAfterShift.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		((Control)labelX73).Anchor = (AnchorStyles)9;
		((Control)labelX73).BackColor = Color.Transparent;
		((BaseItemControl)labelX73).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX73).Location = new Point(539, 5);
		((Control)labelX73).Name = "labelX73";
		((Control)labelX73).Size = new Size(19, 21);
		((Control)labelX73).TabIndex = 35;
		((Control)labelX73).Text = "تا";
		labelX73.TextAlignment = (StringAlignment)1;
		((Control)NumSendSmsToClock).Anchor = (AnchorStyles)9;
		((Control)NumSendSmsToClock).Location = new Point(477, 5);
		NumSendSmsToClock.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)NumSendSmsToClock).Name = "NumSendSmsToClock";
		((Control)NumSendSmsToClock).Size = new Size(59, 21);
		((Control)NumSendSmsToClock).TabIndex = 34;
		((UpDownBase)NumSendSmsToClock).TextAlign = (HorizontalAlignment)2;
		NumSendSmsToClock.Value = new decimal(new int[4] { 22, 0, 0, 0 });
		((Control)NumSendSmsFromClock).Anchor = (AnchorStyles)9;
		((Control)NumSendSmsFromClock).BackColor = SystemColors.Window;
		((Control)NumSendSmsFromClock).Location = new Point(559, 5);
		NumSendSmsFromClock.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)NumSendSmsFromClock).Name = "NumSendSmsFromClock";
		((Control)NumSendSmsFromClock).Size = new Size(59, 21);
		((Control)NumSendSmsFromClock).TabIndex = 33;
		((UpDownBase)NumSendSmsFromClock).TextAlign = (HorizontalAlignment)2;
		NumSendSmsFromClock.Value = new decimal(new int[4] { 7, 0, 0, 0 });
		((Control)labelX74).BackColor = Color.Transparent;
		((BaseItemControl)labelX74).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX74).Location = new Point(624, 5);
		((Control)labelX74).Name = "labelX74";
		((Control)labelX74).RightToLeft = (RightToLeft)1;
		((Control)labelX74).Size = new Size(335, 21);
		((Control)labelX74).TabIndex = 32;
		((Control)labelX74).Text = "پیامک ها  بین چه ساعاتی از روز اجازه ارسال داشته باشند؟";
		labelX74.TextAlignment = (StringAlignment)2;
		((Control)labelX70).Anchor = (AnchorStyles)9;
		((Control)labelX70).BackColor = Color.Transparent;
		((BaseItemControl)labelX70).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX70).Location = new Point(539, 31);
		((Control)labelX70).Name = "labelX70";
		((Control)labelX70).Size = new Size(19, 21);
		((Control)labelX70).TabIndex = 31;
		((Control)labelX70).Text = "تا";
		labelX70.TextAlignment = (StringAlignment)1;
		((Control)NumSendIncompleteSmsToClock).Anchor = (AnchorStyles)9;
		((Control)NumSendIncompleteSmsToClock).Location = new Point(477, 31);
		NumSendIncompleteSmsToClock.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)NumSendIncompleteSmsToClock).Name = "NumSendIncompleteSmsToClock";
		((Control)NumSendIncompleteSmsToClock).Size = new Size(59, 21);
		((Control)NumSendIncompleteSmsToClock).TabIndex = 30;
		((UpDownBase)NumSendIncompleteSmsToClock).TextAlign = (HorizontalAlignment)2;
		NumSendIncompleteSmsToClock.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumSendIncompleteSmsFromClock).Anchor = (AnchorStyles)9;
		((Control)NumSendIncompleteSmsFromClock).BackColor = SystemColors.Window;
		((Control)NumSendIncompleteSmsFromClock).Location = new Point(559, 31);
		NumSendIncompleteSmsFromClock.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)NumSendIncompleteSmsFromClock).Name = "NumSendIncompleteSmsFromClock";
		((Control)NumSendIncompleteSmsFromClock).Size = new Size(59, 21);
		((Control)NumSendIncompleteSmsFromClock).TabIndex = 29;
		((UpDownBase)NumSendIncompleteSmsFromClock).TextAlign = (HorizontalAlignment)2;
		NumSendIncompleteSmsFromClock.Value = new decimal(new int[4] { 9, 0, 0, 0 });
		((Control)labelX51).BackColor = Color.Transparent;
		((BaseItemControl)labelX51).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX51).Location = new Point(624, 31);
		((Control)labelX51).Name = "labelX51";
		((Control)labelX51).RightToLeft = (RightToLeft)1;
		((Control)labelX51).Size = new Size(335, 21);
		((Control)labelX51).TabIndex = 28;
		((Control)labelX51).Text = "پیامک های تردد ناقص روز قبل، بین چه ساعاتی  ارسال شوند؟";
		labelX51.TextAlignment = (StringAlignment)2;
		((Control)TxtSmsIncompletePerson).Enabled = false;
		((Control)TxtSmsIncompletePerson).Location = new Point(15, 370);
		((TextBoxBase)TxtSmsIncompletePerson).Multiline = true;
		((Control)TxtSmsIncompletePerson).Name = "TxtSmsIncompletePerson";
		TxtSmsIncompletePerson.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsIncompletePerson).Size = new Size(454, 50);
		((Control)TxtSmsIncompletePerson).TabIndex = 27;
		((Control)TxtSmsIncompletePerson).TabStop = false;
		((TextBoxBase)TxtSmsIncompletePerson).Click += TxtsSmsManageV_Click;
		((Control)chkBxIncompletePerson).BackColor = Color.Transparent;
		((BaseItemControl)chkBxIncompletePerson).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chkBxIncompletePerson).Location = new Point(179, 341);
		((Control)chkBxIncompletePerson).Name = "chkBxIncompletePerson";
		((Control)chkBxIncompletePerson).RightToLeft = (RightToLeft)1;
		((Control)chkBxIncompletePerson).Size = new Size(290, 23);
		((BaseItemControl)chkBxIncompletePerson).Style = (eDotNetBarStyle)9;
		((Control)chkBxIncompletePerson).TabIndex = 26;
		((Control)chkBxIncompletePerson).Text = "[ارسال پیامک تردد ناقص شخص به والدین / شخص]";
		chkBxIncompletePerson.CheckedChanged += chkBxIncompletePerson_CheckedChanged;
		((Control)labelX78).Anchor = (AnchorStyles)9;
		((Control)labelX78).BackColor = Color.Transparent;
		((BaseItemControl)labelX78).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX78).Location = new Point(239, 31);
		((Control)labelX78).Name = "labelX78";
		((Control)labelX78).RightToLeft = (RightToLeft)1;
		((Control)labelX78).Size = new Size(14, 21);
		((Control)labelX78).TabIndex = 0;
		((Control)labelX78).Text = "تا";
		labelX78.TextAlignment = (StringAlignment)1;
		((Control)labelX77).Anchor = (AnchorStyles)9;
		((Control)labelX77).BackColor = Color.Transparent;
		((BaseItemControl)labelX77).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX77).Location = new Point(30, 31);
		((Control)labelX77).Name = "labelX77";
		((Control)labelX77).RightToLeft = (RightToLeft)1;
		((Control)labelX77).Size = new Size(142, 21);
		((Control)labelX77).TabIndex = 0;
		((Control)labelX77).Text = "دقیقه بعد از شروع شیفت";
		labelX77.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(320, 31);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(116, 21);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "ارسال پیامک غیبت از ";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)TxtSmsAbsentPerson).Enabled = false;
		((Control)TxtSmsAbsentPerson).Location = new Point(15, 285);
		((TextBoxBase)TxtSmsAbsentPerson).Multiline = true;
		((Control)TxtSmsAbsentPerson).Name = "TxtSmsAbsentPerson";
		TxtSmsAbsentPerson.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsAbsentPerson).Size = new Size(454, 50);
		((Control)TxtSmsAbsentPerson).TabIndex = 23;
		((Control)TxtSmsAbsentPerson).TabStop = false;
		((TextBoxBase)TxtSmsAbsentPerson).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsAbsentPerson).Enter += TxtMobs_Enter;
		((Control)TxtSmsAbsentPerson).Leave += TxtMobs_Leave;
		((Control)TxtSmsIncompleteManager).Anchor = (AnchorStyles)9;
		((Control)TxtSmsIncompleteManager).Enabled = false;
		((Control)TxtSmsIncompleteManager).Location = new Point(484, 370);
		((TextBoxBase)TxtSmsIncompleteManager).Multiline = true;
		((Control)TxtSmsIncompleteManager).Name = "TxtSmsIncompleteManager";
		TxtSmsIncompleteManager.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsIncompleteManager).Size = new Size(484, 50);
		((Control)TxtSmsIncompleteManager).TabIndex = 25;
		((Control)TxtSmsIncompleteManager).TabStop = false;
		((TextBoxBase)TxtSmsIncompleteManager).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsIncompleteManager).Enter += TxtMobs_Enter;
		((Control)TxtSmsIncompleteManager).Leave += TxtMobs_Leave;
		((Control)TxtSmsAbsentManager).Anchor = (AnchorStyles)9;
		((Control)TxtSmsAbsentManager).Enabled = false;
		((Control)TxtSmsAbsentManager).Location = new Point(484, 285);
		((TextBoxBase)TxtSmsAbsentManager).Multiline = true;
		((Control)TxtSmsAbsentManager).Name = "TxtSmsAbsentManager";
		TxtSmsAbsentManager.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsAbsentManager).Size = new Size(484, 50);
		((Control)TxtSmsAbsentManager).TabIndex = 21;
		((Control)TxtSmsAbsentManager).TabStop = false;
		((TextBoxBase)TxtSmsAbsentManager).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsAbsentManager).Enter += TxtMobs_Enter;
		((Control)TxtSmsAbsentManager).Leave += TxtMobs_Leave;
		((Control)chkBxIncompleteManager).Anchor = (AnchorStyles)9;
		((Control)chkBxIncompleteManager).BackColor = Color.Transparent;
		((BaseItemControl)chkBxIncompleteManager).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chkBxIncompleteManager).Location = new Point(752, 339);
		((Control)chkBxIncompleteManager).Name = "chkBxIncompleteManager";
		((Control)chkBxIncompleteManager).RightToLeft = (RightToLeft)1;
		((Control)chkBxIncompleteManager).Size = new Size(216, 23);
		((BaseItemControl)chkBxIncompleteManager).Style = (eDotNetBarStyle)9;
		((Control)chkBxIncompleteManager).TabIndex = 24;
		((Control)chkBxIncompleteManager).Text = "[ارسال پیامک تردد ناقص به مدیر]";
		chkBxIncompleteManager.CheckedChanged += chkBxIncompleteManager_CheckedChanged;
		((Control)ChbSMSAbsentPerson).BackColor = Color.Transparent;
		((BaseItemControl)ChbSMSAbsentPerson).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSMSAbsentPerson).Location = new Point(213, 256);
		((Control)ChbSMSAbsentPerson).Name = "ChbSMSAbsentPerson";
		((Control)ChbSMSAbsentPerson).RightToLeft = (RightToLeft)1;
		((Control)ChbSMSAbsentPerson).Size = new Size(256, 23);
		((BaseItemControl)ChbSMSAbsentPerson).Style = (eDotNetBarStyle)9;
		((Control)ChbSMSAbsentPerson).TabIndex = 22;
		((Control)ChbSMSAbsentPerson).Text = "[ارسال پیامک غیبت شخص به والدین / شخص]";
		ChbSMSAbsentPerson.CheckedChanged += ChbSMSAbsentPerson_CheckedChanged;
		((Control)ChbSMSAbsentManager).Anchor = (AnchorStyles)9;
		((Control)ChbSMSAbsentManager).BackColor = Color.Transparent;
		((BaseItemControl)ChbSMSAbsentManager).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSMSAbsentManager).Location = new Point(752, 254);
		((Control)ChbSMSAbsentManager).Name = "ChbSMSAbsentManager";
		((Control)ChbSMSAbsentManager).RightToLeft = (RightToLeft)1;
		((Control)ChbSMSAbsentManager).Size = new Size(216, 23);
		((BaseItemControl)ChbSMSAbsentManager).Style = (eDotNetBarStyle)9;
		((Control)ChbSMSAbsentManager).TabIndex = 20;
		((Control)ChbSMSAbsentManager).Text = "[ارسال پیامک غیبت شخص به مدیر]";
		ChbSMSAbsentManager.CheckedChanged += ChbSMSAbsentManager_CheckedChanged;
		((Control)TxtSmsValedKh).Enabled = false;
		((Control)TxtSmsValedKh).Location = new Point(15, 200);
		((TextBoxBase)TxtSmsValedKh).Multiline = true;
		((Control)TxtSmsValedKh).Name = "TxtSmsValedKh";
		TxtSmsValedKh.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsValedKh).Size = new Size(454, 50);
		((Control)TxtSmsValedKh).TabIndex = 19;
		((Control)TxtSmsValedKh).TabStop = false;
		((TextBoxBase)TxtSmsValedKh).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsValedKh).Enter += TxtMobs_Enter;
		((Control)TxtSmsValedKh).Leave += TxtMobs_Leave;
		((Control)BtnDeleteAcc).AccessibleRole = (AccessibleRole)43;
		BtnDeleteAcc.ColorTable = (eButtonColor)6;
		BtnDeleteAcc.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDeleteAcc).Location = new Point(15, 57);
		((Control)BtnDeleteAcc).Name = "BtnDeleteAcc";
		((Control)BtnDeleteAcc).Size = new Size(27, 21);
		((PopupItemControl)BtnDeleteAcc).Style = (eDotNetBarStyle)9;
		((Control)BtnDeleteAcc).TabIndex = 11;
		BtnDeleteAcc.Tooltip = "[حذف سامانه و ارسال بامودم GSM]";
		((Control)BtnDeleteAcc).Click += BtnDeleteAcc_Click;
		((Control)TxtSmsValedV).Enabled = false;
		((Control)TxtSmsValedV).Location = new Point(15, 115);
		((TextBoxBase)TxtSmsValedV).Multiline = true;
		((Control)TxtSmsValedV).Name = "TxtSmsValedV";
		TxtSmsValedV.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsValedV).Size = new Size(454, 50);
		((Control)TxtSmsValedV).TabIndex = 15;
		((Control)TxtSmsValedV).TabStop = false;
		((TextBoxBase)TxtSmsValedV).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsValedV).Enter += TxtMobs_Enter;
		((Control)TxtSmsValedV).Leave += TxtMobs_Leave;
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Anchor = (AnchorStyles)9;
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Location = new Point(259, 31);
		NumSendAbsentSmsMinMinutesAfterShift.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		NumSendAbsentSmsMinMinutesAfterShift.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Name = "NumSendAbsentSmsMinMinutesAfterShift";
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Size = new Size(55, 21);
		((Control)NumSendAbsentSmsMinMinutesAfterShift).TabIndex = 5;
		((UpDownBase)NumSendAbsentSmsMinMinutesAfterShift).TextAlign = (HorizontalAlignment)2;
		NumSendAbsentSmsMinMinutesAfterShift.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Enter += TxtMobs_Enter;
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Leave += TxtMobs_Leave;
		((Control)NumSendTaradodSmsMaxMinutes).Anchor = (AnchorStyles)9;
		((Control)NumSendTaradodSmsMaxMinutes).Location = new Point(15, 5);
		NumSendTaradodSmsMaxMinutes.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		NumSendTaradodSmsMaxMinutes.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumSendTaradodSmsMaxMinutes).Name = "NumSendTaradodSmsMaxMinutes";
		((Control)NumSendTaradodSmsMaxMinutes).Size = new Size(59, 21);
		((Control)NumSendTaradodSmsMaxMinutes).TabIndex = 1;
		((UpDownBase)NumSendTaradodSmsMaxMinutes).TextAlign = (HorizontalAlignment)2;
		NumSendTaradodSmsMaxMinutes.Value = new decimal(new int[4] { 20, 0, 0, 0 });
		((Control)NumSendTaradodSmsMaxMinutes).Enter += TxtMobs_Enter;
		((Control)NumSendTaradodSmsMaxMinutes).Leave += TxtMobs_Leave;
		((Control)ChbSMSKHValed).BackColor = Color.Transparent;
		((BaseItemControl)ChbSMSKHValed).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSMSKHValed).Location = new Point(213, 171);
		((Control)ChbSMSKHValed).Name = "ChbSMSKHValed";
		((Control)ChbSMSKHValed).RightToLeft = (RightToLeft)1;
		((Control)ChbSMSKHValed).Size = new Size(256, 23);
		((BaseItemControl)ChbSMSKHValed).Style = (eDotNetBarStyle)9;
		((Control)ChbSMSKHValed).TabIndex = 18;
		((Control)ChbSMSKHValed).Text = "[ارسال پیامک خروج به شخص / والدین]";
		ChbSMSKHValed.CheckedChanged += ChbSMSKHValed_CheckedChanged;
		((Control)BtnMoreSms).AccessibleRole = (AccessibleRole)43;
		BtnMoreSms.ColorTable = (eButtonColor)6;
		((Control)BtnMoreSms).Location = new Point(41, 57);
		((Control)BtnMoreSms).Name = "BtnMoreSms";
		((Control)BtnMoreSms).Size = new Size(27, 21);
		((PopupItemControl)BtnMoreSms).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreSms).TabIndex = 10;
		((Control)BtnMoreSms).Text = "...";
		BtnMoreSms.Tooltip = "[افزودن سامانه]";
		((Control)BtnMoreSms).Click += BtnMoreSms_Click;
		((Control)ChbSMSVorodValed).BackColor = Color.Transparent;
		((BaseItemControl)ChbSMSVorodValed).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSMSVorodValed).Location = new Point(186, 86);
		((Control)ChbSMSVorodValed).Name = "ChbSMSVorodValed";
		((Control)ChbSMSVorodValed).RightToLeft = (RightToLeft)1;
		((Control)ChbSMSVorodValed).Size = new Size(283, 23);
		((BaseItemControl)ChbSMSVorodValed).Style = (eDotNetBarStyle)9;
		((Control)ChbSMSVorodValed).TabIndex = 14;
		((Control)ChbSMSVorodValed).Text = "[ارسال پیامک ورود به شخص / والدین]";
		ChbSMSVorodValed.CheckedChanged += ChbSMSVorodValed_CheckedChanged;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(80, 5);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(386, 21);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "پیامک ورود/خروج، حداکثر چند دقیقه بعد از ثبت تردد اعتبار ارسال دارد؟";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtSamaneSMS.Border.Class = "TextBoxBorder";
		TxtSamaneSMS.Border.CornerType = (eCornerType)1;
		((Control)TxtSamaneSMS).Enabled = false;
		((Control)TxtSamaneSMS).Location = new Point(69, 57);
		((Control)TxtSamaneSMS).Name = "TxtSamaneSMS";
		((Control)TxtSamaneSMS).Size = new Size(262, 21);
		((Control)TxtSamaneSMS).TabIndex = 9;
		((Control)TxtsSmsManageV).Anchor = (AnchorStyles)9;
		((Control)TxtsSmsManageV).Enabled = false;
		((Control)TxtsSmsManageV).Location = new Point(484, 115);
		((TextBoxBase)TxtsSmsManageV).Multiline = true;
		((Control)TxtsSmsManageV).Name = "TxtsSmsManageV";
		TxtsSmsManageV.ScrollBars = (ScrollBars)3;
		((Control)TxtsSmsManageV).Size = new Size(484, 50);
		((Control)TxtsSmsManageV).TabIndex = 13;
		((Control)TxtsSmsManageV).TabStop = false;
		((TextBoxBase)TxtsSmsManageV).Click += TxtsSmsManageV_Click;
		((Control)TxtsSmsManageV).Enter += TxtMobs_Enter;
		((Control)TxtsSmsManageV).Leave += TxtMobs_Leave;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(337, 57);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).RightToLeft = (RightToLeft)1;
		((Control)labelX9).Size = new Size(132, 21);
		((Control)labelX9).TabIndex = 1;
		((Control)labelX9).Text = "[سامانه ارسال پیامک ]:";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)ChbSendSmsVacationManager).Anchor = (AnchorStyles)9;
		((Control)ChbSendSmsVacationManager).BackColor = Color.Transparent;
		((BaseItemControl)ChbSendSmsVacationManager).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSendSmsVacationManager).Location = new Point(484, 84);
		((Control)ChbSendSmsVacationManager).Name = "ChbSendSmsVacationManager";
		((Control)ChbSendSmsVacationManager).RightToLeft = (RightToLeft)1;
		((Control)ChbSendSmsVacationManager).Size = new Size(233, 23);
		((BaseItemControl)ChbSendSmsVacationManager).Style = (eDotNetBarStyle)9;
		((Control)ChbSendSmsVacationManager).TabIndex = 12;
		((Control)ChbSendSmsVacationManager).Text = "ارسال پیامک مرخصی روز جاری به مدیر";
		ChbSendSmsVacationManager.CheckedChanged += ChbSmsVManage_CheckedChanged;
		((Control)ChbSmsVManage).Anchor = (AnchorStyles)9;
		((Control)ChbSmsVManage).BackColor = Color.Transparent;
		((BaseItemControl)ChbSmsVManage).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSmsVManage).Location = new Point(727, 84);
		((Control)ChbSmsVManage).Name = "ChbSmsVManage";
		((Control)ChbSmsVManage).RightToLeft = (RightToLeft)1;
		((Control)ChbSmsVManage).Size = new Size(241, 23);
		((BaseItemControl)ChbSmsVManage).Style = (eDotNetBarStyle)9;
		((Control)ChbSmsVManage).TabIndex = 12;
		((Control)ChbSmsVManage).Text = "[ارسال پیامک ورود شخص به مدیریت]";
		ChbSmsVManage.CheckedChanged += ChbSmsVManage_CheckedChanged;
		((Control)TxtAdminMobs).Anchor = (AnchorStyles)9;
		TxtAdminMobs.Border.Class = "TextBoxBorder";
		TxtAdminMobs.Border.CornerType = (eCornerType)1;
		((Control)TxtAdminMobs).Location = new Point(477, 57);
		((Control)TxtAdminMobs).Name = "TxtAdminMobs";
		((Control)TxtAdminMobs).Size = new Size(268, 21);
		((Control)TxtAdminMobs).TabIndex = 7;
		((Control)TxtAdminMobs).Enter += TxtMobs_Enter;
		((Control)TxtAdminMobs).Leave += TxtMobs_Leave;
		((Control)ChbSmsKHManage).Anchor = (AnchorStyles)9;
		((Control)ChbSmsKHManage).BackColor = Color.Transparent;
		((BaseItemControl)ChbSmsKHManage).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSmsKHManage).Location = new Point(754, 169);
		((Control)ChbSmsKHManage).Name = "ChbSmsKHManage";
		((Control)ChbSmsKHManage).RightToLeft = (RightToLeft)1;
		((Control)ChbSmsKHManage).Size = new Size(214, 23);
		((BaseItemControl)ChbSmsKHManage).Style = (eDotNetBarStyle)9;
		((Control)ChbSmsKHManage).TabIndex = 0;
		((Control)ChbSmsKHManage).Text = "[ارسال پیامک خروج شخص به مدیریت]";
		ChbSmsKHManage.CheckedChanged += ChbSmsKHManage_CheckedChanged;
		((Control)labelX17).Anchor = (AnchorStyles)9;
		((Control)labelX17).BackColor = Color.Transparent;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Location = new Point(454, 84);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(19, 21);
		((Control)labelX17).TabIndex = 4;
		((Control)labelX17).Text = ":";
		labelX17.TextAlignment = (StringAlignment)1;
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((Control)labelX15).BackColor = Color.Transparent;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Location = new Point(749, 57);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).RightToLeft = (RightToLeft)1;
		((Control)labelX15).Size = new Size(219, 21);
		((Control)labelX15).TabIndex = 6;
		((Control)labelX15).Text = "[شماره موبایل مدیر(ها) ]:";
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)TxtsSmsManageKH).Anchor = (AnchorStyles)9;
		((Control)TxtsSmsManageKH).Enabled = false;
		((Control)TxtsSmsManageKH).Location = new Point(484, 200);
		((TextBoxBase)TxtsSmsManageKH).Multiline = true;
		((Control)TxtsSmsManageKH).Name = "TxtsSmsManageKH";
		TxtsSmsManageKH.ScrollBars = (ScrollBars)3;
		((Control)TxtsSmsManageKH).Size = new Size(484, 50);
		((Control)TxtsSmsManageKH).TabIndex = 17;
		((Control)TxtsSmsManageKH).TabStop = false;
		((TextBoxBase)TxtsSmsManageKH).Click += TxtsSmsManageV_Click;
		((Control)TxtsSmsManageKH).Enter += TxtMobs_Enter;
		((Control)TxtsSmsManageKH).Leave += TxtMobs_Leave;
		((Control)groupPanel14).Anchor = (AnchorStyles)13;
		((PanelControl)groupPanel14).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel14).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel14).Controls.Add((Control)(object)BtnPerFamily);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnTime);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnKK);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnEK);
		((Control)groupPanel14).Controls.Add((Control)(object)BtntypeTaradod);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnCodeMoein);
		((Control)groupPanel14).Controls.Add((Control)(object)btnCodeT);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnDate);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnPerName);
		groupPanel14.DisabledBackColor = Color.Empty;
		((Control)groupPanel14).Location = new Point(14, 47);
		((Control)groupPanel14).Name = "groupPanel14";
		((Control)groupPanel14).Size = new Size(440, 432);
		((PanelControl)groupPanel14).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel14).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel14).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel14).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel14).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel14).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel14).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel14).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel14).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel14).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel14).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel14).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel14).Style.CornerDiameter = 4;
		((PanelControl)groupPanel14).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel14).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel14).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel14).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel14).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel14).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel14).TabIndex = 1;
		((Control)BtnPerFamily).AccessibleRole = (AccessibleRole)43;
		BtnPerFamily.ColorTable = (eButtonColor)6;
		((Control)BtnPerFamily).Location = new Point(4, 49);
		((Control)BtnPerFamily).Name = "BtnPerFamily";
		((Control)BtnPerFamily).Size = new Size(87, 28);
		((PopupItemControl)BtnPerFamily).Style = (eDotNetBarStyle)9;
		((Control)BtnPerFamily).TabIndex = 8;
		((Control)BtnPerFamily).Text = "[فامیل]";
		((Control)BtnPerFamily).Click += BtnPayamak_Click;
		((Control)BtnTime).AccessibleRole = (AccessibleRole)43;
		BtnTime.ColorTable = (eButtonColor)6;
		((Control)BtnTime).Location = new Point(4, 233);
		((Control)BtnTime).Name = "BtnTime";
		((Control)BtnTime).Size = new Size(87, 28);
		((PopupItemControl)BtnTime).Style = (eDotNetBarStyle)9;
		((Control)BtnTime).TabIndex = 4;
		((Control)BtnTime).Text = "[زمان]";
		((Control)BtnTime).Click += BtnPayamak_Click;
		((Control)BtnKK).AccessibleRole = (AccessibleRole)43;
		BtnKK.ColorTable = (eButtonColor)6;
		((Control)BtnKK).Location = new Point(4, 371);
		((Control)BtnKK).Name = "BtnKK";
		((Control)BtnKK).Size = new Size(87, 28);
		((PopupItemControl)BtnKK).Style = (eDotNetBarStyle)9;
		((Control)BtnKK).TabIndex = 7;
		((Control)BtnKK).Text = "[کسرکار]";
		((Control)BtnKK).Click += BtnPayamak_Click;
		((Control)BtnEK).AccessibleRole = (AccessibleRole)43;
		BtnEK.ColorTable = (eButtonColor)6;
		((Control)BtnEK).Location = new Point(4, 325);
		((Control)BtnEK).Name = "BtnEK";
		((Control)BtnEK).Size = new Size(87, 28);
		((PopupItemControl)BtnEK).Style = (eDotNetBarStyle)9;
		((Control)BtnEK).TabIndex = 6;
		((Control)BtnEK).Text = "[اضافه کار]";
		((Control)BtnEK).Click += BtnPayamak_Click;
		((Control)BtntypeTaradod).AccessibleRole = (AccessibleRole)43;
		BtntypeTaradod.ColorTable = (eButtonColor)6;
		((Control)BtntypeTaradod).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtntypeTaradod).Location = new Point(4, 279);
		((Control)BtntypeTaradod).Name = "BtntypeTaradod";
		((Control)BtntypeTaradod).Size = new Size(87, 28);
		((PopupItemControl)BtntypeTaradod).Style = (eDotNetBarStyle)9;
		((Control)BtntypeTaradod).TabIndex = 5;
		((Control)BtntypeTaradod).Text = "[نوع تردد]";
		((Control)BtntypeTaradod).Click += BtnPayamak_Click;
		((Control)BtnCodeMoein).AccessibleRole = (AccessibleRole)43;
		BtnCodeMoein.ColorTable = (eButtonColor)6;
		((Control)BtnCodeMoein).Location = new Point(4, 95);
		((Control)BtnCodeMoein).Name = "BtnCodeMoein";
		((Control)BtnCodeMoein).Size = new Size(87, 28);
		((PopupItemControl)BtnCodeMoein).Style = (eDotNetBarStyle)9;
		((Control)BtnCodeMoein).TabIndex = 1;
		((Control)BtnCodeMoein).Text = "[کد معین]";
		((Control)BtnCodeMoein).Click += BtnPayamak_Click;
		((Control)btnCodeT).AccessibleRole = (AccessibleRole)43;
		btnCodeT.ColorTable = (eButtonColor)6;
		((Control)btnCodeT).Location = new Point(4, 141);
		((Control)btnCodeT).Name = "btnCodeT";
		((Control)btnCodeT).Size = new Size(87, 28);
		((PopupItemControl)btnCodeT).Style = (eDotNetBarStyle)9;
		((Control)btnCodeT).TabIndex = 2;
		((Control)btnCodeT).Text = "[کد تفصیل]";
		((Control)btnCodeT).Click += BtnPayamak_Click;
		((Control)BtnDate).AccessibleRole = (AccessibleRole)43;
		BtnDate.ColorTable = (eButtonColor)6;
		((Control)BtnDate).Location = new Point(4, 187);
		((Control)BtnDate).Name = "BtnDate";
		((Control)BtnDate).Size = new Size(87, 28);
		((PopupItemControl)BtnDate).Style = (eDotNetBarStyle)9;
		((Control)BtnDate).TabIndex = 3;
		((Control)BtnDate).Text = "[تاریخ]";
		((Control)BtnDate).Click += BtnPayamak_Click;
		((Control)BtnPerName).AccessibleRole = (AccessibleRole)43;
		BtnPerName.ColorTable = (eButtonColor)6;
		((Control)BtnPerName).Location = new Point(4, 3);
		((Control)BtnPerName).Name = "BtnPerName";
		((Control)BtnPerName).Size = new Size(87, 28);
		((PopupItemControl)BtnPerName).Style = (eDotNetBarStyle)9;
		((Control)BtnPerName).TabIndex = 0;
		((Control)BtnPerName).Text = "[نام شخص]";
		((Control)BtnPerName).Click += BtnPayamak_Click;
		((Control)pictureBox6).Anchor = (AnchorStyles)9;
		((Control)pictureBox6).BackColor = Color.Transparent;
		pictureBox6.Image = (Image)(object)Resources.mail_send;
		((Control)pictureBox6).Location = new Point(1051, 4);
		((Control)pictureBox6).Name = "pictureBox6";
		((Control)pictureBox6).Size = new Size(62, 37);
		pictureBox6.SizeMode = (PictureBoxSizeMode)4;
		pictureBox6.TabIndex = 19;
		pictureBox6.TabStop = false;
		tabItem6.AttachedControl = (Control)(object)tabControlPanel6;
		tabItem6.Name = "tabItem6";
		tabItem6.Text = "[پـیـامک]";
		((PanelEx)tabControlPanel1).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[نوع قلم]";
		tabItem1.Visible = false;
		((Control)tabControlPanel7).Controls.Add((Control)(object)pictureBox7);
		((Control)tabControlPanel7).Controls.Add((Control)(object)groupPanel12);
		((PanelEx)tabControlPanel7).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel7).Dock = (DockStyle)5;
		((Control)tabControlPanel7).Location = new Point(0, 26);
		((Control)tabControlPanel7).Name = "tabControlPanel7";
		((Control)tabControlPanel7).Padding = new Padding(1);
		((Control)tabControlPanel7).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel7).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel7).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel7).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel7).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel7).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel7).Style.GradientAngle = 90;
		((Control)tabControlPanel7).TabIndex = 0;
		tabControlPanel7.TabItem = tabItem7;
		((Control)pictureBox7).Anchor = (AnchorStyles)9;
		((Control)pictureBox7).BackColor = Color.Transparent;
		pictureBox7.Image = (Image)(object)Resources.work_network;
		((Control)pictureBox7).Location = new Point(1031, 4);
		((Control)pictureBox7).Name = "pictureBox7";
		((Control)pictureBox7).Size = new Size(81, 78);
		pictureBox7.SizeMode = (PictureBoxSizeMode)4;
		pictureBox7.TabIndex = 14;
		pictureBox7.TabStop = false;
		((Control)groupPanel12).BackColor = Color.Transparent;
		((PanelControl)groupPanel12).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel12).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel12).Controls.Add((Control)(object)labelX14);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtSabt);
		((Control)groupPanel12).Controls.Add((Control)(object)Label42);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtShrh);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX21);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtCodeEkhtesasi);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX20);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtTel);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX19);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtADD);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX18);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtFax);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX16);
		((Control)groupPanel12).Controls.Add((Control)(object)TxtCompany);
		((Control)groupPanel12).Controls.Add((Control)(object)labelX1);
		groupPanel12.DisabledBackColor = Color.Empty;
		((Control)groupPanel12).Dock = (DockStyle)5;
		((Control)groupPanel12).Location = new Point(1, 1);
		((Control)groupPanel12).Name = "groupPanel12";
		((Control)groupPanel12).Size = new Size(1122, 481);
		((PanelControl)groupPanel12).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel12).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel12).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel12).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel12).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel12).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel12).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel12).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel12).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel12).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel12).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel12).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel12).Style.CornerDiameter = 4;
		((PanelControl)groupPanel12).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel12).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel12).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel12).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel12).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel12).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel12).TabIndex = 0;
		((Control)labelX14).Anchor = (AnchorStyles)9;
		((Control)labelX14).BackColor = Color.Transparent;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX14).Location = new Point(956, 162);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(157, 23);
		((Control)labelX14).TabIndex = 5;
		((Control)labelX14).Text = "[شماره ثبت] :";
		labelX14.TextAlignment = (StringAlignment)2;
		((Control)TxtSabt).Anchor = (AnchorStyles)13;
		TxtSabt.Border.Class = "TextBoxBorder";
		TxtSabt.Border.CornerType = (eCornerType)1;
		((Control)TxtSabt).Location = new Point(30, 163);
		((Control)TxtSabt).Name = "TxtSabt";
		((Control)TxtSabt).Size = new Size(922, 21);
		((Control)TxtSabt).TabIndex = 6;
		((Control)TxtSabt).Enter += TxtMobs_Enter;
		((Control)TxtSabt).Leave += TxtMobs_Leave;
		((Control)Label42).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)Label42).ForeColor = Color.Red;
		((Control)Label42).Location = new Point(30, 28);
		((Control)Label42).Name = "Label42";
		((Control)Label42).Size = new Size(922, 71);
		((Control)Label42).TabIndex = 0;
		((Control)Label42).Text = "[توجه :مشخصات شرکت که در این بخش مشخص میشوند ، در اسناد چاپی  نمایش داده خواهند شد .]";
		((Control)TxtShrh).Anchor = (AnchorStyles)15;
		TxtShrh.Border.Class = "TextBoxBorder";
		TxtShrh.Border.CornerType = (eCornerType)1;
		((Control)TxtShrh).Location = new Point(30, 280);
		((TextBoxBase)TxtShrh).Multiline = true;
		((Control)TxtShrh).Name = "TxtShrh";
		((Control)TxtShrh).Size = new Size(922, 178);
		((Control)TxtShrh).TabIndex = 14;
		((Control)TxtShrh).Enter += TxtMobs_Enter;
		((Control)TxtShrh).Leave += TxtMobs_Leave;
		((Control)labelX21).Anchor = (AnchorStyles)9;
		((Control)labelX21).BackColor = Color.Transparent;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX21).Location = new Point(956, 284);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(157, 23);
		((Control)labelX21).TabIndex = 13;
		((Control)labelX21).Text = "[شرح مختصری درباره شرکت] :";
		labelX21.TextAlignment = (StringAlignment)2;
		((Control)TxtCodeEkhtesasi).Anchor = (AnchorStyles)13;
		TxtCodeEkhtesasi.Border.Class = "TextBoxBorder";
		TxtCodeEkhtesasi.Border.CornerType = (eCornerType)1;
		((Control)TxtCodeEkhtesasi).Location = new Point(30, 133);
		((Control)TxtCodeEkhtesasi).Name = "TxtCodeEkhtesasi";
		((Control)TxtCodeEkhtesasi).Size = new Size(922, 21);
		((Control)TxtCodeEkhtesasi).TabIndex = 4;
		((Control)TxtCodeEkhtesasi).Enter += TxtMobs_Enter;
		((Control)TxtCodeEkhtesasi).Leave += TxtMobs_Leave;
		((Control)labelX20).Anchor = (AnchorStyles)9;
		((Control)labelX20).BackColor = Color.Transparent;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX20).Location = new Point(956, 131);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(157, 23);
		((Control)labelX20).TabIndex = 3;
		((Control)labelX20).Text = "[کد اقتصادی] :";
		labelX20.TextAlignment = (StringAlignment)2;
		((Control)TxtTel).Anchor = (AnchorStyles)13;
		TxtTel.Border.Class = "TextBoxBorder";
		TxtTel.Border.CornerType = (eCornerType)1;
		((Control)TxtTel).Location = new Point(30, 193);
		((TextBoxBase)TxtTel).MaxLength = 11;
		((Control)TxtTel).Name = "TxtTel";
		((Control)TxtTel).Size = new Size(922, 21);
		((Control)TxtTel).TabIndex = 8;
		((Control)TxtTel).Enter += TxtMobs_Enter;
		((Control)TxtTel).Leave += TxtMobs_Leave;
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((Control)labelX19).BackColor = Color.Transparent;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX19).Location = new Point(956, 191);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(157, 23);
		((Control)labelX19).TabIndex = 7;
		((Control)labelX19).Text = "[تلفن] :";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)TxtADD).Anchor = (AnchorStyles)13;
		TxtADD.Border.Class = "TextBoxBorder";
		TxtADD.Border.CornerType = (eCornerType)1;
		((Control)TxtADD).Location = new Point(30, 253);
		((TextBoxBase)TxtADD).Multiline = true;
		((Control)TxtADD).Name = "TxtADD";
		((Control)TxtADD).Size = new Size(922, 21);
		((Control)TxtADD).TabIndex = 12;
		((Control)TxtADD).Enter += TxtMobs_Enter;
		((Control)TxtADD).Leave += TxtMobs_Leave;
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((Control)labelX18).BackColor = Color.Transparent;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX18).Location = new Point(956, 221);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(157, 23);
		((Control)labelX18).TabIndex = 9;
		((Control)labelX18).Text = "[فاکس] :";
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)TxtFax).Anchor = (AnchorStyles)13;
		TxtFax.Border.Class = "TextBoxBorder";
		TxtFax.Border.CornerType = (eCornerType)1;
		((Control)TxtFax).Location = new Point(30, 223);
		((TextBoxBase)TxtFax).MaxLength = 11;
		((Control)TxtFax).Name = "TxtFax";
		((Control)TxtFax).Size = new Size(922, 21);
		((Control)TxtFax).TabIndex = 10;
		((Control)TxtFax).Enter += TxtMobs_Enter;
		((Control)TxtFax).Leave += TxtMobs_Leave;
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((Control)labelX16).BackColor = Color.Transparent;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX16).Location = new Point(956, 250);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(157, 23);
		((Control)labelX16).TabIndex = 11;
		((Control)labelX16).Text = "[آدرس] :";
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)TxtCompany).Anchor = (AnchorStyles)13;
		TxtCompany.Border.Class = "TextBoxBorder";
		TxtCompany.Border.CornerType = (eCornerType)1;
		((Control)TxtCompany).Location = new Point(30, 103);
		((Control)TxtCompany).Name = "TxtCompany";
		((Control)TxtCompany).Size = new Size(922, 21);
		((Control)TxtCompany).TabIndex = 1;
		((Control)TxtCompany).Enter += TxtMobs_Enter;
		((Control)TxtCompany).Leave += TxtMobs_Leave;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX1).Location = new Point(956, 101);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(157, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[عنوان شرکت] :";
		labelX1.TextAlignment = (StringAlignment)2;
		tabItem7.AttachedControl = (Control)(object)tabControlPanel7;
		tabItem7.Name = "tabItem7";
		tabItem7.Text = "[مشخصات شرکت]";
		((Control)tabControlPanel9).Anchor = (AnchorStyles)15;
		((Panel)tabControlPanel9).AutoSizeMode = (AutoSizeMode)0;
		((Control)tabControlPanel9).Controls.Add((Control)(object)ChkWebService);
		((Control)tabControlPanel9).Controls.Add((Control)(object)GrpWebservice);
		((PanelEx)tabControlPanel9).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel9).Location = new Point(0, 26);
		((Control)tabControlPanel9).Name = "tabControlPanel9";
		((Control)tabControlPanel9).Padding = new Padding(1);
		((Control)tabControlPanel9).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel9).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel9).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel9).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel9).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel9).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel9).Style.GradientAngle = 90;
		((Control)tabControlPanel9).TabIndex = 2;
		tabControlPanel9.TabItem = WebServiceTab;
		((Control)ChkWebService).Anchor = (AnchorStyles)9;
		((Control)ChkWebService).BackColor = Color.Transparent;
		((BaseItemControl)ChkWebService).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkWebService).Location = new Point(885, 13);
		((Control)ChkWebService).Name = "ChkWebService";
		((Control)ChkWebService).Size = new Size(228, 23);
		((BaseItemControl)ChkWebService).Style = (eDotNetBarStyle)9;
		((Control)ChkWebService).TabIndex = 31;
		((Control)ChkWebService).Text = "[ارتباط با وب سرویس برقرار باشد]";
		ChkWebService.CheckedChanged += ChkWebService_CheckedChanged;
		((Control)GrpWebservice).Anchor = (AnchorStyles)13;
		((PanelControl)GrpWebservice).CanvasColor = SystemColors.Control;
		((PanelControl)GrpWebservice).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GrpWebservice).Controls.Add((Control)(object)CmbURL);
		((Control)GrpWebservice).Controls.Add((Control)(object)lblURL);
		((Control)GrpWebservice).Controls.Add((Control)(object)lbllinkwebservice);
		((Control)GrpWebservice).Controls.Add((Control)(object)labelX67);
		((Control)GrpWebservice).Controls.Add((Control)(object)TxtNpPass);
		((Control)GrpWebservice).Controls.Add((Control)(object)labelX66);
		((Control)GrpWebservice).Controls.Add((Control)(object)TxtNpUser);
		GrpWebservice.DisabledBackColor = Color.Empty;
		((Control)GrpWebservice).Location = new Point(13, 52);
		((Control)GrpWebservice).Name = "GrpWebservice";
		((Control)GrpWebservice).Size = new Size(1100, 69);
		((PanelControl)GrpWebservice).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GrpWebservice).Style.BackColorGradientAngle = 90;
		((PanelControl)GrpWebservice).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GrpWebservice).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GrpWebservice).Style.BorderBottomWidth = 1;
		((PanelControl)GrpWebservice).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GrpWebservice).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GrpWebservice).Style.BorderLeftWidth = 1;
		((PanelControl)GrpWebservice).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GrpWebservice).Style.BorderRightWidth = 1;
		((PanelControl)GrpWebservice).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GrpWebservice).Style.BorderTopWidth = 1;
		((PanelControl)GrpWebservice).Style.CornerDiameter = 4;
		((PanelControl)GrpWebservice).Style.CornerType = (eCornerType)2;
		((PanelControl)GrpWebservice).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)GrpWebservice).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GrpWebservice).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GrpWebservice).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GrpWebservice).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GrpWebservice).TabIndex = 30;
		((ListControl)CmbURL).FormattingEnabled = true;
		CmbURL.Items.AddRange(new object[4] { "http://www.NovinRank.ir/hubCore", "http://www.Api.NovinP.com/hubCore", "http://www.WebService.NpMail.ir/hubCore", "http://localhost:7005/hubCore" });
		((Control)CmbURL).Location = new Point(170, 22);
		((Control)CmbURL).Name = "CmbURL";
		((Control)CmbURL).Size = new Size(247, 21);
		((Control)CmbURL).TabIndex = 10;
		((Control)lblURL).Anchor = (AnchorStyles)9;
		((Control)lblURL).BackColor = Color.Transparent;
		((BaseItemControl)lblURL).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblURL).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblURL).Location = new Point(432, 21);
		((Control)lblURL).Name = "lblURL";
		((Control)lblURL).Size = new Size(75, 23);
		((Control)lblURL).TabIndex = 9;
		((Control)lblURL).Text = "[آدرس سرور]";
		lblURL.TextAlignment = (StringAlignment)2;
		((Control)lbllinkwebservice).BackColor = Color.Cyan;
		lbllinkwebservice.DisabledLinkColor = SystemColors.ActiveCaption;
		((Control)lbllinkwebservice).Location = new Point(23, 21);
		((Control)lbllinkwebservice).Name = "lbllinkwebservice";
		((Control)lbllinkwebservice).RightToLeft = (RightToLeft)0;
		((Control)lbllinkwebservice).Size = new Size(139, 23);
		((Control)lbllinkwebservice).TabIndex = 7;
		lbllinkwebservice.TabStop = true;
		((Control)lbllinkwebservice).Text = "[ثبت نام وب سرویس]";
		((Label)lbllinkwebservice).TextAlign = (ContentAlignment)16;
		lbllinkwebservice.LinkClicked += new LinkLabelLinkClickedEventHandler(LblLinkWebService_LinkClicked);
		((Control)labelX67).Anchor = (AnchorStyles)9;
		((Control)labelX67).BackColor = Color.Transparent;
		((BaseItemControl)labelX67).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX67).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX67).Location = new Point(728, 21);
		((Control)labelX67).Name = "labelX67";
		((Control)labelX67).Size = new Size(75, 23);
		((Control)labelX67).TabIndex = 6;
		((Control)labelX67).Text = "[رمز عبور] :";
		labelX67.TextAlignment = (StringAlignment)2;
		((Control)TxtNpPass).Anchor = (AnchorStyles)9;
		((Control)TxtNpPass).Location = new Point(558, 22);
		((Control)TxtNpPass).Name = "TxtNpPass";
		((Control)TxtNpPass).Size = new Size(155, 21);
		((Control)TxtNpPass).TabIndex = 5;
		((Control)TxtNpPass).TextChanged += TxtNpPass_TextChanged;
		((Control)labelX66).Anchor = (AnchorStyles)9;
		((Control)labelX66).BackColor = Color.Transparent;
		((BaseItemControl)labelX66).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX66).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX66).Location = new Point(1003, 21);
		((Control)labelX66).Name = "labelX66";
		((Control)labelX66).Size = new Size(75, 23);
		((Control)labelX66).TabIndex = 5;
		((Control)labelX66).Text = "[نام کاربری] :";
		labelX66.TextAlignment = (StringAlignment)2;
		((Control)TxtNpUser).Anchor = (AnchorStyles)9;
		((Control)TxtNpUser).Location = new Point(834, 22);
		((Control)TxtNpUser).Name = "TxtNpUser";
		((Control)TxtNpUser).Size = new Size(148, 21);
		((Control)TxtNpUser).TabIndex = 4;
		((Control)TxtNpUser).TextChanged += TxtNpUser_TextChanged;
		WebServiceTab.AttachedControl = (Control)(object)tabControlPanel9;
		WebServiceTab.Name = "WebServiceTab";
		WebServiceTab.Text = "[همگام سازی با سرور]";
		((Control)tabControlPanel10).Controls.Add((Control)(object)labelX79);
		((Control)tabControlPanel10).Controls.Add((Control)(object)NumGetTaradodFormCountDaysAgo);
		((Control)tabControlPanel10).Controls.Add((Control)(object)labelX75);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkCalcEK_KK);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkCalcEkInSourceDate);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkCalcEkWhenHasVacationOrMission);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkAutomaticCalcMoveToNextDay);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkNotKarkardWhenNoGharardad);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkCalcEzafeOutOfShift);
		((Control)tabControlPanel10).Controls.Add((Control)(object)ChkCalcHoliday);
		((PanelEx)tabControlPanel10).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel10).Dock = (DockStyle)5;
		((Control)tabControlPanel10).Location = new Point(0, 26);
		((Control)tabControlPanel10).Name = "tabControlPanel10";
		((Control)tabControlPanel10).Padding = new Padding(1);
		((Control)tabControlPanel10).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel10).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel10).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel10).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel10).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel10).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel10).Style.GradientAngle = 90;
		((Control)tabControlPanel10).TabIndex = 28;
		tabControlPanel10.TabItem = tabItemSettingTaradod;
		((Control)labelX79).Anchor = (AnchorStyles)9;
		((Control)labelX79).BackColor = Color.Transparent;
		((BaseItemControl)labelX79).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX79).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX79).Location = new Point(24, 98);
		((Control)labelX79).Name = "labelX79";
		((Control)labelX79).RightToLeft = (RightToLeft)1;
		((Control)labelX79).Size = new Size(548, 23);
		((Control)labelX79).TabIndex = 28;
		((Control)labelX79).Text = "به جهت عدم پردازش ترددهایی که قبلا نیز دریافت شده است بهتر است بازه متناسب با نیاز درج شود";
		((Control)NumGetTaradodFormCountDaysAgo).Anchor = (AnchorStyles)9;
		((Control)NumGetTaradodFormCountDaysAgo).Location = new Point(586, 100);
		NumGetTaradodFormCountDaysAgo.Maximum = new decimal(new int[4] { 730, 0, 0, 0 });
		NumGetTaradodFormCountDaysAgo.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumGetTaradodFormCountDaysAgo).Name = "NumGetTaradodFormCountDaysAgo";
		((Control)NumGetTaradodFormCountDaysAgo).Size = new Size(70, 21);
		((Control)NumGetTaradodFormCountDaysAgo).TabIndex = 27;
		((UpDownBase)NumGetTaradodFormCountDaysAgo).TextAlign = (HorizontalAlignment)2;
		NumGetTaradodFormCountDaysAgo.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		((Control)labelX75).Anchor = (AnchorStyles)9;
		((Control)labelX75).BackColor = Color.Transparent;
		((BaseItemControl)labelX75).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX75).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX75).Location = new Point(661, 99);
		((Control)labelX75).Name = "labelX75";
		((Control)labelX75).RightToLeft = (RightToLeft)1;
		((Control)labelX75).Size = new Size(432, 23);
		((Control)labelX75).TabIndex = 26;
		((Control)labelX75).Text = "هنگام دریافت ترددها از فایل، ترددهای مربوط به چند روز قبل تا کنون، دریافت شوند؟";
		((Control)ChkCalcEK_KK).Anchor = (AnchorStyles)9;
		((Control)ChkCalcEK_KK).BackColor = Color.Transparent;
		((BaseItemControl)ChkCalcEK_KK).BackgroundStyle.CornerType = (eCornerType)1;
		ChkCalcEK_KK.ConsiderEmptyStringAsNull = false;
		((Control)ChkCalcEK_KK).Location = new Point(216, 215);
		((Control)ChkCalcEK_KK).Name = "ChkCalcEK_KK";
		((Control)ChkCalcEK_KK).RightToLeft = (RightToLeft)1;
		((Control)ChkCalcEK_KK).Size = new Size(877, 23);
		((BaseItemControl)ChkCalcEK_KK).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEK_KK).TabIndex = 24;
		((Control)ChkCalcEK_KK).Text = "  اضافه کار، کسر کار محاسبه نشود . بیشتر برای مدارس و جاهایی که کسر کار و اضافه کار کاربرد ندارد استفاده می شود  تا سرعت پردازش گزارشات بیشتر شود";
		((Control)ChkCalcEK_KK).Visible = false;
		((Control)ChkCalcEkInSourceDate).Anchor = (AnchorStyles)9;
		((Control)ChkCalcEkInSourceDate).BackColor = Color.Transparent;
		((BaseItemControl)ChkCalcEkInSourceDate).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkCalcEkInSourceDate).Enabled = false;
		((Control)ChkCalcEkInSourceDate).Location = new Point(575, 157);
		((Control)ChkCalcEkInSourceDate).Name = "ChkCalcEkInSourceDate";
		((Control)ChkCalcEkInSourceDate).RightToLeft = (RightToLeft)1;
		((Control)ChkCalcEkInSourceDate).Size = new Size(518, 23);
		((BaseItemControl)ChkCalcEkInSourceDate).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEkInSourceDate).TabIndex = 23;
		((Control)ChkCalcEkInSourceDate).Text = "کارکرد و اضافه کار نیز در روز مبدا محاسبه شود";
		((Control)ChkCalcEkWhenHasVacationOrMission).Anchor = (AnchorStyles)9;
		((Control)ChkCalcEkWhenHasVacationOrMission).BackColor = Color.Transparent;
		((BaseItemControl)ChkCalcEkWhenHasVacationOrMission).BackgroundStyle.CornerType = (eCornerType)1;
		ChkCalcEkWhenHasVacationOrMission.Checked = true;
		ChkCalcEkWhenHasVacationOrMission.CheckState = (CheckState)1;
		ChkCalcEkWhenHasVacationOrMission.CheckValue = "Y";
		ChkCalcEkWhenHasVacationOrMission.ConsiderEmptyStringAsNull = false;
		((Control)ChkCalcEkWhenHasVacationOrMission).Location = new Point(575, 70);
		((Control)ChkCalcEkWhenHasVacationOrMission).Name = "ChkCalcEkWhenHasVacationOrMission";
		((Control)ChkCalcEkWhenHasVacationOrMission).RightToLeft = (RightToLeft)1;
		((Control)ChkCalcEkWhenHasVacationOrMission).Size = new Size(518, 23);
		((BaseItemControl)ChkCalcEkWhenHasVacationOrMission).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEkWhenHasVacationOrMission).TabIndex = 22;
		((Control)ChkCalcEkWhenHasVacationOrMission).Text = "کارکرد ایامی که مرخصی یا ماموریت می باشد بعنوان اضافه کار محاسبه شود";
		((Control)ChkAutomaticCalcMoveToNextDay).Anchor = (AnchorStyles)9;
		((Control)ChkAutomaticCalcMoveToNextDay).BackColor = Color.Transparent;
		((BaseItemControl)ChkAutomaticCalcMoveToNextDay).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkAutomaticCalcMoveToNextDay).Location = new Point(12, 128);
		((Control)ChkAutomaticCalcMoveToNextDay).Name = "ChkAutomaticCalcMoveToNextDay";
		((Control)ChkAutomaticCalcMoveToNextDay).RightToLeft = (RightToLeft)1;
		((Control)ChkAutomaticCalcMoveToNextDay).Size = new Size(1081, 23);
		((BaseItemControl)ChkAutomaticCalcMoveToNextDay).Style = (eDotNetBarStyle)9;
		((Control)ChkAutomaticCalcMoveToNextDay).TabIndex = 21;
		((Control)ChkAutomaticCalcMoveToNextDay).Text = "بصورت اتومات عملیات انتقال به روز بعد انجام شود.    در صورتیکه قرارداد شخص فاقد شیفت باشد یا چند روز تعطیلی بدون شیفت داشته باشد این ویژگی کاربرد خواهد داشت";
		ChkAutomaticCalcMoveToNextDay.CheckedChanged += ChkAutomaticCalcMoveToNextDay_CheckedChanged;
		((Control)ChkNotKarkardWhenNoGharardad).Anchor = (AnchorStyles)9;
		((Control)ChkNotKarkardWhenNoGharardad).BackColor = Color.Transparent;
		((BaseItemControl)ChkNotKarkardWhenNoGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		ChkNotKarkardWhenNoGharardad.Checked = true;
		ChkNotKarkardWhenNoGharardad.CheckState = (CheckState)1;
		ChkNotKarkardWhenNoGharardad.CheckValue = "Y";
		((Control)ChkNotKarkardWhenNoGharardad).Location = new Point(575, 41);
		((Control)ChkNotKarkardWhenNoGharardad).Name = "ChkNotKarkardWhenNoGharardad";
		((Control)ChkNotKarkardWhenNoGharardad).RightToLeft = (RightToLeft)1;
		((Control)ChkNotKarkardWhenNoGharardad).Size = new Size(518, 23);
		((BaseItemControl)ChkNotKarkardWhenNoGharardad).Style = (eDotNetBarStyle)9;
		((Control)ChkNotKarkardWhenNoGharardad).TabIndex = 20;
		((Control)ChkNotKarkardWhenNoGharardad).Text = "[روزهای خارج از بازه قرارداد شخص جزء روزهای کارکرد محاسبه نشود]";
		((Control)ChkCalcEzafeOutOfShift).Anchor = (AnchorStyles)9;
		((Control)ChkCalcEzafeOutOfShift).BackColor = Color.Transparent;
		((BaseItemControl)ChkCalcEzafeOutOfShift).BackgroundStyle.CornerType = (eCornerType)1;
		ChkCalcEzafeOutOfShift.Checked = true;
		ChkCalcEzafeOutOfShift.CheckState = (CheckState)1;
		ChkCalcEzafeOutOfShift.CheckValue = "Y";
		((Control)ChkCalcEzafeOutOfShift).Location = new Point(575, 12);
		((Control)ChkCalcEzafeOutOfShift).Name = "ChkCalcEzafeOutOfShift";
		((Control)ChkCalcEzafeOutOfShift).RightToLeft = (RightToLeft)1;
		((Control)ChkCalcEzafeOutOfShift).Size = new Size(518, 23);
		((BaseItemControl)ChkCalcEzafeOutOfShift).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcEzafeOutOfShift).TabIndex = 19;
		((Control)ChkCalcEzafeOutOfShift).Text = "[بازه ترددهای خارج از شیفت بعنوان اضافه کار محاسبه شود]";
		((Control)ChkCalcHoliday).Anchor = (AnchorStyles)9;
		((Control)ChkCalcHoliday).BackColor = Color.Transparent;
		((BaseItemControl)ChkCalcHoliday).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkCalcHoliday).Location = new Point(549, 186);
		((Control)ChkCalcHoliday).Name = "ChkCalcHoliday";
		((Control)ChkCalcHoliday).RightToLeft = (RightToLeft)1;
		((Control)ChkCalcHoliday).Size = new Size(544, 23);
		((BaseItemControl)ChkCalcHoliday).Style = (eDotNetBarStyle)9;
		((Control)ChkCalcHoliday).TabIndex = 18;
		((Control)ChkCalcHoliday).Text = "[روزهای جمعه کارکرد به عنوان جمعه کاری و روزهای تعطیل بعنوان تعطیل کاری در نظر گرفته شود]";
		tabItemSettingTaradod.AttachedControl = (Control)(object)tabControlPanel10;
		tabItemSettingTaradod.Name = "tabItemSettingTaradod";
		tabItemSettingTaradod.Text = "محاسبات تردد";
		((Control)tabControlPanel8).Controls.Add((Control)(object)labelX50);
		((Control)tabControlPanel8).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)tabControlPanel8).Controls.Add((Control)(object)ChkFromDateEndedGharardad);
		((Control)tabControlPanel8).Controls.Add((Control)(object)labelX48);
		((Control)tabControlPanel8).Controls.Add((Control)(object)labelX49);
		((Control)tabControlPanel8).Controls.Add((Control)(object)NUMGharardadEndedNext);
		((Control)tabControlPanel8).Controls.Add((Control)(object)labelX52);
		((Control)tabControlPanel8).Controls.Add((Control)(object)labelX54);
		((Control)tabControlPanel8).Controls.Add((Control)(object)ChkViewEndedGharardad);
		((Control)tabControlPanel8).Controls.Add((Control)(object)NUMGharardadEndedAgo);
		((PanelEx)tabControlPanel8).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel8).Dock = (DockStyle)5;
		((Control)tabControlPanel8).Location = new Point(0, 26);
		((Control)tabControlPanel8).Name = "tabControlPanel8";
		((Control)tabControlPanel8).Padding = new Padding(1);
		((Control)tabControlPanel8).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel8).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel8).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel8).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel8).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel8).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel8).Style.GradientAngle = 90;
		((Control)tabControlPanel8).TabIndex = 1;
		tabControlPanel8.TabItem = tabItem8;
		((Control)labelX50).Anchor = (AnchorStyles)9;
		((Control)labelX50).BackColor = Color.Transparent;
		((BaseItemControl)labelX50).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX50).Location = new Point(834, 163);
		((Control)labelX50).Name = "labelX50";
		((Control)labelX50).Size = new Size(271, 23);
		((Control)labelX50).TabIndex = 19;
		((Control)labelX50).Text = "[نمایش قراردادهای با وضعیت های انتخاب شده زیر] :";
		((Control)flowLayoutPanel1).Anchor = (AnchorStyles)13;
		((Control)flowLayoutPanel1).AutoSize = true;
		((Control)flowLayoutPanel1).BackColor = Color.White;
		((Control)flowLayoutPanel1).Location = new Point(11, 190);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(1096, 33);
		((Control)flowLayoutPanel1).TabIndex = 18;
		((Control)ChkFromDateEndedGharardad).Anchor = (AnchorStyles)9;
		((Control)ChkFromDateEndedGharardad).BackColor = Color.Transparent;
		((BaseItemControl)ChkFromDateEndedGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkFromDateEndedGharardad).Location = new Point(723, 51);
		((Control)ChkFromDateEndedGharardad).Name = "ChkFromDateEndedGharardad";
		((Control)ChkFromDateEndedGharardad).Size = new Size(389, 23);
		((BaseItemControl)ChkFromDateEndedGharardad).Style = (eDotNetBarStyle)9;
		((Control)ChkFromDateEndedGharardad).TabIndex = 3;
		((Control)ChkFromDateEndedGharardad).Text = "[جستجو از تاریخ فعال باشد]";
		((Control)labelX48).Anchor = (AnchorStyles)9;
		((Control)labelX48).BackColor = Color.Transparent;
		((BaseItemControl)labelX48).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX48).Location = new Point(723, 118);
		((Control)labelX48).Name = "labelX48";
		((Control)labelX48).Size = new Size(106, 23);
		((Control)labelX48).TabIndex = 0;
		((Control)labelX48).Text = "[روز آینده]";
		((Control)labelX49).Anchor = (AnchorStyles)9;
		((Control)labelX49).BackColor = Color.Transparent;
		((BaseItemControl)labelX49).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX49).Location = new Point(888, 118);
		((Control)labelX49).Name = "labelX49";
		((Control)labelX49).Size = new Size(199, 23);
		((Control)labelX49).TabIndex = 0;
		((Control)labelX49).Text = "[نمایش قراردادهای در حال خاتمه طی]";
		((Control)NUMGharardadEndedNext).Anchor = (AnchorStyles)9;
		((Control)NUMGharardadEndedNext).Location = new Point(835, 120);
		NUMGharardadEndedNext.Maximum = new decimal(new int[4] { 3650, 0, 0, 0 });
		((Control)NUMGharardadEndedNext).Name = "NUMGharardadEndedNext";
		((Control)NUMGharardadEndedNext).Size = new Size(54, 21);
		((Control)NUMGharardadEndedNext).TabIndex = 2;
		((UpDownBase)NUMGharardadEndedNext).TextAlign = (HorizontalAlignment)2;
		NUMGharardadEndedNext.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)labelX52).Anchor = (AnchorStyles)9;
		((Control)labelX52).BackColor = Color.Transparent;
		((BaseItemControl)labelX52).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX52).Location = new Point(722, 84);
		((Control)labelX52).Name = "labelX52";
		((Control)labelX52).Size = new Size(106, 23);
		((Control)labelX52).TabIndex = 0;
		((Control)labelX52).Text = "[روز قبل]";
		((Control)labelX54).Anchor = (AnchorStyles)9;
		((Control)labelX54).BackColor = Color.Transparent;
		((BaseItemControl)labelX54).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX54).Location = new Point(887, 84);
		((Control)labelX54).Name = "labelX54";
		((Control)labelX54).Size = new Size(198, 23);
		((Control)labelX54).TabIndex = 0;
		((Control)labelX54).Text = "[نمایش قراردادهای خاتمه یافته از ]";
		((Control)ChkViewEndedGharardad).Anchor = (AnchorStyles)9;
		((Control)ChkViewEndedGharardad).BackColor = Color.Transparent;
		((BaseItemControl)ChkViewEndedGharardad).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkViewEndedGharardad).Location = new Point(599, 24);
		((Control)ChkViewEndedGharardad).Name = "ChkViewEndedGharardad";
		((Control)ChkViewEndedGharardad).Size = new Size(513, 23);
		((BaseItemControl)ChkViewEndedGharardad).Style = (eDotNetBarStyle)9;
		((Control)ChkViewEndedGharardad).TabIndex = 0;
		((Control)ChkViewEndedGharardad).Text = "[نمایش صفحه یادآوری قراردادهای خاتمه یافته در حین ورود به نرم افزار]";
		((Control)NUMGharardadEndedAgo).Anchor = (AnchorStyles)9;
		((Control)NUMGharardadEndedAgo).Location = new Point(834, 86);
		NUMGharardadEndedAgo.Maximum = new decimal(new int[4] { 3650, 0, 0, 0 });
		((Control)NUMGharardadEndedAgo).Name = "NUMGharardadEndedAgo";
		((Control)NUMGharardadEndedAgo).Size = new Size(54, 21);
		((Control)NUMGharardadEndedAgo).TabIndex = 1;
		((UpDownBase)NUMGharardadEndedAgo).TextAlign = (HorizontalAlignment)2;
		NUMGharardadEndedAgo.Value = new decimal(new int[4] { 2, 0, 0, 0 });
		tabItem8.AttachedControl = (Control)(object)tabControlPanel8;
		tabItem8.Name = "tabItem8";
		tabItem8.Text = "[قراردادهای منقضی]";
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel9);
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel6);
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel5);
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel1);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LBLPreviwe);
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel3);
		((PanelEx)tabControlPanel2).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel2).Dock = (DockStyle)5;
		((Control)tabControlPanel2).Location = new Point(0, 26);
		((Control)tabControlPanel2).Name = "tabControlPanel2";
		((Control)tabControlPanel2).Padding = new Padding(1);
		((Control)tabControlPanel2).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel2).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel2).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel2).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel2).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel2).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel2).Style.GradientAngle = 90;
		((Control)tabControlPanel2).TabIndex = 0;
		tabControlPanel2.TabItem = tabItem2;
		((Control)groupPanel9).Anchor = (AnchorStyles)15;
		((PanelControl)groupPanel9).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel9).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel9).Controls.Add((Control)(object)ChkViewGridReportDevices);
		((Control)groupPanel9).Controls.Add((Control)(object)labelX69);
		((Control)groupPanel9).Controls.Add((Control)(object)ChkViewGridLastTaradods);
		((Control)groupPanel9).Controls.Add((Control)(object)TbxCountReportDevices);
		((Control)groupPanel9).Controls.Add((Control)(object)labelX68);
		((Control)groupPanel9).Controls.Add((Control)(object)TbxCountLastTaradods);
		((Control)groupPanel9).Controls.Add((Control)(object)CmbCalendarType);
		((Control)groupPanel9).Controls.Add((Control)(object)labelX59);
		groupPanel9.DisabledBackColor = Color.Empty;
		((Control)groupPanel9).Location = new Point(12, 254);
		((Control)groupPanel9).Name = "groupPanel9";
		((Control)groupPanel9).Size = new Size(1100, 100);
		((PanelControl)groupPanel9).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel9).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel9).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel9).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel9).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel9).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel9).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel9).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel9).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel9).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel9).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel9).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel9).Style.CornerDiameter = 4;
		((PanelControl)groupPanel9).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel9).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel9).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel9).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel9).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel9).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel9).TabIndex = 30;
		((Control)ChkViewGridReportDevices).Anchor = (AnchorStyles)9;
		((Control)ChkViewGridReportDevices).BackColor = Color.Transparent;
		((BaseItemControl)ChkViewGridReportDevices).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkViewGridReportDevices).Location = new Point(837, 49);
		((Control)ChkViewGridReportDevices).Name = "ChkViewGridReportDevices";
		((Control)ChkViewGridReportDevices).Size = new Size(254, 28);
		((BaseItemControl)ChkViewGridReportDevices).Style = (eDotNetBarStyle)9;
		((Control)ChkViewGridReportDevices).TabIndex = 36;
		((Control)ChkViewGridReportDevices).Text = "نمایش فرم وضعیت ارتباطات دستگاه ها";
		ChkViewGridReportDevices.CheckedChanged += ChkViewGridReportDevices_CheckedChanged;
		((Control)labelX69).Anchor = (AnchorStyles)9;
		((Control)labelX69).BackColor = Color.Transparent;
		((BaseItemControl)labelX69).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX69).Location = new Point(610, 50);
		((Control)labelX69).Name = "labelX69";
		((Control)labelX69).Size = new Size(109, 23);
		((Control)labelX69).TabIndex = 34;
		((Control)labelX69).Text = "تعداد رکورد گزارش:";
		((Control)ChkViewGridLastTaradods).Anchor = (AnchorStyles)9;
		((Control)ChkViewGridLastTaradods).BackColor = Color.Transparent;
		((BaseItemControl)ChkViewGridLastTaradods).BackgroundStyle.CornerType = (eCornerType)1;
		ChkViewGridLastTaradods.Checked = true;
		ChkViewGridLastTaradods.CheckState = (CheckState)1;
		ChkViewGridLastTaradods.CheckValue = "Y";
		((Control)ChkViewGridLastTaradods).Location = new Point(837, 28);
		((Control)ChkViewGridLastTaradods).Name = "ChkViewGridLastTaradods";
		((Control)ChkViewGridLastTaradods).Size = new Size(254, 28);
		((BaseItemControl)ChkViewGridLastTaradods).Style = (eDotNetBarStyle)9;
		((Control)ChkViewGridLastTaradods).TabIndex = 36;
		((Control)ChkViewGridLastTaradods).Text = "نمایش فرم آخرین ترددها در صفحه اصلی";
		ChkViewGridLastTaradods.CheckedChanged += ChkViewGridLastTaradods_CheckedChanged;
		((Control)TbxCountReportDevices).Anchor = (AnchorStyles)9;
		((Control)TbxCountReportDevices).Enabled = false;
		((Control)TbxCountReportDevices).Location = new Point(547, 51);
		TbxCountReportDevices.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TbxCountReportDevices).Name = "TbxCountReportDevices";
		((Control)TbxCountReportDevices).Size = new Size(54, 21);
		((Control)TbxCountReportDevices).TabIndex = 35;
		((UpDownBase)TbxCountReportDevices).TextAlign = (HorizontalAlignment)2;
		TbxCountReportDevices.Value = new decimal(new int[4] { 20, 0, 0, 0 });
		((Control)labelX68).Anchor = (AnchorStyles)9;
		((Control)labelX68).BackColor = Color.Transparent;
		((BaseItemControl)labelX68).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX68).Location = new Point(610, 29);
		((Control)labelX68).Name = "labelX68";
		((Control)labelX68).Size = new Size(109, 23);
		((Control)labelX68).TabIndex = 34;
		((Control)labelX68).Text = "تعداد آخرین ترددها:";
		((Control)TbxCountLastTaradods).Anchor = (AnchorStyles)9;
		((Control)TbxCountLastTaradods).Enabled = false;
		((Control)TbxCountLastTaradods).Location = new Point(547, 30);
		TbxCountLastTaradods.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)TbxCountLastTaradods).Name = "TbxCountLastTaradods";
		((Control)TbxCountLastTaradods).Size = new Size(54, 21);
		((Control)TbxCountLastTaradods).TabIndex = 35;
		((UpDownBase)TbxCountLastTaradods).TextAlign = (HorizontalAlignment)2;
		TbxCountLastTaradods.Value = new decimal(new int[4] { 20, 0, 0, 0 });
		((Control)CmbCalendarType).Anchor = (AnchorStyles)9;
		((ListControl)CmbCalendarType).FormattingEnabled = true;
		((Control)CmbCalendarType).Location = new Point(821, 4);
		((Control)CmbCalendarType).Name = "CmbCalendarType";
		((Control)CmbCalendarType).Size = new Size(151, 21);
		((Control)CmbCalendarType).TabIndex = 33;
		((Control)CmbCalendarType).Text = "CmbCalendar";
		((Control)labelX59).Anchor = (AnchorStyles)9;
		((Control)labelX59).BackColor = Color.Transparent;
		((BaseItemControl)labelX59).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX59).Location = new Point(978, 3);
		((Control)labelX59).Name = "labelX59";
		((Control)labelX59).Size = new Size(115, 23);
		((Control)labelX59).TabIndex = 32;
		((Control)labelX59).Text = "[زبان محلی نرم افزار]:";
		labelX59.TextAlignment = (StringAlignment)2;
		((Control)groupPanel6).Anchor = (AnchorStyles)15;
		((Control)groupPanel6).BackColor = Color.Transparent;
		((PanelControl)groupPanel6).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel6).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel6).Controls.Add((Control)(object)TbxManualEditTaradod);
		((Control)groupPanel6).Controls.Add((Control)(object)labelX58);
		((Control)groupPanel6).Controls.Add((Control)(object)TbxManualAddTaradod);
		((Control)groupPanel6).Controls.Add((Control)(object)labelX57);
		groupPanel6.DisabledBackColor = Color.Empty;
		((Control)groupPanel6).Location = new Point(12, 190);
		((Control)groupPanel6).Name = "groupPanel6";
		((Control)groupPanel6).Size = new Size(1100, 60);
		((PanelControl)groupPanel6).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel6).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel6).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel6).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel6).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel6).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel6).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel6).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel6).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel6).Style.CornerDiameter = 4;
		((PanelControl)groupPanel6).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel6).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)groupPanel6).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel6).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel6).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel6).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel6).TabIndex = 5;
		((Control)groupPanel6).Text = " [علامت تشخیص تردد در چاپ گزارش تردد کلیه کارکنان]";
		((Control)TbxManualEditTaradod).Anchor = (AnchorStyles)9;
		TbxManualEditTaradod.Border.Class = "TextBoxBorder";
		TbxManualEditTaradod.Border.CornerType = (eCornerType)1;
		((Control)TbxManualEditTaradod).Location = new Point(709, 8);
		((TextBoxBase)TbxManualEditTaradod).MaxLength = 2;
		((Control)TbxManualEditTaradod).Name = "TbxManualEditTaradod";
		((Control)TbxManualEditTaradod).Size = new Size(36, 21);
		((Control)TbxManualEditTaradod).TabIndex = 4;
		((Control)TbxManualEditTaradod).Text = "*";
		((TextBox)TbxManualEditTaradod).TextAlign = (HorizontalAlignment)2;
		((Control)labelX58).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX58).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX58).Location = new Point(745, 8);
		((Control)labelX58).Name = "labelX58";
		((Control)labelX58).Size = new Size(176, 21);
		((Control)labelX58).TabIndex = 3;
		((Control)labelX58).Text = "[تردد ویرایش شده دستی]:";
		((Control)TbxManualAddTaradod).Anchor = (AnchorStyles)9;
		TbxManualAddTaradod.Border.Class = "TextBoxBorder";
		TbxManualAddTaradod.Border.CornerType = (eCornerType)1;
		((Control)TbxManualAddTaradod).Location = new Point(927, 8);
		((TextBoxBase)TbxManualAddTaradod).MaxLength = 2;
		((Control)TbxManualAddTaradod).Name = "TbxManualAddTaradod";
		((Control)TbxManualAddTaradod).Size = new Size(36, 21);
		((Control)TbxManualAddTaradod).TabIndex = 2;
		((Control)TbxManualAddTaradod).Text = "#";
		((TextBox)TbxManualAddTaradod).TextAlign = (HorizontalAlignment)2;
		((Control)labelX57).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX57).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX57).Location = new Point(954, 8);
		((Control)labelX57).Name = "labelX57";
		((Control)labelX57).Size = new Size(137, 21);
		((Control)labelX57).TabIndex = 0;
		((Control)labelX57).Text = "[تردد اضافه شده دستی]:";
		((Control)groupPanel5).Anchor = (AnchorStyles)15;
		((PanelControl)groupPanel5).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel5).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel5).Controls.Add((Control)(object)CBGetPassAfterMaximize);
		((Control)groupPanel5).Controls.Add((Control)(object)CBRunInStart);
		((Control)groupPanel5).Controls.Add((Control)(object)labelX53);
		((Control)groupPanel5).Controls.Add((Control)(object)NumLockTime);
		groupPanel5.DisabledBackColor = Color.Empty;
		((Control)groupPanel5).Location = new Point(12, 11);
		((Control)groupPanel5).Name = "groupPanel5";
		((Control)groupPanel5).Size = new Size(1100, 69);
		((PanelControl)groupPanel5).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel5).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel5).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel5).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel5).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel5).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel5).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel5).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel5).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel5).Style.CornerDiameter = 4;
		((PanelControl)groupPanel5).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel5).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel5).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel5).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel5).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel5).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel5).TabIndex = 29;
		((Control)CBGetPassAfterMaximize).Anchor = (AnchorStyles)9;
		((Control)CBGetPassAfterMaximize).BackColor = Color.Transparent;
		((BaseItemControl)CBGetPassAfterMaximize).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CBGetPassAfterMaximize).Location = new Point(640, 26);
		((Control)CBGetPassAfterMaximize).Name = "CBGetPassAfterMaximize";
		((Control)CBGetPassAfterMaximize).Size = new Size(451, 28);
		((BaseItemControl)CBGetPassAfterMaximize).Style = (eDotNetBarStyle)9;
		((Control)CBGetPassAfterMaximize).TabIndex = 33;
		((Control)CBGetPassAfterMaximize).Text = "[نمایش پنجره دریافت رمز هنگام  باز کردن برنامه]";
		CBGetPassAfterMaximize.CheckedChanged += CBGetPassAfterMaximize_CheckedChanged;
		((Control)CBRunInStart).Anchor = (AnchorStyles)9;
		((Control)CBRunInStart).BackColor = Color.Transparent;
		((BaseItemControl)CBRunInStart).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CBRunInStart).Location = new Point(640, 0);
		((Control)CBRunInStart).Name = "CBRunInStart";
		((Control)CBRunInStart).Size = new Size(451, 28);
		((BaseItemControl)CBRunInStart).Style = (eDotNetBarStyle)9;
		((Control)CBRunInStart).TabIndex = 28;
		((Control)CBRunInStart).Text = "[اجرای اتوماتیک نرم افزار بعد از بالا آمدن ویندوز]";
		((Control)labelX53).Anchor = (AnchorStyles)9;
		((Control)labelX53).BackColor = Color.Transparent;
		((BaseItemControl)labelX53).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX53).Location = new Point(74, 29);
		((Control)labelX53).Name = "labelX53";
		((Control)labelX53).Size = new Size(540, 23);
		((Control)labelX53).TabIndex = 31;
		((Control)labelX53).Text = "[ نمایش پنجره دریافت رمز در صورت سپری شدن مدت زمان تعیین شده(زمان به ثانیه می باشد)]";
		labelX53.TextAlignment = (StringAlignment)2;
		((Control)NumLockTime).Anchor = (AnchorStyles)9;
		((Control)NumLockTime).Enabled = false;
		((Control)NumLockTime).Location = new Point(14, 31);
		NumLockTime.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		NumLockTime.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumLockTime).Name = "NumLockTime";
		((Control)NumLockTime).Size = new Size(54, 21);
		((Control)NumLockTime).TabIndex = 32;
		((UpDownBase)NumLockTime).TextAlign = (HorizontalAlignment)2;
		NumLockTime.Value = new decimal(new int[4] { 120, 0, 0, 0 });
		((Control)groupPanel1).Anchor = (AnchorStyles)15;
		((PanelControl)groupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel1).Controls.Add((Control)(object)TxtFont);
		((Control)groupPanel1).Controls.Add((Control)(object)BtnMore);
		((Control)groupPanel1).Controls.Add((Control)(object)pictureBox3);
		((Control)groupPanel1).Controls.Add((Control)(object)labelX2);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Location = new Point(12, 358);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(1100, 36);
		((PanelControl)groupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel1).Style.CornerDiameter = 4;
		((PanelControl)groupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel1).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel1).TabIndex = 25;
		((Control)TxtFont).Anchor = (AnchorStyles)13;
		TxtFont.Border.Class = "TextBoxBorder";
		TxtFont.Border.CornerType = (eCornerType)1;
		((Control)TxtFont).Location = new Point(69, 5);
		((Control)TxtFont).Name = "TxtFont";
		((TextBoxBase)TxtFont).ReadOnly = true;
		((Control)TxtFont).Size = new Size(923, 21);
		((Control)TxtFont).TabIndex = 1;
		((Control)BtnMore).AccessibleRole = (AccessibleRole)43;
		((Control)BtnMore).Anchor = (AnchorStyles)9;
		BtnMore.ColorTable = (eButtonColor)6;
		((Control)BtnMore).Location = new Point(14, 5);
		((Control)BtnMore).Name = "BtnMore";
		((Control)BtnMore).Size = new Size(49, 21);
		((PopupItemControl)BtnMore).Style = (eDotNetBarStyle)9;
		((Control)BtnMore).TabIndex = 0;
		((Control)BtnMore).TabStop = false;
		((Control)BtnMore).Text = "...";
		((Control)BtnMore).Click += BtnMore_Click_1;
		((Control)pictureBox3).Anchor = (AnchorStyles)9;
		((Control)pictureBox3).BackColor = Color.Transparent;
		pictureBox3.Image = (Image)(object)Resources.font;
		((Control)pictureBox3).Location = new Point(1050, 0);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(39, 31);
		pictureBox3.SizeMode = (PictureBoxSizeMode)4;
		pictureBox3.TabIndex = 27;
		pictureBox3.TabStop = false;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).Location = new Point(998, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(48, 23);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "[نوع قلم] :";
		((Control)LBLPreviwe).Anchor = (AnchorStyles)15;
		((Control)LBLPreviwe).BackColor = Color.Transparent;
		LBLPreviwe.BorderStyle = (BorderStyle)1;
		((Control)LBLPreviwe).ForeColor = Color.Green;
		((Control)LBLPreviwe).Location = new Point(12, 397);
		((Control)LBLPreviwe).Name = "LBLPreviwe";
		((Control)LBLPreviwe).Size = new Size(1100, 79);
		((Control)LBLPreviwe).TabIndex = 26;
		((Control)LBLPreviwe).Text = componentResourceManager.GetString("LBLPreviwe.Text");
		((Control)groupPanel3).Anchor = (AnchorStyles)15;
		((Control)groupPanel3).BackColor = Color.Transparent;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel3).Controls.Add((Control)(object)ChkNotShowUi);
		((Control)groupPanel3).Controls.Add((Control)(object)btnSearchFileName);
		((Control)groupPanel3).Controls.Add((Control)(object)chBxGetFileName);
		((Control)groupPanel3).Controls.Add((Control)(object)txtFileName);
		((Control)groupPanel3).Controls.Add((Control)(object)labelX22);
		groupPanel3.DisabledBackColor = Color.Empty;
		((Control)groupPanel3).Location = new Point(12, 83);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(1100, 103);
		((PanelControl)groupPanel3).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel3).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel3).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel3).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel3).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel3).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel3).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel3).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel3).Style.CornerDiameter = 4;
		((PanelControl)groupPanel3).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 0;
		((Control)groupPanel3).Text = "[انجام عملیاتی دیگر هنگام دریافت تردد]";
		((Control)ChkNotShowUi).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChkNotShowUi).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkNotShowUi).Location = new Point(433, 55);
		((Control)ChkNotShowUi).Name = "ChkNotShowUi";
		((Control)ChkNotShowUi).Size = new Size(656, 23);
		((BaseItemControl)ChkNotShowUi).Style = (eDotNetBarStyle)9;
		((Control)ChkNotShowUi).TabIndex = 4;
		((Control)ChkNotShowUi).Text = "[در اجرای بعدی، برنامه حضور در پس زمینه بصورت مخفی در حال اجرا باشد]";
		ChkNotShowUi.CheckedChanged += ChkNotShowUi_CheckedChanged;
		((Control)btnSearchFileName).AccessibleRole = (AccessibleRole)43;
		((Control)btnSearchFileName).Anchor = (AnchorStyles)9;
		btnSearchFileName.ColorTable = (eButtonColor)6;
		((Control)btnSearchFileName).Location = new Point(14, 32);
		((Control)btnSearchFileName).Name = "btnSearchFileName";
		((Control)btnSearchFileName).Size = new Size(49, 21);
		((PopupItemControl)btnSearchFileName).Style = (eDotNetBarStyle)9;
		((Control)btnSearchFileName).TabIndex = 3;
		((Control)btnSearchFileName).Text = "...";
		btnSearchFileName.Tooltip = "[افزودن سامانه]";
		((Control)btnSearchFileName).Click += btnSearchFileName_Click;
		((Control)chBxGetFileName).Anchor = (AnchorStyles)9;
		((BaseItemControl)chBxGetFileName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxGetFileName).Location = new Point(433, 3);
		((Control)chBxGetFileName).Name = "chBxGetFileName";
		((Control)chBxGetFileName).Size = new Size(658, 23);
		((BaseItemControl)chBxGetFileName).Style = (eDotNetBarStyle)9;
		((Control)chBxGetFileName).TabIndex = 0;
		((Control)chBxGetFileName).Text = "[پس از دریافت تردد، نرم افزار زیر اجرا شود]";
		chBxGetFileName.CheckValueChanged += chBxGetFileName_CheckValueChanged;
		((Control)txtFileName).Anchor = (AnchorStyles)13;
		((Control)txtFileName).Location = new Point(69, 32);
		((Control)txtFileName).Name = "txtFileName";
		((Control)txtFileName).Size = new Size(852, 21);
		((Control)txtFileName).TabIndex = 2;
		((Control)labelX22).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Location = new Point(927, 32);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).RightToLeft = (RightToLeft)1;
		((Control)labelX22).Size = new Size(162, 21);
		((Control)labelX22).TabIndex = 0;
		((Control)labelX22).Text = "[نام فایل اجرایی] :";
		labelX22.TextAlignment = (StringAlignment)2;
		tabItem2.AttachedControl = (Control)(object)tabControlPanel2;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[تنظیمات متفرقه]";
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX65);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilRoundFish);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinRoundFish);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxJarime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceJarime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX62);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilJarime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinJarime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxTatilKari);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX61);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceTatilKari);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinTatilKari);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilTatilKari);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxMission);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX60);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinMission);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceMission);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilMission);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX36);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX13);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxVacation);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxAbsentTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxLeaveTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxBaseSalary);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceAbsentTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceLeaveTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX41);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX40);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxNightTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX39);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceVacation);
		((Control)tabControlPanel4).Controls.Add((Control)(object)lblMoeinOverTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxHolidayTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)lblMoeinShiftTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceNightTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX25);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxShiftTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX26);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceHolidayTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX35);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxTaxOverTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)lblMoeinTaxGov);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceShiftTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX33);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX32);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceOverTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinBaseSalary);
		((Control)tabControlPanel4).Controls.Add((Control)(object)ChBxInsuranceBaseSalary);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilBaseSalary);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilEmployer);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinOverTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilVacation);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinShiftTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinEmployer);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinHolidayTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinVacation);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinNightTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX38);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX28);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilOverTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX27);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilShiftTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilTaxGov);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilHolidayTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinTaxGov);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilNightTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilTaminGov);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinUnEmployMent);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinTaminGov);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilUnEmployMent);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilAbsentTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtTafsilLeaveTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinAbsentTime);
		((Control)tabControlPanel4).Controls.Add((Control)(object)TxtMoeinLeaveTime);
		((PanelEx)tabControlPanel4).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel4).Dock = (DockStyle)5;
		((Control)tabControlPanel4).Location = new Point(0, 26);
		((Control)tabControlPanel4).Name = "tabControlPanel4";
		((Control)tabControlPanel4).Padding = new Padding(1);
		((Control)tabControlPanel4).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel4).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel4).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel4).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel4).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel4).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel4).Style.GradientAngle = 90;
		((Control)tabControlPanel4).TabIndex = 0;
		tabControlPanel4.TabItem = tabItem4;
		((Control)labelX65).Anchor = (AnchorStyles)9;
		((Control)labelX65).BackColor = Color.Transparent;
		((BaseItemControl)labelX65).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX65).Location = new Point(833, 452);
		((Control)labelX65).Name = "labelX65";
		((Control)labelX65).Size = new Size(234, 21);
		((Control)labelX65).TabIndex = 59;
		((Control)labelX65).Text = "[کسر ریال مبلغ قابل پرداخت فیش حقوقی]:";
		labelX65.TextAlignment = (StringAlignment)2;
		((Control)TxtTafsilRoundFish).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilRoundFish).Location = new Point(357, 452);
		((Control)TxtTafsilRoundFish).Name = "TxtTafsilRoundFish";
		((Control)TxtTafsilRoundFish).Size = new Size(213, 21);
		((Control)TxtTafsilRoundFish).TabIndex = 61;
		((Control)TxtTafsilRoundFish).Text = "0";
		TxtTafsilRoundFish.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinRoundFish).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinRoundFish).Location = new Point(608, 452);
		((Control)TxtMoeinRoundFish).Name = "TxtMoeinRoundFish";
		((Control)TxtMoeinRoundFish).Size = new Size(219, 21);
		((Control)TxtMoeinRoundFish).TabIndex = 60;
		((Control)TxtMoeinRoundFish).Text = "0";
		TxtMoeinRoundFish.TextAlign = (HorizontalAlignment)2;
		((Control)ChBxTaxJarime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxJarime).BackgroundStyle.BackColor2SchemePart = (eColorSchemePart)51;
		((BaseItemControl)ChBxTaxJarime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxJarime.Checked = true;
		ChBxTaxJarime.CheckState = (CheckState)1;
		ChBxTaxJarime.CheckValue = "Y";
		((Control)ChBxTaxJarime).Location = new Point(199, 283);
		((Control)ChBxTaxJarime).Name = "ChBxTaxJarime";
		((Control)ChBxTaxJarime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxJarime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxJarime).TabIndex = 39;
		((Control)ChBxInsuranceJarime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceJarime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceJarime.Checked = true;
		ChBxInsuranceJarime.CheckState = (CheckState)1;
		ChBxInsuranceJarime.CheckValue = "Y";
		((Control)ChBxInsuranceJarime).Location = new Point(277, 283);
		((Control)ChBxInsuranceJarime).Name = "ChBxInsuranceJarime";
		((Control)ChBxInsuranceJarime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceJarime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceJarime).TabIndex = 38;
		ChBxInsuranceJarime.TextColor = Color.Transparent;
		((Control)labelX62).Anchor = (AnchorStyles)9;
		((Control)labelX62).BackColor = Color.Transparent;
		((BaseItemControl)labelX62).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX62).Location = new Point(832, 284);
		((Control)labelX62).Name = "labelX62";
		((Control)labelX62).Size = new Size(209, 21);
		((Control)labelX62).TabIndex = 58;
		((Control)labelX62).Text = "[طرف حساب بستانکار جریمه تاخیر] :";
		labelX62.TextAlignment = (StringAlignment)2;
		((Control)TxtTafsilJarime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilJarime).Location = new Point(356, 284);
		((Control)TxtTafsilJarime).Name = "TxtTafsilJarime";
		((Control)TxtTafsilJarime).Size = new Size(213, 21);
		((Control)TxtTafsilJarime).TabIndex = 37;
		((Control)TxtTafsilJarime).Text = "0";
		TxtTafsilJarime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinJarime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinJarime).Location = new Point(608, 284);
		((Control)TxtMoeinJarime).Name = "TxtMoeinJarime";
		((Control)TxtMoeinJarime).Size = new Size(219, 21);
		((Control)TxtMoeinJarime).TabIndex = 36;
		((Control)TxtMoeinJarime).Text = "0";
		TxtMoeinJarime.TextAlign = (HorizontalAlignment)2;
		((Control)ChBxTaxTatilKari).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxTatilKari).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxTatilKari.Checked = true;
		ChBxTaxTatilKari.CheckState = (CheckState)1;
		ChBxTaxTatilKari.CheckValue = "Y";
		((Control)ChBxTaxTatilKari).Location = new Point(200, 171);
		((Control)ChBxTaxTatilKari).Name = "ChBxTaxTatilKari";
		((Control)ChBxTaxTatilKari).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxTatilKari).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxTatilKari).TabIndex = 23;
		((Control)labelX61).Anchor = (AnchorStyles)9;
		((Control)labelX61).BackColor = Color.Transparent;
		((BaseItemControl)labelX61).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX61).Location = new Point(833, 172);
		((Control)labelX61).Name = "labelX61";
		((Control)labelX61).Size = new Size(291, 21);
		((Control)labelX61).TabIndex = 53;
		((Control)labelX61).Text = "[طرف حساب بدهکار هزینه تعطیل کاری] :";
		labelX61.TextAlignment = (StringAlignment)2;
		((Control)ChBxInsuranceTatilKari).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceTatilKari).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceTatilKari.Checked = true;
		ChBxInsuranceTatilKari.CheckState = (CheckState)1;
		ChBxInsuranceTatilKari.CheckValue = "Y";
		((Control)ChBxInsuranceTatilKari).Location = new Point(278, 171);
		((Control)ChBxInsuranceTatilKari).Name = "ChBxInsuranceTatilKari";
		((Control)ChBxInsuranceTatilKari).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceTatilKari).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceTatilKari).TabIndex = 22;
		((Control)TxtMoeinTatilKari).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinTatilKari).Location = new Point(608, 172);
		((Control)TxtMoeinTatilKari).Name = "TxtMoeinTatilKari";
		((Control)TxtMoeinTatilKari).Size = new Size(219, 21);
		((Control)TxtMoeinTatilKari).TabIndex = 20;
		((Control)TxtMoeinTatilKari).Text = "0";
		TxtMoeinTatilKari.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilTatilKari).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilTatilKari).Location = new Point(357, 172);
		((Control)TxtTafsilTatilKari).Name = "TxtTafsilTatilKari";
		((Control)TxtTafsilTatilKari).Size = new Size(213, 21);
		((Control)TxtTafsilTatilKari).TabIndex = 21;
		((Control)TxtTafsilTatilKari).Text = "0";
		TxtTafsilTatilKari.TextAlign = (HorizontalAlignment)2;
		((Control)ChBxTaxMission).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxMission).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxMission.Checked = true;
		ChBxTaxMission.CheckState = (CheckState)1;
		ChBxTaxMission.CheckValue = "Y";
		((Control)ChBxTaxMission).Location = new Point(200, 59);
		((Control)ChBxTaxMission).Name = "ChBxTaxMission";
		((Control)ChBxTaxMission).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxMission).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxMission).TabIndex = 7;
		((Control)labelX60).Anchor = (AnchorStyles)9;
		((Control)labelX60).BackColor = Color.Transparent;
		((BaseItemControl)labelX60).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX60).Location = new Point(833, 60);
		((Control)labelX60).Name = "labelX60";
		((Control)labelX60).Size = new Size(234, 21);
		((Control)labelX60).TabIndex = 48;
		((Control)labelX60).Text = "[طرف حساب بدهکار هزینه حق ماموریت] :";
		labelX60.TextAlignment = (StringAlignment)2;
		((Control)TxtMoeinMission).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinMission).Location = new Point(608, 60);
		((Control)TxtMoeinMission).Name = "TxtMoeinMission";
		((Control)TxtMoeinMission).Size = new Size(219, 21);
		((Control)TxtMoeinMission).TabIndex = 4;
		((Control)TxtMoeinMission).Text = "0";
		TxtMoeinMission.TextAlign = (HorizontalAlignment)2;
		((Control)ChBxInsuranceMission).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceMission).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceMission.Checked = true;
		ChBxInsuranceMission.CheckState = (CheckState)1;
		ChBxInsuranceMission.CheckValue = "Y";
		((Control)ChBxInsuranceMission).Location = new Point(278, 59);
		((Control)ChBxInsuranceMission).Name = "ChBxInsuranceMission";
		((Control)ChBxInsuranceMission).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceMission).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceMission).TabIndex = 6;
		((Control)TxtTafsilMission).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilMission).Location = new Point(357, 60);
		((Control)TxtTafsilMission).Name = "TxtTafsilMission";
		((Control)TxtTafsilMission).Size = new Size(213, 21);
		((Control)TxtTafsilMission).TabIndex = 5;
		((Control)TxtTafsilMission).Text = "0";
		TxtTafsilMission.TextAlign = (HorizontalAlignment)2;
		((Control)labelX36).BackColor = Color.Transparent;
		((BaseItemControl)labelX36).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX36).Location = new Point(186, 2);
		((Control)labelX36).Name = "labelX36";
		((Control)labelX36).Size = new Size(82, 21);
		((Control)labelX36).TabIndex = 0;
		((Control)labelX36).Text = "[مشمول مالیات]";
		labelX36.TextAlignment = (StringAlignment)1;
		((Control)labelX13).BackColor = Color.Transparent;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Location = new Point(274, 2);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(70, 21);
		((Control)labelX13).TabIndex = 0;
		((Control)labelX13).Text = "[مشمول بیمه]";
		labelX13.TextAlignment = (StringAlignment)1;
		((Control)ChBxTaxVacation).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxVacation).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxVacation.Checked = true;
		ChBxTaxVacation.CheckState = (CheckState)1;
		ChBxTaxVacation.CheckValue = "Y";
		((Control)ChBxTaxVacation).Location = new Point(200, 228);
		((Control)ChBxTaxVacation).Name = "ChBxTaxVacation";
		((Control)ChBxTaxVacation).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxVacation).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxVacation).TabIndex = 31;
		((Control)ChBxTaxAbsentTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxAbsentTime).BackgroundStyle.BackColor2SchemePart = (eColorSchemePart)51;
		((BaseItemControl)ChBxTaxAbsentTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxAbsentTime.Checked = true;
		ChBxTaxAbsentTime.CheckState = (CheckState)1;
		ChBxTaxAbsentTime.CheckValue = "Y";
		((Control)ChBxTaxAbsentTime).Location = new Point(199, 312);
		((Control)ChBxTaxAbsentTime).Name = "ChBxTaxAbsentTime";
		((Control)ChBxTaxAbsentTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxAbsentTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxAbsentTime).TabIndex = 43;
		((Control)ChBxTaxLeaveTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxLeaveTime).BackgroundStyle.BackColor2SchemePart = (eColorSchemePart)51;
		((BaseItemControl)ChBxTaxLeaveTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxLeaveTime.Checked = true;
		ChBxTaxLeaveTime.CheckState = (CheckState)1;
		ChBxTaxLeaveTime.CheckValue = "Y";
		((Control)ChBxTaxLeaveTime).Location = new Point(199, 255);
		((Control)ChBxTaxLeaveTime).Name = "ChBxTaxLeaveTime";
		((Control)ChBxTaxLeaveTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxLeaveTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxLeaveTime).TabIndex = 35;
		((Control)ChBxTaxBaseSalary).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxBaseSalary).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxBaseSalary.Checked = true;
		ChBxTaxBaseSalary.CheckState = (CheckState)1;
		ChBxTaxBaseSalary.CheckValue = "Y";
		((Control)ChBxTaxBaseSalary).Location = new Point(200, 32);
		((Control)ChBxTaxBaseSalary).Name = "ChBxTaxBaseSalary";
		((Control)ChBxTaxBaseSalary).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxBaseSalary).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxBaseSalary).TabIndex = 3;
		((Control)ChBxInsuranceAbsentTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceAbsentTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceAbsentTime.Checked = true;
		ChBxInsuranceAbsentTime.CheckState = (CheckState)1;
		ChBxInsuranceAbsentTime.CheckValue = "Y";
		((Control)ChBxInsuranceAbsentTime).Location = new Point(277, 312);
		((Control)ChBxInsuranceAbsentTime).Name = "ChBxInsuranceAbsentTime";
		((Control)ChBxInsuranceAbsentTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceAbsentTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceAbsentTime).TabIndex = 42;
		ChBxInsuranceAbsentTime.TextColor = Color.Transparent;
		((Control)ChBxInsuranceLeaveTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceLeaveTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceLeaveTime.Checked = true;
		ChBxInsuranceLeaveTime.CheckState = (CheckState)1;
		ChBxInsuranceLeaveTime.CheckValue = "Y";
		((Control)ChBxInsuranceLeaveTime).Location = new Point(277, 255);
		((Control)ChBxInsuranceLeaveTime).Name = "ChBxInsuranceLeaveTime";
		((Control)ChBxInsuranceLeaveTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceLeaveTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceLeaveTime).TabIndex = 34;
		ChBxInsuranceLeaveTime.TextColor = Color.Transparent;
		((Control)labelX41).Anchor = (AnchorStyles)9;
		((Control)labelX41).BackColor = Color.Transparent;
		((BaseItemControl)labelX41).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX41).Location = new Point(833, 32);
		((Control)labelX41).Name = "labelX41";
		((Control)labelX41).Size = new Size(209, 21);
		((Control)labelX41).TabIndex = 0;
		((Control)labelX41).Text = "[طرف حساب بدهکار هزینه حقوق پایه] :";
		labelX41.TextAlignment = (StringAlignment)2;
		((Control)labelX40).Anchor = (AnchorStyles)9;
		((Control)labelX40).BackColor = Color.Transparent;
		((BaseItemControl)labelX40).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX40).Location = new Point(723, 2);
		((Control)labelX40).Name = "labelX40";
		((Control)labelX40).Size = new Size(97, 23);
		((Control)labelX40).TabIndex = 0;
		((Control)labelX40).Text = "[کد حساب معین]";
		labelX40.TextAlignment = (StringAlignment)1;
		((Control)ChBxTaxNightTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxNightTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxNightTime.Checked = true;
		ChBxTaxNightTime.CheckState = (CheckState)1;
		ChBxTaxNightTime.CheckValue = "Y";
		((Control)ChBxTaxNightTime).Location = new Point(200, 200);
		((Control)ChBxTaxNightTime).Name = "ChBxTaxNightTime";
		((Control)ChBxTaxNightTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxNightTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxNightTime).TabIndex = 27;
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(466, 3);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(109, 21);
		((Control)labelX39).TabIndex = 0;
		((Control)labelX39).Text = "[کد حساب تفصیل]";
		labelX39.TextAlignment = (StringAlignment)1;
		((Control)ChBxInsuranceVacation).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceVacation).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceVacation.Checked = true;
		ChBxInsuranceVacation.CheckState = (CheckState)1;
		ChBxInsuranceVacation.CheckValue = "Y";
		((Control)ChBxInsuranceVacation).Location = new Point(278, 228);
		((Control)ChBxInsuranceVacation).Name = "ChBxInsuranceVacation";
		((Control)ChBxInsuranceVacation).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceVacation).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceVacation).TabIndex = 30;
		((Control)lblMoeinOverTime).Anchor = (AnchorStyles)9;
		((Control)lblMoeinOverTime).BackColor = Color.Transparent;
		((BaseItemControl)lblMoeinOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMoeinOverTime).Location = new Point(833, 88);
		((Control)lblMoeinOverTime).Name = "lblMoeinOverTime";
		((Control)lblMoeinOverTime).Size = new Size(224, 21);
		((Control)lblMoeinOverTime).TabIndex = 0;
		((Control)lblMoeinOverTime).Text = "[طرف حساب بدهکار هزینه اضافه کاری] :";
		lblMoeinOverTime.TextAlignment = (StringAlignment)2;
		((Control)ChBxTaxHolidayTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxHolidayTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxHolidayTime.Checked = true;
		ChBxTaxHolidayTime.CheckState = (CheckState)1;
		ChBxTaxHolidayTime.CheckValue = "Y";
		((Control)ChBxTaxHolidayTime).Location = new Point(200, 143);
		((Control)ChBxTaxHolidayTime).Name = "ChBxTaxHolidayTime";
		((Control)ChBxTaxHolidayTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxHolidayTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxHolidayTime).TabIndex = 19;
		((Control)lblMoeinShiftTime).Anchor = (AnchorStyles)9;
		((Control)lblMoeinShiftTime).BackColor = Color.Transparent;
		((BaseItemControl)lblMoeinShiftTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMoeinShiftTime).Location = new Point(833, 116);
		((Control)lblMoeinShiftTime).Name = "lblMoeinShiftTime";
		((Control)lblMoeinShiftTime).Size = new Size(209, 21);
		((Control)lblMoeinShiftTime).TabIndex = 0;
		((Control)lblMoeinShiftTime).Text = "[طرف حساب بدهکار هزینه نوبت کاری] :";
		lblMoeinShiftTime.TextAlignment = (StringAlignment)2;
		((Control)ChBxInsuranceNightTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceNightTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceNightTime.Checked = true;
		ChBxInsuranceNightTime.CheckState = (CheckState)1;
		ChBxInsuranceNightTime.CheckValue = "Y";
		((Control)ChBxInsuranceNightTime).Location = new Point(278, 200);
		((Control)ChBxInsuranceNightTime).Name = "ChBxInsuranceNightTime";
		((Control)ChBxInsuranceNightTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceNightTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceNightTime).TabIndex = 26;
		((Control)labelX25).Anchor = (AnchorStyles)9;
		((Control)labelX25).BackColor = Color.Transparent;
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Location = new Point(833, 144);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).Size = new Size(291, 21);
		((Control)labelX25).TabIndex = 0;
		((Control)labelX25).Text = "[طرف حساب بدهکار هزینه جمعه کاری] :";
		labelX25.TextAlignment = (StringAlignment)2;
		((Control)ChBxTaxShiftTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxShiftTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxShiftTime.Checked = true;
		ChBxTaxShiftTime.CheckState = (CheckState)1;
		ChBxTaxShiftTime.CheckValue = "Y";
		((Control)ChBxTaxShiftTime).Location = new Point(200, 115);
		((Control)ChBxTaxShiftTime).Name = "ChBxTaxShiftTime";
		((Control)ChBxTaxShiftTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxShiftTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxShiftTime).TabIndex = 15;
		((Control)labelX26).Anchor = (AnchorStyles)9;
		((Control)labelX26).BackColor = Color.Transparent;
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Location = new Point(833, 200);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).Size = new Size(209, 21);
		((Control)labelX26).TabIndex = 0;
		((Control)labelX26).Text = "[طرف حساب بدهکار هزینه شب کاری] :";
		labelX26.TextAlignment = (StringAlignment)2;
		((Control)ChBxInsuranceHolidayTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceHolidayTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceHolidayTime.Checked = true;
		ChBxInsuranceHolidayTime.CheckState = (CheckState)1;
		ChBxInsuranceHolidayTime.CheckValue = "Y";
		((Control)ChBxInsuranceHolidayTime).Location = new Point(278, 143);
		((Control)ChBxInsuranceHolidayTime).Name = "ChBxInsuranceHolidayTime";
		((Control)ChBxInsuranceHolidayTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceHolidayTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceHolidayTime).TabIndex = 18;
		((Control)labelX35).Anchor = (AnchorStyles)9;
		((Control)labelX35).BackColor = Color.Transparent;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Location = new Point(833, 368);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).Size = new Size(245, 21);
		((Control)labelX35).TabIndex = 0;
		((Control)labelX35).Text = "[طرف حساب بدهکار هزینه بیمه بیکاری (3%)]:";
		labelX35.TextAlignment = (StringAlignment)2;
		((Control)ChBxTaxOverTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxTaxOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxTaxOverTime.Checked = true;
		ChBxTaxOverTime.CheckState = (CheckState)1;
		ChBxTaxOverTime.CheckValue = "Y";
		((Control)ChBxTaxOverTime).Location = new Point(200, 88);
		((Control)ChBxTaxOverTime).Name = "ChBxTaxOverTime";
		((Control)ChBxTaxOverTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxTaxOverTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxTaxOverTime).TabIndex = 11;
		((Control)lblMoeinTaxGov).Anchor = (AnchorStyles)9;
		((Control)lblMoeinTaxGov).BackColor = Color.Transparent;
		((BaseItemControl)lblMoeinTaxGov).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblMoeinTaxGov).Location = new Point(833, 424);
		((Control)lblMoeinTaxGov).Name = "lblMoeinTaxGov";
		((Control)lblMoeinTaxGov).Size = new Size(191, 21);
		((Control)lblMoeinTaxGov).TabIndex = 0;
		((Control)lblMoeinTaxGov).Text = "[طرف حساب بستانکار اداره مالیات] :";
		lblMoeinTaxGov.TextAlignment = (StringAlignment)2;
		((Control)ChBxInsuranceShiftTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceShiftTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceShiftTime.Checked = true;
		ChBxInsuranceShiftTime.CheckState = (CheckState)1;
		ChBxInsuranceShiftTime.CheckValue = "Y";
		((Control)ChBxInsuranceShiftTime).Location = new Point(278, 115);
		((Control)ChBxInsuranceShiftTime).Name = "ChBxInsuranceShiftTime";
		((Control)ChBxInsuranceShiftTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceShiftTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceShiftTime).TabIndex = 14;
		((Control)labelX33).Anchor = (AnchorStyles)9;
		((Control)labelX33).BackColor = Color.Transparent;
		((BaseItemControl)labelX33).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX33).Location = new Point(833, 396);
		((Control)labelX33).Name = "labelX33";
		((Control)labelX33).Size = new Size(245, 21);
		((Control)labelX33).TabIndex = 0;
		((Control)labelX33).Text = "[طرف حساب بستانکار سازمان تامین اجتماعی] :";
		labelX33.TextAlignment = (StringAlignment)2;
		((Control)labelX32).Anchor = (AnchorStyles)9;
		((Control)labelX32).BackColor = Color.Transparent;
		((BaseItemControl)labelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX32).Location = new Point(833, 340);
		((Control)labelX32).Name = "labelX32";
		((Control)labelX32).Size = new Size(279, 21);
		((Control)labelX32).TabIndex = 0;
		((Control)labelX32).Text = "[طرف حساب بدهکار هزینه بیمه سهم کارفرما (20%)]:";
		labelX32.TextAlignment = (StringAlignment)2;
		((Control)ChBxInsuranceOverTime).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceOverTime.Checked = true;
		ChBxInsuranceOverTime.CheckState = (CheckState)1;
		ChBxInsuranceOverTime.CheckValue = "Y";
		((Control)ChBxInsuranceOverTime).Location = new Point(278, 88);
		((Control)ChBxInsuranceOverTime).Name = "ChBxInsuranceOverTime";
		((Control)ChBxInsuranceOverTime).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceOverTime).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceOverTime).TabIndex = 10;
		((Control)TxtMoeinBaseSalary).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinBaseSalary).Location = new Point(608, 32);
		((Control)TxtMoeinBaseSalary).Name = "TxtMoeinBaseSalary";
		((Control)TxtMoeinBaseSalary).Size = new Size(219, 21);
		((Control)TxtMoeinBaseSalary).TabIndex = 0;
		((Control)TxtMoeinBaseSalary).Text = "0";
		TxtMoeinBaseSalary.TextAlign = (HorizontalAlignment)2;
		((Control)ChBxInsuranceBaseSalary).BackColor = Color.Transparent;
		((BaseItemControl)ChBxInsuranceBaseSalary).BackgroundStyle.CornerType = (eCornerType)1;
		ChBxInsuranceBaseSalary.Checked = true;
		ChBxInsuranceBaseSalary.CheckState = (CheckState)1;
		ChBxInsuranceBaseSalary.CheckValue = "Y";
		((Control)ChBxInsuranceBaseSalary).Location = new Point(278, 32);
		((Control)ChBxInsuranceBaseSalary).Name = "ChBxInsuranceBaseSalary";
		((Control)ChBxInsuranceBaseSalary).Size = new Size(28, 21);
		((BaseItemControl)ChBxInsuranceBaseSalary).Style = (eDotNetBarStyle)9;
		((Control)ChBxInsuranceBaseSalary).TabIndex = 2;
		((Control)TxtTafsilBaseSalary).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilBaseSalary).Location = new Point(357, 32);
		((Control)TxtTafsilBaseSalary).Name = "TxtTafsilBaseSalary";
		((Control)TxtTafsilBaseSalary).Size = new Size(213, 21);
		((Control)TxtTafsilBaseSalary).TabIndex = 1;
		((Control)TxtTafsilBaseSalary).Text = "0";
		TxtTafsilBaseSalary.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilEmployer).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilEmployer).Location = new Point(357, 340);
		((Control)TxtTafsilEmployer).Name = "TxtTafsilEmployer";
		((Control)TxtTafsilEmployer).Size = new Size(213, 21);
		((Control)TxtTafsilEmployer).TabIndex = 45;
		((Control)TxtTafsilEmployer).Text = "0";
		TxtTafsilEmployer.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinOverTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinOverTime).Location = new Point(608, 88);
		((Control)TxtMoeinOverTime).Name = "TxtMoeinOverTime";
		((Control)TxtMoeinOverTime).Size = new Size(219, 21);
		((Control)TxtMoeinOverTime).TabIndex = 8;
		((Control)TxtMoeinOverTime).Text = "0";
		TxtMoeinOverTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilVacation).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilVacation).Location = new Point(357, 228);
		((Control)TxtTafsilVacation).Name = "TxtTafsilVacation";
		((Control)TxtTafsilVacation).Size = new Size(213, 21);
		((Control)TxtTafsilVacation).TabIndex = 29;
		((Control)TxtTafsilVacation).Text = "0";
		TxtTafsilVacation.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinShiftTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinShiftTime).Location = new Point(608, 116);
		((Control)TxtMoeinShiftTime).Name = "TxtMoeinShiftTime";
		((Control)TxtMoeinShiftTime).Size = new Size(219, 21);
		((Control)TxtMoeinShiftTime).TabIndex = 12;
		((Control)TxtMoeinShiftTime).Text = "0";
		TxtMoeinShiftTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinEmployer).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinEmployer).Location = new Point(608, 340);
		((Control)TxtMoeinEmployer).Name = "TxtMoeinEmployer";
		((Control)TxtMoeinEmployer).Size = new Size(219, 21);
		((Control)TxtMoeinEmployer).TabIndex = 44;
		((Control)TxtMoeinEmployer).Text = "0";
		TxtMoeinEmployer.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinHolidayTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinHolidayTime).Location = new Point(608, 144);
		((Control)TxtMoeinHolidayTime).Name = "TxtMoeinHolidayTime";
		((Control)TxtMoeinHolidayTime).Size = new Size(219, 21);
		((Control)TxtMoeinHolidayTime).TabIndex = 16;
		((Control)TxtMoeinHolidayTime).Text = "0";
		TxtMoeinHolidayTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinVacation).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinVacation).Location = new Point(608, 228);
		((Control)TxtMoeinVacation).Name = "TxtMoeinVacation";
		((Control)TxtMoeinVacation).Size = new Size(219, 21);
		((Control)TxtMoeinVacation).TabIndex = 28;
		((Control)TxtMoeinVacation).Text = "0";
		TxtMoeinVacation.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinNightTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinNightTime).Location = new Point(608, 200);
		((Control)TxtMoeinNightTime).Name = "TxtMoeinNightTime";
		((Control)TxtMoeinNightTime).Size = new Size(219, 21);
		((Control)TxtMoeinNightTime).TabIndex = 24;
		((Control)TxtMoeinNightTime).Text = "0";
		TxtMoeinNightTime.TextAlign = (HorizontalAlignment)2;
		((Control)labelX38).Anchor = (AnchorStyles)9;
		((Control)labelX38).BackColor = Color.Transparent;
		((BaseItemControl)labelX38).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX38).Location = new Point(832, 312);
		((Control)labelX38).Name = "labelX38";
		((Control)labelX38).Size = new Size(209, 21);
		((Control)labelX38).TabIndex = 0;
		((Control)labelX38).Text = "[طرف حساب بستانکار غیبت] :";
		labelX38.TextAlignment = (StringAlignment)2;
		((Control)labelX28).Anchor = (AnchorStyles)9;
		((Control)labelX28).BackColor = Color.Transparent;
		((BaseItemControl)labelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX28).Location = new Point(832, 256);
		((Control)labelX28).Name = "labelX28";
		((Control)labelX28).Size = new Size(209, 21);
		((Control)labelX28).TabIndex = 0;
		((Control)labelX28).Text = "[طرف حساب بستانکار کسر کار] :";
		labelX28.TextAlignment = (StringAlignment)2;
		((Control)TxtTafsilOverTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilOverTime).Location = new Point(357, 88);
		((Control)TxtTafsilOverTime).Name = "TxtTafsilOverTime";
		((Control)TxtTafsilOverTime).Size = new Size(213, 21);
		((Control)TxtTafsilOverTime).TabIndex = 9;
		((Control)TxtTafsilOverTime).Text = "0";
		TxtTafsilOverTime.TextAlign = (HorizontalAlignment)2;
		((Control)labelX27).Anchor = (AnchorStyles)9;
		((Control)labelX27).BackColor = Color.Transparent;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Location = new Point(833, 228);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).Size = new Size(191, 21);
		((Control)labelX27).TabIndex = 0;
		((Control)labelX27).Text = "[طرف حساب بدهکار هزینه مرخصی] :";
		labelX27.TextAlignment = (StringAlignment)2;
		((Control)TxtTafsilShiftTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilShiftTime).Location = new Point(357, 116);
		((Control)TxtTafsilShiftTime).Name = "TxtTafsilShiftTime";
		((Control)TxtTafsilShiftTime).Size = new Size(213, 21);
		((Control)TxtTafsilShiftTime).TabIndex = 13;
		((Control)TxtTafsilShiftTime).Text = "0";
		TxtTafsilShiftTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilTaxGov).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilTaxGov).Location = new Point(357, 424);
		((Control)TxtTafsilTaxGov).Name = "TxtTafsilTaxGov";
		((Control)TxtTafsilTaxGov).Size = new Size(213, 21);
		((Control)TxtTafsilTaxGov).TabIndex = 51;
		((Control)TxtTafsilTaxGov).Text = "0";
		TxtTafsilTaxGov.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilHolidayTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilHolidayTime).Location = new Point(357, 144);
		((Control)TxtTafsilHolidayTime).Name = "TxtTafsilHolidayTime";
		((Control)TxtTafsilHolidayTime).Size = new Size(213, 21);
		((Control)TxtTafsilHolidayTime).TabIndex = 17;
		((Control)TxtTafsilHolidayTime).Text = "0";
		TxtTafsilHolidayTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinTaxGov).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinTaxGov).Location = new Point(608, 424);
		((Control)TxtMoeinTaxGov).Name = "TxtMoeinTaxGov";
		((Control)TxtMoeinTaxGov).Size = new Size(219, 21);
		((Control)TxtMoeinTaxGov).TabIndex = 50;
		((Control)TxtMoeinTaxGov).Text = "0";
		TxtMoeinTaxGov.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilNightTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilNightTime).Location = new Point(357, 200);
		((Control)TxtTafsilNightTime).Name = "TxtTafsilNightTime";
		((Control)TxtTafsilNightTime).Size = new Size(213, 21);
		((Control)TxtTafsilNightTime).TabIndex = 25;
		((Control)TxtTafsilNightTime).Text = "0";
		TxtTafsilNightTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilTaminGov).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilTaminGov).Location = new Point(357, 396);
		((Control)TxtTafsilTaminGov).Name = "TxtTafsilTaminGov";
		((Control)TxtTafsilTaminGov).Size = new Size(213, 21);
		((Control)TxtTafsilTaminGov).TabIndex = 49;
		((Control)TxtTafsilTaminGov).Text = "0";
		TxtTafsilTaminGov.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinUnEmployMent).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinUnEmployMent).Location = new Point(608, 368);
		((Control)TxtMoeinUnEmployMent).Name = "TxtMoeinUnEmployMent";
		((Control)TxtMoeinUnEmployMent).Size = new Size(219, 21);
		((Control)TxtMoeinUnEmployMent).TabIndex = 46;
		((Control)TxtMoeinUnEmployMent).Text = "0";
		TxtMoeinUnEmployMent.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinTaminGov).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinTaminGov).Location = new Point(608, 396);
		((Control)TxtMoeinTaminGov).Name = "TxtMoeinTaminGov";
		((Control)TxtMoeinTaminGov).Size = new Size(219, 21);
		((Control)TxtMoeinTaminGov).TabIndex = 48;
		((Control)TxtMoeinTaminGov).Text = "0";
		TxtMoeinTaminGov.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilUnEmployMent).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilUnEmployMent).Location = new Point(357, 368);
		((Control)TxtTafsilUnEmployMent).Name = "TxtTafsilUnEmployMent";
		((Control)TxtTafsilUnEmployMent).Size = new Size(213, 21);
		((Control)TxtTafsilUnEmployMent).TabIndex = 47;
		((Control)TxtTafsilUnEmployMent).Text = "0";
		TxtTafsilUnEmployMent.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilAbsentTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilAbsentTime).Location = new Point(356, 312);
		((Control)TxtTafsilAbsentTime).Name = "TxtTafsilAbsentTime";
		((Control)TxtTafsilAbsentTime).Size = new Size(213, 21);
		((Control)TxtTafsilAbsentTime).TabIndex = 41;
		((Control)TxtTafsilAbsentTime).Text = "0";
		TxtTafsilAbsentTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtTafsilLeaveTime).Anchor = (AnchorStyles)9;
		((Control)TxtTafsilLeaveTime).Location = new Point(356, 256);
		((Control)TxtTafsilLeaveTime).Name = "TxtTafsilLeaveTime";
		((Control)TxtTafsilLeaveTime).Size = new Size(213, 21);
		((Control)TxtTafsilLeaveTime).TabIndex = 33;
		((Control)TxtTafsilLeaveTime).Text = "0";
		TxtTafsilLeaveTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinAbsentTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinAbsentTime).Location = new Point(608, 312);
		((Control)TxtMoeinAbsentTime).Name = "TxtMoeinAbsentTime";
		((Control)TxtMoeinAbsentTime).Size = new Size(219, 21);
		((Control)TxtMoeinAbsentTime).TabIndex = 40;
		((Control)TxtMoeinAbsentTime).Text = "0";
		TxtMoeinAbsentTime.TextAlign = (HorizontalAlignment)2;
		((Control)TxtMoeinLeaveTime).Anchor = (AnchorStyles)9;
		((Control)TxtMoeinLeaveTime).Location = new Point(608, 256);
		((Control)TxtMoeinLeaveTime).Name = "TxtMoeinLeaveTime";
		((Control)TxtMoeinLeaveTime).Size = new Size(219, 21);
		((Control)TxtMoeinLeaveTime).TabIndex = 32;
		((Control)TxtMoeinLeaveTime).Text = "0";
		TxtMoeinLeaveTime.TextAlign = (HorizontalAlignment)2;
		tabItem4.AttachedControl = (Control)(object)tabControlPanel4;
		tabItem4.Name = "tabItem4";
		tabItem4.Text = "[سند اتومات حقوق]";
		((Control)tabControlPanel3).Controls.Add((Control)(object)TxtHaghTahol);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX76);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX71);
		((Control)tabControlPanel3).Controls.Add((Control)(object)ZaribMinesBimeOfTax);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX72);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX63);
		((Control)tabControlPanel3).Controls.Add((Control)(object)TxtRoundBelow);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX64);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX55);
		((Control)tabControlPanel3).Controls.Add((Control)(object)ZaribTatil);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX56);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX45);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX12);
		((Control)tabControlPanel3).Controls.Add((Control)(object)ZaribFriday);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX47);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX46);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX11);
		((Control)tabControlPanel3).Controls.Add((Control)(object)ZaribEzafeDay);
		((Control)tabControlPanel3).Controls.Add((Control)(object)txtHaghSpouse);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX43);
		((Control)tabControlPanel3).Controls.Add((Control)(object)MINEzafeDay);
		((Control)tabControlPanel3).Controls.Add((Control)(object)txtHaghMaskan);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX10);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX44);
		((Control)tabControlPanel3).Controls.Add((Control)(object)btnGetLabor);
		((Control)tabControlPanel3).Controls.Add((Control)(object)txtBon);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX42);
		((Control)tabControlPanel3).Controls.Add((Control)(object)txtDailyWage);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX37);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CHAutoEzafe);
		((Control)tabControlPanel3).Controls.Add((Control)(object)groupPanel4);
		((Control)tabControlPanel3).Controls.Add((Control)(object)txtMaxTedadOlaad);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX5);
		((PanelEx)tabControlPanel3).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel3).Dock = (DockStyle)5;
		((Control)tabControlPanel3).Location = new Point(0, 26);
		((Control)tabControlPanel3).Name = "tabControlPanel3";
		((Control)tabControlPanel3).Padding = new Padding(1);
		((Control)tabControlPanel3).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel3).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel3).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel3).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel3).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel3).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel3).Style.GradientAngle = 90;
		((Control)tabControlPanel3).TabIndex = 0;
		tabControlPanel3.TabItem = tabItem3;
		TxtHaghTahol.AllowEmpty = true;
		((Control)TxtHaghTahol).Anchor = (AnchorStyles)9;
		TxtHaghTahol.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtHaghTahol.BackColorTextBoxWhenLostFocus = Color.White;
		TxtHaghTahol.BackcolorTooltip = Color.Red;
		TxtHaghTahol.CurrencyFormat = "";
		((Control)TxtHaghTahol).ForeColor = Color.Green;
		TxtHaghTahol.ForeColorTooltip = Color.Black;
		TxtHaghTahol.GetColoresFromSettingDLL = true;
		TxtHaghTahol.HandleKeydown = true;
		((Control)TxtHaghTahol).Location = new Point(743, 233);
		((TextBoxBase)TxtHaghTahol).MaxLength = 19;
		((Control)TxtHaghTahol).Name = "TxtHaghTahol";
		TxtHaghTahol.SendShiftTabAfterDown = true;
		TxtHaghTahol.SendShiftTabAfterEnter = true;
		TxtHaghTahol.SendTabAfterEnter = true;
		TxtHaghTahol.SendTabAfterUP = true;
		TxtHaghTahol.ShowHorofi = (ENShowHorofi)3;
		TxtHaghTahol.ShowToolTipText = true;
		((Control)TxtHaghTahol).Size = new Size(207, 21);
		((Control)TxtHaghTahol).TabIndex = 23;
		((Control)TxtHaghTahol).Text = "0";
		TxtHaghTahol.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtHaghTahol.ToolTipPostFix = "";
		TxtHaghTahol.TootTipDelay = 1000;
		TxtHaghTahol.Value = new decimal(new int[4]);
		((Control)labelX76).Anchor = (AnchorStyles)9;
		((Control)labelX76).BackColor = Color.Transparent;
		((BaseItemControl)labelX76).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX76).Location = new Point(957, 233);
		((Control)labelX76).Name = "labelX76";
		((Control)labelX76).RightToLeft = (RightToLeft)1;
		((Control)labelX76).Size = new Size(155, 21);
		((Control)labelX76).TabIndex = 22;
		((Control)labelX76).Text = "کمک هزینه عائله مندی:";
		labelX76.TextAlignment = (StringAlignment)2;
		((Control)labelX71).Anchor = (AnchorStyles)9;
		((Control)labelX71).BackColor = Color.Transparent;
		((BaseItemControl)labelX71).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX71).Location = new Point(749, 80);
		((Control)labelX71).Name = "labelX71";
		((Control)labelX71).Size = new Size(53, 23);
		((Control)labelX71).TabIndex = 19;
		((Control)labelX71).Text = "[درصد]";
		((Control)ZaribMinesBimeOfTax).Anchor = (AnchorStyles)9;
		((Control)ZaribMinesBimeOfTax).Location = new Point(808, 82);
		((Control)ZaribMinesBimeOfTax).Name = "ZaribMinesBimeOfTax";
		((Control)ZaribMinesBimeOfTax).Size = new Size(54, 21);
		((Control)ZaribMinesBimeOfTax).TabIndex = 21;
		((Control)ZaribMinesBimeOfTax).Text = "100";
		ZaribMinesBimeOfTax.TextAlign = (HorizontalAlignment)2;
		((Control)labelX72).Anchor = (AnchorStyles)9;
		((Control)labelX72).BackColor = Color.Transparent;
		((BaseItemControl)labelX72).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX72).Location = new Point(863, 80);
		((Control)labelX72).Name = "labelX72";
		((Control)labelX72).Size = new Size(251, 23);
		((Control)labelX72).TabIndex = 20;
		((Control)labelX72).Text = "کسر مبلغ بیمه سهم کارمند از مبلغ مالیات فیش:";
		((Control)labelX63).Anchor = (AnchorStyles)9;
		((Control)labelX63).BackColor = Color.Transparent;
		((BaseItemControl)labelX63).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX63).Location = new Point(823, 122);
		((Control)labelX63).Name = "labelX63";
		((Control)labelX63).Size = new Size(34, 23);
		((Control)labelX63).TabIndex = 14;
		((Control)labelX63).Text = "[ریال]";
		((Control)TxtRoundBelow).Anchor = (AnchorStyles)9;
		((Control)TxtRoundBelow).Location = new Point(859, 124);
		((Control)TxtRoundBelow).Name = "TxtRoundBelow";
		((Control)TxtRoundBelow).Size = new Size(57, 21);
		((Control)TxtRoundBelow).TabIndex = 16;
		((Control)TxtRoundBelow).Text = "100";
		TxtRoundBelow.TextAlign = (HorizontalAlignment)2;
		((Control)labelX64).Anchor = (AnchorStyles)9;
		((Control)labelX64).BackColor = Color.Transparent;
		((BaseItemControl)labelX64).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX64).Location = new Point(922, 122);
		((Control)labelX64).Name = "labelX64";
		((Control)labelX64).Size = new Size(189, 23);
		((Control)labelX64).TabIndex = 15;
		((Control)labelX64).Text = "[گرد کردن مبلغ فیش حقوقی تا ]";
		((Control)labelX55).Anchor = (AnchorStyles)9;
		((Control)labelX55).BackColor = Color.Transparent;
		((BaseItemControl)labelX55).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX55).Location = new Point(369, 54);
		((Control)labelX55).Name = "labelX55";
		((Control)labelX55).Size = new Size(53, 23);
		((Control)labelX55).TabIndex = 0;
		((Control)labelX55).Text = "[درصد]";
		((Control)ZaribTatil).Anchor = (AnchorStyles)9;
		((Control)ZaribTatil).Location = new Point(428, 56);
		((Control)ZaribTatil).Name = "ZaribTatil";
		((Control)ZaribTatil).Size = new Size(54, 21);
		((Control)ZaribTatil).TabIndex = 5;
		((Control)ZaribTatil).Text = "40";
		ZaribTatil.TextAlign = (HorizontalAlignment)2;
		((Control)labelX56).Anchor = (AnchorStyles)9;
		((Control)labelX56).BackColor = Color.Transparent;
		((BaseItemControl)labelX56).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX56).Location = new Point(483, 54);
		((Control)labelX56).Name = "labelX56";
		((Control)labelX56).Size = new Size(251, 23);
		((Control)labelX56).TabIndex = 0;
		((Control)labelX56).Text = "[ضریب افزایش دستمزد هر ساعت تعطیل کاری]:";
		((Control)labelX45).Anchor = (AnchorStyles)9;
		((Control)labelX45).BackColor = Color.Transparent;
		((BaseItemControl)labelX45).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX45).Location = new Point(749, 54);
		((Control)labelX45).Name = "labelX45";
		((Control)labelX45).Size = new Size(53, 23);
		((Control)labelX45).TabIndex = 0;
		((Control)labelX45).Text = "[درصد]";
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(749, 31);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(53, 23);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[درصد]";
		((Control)ZaribFriday).Anchor = (AnchorStyles)9;
		((Control)ZaribFriday).Location = new Point(808, 56);
		((Control)ZaribFriday).Name = "ZaribFriday";
		((Control)ZaribFriday).Size = new Size(54, 21);
		((Control)ZaribFriday).TabIndex = 4;
		((Control)ZaribFriday).Text = "80";
		ZaribFriday.TextAlign = (HorizontalAlignment)2;
		((Control)labelX47).Anchor = (AnchorStyles)9;
		((Control)labelX47).BackColor = Color.Transparent;
		((BaseItemControl)labelX47).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX47).Location = new Point(863, 54);
		((Control)labelX47).Name = "labelX47";
		((Control)labelX47).Size = new Size(251, 23);
		((Control)labelX47).TabIndex = 0;
		((Control)labelX47).Text = "[ضریب افزایش دستمزد هر ساعت جمعه کاری]:";
		((Control)labelX46).Anchor = (AnchorStyles)9;
		((Control)labelX46).BackColor = Color.Transparent;
		((BaseItemControl)labelX46).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX46).Location = new Point(863, 30);
		((Control)labelX46).Name = "labelX46";
		((Control)labelX46).Size = new Size(251, 23);
		((Control)labelX46).TabIndex = 0;
		((Control)labelX46).Text = "[ضریب افزایش دستمزد هر ساعت اضافه کاری]:";
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((Control)labelX11).BackColor = Color.Transparent;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(841, 4);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(106, 23);
		((Control)labelX11).TabIndex = 0;
		((Control)labelX11).Text = "[دقیقه]";
		((Control)labelX11).Visible = false;
		((Control)ZaribEzafeDay).Anchor = (AnchorStyles)9;
		((Control)ZaribEzafeDay).Location = new Point(808, 33);
		((Control)ZaribEzafeDay).Name = "ZaribEzafeDay";
		((Control)ZaribEzafeDay).Size = new Size(54, 21);
		((Control)ZaribEzafeDay).TabIndex = 2;
		((Control)ZaribEzafeDay).Text = "40";
		ZaribEzafeDay.TextAlign = (HorizontalAlignment)2;
		txtHaghSpouse.AllowEmpty = true;
		txtHaghSpouse.BackColorTextBoxWhenFocus = Color.Yellow;
		txtHaghSpouse.BackColorTextBoxWhenLostFocus = Color.White;
		txtHaghSpouse.BackcolorTooltip = Color.Red;
		txtHaghSpouse.CurrencyFormat = "";
		((Control)txtHaghSpouse).ForeColor = Color.Green;
		txtHaghSpouse.ForeColorTooltip = Color.Black;
		txtHaghSpouse.GetColoresFromSettingDLL = true;
		txtHaghSpouse.HandleKeydown = true;
		((Control)txtHaghSpouse).Location = new Point(314, 206);
		((TextBoxBase)txtHaghSpouse).MaxLength = 19;
		((Control)txtHaghSpouse).Name = "txtHaghSpouse";
		txtHaghSpouse.SendShiftTabAfterDown = true;
		txtHaghSpouse.SendShiftTabAfterEnter = true;
		txtHaghSpouse.SendTabAfterEnter = true;
		txtHaghSpouse.SendTabAfterUP = true;
		txtHaghSpouse.ShowHorofi = (ENShowHorofi)3;
		txtHaghSpouse.ShowToolTipText = true;
		((Control)txtHaghSpouse).Size = new Size(209, 21);
		((Control)txtHaghSpouse).TabIndex = 11;
		((Control)txtHaghSpouse).Text = "0";
		txtHaghSpouse.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtHaghSpouse.ToolTipPostFix = "";
		txtHaghSpouse.TootTipDelay = 1000;
		txtHaghSpouse.Value = new decimal(new int[4]);
		((Control)labelX43).BackColor = Color.Transparent;
		((BaseItemControl)labelX43).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX43).Location = new Point(529, 205);
		((Control)labelX43).Name = "labelX43";
		((Control)labelX43).RightToLeft = (RightToLeft)1;
		((Control)labelX43).Size = new Size(146, 21);
		((Control)labelX43).TabIndex = 0;
		((Control)labelX43).Text = "[حق اولاد (1 فرزند)]:";
		labelX43.TextAlignment = (StringAlignment)2;
		((Control)MINEzafeDay).Anchor = (AnchorStyles)9;
		((Control)MINEzafeDay).Location = new Point(936, 6);
		((Control)MINEzafeDay).Name = "MINEzafeDay";
		((Control)MINEzafeDay).Size = new Size(54, 21);
		((Control)MINEzafeDay).TabIndex = 1;
		((Control)MINEzafeDay).Text = "440";
		MINEzafeDay.TextAlign = (HorizontalAlignment)2;
		((Control)MINEzafeDay).Visible = false;
		txtHaghMaskan.AllowEmpty = true;
		txtHaghMaskan.BackColorTextBoxWhenFocus = Color.Yellow;
		txtHaghMaskan.BackColorTextBoxWhenLostFocus = Color.White;
		txtHaghMaskan.BackcolorTooltip = Color.Red;
		txtHaghMaskan.CurrencyFormat = "";
		((Control)txtHaghMaskan).ForeColor = Color.Green;
		txtHaghMaskan.ForeColorTooltip = Color.Black;
		txtHaghMaskan.GetColoresFromSettingDLL = true;
		txtHaghMaskan.HandleKeydown = true;
		((Control)txtHaghMaskan).Location = new Point(314, 179);
		((TextBoxBase)txtHaghMaskan).MaxLength = 19;
		((Control)txtHaghMaskan).Name = "txtHaghMaskan";
		txtHaghMaskan.SendShiftTabAfterDown = true;
		txtHaghMaskan.SendShiftTabAfterEnter = true;
		txtHaghMaskan.SendTabAfterEnter = true;
		txtHaghMaskan.SendTabAfterUP = true;
		txtHaghMaskan.ShowHorofi = (ENShowHorofi)3;
		txtHaghMaskan.ShowToolTipText = true;
		((Control)txtHaghMaskan).Size = new Size(209, 21);
		((Control)txtHaghMaskan).TabIndex = 9;
		((Control)txtHaghMaskan).Text = "0";
		txtHaghMaskan.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtHaghMaskan.ToolTipPostFix = "";
		txtHaghMaskan.TootTipDelay = 1000;
		txtHaghMaskan.Value = new decimal(new int[4]);
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(993, 4);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(121, 23);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "[یک روز کاری معادل]";
		((Control)labelX10).Visible = false;
		((Control)labelX44).BackColor = Color.Transparent;
		((BaseItemControl)labelX44).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX44).Location = new Point(529, 178);
		((Control)labelX44).Name = "labelX44";
		((Control)labelX44).RightToLeft = (RightToLeft)1;
		((Control)labelX44).Size = new Size(106, 21);
		((Control)labelX44).TabIndex = 0;
		((Control)labelX44).Text = "[حق مسکن]:";
		labelX44.TextAlignment = (StringAlignment)2;
		((Control)btnGetLabor).AccessibleRole = (AccessibleRole)43;
		((Control)btnGetLabor).Anchor = (AnchorStyles)6;
		btnGetLabor.ColorTable = (eButtonColor)6;
		((Control)btnGetLabor).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnGetLabor.Image = (Image)(object)Resources.accept1;
		btnGetLabor.ImagePosition = (eImagePosition)1;
		btnGetLabor.ImageTextSpacing = 20;
		((Control)btnGetLabor).Location = new Point(13, 179);
		((Control)btnGetLabor).Name = "btnGetLabor";
		((Control)btnGetLabor).RightToLeft = (RightToLeft)1;
		((Control)btnGetLabor).Size = new Size(292, 48);
		((PopupItemControl)btnGetLabor).Style = (eDotNetBarStyle)9;
		((Control)btnGetLabor).TabIndex = 12;
		((Control)btnGetLabor).Text = "[بروزرسانی طبق حداقل قانون کار]";
		((Control)btnGetLabor).Click += btnGetLabor_Click;
		txtBon.AllowEmpty = true;
		((Control)txtBon).Anchor = (AnchorStyles)9;
		txtBon.BackColorTextBoxWhenFocus = Color.Yellow;
		txtBon.BackColorTextBoxWhenLostFocus = Color.White;
		txtBon.BackcolorTooltip = Color.Red;
		txtBon.CurrencyFormat = "";
		((Control)txtBon).ForeColor = Color.Green;
		txtBon.ForeColorTooltip = Color.Black;
		txtBon.GetColoresFromSettingDLL = true;
		txtBon.HandleKeydown = true;
		((Control)txtBon).Location = new Point(744, 206);
		((TextBoxBase)txtBon).MaxLength = 19;
		((Control)txtBon).Name = "txtBon";
		txtBon.SendShiftTabAfterDown = true;
		txtBon.SendShiftTabAfterEnter = true;
		txtBon.SendTabAfterEnter = true;
		txtBon.SendTabAfterUP = true;
		txtBon.ShowHorofi = (ENShowHorofi)3;
		txtBon.ShowToolTipText = true;
		((Control)txtBon).Size = new Size(207, 21);
		((Control)txtBon).TabIndex = 10;
		((Control)txtBon).Text = "0";
		txtBon.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtBon.ToolTipPostFix = "";
		txtBon.TootTipDelay = 1000;
		txtBon.Value = new decimal(new int[4]);
		((Control)labelX42).Anchor = (AnchorStyles)9;
		((Control)labelX42).BackColor = Color.Transparent;
		((BaseItemControl)labelX42).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX42).Location = new Point(957, 206);
		((Control)labelX42).Name = "labelX42";
		((Control)labelX42).RightToLeft = (RightToLeft)1;
		((Control)labelX42).Size = new Size(156, 21);
		((Control)labelX42).TabIndex = 0;
		((Control)labelX42).Text = "[حق بن و خوار و بار]:";
		labelX42.TextAlignment = (StringAlignment)2;
		txtDailyWage.AllowEmpty = true;
		((Control)txtDailyWage).Anchor = (AnchorStyles)9;
		txtDailyWage.BackColorTextBoxWhenFocus = Color.Yellow;
		txtDailyWage.BackColorTextBoxWhenLostFocus = Color.White;
		txtDailyWage.BackcolorTooltip = Color.Red;
		txtDailyWage.CurrencyFormat = "";
		((Control)txtDailyWage).ForeColor = Color.Green;
		txtDailyWage.ForeColorTooltip = Color.Black;
		txtDailyWage.GetColoresFromSettingDLL = true;
		txtDailyWage.HandleKeydown = true;
		((Control)txtDailyWage).Location = new Point(744, 179);
		((TextBoxBase)txtDailyWage).MaxLength = 19;
		((Control)txtDailyWage).Name = "txtDailyWage";
		txtDailyWage.SendShiftTabAfterDown = true;
		txtDailyWage.SendShiftTabAfterEnter = true;
		txtDailyWage.SendTabAfterEnter = true;
		txtDailyWage.SendTabAfterUP = true;
		txtDailyWage.ShowHorofi = (ENShowHorofi)3;
		txtDailyWage.ShowToolTipText = true;
		((Control)txtDailyWage).Size = new Size(207, 21);
		((Control)txtDailyWage).TabIndex = 8;
		((Control)txtDailyWage).Text = "0";
		txtDailyWage.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		txtDailyWage.ToolTipPostFix = "";
		txtDailyWage.TootTipDelay = 1000;
		txtDailyWage.Value = new decimal(new int[4]);
		((Control)labelX37).Anchor = (AnchorStyles)9;
		((Control)labelX37).BackColor = Color.Transparent;
		((BaseItemControl)labelX37).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX37).Location = new Point(957, 179);
		((Control)labelX37).Name = "labelX37";
		((Control)labelX37).RightToLeft = (RightToLeft)1;
		((Control)labelX37).Size = new Size(156, 21);
		((Control)labelX37).TabIndex = 0;
		((Control)labelX37).Text = "[حداقل دستمزد روزانه]:";
		labelX37.TextAlignment = (StringAlignment)2;
		((Control)CHAutoEzafe).Anchor = (AnchorStyles)9;
		((Control)CHAutoEzafe).BackColor = Color.Transparent;
		((BaseItemControl)CHAutoEzafe).BackgroundStyle.CornerType = (eCornerType)1;
		CHAutoEzafe.Checked = true;
		CHAutoEzafe.CheckState = (CheckState)1;
		CHAutoEzafe.CheckValue = "Y";
		((Control)CHAutoEzafe).Location = new Point(491, 32);
		((Control)CHAutoEzafe).Name = "CHAutoEzafe";
		((Control)CHAutoEzafe).Size = new Size(243, 23);
		((BaseItemControl)CHAutoEzafe).Style = (eDotNetBarStyle)9;
		((Control)CHAutoEzafe).TabIndex = 3;
		((Control)CHAutoEzafe).Text = "[اضافه کاری به صورت اتوماتیک لحاظ شود]";
		((Control)CHAutoEzafe).Visible = false;
		((Control)groupPanel4).BackColor = Color.Transparent;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)btnFraction3);
		((Control)groupPanel4).Controls.Add((Control)(object)btnFraction2);
		((Control)groupPanel4).Controls.Add((Control)(object)btnFraction1);
		((Control)groupPanel4).Controls.Add((Control)(object)btnAdditions1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX31);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX34);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX23);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX24);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction3Title);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction2Title);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction1Title);
		((Control)groupPanel4).Controls.Add((Control)(object)txtAdditions1Title);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction3Tafsil);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction3Moein);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction2Tafsil);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction2Moein);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction1Tafsil);
		((Control)groupPanel4).Controls.Add((Control)(object)txtFraction1Moein);
		((Control)groupPanel4).Controls.Add((Control)(object)txtAdditions1Tafsil);
		((Control)groupPanel4).Controls.Add((Control)(object)txtAdditions1Moein);
		((Control)groupPanel4).Controls.Add((Control)(object)lblAdditions1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX29);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX30);
		groupPanel4.DisabledBackColor = Color.Empty;
		((Control)groupPanel4).Location = new Point(13, 253);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(1099, 180);
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
		((Control)groupPanel4).TabIndex = 0;
		((Control)groupPanel4).Text = "[مشخص کردن کد و شرح حسابهای انتخابی در گزارش لیست حقوق]";
		((Control)btnFraction3).AccessibleRole = (AccessibleRole)43;
		btnFraction3.ColorTable = (eButtonColor)6;
		((Control)btnFraction3).Location = new Point(13, 131);
		((Control)btnFraction3).Name = "btnFraction3";
		((Control)btnFraction3).Size = new Size(29, 21);
		((PopupItemControl)btnFraction3).Style = (eDotNetBarStyle)9;
		((Control)btnFraction3).TabIndex = 28;
		((Control)btnFraction3).TabStop = false;
		((Control)btnFraction3).Text = "...";
		((Control)btnFraction3).Click += btnFraction3_Click;
		((Control)btnFraction2).AccessibleRole = (AccessibleRole)43;
		btnFraction2.ColorTable = (eButtonColor)6;
		((Control)btnFraction2).Location = new Point(13, 100);
		((Control)btnFraction2).Name = "btnFraction2";
		((Control)btnFraction2).Size = new Size(29, 21);
		((PopupItemControl)btnFraction2).Style = (eDotNetBarStyle)9;
		((Control)btnFraction2).TabIndex = 24;
		((Control)btnFraction2).TabStop = false;
		((Control)btnFraction2).Text = "...";
		((Control)btnFraction2).Click += btnFraction2_Click;
		((Control)btnFraction1).AccessibleRole = (AccessibleRole)43;
		btnFraction1.ColorTable = (eButtonColor)6;
		((Control)btnFraction1).Location = new Point(13, 71);
		((Control)btnFraction1).Name = "btnFraction1";
		((Control)btnFraction1).Size = new Size(29, 21);
		((PopupItemControl)btnFraction1).Style = (eDotNetBarStyle)9;
		((Control)btnFraction1).TabIndex = 20;
		((Control)btnFraction1).TabStop = false;
		((Control)btnFraction1).Text = "...";
		((Control)btnFraction1).Click += btnFraction1_Click;
		((Control)btnAdditions1).AccessibleRole = (AccessibleRole)43;
		btnAdditions1.ColorTable = (eButtonColor)6;
		((Control)btnAdditions1).Location = new Point(13, 42);
		((Control)btnAdditions1).Name = "btnAdditions1";
		((Control)btnAdditions1).Size = new Size(29, 21);
		((PopupItemControl)btnAdditions1).Style = (eDotNetBarStyle)9;
		((Control)btnAdditions1).TabIndex = 16;
		((Control)btnAdditions1).TabStop = false;
		((Control)btnAdditions1).Text = "...";
		((Control)btnAdditions1).Click += btnAdditions1_Click;
		((Control)labelX31).Anchor = (AnchorStyles)9;
		((Control)labelX31).BackColor = Color.Transparent;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(898, 129);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(181, 23);
		((Control)labelX31).TabIndex = 0;
		((Control)labelX31).Text = "[حساب انتخابی کسورات 3] :";
		labelX31.TextAlignment = (StringAlignment)2;
		((Control)labelX34).Anchor = (AnchorStyles)13;
		((Control)labelX34).BackColor = Color.Transparent;
		((BaseItemControl)labelX34).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX34).Location = new Point(306, 13);
		((Control)labelX34).Name = "labelX34";
		((Control)labelX34).Size = new Size(126, 23);
		((Control)labelX34).TabIndex = 0;
		((Control)labelX34).Text = "[شرح حساب]";
		((Control)labelX23).Anchor = (AnchorStyles)9;
		((Control)labelX23).BackColor = Color.Transparent;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Location = new Point(679, 13);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(105, 23);
		((Control)labelX23).TabIndex = 0;
		((Control)labelX23).Text = "[کد حساب تفصیل]";
		labelX23.TextAlignment = (StringAlignment)1;
		((Control)labelX24).Anchor = (AnchorStyles)9;
		((Control)labelX24).BackColor = Color.Transparent;
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Location = new Point(792, 13);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).Size = new Size(93, 23);
		((Control)labelX24).TabIndex = 0;
		((Control)labelX24).Text = "[کد حساب معین]";
		labelX24.TextAlignment = (StringAlignment)1;
		((Control)txtFraction3Title).Anchor = (AnchorStyles)13;
		((Control)txtFraction3Title).Location = new Point(48, 129);
		((Control)txtFraction3Title).Name = "txtFraction3Title";
		((Control)txtFraction3Title).Size = new Size(630, 21);
		((Control)txtFraction3Title).TabIndex = 27;
		txtFraction3Title.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction2Title).Anchor = (AnchorStyles)13;
		((Control)txtFraction2Title).Location = new Point(48, 100);
		((Control)txtFraction2Title).Name = "txtFraction2Title";
		((Control)txtFraction2Title).Size = new Size(630, 21);
		((Control)txtFraction2Title).TabIndex = 23;
		txtFraction2Title.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction1Title).Anchor = (AnchorStyles)13;
		((Control)txtFraction1Title).Location = new Point(48, 71);
		((Control)txtFraction1Title).Name = "txtFraction1Title";
		((Control)txtFraction1Title).Size = new Size(630, 21);
		((Control)txtFraction1Title).TabIndex = 19;
		txtFraction1Title.TextAlign = (HorizontalAlignment)2;
		((Control)txtAdditions1Title).Anchor = (AnchorStyles)13;
		((Control)txtAdditions1Title).Location = new Point(48, 42);
		((Control)txtAdditions1Title).Name = "txtAdditions1Title";
		((Control)txtAdditions1Title).Size = new Size(630, 21);
		((Control)txtAdditions1Title).TabIndex = 15;
		txtAdditions1Title.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction3Tafsil).Anchor = (AnchorStyles)9;
		((Control)txtFraction3Tafsil).Location = new Point(684, 129);
		((Control)txtFraction3Tafsil).Name = "txtFraction3Tafsil";
		((Control)txtFraction3Tafsil).Size = new Size(101, 21);
		((Control)txtFraction3Tafsil).TabIndex = 26;
		txtFraction3Tafsil.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction3Moein).Anchor = (AnchorStyles)9;
		((Control)txtFraction3Moein).Location = new Point(791, 129);
		((Control)txtFraction3Moein).Name = "txtFraction3Moein";
		((Control)txtFraction3Moein).Size = new Size(101, 21);
		((Control)txtFraction3Moein).TabIndex = 25;
		txtFraction3Moein.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction2Tafsil).Anchor = (AnchorStyles)9;
		((Control)txtFraction2Tafsil).Location = new Point(684, 100);
		((Control)txtFraction2Tafsil).Name = "txtFraction2Tafsil";
		((Control)txtFraction2Tafsil).Size = new Size(101, 21);
		((Control)txtFraction2Tafsil).TabIndex = 22;
		txtFraction2Tafsil.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction2Moein).Anchor = (AnchorStyles)9;
		((Control)txtFraction2Moein).Location = new Point(791, 100);
		((Control)txtFraction2Moein).Name = "txtFraction2Moein";
		((Control)txtFraction2Moein).Size = new Size(101, 21);
		((Control)txtFraction2Moein).TabIndex = 21;
		txtFraction2Moein.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction1Tafsil).Anchor = (AnchorStyles)9;
		((Control)txtFraction1Tafsil).Location = new Point(684, 71);
		((Control)txtFraction1Tafsil).Name = "txtFraction1Tafsil";
		((Control)txtFraction1Tafsil).Size = new Size(101, 21);
		((Control)txtFraction1Tafsil).TabIndex = 18;
		txtFraction1Tafsil.TextAlign = (HorizontalAlignment)2;
		((Control)txtFraction1Moein).Anchor = (AnchorStyles)9;
		((Control)txtFraction1Moein).Location = new Point(791, 71);
		((Control)txtFraction1Moein).Name = "txtFraction1Moein";
		((Control)txtFraction1Moein).Size = new Size(101, 21);
		((Control)txtFraction1Moein).TabIndex = 17;
		txtFraction1Moein.TextAlign = (HorizontalAlignment)2;
		((Control)txtAdditions1Tafsil).Anchor = (AnchorStyles)9;
		((Control)txtAdditions1Tafsil).Location = new Point(685, 42);
		((Control)txtAdditions1Tafsil).Name = "txtAdditions1Tafsil";
		((Control)txtAdditions1Tafsil).Size = new Size(101, 21);
		((Control)txtAdditions1Tafsil).TabIndex = 14;
		txtAdditions1Tafsil.TextAlign = (HorizontalAlignment)2;
		((Control)txtAdditions1Moein).Anchor = (AnchorStyles)9;
		((Control)txtAdditions1Moein).Location = new Point(791, 42);
		((Control)txtAdditions1Moein).Name = "txtAdditions1Moein";
		((Control)txtAdditions1Moein).Size = new Size(101, 21);
		((Control)txtAdditions1Moein).TabIndex = 13;
		txtAdditions1Moein.TextAlign = (HorizontalAlignment)2;
		((Control)lblAdditions1).Anchor = (AnchorStyles)9;
		((Control)lblAdditions1).BackColor = Color.Transparent;
		((BaseItemControl)lblAdditions1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblAdditions1).Location = new Point(898, 42);
		((Control)lblAdditions1).Name = "lblAdditions1";
		((Control)lblAdditions1).Size = new Size(181, 23);
		((Control)lblAdditions1).TabIndex = 0;
		((Control)lblAdditions1).Text = "[حساب انتخابی مزایا] :";
		lblAdditions1.TextAlignment = (StringAlignment)2;
		((Control)labelX29).Anchor = (AnchorStyles)9;
		((Control)labelX29).BackColor = Color.Transparent;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Location = new Point(898, 71);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).Size = new Size(181, 23);
		((Control)labelX29).TabIndex = 0;
		((Control)labelX29).Text = "[حساب انتخابی کسورات 1] :";
		labelX29.TextAlignment = (StringAlignment)2;
		((Control)labelX30).Anchor = (AnchorStyles)9;
		((Control)labelX30).BackColor = Color.Transparent;
		((BaseItemControl)labelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX30).Location = new Point(898, 100);
		((Control)labelX30).Name = "labelX30";
		((Control)labelX30).Size = new Size(181, 23);
		((Control)labelX30).TabIndex = 0;
		((Control)labelX30).Text = "[حساب انتخابی کسورات 2] :";
		labelX30.TextAlignment = (StringAlignment)2;
		((Control)txtMaxTedadOlaad).Anchor = (AnchorStyles)9;
		((Control)txtMaxTedadOlaad).Location = new Point(639, 150);
		txtMaxTedadOlaad.Maximum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)txtMaxTedadOlaad).Name = "txtMaxTedadOlaad";
		((Control)txtMaxTedadOlaad).Size = new Size(54, 21);
		((Control)txtMaxTedadOlaad).TabIndex = 7;
		((UpDownBase)txtMaxTedadOlaad).TextAlign = (HorizontalAlignment)2;
		txtMaxTedadOlaad.Value = new decimal(new int[4] { 4, 0, 0, 0 });
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(695, 150);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(418, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[حداکثر تعداد فرزندان که شخص میتواند در یک قرارداد بعنوان حق اولاد بگیرد]: ";
		tabItem3.AttachedControl = (Control)(object)tabControlPanel3;
		tabItem3.Name = "tabItem3";
		tabItem3.Text = "[اطلاعات حقوقی]";
		((Control)tabControlPanel5).Controls.Add((Control)(object)groupPanel7);
		((Control)tabControlPanel5).Controls.Add((Control)(object)pictureBox5);
		((Control)tabControlPanel5).Controls.Add((Control)(object)groupPanel8);
		((PanelEx)tabControlPanel5).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel5).Dock = (DockStyle)5;
		((Control)tabControlPanel5).Location = new Point(0, 26);
		((Control)tabControlPanel5).Name = "tabControlPanel5";
		((Control)tabControlPanel5).Padding = new Padding(1);
		((Control)tabControlPanel5).Size = new Size(1124, 483);
		((PanelEx)tabControlPanel5).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel5).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel5).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel5).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel5).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel5).Style.GradientAngle = 90;
		((Control)tabControlPanel5).TabIndex = 0;
		tabControlPanel5.TabItem = tabItem5;
		((Control)groupPanel7).Anchor = (AnchorStyles)14;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)CHEnableAutoBackupAllPartions);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Location = new Point(16, 181);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(1096, 100);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		((Control)groupPanel7).Text = "[پشتیبان گیری اتوماتیک هنگام خروج از نرم افزار]";
		((Control)CHEnableAutoBackupAllPartions).Anchor = (AnchorStyles)9;
		((BaseItemControl)CHEnableAutoBackupAllPartions).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CHEnableAutoBackupAllPartions).Location = new Point(717, 20);
		((Control)CHEnableAutoBackupAllPartions).Name = "CHEnableAutoBackupAllPartions";
		((Control)CHEnableAutoBackupAllPartions).Size = new Size(358, 23);
		((BaseItemControl)CHEnableAutoBackupAllPartions).Style = (eDotNetBarStyle)9;
		((Control)CHEnableAutoBackupAllPartions).TabIndex = 0;
		((Control)CHEnableAutoBackupAllPartions).Text = "[پشتیبان گیری اتوماتیک هنگام خروج از نرم افزار فعال باشد]";
		((Control)pictureBox5).Anchor = (AnchorStyles)9;
		((Control)pictureBox5).BackColor = Color.Transparent;
		pictureBox5.Image = (Image)(object)Resources.backup;
		((Control)pictureBox5).Location = new Point(1051, 4);
		((Control)pictureBox5).Name = "pictureBox5";
		((Control)pictureBox5).Size = new Size(61, 51);
		pictureBox5.SizeMode = (PictureBoxSizeMode)4;
		pictureBox5.TabIndex = 17;
		pictureBox5.TabStop = false;
		((Control)groupPanel8).Anchor = (AnchorStyles)15;
		((Control)groupPanel8).BackColor = Color.Transparent;
		((PanelControl)groupPanel8).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel8).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel8).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel8).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel8).Controls.Add((Control)(object)BackupBTNFindPath);
		((Control)groupPanel8).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel8).Controls.Add((Control)(object)TPBackup_DefaultPath);
		((Control)groupPanel8).Controls.Add((Control)(object)TbxBackupTimeBetween);
		((Control)groupPanel8).Controls.Add((Control)(object)CHEnableAutoBackup);
		groupPanel8.DisabledBackColor = Color.Empty;
		((Control)groupPanel8).Location = new Point(16, 61);
		((Control)groupPanel8).Name = "groupPanel8";
		((Control)groupPanel8).Size = new Size(1096, 114);
		((PanelControl)groupPanel8).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel8).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel8).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel8).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel8).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel8).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel8).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel8).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel8).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel8).Style.CornerDiameter = 4;
		((PanelControl)groupPanel8).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel8).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)groupPanel8).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel8).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel8).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel8).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel8).TabIndex = 0;
		((Control)groupPanel8).Text = "[زمانبندی تهیه نسخه پشتیبان]";
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(762, 32);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(110, 23);
		((Control)labelX6).TabIndex = 3;
		((Control)labelX6).Text = "[دقیقه]";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(938, 61);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(110, 23);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[مسیر تهیه پشتیبان] :";
		((Control)BackupBTNFindPath).Anchor = (AnchorStyles)9;
		((Control)BackupBTNFindPath).Location = new Point(906, 61);
		((Control)BackupBTNFindPath).Name = "BackupBTNFindPath";
		((Control)BackupBTNFindPath).Size = new Size(26, 23);
		((Control)BackupBTNFindPath).TabIndex = 5;
		((Control)BackupBTNFindPath).Text = "...";
		((ButtonBase)BackupBTNFindPath).UseVisualStyleBackColor = true;
		((Control)BackupBTNFindPath).Click += BackupBTNFindPath_Click;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(938, 32);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(110, 23);
		((Control)labelX8).TabIndex = 0;
		((Control)labelX8).Text = "[پشتیبان گیری پس از]";
		((Control)TPBackup_DefaultPath).Anchor = (AnchorStyles)13;
		((Control)TPBackup_DefaultPath).Location = new Point(3, 61);
		((Control)TPBackup_DefaultPath).Name = "TPBackup_DefaultPath";
		((Control)TPBackup_DefaultPath).RightToLeft = (RightToLeft)0;
		((Control)TPBackup_DefaultPath).Size = new Size(897, 23);
		((Control)TPBackup_DefaultPath).TabIndex = 6;
		TPBackup_DefaultPath.TabStop = true;
		((Control)TPBackup_DefaultPath).Text = "LinkLabel1";
		((Label)TPBackup_DefaultPath).TextAlign = (ContentAlignment)16;
		TPBackup_DefaultPath.LinkClicked += new LinkLabelLinkClickedEventHandler(TPBackup_DefaultPath_LinkClicked);
		((Control)TbxBackupTimeBetween).Anchor = (AnchorStyles)9;
		((Control)TbxBackupTimeBetween).Location = new Point(878, 32);
		TbxBackupTimeBetween.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		TbxBackupTimeBetween.Minimum = new decimal(new int[4] { 3, 0, 0, 0 });
		((Control)TbxBackupTimeBetween).Name = "TbxBackupTimeBetween";
		((Control)TbxBackupTimeBetween).Size = new Size(54, 21);
		((Control)TbxBackupTimeBetween).TabIndex = 2;
		TbxBackupTimeBetween.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)CHEnableAutoBackup).Anchor = (AnchorStyles)9;
		((BaseItemControl)CHEnableAutoBackup).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CHEnableAutoBackup).Location = new Point(877, 3);
		((Control)CHEnableAutoBackup).Name = "CHEnableAutoBackup";
		((Control)CHEnableAutoBackup).Size = new Size(198, 23);
		((BaseItemControl)CHEnableAutoBackup).Style = (eDotNetBarStyle)9;
		((Control)CHEnableAutoBackup).TabIndex = 0;
		((Control)CHEnableAutoBackup).Text = "[پشتیبان گیری زمانبندی فعال باشد]";
		tabItem5.AttachedControl = (Control)(object)tabControlPanel5;
		tabItem5.Name = "tabItem5";
		tabItem5.Text = "[پشتیبان گیری]";
		((FileDialog)openFileDialog1).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1124, 561);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(521, 416);
		((Control)this).Name = "FrmSetting";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تنظیمات نرم افزار]*";
		((Form)this).Load += FRMSetting_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMSetting_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel12).ResumeLayout(false);
		((Control)tabControlPanel11).ResumeLayout(false);
		((Control)pnlPayamakServiceSettings).ResumeLayout(false);
		((Control)tabControlPanel6).ResumeLayout(false);
		((Control)tabControlPanel6).PerformLayout();
		((Control)groupPanel2).ResumeLayout(false);
		((Control)groupPanel2).PerformLayout();
		((ISupportInitialize)NumSendAbsentSmsMaxMinutesAfterShift).EndInit();
		((ISupportInitialize)NumSendSmsToClock).EndInit();
		((ISupportInitialize)NumSendSmsFromClock).EndInit();
		((ISupportInitialize)NumSendIncompleteSmsToClock).EndInit();
		((ISupportInitialize)NumSendIncompleteSmsFromClock).EndInit();
		((ISupportInitialize)NumSendAbsentSmsMinMinutesAfterShift).EndInit();
		((ISupportInitialize)NumSendTaradodSmsMaxMinutes).EndInit();
		((Control)groupPanel14).ResumeLayout(false);
		((ISupportInitialize)pictureBox6).EndInit();
		((Control)tabControlPanel7).ResumeLayout(false);
		((ISupportInitialize)pictureBox7).EndInit();
		((Control)groupPanel12).ResumeLayout(false);
		((Control)tabControlPanel9).ResumeLayout(false);
		((Control)GrpWebservice).ResumeLayout(false);
		((Control)GrpWebservice).PerformLayout();
		((Control)tabControlPanel10).ResumeLayout(false);
		((ISupportInitialize)NumGetTaradodFormCountDaysAgo).EndInit();
		((Control)tabControlPanel8).ResumeLayout(false);
		((Control)tabControlPanel8).PerformLayout();
		((ISupportInitialize)NUMGharardadEndedNext).EndInit();
		((ISupportInitialize)NUMGharardadEndedAgo).EndInit();
		((Control)tabControlPanel2).ResumeLayout(false);
		((Control)groupPanel9).ResumeLayout(false);
		((ISupportInitialize)TbxCountReportDevices).EndInit();
		((ISupportInitialize)TbxCountLastTaradods).EndInit();
		((Control)groupPanel6).ResumeLayout(false);
		((Control)groupPanel5).ResumeLayout(false);
		((ISupportInitialize)NumLockTime).EndInit();
		((Control)groupPanel1).ResumeLayout(false);
		((ISupportInitialize)pictureBox3).EndInit();
		((Control)groupPanel3).ResumeLayout(false);
		((Control)groupPanel3).PerformLayout();
		((Control)tabControlPanel4).ResumeLayout(false);
		((Control)tabControlPanel4).PerformLayout();
		((Control)tabControlPanel3).ResumeLayout(false);
		((Control)tabControlPanel3).PerformLayout();
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((ISupportInitialize)txtMaxTedadOlaad).EndInit();
		((Control)tabControlPanel5).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((ISupportInitialize)pictureBox5).EndInit();
		((Control)groupPanel8).ResumeLayout(false);
		((ISupportInitialize)TbxBackupTimeBetween).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
