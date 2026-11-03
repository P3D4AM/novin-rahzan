using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmEnroll : Form
{
	private AxSBXPC bpc;

	private int DATASIZE = 354;

	private int DATASIZE_FACE = 6917;

	private int NAMESIZE = 54;

	private int[] gTemplngEnrollData;

	private byte[] gbytEnrollData;

	private byte[] gbytEnrollData1;

	private int[] gTempEnrollName;

	private int glngEnrollPData;

	private ASCIIEncoding ascii;

	public DataSet dsEnrolls;

	private IContainer components = null;

	public TextBox txtCardNumber;

	public Button cmdDeleteCompany;

	public Button cmdSetCompany;

	public Button cmdGetName;

	public ToolTip ToolTip1;

	public Button cmdSetName;

	public Button cmdModifyPrivilege;

	public Button cmdEnableUser;

	public Button cmdSetAllEnrollData;

	public Button cmdGetAllEnrollData;

	public Button cmdGetEnrollData;

	public Button cmdClearData;

	public Button cmdGetEnrollInfo;

	public Button cmdDeleteEnrollData;

	public Button cmdSetEnrollData;

	public Button cmdDel;

	public Button cmdExit;

	public Button cmdEmptyEnrollData;

	public TextBox txtName;

	public CheckBox chkDisable;

	public ComboBox cmbPrivilege;

	public ListBox lstEnrollData;

	public TextBox txtEnrollNumber;

	public ComboBox cmbBackupNumber;

	public Label lblCardNum;

	public Label lbName;

	public Label Label2;

	public Label Label1;

	public Label lblMessage;

	public Label lblEnrollData;

	public Label lblBackupNumber;

	public Label _lblEnrollNum_0;

	public Button cmdModifyDuressFP;

	public Button cmdSetUserPhoto;

	public Button cmdGetUserPhoto;

	public Button cmdDeleteUserPhoto;

	public Label label4;

	public ComboBox cmbDuressSetting;

	public Label label5;

	public Label label6;

	public Label label7;

	public Label label8;

	public Label label9;

	public TextBox txtUserTz2;

	public TextBox txtUserTz1;

	public TextBox txtUserTz4;

	public TextBox txtUserTz3;

	public TextBox txtUserTz5;

	public TextBox txtDepart;

	public Label label10;

	private OpenFileDialog OpenFileDlg;

	private GroupBox groupBox1;

	private PictureBox picUserPhoto;

	private TextBox txtUserPhotoFile;

	private Button cmdUserPhotoBrowse;

	private CheckBox chkDupCheck;

	private GroupBox groupBox2;

	public frmEnroll(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void frmEnroll_Load(object sender, EventArgs e)
	{
		picUserPhoto.Image = null;
		gbytEnrollData = new byte[DATASIZE_FACE * 5];
		gbytEnrollData1 = new byte[DATASIZE_FACE * 5];
		gTemplngEnrollData = new int[DATASIZE_FACE];
		gTempEnrollName = new int[NAMESIZE];
		ascii = new ASCIIEncoding();
		EnrollDatatbl enrollDatatbl = new EnrollDatatbl();
		enrollDatatbl.New("./");
		dsEnrolls = EnrollDatatbl.DataModule.GetEnrollDatasSql();
	}

	private void cmdGetEnrollData_Click(object sender, EventArgs e)
	{
		int dwMachinePrivilege = 0;
		int dwErrorCode = 0;
		((Control)lblEnrollData).Text = "Enrolled Data";
		lstEnrollData.Items.Clear();
		((Control)Label2).Text = "";
		lstEnrollData.Items.Clear();
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int num = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		if (num == 10)
		{
			num = 15;
		}
		int gMachineNumber = util.gMachineNumber;
		GCHandle gCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
		IntPtr intPtr = gCHandle.AddrOfPinnedObject();
		glngEnrollPData = 0;
		int dwEnrollData = intPtr.ToInt32();
		bool enrollData = bpc.GetEnrollData1(util.gMachineNumber, dwEnrollNumber, num, ref dwMachinePrivilege, ref dwEnrollData, ref glngEnrollPData);
		gCHandle.Free();
		if (enrollData)
		{
			((ListControl)cmbPrivilege).SelectedIndex = dwMachinePrivilege;
			((Control)lblMessage).Text = "GetEnrollData OK";
			switch (num)
			{
			case 11:
				((Control)txtCardNumber).Text = Convert.ToString(glngEnrollPData, 16).ToUpper();
				break;
			case 14:
				((Control)txtUserTz1).Text = Convert.ToString(glngEnrollPData % 64);
				glngEnrollPData /= 64;
				((Control)txtUserTz2).Text = Convert.ToString(glngEnrollPData % 64);
				glngEnrollPData /= 64;
				((Control)txtUserTz3).Text = Convert.ToString(glngEnrollPData % 64);
				glngEnrollPData /= 64;
				((Control)txtUserTz4).Text = Convert.ToString(glngEnrollPData % 64);
				glngEnrollPData /= 64;
				((Control)txtUserTz5).Text = Convert.ToString(glngEnrollPData % 64);
				glngEnrollPData /= 64;
				break;
			case 15:
				((Control)txtCardNumber).Text = "";
				while (glngEnrollPData > 0)
				{
					int i = glngEnrollPData % 16 - 1;
					((Control)txtCardNumber).Text = ((Control)txtCardNumber).Text + Convert.ToString(i);
					glngEnrollPData /= 16;
				}
				break;
			case 16:
				((Control)txtDepart).Text = Convert.ToString(glngEnrollPData);
				break;
			case 17:
			{
				for (int i = 0; i < DATASIZE_FACE; i++)
				{
					lstEnrollData.Items.Add((object)Convert.ToString(gTemplngEnrollData[i]));
				}
				break;
			}
			default:
			{
				for (int i = 0; i < DATASIZE; i++)
				{
					lstEnrollData.Items.Add((object)Convert.ToString(gTemplngEnrollData[i]));
				}
				break;
			}
			}
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetEnrollData_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (((Control)txtEnrollNumber).Text == "")
		{
			((Control)txtEnrollNumber).Text = "0";
		}
		if (((Control)txtCardNumber).Text == "")
		{
			((Control)txtCardNumber).Text = "0";
		}
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int num = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		if (num == 10)
		{
			num = 15;
		}
		int dwMachinePrivilege = Convert.ToInt32(((Control)cmbPrivilege).Text);
		int gMachineNumber = util.gMachineNumber;
		switch (num)
		{
		case 11:
			glngEnrollPData = Convert.ToInt32(((Control)txtCardNumber).Text, 16);
			break;
		case 14:
			glngEnrollPData = Convert.ToInt32(((Control)txtUserTz5).Text);
			glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(((Control)txtUserTz4).Text);
			glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(((Control)txtUserTz3).Text);
			glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(((Control)txtUserTz2).Text);
			glngEnrollPData = glngEnrollPData * 64 + Convert.ToInt32(((Control)txtUserTz1).Text);
			break;
		case 15:
		{
			int num2 = ((Control)txtCardNumber).Text.Length;
			if (num2 > 6)
			{
				num2 = 6;
			}
			glngEnrollPData = 0;
			while (num2 > 0)
			{
				glngEnrollPData = glngEnrollPData * 16 + Convert.ToInt16(((Control)txtCardNumber).Text.Substring(num2 - 1, 1)) + 1;
				num2--;
			}
			break;
		}
		case 16:
			glngEnrollPData = Convert.ToInt32(((Control)txtDepart).Text);
			break;
		}
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		GCHandle gCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
		int dwEnrollData = gCHandle.AddrOfPinnedObject().ToInt32();
		bool flag = bpc.SetEnrollData1(util.gMachineNumber, dwEnrollNumber, num, dwMachinePrivilege, ref dwEnrollData, glngEnrollPData);
		gCHandle.Free();
		if (flag)
		{
			((Control)lblMessage).Text = "SetEnrollData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdDeleteEnrollData_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int gMachineNumber = util.gMachineNumber;
		int dwBackupNumber = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		if (bpc.DeleteEnrollData(util.gMachineNumber, dwEnrollNumber, gMachineNumber, dwBackupNumber))
		{
			((Control)lblMessage).Text = "DeleteEnrollData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	public async void cmdGetAllEnrollData_Click(object sender, EventArgs e)
	{
		int vEnrollNumber = 0;
		int vEMachineNumber = 0;
		int vFingerNumber = 0;
		int vPrivilege = 0;
		int vEnable = 0;
		int vErrorCode = 0;
		string vStr = "";
		string CompanyName = "Ebio";
		lstEnrollData.Items.Clear();
		_ = ((Control)this).Text;
		((Control)Label2).Text = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
		}
		else if (bpc.ReadAllUserID(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "ReadAllUserID OK";
			((Control)this).Cursor = Cursors.WaitCursor;
			bool vFlag = false;
			DataTable dbEnrollTble = dsEnrolls.Tables[0];
			dbEnrollTble.Rows.Clear();
			while (bpc.GetAllUserID(util.gMachineNumber, ref vEnrollNumber, ref vEMachineNumber, ref vFingerNumber, ref vPrivilege, ref vEnable))
			{
				vFlag = true;
				while (true)
				{
					int nAddr = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
					if (vFingerNumber >= 50)
					{
						break;
					}
					if (!bpc.GetEnrollData1(util.gMachineNumber, vEnrollNumber, vFingerNumber, ref vPrivilege, ref nAddr, ref glngEnrollPData))
					{
						vFlag = false;
						vStr = "GetEnrollData";
						bpc.GetLastError(ref vErrorCode);
						int vMsgRet = util.MessageBox(new IntPtr(0), util.ErrorPrint(vErrorCode) + ": Continue ?", "GetEnrollData", 4u);
						if (vMsgRet == 6)
						{
							continue;
						}
						if (vMsgRet == 7)
						{
							((Control)this).Cursor = Cursors.Default;
							bpc.EnableDevice(util.gMachineNumber, 1);
							return;
						}
					}
					DataRow dbRow = dbEnrollTble.NewRow();
					dbRow["EMachineNumber"] = vEMachineNumber;
					dbRow["EnrollNumber"] = vEnrollNumber;
					dbRow["FingerNumber"] = vFingerNumber;
					dbRow["Privilige"] = vPrivilege;
					switch (vFingerNumber)
					{
					case 10:
						dbRow["Password1"] = glngEnrollPData;
						break;
					case 15:
						dbRow["Password1"] = glngEnrollPData;
						break;
					case 11:
						dbRow["Password1"] = glngEnrollPData;
						break;
					case 17:
					{
						dbRow["Password1"] = 0;
						for (int l = 0; l < DATASIZE_FACE; l++)
						{
							gbytEnrollData[l * 5] = 1;
							if (gTemplngEnrollData[l] < 0)
							{
								gbytEnrollData[l * 5] = 0;
								gTemplngEnrollData[l] = Math.Abs(gTemplngEnrollData[l]);
							}
							gbytEnrollData[l * 5 + 1] = (byte)(gTemplngEnrollData[l] / 256 / 256 / 256);
							gbytEnrollData[l * 5 + 2] = (byte)(gTemplngEnrollData[l] / 256 / 256 % 256);
							gbytEnrollData[l * 5 + 3] = (byte)(gTemplngEnrollData[l] / 256 % 256);
							gbytEnrollData[l * 5 + 4] = (byte)(gTemplngEnrollData[l] % 256);
						}
						byte[] gbyt = new byte[DATASIZE_FACE * 5];
						for (int l = 0; l < DATASIZE_FACE * 5; l++)
						{
							gbyt[l] = gbytEnrollData[l];
						}
						dbRow["FPdata"] = gbyt;
						break;
					}
					default:
					{
						dbRow["Password1"] = 0;
						for (int l = 0; l < DATASIZE; l++)
						{
							gbytEnrollData[l * 5] = 1;
							if (gTemplngEnrollData[l] < 0)
							{
								gbytEnrollData[l * 5] = 0;
								gTemplngEnrollData[l] = Math.Abs(gTemplngEnrollData[l]);
							}
							gbytEnrollData[l * 5 + 1] = (byte)(gTemplngEnrollData[l] / 256 / 256 / 256);
							gbytEnrollData[l * 5 + 2] = (byte)(gTemplngEnrollData[l] / 256 / 256 % 256);
							gbytEnrollData[l * 5 + 3] = (byte)(gTemplngEnrollData[l] / 256 % 256);
							gbytEnrollData[l * 5 + 4] = (byte)(gTemplngEnrollData[l] % 256);
						}
						byte[] gbyt2 = new byte[DATASIZE * 5];
						for (int l = 0; l < DATASIZE * 5; l++)
						{
							gbyt2[l] = gbytEnrollData[l];
						}
						dbRow["FPdata"] = gbyt2;
						break;
					}
					}
					dbRow["CompanyName"] = CompanyName;
					dbEnrollTble.Rows.Add(dbRow);
					((Control)lblMessage).Text = $"{vEMachineNumber:D2}" + "-" + $"{vEnrollNumber:D2}" + "-" + vFingerNumber;
					((Control)this).Text = $"{vEnrollNumber:D2}";
					((Control)txtEnrollNumber).Text = Convert.ToString(vEnrollNumber);
					((Control)cmbBackupNumber).Text = Convert.ToString(vFingerNumber);
					((Control)cmbPrivilege).Text = Convert.ToString(vPrivilege);
					Application.DoEvents();
					break;
				}
			}
			((Control)Label2).Text = "Total : " + dsEnrolls.Tables["tblEnroll"].Rows.Count;
			dsEnrolls.GetChanges();
			await EnrollDatatbl.DataModule.SaveEnrollsAsync(dbEnrollTble);
			try
			{
				await EnrollDatatbl.DataModule.SaveEnrollsSqlAsync(dbEnrollTble);
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				MessageBox.Show(ex.Message);
			}
			_ = ((Control)this).Text;
			((Control)this).Cursor = Cursors.Default;
			if (vFlag)
			{
				((Control)lblMessage).Text = "GetAllUserID OK";
			}
			else
			{
				((Control)lblMessage).Text = vStr + ":" + util.ErrorPrint(vErrorCode);
			}
			Application.DoEvents();
			bpc.EnableDevice(util.gMachineNumber, 1);
		}
		else
		{
			bpc.GetLastError(ref vErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(vErrorCode);
			bpc.EnableDevice(util.gMachineNumber, 1);
		}
	}

	private void cmdSetAllEnrollData_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		lstEnrollData.Items.Clear();
		string text = ((Control)this).Text;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		((Control)this).Cursor = Cursors.WaitCursor;
		DataTable dataTable = dsEnrolls.Tables[0];
		int num = 0;
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				int num2 = (int)row["EMachineNumber"];
				int num3 = (int)row["EnrollNumber"];
				int num4 = (int)row["FingerNumber"];
				int dwMachinePrivilege = (int)row["Privilige"];
				glngEnrollPData = (int)row["Password1"];
				num++;
				if (num4 == 17)
				{
					byte[] array = (byte[])row["FPData"];
					for (int i = 0; i < DATASIZE_FACE; i++)
					{
						gTemplngEnrollData[i] = array[i * 5 + 1];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 2];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 3];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 4];
						if (array[i * 5] == 0)
						{
							gTemplngEnrollData[i] = -gTemplngEnrollData[i];
						}
					}
				}
				else if (num4 < 10)
				{
					byte[] array = (byte[])row["FPData"];
					for (int i = 0; i < DATASIZE; i++)
					{
						gTemplngEnrollData[i] = array[i * 5 + 1];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 2];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 3];
						gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 4];
						if (array[i * 5] == 0)
						{
							gTemplngEnrollData[i] = -gTemplngEnrollData[i];
						}
					}
				}
				while (true)
				{
					GCHandle gCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
					int dwEnrollData = gCHandle.AddrOfPinnedObject().ToInt32();
					int num5 = num4;
					if (num5 >= 0 && num5 <= 9 && !chkDupCheck.Checked)
					{
						num5 += 20;
					}
					bool flag = bpc.SetEnrollData1(util.gMachineNumber, num3, num5, dwMachinePrivilege, ref dwEnrollData, glngEnrollPData);
					gCHandle.Free();
					if (!flag)
					{
						bpc.GetLastError(ref dwErrorCode);
						switch (util.MessageBox(new IntPtr(0), util.ErrorPrint(dwErrorCode) + ": Continue ?", "SetEnrollData", 4u))
						{
						case 6:
							continue;
						case 7:
							goto end_IL_02f6;
						}
					}
					((Control)lblMessage).Text = "EMachine = " + Convert.ToString(num2) + ", ID = " + $"{num3:D5}" + ", FpNo = " + num4 + ", Count = " + num;
					((Control)this).Text = Convert.ToString(num);
					Application.DoEvents();
					goto IL_0444;
					continue;
					end_IL_02f6:
					break;
				}
				break;
				IL_0444:;
			}
		}
		text = ((Control)this).Text;
		((Control)this).Cursor = Cursors.Default;
		((Control)lblMessage).Text = "SetAllUserData OK";
		Application.DoEvents();
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetName_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string lpszUserName = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwEnrollNumber = 0;
		if (((Control)txtEnrollNumber).Text != "")
		{
			dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		}
		int gMachineNumber = util.gMachineNumber;
		if (bpc.GetUserName1(util.gMachineNumber, dwEnrollNumber, ref lpszUserName))
		{
			((Control)txtName).Text = lpszUserName;
			((Control)lblMessage).Text = "GetUserName OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetName_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string text = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int gMachineNumber = util.gMachineNumber;
		text = ((Control)txtName).Text;
		if (bpc.SetUserName1(util.gMachineNumber, dwEnrollNumber, ref text))
		{
			((Control)lblMessage).Text = "SetUserName OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetCompany_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string text = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int gMachineNumber = util.gMachineNumber;
		text = ((Control)txtName).Text;
		if (bpc.SetCompanyName1(util.gMachineNumber, 1, ref text))
		{
			((Control)lblMessage).Text = "Set Company Name OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdDeleteCompany_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string dwCompanyName = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int gMachineNumber = util.gMachineNumber;
		if (bpc.SetCompanyName1(util.gMachineNumber, 0, ref dwCompanyName))
		{
			((Control)lblMessage).Text = "Delete Company Name OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetEnrollInfo_Click(object sender, EventArgs e)
	{
		int dwEMachineNumber = 0;
		int dwEnrollNumber = 0;
		int dwBackupNumber = 0;
		int dwMachinePrivilege = 0;
		int dwEnable = 0;
		int dwErrorCode = 0;
		((Control)lblEnrollData).Text = "User IDs";
		lstEnrollData.Items.Clear();
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
		}
		else if (bpc.ReadAllUserID(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "ReadAllUserID OK";
			bool flag = false;
			int num = 0;
			lstEnrollData.Items.Add((object)"No.  EnNo   EMNo   Fp   Priv  Enable Duress");
			while (bpc.GetAllUserID(util.gMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwBackupNumber, ref dwMachinePrivilege, ref dwEnable))
			{
				flag = true;
				lstEnrollData.Items.Add((object)($"{num:D5}" + "    " + $"{dwEnrollNumber:D5}" + "    " + $"{dwEMachineNumber:D3}" + "    " + $"{dwBackupNumber:D2}" + "    " + Convert.ToString(dwMachinePrivilege) + "    " + Convert.ToString(dwEnable % 256) + "     " + Convert.ToString(dwEnable / 256)));
				num++;
				((Control)Label2).Text = "Total : " + num;
			}
			if (flag)
			{
				((Control)lblMessage).Text = "GetAllUserID OK";
			}
			else
			{
				((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			}
			bpc.EnableDevice(util.gMachineNumber, 1);
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			bpc.EnableDevice(util.gMachineNumber, 1);
		}
	}

	private void cmdEnableUser_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int gMachineNumber = util.gMachineNumber;
		int dwEnrollNumber = Convert.ToInt32((((Control)txtEnrollNumber).Text == "") ? "0" : ((Control)txtEnrollNumber).Text);
		int dwBackupNumber = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		int bFlag = ((!chkDisable.Checked) ? 1 : 0);
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.EnableUser(util.gMachineNumber, dwEnrollNumber, gMachineNumber, dwBackupNumber, bFlag))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdModifyPrivilege_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		int gMachineNumber = util.gMachineNumber;
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int dwBackupNumber = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		int dwMachinePrivilege = Convert.ToInt32(((Control)cmbPrivilege).Text);
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.ModifyPrivilege(util.gMachineNumber, dwEnrollNumber, gMachineNumber, dwBackupNumber, dwMachinePrivilege))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdEmptyEnrollData_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.EmptyEnrollData(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "Success!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdClearData_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.ClearKeeperData(util.gMachineNumber))
		{
			((Control)lblMessage).Text = "ClearKeeperData OK!";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void frmEnroll_FormClosed(object sender, FormClosedEventArgs e)
	{
		ClearUserPhoto();
	}

	private void cmdDel_Click(object sender, EventArgs e)
	{
		EnrollDatatbl.DataModule.DeleteDB();
		((Control)Label2).Text = "Total : 0";
		((Control)lblMessage).Text = "Deleted PC Database";
	}

	private void cmdModifyDuressFP_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwEnrollNumber = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		int dwBackupNumber = Convert.ToInt32(((Control)cmbBackupNumber).Text);
		int dwDuressSetting = Convert.ToInt32(((Control)cmbDuressSetting).Text);
		if (bpc.ModifyDuressFP(util.gMachineNumber, dwEnrollNumber, dwBackupNumber, dwDuressSetting))
		{
			((Control)lblMessage).Text = "ModifyDuressFP OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdGetUserPhoto_Click(object sender, EventArgs e)
	{
		string strXML = "";
		int dwErrorCode = 0;
		((Control)txtUserPhotoFile).Text = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		int dwValue = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetUserPhotoData");
		bpc.XML_AddLong(ref strXML, "UserID", dwValue);
		bool flag;
		if (!bpc.GeneralOperationXML(ref strXML))
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			ClearUserPhoto();
		}
		else
		{
			((Control)lblMessage).Text = "GetUserPhotoData OK";
			byte[] array = new byte[util.gCompressPhotoSize];
			int pData = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
			flag = bpc.XML_ParseBinaryLong(ref strXML, "PhotoData", ref pData, util.gCompressPhotoSize);
			ClearUserPhoto();
			if (!flag)
			{
				((Control)lblMessage).Text = "GetGlogPhotoData - XML Parse Error";
			}
			else
			{
				if (File.Exists(util.gTempPhotoFile))
				{
					File.Delete(util.gTempPhotoFile);
				}
				FileStream fileStream = File.Create(util.gTempPhotoFile);
				fileStream.Write(array, 0, util.gCompressPhotoSize);
				fileStream.Close();
				fileStream.Dispose();
				fileStream = null;
				picUserPhoto.Image = Image.FromFile(util.gTempPhotoFile);
				((Control)txtUserPhotoFile).Text = util.gTempPhotoFile;
			}
		}
		flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdSetUserPhoto_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		string text = ((Control)txtUserPhotoFile).Text;
		if (!File.Exists(text))
		{
			((Control)lblMessage).Text = "Can not find the photo file.";
			return;
		}
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		ClearUserPhoto();
		FileStream fileStream = File.Open(text, FileMode.Open, FileAccess.Read, FileShare.None);
		if (fileStream.Length != util.gCompressPhotoSize)
		{
			((Control)lblMessage).Text = "Photo file size is not" + Convert.ToString(util.gCompressPhotoSize) + "Byte";
			return;
		}
		byte[] array = new byte[util.gCompressPhotoSize];
		fileStream.Read(array, 0, util.gCompressPhotoSize);
		fileStream.Close();
		fileStream.Dispose();
		fileStream = null;
		picUserPhoto.Image = Image.FromFile(text);
		int dwData = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
		int dwValue = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserPhotoData");
		bpc.XML_AddLong(ref strXML, "UserID", dwValue);
		bpc.XML_AddBinaryLong(ref strXML, "PhotoData", ref dwData, util.gCompressPhotoSize);
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "SetUserPhotoData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
	}

	private void cmdUserPhotoBrowse_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((CommonDialog)OpenFileDlg).ShowDialog();
		((Control)txtUserPhotoFile).Text = ((FileDialog)OpenFileDlg).FileName;
		ClearUserPhoto();
		if (File.Exists(((Control)txtUserPhotoFile).Text))
		{
			picUserPhoto.Image = Image.FromFile(((Control)txtUserPhotoFile).Text);
		}
	}

	private void ClearUserPhoto()
	{
		if (picUserPhoto.Image != null)
		{
			picUserPhoto.Image.Dispose();
		}
		picUserPhoto.Image = null;
	}

	private void cmdDeleteUserPhoto_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		string strXML = "";
		if (!bpc.EnableDevice(util.gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		string text = ((Control)txtUserPhotoFile).Text;
		if (!File.Exists(text))
		{
			((Control)lblMessage).Text = "Can not find the photo file.";
			return;
		}
		int dwValue = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		util.MakeXMLRequestHeader(bpc, ref strXML, "SetUserPhotoData");
		bpc.XML_AddLong(ref strXML, "UserID", dwValue);
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "DeleteUserPhotoData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bool flag = bpc.EnableDevice(util.gMachineNumber, 1);
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
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Expected O, but got Unknown
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Expected O, but got Unknown
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Expected O, but got Unknown
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f25: Expected O, but got Unknown
		//IL_1015: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Expected O, but got Unknown
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1119: Expected O, but got Unknown
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Expected O, but got Unknown
		//IL_12e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f2: Expected O, but got Unknown
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ad: Expected O, but got Unknown
		//IL_146b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1475: Expected O, but got Unknown
		//IL_1607: Unknown result type (might be due to invalid IL or missing references)
		//IL_1611: Expected O, but got Unknown
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d9: Expected O, but got Unknown
		//IL_17ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1809: Expected O, but got Unknown
		//IL_18c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ce: Expected O, but got Unknown
		//IL_198c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1996: Expected O, but got Unknown
		//IL_1a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5e: Expected O, but got Unknown
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b26: Expected O, but got Unknown
		//IL_1bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfb: Expected O, but got Unknown
		//IL_1cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Expected O, but got Unknown
		//IL_1d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8b: Expected O, but got Unknown
		//IL_1e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e43: Expected O, but got Unknown
		//IL_1f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f29: Expected O, but got Unknown
		//IL_2005: Unknown result type (might be due to invalid IL or missing references)
		//IL_200f: Expected O, but got Unknown
		//IL_20eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f5: Expected O, but got Unknown
		//IL_21de: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e8: Expected O, but got Unknown
		//IL_2299: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a3: Expected O, but got Unknown
		//IL_238b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2395: Expected O, but got Unknown
		//IL_2453: Unknown result type (might be due to invalid IL or missing references)
		//IL_245d: Expected O, but got Unknown
		//IL_251b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2525: Expected O, but got Unknown
		//IL_25e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ed: Expected O, but got Unknown
		//IL_26ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b5: Expected O, but got Unknown
		//IL_2773: Unknown result type (might be due to invalid IL or missing references)
		//IL_277d: Expected O, but got Unknown
		//IL_283a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2844: Expected O, but got Unknown
		//IL_2901: Unknown result type (might be due to invalid IL or missing references)
		//IL_290b: Expected O, but got Unknown
		//IL_29c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d2: Expected O, but got Unknown
		//IL_2a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a99: Expected O, but got Unknown
		//IL_2b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b60: Expected O, but got Unknown
		//IL_2c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c27: Expected O, but got Unknown
		//IL_2d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d13: Expected O, but got Unknown
		//IL_33d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e3: Expected O, but got Unknown
		components = new Container();
		txtCardNumber = new TextBox();
		cmdDeleteCompany = new Button();
		cmdSetCompany = new Button();
		cmdGetName = new Button();
		cmdSetName = new Button();
		cmdModifyPrivilege = new Button();
		cmdEnableUser = new Button();
		cmdSetAllEnrollData = new Button();
		cmdGetAllEnrollData = new Button();
		cmdGetEnrollData = new Button();
		cmdClearData = new Button();
		ToolTip1 = new ToolTip(components);
		cmdGetEnrollInfo = new Button();
		cmdDeleteEnrollData = new Button();
		cmdSetEnrollData = new Button();
		cmdDel = new Button();
		cmdExit = new Button();
		cmdEmptyEnrollData = new Button();
		txtName = new TextBox();
		chkDisable = new CheckBox();
		cmbPrivilege = new ComboBox();
		lstEnrollData = new ListBox();
		txtEnrollNumber = new TextBox();
		cmbBackupNumber = new ComboBox();
		lblCardNum = new Label();
		lbName = new Label();
		Label2 = new Label();
		Label1 = new Label();
		lblMessage = new Label();
		lblEnrollData = new Label();
		lblBackupNumber = new Label();
		_lblEnrollNum_0 = new Label();
		cmdModifyDuressFP = new Button();
		cmdSetUserPhoto = new Button();
		cmdGetUserPhoto = new Button();
		cmdDeleteUserPhoto = new Button();
		label4 = new Label();
		cmbDuressSetting = new ComboBox();
		label5 = new Label();
		label6 = new Label();
		label7 = new Label();
		label8 = new Label();
		label9 = new Label();
		txtUserTz2 = new TextBox();
		txtUserTz1 = new TextBox();
		txtUserTz4 = new TextBox();
		txtUserTz3 = new TextBox();
		txtUserTz5 = new TextBox();
		txtDepart = new TextBox();
		label10 = new Label();
		OpenFileDlg = new OpenFileDialog();
		groupBox1 = new GroupBox();
		picUserPhoto = new PictureBox();
		txtUserPhotoFile = new TextBox();
		cmdUserPhotoBrowse = new Button();
		chkDupCheck = new CheckBox();
		groupBox2 = new GroupBox();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)picUserPhoto).BeginInit();
		((Control)groupBox2).SuspendLayout();
		((Control)this).SuspendLayout();
		txtCardNumber.AcceptsReturn = true;
		((Control)txtCardNumber).BackColor = SystemColors.Window;
		((Control)txtCardNumber).Cursor = Cursors.IBeam;
		((Control)txtCardNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtCardNumber).ForeColor = SystemColors.WindowText;
		((Control)txtCardNumber).Location = new Point(150, 101);
		((TextBoxBase)txtCardNumber).MaxLength = 0;
		((Control)txtCardNumber).Name = "txtCardNumber";
		((Control)txtCardNumber).RightToLeft = (RightToLeft)0;
		((Control)txtCardNumber).Size = new Size(81, 26);
		((Control)txtCardNumber).TabIndex = 71;
		((Control)cmdDeleteCompany).BackColor = SystemColors.Control;
		((Control)cmdDeleteCompany).Cursor = Cursors.Default;
		((Control)cmdDeleteCompany).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDeleteCompany).ForeColor = SystemColors.ControlText;
		((Control)cmdDeleteCompany).Location = new Point(780, 218);
		((Control)cmdDeleteCompany).Name = "cmdDeleteCompany";
		((Control)cmdDeleteCompany).RightToLeft = (RightToLeft)0;
		((Control)cmdDeleteCompany).Size = new Size(201, 30);
		((Control)cmdDeleteCompany).TabIndex = 69;
		((Control)cmdDeleteCompany).Text = "Delete Company Name";
		((ButtonBase)cmdDeleteCompany).UseVisualStyleBackColor = false;
		((Control)cmdDeleteCompany).Click += cmdDeleteCompany_Click;
		((Control)cmdSetCompany).BackColor = SystemColors.Control;
		((Control)cmdSetCompany).Cursor = Cursors.Default;
		((Control)cmdSetCompany).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetCompany).ForeColor = SystemColors.ControlText;
		((Control)cmdSetCompany).Location = new Point(573, 218);
		((Control)cmdSetCompany).Name = "cmdSetCompany";
		((Control)cmdSetCompany).RightToLeft = (RightToLeft)0;
		((Control)cmdSetCompany).Size = new Size(201, 30);
		((Control)cmdSetCompany).TabIndex = 68;
		((Control)cmdSetCompany).Text = "Set Company Name";
		((ButtonBase)cmdSetCompany).UseVisualStyleBackColor = false;
		((Control)cmdSetCompany).Click += cmdSetCompany_Click;
		((Control)cmdGetName).BackColor = SystemColors.Control;
		((Control)cmdGetName).Cursor = Cursors.Default;
		((Control)cmdGetName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetName).ForeColor = SystemColors.ControlText;
		((Control)cmdGetName).Location = new Point(572, 182);
		((Control)cmdGetName).Name = "cmdGetName";
		((Control)cmdGetName).RightToLeft = (RightToLeft)0;
		((Control)cmdGetName).Size = new Size(201, 30);
		((Control)cmdGetName).TabIndex = 66;
		((Control)cmdGetName).Text = "Get Name Data";
		ToolTip1.SetToolTip((Control)(object)cmdGetName, "Get All Enroll Data From Device And Save To DataBase");
		((ButtonBase)cmdGetName).UseVisualStyleBackColor = false;
		((Control)cmdGetName).Click += cmdGetName_Click;
		((Control)cmdSetName).BackColor = SystemColors.Control;
		((Control)cmdSetName).Cursor = Cursors.Default;
		((Control)cmdSetName).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetName).ForeColor = SystemColors.ControlText;
		((Control)cmdSetName).Location = new Point(779, 182);
		((Control)cmdSetName).Name = "cmdSetName";
		((Control)cmdSetName).RightToLeft = (RightToLeft)0;
		((Control)cmdSetName).Size = new Size(201, 30);
		((Control)cmdSetName).TabIndex = 65;
		((Control)cmdSetName).Text = "Set Name Data";
		ToolTip1.SetToolTip((Control)(object)cmdSetName, "Load All Enroll Data From DataBase And Set To Device");
		((ButtonBase)cmdSetName).UseVisualStyleBackColor = false;
		((Control)cmdSetName).Click += cmdSetName_Click;
		((Control)cmdModifyPrivilege).BackColor = SystemColors.Control;
		((Control)cmdModifyPrivilege).Cursor = Cursors.Default;
		((Control)cmdModifyPrivilege).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdModifyPrivilege).ForeColor = SystemColors.ControlText;
		((Control)cmdModifyPrivilege).Location = new Point(574, 290);
		((Control)cmdModifyPrivilege).Name = "cmdModifyPrivilege";
		((Control)cmdModifyPrivilege).RightToLeft = (RightToLeft)0;
		((Control)cmdModifyPrivilege).Size = new Size(200, 30);
		((Control)cmdModifyPrivilege).TabIndex = 64;
		((Control)cmdModifyPrivilege).Text = "ModifyPrivilege";
		((ButtonBase)cmdModifyPrivilege).UseVisualStyleBackColor = false;
		((Control)cmdModifyPrivilege).Click += cmdModifyPrivilege_Click;
		((Control)cmdEnableUser).BackColor = SystemColors.Control;
		((Control)cmdEnableUser).Cursor = Cursors.Default;
		((Control)cmdEnableUser).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEnableUser).ForeColor = SystemColors.ControlText;
		((Control)cmdEnableUser).Location = new Point(781, 254);
		((Control)cmdEnableUser).Name = "cmdEnableUser";
		((Control)cmdEnableUser).RightToLeft = (RightToLeft)0;
		((Control)cmdEnableUser).Size = new Size(200, 30);
		((Control)cmdEnableUser).TabIndex = 63;
		((Control)cmdEnableUser).Text = "EnableUser";
		((ButtonBase)cmdEnableUser).UseVisualStyleBackColor = false;
		((Control)cmdEnableUser).Click += cmdEnableUser_Click;
		((Control)cmdSetAllEnrollData).BackColor = SystemColors.Control;
		((Control)cmdSetAllEnrollData).Cursor = Cursors.Default;
		((Control)cmdSetAllEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetAllEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdSetAllEnrollData).Location = new Point(4, 7);
		((Control)cmdSetAllEnrollData).Name = "cmdSetAllEnrollData";
		((Control)cmdSetAllEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdSetAllEnrollData).Size = new Size(200, 30);
		((Control)cmdSetAllEnrollData).TabIndex = 62;
		((Control)cmdSetAllEnrollData).Text = "Set All Enroll Data";
		ToolTip1.SetToolTip((Control)(object)cmdSetAllEnrollData, "Load All Enroll Data From DataBase And Set To Device");
		((ButtonBase)cmdSetAllEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdSetAllEnrollData).Click += cmdSetAllEnrollData_Click;
		((Control)cmdGetAllEnrollData).BackColor = SystemColors.Control;
		((Control)cmdGetAllEnrollData).Cursor = Cursors.Default;
		((Control)cmdGetAllEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetAllEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdGetAllEnrollData).Location = new Point(573, 129);
		((Control)cmdGetAllEnrollData).Name = "cmdGetAllEnrollData";
		((Control)cmdGetAllEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdGetAllEnrollData).Size = new Size(200, 30);
		((Control)cmdGetAllEnrollData).TabIndex = 61;
		((Control)cmdGetAllEnrollData).Text = "Get All Enroll Data";
		ToolTip1.SetToolTip((Control)(object)cmdGetAllEnrollData, "Get All Enroll Data From Device And Save To DataBase");
		((ButtonBase)cmdGetAllEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdGetAllEnrollData).Click += cmdGetAllEnrollData_Click;
		((Control)cmdGetEnrollData).BackColor = SystemColors.Control;
		((Control)cmdGetEnrollData).Cursor = Cursors.Default;
		((Control)cmdGetEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdGetEnrollData).Location = new Point(573, 60);
		((Control)cmdGetEnrollData).Name = "cmdGetEnrollData";
		((Control)cmdGetEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdGetEnrollData).Size = new Size(200, 30);
		((Control)cmdGetEnrollData).TabIndex = 60;
		((Control)cmdGetEnrollData).Text = "Get Enroll Data";
		ToolTip1.SetToolTip((Control)(object)cmdGetEnrollData, "Get EnrollData From Device");
		((ButtonBase)cmdGetEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdGetEnrollData).Click += cmdGetEnrollData_Click;
		((Control)cmdClearData).BackColor = SystemColors.Control;
		((Control)cmdClearData).Cursor = Cursors.Default;
		((Control)cmdClearData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdClearData).ForeColor = SystemColors.ControlText;
		((Control)cmdClearData).Location = new Point(778, 396);
		((Control)cmdClearData).Name = "cmdClearData";
		((Control)cmdClearData).RightToLeft = (RightToLeft)0;
		((Control)cmdClearData).Size = new Size(200, 30);
		((Control)cmdClearData).TabIndex = 59;
		((Control)cmdClearData).Text = "Clear All Data(E,GL,SL) ";
		ToolTip1.SetToolTip((Control)(object)cmdClearData, "Clear EnrollData and LogDat Into Device");
		((ButtonBase)cmdClearData).UseVisualStyleBackColor = false;
		((Control)cmdClearData).Click += cmdClearData_Click;
		((Control)cmdGetEnrollInfo).BackColor = SystemColors.Control;
		((Control)cmdGetEnrollInfo).Cursor = Cursors.Default;
		((Control)cmdGetEnrollInfo).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetEnrollInfo).ForeColor = SystemColors.ControlText;
		((Control)cmdGetEnrollInfo).Location = new Point(573, 254);
		((Control)cmdGetEnrollInfo).Name = "cmdGetEnrollInfo";
		((Control)cmdGetEnrollInfo).RightToLeft = (RightToLeft)0;
		((Control)cmdGetEnrollInfo).Size = new Size(200, 30);
		((Control)cmdGetEnrollInfo).TabIndex = 57;
		((Control)cmdGetEnrollInfo).Text = "Get Enroll Info";
		ToolTip1.SetToolTip((Control)(object)cmdGetEnrollInfo, "Get All Enrolled User Info From Device");
		((ButtonBase)cmdGetEnrollInfo).UseVisualStyleBackColor = false;
		((Control)cmdGetEnrollInfo).Click += cmdGetEnrollInfo_Click;
		((Control)cmdDeleteEnrollData).BackColor = SystemColors.Control;
		((Control)cmdDeleteEnrollData).Cursor = Cursors.Default;
		((Control)cmdDeleteEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDeleteEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdDeleteEnrollData).Location = new Point(779, 95);
		((Control)cmdDeleteEnrollData).Name = "cmdDeleteEnrollData";
		((Control)cmdDeleteEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdDeleteEnrollData).Size = new Size(200, 30);
		((Control)cmdDeleteEnrollData).TabIndex = 56;
		((Control)cmdDeleteEnrollData).Text = "Delete Enroll Data";
		ToolTip1.SetToolTip((Control)(object)cmdDeleteEnrollData, "Delete Enroll Data Into Device");
		((ButtonBase)cmdDeleteEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdDeleteEnrollData).Click += cmdDeleteEnrollData_Click;
		((Control)cmdSetEnrollData).BackColor = SystemColors.Control;
		((Control)cmdSetEnrollData).Cursor = Cursors.Default;
		((Control)cmdSetEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdSetEnrollData).Location = new Point(779, 60);
		((Control)cmdSetEnrollData).Name = "cmdSetEnrollData";
		((Control)cmdSetEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdSetEnrollData).Size = new Size(200, 30);
		((Control)cmdSetEnrollData).TabIndex = 55;
		((Control)cmdSetEnrollData).Text = "Set Enroll Data";
		ToolTip1.SetToolTip((Control)(object)cmdSetEnrollData, "Set EnrollData To Device");
		((ButtonBase)cmdSetEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdSetEnrollData).Click += cmdSetEnrollData_Click;
		((Control)cmdDel).BackColor = SystemColors.Control;
		((Control)cmdDel).Cursor = Cursors.Default;
		((Control)cmdDel).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDel).ForeColor = SystemColors.ControlText;
		((Control)cmdDel).Location = new Point(429, 474);
		((Control)cmdDel).Name = "cmdDel";
		((Control)cmdDel).RightToLeft = (RightToLeft)0;
		((Control)cmdDel).Size = new Size(100, 30);
		((Control)cmdDel).TabIndex = 47;
		((Control)cmdDel).Text = "Delete DB";
		ToolTip1.SetToolTip((Control)(object)cmdDel, "Delete All Saved Data From DataBase");
		((ButtonBase)cmdDel).UseVisualStyleBackColor = false;
		((Control)cmdDel).Click += cmdDel_Click;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(678, 432);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(200, 30);
		((Control)cmdExit).TabIndex = 58;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdEmptyEnrollData).BackColor = SystemColors.Control;
		((Control)cmdEmptyEnrollData).Cursor = Cursors.Default;
		((Control)cmdEmptyEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptyEnrollData).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptyEnrollData).Location = new Point(572, 396);
		((Control)cmdEmptyEnrollData).Name = "cmdEmptyEnrollData";
		((Control)cmdEmptyEnrollData).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptyEnrollData).Size = new Size(200, 30);
		((Control)cmdEmptyEnrollData).TabIndex = 54;
		((Control)cmdEmptyEnrollData).Text = "Empty Enroll Data";
		((ButtonBase)cmdEmptyEnrollData).UseVisualStyleBackColor = false;
		((Control)cmdEmptyEnrollData).Click += cmdEmptyEnrollData_Click;
		txtName.AcceptsReturn = true;
		((Control)txtName).BackColor = SystemColors.Window;
		((Control)txtName).Cursor = Cursors.IBeam;
		((Control)txtName).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtName).ForeColor = SystemColors.WindowText;
		((Control)txtName).Location = new Point(323, 66);
		((TextBoxBase)txtName).MaxLength = 24;
		((Control)txtName).Name = "txtName";
		((Control)txtName).RightToLeft = (RightToLeft)0;
		((Control)txtName).Size = new Size(154, 26);
		((Control)txtName).TabIndex = 52;
		((Control)chkDisable).BackColor = SystemColors.Control;
		((Control)chkDisable).Cursor = Cursors.Default;
		((Control)chkDisable).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)chkDisable).ForeColor = SystemColors.ControlText;
		((Control)chkDisable).Location = new Point(267, 104);
		((Control)chkDisable).Name = "chkDisable";
		((Control)chkDisable).RightToLeft = (RightToLeft)0;
		((Control)chkDisable).Size = new Size(112, 19);
		((Control)chkDisable).TabIndex = 49;
		((Control)chkDisable).Text = "Disable User";
		((ButtonBase)chkDisable).UseVisualStyleBackColor = false;
		((Control)cmbPrivilege).BackColor = SystemColors.Window;
		((Control)cmbPrivilege).Cursor = Cursors.Default;
		((Control)cmbPrivilege).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbPrivilege).ForeColor = SystemColors.WindowText;
		cmbPrivilege.Items.AddRange(new object[3] { "0", "1", "2" });
		((Control)cmbPrivilege).Location = new Point(150, 205);
		((Control)cmbPrivilege).Name = "cmbPrivilege";
		((Control)cmbPrivilege).RightToLeft = (RightToLeft)0;
		((Control)cmbPrivilege).Size = new Size(81, 27);
		((Control)cmbPrivilege).TabIndex = 43;
		((Control)cmbPrivilege).Text = "0";
		((Control)lstEnrollData).BackColor = SystemColors.Window;
		((Control)lstEnrollData).Cursor = Cursors.Default;
		((Control)lstEnrollData).ForeColor = SystemColors.WindowText;
		lstEnrollData.HorizontalScrollbar = true;
		((Control)lstEnrollData).Location = new Point(267, 149);
		((Control)lstEnrollData).Name = "lstEnrollData";
		((Control)lstEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lstEnrollData).Size = new Size(220, 134);
		((Control)lstEnrollData).TabIndex = 40;
		txtEnrollNumber.AcceptsReturn = true;
		((Control)txtEnrollNumber).BackColor = SystemColors.Window;
		((Control)txtEnrollNumber).Cursor = Cursors.IBeam;
		((Control)txtEnrollNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEnrollNumber).ForeColor = SystemColors.WindowText;
		((Control)txtEnrollNumber).Location = new Point(150, 64);
		((TextBoxBase)txtEnrollNumber).MaxLength = 8;
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).RightToLeft = (RightToLeft)0;
		((Control)txtEnrollNumber).Size = new Size(81, 26);
		((Control)txtEnrollNumber).TabIndex = 38;
		((Control)txtEnrollNumber).Text = "1";
		((Control)cmbBackupNumber).BackColor = SystemColors.Window;
		((Control)cmbBackupNumber).Cursor = Cursors.Default;
		((Control)cmbBackupNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbBackupNumber).ForeColor = SystemColors.WindowText;
		cmbBackupNumber.Items.AddRange(new object[11]
		{
			"0", "1", "2", "10", "11", "12", "13", "14", "15", "16",
			"17"
		});
		((Control)cmbBackupNumber).Location = new Point(150, 172);
		((Control)cmbBackupNumber).Name = "cmbBackupNumber";
		((Control)cmbBackupNumber).RightToLeft = (RightToLeft)0;
		((Control)cmbBackupNumber).Size = new Size(81, 27);
		((Control)cmbBackupNumber).TabIndex = 36;
		((Control)cmbBackupNumber).Text = "0";
		((Control)lblCardNum).BackColor = SystemColors.Control;
		((Control)lblCardNum).Cursor = Cursors.Default;
		((Control)lblCardNum).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblCardNum).ForeColor = SystemColors.ControlText;
		((Control)lblCardNum).Location = new Point(22, 105);
		((Control)lblCardNum).Name = "lblCardNum";
		((Control)lblCardNum).RightToLeft = (RightToLeft)0;
		((Control)lblCardNum).Size = new Size(122, 39);
		((Control)lblCardNum).TabIndex = 70;
		((Control)lblCardNum).Text = "Card or Password Number :";
		((Control)lbName).AutoSize = true;
		((Control)lbName).BackColor = SystemColors.Control;
		((Control)lbName).Cursor = Cursors.Default;
		((Control)lbName).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lbName).ForeColor = SystemColors.ControlText;
		((Control)lbName).Location = new Point(275, 71);
		((Control)lbName).Name = "lbName";
		((Control)lbName).RightToLeft = (RightToLeft)0;
		((Control)lbName).Size = new Size(53, 19);
		((Control)lbName).TabIndex = 53;
		((Control)lbName).Text = "Name :";
		((Control)Label2).AutoSize = true;
		((Control)Label2).BackColor = SystemColors.Control;
		((Control)Label2).Cursor = Cursors.Default;
		((Control)Label2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label2).ForeColor = SystemColors.ControlText;
		((Control)Label2).Location = new Point(388, 125);
		((Control)Label2).Name = "Label2";
		((Control)Label2).RightToLeft = (RightToLeft)0;
		((Control)Label2).Size = new Size(50, 19);
		((Control)Label2).TabIndex = 48;
		((Control)Label2).Text = "Total : ";
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(22, 206);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(67, 19);
		((Control)Label1).TabIndex = 44;
		((Control)Label1).Text = "Privilege :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(14, 17);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(528, 29);
		((Control)lblMessage).TabIndex = 42;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(268, 124);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(99, 19);
		((Control)lblEnrollData).TabIndex = 41;
		((Control)lblEnrollData).Text = "Enrolled Data :";
		((Control)lblBackupNumber).AutoSize = true;
		((Control)lblBackupNumber).BackColor = SystemColors.Control;
		((Control)lblBackupNumber).Cursor = Cursors.Default;
		((Control)lblBackupNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblBackupNumber).ForeColor = SystemColors.ControlText;
		((Control)lblBackupNumber).Location = new Point(22, 176);
		((Control)lblBackupNumber).Name = "lblBackupNumber";
		((Control)lblBackupNumber).RightToLeft = (RightToLeft)0;
		((Control)lblBackupNumber).Size = new Size(117, 19);
		((Control)lblBackupNumber).TabIndex = 39;
		((Control)lblBackupNumber).Text = "Backup Number :";
		((Control)_lblEnrollNum_0).AutoSize = true;
		((Control)_lblEnrollNum_0).BackColor = SystemColors.Control;
		((Control)_lblEnrollNum_0).Cursor = Cursors.Default;
		((Control)_lblEnrollNum_0).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)_lblEnrollNum_0).ForeColor = SystemColors.ControlText;
		((Control)_lblEnrollNum_0).Location = new Point(22, 68);
		((Control)_lblEnrollNum_0).Name = "_lblEnrollNum_0";
		((Control)_lblEnrollNum_0).RightToLeft = (RightToLeft)0;
		((Control)_lblEnrollNum_0).Size = new Size(105, 19);
		((Control)_lblEnrollNum_0).TabIndex = 37;
		((Control)_lblEnrollNum_0).Text = "Enroll Number :";
		((Control)cmdModifyDuressFP).BackColor = SystemColors.Control;
		((Control)cmdModifyDuressFP).Cursor = Cursors.Default;
		((Control)cmdModifyDuressFP).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdModifyDuressFP).ForeColor = SystemColors.ControlText;
		((Control)cmdModifyDuressFP).Location = new Point(780, 289);
		((Control)cmdModifyDuressFP).Name = "cmdModifyDuressFP";
		((Control)cmdModifyDuressFP).RightToLeft = (RightToLeft)0;
		((Control)cmdModifyDuressFP).Size = new Size(200, 30);
		((Control)cmdModifyDuressFP).TabIndex = 72;
		((Control)cmdModifyDuressFP).Text = "Modify Duress FP";
		((ButtonBase)cmdModifyDuressFP).UseVisualStyleBackColor = false;
		((Control)cmdModifyDuressFP).Click += cmdModifyDuressFP_Click;
		((Control)cmdSetUserPhoto).BackColor = SystemColors.Control;
		((Control)cmdSetUserPhoto).Cursor = Cursors.Default;
		((Control)cmdSetUserPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetUserPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdSetUserPhoto).Location = new Point(779, 325);
		((Control)cmdSetUserPhoto).Name = "cmdSetUserPhoto";
		((Control)cmdSetUserPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdSetUserPhoto).Size = new Size(200, 30);
		((Control)cmdSetUserPhoto).TabIndex = 74;
		((Control)cmdSetUserPhoto).Text = "Set User Photo";
		((ButtonBase)cmdSetUserPhoto).UseVisualStyleBackColor = false;
		((Control)cmdSetUserPhoto).Click += cmdSetUserPhoto_Click;
		((Control)cmdGetUserPhoto).BackColor = SystemColors.Control;
		((Control)cmdGetUserPhoto).Cursor = Cursors.Default;
		((Control)cmdGetUserPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetUserPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdGetUserPhoto).Location = new Point(573, 326);
		((Control)cmdGetUserPhoto).Name = "cmdGetUserPhoto";
		((Control)cmdGetUserPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdGetUserPhoto).Size = new Size(200, 30);
		((Control)cmdGetUserPhoto).TabIndex = 73;
		((Control)cmdGetUserPhoto).Text = "Get User Photo";
		((ButtonBase)cmdGetUserPhoto).UseVisualStyleBackColor = false;
		((Control)cmdGetUserPhoto).Click += cmdGetUserPhoto_Click;
		((Control)cmdDeleteUserPhoto).BackColor = SystemColors.Control;
		((Control)cmdDeleteUserPhoto).Cursor = Cursors.Default;
		((Control)cmdDeleteUserPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDeleteUserPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdDeleteUserPhoto).Location = new Point(779, 361);
		((Control)cmdDeleteUserPhoto).Name = "cmdDeleteUserPhoto";
		((Control)cmdDeleteUserPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdDeleteUserPhoto).Size = new Size(200, 30);
		((Control)cmdDeleteUserPhoto).TabIndex = 75;
		((Control)cmdDeleteUserPhoto).Text = "Delete User Photo";
		((ButtonBase)cmdDeleteUserPhoto).UseVisualStyleBackColor = false;
		((Control)cmdDeleteUserPhoto).Click += cmdDeleteUserPhoto_Click;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = SystemColors.Control;
		((Control)label4).Cursor = Cursors.Default;
		((Control)label4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(22, 243);
		((Control)label4).Name = "label4";
		((Control)label4).RightToLeft = (RightToLeft)0;
		((Control)label4).Size = new Size(54, 19);
		((Control)label4).TabIndex = 76;
		((Control)label4).Text = "Duress:";
		((Control)cmbDuressSetting).BackColor = SystemColors.Window;
		((Control)cmbDuressSetting).Cursor = Cursors.Default;
		((Control)cmbDuressSetting).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmbDuressSetting).ForeColor = SystemColors.WindowText;
		cmbDuressSetting.Items.AddRange(new object[2] { "0", "1" });
		((Control)cmbDuressSetting).Location = new Point(150, 238);
		((Control)cmbDuressSetting).Name = "cmbDuressSetting";
		((Control)cmbDuressSetting).RightToLeft = (RightToLeft)0;
		((Control)cmbDuressSetting).Size = new Size(81, 27);
		((Control)cmbDuressSetting).TabIndex = 77;
		((Control)cmbDuressSetting).Text = "0";
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = SystemColors.Control;
		((Control)label5).Cursor = Cursors.Default;
		((Control)label5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label5).ForeColor = SystemColors.ControlText;
		((Control)label5).Location = new Point(22, 318);
		((Control)label5).Name = "label5";
		((Control)label5).RightToLeft = (RightToLeft)0;
		((Control)label5).Size = new Size(71, 19);
		((Control)label5).TabIndex = 78;
		((Control)label5).Text = "User TZ1:";
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = SystemColors.Control;
		((Control)label6).Cursor = Cursors.Default;
		((Control)label6).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label6).ForeColor = SystemColors.ControlText;
		((Control)label6).Location = new Point(22, 349);
		((Control)label6).Name = "label6";
		((Control)label6).RightToLeft = (RightToLeft)0;
		((Control)label6).Size = new Size(71, 19);
		((Control)label6).TabIndex = 79;
		((Control)label6).Text = "User TZ2:";
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = SystemColors.Control;
		((Control)label7).Cursor = Cursors.Default;
		((Control)label7).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label7).ForeColor = SystemColors.ControlText;
		((Control)label7).Location = new Point(22, 379);
		((Control)label7).Name = "label7";
		((Control)label7).RightToLeft = (RightToLeft)0;
		((Control)label7).Size = new Size(71, 19);
		((Control)label7).TabIndex = 80;
		((Control)label7).Text = "User TZ3:";
		((Control)label8).AutoSize = true;
		((Control)label8).BackColor = SystemColors.Control;
		((Control)label8).Cursor = Cursors.Default;
		((Control)label8).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label8).ForeColor = SystemColors.ControlText;
		((Control)label8).Location = new Point(22, 406);
		((Control)label8).Name = "label8";
		((Control)label8).RightToLeft = (RightToLeft)0;
		((Control)label8).Size = new Size(71, 19);
		((Control)label8).TabIndex = 81;
		((Control)label8).Text = "User TZ4:";
		((Control)label9).AutoSize = true;
		((Control)label9).BackColor = SystemColors.Control;
		((Control)label9).Cursor = Cursors.Default;
		((Control)label9).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label9).ForeColor = SystemColors.ControlText;
		((Control)label9).Location = new Point(22, 432);
		((Control)label9).Name = "label9";
		((Control)label9).RightToLeft = (RightToLeft)0;
		((Control)label9).Size = new Size(71, 19);
		((Control)label9).TabIndex = 82;
		((Control)label9).Text = "User TZ5:";
		txtUserTz2.AcceptsReturn = true;
		((Control)txtUserTz2).BackColor = SystemColors.Window;
		((Control)txtUserTz2).Cursor = Cursors.IBeam;
		((Control)txtUserTz2).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserTz2).ForeColor = SystemColors.WindowText;
		((Control)txtUserTz2).Location = new Point(150, 343);
		((TextBoxBase)txtUserTz2).MaxLength = 0;
		((Control)txtUserTz2).Name = "txtUserTz2";
		((Control)txtUserTz2).RightToLeft = (RightToLeft)0;
		((Control)txtUserTz2).Size = new Size(81, 26);
		((Control)txtUserTz2).TabIndex = 84;
		txtUserTz1.AcceptsReturn = true;
		((Control)txtUserTz1).BackColor = SystemColors.Window;
		((Control)txtUserTz1).Cursor = Cursors.IBeam;
		((Control)txtUserTz1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserTz1).ForeColor = SystemColors.WindowText;
		((Control)txtUserTz1).Location = new Point(150, 315);
		((TextBoxBase)txtUserTz1).MaxLength = 8;
		((Control)txtUserTz1).Name = "txtUserTz1";
		((Control)txtUserTz1).RightToLeft = (RightToLeft)0;
		((Control)txtUserTz1).Size = new Size(81, 26);
		((Control)txtUserTz1).TabIndex = 83;
		txtUserTz4.AcceptsReturn = true;
		((Control)txtUserTz4).BackColor = SystemColors.Window;
		((Control)txtUserTz4).Cursor = Cursors.IBeam;
		((Control)txtUserTz4).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserTz4).ForeColor = SystemColors.WindowText;
		((Control)txtUserTz4).Location = new Point(150, 399);
		((TextBoxBase)txtUserTz4).MaxLength = 0;
		((Control)txtUserTz4).Name = "txtUserTz4";
		((Control)txtUserTz4).RightToLeft = (RightToLeft)0;
		((Control)txtUserTz4).Size = new Size(81, 26);
		((Control)txtUserTz4).TabIndex = 86;
		txtUserTz3.AcceptsReturn = true;
		((Control)txtUserTz3).BackColor = SystemColors.Window;
		((Control)txtUserTz3).Cursor = Cursors.IBeam;
		((Control)txtUserTz3).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserTz3).ForeColor = SystemColors.WindowText;
		((Control)txtUserTz3).Location = new Point(150, 371);
		((TextBoxBase)txtUserTz3).MaxLength = 8;
		((Control)txtUserTz3).Name = "txtUserTz3";
		((Control)txtUserTz3).RightToLeft = (RightToLeft)0;
		((Control)txtUserTz3).Size = new Size(81, 26);
		((Control)txtUserTz3).TabIndex = 85;
		txtUserTz5.AcceptsReturn = true;
		((Control)txtUserTz5).BackColor = SystemColors.Window;
		((Control)txtUserTz5).Cursor = Cursors.IBeam;
		((Control)txtUserTz5).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtUserTz5).ForeColor = SystemColors.WindowText;
		((Control)txtUserTz5).Location = new Point(150, 427);
		((TextBoxBase)txtUserTz5).MaxLength = 0;
		((Control)txtUserTz5).Name = "txtUserTz5";
		((Control)txtUserTz5).RightToLeft = (RightToLeft)0;
		((Control)txtUserTz5).Size = new Size(81, 26);
		((Control)txtUserTz5).TabIndex = 87;
		txtDepart.AcceptsReturn = true;
		((Control)txtDepart).BackColor = SystemColors.Window;
		((Control)txtDepart).Cursor = Cursors.IBeam;
		((Control)txtDepart).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtDepart).ForeColor = SystemColors.WindowText;
		((Control)txtDepart).Location = new Point(150, 470);
		((TextBoxBase)txtDepart).MaxLength = 0;
		((Control)txtDepart).Name = "txtDepart";
		((Control)txtDepart).RightToLeft = (RightToLeft)0;
		((Control)txtDepart).Size = new Size(81, 26);
		((Control)txtDepart).TabIndex = 89;
		((Control)label10).AutoSize = true;
		((Control)label10).BackColor = SystemColors.Control;
		((Control)label10).Cursor = Cursors.Default;
		((Control)label10).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label10).ForeColor = SystemColors.ControlText;
		((Control)label10).Location = new Point(22, 475);
		((Control)label10).Name = "label10";
		((Control)label10).RightToLeft = (RightToLeft)0;
		((Control)label10).Size = new Size(84, 19);
		((Control)label10).TabIndex = 88;
		((Control)label10).Text = "User Depart";
		((FileDialog)OpenFileDlg).FileName = "openFileDialog1";
		((Control)groupBox1).Controls.Add((Control)(object)picUserPhoto);
		((Control)groupBox1).Controls.Add((Control)(object)txtUserPhotoFile);
		((Control)groupBox1).Controls.Add((Control)(object)cmdUserPhotoBrowse);
		((Control)groupBox1).Font = new Font("Microsoft Sans Serif", 10f);
		((Control)groupBox1).ForeColor = Color.Black;
		((Control)groupBox1).Location = new Point(267, 293);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(267, 160);
		((Control)groupBox1).TabIndex = 90;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "User Photo Related";
		picUserPhoto.BorderStyle = (BorderStyle)2;
		((Control)picUserPhoto).Location = new Point(17, 52);
		((Control)picUserPhoto).Name = "picUserPhoto";
		((Control)picUserPhoto).Size = new Size(117, 99);
		picUserPhoto.SizeMode = (PictureBoxSizeMode)1;
		picUserPhoto.TabIndex = 2;
		picUserPhoto.TabStop = false;
		((Control)txtUserPhotoFile).Location = new Point(12, 22);
		((Control)txtUserPhotoFile).Name = "txtUserPhotoFile";
		((Control)txtUserPhotoFile).Size = new Size(216, 23);
		((Control)txtUserPhotoFile).TabIndex = 1;
		((Control)cmdUserPhotoBrowse).Location = new Point(228, 21);
		((Control)cmdUserPhotoBrowse).Name = "cmdUserPhotoBrowse";
		((Control)cmdUserPhotoBrowse).Size = new Size(27, 25);
		((Control)cmdUserPhotoBrowse).TabIndex = 0;
		((Control)cmdUserPhotoBrowse).Text = "...";
		((ButtonBase)cmdUserPhotoBrowse).UseVisualStyleBackColor = true;
		((Control)cmdUserPhotoBrowse).Click += cmdUserPhotoBrowse_Click;
		((Control)chkDupCheck).AutoSize = true;
		chkDupCheck.Checked = true;
		chkDupCheck.CheckState = (CheckState)1;
		((Control)chkDupCheck).Location = new Point(24, 36);
		((Control)chkDupCheck).Name = "chkDupCheck";
		((Control)chkDupCheck).Size = new Size(165, 17);
		((Control)chkDupCheck).TabIndex = 91;
		((Control)chkDupCheck).Text = "Fingerprint Duplication Check";
		((ButtonBase)chkDupCheck).UseVisualStyleBackColor = true;
		((Control)groupBox2).Controls.Add((Control)(object)chkDupCheck);
		((Control)groupBox2).Controls.Add((Control)(object)cmdSetAllEnrollData);
		((Control)groupBox2).Location = new Point(775, 124);
		((Control)groupBox2).Name = "groupBox2";
		((Control)groupBox2).Size = new Size(210, 57);
		((Control)groupBox2).TabIndex = 92;
		groupBox2.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(1011, 522);
		((Control)this).Controls.Add((Control)(object)groupBox2);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)txtDepart);
		((Control)this).Controls.Add((Control)(object)label10);
		((Control)this).Controls.Add((Control)(object)txtUserTz5);
		((Control)this).Controls.Add((Control)(object)txtUserTz4);
		((Control)this).Controls.Add((Control)(object)txtUserTz3);
		((Control)this).Controls.Add((Control)(object)txtUserTz2);
		((Control)this).Controls.Add((Control)(object)txtUserTz1);
		((Control)this).Controls.Add((Control)(object)label9);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)cmbDuressSetting);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)cmdDeleteUserPhoto);
		((Control)this).Controls.Add((Control)(object)cmdSetUserPhoto);
		((Control)this).Controls.Add((Control)(object)cmdGetUserPhoto);
		((Control)this).Controls.Add((Control)(object)cmdModifyDuressFP);
		((Control)this).Controls.Add((Control)(object)txtCardNumber);
		((Control)this).Controls.Add((Control)(object)cmdDeleteCompany);
		((Control)this).Controls.Add((Control)(object)cmdSetCompany);
		((Control)this).Controls.Add((Control)(object)cmdGetName);
		((Control)this).Controls.Add((Control)(object)cmdSetName);
		((Control)this).Controls.Add((Control)(object)cmdModifyPrivilege);
		((Control)this).Controls.Add((Control)(object)cmdEnableUser);
		((Control)this).Controls.Add((Control)(object)cmdGetAllEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdGetEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdClearData);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdGetEnrollInfo);
		((Control)this).Controls.Add((Control)(object)cmdDeleteEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdSetEnrollData);
		((Control)this).Controls.Add((Control)(object)cmdEmptyEnrollData);
		((Control)this).Controls.Add((Control)(object)txtName);
		((Control)this).Controls.Add((Control)(object)chkDisable);
		((Control)this).Controls.Add((Control)(object)cmdDel);
		((Control)this).Controls.Add((Control)(object)cmbPrivilege);
		((Control)this).Controls.Add((Control)(object)lstEnrollData);
		((Control)this).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)this).Controls.Add((Control)(object)cmbBackupNumber);
		((Control)this).Controls.Add((Control)(object)lblCardNum);
		((Control)this).Controls.Add((Control)(object)lbName);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblBackupNumber);
		((Control)this).Controls.Add((Control)(object)_lblEnrollNum_0);
		((Control)this).Name = "frmEnroll";
		((Control)this).Text = "frmEnroll";
		((Form)this).Load += frmEnroll_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(frmEnroll_FormClosed);
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)picUserPhoto).EndInit();
		((Control)groupBox2).ResumeLayout(false);
		((Control)groupBox2).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
