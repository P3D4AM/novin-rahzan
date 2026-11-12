using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiSqlDatabaseEditForm : StiFormUX
{
	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiTextBox tbName;

	private StiLabelUX lbConnectionString;

	public StiTextBoxUX tbConnectionString;

	private ErrorProvider errorProvider;

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX buttonBuild;

	private StiLabelUX labelName;

	public StiTextBox tbAlias;

	private StiLabelUX labelAlias;

	public StiCheckBoxUX checkBoxPromptUserNameAndPassword;

	private StiButtonUX buttonClean;

	private StiButtonUX buttonTestConnection;

	private StiButtonUX buttonInfoConnectionString;

	private ToolTip toolTip;

	private StiPanelUX panelBorder;

	public StiCheckBoxUX checkBoxODataV3;

	private StiSqlDatabase database;

	private StiReport report;

	private string originalName;

	private string oldValue;

	public StiSqlDatabaseEditForm(StiReport report, StiSqlDatabase database, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		DoTheming();
		DoScaling();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.report = report;
			this.database = database;
			this.originalName = originalName;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((CheckBox)checkBoxPromptUserNameAndPassword).Checked = StiSettings.GetBool("SqlDatabaseEditForm", "PromptUserNameAndPassword", nullValue: false);
			if (!database.CanEditConnectionString)
			{
				((Control)buttonBuild).Visible = false;
				((Control)buttonInfoConnectionString).Top = ((Control)buttonTestConnection).Top;
				((Control)buttonTestConnection).Top = ((Control)buttonClean).Top;
				((Control)buttonClean).Top = ((Control)buttonBuild).Top;
			}
			if (database is StiODataDatabase)
			{
				((Control)checkBoxPromptUserNameAndPassword).Visible = false;
				((Control)checkBoxODataV3).Visible = true;
				((Control)checkBoxODataV3).Location = new Point(StiScale.XXI(12.0), ((Control)checkBoxODataV3).Location.Y);
			}
			((Form)this).ShowIcon = false;
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSqlDatabaseEditForm));
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		lbConnectionString = new StiLabelUX();
		tbConnectionString = new StiTextBoxUX();
		errorProvider = new ErrorProvider(components);
		glButtons = new StiButtonPanelUX();
		buttonBuild = new StiButtonUX();
		tbAlias = new StiTextBox();
		labelAlias = new StiLabelUX();
		checkBoxPromptUserNameAndPassword = new StiCheckBoxUX();
		buttonClean = new StiButtonUX();
		buttonTestConnection = new StiButtonUX();
		buttonInfoConnectionString = new StiButtonUX();
		toolTip = new ToolTip(components);
		panelBorder = new StiPanelUX();
		checkBoxODataV3 = new StiCheckBoxUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)panelBorder).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(497, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(416, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(126, 20);
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(144, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(278, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)lbConnectionString).ForeColor = Color.DimGray;
		((Control)lbConnectionString).Location = new Point(12, 80);
		((Control)lbConnectionString).Name = "lbConnectionString";
		((Control)lbConnectionString).Size = new Size(136, 20);
		((Control)lbConnectionString).TabIndex = 7;
		((Control)lbConnectionString).Text = "Connection String";
		((Label)lbConnectionString).TextAlign = (ContentAlignment)16;
		((TextBox)tbConnectionString).AcceptsReturn = true;
		((Control)tbConnectionString).Anchor = (AnchorStyles)15;
		((TextBoxBase)tbConnectionString).BorderStyle = (BorderStyle)0;
		((Control)tbConnectionString).Location = new Point(3, 3);
		((TextBoxBase)tbConnectionString).Multiline = true;
		((Control)tbConnectionString).Name = "tbConnectionString";
		((Control)tbConnectionString).Size = new Size(521, 194);
		((Control)tbConnectionString).TabIndex = 0;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 343);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(584, 46);
		((Control)glButtons).TabIndex = 4;
		((Control)buttonBuild).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonBuild).Image = (Image)componentResourceManager.GetObject("buttonBuild.Image");
		((Control)buttonBuild).Location = new Point(530, 3);
		((Control)buttonBuild).Name = "buttonBuild";
		((Control)buttonBuild).Size = new Size(24, 24);
		((Control)buttonBuild).TabIndex = 1;
		((ButtonBase)buttonBuild).UseVisualStyleBackColor = true;
		((Control)buttonBuild).Click += ButtonBuild_Click;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(144, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(278, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(126, 20);
		((Control)labelAlias).TabIndex = 6;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)checkBoxPromptUserNameAndPassword).Anchor = (AnchorStyles)14;
		((Control)checkBoxPromptUserNameAndPassword).Location = new Point(12, 314);
		((Control)checkBoxPromptUserNameAndPassword).Name = "checkBoxPromptUserNameAndPassword";
		((Control)checkBoxPromptUserNameAndPassword).Size = new Size(430, 24);
		((Control)checkBoxPromptUserNameAndPassword).TabIndex = 3;
		((Control)checkBoxPromptUserNameAndPassword).Text = "Prompt User Name and Password";
		((Control)buttonClean).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonClean).Image = (Image)componentResourceManager.GetObject("buttonClean.Image");
		((Control)buttonClean).Location = new Point(530, 29);
		((Control)buttonClean).Name = "buttonClean";
		((Control)buttonClean).Size = new Size(24, 24);
		((Control)buttonClean).TabIndex = 2;
		((ButtonBase)buttonClean).UseVisualStyleBackColor = true;
		((Control)buttonClean).Click += ButtonClean_Click;
		((Control)buttonTestConnection).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonTestConnection).Image = (Image)componentResourceManager.GetObject("buttonTestConnection.Image");
		((Control)buttonTestConnection).Location = new Point(530, 56);
		((Control)buttonTestConnection).Name = "buttonTestConnection";
		((Control)buttonTestConnection).Size = new Size(24, 24);
		((Control)buttonTestConnection).TabIndex = 3;
		((ButtonBase)buttonTestConnection).UseVisualStyleBackColor = true;
		((Control)buttonTestConnection).Click += ButtonTestConnection_Click;
		((Control)buttonInfoConnectionString).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonInfoConnectionString).Image = (Image)componentResourceManager.GetObject("buttonInfoConnectionString.Image");
		((Control)buttonInfoConnectionString).Location = new Point(530, 83);
		((Control)buttonInfoConnectionString).Name = "buttonInfoConnectionString";
		((Control)buttonInfoConnectionString).Size = new Size(24, 24);
		((Control)buttonInfoConnectionString).TabIndex = 4;
		((ButtonBase)buttonInfoConnectionString).UseVisualStyleBackColor = true;
		((Control)buttonInfoConnectionString).Click += ButtonInfoConnectionString_Click;
		((Control)panelBorder).Anchor = (AnchorStyles)15;
		((Panel)panelBorder).BorderStyle = (BorderStyle)1;
		((Control)panelBorder).Controls.Add((Control)(object)buttonInfoConnectionString);
		((Control)panelBorder).Controls.Add((Control)(object)tbConnectionString);
		((Control)panelBorder).Controls.Add((Control)(object)buttonTestConnection);
		((Control)panelBorder).Controls.Add((Control)(object)buttonBuild);
		((Control)panelBorder).Controls.Add((Control)(object)buttonClean);
		((Control)panelBorder).Location = new Point(12, 102);
		((Control)panelBorder).Name = "panelBorder";
		((Control)panelBorder).Size = new Size(559, 204);
		((Control)panelBorder).TabIndex = 2;
		((Control)checkBoxODataV3).Anchor = (AnchorStyles)14;
		((Control)checkBoxODataV3).Location = new Point(274, 314);
		((Control)checkBoxODataV3).Name = "checkBoxODataV3";
		((Control)checkBoxODataV3).Size = new Size(298, 24);
		((Control)checkBoxODataV3).TabIndex = 14;
		((Control)checkBoxODataV3).Text = "OData V3";
		((Control)checkBoxODataV3).Visible = false;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(584, 389);
		((Control)this).Controls.Add((Control)(object)checkBoxODataV3);
		((Control)this).Controls.Add((Control)(object)checkBoxPromptUserNameAndPassword);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)lbConnectionString);
		((Control)this).Controls.Add((Control)(object)panelBorder);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(595, 385);
		((Control)this).Name = "StiSqlDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiSqlDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)panelBorder).ResumeLayout(false);
		((Control)panelBorder).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)lbConnectionString).Text = Loc.GetCleaned("FormDatabaseEdit", "ConnectionString");
		((Control)checkBoxPromptUserNameAndPassword).Text = StiLocalization.Get("FormDatabaseEdit", "PromptUserNameAndPassword");
		toolTip.SetToolTip((Control)(object)buttonBuild, StiLocalization.Get("Buttons", "Build").Replace("...", string.Empty));
		toolTip.SetToolTip((Control)(object)buttonClean, StiLocalization.Get("Buttons", "Remove"));
		toolTip.SetToolTip((Control)(object)buttonTestConnection, StiLocalization.Get("DesignerFx", "TestConnection"));
		toolTip.SetToolTip((Control)(object)buttonInfoConnectionString, database.CreateSqlConnector().GetSampleConnectionString());
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			panelBorder.IsDarkerBackground = true;
		}
	}

	private void DoScaling()
	{
		if (StiScale.Step >= 1.5)
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(StiSqlDatabaseEditForm));
			errorProvider.Icon = StiComponentResourceHelper.GetIcon(resources, "errorProvider.Icon");
		}
		((ButtonBase)buttonClean).Image = StiReportDesignImages.Editor.ClearContent();
		((ButtonBase)buttonTestConnection).Image = StiReportDesignImages.Actions.Check();
		((ButtonBase)buttonBuild).Image = StiReportDesignImages.Actions.Build();
		((ButtonBase)buttonInfoConnectionString).Image = StiReportDesignImages.Actions.Overview();
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void ButtonBuild_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = database.EditConnectionString(((Control)tbConnectionString).Text);
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

	private void ButtonClean_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = string.Empty;
	}

	private void ButtonTestConnection_Click(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		string text = database.GetDataAdapter().TestConnection(((Control)tbConnectionString).Text);
		bool flag = text != StiLocalization.Get("DesignerFx", "ConnectionSuccessfull");
		MessageBox.Show((IWin32Window)(object)this, text, StiLocalization.Get("DesignerFx", "TestConnection"), (MessageBoxButtons)0, (MessageBoxIcon)(flag ? 16 : 64));
	}

	private void ButtonInfoConnectionString_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = database.CreateSqlConnector().GetSampleConnectionString();
		((Control)tbConnectionString).Focus();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiSettings.Set("SqlDatabaseEditForm", "PromptUserNameAndPassword", ((CheckBox)checkBoxPromptUserNameAndPassword).Checked);
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Save();
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
}
