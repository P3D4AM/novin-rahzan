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

public class StiQuickBooksDatabaseEditForm : StiFormUX
{
	private StiReport report;

	private string oldValue;

	private string originalName;

	private StiQuickBooksConnector connector;

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	private StiLabelUX labelName;

	public StiTextBox tbName;

	private StiLabelUX labelAuthorizationCode;

	public StiTextBox textBoxAuthorizationCode;

	private ErrorProvider errorProvider;

	private StiLabelUX labelRealmId;

	public StiTextBox textBoxRealmId;

	private StiLabelUX labelRefreshToken;

	public StiTextBox textBoxRefreshToken;

	private StiLabelUX labelAccessToken;

	public StiTextBox textBoxAccessToken;

	private StiButtonUX buttonGetTokens;

	private StiButtonUX buttonGetAuthorizationCode;

	private StiLabelUX labelClientId;

	public StiTextBox textBoxClientId;

	private StiLabelUX labelClientSecret;

	public StiTextBox textBoxClientSecret;

	private StiCheckBoxUX checkBoxUseApp;

	private StiLabelUX labelExpiresAccessToken;

	private StiButtonUX buttonRefreshAccessToken;

	private StiLabelUX labelExpiresRefreshToken;

	private StiLabelUX labelRedirectUrl;

	public StiTextBox textBoxRedirectUrl;

