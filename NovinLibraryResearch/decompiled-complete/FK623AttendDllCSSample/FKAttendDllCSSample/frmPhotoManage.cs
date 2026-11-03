using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AxAXIMAGELib;

namespace FKAttendDllCSSample;

public class frmPhotoManage : Form
{
	private int mnCommHandleIndex;

	private byte[] mbytPhotoImage;

	private IContainer components = null;

	public TextBox txtEnrollNumber;

	public Button cmdGetEnrollPhoto;

	public ToolTip ToolTip1;

	public Button cmdSetEnrollPhoto;

	public Button cmdDeleteEnrollPhoto;

	public Label lblMessage;

	public Label lblEnrollNum;

	private AxAxImage axAxImage1;

	public frmPhotoManage()
	{
		InitializeComponent();
	}

	private void frmPhotoManage_Load(object sender, EventArgs e)
	{
		mnCommHandleIndex = FKAttendDLL.nCommHandleIndex;
	}

	private void cmdGetEnrollPhoto_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		num2 = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		((Control)cmdGetEnrollPhoto).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		num = FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdGetEnrollPhoto).Enabled = true;
			return;
		}
		mbytPhotoImage = new byte[0];
		num3 = 0;
		num = FKAttendDLL.FK_GetEnrollPhoto(mnCommHandleIndex, num2, mbytPhotoImage, ref num3);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
			FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 1);
			((Control)cmdGetEnrollPhoto).Enabled = true;
			return;
		}
		mbytPhotoImage = new byte[num3];
		num = FKAttendDLL.FK_GetEnrollPhoto(mnCommHandleIndex, num2, mbytPhotoImage, ref num3);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		else
		{
			((Control)lblMessage).Text = "GetEnrollPhoto OK";
			SaveEnrollPhoto();
		}
		FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 1);
		((Control)cmdGetEnrollPhoto).Enabled = true;
	}

	private void SaveEnrollPhoto()
	{
		string text = "";
		if (mbytPhotoImage.Length >= 1)
		{
			text = Directory.GetCurrentDirectory() + "\\EnrollPhoto.jpg";
			File.WriteAllBytes(text, mbytPhotoImage);
			object obj = new VariantWrapper(mbytPhotoImage);
			axAxImage1.SetJpgImage(ref obj);
		}
	}

	private void cmdSetEnrollPhoto_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		num3 = mbytPhotoImage.Length;
		if (num3 < 1)
		{
			return;
		}
		num2 = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		((Control)cmdSetEnrollPhoto).Enabled = false;
		((Control)lblMessage).Text = "Writing Photo...";
		Application.DoEvents();
		num = FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdSetEnrollPhoto).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_SetEnrollPhoto(mnCommHandleIndex, num2, mbytPhotoImage, num3);
		if (num == 1)
		{
			((Control)lblMessage).Text = "SetEnrollPhoto OK";
		}
		else
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 1);
		((Control)cmdSetEnrollPhoto).Enabled = true;
	}

	private void cmdDeleteEnrollPhoto_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = 0;
		num2 = Convert.ToInt32(((Control)txtEnrollNumber).Text);
		((Control)cmdDeleteEnrollPhoto).Enabled = false;
		((Control)lblMessage).Text = "Working...";
		Application.DoEvents();
		num = FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 0);
		if (num != 1)
		{
			((Control)lblMessage).Text = "No Device";
			((Control)cmdDeleteEnrollPhoto).Enabled = true;
			return;
		}
		num = FKAttendDLL.FK_DeleteEnrollPhoto(mnCommHandleIndex, num2);
		if (num != 1)
		{
			((Control)lblMessage).Text = FKAttendDLL.ReturnResultPrint(num);
		}
		else
		{
			((Control)lblMessage).Text = "Delete Photo OK";
		}
		FKAttendDLL.FK_EnableDevice(mnCommHandleIndex, 1);
		((Control)cmdDeleteEnrollPhoto).Enabled = true;
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
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
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmPhotoManage));
		txtEnrollNumber = new TextBox();
		cmdGetEnrollPhoto = new Button();
		ToolTip1 = new ToolTip(components);
		cmdSetEnrollPhoto = new Button();
		cmdDeleteEnrollPhoto = new Button();
		lblMessage = new Label();
		lblEnrollNum = new Label();
		axAxImage1 = new AxAxImage();
		((ISupportInitialize)axAxImage1).BeginInit();
		((Control)this).SuspendLayout();
		txtEnrollNumber.AcceptsReturn = true;
		((Control)txtEnrollNumber).BackColor = SystemColors.Window;
		((Control)txtEnrollNumber).Cursor = Cursors.IBeam;
		((Control)txtEnrollNumber).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)txtEnrollNumber).ForeColor = SystemColors.WindowText;
		((Control)txtEnrollNumber).Location = new Point(149, 59);
		((TextBoxBase)txtEnrollNumber).MaxLength = 8;
		((Control)txtEnrollNumber).Name = "txtEnrollNumber";
		((Control)txtEnrollNumber).RightToLeft = (RightToLeft)0;
		((Control)txtEnrollNumber).Size = new Size(81, 26);
		((Control)txtEnrollNumber).TabIndex = 10;
		((Control)cmdGetEnrollPhoto).BackColor = SystemColors.Control;
		((Control)cmdGetEnrollPhoto).Cursor = Cursors.Default;
		((Control)cmdGetEnrollPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdGetEnrollPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdGetEnrollPhoto).Location = new Point(245, 59);
		((Control)cmdGetEnrollPhoto).Name = "cmdGetEnrollPhoto";
		((Control)cmdGetEnrollPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdGetEnrollPhoto).Size = new Size(200, 30);
		((Control)cmdGetEnrollPhoto).TabIndex = 7;
		((Control)cmdGetEnrollPhoto).Text = "Get Enroll Photo";
		ToolTip1.SetToolTip((Control)(object)cmdGetEnrollPhoto, "Get EnrollData From Device");
		((ButtonBase)cmdGetEnrollPhoto).UseVisualStyleBackColor = false;
		((Control)cmdGetEnrollPhoto).Click += cmdGetEnrollPhoto_Click;
		((Control)cmdSetEnrollPhoto).BackColor = SystemColors.Control;
		((Control)cmdSetEnrollPhoto).Cursor = Cursors.Default;
		((Control)cmdSetEnrollPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdSetEnrollPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdSetEnrollPhoto).Location = new Point(245, 90);
		((Control)cmdSetEnrollPhoto).Name = "cmdSetEnrollPhoto";
		((Control)cmdSetEnrollPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdSetEnrollPhoto).Size = new Size(200, 30);
		((Control)cmdSetEnrollPhoto).TabIndex = 9;
		((Control)cmdSetEnrollPhoto).Text = "Set Enroll Photo";
		ToolTip1.SetToolTip((Control)(object)cmdSetEnrollPhoto, "Set EnrollData To Device");
		((ButtonBase)cmdSetEnrollPhoto).UseVisualStyleBackColor = false;
		((Control)cmdSetEnrollPhoto).Click += cmdSetEnrollPhoto_Click;
		((Control)cmdDeleteEnrollPhoto).BackColor = SystemColors.Control;
		((Control)cmdDeleteEnrollPhoto).Cursor = Cursors.Default;
		((Control)cmdDeleteEnrollPhoto).Font = new Font("Times New Roman", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)cmdDeleteEnrollPhoto).ForeColor = SystemColors.ControlText;
		((Control)cmdDeleteEnrollPhoto).Location = new Point(245, 122);
		((Control)cmdDeleteEnrollPhoto).Name = "cmdDeleteEnrollPhoto";
		((Control)cmdDeleteEnrollPhoto).RightToLeft = (RightToLeft)0;
		((Control)cmdDeleteEnrollPhoto).Size = new Size(200, 30);
		((Control)cmdDeleteEnrollPhoto).TabIndex = 8;
		((Control)cmdDeleteEnrollPhoto).Text = "Delete Enroll Photo";
		ToolTip1.SetToolTip((Control)(object)cmdDeleteEnrollPhoto, "Delete Enroll Data Into Device");
		((ButtonBase)cmdDeleteEnrollPhoto).UseVisualStyleBackColor = false;
		((Control)cmdDeleteEnrollPhoto).Click += cmdDeleteEnrollPhoto_Click;
		((Control)lblMessage).BackColor = SystemColors.Control;
		lblMessage.BorderStyle = (BorderStyle)2;
		((Control)lblMessage).Cursor = Cursors.Default;
		((Control)lblMessage).Font = new Font("Times New Roman", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblMessage).ForeColor = SystemColors.ControlText;
		((Control)lblMessage).Location = new Point(13, 5);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).RightToLeft = (RightToLeft)0;
		((Control)lblMessage).Size = new Size(437, 33);
		((Control)lblMessage).TabIndex = 12;
		((Control)lblMessage).Text = "Message";
		lblMessage.TextAlign = (ContentAlignment)2;
		((Control)lblEnrollNum).AutoSize = true;
		((Control)lblEnrollNum).BackColor = SystemColors.Control;
		((Control)lblEnrollNum).Cursor = Cursors.Default;
		((Control)lblEnrollNum).Font = new Font("Times New Roman", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)lblEnrollNum).ForeColor = SystemColors.ControlText;
		((Control)lblEnrollNum).Location = new Point(14, 66);
		((Control)lblEnrollNum).Name = "lblEnrollNum";
		((Control)lblEnrollNum).RightToLeft = (RightToLeft)0;
		((Control)lblEnrollNum).Size = new Size(105, 19);
		((Control)lblEnrollNum).TabIndex = 11;
		((Control)lblEnrollNum).Text = "Enroll Number :";
		((AxHost)axAxImage1).Enabled = true;
		((Control)axAxImage1).Location = new Point(21, 105);
		((Control)axAxImage1).Name = "axAxImage1";
		((AxHost)axAxImage1).OcxState = (State)componentResourceManager.GetObject("axAxImage1.OcxState");
		((Control)axAxImage1).Size = new Size(208, 237);
		((Control)axAxImage1).TabIndex = 13;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(466, 354);
		((Control)this).Controls.Add((Control)(object)axAxImage1);
		((Control)this).Controls.Add((Control)(object)txtEnrollNumber);
		((Control)this).Controls.Add((Control)(object)cmdGetEnrollPhoto);
		((Control)this).Controls.Add((Control)(object)cmdSetEnrollPhoto);
		((Control)this).Controls.Add((Control)(object)cmdDeleteEnrollPhoto);
		((Control)this).Controls.Add((Control)(object)lblMessage);
		((Control)this).Controls.Add((Control)(object)lblEnrollNum);
		((Control)this).Name = "frmPhotoManage";
		((Form)this).ShowIcon = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Photo Manage";
		((Form)this).Load += frmPhotoManage_Load;
		((ISupportInitialize)axAxImage1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
