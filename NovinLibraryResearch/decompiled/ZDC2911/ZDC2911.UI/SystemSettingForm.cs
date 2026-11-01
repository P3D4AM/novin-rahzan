using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.Business;
using ZDC2911.Entity;
using ZDC2911.IConvert;

namespace ZDC2911.UI;

public class SystemSettingForm : Form
{
	private Device device;

	private DeviceConnection deviceConnection;

	private int dataChunk = 0;

	private IContainer components = null;

	private GroupBox groupBox1;

	private ComboBox cbo_Param;

	private Label lbl_Param;

	private Button btn_ParamGet;

	private Button btn_ParamSet;

	private TextBox txt_ParamValue;

	private Label lbl_ParamValue;

	private GroupBox groupBox2;

	private ComboBox cbo_PowerMinute;

	private ComboBox cbo_PowerHour;

	private ComboBox cbo_DN;

	private ComboBox cbo_PowerType;

	private Label label1;

	private Label lbl_Time;

	private Label lbl_DN;

	private Label lbl_PowerType;

	private Label lbl_Status;

	private Button btn_PowerGet;

	private Button btn_PowerSet;

	private GroupBox groupBox3;

	private Button btn_TimeGet;

	private Button btn_TimeSet;

	private GroupBox groupBox4;

	private Button btn_SystemGet;

	private TextBox txt_PwdCount;

	private TextBox txt_CardCount;

	private TextBox txt_ManageCount;

	private TextBox txt_ExitCount;

	private TextBox txt_AdminCount;

	private TextBox txt_FingerCount;

	private Label lbl_PwdCount;

	private Label lbl_CardCount;

	private Label lbl_ManageCount;

	private Label lbl_ExitCount;

	private TextBox txt_UserCount;

	private Label lbl_AdminCount;

	private Label lbl_FingerCount;

	private Label lbl_UserCount;

	private Button btn_MacAddress;

	private Button btn_Reset;

	private TextBox txt_Welcome;

	private Button btn_WelcomeGet;

	private Button btn_WelcomeSet;

	private Label lbl_WelCome;

	private TextBox txt_Standby;

	private Button btn_StandbyGet;

	private Button btn_StandbySet;

	private Label lbl_Standby;

	private TextBox txt_DeviceTime;

	private Label lbl_DeviceTime;

	private GroupBox groupBox5;

	private TextBox txt_HisManageCount;

	private TextBox txt_HisExitCount;

	private Label lbl_HisManageCount;

	private Label lbl_HisExitCount;

	private GroupBox groupBox6;

	private Button btn_ModelGet;

	private TextBox txt_Model;

	private Button btn_FirmwareVersionGet;

	private Label lbl_Model;

	private TextBox txt_FirmwareVersion;

	private Label lbl_FirmwareVersion;

	private CheckBox chk_Power;

	private GroupBox groupBox7;

	private Button btn_UploadSound;

	private ComboBox cbo_SoundType;

	private Label lbl_SoundType;

	public SystemSettingForm(DeviceCommEty deviceEty)
	{
		InitializeComponent();
		device = deviceEty.Device;
		deviceConnection = deviceEty.DeviceConnection;
	}

	private void SystemSettingForm_Load(object sender, EventArgs e)
	{
		InitData.InitTimerOnOffDN(cbo_DN);
		InitData.InitHour(cbo_PowerHour);
		InitData.InitMinuteOrSecond(cbo_PowerMinute);
	}

