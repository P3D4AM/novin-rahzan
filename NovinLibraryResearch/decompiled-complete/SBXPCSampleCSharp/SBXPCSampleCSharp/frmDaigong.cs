using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AxSBXPCLib;

namespace SBXPCSampleCSharp;

public class frmDaigong : Form
{
	private AxSBXPC bpc;

	private const int PROXY_COUNT = 20;

	private string[] proxyNameList;

	private IContainer components = null;

	public Button cmdExit;

	public Button cmdWrite;

	public Button cmdRead;

	private ListBox lstProxy;

	public Button cmdUpdate;

	public TextBox txtProxy;

	public Label lblEnrollData;

	public Label lblMessage;

	public frmDaigong(AxSBXPC axSbxpc)
	{
		InitializeComponent();
		bpc = axSbxpc;
	}

	private void DrawDepartmentList()
	{
		lstProxy.Items.Clear();
		for (int i = 0; i < 20; i++)
		{
			string text = "[No.]" + $"{i:D2}" + " ";
			text = text + "[Name]" + proxyNameList[i];
			lstProxy.Items.Add((object)text);
		}
	}

	private void lstDepartment_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)txtProxy).Text = proxyNameList[((ListControl)lstProxy).SelectedIndex];
		((Control)txtProxy).Focus();
	}

	private void cmdUpdate_Click(object sender, EventArgs e)
	{
		proxyNameList[((ListControl)lstProxy).SelectedIndex] = ((Control)txtProxy).Text;
		DrawDepartmentList();
	}

	private void frmDaigong_FormClosing(object sender, FormClosingEventArgs e)
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
			flag = bpc.GetDepartName(util.gMachineNumber, i + 1, 1, ref proxyNameList[i]) && flag;
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
			flag = bpc.SetDepartName(util.gMachineNumber, i + 1, 1, ref proxyNameList[i]) && flag;
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

	private void frmDaigong_Load(object sender, EventArgs e)
	{
		proxyNameList = new string[20];
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Expected O, but got Unknown
		cmdExit = new Button();
		cmdWrite = new Button();
		cmdRead = new Button();
		lstProxy = new ListBox();
		cmdUpdate = new Button();
		txtProxy = new TextBox();
		lblEnrollData = new Label();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		((Control)cmdExit).BackColor = SystemColors.Control;
		((Control)cmdExit).Cursor = Cursors.Default;
		((Control)cmdExit).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdExit).ForeColor = SystemColors.ControlText;
		((Control)cmdExit).Location = new Point(555, 462);
		((Control)cmdExit).Name = "cmdExit";
		((Control)cmdExit).RightToLeft = (RightToLeft)0;
		((Control)cmdExit).Size = new Size(104, 46);
		((Control)cmdExit).TabIndex = 68;
		((Control)cmdExit).Text = "Exit";
		((ButtonBase)cmdExit).UseVisualStyleBackColor = false;
		((Control)cmdExit).Click += cmdExit_Click;
		((Control)cmdWrite).BackColor = SystemColors.Control;
		((Control)cmdWrite).Cursor = Cursors.Default;
		((Control)cmdWrite).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdWrite).ForeColor = SystemColors.ControlText;
		((Control)cmdWrite).Location = new Point(555, 398);
		((Control)cmdWrite).Name = "cmdWrite";
		((Control)cmdWrite).RightToLeft = (RightToLeft)0;
		((Control)cmdWrite).Size = new Size(104, 46);
		((Control)cmdWrite).TabIndex = 67;
		((Control)cmdWrite).Text = "Write";
		((ButtonBase)cmdWrite).UseVisualStyleBackColor = false;
		((Control)cmdWrite).Click += cmdWrite_Click;
		((Control)cmdRead).BackColor = SystemColors.Control;
		((Control)cmdRead).Cursor = Cursors.Default;
		((Control)cmdRead).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdRead).ForeColor = SystemColors.ControlText;
		((Control)cmdRead).Location = new Point(555, 333);
		((Control)cmdRead).Name = "cmdRead";
		((Control)cmdRead).RightToLeft = (RightToLeft)0;
		((Control)cmdRead).Size = new Size(104, 46);
		((Control)cmdRead).TabIndex = 66;
		((Control)cmdRead).Text = "Read";
		((ButtonBase)cmdRead).UseVisualStyleBackColor = false;
		((Control)cmdRead).Click += cmdRead_Click;
		((Control)lstProxy).Font = new Font("Times New Roman", 12f);
		((ListControl)lstProxy).FormattingEnabled = true;
		lstProxy.ItemHeight = 19;
		((Control)lstProxy).Location = new Point(27, 97);
		((Control)lstProxy).Name = "lstProxy";
		((Control)lstProxy).Size = new Size(510, 422);
		((Control)lstProxy).TabIndex = 65;
		lstProxy.SelectedIndexChanged += lstDepartment_SelectedIndexChanged;
		((Control)cmdUpdate).BackColor = SystemColors.Control;
		((Control)cmdUpdate).Cursor = Cursors.Default;
		((Control)cmdUpdate).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdUpdate).ForeColor = SystemColors.ControlText;
		((Control)cmdUpdate).Location = new Point(555, 97);
		((Control)cmdUpdate).Name = "cmdUpdate";
		((Control)cmdUpdate).RightToLeft = (RightToLeft)0;
		((Control)cmdUpdate).Size = new Size(104, 46);
		((Control)cmdUpdate).TabIndex = 64;
		((Control)cmdUpdate).Text = "Update";
		((ButtonBase)cmdUpdate).UseVisualStyleBackColor = false;
		((Control)cmdUpdate).Click += cmdUpdate_Click;
		txtProxy.AcceptsReturn = true;
		((Control)txtProxy).BackColor = SystemColors.Window;
		((Control)txtProxy).Cursor = Cursors.IBeam;
		((Control)txtProxy).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtProxy).ForeColor = SystemColors.WindowText;
		((Control)txtProxy).Location = new Point(148, 52);
		((TextBoxBase)txtProxy).MaxLength = 12;
		((Control)txtProxy).Name = "txtProxy";
		((Control)txtProxy).RightToLeft = (RightToLeft)0;
		((Control)txtProxy).Size = new Size(248, 26);
		((Control)txtProxy).TabIndex = 63;
		((Control)lblEnrollData).AutoSize = true;
		((Control)lblEnrollData).BackColor = SystemColors.Control;
		((Control)lblEnrollData).Cursor = Cursors.Default;
		((Control)lblEnrollData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollData).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollData).Location = new Point(27, 55);
		((Control)lblEnrollData).Name = "lblEnrollData";
		((Control)lblEnrollData).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollData).Size = new Size(89, 19);
		((Control)lblEnrollData).TabIndex = 62;
		((Control)lblEnrollData).Text = "Proxy Name:";
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(27, 6);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(632, 28);
		((Control)lblMessage).TabIndex = 61;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(686, 534);
		((Control)this).Controls.Add((Control)(object)cmdExit);
		((Control)this).Controls.Add((Control)(object)cmdWrite);
		((Control)this).Controls.Add((Control)(object)cmdRead);
		((Control)this).Controls.Add((Control)(object)lstProxy);
		((Control)this).Controls.Add((Control)(object)cmdUpdate);
		((Control)this).Controls.Add((Control)(object)txtProxy);
		((Control)this).Controls.Add((Control)(object)lblEnrollData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Name = "frmDaigong";
		((Control)this).Text = "frmDaigong";
		((Form)this).Load += frmDaigong_Load;
		((Form)this).FormClosing += new FormClosingEventHandler(frmDaigong_FormClosing);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
