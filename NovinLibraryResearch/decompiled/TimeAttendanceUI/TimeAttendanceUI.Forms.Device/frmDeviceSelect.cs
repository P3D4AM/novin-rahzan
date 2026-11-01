using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.UC;
using FormCustom.Waiting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceUI.Forms.Service;

namespace TimeAttendanceUI.Forms.Device;

public class frmDeviceSelect : AccountingBaseForm
{
	private bool _isDisposed;

	private List<DeviceInfo> lstDeviceInfo;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _loadDeviceToken;

	private IContainer components;

	private Label label1;

	private Label lblDeviceName;

	private BindingSource deviceInfoBindingSource;

	internal DataGridView DGrid;

	private Label label2;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewCheckBoxColumn dgIsChecked;

	private DataGridViewTextBoxColumn deviceNoDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn modelDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn TransferMode;

	private DataGridViewTextBoxColumn SerialNumberStr;

	public DeviceInfo DeviceInfo { get; private set; }

	public DeviceInfo DestinationDevice { get; private set; }

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

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
			_loadDeviceToken?.Cancel();
			_loadDeviceToken = new CancellationTokenSource(60000);
			CancellationToken deviceToken = CancellationTokenSource.CreateLinkedTokenSource(token, _loadDeviceToken.Token).Token;
			Task.Run(() => LoadDevicesAsync(deviceToken));
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
					FormExtentions.Initialize(DGrid, setting.FontProgram, true);
					((Control)lblDeviceName).Text = DeviceInfo.DeviceName;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 60);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 63);
		}
	}

	private async Task LoadDevicesAsync(CancellationToken token)
	{
		try
		{
			lstDeviceInfo = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			if (DeviceInfo != null)
			{
				lstDeviceInfo = lstDeviceInfo?.Where((DeviceInfo q) => q.Guid != DeviceInfo.Guid)?.ToList();
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
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						BindingSource obj = deviceInfoBindingSource;
						List<DeviceInfo> list = lstDeviceInfo;
						obj.DataSource = ((list != null) ? Utilities.ToSortableBindingList<DeviceInfo>((IEnumerable<DeviceInfo>)list, token) : null);
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex3)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex3, "", "LoadDevicesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 88);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDevicesAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 91);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 107);
		}
	}

	public frmDeviceSelect(string contextGuid, DeviceInfo device)
		: base(contextGuid)
	{
		frmDeviceSelect frmDeviceSelect2 = this;
		if (device == null)
		{
			throw new ArgumentNullException("Device is Null");
		}
		try
		{
			InitializeComponent();
			DeviceInfo = device;
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmDeviceSelect2.LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 123);
		}
	}

	private void frmDeviceSelect_KeyDown(object sender, KeyEventArgs e)
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
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmDeviceSelect_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 136);
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 145);
		}
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		Waiter waiter = null;
		try
		{
			_ = 7;
			try
			{
				List<DeviceInfo> selectedDevices = lstDeviceInfo?.Where((DeviceInfo q) => q.IsChecked)?.ToList();
				if (!(selectedDevices?.Any() ?? false) && DestinationDevice != null)
				{
					selectedDevices = new List<DeviceInfo> { DestinationDevice };
				}
				if (!(selectedDevices?.Any() ?? false))
				{
					await res.AddErrorAsync("لطفا دستگاه (های) مقصد را انتخاب نمایید", "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 158, (StringBuilder)null);
				}
				IEnumerable<BioDeviceRelationBusiness> sourceRelation = await BioDeviceRelationBusiness.GetAllAsync(((CacheBaseForm)this).ContextGuid, new BioDeviceRelationFilter
				{
					DeviceGuid = DeviceInfo.Guid
				}, default(CancellationToken), "");
				if (!(sourceRelation?.Any() ?? false))
				{
					await res.AddErrorAsync("اطلاعات از دستگاه مبدا دریافت نشد", "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 162, (StringBuilder)null);
				}
				if (res.HasError)
				{
					goto end_IL_0065;
				}
				waiter = new Waiter("درحال کپی کردن اطلاعات ...", (Form)(object)this, (Task)null, true, (EnWaiterType)0, (Control)null, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs");
				List<BioDeviceRelationBusiness> mappedData = await BioDeviceRelationMapper.Instance.MapListAsync(((CacheBaseForm)this).ContextGuid, sourceRelation, (List<BioDeviceRelationBusiness>)null, "", default(CancellationToken));
				foreach (DeviceInfo device in selectedDevices)
				{
					if (device == null)
					{
						continue;
					}
					DeviceServiceHandler instance = DeviceServiceHandler.GetInstance(((CacheBaseForm)this).ContextGuid, (CacheBaseForm)(object)this);
					ReturnedSaveFuncInfo val = res;
					val.AddReturnedValue(await instance.SendDeleteRequestToServiceAsync(((CacheBaseForm)this).ContextGuid, device.Guid), (ReturnedState)0, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 174);
					if (res.HasError)
					{
						continue;
					}
					foreach (BioDeviceRelationBusiness item in mappedData)
					{
						item.DeviceGuid = device.Guid;
					}
					val = res;
					val.AddReturnedValue(await BioDeviceRelationBusiness.SaveRangeAsync(((CacheBaseForm)this).ContextGuid, mappedData, UserAttendance.SelectedUser.Guid, (EnDeviceMessageType)0, default(CancellationToken), ""), (ReturnedState)0, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 179);
					val = res;
					val.AddReturnedValue(await HardwareCodeCus.CopyAsync(((CacheBaseForm)this).ContextGuid, DeviceInfo.Guid, device.Guid), (ReturnedState)0, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 180);
					mappedData = await BioDeviceRelationMapper.Instance.MapListAsync(((CacheBaseForm)this).ContextGuid, (IEnumerable<BioDeviceRelationBusiness>)mappedData, (List<BioDeviceRelationBusiness>)null, "", default(CancellationToken));
				}
				goto end_IL_003a;
				end_IL_0065:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 186);
				res.AddReturnedValue(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 187);
				goto end_IL_003a;
			}
			end_IL_003a:;
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
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<frmDeviceSelect>(this, res, "پیغام سیستم", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void deviceInfoBindingSource_CurrentChanged(object sender, EventArgs e)
	{
		try
		{
			DestinationDevice = (DeviceInfo)deviceInfoBindingSource.Current;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "deviceInfoBindingSource_CurrentChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 206);
		}
	}

	private void DGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		try
		{
			if (DestinationDevice == null || (int)((MouseEventArgs)e).Button == 2097152)
			{
				return;
			}
			DataGridView dGrid = DGrid;
			if (((dGrid != null) ? dGrid.CurrentCell : null) == null)
			{
				return;
			}
			DataGridView dGrid2 = DGrid;
			if (((dGrid2 != null) ? dGrid2.CurrentRow : null) != null)
			{
				DataGridView dGrid3 = DGrid;
				if (((dGrid3 != null) ? new int?(dGrid3.CurrentCell.ColumnIndex) : null) == ((DataGridViewBand)dgIsChecked).Index)
				{
					DestinationDevice.IsChecked = !DestinationDevice.IsChecked;
					((DataGridViewBand)DGrid.CurrentRow).DefaultCellStyle.BackColor = (DestinationDevice.IsChecked ? Color.LightBlue : Color.White);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellMouseClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 217);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode != 32 || DestinationDevice == null)
			{
				return;
			}
			DataGridView dGrid = DGrid;
			if (((dGrid != null) ? dGrid.CurrentCell : null) == null)
			{
				return;
			}
			DataGridView dGrid2 = DGrid;
			if (((dGrid2 != null) ? dGrid2.CurrentRow : null) != null)
			{
				DataGridView dGrid3 = DGrid;
				if (((dGrid3 != null) ? new int?(dGrid3.CurrentCell.ColumnIndex) : null) == ((DataGridViewBand)dgIsChecked).Index)
				{
					DestinationDevice.IsChecked = !DestinationDevice.IsChecked;
					((DataGridViewBand)DGrid.CurrentRow).DefaultCellStyle.BackColor = (DestinationDevice.IsChecked ? Color.LightBlue : Color.White);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\frmDeviceSelect.cs", 228);
		}
	}

	private void frmDeviceSelect_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
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
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0956: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Expected O, but got Unknown
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Expected O, but got Unknown
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		label1 = new Label();
		lblDeviceName = new Label();
		deviceInfoBindingSource = new BindingSource(components);
		DGrid = new DataGridView();
		DGID = new DataGridViewTextBoxColumn();
		dgIsChecked = new DataGridViewCheckBoxColumn();
		deviceNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		modelDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		TransferMode = new DataGridViewTextBoxColumn();
		SerialNumberStr = new DataGridViewTextBoxColumn();
		label2 = new Label();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		((ISupportInitialize)deviceInfoBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)label1).Location = new Point(641, 18);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(131, 29);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "[عنوان دستگاه مبدا:]";
		((Control)lblDeviceName).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblDeviceName).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)lblDeviceName).Location = new Point(74, 18);
		((Control)lblDeviceName).Name = "lblDeviceName";
		((Control)lblDeviceName).Size = new Size(561, 29);
		((Control)lblDeviceName).TabIndex = 2;
		((Control)lblDeviceName).Text = "---";
		deviceInfoBindingSource.DataSource = typeof(DeviceInfo);
		deviceInfoBindingSource.CurrentChanged += deviceInfoBindingSource_CurrentChanged;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		DGrid.AllowUserToOrderColumns = true;
		DGrid.AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)dgIsChecked,
			(DataGridViewColumn)deviceNoDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)modelDataGridViewTextBoxColumn,
			(DataGridViewColumn)TransferMode,
			(DataGridViewColumn)SerialNumberStr
		});
		DGrid.DataSource = deviceInfoBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		DGrid.DefaultCellStyle = val;
		DGrid.GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 95);
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersVisible = false;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(779, 416);
		((Control)DGrid).TabIndex = 8;
		DGrid.CellMouseClick += new DataGridViewCellMouseEventHandler(DGrid_CellMouseClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGID).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)dgIsChecked).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dgIsChecked).DataPropertyName = "IsChecked";
		((DataGridViewColumn)dgIsChecked).HeaderText = "";
		((DataGridViewColumn)dgIsChecked).MinimumWidth = 20;
		((DataGridViewColumn)dgIsChecked).Name = "dgIsChecked";
		((DataGridViewBand)dgIsChecked).ReadOnly = true;
		((DataGridViewBand)dgIsChecked).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)dgIsChecked).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)dgIsChecked).Width = 20;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).DataPropertyName = "DeviceNo";
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).HeaderText = "[شماره دستگاه]";
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).MinimumWidth = 110;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).Name = "deviceNoDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceNoDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)deviceNoDataGridViewTextBoxColumn).Width = 111;
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
		((DataGridViewColumn)modelDataGridViewTextBoxColumn).Width = 103;
		((DataGridViewColumn)TransferMode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)TransferMode).DataPropertyName = "TransferMode";
		((DataGridViewColumn)TransferMode).HeaderText = "[روش ارتباط]";
		((DataGridViewColumn)TransferMode).Name = "TransferMode";
		((DataGridViewBand)TransferMode).ReadOnly = true;
		((DataGridViewColumn)TransferMode).Width = 101;
		((DataGridViewColumn)SerialNumberStr).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)SerialNumberStr).DataPropertyName = "SerialNumberStr";
		((DataGridViewColumn)SerialNumberStr).HeaderText = "[شماره سریال]";
		((DataGridViewColumn)SerialNumberStr).MinimumWidth = 250;
		((DataGridViewColumn)SerialNumberStr).Name = "SerialNumberStr";
		((DataGridViewBand)SerialNumberStr).ReadOnly = true;
		((DataGridViewColumn)SerialNumberStr).Width = 250;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label2).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)label2).Location = new Point(602, 56);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(178, 29);
		((Control)label2).TabIndex = 2;
		((Control)label2).Text = "[انتخاب دستگاه (های) مقصد]";
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = (EnButtonCornerType)0;
		btnCancel.DisplayText = "[خروج (Esc)]";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(3, 521);
		((Control)btnCancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(135, 30);
		((Control)btnCancel).TabIndex = 9;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = (EnButtonType)0;
		btnCancel.OnClick += new OnClickEventHandler(btnCancel_OnClick);
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = (EnButtonCornerType)0;
		btnOk.DisplayText = "[تایید (F5)]";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(141, 521);
		((Control)btnOk).Margin = new Padding(4, 5, 4, 5);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(387, 30);
		((Control)btnOk).TabIndex = 9;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = (EnButtonType)1;
		btnOk.OnClick += new OnClickEventHandler(btnOk_OnClick);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)lblDeviceName);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Margin = new Padding(5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmDeviceSelect";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[کپی کردن اطلاعات ثبت نام در دستگاه ها]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmDeviceSelect_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmDeviceSelect_KeyDown);
		((ISupportInitialize)deviceInfoBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