	public StiQuickBooksDatabaseEditForm(StiReport report, string originalName, StiQuickBooksConnector connector)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			this.originalName = originalName;
			this.connector = connector;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			DoScaling();
			((CheckBox)checkBoxUseApp).Checked = connector.UseApp;
			((Control)textBoxClientId).Text = connector.ClientId;
			((Control)textBoxClientSecret).Text = connector.ClientSecret;
			((Control)textBoxRedirectUrl).Text = connector.RedirectURL;
			((Control)textBoxAuthorizationCode).Text = connector.AuthorizationCode;
			((Control)textBoxRealmId).Text = connector.RealmId;
			((Control)textBoxAccessToken).Text = connector.AccessToken;
			((Control)textBoxRefreshToken).Text = connector.RefreshToken;
			UpdateControlState();
		}
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
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

	private void UpdateControlState()
	{
		((CheckBox)checkBoxUseApp).Checked = connector.UseApp;
		((Control)textBoxClientId).Text = connector.ClientId;
		((Control)textBoxClientSecret).Text = connector.ClientSecret;
		((Control)textBoxRedirectUrl).Text = connector.RedirectURL;
		((Control)textBoxAuthorizationCode).Text = connector.AuthorizationCode;
		((Control)textBoxRealmId).Text = connector.RealmId;
		((Control)textBoxAccessToken).Text = connector.AccessToken;
		((Control)textBoxRefreshToken).Text = connector.RefreshToken;
		((Control)textBoxClientId).Enabled = connector.UseApp;
		((Control)textBoxClientSecret).Enabled = connector.UseApp;
		((Control)textBoxRedirectUrl).Enabled = connector.UseApp;
		((Control)buttonGetAuthorizationCode).Enabled = !connector.UseApp || (!string.IsNullOrWhiteSpace(connector.ClientId) && !string.IsNullOrWhiteSpace(connector.ClientSecret) && !string.IsNullOrWhiteSpace(connector.RedirectURL));
		((Control)buttonGetTokens).Enabled = !string.IsNullOrWhiteSpace(connector.AuthorizationCode) && !string.IsNullOrWhiteSpace(connector.RealmId);
		((Control)buttonRefreshAccessToken).Enabled = !string.IsNullOrWhiteSpace(connector.RefreshToken);
		((Control)btOk).Enabled = IsEditMode() || ((!connector.UseApp || (!string.IsNullOrWhiteSpace(connector.ClientId) && !string.IsNullOrWhiteSpace(connector.ClientSecret) && !string.IsNullOrWhiteSpace(connector.RedirectURL))) && !string.IsNullOrWhiteSpace(connector.AuthorizationCode) && !string.IsNullOrWhiteSpace(connector.RealmId) && !string.IsNullOrWhiteSpace(connector.AccessToken));
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

	private void CheckBoxUseApp_CheckedChanged(object sender, EventArgs e)
	{
		connector.UseApp = ((CheckBox)checkBoxUseApp).Checked;
		UpdateControlState();
	}

	private void ButtonGetAuthorizationCode_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			connector.FillAuthorizationCode();
			UpdateControlState();
		}
		catch
		{
			MessageBox.Show("Error get authorization code", "Error", (MessageBoxButtons)0, (MessageBoxIcon)48);
		}
	}

	private void ButtonGetTokens_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			connector.FillTokens();
			UpdateControlState();
		}
		catch
		{
			MessageBox.Show("Error get access token", "Error", (MessageBoxButtons)0, (MessageBoxIcon)48);
		}
	}

	private void ButtonRefreshAccessToken_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			connector.RefreshAccessToken();
			UpdateControlState();
		}
		catch
		{
			MessageBox.Show("Error refresh access token", "Error", (MessageBoxButtons)0, (MessageBoxIcon)48);
		}
	}

	private void TextBoxClientId_TextChanged(object sender, EventArgs e)
	{
		connector.ClientId = ((Control)textBoxClientId).Text;
		UpdateControlState();
	}

	private void TextBoxClientSecret_TextChanged(object sender, EventArgs e)
	{
		connector.ClientSecret = ((Control)textBoxClientSecret).Text;
		UpdateControlState();
	}

	private void TextBoxRedirectUrl_TextChanged(object sender, EventArgs e)
	{
		connector.RedirectURL = ((Control)textBoxRedirectUrl).Text;
		UpdateControlState();
	}

	private void TextBoxAuthorizationCode_TextChanged(object sender, EventArgs e)
	{
		connector.AuthorizationCode = ((Control)textBoxAuthorizationCode).Text;
		UpdateControlState();
	}

	private void TextBoxRealmId_TextChanged(object sender, EventArgs e)
	{
		connector.RealmId = ((Control)textBoxRealmId).Text;
		UpdateControlState();
	}

	private void TextBoxAccessToken_TextChanged(object sender, EventArgs e)
	{
		connector.AccessToken = ((Control)textBoxAccessToken).Text;
		UpdateControlState();
	}

	private void TextBoxRefreshToken_TextChanged(object sender, EventArgs e)
	{
		connector.RefreshToken = ((Control)textBoxRefreshToken).Text;
		UpdateControlState();
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_130e: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		labelAlias = new StiLabelUX();
		labelName = new StiLabelUX();
		labelAuthorizationCode = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelRealmId = new StiLabelUX();
		checkBoxUseApp = new StiCheckBoxUX();
		labelClientId = new StiLabelUX();
		labelClientSecret = new StiLabelUX();
		buttonGetAuthorizationCode = new StiButtonUX();
		buttonGetTokens = new StiButtonUX();
		labelRefreshToken = new StiLabelUX();
		labelAccessToken = new StiLabelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		textBoxRefreshToken = new StiTextBox();
		textBoxAccessToken = new StiTextBox();
		textBoxClientId = new StiTextBox();
		textBoxClientSecret = new StiTextBox();
		textBoxRealmId = new StiTextBox();
		textBoxAuthorizationCode = new StiTextBox();
		tbAlias = new StiTextBox();
		tbName = new StiTextBox();
		glButtons = new StiButtonPanelUX();
		labelExpiresAccessToken = new StiLabelUX();
		labelExpiresRefreshToken = new StiLabelUX();
		buttonRefreshAccessToken = new StiButtonUX();
		labelRedirectUrl = new StiLabelUX();
		textBoxRedirectUrl = new StiTextBox();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(24, 88);
		((Control)labelAlias).Margin = new Padding(6, 0, 6, 0);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(268, 38);
		((Control)labelAlias).TabIndex = 2;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(24, 23);
		((Control)labelName).Margin = new Padding(6, 0, 6, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(268, 38);
		((Control)labelName).TabIndex = 0;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)labelAuthorizationCode).ForeColor = Color.DimGray;
		((Control)labelAuthorizationCode).Location = new Point(24, 442);
		((Control)labelAuthorizationCode).Margin = new Padding(6, 0, 6, 0);
		((Control)labelAuthorizationCode).Name = "labelAuthorizationCode";
		((Control)labelAuthorizationCode).Size = new Size(268, 38);
		((Control)labelAuthorizationCode).TabIndex = 12;
		((Control)labelAuthorizationCode).Text = "Authorization Code";
		((Label)labelAuthorizationCode).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		((Control)labelRealmId).ForeColor = Color.DimGray;
		((Control)labelRealmId).Location = new Point(24, 506);
		((Control)labelRealmId).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRealmId).Name = "labelRealmId";
		((Control)labelRealmId).Size = new Size(268, 38);
		((Control)labelRealmId).TabIndex = 14;
		((Control)labelRealmId).Text = "RealmId";
		((Label)labelRealmId).TextAlign = (ContentAlignment)16;
		((Control)checkBoxUseApp).AutoSize = true;
		((Control)checkBoxUseApp).Location = new Point(304, 146);
		((Control)checkBoxUseApp).Margin = new Padding(4, 4, 4, 4);
		((Control)checkBoxUseApp).Name = "checkBoxUseApp";
		((Control)checkBoxUseApp).Size = new Size(413, 29);
		((Control)checkBoxUseApp).TabIndex = 4;
		((Control)checkBoxUseApp).Text = "Use own \"Client ID\" and \"Client Secret\"";
		((ButtonBase)checkBoxUseApp).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxUseApp).CheckedChanged += CheckBoxUseApp_CheckedChanged;
		((Control)labelClientId).ForeColor = Color.DimGray;
		((Control)labelClientId).Location = new Point(22, 196);
		((Control)labelClientId).Margin = new Padding(6, 0, 6, 0);
		((Control)labelClientId).Name = "labelClientId";
		((Control)labelClientId).Size = new Size(268, 38);
		((Control)labelClientId).TabIndex = 5;
		((Control)labelClientId).Text = "Client Id";
		((Label)labelClientId).TextAlign = (ContentAlignment)16;
		((Control)labelClientSecret).ForeColor = Color.DimGray;
		((Control)labelClientSecret).Location = new Point(24, 258);
		((Control)labelClientSecret).Margin = new Padding(6, 0, 6, 0);
		((Control)labelClientSecret).Name = "labelClientSecret";
		((Control)labelClientSecret).Size = new Size(268, 38);
		((Control)labelClientSecret).TabIndex = 7;
		((Control)labelClientSecret).Text = "Client Secret";
		((Label)labelClientSecret).TextAlign = (ContentAlignment)16;
		((Control)buttonGetAuthorizationCode).Location = new Point(304, 377);
		((Control)buttonGetAuthorizationCode).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonGetAuthorizationCode).Name = "buttonGetAuthorizationCode";
		((Control)buttonGetAuthorizationCode).Size = new Size(248, 46);
		((Control)buttonGetAuthorizationCode).TabIndex = 11;
		((Control)buttonGetAuthorizationCode).Text = "Get authorization code";
		((ButtonBase)buttonGetAuthorizationCode).UseVisualStyleBackColor = true;
		((Control)buttonGetAuthorizationCode).Click += ButtonGetAuthorizationCode_Click;
		((Control)buttonGetTokens).Location = new Point(304, 563);
		((Control)buttonGetTokens).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonGetTokens).Name = "buttonGetTokens";
		((Control)buttonGetTokens).Size = new Size(248, 46);
		((Control)buttonGetTokens).TabIndex = 16;
		((Control)buttonGetTokens).Text = "Get tokens";
		((ButtonBase)buttonGetTokens).UseVisualStyleBackColor = true;
		((Control)buttonGetTokens).Click += ButtonGetTokens_Click;
		((Control)labelRefreshToken).ForeColor = Color.DimGray;
		((Control)labelRefreshToken).Location = new Point(22, 717);
		((Control)labelRefreshToken).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRefreshToken).Name = "labelRefreshToken";
		((Control)labelRefreshToken).Size = new Size(268, 38);
		((Control)labelRefreshToken).TabIndex = 20;
		((Control)labelRefreshToken).Text = "Refresh token";
		((Label)labelRefreshToken).TextAlign = (ContentAlignment)16;
		((Control)labelAccessToken).ForeColor = Color.DimGray;
		((Control)labelAccessToken).Location = new Point(24, 629);
		((Control)labelAccessToken).Margin = new Padding(6, 0, 6, 0);
		((Control)labelAccessToken).Name = "labelAccessToken";
		((Control)labelAccessToken).Size = new Size(268, 38);
		((Control)labelAccessToken).TabIndex = 17;
		((Control)labelAccessToken).Text = "Access token";
		((Label)labelAccessToken).TextAlign = (ContentAlignment)16;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(754, 23);
		((Control)btOk).Margin = new Padding(6, 6, 6, 6);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 46);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(916, 23);
		((Control)btCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 46);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)textBoxRefreshToken).Anchor = (AnchorStyles)13;
		((Control)textBoxRefreshToken).BackColor = SystemColors.Window;
		((Control)textBoxRefreshToken).Location = new Point(304, 717);
		((Control)textBoxRefreshToken).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxRefreshToken).Name = "textBoxRefreshToken";
		((Control)textBoxRefreshToken).Size = new Size(742, 42);
		((Control)textBoxRefreshToken).TabIndex = 21;
		((Control)textBoxRefreshToken).TextChanged += TextBoxRefreshToken_TextChanged;
		((Control)textBoxAccessToken).Anchor = (AnchorStyles)13;
		((Control)textBoxAccessToken).BackColor = SystemColors.Window;
		((Control)textBoxAccessToken).Location = new Point(304, 629);
		((Control)textBoxAccessToken).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxAccessToken).Name = "textBoxAccessToken";
		((Control)textBoxAccessToken).Size = new Size(742, 42);
		((Control)textBoxAccessToken).TabIndex = 18;
		((Control)textBoxAccessToken).TextChanged += TextBoxAccessToken_TextChanged;
		((Control)textBoxClientId).Anchor = (AnchorStyles)13;
		((Control)textBoxClientId).BackColor = SystemColors.Window;
		((Control)textBoxClientId).Location = new Point(304, 196);
		((Control)textBoxClientId).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxClientId).Name = "textBoxClientId";
		((Control)textBoxClientId).Size = new Size(742, 42);
		((Control)textBoxClientId).TabIndex = 6;
		((Control)textBoxClientId).TextChanged += TextBoxClientId_TextChanged;
		((Control)textBoxClientSecret).Anchor = (AnchorStyles)13;
		((Control)textBoxClientSecret).BackColor = SystemColors.Window;
		((Control)textBoxClientSecret).Location = new Point(304, 258);
		((Control)textBoxClientSecret).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxClientSecret).Name = "textBoxClientSecret";
		((Control)textBoxClientSecret).Size = new Size(742, 42);
		((Control)textBoxClientSecret).TabIndex = 8;
		((Control)textBoxClientSecret).TextChanged += TextBoxClientSecret_TextChanged;
		((Control)textBoxRealmId).Anchor = (AnchorStyles)13;
		((Control)textBoxRealmId).BackColor = SystemColors.Window;
		((Control)textBoxRealmId).Location = new Point(304, 504);
		((Control)textBoxRealmId).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxRealmId).Name = "textBoxRealmId";
		((Control)textBoxRealmId).Size = new Size(742, 42);
		((Control)textBoxRealmId).TabIndex = 15;
		((Control)textBoxRealmId).TextChanged += TextBoxRealmId_TextChanged;
		((Control)textBoxAuthorizationCode).Anchor = (AnchorStyles)13;
		((Control)textBoxAuthorizationCode).BackColor = SystemColors.Window;
		((Control)textBoxAuthorizationCode).Location = new Point(304, 442);
		((Control)textBoxAuthorizationCode).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxAuthorizationCode).Name = "textBoxAuthorizationCode";
		((Control)textBoxAuthorizationCode).Size = new Size(742, 42);
		((Control)textBoxAuthorizationCode).TabIndex = 13;
		((Control)textBoxAuthorizationCode).TextChanged += TextBoxAuthorizationCode_TextChanged;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(304, 88);
		((Control)tbAlias).Margin = new Padding(6, 6, 6, 6);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(460, 42);
		((Control)tbAlias).TabIndex = 3;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(304, 23);
		((Control)tbName).Margin = new Padding(6, 6, 6, 6);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(460, 42);
		((Control)tbName).TabIndex = 1;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 866);
		((Control)glButtons).Margin = new Padding(6, 6, 6, 6);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(1090, 88);
		((Control)glButtons).TabIndex = 24;
		((Control)labelExpiresAccessToken).ForeColor = Color.DimGray;
		((Control)labelExpiresAccessToken).Location = new Point(298, 673);
		((Control)labelExpiresAccessToken).Margin = new Padding(6, 0, 6, 0);
		((Control)labelExpiresAccessToken).Name = "labelExpiresAccessToken";
		((Control)labelExpiresAccessToken).Size = new Size(748, 38);
		((Control)labelExpiresAccessToken).TabIndex = 19;
		((Control)labelExpiresAccessToken).Text = "expires in 60 minutes";
		((Label)labelExpiresAccessToken).TextAlign = (ContentAlignment)4;
		((Control)labelExpiresRefreshToken).ForeColor = Color.DimGray;
		((Control)labelExpiresRefreshToken).Location = new Point(298, 762);
		((Control)labelExpiresRefreshToken).Margin = new Padding(6, 0, 6, 0);
		((Control)labelExpiresRefreshToken).Name = "labelExpiresRefreshToken";
		((Control)labelExpiresRefreshToken).Size = new Size(748, 38);
		((Control)labelExpiresRefreshToken).TabIndex = 22;
		((Control)labelExpiresRefreshToken).Text = "expires in 100 days";
		((Label)labelExpiresRefreshToken).TextAlign = (ContentAlignment)4;
		((Control)buttonRefreshAccessToken).Location = new Point(304, 775);
		((Control)buttonRefreshAccessToken).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonRefreshAccessToken).Name = "buttonRefreshAccessToken";
		((Control)buttonRefreshAccessToken).Size = new Size(248, 46);
		((Control)buttonRefreshAccessToken).TabIndex = 23;
		((Control)buttonRefreshAccessToken).Text = "Refresh Access Token";
		((ButtonBase)buttonRefreshAccessToken).UseVisualStyleBackColor = true;
		((Control)buttonRefreshAccessToken).Click += ButtonRefreshAccessToken_Click;
		((Control)labelRedirectUrl).ForeColor = Color.DimGray;
		((Control)labelRedirectUrl).Location = new Point(24, 319);
		((Control)labelRedirectUrl).Margin = new Padding(6, 0, 6, 0);
		((Control)labelRedirectUrl).Name = "labelRedirectUrl";
		((Control)labelRedirectUrl).Size = new Size(268, 38);
		((Control)labelRedirectUrl).TabIndex = 9;
		((Control)labelRedirectUrl).Text = "Redirect URL";
		((Label)labelRedirectUrl).TextAlign = (ContentAlignment)16;
		((Control)textBoxRedirectUrl).Anchor = (AnchorStyles)13;
		((Control)textBoxRedirectUrl).BackColor = SystemColors.Window;
		((Control)textBoxRedirectUrl).Location = new Point(304, 319);
		((Control)textBoxRedirectUrl).Margin = new Padding(6, 6, 6, 6);
		((Control)textBoxRedirectUrl).Name = "textBoxRedirectUrl";
		((Control)textBoxRedirectUrl).Size = new Size(742, 42);
		((Control)textBoxRedirectUrl).TabIndex = 10;
		((Control)textBoxRedirectUrl).TextChanged += TextBoxRedirectUrl_TextChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(1090, 954);
		((Control)this).Controls.Add((Control)(object)labelRedirectUrl);
		((Control)this).Controls.Add((Control)(object)textBoxRedirectUrl);
		((Control)this).Controls.Add((Control)(object)buttonRefreshAccessToken);
		((Control)this).Controls.Add((Control)(object)labelExpiresRefreshToken);
		((Control)this).Controls.Add((Control)(object)labelExpiresAccessToken);
		((Control)this).Controls.Add((Control)(object)labelRefreshToken);
		((Control)this).Controls.Add((Control)(object)textBoxRefreshToken);
		((Control)this).Controls.Add((Control)(object)labelAccessToken);
		((Control)this).Controls.Add((Control)(object)textBoxAccessToken);
		((Control)this).Controls.Add((Control)(object)buttonGetTokens);
		((Control)this).Controls.Add((Control)(object)buttonGetAuthorizationCode);
		((Control)this).Controls.Add((Control)(object)labelClientId);
		((Control)this).Controls.Add((Control)(object)textBoxClientId);
		((Control)this).Controls.Add((Control)(object)labelClientSecret);
		((Control)this).Controls.Add((Control)(object)textBoxClientSecret);
		((Control)this).Controls.Add((Control)(object)checkBoxUseApp);
		((Control)this).Controls.Add((Control)(object)labelRealmId);
		((Control)this).Controls.Add((Control)(object)textBoxRealmId);
		((Control)this).Controls.Add((Control)(object)labelAuthorizationCode);
		((Control)this).Controls.Add((Control)(object)textBoxAuthorizationCode);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiQuickBooksDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiQuickBooksDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
