using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EntityCache.Business.Accounting;
using FormCustom.UC;
using PacketParser.Entities;

namespace FormCustom.Forms;

public class frmHashedPassword : AccountingBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	private UcGroupBox ucGroupBox1;

	internal TextBox txtPassword;

	internal Label Label1;

	internal Label label2;

	internal TextBox txtPasswordRepeat;

	private UcButtons btnOk;

	private UcButtons btnCancel;

	public Guid Guid { get; private set; }

	public byte[] HashPassword { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 27);
		}
	}

	public frmHashedPassword(string contextGuid, Guid guid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			Guid = guid;
			((Control)txtPassword).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 38);
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 48);
		}
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (string.IsNullOrEmpty(((Control)txtPassword).Text))
			{
				await this.ShowErrorAsync("لطفا کلمه عبور را وارد نمایید");
				return;
			}
			if (string.IsNullOrEmpty(((Control)txtPasswordRepeat).Text))
			{
				await this.ShowErrorAsync("لطفا تکرار کلمه عبور را وارد نمایید");
				return;
			}
			if (((Control)txtPassword).Text != ((Control)txtPasswordRepeat).Text)
			{
				await this.ShowErrorAsync("کلمه عبور و تکرار آن با هم همخوانی ندارند");
				return;
			}
			HashPassword = CashierBusiness.CalcHashedPassword(((Control)txtPassword).Text, Guid);
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 74);
		}
	}

	private void frmHashedPassword_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmHashedPassword_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 86);
		}
	}

	private void frmHashedPassword_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void txtPassword_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				((Control)txtPasswordRepeat).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtPassword_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmHashedPassword.cs", 96);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		ucGroupBox1 = new UcGroupBox();
		txtPassword = new TextBox();
		Label1 = new Label();
		label2 = new Label();
		txtPasswordRepeat = new TextBox();
		btnOk = new UcButtons();
		btnCancel = new UcButtons();
		((Control)this).SuspendLayout();
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.IsShowHeader = true;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(373, 231);
		((Control)ucGroupBox1).TabIndex = 4;
		ucGroupBox1.Title = "[دریافت کلمه عبور]";
		((Control)txtPassword).Anchor = (AnchorStyles)13;
		((Control)txtPassword).Font = new Font("B Yekan", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtPassword).Location = new Point(12, 75);
		((Control)txtPassword).Name = "txtPassword";
		((Control)txtPassword).Size = new Size(345, 27);
		((Control)txtPassword).TabIndex = 0;
		txtPassword.TextAlign = (HorizontalAlignment)2;
		txtPassword.UseSystemPasswordChar = true;
		((Control)txtPassword).KeyDown += new KeyEventHandler(txtPassword_KeyDown);
		((Control)Label1).Anchor = (AnchorStyles)9;
		((Control)Label1).AutoSize = true;
		((Control)Label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)Label1).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)Label1).Location = new Point(215, 52);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(146, 20);
		((Control)Label1).TabIndex = 5;
		((Control)Label1).Text = "[لطفا کلمه عبور را وارد نمایید]";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label2).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)label2).Location = new Point(186, 115);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(175, 20);
		((Control)label2).TabIndex = 5;
		((Control)label2).Text = "[لطفا تکرار کلمه عبور را وارد نمایید]";
		((Control)txtPasswordRepeat).Anchor = (AnchorStyles)13;
		((Control)txtPasswordRepeat).Font = new Font("B Yekan", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtPasswordRepeat).Location = new Point(12, 138);
		((Control)txtPasswordRepeat).Name = "txtPasswordRepeat";
		((Control)txtPasswordRepeat).Size = new Size(345, 27);
		((Control)txtPasswordRepeat).TabIndex = 1;
		txtPasswordRepeat.TextAlign = (HorizontalAlignment)2;
		txtPasswordRepeat.UseSystemPasswordChar = true;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = EnButtonCornerType.Round;
		btnOk.DisplayText = "[تایید] (F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(222, 190);
		((Control)btnOk).Margin = new Padding(4, 5, 4, 5);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(135, 30);
		((Control)btnOk).TabIndex = 2;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		btnCancel.DisplayText = "[انصراف] (Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(12, 190);
		((Control)btnCancel).Margin = new Padding(4, 5, 4, 5);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(135, 30);
		((Control)btnCancel).TabIndex = 3;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(373, 231);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Controls.Add((Control)(object)txtPasswordRepeat);
		((Control)this).Controls.Add((Control)(object)txtPassword);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmHashedPassword";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmHashedPassword_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmHashedPassword_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