	private void btn_ParamGet_Click(object sender, EventArgs e)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_Param).SelectedIndex)
		{
			MessageBox.Show("Please Select Parameter", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Param).Focus();
			return;
		}
		try
		{
			object obj = new object();
			object obj2 = new object();
			byte[] bytes = BitConverter.GetBytes(((ListControl)cbo_Param).SelectedIndex);
			obj2 = bytes;
			if (deviceConnection.GetProperty((DeviceProperty)26, obj, ref device, ref obj2))
			{
				int intIPAddress = BitConverter.ToInt32((byte[])obj2, 0);
				int selectedIndex = ((ListControl)cbo_Param).SelectedIndex;
				int num = selectedIndex;
				if ((uint)(num - 26) <= 3u)
				{
					((Control)txt_ParamValue).Text = ConvertObject.ConvertNumberToIPAddress(intIPAddress);
				}
				else
				{
					((Control)txt_ParamValue).Text = intIPAddress.ToString();
				}
			}
			else
			{
				MessageBox.Show("Get Parameter Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_ParamSet_Click(object sender, EventArgs e)
	{
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_Param).SelectedIndex)
		{
			MessageBox.Show("Please Select Parameter", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Param).Focus();
			return;
		}
		try
		{
			int num = -1;
			int selectedIndex = ((ListControl)cbo_Param).SelectedIndex;
			int num2 = selectedIndex;
			if ((uint)(num2 - 26) <= 3u)
			{
				if (!ConvertObject.IsCorrenctIP(((Control)txt_ParamValue).Text.Trim()))
				{
					MessageBox.Show("Illegal IP address", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					((Control)txt_ParamValue).Focus();
					return;
				}
				num = ConvertObject.ConvertIPAddressToNumber(((Control)txt_ParamValue).Text.Trim());
			}
			else
			{
				num = Convert.ToInt32(((Control)txt_ParamValue).Text.Trim());
			}
			object obj = new object();
			object obj2 = new object();
			byte[] array = new byte[8];
			Array.Copy(BitConverter.GetBytes(((ListControl)cbo_Param).SelectedIndex), 0, array, 0, 4);
			Array.Copy(BitConverter.GetBytes(num), 0, array, 4, 4);
			obj2 = array;
			if (deviceConnection.SetProperty((DeviceProperty)26, obj, device, obj2))
			{
				MessageBox.Show("Set Parameter Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Parameter Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_PowerGet_Click(object sender, EventArgs e)
	{
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_PowerType).SelectedIndex)
		{
			MessageBox.Show("Please Select Type", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_PowerType).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_DN).SelectedIndex)
		{
			MessageBox.Show("Please Select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_DN).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 66;
			if (deviceConnection.GetProperty((DeviceProperty)21, obj, ref device, ref obj2))
			{
				string s = (string)obj2;
				byte[] bytes = Encoding.Unicode.GetBytes(s);
				byte[] array = new byte[4];
				int sourceIndex = (((ListControl)cbo_PowerType).SelectedIndex + 1) * 12 * 4 + ((ListControl)cbo_DN).SelectedIndex * 4;
				Array.Copy(bytes, sourceIndex, array, 0, array.Length);
				((ListControl)cbo_PowerHour).SelectedIndex = array[0];
				((ListControl)cbo_PowerMinute).SelectedIndex = array[1];
				chk_Power.Checked = Convert.ToBoolean(array[2]);
			}
			else
			{
				MessageBox.Show("Get Timer Off Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_PowerSet_Click(object sender, EventArgs e)
	{
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_PowerType).SelectedIndex)
		{
			MessageBox.Show("Please Select Type", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_PowerType).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_DN).SelectedIndex)
		{
			MessageBox.Show("Please Select SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_DN).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_PowerHour).SelectedIndex)
		{
			MessageBox.Show("Please Select Time: Hour", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_PowerHour).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_PowerMinute).SelectedIndex)
		{
			MessageBox.Show("Please Select Time: Minute", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_PowerMinute).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 66;
			if (deviceConnection.GetProperty((DeviceProperty)21, obj, ref device, ref obj2))
			{
				string s = (string)obj2;
				byte[] bytes = Encoding.Unicode.GetBytes(s);
				int num = (((ListControl)cbo_PowerType).SelectedIndex + 1) * 12 * 4 + ((ListControl)cbo_DN).SelectedIndex * 4;
				bytes[num] = (byte)((ListControl)cbo_PowerHour).SelectedIndex;
				bytes[num + 1] = (byte)((ListControl)cbo_PowerMinute).SelectedIndex;
				bytes[num + 2] = Convert.ToByte(chk_Power.Checked);
				obj = 66;
				obj2 = Encoding.Unicode.GetString(bytes);
				if (deviceConnection.SetProperty((DeviceProperty)21, obj, device, obj2))
				{
					MessageBox.Show("Set Timer Off Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
				}
				else
				{
					MessageBox.Show("Set Timer Off Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				}
			}
			else
			{
				MessageBox.Show("Set Timer Off Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_TimeGet_Click(object sender, EventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			if (deviceConnection.GetProperty((DeviceProperty)10, obj, ref device, ref obj2))
			{
				DateTime dateTime = (DateTime)obj2;
				((Control)txt_DeviceTime).Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				MessageBox.Show("Get Device Time Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_TimeSet_Click(object sender, EventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			if (deviceConnection.SetProperty((DeviceProperty)10, obj, device, obj2))
			{
				MessageBox.Show("Synchronize the Device Time Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
				btn_TimeGet_Click(btn_TimeGet, e);
			}
			else
			{
				MessageBox.Show("Synchronize the Device Time Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_SystemGet_Click(object sender, EventArgs e)
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			if (deviceConnection.GetProperty((DeviceProperty)8, obj, ref device, ref obj2))
			{
				uint[] array = (uint[])obj2;
				((Control)txt_UserCount).Text = array[0].ToString();
				((Control)txt_AdminCount).Text = array[1].ToString();
				((Control)txt_FingerCount).Text = array[2].ToString();
				((Control)txt_CardCount).Text = array[3].ToString();
				((Control)txt_PwdCount).Text = array[4].ToString();
				((Control)txt_ManageCount).Text = array[5].ToString();
				((Control)txt_ExitCount).Text = array[6].ToString();
				((Control)txt_HisManageCount).Text = array[7].ToString();
				((Control)txt_HisExitCount).Text = array[8].ToString();
			}
			else
			{
				MessageBox.Show("Get Log Information Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_MacAddress_Click(object sender, EventArgs e)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			if (deviceConnection.GetProperty((DeviceProperty)22, obj, ref device, ref obj2))
			{
				byte[] bytes = (byte[])obj2;
				MessageBox.Show("Mac Address: " + ConvertObject.ConvertByteToHex(bytes), "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Get Mac Address Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_Reset_Click(object sender, EventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = new object();
			if (deviceConnection.SetProperty((DeviceProperty)7, obj, device, obj2))
			{
				MessageBox.Show("Device Init Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Device Init Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			obj2 = 0L;
			deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
		}
	}

	private void btn_WelcomeGet_Click(object sender, EventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			obj2 = 60;
			if (deviceConnection.GetProperty((DeviceProperty)5, obj, ref device, ref obj2))
			{
				((Control)txt_Welcome).Text = (string)obj2;
			}
			else
			{
				MessageBox.Show("Get Welcome Title Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_WelcomeSet_Click(object sender, EventArgs e)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (string.IsNullOrEmpty(((Control)txt_Welcome).Text.Trim()))
			{
				MessageBox.Show("Please Input Welcome Title", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				((Control)txt_Welcome).Focus();
				return;
			}
			object obj = new object();
			obj = 60;
			object obj2 = new object();
			obj2 = ((Control)txt_Welcome).Text.Trim();
			if (deviceConnection.SetProperty((DeviceProperty)5, obj, device, obj2))
			{
				MessageBox.Show("Set Welcome Title Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Welcome Title Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_StandbyGet_Click(object sender, EventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			obj2 = 61;
			if (deviceConnection.GetProperty((DeviceProperty)6, obj, ref device, ref obj2))
			{
				((Control)txt_Standby).Text = (string)obj2;
			}
			else
			{
				MessageBox.Show("Get Standby Title Settings Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_StandbySet_Click(object sender, EventArgs e)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (string.IsNullOrEmpty(((Control)txt_Standby).Text.Trim()))
			{
				MessageBox.Show("Please Input Standby Title", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				((Control)txt_Standby).Focus();
				return;
			}
			object obj = new object();
			obj = 61;
			object obj2 = new object();
			obj2 = ((Control)txt_Standby).Text.Trim();
			if (deviceConnection.SetProperty((DeviceProperty)6, obj, device, obj2))
			{
				MessageBox.Show("Set Standby Title Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Standby Title Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_FirmwareVersionGet_Click(object sender, EventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			obj2 = 63;
			if (deviceConnection.GetProperty((DeviceProperty)0, obj, ref device, ref obj2))
			{
				((Control)txt_FirmwareVersion).Text = (string)obj2;
			}
			else
			{
				MessageBox.Show("Get Firmware Version Settings Fail.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_ModelGet_Click(object sender, EventArgs e)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			obj2 = 62;
			if (deviceConnection.GetProperty((DeviceProperty)25, obj, ref device, ref obj2))
			{
				((Control)txt_Model).Text = (string)obj2;
			}
			else
			{
				MessageBox.Show("Get Model Settings Fail.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_UploadSound_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between I4 and Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (2 != (int)device.CommunicationType)
		{
			MessageBox.Show("Upload Sound Only Supports USB Communication", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		if (-1 == ((ListControl)cbo_SoundType).SelectedIndex)
		{
			MessageBox.Show("Please Selected Sound Type", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_SoundType).Focus();
			return;
		}
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			((FileDialog)val).Filter = "WAV|*.wav";
			DialogResult val2 = (DialogResult)1;
			if (!((object)(DialogResult)(ref val2)).Equals((object?)((CommonDialog)val).ShowDialog()))
			{
				return;
			}
			FileStream fileStream = new FileStream(((FileDialog)val).FileName, FileMode.Open);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, (int)fileStream.Length);
			fileStream.Close();
			if (CheckWaveFormat(array, ref dataChunk))
			{
				if (UploadSound(array))
				{
					MessageBox.Show("Upload Sound Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
				}
				else
				{
					MessageBox.Show("Upload Sound Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Upload Sound Error: " + ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private bool CheckWaveFormat(byte[] buffer, ref int offSet)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		offSet = 0;
		string @string = Encoding.ASCII.GetString(buffer, 0, 4);
		string string2 = Encoding.ASCII.GetString(buffer, 8, 4);
		string string3 = Encoding.ASCII.GetString(buffer, 12, 4);
		if (!@string.Equals("RIFF") || !string2.Equals("WAVE") || !string3.Equals("fmt "))
		{
			MessageBox.Show("Incorrect Wave Format", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		int num = BitConverter.ToInt32(buffer, 16);
		byte[] array = new byte[18];
		Array.Copy(buffer, 20, array, 0, array.Length);
		ushort num2 = BitConverter.ToUInt16(array, 0);
		if (1 != num2)
		{
			MessageBox.Show("Audio Format is not PCM", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		ushort num3 = BitConverter.ToUInt16(array, 2);
		if (1 != num3)
		{
			MessageBox.Show("Channels is not Stereo", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		uint num4 = BitConverter.ToUInt32(array, 4);
		if (22050 != num4)
		{
			MessageBox.Show("Audio sample rate is not 22.05KHZ", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		ushort num5 = BitConverter.ToUInt16(array, 14);
		if (16 != num5)
		{
			MessageBox.Show("Audio sample size is not 8bit", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		int num6 = 0;
		int num7 = 20 + num;
		byte[] array2 = Zd2911Utils.CreateChunkHeader(buffer, num7);
		string string4 = Encoding.ASCII.GetString(array2, 0, 4);
		int num8 = BitConverter.ToInt32(array2, 4);
		if (string4.Equals("fact"))
		{
			offSet = offSet + num6 + num8;
		}
		offSet = num7;
		array2 = Zd2911Utils.CreateChunkHeader(buffer, offSet);
		string string5 = Encoding.ASCII.GetString(array2, 0, 4);
		num8 = BitConverter.ToInt32(array2, 4);
		if (!string5.Equals("data"))
		{
			MessageBox.Show("Incorrect Wave Format", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		if (76800 < num8)
		{
			MessageBox.Show("Data size is too big. Must be less than 75K byte", "Pormpt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return false;
		}
		return true;
	}

	private bool UploadSound(byte[] soundBuffer)
	{
		int num = 0;
		byte[] array = Zd2911Utils.CreateChunkHeader(soundBuffer, dataChunk);
		int num2 = BitConverter.ToInt32(array, 4);
		int num3 = num2 + 4;
		byte[] array2 = new byte[num3];
		Array.Copy(array, 4, array2, 0, 4);
		Array.Copy(soundBuffer, dataChunk + num, array2, 4, num2);
		int num4 = 61440;
		bool flag = false;
		int num5 = num3 / num4;
		object obj = new object();
		object obj2 = new object();
		int i;
		for (i = 0; i < num5; i++)
		{
			byte[] array3 = new byte[num4];
			Array.Copy(array2, i * num4, array3, 0, num4);
			List<int> list = new List<int>();
			list.Add(((ListControl)cbo_SoundType).SelectedIndex + 8);
			list.Add(i * num4);
			obj = list;
			obj2 = array3;
			flag = deviceConnection.SetProperty((DeviceProperty)27, obj, device, obj2);
			if (!flag)
			{
				return false;
			}
		}
		num5 = num3 % num4;
		if (num5 > 0)
		{
			byte[] array4 = new byte[num5];
			Array.Copy(array2, i * num4, array4, 0, num5);
			List<int> list2 = new List<int>();
			list2.Add(((ListControl)cbo_SoundType).SelectedIndex + 8);
			list2.Add(i * num4);
			obj = list2;
			obj2 = array4;
			flag = deviceConnection.SetProperty((DeviceProperty)27, obj, device, obj2);
			if (!flag)
			{
				return false;
			}
		}
		return flag;
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
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		groupBox1 = new GroupBox();
		btn_ParamGet = new Button();
		btn_ParamSet = new Button();
		txt_ParamValue = new TextBox();
		cbo_Param = new ComboBox();
		lbl_ParamValue = new Label();
		lbl_Param = new Label();
		groupBox2 = new GroupBox();
		chk_Power = new CheckBox();
		btn_PowerGet = new Button();
		btn_PowerSet = new Button();
		cbo_PowerMinute = new ComboBox();
		cbo_PowerHour = new ComboBox();
		cbo_DN = new ComboBox();
		cbo_PowerType = new ComboBox();
		label1 = new Label();
		lbl_Status = new Label();
		lbl_Time = new Label();
		lbl_DN = new Label();
		lbl_PowerType = new Label();
		groupBox3 = new GroupBox();
		txt_DeviceTime = new TextBox();
		lbl_DeviceTime = new Label();
		btn_TimeGet = new Button();
		btn_TimeSet = new Button();
		groupBox4 = new GroupBox();
		btn_SystemGet = new Button();
		txt_PwdCount = new TextBox();
		txt_CardCount = new TextBox();
		txt_HisManageCount = new TextBox();
		txt_ManageCount = new TextBox();
		txt_HisExitCount = new TextBox();
		txt_ExitCount = new TextBox();
		txt_AdminCount = new TextBox();
		txt_FingerCount = new TextBox();
		lbl_PwdCount = new Label();
		lbl_CardCount = new Label();
		lbl_HisManageCount = new Label();
		lbl_HisExitCount = new Label();
		lbl_ManageCount = new Label();
		lbl_ExitCount = new Label();
		txt_UserCount = new TextBox();
		lbl_AdminCount = new Label();
		lbl_FingerCount = new Label();
		lbl_UserCount = new Label();
		btn_MacAddress = new Button();
		btn_Reset = new Button();
		txt_Welcome = new TextBox();
		btn_WelcomeGet = new Button();
		btn_WelcomeSet = new Button();
		lbl_WelCome = new Label();
		txt_Standby = new TextBox();
		btn_StandbyGet = new Button();
		btn_StandbySet = new Button();
		lbl_Standby = new Label();
		groupBox5 = new GroupBox();
		groupBox6 = new GroupBox();
		btn_ModelGet = new Button();
		txt_Model = new TextBox();
		btn_FirmwareVersionGet = new Button();
		lbl_Model = new Label();
		txt_FirmwareVersion = new TextBox();
		lbl_FirmwareVersion = new Label();
		groupBox7 = new GroupBox();
		btn_UploadSound = new Button();
		cbo_SoundType = new ComboBox();
		lbl_SoundType = new Label();
		((Control)groupBox1).SuspendLayout();
		((Control)groupBox2).SuspendLayout();
		((Control)groupBox3).SuspendLayout();
		((Control)groupBox4).SuspendLayout();
		((Control)groupBox5).SuspendLayout();
		((Control)groupBox6).SuspendLayout();
		((Control)groupBox7).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBox1).Controls.Add((Control)(object)btn_ParamGet);
		((Control)groupBox1).Controls.Add((Control)(object)btn_ParamSet);
		((Control)groupBox1).Controls.Add((Control)(object)txt_ParamValue);
		((Control)groupBox1).Controls.Add((Control)(object)cbo_Param);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_ParamValue);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_Param);
		((Control)groupBox1).Location = new Point(12, 12);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(289, 104);
		((Control)groupBox1).TabIndex = 0;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "System Parameter Settings";
		((Control)btn_ParamGet).Location = new Point(121, 73);
		((Control)btn_ParamGet).Name = "btn_ParamGet";
		((Control)btn_ParamGet).Size = new Size(75, 23);
		((Control)btn_ParamGet).TabIndex = 4;
		((Control)btn_ParamGet).Text = "Get";
		((ButtonBase)btn_ParamGet).UseVisualStyleBackColor = true;
		((Control)btn_ParamGet).Click += btn_ParamGet_Click;
		((Control)btn_ParamSet).Location = new Point(202, 73);
		((Control)btn_ParamSet).Name = "btn_ParamSet";
		((Control)btn_ParamSet).Size = new Size(75, 23);
		((Control)btn_ParamSet).TabIndex = 5;
		((Control)btn_ParamSet).Text = "Set";
		((ButtonBase)btn_ParamSet).UseVisualStyleBackColor = true;
		((Control)btn_ParamSet).Click += btn_ParamSet_Click;
		((Control)txt_ParamValue).Location = new Point(77, 46);
		((Control)txt_ParamValue).Name = "txt_ParamValue";
		((Control)txt_ParamValue).Size = new Size(200, 21);
		((Control)txt_ParamValue).TabIndex = 3;
		cbo_Param.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_Param).FormattingEnabled = true;
		cbo_Param.Items.AddRange(new object[32]
		{
			"Admin Count", "Language Format", "ID Length", "Volume Size", "Auto Off Time", "Auto Power On", "Verify Mode", "Auto Learning", "Auto Return Time", "Standby Time",
			"Enable Alarm In Standby", "Card ID Type", "Auto Restart", "Enable Shutdown", "Enable Relay Alarm", "Fire Alarm", "One To One Security Level", "One To N Security Level", "SLog Warning Count", "GLog Warning Count",
			"Reverify Time", "Device ID", "Baudrate", "User Real Time Log", "UDP Port", "Device Password", "IP Address", "Sub Net Address", "Default Gate", "Server IP Address",
			"Server UDP Port", "RS485 Use"
		});
		((Control)cbo_Param).Location = new Point(77, 20);
		((Control)cbo_Param).Name = "cbo_Param";
		((Control)cbo_Param).Size = new Size(200, 20);
		((Control)cbo_Param).TabIndex = 1;
		((Control)lbl_ParamValue).AutoSize = true;
		((Control)lbl_ParamValue).Location = new Point(30, 49);
		((Control)lbl_ParamValue).Name = "lbl_ParamValue";
		((Control)lbl_ParamValue).Size = new Size(41, 12);
		((Control)lbl_ParamValue).TabIndex = 2;
		((Control)lbl_ParamValue).Text = "Value:";
		lbl_ParamValue.TextAlign = (ContentAlignment)32;
		((Control)lbl_Param).AutoSize = true;
		((Control)lbl_Param).Location = new Point(6, 23);
		((Control)lbl_Param).Name = "lbl_Param";
		((Control)lbl_Param).Size = new Size(65, 12);
		((Control)lbl_Param).TabIndex = 0;
		((Control)lbl_Param).Text = "Parameter:";
		lbl_Param.TextAlign = (ContentAlignment)32;
		((Control)groupBox2).Controls.Add((Control)(object)chk_Power);
		((Control)groupBox2).Controls.Add((Control)(object)btn_PowerGet);
		((Control)groupBox2).Controls.Add((Control)(object)btn_PowerSet);
		((Control)groupBox2).Controls.Add((Control)(object)cbo_PowerMinute);
		((Control)groupBox2).Controls.Add((Control)(object)cbo_PowerHour);
		((Control)groupBox2).Controls.Add((Control)(object)cbo_DN);
		((Control)groupBox2).Controls.Add((Control)(object)cbo_PowerType);
		((Control)groupBox2).Controls.Add((Control)(object)label1);
		((Control)groupBox2).Controls.Add((Control)(object)lbl_Status);
		((Control)groupBox2).Controls.Add((Control)(object)lbl_Time);
		((Control)groupBox2).Controls.Add((Control)(object)lbl_DN);
		((Control)groupBox2).Controls.Add((Control)(object)lbl_PowerType);
		((Control)groupBox2).Location = new Point(12, 122);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(225, 156);
		((Control)groupBox2).TabIndex = 2;
		groupBox2.TabStop = false;
		((Control)groupBox2).Text = "Timer Off Settings";
		((Control)chk_Power).AutoSize = true;
		((Control)chk_Power).Location = new Point(47, 98);
		((Control)chk_Power).Name = "chk_Power";
		((Control)chk_Power).Size = new Size(60, 16);
		((Control)chk_Power).TabIndex = 8;
		((Control)chk_Power).Text = "Enable";
		((ButtonBase)chk_Power).UseVisualStyleBackColor = true;
		((Control)btn_PowerGet).Location = new Point(32, 120);
		((Control)btn_PowerGet).Name = "btn_PowerGet";
		((Control)btn_PowerGet).Size = new Size(75, 23);
		((Control)btn_PowerGet).TabIndex = 8;
		((Control)btn_PowerGet).Text = "Get";
		((ButtonBase)btn_PowerGet).UseVisualStyleBackColor = true;
		((Control)btn_PowerGet).Click += btn_PowerGet_Click;
		((Control)btn_PowerSet).Location = new Point(121, 120);
		((Control)btn_PowerSet).Name = "btn_PowerSet";
		((Control)btn_PowerSet).Size = new Size(75, 23);
		((Control)btn_PowerSet).TabIndex = 9;
		((Control)btn_PowerSet).Text = "Set";
		((ButtonBase)btn_PowerSet).UseVisualStyleBackColor = true;
		((Control)btn_PowerSet).Click += btn_PowerSet_Click;
		cbo_PowerMinute.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_PowerMinute).FormattingEnabled = true;
		((Control)cbo_PowerMinute).Location = new Point(136, 72);
		((Control)cbo_PowerMinute).Name = "cbo_PowerMinute";
		((Control)cbo_PowerMinute).Size = new Size(60, 20);
		((Control)cbo_PowerMinute).TabIndex = 7;
		cbo_PowerHour.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_PowerHour).FormattingEnabled = true;
		((Control)cbo_PowerHour).Location = new Point(47, 72);
		((Control)cbo_PowerHour).Name = "cbo_PowerHour";
		((Control)cbo_PowerHour).Size = new Size(60, 20);
		((Control)cbo_PowerHour).TabIndex = 5;
		cbo_DN.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_DN).FormattingEnabled = true;
		((Control)cbo_DN).Location = new Point(47, 46);
		((Control)cbo_DN).Name = "cbo_DN";
		((Control)cbo_DN).Size = new Size(149, 20);
		((Control)cbo_DN).TabIndex = 3;
		cbo_PowerType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_PowerType).FormattingEnabled = true;
		cbo_PowerType.Items.AddRange(new object[1] { "Shutdown" });
		((Control)cbo_PowerType).Location = new Point(47, 20);
		((Control)cbo_PowerType).Name = "cbo_PowerType";
		((Control)cbo_PowerType).Size = new Size(149, 20);
		((Control)cbo_PowerType).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(113, 75);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(17, 12);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = "：";
		label1.TextAlign = (ContentAlignment)32;
		((Control)lbl_Status).AutoSize = true;
		((Control)lbl_Status).Location = new Point(18, 101);
		((Control)lbl_Status).Name = "lbl_Status";
		((Control)lbl_Status).Size = new Size(0, 12);
		((Control)lbl_Status).TabIndex = 0;
		lbl_Status.TextAlign = (ContentAlignment)32;
		((Control)lbl_Time).AutoSize = true;
		((Control)lbl_Time).Location = new Point(6, 74);
		((Control)lbl_Time).Name = "lbl_Time";
		((Control)lbl_Time).Size = new Size(35, 12);
		((Control)lbl_Time).TabIndex = 4;
		((Control)lbl_Time).Text = "Time:";
		lbl_Time.TextAlign = (ContentAlignment)32;
		((Control)lbl_DN).AutoSize = true;
		((Control)lbl_DN).Location = new Point(18, 49);
		((Control)lbl_DN).Name = "lbl_DN";
		((Control)lbl_DN).Size = new Size(23, 12);
		((Control)lbl_DN).TabIndex = 2;
		((Control)lbl_DN).Text = "SN:";
		lbl_DN.TextAlign = (ContentAlignment)32;
		((Control)lbl_PowerType).AutoSize = true;
		((Control)lbl_PowerType).Location = new Point(6, 23);
		((Control)lbl_PowerType).Name = "lbl_PowerType";
		((Control)lbl_PowerType).Size = new Size(35, 12);
		((Control)lbl_PowerType).TabIndex = 0;
		((Control)lbl_PowerType).Text = "Type:";
		lbl_PowerType.TextAlign = (ContentAlignment)32;
		((Control)groupBox3).Controls.Add((Control)(object)txt_DeviceTime);
		((Control)groupBox3).Controls.Add((Control)(object)lbl_DeviceTime);
		((Control)groupBox3).Controls.Add((Control)(object)btn_TimeGet);
		((Control)groupBox3).Controls.Add((Control)(object)btn_TimeSet);
		((Control)groupBox3).Location = new Point(307, 12);
		((Control)groupBox3).Name = "groupBox3";
		((Control)groupBox3).Size = new Size(457, 104);
		((Control)groupBox3).TabIndex = 1;
		groupBox3.TabStop = false;
		((Control)groupBox3).Text = "Time Settings";
		((Control)txt_DeviceTime).BackColor = Color.White;
		((Control)txt_DeviceTime).Location = new Point(47, 20);
		((Control)txt_DeviceTime).Name = "txt_DeviceTime";
		((TextBoxBase)txt_DeviceTime).ReadOnly = true;
		((Control)txt_DeviceTime).Size = new Size(399, 21);
		((Control)txt_DeviceTime).TabIndex = 1;
		((Control)lbl_DeviceTime).AutoSize = true;
		((Control)lbl_DeviceTime).Location = new Point(6, 23);
		((Control)lbl_DeviceTime).Name = "lbl_DeviceTime";
		((Control)lbl_DeviceTime).Size = new Size(35, 12);
		((Control)lbl_DeviceTime).TabIndex = 0;
		((Control)lbl_DeviceTime).Text = "Time:";
		lbl_DeviceTime.TextAlign = (ContentAlignment)16;
		((Control)btn_TimeGet).Location = new Point(176, 47);
		((Control)btn_TimeGet).Name = "btn_TimeGet";
		((Control)btn_TimeGet).Size = new Size(75, 23);
		((Control)btn_TimeGet).TabIndex = 2;
		((Control)btn_TimeGet).Text = "Get";
		((ButtonBase)btn_TimeGet).UseVisualStyleBackColor = true;
		((Control)btn_TimeGet).Click += btn_TimeGet_Click;
		((Control)btn_TimeSet).Location = new Point(257, 47);
		((Control)btn_TimeSet).Name = "btn_TimeSet";
		((Control)btn_TimeSet).Size = new Size(189, 23);
		((Control)btn_TimeSet).TabIndex = 3;
		((Control)btn_TimeSet).Text = "Synchronize the device time";
		((ButtonBase)btn_TimeSet).UseVisualStyleBackColor = true;
		((Control)btn_TimeSet).Click += btn_TimeSet_Click;
		((Control)groupBox4).Controls.Add((Control)(object)btn_SystemGet);
		((Control)groupBox4).Controls.Add((Control)(object)txt_PwdCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_CardCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_HisManageCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_ManageCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_HisExitCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_ExitCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_AdminCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_FingerCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_PwdCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_CardCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_HisManageCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_HisExitCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_ManageCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_ExitCount);
		((Control)groupBox4).Controls.Add((Control)(object)txt_UserCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_AdminCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_FingerCount);
		((Control)groupBox4).Controls.Add((Control)(object)lbl_UserCount);
		((Control)groupBox4).Location = new Point(243, 122);
		((Control)groupBox4).Name = "groupBox4";
		((Control)groupBox4).Size = new Size(521, 156);
		((Control)groupBox4).TabIndex = 3;
		groupBox4.TabStop = false;
		((Control)groupBox4).Text = "Device Record Information Settings";
		((Control)btn_SystemGet).Location = new Point(435, 128);
		((Control)btn_SystemGet).Name = "btn_SystemGet";
		((Control)btn_SystemGet).Size = new Size(75, 23);
		((Control)btn_SystemGet).TabIndex = 18;
		((Control)btn_SystemGet).Text = "Get";
		((ButtonBase)btn_SystemGet).UseVisualStyleBackColor = true;
		((Control)btn_SystemGet).Click += btn_SystemGet_Click;
		((Control)txt_PwdCount).BackColor = Color.White;
		((Control)txt_PwdCount).Location = new Point(410, 47);
		((Control)txt_PwdCount).Name = "txt_PwdCount";
		((TextBoxBase)txt_PwdCount).ReadOnly = true;
		((Control)txt_PwdCount).Size = new Size(100, 21);
		((Control)txt_PwdCount).TabIndex = 7;
		((Control)txt_CardCount).BackColor = Color.White;
		((Control)txt_CardCount).Location = new Point(131, 74);
		((Control)txt_CardCount).Name = "txt_CardCount";
		((TextBoxBase)txt_CardCount).ReadOnly = true;
		((Control)txt_CardCount).Size = new Size(100, 21);
		((Control)txt_CardCount).TabIndex = 9;
		((Control)txt_HisManageCount).BackColor = Color.White;
		((Control)txt_HisManageCount).Location = new Point(131, 128);
		((Control)txt_HisManageCount).Name = "txt_HisManageCount";
		((TextBoxBase)txt_HisManageCount).ReadOnly = true;
		((Control)txt_HisManageCount).Size = new Size(100, 21);
		((Control)txt_HisManageCount).TabIndex = 17;
		((Control)txt_ManageCount).BackColor = Color.White;
		((Control)txt_ManageCount).Location = new Point(131, 101);
		((Control)txt_ManageCount).Name = "txt_ManageCount";
		((TextBoxBase)txt_ManageCount).ReadOnly = true;
		((Control)txt_ManageCount).Size = new Size(100, 21);
		((Control)txt_ManageCount).TabIndex = 13;
		((Control)txt_HisExitCount).BackColor = Color.White;
		((Control)txt_HisExitCount).Location = new Point(410, 99);
		((Control)txt_HisExitCount).Name = "txt_HisExitCount";
		((TextBoxBase)txt_HisExitCount).ReadOnly = true;
		((Control)txt_HisExitCount).Size = new Size(100, 21);
		((Control)txt_HisExitCount).TabIndex = 15;
		((Control)txt_ExitCount).BackColor = Color.White;
		((Control)txt_ExitCount).Location = new Point(410, 72);
		((Control)txt_ExitCount).Name = "txt_ExitCount";
		((TextBoxBase)txt_ExitCount).ReadOnly = true;
		((Control)txt_ExitCount).Size = new Size(100, 21);
		((Control)txt_ExitCount).TabIndex = 11;
		((Control)txt_AdminCount).BackColor = Color.White;
		((Control)txt_AdminCount).Location = new Point(410, 20);
		((Control)txt_AdminCount).Name = "txt_AdminCount";
		((TextBoxBase)txt_AdminCount).ReadOnly = true;
		((Control)txt_AdminCount).Size = new Size(100, 21);
		((Control)txt_AdminCount).TabIndex = 3;
		((Control)txt_FingerCount).BackColor = Color.White;
		((Control)txt_FingerCount).Location = new Point(131, 47);
		((Control)txt_FingerCount).Name = "txt_FingerCount";
		((TextBoxBase)txt_FingerCount).ReadOnly = true;
		((Control)txt_FingerCount).Size = new Size(100, 21);
		((Control)txt_FingerCount).TabIndex = 5;
		((Control)lbl_PwdCount).AutoSize = true;
		((Control)lbl_PwdCount).Location = new Point(339, 50);
		((Control)lbl_PwdCount).Name = "lbl_PwdCount";
		((Control)lbl_PwdCount).Size = new Size(65, 12);
		((Control)lbl_PwdCount).TabIndex = 6;
		((Control)lbl_PwdCount).Text = "PWD Count:";
		lbl_PwdCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_CardCount).AutoSize = true;
		((Control)lbl_CardCount).Location = new Point(54, 77);
		((Control)lbl_CardCount).Name = "lbl_CardCount";
		((Control)lbl_CardCount).Size = new Size(71, 12);
		((Control)lbl_CardCount).TabIndex = 8;
		((Control)lbl_CardCount).Text = "Card Count:";
		lbl_CardCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_HisManageCount).AutoSize = true;
		((Control)lbl_HisManageCount).Location = new Point(6, 131);
		((Control)lbl_HisManageCount).Name = "lbl_HisManageCount";
		((Control)lbl_HisManageCount).Size = new Size(119, 12);
		((Control)lbl_HisManageCount).TabIndex = 16;
		((Control)lbl_HisManageCount).Text = "History SLog Count:";
		lbl_HisManageCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_HisExitCount).AutoSize = true;
		((Control)lbl_HisExitCount).Location = new Point(285, 104);
		((Control)lbl_HisExitCount).Name = "lbl_HisExitCount";
		((Control)lbl_HisExitCount).Size = new Size(119, 12);
		((Control)lbl_HisExitCount).TabIndex = 14;
		((Control)lbl_HisExitCount).Text = "History GLog Count:";
		lbl_HisExitCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_ManageCount).AutoSize = true;
		((Control)lbl_ManageCount).Location = new Point(54, 104);
		((Control)lbl_ManageCount).Name = "lbl_ManageCount";
		((Control)lbl_ManageCount).Size = new Size(71, 12);
		((Control)lbl_ManageCount).TabIndex = 12;
		((Control)lbl_ManageCount).Text = "SLog Count:";
		lbl_ManageCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_ExitCount).AutoSize = true;
		((Control)lbl_ExitCount).Location = new Point(333, 77);
		((Control)lbl_ExitCount).Name = "lbl_ExitCount";
		((Control)lbl_ExitCount).Size = new Size(71, 12);
		((Control)lbl_ExitCount).TabIndex = 10;
		((Control)lbl_ExitCount).Text = "GLog Count:";
		lbl_ExitCount.TextAlign = (ContentAlignment)32;
		((Control)txt_UserCount).BackColor = Color.White;
		((Control)txt_UserCount).Location = new Point(131, 20);
		((Control)txt_UserCount).Name = "txt_UserCount";
		((TextBoxBase)txt_UserCount).ReadOnly = true;
		((Control)txt_UserCount).Size = new Size(100, 21);
		((Control)txt_UserCount).TabIndex = 1;
		((Control)lbl_AdminCount).AutoSize = true;
		((Control)lbl_AdminCount).Location = new Point(351, 23);
		((Control)lbl_AdminCount).Name = "lbl_AdminCount";
		((Control)lbl_AdminCount).Size = new Size(53, 12);
		((Control)lbl_AdminCount).TabIndex = 2;
		((Control)lbl_AdminCount).Text = "Manager:";
		lbl_AdminCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_FingerCount).AutoSize = true;
		((Control)lbl_FingerCount).Location = new Point(66, 50);
		((Control)lbl_FingerCount).Name = "lbl_FingerCount";
		((Control)lbl_FingerCount).Size = new Size(59, 12);
		((Control)lbl_FingerCount).TabIndex = 4;
		((Control)lbl_FingerCount).Text = "FP Count:";
		lbl_FingerCount.TextAlign = (ContentAlignment)32;
		((Control)lbl_UserCount).AutoSize = true;
		((Control)lbl_UserCount).Location = new Point(90, 23);
		((Control)lbl_UserCount).Name = "lbl_UserCount";
		((Control)lbl_UserCount).Size = new Size(35, 12);
		((Control)lbl_UserCount).TabIndex = 0;
		((Control)lbl_UserCount).Text = "User:";
		lbl_UserCount.TextAlign = (ContentAlignment)32;
		((Control)btn_MacAddress).Location = new Point(552, 531);
		((Control)btn_MacAddress).Name = "btn_MacAddress";
		((Control)btn_MacAddress).Size = new Size(113, 23);
		((Control)btn_MacAddress).TabIndex = 7;
		((Control)btn_MacAddress).Text = "Get Mac Address";
		((ButtonBase)btn_MacAddress).UseVisualStyleBackColor = true;
		((Control)btn_MacAddress).Click += btn_MacAddress_Click;
		((Control)btn_Reset).Location = new Point(671, 531);
		((Control)btn_Reset).Name = "btn_Reset";
		((Control)btn_Reset).Size = new Size(93, 23);
		((Control)btn_Reset).TabIndex = 8;
		((Control)btn_Reset).Text = "Device Init";
		((ButtonBase)btn_Reset).UseVisualStyleBackColor = true;
		((Control)btn_Reset).Click += btn_Reset_Click;
		((Control)txt_Welcome).Location = new Point(101, 20);
		((TextBoxBase)txt_Welcome).MaxLength = 32;
		((Control)txt_Welcome).Name = "txt_Welcome";
		((Control)txt_Welcome).Size = new Size(238, 21);
		((Control)txt_Welcome).TabIndex = 1;
		((Control)btn_WelcomeGet).Location = new Point(345, 19);
		((Control)btn_WelcomeGet).Name = "btn_WelcomeGet";
		((Control)btn_WelcomeGet).Size = new Size(75, 21);
		((Control)btn_WelcomeGet).TabIndex = 2;
		((Control)btn_WelcomeGet).Text = "Get";
		((ButtonBase)btn_WelcomeGet).UseVisualStyleBackColor = true;
		((Control)btn_WelcomeGet).Click += btn_WelcomeGet_Click;
		((Control)btn_WelcomeSet).Location = new Point(426, 19);
		((Control)btn_WelcomeSet).Name = "btn_WelcomeSet";
		((Control)btn_WelcomeSet).Size = new Size(75, 21);
		((Control)btn_WelcomeSet).TabIndex = 3;
		((Control)btn_WelcomeSet).Text = "Set";
		((ButtonBase)btn_WelcomeSet).UseVisualStyleBackColor = true;
		((Control)btn_WelcomeSet).Click += btn_WelcomeSet_Click;
		((Control)lbl_WelCome).AutoSize = true;
		((Control)lbl_WelCome).Location = new Point(6, 23);
		((Control)lbl_WelCome).Name = "lbl_WelCome";
		((Control)lbl_WelCome).Size = new Size(89, 12);
		((Control)lbl_WelCome).TabIndex = 0;
		((Control)lbl_WelCome).Text = "Welcome Title:";
		lbl_WelCome.TextAlign = (ContentAlignment)16;
		((Control)txt_Standby).Location = new Point(101, 47);
		((TextBoxBase)txt_Standby).MaxLength = 32;
		((Control)txt_Standby).Name = "txt_Standby";
		((Control)txt_Standby).Size = new Size(238, 21);
		((Control)txt_Standby).TabIndex = 5;
		((Control)btn_StandbyGet).Location = new Point(345, 46);
		((Control)btn_StandbyGet).Name = "btn_StandbyGet";
		((Control)btn_StandbyGet).Size = new Size(75, 21);
		((Control)btn_StandbyGet).TabIndex = 6;
		((Control)btn_StandbyGet).Text = "Get";
		((ButtonBase)btn_StandbyGet).UseVisualStyleBackColor = true;
		((Control)btn_StandbyGet).Click += btn_StandbyGet_Click;
		((Control)btn_StandbySet).Location = new Point(426, 46);
		((Control)btn_StandbySet).Name = "btn_StandbySet";
		((Control)btn_StandbySet).Size = new Size(75, 21);
		((Control)btn_StandbySet).TabIndex = 7;
		((Control)btn_StandbySet).Text = "Set";
		((ButtonBase)btn_StandbySet).UseVisualStyleBackColor = true;
		((Control)btn_StandbySet).Click += btn_StandbySet_Click;
		((Control)lbl_Standby).AutoSize = true;
		((Control)lbl_Standby).Location = new Point(6, 50);
		((Control)lbl_Standby).Name = "lbl_Standby";
		((Control)lbl_Standby).Size = new Size(89, 12);
		((Control)lbl_Standby).TabIndex = 4;
		((Control)lbl_Standby).Text = "Standby Title:";
		lbl_Standby.TextAlign = (ContentAlignment)16;
		((Control)groupBox5).Controls.Add((Control)(object)txt_Welcome);
		((Control)groupBox5).Controls.Add((Control)(object)btn_StandbySet);
		((Control)groupBox5).Controls.Add((Control)(object)lbl_Standby);
		((Control)groupBox5).Controls.Add((Control)(object)btn_StandbyGet);
		((Control)groupBox5).Controls.Add((Control)(object)lbl_WelCome);
		((Control)groupBox5).Controls.Add((Control)(object)btn_WelcomeSet);
		((Control)groupBox5).Controls.Add((Control)(object)txt_Standby);
		((Control)groupBox5).Controls.Add((Control)(object)btn_WelcomeGet);
		((Control)groupBox5).Location = new Point(12, 284);
		((Control)groupBox5).Name = "groupBox5";
		((Control)groupBox5).Size = new Size(752, 78);
		((Control)groupBox5).TabIndex = 4;
		groupBox5.TabStop = false;
		((Control)groupBox5).Text = "Title Settings";
		((Control)groupBox6).Controls.Add((Control)(object)btn_ModelGet);
		((Control)groupBox6).Controls.Add((Control)(object)txt_Model);
		((Control)groupBox6).Controls.Add((Control)(object)btn_FirmwareVersionGet);
		((Control)groupBox6).Controls.Add((Control)(object)lbl_Model);
		((Control)groupBox6).Controls.Add((Control)(object)txt_FirmwareVersion);
		((Control)groupBox6).Controls.Add((Control)(object)lbl_FirmwareVersion);
		((Control)groupBox6).Location = new Point(12, 368);
		((Control)groupBox6).Name = "groupBox6";
		((Control)groupBox6).Size = new Size(752, 100);
		((Control)groupBox6).TabIndex = 5;
		groupBox6.TabStop = false;
		((Control)groupBox6).Text = "Device Information";
		((Control)btn_ModelGet).Location = new Point(321, 47);
		((Control)btn_ModelGet).Name = "btn_ModelGet";
		((Control)btn_ModelGet).Size = new Size(75, 23);
		((Control)btn_ModelGet).TabIndex = 5;
		((Control)btn_ModelGet).Text = "Get";
		((ButtonBase)btn_ModelGet).UseVisualStyleBackColor = true;
		((Control)btn_ModelGet).Click += btn_ModelGet_Click;
		((Control)txt_Model).BackColor = Color.White;
		((Control)txt_Model).Location = new Point(113, 49);
		((TextBoxBase)txt_Model).MaxLength = 16;
		((Control)txt_Model).Name = "txt_Model";
		((Control)txt_Model).Size = new Size(202, 21);
		((Control)txt_Model).TabIndex = 4;
		((Control)btn_FirmwareVersionGet).Location = new Point(321, 18);
		((Control)btn_FirmwareVersionGet).Name = "btn_FirmwareVersionGet";
		((Control)btn_FirmwareVersionGet).Size = new Size(75, 23);
		((Control)btn_FirmwareVersionGet).TabIndex = 2;
		((Control)btn_FirmwareVersionGet).Text = "Get";
		((ButtonBase)btn_FirmwareVersionGet).UseVisualStyleBackColor = true;
		((Control)btn_FirmwareVersionGet).Click += btn_FirmwareVersionGet_Click;
		((Control)lbl_Model).AutoSize = true;
		((Control)lbl_Model).Location = new Point(66, 52);
		((Control)lbl_Model).Name = "lbl_Model";
		((Control)lbl_Model).Size = new Size(41, 12);
		((Control)lbl_Model).TabIndex = 3;
		((Control)lbl_Model).Text = "Model:";
		lbl_Model.TextAlign = (ContentAlignment)16;
		((Control)txt_FirmwareVersion).BackColor = Color.White;
		((Control)txt_FirmwareVersion).Location = new Point(113, 20);
		((Control)txt_FirmwareVersion).Name = "txt_FirmwareVersion";
		((TextBoxBase)txt_FirmwareVersion).ReadOnly = true;
		((Control)txt_FirmwareVersion).Size = new Size(202, 21);
		((Control)txt_FirmwareVersion).TabIndex = 1;
		((Control)lbl_FirmwareVersion).AutoSize = true;
		((Control)lbl_FirmwareVersion).Location = new Point(6, 23);
		((Control)lbl_FirmwareVersion).Name = "lbl_FirmwareVersion";
		((Control)lbl_FirmwareVersion).Size = new Size(101, 12);
		((Control)lbl_FirmwareVersion).TabIndex = 0;
		((Control)lbl_FirmwareVersion).Text = "FirmwareVersion:";
		lbl_FirmwareVersion.TextAlign = (ContentAlignment)16;
		((Control)groupBox7).Controls.Add((Control)(object)btn_UploadSound);
		((Control)groupBox7).Controls.Add((Control)(object)cbo_SoundType);
		((Control)groupBox7).Controls.Add((Control)(object)lbl_SoundType);
		((Control)groupBox7).Location = new Point(12, 474);
		((Control)groupBox7).Name = "groupBox7";
		((Control)groupBox7).Size = new Size(752, 51);
		((Control)groupBox7).TabIndex = 6;
		groupBox7.TabStop = false;
		((Control)groupBox7).Text = "Upload Sound";
		((Control)btn_UploadSound).Location = new Point(173, 18);
		((Control)btn_UploadSound).Name = "btn_UploadSound";
		((Control)btn_UploadSound).Size = new Size(75, 23);
		((Control)btn_UploadSound).TabIndex = 2;
		((Control)btn_UploadSound).Text = "Upload";
		((ButtonBase)btn_UploadSound).UseVisualStyleBackColor = true;
		((Control)btn_UploadSound).Click += btn_UploadSound_Click;
		cbo_SoundType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_SoundType).FormattingEnabled = true;
		cbo_SoundType.Items.AddRange(new object[2] { "User Defined 1", "User Defined 2" });
		((Control)cbo_SoundType).Location = new Point(47, 20);
		((Control)cbo_SoundType).Name = "cbo_SoundType";
		((Control)cbo_SoundType).Size = new Size(120, 20);
		((Control)cbo_SoundType).TabIndex = 1;
		((Control)lbl_SoundType).AutoSize = true;
		((Control)lbl_SoundType).Location = new Point(6, 23);
		((Control)lbl_SoundType).Name = "lbl_SoundType";
		((Control)lbl_SoundType).Size = new Size(35, 12);
		((Control)lbl_SoundType).TabIndex = 0;
		((Control)lbl_SoundType).Text = "Type:";
		lbl_SoundType.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(773, 564);
		((Control)this).Controls.Add((Control)(object)groupBox7);
		((Control)this).Controls.Add((Control)(object)groupBox6);
		((Control)this).Controls.Add((Control)(object)groupBox5);
		((Control)this).Controls.Add((Control)(object)btn_Reset);
		((Control)this).Controls.Add((Control)(object)btn_MacAddress);
		((Control)this).Controls.Add((Control)(object)groupBox4);
		((Control)this).Controls.Add((Control)(object)groupBox3);
		((Control)this).Controls.Add((Control)(object)groupBox2);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "SystemSettingForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "System Parameter Settings";
		((Form)this).Load += SystemSettingForm_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)groupBox3).ResumeLayout(false);
		((Control)groupBox3).PerformLayout();
		((Control)groupBox4).ResumeLayout(false);
		((Control)groupBox4).PerformLayout();
		((Control)groupBox5).ResumeLayout(false);
		((Control)groupBox5).PerformLayout();
		((Control)groupBox6).ResumeLayout(false);
		((Control)groupBox6).PerformLayout();
		((Control)groupBox7).ResumeLayout(false);
		((Control)groupBox7).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
