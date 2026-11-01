using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Mapper;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDevices.FLRHysoon;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.HardwareCode;

public class FrmDefinitionNewCode : BaseForm
{
	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _tokenLoadDate = new CancellationTokenSource();

	private List<Person_Hardwarecodes> _list;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private BindingSource bindingSourcePersonHardwares;

	private Label lblCount;

	private Label labelX3;

	internal DataGridViewX DGrid;

	private PanelEx panelEx2;

	private Label LblPersonFullName;

	private Label LblPersonFather;

	private Label labelX1;

	private Label labelX4;

	private DataGridViewTextBoxColumn Dg_DeviceName;

	private DataGridViewTextBoxColumn Dg_PersonNo;

	private DataGridViewTextBoxColumn Dg_PersonName;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private DataGridViewTextBoxColumn DG_DeviceId;

	private DataGridViewTextBoxColumn dgDeviceGuid;

	private DataGridViewTextBoxColumn dgPersonGuid;

	private DataGridViewTextBoxColumn DG_PersonId;

	private DataGridViewTextBoxColumn DGDeviceName;

	private DataGridViewTextBoxColumn DG_HardWareCode;

	private DataGridViewTextBoxColumn DG_PersonFullName;

	private DataGridViewTextBoxColumn DG_NameInDevice;

	private DataGridViewImageColumn Dg_InsertInDevice;

	private DataGridViewImageColumn Dg_InsertInTable;

	private DataGridViewImageColumn Dg_Delete;

	public TimeAttendanceDAL.Dal.FLRPerson.Person ClsPerson { get; private set; }

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

	public short SelectedDeviceId { get; private set; }

	public string SelectedHardwareCode { get; private set; }

	public string SelectedDeviceName { get; private set; }

