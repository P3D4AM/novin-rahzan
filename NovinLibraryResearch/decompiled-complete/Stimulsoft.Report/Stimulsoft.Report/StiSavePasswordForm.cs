using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report;

public class StiSavePasswordForm : Form
{
	private IContainer components;

	private Label lbPassword;

	private TextBox tbPassword;

	private Button btOk;

	private Button btCancel;

	private ErrorProvider errorProvider;

	public string Password { get; private set; }

	public StiSavePasswordForm()
	{
		InitializeComponent();
		Localize();
		((Control)tbPassword).Text = string.Empty;
		((Control)tbPassword).Focus();
		((Control)tbPassword).Width = ((Control)btCancel).Right - ((Control)tbPassword).Left;
		((Control)lbPassword).Height = ((Control)tbPassword).Height;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Password", "StiSavePasswordForm");
		((Control)lbPassword).Text = Loc.GetCleaned("Password", "lbPasswordSave");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void OnOkClick(object sender, EventArgs e)
	{
		if (((Control)tbPassword).Text.Length == 0)
		{
			((Control)tbPassword).Width = 219;
			errorProvider.SetError((Control)(object)tbPassword, string.Format(StiLocalization.Get("Password", "PasswordNotEntered"), StiLocalization.Get("PropertyMain", "Alias")));
			((Control)tbPassword).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
			Password = ((Control)tbPassword).Text;
			((Form)this).Close();
		}
	}

	private void OnFormKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void OnCancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		components = new Container();
		lbPassword = new Label();
		tbPassword = new TextBox();
		btOk = new Button();
		btCancel = new Button();
		errorProvider = new ErrorProvider(components);
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lbPassword).Anchor = (AnchorStyles)6;
		((Control)lbPassword).ForeColor = Color.DimGray;
		((Control)lbPassword).Location = new Point(12, 17);
		((Control)lbPassword).Name = "lbPassword";
		((Control)lbPassword).Size = new Size(87, 20);
		((Control)lbPassword).TabIndex = 4;
		((Control)lbPassword).Text = "Password";
		lbPassword.TextAlign = (ContentAlignment)16;
		((Control)tbPassword).Anchor = (AnchorStyles)14;
		((Control)tbPassword).Location = new Point(105, 17);
		((Control)tbPassword).Name = "tbPassword";
		tbPassword.PasswordChar = '*';
		((Control)tbPassword).Size = new Size(179, 20);
		((Control)tbPassword).TabIndex = 0;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(128, 52);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += OnOkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(209, 52);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += OnCancelClick;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).AutoSizeMode = (AutoSizeMode)0;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(297, 89);
		((Control)this).Controls.Add((Control)(object)lbPassword);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)tbPassword);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSavePasswordForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "SavePasswordForm";
		((Control)this).KeyDown += new KeyEventHandler(OnFormKeyDown);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
