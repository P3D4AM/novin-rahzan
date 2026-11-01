using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Attendance;
using PacketParser.Settings.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Enroll;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceDevices;
using TimeAttendanceUI.Forms.Device;
using TimeAttendanceUI.Forms.Service;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UCDevice : UserControl, IHasContext, INpDisposable, IDisposable
{
	public delegate void Exit_UserControlEventHandler();

	private bool _isDisposed;

	private List<DeviceInfo> lstDeviceInfosDevice;

	private CancellationTokenSource _loadDataToken;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	private LabelX CountRows;

	private LabelX labelX2;

	internal ButtonX btnPutEnrollData;

	internal ButtonX btnGetEnrollData;

	internal DataGridViewX DGrid;

	private BindingSource deviceInfoBindingSource;

	internal ButtonX btnGetBiometricsData;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewTextBoxColumn deviceNoDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modelDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TransferMode;

	private DataGridViewTextBoxColumn SerialNumberStr;

	internal ButtonX btnCopyBiometricInfo;

	internal ButtonX btnWriteDevice;

	internal ButtonX btnLog;

	internal ButtonX btnRelation;

	public string ContextGuid { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	private List<int> ListDeviceSelected
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<int> list = new List<int>();
			try
			{
				if (((DataGridView)DGrid).RowCount > 0)
				{
					for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
					{
						if (Convert.ToBoolean(((DataGridViewCell)(DataGridViewCheckBoxCell)((DataGridView)DGrid)[((DataGridViewBand)DGCheck).Index, i]).Value))
						{
							list.Add(int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, i].Value.ToString()));
						}
					}
				}
				if (list.Count <= 0 && ID > 0)
				{
					list.Add(ID);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ListDeviceSelected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 58);
			}
			return list;
		}
	}

	public int ID
	{
		get
		{
			try
			{
				return (((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null) ? int.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGID).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString()) : 0;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ID", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 70);
			}
			return 0;
		}
	}

	public DeviceInfo CurrentDevice { get; private set; }

	public event Exit_UserControlEventHandler Exit_UserControl;

	public async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			((Control)DGrid).Focus();
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			lstDeviceInfosDevice = await DeviceInfo.GetAllAsync(ContextGuid, token);
			RefreshLables();
			((Control)TxtSearch).Focus();
			DeviceServiceSettings deviceServiceSetting = await LocalSettingBusiness.GetDeviceServiceSettingsAsync(ContextGuid, token, true);
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
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)DGrid).Font = setting.FontProgram;
						deviceInfoBindingSource.DataSource = lstDeviceInfosDevice;
						((Control)BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Device.DeviceInsert;
						((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Device.DeviceDelete;
						((Control)BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Device.DeviceUpdate;
						ButtonX obj = btnCopyBiometricInfo;
						ButtonX obj2 = btnGetBiometricsData;
						bool flag = (((Control)btnWriteDevice).Enabled = deviceServiceSetting.HandleService);
						bool enabled = (((Control)obj2).Enabled = flag);
						((Control)obj).Enabled = enabled;
						ButtonX obj3 = btnRelation;
						enabled = (((Control)btnLog).Enabled = deviceServiceSetting.HandleService);
						((Control)obj3).Enabled = enabled;
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 113);
		}
	}

	private void RefreshLables()
	{
		try
		{
			((Control)CountRows).Text = deviceInfoBindingSource.Count.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RefreshLables", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 123);
		}
	}

	public void Insert()
	{
		((PopupItemControl)BtnAdd).PerformClick();
	}

	public void UpDate()
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	public void Delete()
	{
		((PopupItemControl)BtnDelete).PerformClick();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public UCDevice(string contextGuid)
	{
		InitializeComponent();
		ContextGuid = contextGuid;
	}

	private void BtnExit_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)this).Hide();
			this.Exit_UserControl?.Invoke();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnExit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 157);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!((Control)BtnAdd).Enabled || !((Control)BtnAdd).Visible)
			{
				return;
			}
			if ((int)AccessLevelClass.Instance.LevelHozor <= 3)
			{
				List<DeviceInfo> list = lstDeviceInfosDevice;
				if (list != null && list.Count >= 2)
				{
					FMessageBox.Show("برای افزودن دستگاه های بیشتر، بایستی سطح نرم افزار خود را ارتقاء دهید", "افزودن دستگاه", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
					return;
				}
			}
			if ((int)AccessLevelClass.Instance.LevelHozor <= 4)
			{
				List<DeviceInfo> list2 = lstDeviceInfosDevice;
				if (list2 != null && list2.Count >= 3)
				{
					FMessageBox.Show("برای افزودن دستگاه های بیشتر، بایستی سطح نرم افزار خود را ارتقاء دهید", "افزودن دستگاه", FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation);
					return;
				}
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			FrmDevice frm = new FrmDevice(ContextGuid);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync(token);
			}
			((Component)(object)frm).Dispose();
			LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 185);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)BtnEdit).Enabled || !((Control)BtnEdit).Visible)
		{
			return;
		}
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			FrmDevice frm = new FrmDevice(ContextGuid, ID);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataAsync(token);
			}
			((Component)(object)frm).Dispose();
			LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 203);
		}
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		if (!((Control)BtnDelete).Enabled || !((Control)BtnDelete).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			if (ID > 0)
			{
				DeviceInfo deviceInfo = await DeviceInfo.GetByIdAsync(ContextGuid, ID, token);
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به حذف دستگاه انتخاب شده هستید ؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
				{
					await deviceInfo.DeleteAsync(token);
					await LoadDataAsync(token);
				}
			}
			LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Utility.MessageBoxShowError("[به دلیل وجود اطلاعات مرتبط، قادر به حذف این دستگاه نمی باشید]", ContextGuid);
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 232);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			List<DeviceInfo> list = lstDeviceInfosDevice;
			foreach (string word in Text.SplitString(Text.FixString(((Control)TxtSearch).Text), " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:"))
			{
				if (!string.IsNullOrEmpty(word))
				{
					list = list.Where((DeviceInfo p) => p.DeviceName.ToLower().Contains(word.ToLower()) || Utilities.GetDisplay<DeviceModelEnumType>(p.Model).ToLower().Contains(word.ToLower())).ToList();
				}
			}
			deviceInfoBindingSource.DataSource = list;
			((Control)DGrid).ResetBindings();
			((Control)DGrid).Update();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 252);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Focus();
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = 9999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 266);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	private void TxtSearch_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TxtSearch).SelectAll();
			((Control)TxtSearch).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 282);
		}
	}

	private void TxtSearch_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 293);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13 || (int)e.KeyCode == 40)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 305);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
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
			if ((int)keyCode != 13)
			{
				if ((int)keyCode != 32)
				{
					return;
				}
				e.Handled = true;
				if (((Control)DGrid).Focused && ID >= 0 && ((DataGridView)DGrid).CurrentCell.ColumnIndex != ((DataGridViewBand)DGCheck).Index)
				{
					DataGridViewCell obj = ((DataGridView)DGrid).SelectedRows[0].Cells[((DataGridViewBand)DGCheck).Index];
					DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((obj is DataGridViewCheckBoxCell) ? obj : null);
					if (val != null)
					{
						((DataGridViewCell)val).Value = !Convert.ToBoolean(((DataGridViewCell)val).Value);
					}
				}
			}
			else
			{
				((PopupItemControl)BtnEdit).PerformClick();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 328);
		}
	}

	private async void btnGetEnrollData_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (!(ListDeviceSelected?.Any() ?? false))
			{
				FMessageBox.Show(Utilities.GetTranslate("[هیچ دستگاهی انتخاب نشده است. برای خواندن اطلاعات اثر انگشت یا چهره ابتدا دستگاههای مورد نظر را انتخاب نمائید]"), Utilities.GetTranslate("[توجه]"), FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1);
			}
			else if ((int)FMessageBox.Show(Utilities.GetTranslate("[شما قصد خواندن اطلاعات اثر انگشت از دستگاههای انتخاب شده را دارید . آیا مطمئن هستید؟]"), Utilities.GetTranslate("[توجه]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1) == 6)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				List<EnrollData> enrollDataList = await EnrollManage.GetEnrollManage(ContextGuid).GetEnrollDataFromDeviceListAsync(ListDeviceSelected, token);
				await Person.SaveEnrollDataAsync(ContextGuid, enrollDataList, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnGetEnrollData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 351);
		}
	}

	private async void BtnPutEnrollData_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (ListDeviceSelected.Count <= 0)
			{
				Utility.MessageBoxShowError("[هیچ دستگاهی انتخاب نشده است. برای نوشتن اطلاعات اثر انگشت یا چهره ابتدا دستگاههای مورد نظر را انتخاب نمائید]", ContextGuid);
				return;
			}
			string translate = Utilities.GetTranslate("[شما قصد نوشتن اطلاعات اثر انگشت روی دستگاههای انتخاب شده را دارید . آیا مطمئن هستید؟]");
			string translate2 = Utilities.GetTranslate("[توجه]");
			if ((int)FMessageBox.Show(translate, translate2, FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1) == 6)
			{
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				List<EnrollData> enrollDataList = await Person.LoadEnrollDataAsync(ContextGuid, "", token);
				await EnrollManage.GetEnrollManage(ContextGuid).PutEnrollDataAsync(enrollDataList, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPutEnrollData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 377);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if ((e == null || e.RowIndex >= 0) && DGCheck != null && e != null && e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && ID >= 0)
			{
				DataGridViewCell currentCell = ((DataGridView)DGrid).CurrentCell;
				DataGridViewCheckBoxCell val = (DataGridViewCheckBoxCell)(object)((currentCell is DataGridViewCheckBoxCell) ? currentCell : null);
				if (val != null)
				{
					((DataGridViewCell)val).Value = !Utilities.ParseToBoolean((((DataGridViewCell)val).Value ?? ((object)true)).ToString(), false);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 392);
		}
	}

	private async void btnGetBiometricsData_Click(object sender, EventArgs e)
	{
		Waiter waiter = null;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				if (!(ListDeviceSelected?.Any() ?? false))
				{
					FMessageBox.Show(Utilities.GetTranslate("[هیچ دستگاهی انتخاب نشده است. برای خواندن اطلاعات اثر انگشت یا چهره ابتدا دستگاههای مورد نظر را انتخاب نمائید]"), Utilities.GetTranslate("[توجه]"), FMessageBox.FMessageBoxButtons.OK, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1);
					goto end_IL_0042;
				}
				if ((int)FMessageBox.Show(Utilities.GetTranslate("[شما قصد خواندن اطلاعات اثر انگشت از دستگاههای انتخاب شده را دارید . آیا مطمئن هستید؟]"), Utilities.GetTranslate("[توجه]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button1) != 6)
				{
					goto end_IL_0042;
				}
				waiter = new Waiter("درحال ارسال درخواست به سرویس ...", (Control)(object)this, (Task)null, true, (EnWaiterType)0, (Control)null, "btnGetBiometricsData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs");
				foreach (int item in ListDeviceSelected)
				{
					if (item > 0)
					{
						DeviceInfo deviceInfo = lstDeviceInfosDevice?.SingleOrDefault((DeviceInfo q) => q.Id == item);
						if (deviceInfo != null)
						{
							DeviceServiceHandler instance = DeviceServiceHandler.GetInstance(ContextGuid, (CacheBaseForm)((Control)this).FindForm());
							ReturnedSaveFuncInfo val = res;
							val.AddReturnedValue(await instance.SendReadRequestToServiceAsync(ContextGuid, deviceInfo.Guid), (ReturnedState)0, "btnGetBiometricsData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 417);
						}
					}
				}
				goto end_IL_0039;
				end_IL_0042:;
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "btnGetBiometricsData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 422);
				res.AddReturnedValue(ex, "", "btnGetBiometricsData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 423);
				goto end_IL_0039;
			}
			end_IL_0039:;
		}
		finally
		{
			Waiter obj = waiter;
			if (obj != null)
			{
				obj.Dispose();
			}
			if (res.HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoForUserControlAsync<UCDevice>(this, res, "پیغام سیستم", "btnGetBiometricsData_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs");
			}
			else
			{
				AggregatorExtention.ShowMessage((UserControl)(object)this, "[اطلاعات دستگاه با موفقیت دریافت و ذخیره سازی شد]", "پیغام سیستم");
			}
		}
	}

	private async void btnCopyBiometricInfo_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentDevice != null)
			{
				await ((CacheBaseForm)new frmDeviceSelect(ContextGuid, CurrentDevice)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCopyBiometricInfo_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 440);
		}
	}

	private void deviceInfoBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			CurrentDevice = (DeviceInfo)deviceInfoBindingSource.Current;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "deviceInfoBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 448);
		}
	}

	private async void btnWriteDevice_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new frmBioWriteDevice(ContextGuid)).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnWriteDevice_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 457);
		}
	}

	private void DGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
	{
	}

	private async void btnRelation_Click(object sender, EventArgs e)
	{
		try
		{
			if (CurrentDevice != null)
			{
				await ((CacheBaseForm)new frmBioDeviceRelation(ContextGuid, CurrentDevice.Guid)).ShowAsync((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnRelation_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 468);
		}
	}

	private async void btnLog_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new frmBioWriteDeviceLog(ContextGuid, new BioWriteDeviceLogFilter
			{
				DeviceGuid = CurrentDevice?.Guid
			})).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnLog_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCDevice.cs", 477);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Expected O, but got Unknown
		//IL_0d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Expected O, but got Unknown
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Expected O, but got Unknown
		//IL_0e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Expected O, but got Unknown
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f64: Expected O, but got Unknown
		//IL_1029: Unknown result type (might be due to invalid IL or missing references)
		//IL_1033: Expected O, but got Unknown
		//IL_10f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Expected O, but got Unknown
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d5: Expected O, but got Unknown
		//IL_12a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b1: Expected O, but got Unknown
		//IL_1377: Unknown result type (might be due to invalid IL or missing references)
		//IL_1381: Expected O, but got Unknown
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1444: Expected O, but got Unknown
		//IL_1508: Unknown result type (might be due to invalid IL or missing references)
		//IL_1512: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		DGCheck = new DataGridViewCheckBoxColumn();
		deviceNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modelDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		TransferMode = new DataGridViewTextBoxColumn();
		SerialNumberStr = new DataGridViewTextBoxColumn();
		deviceInfoBindingSource = new BindingSource(components);
		CountRows = new LabelX();
		labelX2 = new LabelX();
		GroupPanel1 = new GroupPanel();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		btnWriteDevice = new ButtonX();
		btnCopyBiometricInfo = new ButtonX();
		btnGetBiometricsData = new ButtonX();
		btnPutEnrollData = new ButtonX();
		btnGetEnrollData = new ButtonX();
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		btnRelation = new ButtonX();
		btnLog = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)deviceInfoBindingSource).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)CountRows);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)btnLog);
		((Control)PanelEx1).Controls.Add((Control)(object)btnWriteDevice);
		((Control)PanelEx1).Controls.Add((Control)(object)btnRelation);
		((Control)PanelEx1).Controls.Add((Control)(object)btnCopyBiometricInfo);
		((Control)PanelEx1).Controls.Add((Control)(object)btnGetBiometricsData);
		((Control)PanelEx1).Controls.Add((Control)(object)btnPutEnrollData);
		((Control)PanelEx1).Controls.Add((Control)(object)btnGetEnrollData);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(596, 484);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)deviceNoDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)modelDataGridViewTextBoxColumn,
			(DataGridViewColumn)TransferMode,
			(DataGridViewColumn)SerialNumberStr
		});
		((DataGridView)DGrid).DataSource = deviceInfoBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(139, 72);
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(451, 371);
		((Control)DGrid).TabIndex = 7;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).DataError += new DataGridViewDataErrorEventHandler(DGrid_DataError);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((Control)DGrid).KeyPress += new KeyPressEventHandler(DGrid_KeyPress);
		((DataGridViewColumn)DGID).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).HeaderText = "";
		((DataGridViewColumn)DGCheck).MinimumWidth = 20;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGCheck).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGCheck).Width = 20;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).DataPropertyName = "DeviceNo";
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).HeaderText = "[شماره دستگاه]";
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).MinimumWidth = 110;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).Name = "deviceNoDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceNoDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).Width = 110;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).DataPropertyName = "DeviceName";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).HeaderText = "[نام دستگاه]";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).Name = "deviceNameDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).DataPropertyName = "Model";
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).HeaderText = "[مدل دستگاه]";
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).Name = "modelDataGridViewTextBoxColumn";
		((DataGridViewBand)modelDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)TransferMode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TransferMode).DataPropertyName = "TransferMode";
		((DataGridViewColumn)TransferMode).HeaderText = "[روش ارتباط]";
		((DataGridViewColumn)TransferMode).Name = "TransferMode";
		((DataGridViewBand)TransferMode).ReadOnly = true;
		((DataGridViewColumn)TransferMode).Width = 93;
		((DataGridViewColumn)SerialNumberStr).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)SerialNumberStr).DataPropertyName = "SerialNumberStr";
		((DataGridViewColumn)SerialNumberStr).HeaderText = "[شماره سریال]";
		((DataGridViewColumn)SerialNumberStr).MinimumWidth = 250;
		((DataGridViewColumn)SerialNumberStr).Name = "SerialNumberStr";
		((DataGridViewBand)SerialNumberStr).ReadOnly = true;
		((DataGridViewColumn)SerialNumberStr).Width = 250;
		deviceInfoBindingSource.DataSource = typeof(DeviceInfo);
		deviceInfoBindingSource.CurrentChanged += deviceInfoBindingSource_CurrentChanged;
		((Control)CountRows).Anchor = (AnchorStyles)10;
		((BaseItemControl)CountRows).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)CountRows).ForeColor = Color.DarkRed;
		((Control)CountRows).Location = new Point(177, 449);
		((Control)CountRows).Name = "CountRows";
		((Control)CountRows).Size = new Size(293, 23);
		((Control)CountRows).TabIndex = 6;
		((Control)CountRows).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(476, 449);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(114, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "[تعداد کل دستگاه ها]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(5, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(585, 60);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(497, 3);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(67, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو] :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(3, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(488, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).Enter += TxtSearch_Enter;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtSearch_Leave;
		((Control)btnWriteDevice).AccessibleRole = (AccessibleRole)43;
		btnWriteDevice.ColorTable = (eButtonColor)6;
		((Control)btnWriteDevice).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnWriteDevice.ImagePosition = (eImagePosition)1;
		((Control)btnWriteDevice).Location = new Point(5, 344);
		((Control)btnWriteDevice).Name = "btnWriteDevice";
		((Control)btnWriteDevice).Size = new Size(128, 33);
		((PopupItemControl)btnWriteDevice).Style = (eDotNetBarStyle)9;
		((Control)btnWriteDevice).TabIndex = 4;
		((Control)btnWriteDevice).Text = "[لیست دستورات دستگاه ها]";
		((Control)btnWriteDevice).Click += btnWriteDevice_Click;
		((Control)btnCopyBiometricInfo).AccessibleRole = (AccessibleRole)43;
		btnCopyBiometricInfo.ColorTable = (eButtonColor)6;
		((Control)btnCopyBiometricInfo).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnCopyBiometricInfo.ImagePosition = (eImagePosition)1;
		((Control)btnCopyBiometricInfo).Location = new Point(5, 306);
		((Control)btnCopyBiometricInfo).Name = "btnCopyBiometricInfo";
		((Control)btnCopyBiometricInfo).Size = new Size(128, 33);
		((PopupItemControl)btnCopyBiometricInfo).Style = (eDotNetBarStyle)9;
		((Control)btnCopyBiometricInfo).TabIndex = 4;
		((Control)btnCopyBiometricInfo).Text = "[کپی کردن اطلاعات ثبت نام]";
		((Control)btnCopyBiometricInfo).Click += btnCopyBiometricInfo_Click;
		((Control)btnGetBiometricsData).AccessibleRole = (AccessibleRole)43;
		btnGetBiometricsData.ColorTable = (eButtonColor)6;
		((Control)btnGetBiometricsData).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnGetBiometricsData.ImagePosition = (eImagePosition)1;
		((Control)btnGetBiometricsData).Location = new Point(5, 267);
		((Control)btnGetBiometricsData).Name = "btnGetBiometricsData";
		((Control)btnGetBiometricsData).Size = new Size(128, 33);
		((PopupItemControl)btnGetBiometricsData).Style = (eDotNetBarStyle)9;
		((Control)btnGetBiometricsData).TabIndex = 4;
		((Control)btnGetBiometricsData).Text = "[دریافت اطلاعات ثبت نام]";
		((Control)btnGetBiometricsData).Click += btnGetBiometricsData_Click;
		((Control)btnPutEnrollData).AccessibleRole = (AccessibleRole)43;
		btnPutEnrollData.ColorTable = (eButtonColor)6;
		((Control)btnPutEnrollData).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnPutEnrollData.ImagePosition = (eImagePosition)1;
		((Control)btnPutEnrollData).Location = new Point(5, 228);
		((Control)btnPutEnrollData).Name = "btnPutEnrollData";
		((Control)btnPutEnrollData).Size = new Size(128, 33);
		((PopupItemControl)btnPutEnrollData).Style = (eDotNetBarStyle)9;
		((Control)btnPutEnrollData).TabIndex = 4;
		((Control)btnPutEnrollData).Text = "[نوشتن اطلاعات اثر انگشت]";
		((Control)btnPutEnrollData).Visible = false;
		((Control)btnPutEnrollData).Click += BtnPutEnrollData_Click;
		((Control)btnGetEnrollData).AccessibleRole = (AccessibleRole)43;
		btnGetEnrollData.ColorTable = (eButtonColor)6;
		((Control)btnGetEnrollData).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnGetEnrollData.ImagePosition = (eImagePosition)1;
		((Control)btnGetEnrollData).Location = new Point(5, 189);
		((Control)btnGetEnrollData).Name = "btnGetEnrollData";
		((Control)btnGetEnrollData).Size = new Size(128, 33);
		((PopupItemControl)btnGetEnrollData).Style = (eDotNetBarStyle)9;
		((Control)btnGetEnrollData).TabIndex = 4;
		((Control)btnGetEnrollData).Text = "[خواندن اطلاعات اثر انگشت]";
		((Control)btnGetEnrollData).Visible = false;
		((Control)btnGetEnrollData).Click += btnGetEnrollData_Click;
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(5, 150);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(128, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 4;
		((Control)BtnDelete).Text = "[حذف](Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(5, 111);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(128, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 3;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(5, 72);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(128, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 2;
		((Control)BtnAdd).Text = "[اضافه](Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)btnRelation).AccessibleRole = (AccessibleRole)43;
		btnRelation.ColorTable = (eButtonColor)6;
		((Control)btnRelation).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnRelation.ImagePosition = (eImagePosition)1;
		((Control)btnRelation).Location = new Point(5, 383);
		((Control)btnRelation).Name = "btnRelation";
		((Control)btnRelation).Size = new Size(128, 33);
		((PopupItemControl)btnRelation).Style = (eDotNetBarStyle)9;
		((Control)btnRelation).TabIndex = 4;
		((Control)btnRelation).Text = "[رابطه دستگاه و پرسنل]";
		((Control)btnRelation).Click += btnRelation_Click;
		((Control)btnLog).AccessibleRole = (AccessibleRole)43;
		btnLog.ColorTable = (eButtonColor)6;
		((Control)btnLog).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnLog.ImagePosition = (eImagePosition)1;
		((Control)btnLog).Location = new Point(5, 421);
		((Control)btnLog).Name = "btnLog";
		((Control)btnLog).Size = new Size(128, 33);
		((PopupItemControl)btnLog).Style = (eDotNetBarStyle)9;
		((Control)btnLog).TabIndex = 4;
		((Control)btnLog).Text = "[سابقه اطلاعات دستگاه]";
		((Control)btnLog).Click += btnLog_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).Name = "UCDevice";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(596, 484);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)deviceInfoBindingSource).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
