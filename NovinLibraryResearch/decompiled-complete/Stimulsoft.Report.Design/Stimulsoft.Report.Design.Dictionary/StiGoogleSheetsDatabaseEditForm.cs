using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiGoogleSheetsDatabaseEditForm : StiFormUX
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

	private StiLabelUX labelClientId;

	public StiTextBox textBoxClientId;

	private StiLabelUX labelClientSecret;

	public StiTextBox textBoxClientSecret;

	private StiLabelUX labelSpreadsheetId;

	public StiTextBox textBoxSpreadsheetId;

	public StiCheckBoxUX checkBoxFirstRowIsHeader;

	private ErrorProvider errorProvider;

	public StiGoogleSheetsDatabaseEditForm(StiReport report, string originalName)
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
		((Control)labelClientId).Text = Loc.Get("FormDatabaseEdit", "ClientId");
		((Control)labelClientSecret).Text = Loc.Get("FormDatabaseEdit", "ClientSecret");
		((Control)labelSpreadsheetId).Text = Loc.Get("FormDatabaseEdit", "SpreadsheetId");
		((Control)checkBoxFirstRowIsHeader).Text = StiLocalization.Get("FormDatabaseEdit", "FirstRowIsHeader");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
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
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
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

	private void This_Load(object sender, EventArgs e)
	{
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
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGoogleSheetsDatabaseEditForm));
		glButtons = new StiButtonPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		labelAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		labelClientId = new StiLabelUX();
		textBoxClientId = new StiTextBox();
		labelClientSecret = new StiLabelUX();
		textBoxClientSecret = new StiTextBox();
		labelSpreadsheetId = new StiLabelUX();
		textBoxSpreadsheetId = new StiTextBox();
		checkBoxFirstRowIsHeader = new StiCheckBoxUX();
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
		((Control)labelClientId).ForeColor = Color.DimGray;
		((Control)labelClientId).Location = new Point(12, 78);
		((Control)labelClientId).Name = "labelClientId";
		((Control)labelClientId).Size = new Size(134, 20);
		((Control)labelClientId).TabIndex = 8;
		((Control)labelClientId).Text = "Client Id";
		((Label)labelClientId).TextAlign = (ContentAlignment)16;
		((Control)textBoxClientId).Anchor = (AnchorStyles)13;
		((Control)textBoxClientId).BackColor = SystemColors.Window;
		((Control)textBoxClientId).Location = new Point(152, 78);
		((Control)textBoxClientId).Name = "textBoxClientId";
		((Control)textBoxClientId).Size = new Size(371, 21);
		((Control)textBoxClientId).TabIndex = 2;
		((Control)labelClientSecret).ForeColor = Color.DimGray;
		((Control)labelClientSecret).Location = new Point(12, 111);
		((Control)labelClientSecret).Name = "labelClientSecret";
		((Control)labelClientSecret).Size = new Size(134, 20);
		((Control)labelClientSecret).TabIndex = 9;
		((Control)labelClientSecret).Text = "Client Secret";
		((Label)labelClientSecret).TextAlign = (ContentAlignment)16;
		((Control)textBoxClientSecret).Anchor = (AnchorStyles)13;
		((Control)textBoxClientSecret).BackColor = SystemColors.Window;
		((Control)textBoxClientSecret).Location = new Point(152, 111);
		((Control)textBoxClientSecret).Name = "textBoxClientSecret";
		((Control)textBoxClientSecret).Size = new Size(371, 21);
		((Control)textBoxClientSecret).TabIndex = 3;
		((Control)labelSpreadsheetId).ForeColor = Color.DimGray;
		((Control)labelSpreadsheetId).Location = new Point(12, 142);
		((Control)labelSpreadsheetId).Name = "labelSpreadsheetId";
		((Control)labelSpreadsheetId).Size = new Size(134, 20);
		((Control)labelSpreadsheetId).TabIndex = 10;
		((Control)labelSpreadsheetId).Text = "Spreadsheet Id";
		((Label)labelSpreadsheetId).TextAlign = (ContentAlignment)16;
		((Control)textBoxSpreadsheetId).Anchor = (AnchorStyles)13;
		((Control)textBoxSpreadsheetId).BackColor = SystemColors.Window;
		((Control)textBoxSpreadsheetId).Location = new Point(152, 142);
		((Control)textBoxSpreadsheetId).Name = "textBoxSpreadsheetId";
		((Control)textBoxSpreadsheetId).Size = new Size(371, 21);
		((Control)textBoxSpreadsheetId).TabIndex = 4;
		((Control)checkBoxFirstRowIsHeader).Anchor = (AnchorStyles)13;
		((CheckBox)checkBoxFirstRowIsHeader).Checked = true;
		((CheckBox)checkBoxFirstRowIsHeader).CheckState = (CheckState)1;
		((Control)checkBoxFirstRowIsHeader).Location = new Point(152, 177);
		((Control)checkBoxFirstRowIsHeader).Name = "checkBoxFirstRowIsHeader";
		((Control)checkBoxFirstRowIsHeader).Size = new Size(334, 21);
		((Control)checkBoxFirstRowIsHeader).TabIndex = 5;
		((Control)checkBoxFirstRowIsHeader).Text = "First Row is Header";
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(545, 253);
		((Control)this).Controls.Add((Control)(object)checkBoxFirstRowIsHeader);
		((Control)this).Controls.Add((Control)(object)labelSpreadsheetId);
		((Control)this).Controls.Add((Control)(object)textBoxSpreadsheetId);
		((Control)this).Controls.Add((Control)(object)labelClientSecret);
		((Control)this).Controls.Add((Control)(object)textBoxClientSecret);
		((Control)this).Controls.Add((Control)(object)labelClientId);
		((Control)this).Controls.Add((Control)(object)textBoxClientId);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGoogleSheetsDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiGoogleSheetsDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((Control)glButtons).ResumeLayout(false);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
