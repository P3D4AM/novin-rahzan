using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class ToolbarName : Form
{
	public bool RenameDialog;

	public TextBox txtName;

	private Label label1;

	private Button cmdOK;

	private Button cmdCancel;

	private Container components;

	public ToolbarName()
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		cmdOK = new Button();
		cmdCancel = new Button();
		label1 = new Label();
		txtName = new TextBox();
		((Control)this).SuspendLayout();
		cmdOK.DialogResult = (DialogResult)1;
		((ButtonBase)cmdOK).FlatStyle = (FlatStyle)3;
		((Control)cmdOK).Location = new Point(113, 56);
		((Control)cmdOK).Name = "cmdOK";
		((Control)cmdOK).Size = new Size(80, 24);
		((Control)cmdOK).TabIndex = 2;
		((Control)cmdOK).Text = "barname_ok";
		((Control)cmdCancel).CausesValidation = false;
		cmdCancel.DialogResult = (DialogResult)2;
		((ButtonBase)cmdCancel).FlatStyle = (FlatStyle)3;
		((Control)cmdCancel).Location = new Point(197, 56);
		((Control)cmdCancel).Name = "cmdCancel";
		((Control)cmdCancel).Size = new Size(80, 24);
		((Control)cmdCancel).TabIndex = 3;
		((Control)cmdCancel).Text = "barname_cancel";
		((Control)label1).Location = new Point(8, 8);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(272, 16);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "barname_name";
		((Control)txtName).Location = new Point(8, 28);
		((Control)txtName).Name = "txtName";
		((Control)txtName).Size = new Size(268, 20);
		((Control)txtName).TabIndex = 1;
		((Control)txtName).Text = "";
		((Control)txtName).Validating += OnOK;
		((Form)this).AcceptButton = (IButtonControl)(object)cmdOK;
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).CancelButton = (IButtonControl)(object)cmdCancel;
		((Form)this).ClientSize = new Size(286, 87);
		((Control)this).Controls.AddRange((Control[])(object)new Control[4]
		{
			(Control)cmdCancel,
			(Control)cmdOK,
			(Control)label1,
			(Control)txtName
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "ToolbarName";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "barname_caption";
		((Form)this).Load += OnLoad;
		((Control)this).ResumeLayout(false);
	}

	private void OnLoad(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		DotNetBarManager manager = null;
		if (((Form)this).Owner is frmCustomize)
		{
			manager = ((frmCustomize)(object)((Form)this).Owner).GetDotNetBarManager();
		}
		using (LocalizationManager localizationManager = new LocalizationManager(manager))
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (!(val is TextBox))
				{
					val.Text = localizationManager.GetLocalizedString(val.Text);
				}
			}
			if (RenameDialog)
			{
				((Control)this).Text = localizationManager.GetLocalizedString(LocalizationKeys.RenameBarDialogCaption);
			}
			else
			{
				((Control)this).Text = localizationManager.GetLocalizedString(((Control)this).Text);
			}
		}
		manager = null;
	}

	private void OnOK(object sender, CancelEventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)txtName).Text.Trim() == "")
		{
			DotNetBarManager manager = null;
			if (((Form)this).Owner is frmCustomize)
			{
				manager = ((frmCustomize)(object)((Form)this).Owner).GetDotNetBarManager();
			}
			using (LocalizationManager localizationManager = new LocalizationManager(manager))
			{
				MessageBox.Show(localizationManager.GetLocalizedString(LocalizationKeys.BarEditDialogInvalidNameMessage));
			}
			manager = null;
			e.Cancel = true;
		}
	}
}
