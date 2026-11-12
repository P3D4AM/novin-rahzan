using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiAzureBlobStorageDatabaseEditForm : StiFormUX
{
	private StiReport report;

	private string oldValue;

	private string originalName;

	private bool panelCsvOptionsShouldBeVisible;

	private bool panelExcelOptionsShouldBeVisible;

	private string[] csvDelimiterOptionsNames;

	private string csvCustomDelimiterOptionName;

	private readonly string[] csvDelimiters = new string[5] { "", "\t", ";", ",", " " };

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	private StiLabelUX labelName;

	public StiTextBox tbName;

	private StiLabelUX labelAccountName;

	public StiTextBox tbAccountName;

	private ErrorProvider errorProvider;

	private StiLabelUX labelAccountKey;

	private StiLabelUX labelContainerName;

	private StiLabelUX labelBlobName;

	private StiDropDownBox cbBlobName;

	private StiDropDownBox cbContainerName;

	private StiLabelUX labelBlobContentType;

	private StiComboBox cbBlobContentType;

	private StiTextBoxUX tbAccountKey;

	private Timer timerContentTypeUpdateDelay;

	public StiTextBox tbCsvCustomDelimeter;

	private StiComboBox cbCsvDelimiter;

	private StiLabelUX labelCsvDelimiter;

	private StiLabelUX labelCsvEncoding;

	private StiComboBox cbCsvEncoding;

	public StiCheckBoxUX checkFirstRowIsHeader;

	private StiPanelUX panelExcelOptions;

	private StiPanelUX panelCsvOptions;

	private StiPanelUX panelMain;

	public string ConnectionString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder("AccountKey=" + AccountKey + ";AccountName=" + AccountName + ";ContainerName=" + ContainerName + ";BlobName=" + BlobName + ";BlobContentType=" + BlobContentType + ";");
			if (BlobContentType == "CSV")
			{
				stringBuilder.Append($"CodePage={CodePage};Delimiter=\"{Delimiter}\";");
			}
			else if (BlobContentType == "Excel")
			{
				stringBuilder.Append($"FirstRowIsHeader={FirstRowIsHeader};");
			}
			return stringBuilder.ToString();
		}
	}

	public string AccountKey
	{
		get
		{
			return ((Control)tbAccountKey).Text;
		}
		set
		{
			((Control)tbAccountKey).Text = value;
		}
	}

	public string AccountName
	{
		get
		{
			return ((Control)tbAccountName).Text;
		}
		set
		{
			((Control)tbAccountName).Text = value;
		}
	}

	public string ContainerName
	{
		get
		{
			return ((Control)cbContainerName).Text;
		}
		set
		{
			((Control)cbContainerName).Text = value;
		}
	}

	public string BlobName
	{
		get
		{
			return ((Control)cbBlobName).Text;
		}
		set
		{
			((Control)cbBlobName).Text = value;
		}
	}

	public string BlobContentType
	{
		get
		{
			return ((Control)cbBlobContentType).Text;
		}
		set
		{
			if (value == null)
			{
				((ListControl)cbBlobContentType).SelectedIndex = 0;
				return;
			}
			int num = ((ComboBox)cbBlobContentType).Items.IndexOf((object)value);
			((ListControl)cbBlobContentType).SelectedIndex = ((num > 0) ? num : 0);
		}
	}

	public int CodePage
	{
		get
		{
			return StiCsvHelper.CodePageCodes[((ListControl)cbCsvEncoding).SelectedIndex];
		}
		set
		{
			((ListControl)cbCsvEncoding).SelectedIndex = Array.FindIndex(StiCsvHelper.CodePageCodes, (int c) => c == value);
			if (((ListControl)cbCsvEncoding).SelectedIndex == -1)
			{
				((ListControl)cbCsvEncoding).SelectedIndex = 0;
			}
		}
	}

	public string Delimiter
	{
		get
		{
			if (((ListControl)cbCsvDelimiter).SelectedIndex >= csvDelimiters.Length)
			{
				return ((Control)tbCsvCustomDelimeter).Text;
			}
			return csvDelimiters[((ListControl)cbCsvDelimiter).SelectedIndex];
		}
		set
		{
			if (value == null)
			{
				((ListControl)cbCsvDelimiter).SelectedIndex = 0;
				return;
			}
			int num = Array.FindIndex(csvDelimiters, (string d) => d == value);
			if (num == -1)
			{
				((ListControl)cbCsvDelimiter).SelectedIndex = csvDelimiters.Length;
				((Control)tbCsvCustomDelimeter).Text = value;
			}
			else
			{
				((ListControl)cbCsvDelimiter).SelectedIndex = num;
			}
		}
	}

	public bool FirstRowIsHeader
	{
		get
		{
			return ((CheckBox)checkFirstRowIsHeader).Checked;
		}
		set
		{
			((CheckBox)checkFirstRowIsHeader).Checked = value;
		}
	}

	public StiAzureBlobStorageDatabaseEditForm(StiReport report, string originalName)
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
			((ComboBox)cbBlobContentType).Items.Add((object)string.Empty);
			((ComboBox)cbBlobContentType).Items.Add((object)"CSV");
			((ComboBox)cbBlobContentType).Items.Add((object)"Excel");
			((ComboBox)cbBlobContentType).Items.Add((object)"JSON");
			((ComboBox)cbBlobContentType).Items.Add((object)"XML");
			((ListControl)cbBlobContentType).SelectedIndex = 0;
			ObjectCollection items = ((ComboBox)cbCsvDelimiter).Items;
			object[] array = csvDelimiterOptionsNames;
			items.AddRange(array);
			((ComboBox)cbCsvDelimiter).Items.Add((object)csvCustomDelimiterOptionName);
			((ListControl)cbCsvDelimiter).SelectedIndex = 0;
			ObjectCollection items2 = ((ComboBox)cbCsvEncoding).Items;
			array = StiCsvHelper.CodePageNames;
			items2.AddRange(array);
		}
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelAccountName).Text = Loc.Get("FormDatabaseEdit", "AccountName");
		((Control)labelAccountKey).Text = Loc.Get("FormDatabaseEdit", "AccountKey");
		((Control)labelContainerName).Text = Loc.Get("FormDatabaseEdit", "ContainerName");
		((Control)labelBlobName).Text = Loc.Get("FormDatabaseEdit", "BlobName");
		((Control)labelBlobContentType).Text = Loc.Get("FormDatabaseEdit", "BlobContentType");
		csvDelimiterOptionsNames = new string[5]
		{
			StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSystem"),
			StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorTab"),
			StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSemicolon"),
			StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorComma"),
			StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSpace")
		};
		csvCustomDelimiterOptionName = StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorOther");
		((Control)checkFirstRowIsHeader).Text = StiLocalization.Get("FormDatabaseEdit", "FirstRowIsHeader");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		((Control)tbCsvCustomDelimeter).Location = new Point
		{
			X = ((Control)tbCsvCustomDelimeter).Location.X,
			Y = ((Control)tbCsvCustomDelimeter).Location.Y - (((Control)tbCsvCustomDelimeter).Height - ((Control)cbCsvDelimiter).Height) / 2
		};
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
		((Control)tbAccountName).Width = ((Control)tbAccountKey).Width;
		StiComboBox stiComboBox = cbBlobContentType;
		StiComboBox stiComboBox2 = cbCsvEncoding;
		int num = (((Control)cbCsvDelimiter).Width = ((Control)cbBlobName).Width);
		int width2 = (((Control)stiComboBox2).Width = num);
		((Control)stiComboBox).Width = width2;
	}

	private void RedrawContentOptionsPanels()
	{
		((Control)panelCsvOptions).Visible = panelCsvOptionsShouldBeVisible;
		((Control)panelExcelOptions).Visible = panelExcelOptionsShouldBeVisible;
		int num = ((Control)panelMain).Height + ((Control)glButtons).Height;
		if (panelExcelOptionsShouldBeVisible)
		{
			num += ((Control)panelExcelOptions).Height;
		}
		if (panelCsvOptionsShouldBeVisible)
		{
			num += ((Control)panelCsvOptions).Height;
		}
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, num);
	}

	private bool SetContolErrorAndFocusIfEmpty(Control control, Label label)
	{
		if (control.Text.Trim().Length == 0)
		{
			string text = string.Format(StiLocalization.Get("Errors", "FieldRequire"), ((Control)label).Text);
			control.Focus();
			errorProvider.SetError(control, text);
			return true;
		}
		errorProvider.SetError(control, string.Empty);
		return false;
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

	private void ComboBoxContainerName_DropDown(object sender, EventArgs e)
	{
		errorProvider.SetError((Control)(object)cbContainerName, string.Empty);
		if (SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountKey, (Label)(object)labelAccountKey) | SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountName, (Label)(object)labelAccountName))
		{
			return;
		}
		try
		{
			StiAzureBlobStorageConnector stiAzureBlobStorageConnector = StiAzureBlobStorageConnector.Get(ConnectionString);
			cbContainerName.DataSource = stiAzureBlobStorageConnector.GetContainerNamesList();
		}
		catch
		{
			cbContainerName.DataSource = null;
		}
	}

	private void ComboBoxBlobName_DropDown(object sender, EventArgs e)
	{
		errorProvider.SetError((Control)(object)cbBlobName, string.Empty);
		if (SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountKey, (Label)(object)labelAccountKey) | SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountName, (Label)(object)labelAccountName) | SetContolErrorAndFocusIfEmpty((Control)(object)cbContainerName, (Label)(object)labelContainerName))
		{
			return;
		}
		try
		{
			StiAzureBlobStorageConnector stiAzureBlobStorageConnector = StiAzureBlobStorageConnector.Get(ConnectionString);
			cbBlobName.DataSource = stiAzureBlobStorageConnector.GetBlobNamesList();
		}
		catch
		{
			cbBlobName.DataSource = null;
		}
	}

	private void ComboBoxBlobName_TextChanged(object sender, EventArgs e)
	{
		if (timerContentTypeUpdateDelay.Enabled)
		{
			timerContentTypeUpdateDelay.Stop();
		}
		timerContentTypeUpdateDelay.Start();
		((Control)btOk).Enabled = false;
	}

	private void TimerContentTypeUpdateDelay_Tick(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(((Control)tbAccountName).Text) && !string.IsNullOrWhiteSpace(((Control)tbAccountKey).Text) && !string.IsNullOrWhiteSpace(((Control)cbContainerName).Text) && !string.IsNullOrWhiteSpace(((Control)cbBlobName).Text))
		{
			StiAzureBlobStorageConnector stiAzureBlobStorageConnector = StiAzureBlobStorageConnector.Get(ConnectionString);
			BlobContentType = stiAzureBlobStorageConnector.GetBlobContentTypeOrDefault();
			timerContentTypeUpdateDelay.Stop();
			((Control)btOk).Enabled = true;
		}
	}

	private void ComboBlobContentType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (BlobContentType == "CSV")
		{
			panelCsvOptionsShouldBeVisible = true;
			panelExcelOptionsShouldBeVisible = false;
		}
		else if (BlobContentType == "Excel")
		{
			panelCsvOptionsShouldBeVisible = false;
			panelExcelOptionsShouldBeVisible = true;
		}
		else
		{
			panelCsvOptionsShouldBeVisible = false;
			panelExcelOptionsShouldBeVisible = false;
		}
		RedrawContentOptionsPanels();
	}

	private void ComboBoxCsvDelimiter_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)tbCsvCustomDelimeter).Visible = ((ComboBox)cbCsvDelimiter).SelectedItem.ToString().Equals(csvCustomDelimiterOptionName);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = SetContolErrorAndFocusIfEmpty((Control)(object)cbBlobContentType, (Label)(object)labelBlobContentType) | SetContolErrorAndFocusIfEmpty((Control)(object)cbBlobName, (Label)(object)labelBlobName) | SetContolErrorAndFocusIfEmpty((Control)(object)cbContainerName, (Label)(object)labelContainerName) | SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountKey, (Label)(object)labelAccountKey) | SetContolErrorAndFocusIfEmpty((Control)(object)tbAccountName, (Label)(object)labelAccountName);
		flag |= BlobContentType == "CSV" && ((Control)tbCsvCustomDelimeter).Visible && SetContolErrorAndFocusIfEmpty((Control)(object)tbCsvCustomDelimeter, (Label)(object)labelCsvDelimiter);
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			((Control)tbName).Focus();
			flag = true;
		}
		if (((Control)tbName).Text != originalName && StiNameCreation.IsConnectionNameExists(report, ((Control)tbName).Text))
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
			((Control)tbName).Focus();
			flag = true;
		}
		if (!flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)tbName).Focus();
		((Control)cbBlobName).TextChanged += ComboBoxBlobName_TextChanged;
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiAzureBlobStorageDatabaseEditForm));
		labelAlias = new StiLabelUX();
		labelName = new StiLabelUX();
		labelAccountName = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelAccountKey = new StiLabelUX();
		labelBlobName = new StiLabelUX();
		labelContainerName = new StiLabelUX();
		labelBlobContentType = new StiLabelUX();
		tbAccountKey = new StiTextBoxUX();
		timerContentTypeUpdateDelay = new Timer(components);
		labelCsvDelimiter = new StiLabelUX();
		labelCsvEncoding = new StiLabelUX();
		checkFirstRowIsHeader = new StiCheckBoxUX();
		tbCsvCustomDelimeter = new StiTextBox();
		cbCsvDelimiter = new StiComboBox();
		cbCsvEncoding = new StiComboBox();
		cbBlobContentType = new StiComboBox();
		cbBlobName = new StiDropDownBox();
		cbContainerName = new StiDropDownBox();
		tbAccountName = new StiTextBox();
		tbAlias = new StiTextBox();
		tbName = new StiTextBox();
		panelExcelOptions = new StiPanelUX();
		panelCsvOptions = new StiPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		glButtons = new StiButtonPanelUX();
		panelMain = new StiPanelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)panelExcelOptions).SuspendLayout();
		((Control)panelCsvOptions).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)panelMain).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(192, 20);
		((Control)labelAlias).TabIndex = 7;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(192, 20);
		((Control)labelName).TabIndex = 6;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)labelAccountName).ForeColor = Color.DimGray;
		((Control)labelAccountName).Location = new Point(12, 78);
		((Control)labelAccountName).Name = "labelAccountName";
		((Control)labelAccountName).Size = new Size(192, 20);
		((Control)labelAccountName).TabIndex = 9;
		((Control)labelAccountName).Text = "Account Name";
		((Label)labelAccountName).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelAccountKey).ForeColor = Color.DimGray;
		((Control)labelAccountKey).Location = new Point(12, 111);
		((Control)labelAccountKey).Name = "labelAccountKey";
		((Control)labelAccountKey).Size = new Size(192, 20);
		((Control)labelAccountKey).TabIndex = 13;
		((Control)labelAccountKey).Text = "Account Key";
		((Label)labelAccountKey).TextAlign = (ContentAlignment)16;
		((Control)labelBlobName).ForeColor = Color.DimGray;
		((Control)labelBlobName).Location = new Point(12, 218);
		((Control)labelBlobName).Name = "labelBlobName";
		((Control)labelBlobName).Size = new Size(192, 20);
		((Control)labelBlobName).TabIndex = 14;
		((Control)labelBlobName).Text = "Blob Name";
		((Label)labelBlobName).TextAlign = (ContentAlignment)16;
		((Control)labelContainerName).ForeColor = Color.DimGray;
		((Control)labelContainerName).Location = new Point(12, 185);
		((Control)labelContainerName).Name = "labelContainerName";
		((Control)labelContainerName).Size = new Size(192, 20);
		((Control)labelContainerName).TabIndex = 15;
		((Control)labelContainerName).Text = "Container Name";
		((Label)labelContainerName).TextAlign = (ContentAlignment)16;
		((Control)labelBlobContentType).ForeColor = Color.DimGray;
		((Control)labelBlobContentType).Location = new Point(12, 251);
		((Control)labelBlobContentType).Name = "labelBlobContentType";
		((Control)labelBlobContentType).Size = new Size(192, 20);
		((Control)labelBlobContentType).TabIndex = 16;
		((Control)labelBlobContentType).Text = "Blob Content Type";
		((Label)labelBlobContentType).TextAlign = (ContentAlignment)16;
		((Control)tbAccountKey).Anchor = (AnchorStyles)13;
		((Control)tbAccountKey).Location = new Point(210, 111);
		((TextBoxBase)tbAccountKey).Multiline = true;
		((Control)tbAccountKey).Name = "tbAccountKey";
		((Control)tbAccountKey).Size = new Size(322, 63);
		((Control)tbAccountKey).TabIndex = 3;
		timerContentTypeUpdateDelay.Interval = 1000;
		timerContentTypeUpdateDelay.Tick += TimerContentTypeUpdateDelay_Tick;
		((Control)labelCsvDelimiter).ForeColor = Color.DimGray;
		((Control)labelCsvDelimiter).Location = new Point(12, 33);
		((Control)labelCsvDelimiter).Name = "labelCsvDelimiter";
		((Control)labelCsvDelimiter).Size = new Size(192, 20);
		((Control)labelCsvDelimiter).TabIndex = 21;
		((Control)labelCsvDelimiter).Text = "Delimiter";
		((Label)labelCsvDelimiter).TextAlign = (ContentAlignment)16;
		((Control)labelCsvEncoding).ForeColor = Color.DimGray;
		((Control)labelCsvEncoding).Location = new Point(12, 0);
		((Control)labelCsvEncoding).Name = "labelCsvEncoding";
		((Control)labelCsvEncoding).Size = new Size(192, 20);
		((Control)labelCsvEncoding).TabIndex = 20;
		((Control)labelCsvEncoding).Text = "Encoding";
		((Label)labelCsvEncoding).TextAlign = (ContentAlignment)16;
		((Control)checkFirstRowIsHeader).Anchor = (AnchorStyles)13;
		((CheckBox)checkFirstRowIsHeader).Checked = true;
		((CheckBox)checkFirstRowIsHeader).CheckState = (CheckState)1;
		((Control)checkFirstRowIsHeader).Location = new Point(210, 0);
		((Control)checkFirstRowIsHeader).Name = "checkFirstRowIsHeader";
		((Control)checkFirstRowIsHeader).Size = new Size(314, 21);
		((Control)checkFirstRowIsHeader).TabIndex = 7;
		((Control)checkFirstRowIsHeader).Text = "First Row is Header";
		((Control)tbCsvCustomDelimeter).BackColor = SystemColors.Window;
		((Control)tbCsvCustomDelimeter).Location = new Point(415, 33);
		((Control)tbCsvCustomDelimeter).Name = "tbCsvCustomDelimeter";
		((Control)tbCsvCustomDelimeter).Size = new Size(117, 21);
		((Control)tbCsvCustomDelimeter).TabIndex = 9;
		cbCsvDelimiter.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCsvDelimiter).DropDownWidth = 250;
		((Control)cbCsvDelimiter).Location = new Point(210, 33);
		((ComboBox)cbCsvDelimiter).MaxDropDownItems = 20;
		((Control)cbCsvDelimiter).Name = "cbCsvDelimiter";
		((Control)cbCsvDelimiter).Size = new Size(199, 21);
		((Control)cbCsvDelimiter).TabIndex = 8;
		((ComboBox)cbCsvDelimiter).SelectedIndexChanged += ComboBoxCsvDelimiter_SelectedIndexChanged;
		cbCsvEncoding.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCsvEncoding).DropDownWidth = 250;
		((Control)cbCsvEncoding).Location = new Point(210, 0);
		((ComboBox)cbCsvEncoding).MaxDropDownItems = 20;
		((Control)cbCsvEncoding).Name = "cbCsvEncoding";
		((Control)cbCsvEncoding).Size = new Size(199, 21);
		((Control)cbCsvEncoding).TabIndex = 7;
		((Control)cbBlobContentType).BackColor = Color.White;
		cbBlobContentType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbBlobContentType).Location = new Point(210, 251);
		((Control)cbBlobContentType).Margin = new Padding(2, 2, 2, 2);
		((Control)cbBlobContentType).Name = "cbBlobContentType";
		((Control)cbBlobContentType).Size = new Size(199, 21);
		((Control)cbBlobContentType).TabIndex = 6;
		((ComboBox)cbBlobContentType).SelectedIndexChanged += ComboBlobContentType_SelectedIndexChanged;
		((Control)cbBlobName).Anchor = (AnchorStyles)13;
		((Control)cbBlobName).BackColor = Color.White;
		cbBlobName.DrawMode = (DrawMode)2;
		cbBlobName.IsEditable = true;
		((Control)cbBlobName).Location = new Point(210, 218);
		((Control)cbBlobName).Margin = new Padding(2, 2, 2, 2);
		((Control)cbBlobName).Name = "cbBlobName";
		cbBlobName.SelectedIndex = -1;
		cbBlobName.SelectedItem = null;
		cbBlobName.SelectedValue = null;
		((Control)cbBlobName).Size = new Size(199, 21);
		((Control)cbBlobName).TabIndex = 5;
		cbBlobName.DropDown += ComboBoxBlobName_DropDown;
		((Control)cbContainerName).Anchor = (AnchorStyles)13;
		((Control)cbContainerName).BackColor = Color.White;
		cbContainerName.DrawMode = (DrawMode)2;
		cbContainerName.IsEditable = true;
		((Control)cbContainerName).Location = new Point(210, 185);
		((Control)cbContainerName).Margin = new Padding(2, 2, 2, 2);
		((Control)cbContainerName).Name = "cbContainerName";
		cbContainerName.SelectedIndex = -1;
		cbContainerName.SelectedItem = null;
		cbContainerName.SelectedValue = null;
		((Control)cbContainerName).Size = new Size(199, 21);
		((Control)cbContainerName).TabIndex = 4;
		cbContainerName.DropDown += ComboBoxContainerName_DropDown;
		((Control)tbAccountName).Anchor = (AnchorStyles)13;
		((Control)tbAccountName).BackColor = SystemColors.Window;
		((Control)tbAccountName).Location = new Point(210, 78);
		((Control)tbAccountName).Name = "tbAccountName";
		((Control)tbAccountName).Size = new Size(322, 21);
		((Control)tbAccountName).TabIndex = 2;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(210, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(199, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(210, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(199, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)panelExcelOptions).Controls.Add((Control)(object)checkFirstRowIsHeader);
		((Control)panelExcelOptions).Dock = (DockStyle)1;
		((Control)panelExcelOptions).Location = new Point(0, 350);
		((Control)panelExcelOptions).Margin = new Padding(2, 2, 2, 2);
		((Control)panelExcelOptions).Name = "panelExcelOptions";
		((Control)panelExcelOptions).Size = new Size(545, 33);
		((Control)panelExcelOptions).TabIndex = 1;
		((Control)panelCsvOptions).Controls.Add((Control)(object)cbCsvDelimiter);
		((Control)panelCsvOptions).Controls.Add((Control)(object)cbCsvEncoding);
		((Control)panelCsvOptions).Controls.Add((Control)(object)tbCsvCustomDelimeter);
		((Control)panelCsvOptions).Controls.Add((Control)(object)labelCsvEncoding);
		((Control)panelCsvOptions).Controls.Add((Control)(object)labelCsvDelimiter);
		((Control)panelCsvOptions).Dock = (DockStyle)1;
		((Control)panelCsvOptions).Location = new Point(0, 284);
		((Control)panelCsvOptions).Margin = new Padding(2, 2, 2, 2);
		((Control)panelCsvOptions).Name = "panelCsvOptions";
		((Control)panelCsvOptions).Size = new Size(545, 66);
		((Control)panelCsvOptions).TabIndex = 1;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(376, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 10;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(457, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 11;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 383);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(545, 48);
		((Control)glButtons).TabIndex = 2;
		((Control)glButtons).TabStop = false;
		((Control)panelMain).Controls.Add((Control)(object)labelName);
		((Control)panelMain).Controls.Add((Control)(object)tbName);
		((Control)panelMain).Controls.Add((Control)(object)tbAlias);
		((Control)panelMain).Controls.Add((Control)(object)tbAccountKey);
		((Control)panelMain).Controls.Add((Control)(object)labelAlias);
		((Control)panelMain).Controls.Add((Control)(object)cbBlobContentType);
		((Control)panelMain).Controls.Add((Control)(object)tbAccountName);
		((Control)panelMain).Controls.Add((Control)(object)labelBlobContentType);
		((Control)panelMain).Controls.Add((Control)(object)labelAccountName);
		((Control)panelMain).Controls.Add((Control)(object)cbBlobName);
		((Control)panelMain).Controls.Add((Control)(object)labelAccountKey);
		((Control)panelMain).Controls.Add((Control)(object)cbContainerName);
		((Control)panelMain).Controls.Add((Control)(object)labelBlobName);
		((Control)panelMain).Controls.Add((Control)(object)labelContainerName);
		((Control)panelMain).Dock = (DockStyle)1;
		((Control)panelMain).Location = new Point(0, 0);
		((Control)panelMain).Margin = new Padding(2, 2, 2, 2);
		((Control)panelMain).Name = "panelMain";
		((Control)panelMain).Size = new Size(545, 284);
		((Control)panelMain).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(545, 431);
		((Control)this).Controls.Add((Control)(object)panelExcelOptions);
		((Control)this).Controls.Add((Control)(object)panelCsvOptions);
		((Control)this).Controls.Add((Control)(object)panelMain);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiAzureBlobStorageDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiAzureBlobStorageDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)panelExcelOptions).ResumeLayout(false);
		((Control)panelCsvOptions).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)panelMain).ResumeLayout(false);
		((Control)panelMain).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
