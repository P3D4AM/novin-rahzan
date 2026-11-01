using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.Entity;
using ZDC2911.IConvert;
using ZDC2911.SysEnum;

namespace ZDC2911.UI;

public class EnrollForm : Form
{
	private byte[] fpBytes;

	private Device device;

	private DeviceConnection deviceConnection;

	private Zd2911EnrollFileManagement enrollFileMgr;

	private IContainer components = null;

	private Label lbl_UserId;

	private Label lbl_FpNo;

	private ComboBox cbo_FpNo;

	private Label lbl_Role;

	private ComboBox cbo_Role;

	private Label lbl_Pwd;

	private TextBox txt_Pwd;

	private Label lbl_Card;

	private TextBox txt_Card;

	private Label lbl_UserName;

	private TextBox txt_UserName;

	private Label lbl_FpData;

	private TextBox txt_UserFpData;

	private Button btn_SaveFpData;

	private Button btn_OpenFpData;

	private Button btn_ReadFp;

	private Button btn_WriteFp;

	private Button btn_DeleteFp;

	private Button btn_ReadPwd;

	private Button btn_WritePwd;

	private Button btn_DeletePwd;

	private Button btn_ReadCard;

	private Button btn_WriteCard;

	private Button btn_DeleteCard;

	private Button btn_ReadUserName;

	private Button btn_WriteUserName;

	private Button btn_DeleteUser;

	private Button btn_DeleteAllUser;

	private Button btn_SetRole;

	private Button btn_GetAllEnrollData;

	private Button btn_GetUserInfoByUserId;

	private Button btn_UploadUserData;

	private Button btn_DownloadUserData;

	private ListView lvw_UserList;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader4;

	private ColumnHeader columnHeader5;

	private ColumnHeader columnHeader6;

	private ColumnHeader columnHeader7;

	private ColumnHeader columnHeader8;

	private ColumnHeader columnHeader9;

	private ColumnHeader columnHeader10;

	private ColumnHeader columnHeader11;

	private ColumnHeader columnHeader12;

	private ColumnHeader columnHeader13;

	private ColumnHeader columnHeader14;

	private ColumnHeader columnHeader15;

	private ColumnHeader columnHeader16;

	private Button btn_Select;

	private Button btn_Open;

	private Button btn_Save;

	private SaveFileDialog sfd_SaveFpData;

	private OpenFileDialog ofd_OpenFpData;

	private Label ExtInfoLabel;

	private TextBox ExtInfoTextBox;

	private Button getExtInfoButton;

	private Button setExtInfoButton;

	private NumericUpDown nud_DIN;

	private ColumnHeader columnHeader3;

	private Button btn_SaveOneEnroll;

	private Button btn_OpenOneEnroll;

