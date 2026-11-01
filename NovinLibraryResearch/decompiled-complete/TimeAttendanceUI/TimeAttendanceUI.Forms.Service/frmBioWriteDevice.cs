using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.UC;
using FormCustom.Waiting;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceUI.Forms.Service;

public class frmBioWriteDevice : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _initToken;

	private IContainer components;

	private BindingSource WriteDeviceBindingSource;

	private DataGridView DGrid;

	private DataGridViewTextBoxColumn personNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn hardwareCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn harwareNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn insertedDateShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn tryCountDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nextTryShDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fingerPrintCountDataGridViewTextBoxColumn;

	private DataGridViewCheckBoxColumn hasPalmDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn hasFaceDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn hasCardDataGridViewCheckBoxColumn;

	private DataGridViewCheckBoxColumn hasPasswordDataGridViewCheckBoxColumn;

	private DataGridViewTextBoxColumn bioGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceGroupDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn insertedDateMDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn nextTryDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn userGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn deviceGroupForDisplayDataGridViewTextBoxColumn;

	private UcButtons btnCancel;

	private UcButtons btnOk;

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

	private async Task InitializeAsync(CancellationToken token)
	{
		try
		{
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid, token);
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
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 45);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 48);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			IEnumerable<BioWriteDeviceModel> list = await BioWriteDeviceBusiness.GetAllAync(((CacheBaseForm)this).ContextGuid, token, "");
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
					BindingSource writeDeviceBindingSource = WriteDeviceBindingSource;
					IEnumerable<BioWriteDeviceModel> enumerable = list;
					object dataSource;
					if (enumerable == null)
					{
						dataSource = null;
					}
					else
					{
						IOrderedEnumerable<BioWriteDeviceModel> orderedEnumerable = enumerable.OrderBy((BioWriteDeviceModel q) => q.InsertedDateM);
						dataSource = ((orderedEnumerable != null) ? Utilities.ToSortableBindingList<BioWriteDeviceModel>((IEnumerable<BioWriteDeviceModel>)orderedEnumerable, token) : null);
					}
					writeDeviceBindingSource.DataSource = dataSource;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 69);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 72);
		}
	}

	private async Task BioWriteDevice_OnSaveResult(object sender, SaveResponse<BioWriteDeviceCompositeKey> e)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			await Task.Delay(1000);
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				Task.Run(() => LoadDataAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BioWriteDevice_OnSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 90);
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
				_initToken?.Cancel();
				_initToken?.Dispose();
				_initToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 110);
		}
	}

	public frmBioWriteDevice(string contextGuid)
		: base(contextGuid)
	{
		frmBioWriteDevice frmBioWriteDevice2 = this;
		try
		{
			InitializeComponent();
			_initToken = new CancellationTokenSource(60000);
			CancellationToken initToken = _initToken.Token;
			Task.Run(() => frmBioWriteDevice2.InitializeAsync(initToken));
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => frmBioWriteDevice2.LoadDataAsync(token));
			((CacheBaseForm)this).Cache.BusinessAggregator.BioWriteDevice.OnSaveResult -= BioWriteDevice_OnSaveResult;
			((CacheBaseForm)this).Cache.BusinessAggregator.BioWriteDevice.OnSaveResult += BioWriteDevice_OnSaveResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 130);
		}
	}

	private void frmBioWriteDevice_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 141);
		}
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		Waiter waiter = new Waiter("در حال ارسال مجدد ...", (Form)(object)this, (Task)null, false, (EnWaiterType)0, (Control)null, "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs");
		try
		{
			BioWriteDeviceBusiness.ForceSendToService();
			await Task.Delay(1000);
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource(60000);
			CancellationToken token = _loadDataToken.Token;
			Task.Run(() => LoadDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 157);
		}
		finally
		{
			((IDisposable)waiter)?.Dispose();
		}
	}

	private void frmBioWriteDevice_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource(60000);
					CancellationToken token = _loadDataToken.Token;
					Task.Run(() => LoadDataAsync(token));
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBioWriteDevice_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmBioWriteDevice.cs", 174);
		}
	}

	private void InitializeComponent()
	{
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
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
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
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Expected O, but got Unknown
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Expected O, but got Unknown
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Expected O, but got Unknown
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Expected O, but got Unknown
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Expected O, but got Unknown
		//IL_0b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Expected O, but got Unknown
		//IL_0c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2f: Expected O, but got Unknown
		components = new Container();
		WriteDeviceBindingSource = new BindingSource(components);
		DGrid = new DataGridView();
		personNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hardwareCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		harwareNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		insertedDateShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		tryCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nextTryShDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		messageDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fingerPrintCountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		hasPalmDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		hasFaceDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		hasCardDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		hasPasswordDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
		bioGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceGroupDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		insertedDateMDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		nextTryDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		userGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		deviceGroupForDisplayDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		((ISupportInitialize)WriteDeviceBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		WriteDeviceBindingSource.DataSource = typeof(BioWriteDeviceModel);
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.AutoGenerateColumns = false;
		DGrid.BackgroundColor = SystemColors.ControlLightLight;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[20]
		{
			(DataGridViewColumn)personNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)harwareNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn,
			(DataGridViewColumn)tryCountDataGridViewTextBoxColumn,
			(DataGridViewColumn)nextTryShDataGridViewTextBoxColumn,
			(DataGridViewColumn)messageDataGridViewTextBoxColumn,
			(DataGridViewColumn)fingerPrintCountDataGridViewTextBoxColumn,
			(DataGridViewColumn)hasPalmDataGridViewCheckBoxColumn,
			(DataGridViewColumn)hasFaceDataGridViewCheckBoxColumn,
			(DataGridViewColumn)hasCardDataGridViewCheckBoxColumn,
			(DataGridViewColumn)hasPasswordDataGridViewCheckBoxColumn,
			(DataGridViewColumn)bioGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceGroupDataGridViewTextBoxColumn,
			(DataGridViewColumn)insertedDateMDataGridViewTextBoxColumn,
			(DataGridViewColumn)nextTryDataGridViewTextBoxColumn,
			(DataGridViewColumn)userGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)deviceGroupForDisplayDataGridViewTextBoxColumn
		});
		DGrid.DataSource = WriteDeviceBindingSource;
		((Control)DGrid).Location = new Point(0, 2);
		DGrid.MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersWidth = 62;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(784, 515);
		((Control)DGrid).TabIndex = 21;
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).DataPropertyName = "PersonName";
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).HeaderText = "[نام شخص]";
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).Name = "personNameDataGridViewTextBoxColumn";
		((DataGridViewBand)personNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)personNameDataGridViewTextBoxColumn).Width = 91;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).DataPropertyName = "DeviceName";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).HeaderText = "[دستگاه]";
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).Name = "deviceNameDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)deviceNameDataGridViewTextBoxColumn).Width = 79;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).DataPropertyName = "HardwareCode";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).HeaderText = "[کد سخت افزاری]";
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Name = "hardwareCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)hardwareCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)hardwareCodeDataGridViewTextBoxColumn).Width = 123;
		((DataGridViewColumn)harwareNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)harwareNameDataGridViewTextBoxColumn).DataPropertyName = "HarwareName";
		((DataGridViewColumn)harwareNameDataGridViewTextBoxColumn).HeaderText = "[نام سخت افزاری]";
		((DataGridViewColumn)harwareNameDataGridViewTextBoxColumn).Name = "harwareNameDataGridViewTextBoxColumn";
		((DataGridViewBand)harwareNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)harwareNameDataGridViewTextBoxColumn).Width = 125;
		((DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn).DataPropertyName = "InsertedDateSh";
		((DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn).HeaderText = "[تاریخ درج]";
		((DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn).Name = "insertedDateShDataGridViewTextBoxColumn";
		((DataGridViewBand)insertedDateShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)insertedDateShDataGridViewTextBoxColumn).Width = 90;
		((DataGridViewColumn)tryCountDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)tryCountDataGridViewTextBoxColumn).DataPropertyName = "TryCount";
		((DataGridViewColumn)tryCountDataGridViewTextBoxColumn).HeaderText = "[تعداد تلاش]";
		((DataGridViewColumn)tryCountDataGridViewTextBoxColumn).Name = "tryCountDataGridViewTextBoxColumn";
		((DataGridViewBand)tryCountDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)tryCountDataGridViewTextBoxColumn).Width = 98;
		((DataGridViewColumn)nextTryShDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)nextTryShDataGridViewTextBoxColumn).DataPropertyName = "NextTrySh";
		((DataGridViewColumn)nextTryShDataGridViewTextBoxColumn).HeaderText = "[زمان ارسال بعدی]";
		((DataGridViewColumn)nextTryShDataGridViewTextBoxColumn).Name = "nextTryShDataGridViewTextBoxColumn";
		((DataGridViewBand)nextTryShDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)nextTryShDataGridViewTextBoxColumn).Width = 127;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).DataPropertyName = "Message";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).HeaderText = "[شرح]";
		((DataGridViewColumn)messageDataGridViewTextBoxColumn).Name = "messageDataGridViewTextBoxColumn";
		((DataGridViewBand)messageDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)fingerPrintCountDataGridViewTextBoxColumn).DataPropertyName = "FingerPrintCount";
		((DataGridViewColumn)fingerPrintCountDataGridViewTextBoxColumn).HeaderText = "FingerPrintCount";
		((DataGridViewColumn)fingerPrintCountDataGridViewTextBoxColumn).Name = "fingerPrintCountDataGridViewTextBoxColumn";
		((DataGridViewBand)fingerPrintCountDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)fingerPrintCountDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)hasPalmDataGridViewCheckBoxColumn).DataPropertyName = "HasPalm";
		((DataGridViewColumn)hasPalmDataGridViewCheckBoxColumn).HeaderText = "HasPalm";
		((DataGridViewColumn)hasPalmDataGridViewCheckBoxColumn).Name = "hasPalmDataGridViewCheckBoxColumn";
		((DataGridViewBand)hasPalmDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)hasPalmDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)hasFaceDataGridViewCheckBoxColumn).DataPropertyName = "HasFace";
		((DataGridViewColumn)hasFaceDataGridViewCheckBoxColumn).HeaderText = "HasFace";
		((DataGridViewColumn)hasFaceDataGridViewCheckBoxColumn).Name = "hasFaceDataGridViewCheckBoxColumn";
		((DataGridViewBand)hasFaceDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)hasFaceDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)hasCardDataGridViewCheckBoxColumn).DataPropertyName = "HasCard";
		((DataGridViewColumn)hasCardDataGridViewCheckBoxColumn).HeaderText = "HasCard";
		((DataGridViewColumn)hasCardDataGridViewCheckBoxColumn).Name = "hasCardDataGridViewCheckBoxColumn";
		((DataGridViewBand)hasCardDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)hasCardDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)hasPasswordDataGridViewCheckBoxColumn).DataPropertyName = "HasPassword";
		((DataGridViewColumn)hasPasswordDataGridViewCheckBoxColumn).HeaderText = "HasPassword";
		((DataGridViewColumn)hasPasswordDataGridViewCheckBoxColumn).Name = "hasPasswordDataGridViewCheckBoxColumn";
		((DataGridViewBand)hasPasswordDataGridViewCheckBoxColumn).ReadOnly = true;
		((DataGridViewBand)hasPasswordDataGridViewCheckBoxColumn).Visible = false;
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).DataPropertyName = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).HeaderText = "BioGuid";
		((DataGridViewColumn)bioGuidDataGridViewTextBoxColumn).Name = "bioGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)bioGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).DataPropertyName = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).HeaderText = "DeviceGuid";
		((DataGridViewColumn)deviceGuidDataGridViewTextBoxColumn).Name = "deviceGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deviceGroupDataGridViewTextBoxColumn).DataPropertyName = "DeviceGroup";
		((DataGridViewColumn)deviceGroupDataGridViewTextBoxColumn).HeaderText = "DeviceGroup";
		((DataGridViewColumn)deviceGroupDataGridViewTextBoxColumn).Name = "deviceGroupDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceGroupDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceGroupDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)insertedDateMDataGridViewTextBoxColumn).DataPropertyName = "InsertedDateM";
		((DataGridViewColumn)insertedDateMDataGridViewTextBoxColumn).HeaderText = "InsertedDateM";
		((DataGridViewColumn)insertedDateMDataGridViewTextBoxColumn).Name = "insertedDateMDataGridViewTextBoxColumn";
		((DataGridViewBand)insertedDateMDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)insertedDateMDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)nextTryDataGridViewTextBoxColumn).DataPropertyName = "NextTry";
		((DataGridViewColumn)nextTryDataGridViewTextBoxColumn).HeaderText = "NextTry";
		((DataGridViewColumn)nextTryDataGridViewTextBoxColumn).Name = "nextTryDataGridViewTextBoxColumn";
		((DataGridViewBand)nextTryDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)nextTryDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).DataPropertyName = "UserGuid";
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).HeaderText = "UserGuid";
		((DataGridViewColumn)userGuidDataGridViewTextBoxColumn).Name = "userGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)userGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)userGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)deviceGroupForDisplayDataGridViewTextBoxColumn).DataPropertyName = "DeviceGroupForDisplay";
		((DataGridViewColumn)deviceGroupForDisplayDataGridViewTextBoxColumn).HeaderText = "DeviceGroupForDisplay";
		((DataGridViewColumn)deviceGroupForDisplayDataGridViewTextBoxColumn).Name = "deviceGroupForDisplayDataGridViewTextBoxColumn";
		((DataGridViewBand)deviceGroupForDisplayDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)deviceGroupForDisplayDataGridViewTextBoxColumn).Visible = false;
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = (EnButtonCornerType)0;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(12, 525);
		((Control)btnCancel).Margin = new Padding(5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(145, 30);
		((Control)btnCancel).TabIndex = 23;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = (EnButtonType)0;
		btnCancel.OnClick += new OnClickEventHandler(btnCancel_OnClick);
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = (EnButtonCornerType)0;
		btnOk.DisplayText = "[ارسال مجدد]";
		((Control)btnOk).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(166, 525);
		((Control)btnOk).Margin = new Padding(5);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(254, 30);
		((Control)btnOk).TabIndex = 22;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = (EnButtonType)1;
		btnOk.OnClick += new OnClickEventHandler(btnOk_OnClick);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmBioWriteDevice";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[لیست دستورات دستگاه ها]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).FormClosed += new FormClosedEventHandler(frmBioWriteDevice_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmBioWriteDevice_KeyDown);
		((ISupportInitialize)WriteDeviceBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