	private async Task LoadDataDeviceAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<HardwareCodeCUSView> allHardwareCodesView = await HardwareCodeCUSView.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			List<HardwareCodeCUSView> personHCodes = allHardwareCodesView.Where((HardwareCodeCUSView p) => p.PersonId == ((PersonBase)ClsPerson).Id).ToList();
			List<DeviceInfo> list = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (HardwareCodeCUSView hcode in personHCodes)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				list.RemoveAll((DeviceInfo p) => p.Id == hcode.DeviceId);
			}
			if (list.Any())
			{
				foreach (DeviceInfo device in list)
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					int num = 0;
					IEnumerable<HardwareCodeCUSView> enumerable = allHardwareCodesView.Where((HardwareCodeCUSView p) => p.DeviceId == device.Id);
					if (enumerable != null && enumerable.Count() > 0)
					{
						num = enumerable.Max((HardwareCodeCUSView p) => int.Parse(p.HardwareCode));
					}
					personHCodes.Add(new HardwareCodeCUSView(((CacheBaseForm)this).ContextGuid)
					{
						DeviceId = device.Id,
						HardwareCode = (num + 1).ToString(),
						PersonId = ((PersonBase)ClsPerson).Id,
						DeviceName = device.DeviceName,
						DeviceGuid = device.Guid,
						PersonGuid = ((PersonBase)ClsPerson).Guid
					});
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_list = await HardwareCodeCusViewMapper.Instance.MapListAsync(((CacheBaseForm)this).ContextGuid, personHCodes, null, "", token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						bindingSourcePersonHardwares.Clear();
						bindingSourcePersonHardwares.DataSource = _list;
						((Control)lblCount).Text = bindingSourcePersonHardwares.Count.ToString();
						((Control)LblPersonFullName).Text = ((PersonBase)ClsPerson).FullName;
						((Control)LblPersonFather).Text = ((PersonBase)ClsPerson).FatherName;
						((DataGridViewBand)DG_HardWareCode).ReadOnly = false;
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
			Log.Error(ex, "");
		}
	}

	private void SetNameIntoDgrid(DeviceInfo selectDevice, string uname)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
			{
				DataGridViewRow val = item;
				if (val.Cells[((DataGridViewBand)Dg_DeviceName).Index].Value.ToString() == selectDevice.DeviceName)
				{
					val.Cells[((DataGridViewBand)Dg_PersonName).Index].Value = uname;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<string> ConnectAndGetUserNameFromDeviceAsync(DeviceInfo selectDevice, int hardwareCode)
	{
		try
		{
			if (selectDevice == null)
			{
				return null;
			}
			if ((int)selectDevice.TypeDeviceManufacture != 3)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "دستگاه " + selectDevice.DeviceName + " امکان درج کاربر از طریق نرم افزار را ندارد.", "", 3);
				return null;
			}
			clsHysoon clsHysoon = (((int)selectDevice.TransferMode == 1) ? new clsHysoon(((CacheBaseForm)this).ContextGuid, selectDevice.DeviceNo, int.Parse(selectDevice.CommunicationPass), selectDevice.Ip, selectDevice.TcpPort, getNewTaradodOnly: true) : new clsHysoon(((CacheBaseForm)this).ContextGuid, selectDevice.DeviceNo));
			if (!clsHysoon.Conected)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, "ارتباط با دستگاه " + selectDevice.DeviceName + " امکان پذیر نیست", "", 3);
				return null;
			}
			ReturnedSaveFuncInfoWithValue<string> userName = clsHysoon.GetUserName(hardwareCode);
			clsHysoon.Disconnect();
			if (((ReturnedSaveFuncInfo)userName).HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<FrmDefinitionNewCode>(this, (ReturnedSaveFuncInfo)(object)userName, "خطا", "ConnectAndGetUserNameFromDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs");
				return null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	private async Task<bool> ConnectAndSetUserNameToDeviceAsync(DeviceInfo selectDevice, int personNo, string userName)
	{
		try
		{
			if (selectDevice == null)
			{
				return false;
			}
			if ((int)selectDevice.TypeDeviceManufacture != 3)
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "دستگاه " + selectDevice.DeviceName + " امکان درج کاربر از طریق نرم افزار را ندارد.", "", 3);
				return false;
			}
			clsHysoon clsHysoon = (((int)selectDevice.TransferMode == 1) ? new clsHysoon(((CacheBaseForm)this).ContextGuid, selectDevice.DeviceNo, int.Parse(selectDevice.CommunicationPass), selectDevice.Ip, selectDevice.TcpPort, getNewTaradodOnly: true) : new clsHysoon(((CacheBaseForm)this).ContextGuid, selectDevice.DeviceNo));
			if (clsHysoon.Conected)
			{
				ReturnedSaveFuncInfoWithValue<bool> val = clsHysoon.SetUserName(personNo, userName?.Trim() ?? "");
				clsHysoon.Disconnect();
				if (((ReturnedSaveFuncInfo)val).HasError)
				{
					await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<FrmDefinitionNewCode>(this, (ReturnedSaveFuncInfo)(object)val, "خطا", "ConnectAndSetUserNameToDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs");
					return false;
				}
				if (val.value)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "کاربر (" + userName.Trim() + ") در دستگاه " + selectDevice.DeviceName + " درج شد", "", 3);
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	private async Task<DeviceInfo> ConnectAndGetDeviceAsync(int deviceId)
	{
		return null;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmDefinitionNewCode(string contextGuid, TimeAttendanceDAL.Dal.FLRPerson.Person person)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			ClsPerson = person;
			_tokenLoadDate = new CancellationTokenSource();
			CancellationToken token = _tokenLoadDate.Token;
			LoadDataDeviceAsync(token);
			((DataGridViewBand)DG_HardWareCode).ReadOnly = false;
			((DataGridViewBand)Dg_Delete).Visible = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs", 235);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		Waiter waiter = new Waiter("در حال پردازش", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs");
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 8;
			try
			{
				if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible || !(_list?.Any() ?? false) || ClsPerson == null)
				{
					goto end_IL_0082;
				}
				List<HardwareCodeCus> allHardwareCodes = await HardwareCodeCus.GetAllAsync(((CacheBaseForm)this).ContextGuid);
				List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid);
				foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
				{
					DataGridViewRow row = item;
					string hardwareCode = row.Cells[((DataGridViewBand)DG_HardWareCode).Index].Value.ToString();
					short personId = short.Parse(row.Cells[((DataGridViewBand)DG_PersonId).Index].Value.ToString());
					short deviceId = short.Parse(row.Cells[((DataGridViewBand)DG_DeviceId).Index].Value.ToString());
					Guid deviceGuid = (Guid)row.Cells[((DataGridViewBand)dgDeviceGuid).Index].Value;
					Guid personGuid = (Guid)row.Cells[((DataGridViewBand)dgPersonGuid).Index].Value;
					HardwareCodeCus exist = allHardwareCodes.SingleOrDefault((HardwareCodeCus p) => p.DeviceId == deviceId && p.HardwareCode == hardwareCode);
					if (exist == null)
					{
						await new HardwareCodeCus(((CacheBaseForm)this).ContextGuid)
						{
							HardwareCode = hardwareCode,
							PersonId = personId,
							DeviceId = deviceId
						}.SaveAsync();
						row.Cells[((DataGridViewBand)Dg_InsertInTable).Index].Value = Resources.check;
					}
					else
					{
						TimeAttendanceDAL.Dal.FLRPerson.Person person = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, exist.PersonId);
						if (exist.PersonId != ((PersonBase)ClsPerson).Id && (int)MessageBox.Show("قبلا کد سخت افزاری " + hardwareCode + " به شخصی با نام " + ((person != null) ? ((PersonBase)person).FullName : null) + " اختصاص یافته است\r\n آیا قصد جابجایی دارید؟", "هشدار", (MessageBoxButtons)4) == 6)
						{
							exist.HardwareCode = "0";
							await exist.SaveAsync();
							await new HardwareCodeCus(((CacheBaseForm)this).ContextGuid)
							{
								HardwareCode = hardwareCode,
								PersonId = personId,
								DeviceId = deviceId
							}.SaveAsync();
							row.Cells[((DataGridViewBand)Dg_InsertInTable).Index].Value = Resources.check;
						}
					}
					ReturnedSaveFuncInfo val = res;
					val.AddReturnedValue(await BiometricBusiness.ChangeHardwareAsync(((CacheBaseForm)this).ContextGuid, deviceGuid, (Guid?)personGuid, Utilities.ParseToInt(hardwareCode, 0), default(CancellationToken), ""), (ReturnedState)0, "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs", 294);
					DeviceInfo selectDevice = devices.SingleOrDefault((DeviceInfo p) => p.Id == deviceId);
					if (selectDevice == null)
					{
						MessageBox.Show($"دستگاهی با شناسه {deviceId} پیدا نشد.");
						continue;
					}
					if ((int)Utilities.GetDeviceGroup(selectDevice.Model) != 0)
					{
						string userName = row.Cells[((DataGridViewBand)DG_NameInDevice).Index].Value.ToString();
						if (await ConnectAndSetUserNameToDeviceAsync(selectDevice, int.Parse(hardwareCode), userName))
						{
							row.Cells[((DataGridViewBand)Dg_InsertInDevice).Index].Value = Resources.check;
							string value = await ConnectAndGetUserNameFromDeviceAsync(selectDevice, int.Parse(hardwareCode));
							row.Cells[((DataGridViewBand)DG_NameInDevice).Index].Value = value;
							continue;
						}
					}
					Application.DoEvents();
				}
				((Control)DGrid).Refresh();
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
				goto end_IL_0053;
				end_IL_0082:;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				goto end_IL_0053;
			}
			end_IL_0053:;
		}
		finally
		{
			if (res.HasError)
			{
				await AggregatorExtention.ShowReturnedSavedFuncInfoAsync<FrmDefinitionNewCode>(this, res, "پیغام سیستم", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs");
			}
			if (waiter != null)
			{
				waiter.Dispose();
			}
			BioWriteDeviceBusiness.ForceSendToService();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\HardwareCode\\FrmDefinitionNewCode.cs", 339);
		}
	}

	private void FrmDefinitionNewCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		Keys keyCode = e.KeyCode;
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

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (((DataGridView)DGrid).RowCount != 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				if (((DataGridView)DGrid).CurrentCell.ColumnIndex == ((DataGridViewBand)Dg_Delete).Index)
				{
					((DataGridView)DGrid).Rows.RemoveAt(((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index);
				}
				((Control)lblCount).Text = ((DataGridView)DGrid).Rows.Count.ToString();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
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
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Expected O, but got Unknown
		//IL_0bf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfe: Expected O, but got Unknown
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Expected O, but got Unknown
		//IL_0d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5e: Expected O, but got Unknown
		//IL_0df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Expected O, but got Unknown
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eac: Expected O, but got Unknown
		//IL_0f97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa1: Expected O, but got Unknown
		//IL_1496: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a0: Expected O, but got Unknown
		//IL_14da: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e4: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		label6 = new Label();
		label5 = new Label();
		label4 = new Label();
		label3 = new Label();
		label2 = new Label();
		label1 = new Label();
		lblCount = new Label();
		labelX3 = new Label();
		DGrid = new DataGridViewX();
		bindingSourcePersonHardwares = new BindingSource(components);
		panelEx2 = new PanelEx();
		LblPersonFullName = new Label();
		LblPersonFather = new Label();
		labelX1 = new Label();
		labelX4 = new Label();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		DG_DeviceId = new DataGridViewTextBoxColumn();
		dgDeviceGuid = new DataGridViewTextBoxColumn();
		dgPersonGuid = new DataGridViewTextBoxColumn();
		DG_PersonId = new DataGridViewTextBoxColumn();
		DGDeviceName = new DataGridViewTextBoxColumn();
		DG_HardWareCode = new DataGridViewTextBoxColumn();
		DG_PersonFullName = new DataGridViewTextBoxColumn();
		DG_NameInDevice = new DataGridViewTextBoxColumn();
		Dg_InsertInDevice = new DataGridViewImageColumn();
		Dg_InsertInTable = new DataGridViewImageColumn();
		Dg_Delete = new DataGridViewImageColumn();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)bindingSourcePersonHardwares).BeginInit();
		((Control)panelEx2).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)label6);
		((Control)panelEx1).Controls.Add((Control)(object)label5);
		((Control)panelEx1).Controls.Add((Control)(object)label4);
		((Control)panelEx1).Controls.Add((Control)(object)label3);
		((Control)panelEx1).Controls.Add((Control)(object)label2);
		((Control)panelEx1).Controls.Add((Control)(object)label1);
		((Control)panelEx1).Controls.Add((Control)(object)lblCount);
		((Control)panelEx1).Controls.Add((Control)(object)labelX3);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)panelEx2);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(994, 674);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)label6).Location = new Point(281, 571);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)1;
		((Control)label6).Size = new Size(698, 13);
		((Control)label6).TabIndex = 61;
		((Control)label6).Text = "5- در صورتیکه از قبل شخصی با کد شخص فعلی در دستگاه موجود باشد اطلاعات جاری جایگزین اطلاعات قبلی روی دستگاه خواهد شد ";
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)label5).Location = new Point(24, 546);
		((Control)label5).Name = "label5";
		((Control)label5).RightToLeft = (RightToLeft)1;
		((Control)label5).Size = new Size(956, 13);
		((Control)label5).TabIndex = 61;
		((Control)label5).Text = "4- با کلیک بر روی ثبت ابتدا کدهای شخص در نرم افزار درج می شود سپس در دستگاهها درج خواهند شد. اگر دستگاهی امکان ارتباط نداشته باشد درج روی آن دستگاه انجام نخواهد شد.";
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)label4).Location = new Point(217, 521);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)1;
		((Control)label4).Size = new Size(763, 13);
		((Control)label4).TabIndex = 61;
		((Control)label4).Text = "3- در صورتیکه تمایل ندارید شخصی در دستگاه ردیف مورد نظردرج شود با کلیک بر روی علامت حذف در ستون \"حذف ردیف\" آن ردیف حذف خواهد شد.";
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)label3).Location = new Point(350, 496);
		((Control)label3).Name = "label3";
		((Control)label3).RightToLeft = (RightToLeft)1;
		((Control)label3).Size = new Size(630, 13);
		((Control)label3).TabIndex = 61;
		((Control)label3).Text = "2- نام شخص در ستون \"نام شخص روی دستگاه\" بصورت پیش فرض پر شده است . در صورت نیاز می توانید آنرا تغییر دهید";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)label2).Location = new Point(6, 471);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(974, 13);
		((Control)label2).TabIndex = 61;
		((Control)label2).Text = "1- کد شخص در ستون \"کد شخص\" بصورت پیش فرض یک واحد بزرگتر از آخرین کد اختصاص داده شده در نرم افزار برای آن دستگاه می باشد، لیکن شما به صلاحدید می توانید آنرا تغییر دهید.";
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.Maroon;
		((Control)label1).Location = new Point(891, 446);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(89, 13);
		((Control)label1).TabIndex = 61;
		((Control)label1).Text = "نکات قابل توجه:";
		((Control)lblCount).ForeColor = Color.DarkRed;
		((Control)lblCount).Location = new Point(10, 419);
		((Control)lblCount).Name = "lblCount";
		((Control)lblCount).RightToLeft = (RightToLeft)1;
		((Control)lblCount).Size = new Size(87, 40);
		((Control)lblCount).TabIndex = 57;
		((Control)lblCount).Text = "---------";
		lblCount.TextAlign = (ContentAlignment)16;
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(103, 419);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(92, 40);
		((Control)labelX3).TabIndex = 56;
		((Control)labelX3).Text = "تعداد دستگاه:";
		labelX3.TextAlign = (ContentAlignment)64;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[11]
		{
			(DataGridViewColumn)DG_DeviceId,
			(DataGridViewColumn)dgDeviceGuid,
			(DataGridViewColumn)dgPersonGuid,
			(DataGridViewColumn)DG_PersonId,
			(DataGridViewColumn)DGDeviceName,
			(DataGridViewColumn)DG_HardWareCode,
			(DataGridViewColumn)DG_PersonFullName,
			(DataGridViewColumn)DG_NameInDevice,
			(DataGridViewColumn)Dg_InsertInDevice,
			(DataGridViewColumn)Dg_InsertInTable,
			(DataGridViewColumn)Dg_Delete
		});
		((DataGridView)DGrid).DataSource = bindingSourcePersonHardwares;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(13, 54);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((Control)DGrid).RightToLeft = (RightToLeft)1;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)0;
		((Control)DGrid).Size = new Size(967, 364);
		((Control)DGrid).TabIndex = 55;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		bindingSourcePersonHardwares.DataSource = typeof(Person_Hardwarecodes);
		((Control)panelEx2).Anchor = (AnchorStyles)13;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)LblPersonFullName);
		((Control)panelEx2).Controls.Add((Control)(object)LblPersonFather);
		((Control)panelEx2).Controls.Add((Control)(object)labelX1);
		((Control)panelEx2).Controls.Add((Control)(object)labelX4);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Location = new Point(12, 12);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(971, 32);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.CornerType = (eCornerType)2;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 54;
		((Control)LblPersonFullName).Anchor = (AnchorStyles)9;
		((Control)LblPersonFullName).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblPersonFullName).ForeColor = Color.DarkRed;
		((Control)LblPersonFullName).Location = new Point(711, 3);
		((Control)LblPersonFullName).Name = "LblPersonFullName";
		((Control)LblPersonFullName).RightToLeft = (RightToLeft)1;
		((Control)LblPersonFullName).Size = new Size(172, 23);
		((Control)LblPersonFullName).TabIndex = 3;
		((Control)LblPersonFullName).Text = "---------";
		LblPersonFullName.TextAlign = (ContentAlignment)16;
		((Control)LblPersonFather).Anchor = (AnchorStyles)13;
		((Control)LblPersonFather).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LblPersonFather).ForeColor = Color.DarkRed;
		((Control)LblPersonFather).Location = new Point(263, 3);
		((Control)LblPersonFather).Name = "LblPersonFather";
		((Control)LblPersonFather).RightToLeft = (RightToLeft)1;
		((Control)LblPersonFather).Size = new Size(392, 23);
		((Control)LblPersonFather).TabIndex = 2;
		((Control)LblPersonFather).Text = "---------";
		LblPersonFather.TextAlign = (ContentAlignment)16;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).Location = new Point(661, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(84, 23);
		((Control)labelX1).TabIndex = 1;
		((Control)labelX1).Text = "نام پدر:";
		labelX1.TextAlign = (ContentAlignment)64;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(889, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(79, 23);
		((Control)labelX4).TabIndex = 4;
		((Control)labelX4).Text = "نام شخص:";
		labelX4.TextAlign = (ContentAlignment)64;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(15, 630);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(153, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[خروج]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)10;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(174, 630);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(153, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((DataGridViewColumn)DG_DeviceId).DataPropertyName = "DeviceId";
		((DataGridViewColumn)DG_DeviceId).HeaderText = "DeviceId";
		((DataGridViewColumn)DG_DeviceId).Name = "DG_DeviceId";
		((DataGridViewBand)DG_DeviceId).ReadOnly = true;
		((DataGridViewBand)DG_DeviceId).Visible = false;
		((DataGridViewColumn)dgDeviceGuid).DataPropertyName = "DeviceGuid";
		((DataGridViewColumn)dgDeviceGuid).HeaderText = "DeviceGuid";
		((DataGridViewColumn)dgDeviceGuid).Name = "dgDeviceGuid";
		((DataGridViewBand)dgDeviceGuid).ReadOnly = true;
		((DataGridViewBand)dgDeviceGuid).Visible = false;
		((DataGridViewColumn)dgPersonGuid).DataPropertyName = "PersonGuid";
		((DataGridViewColumn)dgPersonGuid).HeaderText = "PersonGuid";
		((DataGridViewColumn)dgPersonGuid).Name = "dgPersonGuid";
		((DataGridViewBand)dgPersonGuid).ReadOnly = true;
		((DataGridViewBand)dgPersonGuid).Visible = false;
		((DataGridViewColumn)DG_PersonId).DataPropertyName = "PersonId";
		((DataGridViewColumn)DG_PersonId).HeaderText = "PersonId";
		((DataGridViewColumn)DG_PersonId).Name = "DG_PersonId";
		((DataGridViewBand)DG_PersonId).ReadOnly = true;
		((DataGridViewBand)DG_PersonId).Visible = false;
		((DataGridViewColumn)DGDeviceName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDeviceName).DataPropertyName = "DeviceName";
		((DataGridViewColumn)DGDeviceName).HeaderText = "نام دستگاه";
		((DataGridViewColumn)DGDeviceName).MinimumWidth = 200;
		((DataGridViewColumn)DGDeviceName).Name = "DGDeviceName";
		((DataGridViewBand)DGDeviceName).ReadOnly = true;
		((DataGridViewColumn)DG_HardWareCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_HardWareCode).DataPropertyName = "HardWareCode";
		((DataGridViewColumn)DG_HardWareCode).HeaderText = "کد شخص *";
		((DataGridViewColumn)DG_HardWareCode).Name = "DG_HardWareCode";
		((DataGridViewBand)DG_HardWareCode).ReadOnly = true;
		((DataGridViewColumn)DG_HardWareCode).Width = 93;
		((DataGridViewColumn)DG_PersonFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_PersonFullName).DataPropertyName = "PersonFullName";
		((DataGridViewColumn)DG_PersonFullName).HeaderText = "نام شخص در نرم افزار";
		((DataGridViewColumn)DG_PersonFullName).MinimumWidth = 200;
		((DataGridViewColumn)DG_PersonFullName).Name = "DG_PersonFullName";
		((DataGridViewBand)DG_PersonFullName).ReadOnly = true;
		((DataGridViewColumn)DG_PersonFullName).Width = 200;
		((DataGridViewColumn)DG_NameInDevice).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_NameInDevice).DataPropertyName = "NameInDevice";
		((DataGridViewColumn)DG_NameInDevice).HeaderText = "نام شخص روی دستگاه *";
		((DataGridViewColumn)DG_NameInDevice).MinimumWidth = 200;
		((DataGridViewColumn)DG_NameInDevice).Name = "DG_NameInDevice";
		((DataGridViewBand)DG_NameInDevice).ReadOnly = true;
		((DataGridViewColumn)DG_NameInDevice).Width = 200;
		((DataGridViewColumn)Dg_InsertInDevice).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)Dg_InsertInDevice).DataPropertyName = "InsertInDevice";
		((DataGridViewColumn)Dg_InsertInDevice).HeaderText = "درج در دستگاه";
		Dg_InsertInDevice.Image = (Image)(object)Resources.question;
		Dg_InsertInDevice.ImageLayout = (DataGridViewImageCellLayout)3;
		((DataGridViewColumn)Dg_InsertInDevice).MinimumWidth = 60;
		((DataGridViewColumn)Dg_InsertInDevice).Name = "Dg_InsertInDevice";
		((DataGridViewBand)Dg_InsertInDevice).ReadOnly = true;
		((DataGridViewColumn)Dg_InsertInDevice).Width = 60;
		((DataGridViewColumn)Dg_InsertInTable).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)Dg_InsertInTable).DataPropertyName = "InsertInTableImage";
		((DataGridViewColumn)Dg_InsertInTable).HeaderText = "درج در نرم افزار";
		Dg_InsertInTable.Image = (Image)(object)Resources.question;
		Dg_InsertInTable.ImageLayout = (DataGridViewImageCellLayout)3;
		((DataGridViewColumn)Dg_InsertInTable).MinimumWidth = 60;
		((DataGridViewColumn)Dg_InsertInTable).Name = "Dg_InsertInTable";
		((DataGridViewBand)Dg_InsertInTable).ReadOnly = true;
		((DataGridViewColumn)Dg_InsertInTable).Width = 60;
		((DataGridViewColumn)Dg_Delete).AutoSizeMode = (DataGridViewAutoSizeColumnMode)4;
		((DataGridViewColumn)Dg_Delete).HeaderText = "حذف ردیف";
		Dg_Delete.Image = (Image)(object)Resources.edit_delete;
		((DataGridViewColumn)Dg_Delete).MinimumWidth = 40;
		((DataGridViewColumn)Dg_Delete).Name = "Dg_Delete";
		((DataGridViewBand)Dg_Delete).ReadOnly = true;
		((DataGridViewColumn)Dg_Delete).Width = 40;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(994, 674);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmDefinitionNewCode";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "انتساب کد سخت افزاری جدید و ذخیره در دستگاه";
		((Control)this).KeyDown += new KeyEventHandler(FrmDefinitionNewCode_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx1).PerformLayout();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)bindingSourcePersonHardwares).EndInit();
		((Control)panelEx2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
