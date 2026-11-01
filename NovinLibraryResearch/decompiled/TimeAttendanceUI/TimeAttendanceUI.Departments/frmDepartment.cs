using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.Attendance;
using FormCustom.Cache;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.MetaData.Attendance;
using PacketParser.Payamak.Entities;
using PayamakUi.Users;
using Serilog;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Departments;

public class frmDepartment : CacheBaseForm
{
	private bool _isDisposed;

	private Guid? _adminGuid;

	private TextBox _whichTextBoxClicked;

	private CancellationTokenSource _adminToken;

	private PayamakClientHubInstances _payamakService;

	private IContainer components;

	internal ButtonX BTNMoreG;

	internal LabelX LabelX12;

	private TextBoxX TxtName;

	private LabelX labelX1;

	private TextBoxX txtAdmin;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	internal GroupPanel groupPanel14;

	private CheckBox CBPayamakEnable;

	internal GroupPanel groupPanel2;

	internal LabelX labelX73;

	internal NumericUpDown NumSendSmsToClock;

	internal NumericUpDown NumSendSmsFromClock;

	internal LabelX labelX74;

	internal LabelX labelX70;

	internal NumericUpDown NumSendIncompleteSmsToClock;

	internal NumericUpDown NumSendIncompleteSmsFromClock;

	internal LabelX labelX51;

	internal LabelX labelX4;

	internal TextBox TxtSmsIncompleteManager;

	internal TextBox TxtSmsAbsentManager;

	internal CheckBoxX chkBxIncompleteManager;

	internal CheckBoxX ChbSMSAbsentManager;

	internal ButtonX BtnDeleteAcc;

	internal NumericUpDown NumSendAbsentSmsMinMinutesAfterShift;

	internal NumericUpDown NumSendTaradodSmsMaxMinutes;

	internal ButtonX BtnMoreSms;

	internal LabelX labelX3;

	private TextBoxX TxtSamaneSMS;

	internal TextBox TxtsSmsManageV;

	internal LabelX labelX9;

	internal CheckBoxX ChbAutomateAccept;

	private TextBoxX TxtAdminMobs;

	internal CheckBoxX ChbSmsKHManage;

	internal LabelX labelX15;

	internal TextBox TxtsSmsManageKH;

	internal ButtonX BtnPerFamily;

	internal ButtonX BtnTime;

	internal ButtonX BtnKK;

	internal ButtonX BtnEK;

	internal ButtonX BtntypeTaradod;

	internal ButtonX BtnDate;

	internal ButtonX BtnPerName;

	internal GroupPanel groupPanel3;

	internal NumericUpDown NumForAutomateAccept;

	internal LabelX labelX2;

