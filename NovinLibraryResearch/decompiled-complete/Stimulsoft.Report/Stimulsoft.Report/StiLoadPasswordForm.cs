using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report;

public class StiLoadPasswordForm : Form
{
	private string fileName = "";

	private IContainer components;

	private Button btOk;

	private Label lbPassword;

	private TextBox tbPassword;

	private Button btCancel;

	private ErrorProvider errorProvider;

	public string Password { get; private set; } = string.Empty;


	public StiLoadPasswordForm(string fileName = null)
	{
		InitializeComponent();
		this.fileName = fileName;
		Localize();
		((Control)tbPassword).Text = string.Empty;
		((Control)tbPassword).Focus();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Password", "StiLoadPasswordForm");
		string text = Loc.GetCleaned("Password", "lbPasswordLoad");
		if (!string.IsNullOrWhiteSpace(fileName))
		{
			text = text + " '" + Path.GetFileName(fileName) + "'";
		}
		((Control)lbPassword).Text = text;
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void OnOkClick(object sender, EventArgs e)
	{
		if (((Control)tbPassword).Text.Length == 0)
		{
			((Control)tbPassword).Width = 208;
			errorProvider.SetError((Control)(object)tbPassword, string.Format(StiLocalization.Get("Password", "PasswordNotEntered"), StiLocalization.Get("PropertyMain", "Alias")));
			((Control)tbPassword).Focus();
		}
		else
		{
			Password = ((Control)tbPassword).Text;
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void OnCancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Expected O, but got Unknown
		components = new Container();
		btOk = new Button();
		lbPassword = new Label();
		tbPassword = new TextBox();
		btCancel = new Button();
		errorProvider = new ErrorProvider(components);
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(128, 80);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += OnOkClick;
		((Control)lbPassword).Anchor = (AnchorStyles)13;
		((Control)lbPassword).ForeColor = Color.DimGray;
		((Control)lbPassword).Location = new Point(12, 12);
		((Control)lbPassword).Name = "lbPassword";
		((Control)lbPassword).Size = new Size(272, 30);
		((Control)lbPassword).TabIndex = 0;
		((Control)lbPassword).Text = "Password";
		((Control)tbPassword).Anchor = (AnchorStyles)14;
		((Control)tbPassword).Location = new Point(12, 45);
		((Control)tbPassword).Name = "tbPassword";
		tbPassword.PasswordChar = '*';
		((Control)tbPassword).Size = new Size(272, 20);
		((Control)tbPassword).TabIndex = 0;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(209, 80);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
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
		((Form)this).ClientSize = new Size(297, 117);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)lbPassword);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Control)this).Controls.Add((Control)(object)tbPassword);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiLoadPasswordForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "LoadPasswordForm";
		((Control)this).KeyDown += new KeyEventHandler(OnFormKeyDown);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
