using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmDepartment : Form
{
	private AxSBXPC bpc;

	private const int DEPT_COUNT = 20;

	private string[] departmentNameList;

	private IContainer components = null;

	public Label lblMessage;

	public TextBox txtDeparmtmentName;

	public Label lblEnrollData;

	public Button cmdUpdate;

	private ListBox lstDepartment;

	public Button cmdRead;

	public Button cmdWrite;

	public Button cmdExit;

	public frmDepartment(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void frmDepartment_Load(object sender, EventArgs e)
	{
		departmentNameList = new string[20];
		DrawDepartmentList();
	}

	private void DrawDepartmentList()
	{
		lstDepartment.Items.Clear();
		for (int i = 0; i < 20; i++)
		{
			string text = "[No.]" + $"{i:D2}" + " ";
			text = text + "[Name]" + departmentNameList[i];
			lstDepartment.Items.Add((object)text);
		}
	}

	private void lstDepartment_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)txtDeparmtmentName).Text = departmentNameList[((ListControl)lstDepartment).SelectedIndex];
		((Control)txtDeparmtmentName).Focus();
	}

	private void cmdUpdate_Click(object sender, EventArgs e)
	{
		if (((ListControl)lstDepartment).SelectedIndex != -1)
		{
			departmentNameList[((ListControl)lstDepartment).SelectedIndex] = ((Control)txtDeparmtmentName).Text;
			DrawDepartmentList();
		}
	}

	private void frmDepartment_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void cmdExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void cmdRead_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		bool flag = bpc.EnableDevice(util.gMachineNumber, 0);
		if (!flag)
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			flag = bpc.GetDepartName(util.gMachineNumber, i, 0, ref departmentNameList[i]) && flag;
		}
		if (flag)
		{
			((Control)lblMessage).Text = "Success";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawDepartmentList();
	}

	private void cmdWrite_Click(object sender, EventArgs e)
	{
		int dwErrorCode = 0;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		bool flag = bpc.EnableDevice(util.gMachineNumber, 0);
		if (!flag)
		{
			((Control)lblMessage).Text = util.gstrNoDevice;
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			flag = bpc.SetDepartName(util.gMachineNumber, i, 0, ref departmentNameList[i]) && flag;
		}
		if (flag)
		{
			((Control)lblMessage).Text = "Success";
		}
		else
		{
			bpc.GetLastError(ref dwErrorCode);
			((Control)lblMessage).Text = util.ErrorPrint(dwErrorCode);
		}
		flag = bpc.EnableDevice(util.gMachineNumber, 1);
		DrawDepartmentList();
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
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Expected O, but got Unknown
		lblMessage = new Label();
		txtDeparmtmentName = new TextBox();
		lblEnrollData = new Label();
		cmdUpdate = new Button();
		lstDepartment = new ListBox();
		cmdRead = new Button();
		cmdWrite = new Button();
		cmdExit = new Button();
		((Control)this).SuspendLayout();
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(12, 9);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(632, 28);
		((Control)lblMessage).TabIndex = 29;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		txtDeparmtmentName.AcceptsReturn = true;
		((Control)txtDeparmtmentName).BackColor = SystemColors.Window;
		((Control)txtDeparmtmentName).Cursor = Cursors.IBeam;
		((Control)txtDeparmtmentName).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtDeparmtmentName).ForeColor = SystemColors.WindowText;
		((Control)txtDeparmtmentName).Location = new Point(154, 55);
		((TextBoxBase)txtDeparmtmentName).MaxLength = 12;
		((Control)txtDeparmtmentName).Name = "txtDeparmtmentName";
		((Control)txtDeparmtmentName).RightToLeft = (RightToLeft)0;
		((Control)txtDeparmtmentName).Size = new Size(248, 26);
		((Control)txtDeparmtmentName).TabIndex = 41;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(12, 58);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(124, 19);
		((Control)lblEnrollData).TabIndex = 40;
		((Control)lblEnrollData).Text = "Department Name:";
		((Control)cmdUpdate).BackColor = SystemColors.Control;
		((Control)cmdUpdate).Cursor = Cursors.Default;
		((Control)cmdUpdate).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUpdate).ForeColor = SystemColors.ControlText;
		((Control)cmdUpdate).Location = new Point(540, 100);
		((Control)cmdUpdate).Name = "cmdUpdate";
		((Control)cmdUpdate).RightToLeft = (RightToLeft)0;
		((Control)cmdUpdate).Size = new Size(104, 46);
		((Control)cmdUpdate).TabIndex = 56;
		((Control)cmdUpdate).Text = "Update";
		((ButtonBase)cmdUpdate).UseVisualStyleBackColor = false;
		((Control)cmdUpdate).Click += cmdUpdate_Click;
		((Control)lstDepartment).Font = new Font("Times New Roman", 12f);
		((ListControl)lstDepartment).FormattingEnabled = true;
		lstDepartment.ItemHeight = 19;
		((Control)lstDepartment).Location = new Point(12, 100);
		((Control)lstDepartment).Name = "lstDepartment";
		((Control)lstDepartment).Size = new Size(510, 422);
		((Control)lstDepartment).TabIndex = 57;
		lstDepartment.SelectedIndexChanged += lstDepartment_SelectedIndexChanged;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(540, 336);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(104, 46);
		((Control)cmdRead).TabIndex = 58;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(540, 401);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(104, 46);
		((Control)cmdWrite).TabIndex = 59;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(540, 465);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(104, 46);
		((Control)cmdExit).TabIndex = 60;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(656, 540);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)lstDepartment);
		((Control)this).Controls.Add((Control)(object)cmdUpdate);
		((Control)this).Controls.Add((Control)(object)txtDeparmtmentName);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmDepartment";
		((Control)this).Text = "frmDepartment";
		((Form)this).Load += frmDepartment_Load;
		((Form)this).FormClosing += new FormClosingEventHandler(frmDepartment_FormClosing);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
