using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxSBXPCLib;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Attendance.SDK.LAN_VisibleLight;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDevices.FLREbio;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.UC.Base;

namespace TimeAttendanceUI.Forms.Device;

public class FrmDevice : BaseForm
{
	private DeviceInfo _clsDeviceInfo;

	private int _id;

	private string frm_Text = "[دستگاه]";

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _SaveToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _GetTaradodToken;

	public AxSBXPC bpc;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private LabelX labelX5;

	private LabelX labelX1;

	private TextBoxX TxtName;

	private PanelEx PanelTcp;

	private TextBoxX TxtPort;

	private LabelX labelX4;

	private TextBoxX TxtIP;

	private LabelX labelX3;

	private TextBoxX TxtPass;

	private RadioButton RbtnLan;

	private RadioButton RbtnUsb;

	private TextBoxX TxtDesc;

	private LabelX labelX9;

	private ButtonX BtnTestConnection;

	private NumericUpDown NumCount;

	private LabelX labelX8;

	private CheckBoxX ChbAutoRecieve;

	private NumericUpDown numCodeDevice;

	private FlowLayoutPanel flowLayoutPanel1;

	private ButtonX BtnRecieve;

	private UCDeviceImg ucDIEC160;

	private UCDeviceImg ucDIETC132;

	private UCDeviceImg ucDIN1;

	private UCDeviceImg ucDIRN1;

	private UCDeviceImg ucDINP2815;

	private UCDeviceImg ucDINP302;

	private UCDeviceImg ucDINP395;

	private UCDeviceImg ucDINP586;

	private UCDeviceImg ucDIZKT;

	private ButtonX BtnAccept;

	private UCDeviceImg ucDIRETC132;

	private UCDeviceImg ucDINP261;

	private CheckBoxX chBxPassword;

	private CheckBoxX chbAutoSetDateTime;

	private LabelX lblPortDesc;

	private CheckBoxX ChbNewTaradodOnly;

	private RadioButton RbtnWan;

	private TextBoxX TbxSerial;

	private LabelX labelX10;

	private UCDeviceImg ucDINP587;

	private UCDeviceImg ucNP255;

	private ButtonX BtnManageEnroll;

	private UCDeviceImg ucNP290;

	private UCDeviceImg ucNP550;

	private TextBoxX TbxPortRealTime;

	private LabelX labelX2;

	private CheckBoxX ChkRealTimeDevice;

	private UCDeviceImg ucNP770;

	private UCDeviceImg ucNP756P;

	private UCDeviceImg ucNP761P;

	private PanelEx pnlColor;

	private ColorDialog colorDialog1;

	private LabelX LblCallBack;

	private ComboBox CmbCallBack;

	private PictureBox infoBoxCallBack;

	private PictureBox pictureBoxLoading;

	private PictureBox PbColorTaradod;

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

