using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataWorldDatabaseEditForm : StiFormUX
{
	private StiReport report;

	private string oldValue;

	private string originalName;

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	private StiLabelUX labelName;

	public StiTextBox tbName;

	private StiLabelUX labelOwner;

	public StiTextBox textBoxOwner;

	private StiLabelUX labelDatabase;

	public StiTextBox textBoxDatabase;

	private StiLabelUX labelToken;

	public StiTextBox textBoxToken;

	public StiCheckBoxUX checkBoxOAuthv2;

	private ErrorProvider errorProvider;

	public StiDataWorldDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			this.originalName = originalName;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			DoScaling();
		}
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelOwner).Text = Loc.Get("Cloud", "TextOwner");
		((Control)labelDatabase).Text = Loc.Get("QueryBuilder", "Database");
		((Control)labelToken).Text = Loc.Get("FormDatabaseEdit", "Token");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private bool IsEditMode()
	{
		return originalName != null;
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_connection.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			((Control)tbName).Focus();
		}
		else if (((Control)tbName).Text != originalName && StiNameCreation.IsConnectionNameExists(report, ((Control)tbName).Text))
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
			((Control)tbName).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void CheckBoxOAuthv2_CheckedChanged(object sender, EventArgs e)
	{
		((Control)textBoxToken).Enabled = !((CheckBox)checkBoxOAuthv2).Checked;
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)checkBoxOAuthv2).Visible = StiAccountSettings.CurrentAccount != null;
		((CheckBox)checkBoxOAuthv2).Checked = StiAccountSettings.CurrentAccount != null && string.IsNullOrEmpty(((Control)textBoxToken).Text);
		((Control)textBoxToken).Enabled = !((CheckBox)checkBoxOAuthv2).Checked;
		((CheckBox)checkBoxOAuthv2).CheckedChanged += CheckBoxOAuthv2_CheckedChanged;
		((Control)tbName).Focus();
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataWorldDatabaseEditForm));
		glButtons = new StiButtonPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		labelAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		labelOwner = new StiLabelUX();
		textBoxOwner = new StiTextBox();
		labelDatabase = new StiLabelUX();
		textBoxDatabase = new StiTextBox();
		labelToken = new StiLabelUX();
		textBoxToken = new StiTextBox();
		checkBoxOAuthv2 = new StiCheckBoxUX();
		errorProvider = new ErrorProvider(components);
		((Control)glButtons).SuspendLayout();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 207);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(545, 46);
		((Control)glButtons).TabIndex = 11;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(377, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(458, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(134, 20);
		((Control)labelAlias).TabIndex = 7;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(230, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(134, 20);
		((Control)labelName).TabIndex = 6;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(230, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)labelOwner).ForeColor = Color.DimGray;
		((Control)labelOwner).Location = new Point(12, 78);
		((Control)labelOwner).Name = "labelOwner";
		((Control)labelOwner).Size = new Size(134, 20);
		((Control)labelOwner).TabIndex = 8;
		((Control)labelOwner).Text = "Owner";
		((Label)labelOwner).TextAlign = (ContentAlignment)16;
		((Control)textBoxOwner).Anchor = (AnchorStyles)13;
		((Control)textBoxOwner).BackColor = SystemColors.Window;
		((Control)textBoxOwner).Location = new Point(152, 78);
		((Control)textBoxOwner).Name = "textBoxOwner";
		((Control)textBoxOwner).Size = new Size(371, 21);
		((Control)textBoxOwner).TabIndex = 2;
		((Control)labelDatabase).ForeColor = Color.DimGray;
		((Control)labelDatabase).Location = new Point(12, 111);
		((Control)labelDatabase).Name = "labelDataset";
		((Control)labelDatabase).Size = new Size(134, 20);
		((Control)labelDatabase).TabIndex = 9;
		((Control)labelDatabase).Text = "Dataset";
		((Label)labelDatabase).TextAlign = (ContentAlignment)16;
		((Control)textBoxDatabase).Anchor = (AnchorStyles)13;
		((Control)textBoxDatabase).BackColor = SystemColors.Window;
		((Control)textBoxDatabase).Location = new Point(152, 111);
		((Control)textBoxDatabase).Name = "textBoxDataset";
		((Control)textBoxDatabase).Size = new Size(371, 21);
		((Control)textBoxDatabase).TabIndex = 3;
		((Control)labelToken).ForeColor = Color.DimGray;
		((Control)labelToken).Location = new Point(12, 142);
		((Control)labelToken).Name = "labelToken";
		((Control)labelToken).Size = new Size(134, 20);
		((Control)labelToken).TabIndex = 10;
		((Control)labelToken).Text = "Token";
		((Label)labelToken).TextAlign = (ContentAlignment)16;
		((Control)textBoxToken).Anchor = (AnchorStyles)13;
		((Control)textBoxToken).BackColor = SystemColors.Window;
		((Control)textBoxToken).Location = new Point(152, 142);
		((Control)textBoxToken).Name = "textBoxToken";
		((Control)textBoxToken).Size = new Size(371, 21);
		((Control)textBoxToken).TabIndex = 4;
		((Control)checkBoxOAuthv2).Anchor = (AnchorStyles)13;
		((CheckBox)checkBoxOAuthv2).Checked = false;
		((CheckBox)checkBoxOAuthv2).CheckState = (CheckState)1;
		((Control)checkBoxOAuthv2).Location = new Point(152, 177);
		((Control)checkBoxOAuthv2).Name = "checkBoxOAuthv2";
		((Control)checkBoxOAuthv2).Size = new Size(334, 21);
		((Control)checkBoxOAuthv2).TabIndex = 5;
		((Control)checkBoxOAuthv2).Text = "OAuth v2";
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(545, 253);
		((Control)this).Controls.Add((Control)(object)checkBoxOAuthv2);
		((Control)this).Controls.Add((Control)(object)labelToken);
		((Control)this).Controls.Add((Control)(object)textBoxToken);
		((Control)this).Controls.Add((Control)(object)labelDatabase);
		((Control)this).Controls.Add((Control)(object)textBoxDatabase);
		((Control)this).Controls.Add((Control)(object)labelOwner);
		((Control)this).Controls.Add((Control)(object)textBoxOwner);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataWorldDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDataWorldDatabaseEditForm";
		((Form)this).Load += This_Load;
		((Control)glButtons).ResumeLayout(false);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
