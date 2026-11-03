using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.Froms.LkManage;

public class FRM_Activation_Barcod_Tolid_Special : AccountingBaseForm
{
	private bool _isDisposed;

	private Container components;

	internal Button Btn_Ok;

	internal Label Label1;

	internal Label Label2;

	internal TextBox Txt_Tolid;

	internal TextBox TxT_Barcode;

	internal Button Btn_ecp;

	internal Panel Panel1;

	internal Label Label3;

	internal PictureBox Pbox_tolid;

	internal PictureBox Pbox_barcode;

	internal Label Label4;

	internal PictureBox Pbox_Special;

	internal TextBox TXT_Special;

	internal Label Label5;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FRM_Activation_Barcod_Tolid_Special(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	public void Btn_ecp_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	public void TxT_Barcode_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_barcode).Visible = true;
			if (((Control)TxT_Barcode).Text.Length != 16)
			{
				Pbox_barcode.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_barcode.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxT_Barcode_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRM_Activation_Barcod_Tolid_Special.cs", 30);
		}
	}

	public void Txt_Tolid_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_tolid).Visible = true;
			if (((Control)Txt_Tolid).Text.Length != 16)
			{
				Pbox_tolid.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_tolid.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Txt_Tolid_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRM_Activation_Barcod_Tolid_Special.cs", 43);
		}
	}

	public void TXT_Special_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_Special).Visible = true;
			if (((Control)TXT_Special).Text.Length != 16)
			{
				Pbox_Special.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_Special.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXT_Special_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRM_Activation_Barcod_Tolid_Special.cs", 56);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRM_Activation_Barcod_Tolid_Special.cs", 72);
		}
	}

	private void FRM_Activation_Barcod_Tolid_Special_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			((Component)(object)this).Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRM_Activation_Barcod_Tolid_Special_FormClosed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRM_Activation_Barcod_Tolid_Special.cs", 81);
		}
	}

	[DebuggerStepThrough]
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		Btn_Ok = new Button();
		Label1 = new Label();
		Label2 = new Label();
		Txt_Tolid = new TextBox();
		TxT_Barcode = new TextBox();
		Btn_ecp = new Button();
		Panel1 = new Panel();
		Pbox_Special = new PictureBox();
		TXT_Special = new TextBox();
		Label5 = new Label();
		Pbox_tolid = new PictureBox();
		Pbox_barcode = new PictureBox();
		Label3 = new Label();
		Label4 = new Label();
		((Control)Panel1).SuspendLayout();
		((ISupportInitialize)Pbox_Special).BeginInit();
		((ISupportInitialize)Pbox_tolid).BeginInit();
		((ISupportInitialize)Pbox_barcode).BeginInit();
		((Control)this).SuspendLayout();
		Btn_Ok.DialogResult = (DialogResult)1;
		((Control)Btn_Ok).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)Btn_Ok).Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((ButtonBase)Btn_Ok).ImageAlign = (ContentAlignment)64;
		((Control)Btn_Ok).Location = new Point(106, 197);
		((Control)Btn_Ok).Name = "Btn_Ok";
		((Control)Btn_Ok).Size = new Size(92, 40);
		((Control)Btn_Ok).TabIndex = 3;
		((Control)Btn_Ok).Text = "[تائید]";
		((ButtonBase)Btn_Ok).TextAlign = (ContentAlignment)16;
		((ButtonBase)Btn_Ok).UseVisualStyleBackColor = true;
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).Location = new Point(293, 18);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)1;
		((Control)Label1).Size = new Size(178, 13);
		((Control)Label1).TabIndex = 1;
		((Control)Label1).Text = "[کد فعال  سازی زیر بسته ی بارکد]:";
		((Control)Label2).AutoSize = true;
		((Control)Label2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label2).Location = new Point(288, 52);
		((Control)Label2).Name = "Label2";
		((Control)Label2).RightToLeft = (RightToLeft)1;
		((Control)Label2).Size = new Size(183, 13);
		((Control)Label2).TabIndex = 2;
		((Control)Label2).Text = "[کد فعال  سازی زیر بسته ی تولید] : ";
		((Control)Txt_Tolid).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Txt_Tolid).Location = new Point(56, 43);
		((Control)Txt_Tolid).Name = "Txt_Tolid";
		((Control)Txt_Tolid).RightToLeft = (RightToLeft)0;
		((Control)Txt_Tolid).Size = new Size(191, 33);
		((Control)Txt_Tolid).TabIndex = 2;
		((Control)Txt_Tolid).TextChanged += Txt_Tolid_TextChanged;
		((Control)TxT_Barcode).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)TxT_Barcode).Location = new Point(56, 9);
		((Control)TxT_Barcode).Name = "TxT_Barcode";
		((Control)TxT_Barcode).RightToLeft = (RightToLeft)0;
		((Control)TxT_Barcode).Size = new Size(191, 33);
		((Control)TxT_Barcode).TabIndex = 1;
		((Control)TxT_Barcode).TextChanged += TxT_Barcode_TextChanged;
		Btn_ecp.DialogResult = (DialogResult)2;
		((Control)Btn_ecp).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)Btn_ecp).Image = (Image)(object)FormCustom.Properties.Resources.stop;
		((ButtonBase)Btn_ecp).ImageAlign = (ContentAlignment)64;
		((Control)Btn_ecp).Location = new Point(8, 197);
		((Control)Btn_ecp).Name = "Btn_ecp";
		((Control)Btn_ecp).Size = new Size(92, 40);
		((Control)Btn_ecp).TabIndex = 4;
		((Control)Btn_ecp).Text = "[انصراف]";
		((ButtonBase)Btn_ecp).TextAlign = (ContentAlignment)16;
		((ButtonBase)Btn_ecp).UseVisualStyleBackColor = true;
		((Control)Btn_ecp).Click += Btn_ecp_Click;
		Panel1.BorderStyle = (BorderStyle)2;
		((Control)Panel1).Controls.Add((Control)(object)Pbox_Special);
		((Control)Panel1).Controls.Add((Control)(object)TXT_Special);
		((Control)Panel1).Controls.Add((Control)(object)Label5);
		((Control)Panel1).Controls.Add((Control)(object)Pbox_tolid);
		((Control)Panel1).Controls.Add((Control)(object)Pbox_barcode);
		((Control)Panel1).Controls.Add((Control)(object)TxT_Barcode);
		((Control)Panel1).Controls.Add((Control)(object)Txt_Tolid);
		((Control)Panel1).Controls.Add((Control)(object)Label2);
		((Control)Panel1).Controls.Add((Control)(object)Label1);
		((Control)Panel1).Location = new Point(8, 64);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(495, 127);
		((Control)Panel1).TabIndex = 6;
		((Control)Pbox_Special).Location = new Point(8, 79);
		((Control)Pbox_Special).Name = "Pbox_Special";
		((Control)Pbox_Special).Size = new Size(43, 32);
		Pbox_Special.TabIndex = 7;
		Pbox_Special.TabStop = false;
		((Control)TXT_Special).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)TXT_Special).Location = new Point(56, 78);
		((Control)TXT_Special).Name = "TXT_Special";
		((Control)TXT_Special).RightToLeft = (RightToLeft)0;
		((Control)TXT_Special).Size = new Size(191, 33);
		((Control)TXT_Special).TabIndex = 5;
		((Control)TXT_Special).TextChanged += TXT_Special_TextChanged;
		((Control)Label5).AutoSize = true;
		((Control)Label5).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label5).Location = new Point(247, 87);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)1;
		((Control)Label5).Size = new Size(224, 13);
		((Control)Label5).TabIndex = 6;
		((Control)Label5).Text = "[کد فعال  سازی زیر بسته ی امکانات ویژه ]: ";
		((Control)Pbox_tolid).Location = new Point(8, 44);
		((Control)Pbox_tolid).Name = "Pbox_tolid";
		((Control)Pbox_tolid).Size = new Size(43, 32);
		Pbox_tolid.TabIndex = 4;
		Pbox_tolid.TabStop = false;
		((Control)Pbox_barcode).Location = new Point(8, 9);
		((Control)Pbox_barcode).Name = "Pbox_barcode";
		((Control)Pbox_barcode).Size = new Size(43, 32);
		Pbox_barcode.TabIndex = 3;
		Pbox_barcode.TabStop = false;
		((Control)Label3).AutoSize = true;
		((Control)Label3).Font = new Font("B Titr", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).ForeColor = Color.Red;
		((Control)Label3).Location = new Point(27, 9);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(403, 26);
		((Control)Label3).TabIndex = 7;
		((Control)Label3).Text = "[در صورتی که کد فعال سازی زیر بسته های تولید، بارکد و امکانات ویژه را] ";
		((Control)Label4).AutoSize = true;
		((Control)Label4).Font = new Font("B Titr", 11.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label4).ForeColor = Color.Red;
		((Control)Label4).Location = new Point(106, 35);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(318, 26);
		((Control)Label4).TabIndex = 8;
		((Control)Label4).Text = "[از طریق پیامک در یافت کرده اید در قسمت ذیل وارد کنید]";
		((Form)this).AcceptButton = (IButtonControl)(object)Btn_Ok;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)Btn_ecp;
		((Form)this).ClientSize = new Size(510, 241);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)Panel1);
		((Control)this).Controls.Add((Control)(object)Btn_ecp);
		((Control)this).Controls.Add((Control)(object)Btn_Ok);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRM_Activation_Barcod_Tolid_Special";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فعال سازی زیر بسته های تولید و بارکد امکانات ویژه]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRM_Activation_Barcod_Tolid_Special_FormClosed);
		((Control)Panel1).ResumeLayout(false);
		((Control)Panel1).PerformLayout();
		((ISupportInitialize)Pbox_Special).EndInit();
		((ISupportInitialize)Pbox_tolid).EndInit();
		((ISupportInitialize)Pbox_barcode).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