	public EnrollForm(DeviceCommEty deviceEty)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		InitializeComponent();
		device = deviceEty.Device;
		deviceConnection = deviceEty.DeviceConnection;
		fpBytes = new byte[498];
		enrollFileMgr = new Zd2911EnrollFileManagement();
	}

	private int GetPrivilege()
	{
		return ((ListControl)cbo_Role).SelectedIndex switch
		{
			0 => 1, 
			1 => 8, 
			2 => 2, 
			3 => 4, 
			4 => 16, 
			_ => 0, 
		};
	}

	private void AddUserInfoToViewList(User user, int no)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected I4, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected I4, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected I4, but got Unknown
		ListViewItem val = new ListViewItem(no.ToString());
		val.SubItems.Add(user.DIN.ToString());
		val.SubItems.Add(user.UserName);
		Enroll val2 = user.Enrolls[0];
		for (int i = 0; i < 10; i++)
		{
			if (Zd2911Utils.BitCheck((int)val2.EnrollType, i) != 0)
			{
				val.SubItems.Add("Y");
			}
			else
			{
				val.SubItems.Add("N");
			}
		}
		if (Zd2911Utils.BitCheck((int)val2.EnrollType, 10) != 0)
		{
			val.SubItems.Add("Y");
		}
		else
		{
			val.SubItems.Add("N");
		}
		if (Zd2911Utils.BitCheck((int)val2.EnrollType, 11) != 0)
		{
			val.SubItems.Add("Y");
		}
		else
		{
			val.SubItems.Add("N");
		}
		string userPrivilege = ConvertObject.GetUserPrivilege((UserPrivilege)user.Privilege);
		val.SubItems.Add(userPrivilege);
		val.Tag = user;
		lvw_UserList.Items.Add(val);
	}

	private void btn_SaveFpData_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)1;
		if (!((object)(DialogResult)(ref val)).Equals((object?)((CommonDialog)sfd_SaveFpData).ShowDialog()))
		{
			return;
		}
		FileStream fileStream = new FileStream(((FileDialog)sfd_SaveFpData).FileName, FileMode.Create);
		try
		{
			fileStream.Write(fpBytes, 0, fpBytes.Length);
			fileStream.Flush();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			fileStream.Close();
		}
	}

	private void btn_OpenFpData_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)1;
		if (!((object)(DialogResult)(ref val)).Equals((object?)((CommonDialog)ofd_OpenFpData).ShowDialog()))
		{
			return;
		}
		FileStream fileStream = new FileStream(((FileDialog)ofd_OpenFpData).FileName, FileMode.Open);
		try
		{
			fileStream.Read(fpBytes, 0, fpBytes.Length);
			((Control)txt_UserFpData).Text = ConvertObject.ConvertByteToHex(fpBytes).Replace("-", " ");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
		finally
		{
			fileStream.Close();
		}
	}

	private void btn_ReadFp_Click(object sender, EventArgs e)
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_FpNo).SelectedIndex)
		{
			MessageBox.Show("Please Select Finger SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_FpNo).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			User val = new User();
			Enroll val2 = new Enroll();
			val.DIN = (ulong)nud_DIN.Value;
			val2.DIN = val.DIN;
			val2.EnrollType = (EnrollType)((ListControl)cbo_FpNo).SelectedIndex;
			val2.Fingerprint = new byte[498];
			val.Enrolls.Add(val2);
			obj = (object)(UserEnrollCommand)0;
			if (deviceConnection.GetProperty((UserProperty)7, obj, ref val, ref obj2))
			{
				fpBytes = val.Enrolls[0].Fingerprint;
				((Control)txt_UserFpData).Text = ConvertObject.ConvertByteToHex(fpBytes).Replace("-", " ");
			}
			else
			{
				MessageBox.Show("Read FP Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_WriteFp_Click(object sender, EventArgs e)
	{
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_FpNo).SelectedIndex)
		{
			MessageBox.Show("Please Select Finger SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_FpNo).Focus();
			return;
		}
		if (-1 == ((ListControl)cbo_Role).SelectedIndex)
		{
			MessageBox.Show("Please Select Privilege", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Role).Focus();
			return;
		}
		if (string.Empty.Equals(((Control)txt_UserFpData).Text.Trim()))
		{
			MessageBox.Show("Current FP Data is Empty", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			User val = new User();
			Enroll val2 = new Enroll();
			val.DIN = (ulong)nud_DIN.Value;
			val.Privilege = GetPrivilege();
			val2.DIN = val.DIN;
			val2.EnrollType = (EnrollType)((ListControl)cbo_FpNo).SelectedIndex;
			byte[] array = new byte[498 * (((ListControl)cbo_FpNo).SelectedIndex + 1)];
			Array.Copy(fpBytes, 0, array, ((ListControl)cbo_FpNo).SelectedIndex * 498, fpBytes.Length);
			val2.Fingerprint = array;
			val.Enrolls.Add(val2);
			obj = (object)(UserEnrollCommand)1;
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Write FP Data Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Write FP Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_DeleteFp_Click(object sender, EventArgs e)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_FpNo).SelectedIndex)
		{
			MessageBox.Show("Please Select Finger SN", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_FpNo).Focus();
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			User val = new User();
			Enroll val2 = new Enroll();
			val.DIN = (ulong)nud_DIN.Value;
			val2.DIN = val.DIN;
			val2.EnrollType = (EnrollType)((ListControl)cbo_FpNo).SelectedIndex;
			val.Enrolls.Add(val2);
			obj = (object)(UserEnrollCommand)2;
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Clear FP Data Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear FP Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_ReadPwd_Click(object sender, EventArgs e)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)3;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val.Enrolls.Add(val2);
			if (deviceConnection.GetProperty((UserProperty)7, obj, ref val, ref obj2))
			{
				((Control)txt_Pwd).Text = val.Enrolls[0].Password.Replace("\0", "");
			}
			else
			{
				MessageBox.Show("Read Password Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_WritePwd_Click(object sender, EventArgs e)
	{
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_Role).SelectedIndex)
		{
			MessageBox.Show("Please Select Privilege", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Role).Focus();
			return;
		}
		if (string.Empty.Equals(((Control)txt_Pwd).Text.Trim()))
		{
			MessageBox.Show("Please Input Password", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)txt_Pwd).Focus();
			return;
		}
		if (!ConvertObject.IsInt(((Control)txt_Pwd).Text.Trim()))
		{
			MessageBox.Show("Password can only enter a number", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)txt_Pwd).Focus();
			return;
		}
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)4;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			val.Privilege = GetPrivilege();
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val2.Password = ((Control)txt_Pwd).Text.Trim();
			val.Enrolls.Add(val2);
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Write Password Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Wrtie Password Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_DeletePwd_Click(object sender, EventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)5;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val.Enrolls.Add(val2);
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Clear Password Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear Password Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_ReadCard_Click(object sender, EventArgs e)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)6;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val.Enrolls.Add(val2);
			if (deviceConnection.GetProperty((UserProperty)7, obj, ref val, ref obj2))
			{
				((Control)txt_Card).Text = val.Enrolls[0].CardID;
			}
			else
			{
				MessageBox.Show("Read Card Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_WriteCard_Click(object sender, EventArgs e)
	{
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_Role).SelectedIndex)
		{
			MessageBox.Show("Please Select Privilege", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Role).Focus();
			return;
		}
		if (string.Empty.Equals(((Control)txt_Card).Text.Trim()))
		{
			MessageBox.Show("Please Input Card ID", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)txt_Card).Focus();
			return;
		}
		if (!ConvertObject.IsInt(((Control)txt_Card).Text.Trim()))
		{
			MessageBox.Show("Card ID can only enter a number", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)txt_Card).Focus();
			return;
		}
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)7;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			val.Privilege = GetPrivilege();
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val2.CardID = ((Control)txt_Card).Text.Trim();
			val.Enrolls.Add(val2);
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Write Card Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Write Card Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_DeleteCard_Click(object sender, EventArgs e)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			obj = (object)(UserEnrollCommand)8;
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			Enroll val2 = new Enroll();
			val2.DIN = val.DIN;
			val.Enrolls.Add(val2);
			if (deviceConnection.SetProperty((UserProperty)7, obj, val, obj2))
			{
				MessageBox.Show("Clear Card Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear Card Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_ReadUserName_Click(object sender, EventArgs e)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			if (deviceConnection.GetProperty((UserProperty)0, obj, ref val, ref obj2))
			{
				((Control)txt_UserName).Text = val.UserName;
			}
			else
			{
				MessageBox.Show("Get Username Fial", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_WriteUserName_Click(object sender, EventArgs e)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (string.Empty.Equals(((Control)txt_UserName).Text.Trim()))
		{
			MessageBox.Show("Please Input Username", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)txt_UserName).Focus();
			return;
		}
		try
		{
			object obj = new object();
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			val.UserName = ((Control)txt_UserName).Text.Trim();
			if (deviceConnection.SetProperty((UserProperty)0, obj, val, obj2))
			{
				MessageBox.Show("Set Username Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Username Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_SetRole_Click(object sender, EventArgs e)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if (-1 == ((ListControl)cbo_Role).SelectedIndex)
		{
			MessageBox.Show("Please Select Privilege", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)cbo_Role).Focus();
			return;
		}
		try
		{
			object obj = new object();
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			val.Privilege = GetPrivilege();
			if (deviceConnection.SetProperty((UserProperty)5, obj, val, obj2))
			{
				MessageBox.Show("Set Privilege Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set Privilege Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_DeleteUser_Click(object sender, EventArgs e)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			obj2 = (ulong)nud_DIN.Value;
			if (deviceConnection.SetProperty((DeviceProperty)14, obj, device, obj2))
			{
				MessageBox.Show("Clear User Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear User Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_DeleteAllUser_Click(object sender, EventArgs e)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj2 = 0uL;
			if (deviceConnection.SetProperty((DeviceProperty)14, obj, device, obj2))
			{
				MessageBox.Show("Clear All User Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Clear All User Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_GetUserInfoByUserId_Click(object sender, EventArgs e)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			obj = (ulong)nud_DIN.Value;
			object obj2 = new object();
			if (deviceConnection.GetProperty((DeviceProperty)14, obj, ref device, ref obj2))
			{
				User user = (User)obj2;
				EnrollDetailForm enrollDetailForm = new EnrollDetailForm(user);
				((Form)enrollDetailForm).ShowDialog();
			}
			else
			{
				MessageBox.Show("Get Enroll Information Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_GetAllEnrollData_Click(object sender, EventArgs e)
	{
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected I4, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected I4, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected I4, but got Unknown
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj = 0uL;
			if (deviceConnection.GetProperty((DeviceProperty)14, obj, ref device, ref obj2))
			{
				if (((Control)btn_Select).Text.Equals("Deselect All"))
				{
					((Control)btn_Select).Text = "Select All";
				}
				lvw_UserList.Items.Clear();
				List<User> list = (List<User>)obj2;
				int num = 1;
				{
					foreach (User item in list)
					{
						Enroll val = item.Enrolls[0];
						ListViewItem val2 = new ListViewItem(num.ToString());
						val2.SubItems.Add(item.DIN.ToString());
						val2.SubItems.Add(item.UserName);
						for (int i = 0; i < 10; i++)
						{
							if (Zd2911Utils.BitCheck((int)val.EnrollType, i) != 0)
							{
								val2.SubItems.Add("Y");
							}
							else
							{
								val2.SubItems.Add("N");
							}
						}
						if (Zd2911Utils.BitCheck((int)val.EnrollType, 10) != 0)
						{
							val2.SubItems.Add("Y");
						}
						else
						{
							val2.SubItems.Add("N");
						}
						if (Zd2911Utils.BitCheck((int)val.EnrollType, 11) != 0)
						{
							val2.SubItems.Add("Y");
						}
						else
						{
							val2.SubItems.Add("N");
						}
						string userPrivilege = ConvertObject.GetUserPrivilege((UserPrivilege)item.Privilege);
						val2.SubItems.Add(userPrivilege);
						lvw_UserList.Items.Add(val2);
						num++;
					}
					return;
				}
			}
			MessageBox.Show("Get All Enroll Information Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
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

	private void btn_Select_Click(object sender, EventArgs e)
	{
		if (lvw_UserList.Items.Count == 0)
		{
			return;
		}
		if (((Control)btn_Select).Text.Equals("Select All"))
		{
			for (int i = 0; i < lvw_UserList.Items.Count; i++)
			{
				lvw_UserList.Items[i].Checked = true;
			}
			((Control)btn_Select).Text = "Deselect All";
		}
		else
		{
			for (int j = 0; j < lvw_UserList.Items.Count; j++)
			{
				lvw_UserList.Items[j].Checked = false;
			}
			((Control)btn_Select).Text = "Select All";
		}
	}

	private void btn_DownloadUserData_Click(object sender, EventArgs e)
	{
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected I4, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected I4, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected I4, but got Unknown
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			obj = 0uL;
			if (!deviceConnection.GetProperty((DeviceProperty)14, obj, ref device, ref obj2))
			{
				MessageBox.Show("Get All Enroll Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			int num = 1;
			if (((Control)btn_Select).Text.Equals("Deselect All"))
			{
				((Control)btn_Select).Text = "Select All";
			}
			lvw_UserList.Items.Clear();
			List<User> list = (List<User>)obj2;
			foreach (User item in list)
			{
				Enroll val = item.Enrolls[0];
				ListViewItem val2 = new ListViewItem(num.ToString());
				val2.SubItems.Add(item.DIN.ToString());
				User val3 = item;
				if (!deviceConnection.GetProperty((UserProperty)1, obj, ref val3, ref obj2))
				{
					MessageBox.Show("Get All Enroll Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					break;
				}
				val2.SubItems.Add(val3.UserName);
				for (int i = 0; i < 10; i++)
				{
					if (Zd2911Utils.BitCheck((int)val.EnrollType, i) != 0)
					{
						val2.SubItems.Add("Y");
					}
					else
					{
						val2.SubItems.Add("N");
					}
				}
				if (Zd2911Utils.BitCheck((int)val.EnrollType, 10) != 0)
				{
					val2.SubItems.Add("Y");
				}
				else
				{
					val2.SubItems.Add("N");
				}
				if (Zd2911Utils.BitCheck((int)val.EnrollType, 11) != 0)
				{
					val2.SubItems.Add("Y");
				}
				else
				{
					val2.SubItems.Add("N");
				}
				string userPrivilege = ConvertObject.GetUserPrivilege((UserPrivilege)item.Privilege);
				val2.SubItems.Add(userPrivilege);
				val2.Tag = val3;
				lvw_UserList.Items.Add(val2);
				num++;
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

	private void btn_UploadUserData_Click(object sender, EventArgs e)
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.Items.Count == 0)
		{
			MessageBox.Show("Enroll Data is Empty", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		if (lvw_UserList.CheckedItems.Count == 0)
		{
			MessageBox.Show("Please Select the Record After Operation", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		object obj = new object();
		object obj2 = new object();
		obj2 = 1L;
		try
		{
			bool flag = deviceConnection.SetProperty((DeviceProperty)16, obj, device, obj2);
			foreach (ListViewItem checkedItem in lvw_UserList.CheckedItems)
			{
				ListViewItem val = checkedItem;
				object tag = val.Tag;
				User val2 = (User)((tag is User) ? tag : null);
				if (val2 == null)
				{
					MessageBox.Show("Enroll Data is Empty", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
					return;
				}
				obj2 = false;
				if (!deviceConnection.SetProperty((UserProperty)1, obj, val2, obj2))
				{
					MessageBox.Show("Set All Enroll Data Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					return;
				}
			}
			MessageBox.Show("Set All Enroll Data Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
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

	private void btn_Save_Click(object sender, EventArgs e)
	{
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.Items.Count == 0)
		{
			MessageBox.Show("No user enroll information, cannot be saved", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		if (lvw_UserList.CheckedItems.Count == 0)
		{
			MessageBox.Show("Please select the record after the operation", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		try
		{
			List<User> list = new List<User>();
			foreach (ListViewItem checkedItem in lvw_UserList.CheckedItems)
			{
				ListViewItem val = checkedItem;
				object tag = val.Tag;
				User val2 = (User)((tag is User) ? tag : null);
				if (val2 == null)
				{
					MessageBox.Show("No user enroll information, cannot be saved", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
					return;
				}
				list.Add(val2);
			}
			FolderBrowserDialog val3 = new FolderBrowserDialog();
			DialogResult val4 = (DialogResult)1;
			if (!((object)(DialogResult)(ref val4)).Equals((object?)((CommonDialog)val3).ShowDialog()))
			{
				return;
			}
			string text = val3.SelectedPath + "\\UserData";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			text += "\\AllEnrollData.fps";
			if (File.Exists(text))
			{
				val4 = (DialogResult)7;
				if (((object)(DialogResult)(ref val4)).Equals((object?)MessageBox.Show("The file AllEnrollData.fps already exists, overwrite", "Prompt", (MessageBoxButtons)4, (MessageBoxIcon)64)))
				{
					return;
				}
			}
			if (enrollFileMgr.SaveAllUserEnrollDataAsDB(text, list))
			{
				MessageBox.Show("Save All Enroll to File Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Save All Enroll to File Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_Open_Click(object sender, EventArgs e)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			((FileDialog)val).Filter = "AllEnrollData File|AllEnrollData.fps";
			((FileDialog)val).FileName = "AllEnrollData.fps";
			DialogResult val2 = (DialogResult)1;
			if (!((object)(DialogResult)(ref val2)).Equals((object?)((CommonDialog)val).ShowDialog()))
			{
				return;
			}
			lvw_UserList.Items.Clear();
			if (((Control)btn_Select).Text.Equals("Deselect All"))
			{
				((Control)btn_Select).Text = "Select All";
			}
			List<User> list = new List<User>();
			if (!enrollFileMgr.LoadAllUserEnrollDataFromDB(((FileDialog)val).FileName, ref list))
			{
				MessageBox.Show("打开文件失败", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
				return;
			}
			int num = 1;
			foreach (User item in list)
			{
				AddUserInfoToViewList(item, num);
				num++;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void getExtInfoButton_Click(object sender, EventArgs e)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object obj = new object();
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			if (deviceConnection.GetProperty((UserProperty)2, obj, ref val, ref obj2))
			{
				((Control)ExtInfoTextBox).Text = val.Comment;
			}
			else
			{
				MessageBox.Show("Get User Ext Info Failed.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ERROR", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void setExtInfoButton_Click(object sender, EventArgs e)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (string.Empty.Equals(((Control)ExtInfoTextBox).Text.Trim()))
		{
			MessageBox.Show("Please Enter User Ext Info", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Control)ExtInfoTextBox).Focus();
			return;
		}
		try
		{
			object obj = new object();
			object obj2 = new object();
			User val = new User();
			val.DIN = (ulong)nud_DIN.Value;
			val.Comment = ((Control)ExtInfoTextBox).Text.Trim();
			if (deviceConnection.SetProperty((UserProperty)2, obj, val, obj2))
			{
				MessageBox.Show("Set User Ext Info Success.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			else
			{
				MessageBox.Show("Set User Ext Info Failed.", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ERROR", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_OpenOneEnroll_Click(object sender, EventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			((FileDialog)val).Filter = "OneEnrollData File|OD_*.db";
			val.Multiselect = true;
			DialogResult val2 = (DialogResult)1;
			if (!((object)(DialogResult)(ref val2)).Equals((object?)((CommonDialog)val).ShowDialog()))
			{
				return;
			}
			lvw_UserList.Items.Clear();
			if (((Control)btn_Select).Text.Equals("Deselect All"))
			{
				((Control)btn_Select).Text = "Select All";
			}
			int num = 1;
			string[] fileNames = ((FileDialog)val).FileNames;
			foreach (string text in fileNames)
			{
				User user = new User();
				if (!enrollFileMgr.LoadUserEnrollDataFromDB(text, ref user))
				{
					MessageBox.Show("打开文件失败", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
					break;
				}
				AddUserInfoToViewList(user, num);
				num++;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_SaveOneEnroll_Click(object sender, EventArgs e)
	{
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.Items.Count == 0)
		{
			MessageBox.Show("No user enroll information, cannot be saved", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		if (lvw_UserList.CheckedItems.Count == 0)
		{
			MessageBox.Show("Please select the record after the operation", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
			return;
		}
		try
		{
			FolderBrowserDialog val = new FolderBrowserDialog();
			DialogResult val2 = (DialogResult)1;
			if (!((object)(DialogResult)(ref val2)).Equals((object?)((CommonDialog)val).ShowDialog()))
			{
				return;
			}
			foreach (ListViewItem checkedItem in lvw_UserList.CheckedItems)
			{
				ListViewItem val3 = checkedItem;
				object tag = val3.Tag;
				User val4 = (User)((tag is User) ? tag : null);
				if (val4 == null)
				{
					MessageBox.Show("No user enroll information, cannot be saved", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
					return;
				}
				string text = val.SelectedPath + "\\UserData";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				text = text + "\\OD_" + val4.DIN.ToString().PadLeft(18, '0') + ".db";
				if (File.Exists(text))
				{
					val2 = (DialogResult)7;
					if (((object)(DialogResult)(ref val2)).Equals((object?)MessageBox.Show(string.Format("The file {0} already exists, overwrite?", "OD_" + val4.DIN.ToString().PadLeft(18, '0') + ".db"), "Prompt", (MessageBoxButtons)4, (MessageBoxIcon)64)))
					{
						continue;
					}
				}
				if (!enrollFileMgr.SaveUserEnrollDataAsDB(text, val4))
				{
					MessageBox.Show("Save One Enroll to File Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
					return;
				}
			}
			MessageBox.Show("Save One Enroll to File Success", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
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
		lbl_UserId = new Label();
		lbl_FpNo = new Label();
		cbo_FpNo = new ComboBox();
		lbl_Role = new Label();
		cbo_Role = new ComboBox();
		lbl_Pwd = new Label();
		txt_Pwd = new TextBox();
		lbl_Card = new Label();
		txt_Card = new TextBox();
		lbl_UserName = new Label();
		txt_UserName = new TextBox();
		lbl_FpData = new Label();
		txt_UserFpData = new TextBox();
		btn_SaveFpData = new Button();
		btn_OpenFpData = new Button();
		btn_ReadFp = new Button();
		btn_WriteFp = new Button();
		btn_DeleteFp = new Button();
		btn_ReadPwd = new Button();
		btn_WritePwd = new Button();
		btn_DeletePwd = new Button();
		btn_ReadCard = new Button();
		btn_WriteCard = new Button();
		btn_DeleteCard = new Button();
		btn_ReadUserName = new Button();
		btn_WriteUserName = new Button();
		btn_DeleteUser = new Button();
		btn_DeleteAllUser = new Button();
		btn_SetRole = new Button();
		btn_GetAllEnrollData = new Button();
		btn_GetUserInfoByUserId = new Button();
		btn_UploadUserData = new Button();
		btn_DownloadUserData = new Button();
		lvw_UserList = new ListView();
		columnHeader1 = new ColumnHeader();
		columnHeader2 = new ColumnHeader();
		columnHeader3 = new ColumnHeader();
		columnHeader4 = new ColumnHeader();
		columnHeader5 = new ColumnHeader();
		columnHeader6 = new ColumnHeader();
		columnHeader7 = new ColumnHeader();
		columnHeader8 = new ColumnHeader();
		columnHeader9 = new ColumnHeader();
		columnHeader10 = new ColumnHeader();
		columnHeader11 = new ColumnHeader();
		columnHeader12 = new ColumnHeader();
		columnHeader13 = new ColumnHeader();
		columnHeader14 = new ColumnHeader();
		columnHeader15 = new ColumnHeader();
		columnHeader16 = new ColumnHeader();
		btn_Select = new Button();
		btn_Open = new Button();
		btn_Save = new Button();
		sfd_SaveFpData = new SaveFileDialog();
		ofd_OpenFpData = new OpenFileDialog();
		ExtInfoLabel = new Label();
		ExtInfoTextBox = new TextBox();
		getExtInfoButton = new Button();
		setExtInfoButton = new Button();
		nud_DIN = new NumericUpDown();
		btn_SaveOneEnroll = new Button();
		btn_OpenOneEnroll = new Button();
		((ISupportInitialize)nud_DIN).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lbl_UserId).AutoSize = true;
		((Control)lbl_UserId).Location = new Point(24, 9);
		((Control)lbl_UserId).Name = "lbl_UserId";
		((Control)lbl_UserId).Size = new Size(53, 12);
		((Control)lbl_UserId).TabIndex = 0;
		((Control)lbl_UserId).Text = "User ID:";
		lbl_UserId.TextAlign = (ContentAlignment)32;
		((Control)lbl_FpNo).AutoSize = true;
		((Control)lbl_FpNo).Location = new Point(12, 36);
		((Control)lbl_FpNo).Name = "lbl_FpNo";
		((Control)lbl_FpNo).Size = new Size(65, 12);
		((Control)lbl_FpNo).TabIndex = 2;
		((Control)lbl_FpNo).Text = "Finger SN:";
		lbl_FpNo.TextAlign = (ContentAlignment)32;
		cbo_FpNo.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_FpNo).FormattingEnabled = true;
		cbo_FpNo.Items.AddRange(new object[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
		((Control)cbo_FpNo).Location = new Point(83, 33);
		((Control)cbo_FpNo).Name = "cbo_FpNo";
		((Control)cbo_FpNo).Size = new Size(150, 20);
		((Control)cbo_FpNo).TabIndex = 3;
		((Control)lbl_Role).AutoSize = true;
		((Control)lbl_Role).Location = new Point(12, 62);
		((Control)lbl_Role).Name = "lbl_Role";
		((Control)lbl_Role).Size = new Size(65, 12);
		((Control)lbl_Role).TabIndex = 4;
		((Control)lbl_Role).Text = "Privilege:";
		lbl_Role.TextAlign = (ContentAlignment)32;
		cbo_Role.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbo_Role).FormattingEnabled = true;
		cbo_Role.Items.AddRange(new object[5] { "User", "Enroll User", "View User", "Super User", "Customer" });
		((Control)cbo_Role).Location = new Point(83, 59);
		((Control)cbo_Role).Name = "cbo_Role";
		((Control)cbo_Role).Size = new Size(150, 20);
		((Control)cbo_Role).TabIndex = 5;
		((Control)lbl_Pwd).AutoSize = true;
		((Control)lbl_Pwd).Location = new Point(18, 88);
		((Control)lbl_Pwd).Name = "lbl_Pwd";
		((Control)lbl_Pwd).Size = new Size(59, 12);
		((Control)lbl_Pwd).TabIndex = 6;
		((Control)lbl_Pwd).Text = "Password:";
		lbl_Pwd.TextAlign = (ContentAlignment)32;
		((Control)txt_Pwd).Location = new Point(83, 85);
		((TextBoxBase)txt_Pwd).MaxLength = 8;
		((Control)txt_Pwd).Name = "txt_Pwd";
		((Control)txt_Pwd).Size = new Size(150, 21);
		((Control)txt_Pwd).TabIndex = 7;
		((Control)lbl_Card).AutoSize = true;
		((Control)lbl_Card).Location = new Point(24, 115);
		((Control)lbl_Card).Name = "lbl_Card";
		((Control)lbl_Card).Size = new Size(53, 12);
		((Control)lbl_Card).TabIndex = 8;
		((Control)lbl_Card).Text = "Card ID:";
		lbl_Card.TextAlign = (ContentAlignment)32;
		((Control)txt_Card).Location = new Point(83, 112);
		((TextBoxBase)txt_Card).MaxLength = 10;
		((Control)txt_Card).Name = "txt_Card";
		((Control)txt_Card).Size = new Size(150, 21);
		((Control)txt_Card).TabIndex = 9;
		((Control)lbl_UserName).AutoSize = true;
		((Control)lbl_UserName).Location = new Point(18, 142);
		((Control)lbl_UserName).Name = "lbl_UserName";
		((Control)lbl_UserName).Size = new Size(59, 12);
		((Control)lbl_UserName).TabIndex = 10;
		((Control)lbl_UserName).Text = "Username:";
		lbl_UserName.TextAlign = (ContentAlignment)32;
		((Control)txt_UserName).Location = new Point(83, 139);
		((Control)txt_UserName).Name = "txt_UserName";
		((Control)txt_UserName).Size = new Size(150, 21);
		((Control)txt_UserName).TabIndex = 11;
		((Control)lbl_FpData).AutoSize = true;
		((Control)lbl_FpData).Location = new Point(12, 194);
		((Control)lbl_FpData).Name = "lbl_FpData";
		((Control)lbl_FpData).Size = new Size(113, 12);
		((Control)lbl_FpData).TabIndex = 12;
		((Control)lbl_FpData).Text = "Fringerprint Data:";
		lbl_FpData.TextAlign = (ContentAlignment)32;
		((Control)txt_UserFpData).BackColor = Color.White;
		((Control)txt_UserFpData).Location = new Point(12, 209);
		((TextBoxBase)txt_UserFpData).Multiline = true;
		((Control)txt_UserFpData).Name = "txt_UserFpData";
		((TextBoxBase)txt_UserFpData).ReadOnly = true;
		txt_UserFpData.ScrollBars = (ScrollBars)2;
		((Control)txt_UserFpData).Size = new Size(221, 358);
		((Control)txt_UserFpData).TabIndex = 13;
		((Control)btn_SaveFpData).Location = new Point(138, 573);
		((Control)btn_SaveFpData).Name = "btn_SaveFpData";
		((Control)btn_SaveFpData).Size = new Size(95, 23);
		((Control)btn_SaveFpData).TabIndex = 15;
		((Control)btn_SaveFpData).Text = "Save FP Data";
		((ButtonBase)btn_SaveFpData).UseVisualStyleBackColor = true;
		((Control)btn_SaveFpData).Click += btn_SaveFpData_Click;
		((Control)btn_OpenFpData).Location = new Point(37, 573);
		((Control)btn_OpenFpData).Name = "btn_OpenFpData";
		((Control)btn_OpenFpData).Size = new Size(95, 23);
		((Control)btn_OpenFpData).TabIndex = 14;
		((Control)btn_OpenFpData).Text = "Open FP Data";
		((ButtonBase)btn_OpenFpData).UseVisualStyleBackColor = true;
		((Control)btn_OpenFpData).Click += btn_OpenFpData_Click;
		((Control)btn_ReadFp).Location = new Point(368, 4);
		((Control)btn_ReadFp).Name = "btn_ReadFp";
		((Control)btn_ReadFp).Size = new Size(95, 23);
		((Control)btn_ReadFp).TabIndex = 16;
		((Control)btn_ReadFp).Text = "Read FP Data";
		((ButtonBase)btn_ReadFp).UseVisualStyleBackColor = true;
		((Control)btn_ReadFp).Click += btn_ReadFp_Click;
		((Control)btn_WriteFp).Location = new Point(469, 5);
		((Control)btn_WriteFp).Name = "btn_WriteFp";
		((Control)btn_WriteFp).Size = new Size(95, 23);
		((Control)btn_WriteFp).TabIndex = 17;
		((Control)btn_WriteFp).Text = "Write FP Data";
		((ButtonBase)btn_WriteFp).UseVisualStyleBackColor = true;
		((Control)btn_WriteFp).Click += btn_WriteFp_Click;
		((Control)btn_DeleteFp).Location = new Point(570, 5);
		((Control)btn_DeleteFp).Name = "btn_DeleteFp";
		((Control)btn_DeleteFp).Size = new Size(95, 23);
		((Control)btn_DeleteFp).TabIndex = 18;
		((Control)btn_DeleteFp).Text = "Clear FP Data";
		((ButtonBase)btn_DeleteFp).UseVisualStyleBackColor = true;
		((Control)btn_DeleteFp).Click += btn_DeleteFp_Click;
		((Control)btn_ReadPwd).Location = new Point(368, 33);
		((Control)btn_ReadPwd).Name = "btn_ReadPwd";
		((Control)btn_ReadPwd).Size = new Size(95, 23);
		((Control)btn_ReadPwd).TabIndex = 19;
		((Control)btn_ReadPwd).Text = "Read PWD";
		((ButtonBase)btn_ReadPwd).UseVisualStyleBackColor = true;
		((Control)btn_ReadPwd).Click += btn_ReadPwd_Click;
		((Control)btn_WritePwd).Location = new Point(469, 31);
		((Control)btn_WritePwd).Name = "btn_WritePwd";
		((Control)btn_WritePwd).Size = new Size(95, 23);
		((Control)btn_WritePwd).TabIndex = 20;
		((Control)btn_WritePwd).Text = "Write PWD";
		((ButtonBase)btn_WritePwd).UseVisualStyleBackColor = true;
		((Control)btn_WritePwd).Click += btn_WritePwd_Click;
		((Control)btn_DeletePwd).Location = new Point(570, 33);
		((Control)btn_DeletePwd).Name = "btn_DeletePwd";
		((Control)btn_DeletePwd).Size = new Size(95, 23);
		((Control)btn_DeletePwd).TabIndex = 21;
		((Control)btn_DeletePwd).Text = "Clear PWD";
		((ButtonBase)btn_DeletePwd).UseVisualStyleBackColor = true;
		((Control)btn_DeletePwd).Click += btn_DeletePwd_Click;
		((Control)btn_ReadCard).Location = new Point(368, 62);
		((Control)btn_ReadCard).Name = "btn_ReadCard";
		((Control)btn_ReadCard).Size = new Size(95, 23);
		((Control)btn_ReadCard).TabIndex = 22;
		((Control)btn_ReadCard).Text = "Read Card";
		((ButtonBase)btn_ReadCard).UseVisualStyleBackColor = true;
		((Control)btn_ReadCard).Click += btn_ReadCard_Click;
		((Control)btn_WriteCard).Location = new Point(469, 62);
		((Control)btn_WriteCard).Name = "btn_WriteCard";
		((Control)btn_WriteCard).Size = new Size(95, 23);
		((Control)btn_WriteCard).TabIndex = 23;
		((Control)btn_WriteCard).Text = "Write Card";
		((ButtonBase)btn_WriteCard).UseVisualStyleBackColor = true;
		((Control)btn_WriteCard).Click += btn_WriteCard_Click;
		((Control)btn_DeleteCard).Location = new Point(570, 62);
		((Control)btn_DeleteCard).Name = "btn_DeleteCard";
		((Control)btn_DeleteCard).Size = new Size(95, 23);
		((Control)btn_DeleteCard).TabIndex = 24;
		((Control)btn_DeleteCard).Text = "Clear Card";
		((ButtonBase)btn_DeleteCard).UseVisualStyleBackColor = true;
		((Control)btn_DeleteCard).Click += btn_DeleteCard_Click;
		((Control)btn_ReadUserName).Location = new Point(368, 91);
		((Control)btn_ReadUserName).Name = "btn_ReadUserName";
		((Control)btn_ReadUserName).Size = new Size(95, 23);
		((Control)btn_ReadUserName).TabIndex = 25;
		((Control)btn_ReadUserName).Text = "Get Username";
		((ButtonBase)btn_ReadUserName).UseVisualStyleBackColor = true;
		((Control)btn_ReadUserName).Click += btn_ReadUserName_Click;
		((Control)btn_WriteUserName).Location = new Point(469, 91);
		((Control)btn_WriteUserName).Name = "btn_WriteUserName";
		((Control)btn_WriteUserName).Size = new Size(95, 23);
		((Control)btn_WriteUserName).TabIndex = 26;
		((Control)btn_WriteUserName).Text = "Set Username";
		((ButtonBase)btn_WriteUserName).UseVisualStyleBackColor = true;
		((Control)btn_WriteUserName).Click += btn_WriteUserName_Click;
		((Control)btn_DeleteUser).Location = new Point(570, 91);
		((Control)btn_DeleteUser).Name = "btn_DeleteUser";
		((Control)btn_DeleteUser).Size = new Size(95, 23);
		((Control)btn_DeleteUser).TabIndex = 27;
		((Control)btn_DeleteUser).Text = "Clear User";
		((ButtonBase)btn_DeleteUser).UseVisualStyleBackColor = true;
		((Control)btn_DeleteUser).Click += btn_DeleteUser_Click;
		((Control)btn_DeleteAllUser).Location = new Point(671, 91);
		((Control)btn_DeleteAllUser).Name = "btn_DeleteAllUser";
		((Control)btn_DeleteAllUser).Size = new Size(99, 23);
		((Control)btn_DeleteAllUser).TabIndex = 28;
		((Control)btn_DeleteAllUser).Text = "Clear All User";
		((ButtonBase)btn_DeleteAllUser).UseVisualStyleBackColor = true;
		((Control)btn_DeleteAllUser).Click += btn_DeleteAllUser_Click;
		((Control)btn_SetRole).Location = new Point(368, 120);
		((Control)btn_SetRole).Name = "btn_SetRole";
		((Control)btn_SetRole).Size = new Size(95, 23);
		((Control)btn_SetRole).TabIndex = 29;
		((Control)btn_SetRole).Text = "Set Privilege";
		((ButtonBase)btn_SetRole).UseVisualStyleBackColor = true;
		((Control)btn_SetRole).Click += btn_SetRole_Click;
		((Control)btn_GetAllEnrollData).Location = new Point(469, 120);
		((Control)btn_GetAllEnrollData).Name = "btn_GetAllEnrollData";
		((Control)btn_GetAllEnrollData).Size = new Size(196, 23);
		((Control)btn_GetAllEnrollData).TabIndex = 30;
		((Control)btn_GetAllEnrollData).Text = "Get All Enroll Information";
		((ButtonBase)btn_GetAllEnrollData).UseVisualStyleBackColor = true;
		((Control)btn_GetAllEnrollData).Click += btn_GetAllEnrollData_Click;
		((Control)btn_GetUserInfoByUserId).Location = new Point(368, 149);
		((Control)btn_GetUserInfoByUserId).Name = "btn_GetUserInfoByUserId";
		((Control)btn_GetUserInfoByUserId).Size = new Size(297, 23);
		((Control)btn_GetUserInfoByUserId).TabIndex = 31;
		((Control)btn_GetUserInfoByUserId).Text = "Get Enroll Information By User ID";
		((ButtonBase)btn_GetUserInfoByUserId).UseVisualStyleBackColor = true;
		((Control)btn_GetUserInfoByUserId).Click += btn_GetUserInfoByUserId_Click;
		((Control)btn_UploadUserData).Location = new Point(368, 178);
		((Control)btn_UploadUserData).Name = "btn_UploadUserData";
		((Control)btn_UploadUserData).Size = new Size(145, 23);
		((Control)btn_UploadUserData).TabIndex = 32;
		((Control)btn_UploadUserData).Text = "Set All Enroll Data";
		((ButtonBase)btn_UploadUserData).UseVisualStyleBackColor = true;
		((Control)btn_UploadUserData).Click += btn_UploadUserData_Click;
		((Control)btn_DownloadUserData).Location = new Point(519, 178);
		((Control)btn_DownloadUserData).Name = "btn_DownloadUserData";
		((Control)btn_DownloadUserData).Size = new Size(145, 23);
		((Control)btn_DownloadUserData).TabIndex = 33;
		((Control)btn_DownloadUserData).Text = "Get All Enroll Data";
		((ButtonBase)btn_DownloadUserData).UseVisualStyleBackColor = true;
		((Control)btn_DownloadUserData).Click += btn_DownloadUserData_Click;
		lvw_UserList.CheckBoxes = true;
		lvw_UserList.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[16]
		{
			columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7, columnHeader8, columnHeader9, columnHeader10,
			columnHeader11, columnHeader12, columnHeader13, columnHeader14, columnHeader15, columnHeader16
		});
		lvw_UserList.FullRowSelect = true;
		lvw_UserList.GridLines = true;
		lvw_UserList.HideSelection = false;
		((Control)lvw_UserList).Location = new Point(239, 209);
		((Control)lvw_UserList).Name = "lvw_UserList";
		((Control)lvw_UserList).Size = new Size(571, 358);
		((Control)lvw_UserList).TabIndex = 35;
		lvw_UserList.UseCompatibleStateImageBehavior = false;
		lvw_UserList.View = (View)1;
		columnHeader1.Text = "SN";
		columnHeader2.Text = "User ID";
		columnHeader2.TextAlign = (HorizontalAlignment)2;
		columnHeader2.Width = 120;
		columnHeader3.Text = "Username";
		columnHeader3.TextAlign = (HorizontalAlignment)2;
		columnHeader3.Width = 120;
		columnHeader4.Text = "FP 0";
		columnHeader4.TextAlign = (HorizontalAlignment)2;
		columnHeader5.Text = "FP 1";
		columnHeader5.TextAlign = (HorizontalAlignment)2;
		columnHeader6.Text = "FP 2";
		columnHeader6.TextAlign = (HorizontalAlignment)2;
		columnHeader7.Text = "FP 3";
		columnHeader7.TextAlign = (HorizontalAlignment)2;
		columnHeader8.Text = "FP 4";
		columnHeader8.TextAlign = (HorizontalAlignment)2;
		columnHeader9.Text = "FP 5";
		columnHeader9.TextAlign = (HorizontalAlignment)2;
		columnHeader10.Text = "FP 6";
		columnHeader10.TextAlign = (HorizontalAlignment)2;
		columnHeader11.Text = "FP 7";
		columnHeader11.TextAlign = (HorizontalAlignment)2;
		columnHeader12.Text = "FP 8";
		columnHeader12.TextAlign = (HorizontalAlignment)2;
		columnHeader13.Text = "FP 9";
		columnHeader13.TextAlign = (HorizontalAlignment)2;
		columnHeader14.Text = "PWD";
		columnHeader14.TextAlign = (HorizontalAlignment)2;
		columnHeader15.Text = "Card";
		columnHeader15.TextAlign = (HorizontalAlignment)2;
		columnHeader16.Text = "Privilege";
		columnHeader16.TextAlign = (HorizontalAlignment)2;
		columnHeader16.Width = 120;
		((Control)btn_Select).Location = new Point(261, 576);
		((Control)btn_Select).Name = "btn_Select";
		((Control)btn_Select).Size = new Size(105, 23);
		((Control)btn_Select).TabIndex = 36;
		((Control)btn_Select).Text = "Select All";
		((ButtonBase)btn_Select).UseVisualStyleBackColor = true;
		((Control)btn_Select).Click += btn_Select_Click;
		((Control)btn_Open).Location = new Point(483, 576);
		((Control)btn_Open).Name = "btn_Open";
		((Control)btn_Open).Size = new Size(105, 23);
		((Control)btn_Open).TabIndex = 37;
		((Control)btn_Open).Text = "Open All Enroll";
		((ButtonBase)btn_Open).UseVisualStyleBackColor = true;
		((Control)btn_Open).Click += btn_Open_Click;
		((Control)btn_Save).Location = new Point(372, 576);
		((Control)btn_Save).Name = "btn_Save";
		((Control)btn_Save).Size = new Size(105, 23);
		((Control)btn_Save).TabIndex = 38;
		((Control)btn_Save).Text = "Save All Enroll";
		((ButtonBase)btn_Save).UseVisualStyleBackColor = true;
		((Control)btn_Save).Click += btn_Save_Click;
		((FileDialog)sfd_SaveFpData).Filter = "Template File|*.fpt";
		((FileDialog)ofd_OpenFpData).Filter = "Template File|*.fpt";
		((Control)ExtInfoLabel).AutoSize = true;
		((Control)ExtInfoLabel).Location = new Point(24, 167);
		((Control)ExtInfoLabel).Name = "ExtInfoLabel";
		((Control)ExtInfoLabel).Size = new Size(53, 12);
		((Control)ExtInfoLabel).TabIndex = 10;
		((Control)ExtInfoLabel).Text = "ExtInfo:";
		ExtInfoLabel.TextAlign = (ContentAlignment)32;
		((Control)ExtInfoTextBox).Location = new Point(83, 164);
		((Control)ExtInfoTextBox).Name = "ExtInfoTextBox";
		((Control)ExtInfoTextBox).Size = new Size(150, 21);
		((Control)ExtInfoTextBox).TabIndex = 11;
		((Control)getExtInfoButton).Location = new Point(240, 163);
		((Control)getExtInfoButton).Name = "getExtInfoButton";
		((Control)getExtInfoButton).Size = new Size(50, 21);
		((Control)getExtInfoButton).TabIndex = 39;
		((Control)getExtInfoButton).Text = "Get";
		((ButtonBase)getExtInfoButton).UseVisualStyleBackColor = true;
		((Control)getExtInfoButton).Click += getExtInfoButton_Click;
		((Control)setExtInfoButton).Location = new Point(296, 163);
		((Control)setExtInfoButton).Name = "setExtInfoButton";
		((Control)setExtInfoButton).Size = new Size(50, 21);
		((Control)setExtInfoButton).TabIndex = 40;
		((Control)setExtInfoButton).Text = "Set";
		((ButtonBase)setExtInfoButton).UseVisualStyleBackColor = true;
		((Control)setExtInfoButton).Click += setExtInfoButton_Click;
		((Control)nud_DIN).Location = new Point(83, 7);
		nud_DIN.Maximum = new decimal(new int[4] { -1486618625, 232830643, 0, 0 });
		nud_DIN.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)nud_DIN).Name = "nud_DIN";
		((Control)nud_DIN).Size = new Size(150, 21);
		((Control)nud_DIN).TabIndex = 1;
		nud_DIN.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)btn_SaveOneEnroll).Location = new Point(594, 576);
		((Control)btn_SaveOneEnroll).Name = "btn_SaveOneEnroll";
		((Control)btn_SaveOneEnroll).Size = new Size(105, 23);
		((Control)btn_SaveOneEnroll).TabIndex = 41;
		((Control)btn_SaveOneEnroll).Text = "Save One Enroll";
		((ButtonBase)btn_SaveOneEnroll).UseVisualStyleBackColor = true;
		((Control)btn_SaveOneEnroll).Click += btn_SaveOneEnroll_Click;
		((Control)btn_OpenOneEnroll).Location = new Point(705, 576);
		((Control)btn_OpenOneEnroll).Name = "btn_OpenOneEnroll";
		((Control)btn_OpenOneEnroll).Size = new Size(105, 23);
		((Control)btn_OpenOneEnroll).TabIndex = 42;
		((Control)btn_OpenOneEnroll).Text = "Open One Enroll";
		((ButtonBase)btn_OpenOneEnroll).UseVisualStyleBackColor = true;
		((Control)btn_OpenOneEnroll).Click += btn_OpenOneEnroll_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(822, 611);
		((Control)this).Controls.Add((Control)(object)btn_OpenOneEnroll);
		((Control)this).Controls.Add((Control)(object)btn_SaveOneEnroll);
		((Control)this).Controls.Add((Control)(object)nud_DIN);
		((Control)this).Controls.Add((Control)(object)setExtInfoButton);
		((Control)this).Controls.Add((Control)(object)getExtInfoButton);
		((Control)this).Controls.Add((Control)(object)btn_Save);
		((Control)this).Controls.Add((Control)(object)btn_Open);
		((Control)this).Controls.Add((Control)(object)btn_Select);
		((Control)this).Controls.Add((Control)(object)lvw_UserList);
		((Control)this).Controls.Add((Control)(object)btn_DeleteFp);
		((Control)this).Controls.Add((Control)(object)btn_WriteFp);
		((Control)this).Controls.Add((Control)(object)btn_DeletePwd);
		((Control)this).Controls.Add((Control)(object)btn_WritePwd);
		((Control)this).Controls.Add((Control)(object)btn_DeleteCard);
		((Control)this).Controls.Add((Control)(object)btn_WriteCard);
		((Control)this).Controls.Add((Control)(object)btn_DeleteAllUser);
		((Control)this).Controls.Add((Control)(object)btn_DeleteUser);
		((Control)this).Controls.Add((Control)(object)btn_WriteUserName);
		((Control)this).Controls.Add((Control)(object)btn_DownloadUserData);
		((Control)this).Controls.Add((Control)(object)btn_UploadUserData);
		((Control)this).Controls.Add((Control)(object)btn_GetUserInfoByUserId);
		((Control)this).Controls.Add((Control)(object)btn_GetAllEnrollData);
		((Control)this).Controls.Add((Control)(object)btn_SetRole);
		((Control)this).Controls.Add((Control)(object)btn_ReadUserName);
		((Control)this).Controls.Add((Control)(object)btn_ReadCard);
		((Control)this).Controls.Add((Control)(object)btn_ReadPwd);
		((Control)this).Controls.Add((Control)(object)btn_ReadFp);
		((Control)this).Controls.Add((Control)(object)btn_OpenFpData);
		((Control)this).Controls.Add((Control)(object)btn_SaveFpData);
		((Control)this).Controls.Add((Control)(object)cbo_Role);
		((Control)this).Controls.Add((Control)(object)cbo_FpNo);
		((Control)this).Controls.Add((Control)(object)txt_UserFpData);
		((Control)this).Controls.Add((Control)(object)ExtInfoTextBox);
		((Control)this).Controls.Add((Control)(object)txt_UserName);
		((Control)this).Controls.Add((Control)(object)txt_Card);
		((Control)this).Controls.Add((Control)(object)txt_Pwd);
		((Control)this).Controls.Add((Control)(object)ExtInfoLabel);
		((Control)this).Controls.Add((Control)(object)lbl_FpData);
		((Control)this).Controls.Add((Control)(object)lbl_UserName);
		((Control)this).Controls.Add((Control)(object)lbl_Card);
		((Control)this).Controls.Add((Control)(object)lbl_Pwd);
		((Control)this).Controls.Add((Control)(object)lbl_Role);
		((Control)this).Controls.Add((Control)(object)lbl_FpNo);
		((Control)this).Controls.Add((Control)(object)lbl_UserId);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "EnrollForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Enroll Data Management";
		((ISupportInitialize)nud_DIN).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
