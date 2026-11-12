using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary.Databases.Azure;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiNoSqlDatabaseEditForm : StiFormUX
{
	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiTextBox tbName;

	private StiLabelUX lbConnectionString;

	private StiTextBoxUX tbConnectionString;

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

	private ToolTip toolTip1;

	private StiDropDownBox comboBoxApi;

	private StiLabelUX labelApi;

	private StiNoSqlDatabase database;

	private string oldValue;

	public string ConnectionString
	{
		get
		{
			if (database is StiCosmosDbDatabase)
			{
				StiCosmosDbApi stiCosmosDbApi = (StiCosmosDbApi)Enum.Parse(typeof(StiCosmosDbApi), comboBoxApi.SelectedIndex.ToString());
				return $"{((Control)tbConnectionString).Text};Api={stiCosmosDbApi}";
			}
			return ((Control)tbConnectionString).Text;
		}
		set
		{
			if (database is StiCosmosDbDatabase)
			{
				int num = value.IndexOf(";Api=");
				if (num > 0)
				{
					((Control)tbConnectionString).Text = value.Substring(0, num);
				}
				else
				{
					((Control)tbConnectionString).Text = value;
				}
				string connectionStringKey = StiConnectionStringHelper.GetConnectionStringKey(value, "Api");
				if (!string.IsNullOrEmpty(connectionStringKey))
				{
					comboBoxApi.SelectedItem = connectionStringKey;
				}
				else
				{
					comboBoxApi.SelectedItem = "SQL";
				}
			}
			else
			{
				((Control)tbConnectionString).Text = value;
			}
		}
	}

	public StiNoSqlDatabaseEditForm(StiNoSqlDatabase database)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			DoScaling();
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.database = database;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			if (!database.CanEditConnectionString)
			{
				((Control)buttonBuild).Visible = false;
				StiButtonUX stiButtonUX = buttonClean;
				((Control)stiButtonUX).Top = ((Control)stiButtonUX).Top - StiScale.I(27);
				StiButtonUX stiButtonUX2 = buttonInfoConnectionString;
				((Control)stiButtonUX2).Top = ((Control)stiButtonUX2).Top - StiScale.I(27);
				StiButtonUX stiButtonUX3 = buttonTestConnection;
				((Control)stiButtonUX3).Top = ((Control)stiButtonUX3).Top - StiScale.I(27);
			}
			if (database is StiCosmosDbDatabase)
			{
				((Control)checkBoxPromptUserNameAndPassword).Visible = false;
				((Control)labelApi).Visible = true;
				((Control)comboBoxApi).Visible = true;
				((Control)comboBoxApi).Location = new Point(((Control)tbAlias).Location.X, ((Control)checkBoxPromptUserNameAndPassword).Location.Y);
				((Control)labelApi).Location = new Point(((Control)labelAlias).Location.X, ((Control)checkBoxPromptUserNameAndPassword).Location.Y);
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
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiNoSqlDatabaseEditForm));
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
		toolTip1 = new ToolTip(components);
		comboBoxApi = new StiDropDownBox();
		labelApi = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(508, 13);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 26);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(425, 13);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 26);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += BtOk_Click;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(124, 20);
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(144, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(436, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TbName_Enter;
		((Control)tbName).Leave += TbName_Leave;
		((Control)lbConnectionString).Location = new Point(12, 83);
		((Control)lbConnectionString).Name = "lbConnectionString";
		((Control)lbConnectionString).Size = new Size(136, 20);
		((Control)lbConnectionString).TabIndex = 7;
		((Control)lbConnectionString).Text = "Connection String";
		((Label)lbConnectionString).TextAlign = (ContentAlignment)16;
		((TextBox)tbConnectionString).AcceptsReturn = true;
		((Control)tbConnectionString).Anchor = (AnchorStyles)15;
		((Control)tbConnectionString).Location = new Point(12, 106);
		((TextBoxBase)tbConnectionString).Multiline = true;
		((Control)tbConnectionString).Name = "tbConnectionString";
		((Control)tbConnectionString).Size = new Size(568, 160);
		((Control)tbConnectionString).TabIndex = 2;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 311);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(595, 50);
		((Control)glButtons).TabIndex = 9;
		((Control)buttonBuild).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonBuild).Image = (Image)componentResourceManager.GetObject("buttonBuild.Image");
		((Control)buttonBuild).Location = new Point(552, 111);
		((Control)buttonBuild).Name = "buttonBuild";
		((Control)buttonBuild).Size = new Size(24, 24);
		((Control)buttonBuild).TabIndex = 3;
		((ButtonBase)buttonBuild).UseVisualStyleBackColor = true;
		((Control)buttonBuild).Click += ButtonBuild_Click;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(144, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(436, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)labelAlias).Location = new Point(12, 47);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(124, 17);
		((Control)labelAlias).TabIndex = 6;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)checkBoxPromptUserNameAndPassword).Anchor = (AnchorStyles)14;
		((Control)checkBoxPromptUserNameAndPassword).Location = new Point(12, 277);
		((Control)checkBoxPromptUserNameAndPassword).Name = "checkBoxPromptUserNameAndPassword";
		((Control)checkBoxPromptUserNameAndPassword).Size = new Size(436, 24);
		((Control)checkBoxPromptUserNameAndPassword).TabIndex = 7;
		((Control)checkBoxPromptUserNameAndPassword).Text = "Prompt User Name and Password";
		((Control)buttonClean).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonClean).Image = (Image)componentResourceManager.GetObject("buttonClean.Image");
		((Control)buttonClean).Location = new Point(552, 137);
		((Control)buttonClean).Name = "buttonClean";
		((Control)buttonClean).Size = new Size(24, 24);
		((Control)buttonClean).TabIndex = 4;
		((ButtonBase)buttonClean).UseVisualStyleBackColor = true;
		((Control)buttonClean).Click += ButtonClean_Click;
		((Control)buttonTestConnection).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonTestConnection).Image = (Image)componentResourceManager.GetObject("buttonTestConnection.Image");
		((Control)buttonTestConnection).Location = new Point(552, 163);
		((Control)buttonTestConnection).Name = "buttonTestConnection";
		((Control)buttonTestConnection).Size = new Size(24, 24);
		((Control)buttonTestConnection).TabIndex = 5;
		((ButtonBase)buttonTestConnection).UseVisualStyleBackColor = true;
		((Control)buttonTestConnection).Click += ButtonTestConnection_Click;
		((Control)buttonInfoConnectionString).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonInfoConnectionString).Image = (Image)componentResourceManager.GetObject("buttonInfoConnectionString.Image");
		((Control)buttonInfoConnectionString).Location = new Point(552, 189);
		((Control)buttonInfoConnectionString).Name = "buttonInfoConnectionString";
		((Control)buttonInfoConnectionString).Size = new Size(24, 24);
		((Control)buttonInfoConnectionString).TabIndex = 6;
		((ButtonBase)buttonInfoConnectionString).UseVisualStyleBackColor = true;
		((Control)buttonInfoConnectionString).Click += ButtonInfoConnectionString_Click;
		((Control)comboBoxApi).Anchor = (AnchorStyles)14;
		comboBoxApi.DrawMode = (DrawMode)2;
		comboBoxApi.FormattingEnabled = true;
		comboBoxApi.Items.AddRange(new object[2] { "SQL", "MongoDB" });
		((Control)comboBoxApi).Location = new Point(398, 277);
		((Control)comboBoxApi).Name = "comboBoxApi";
		comboBoxApi.ReadOnly = true;
		comboBoxApi.SelectedIndex = -1;
		comboBoxApi.SelectedItem = null;
		comboBoxApi.SelectedValue = null;
		((Control)comboBoxApi).Size = new Size(180, 21);
		((Control)comboBoxApi).TabIndex = 8;
		((Control)comboBoxApi).Visible = false;
		((Control)labelApi).Location = new Point(280, 277);
		((Control)labelApi).Name = "labelApi";
		((Control)labelApi).Size = new Size(112, 21);
		((Control)labelApi).TabIndex = 17;
		((Control)labelApi).Text = "API";
		((Label)labelApi).TextAlign = (ContentAlignment)16;
		((Control)labelApi).Visible = false;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(595, 361);
		((Control)this).Controls.Add((Control)(object)comboBoxApi);
		((Control)this).Controls.Add((Control)(object)labelApi);
		((Control)this).Controls.Add((Control)(object)buttonInfoConnectionString);
		((Control)this).Controls.Add((Control)(object)buttonTestConnection);
		((Control)this).Controls.Add((Control)(object)buttonClean);
		((Control)this).Controls.Add((Control)(object)checkBoxPromptUserNameAndPassword);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)buttonBuild);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)lbConnectionString);
		((Control)this).Controls.Add((Control)(object)tbConnectionString);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(595, 385);
		((Control)this).Name = "StiNoSqlDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiSqlDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)lbConnectionString).Text = Loc.GetCleaned("FormDatabaseEdit", "ConnectionString");
		((Control)checkBoxPromptUserNameAndPassword).Text = StiLocalization.Get("FormDatabaseEdit", "PromptUserNameAndPassword");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		BuildToolTip();
	}

	private void DoTheming()
	{
		StiButtonUX stiButtonUX = buttonBuild;
		((Control)stiButtonUX).Left = ((Control)stiButtonUX).Left - StiScale.I4;
		StiButtonUX stiButtonUX2 = buttonBuild;
		((Control)stiButtonUX2).Top = ((Control)stiButtonUX2).Top + StiScale.I4;
		StiButtonUX stiButtonUX3 = buttonClean;
		((Control)stiButtonUX3).Left = ((Control)stiButtonUX3).Left - StiScale.I4;
		StiButtonUX stiButtonUX4 = buttonClean;
		((Control)stiButtonUX4).Top = ((Control)stiButtonUX4).Top + StiScale.I4;
		StiButtonUX stiButtonUX5 = buttonInfoConnectionString;
		((Control)stiButtonUX5).Left = ((Control)stiButtonUX5).Left - StiScale.I4;
		StiButtonUX stiButtonUX6 = buttonInfoConnectionString;
		((Control)stiButtonUX6).Top = ((Control)stiButtonUX6).Top + StiScale.I4;
		StiButtonUX stiButtonUX7 = buttonTestConnection;
		((Control)stiButtonUX7).Left = ((Control)stiButtonUX7).Left - StiScale.I4;
		StiButtonUX stiButtonUX8 = buttonTestConnection;
		((Control)stiButtonUX8).Top = ((Control)stiButtonUX8).Top + StiScale.I4;
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

	private void BuildToolTip()
	{
		toolTip1.SetToolTip((Control)(object)buttonInfoConnectionString, database.CreateNoSqlConnector(ConnectionString).GetSampleConnectionString());
	}

	private void ComboBoxApi_SelectedValueChanged(object sender, EventArgs e)
	{
		BuildToolTip();
	}

	private void BtOk_Click(object sender, EventArgs e)
	{
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiHint.CreateHint(StiLocalization.Get("Report", "LabelName"))));
			((Control)tbName).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbName, string.Empty);
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void ButtonBuild_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = database.EditConnectionString(((Control)tbConnectionString).Text);
	}

	private void TbName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TbName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void ButtonClean_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = string.Empty;
	}

	private void ButtonTestConnection_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		string text = database.GetDataAdapter().TestConnection(ConnectionString);
		MessageBox.Show(text);
	}

	private void ButtonInfoConnectionString_Click(object sender, EventArgs e)
	{
		((Control)tbConnectionString).Text = database.CreateNoSqlConnector(ConnectionString).GetSampleConnectionString();
		((Control)tbConnectionString).Focus();
	}
}
