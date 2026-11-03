using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FKAttendDllCSSample;

public class frmProductData : Form
{
	public TextBox txtProductCode;

	public Button cmdGetData;

	public TextBox txtBackupNo;

	public TextBox txtSerialNo;

	public Label lblBackuplNo;

	public Label Label1;

	public Label lblSerialNo;

	public Label lblMessage;

	private Container components = null;

	public frmProductData()
	{
		InitializeComponent();
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
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		txtProductCode = new TextBox();
		cmdGetData = new Button();
		txtBackupNo = new TextBox();
		txtSerialNo = new TextBox();
		lblBackuplNo = new Label();
		Label1 = new Label();
		lblSerialNo = new Label();
		lblMessage = new Label();
		((Control)this).SuspendLayout();
		txtProductCode.AcceptsReturn = true;
		((Control)txtProductCode).BackColor = SystemColors.Window;
		((Control)txtProductCode).Cursor = Cursors.IBeam;
		((Control)txtProductCode).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtProductCode).ForeColor = SystemColors.WindowText;
		((Control)txtProductCode).Location = new Point(173, 132);
		((TextBoxBase)txtProductCode).MaxLength = 32;
		((Control)txtProductCode).Name = "txtProductCode";
		((Control)txtProductCode).RightToLeft = (RightToLeft)0;
		((Control)txtProductCode).Size = new Size(214, 28);
		((Control)txtProductCode).TabIndex = 19;
		((Control)cmdGetData).BackColor = SystemColors.Control;
		((Control)cmdGetData).Cursor = Cursors.Default;
		((Control)cmdGetData).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetData).ForeColor = SystemColors.ControlText;
		((Control)cmdGetData).Location = new Point(267, 173);
		((Control)cmdGetData).Name = "cmdGetData";
		((Control)cmdGetData).RightToLeft = (RightToLeft)0;
		((Control)cmdGetData).Size = new Size(121, 44);
		((Control)cmdGetData).TabIndex = 18;
		((Control)cmdGetData).Text = "Get";
		((ButtonBase)cmdGetData).UseVisualStyleBackColor = false;
		((Control)cmdGetData).Click += cmdGetData_Click;
		txtBackupNo.AcceptsReturn = true;
		((Control)txtBackupNo).BackColor = SystemColors.Window;
		((Control)txtBackupNo).Cursor = Cursors.IBeam;
		((Control)txtBackupNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtBackupNo).ForeColor = SystemColors.WindowText;
		((Control)txtBackupNo).Location = new Point(173, 92);
		((TextBoxBase)txtBackupNo).MaxLength = 32;
		((Control)txtBackupNo).Name = "txtBackupNo";
		((Control)txtBackupNo).RightToLeft = (RightToLeft)0;
		((Control)txtBackupNo).Size = new Size(214, 27);
		((Control)txtBackupNo).TabIndex = 17;
		txtSerialNo.AcceptsReturn = true;
		((Control)txtSerialNo).BackColor = SystemColors.Window;
		((Control)txtSerialNo).Cursor = Cursors.IBeam;
		((Control)txtSerialNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtSerialNo).ForeColor = SystemColors.WindowText;
		((Control)txtSerialNo).Location = new Point(173, 52);
		((TextBoxBase)txtSerialNo).MaxLength = 32;
		((Control)txtSerialNo).Name = "txtSerialNo";
		((Control)txtSerialNo).RightToLeft = (RightToLeft)0;
		((Control)txtSerialNo).Size = new Size(214, 27);
		((Control)txtSerialNo).TabIndex = 16;
		((Control)lblBackuplNo).AutoSize = true;
		((Control)lblBackuplNo).BackColor = SystemColors.Control;
		((Control)lblBackuplNo).Cursor = Cursors.Default;
		((Control)lblBackuplNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblBackuplNo).ForeColor = SystemColors.ControlText;
		((Control)lblBackuplNo).Location = new Point(37, 100);
		((Control)lblBackuplNo).Name = "lblBackuplNo";
		((Control)lblBackuplNo).RightToLeft = (RightToLeft)0;
		((Control)lblBackuplNo).Size = new Size(117, 19);
		((Control)lblBackuplNo).TabIndex = 23;
		((Control)lblBackuplNo).Text = "Backup Number :";
		lblBackuplNo.TextAlign = (ContentAlignment)4;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = SystemColors.Control;
		((Control)Label1).Cursor = Cursors.Default;
		((Control)Label1).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = SystemColors.ControlText;
		((Control)Label1).Location = new Point(37, 140);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)0;
		((Control)Label1).Size = new Size(102, 19);
		((Control)Label1).TabIndex = 22;
		((Control)Label1).Text = "Product Code :";
		Label1.TextAlign = (ContentAlignment)4;
		((Control)lblSerialNo).AutoSize = true;
		((Control)lblSerialNo).BackColor = SystemColors.Control;
		((Control)lblSerialNo).Cursor = Cursors.Default;
		((Control)lblSerialNo).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblSerialNo).ForeColor = SystemColors.ControlText;
		((Control)lblSerialNo).Location = new Point(37, 60);
		((Control)lblSerialNo).Name = "lblSerialNo";
		((Control)lblSerialNo).RightToLeft = (RightToLeft)0;
		((Control)lblSerialNo).Size = new Size(104, 19);
		((Control)lblSerialNo).TabIndex = 21;
		((Control)lblSerialNo).Text = "Serial Number :";
		lblSerialNo.TextAlign = (ContentAlignment)4;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(21, 12);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(367, 28);
		((Control)lblMessage).TabIndex = 20;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(413, 229);
		((Control)this).Controls.Add((Control)(object)txtProductCode);
		((Control)this).Controls.Add((Control)(object)txtBackupNo);
		((Control)this).Controls.Add((Control)(object)txtSerialNo);
		((Control)this).Controls.Add((Control)(object)lblBackuplNo);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)lblSerialNo);
		((Control)this).Controls.Add((Control)(object)cmdGetData);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmProductData";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Product Information";
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void cmdGetData_Click(object sender, EventArgs e)
	{
		string astrItem = "";
		((Control)cmdGetData).Enabled = false;
		((Control)txtSerialNo).Text = "";
		((Control)txtBackupNo).Text = "";
		((Control)txtProductCode).Text = "";
		((Control)lblMessage).Text = "Waiting...";
		Application.DoEvents();
		long num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetData).Enabled = true;
			return;
		}
		num = FuncGetProductData(1L, ref astrItem);
		if (num == 1)
		{
			((Control)txtSerialNo).Text = astrItem;
			num = FuncGetProductData(2L, ref astrItem);
			if (num == 1)
			{
				((Control)txtBackupNo).Text = astrItem;
				num = FuncGetProductData(3L, ref astrItem);
				if (num == 1)
				{
					((Control)txtProductCode).Text = astrItem;
				}
			}
		}
		if (num == 1)
		{
			((Control)lblMessage).Text = "GetProductData OK";
		}
		FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, 1);
		((Control)cmdGetData).Enabled = true;
	}

	private long FuncGetProductData(long anIndex, ref string astrItem)
	{
		string apstrValue = new string(' ', 256);
		long num = FKAttendDLL.FK_GetProductData(FKAttendDLL.nCommHandleIndex, (int)anIndex, ref apstrValue);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			return num;
		}
		astrItem = apstrValue.Trim();
		return num;
	}
}
