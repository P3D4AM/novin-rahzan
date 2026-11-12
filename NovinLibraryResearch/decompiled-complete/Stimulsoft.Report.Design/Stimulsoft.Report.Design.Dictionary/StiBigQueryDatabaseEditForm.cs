using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiBigQueryDatabaseEditForm : StiFormUX
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

	private StiLabelUX labelServiceAccountKey;

	private StiLabelUX labelProjectId;

	public StiTextBox tbProjectId;

	private ErrorProvider errorProvider;

	private StiLabelUX labelDatasetId;

	public StiTextBox tbDatasetId;

	private StiTextBoxUX tbServiceAccountKey;

	private StiButtonUX btOpenServiceAccountKeyFile;

	public string Base64EncodedAuthSecret
	{
		get
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(((Control)tbServiceAccountKey).Text));
		}
		set
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(value ?? string.Empty);
				((Control)tbServiceAccountKey).Text = Encoding.UTF8.GetString(bytes);
			}
			catch (FormatException)
			{
				((Control)tbServiceAccountKey).Text = string.Empty;
			}
		}
	}

	public StiBigQueryDatabaseEditForm(StiReport report, string originalName)
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
		((Control)labelServiceAccountKey).Text = "Service Account Key File";
		((Control)labelProjectId).Text = "Project ID";
		((Control)labelDatasetId).Text = "Dataset ID";
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		((ButtonBase)btOpenServiceAccountKeyFile).Image = StiReportDesignImages.Actions.OpenFile();
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

	private void ButtonOpenServiceAccountKeyFile_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).DefaultExt = "json";
			((FileDialog)val).Filter = "JSON files|*.json";
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			((TextBoxBase)tbServiceAccountKey).Lines = File.ReadAllLines(((FileDialog)val).FileName);
			if (!string.IsNullOrEmpty(((Control)tbProjectId).Text?.Trim()))
			{
				return;
			}
			try
			{
				string text = JObject.Parse(((Control)tbServiceAccountKey).Text)["project_id"].ToString();
				if (!string.IsNullOrEmpty(text?.Trim()))
				{
					((Control)tbProjectId).Text = text ?? ((Control)tbProjectId).Text;
				}
			}
			catch (JsonException)
			{
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)tbServiceAccountKey).Text.Trim().Length == 0)
		{
			string text = string.Format(StiLocalization.Get("Errors", "FieldRequire"), ((Control)labelServiceAccountKey).Text);
			errorProvider.SetError((Control)(object)tbServiceAccountKey, text);
			((Control)btOpenServiceAccountKeyFile).Focus();
		}
		else if (((Control)tbProjectId).Text.Trim().Length == 0)
		{
			string text2 = string.Format(StiLocalization.Get("Errors", "FieldRequire"), ((Control)labelProjectId).Text);
			errorProvider.SetError((Control)(object)tbProjectId, text2);
			((Control)tbProjectId).Focus();
		}
		else if (((Control)tbDatasetId).Text.Trim().Length == 0)
		{
			string text3 = string.Format(StiLocalization.Get("Errors", "FieldRequire"), ((Control)labelDatasetId).Text);
			errorProvider.SetError((Control)(object)tbDatasetId, text3);
			((Control)tbDatasetId).Focus();
		}
		else if (((Control)tbName).Text.Trim().Length == 0)
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiBigQueryDatabaseEditForm));
		labelAlias = new StiLabelUX();
		labelName = new StiLabelUX();
		labelServiceAccountKey = new StiLabelUX();
		labelProjectId = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelDatasetId = new StiLabelUX();
		tbServiceAccountKey = new StiTextBoxUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tbDatasetId = new StiTextBox();
		tbProjectId = new StiTextBox();
		tbAlias = new StiTextBox();
		tbName = new StiTextBox();
		glButtons = new StiButtonPanelUX();
		btOpenServiceAccountKeyFile = new StiButtonUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(134, 20);
		((Control)labelAlias).TabIndex = 7;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(134, 20);
		((Control)labelName).TabIndex = 6;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)labelServiceAccountKey).Location = new Point(12, 144);
		((Control)labelServiceAccountKey).Name = "labelServiceAccountKey";
		((Control)labelServiceAccountKey).Size = new Size(134, 24);
		((Control)labelServiceAccountKey).TabIndex = 8;
		((Control)labelServiceAccountKey).Text = "Service Account Key";
		((Label)labelServiceAccountKey).TextAlign = (ContentAlignment)16;
		((Control)labelProjectId).Location = new Point(12, 78);
		((Control)labelProjectId).Name = "labelProjectId";
		((Control)labelProjectId).Size = new Size(134, 20);
		((Control)labelProjectId).TabIndex = 9;
		((Control)labelProjectId).Text = "Project ID";
		((Label)labelProjectId).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelDatasetId).Location = new Point(12, 111);
		((Control)labelDatasetId).Name = "labelDatasetId";
		((Control)labelDatasetId).Size = new Size(134, 20);
		((Control)labelDatasetId).TabIndex = 13;
		((Control)labelDatasetId).Text = "Dataset ID";
		((Label)labelDatasetId).TextAlign = (ContentAlignment)16;
		((TextBox)tbServiceAccountKey).AcceptsReturn = true;
		((Control)tbServiceAccountKey).Anchor = (AnchorStyles)15;
		((Control)tbServiceAccountKey).Location = new Point(152, 144);
		((Control)tbServiceAccountKey).Margin = new Padding(2);
		((TextBoxBase)tbServiceAccountKey).Multiline = true;
		((Control)tbServiceAccountKey).Name = "tbServiceAccountKey";
		((Control)tbServiceAccountKey).Size = new Size(428, 197);
		((Control)tbServiceAccountKey).TabIndex = 4;
		((TextBoxBase)tbServiceAccountKey).WordWrap = false;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(462, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(543, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)tbDatasetId).Anchor = (AnchorStyles)13;
		((Control)tbDatasetId).Location = new Point(152, 111);
		((Control)tbDatasetId).Name = "tbDatasetId";
		((Control)tbDatasetId).Size = new Size(457, 21);
		((Control)tbDatasetId).TabIndex = 3;
		((Control)tbProjectId).Anchor = (AnchorStyles)13;
		((Control)tbProjectId).Location = new Point(152, 78);
		((Control)tbProjectId).Name = "tbProjectId";
		((Control)tbProjectId).Size = new Size(457, 21);
		((Control)tbProjectId).TabIndex = 2;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(316, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(316, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 353);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(631, 48);
		((Control)glButtons).TabIndex = 6;
		((Control)btOpenServiceAccountKeyFile).Anchor = (AnchorStyles)9;
		((ButtonBase)btOpenServiceAccountKeyFile).Image = (Image)componentResourceManager.GetObject("btOpenServiceAccountKeyFile.Image");
		((Control)btOpenServiceAccountKeyFile).Location = new Point(585, 144);
		((Control)btOpenServiceAccountKeyFile).Name = "btOpenServiceAccountKeyFile";
		((Control)btOpenServiceAccountKeyFile).Size = new Size(24, 24);
		((Control)btOpenServiceAccountKeyFile).TabIndex = 5;
		((ButtonBase)btOpenServiceAccountKeyFile).UseVisualStyleBackColor = true;
		((Control)btOpenServiceAccountKeyFile).Click += ButtonOpenServiceAccountKeyFile_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(631, 401);
		((Control)this).Controls.Add((Control)(object)btOpenServiceAccountKeyFile);
		((Control)this).Controls.Add((Control)(object)tbServiceAccountKey);
		((Control)this).Controls.Add((Control)(object)labelDatasetId);
		((Control)this).Controls.Add((Control)(object)tbDatasetId);
		((Control)this).Controls.Add((Control)(object)labelProjectId);
		((Control)this).Controls.Add((Control)(object)tbProjectId);
		((Control)this).Controls.Add((Control)(object)labelServiceAccountKey);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiBigQueryDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiBigQueryDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