	public FrmDevice(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			frm_Text = "[افزودن دستگاه جدید]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 43);
		}
	}

	public FrmDevice(string contextGuid, int id)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			_id = id;
			frm_Text = "[ویرایش دستگاه]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 55);
		}
	}

	public FrmDevice(string contextGuid, DeviceInfo pDeviceInfo)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			InitializeComponent();
			_clsDeviceInfo = pDeviceInfo;
			((Control)BtnAccept).Enabled = false;
			((Control)BtnRecieve).Enabled = true;
			frm_Text = "[مشاهده لاگ عملکرد کاربران ( قفط خواندنی ) اطلاعات دستگاه]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 72);
		}
	}

	public void CreateBpc()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			bpc = new AxSBXPC();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmDevice));
			((ISupportInitialize)bpc).BeginInit();
			((AxHost)bpc).OcxState = (State)componentResourceManager.GetObject("bpc.OcxState");
			((AxHost)bpc).Enabled = true;
			((Control)bpc).Location = new Point(10, 9);
			((Control)bpc).Name = "bpc";
			((Control)bpc).Size = new Size(100, 50);
			((Control)bpc).TabIndex = 37;
			((Control)this).Controls.Add((Control)(object)bpc);
			((ISupportInitialize)bpc).EndInit();
			bpc.DotNET();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateBpc", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 100);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			CreateBpc();
			if (_clsDeviceInfo == null)
			{
				if (_id > 0)
				{
					_clsDeviceInfo = await DeviceInfo.GetByIdAsync(((CacheBaseForm)this).ContextGuid, _id, token);
				}
				else
				{
					_clsDeviceInfo = new DeviceInfo(((CacheBaseForm)this).ContextGuid);
					DeviceInfo clsDeviceInfo = _clsDeviceInfo;
					clsDeviceInfo.DeviceNo = (short)(await _clsDeviceInfo.LastDeviceNoAsync(token));
					_clsDeviceInfo.DeviceName = $"دستگاه {_clsDeviceInfo.DeviceNo}";
				}
			}
			((Control)numCodeDevice).Focus();
			FillForm();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FillForm()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected I4, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected I4, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected I4, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected I4, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Invalid comparison between Unknown and I4
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Invalid comparison between Unknown and I4
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Invalid comparison between Unknown and I4
		try
		{
			((Control)TxtName).Text = _clsDeviceInfo.DeviceName;
			((Control)TbxSerial).Text = _clsDeviceInfo.SerialNumberStr;
			numCodeDevice.Value = 1m;
			if (_clsDeviceInfo.DeviceNo > 1)
			{
				numCodeDevice.Value = _clsDeviceInfo.DeviceNo;
			}
			((Control)TxtDesc).Text = _clsDeviceInfo.Desc;
			((Control)BtnRecieve).Enabled = false;
			((Control)BtnTestConnection).Enabled = false;
			((Control)PbColorTaradod).BackColor = ColorTranslator.FromHtml(_clsDeviceInfo.DeviceColor);
			ActiveCmbCallBackIps(visible: false);
			LoadCmbCallBackIps();
			TypeConnection transferMode = _clsDeviceInfo.TransferMode;
			switch ((int)transferMode)
			{
			case 1:
				RbtnLan.Checked = true;
				ChkRealTimeDevice.Checked = _clsDeviceInfo.UseRealTime;
				((Control)BtnRecieve).Enabled = true;
				((Control)BtnTestConnection).Enabled = true;
				break;
			case 0:
				RbtnWan.Checked = true;
				((Control)BtnTestConnection).Enabled = false;
				((Control)BtnRecieve).Enabled = false;
				break;
			case 2:
				RbtnUsb.Checked = true;
				((Control)BtnRecieve).Enabled = true;
				((Control)BtnTestConnection).Enabled = true;
				break;
			}
			DeviceModelEnumType model = _clsDeviceInfo.Model;
			if ((int)model <= 9102)
			{
				switch (model - 9001)
				{
				case 0:
					ucDIEC160.SetCheckRDBTN(sw: true);
					return;
				case 1:
					ucDIETC132.SetCheckRDBTN(sw: true);
					return;
				case 2:
					ucDIN1.SetCheckRDBTN(sw: true);
					return;
				}
				if ((int)model != 9101)
				{
					if ((int)model == 9102)
					{
						ucDIRN1.SetCheckRDBTN(sw: true);
					}
				}
				else
				{
					ucDIRETC132.SetCheckRDBTN(sw: true);
				}
				return;
			}
			switch (model - 9201)
			{
			case 0:
				ucDINP2815.SetCheckRDBTN(sw: true);
				return;
			case 1:
				ucDINP302.SetCheckRDBTN(sw: true);
				return;
			case 2:
				ucDINP261.SetCheckRDBTN(sw: true);
				return;
			case 3:
				ucNP550.SetCheckRDBTN(sw: true);
				return;
			}
			switch (model - 9301)
			{
			case 0:
				ucDINP395.SetCheckRDBTN(sw: true);
				return;
			case 1:
				ucDINP586.SetCheckRDBTN(sw: true);
				return;
			case 3:
				ucDINP587.SetCheckRDBTN(sw: true);
				return;
			case 4:
				ucNP255.SetCheckRDBTN(sw: true);
				return;
			case 5:
				ucNP290.SetCheckRDBTN(sw: true);
				return;
			case 6:
				ucNP770.SetCheckRDBTN(sw: true);
				return;
			case 7:
				ucNP756P.SetCheckRDBTN(sw: true);
				return;
			case 8:
				ucNP761P.SetCheckRDBTN(sw: true);
				ActiveCmbCallBackIps(visible: true);
				return;
			case 2:
				return;
			}
			if ((int)model == 9400)
			{
				ucDIZKT.SetCheckRDBTN(sw: true);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task<ReturnedSaveFuncInfo> ValidateFormAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			DeviceInfo deviceInfo = await DeviceInfo.GetByDeviceNoAsync(((CacheBaseForm)this).ContextGuid, (short)numCodeDevice.Value, token);
			if (deviceInfo != null && (_clsDeviceInfo.Id == 0 || deviceInfo.Id != _clsDeviceInfo.Id))
			{
				res.AddError((EnSystem)2, "[کد دستگاه تکراری یا نامعتبر است]", "", "ValidateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 235, (StringBuilder)null);
			}
			if (RbtnLan.Checked && string.IsNullOrEmpty(((Control)TxtIP).Text.Trim()))
			{
				res.AddError((EnSystem)2, "[فیلد آی پی نباید خالی باشد]", "", "ValidateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 240, (StringBuilder)null);
			}
			if (((Control)TbxSerial).Text.Length < 5)
			{
				res.AddError((EnSystem)2, "[سریال دستگاه نامعتبر است]", "", "ValidateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 248, (StringBuilder)null);
			}
			if (ChkRealTimeDevice.Checked && string.IsNullOrEmpty(((Control)TbxPortRealTime).Text))
			{
				res.AddError((EnSystem)2, "پورت tcp را مشخص نمائید", "", "ValidateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 252, (StringBuilder)null);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ValidateFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 257);
		}
		return res;
	}

	private void FillClsDevice()
	{
		try
		{
			_clsDeviceInfo.DeviceNo = (short)(string.IsNullOrEmpty(numCodeDevice.Value.ToString()) ? 1 : short.Parse(numCodeDevice.Value.ToString()));
			_clsDeviceInfo.DeviceName = Text.FixString(((Control)TxtName).Text.Trim());
			if (_clsDeviceInfo.DeviceName == "")
			{
				_clsDeviceInfo.DeviceName = "device_" + _clsDeviceInfo.DeviceNo;
			}
			_clsDeviceInfo.Desc = ((((Control)TxtDesc).Text == "") ? " " : ((Control)TxtDesc).Text);
			_clsDeviceInfo.SerialNumberStr = ((Control)TbxSerial).Text.Trim();
			_clsDeviceInfo.DeviceColor = ((Control)PbColorTaradod).BackColor.Name;
			if (_clsDeviceInfo.DeviceColor.Any(char.IsDigit))
			{
				_clsDeviceInfo.DeviceColor = "#" + _clsDeviceInfo.DeviceColor;
			}
			DeviceInfo clsDeviceInfo = _clsDeviceInfo;
			ComboBox cmbCallBack = CmbCallBack;
			clsDeviceInfo.CallBackIp = ((cmbCallBack == null) ? null : cmbCallBack.SelectedItem?.ToString());
			ComboBox cmbCallBack2 = CmbCallBack;
			if (string.IsNullOrEmpty((cmbCallBack2 == null) ? null : cmbCallBack2.SelectedItem?.ToString()))
			{
				_clsDeviceInfo.CallBackIp = ((Control)CmbCallBack).Text?.Trim();
			}
			if (RbtnUsb.Checked)
			{
				_clsDeviceInfo.TransferMode = (TypeConnection)2;
				_clsDeviceInfo.CommunicationPass = (chBxPassword.Checked ? ((Control)TxtPass).Text : "0");
			}
			else if (RbtnLan.Checked)
			{
				_clsDeviceInfo.TransferMode = (TypeConnection)1;
				_clsDeviceInfo.Ip = ((Control)TxtIP).Text;
				_clsDeviceInfo.TcpPort = int.Parse(((Control)TxtPort).Text);
				_clsDeviceInfo.PortRealtime = int.Parse(((Control)TbxPortRealTime).Text);
				_clsDeviceInfo.CommunicationPass = (chBxPassword.Checked ? ((Control)TxtPass).Text : "0");
				_clsDeviceInfo.MinCheck = int.Parse(NumCount.Value.ToString());
				_clsDeviceInfo.AutoRecieve = ChbAutoRecieve.Checked;
				_clsDeviceInfo.UseRealTime = ChkRealTimeDevice.Checked;
				_clsDeviceInfo.GetNewTaradodOnly = ChbNewTaradodOnly.Checked;
				_clsDeviceInfo.AutoSetDateTime = chbAutoSetDateTime.Checked;
			}
			else if (RbtnWan.Checked)
			{
				_clsDeviceInfo.TransferMode = (TypeConnection)0;
			}
			if (ucDIEC160.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9001;
			}
			else if (ucDIETC132.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9002;
			}
			else if (ucDIN1.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9003;
			}
			else if (ucDIRETC132.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9101;
			}
			else if (ucDIRN1.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9102;
			}
			else if (ucDINP2815.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9201;
			}
			else if (ucDINP302.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9202;
			}
			else if (ucDINP261.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9203;
			}
			else if (ucDINP395.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9301;
			}
			else if (ucDINP586.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9302;
			}
			else if (ucDINP587.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9304;
			}
			else if (ucDIZKT.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9400;
			}
			else if (ucNP255.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9305;
			}
			else if (ucNP290.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9306;
			}
			else if (ucNP550.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9204;
			}
			else if (ucNP770.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9307;
			}
			else if (ucNP756P.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9308;
			}
			else if (ucNP761P.GetRDBTN.Checked)
			{
				_clsDeviceInfo.Model = (DeviceModelEnumType)9309;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillClsDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 342);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
			{
				return;
			}
			((Control)BtnAccept).Enabled = false;
			_SaveToken?.Cancel();
			_SaveToken = new CancellationTokenSource();
			CancellationToken token = _SaveToken.Token;
			ReturnedSaveFuncInfo val = await ValidateFormAsync(token);
			if (val.HasError)
			{
				Utility.MessageBoxShowError(val.ErrorMessage, ((CacheBaseForm)this).ContextGuid);
				return;
			}
			FillClsDevice();
			if (await _clsDeviceInfo.SaveAsync(token))
			{
				await UpdateDeviceAsync(_clsDeviceInfo);
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				Utility.MessageBoxShowError("[ذخیره مشخصات دستگاه با خطا مواجه شد]", ((CacheBaseForm)this).ContextGuid);
				((Control)numCodeDevice).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 380);
		}
		finally
		{
			((Control)BtnAccept).Enabled = true;
		}
	}

	private async Task<ReturnedSaveFuncInfo> UpdateDeviceAsync(DeviceInfo clsDeviceInfo)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)clsDeviceInfo.TypeDeviceManufacture == 6)
			{
				string text = "";
				if (clsDeviceInfo.CommunicationPass.Trim().Length > 0 && clsDeviceInfo.CommunicationPass != "0")
				{
					text = clsDeviceInfo.CommunicationPass;
				}
				VisibleLightLanSDK sdk = new VisibleLightLanSDK(clsDeviceInfo.Ip, text);
				if (!(await sdk.IsConnectAsync()))
				{
					return ret.AddError("ارتباط با دستگاه NP761Plus  با آدرس آی پی " + clsDeviceInfo.Ip + " میسر نیست", "", "UpdateDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 401, (StringBuilder)null);
				}
				if (clsDeviceInfo.UseRealTime)
				{
					ReturnedSaveFuncInfo val = ret;
					val.AddReturnedValue(await sdk.SetLogCallBackAsync($"http://{clsDeviceInfo.CallBackIp}:{clsDeviceInfo.PortRealtime}", 0), (ReturnedState)0, "UpdateDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 403);
				}
				else
				{
					ReturnedSaveFuncInfo val = ret;
					val.AddReturnedValue(await sdk.SetLogCallBackAsync("http://NovinRank.ir", 0), (ReturnedState)0, "UpdateDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 404);
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddError(ex.Message, "", "UpdateDeviceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 413, (StringBuilder)null);
			Log.Error(ex, "");
		}
		return ret;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 428);
		}
	}

	private void FRMDevice_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMDevice_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 454);
		}
	}

	private async void BtnTestConnection_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnTestConnection).Enabled)
		{
			return;
		}
		((Control)BtnTestConnection).Enabled = false;
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			ReturnedSaveFuncInfo val = await ValidateFormAsync(token);
			if (val.HasError)
			{
				Utility.MessageBoxShowError(val.ErrorMessage, ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				FillClsDevice();
				ReturnedSaveFuncInfoWithValue<bool> val2 = await _clsDeviceInfo.TestConnectionAsync();
				if (((ReturnedSaveFuncInfo)val2).HasError)
				{
					((ReturnedSaveFuncInfo)(object)val2).Show(((CacheBaseForm)this).ContextGuid);
				}
				if (val2.value)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[ارتباط با موفقیت برقرار شد.]", "", 3);
				}
				else
				{
					Utility.MessageBoxShowError("[خطا در برقراری ارتباط : لطفا ارتباط دستگاه و سیستم را بررسی کنید. ]", ((CacheBaseForm)this).ContextGuid);
				}
			}
		}
		catch (Exception ex)
		{
			Utility.MessageBoxShowError(ex.Message, ((CacheBaseForm)this).ContextGuid);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnTestConnection_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 484);
		}
		finally
		{
			((Control)BtnTestConnection).Enabled = true;
		}
		((Control)TxtName).Focus();
	}

	private Task ShowLoadingAsync()
	{
		return Task.Run(delegate
		{
			if (((Control)pictureBoxLoading).InvokeRequired)
			{
				((Control)pictureBoxLoading).Invoke((Delegate)(Action)delegate
				{
					((Control)pictureBoxLoading).Visible = true;
				});
			}
			else
			{
				((Control)pictureBoxLoading).Visible = true;
			}
		});
	}

	private Task HideLoadingAsync()
	{
		return Task.Run(delegate
		{
			if (((Control)pictureBoxLoading).InvokeRequired)
			{
				((Control)pictureBoxLoading).Invoke((Delegate)(Action)delegate
				{
					((Control)pictureBoxLoading).Visible = false;
				});
			}
			else
			{
				((Control)pictureBoxLoading).Visible = false;
			}
		});
	}

	private async void BtnRecieve_Click(object sender, EventArgs e)
	{
		try
		{
			_ = 3;
			try
			{
				Task.Run(delegate
				{
					ShowLoadingAsync();
				});
				StringBuilder sb = new StringBuilder();
				((Control)BtnRecieve).Enabled = false;
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				ReturnedSaveFuncInfo val = await ValidateFormAsync(token);
				if (val.HasError)
				{
					if (val.HasError)
					{
						Utility.MessageBoxShowError(val.ErrorMessage, ((CacheBaseForm)this).ContextGuid);
					}
					return;
				}
				FillClsDevice();
				if (_clsDeviceInfo == null)
				{
					return;
				}
				FrmEbio ifDeviceIsEbio = null;
				if ((int)_clsDeviceInfo.TypeDeviceManufacture == 2)
				{
					ifDeviceIsEbio = (((int)_clsDeviceInfo.TransferMode == 1) ? new FrmEbio(((CacheBaseForm)this).ContextGuid, _clsDeviceInfo.DeviceNo, int.Parse(_clsDeviceInfo.CommunicationPass), _clsDeviceInfo.Ip, _clsDeviceInfo.TcpPort) : new FrmEbio(((CacheBaseForm)this).ContextGuid, _clsDeviceInfo.DeviceNo));
				}
				ReturnedSaveFuncInfo val2 = await _clsDeviceInfo.ReceiveDataAsync(sb, ifDeviceIsEbio, token);
				if (val2.HasError || val2.HasWarning)
				{
					if (val2.HasError)
					{
						foreach (string error in val2.ErrorList)
						{
							Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, error, "", 3);
							await Task.Delay(2000);
						}
						return;
					}
					foreach (string warning in val2.WarningList)
					{
						Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, warning, "", 3);
						await Task.Delay(2000);
					}
				}
				else if (val2.HasInformation)
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, val2.InformationMessage, "[دریافت داده]", 3);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnRecieve_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 563);
			}
		}
		finally
		{
			await HideLoadingAsync();
			((Control)BtnRecieve).Enabled = true;
		}
	}

	private void ucDIEC16_ChangeValue(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and O
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and O
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and O
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Invalid comparison between Unknown and O
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and O
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Invalid comparison between Unknown and O
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and O
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and O
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and O
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Invalid comparison between Unknown and O
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Invalid comparison between Unknown and O
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and O
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Invalid comparison between Unknown and O
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and O
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Invalid comparison between Unknown and O
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Invalid comparison between Unknown and O
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Invalid comparison between Unknown and O
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Invalid comparison between Unknown and O
		//IL_0218: Invalid comparison between Unknown and O
		try
		{
			((Control)lblPortDesc).Text = Utilities.GetTranslate("[پورت پیش فرض 5005]");
			RadioButton val = (RadioButton)sender;
			if ((object)val != ucDIEC160.GetRDBTN)
			{
				ucDIEC160.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDIETC132.GetRDBTN)
			{
				ucDIETC132.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDIN1.GetRDBTN)
			{
				ucDIN1.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDIRETC132.GetRDBTN)
			{
				ucDIRETC132.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDIRN1.GetRDBTN)
			{
				ucDIRN1.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP2815.GetRDBTN)
			{
				ucDINP2815.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP302.GetRDBTN)
			{
				ucDINP302.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP261.GetRDBTN)
			{
				ucDINP261.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP395.GetRDBTN)
			{
				ucDINP395.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP586.GetRDBTN)
			{
				ucDINP586.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucDINP587.GetRDBTN)
			{
				ucDINP587.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP255.GetRDBTN)
			{
				ucNP255.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP290.GetRDBTN)
			{
				ucNP290.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP550.GetRDBTN)
			{
				ucNP550.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP770.GetRDBTN)
			{
				ucNP770.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP756P.GetRDBTN)
			{
				ucNP756P.SetCheckRDBTN(sw: false);
			}
			if ((object)val != ucNP761P.GetRDBTN)
			{
				ucNP761P.SetCheckRDBTN(sw: false);
				ActiveCmbCallBackIps(visible: false);
			}
			if ((object)val != ucDIZKT.GetRDBTN)
			{
				ucDIZKT.SetCheckRDBTN(sw: false);
			}
			else
			{
				((Control)lblPortDesc).Text = Utilities.GetTranslate("[پورت پیش فرض 4370]");
			}
			if ((object)val == ucNP761P.GetRDBTN)
			{
				ActiveCmbCallBackIps(visible: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ucDIEC16_ChangeValue", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 629);
		}
	}

	private void ActiveCmbCallBackIps(bool visible)
	{
		try
		{
			ComboBox cmbCallBack = CmbCallBack;
			LabelX lblCallBack = LblCallBack;
			bool flag2 = (((Control)infoBoxCallBack).Visible = visible);
			bool visible2 = (((Control)lblCallBack).Visible = flag2);
			((Control)cmbCallBack).Visible = visible2;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void chbAutoSetDateTime_CheckValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (!chbAutoSetDateTime.Checked)
			{
				string msg = "[توصیه میشود تیک این گزینه حتما گذاشته شود تا هنگام دریافت تردد بصورت اتومات تاریخ و ساعت دستگاه بروزرسانی شود \n و در غیر این صورت بروزرسانی انجام نمیشود]";
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, msg, "", 3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chbAutoSetDateTime_CheckValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 654);
		}
	}

	private void numCodeDevice_Leave(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(((Control)numCodeDevice).Text))
			{
				((Control)numCodeDevice).Text = "1";
				numCodeDevice.Value = 1m;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "numCodeDevice_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 668);
		}
	}

	private void RbtnConnectionTypes_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RbtnLan.Checked)
			{
				((Control)PanelTcp).Enabled = true;
				ChbAutoRecieve.Checked = _clsDeviceInfo.AutoRecieve;
				ChkRealTimeDevice.Checked = _clsDeviceInfo.UseRealTime;
				ChbNewTaradodOnly.Checked = _clsDeviceInfo.GetNewTaradodOnly;
				chbAutoSetDateTime.Checked = _clsDeviceInfo.AutoSetDateTime;
				if (_clsDeviceInfo.MinCheck >= 10)
				{
					NumCount.Value = _clsDeviceInfo.MinCheck;
				}
				((Control)TxtIP).Text = ((_clsDeviceInfo.Ip.Length < 7) ? "192.168.1.224" : _clsDeviceInfo.Ip);
				((Control)TxtPort).Text = ((_clsDeviceInfo.TcpPort <= 0) ? "5005" : _clsDeviceInfo.TcpPort.ToString());
				((Control)TbxPortRealTime).Text = ((_clsDeviceInfo.PortRealtime <= 0) ? "5005" : _clsDeviceInfo.PortRealtime.ToString());
				((Control)TxtPass).Text = ((_clsDeviceInfo.CommunicationPass.Length == 0) ? "0" : _clsDeviceInfo.CommunicationPass);
				CheckBoxX obj = chBxPassword;
				bool @checked = (((Control)TxtPass).Enabled = ((Control)TxtPass).Text.Trim() != "0");
				obj.Checked = @checked;
				ButtonX btnTestConnection = BtnTestConnection;
				@checked = (((Control)BtnRecieve).Enabled = true);
				((Control)btnTestConnection).Enabled = @checked;
			}
			else
			{
				((Control)PanelTcp).Enabled = false;
				ButtonX btnTestConnection2 = BtnTestConnection;
				bool enabled;
				if (!RbtnUsb.Checked)
				{
					bool @checked = (((Control)BtnRecieve).Enabled = false);
					enabled = @checked;
				}
				else
				{
					bool @checked = (((Control)BtnRecieve).Enabled = true);
					enabled = @checked;
				}
				((Control)btnTestConnection2).Enabled = enabled;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbtnConnectionTypes_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 699);
		}
	}

	private void LoadCmbCallBackIps()
	{
		try
		{
			CmbCallBack.Items.Clear();
			List<string> list = (from ip in Utility.GetLocalIps(onlyIpV4: true)
				select ip.ToString()).ToList();
			if (!string.IsNullOrEmpty(_clsDeviceInfo.CallBackIp) && !list.Contains(_clsDeviceInfo.CallBackIp))
			{
				list.Add(_clsDeviceInfo.CallBackIp);
			}
			ObjectCollection items = CmbCallBack.Items;
			object[] array = list.ToArray();
			items.AddRange(array);
			if (string.IsNullOrEmpty(_clsDeviceInfo.CallBackIp))
			{
				return;
			}
			int num = CmbCallBack.FindStringExact(_clsDeviceInfo.CallBackIp);
			if (num != -1)
			{
				((ListControl)CmbCallBack).SelectedIndex = num;
				return;
			}
			for (int i = 0; i < CmbCallBack.Items.Count; i++)
			{
				if (CmbCallBack.Items[i].ToString() == _clsDeviceInfo.CallBackIp)
				{
					((ListControl)CmbCallBack).SelectedIndex = i;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void chBxPassword_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtPass).Enabled = chBxPassword.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "chBxPassword_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 744);
		}
	}

	private async void FrmDevice_Load(object sender, EventArgs e)
	{
		try
		{
			pictureBoxLoading.Image = (Image)(object)Resources.Time1;
			((Control)pictureBoxLoading).Visible = false;
			((Control)BtnManageEnroll).Visible = false;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await LoadDataAsync(token);
			frm_Text = Utilities.GetTranslate(frm_Text);
			((Control)this).Text = frm_Text;
			FillForm();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmDevice_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 767);
		}
	}

	public async void BtnManageEnroll_Click(object sender, EventArgs e)
	{
		try
		{
			await ((CacheBaseForm)new UserEnrolls(((CacheBaseForm)this).ContextGuid, _clsDeviceInfo, bpc)).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnManageEnroll_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 780);
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
				_SaveToken?.Cancel();
				_SaveToken?.Dispose();
				_SaveToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				_GetTaradodToken?.Cancel();
				_GetTaradodToken?.Dispose();
				_GetTaradodToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void TxtIP_TextChanged(object sender, EventArgs e)
	{
	}

	private void TxtIP_Leave(object sender, EventArgs e)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!string.IsNullOrEmpty(((Control)TxtIP).Text) && ((Control)TxtIP).Text.Length != 15)
			{
				((Control)TxtIP).Text = ((Control)TxtIP).Text.Replace(',', '.').Replace('/', '.');
				if (((Control)TxtIP).Text.Split(new char[1] { '.' }).Length != 4)
				{
					MessageBoxFarsi.Show("آدرس وارد شده نامعتبر است", "هشدار");
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void pnlColor_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		try
		{
			if ((int)((CommonDialog)colorDialog1).ShowDialog() == 1)
			{
				((Control)PbColorTaradod).BackColor = colorDialog1.Color;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "pnlColor_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Device\\FRMDevice.cs", 862);
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("در صورتیکه تیک تردد آنی فعال باشد");
			stringBuilder.AppendLine("بایستی آی پی سیستم خودتان را انتخاب کنید");
			stringBuilder.AppendLine("تا ترددها بصورت آنی دریافت شوند");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("و اگر تیک تردد آنی فعال نباشد");
			stringBuilder.AppendLine("تردد ها به سرور کلود نوین پرداز ارسال خواهد شد");
			stringBuilder.AppendLine("و شما از طریق کلود ترددها را دریافت خواهید کرد");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("برای هر دستگاه یک شماره پورت یکتا ثبت کنید");
			stringBuilder.AppendLine("تا در دریافت ترددهای دستگاهها تداخل نداشته باشید");
			stringBuilder.AppendLine();
			MessageBoxFarsi.Show(stringBuilder.ToString());
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdd: Expected O, but got Unknown
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Expected O, but got Unknown
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d76: Expected O, but got Unknown
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Expected O, but got Unknown
		//IL_0e6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f22: Expected O, but got Unknown
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Expected O, but got Unknown
		//IL_1013: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Expected O, but got Unknown
		//IL_10ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_1196: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a0: Expected O, but got Unknown
		//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1277: Expected O, but got Unknown
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_1343: Unknown result type (might be due to invalid IL or missing references)
		//IL_134d: Expected O, but got Unknown
		//IL_136e: Unknown result type (might be due to invalid IL or missing references)
		//IL_141a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1424: Expected O, but got Unknown
		//IL_1448: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fe: Expected O, but got Unknown
		//IL_1522: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d8: Expected O, but got Unknown
		//IL_15f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ae: Expected O, but got Unknown
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_177b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Expected O, but got Unknown
		//IL_17a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1855: Unknown result type (might be due to invalid IL or missing references)
		//IL_185f: Expected O, but got Unknown
		//IL_1883: Unknown result type (might be due to invalid IL or missing references)
		//IL_192f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Expected O, but got Unknown
		//IL_1959: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0f: Expected O, but got Unknown
		//IL_1a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_207a: Unknown result type (might be due to invalid IL or missing references)
		//IL_210c: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_222d: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2375: Unknown result type (might be due to invalid IL or missing references)
		//IL_2424: Unknown result type (might be due to invalid IL or missing references)
		//IL_242e: Expected O, but got Unknown
		//IL_245e: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2626: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_276b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2805: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_292a: Unknown result type (might be due to invalid IL or missing references)
		//IL_29cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cad: Expected O, but got Unknown
		//IL_2d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dcc: Expected O, but got Unknown
		//IL_2e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3088: Unknown result type (might be due to invalid IL or missing references)
		//IL_3123: Unknown result type (might be due to invalid IL or missing references)
		//IL_312d: Expected O, but got Unknown
		//IL_318d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3258: Unknown result type (might be due to invalid IL or missing references)
		//IL_3262: Expected O, but got Unknown
		//IL_32bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3432: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ec: Expected O, but got Unknown
		//IL_34f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3547: Unknown result type (might be due to invalid IL or missing references)
		//IL_3551: Expected O, but got Unknown
		panelEx1 = new PanelEx();
		PbColorTaradod = new PictureBox();
		pictureBoxLoading = new PictureBox();
		BtnManageEnroll = new ButtonX();
		labelX10 = new LabelX();
		TbxSerial = new TextBoxX();
		RbtnWan = new RadioButton();
		flowLayoutPanel1 = new FlowLayoutPanel();
		ucDIEC160 = new UCDeviceImg();
		ucDIETC132 = new UCDeviceImg();
		ucDIN1 = new UCDeviceImg();
		ucDINP2815 = new UCDeviceImg();
		ucDINP302 = new UCDeviceImg();
		ucDIRETC132 = new UCDeviceImg();
		ucDIRN1 = new UCDeviceImg();
		ucDINP261 = new UCDeviceImg();
		ucDINP395 = new UCDeviceImg();
		ucDINP586 = new UCDeviceImg();
		ucDINP587 = new UCDeviceImg();
		ucDIZKT = new UCDeviceImg();
		ucNP255 = new UCDeviceImg();
		ucNP290 = new UCDeviceImg();
		ucNP550 = new UCDeviceImg();
		ucNP770 = new UCDeviceImg();
		ucNP756P = new UCDeviceImg();
		ucNP761P = new UCDeviceImg();
		numCodeDevice = new NumericUpDown();
		PanelTcp = new PanelEx();
		infoBoxCallBack = new PictureBox();
		CmbCallBack = new ComboBox();
		TbxPortRealTime = new TextBoxX();
		labelX2 = new LabelX();
		ChkRealTimeDevice = new CheckBoxX();
		labelX8 = new LabelX();
		ChbNewTaradodOnly = new CheckBoxX();
		NumCount = new NumericUpDown();
		chbAutoSetDateTime = new CheckBoxX();
		lblPortDesc = new LabelX();
		chBxPassword = new CheckBoxX();
		TxtPort = new TextBoxX();
		ChbAutoRecieve = new CheckBoxX();
		labelX4 = new LabelX();
		TxtIP = new TextBoxX();
		LblCallBack = new LabelX();
		TxtPass = new TextBoxX();
		labelX3 = new LabelX();
		RbtnLan = new RadioButton();
		TxtDesc = new TextBoxX();
		labelX5 = new LabelX();
		RbtnUsb = new RadioButton();
		BtnRecieve = new ButtonX();
		BtnTestConnection = new ButtonX();
		pnlColor = new PanelEx();
		labelX9 = new LabelX();
		BtnAccept = new ButtonX();
		BtnCancel = new ButtonX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		colorDialog1 = new ColorDialog();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)PbColorTaradod).BeginInit();
		((ISupportInitialize)pictureBoxLoading).BeginInit();
		((Control)flowLayoutPanel1).SuspendLayout();
		((ISupportInitialize)numCodeDevice).BeginInit();
		((Control)PanelTcp).SuspendLayout();
		((ISupportInitialize)infoBoxCallBack).BeginInit();
		((ISupportInitialize)NumCount).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panelEx1).Anchor = (AnchorStyles)13;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)PbColorTaradod);
		((Control)panelEx1).Controls.Add((Control)(object)pictureBoxLoading);
		((Control)panelEx1).Controls.Add((Control)(object)BtnManageEnroll);
		((Control)panelEx1).Controls.Add((Control)(object)labelX10);
		((Control)panelEx1).Controls.Add((Control)(object)TbxSerial);
		((Control)panelEx1).Controls.Add((Control)(object)RbtnWan);
		((Control)panelEx1).Controls.Add((Control)(object)flowLayoutPanel1);
		((Control)panelEx1).Controls.Add((Control)(object)numCodeDevice);
		((Control)panelEx1).Controls.Add((Control)(object)PanelTcp);
		((Control)panelEx1).Controls.Add((Control)(object)RbtnLan);
		((Control)panelEx1).Controls.Add((Control)(object)TxtDesc);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)RbtnUsb);
		((Control)panelEx1).Controls.Add((Control)(object)BtnRecieve);
		((Control)panelEx1).Controls.Add((Control)(object)BtnTestConnection);
		((Control)panelEx1).Controls.Add((Control)(object)pnlColor);
		((Control)panelEx1).Controls.Add((Control)(object)labelX9);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)TxtName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(-1, 0);
		((Control)panelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(853, 615);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)PbColorTaradod).Anchor = (AnchorStyles)10;
		PbColorTaradod.BorderStyle = (BorderStyle)1;
		((Control)PbColorTaradod).Location = new Point(439, 529);
		((Control)PbColorTaradod).Name = "PbColorTaradod";
		((Control)PbColorTaradod).Size = new Size(24, 24);
		PbColorTaradod.TabIndex = 150;
		PbColorTaradod.TabStop = false;
		((Control)pictureBoxLoading).Anchor = (AnchorStyles)9;
		((Control)pictureBoxLoading).BackColor = SystemColors.Control;
		((Control)pictureBoxLoading).BackgroundImageLayout = (ImageLayout)3;
		pictureBoxLoading.BorderStyle = (BorderStyle)1;
		((Control)pictureBoxLoading).Location = new Point(444, 117);
		((Control)pictureBoxLoading).Name = "pictureBoxLoading";
		((Control)pictureBoxLoading).Size = new Size(96, 95);
		pictureBoxLoading.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxLoading.TabIndex = 143;
		pictureBoxLoading.TabStop = false;
		((Control)BtnManageEnroll).AccessibleRole = (AccessibleRole)43;
		((Control)BtnManageEnroll).Anchor = (AnchorStyles)10;
		BtnManageEnroll.ColorTable = (eButtonColor)6;
		((Control)BtnManageEnroll).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)BtnManageEnroll).Location = new Point(291, 569);
		((Control)BtnManageEnroll).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnManageEnroll).Name = "BtnManageEnroll";
		((Control)BtnManageEnroll).Size = new Size(142, 34);
		((PopupItemControl)BtnManageEnroll).Style = (eDotNetBarStyle)9;
		((Control)BtnManageEnroll).TabIndex = 19;
		((Control)BtnManageEnroll).Text = "مدیریت پرسنل دستگاه";
		((Control)BtnManageEnroll).Click += BtnManageEnroll_Click;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Location = new Point(742, 93);
		((Control)labelX10).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).RightToLeft = (RightToLeft)1;
		((Control)labelX10).Size = new Size(107, 21);
		((Control)labelX10).TabIndex = 0;
		((Control)labelX10).Text = "[سریال دستگاه] :";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)TbxSerial).Anchor = (AnchorStyles)9;
		TbxSerial.Border.Class = "TextBoxBorder";
		TbxSerial.Border.CornerType = (eCornerType)1;
		((Control)TbxSerial).Location = new Point(480, 93);
		((Control)TbxSerial).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TbxSerial).MaxLength = 32;
		((Control)TbxSerial).Name = "TbxSerial";
		((Control)TbxSerial).Size = new Size(254, 21);
		((Control)TbxSerial).TabIndex = 3;
		((Control)RbtnWan).Anchor = (AnchorStyles)9;
		((Control)RbtnWan).ForeColor = SystemColors.HotTrack;
		((Control)RbtnWan).Location = new Point(461, 157);
		((Control)RbtnWan).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtnWan).Name = "RbtnWan";
		((Control)RbtnWan).RightToLeft = (RightToLeft)1;
		((Control)RbtnWan).Size = new Size(340, 17);
		((Control)RbtnWan).TabIndex = 5;
		((Control)RbtnWan).Text = "[ارتباط از طریق شبکه اینترنت]";
		((ButtonBase)RbtnWan).UseVisualStyleBackColor = true;
		RbtnWan.CheckedChanged += RbtnConnectionTypes_CheckedChanged;
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIEC160);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIETC132);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIN1);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP2815);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP302);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIRETC132);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIRN1);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP261);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP395);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP586);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDINP587);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucDIZKT);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP255);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP290);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP550);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP770);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP756P);
		((Control)flowLayoutPanel1).Controls.Add((Control)(object)ucNP761P);
		((Control)flowLayoutPanel1).Location = new Point(12, 12);
		((Control)flowLayoutPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)flowLayoutPanel1).Name = "flowLayoutPanel1";
		((Control)flowLayoutPanel1).Size = new Size(422, 551);
		((Control)flowLayoutPanel1).TabIndex = 10;
		((Control)ucDIEC160).AutoSize = true;
		ucDIEC160.DeviceName = "EC160";
		((Control)ucDIEC160).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIEC160).Location = new Point(4, 3);
		((Control)ucDIEC160).Margin = new Padding(4, 3, 4, 3);
		ucDIEC160.Model = (DeviceModelEnumType)9001;
		((Control)ucDIEC160).Name = "ucDIEC160";
		((Control)ucDIEC160).RightToLeft = (RightToLeft)1;
		((Control)ucDIEC160).Size = new Size(89, 81);
		((Control)ucDIEC160).TabIndex = 1000;
		((Control)ucDIEC160).TabStop = false;
		ucDIEC160.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDIETC132).AutoSize = true;
		ucDIETC132.DeviceName = "ETC132";
		((Control)ucDIETC132).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIETC132).Location = new Point(101, 3);
		((Control)ucDIETC132).Margin = new Padding(4, 3, 4, 3);
		ucDIETC132.Model = (DeviceModelEnumType)9002;
		((Control)ucDIETC132).Name = "ucDIETC132";
		((Control)ucDIETC132).RightToLeft = (RightToLeft)1;
		((Control)ucDIETC132).Size = new Size(89, 81);
		((Control)ucDIETC132).TabIndex = 1000;
		((Control)ucDIETC132).TabStop = false;
		ucDIETC132.ChangeValue += ucDIEC16_ChangeValue;
		ucDIN1.DeviceName = "N1";
		((Control)ucDIN1).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIN1).Location = new Point(198, 3);
		((Control)ucDIN1).Margin = new Padding(4, 3, 4, 3);
		ucDIN1.Model = (DeviceModelEnumType)9003;
		((Control)ucDIN1).Name = "ucDIN1";
		((Control)ucDIN1).RightToLeft = (RightToLeft)1;
		((Control)ucDIN1).Size = new Size(86, 82);
		((Control)ucDIN1).TabIndex = 1000;
		((Control)ucDIN1).TabStop = false;
		ucDIN1.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP2815).AutoSize = true;
		ucDINP2815.DeviceName = "NP2815";
		((Control)ucDINP2815).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP2815).Location = new Point(292, 3);
		((Control)ucDINP2815).Margin = new Padding(4, 3, 4, 3);
		ucDINP2815.Model = (DeviceModelEnumType)9201;
		((Control)ucDINP2815).Name = "ucDINP2815";
		((Control)ucDINP2815).RightToLeft = (RightToLeft)1;
		((Control)ucDINP2815).Size = new Size(89, 81);
		((Control)ucDINP2815).TabIndex = 1000;
		((Control)ucDINP2815).TabStop = false;
		ucDINP2815.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP302).AutoSize = true;
		ucDINP302.DeviceName = "NP302";
		((Control)ucDINP302).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP302).Location = new Point(4, 91);
		((Control)ucDINP302).Margin = new Padding(4, 3, 4, 3);
		ucDINP302.Model = (DeviceModelEnumType)9202;
		((Control)ucDINP302).Name = "ucDINP302";
		((Control)ucDINP302).RightToLeft = (RightToLeft)1;
		((Control)ucDINP302).Size = new Size(89, 81);
		((Control)ucDINP302).TabIndex = 1000;
		((Control)ucDINP302).TabStop = false;
		ucDINP302.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDIRETC132).AutoSize = true;
		ucDIRETC132.DeviceName = "RETC132";
		((Control)ucDIRETC132).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIRETC132).Location = new Point(101, 91);
		((Control)ucDIRETC132).Margin = new Padding(4, 3, 4, 3);
		ucDIRETC132.Model = (DeviceModelEnumType)9101;
		((Control)ucDIRETC132).Name = "ucDIRETC132";
		((Control)ucDIRETC132).RightToLeft = (RightToLeft)1;
		((Control)ucDIRETC132).Size = new Size(89, 81);
		((Control)ucDIRETC132).TabIndex = 1000;
		((Control)ucDIRETC132).TabStop = false;
		ucDIRETC132.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDIRN1).AutoSize = true;
		ucDIRN1.DeviceName = "RN1";
		((Control)ucDIRN1).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIRN1).Location = new Point(198, 91);
		((Control)ucDIRN1).Margin = new Padding(4, 3, 4, 3);
		ucDIRN1.Model = (DeviceModelEnumType)9102;
		((Control)ucDIRN1).Name = "ucDIRN1";
		((Control)ucDIRN1).RightToLeft = (RightToLeft)1;
		((Control)ucDIRN1).Size = new Size(89, 81);
		((Control)ucDIRN1).TabIndex = 1000;
		((Control)ucDIRN1).TabStop = false;
		ucDIRN1.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP261).AutoSize = true;
		ucDINP261.DeviceName = "NP261";
		((Control)ucDINP261).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP261).Location = new Point(295, 91);
		((Control)ucDINP261).Margin = new Padding(4, 3, 4, 3);
		ucDINP261.Model = (DeviceModelEnumType)9203;
		((Control)ucDINP261).Name = "ucDINP261";
		((Control)ucDINP261).RightToLeft = (RightToLeft)1;
		((Control)ucDINP261).Size = new Size(89, 81);
		((Control)ucDINP261).TabIndex = 1000;
		((Control)ucDINP261).TabStop = false;
		ucDINP261.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP395).AutoSize = true;
		ucDINP395.DeviceName = "NP395";
		((Control)ucDINP395).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP395).Location = new Point(4, 178);
		((Control)ucDINP395).Margin = new Padding(4, 3, 4, 3);
		ucDINP395.Model = (DeviceModelEnumType)9301;
		((Control)ucDINP395).Name = "ucDINP395";
		((Control)ucDINP395).RightToLeft = (RightToLeft)1;
		((Control)ucDINP395).Size = new Size(89, 81);
		((Control)ucDINP395).TabIndex = 1000;
		((Control)ucDINP395).TabStop = false;
		ucDINP395.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP586).AutoSize = true;
		ucDINP586.DeviceName = "NP586";
		((Control)ucDINP586).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP586).Location = new Point(101, 178);
		((Control)ucDINP586).Margin = new Padding(4, 3, 4, 3);
		ucDINP586.Model = (DeviceModelEnumType)9302;
		((Control)ucDINP586).Name = "ucDINP586";
		((Control)ucDINP586).RightToLeft = (RightToLeft)1;
		((Control)ucDINP586).Size = new Size(89, 81);
		((Control)ucDINP586).TabIndex = 1000;
		((Control)ucDINP586).TabStop = false;
		ucDINP586.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDINP587).AutoSize = true;
		ucDINP587.DeviceName = "NP587";
		((Control)ucDINP587).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDINP587).Location = new Point(198, 178);
		((Control)ucDINP587).Margin = new Padding(4, 3, 4, 3);
		ucDINP587.Model = (DeviceModelEnumType)9304;
		((Control)ucDINP587).Name = "ucDINP587";
		((Control)ucDINP587).RightToLeft = (RightToLeft)1;
		((Control)ucDINP587).Size = new Size(89, 81);
		((Control)ucDINP587).TabIndex = 1000;
		((Control)ucDINP587).TabStop = false;
		ucDINP587.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucDIZKT).AutoSize = true;
		ucDIZKT.DeviceName = "ZKT";
		((Control)ucDIZKT).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucDIZKT).Location = new Point(295, 178);
		((Control)ucDIZKT).Margin = new Padding(4, 3, 4, 3);
		ucDIZKT.Model = (DeviceModelEnumType)9400;
		((Control)ucDIZKT).Name = "ucDIZKT";
		((Control)ucDIZKT).RightToLeft = (RightToLeft)1;
		((Control)ucDIZKT).Size = new Size(89, 81);
		((Control)ucDIZKT).TabIndex = 1000;
		((Control)ucDIZKT).TabStop = false;
		ucDIZKT.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP255).AutoSize = true;
		ucNP255.DeviceName = "NP255";
		((Control)ucNP255).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP255).Location = new Point(4, 265);
		((Control)ucNP255).Margin = new Padding(4, 3, 4, 3);
		ucNP255.Model = (DeviceModelEnumType)9305;
		((Control)ucNP255).Name = "ucNP255";
		((Control)ucNP255).RightToLeft = (RightToLeft)1;
		((Control)ucNP255).Size = new Size(89, 81);
		((Control)ucNP255).TabIndex = 1001;
		((Control)ucNP255).TabStop = false;
		ucNP255.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP290).AutoSize = true;
		ucNP290.DeviceName = "NP290";
		((Control)ucNP290).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP290).Location = new Point(101, 265);
		((Control)ucNP290).Margin = new Padding(4, 3, 4, 3);
		ucNP290.Model = (DeviceModelEnumType)9306;
		((Control)ucNP290).Name = "ucNP290";
		((Control)ucNP290).RightToLeft = (RightToLeft)1;
		((Control)ucNP290).Size = new Size(89, 81);
		((Control)ucNP290).TabIndex = 1002;
		((Control)ucNP290).TabStop = false;
		ucNP290.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP550).AutoSize = true;
		ucNP550.DeviceName = "NP550";
		((Control)ucNP550).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP550).Location = new Point(198, 265);
		((Control)ucNP550).Margin = new Padding(4, 3, 4, 3);
		ucNP550.Model = (DeviceModelEnumType)9204;
		((Control)ucNP550).Name = "ucNP550";
		((Control)ucNP550).RightToLeft = (RightToLeft)1;
		((Control)ucNP550).Size = new Size(89, 81);
		((Control)ucNP550).TabIndex = 1003;
		((Control)ucNP550).TabStop = false;
		ucNP550.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP770).AutoSize = true;
		ucNP770.DeviceName = "NP770";
		((Control)ucNP770).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP770).Location = new Point(295, 265);
		((Control)ucNP770).Margin = new Padding(4, 3, 4, 3);
		ucNP770.Model = (DeviceModelEnumType)9307;
		((Control)ucNP770).Name = "ucNP770";
		((Control)ucNP770).RightToLeft = (RightToLeft)1;
		((Control)ucNP770).Size = new Size(89, 81);
		((Control)ucNP770).TabIndex = 1004;
		((Control)ucNP770).TabStop = false;
		ucNP770.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP756P).AutoSize = true;
		ucNP756P.DeviceName = "NP756P";
		((Control)ucNP756P).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP756P).Location = new Point(4, 352);
		((Control)ucNP756P).Margin = new Padding(4, 3, 4, 3);
		ucNP756P.Model = (DeviceModelEnumType)9308;
		((Control)ucNP756P).Name = "ucNP756P";
		((Control)ucNP756P).RightToLeft = (RightToLeft)1;
		((Control)ucNP756P).Size = new Size(89, 81);
		((Control)ucNP756P).TabIndex = 1005;
		((Control)ucNP756P).TabStop = false;
		ucNP756P.ChangeValue += ucDIEC16_ChangeValue;
		((Control)ucNP761P).AutoSize = true;
		ucNP761P.DeviceName = "NP761P";
		((Control)ucNP761P).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)ucNP761P).Location = new Point(101, 352);
		((Control)ucNP761P).Margin = new Padding(4, 3, 4, 3);
		ucNP761P.Model = (DeviceModelEnumType)9309;
		((Control)ucNP761P).Name = "ucNP761P";
		((Control)ucNP761P).RightToLeft = (RightToLeft)1;
		((Control)ucNP761P).Size = new Size(89, 81);
		((Control)ucNP761P).TabIndex = 1006;
		((Control)ucNP761P).TabStop = false;
		ucNP761P.ChangeValue += ucDIEC16_ChangeValue;
		((Control)numCodeDevice).Anchor = (AnchorStyles)9;
		((Control)numCodeDevice).ForeColor = Color.Red;
		((Control)numCodeDevice).Location = new Point(661, 10);
		((Control)numCodeDevice).Margin = new Padding(4, 3, 4, 3);
		numCodeDevice.Maximum = new decimal(new int[4] { 999, 0, 0, 0 });
		numCodeDevice.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numCodeDevice).Name = "numCodeDevice";
		((Control)numCodeDevice).Size = new Size(74, 21);
		((Control)numCodeDevice).TabIndex = 0;
		((UpDownBase)numCodeDevice).TextAlign = (HorizontalAlignment)2;
		numCodeDevice.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numCodeDevice).Leave += numCodeDevice_Leave;
		((Control)PanelTcp).Anchor = (AnchorStyles)9;
		PanelTcp.CanvasColor = SystemColors.Control;
		PanelTcp.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelTcp).Controls.Add((Control)(object)infoBoxCallBack);
		((Control)PanelTcp).Controls.Add((Control)(object)CmbCallBack);
		((Control)PanelTcp).Controls.Add((Control)(object)TbxPortRealTime);
		((Control)PanelTcp).Controls.Add((Control)(object)labelX2);
		((Control)PanelTcp).Controls.Add((Control)(object)ChkRealTimeDevice);
		((Control)PanelTcp).Controls.Add((Control)(object)labelX8);
		((Control)PanelTcp).Controls.Add((Control)(object)ChbNewTaradodOnly);
		((Control)PanelTcp).Controls.Add((Control)(object)NumCount);
		((Control)PanelTcp).Controls.Add((Control)(object)chbAutoSetDateTime);
		((Control)PanelTcp).Controls.Add((Control)(object)lblPortDesc);
		((Control)PanelTcp).Controls.Add((Control)(object)chBxPassword);
		((Control)PanelTcp).Controls.Add((Control)(object)TxtPort);
		((Control)PanelTcp).Controls.Add((Control)(object)ChbAutoRecieve);
		((Control)PanelTcp).Controls.Add((Control)(object)labelX4);
		((Control)PanelTcp).Controls.Add((Control)(object)TxtIP);
		((Control)PanelTcp).Controls.Add((Control)(object)LblCallBack);
		((Control)PanelTcp).Controls.Add((Control)(object)TxtPass);
		((Control)PanelTcp).Controls.Add((Control)(object)labelX3);
		PanelTcp.DisabledBackColor = Color.Empty;
		((Control)PanelTcp).Enabled = false;
		((Control)PanelTcp).Location = new Point(443, 215);
		((Control)PanelTcp).Margin = new Padding(4, 3, 4, 3);
		((Control)PanelTcp).Name = "PanelTcp";
		((Control)PanelTcp).Size = new Size(376, 301);
		PanelTcp.Style.Alignment = (StringAlignment)1;
		PanelTcp.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		PanelTcp.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		PanelTcp.Style.Border = (eBorderType)1;
		PanelTcp.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelTcp.Style.CornerType = (eCornerType)2;
		PanelTcp.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelTcp.Style.GradientAngle = 90;
		((Control)PanelTcp).TabIndex = 8;
		((Control)infoBoxCallBack).BackColor = Color.Transparent;
		((Control)infoBoxCallBack).BackgroundImage = (Image)(object)Resources.question;
		((Control)infoBoxCallBack).BackgroundImageLayout = (ImageLayout)3;
		((Control)infoBoxCallBack).Cursor = Cursors.Help;
		((Control)infoBoxCallBack).Location = new Point(106, 61);
		((Control)infoBoxCallBack).Name = "infoBoxCallBack";
		((Control)infoBoxCallBack).Size = new Size(20, 20);
		infoBoxCallBack.TabIndex = 19;
		infoBoxCallBack.TabStop = false;
		((Control)infoBoxCallBack).Visible = false;
		((Control)infoBoxCallBack).Click += pictureBox1_Click;
		((Control)CmbCallBack).Anchor = (AnchorStyles)9;
		((ListControl)CmbCallBack).FormattingEnabled = true;
		((Control)CmbCallBack).Location = new Point(132, 61);
		((Control)CmbCallBack).Name = "CmbCallBack";
		((Control)CmbCallBack).RightToLeft = (RightToLeft)1;
		((Control)CmbCallBack).Size = new Size(120, 21);
		((Control)CmbCallBack).TabIndex = 18;
		((Control)CmbCallBack).Text = "127.0.0.1";
		((Control)CmbCallBack).Visible = false;
		TbxPortRealTime.Border.Class = "TextBoxBorder";
		TbxPortRealTime.Border.CornerType = (eCornerType)1;
		((Control)TbxPortRealTime).Location = new Point(132, 34);
		((Control)TbxPortRealTime).Margin = new Padding(4, 3, 4, 3);
		((Control)TbxPortRealTime).Name = "TbxPortRealTime";
		((Control)TbxPortRealTime).RightToLeft = (RightToLeft)1;
		((Control)TbxPortRealTime).Size = new Size(120, 21);
		((Control)TbxPortRealTime).TabIndex = 17;
		((Control)TbxPortRealTime).Text = "7005";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(259, 34);
		((Control)labelX2).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(105, 21);
		((Control)labelX2).TabIndex = 16;
		((Control)labelX2).Text = "پورت سرور : ";
		labelX2.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChkRealTimeDevice).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChkRealTimeDevice).Location = new Point(42, 10);
		((Control)ChkRealTimeDevice).Margin = new Padding(4, 3, 4, 3);
		((Control)ChkRealTimeDevice).Name = "ChkRealTimeDevice";
		((Control)ChkRealTimeDevice).RightToLeft = (RightToLeft)1;
		((Control)ChkRealTimeDevice).Size = new Size(316, 21);
		((BaseItemControl)ChkRealTimeDevice).Style = (eDotNetBarStyle)9;
		((Control)ChkRealTimeDevice).TabIndex = 15;
		((Control)ChkRealTimeDevice).Text = "دریافت لحظه ای تردد ها از دستگاه";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(34, 200);
		((Control)labelX8).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(51, 21);
		((Control)labelX8).TabIndex = 0;
		((Control)labelX8).Text = "[دقیقه]";
		labelX8.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbNewTaradodOnly).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbNewTaradodOnly).Location = new Point(42, 231);
		((Control)ChbNewTaradodOnly).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbNewTaradodOnly).Name = "ChbNewTaradodOnly";
		((Control)ChbNewTaradodOnly).RightToLeft = (RightToLeft)1;
		((Control)ChbNewTaradodOnly).Size = new Size(316, 21);
		((BaseItemControl)ChbNewTaradodOnly).Style = (eDotNetBarStyle)9;
		((Control)ChbNewTaradodOnly).TabIndex = 13;
		((Control)ChbNewTaradodOnly).Text = "[هنگام دریافت اطلاعات فقط ترددهای جدید دریافت شود]";
		((Control)NumCount).Location = new Point(97, 200);
		((Control)NumCount).Margin = new Padding(4, 3, 4, 3);
		NumCount.Maximum = new decimal(new int[4] { 1500, 0, 0, 0 });
		NumCount.Minimum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumCount).Name = "NumCount";
		((Control)NumCount).Size = new Size(46, 21);
		((Control)NumCount).TabIndex = 12;
		NumCount.Value = new decimal(new int[4] { 10, 0, 0, 0 });
		((BaseItemControl)chbAutoSetDateTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbAutoSetDateTime).Location = new Point(1, 262);
		((Control)chbAutoSetDateTime).Margin = new Padding(4, 3, 4, 3);
		((Control)chbAutoSetDateTime).Name = "chbAutoSetDateTime";
		((Control)chbAutoSetDateTime).RightToLeft = (RightToLeft)1;
		((Control)chbAutoSetDateTime).Size = new Size(357, 21);
		((BaseItemControl)chbAutoSetDateTime).Style = (eDotNetBarStyle)9;
		((Control)chbAutoSetDateTime).TabIndex = 14;
		((Control)chbAutoSetDateTime).Text = "[تنظیم اتومات تاریخ و ساعت دستگاه هنگام دریافت ترددها]";
		chbAutoSetDateTime.CheckValueChanged += chbAutoSetDateTime_CheckValueChanged;
		((Control)lblPortDesc).BackColor = Color.Transparent;
		((BaseItemControl)lblPortDesc).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblPortDesc).Font = new Font("Microsoft Sans Serif", 8f);
		((Control)lblPortDesc).ForeColor = Color.Red;
		((Control)lblPortDesc).Location = new Point(4, 134);
		((Control)lblPortDesc).Margin = new Padding(4, 3, 4, 3);
		((Control)lblPortDesc).Name = "lblPortDesc";
		((Control)lblPortDesc).Size = new Size(121, 23);
		((Control)lblPortDesc).TabIndex = 0;
		((Control)lblPortDesc).Text = "[پورت پیش فرض 5005]";
		lblPortDesc.TextAlignment = (StringAlignment)1;
		((Control)chBxPassword).AutoSize = true;
		((BaseItemControl)chBxPassword).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxPassword).Location = new Point(257, 161);
		((Control)chBxPassword).Margin = new Padding(4, 3, 4, 3);
		((Control)chBxPassword).Name = "chBxPassword";
		((Control)chBxPassword).RightToLeft = (RightToLeft)1;
		((Control)chBxPassword).Size = new Size(101, 16);
		((BaseItemControl)chBxPassword).Style = (eDotNetBarStyle)9;
		((Control)chBxPassword).TabIndex = 9;
		((Control)chBxPassword).Text = ": رمــــز دستگاه ";
		chBxPassword.CheckedChanged += chBxPassword_CheckedChanged;
		TxtPort.Border.Class = "TextBoxBorder";
		TxtPort.Border.CornerType = (eCornerType)1;
		((Control)TxtPort).Location = new Point(132, 134);
		((Control)TxtPort).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtPort).Name = "TxtPort";
		((Control)TxtPort).Size = new Size(120, 21);
		((Control)TxtPort).TabIndex = 8;
		((BaseItemControl)ChbAutoRecieve).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbAutoRecieve).Location = new Point(127, 200);
		((Control)ChbAutoRecieve).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbAutoRecieve).Name = "ChbAutoRecieve";
		((Control)ChbAutoRecieve).RightToLeft = (RightToLeft)1;
		((Control)ChbAutoRecieve).Size = new Size(231, 21);
		((BaseItemControl)ChbAutoRecieve).Style = (eDotNetBarStyle)9;
		((Control)ChbAutoRecieve).TabIndex = 11;
		((Control)ChbAutoRecieve).Text = "[دریافت خودکار ترددها از دستگاه هر]";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(259, 134);
		((Control)labelX4).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(105, 21);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "پــورت دستگاه : ";
		labelX4.TextAlignment = (StringAlignment)2;
		TxtIP.Border.Class = "TextBoxBorder";
		TxtIP.Border.CornerType = (eCornerType)1;
		((Control)TxtIP).Location = new Point(132, 110);
		((Control)TxtIP).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtIP).Name = "TxtIP";
		((Control)TxtIP).Size = new Size(120, 21);
		((Control)TxtIP).TabIndex = 7;
		((Control)TxtIP).TextChanged += TxtIP_TextChanged;
		((Control)TxtIP).Leave += TxtIP_Leave;
		((BaseItemControl)LblCallBack).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblCallBack).Location = new Point(259, 61);
		((Control)LblCallBack).Margin = new Padding(4, 3, 4, 3);
		((Control)LblCallBack).Name = "LblCallBack";
		((Control)LblCallBack).RightToLeft = (RightToLeft)1;
		((Control)LblCallBack).Size = new Size(105, 21);
		((Control)LblCallBack).TabIndex = 0;
		((Control)LblCallBack).Text = "آدرس سرور : ";
		LblCallBack.TextAlignment = (StringAlignment)2;
		((Control)LblCallBack).Visible = false;
		TxtPass.Border.Class = "TextBoxBorder";
		TxtPass.Border.CornerType = (eCornerType)1;
		((Control)TxtPass).Location = new Point(132, 158);
		((Control)TxtPass).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtPass).Name = "TxtPass";
		((Control)TxtPass).Size = new Size(120, 21);
		((Control)TxtPass).TabIndex = 10;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(259, 110);
		((Control)labelX3).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(105, 21);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "آدرس دستگاه : ";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)RbtnLan).Anchor = (AnchorStyles)9;
		((Control)RbtnLan).ForeColor = SystemColors.HotTrack;
		((Control)RbtnLan).Location = new Point(461, 181);
		((Control)RbtnLan).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtnLan).Name = "RbtnLan";
		((Control)RbtnLan).RightToLeft = (RightToLeft)1;
		((Control)RbtnLan).Size = new Size(340, 17);
		((Control)RbtnLan).TabIndex = 6;
		((Control)RbtnLan).Text = "[ارتباط از طریق شبکه داخلی]";
		((ButtonBase)RbtnLan).UseVisualStyleBackColor = true;
		RbtnLan.CheckedChanged += RbtnConnectionTypes_CheckedChanged;
		((Control)TxtDesc).Anchor = (AnchorStyles)9;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(480, 64);
		((Control)TxtDesc).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((Control)TxtDesc).Size = new Size(254, 21);
		((Control)TxtDesc).TabIndex = 2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(742, 9);
		((Control)labelX5).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(98, 21);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[شماره] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)RbtnUsb).Anchor = (AnchorStyles)9;
		RbtnUsb.Checked = true;
		((Control)RbtnUsb).ForeColor = SystemColors.HotTrack;
		((Control)RbtnUsb).Location = new Point(461, 134);
		((Control)RbtnUsb).Margin = new Padding(4, 3, 4, 3);
		((Control)RbtnUsb).Name = "RbtnUsb";
		((Control)RbtnUsb).RightToLeft = (RightToLeft)1;
		((Control)RbtnUsb).Size = new Size(340, 17);
		((Control)RbtnUsb).TabIndex = 4;
		RbtnUsb.TabStop = true;
		((Control)RbtnUsb).Text = "[ارتباط از طریق USB]";
		((ButtonBase)RbtnUsb).UseVisualStyleBackColor = true;
		RbtnUsb.CheckedChanged += RbtnConnectionTypes_CheckedChanged;
		((Control)BtnRecieve).AccessibleRole = (AccessibleRole)43;
		((Control)BtnRecieve).Anchor = (AnchorStyles)10;
		BtnRecieve.ColorTable = (eButtonColor)6;
		((Control)BtnRecieve).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		BtnRecieve.Image = (Image)(object)Resources.GetTaradods;
		BtnRecieve.ImageFixedSize = new Size(30, 30);
		BtnRecieve.ImagePosition = (eImagePosition)1;
		((Control)BtnRecieve).Location = new Point(631, 569);
		((Control)BtnRecieve).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnRecieve).Name = "BtnRecieve";
		((Control)BtnRecieve).RightToLeft = (RightToLeft)1;
		((Control)BtnRecieve).Size = new Size(174, 34);
		((PopupItemControl)BtnRecieve).Style = (eDotNetBarStyle)9;
		((Control)BtnRecieve).TabIndex = 15;
		((Control)BtnRecieve).Text = "    [دریافت اطلاعات]";
		BtnRecieve.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnRecieve).Click += BtnRecieve_Click;
		((Control)BtnTestConnection).AccessibleRole = (AccessibleRole)43;
		((Control)BtnTestConnection).Anchor = (AnchorStyles)10;
		BtnTestConnection.ColorTable = (eButtonColor)6;
		((Control)BtnTestConnection).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		BtnTestConnection.Image = (Image)(object)Resources.deviceConnect;
		BtnTestConnection.ImageFixedSize = new Size(35, 30);
		BtnTestConnection.ImagePosition = (eImagePosition)1;
		((Control)BtnTestConnection).Location = new Point(461, 569);
		((Control)BtnTestConnection).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnTestConnection).Name = "BtnTestConnection";
		((Control)BtnTestConnection).RightToLeft = (RightToLeft)1;
		((Control)BtnTestConnection).Size = new Size(162, 34);
		((PopupItemControl)BtnTestConnection).Style = (eDotNetBarStyle)9;
		((Control)BtnTestConnection).TabIndex = 16;
		((Control)BtnTestConnection).Text = "    [تست ارتباط]";
		BtnTestConnection.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnTestConnection).Click += BtnTestConnection_Click;
		((Control)pnlColor).Anchor = (AnchorStyles)14;
		pnlColor.CanvasColor = SystemColors.Control;
		pnlColor.ColorSchemeStyle = (eDotNetBarStyle)9;
		pnlColor.DisabledBackColor = Color.Empty;
		((Control)pnlColor).Location = new Point(466, 528);
		((Control)pnlColor).Margin = new Padding(0);
		((Control)pnlColor).Name = "pnlColor";
		((Control)pnlColor).RightToLeft = (RightToLeft)1;
		((Control)pnlColor).Size = new Size(353, 30);
		pnlColor.Style.Alignment = (StringAlignment)1;
		pnlColor.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		pnlColor.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		pnlColor.Style.BackgroundImagePosition = (eBackgroundImagePosition)4;
		pnlColor.Style.Border = (eBorderType)1;
		pnlColor.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		pnlColor.Style.CornerDiameter = 5;
		pnlColor.Style.CornerType = (eCornerType)2;
		pnlColor.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		pnlColor.Style.GradientAngle = 90;
		((Control)pnlColor).TabIndex = 26;
		((Control)pnlColor).Text = "انتخاب رنگ جهت تفکیک تردد های این دستگاه در فرم گزارش تردد";
		((Control)pnlColor).Click += pnlColor_Click;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(742, 61);
		((Control)labelX9).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).RightToLeft = (RightToLeft)1;
		((Control)labelX9).Size = new Size(98, 23);
		((Control)labelX9).TabIndex = 0;
		((Control)labelX9).Text = "[شرح] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)14;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImageFixedSize = new Size(30, 30);
		BtnAccept.ImagePosition = (eImagePosition)1;
		BtnAccept.ImageTextSpacing = 2;
		((Control)BtnAccept).Location = new Point(155, 569);
		((Control)BtnAccept).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(120, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 17;
		((Control)BtnAccept).TabStop = false;
		((Control)BtnAccept).Text = "    [تائید] (F5)   ";
		BtnAccept.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImageFixedSize = new Size(32, 32);
		BtnCancel.ImagePosition = (eImagePosition)1;
		BtnCancel.ImageTextSpacing = 2;
		((Control)BtnCancel).Location = new Point(12, 569);
		((Control)BtnCancel).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(120, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 18;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "    [لغو] (Esc)";
		BtnCancel.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)TxtName).Anchor = (AnchorStyles)9;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(480, 37);
		((Control)TxtName).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtName).Name = "TxtName";
		((Control)TxtName).Size = new Size(254, 21);
		((Control)TxtName).TabIndex = 1;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(742, 35);
		((Control)labelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(98, 23);
		((Control)labelX1).TabIndex = 0;
		((Control)labelX1).Text = "[نام دستگاه] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(852, 616);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmDevice";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[دستگاه]";
		((Form)this).Load += FrmDevice_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMDevice_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)PbColorTaradod).EndInit();
		((ISupportInitialize)pictureBoxLoading).EndInit();
		((Control)flowLayoutPanel1).ResumeLayout(false);
		((Control)flowLayoutPanel1).PerformLayout();
		((ISupportInitialize)numCodeDevice).EndInit();
		((Control)PanelTcp).ResumeLayout(false);
		((Control)PanelTcp).PerformLayout();
		((ISupportInitialize)infoBoxCallBack).EndInit();
		((ISupportInitialize)NumCount).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