	public AttDepartmentBusiness CurrentObject { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!((CacheBaseForm)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private async Task SetDataAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> smsNetAccount = null;
			frmDepartment obj = this;
			AttDepartmentBusiness currentObject = CurrentObject;
			obj._adminGuid = ((currentObject != null) ? currentObject.AdminGuid : null);
			Task.Run(() => LoadAdminAsync(token));
			AttDepartmentBusiness currentObject2 = CurrentObject;
			bool? obj2;
			if (currentObject2 == null)
			{
				obj2 = null;
			}
			else
			{
				AttDepartmentMetaData metaData = currentObject2.MetaData;
				if (metaData == null)
				{
					obj2 = null;
				}
				else
				{
					AttDepartmentAdminSms adminSms = metaData.AdminSms;
					obj2 = ((adminSms != null) ? new bool?(adminSms.EnablePayamak) : null);
				}
			}
			bool? flag = obj2;
			if (flag.GetValueOrDefault())
			{
				if (CurrentObject.MetaData.AdminSms.IdSamane == 0)
				{
					CurrentObject.MetaData.AdminSms.IdSamane = 10000;
				}
				smsNetAccount = await _payamakService.SmsNetAccount.GetByIdAsync(CurrentObject.MetaData.AdminSms.IdSamane);
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					TextBoxX txtName = TxtName;
					AttDepartmentBusiness currentObject3 = CurrentObject;
					((Control)txtName).Text = ((currentObject3 != null) ? currentObject3.Name : null) ?? "";
					CheckBoxX chbAutomateAccept = ChbAutomateAccept;
					AttDepartmentAdminSetting adminSetting = CurrentObject.MetaData.AdminSetting;
					chbAutomateAccept.Checked = adminSetting != null && adminSetting.AcceptAutomate;
					NumericUpDown numForAutomateAccept = NumForAutomateAccept;
					AttDepartmentAdminSetting adminSetting2 = CurrentObject.MetaData.AdminSetting;
					numForAutomateAccept.Value = ((adminSetting2 != null) ? adminSetting2.AcceptAutomateMinutes : 0);
					((Control)groupPanel2).Enabled = false;
					AttDepartmentBusiness currentObject4 = CurrentObject;
					object obj3;
					if (currentObject4 == null)
					{
						obj3 = null;
					}
					else
					{
						AttDepartmentMetaData metaData2 = currentObject4.MetaData;
						obj3 = ((metaData2 != null) ? metaData2.AdminSms : null);
					}
					if (obj3 != null)
					{
						CBPayamakEnable.Checked = CurrentObject.MetaData.AdminSms.EnablePayamak;
						((Control)groupPanel2).Enabled = CBPayamakEnable.Checked;
						NumSendSmsFromClock.Value = CurrentObject.MetaData.AdminSms.SendSmsFromClockOfDay;
						NumSendSmsToClock.Value = CurrentObject.MetaData.AdminSms.SendSmsToClockOfDay;
						NumSendTaradodSmsMaxMinutes.Value = CurrentObject.MetaData.AdminSms.SendTaradodSmsMaxMinutes;
						NumSendIncompleteSmsFromClock.Value = CurrentObject.MetaData.AdminSms.SendIncompleteSmsFromClock;
						NumSendIncompleteSmsToClock.Value = CurrentObject.MetaData.AdminSms.SendIncompleteSmsToClock;
						NumSendAbsentSmsMinMinutesAfterShift.Value = CurrentObject.MetaData.AdminSms.SendAbsentSmsMinMinutesAfterShift;
						((Control)TxtAdminMobs).Text = CurrentObject.MetaData.AdminSms.PayamakManageAdminMobs;
						ChbAutomateAccept.Checked = CurrentObject.MetaData.AdminSms.SendVorodManage;
						((Control)TxtsSmsManageV).Text = CurrentObject.MetaData.AdminSms.PayamakManageVorod;
						((Control)TxtsSmsManageV).Enabled = ChbAutomateAccept.Checked;
						ChbSmsKHManage.Checked = CurrentObject.MetaData.AdminSms.SendKhorojManage;
						((Control)TxtsSmsManageKH).Text = CurrentObject.MetaData.AdminSms.PayamakManageKhoroj;
						((Control)TxtsSmsManageKH).Enabled = ChbSmsKHManage.Checked;
						ChbSMSAbsentManager.Checked = CurrentObject.MetaData.AdminSms.IsSendSmsAbsentManager;
						((Control)TxtSmsAbsentManager).Text = CurrentObject.MetaData.AdminSms.SmsAbsentManager;
						((Control)TxtSmsAbsentManager).Enabled = ChbSMSAbsentManager.Checked;
						chkBxIncompleteManager.Checked = CurrentObject.MetaData.AdminSms.IsSendSmsIncompleteManager;
						((Control)TxtSmsIncompleteManager).Text = CurrentObject.MetaData.AdminSms.SmsIncompleteManager;
						((Control)TxtSmsIncompleteManager).Enabled = chkBxIncompleteManager.Checked;
						TextBoxX txtSamaneSMS = TxtSamaneSMS;
						ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> obj4 = smsNetAccount;
						object obj5;
						if (obj4 == null)
						{
							obj5 = null;
						}
						else
						{
							BaseSmsNetAccount value = obj4.value;
							obj5 = ((value != null) ? value.Title : null);
						}
						if (obj5 == null)
						{
							obj5 = "";
						}
						((Control)txtSamaneSMS).Text = (string)obj5;
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadAdminAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			string adminName = "";
			if ((_adminGuid ?? Guid.Empty) != Guid.Empty)
			{
				Person person = await Person.GetBypersonGuidAsync(((CacheBaseForm)this).ContextGuid, _adminGuid.Value, token);
				adminName = ((person != null) ? ((PersonBase)person).FullName : null) ?? "";
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					((Control)txtAdmin).Text = adminName;
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				((CacheBaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public frmDepartment(string contextGuid, AttDepartmentBusiness str)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			CurrentObject = str;
			_payamakService = PayamakClientHubInstances.GetInstance(contextGuid);
			Task.Run(() => SetDataAsync());
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CurrentObject.Guid == Guid.Empty)
			{
				CurrentObject.Guid = Guid.NewGuid();
			}
			CurrentObject.AdminGuid = _adminGuid;
			CurrentObject.Name = ((Control)TxtName).Text;
			CurrentObject.ServerStatus = (ServerStatusForClient)0;
			AttDepartmentBusiness currentObject = CurrentObject;
			if (currentObject.MetaData == null)
			{
				currentObject.MetaData = new AttDepartmentMetaData();
			}
			AttDepartmentMetaData metaData = CurrentObject.MetaData;
			if (metaData.AdminSetting == null)
			{
				metaData.AdminSetting = new AttDepartmentAdminSetting();
			}
			CurrentObject.MetaData.AdminSetting.AcceptAutomate = ChbAutomateAccept.Checked;
			CurrentObject.MetaData.AdminSetting.AcceptAutomateMinutes = (int)NumForAutomateAccept.Value;
			metaData = CurrentObject.MetaData;
			if (metaData.AdminSms == null)
			{
				metaData.AdminSms = new AttDepartmentAdminSms();
			}
			CurrentObject.MetaData.AdminSms.EnablePayamak = CBPayamakEnable.Checked;
			CurrentObject.MetaData.AdminSms.SendSmsFromClockOfDay = NumSendSmsFromClock.Value;
			CurrentObject.MetaData.AdminSms.SendSmsToClockOfDay = NumSendSmsToClock.Value;
			CurrentObject.MetaData.AdminSms.SendTaradodSmsMaxMinutes = NumSendTaradodSmsMaxMinutes.Value;
			CurrentObject.MetaData.AdminSms.SendIncompleteSmsFromClock = NumSendIncompleteSmsFromClock.Value;
			CurrentObject.MetaData.AdminSms.SendIncompleteSmsToClock = NumSendIncompleteSmsToClock.Value;
			CurrentObject.MetaData.AdminSms.SendAbsentSmsMinMinutesAfterShift = NumSendAbsentSmsMinMinutesAfterShift.Value;
			CurrentObject.MetaData.AdminSms.PayamakManageAdminMobs = ((Control)TxtAdminMobs).Text;
			CurrentObject.MetaData.AdminSms.SendVorodManage = ChbAutomateAccept.Checked;
			CurrentObject.MetaData.AdminSms.PayamakManageVorod = ((Control)TxtsSmsManageV).Text;
			CurrentObject.MetaData.AdminSms.SendKhorojManage = ChbSmsKHManage.Checked;
			CurrentObject.MetaData.AdminSms.PayamakManageKhoroj = ((Control)TxtsSmsManageKH).Text;
			CurrentObject.MetaData.AdminSms.IsSendSmsAbsentManager = ChbSMSAbsentManager.Checked;
			CurrentObject.MetaData.AdminSms.SmsAbsentManager = ((Control)TxtSmsAbsentManager).Text;
			CurrentObject.MetaData.AdminSms.IsSendSmsIncompleteManager = chkBxIncompleteManager.Checked;
			CurrentObject.MetaData.AdminSms.SmsIncompleteManager = ((Control)TxtSmsIncompleteManager).Text;
			ReturnedSaveFuncInfo val = res;
			val.AddReturnedValue(await CurrentObject.SaveAsync(((CacheBaseForm)this).system, true, false, default(CancellationToken), (short)10, ""), (ReturnedState)0, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartment.cs", 175);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			res.AddReturnedValue(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartment.cs", 180);
		}
		finally
		{
			if (res.HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<frmDepartment>(this, res, "پیغام سیستم", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Departments\\frmDepartment.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void frmDepartment_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private async void BTNMoreG_Click(object sender, EventArgs e)
	{
		try
		{
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			FrmSelectPersonel frm = new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, isSelect: true, timeAttendanceSettings.FontProgram);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				_adminGuid = ((PersonBase)frm.SelectedPerson).Guid;
				_adminToken?.Cancel();
				_adminToken = new CancellationTokenSource(60000);
				CancellationToken token = _adminToken.Token;
				Task.Run(() => LoadAdminAsync(token));
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void frmDepartment_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					((PopupItemControl)BtnAccept).PerformClick();
				}
			}
			else
			{
				((PopupItemControl)BtnCancel).PerformClick();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void CBPayamakEnable_CheckedChanged(object sender, EventArgs e)
	{
		((Control)groupPanel2).Enabled = CBPayamakEnable.Checked;
	}

	private void ChbSmsVManage_CheckedChanged(object sender, EventArgs e)
	{
		((Control)TxtsSmsManageV).Enabled = ChbAutomateAccept.Checked;
	}

	private void ChbSmsKHManage_CheckedChanged(object sender, EventArgs e)
	{
		((Control)TxtsSmsManageKH).Enabled = ChbSmsKHManage.Checked;
	}

	private void ChbSMSAbsentManager_CheckedChanged(object sender, EventArgs e)
	{
		((Control)TxtSmsAbsentManager).Enabled = ChbSMSAbsentManager.Checked;
	}

	private void chkBxIncompleteManager_CheckedChanged(object sender, EventArgs e)
	{
		((Control)TxtSmsIncompleteManager).Enabled = chkBxIncompleteManager.Checked;
	}

	private async void BtnMoreSms_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSamaneSMS).Focus();
			FRMAccountMain frm = new FRMAccountMain(((CacheBaseForm)this).ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				AttDepartmentBusiness currentObject = CurrentObject;
				if (currentObject.MetaData == null)
				{
					currentObject.MetaData = new AttDepartmentMetaData();
				}
				AttDepartmentMetaData metaData = CurrentObject.MetaData;
				if (metaData.AdminSms == null)
				{
					metaData.AdminSms = new AttDepartmentAdminSms();
				}
				CurrentObject.MetaData.AdminSms.IdSamane = ((BaseSmsNetAccount)frm.SelectedAcount).Id;
				((Control)TxtSamaneSMS).Text = ((BaseSmsNetAccount)frm.SelectedAcount).Title;
			}
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnDeleteAcc_Click(object sender, EventArgs e)
	{
		try
		{
			AttDepartmentBusiness currentObject = CurrentObject;
			object obj;
			if (currentObject == null)
			{
				obj = null;
			}
			else
			{
				AttDepartmentMetaData metaData = currentObject.MetaData;
				obj = ((metaData != null) ? metaData.AdminSms : null);
			}
			if (obj != null)
			{
				CurrentObject.MetaData.AdminSms.IdSamane = 0;
				((Control)TxtSamaneSMS).Text = "";
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void BtnPerName_Click(object sender, EventArgs e)
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
			Log.Error(ex, "");
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
			Log.Error(ex, "");
		}
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_1411: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Expected O, but got Unknown
		//IL_26bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c5: Expected O, but got Unknown
		//IL_26d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_271c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2726: Expected O, but got Unknown
		//IL_272e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2738: Expected O, but got Unknown
		BTNMoreG = new ButtonX();
		LabelX12 = new LabelX();
		txtAdmin = new TextBoxX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		groupPanel14 = new GroupPanel();
		labelX2 = new LabelX();
		NumForAutomateAccept = new NumericUpDown();
		groupPanel2 = new GroupPanel();
		groupPanel3 = new GroupPanel();
		BtnKK = new ButtonX();
		BtnTime = new ButtonX();
		BtnEK = new ButtonX();
		BtnPerFamily = new ButtonX();
		BtntypeTaradod = new ButtonX();
		BtnPerName = new ButtonX();
		BtnDate = new ButtonX();
		labelX73 = new LabelX();
		NumSendSmsToClock = new NumericUpDown();
		NumSendSmsFromClock = new NumericUpDown();
		labelX74 = new LabelX();
		labelX70 = new LabelX();
		NumSendIncompleteSmsToClock = new NumericUpDown();
		NumSendIncompleteSmsFromClock = new NumericUpDown();
		labelX51 = new LabelX();
		labelX4 = new LabelX();
		TxtSmsIncompleteManager = new TextBox();
		TxtSmsAbsentManager = new TextBox();
		chkBxIncompleteManager = new CheckBoxX();
		ChbSMSAbsentManager = new CheckBoxX();
		BtnDeleteAcc = new ButtonX();
		NumSendAbsentSmsMinMinutesAfterShift = new NumericUpDown();
		NumSendTaradodSmsMaxMinutes = new NumericUpDown();
		BtnMoreSms = new ButtonX();
		labelX3 = new LabelX();
		TxtSamaneSMS = new TextBoxX();
		TxtsSmsManageV = new TextBox();
		labelX9 = new LabelX();
		TxtAdminMobs = new TextBoxX();
		ChbSmsKHManage = new CheckBoxX();
		labelX15 = new LabelX();
		TxtsSmsManageKH = new TextBox();
		CBPayamakEnable = new CheckBox();
		ChbAutomateAccept = new CheckBoxX();
		((Control)groupPanel14).SuspendLayout();
		((ISupportInitialize)NumForAutomateAccept).BeginInit();
		((Control)groupPanel2).SuspendLayout();
		((Control)groupPanel3).SuspendLayout();
		((ISupportInitialize)NumSendSmsToClock).BeginInit();
		((ISupportInitialize)NumSendSmsFromClock).BeginInit();
		((ISupportInitialize)NumSendIncompleteSmsToClock).BeginInit();
		((ISupportInitialize)NumSendIncompleteSmsFromClock).BeginInit();
		((ISupportInitialize)NumSendAbsentSmsMinMinutesAfterShift).BeginInit();
		((ISupportInitialize)NumSendTaradodSmsMaxMinutes).BeginInit();
		((Control)this).SuspendLayout();
		((Control)BTNMoreG).AccessibleRole = (AccessibleRole)43;
		BTNMoreG.ColorTable = (eButtonColor)6;
		((Control)BTNMoreG).Location = new Point(9, 9);
		((Control)BTNMoreG).Name = "BTNMoreG";
		((Control)BTNMoreG).Size = new Size(27, 27);
		((PopupItemControl)BTNMoreG).Style = (eDotNetBarStyle)9;
		((Control)BTNMoreG).TabIndex = 6;
		((Control)BTNMoreG).TabStop = false;
		((Control)BTNMoreG).Text = "...";
		((Control)BTNMoreG).Click += BTNMoreG_Click;
		((Control)LabelX12).BackColor = Color.Transparent;
		((BaseItemControl)LabelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX12).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)LabelX12).Location = new Point(404, 9);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).Size = new Size(98, 21);
		((Control)LabelX12).TabIndex = 4;
		((Control)LabelX12).Text = "مدیر:";
		LabelX12.TextAlignment = (StringAlignment)2;
		txtAdmin.Border.Class = "TextBoxBorder";
		txtAdmin.Border.CornerType = (eCornerType)1;
		((Control)txtAdmin).Location = new Point(39, 9);
		((TextBoxBase)txtAdmin).MaxLength = 50;
		((Control)txtAdmin).Name = "txtAdmin";
		((TextBoxBase)txtAdmin).ReadOnly = true;
		((Control)txtAdmin).Size = new Size(358, 27);
		((Control)txtAdmin).TabIndex = 11;
		((Control)TxtName).Anchor = (AnchorStyles)13;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(508, 9);
		((TextBoxBase)TxtName).MaxLength = 50;
		((Control)TxtName).Name = "TxtName";
		((Control)TxtName).Size = new Size(363, 27);
		((Control)TxtName).TabIndex = 11;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(878, 9);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(98, 21);
		((Control)labelX1).TabIndex = 10;
		((Control)labelX1).Text = "عنوان:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(3, 383);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 13;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "انصراف(Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(127, 383);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 12;
		((Control)BtnAccept).Text = "تایید  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((PanelControl)groupPanel14).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel14).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel14).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel14).Controls.Add((Control)(object)NumForAutomateAccept);
		((Control)groupPanel14).Controls.Add((Control)(object)groupPanel2);
		((Control)groupPanel14).Controls.Add((Control)(object)CBPayamakEnable);
		((Control)groupPanel14).Controls.Add((Control)(object)TxtName);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnCancel);
		((Control)groupPanel14).Controls.Add((Control)(object)BtnAccept);
		((Control)groupPanel14).Controls.Add((Control)(object)BTNMoreG);
		((Control)groupPanel14).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel14).Controls.Add((Control)(object)txtAdmin);
		((Control)groupPanel14).Controls.Add((Control)(object)LabelX12);
		((Control)groupPanel14).Controls.Add((Control)(object)ChbAutomateAccept);
		groupPanel14.DisabledBackColor = Color.Empty;
		((Control)groupPanel14).Dock = (DockStyle)5;
		((Control)groupPanel14).Location = new Point(0, 0);
		((Control)groupPanel14).Name = "groupPanel14";
		((Control)groupPanel14).Size = new Size(986, 426);
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
		((Control)groupPanel14).TabIndex = 14;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(41, 45);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(34, 21);
		((Control)labelX2).TabIndex = 41;
		((Control)labelX2).Text = "دقیقه";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)NumForAutomateAccept).Anchor = (AnchorStyles)9;
		((Control)NumForAutomateAccept).Location = new Point(80, 41);
		NumForAutomateAccept.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		((Control)NumForAutomateAccept).Name = "NumForAutomateAccept";
		((Control)NumForAutomateAccept).Size = new Size(59, 27);
		((Control)NumForAutomateAccept).TabIndex = 40;
		((UpDownBase)NumForAutomateAccept).TextAlign = (HorizontalAlignment)2;
		NumForAutomateAccept.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		((PanelControl)groupPanel2).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel2).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel2).Controls.Add((Control)(object)groupPanel3);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX73);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendSmsToClock);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendSmsFromClock);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX74);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX70);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendIncompleteSmsToClock);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendIncompleteSmsFromClock);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX51);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsIncompleteManager);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSmsAbsentManager);
		((Control)groupPanel2).Controls.Add((Control)(object)chkBxIncompleteManager);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSMSAbsentManager);
		((Control)groupPanel2).Controls.Add((Control)(object)BtnDeleteAcc);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendAbsentSmsMinMinutesAfterShift);
		((Control)groupPanel2).Controls.Add((Control)(object)NumSendTaradodSmsMaxMinutes);
		((Control)groupPanel2).Controls.Add((Control)(object)BtnMoreSms);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtSamaneSMS);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtsSmsManageV);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtAdminMobs);
		((Control)groupPanel2).Controls.Add((Control)(object)ChbSmsKHManage);
		((Control)groupPanel2).Controls.Add((Control)(object)labelX15);
		((Control)groupPanel2).Controls.Add((Control)(object)TxtsSmsManageKH);
		groupPanel2.DisabledBackColor = Color.Empty;
		((Control)groupPanel2).Location = new Point(0, 83);
		((Control)groupPanel2).Name = "groupPanel2";
		((Control)groupPanel2).Size = new Size(980, 294);
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
		((Control)groupPanel2).TabIndex = 38;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel3).Controls.Add((Control)(object)BtnKK);
		((Control)groupPanel3).Controls.Add((Control)(object)BtnTime);
		((Control)groupPanel3).Controls.Add((Control)(object)BtnEK);
		((Control)groupPanel3).Controls.Add((Control)(object)BtnPerFamily);
		((Control)groupPanel3).Controls.Add((Control)(object)BtntypeTaradod);
		((Control)groupPanel3).Controls.Add((Control)(object)BtnPerName);
		((Control)groupPanel3).Controls.Add((Control)(object)BtnDate);
		groupPanel3.DisabledBackColor = Color.Empty;
		((Control)groupPanel3).Location = new Point(9, 243);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(953, 43);
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
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 40;
		((Control)BtnKK).AccessibleRole = (AccessibleRole)43;
		BtnKK.ColorTable = (eButtonColor)6;
		((Control)BtnKK).Location = new Point(299, 4);
		((Control)BtnKK).Name = "BtnKK";
		((Control)BtnKK).Size = new Size(87, 28);
		((PopupItemControl)BtnKK).Style = (eDotNetBarStyle)9;
		((Control)BtnKK).TabIndex = 7;
		((Control)BtnKK).Text = "کسر کار";
		((Control)BtnKK).Click += BtnPerName_Click;
		((Control)BtnTime).AccessibleRole = (AccessibleRole)43;
		BtnTime.ColorTable = (eButtonColor)6;
		((Control)BtnTime).Location = new Point(578, 4);
		((Control)BtnTime).Name = "BtnTime";
		((Control)BtnTime).Size = new Size(87, 28);
		((PopupItemControl)BtnTime).Style = (eDotNetBarStyle)9;
		((Control)BtnTime).TabIndex = 4;
		((Control)BtnTime).Text = "[زمان]";
		((Control)BtnTime).Click += BtnPerName_Click;
		((Control)BtnEK).AccessibleRole = (AccessibleRole)43;
		BtnEK.ColorTable = (eButtonColor)6;
		((Control)BtnEK).Location = new Point(392, 4);
		((Control)BtnEK).Name = "BtnEK";
		((Control)BtnEK).Size = new Size(87, 28);
		((PopupItemControl)BtnEK).Style = (eDotNetBarStyle)9;
		((Control)BtnEK).TabIndex = 6;
		((Control)BtnEK).Text = "اضافه کار";
		((Control)BtnEK).Click += BtnPerName_Click;
		((Control)BtnPerFamily).AccessibleRole = (AccessibleRole)43;
		BtnPerFamily.ColorTable = (eButtonColor)6;
		((Control)BtnPerFamily).Location = new Point(764, 4);
		((Control)BtnPerFamily).Name = "BtnPerFamily";
		((Control)BtnPerFamily).Size = new Size(87, 28);
		((PopupItemControl)BtnPerFamily).Style = (eDotNetBarStyle)9;
		((Control)BtnPerFamily).TabIndex = 8;
		((Control)BtnPerFamily).Text = "نام خانوادگی";
		((Control)BtnPerFamily).Click += BtnPerName_Click;
		((Control)BtntypeTaradod).AccessibleRole = (AccessibleRole)43;
		BtntypeTaradod.ColorTable = (eButtonColor)6;
		((Control)BtntypeTaradod).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)BtntypeTaradod).Location = new Point(485, 4);
		((Control)BtntypeTaradod).Name = "BtntypeTaradod";
		((Control)BtntypeTaradod).Size = new Size(87, 28);
		((PopupItemControl)BtntypeTaradod).Style = (eDotNetBarStyle)9;
		((Control)BtntypeTaradod).TabIndex = 5;
		((Control)BtntypeTaradod).Text = "نوع تردد";
		((Control)BtntypeTaradod).Click += BtnPerName_Click;
		((Control)BtnPerName).AccessibleRole = (AccessibleRole)43;
		BtnPerName.ColorTable = (eButtonColor)6;
		((Control)BtnPerName).Location = new Point(857, 4);
		((Control)BtnPerName).Name = "BtnPerName";
		((Control)BtnPerName).Size = new Size(87, 28);
		((PopupItemControl)BtnPerName).Style = (eDotNetBarStyle)9;
		((Control)BtnPerName).TabIndex = 0;
		((Control)BtnPerName).Text = "نام";
		((Control)BtnPerName).Click += BtnPerName_Click;
		((Control)BtnDate).AccessibleRole = (AccessibleRole)43;
		BtnDate.ColorTable = (eButtonColor)6;
		((Control)BtnDate).Location = new Point(671, 4);
		((Control)BtnDate).Name = "BtnDate";
		((Control)BtnDate).Size = new Size(87, 28);
		((PopupItemControl)BtnDate).Style = (eDotNetBarStyle)9;
		((Control)BtnDate).TabIndex = 3;
		((Control)BtnDate).Text = "تاریخ";
		((Control)BtnDate).Click += BtnPerName_Click;
		((Control)labelX73).Anchor = (AnchorStyles)9;
		((Control)labelX73).BackColor = Color.Transparent;
		((BaseItemControl)labelX73).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX73).Location = new Point(533, 4);
		((Control)labelX73).Name = "labelX73";
		((Control)labelX73).Size = new Size(19, 21);
		((Control)labelX73).TabIndex = 35;
		((Control)labelX73).Text = "تا";
		labelX73.TextAlignment = (StringAlignment)1;
		((Control)NumSendSmsToClock).Anchor = (AnchorStyles)9;
		((Control)NumSendSmsToClock).Location = new Point(471, 4);
		NumSendSmsToClock.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)NumSendSmsToClock).Name = "NumSendSmsToClock";
		((Control)NumSendSmsToClock).Size = new Size(59, 27);
		((Control)NumSendSmsToClock).TabIndex = 34;
		((UpDownBase)NumSendSmsToClock).TextAlign = (HorizontalAlignment)2;
		NumSendSmsToClock.Value = new decimal(new int[4] { 22, 0, 0, 0 });
		((Control)NumSendSmsFromClock).Anchor = (AnchorStyles)9;
		((Control)NumSendSmsFromClock).BackColor = SystemColors.Window;
		((Control)NumSendSmsFromClock).Location = new Point(553, 4);
		NumSendSmsFromClock.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)NumSendSmsFromClock).Name = "NumSendSmsFromClock";
		((Control)NumSendSmsFromClock).Size = new Size(59, 27);
		((Control)NumSendSmsFromClock).TabIndex = 33;
		((UpDownBase)NumSendSmsFromClock).TextAlign = (HorizontalAlignment)2;
		NumSendSmsFromClock.Value = new decimal(new int[4] { 7, 0, 0, 0 });
		((Control)labelX74).BackColor = Color.Transparent;
		((BaseItemControl)labelX74).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX74).Location = new Point(624, 4);
		((Control)labelX74).Name = "labelX74";
		((Control)labelX74).RightToLeft = (RightToLeft)1;
		((Control)labelX74).Size = new Size(335, 21);
		((Control)labelX74).TabIndex = 32;
		((Control)labelX74).Text = "پیامک ها  بین چه ساعاتی از روز اجازه ارسال داشته باشند؟";
		labelX74.TextAlignment = (StringAlignment)2;
		((Control)labelX70).Anchor = (AnchorStyles)9;
		((Control)labelX70).BackColor = Color.Transparent;
		((BaseItemControl)labelX70).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX70).Location = new Point(533, 33);
		((Control)labelX70).Name = "labelX70";
		((Control)labelX70).Size = new Size(19, 21);
		((Control)labelX70).TabIndex = 31;
		((Control)labelX70).Text = "تا";
		labelX70.TextAlignment = (StringAlignment)1;
		((Control)NumSendIncompleteSmsToClock).Anchor = (AnchorStyles)9;
		((Control)NumSendIncompleteSmsToClock).Location = new Point(471, 33);
		NumSendIncompleteSmsToClock.Maximum = new decimal(new int[4] { 24, 0, 0, 0 });
		((Control)NumSendIncompleteSmsToClock).Name = "NumSendIncompleteSmsToClock";
		((Control)NumSendIncompleteSmsToClock).Size = new Size(59, 27);
		((Control)NumSendIncompleteSmsToClock).TabIndex = 30;
		((UpDownBase)NumSendIncompleteSmsToClock).TextAlign = (HorizontalAlignment)2;
		NumSendIncompleteSmsToClock.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumSendIncompleteSmsFromClock).Anchor = (AnchorStyles)9;
		((Control)NumSendIncompleteSmsFromClock).BackColor = SystemColors.Window;
		((Control)NumSendIncompleteSmsFromClock).Location = new Point(553, 33);
		NumSendIncompleteSmsFromClock.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)NumSendIncompleteSmsFromClock).Name = "NumSendIncompleteSmsFromClock";
		((Control)NumSendIncompleteSmsFromClock).Size = new Size(59, 27);
		((Control)NumSendIncompleteSmsFromClock).TabIndex = 29;
		((UpDownBase)NumSendIncompleteSmsFromClock).TextAlign = (HorizontalAlignment)2;
		NumSendIncompleteSmsFromClock.Value = new decimal(new int[4] { 9, 0, 0, 0 });
		((Control)labelX51).BackColor = Color.Transparent;
		((BaseItemControl)labelX51).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX51).Location = new Point(624, 33);
		((Control)labelX51).Name = "labelX51";
		((Control)labelX51).RightToLeft = (RightToLeft)1;
		((Control)labelX51).Size = new Size(335, 21);
		((Control)labelX51).TabIndex = 28;
		((Control)labelX51).Text = "پیامک های تردد ناقص روز قبل، بین چه ساعاتی  ارسال شوند؟";
		labelX51.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(74, 33);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(386, 21);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "پیامک غیبت حداقل چند دقیقه بعد از شروع شیفت  ارسال شود؟";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)TxtSmsIncompleteManager).Anchor = (AnchorStyles)9;
		((Control)TxtSmsIncompleteManager).Enabled = false;
		((Control)TxtSmsIncompleteManager).Location = new Point(9, 186);
		((TextBoxBase)TxtSmsIncompleteManager).Multiline = true;
		((Control)TxtSmsIncompleteManager).Name = "TxtSmsIncompleteManager";
		TxtSmsIncompleteManager.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsIncompleteManager).Size = new Size(451, 50);
		((Control)TxtSmsIncompleteManager).TabIndex = 25;
		((Control)TxtSmsIncompleteManager).TabStop = false;
		((TextBoxBase)TxtSmsIncompleteManager).Click += TxtsSmsManageV_Click;
		((Control)TxtSmsAbsentManager).Anchor = (AnchorStyles)9;
		((Control)TxtSmsAbsentManager).Enabled = false;
		((Control)TxtSmsAbsentManager).Location = new Point(478, 186);
		((TextBoxBase)TxtSmsAbsentManager).Multiline = true;
		((Control)TxtSmsAbsentManager).Name = "TxtSmsAbsentManager";
		TxtSmsAbsentManager.ScrollBars = (ScrollBars)3;
		((Control)TxtSmsAbsentManager).Size = new Size(484, 50);
		((Control)TxtSmsAbsentManager).TabIndex = 21;
		((Control)TxtSmsAbsentManager).TabStop = false;
		((TextBoxBase)TxtSmsAbsentManager).Click += TxtsSmsManageV_Click;
		((Control)chkBxIncompleteManager).Anchor = (AnchorStyles)9;
		((Control)chkBxIncompleteManager).BackColor = Color.Transparent;
		((BaseItemControl)chkBxIncompleteManager).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chkBxIncompleteManager).Location = new Point(244, 164);
		((Control)chkBxIncompleteManager).Name = "chkBxIncompleteManager";
		((Control)chkBxIncompleteManager).RightToLeft = (RightToLeft)1;
		((Control)chkBxIncompleteManager).Size = new Size(216, 23);
		((BaseItemControl)chkBxIncompleteManager).Style = (eDotNetBarStyle)9;
		((Control)chkBxIncompleteManager).TabIndex = 24;
		((Control)chkBxIncompleteManager).Text = "ارسال پیامک تردد ناقص";
		chkBxIncompleteManager.CheckedChanged += chkBxIncompleteManager_CheckedChanged;
		((Control)ChbSMSAbsentManager).Anchor = (AnchorStyles)9;
		((Control)ChbSMSAbsentManager).BackColor = Color.Transparent;
		((BaseItemControl)ChbSMSAbsentManager).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSMSAbsentManager).Location = new Point(746, 164);
		((Control)ChbSMSAbsentManager).Name = "ChbSMSAbsentManager";
		((Control)ChbSMSAbsentManager).RightToLeft = (RightToLeft)1;
		((Control)ChbSMSAbsentManager).Size = new Size(216, 23);
		((BaseItemControl)ChbSMSAbsentManager).Style = (eDotNetBarStyle)9;
		((Control)ChbSMSAbsentManager).TabIndex = 20;
		((Control)ChbSMSAbsentManager).Text = "ارسال پیامک غیبت";
		ChbSMSAbsentManager.CheckedChanged += ChbSMSAbsentManager_CheckedChanged;
		((Control)BtnDeleteAcc).AccessibleRole = (AccessibleRole)43;
		BtnDeleteAcc.ColorTable = (eButtonColor)6;
		BtnDeleteAcc.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDeleteAcc).Location = new Point(12, 62);
		((Control)BtnDeleteAcc).Name = "BtnDeleteAcc";
		((Control)BtnDeleteAcc).Size = new Size(27, 27);
		((PopupItemControl)BtnDeleteAcc).Style = (eDotNetBarStyle)9;
		((Control)BtnDeleteAcc).TabIndex = 11;
		BtnDeleteAcc.Tooltip = "[حذف سامانه و ارسال بامودم GSM]";
		((Control)BtnDeleteAcc).Click += BtnDeleteAcc_Click;
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Anchor = (AnchorStyles)9;
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Location = new Point(9, 33);
		NumSendAbsentSmsMinMinutesAfterShift.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Name = "NumSendAbsentSmsMinMinutesAfterShift";
		((Control)NumSendAbsentSmsMinMinutesAfterShift).Size = new Size(59, 27);
		((Control)NumSendAbsentSmsMinMinutesAfterShift).TabIndex = 5;
		((UpDownBase)NumSendAbsentSmsMinMinutesAfterShift).TextAlign = (HorizontalAlignment)2;
		NumSendAbsentSmsMinMinutesAfterShift.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		((Control)NumSendTaradodSmsMaxMinutes).Anchor = (AnchorStyles)9;
		((Control)NumSendTaradodSmsMaxMinutes).Location = new Point(9, 4);
		NumSendTaradodSmsMaxMinutes.Maximum = new decimal(new int[4] { 1439, 0, 0, 0 });
		NumSendTaradodSmsMaxMinutes.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)NumSendTaradodSmsMaxMinutes).Name = "NumSendTaradodSmsMaxMinutes";
		((Control)NumSendTaradodSmsMaxMinutes).Size = new Size(59, 27);
		((Control)NumSendTaradodSmsMaxMinutes).TabIndex = 1;
		((UpDownBase)NumSendTaradodSmsMaxMinutes).TextAlign = (HorizontalAlignment)2;
		NumSendTaradodSmsMaxMinutes.Value = new decimal(new int[4] { 20, 0, 0, 0 });
		((Control)BtnMoreSms).AccessibleRole = (AccessibleRole)43;
		BtnMoreSms.ColorTable = (eButtonColor)6;
		((Control)BtnMoreSms).Location = new Point(41, 62);
		((Control)BtnMoreSms).Name = "BtnMoreSms";
		((Control)BtnMoreSms).Size = new Size(27, 27);
		((PopupItemControl)BtnMoreSms).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreSms).TabIndex = 10;
		((Control)BtnMoreSms).Text = "...";
		BtnMoreSms.Tooltip = "[افزودن سامانه]";
		((Control)BtnMoreSms).Click += BtnMoreSms_Click;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(74, 4);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(386, 21);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "پیامک ورود/خروج، حداکثر چند دقیقه بعد از ثبت تردد اعتبار ارسال دارد؟";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtSamaneSMS.Border.Class = "TextBoxBorder";
		TxtSamaneSMS.Border.CornerType = (eCornerType)1;
		((Control)TxtSamaneSMS).Enabled = false;
		((Control)TxtSamaneSMS).Location = new Point(69, 62);
		((Control)TxtSamaneSMS).Name = "TxtSamaneSMS";
		((Control)TxtSamaneSMS).Size = new Size(262, 27);
		((Control)TxtSamaneSMS).TabIndex = 9;
		((Control)TxtsSmsManageV).Anchor = (AnchorStyles)9;
		((Control)TxtsSmsManageV).Enabled = false;
		((Control)TxtsSmsManageV).Location = new Point(478, 113);
		((TextBoxBase)TxtsSmsManageV).Multiline = true;
		((Control)TxtsSmsManageV).Name = "TxtsSmsManageV";
		TxtsSmsManageV.ScrollBars = (ScrollBars)3;
		((Control)TxtsSmsManageV).Size = new Size(484, 50);
		((Control)TxtsSmsManageV).TabIndex = 13;
		((Control)TxtsSmsManageV).TabStop = false;
		((TextBoxBase)TxtsSmsManageV).Click += TxtsSmsManageV_Click;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(337, 62);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).RightToLeft = (RightToLeft)1;
		((Control)labelX9).Size = new Size(132, 21);
		((Control)labelX9).TabIndex = 1;
		((Control)labelX9).Text = "سامانه ارسال کننده:";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)TxtAdminMobs).Anchor = (AnchorStyles)9;
		TxtAdminMobs.Border.Class = "TextBoxBorder";
		TxtAdminMobs.Border.CornerType = (eCornerType)1;
		((Control)TxtAdminMobs).Location = new Point(471, 62);
		((Control)TxtAdminMobs).Name = "TxtAdminMobs";
		((Control)TxtAdminMobs).Size = new Size(268, 27);
		((Control)TxtAdminMobs).TabIndex = 7;
		((Control)ChbSmsKHManage).Anchor = (AnchorStyles)9;
		((Control)ChbSmsKHManage).BackColor = Color.Transparent;
		((BaseItemControl)ChbSmsKHManage).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSmsKHManage).Location = new Point(246, 89);
		((Control)ChbSmsKHManage).Name = "ChbSmsKHManage";
		((Control)ChbSmsKHManage).RightToLeft = (RightToLeft)1;
		((Control)ChbSmsKHManage).Size = new Size(214, 23);
		((BaseItemControl)ChbSmsKHManage).Style = (eDotNetBarStyle)9;
		((Control)ChbSmsKHManage).TabIndex = 0;
		((Control)ChbSmsKHManage).Text = "ارسال پیامک خروج";
		ChbSmsKHManage.CheckedChanged += ChbSmsKHManage_CheckedChanged;
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((Control)labelX15).BackColor = Color.Transparent;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Location = new Point(743, 62);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).RightToLeft = (RightToLeft)1;
		((Control)labelX15).Size = new Size(219, 21);
		((Control)labelX15).TabIndex = 6;
		((Control)labelX15).Text = "شماره موبایل گیرنده:";
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)TxtsSmsManageKH).Anchor = (AnchorStyles)9;
		((Control)TxtsSmsManageKH).Enabled = false;
		((Control)TxtsSmsManageKH).Location = new Point(9, 113);
		((TextBoxBase)TxtsSmsManageKH).Multiline = true;
		((Control)TxtsSmsManageKH).Name = "TxtsSmsManageKH";
		TxtsSmsManageKH.ScrollBars = (ScrollBars)3;
		((Control)TxtsSmsManageKH).Size = new Size(451, 50);
		((Control)TxtsSmsManageKH).TabIndex = 17;
		((Control)TxtsSmsManageKH).TabStop = false;
		((TextBoxBase)TxtsSmsManageKH).Click += TxtsSmsManageV_Click;
		((Control)CBPayamakEnable).Anchor = (AnchorStyles)9;
		((Control)CBPayamakEnable).AutoSize = true;
		((Control)CBPayamakEnable).BackColor = Color.Transparent;
		((Control)CBPayamakEnable).Location = new Point(760, 42);
		((Control)CBPayamakEnable).Name = "CBPayamakEnable";
		((Control)CBPayamakEnable).Size = new Size(111, 24);
		((Control)CBPayamakEnable).TabIndex = 37;
		((Control)CBPayamakEnable).Text = "فعال سازی پیامک";
		((ButtonBase)CBPayamakEnable).UseVisualStyleBackColor = false;
		CBPayamakEnable.CheckedChanged += CBPayamakEnable_CheckedChanged;
		((Control)ChbAutomateAccept).Anchor = (AnchorStyles)9;
		((Control)ChbAutomateAccept).BackColor = Color.Transparent;
		((BaseItemControl)ChbAutomateAccept).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbAutomateAccept).Location = new Point(143, 45);
		((Control)ChbAutomateAccept).Name = "ChbAutomateAccept";
		((Control)ChbAutomateAccept).Size = new Size(254, 23);
		((BaseItemControl)ChbAutomateAccept).Style = (eDotNetBarStyle)9;
		((Control)ChbAutomateAccept).TabIndex = 12;
		((Control)ChbAutomateAccept).Text = "تایید اتومات درخواست بعد از زمان مشخص شده";
		ChbAutomateAccept.CheckedChanged += ChbSmsVManage_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(986, 426);
		((Control)this).Controls.Add((Control)(object)groupPanel14);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmDepartment";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "مدیریت دپارتمان";
		((Form)this).FormClosed += new FormClosedEventHandler(frmDepartment_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmDepartment_KeyDown);
		((Control)groupPanel14).ResumeLayout(false);
		((Control)groupPanel14).PerformLayout();
		((ISupportInitialize)NumForAutomateAccept).EndInit();
		((Control)groupPanel2).ResumeLayout(false);
		((Control)groupPanel2).PerformLayout();
		((Control)groupPanel3).ResumeLayout(false);
		((ISupportInitialize)NumSendSmsToClock).EndInit();
		((ISupportInitialize)NumSendSmsFromClock).EndInit();
		((ISupportInitialize)NumSendIncompleteSmsToClock).EndInit();
		((ISupportInitialize)NumSendIncompleteSmsFromClock).EndInit();
		((ISupportInitialize)NumSendAbsentSmsMinMinutesAfterShift).EndInit();
		((ISupportInitialize)NumSendTaradodSmsMaxMinutes).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
