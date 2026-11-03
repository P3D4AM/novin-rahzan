using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxMSFlexGridLib;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class FormlogNew : Form
{
	private int gMachineNumber;

	private AxSBXPC bpc;

	private object[] gstrLogItem;

	private const int gMaxLow = 30000;

	private bool glogSearched = false;

	private int prevSelectLogIndex = -1;

	private IContainer components = null;

	public CheckBox chkShowGlogPhoto;

	public Label lblEnrollData;

	public CheckBox chkAndDelete;

	public CheckBox chkReadMark;

	public Label LabelTotal;

	public Label lblMessage;

	private GroupBox groupBox1;

	private Button btnSetRange;

	private DateTimePicker dtGlogSearchEnd;

	private DateTimePicker dtGlogSearchStart;

	private Label label2;

	private Label label1;

	private CheckBox chkSearchRangeUse;

	private PictureBox picGlogPhoto;

	public Button cmdExit;

	public Button cmdGlogData;

	public Button cmdSLogData;

	public Button cmdEmptySLog;

	public Button cmdEmptyGLog;

	public Button cmdAllGLogData;

	public Button cmdAllSLogData;

	private AxMSFlexGrid gridSLogData;

	private AxMSFlexGrid gridSLogData1;

	private AxMSFlexGrid gridSLogData2;

	public FormlogNew(AxSBXPC bpc, int gMachineNumber)
	{
		InitializeComponent();
		this.bpc = bpc;
		this.gMachineNumber = gMachineNumber;
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
		((Control)Application.OpenForms["frmMain"]).Visible = true;
	}

	private void cmdGlogData_Click(object sender, EventArgs e)
	{
		glogSearched = true;
		int dwTMachineNumber = 0;
		int dwEMachineNumber = 0;
		int dwEnrollNumber = 0;
		int dwVerifyMode = 0;
		int dwYear = 0;
		int dwMonth = 0;
		int dwDay = 0;
		int dwHour = 0;
		int dwMinute = 0;
		int dwSecond = 0;
		int dwErrorCode = 0;
		int num = 30000;
		((Control)lblMessage).Text = "Waiting...";
		((Control)LabelTotal).Text = "Total : ";
		Application.DoEvents();
		gridSLogData.Redraw = false;
		((Control)gridSLogData).Height = 298;
		gridSLogData.Clear();
		((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
		((Control)gridSLogData1).Height = 0;
		gridSLogData1.Redraw = false;
		gridSLogData1.Clear();
		((Control)gridSLogData2).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
		((Control)gridSLogData2).Height = 0;
		gridSLogData2.Redraw = false;
		gridSLogData2.Clear();
		gstrLogItem = new object[6] { "", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime" };
		gridSLogData.Row = 0;
		gridSLogData.Cols = 9;
		gridSLogData[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData.Col = i;
			((Control)gridSLogData).Text = (string)gstrLogItem[i];
			gridSLogData[i] = 3;
			gridSLogData[i] = 1200;
		}
		gridSLogData[4] = 2000;
		gridSLogData.Col = 5;
		gridSLogData[5] = 2000;
		gridSLogData[6] = 700;
		gridSLogData[7] = 700;
		gridSLogData[8] = 700;
		int num2 = gridSLogData.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData.Redraw = true;
		gridSLogData1.Row = 0;
		gridSLogData1.Cols = 9;
		gridSLogData1[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData1.Col = i;
			((Control)gridSLogData1).Text = (string)gstrLogItem[i];
			gridSLogData1[i] = 3;
			gridSLogData1[i] = 1200;
		}
		gridSLogData1[4] = 2000;
		gridSLogData1.Col = 5;
		gridSLogData1[5] = 2000;
		gridSLogData1[6] = 700;
		gridSLogData1[7] = 700;
		gridSLogData1[8] = 700;
		num2 = gridSLogData1.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData1.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData1.Redraw = true;
		gridSLogData2.Row = 0;
		gridSLogData2.Cols = 9;
		gridSLogData2[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData2.Col = i;
			((Control)gridSLogData2).Text = (string)gstrLogItem[i];
			gridSLogData2[i] = 3;
			gridSLogData2[i] = 1200;
		}
		gridSLogData2[4] = 2000;
		gridSLogData2.Col = 5;
		gridSLogData2[5] = 2000;
		gridSLogData2[6] = 700;
		gridSLogData2[7] = 700;
		gridSLogData2[8] = 700;
		num2 = gridSLogData2.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData2.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData2.Redraw = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			((Control)this).Cursor = Cursors.Default;
			return;
		}
		bool flag = bpc.ReadGeneralLogData(gMachineNumber);
		if (!flag)
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else if (chkAndDelete.Checked)
		{
			bpc.EmptyGeneralLogData(gMachineNumber);
		}
		if (flag)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
			((Control)lblMessage).Text = "Getting ...";
			Application.DoEvents();
			gridSLogData.Redraw = false;
			gridSLogData1.Redraw = false;
			gridSLogData2.Redraw = false;
			int i = 1;
			do
			{
				flag = bpc.GetGeneralLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
				if (!flag)
				{
					break;
				}
				if (flag && i != 1)
				{
					gridSLogData.AddItem(Convert.ToString(1));
				}
				AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, 0, gridSLogData);
				((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
				Application.DoEvents();
				i++;
			}
			while (i <= num);
			if (i > num)
			{
				((Control)gridSLogData).Height = ((Control)gridSLogData).Height / 2;
				((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
				((Control)gridSLogData1).Height = ((Control)gridSLogData).Height;
				do
				{
					flag = bpc.GetGeneralLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
					if (!flag)
					{
						break;
					}
					if (flag && i != 1 && i - num > 1)
					{
						gridSLogData1.AddItem(Convert.ToString(1));
					}
					AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, 0, gridSLogData1);
					((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
					Application.DoEvents();
					i++;
				}
				while (i <= num * 2);
			}
			num *= 2;
			if (i > num)
			{
				((Control)gridSLogData).Height = ((Control)gridSLogData).Height * 2 / 3;
				((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
				((Control)gridSLogData1).Height = ((Control)gridSLogData).Height;
				((Control)gridSLogData2).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height * 2;
				((Control)gridSLogData2).Height = ((Control)gridSLogData).Height;
				do
				{
					flag = bpc.GetGeneralLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
					if (!flag)
					{
						break;
					}
					if (flag && i != 1 && i - num > 1)
					{
						gridSLogData2.AddItem(Convert.ToString(1));
					}
					AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, 0, gridSLogData2);
					((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
					Application.DoEvents();
					i++;
				}
				while (i <= 90000);
			}
			gridSLogData.Redraw = true;
			gridSLogData1.Redraw = true;
			gridSLogData2.Redraw = true;
			((Control)lblMessage).Text = "ReadGeneralLogData OK";
		}
		((Control)this).Cursor = Cursors.Default;
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void cmdSLogData_Click(object sender, EventArgs e)
	{
		glogSearched = false;
		int dwTMachineNumber = 0;
		int dwSMachineNumber = 0;
		int dwSEnrollNumber = 0;
		int dwGEnrollNumber = 0;
		int dwGMachineNumber = 0;
		int dwManipulation = 0;
		int dwBackupNumber = 0;
		int dwYear = 0;
		int dwMonth = 0;
		int dwDay = 0;
		int dwHour = 0;
		int dwMinute = 0;
		int dwSecond = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Waiting...";
		((Control)LabelTotal).Text = "Total : ";
		Application.DoEvents();
		gridSLogData.Redraw = false;
		((Control)gridSLogData).Height = 298;
		gridSLogData.Clear();
		gstrLogItem = new object[9] { "", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime" };
		gridSLogData.Cols = 9;
		gridSLogData.Row = 0;
		gridSLogData[0] = 600;
		for (int i = 1; i < 9; i++)
		{
			gridSLogData.Col = i;
			((Control)gridSLogData).Text = (string)gstrLogItem[i];
			gridSLogData[i] = 3;
			gridSLogData[i] = 900;
		}
		gridSLogData.Col = 6;
		gridSLogData[6] = 2000;
		gridSLogData[6] = 2;
		gridSLogData[7] = 800;
		gridSLogData.Col = 8;
		((Control)gridSLogData).Text = (string)gstrLogItem[8];
		gridSLogData[8] = 2000;
		int num = gridSLogData.Rows;
		if (num > 2)
		{
			while (num != 2)
			{
				gridSLogData.RemoveItem(num);
				num--;
			}
		}
		gridSLogData.Redraw = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			((Control)this).Cursor = Cursors.Default;
			return;
		}
		bool flag = bpc.ReadSuperLogData(gMachineNumber);
		if (!flag)
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else if (chkAndDelete.Checked)
		{
			bpc.EmptySuperLogData(gMachineNumber);
		}
		if (flag)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
			((Control)lblMessage).Text = "Getting ...";
			Application.DoEvents();
			gridSLogData.Redraw = false;
			int i = 1;
			while (true)
			{
				flag = bpc.GetSuperLogData(gMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref dwSMachineNumber, ref dwGEnrollNumber, ref dwGMachineNumber, ref dwManipulation, ref dwBackupNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
				if (!flag)
				{
					break;
				}
				if (flag && i != 1)
				{
					gridSLogData.AddItem(Convert.ToString(1));
				}
				gridSLogData.Row = i;
				gridSLogData.Col = 0;
				((Control)gridSLogData).Text = Convert.ToString(i);
				gridSLogData.Col = 1;
				((Control)gridSLogData).Text = Convert.ToString(dwTMachineNumber);
				gridSLogData.Col = 2;
				((Control)gridSLogData).Text = Convert.ToString(dwSEnrollNumber);
				gridSLogData.Col = 3;
				((Control)gridSLogData).Text = Convert.ToString(dwSMachineNumber);
				gridSLogData.Col = 4;
				((Control)gridSLogData).Text = Convert.ToString(dwGEnrollNumber);
				gridSLogData.Col = 5;
				((Control)gridSLogData).Text = Convert.ToString(dwGMachineNumber);
				gridSLogData.Col = 6;
				switch (dwManipulation)
				{
				case 1:
				case 2:
				case 3:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Enroll user";
					break;
				case 4:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Enroll Manager";
					break;
				case 5:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Fp Data";
					break;
				case 6:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Password";
					break;
				case 7:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Card Data";
					break;
				case 8:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete All LogData";
					break;
				case 9:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify System Info";
					break;
				case 10:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify System Time";
					break;
				case 11:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Log Setting";
					break;
				case 12:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Comm Setting";
					break;
				case 13:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Timezone Setting";
					break;
				case 14:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Face";
					break;
				default:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Unknown";
					break;
				}
				gridSLogData.Col = 7;
				if (dwBackupNumber < 10)
				{
					((Control)gridSLogData).Text = Convert.ToString(dwBackupNumber);
				}
				else
				{
					switch (dwBackupNumber)
					{
					case 10:
						((Control)gridSLogData).Text = "Password";
						break;
					case 14:
						((Control)gridSLogData).Text = "Face";
						break;
					default:
						((Control)gridSLogData).Text = "Card";
						break;
					}
				}
				gridSLogData.Col = 8;
				((Control)gridSLogData).Text = Convert.ToString(dwYear) + "/" + $"{dwMonth:D2}" + "/" + $"{dwDay:D2}" + " " + $"{dwHour:D2}" + ":" + $"{dwMinute:D2}";
				((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
				Application.DoEvents();
				i++;
			}
			gridSLogData.Redraw = true;
			((Control)lblMessage).Text = "ReadSuperLogData OK";
		}
		((Control)this).Cursor = Cursors.Default;
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void cmdEmptySLog_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.EmptySuperLogData(gMachineNumber))
		{
			((Control)lblMessage).Text = "EmptySuperLogData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void cmdEmptyGLog_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		if (bpc.EmptyGeneralLogData(gMachineNumber))
		{
			((Control)lblMessage).Text = "EmptyGeneralLogData OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void chkReadMark_CheckedChanged(object sender, EventArgs e)
	{
		bpc.ReadMark = chkReadMark.Checked;
	}

	private void cmdAllGLogData_Click(object sender, EventArgs e)
	{
		glogSearched = true;
		int dwTMachineNumber = 0;
		int dwEMachineNumber = 0;
		int dwEnrollNumber = 0;
		int dwVerifyMode = 0;
		int dwYear = 0;
		int dwMonth = 0;
		int dwDay = 0;
		int dwHour = 0;
		int dwMinute = 0;
		int dwSecond = 0;
		int dwErrorCode = 0;
		int num = 30000;
		((Control)lblMessage).Text = "Waiting...";
		((Control)LabelTotal).Text = "Total : ";
		gridSLogData.Redraw = false;
		((Control)gridSLogData).Height = 298;
		gridSLogData.Clear();
		((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
		((Control)gridSLogData1).Height = 0;
		gridSLogData1.Redraw = false;
		gridSLogData1.Clear();
		((Control)gridSLogData2).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
		((Control)gridSLogData2).Height = 0;
		gridSLogData2.Redraw = false;
		gridSLogData2.Clear();
		gstrLogItem = new object[6] { "", "PhotoNo", "EnrollNo", "EMachineNo", "VeriMode", "DateTime" };
		gridSLogData.Row = 0;
		gridSLogData.Cols = 9;
		gridSLogData[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData.Col = i;
			((Control)gridSLogData).Text = (string)gstrLogItem[i];
			gridSLogData[i] = 3;
			gridSLogData[i] = 1200;
		}
		gridSLogData[4] = 2000;
		gridSLogData.Col = 5;
		gridSLogData[5] = 2000;
		gridSLogData[6] = 700;
		gridSLogData[7] = 700;
		gridSLogData[8] = 700;
		int num2 = gridSLogData.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData.Redraw = true;
		gridSLogData1.Row = 0;
		gridSLogData1.Cols = 9;
		gridSLogData1[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData1.Col = i;
			((Control)gridSLogData1).Text = (string)gstrLogItem[i];
			gridSLogData1[i] = 3;
			gridSLogData1[i] = 1200;
		}
		gridSLogData1[4] = 2000;
		gridSLogData1.Col = 5;
		gridSLogData1[5] = 2000;
		gridSLogData1[6] = 700;
		gridSLogData1[7] = 700;
		gridSLogData1[8] = 700;
		num2 = gridSLogData1.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData1.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData1.Redraw = true;
		gridSLogData2.Row = 0;
		gridSLogData2.Cols = 9;
		gridSLogData2[0] = 600;
		for (int i = 1; i < 6; i++)
		{
			gridSLogData2.Col = i;
			((Control)gridSLogData2).Text = (string)gstrLogItem[i];
			gridSLogData2[i] = 3;
			gridSLogData2[i] = 1200;
		}
		gridSLogData2[4] = 2000;
		gridSLogData2.Col = 5;
		gridSLogData2[5] = 2000;
		gridSLogData2[6] = 700;
		gridSLogData2[7] = 700;
		gridSLogData2[8] = 700;
		num2 = gridSLogData2.Rows;
		if (num2 > 2)
		{
			while (num2 != 2)
			{
				gridSLogData2.RemoveItem(num2);
				num2--;
			}
		}
		gridSLogData2.Redraw = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			((Control)this).Cursor = Cursors.Default;
			return;
		}
		bool flag = bpc.ReadAllGLogData(gMachineNumber);
		if (!flag)
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else if (chkAndDelete.Checked)
		{
			bpc.EmptyGeneralLogData(gMachineNumber);
		}
		if (flag)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
			((Control)lblMessage).Text = "Getting ...";
			Application.DoEvents();
			gridSLogData.Redraw = false;
			gridSLogData1.Redraw = false;
			gridSLogData2.Redraw = false;
			int i = 1;
			do
			{
				flag = bpc.GetAllGLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
				if (!flag)
				{
					break;
				}
				if (flag && i != 1)
				{
					gridSLogData.AddItem(Convert.ToString(1));
				}
				AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, 0, gridSLogData);
				((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
				Application.DoEvents();
				i++;
			}
			while (i <= num);
			if (i > num)
			{
				((Control)gridSLogData).Height = ((Control)gridSLogData).Height / 2;
				((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
				((Control)gridSLogData1).Height = ((Control)gridSLogData).Height;
				do
				{
					flag = bpc.GetAllGLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
					if (!flag)
					{
						break;
					}
					if (flag && i != 1 && i - num > 1)
					{
						gridSLogData1.AddItem(Convert.ToString(1));
					}
					AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, num, gridSLogData1);
					((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
					Application.DoEvents();
					i++;
				}
				while (i <= num * 2);
			}
			num *= 2;
			if (i > num)
			{
				((Control)gridSLogData).Height = ((Control)gridSLogData).Height * 2 / 3;
				((Control)gridSLogData1).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height;
				((Control)gridSLogData1).Height = ((Control)gridSLogData).Height;
				((Control)gridSLogData2).Top = ((Control)gridSLogData).Top + ((Control)gridSLogData).Height * 2;
				((Control)gridSLogData2).Height = ((Control)gridSLogData).Height;
				do
				{
					flag = bpc.GetAllGLogData(gMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
					if (!flag)
					{
						break;
					}
					if (flag && i != 1 && i - num > 1)
					{
						gridSLogData2.AddItem(Convert.ToString(1));
					}
					AddGLogItem(dwTMachineNumber, dwEnrollNumber, dwEMachineNumber, dwVerifyMode, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond, i, num, gridSLogData2);
					((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
					Application.DoEvents();
					i++;
				}
				while (i <= 90000);
			}
			gridSLogData.Redraw = true;
			gridSLogData1.Redraw = true;
			gridSLogData2.Redraw = true;
			((Control)lblMessage).Text = "ReadAllGLogData OK";
		}
		((Control)this).Cursor = Cursors.Default;
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void cmdAllSLogData_Click(object sender, EventArgs e)
	{
		glogSearched = false;
		int dwTMachineNumber = 0;
		int dwSMachineNumber = 0;
		int dwSEnrollNumber = 0;
		int dwGEnrollNumber = 0;
		int dwGMachineNumber = 0;
		int dwManipulation = 0;
		int dwBackupNumber = 0;
		int dwYear = 0;
		int dwMonth = 0;
		int dwDay = 0;
		int dwHour = 0;
		int dwMinute = 0;
		int dwSecond = 0;
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Waiting...";
		((Control)LabelTotal).Text = "Total : ";
		Application.DoEvents();
		gridSLogData.Redraw = false;
		((Control)gridSLogData).Height = 298;
		gridSLogData.Clear();
		gstrLogItem = new object[9] { "", "TMNo", "SEnlNo", "SMNo", "GEnlNo", "GMNo", "Manipulation", "FpNo", "DateTime" };
		gridSLogData.Cols = 9;
		gridSLogData.Row = 0;
		gridSLogData[0] = 600;
		for (int i = 1; i < 9; i++)
		{
			gridSLogData.Col = i;
			((Control)gridSLogData).Text = (string)gstrLogItem[i];
			gridSLogData[i] = 3;
			gridSLogData[i] = 900;
		}
		gridSLogData.Col = 6;
		gridSLogData[6] = 2000;
		gridSLogData[6] = 2;
		gridSLogData[7] = 800;
		gridSLogData.Col = 8;
		((Control)gridSLogData).Text = (string)gstrLogItem[8];
		gridSLogData[8] = 2000;
		int num = gridSLogData.Rows;
		if (num > 2)
		{
			while (num != 2)
			{
				gridSLogData.RemoveItem(num);
				num--;
			}
		}
		gridSLogData.Redraw = true;
		((Control)this).Cursor = Cursors.WaitCursor;
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			((Control)this).Cursor = Cursors.Default;
			return;
		}
		bool flag = bpc.ReadAllSLogData(gMachineNumber);
		if (!flag)
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		else if (chkAndDelete.Checked)
		{
			bpc.EmptySuperLogData(gMachineNumber);
		}
		if (flag)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
			((Control)lblMessage).Text = "Getting ...";
			Application.DoEvents();
			gridSLogData.Redraw = false;
			int i = 1;
			while (true)
			{
				flag = bpc.GetAllSLogData(gMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref dwSMachineNumber, ref dwGEnrollNumber, ref dwGMachineNumber, ref dwManipulation, ref dwBackupNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
				if (!flag)
				{
					break;
				}
				if (flag && i != 1)
				{
					gridSLogData.AddItem(Convert.ToString(1));
				}
				gridSLogData.Row = i;
				gridSLogData.Col = 0;
				((Control)gridSLogData).Text = Convert.ToString(i);
				gridSLogData.Col = 1;
				((Control)gridSLogData).Text = Convert.ToString(dwTMachineNumber);
				gridSLogData.Col = 2;
				((Control)gridSLogData).Text = Convert.ToString(dwSEnrollNumber);
				gridSLogData.Col = 3;
				((Control)gridSLogData).Text = Convert.ToString(dwSMachineNumber);
				gridSLogData.Col = 4;
				((Control)gridSLogData).Text = Convert.ToString(dwGEnrollNumber);
				gridSLogData.Col = 5;
				((Control)gridSLogData).Text = Convert.ToString(dwGMachineNumber);
				gridSLogData.Col = 6;
				switch (dwManipulation)
				{
				case 1:
				case 2:
				case 3:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Enroll user";
					break;
				case 4:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Enroll Manager";
					break;
				case 5:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Fp Data";
					break;
				case 6:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Password";
					break;
				case 7:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Card Data";
					break;
				case 8:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete All LogData";
					break;
				case 9:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify System Info";
					break;
				case 10:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify System Time";
					break;
				case 11:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Log Setting";
					break;
				case 12:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Comm Setting";
					break;
				case 13:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Modify Timezone Setting";
					break;
				case 14:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Delete Face";
					break;
				default:
					((Control)gridSLogData).Text = Convert.ToString(dwManipulation) + "--Unknown";
					break;
				}
				gridSLogData.Col = 7;
				if (dwBackupNumber < 10)
				{
					((Control)gridSLogData).Text = Convert.ToString(dwBackupNumber);
				}
				else
				{
					switch (dwBackupNumber)
					{
					case 10:
						((Control)gridSLogData).Text = "Password";
						break;
					case 14:
						((Control)gridSLogData).Text = "Face";
						break;
					default:
						((Control)gridSLogData).Text = "Card";
						break;
					}
				}
				gridSLogData.Col = 8;
				((Control)gridSLogData).Text = Convert.ToString(dwYear) + "/" + $"{dwMonth:D2}" + "/" + $"{dwDay:D2}" + " " + $"{dwHour:D2}" + ":" + $"{dwMinute:D2}";
				((Control)LabelTotal).Text = "Total : " + Convert.ToString(i);
				Application.DoEvents();
				i++;
			}
			gridSLogData.Redraw = true;
			((Control)lblMessage).Text = "ReadAllSLogData OK";
		}
		((Control)this).Cursor = Cursors.Default;
		bpc.EnableDevice(gMachineNumber, 1);
	}

	private void FormlogNew_Load(object sender, EventArgs e)
	{
		picGlogPhoto.Image = null;
		chkReadMark.Checked = true;
	}

	private void btnSetRange_Click(object sender, EventArgs e)
	{
		bool flag = false;
		int dwErrorCode = 0;
		string strXML = "";
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!(bpc?.EnableDevice(gMachineNumber, 0)).Value)
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "SetGLogSearchRange");
		bpc.XML_AddBoolean(ref strXML, "UseSearchRange", chkSearchRangeUse.Checked);
		if (chkSearchRangeUse.Checked)
		{
			bpc.XML_AddLong(ref strXML, "StartYear", dtGlogSearchStart.Value.Year);
			bpc.XML_AddLong(ref strXML, "StartMonth", dtGlogSearchStart.Value.Month);
			bpc.XML_AddLong(ref strXML, "StartDate", dtGlogSearchStart.Value.Day);
			bpc.XML_AddLong(ref strXML, "EndYear", dtGlogSearchEnd.Value.Year);
			bpc.XML_AddLong(ref strXML, "EndMonth", dtGlogSearchEnd.Value.Month);
			bpc.XML_AddLong(ref strXML, "EndDate", dtGlogSearchEnd.Value.Day);
		}
		if (bpc.GeneralOperationXML(ref strXML))
		{
			((Control)lblMessage).Text = "SetGlogSearchRange OK";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		flag = bpc.EnableDevice(gMachineNumber, 1);
	}

	private void AddGLogItem(int vTMachineNumber, int vSEnrollNumber, int vSMachineNumber, int vVerifyMode, int vYear, int vMonth, int vDay, int vHour, int vMinute, int vSecond, int index, int vMaxLogCnt, AxMSFlexGrid gridGlogData)
	{
		int num = 65536;
		int num2 = vVerifyMode / num;
		vVerifyMode %= num;
		int num3 = vVerifyMode / 256;
		vVerifyMode %= 256;
		string text = "";
		string text2 = "";
		switch (num3)
		{
		case 0:
			text = "_DutyOn";
			break;
		case 1:
			text = "_DutyOff";
			break;
		case 2:
			text = "_OverOn";
			break;
		case 3:
			text = "_OverOff";
			break;
		case 4:
			text = "_GoIn";
			break;
		case 5:
			text = "_GoOut";
			break;
		}
		switch (num2)
		{
		case 1:
			text2 = "(AP_In)";
			break;
		case 3:
			text2 = "(AP_Out)";
			break;
		}
		gridGlogData.Row = index - vMaxLogCnt;
		gridGlogData.Col = 0;
		((Control)gridGlogData).Text = Convert.ToString(index);
		gridGlogData.Col = 1;
		((Control)gridGlogData).Text = ((vTMachineNumber == -1) ? "No Photo" : Convert.ToString(vTMachineNumber));
		gridGlogData.Col = 2;
		((Control)gridGlogData).Text = Convert.ToString(vSEnrollNumber);
		gridGlogData.Col = 3;
		((Control)gridGlogData).Text = Convert.ToString(vSMachineNumber);
		gridGlogData.Col = 4;
		switch (vVerifyMode)
		{
		case 1:
			((Control)gridGlogData).Text = "Fp";
			break;
		case 2:
			((Control)gridGlogData).Text = "Password";
			break;
		case 3:
			((Control)gridGlogData).Text = "Card";
			break;
		case 4:
			((Control)gridGlogData).Text = "FP+Card";
			break;
		case 5:
			((Control)gridGlogData).Text = "FP+Pwd";
			break;
		case 6:
			((Control)gridGlogData).Text = "Card+Pwd";
			break;
		case 7:
			((Control)gridGlogData).Text = "FP+Card+Pwd";
			break;
		case 10:
			((Control)gridGlogData).Text = "Hand Lock";
			break;
		case 11:
			((Control)gridGlogData).Text = "Prog Lock";
			break;
		case 12:
			((Control)gridGlogData).Text = "Prog Open";
			break;
		case 13:
			((Control)gridGlogData).Text = "Prog Close";
			break;
		case 14:
			((Control)gridGlogData).Text = "Auto Recover";
			break;
		case 20:
			((Control)gridGlogData).Text = "Lock Over";
			break;
		case 21:
			((Control)gridGlogData).Text = "Illegal Open";
			break;
		case 22:
			((Control)gridGlogData).Text = "Duress alarm";
			break;
		case 23:
			((Control)gridGlogData).Text = "Tamper detect";
			break;
		case 30:
			((Control)gridGlogData).Text = "FACE";
			break;
		case 31:
			((Control)gridGlogData).Text = "FACE+CARD";
			break;
		case 32:
			((Control)gridGlogData).Text = "FACE+PWD";
			break;
		case 33:
			((Control)gridGlogData).Text = "FACE+CARD+PWD";
			break;
		case 34:
			((Control)gridGlogData).Text = "FACE+FP";
			break;
		case 51:
			((Control)gridGlogData).Text = "Fp";
			break;
		case 52:
			((Control)gridGlogData).Text = "Password";
			break;
		case 53:
			((Control)gridGlogData).Text = "Card";
			break;
		case 101:
			((Control)gridGlogData).Text = "Fp";
			break;
		case 102:
			((Control)gridGlogData).Text = "Password";
			break;
		case 103:
			((Control)gridGlogData).Text = "Card";
			break;
		case 151:
			((Control)gridGlogData).Text = "Fp";
			break;
		case 152:
			((Control)gridGlogData).Text = "Password";
			break;
		case 153:
			((Control)gridGlogData).Text = "Card";
			break;
		default:
			((Control)gridGlogData).Text = "--";
			break;
		}
		if (1 <= vVerifyMode && vVerifyMode <= 7)
		{
			((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text;
		}
		else if (30 <= vVerifyMode && vVerifyMode <= 33)
		{
			((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text;
		}
		else if (51 <= vVerifyMode && vVerifyMode <= 53)
		{
			((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text;
		}
		else if (101 <= vVerifyMode && vVerifyMode <= 103)
		{
			((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text;
		}
		else if (151 <= vVerifyMode && vVerifyMode <= 153)
		{
			((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text;
		}
		((Control)gridGlogData).Text = ((Control)gridGlogData).Text + text2;
		gridGlogData.Col = 5;
		((Control)gridGlogData).Text = Convert.ToString(vYear) + "/" + $"{vMonth:D2}" + "/" + $"{vDay:D2}" + " " + $"{vHour:D2}" + ":" + $"{vMinute:D2}";
	}

	private void gridSLogData_ClickEvent(object sender, EventArgs e)
	{
		if (!glogSearched || !chkShowGlogPhoto.Checked || prevSelectLogIndex == gridSLogData.Row)
		{
			return;
		}
		prevSelectLogIndex = gridSLogData.Row;
		int dwErrorCode = 0;
		string strXML = "";
		string text = gridSLogData[gridSLogData.Row, 1];
		if (text == "No Photo")
		{
			ClearGlogPhoto();
			return;
		}
		int dwValue = Convert.ToInt32(text);
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		util.MakeXMLRequestHeader(bpc, ref strXML, "GetGLogPhotoData");
		bpc.XML_AddLong(ref strXML, "PhotoPos", dwValue);
		if (!bpc.GeneralOperationXML(ref strXML))
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
			ClearGlogPhoto();
		}
		else
		{
			byte[] array = new byte[util.gCompressPhotoSize];
			int pData = GCHandle.Alloc(array, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
			if (!bpc.XML_ParseBinaryLong(ref strXML, "PhotoData", ref pData, util.gCompressPhotoSize))
			{
				((Control)lblMessage).Text = "GetGlogPhotoData - XML Parse Error";
				ClearGlogPhoto();
			}
			else
			{
				((Control)lblMessage).Text = "GetGLogPhotoData OK";
				ClearGlogPhoto();
				if (File.Exists(util.gTempPhotoFile))
				{
					File.Delete(util.gTempPhotoFile);
				}
				FileStream fileStream = File.Create(util.gTempPhotoFile);
				fileStream.Write(array, 0, util.gCompressPhotoSize);
				fileStream.Close();
				fileStream.Dispose();
				fileStream = null;
				picGlogPhoto.Image = Image.FromFile(util.gTempPhotoFile);
			}
		}
		bool flag = bpc.EnableDevice(gMachineNumber, 1);
	}

	private void FormlogNew_FormClosing(object sender, FormClosingEventArgs e)
	{
		((Control)Application.OpenForms["frmMain"]).Visible = true;
		ClearGlogPhoto();
	}

	private void ClearGlogPhoto()
	{
		if (picGlogPhoto.Image != null)
		{
			picGlogPhoto.Image.Dispose();
		}
		picGlogPhoto.Image = null;
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
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Expected O, but got Unknown
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8a: Expected O, but got Unknown
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Expected O, but got Unknown
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Expected O, but got Unknown
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Expected O, but got Unknown
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e13: Expected O, but got Unknown
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_10b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bb: Expected O, but got Unknown
		//IL_1137: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Expected O, but got Unknown
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11af: Expected O, but got Unknown
		//IL_136e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1378: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormlogNew));
		chkShowGlogPhoto = new CheckBox();
		lblEnrollData = new Label();
		chkAndDelete = new CheckBox();
		chkReadMark = new CheckBox();
		LabelTotal = new Label();
		lblMessage = new Label();
		groupBox1 = new GroupBox();
		btnSetRange = new Button();
		dtGlogSearchEnd = new DateTimePicker();
		dtGlogSearchStart = new DateTimePicker();
		label2 = new Label();
		label1 = new Label();
		chkSearchRangeUse = new CheckBox();
		picGlogPhoto = new PictureBox();
		cmdExit = new Button();
		cmdGlogData = new Button();
		cmdSLogData = new Button();
		cmdEmptySLog = new Button();
		cmdEmptyGLog = new Button();
		cmdAllGLogData = new Button();
		cmdAllSLogData = new Button();
		gridSLogData = new AxMSFlexGrid();
		gridSLogData1 = new AxMSFlexGrid();
		gridSLogData2 = new AxMSFlexGrid();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)picGlogPhoto).BeginInit();
		((ISupportInitialize)gridSLogData).BeginInit();
		((ISupportInitialize)gridSLogData1).BeginInit();
		((ISupportInitialize)gridSLogData2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)chkShowGlogPhoto).BackColor = SystemColors.Control;
		((Control)chkShowGlogPhoto).Cursor = Cursors.Default;
		((Control)chkShowGlogPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkShowGlogPhoto).ForeColor = SystemColors.ControlText;
		((Control)chkShowGlogPhoto).Location = new Point(715, 71);
		((Control)chkShowGlogPhoto).Name = "chkShowGlogPhoto";
		((Control)chkShowGlogPhoto).RightToLeft = (RightToLeft)0;
		((Control)chkShowGlogPhoto).Size = new Size(151, 25);
		((Control)chkShowGlogPhoto).TabIndex = 48;
		((Control)chkShowGlogPhoto).Text = "Show Glog Photo";
		((ButtonBase)chkShowGlogPhoto).UseVisualStyleBackColor = true;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(15, 72);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(73, 19);
		((Control)lblEnrollData).TabIndex = 46;
		((Control)lblEnrollData).Text = "Log Data :";
		((Control)chkAndDelete).BackColor = SystemColors.Control;
		((Control)chkAndDelete).Cursor = Cursors.Default;
		((Control)chkAndDelete).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkAndDelete).ForeColor = SystemColors.ControlText;
		((Control)chkAndDelete).Location = new Point(445, 71);
		((Control)chkAndDelete).Name = "chkAndDelete";
		((Control)chkAndDelete).RightToLeft = (RightToLeft)0;
		((Control)chkAndDelete).Size = new Size(138, 19);
		((Control)chkAndDelete).TabIndex = 47;
		((Control)chkAndDelete).Text = "and Delete ";
		((ButtonBase)chkAndDelete).UseVisualStyleBackColor = true;
		((Control)chkReadMark).BackColor = SystemColors.Control;
		((Control)chkReadMark).Cursor = Cursors.Default;
		((Control)chkReadMark).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)chkReadMark).ForeColor = SystemColors.ControlText;
		((Control)chkReadMark).Location = new Point(589, 71);
		((Control)chkReadMark).Name = "chkReadMark";
		((Control)chkReadMark).RightToLeft = (RightToLeft)0;
		((Control)chkReadMark).Size = new Size(101, 19);
		((Control)chkReadMark).TabIndex = 44;
		((Control)chkReadMark).Text = "ReadMark";
		((ButtonBase)chkReadMark).UseVisualStyleBackColor = true;
		chkReadMark.CheckedChanged += chkReadMark_CheckedChanged;
		((Control)LabelTotal).AutoSize = true;
		((Control)LabelTotal).BackColor = SystemColors.Control;
		((Control)LabelTotal).Cursor = Cursors.Default;
		((Control)LabelTotal).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)LabelTotal).ForeColor = SystemColors.ControlText;
		((Control)LabelTotal).Location = new Point(116, 72);
		((Control)LabelTotal).Name = "LabelTotal";
		((Control)LabelTotal).RightToLeft = (RightToLeft)0;
		((Control)LabelTotal).Size = new Size(46, 19);
		((Control)LabelTotal).TabIndex = 45;
		((Control)LabelTotal).Text = "Total :";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 26);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(871, 28);
		((Control)lblMessage).TabIndex = 43;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)groupBox1).Controls.Add((Control)(object)btnSetRange);
		((Control)groupBox1).Controls.Add((Control)(object)dtGlogSearchEnd);
		((Control)groupBox1).Controls.Add((Control)(object)dtGlogSearchStart);
		((Control)groupBox1).Controls.Add((Control)(object)label2);
		((Control)groupBox1).Controls.Add((Control)(object)label1);
		((Control)groupBox1).Controls.Add((Control)(object)chkSearchRangeUse);
		((Control)groupBox1).Font = new Font("Times New Roman", 12f);
		((Control)groupBox1).Location = new Point(756, 256);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(184, 182);
		((Control)groupBox1).TabIndex = 50;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Glog Search Range";
		((Control)btnSetRange).Font = new Font("Times New Roman", 12f, (FontStyle)1);
		((Control)btnSetRange).Location = new Point(33, 138);
		((Control)btnSetRange).Name = "btnSetRange";
		((Control)btnSetRange).Size = new Size(116, 27);
		((Control)btnSetRange).TabIndex = 5;
		((Control)btnSetRange).Text = "Set Range";
		((ButtonBase)btnSetRange).UseVisualStyleBackColor = true;
		((Control)btnSetRange).Click += btnSetRange_Click;
		dtGlogSearchEnd.Format = (DateTimePickerFormat)2;
		((Control)dtGlogSearchEnd).Location = new Point(60, 98);
		((Control)dtGlogSearchEnd).Name = "dtGlogSearchEnd";
		((Control)dtGlogSearchEnd).Size = new Size(118, 26);
		((Control)dtGlogSearchEnd).TabIndex = 4;
		dtGlogSearchStart.Format = (DateTimePickerFormat)2;
		((Control)dtGlogSearchStart).Location = new Point(60, 64);
		((Control)dtGlogSearchStart).Name = "dtGlogSearchStart";
		((Control)dtGlogSearchStart).Size = new Size(118, 26);
		((Control)dtGlogSearchStart).TabIndex = 3;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(9, 102);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(36, 19);
		((Control)label2).TabIndex = 2;
		((Control)label2).Text = "End:";
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(9, 68);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(41, 19);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "Start:";
		((Control)chkSearchRangeUse).AutoSize = true;
		((Control)chkSearchRangeUse).Location = new Point(21, 25);
		((Control)chkSearchRangeUse).Name = "chkSearchRangeUse";
		((Control)chkSearchRangeUse).Size = new Size(140, 23);
		((Control)chkSearchRangeUse).TabIndex = 0;
		((Control)chkSearchRangeUse).Text = "Use Search Range";
		((ButtonBase)chkSearchRangeUse).UseVisualStyleBackColor = true;
		picGlogPhoto.BorderStyle = (BorderStyle)2;
		((Control)picGlogPhoto).Location = new Point(758, 121);
		((Control)picGlogPhoto).Name = "picGlogPhoto";
		((Control)picGlogPhoto).Size = new Size(164, 129);
		picGlogPhoto.SizeMode = (PictureBoxSizeMode)1;
		picGlogPhoto.TabIndex = 49;
		picGlogPhoto.TabStop = false;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(606, 528);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(94, 43);
		((Control)cmdExit).TabIndex = 53;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = true;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdGlogData).BackColor = SystemColors.Control;
		((Control)cmdGlogData).Cursor = Cursors.Default;
		((Control)cmdGlogData).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGlogData).ForeColor = SystemColors.ControlText;
		((Control)cmdGlogData).Location = new Point(315, 528);
		((Control)cmdGlogData).Name = "cmdGlogData";
		((Control)cmdGlogData).RightToLeft = (RightToLeft)0;
		((Control)cmdGlogData).Size = new Size(94, 43);
		((Control)cmdGlogData).TabIndex = 52;
		((Control)cmdGlogData).Text = "Read GLogData";
		((ButtonBase)cmdGlogData).UseVisualStyleBackColor = true;
		((Control)cmdGlogData).Click += cmdGlogData_Click;
		((Control)cmdSLogData).BackColor = SystemColors.Control;
		((Control)cmdSLogData).Cursor = Cursors.Default;
		((Control)cmdSLogData).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdSLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdSLogData).Location = new Point(22, 528);
		((Control)cmdSLogData).Name = "cmdSLogData";
		((Control)cmdSLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdSLogData).Size = new Size(94, 43);
		((Control)cmdSLogData).TabIndex = 51;
		((Control)cmdSLogData).Text = "Read SLogData";
		((ButtonBase)cmdSLogData).UseVisualStyleBackColor = true;
		((Control)cmdSLogData).Click += cmdSLogData_Click;
		((Control)cmdEmptySLog).BackColor = SystemColors.Control;
		((Control)cmdEmptySLog).Cursor = Cursors.Default;
		((Control)cmdEmptySLog).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptySLog).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptySLog).Location = new Point(218, 528);
		((Control)cmdEmptySLog).Name = "cmdEmptySLog";
		((Control)cmdEmptySLog).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptySLog).Size = new Size(94, 43);
		((Control)cmdEmptySLog).TabIndex = 57;
		((Control)cmdEmptySLog).Text = "Empty SLogData";
		((ButtonBase)cmdEmptySLog).UseVisualStyleBackColor = true;
		((Control)cmdEmptySLog).Click += cmdEmptySLog_Click;
		((Control)cmdEmptyGLog).BackColor = SystemColors.Control;
		((Control)cmdEmptyGLog).Cursor = Cursors.Default;
		((Control)cmdEmptyGLog).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdEmptyGLog).ForeColor = SystemColors.ControlText;
		((Control)cmdEmptyGLog).Location = new Point(511, 528);
		((Control)cmdEmptyGLog).Name = "cmdEmptyGLog";
		((Control)cmdEmptyGLog).RightToLeft = (RightToLeft)0;
		((Control)cmdEmptyGLog).Size = new Size(94, 43);
		((Control)cmdEmptyGLog).TabIndex = 56;
		((Control)cmdEmptyGLog).Text = "Empty GLogData";
		((ButtonBase)cmdEmptyGLog).UseVisualStyleBackColor = true;
		((Control)cmdEmptyGLog).Click += cmdEmptyGLog_Click;
		((Control)cmdAllGLogData).BackColor = SystemColors.Control;
		((Control)cmdAllGLogData).Cursor = Cursors.Default;
		((Control)cmdAllGLogData).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAllGLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdAllGLogData).Location = new Point(413, 528);
		((Control)cmdAllGLogData).Name = "cmdAllGLogData";
		((Control)cmdAllGLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdAllGLogData).Size = new Size(94, 43);
		((Control)cmdAllGLogData).TabIndex = 55;
		((Control)cmdAllGLogData).Text = "Read All GLogData";
		((ButtonBase)cmdAllGLogData).UseVisualStyleBackColor = true;
		((Control)cmdAllGLogData).Click += cmdAllGLogData_Click;
		((Control)cmdAllSLogData).BackColor = SystemColors.Control;
		((Control)cmdAllSLogData).Cursor = Cursors.Default;
		((Control)cmdAllSLogData).Font = new Font("Times New Roman", 11f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdAllSLogData).ForeColor = SystemColors.ControlText;
		((Control)cmdAllSLogData).Location = new Point(120, 528);
		((Control)cmdAllSLogData).Name = "cmdAllSLogData";
		((Control)cmdAllSLogData).RightToLeft = (RightToLeft)0;
		((Control)cmdAllSLogData).Size = new Size(94, 43);
		((Control)cmdAllSLogData).TabIndex = 54;
		((Control)cmdAllSLogData).Text = "Read All SLogData";
		((ButtonBase)cmdAllSLogData).UseVisualStyleBackColor = true;
		((Control)cmdAllSLogData).Click += cmdAllSLogData_Click;
		((Control)gridSLogData).Location = new Point(22, 121);
		((Control)gridSLogData).Name = "gridSLogData";
		((AxHost)gridSLogData).OcxState = (State)componentResourceManager.GetObject("gridSLogData.OcxState");
		((Control)gridSLogData).Size = new Size(711, 387);
		((Control)gridSLogData).TabIndex = 58;
		gridSLogData.ClickEvent += gridSLogData_ClickEvent;
		((Control)gridSLogData1).Location = new Point(22, 380);
		((Control)gridSLogData1).Name = "gridSLogData1";
		((AxHost)gridSLogData1).OcxState = (State)componentResourceManager.GetObject("gridSLogData1.OcxState");
		((Control)gridSLogData1).Size = new Size(711, 128);
		((Control)gridSLogData1).TabIndex = 59;
		((Control)gridSLogData2).Location = new Point(22, 443);
		((Control)gridSLogData2).Name = "gridSLogData2";
		((AxHost)gridSLogData2).OcxState = (State)componentResourceManager.GetObject("gridSLogData2.OcxState");
		((Control)gridSLogData2).Size = new Size(711, 65);
		((Control)gridSLogData2).TabIndex = 60;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(958, 583);
		((Control)this).Controls.Add((Control)(object)gridSLogData);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdGlogData);
		((Control)this).Controls.Add((Control)(object)cmdSLogData);
		((Control)this).Controls.Add((Control)(object)cmdEmptySLog);
		((Control)this).Controls.Add((Control)(object)cmdEmptyGLog);
		((Control)this).Controls.Add((Control)(object)cmdAllGLogData);
		((Control)this).Controls.Add((Control)(object)cmdAllSLogData);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)picGlogPhoto);
		((Control)this).Controls.Add((Control)(object)chkShowGlogPhoto);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)chkAndDelete);
		((Control)this).Controls.Add((Control)(object)chkReadMark);
		((Control)this).Controls.Add((Control)(object)LabelTotal);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)gridSLogData1);
		((Control)this).Controls.Add((Control)(object)gridSLogData2);
		((Control)this).Name = "FormlogNew";
		((Control)this).Text = "FormlogNew";
		((Form)this).FormClosing += new FormClosingEventHandler(FormlogNew_FormClosing);
		((Form)this).Load += FormlogNew_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)picGlogPhoto).EndInit();
		((ISupportInitialize)gridSLogData).EndInit();
		((ISupportInitialize)gridSLogData1).EndInit();
		((ISupportInitialize)gridSLogData2).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
