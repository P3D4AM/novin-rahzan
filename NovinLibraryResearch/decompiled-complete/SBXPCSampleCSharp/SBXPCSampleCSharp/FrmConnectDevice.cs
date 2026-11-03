using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class FrmConnectDevice : Form
{
	private FrmMonitoringEbio frmMonitoringEbio;

	private bool mOpenFlag;

	public AxSBXPC bpc = new AxSBXPC();

	private IContainer components = null;

	public GroupBox groupBox1;

	public Button cmdEventMonitor;

	public Button cmdLogTz;

	public Button cmdScreenSaver;

	public Button cmdProxy;

	public Button cmdAccessTz;

	public Button cmdDepartment;

	public Button cmdLockCtl;

	public Button cmdSystemInfo;

	public Button cmdBellInfo;

	public Button cmdExit;

	public Button cmdLogData;

	public Button cmdEnrollData;

	public Button cmdProuctCode;

	public Label Label1;

	public Label lbSubject;

	public GroupBox Frame2;

	public TextBox txtDevId;

	public TextBox txtPassword;

	public TextBox txtIPAddress;

	public TextBox txtPortNo;

	public Label label2;

	public RadioButton optNetworkDevice;

	public Label lblPassword;

	public Label lblIPAddress;

	public Label lblPortNo;

	public RadioButton optUSBDevice;

	public RadioButton optSerialDevice;

	public GroupBox Frame4;

	public Button cmdOpen;

	public Button cmdClose;

	public ComboBox cmbMachineNumber;

	public Label lblMachineNumber;

	public GroupBox Frame3;

	public ComboBox cmbBaudrate;

	public ComboBox cmbComPort;

	public Label lblBaudrate;

	public Label lblComPort;

	public FrmConnectDevice()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmConnectDevice));
		InitializeComponent();
		bpc = new AxSBXPC();
		((ISupportInitialize)bpc).BeginInit();
		((AxHost)bpc).Enabled = true;
		((Control)bpc).Location = new Point(10, 9);
		((Control)bpc).Name = "bpc";
		((AxHost)bpc).OcxState = (State)componentResourceManager.GetObject("bpc.OcxState");
		((Control)bpc).Size = new Size(100, 50);
		((Control)bpc).TabIndex = 37;
		bpc.OnReceiveEventXML += SBXPC1_OnReceiveEventXML;
		((ISupportInitialize)bpc).EndInit();
		((Control)this).Controls.Add((Control)(object)bpc);
		frmMonitoringEbio = new FrmMonitoringEbio();
	}

	private void cmdOpen_Click(object sender, EventArgs e)
	{
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		util.gMachineNumber = Convert.ToInt32(((Control)cmbMachineNumber).Text);
		if (optNetworkDevice.Checked)
		{
			string lpszIPAddress = ((Control)txtIPAddress).Text;
			string lpszMachineID = ((Control)txtDevId).Text;
			if (lpszMachineID.Length >= 6 && lpszMachineID.Length <= 16)
			{
				int pnError = 0;
				util.gMachineNumber = bpc.ConnectP2p(ref lpszMachineID, ref lpszIPAddress, Convert.ToInt32(((Control)txtPortNo).Text), Convert.ToInt32(((Control)txtPassword).Text), ref pnError);
				if (util.gMachineNumber != 0)
				{
					mOpenFlag = true;
					((Control)cmdOpen).Enabled = false;
					((Control)cmdClose).Enabled = true;
					((Control)cmdEnrollData).Enabled = true;
					((Control)cmdLogData).Enabled = true;
					((Control)cmdSystemInfo).Enabled = true;
					((Control)cmdProuctCode).Enabled = true;
					((Control)cmdBellInfo).Enabled = true;
					((Control)cmdLockCtl).Enabled = true;
					((Control)cmdAccessTz).Enabled = true;
					((Control)cmdLogTz).Enabled = true;
					((Control)cmdDepartment).Enabled = true;
					((Control)cmdProxy).Enabled = true;
					((Control)cmdScreenSaver).Enabled = true;
					((Control)cmdEventMonitor).Enabled = true;
					switch (pnError)
					{
					case 4:
						MessageBox.Show("Relayed Connection!");
						break;
					case 5:
						MessageBox.Show("Direct Local Connection!");
						break;
					}
				}
				else
				{
					switch (pnError)
					{
					case 1:
						MessageBox.Show("Cannot Connect To Server!");
						break;
					case 2:
						MessageBox.Show("Device Not Found!");
						break;
					case 3:
						MessageBox.Show("Password Mismatched!");
						break;
					default:
						MessageBox.Show("Unknown Error!");
						break;
					}
				}
			}
			else if (bpc.ConnectTcpip(util.gMachineNumber, ref lpszIPAddress, Convert.ToInt32(((Control)txtPortNo).Text), Convert.ToInt32(((Control)txtPassword).Text)))
			{
				mOpenFlag = true;
				((Control)cmdOpen).Enabled = false;
				((Control)cmdClose).Enabled = true;
				((Control)cmdEnrollData).Enabled = true;
				((Control)cmdLogData).Enabled = true;
				((Control)cmdSystemInfo).Enabled = true;
				((Control)cmdProuctCode).Enabled = true;
				((Control)cmdBellInfo).Enabled = true;
				((Control)cmdLockCtl).Enabled = true;
				((Control)cmdAccessTz).Enabled = true;
				((Control)cmdLogTz).Enabled = true;
				((Control)cmdDepartment).Enabled = true;
				((Control)cmdProxy).Enabled = true;
				((Control)cmdScreenSaver).Enabled = true;
				((Control)cmdEventMonitor).Enabled = true;
			}
		}
		if (optSerialDevice.Checked && bpc.ConnectSerial(util.gMachineNumber, ((ListControl)cmbComPort).SelectedIndex + 1, Convert.ToInt32(((Control)cmbBaudrate).Text)))
		{
			mOpenFlag = true;
			((Control)cmdOpen).Enabled = false;
			((Control)cmdClose).Enabled = true;
			((Control)cmdEnrollData).Enabled = true;
			((Control)cmdLogData).Enabled = true;
			((Control)cmdSystemInfo).Enabled = true;
			((Control)cmdProuctCode).Enabled = true;
			((Control)cmdBellInfo).Enabled = true;
			((Control)cmdLockCtl).Enabled = true;
			((Control)cmdAccessTz).Enabled = true;
			((Control)cmdLogTz).Enabled = true;
			((Control)cmdDepartment).Enabled = true;
			((Control)cmdProxy).Enabled = true;
			((Control)cmdScreenSaver).Enabled = true;
			((Control)cmdEventMonitor).Enabled = true;
		}
		if (optUSBDevice.Checked && bpc.ConnectSerial(util.gMachineNumber, 0, 0))
		{
			mOpenFlag = true;
			((Control)cmdOpen).Enabled = false;
			((Control)cmdClose).Enabled = true;
			((Control)cmdEnrollData).Enabled = true;
			((Control)cmdLogData).Enabled = true;
			((Control)cmdSystemInfo).Enabled = true;
			((Control)cmdProuctCode).Enabled = true;
			((Control)cmdBellInfo).Enabled = true;
			((Control)cmdLockCtl).Enabled = true;
			((Control)cmdAccessTz).Enabled = true;
			((Control)cmdLogTz).Enabled = true;
			((Control)cmdDepartment).Enabled = true;
			((Control)cmdProxy).Enabled = true;
			((Control)cmdScreenSaver).Enabled = true;
			((Control)cmdEventMonitor).Enabled = true;
		}
	}

	public bool ConnectTcpIp(int machineNumber, int portNo, int psssword, string ip)
	{
		string lpszIPAddress = ip;
		try
		{
			util.gMachineNumber = Convert.ToInt32(((Control)cmbMachineNumber).Text);
			if (bpc.ConnectTcpip(util.gMachineNumber, ref lpszIPAddress, portNo, psssword))
			{
				frmEnroll frmEnroll2 = new frmEnroll(bpc);
				frmEnroll2.cmdGetAllEnrollData_Click(null, null);
				return true;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	private void FrmMain_Load(object sender, EventArgs e)
	{
		optSerialDevice.Checked = false;
		((Control)lblComPort).Enabled = false;
		((Control)cmbComPort).Enabled = false;
		((Control)lblBaudrate).Enabled = false;
		((Control)cmbBaudrate).Enabled = false;
		optNetworkDevice.Checked = true;
		((Control)lblIPAddress).Enabled = true;
		((Control)txtIPAddress).Enabled = true;
		((Control)lblPortNo).Enabled = true;
		((Control)txtPortNo).Enabled = true;
		((Control)lblPassword).Enabled = true;
		((Control)txtPassword).Enabled = true;
		((Control)cmdOpen).Enabled = true;
		((Control)cmdClose).Enabled = false;
		((Control)cmdEnrollData).Enabled = false;
		((Control)cmdLogData).Enabled = false;
		((Control)cmdSystemInfo).Enabled = false;
		((Control)cmdProuctCode).Enabled = false;
		((Control)cmdBellInfo).Enabled = false;
		((Control)cmdLockCtl).Enabled = false;
		((Control)cmdAccessTz).Enabled = false;
		((Control)cmdLogTz).Enabled = false;
		((Control)cmdDepartment).Enabled = false;
		((Control)cmdProxy).Enabled = false;
		((Control)cmdScreenSaver).Enabled = false;
		((Control)cmdEventMonitor).Enabled = false;
		mOpenFlag = false;
		((Control)cmbMachineNumber).Text = Convert.ToString(1);
		((Control)cmbComPort).Text = Convert.ToString(1);
		((Control)cmbBaudrate).Text = "115200";
		bpc.DotNET();
	}

	private void cmdClose_Click(object sender, EventArgs e)
	{
		if (mOpenFlag)
		{
			bpc.Disconnect();
			mOpenFlag = false;
			((Control)cmdOpen).Enabled = true;
			((Control)cmdClose).Enabled = false;
			((Control)cmdEnrollData).Enabled = false;
			((Control)cmdLogData).Enabled = false;
			((Control)cmdSystemInfo).Enabled = false;
			((Control)cmdProuctCode).Enabled = false;
			((Control)cmdBellInfo).Enabled = false;
			((Control)cmdLockCtl).Enabled = false;
			((Control)cmdAccessTz).Enabled = false;
			((Control)cmdLogTz).Enabled = false;
			((Control)cmdDepartment).Enabled = false;
			((Control)cmdProxy).Enabled = false;
			((Control)cmdScreenSaver).Enabled = false;
			((Control)cmdEventMonitor).Enabled = false;
		}
	}

	private void optSerialDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (optSerialDevice.Checked)
		{
			optSerialDevice.Checked = true;
			optNetworkDevice.Checked = false;
			optUSBDevice.Checked = false;
			if (optSerialDevice.Checked)
			{
				((Control)lblComPort).Enabled = true;
				((Control)cmbComPort).Enabled = true;
				((Control)lblBaudrate).Enabled = true;
				((Control)cmbBaudrate).Enabled = true;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
			else if (optNetworkDevice.Checked)
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = true;
				((Control)txtIPAddress).Enabled = true;
				((Control)lblPortNo).Enabled = true;
				((Control)txtPortNo).Enabled = true;
				((Control)lblPassword).Enabled = true;
				((Control)txtPassword).Enabled = true;
				string text = ((Control)txtIPAddress).Text;
			}
			else
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
		}
	}

	private void optNetworkDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (optNetworkDevice.Checked)
		{
			optSerialDevice.Checked = false;
			optNetworkDevice.Checked = true;
			optUSBDevice.Checked = false;
			if (optSerialDevice.Checked)
			{
				((Control)lblComPort).Enabled = true;
				((Control)cmbComPort).Enabled = true;
				((Control)lblBaudrate).Enabled = true;
				((Control)cmbBaudrate).Enabled = true;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
			else if (optNetworkDevice.Checked)
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = true;
				((Control)txtIPAddress).Enabled = true;
				((Control)lblPortNo).Enabled = true;
				((Control)txtPortNo).Enabled = true;
				((Control)lblPassword).Enabled = true;
				((Control)txtPassword).Enabled = true;
				string text = ((Control)txtIPAddress).Text;
			}
			else
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
		}
	}

	private void optUSBDevice_CheckedChanged(object sender, EventArgs e)
	{
		if (optUSBDevice.Checked)
		{
			optSerialDevice.Checked = false;
			optNetworkDevice.Checked = false;
			optUSBDevice.Checked = true;
			if (optSerialDevice.Checked)
			{
				((Control)lblComPort).Enabled = true;
				((Control)cmbComPort).Enabled = true;
				((Control)lblBaudrate).Enabled = true;
				((Control)cmbBaudrate).Enabled = true;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
			else if (optNetworkDevice.Checked)
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = true;
				((Control)txtIPAddress).Enabled = true;
				((Control)lblPortNo).Enabled = true;
				((Control)txtPortNo).Enabled = true;
				((Control)lblPassword).Enabled = true;
				((Control)txtPassword).Enabled = true;
				string text = ((Control)txtIPAddress).Text;
			}
			else
			{
				((Control)lblComPort).Enabled = false;
				((Control)cmbComPort).Enabled = false;
				((Control)lblBaudrate).Enabled = false;
				((Control)cmbBaudrate).Enabled = false;
				((Control)lblIPAddress).Enabled = false;
				((Control)txtIPAddress).Enabled = false;
				((Control)lblPortNo).Enabled = false;
				((Control)txtPortNo).Enabled = false;
				((Control)lblPassword).Enabled = false;
				((Control)txtPassword).Enabled = false;
			}
		}
	}

	private void cmdProuctCode_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmPrtCode frmPrtCode2 = new frmPrtCode(bpc);
		((Form)frmPrtCode2).ShowDialog();
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		if (mOpenFlag)
		{
			bpc.Disconnect();
		}
		((Form)this).Close();
	}

	private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void cmdSystemInfo_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmSystemInfo frmSystemInfo2 = new frmSystemInfo(bpc);
		((Form)frmSystemInfo2).ShowDialog();
	}

	private void cmdLockCtl_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmLockCtrl frmLockCtrl2 = new frmLockCtrl(bpc);
		((Form)frmLockCtrl2).ShowDialog();
	}

	private void cmdBellInfo_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmBellInfo frmBellInfo2 = new frmBellInfo(bpc);
		((Form)frmBellInfo2).ShowDialog();
	}

	private void cmdLogData_Click(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		FormlogNew formlogNew = new FormlogNew(bpc, util.gMachineNumber);
		((Form)formlogNew).ShowDialog((IWin32Window)(object)this);
	}

	private void cmdEnrollData_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmEnroll frmEnroll2 = new frmEnroll(bpc);
		((Form)frmEnroll2).ShowDialog();
	}

	private void cmdScreenSaver_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmScreenSaver frmScreenSaver2 = new frmScreenSaver(bpc);
		((Form)frmScreenSaver2).ShowDialog();
	}

	private void cmdDepartment_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmDepartment frmDepartment2 = new frmDepartment(bpc);
		((Form)frmDepartment2).ShowDialog();
	}

	private void cmdProxy_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmDaigong frmDaigong2 = new frmDaigong(bpc);
		((Form)frmDaigong2).ShowDialog();
	}

	private void cmdLogTz_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmTrMode frmTrMode2 = new frmTrMode(bpc);
		((Form)frmTrMode2).ShowDialog();
	}

	private void cmdAccessTz_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		frmAccessTz frmAccessTz2 = new frmAccessTz(bpc);
		((Form)frmAccessTz2).ShowDialog();
	}

	private void SBXPC1_OnReceiveEventXML(object sender, _DSBXPCEvents_OnReceiveEventXMLEvent e)
	{
	}

	private void cmdEventMoniter_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Form)frmMonitoringEbio).ShowDialog();
	}

	private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			((Form)frmMonitoringEbio).Close();
			if (((Control)cmdClose).Enabled && ((Control)cmdClose).Visible)
			{
				cmdClose.PerformClick();
			}
		}
		catch
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_0792: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Expected O, but got Unknown
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Expected O, but got Unknown
		//IL_0b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Expected O, but got Unknown
		//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Expected O, but got Unknown
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Expected O, but got Unknown
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dba: Expected O, but got Unknown
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e99: Expected O, but got Unknown
		//IL_0f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f85: Expected O, but got Unknown
		//IL_1050: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Expected O, but got Unknown
		//IL_1207: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b2: Expected O, but got Unknown
		//IL_136f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1379: Expected O, but got Unknown
		//IL_1441: Unknown result type (might be due to invalid IL or missing references)
		//IL_144b: Expected O, but got Unknown
		//IL_1512: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Expected O, but got Unknown
		//IL_15e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ed: Expected O, but got Unknown
		//IL_16a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b2: Expected O, but got Unknown
		//IL_17a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ac: Expected O, but got Unknown
		//IL_1867: Unknown result type (might be due to invalid IL or missing references)
		//IL_1871: Expected O, but got Unknown
		//IL_192c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Expected O, but got Unknown
		//IL_19e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ee: Expected O, but got Unknown
		//IL_1ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adb: Expected O, but got Unknown
		//IL_1c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c13: Expected O, but got Unknown
		//IL_1c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ced: Expected O, but got Unknown
		//IL_1dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcd: Expected O, but got Unknown
		//IL_1eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eba: Expected O, but got Unknown
		//IL_1fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd1: Expected O, but got Unknown
		//IL_20f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2196: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a0: Expected O, but got Unknown
		//IL_2289: Unknown result type (might be due to invalid IL or missing references)
		//IL_2293: Expected O, but got Unknown
		//IL_2394: Unknown result type (might be due to invalid IL or missing references)
		//IL_239e: Expected O, but got Unknown
		//IL_2466: Unknown result type (might be due to invalid IL or missing references)
		//IL_2470: Expected O, but got Unknown
		//IL_25da: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e4: Expected O, but got Unknown
		//IL_25ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f7: Expected O, but got Unknown
		groupBox1 = new GroupBox();
		cmdEventMonitor = new Button();
		cmdLogTz = new Button();
		cmdScreenSaver = new Button();
		cmdProxy = new Button();
		cmdAccessTz = new Button();
		cmdDepartment = new Button();
		cmdLockCtl = new Button();
		cmdSystemInfo = new Button();
		cmdBellInfo = new Button();
		cmdExit = new Button();
		cmdLogData = new Button();
		cmdEnrollData = new Button();
		cmdProuctCode = new Button();
		Label1 = new Label();
		lbSubject = new Label();
		Frame2 = new GroupBox();
		txtDevId = new TextBox();
		txtPassword = new TextBox();
		txtIPAddress = new TextBox();
		txtPortNo = new TextBox();
		label2 = new Label();
		optNetworkDevice = new RadioButton();
		lblPassword = new Label();
		lblIPAddress = new Label();
		lblPortNo = new Label();
		optUSBDevice = new RadioButton();
		optSerialDevice = new RadioButton();
		Frame4 = new GroupBox();
		cmdOpen = new Button();
		cmdClose = new Button();
		cmbMachineNumber = new ComboBox();
		lblMachineNumber = new Label();
		Frame3 = new GroupBox();
		cmbBaudrate = new ComboBox();
		cmbComPort = new ComboBox();
		lblBaudrate = new Label();
		lblComPort = new Label();
		((Control)groupBox1).SuspendLayout();
		((Control)Frame2).SuspendLayout();
		((Control)Frame4).SuspendLayout();
		((Control)Frame3).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBox1).BackColor = SystemColors.Control;
		((Control)groupBox1).Controls.Add((Control)(object)cmdEventMonitor);
		((Control)groupBox1).Controls.Add((Control)(object)cmdLogTz);
		((Control)groupBox1).Controls.Add((Control)(object)cmdScreenSaver);
		((Control)groupBox1).Controls.Add((Control)(object)cmdProxy);
		((Control)groupBox1).Controls.Add((Control)(object)cmdAccessTz);
		((Control)groupBox1).Controls.Add((Control)(object)cmdDepartment);
		((Control)groupBox1).Controls.Add((Control)(object)cmdLockCtl);
		((Control)groupBox1).Controls.Add((Control)(object)cmdSystemInfo);
		((Control)groupBox1).Controls.Add((Control)(object)cmdBellInfo);
		((Control)groupBox1).Controls.Add((Control)(object)cmdExit);
		((Control)groupBox1).Controls.Add((Control)(object)cmdLogData);
		((Control)groupBox1).Controls.Add((Control)(object)cmdEnrollData);
		((Control)groupBox1).Controls.Add((Control)(object)cmdProuctCode);
		((Control)groupBox1).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupBox1).ForeColor = SystemColors.ControlText;
		((Control)groupBox1).Location = new Point(262, 158);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Padding = new Padding(0);
		((Control)groupBox1).RightToLeft = (RightToLeft)0;
		((Control)groupBox1).Size = new Size(272, 313);
		((Control)groupBox1).TabIndex = 30;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Management Group";
		((Control)cmdEventMonitor).BackColor = SystemColors.Control;
		((Control)cmdEventMonitor).Cursor = Cursors.Default;
		((Control)cmdEventMonitor).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEventMonitor).ForeColor = SystemColors.ControlText;
		((Control)cmdEventMonitor).Location = new Point(135, 224);
		((Control)cmdEventMonitor).Name = "cmdEventMonitor";
		((Control)cmdEventMonitor).RightToLeft = (RightToLeft)0;
		((Control)cmdEventMonitor).Size = new Size(117, 32);
		((Control)cmdEventMonitor).TabIndex = 33;
		((Control)cmdEventMonitor).Text = "Event Monitor";
		((ButtonBase)cmdEventMonitor).UseVisualStyleBackColor = false;
		((Control)cmdEventMonitor).Click += cmdEventMoniter_Click;
		((Control)cmdLogTz).BackColor = SystemColors.Control;
		((Control)cmdLogTz).Cursor = Cursors.Default;
		((Control)cmdLogTz).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdLogTz).ForeColor = SystemColors.ControlText;
		((Control)cmdLogTz).Location = new Point(135, 122);
		((Control)cmdLogTz).Name = "cmdLogTz";
		((Control)cmdLogTz).RightToLeft = (RightToLeft)0;
		((Control)cmdLogTz).Size = new Size(117, 32);
		((Control)cmdLogTz).TabIndex = 32;
		((Control)cmdLogTz).Text = "Log TZone";
		((ButtonBase)cmdLogTz).UseVisualStyleBackColor = false;
		((Control)cmdLogTz).Click += cmdLogTz_Click;
		((Control)cmdScreenSaver).BackColor = SystemColors.Control;
		((Control)cmdScreenSaver).Cursor = Cursors.Default;
		((Control)cmdScreenSaver).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdScreenSaver).ForeColor = SystemColors.ControlText;
		((Control)cmdScreenSaver).Location = new Point(16, 224);
		((Control)cmdScreenSaver).Name = "cmdScreenSaver";
		((Control)cmdScreenSaver).RightToLeft = (RightToLeft)0;
		((Control)cmdScreenSaver).Size = new Size(113, 32);
		((Control)cmdScreenSaver).TabIndex = 31;
		((Control)cmdScreenSaver).Text = "Screen Saver";
		((ButtonBase)cmdScreenSaver).UseVisualStyleBackColor = false;
		((Control)cmdScreenSaver).Click += cmdScreenSaver_Click;
		((Control)cmdProxy).BackColor = SystemColors.Control;
		((Control)cmdProxy).Cursor = Cursors.Default;
		((Control)cmdProxy).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdProxy).ForeColor = SystemColors.ControlText;
		((Control)cmdProxy).Location = new Point(135, 188);
		((Control)cmdProxy).Name = "cmdProxy";
		((Control)cmdProxy).RightToLeft = (RightToLeft)0;
		((Control)cmdProxy).Size = new Size(117, 32);
		((Control)cmdProxy).TabIndex = 30;
		((Control)cmdProxy).Text = "Daigong";
		((ButtonBase)cmdProxy).UseVisualStyleBackColor = false;
		((Control)cmdProxy).Click += cmdProxy_Click;
		((Control)cmdAccessTz).BackColor = SystemColors.Control;
		((Control)cmdAccessTz).Cursor = Cursors.Default;
		((Control)cmdAccessTz).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdAccessTz).ForeColor = SystemColors.ControlText;
		((Control)cmdAccessTz).Location = new Point(135, 156);
		((Control)cmdAccessTz).Name = "cmdAccessTz";
		((Control)cmdAccessTz).RightToLeft = (RightToLeft)0;
		((Control)cmdAccessTz).Size = new Size(117, 32);
		((Control)cmdAccessTz).TabIndex = 29;
		((Control)cmdAccessTz).Text = "Access TZone";
		((ButtonBase)cmdAccessTz).UseVisualStyleBackColor = false;
		((Control)cmdAccessTz).Click += cmdAccessTz_Click;
		((Control)cmdDepartment).BackColor = SystemColors.Control;
		((Control)cmdDepartment).Cursor = Cursors.Default;
		((Control)cmdDepartment).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDepartment).ForeColor = SystemColors.ControlText;
		((Control)cmdDepartment).Location = new Point(16, 188);
		((Control)cmdDepartment).Name = "cmdDepartment";
		((Control)cmdDepartment).RightToLeft = (RightToLeft)0;
		((Control)cmdDepartment).Size = new Size(113, 32);
		((Control)cmdDepartment).TabIndex = 27;
		((Control)cmdDepartment).Text = "Department";
		((ButtonBase)cmdDepartment).UseVisualStyleBackColor = false;
		((Control)cmdDepartment).Click += cmdDepartment_Click;
		((Control)cmdLockCtl).BackColor = SystemColors.Control;
		((Control)cmdLockCtl).Cursor = Cursors.Default;
		((Control)cmdLockCtl).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdLockCtl).ForeColor = SystemColors.ControlText;
		((Control)cmdLockCtl).Location = new Point(135, 88);
		((Control)cmdLockCtl).Name = "cmdLockCtl";
		((Control)cmdLockCtl).RightToLeft = (RightToLeft)0;
		((Control)cmdLockCtl).Size = new Size(117, 32);
		((Control)cmdLockCtl).TabIndex = 26;
		((Control)cmdLockCtl).Text = "Lock Control";
		((ButtonBase)cmdLockCtl).UseVisualStyleBackColor = false;
		((Control)cmdLockCtl).Click += cmdLockCtl_Click;
		((Control)cmdSystemInfo).BackColor = SystemColors.Control;
		((Control)cmdSystemInfo).Cursor = Cursors.Default;
		((Control)cmdSystemInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSystemInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdSystemInfo).Location = new Point(16, 88);
		((Control)cmdSystemInfo).Name = "cmdSystemInfo";
		((Control)cmdSystemInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdSystemInfo).Size = new Size(113, 32);
		((Control)cmdSystemInfo).TabIndex = 25;
		((Control)cmdSystemInfo).Text = "System Info";
		((ButtonBase)cmdSystemInfo).UseVisualStyleBackColor = false;
		((Control)cmdSystemInfo).Click += cmdSystemInfo_Click;
		((Control)cmdBellInfo).BackColor = SystemColors.Control;
		((Control)cmdBellInfo).Cursor = Cursors.Default;
		((Control)cmdBellInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdBellInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdBellInfo).Location = new Point(16, 122);
		((Control)cmdBellInfo).Name = "cmdBellInfo";
		((Control)cmdBellInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdBellInfo).Size = new Size(113, 32);
		((Control)cmdBellInfo).TabIndex = 24;
		((Control)cmdBellInfo).Text = "Bell time";
		((ButtonBase)cmdBellInfo).UseVisualStyleBackColor = false;
		((Control)cmdBellInfo).Click += cmdBellInfo_Click;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(16, 278);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(236, 32);
		((Control)cmdExit).TabIndex = 5;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdLogData).BackColor = SystemColors.Control;
		((Control)cmdLogData).Cursor = Cursors.Default;
		((Control)cmdLogData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdLogData).Location = new Point(16, 55);
		((Control)cmdLogData).Name = "cmdLogData";
		((Control)cmdLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdLogData).Size = new Size(236, 32);
		((Control)cmdLogData).TabIndex = 4;
		((Control)cmdLogData).Text = "حذف اطلاعات تردد آنلاین ";
		((ButtonBase)cmdLogData).UseVisualStyleBackColor = false;
		((Control)cmdLogData).Click += cmdLogData_Click;
		((Control)cmdEnrollData).BackColor = SystemColors.Control;
		((Control)cmdEnrollData).Cursor = Cursors.Default;
		((Control)cmdEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdEnrollData).Location = new Point(16, 22);
		((Control)cmdEnrollData).Name = "cmdEnrollData";
		((Control)cmdEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdEnrollData).Size = new Size(236, 32);
		((Control)cmdEnrollData).TabIndex = 3;
		((Control)cmdEnrollData).Text = "Enroll Data Management";
		((ButtonBase)cmdEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdEnrollData).Click += cmdEnrollData_Click;
		((Control)cmdProuctCode).BackColor = SystemColors.Control;
		((Control)cmdProuctCode).Cursor = Cursors.Default;
		((Control)cmdProuctCode).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdProuctCode).ForeColor = SystemColors.ControlText;
		((Control)cmdProuctCode).Location = new Point(16, 156);
		((Control)cmdProuctCode).Name = "cmdProuctCode";
		((Control)cmdProuctCode).RightToLeft = (RightToLeft)0;
		((Control)cmdProuctCode).Size = new Size(113, 32);
		((Control)cmdProuctCode).TabIndex = 2;
		((Control)cmdProuctCode).Text = "Get SN";
		((ButtonBase)cmdProuctCode).UseVisualStyleBackColor = false;
		((Control)cmdProuctCode).Click += cmdProuctCode_Click;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 14.25f, (FontStyle)3, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = Color.Blue;
		((Control)Label1).Location = new Point(229, 58);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(53, 22);
		((Control)Label1).TabIndex = 35;
		((Control)Label1).Text = "F500";
		Label1.TextAlign = (ContentAlignment)2;
		((Control)lbSubject).AutoSize = true;
		((Control)lbSubject).BackColor = Color.Transparent;
		((Control)lbSubject).Cursor = Cursors.Default;
		((Control)lbSubject).Font = new Font("Times New Roman", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lbSubject).ForeColor = Color.Red;
		((Control)lbSubject).Location = new Point(173, 25);
		((Control)lbSubject).Name = "lbSubject";
		((Control)lbSubject).RightToLeft = (RightToLeft)0;
		((Control)lbSubject).Size = new Size(196, 31);
		((Control)lbSubject).TabIndex = 29;
		((Control)lbSubject).Text = "SBXPC Sample";
		lbSubject.TextAlign = (ContentAlignment)2;
		((Control)Frame2).BackColor = SystemColors.Control;
		((Control)Frame2).Controls.Add((Control)(object)txtDevId);
		((Control)Frame2).Controls.Add((Control)(object)txtPassword);
		((Control)Frame2).Controls.Add((Control)(object)txtIPAddress);
		((Control)Frame2).Controls.Add((Control)(object)txtPortNo);
		((Control)Frame2).Controls.Add((Control)(object)label2);
		((Control)Frame2).Controls.Add((Control)(object)optNetworkDevice);
		((Control)Frame2).Controls.Add((Control)(object)lblPassword);
		((Control)Frame2).Controls.Add((Control)(object)lblIPAddress);
		((Control)Frame2).Controls.Add((Control)(object)lblPortNo);
		((Control)Frame2).ForeColor = SystemColors.ControlText;
		((Control)Frame2).Location = new Point(10, 258);
		((Control)Frame2).Name = "Frame2";
		((Control)Frame2).Padding = new Padding(0);
		((Control)Frame2).RightToLeft = (RightToLeft)0;
		((Control)Frame2).Size = new Size(244, 180);
		((Control)Frame2).TabIndex = 31;
		Frame2.TabStop = false;
		((Control)Frame2).Text = "  ";
		txtDevId.AcceptsReturn = true;
		((Control)txtDevId).BackColor = SystemColors.Window;
		((Control)txtDevId).Cursor = Cursors.IBeam;
		((Control)txtDevId).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtDevId).ForeColor = SystemColors.WindowText;
		((Control)txtDevId).Location = new Point(29, 142);
		((TextBoxBase)txtDevId).MaxLength = 0;
		((Control)txtDevId).Name = "txtDevId";
		((Control)txtDevId).RightToLeft = (RightToLeft)0;
		((Control)txtDevId).Size = new Size(199, 26);
		((Control)txtDevId).TabIndex = 17;
		txtPassword.AcceptsReturn = true;
		((Control)txtPassword).BackColor = SystemColors.Window;
		((Control)txtPassword).Cursor = Cursors.IBeam;
		((Control)txtPassword).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtPassword).ForeColor = SystemColors.WindowText;
		((Control)txtPassword).Location = new Point(115, 88);
		((TextBoxBase)txtPassword).MaxLength = 0;
		((Control)txtPassword).Name = "txtPassword";
		((Control)txtPassword).RightToLeft = (RightToLeft)0;
		((Control)txtPassword).Size = new Size(113, 26);
		((Control)txtPassword).TabIndex = 17;
		((Control)txtPassword).Text = "0";
		txtIPAddress.AcceptsReturn = true;
		((Control)txtIPAddress).BackColor = SystemColors.Window;
		((Control)txtIPAddress).Cursor = Cursors.IBeam;
		((Control)txtIPAddress).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtIPAddress).ForeColor = SystemColors.WindowText;
		((Control)txtIPAddress).Location = new Point(115, 24);
		((TextBoxBase)txtIPAddress).MaxLength = 0;
		((Control)txtIPAddress).Name = "txtIPAddress";
		((Control)txtIPAddress).RightToLeft = (RightToLeft)0;
		((Control)txtIPAddress).Size = new Size(113, 26);
		((Control)txtIPAddress).TabIndex = 8;
		((Control)txtIPAddress).Text = "192.168.1.224";
		txtPortNo.AcceptsReturn = true;
		((Control)txtPortNo).BackColor = SystemColors.Window;
		((Control)txtPortNo).Cursor = Cursors.IBeam;
		((Control)txtPortNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtPortNo).ForeColor = SystemColors.WindowText;
		((Control)txtPortNo).Location = new Point(115, 56);
		((TextBoxBase)txtPortNo).MaxLength = 0;
		((Control)txtPortNo).Name = "txtPortNo";
		((Control)txtPortNo).RightToLeft = (RightToLeft)0;
		((Control)txtPortNo).Size = new Size(113, 26);
		((Control)txtPortNo).TabIndex = 7;
		((Control)txtPortNo).Text = "5005";
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.Transparent;
		((Control)label2).Cursor = Cursors.Default;
		((Control)label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(24, 120);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(78, 19);
		((Control)label2).TabIndex = 18;
		((Control)label2).Text = "Device ID :";
		((Control)optNetworkDevice).BackColor = SystemColors.Control;
		optNetworkDevice.Checked = true;
		((Control)optNetworkDevice).Cursor = Cursors.Default;
		((Control)optNetworkDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)optNetworkDevice).ForeColor = SystemColors.ControlText;
		((Control)optNetworkDevice).Location = new Point(13, -2);
		((Control)optNetworkDevice).Name = "optNetworkDevice";
		((Control)optNetworkDevice).RightToLeft = (RightToLeft)0;
		((Control)optNetworkDevice).Size = new Size(147, 20);
		((Control)optNetworkDevice).TabIndex = 14;
		optNetworkDevice.TabStop = true;
		((Control)optNetworkDevice).Text = "Network Device";
		((ButtonBase)optNetworkDevice).UseVisualStyleBackColor = false;
		((Control)optNetworkDevice).Click += optNetworkDevice_CheckedChanged;
		((Control)lblPassword).AutoSize = true;
		((Control)lblPassword).BackColor = Color.Transparent;
		((Control)lblPassword).Cursor = Cursors.Default;
		((Control)lblPassword).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblPassword).ForeColor = SystemColors.ControlText;
		((Control)lblPassword).Location = new Point(24, 88);
		((Control)lblPassword).Name = "lblPassword";
		((Control)lblPassword).RightToLeft = (RightToLeft)0;
		((Control)lblPassword).Size = new Size(76, 19);
		((Control)lblPassword).TabIndex = 18;
		((Control)lblPassword).Text = "Password :";
		((Control)lblIPAddress).AutoSize = true;
		((Control)lblIPAddress).BackColor = Color.Transparent;
		((Control)lblIPAddress).Cursor = Cursors.Default;
		((Control)lblIPAddress).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblIPAddress).ForeColor = SystemColors.ControlText;
		((Control)lblIPAddress).Location = new Point(24, 24);
		((Control)lblIPAddress).Name = "lblIPAddress";
		((Control)lblIPAddress).RightToLeft = (RightToLeft)0;
		((Control)lblIPAddress).Size = new Size(83, 19);
		((Control)lblIPAddress).TabIndex = 10;
		((Control)lblIPAddress).Text = "Ip Address :";
		((Control)lblPortNo).AutoSize = true;
		((Control)lblPortNo).BackColor = Color.Transparent;
		((Control)lblPortNo).Cursor = Cursors.Default;
		((Control)lblPortNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblPortNo).ForeColor = SystemColors.ControlText;
		((Control)lblPortNo).Location = new Point(16, 56);
		((Control)lblPortNo).Name = "lblPortNo";
		((Control)lblPortNo).RightToLeft = (RightToLeft)0;
		((Control)lblPortNo).Size = new Size(96, 19);
		((Control)lblPortNo).TabIndex = 9;
		((Control)lblPortNo).Text = "Port Number :";
		((Control)optUSBDevice).BackColor = SystemColors.Control;
		((Control)optUSBDevice).Cursor = Cursors.Default;
		((Control)optUSBDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)optUSBDevice).ForeColor = SystemColors.ControlText;
		((Control)optUSBDevice).Location = new Point(23, 444);
		((Control)optUSBDevice).Name = "optUSBDevice";
		((Control)optUSBDevice).RightToLeft = (RightToLeft)0;
		((Control)optUSBDevice).Size = new Size(123, 27);
		((Control)optUSBDevice).TabIndex = 33;
		optUSBDevice.TabStop = true;
		((Control)optUSBDevice).Text = "USB Device";
		((ButtonBase)optUSBDevice).UseVisualStyleBackColor = false;
		((Control)optUSBDevice).Click += optUSBDevice_CheckedChanged;
		((Control)optSerialDevice).BackColor = SystemColors.Control;
		((Control)optSerialDevice).Cursor = Cursors.Default;
		((Control)optSerialDevice).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)optSerialDevice).ForeColor = SystemColors.ControlText;
		((Control)optSerialDevice).Location = new Point(23, 159);
		((Control)optSerialDevice).Name = "optSerialDevice";
		((Control)optSerialDevice).RightToLeft = (RightToLeft)0;
		((Control)optSerialDevice).Size = new Size(123, 27);
		((Control)optSerialDevice).TabIndex = 34;
		optSerialDevice.TabStop = true;
		((Control)optSerialDevice).Text = "Serial Device";
		((ButtonBase)optSerialDevice).UseVisualStyleBackColor = false;
		((Control)optSerialDevice).Click += optSerialDevice_CheckedChanged;
		((Control)Frame4).BackColor = SystemColors.Control;
		((Control)Frame4).Controls.Add((Control)(object)cmdOpen);
		((Control)Frame4).Controls.Add((Control)(object)cmdClose);
		((Control)Frame4).Controls.Add((Control)(object)cmbMachineNumber);
		((Control)Frame4).Controls.Add((Control)(object)lblMachineNumber);
		((Control)Frame4).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Frame4).ForeColor = SystemColors.ControlText;
		((Control)Frame4).Location = new Point(10, 88);
		((Control)Frame4).Name = "Frame4";
		((Control)Frame4).Padding = new Padding(0);
		((Control)Frame4).RightToLeft = (RightToLeft)0;
		((Control)Frame4).Size = new Size(524, 67);
		((Control)Frame4).TabIndex = 36;
		Frame4.TabStop = false;
		((Control)Frame4).Text = "Connect";
		((Control)cmdOpen).BackColor = SystemColors.Control;
		((Control)cmdOpen).Cursor = Cursors.Default;
		((Control)cmdOpen).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdOpen).ForeColor = SystemColors.ControlText;
		((Control)cmdOpen).Location = new Point(268, 24);
		((Control)cmdOpen).Name = "cmdOpen";
		((Control)cmdOpen).RightToLeft = (RightToLeft)0;
		((Control)cmdOpen).Size = new Size(91, 32);
		((Control)cmdOpen).TabIndex = 23;
		((Control)cmdOpen).Text = "Open";
		((ButtonBase)cmdOpen).UseVisualStyleBackColor = false;
		((Control)cmdOpen).Click += cmdOpen_Click;
		((Control)cmdClose).BackColor = SystemColors.Control;
		((Control)cmdClose).Cursor = Cursors.Default;
		((Control)cmdClose).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdClose).ForeColor = SystemColors.ControlText;
		((Control)cmdClose).Location = new Point(378, 24);
		((Control)cmdClose).Name = "cmdClose";
		((Control)cmdClose).RightToLeft = (RightToLeft)0;
		((Control)cmdClose).Size = new Size(91, 32);
		((Control)cmdClose).TabIndex = 22;
		((Control)cmdClose).Text = "Close";
		((ButtonBase)cmdClose).UseVisualStyleBackColor = false;
		((Control)cmdClose).Click += cmdClose_Click;
		((Control)cmbMachineNumber).BackColor = SystemColors.Window;
		((Control)cmbMachineNumber).Cursor = Cursors.Default;
		cmbMachineNumber.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbMachineNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbMachineNumber).ForeColor = SystemColors.WindowText;
		cmbMachineNumber.Items.AddRange(new object[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
		((Control)cmbMachineNumber).Location = new Point(139, 25);
		((Control)cmbMachineNumber).Name = "cmbMachineNumber";
		((Control)cmbMachineNumber).RightToLeft = (RightToLeft)0;
		((Control)cmbMachineNumber).Size = new Size(89, 27);
		((Control)cmbMachineNumber).TabIndex = 20;
		((Control)lblMachineNumber).AutoSize = true;
		((Control)lblMachineNumber).BackColor = SystemColors.Control;
		((Control)lblMachineNumber).Cursor = Cursors.Default;
		((Control)lblMachineNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMachineNumber).ForeColor = SystemColors.ControlText;
		((Control)lblMachineNumber).Location = new Point(14, 29);
		((Control)lblMachineNumber).Name = "lblMachineNumber";
		((Control)lblMachineNumber).RightToLeft = (RightToLeft)0;
		((Control)lblMachineNumber).Size = new Size(122, 19);
		((Control)lblMachineNumber).TabIndex = 21;
		((Control)lblMachineNumber).Text = "Machine Number :";
		((Control)Frame3).BackColor = SystemColors.Control;
		((Control)Frame3).Controls.Add((Control)(object)cmbBaudrate);
		((Control)Frame3).Controls.Add((Control)(object)cmbComPort);
		((Control)Frame3).Controls.Add((Control)(object)lblBaudrate);
		((Control)Frame3).Controls.Add((Control)(object)lblComPort);
		((Control)Frame3).ForeColor = SystemColors.ControlText;
		((Control)Frame3).Location = new Point(11, 161);
		((Control)Frame3).Name = "Frame3";
		((Control)Frame3).Padding = new Padding(0);
		((Control)Frame3).RightToLeft = (RightToLeft)0;
		((Control)Frame3).Size = new Size(244, 94);
		((Control)Frame3).TabIndex = 32;
		Frame3.TabStop = false;
		((Control)Frame3).Text = "    ";
		((Control)cmbBaudrate).BackColor = SystemColors.Window;
		((Control)cmbBaudrate).Cursor = Cursors.Default;
		cmbBaudrate.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbBaudrate).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbBaudrate).ForeColor = SystemColors.WindowText;
		cmbBaudrate.Items.AddRange(new object[5] { "9600", "19200", "38400", "57600", "115200" });
		((Control)cmbBaudrate).Location = new Point(112, 61);
		((Control)cmbBaudrate).Name = "cmbBaudrate";
		((Control)cmbBaudrate).RightToLeft = (RightToLeft)0;
		((Control)cmbBaudrate).Size = new Size(113, 27);
		((Control)cmbBaudrate).TabIndex = 29;
		((Control)cmbComPort).BackColor = SystemColors.Window;
		((Control)cmbComPort).Cursor = Cursors.Default;
		cmbComPort.DropDownStyle = (ComboBoxStyle)2;
		((Control)cmbComPort).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbComPort).ForeColor = SystemColors.WindowText;
		cmbComPort.Items.AddRange(new object[8] { "1", "2", "3", "4", "5", "6", "7", "8" });
		((Control)cmbComPort).Location = new Point(112, 31);
		((Control)cmbComPort).Name = "cmbComPort";
		((Control)cmbComPort).RightToLeft = (RightToLeft)0;
		((Control)cmbComPort).Size = new Size(113, 27);
		((Control)cmbComPort).TabIndex = 12;
		((Control)lblBaudrate).AutoSize = true;
		((Control)lblBaudrate).BackColor = Color.Transparent;
		((Control)lblBaudrate).Cursor = Cursors.Default;
		((Control)lblBaudrate).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblBaudrate).ForeColor = SystemColors.ControlText;
		((Control)lblBaudrate).Location = new Point(24, 61);
		((Control)lblBaudrate).Name = "lblBaudrate";
		((Control)lblBaudrate).RightToLeft = (RightToLeft)0;
		((Control)lblBaudrate).Size = new Size(75, 19);
		((Control)lblBaudrate).TabIndex = 28;
		((Control)lblBaudrate).Text = "Baudrate : ";
		lblBaudrate.TextAlign = (ContentAlignment)4;
		((Control)lblComPort).AutoSize = true;
		((Control)lblComPort).BackColor = Color.Transparent;
		((Control)lblComPort).Cursor = Cursors.Default;
		((Control)lblComPort).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblComPort).ForeColor = SystemColors.ControlText;
		((Control)lblComPort).Location = new Point(21, 31);
		((Control)lblComPort).Name = "lblComPort";
		((Control)lblComPort).RightToLeft = (RightToLeft)0;
		((Control)lblComPort).Size = new Size(76, 19);
		((Control)lblComPort).TabIndex = 13;
		((Control)lblComPort).Text = "ComPort : ";
		lblComPort.TextAlign = (ContentAlignment)4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(546, 482);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lbSubject);
		((Control)this).Controls.Add((Control)(object)Frame2);
		((Control)this).Controls.Add((Control)(object)optUSBDevice);
		((Control)this).Controls.Add((Control)(object)optSerialDevice);
		((Control)this).Controls.Add((Control)(object)Frame4);
		((Control)this).Controls.Add((Control)(object)Frame3);
		((Control)this).Name = "FrmConnectDevice";
		((Control)this).Text = "ارتباط با دستگاه حضور و غیاب";
		((Form)this).FormClosing += new FormClosingEventHandler(frmMain_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(frmMain_FormClosed);
		((Form)this).Load += FrmMain_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)Frame2).ResumeLayout(false);
		((Control)Frame2).PerformLayout();
		((Control)Frame4).ResumeLayout(false);
		((Control)Frame4).PerformLayout();
		((Control)Frame3).ResumeLayout(false);
		((Control)Frame3).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
