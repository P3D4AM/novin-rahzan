using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Export.Tools;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiPdfExportSetupForm : Form
{
	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox groupBoxExportSettings;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiLabelInfo labelResolution;

	private StiComboBox comboBoxImageResolution;

	private StiCheckBoxInfo checkBoxEmbeddedFonts;

	private StiGroupLine glButtons;

	private StiCheckBoxInfo checkBoxExportRtfTextAsImage;

	private ButtonX buttonDocumentSecurity;

	private ControlContainerItem controlContainerItem1;

	private StiDocumentSecurityUserControl documentSecurity;

	private Label label6;

	private StiComboBox comboBoxImageCompressionMethod;

	private StiLabelInfo labelCompressionMethod;

	private StiCheckBoxInfo checkBoxPdfACompliance;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	private StiComboBox comboBoxAllowEditable;

	private StiLabelInfo labelAllowEditable;

	private StiComboBox comboBoxImageResolutionMode;

	private StiLabelInfo labelResolutionMode;

	private StiComboBox comboBoxComplianceMode;

	private Panel panelOpenAfterExport;

	private Button buttonGetDigitalSignature;

	internal StiTextBox textBoxDigitalSignature;

	internal StiCheckBoxInfo checkBoxDigitalSignature;

	private ToolTip toolTip1;

	public StiPagesRange PagesRange => PagesRangeControl.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

	public int CurrentPage
	{
		get
		{
			return PagesRangeControl.CurrentPage;
		}
		set
		{
			PagesRangeControl.CurrentPage = value;
		}
	}

	public bool OpenAfterExportVisible
	{
		get
		{
			return ((Control)checkBoxOpenAfterExport).Visible;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Visible = value;
		}
	}

	public bool OpenAfterExportEnabled
	{
		get
		{
			return ((Control)checkBoxOpenAfterExport).Enabled;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Enabled = value;
		}
	}

	public float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	public StiImageResolutionMode ImageResolutionMode
	{
		get
		{
			if (((ListControl)comboBoxImageResolutionMode).SelectedIndex == 0)
			{
				return StiImageResolutionMode.Exactly;
			}
			if (((ListControl)comboBoxImageResolutionMode).SelectedIndex == 1)
			{
				return StiImageResolutionMode.NoMoreThan;
			}
			if (((ListControl)comboBoxImageResolutionMode).SelectedIndex == 2)
			{
				return StiImageResolutionMode.Auto;
			}
			return StiImageResolutionMode.Exactly;
		}
	}

	public StiPdfImageCompressionMethod ImageCompressionMethod
	{
		get
		{
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 0)
			{
				return StiPdfImageCompressionMethod.Jpeg;
			}
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 2)
			{
				return StiPdfImageCompressionMethod.Indexed;
			}
			return StiPdfImageCompressionMethod.Flate;
		}
	}

	public StiImageFormat ImageFormat
	{
		get
		{
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex <= 2)
			{
				return StiImageFormat.Color;
			}
			return StiImageFormat.Monochrome;
		}
	}

	public StiMonochromeDitheringType MonochromeDitheringType
	{
		get
		{
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 3)
			{
				return StiMonochromeDitheringType.None;
			}
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 4)
			{
				return StiMonochromeDitheringType.Ordered;
			}
			if (((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 5)
			{
				return StiMonochromeDitheringType.FloydSteinberg;
			}
			return StiMonochromeDitheringType.None;
		}
	}

	public float ImageQuality
	{
		get
		{
			if (int.TryParse(((Control)comboBoxImageQuality).Text, out var result))
			{
				return (float)result / 100f;
			}
			return 1f;
		}
	}

	public bool EmbeddedFonts => ((CheckBox)checkBoxEmbeddedFonts).Checked;

	public bool EmbeddedFontsEnabled => ((Control)checkBoxEmbeddedFonts).Enabled;

	public StiUserAccessPrivileges UserAccessPrivileges
	{
		get
		{
			StiUserAccessPrivileges stiUserAccessPrivileges = StiUserAccessPrivileges.None;
			if (((CheckBox)documentSecurity.checkBoxCopyTextAndGraphics).Checked)
			{
				stiUserAccessPrivileges |= StiUserAccessPrivileges.CopyTextAndGraphics;
			}
			if (((CheckBox)documentSecurity.checkBoxModifyContents).Checked)
			{
				stiUserAccessPrivileges |= StiUserAccessPrivileges.ModifyContents;
			}
			if (((CheckBox)documentSecurity.checkBoxPrintDocument).Checked)
			{
				stiUserAccessPrivileges |= StiUserAccessPrivileges.PrintDocument;
			}
			if (((CheckBox)documentSecurity.checkBoxAddOrModifyTextAnnotations).Checked)
			{
				stiUserAccessPrivileges |= StiUserAccessPrivileges.AddOrModifyTextAnnotations;
			}
			return stiUserAccessPrivileges;
		}
	}

	public string UserPassword => ((Control)documentSecurity.textBoxUserPassword).Text;

	public string OwnerPassword => ((Control)documentSecurity.textBoxOwnerPassword).Text;

	public bool ExportRtfTextAsImage => ((CheckBox)checkBoxExportRtfTextAsImage).Checked;

	public string SubjectNameString => ((Control)textBoxDigitalSignature).Text;

	public bool GetCertificateFromCryptoUI => false;

	public bool UseDigitalSignature => ((CheckBox)checkBoxDigitalSignature).Checked;

	public StiPdfEncryptionKeyLength EncryptionKeyLength => ((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex switch
	{
		1 => StiPdfEncryptionKeyLength.Bit128, 
		2 => StiPdfEncryptionKeyLength.Bit128_r4, 
		3 => StiPdfEncryptionKeyLength.Bit256_r5, 
		4 => StiPdfEncryptionKeyLength.Bit256_r6, 
		_ => StiPdfEncryptionKeyLength.Bit40, 
	};

	public StiPdfComplianceMode PdfComplianceMode
	{
		get
		{
			if (((CheckBox)checkBoxPdfACompliance).Checked)
			{
				switch (((ListControl)comboBoxComplianceMode).SelectedIndex)
				{
				case 0:
					return StiPdfComplianceMode.A1;
				case 1:
					return StiPdfComplianceMode.A2;
				case 2:
					return StiPdfComplianceMode.A3;
				}
			}
			return StiPdfComplianceMode.None;
		}
	}

	public StiPdfAllowEditable AllowEditable => (StiPdfAllowEditable)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxAllowEditable).SelectedItem).Tag;

	public string CertificateThumbprint { get; set; }

	public StiPdfExportSetupForm()
	{
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
		{
			Form val = Application.OpenForms[((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1];
			if (val != null && val.TopMost)
			{
				((Form)this).TopMost = true;
			}
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		Localize();
		Build();
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		((CheckBox)checkBoxEmbeddedFonts).Checked = StiSettings.GetBool(((Control)this).Name, "EmbeddedFonts", ((CheckBox)checkBoxEmbeddedFonts).Checked);
		((CheckBox)documentSecurity.checkBoxPrintDocument).Checked = StiSettings.GetBool(((Control)this).Name, "PrintDocument", ((CheckBox)documentSecurity.checkBoxPrintDocument).Checked);
		((CheckBox)documentSecurity.checkBoxModifyContents).Checked = StiSettings.GetBool(((Control)this).Name, "ModifyContents", ((CheckBox)documentSecurity.checkBoxModifyContents).Checked);
		((CheckBox)documentSecurity.checkBoxCopyTextAndGraphics).Checked = StiSettings.GetBool(((Control)this).Name, "CopyTextAndGraphics", ((CheckBox)documentSecurity.checkBoxCopyTextAndGraphics).Checked);
		((CheckBox)documentSecurity.checkBoxAddOrModifyTextAnnotations).Checked = StiSettings.GetBool(((Control)this).Name, "AddOrModifyTextAnnotations", ((CheckBox)documentSecurity.checkBoxAddOrModifyTextAnnotations).Checked);
		((CheckBox)checkBoxExportRtfTextAsImage).Checked = StiSettings.GetBool(((Control)this).Name, "ExportRtfTextAsImage", ((CheckBox)checkBoxExportRtfTextAsImage).Checked);
		((Control)comboBoxImageResolution).Handle.ToString();
		((ListControl)comboBoxImageResolution).SelectedIndex = 2;
		((ComboBox)comboBoxImageResolution).SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		((ListControl)comboBoxImageCompressionMethod).SelectedIndex = 0;
		((ComboBox)comboBoxImageCompressionMethod).SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageCompressionMethod", ((Control)comboBoxImageCompressionMethod).Text);
		((ListControl)comboBoxImageResolutionMode).SelectedIndex = 0;
		((ComboBox)comboBoxImageResolutionMode).SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolutionMode", ((Control)comboBoxImageResolutionMode).Text);
		((Control)documentSecurity.comboBoxEncryptionKeyLength).Handle.ToString();
		if (((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex == -1)
		{
			((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex = 0;
		}
		((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "EncryptionKeyLength", ((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex);
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "75");
		((CheckBox)checkBoxPdfACompliance).Checked = StiSettings.GetBool(((Control)this).Name, "PdfACompliance", ((CheckBox)checkBoxPdfACompliance).Checked);
		((Control)comboBoxComplianceMode).Enabled = ((CheckBox)checkBoxPdfACompliance).Checked;
		if (((ListControl)comboBoxComplianceMode).SelectedIndex == -1)
		{
			((ListControl)comboBoxComplianceMode).SelectedIndex = 0;
		}
		((ListControl)comboBoxComplianceMode).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ComplianceMode", ((ListControl)comboBoxComplianceMode).SelectedIndex);
		int @int = StiSettings.GetInt(((Control)this).Name, "ImageResolutionMode", 0);
		if (@int < ((ComboBox)comboBoxImageResolutionMode).Items.Count)
		{
			((ListControl)comboBoxImageResolutionMode).SelectedIndex = @int;
		}
		int int2 = StiSettings.GetInt(((Control)this).Name, "AllowEditable", 1);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxAllowEditable).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(int2))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxAllowEditable).SelectedIndex = ((num < ((ComboBox)comboBoxAllowEditable).Items.Count) ? num : 0);
		if (!VisualStyleInformation.IsEnabledByUser)
		{
			((PopupItemControl)buttonDocumentSecurity).ThemeAware = false;
			((PopupItemControl)buttonDocumentSecurity).Style = (eDotNetBarStyle)0;
		}
		RefreshDigitalCertificateStatus();
		LocalizeSuperToolTip();
		DoScaling();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "EmbeddedFonts", ((CheckBox)checkBoxEmbeddedFonts).Checked);
		StiSettings.Set(((Control)this).Name, "PdfACompliance", ((CheckBox)checkBoxPdfACompliance).Checked);
		StiSettings.Set(((Control)this).Name, "ComplianceMode", ((ListControl)comboBoxComplianceMode).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "PrintDocument", ((CheckBox)documentSecurity.checkBoxPrintDocument).Checked);
		StiSettings.Set(((Control)this).Name, "ModifyContents", ((CheckBox)documentSecurity.checkBoxModifyContents).Checked);
		StiSettings.Set(((Control)this).Name, "CopyTextAndGraphics", ((CheckBox)documentSecurity.checkBoxCopyTextAndGraphics).Checked);
		StiSettings.Set(((Control)this).Name, "AddOrModifyTextAnnotations", ((CheckBox)documentSecurity.checkBoxAddOrModifyTextAnnotations).Checked);
		StiSettings.Set(((Control)this).Name, "ExportRtfTextAsImage", ((CheckBox)checkBoxExportRtfTextAsImage).Checked);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageResolutionMode", ((ComboBox)comboBoxImageResolutionMode).SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageCompressionMethod", ((ComboBox)comboBoxImageCompressionMethod).SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text);
		StiSettings.Set(((Control)this).Name, "EncryptionKeyLength", ((ListControl)documentSecurity.comboBoxEncryptionKeyLength).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "AllowEditable", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxAllowEditable).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "ImageResolutionMode", ((ListControl)comboBoxImageResolutionMode).SelectedIndex);
		StiSettings.Save();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (((CheckBox)checkBoxDigitalSignature).Checked && string.IsNullOrWhiteSpace(((Control)textBoxDigitalSignature).Text))
		{
			MessageBox.Show("Please specify a digital certificate!");
			((Control)textBoxDigitalSignature).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_formats_with_fixed_page_layout_pdf.htm");
	}

	private void ComboBoxImageCompressionMethod_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)comboBoxImageQuality).Enabled = ((ListControl)comboBoxImageCompressionMethod).SelectedIndex == 0;
	}

	private void CheckBoxPdfaCompliance_CheckedChanged(object sender, EventArgs e)
	{
		if (((CheckBox)checkBoxPdfACompliance).Checked)
		{
			((CheckBox)checkBoxEmbeddedFonts).Checked = true;
		}
		((Control)checkBoxEmbeddedFonts).Enabled = !((CheckBox)checkBoxPdfACompliance).Checked;
		((Control)buttonDocumentSecurity).Enabled = !((CheckBox)checkBoxPdfACompliance).Checked;
		((Control)comboBoxComplianceMode).Enabled = ((CheckBox)checkBoxPdfACompliance).Checked;
	}

	private void ButtonGetDigitalCertificate_Click(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		string certificateName = null;
		string errorMessage = null;
		string certificateThumbprintFromCryptoUI = StiPdfSecurity.GetCertificateThumbprintFromCryptoUI(useLocalMachineCertificates: false, out certificateName, out errorMessage);
		if (!string.IsNullOrWhiteSpace(errorMessage))
		{
			MessageBox.Show(errorMessage);
		}
		if (certificateThumbprintFromCryptoUI != null)
		{
			CertificateThumbprint = certificateThumbprintFromCryptoUI;
			((Control)textBoxDigitalSignature).Text = certificateName;
			((StiEditBase)textBoxDigitalSignature).ReadOnly = true;
		}
	}

	private void CheckBoxDigitalSignature_CheckedChanged(object sender, EventArgs e)
	{
		RefreshDigitalCertificateStatus();
		if (((CheckBox)checkBoxDigitalSignature).Checked && string.IsNullOrWhiteSpace(((Control)textBoxDigitalSignature).Text))
		{
			ButtonGetDigitalCertificate_Click(sender, e);
		}
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypePdfFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)labelImageQuality).Text = Loc.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)labelResolutionMode).Text = Loc.GetCleaned("Export", "ImageResolutionMode");
		((Control)labelCompressionMethod).Text = Loc.GetCleaned("Export", "ImageCompressionMethod");
		((Control)labelAllowEditable).Text = Loc.GetCleaned("Export", "AllowEditable");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxEmbeddedFonts).Text = StiLocalization.Get("Export", "EmbeddedFonts");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)buttonDocumentSecurity).Text = StiLocalization.Get("Export", "DocumentSecurity");
		((Control)documentSecurity.labelUserPassword).Text = Loc.GetCleaned("Export", "labelUserPassword");
		((Control)documentSecurity.labelOwnerPassword).Text = Loc.GetCleaned("Export", "labelOwnerPassword");
		((Control)documentSecurity.labelEncryptionKeyLength).Text = Loc.GetCleaned("Export", "labelEncryptionKeyLength");
		((Control)documentSecurity.checkBoxCopyTextAndGraphics).Text = StiLocalization.Get("Export", "AllowCopyTextAndGraphics");
		((Control)documentSecurity.checkBoxModifyContents).Text = StiLocalization.Get("Export", "AllowModifyContents");
		((Control)documentSecurity.checkBoxPrintDocument).Text = StiLocalization.Get("Export", "AllowPrintDocument");
		((Control)documentSecurity.checkBoxAddOrModifyTextAnnotations).Text = StiLocalization.Get("Export", "AllowAddOrModifyTextAnnotations");
		((Control)checkBoxExportRtfTextAsImage).Text = StiLocalization.Get("Export", "ExportRtfTextAsImage");
		((Control)checkBoxPdfACompliance).Text = StiLocalization.Get("Export", "PdfACompliance");
		((Control)checkBoxDigitalSignature).Text = StiLocalization.Get("Export", "DigitalSignature");
		toolTip1.SetToolTip((Control)(object)buttonGetDigitalSignature, StiLocalization.Get("Export", "GetCertificateFromCryptoUI"));
		checkBoxDigitalSignature.SetToolTip(StiLocalization.Get("HelpViewer", "DigitalSignature"));
		((ComboBox)comboBoxImageResolutionMode).BeginUpdate();
		((ComboBox)comboBoxImageResolutionMode).Items.Clear();
		((ComboBox)comboBoxImageResolutionMode).Items.Add((object)StiLocalization.Get("Export", "Exactly"));
		((ComboBox)comboBoxImageResolutionMode).Items.Add((object)StiLocalization.Get("Export", "NoMoreThan"));
		((ComboBox)comboBoxImageResolutionMode).Items.Add((object)StiLocalization.Get("Export", "Auto"));
		((ComboBox)comboBoxImageResolutionMode).EndUpdate();
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelCompressionMethod.SetToolTip(StiLocalization.Get("HelpViewer", "ImageCompressionMethod"));
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQualityPdf"));
		labelAllowEditable.SetToolTip(StiLocalization.Get("HelpViewer", "AllowEditable"));
		checkBoxEmbeddedFonts.SetToolTip(StiLocalization.Get("HelpViewer", "EmbeddedFonts"));
		checkBoxExportRtfTextAsImage.SetToolTip(StiLocalization.Get("HelpViewer", "ExportRtfTextAsImage"));
		checkBoxPdfACompliance.SetToolTip(StiLocalization.Get("HelpViewer", "PdfACompliance"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
	}

	private void DoScaling()
	{
		((Control)documentSecurity).Size = new Size(StiScale.XXI(380.0), StiScale.YYI(229.0));
		((Control)buttonGetDigitalSignature).Top = ((Control)textBoxDigitalSignature).Top;
		((Control)buttonGetDigitalSignature).Height = ((Control)textBoxDigitalSignature).Height;
	}

	private void RefreshDigitalCertificateStatus()
	{
		if (StiOptions.Export.Pdf.DisableDigitalSignatureButtonInExportMenu)
		{
			((Control)checkBoxDigitalSignature).Enabled = false;
			((Control)buttonGetDigitalSignature).Enabled = false;
			((Control)textBoxDigitalSignature).Enabled = false;
		}
		else
		{
			((Control)checkBoxDigitalSignature).Enabled = true;
			((Control)buttonGetDigitalSignature).Enabled = ((CheckBox)checkBoxDigitalSignature).Checked;
			((Control)textBoxDigitalSignature).Enabled = ((CheckBox)checkBoxDigitalSignature).Checked;
		}
	}

	private void Build()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		((ComboBox)comboBoxAllowEditable).Items.Add((object)new ComboBoxItem
		{
			Text = StiLocalization.Get("Buttons", "No").Replace("&", ""),
			Tag = StiPdfAllowEditable.No
		});
		((ComboBox)comboBoxAllowEditable).Items.Add((object)new ComboBoxItem
		{
			Text = StiLocalization.Get("Buttons", "Yes").Replace("&", ""),
			Tag = StiPdfAllowEditable.Yes
		});
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1058: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1182: Unknown result type (might be due to invalid IL or missing references)
		//IL_122f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1556: Unknown result type (might be due to invalid IL or missing references)
		//IL_1560: Expected O, but got Unknown
		components = new Container();
		buttonOk = new Button();
		buttonCancel = new Button();
		glButtons = new StiGroupLine();
		groupBoxExportSettings = new StiGroupBox();
		buttonGetDigitalSignature = new Button();
		textBoxDigitalSignature = new StiTextBox();
		checkBoxDigitalSignature = new StiCheckBoxInfo();
		comboBoxAllowEditable = new StiComboBox();
		labelAllowEditable = new StiLabelInfo();
		comboBoxComplianceMode = new StiComboBox();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		checkBoxPdfACompliance = new StiCheckBoxInfo();
		label6 = new Label();
		buttonDocumentSecurity = new ButtonX();
		controlContainerItem1 = new ControlContainerItem();
		documentSecurity = new StiDocumentSecurityUserControl();
		comboBoxImageResolutionMode = new StiComboBox();
		comboBoxImageCompressionMethod = new StiComboBox();
		comboBoxImageResolution = new StiComboBox();
		labelResolutionMode = new StiLabelInfo();
		labelCompressionMethod = new StiLabelInfo();
		labelResolution = new StiLabelInfo();
		checkBoxEmbeddedFonts = new StiCheckBoxInfo();
		checkBoxExportRtfTextAsImage = new StiCheckBoxInfo();
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		toolTip1 = new ToolTip(components);
		PagesRangeControl = new StiPagesRangeControl();
		((Control)glButtons).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(201, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(282, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 564);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Anchor = (AnchorStyles)13;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)buttonGetDigitalSignature);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)textBoxDigitalSignature);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxDigitalSignature);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxAllowEditable);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelAllowEditable);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxComplianceMode);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxPdfACompliance);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)buttonDocumentSecurity);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolutionMode);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageCompressionMethod);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolutionMode);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelCompressionMethod);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxEmbeddedFonts);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxExportRtfTextAsImage);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 366;
		((Control)groupBoxExportSettings).Size = new Size(345, 347);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)buttonGetDigitalSignature).Location = new Point(304, 275);
		((Control)buttonGetDigitalSignature).Name = "buttonGetDigitalSignature";
		((Control)buttonGetDigitalSignature).Size = new Size(26, 23);
		((Control)buttonGetDigitalSignature).TabIndex = 11;
		((Control)buttonGetDigitalSignature).Text = "...";
		((ButtonBase)buttonGetDigitalSignature).UseVisualStyleBackColor = true;
		((Control)buttonGetDigitalSignature).Click += ButtonGetDigitalCertificate_Click;
		((Control)textBoxDigitalSignature).Anchor = (AnchorStyles)13;
		((Control)textBoxDigitalSignature).BackColor = Color.White;
		((Control)textBoxDigitalSignature).Enabled = false;
		((Control)textBoxDigitalSignature).ForeColor = SystemColors.ControlText;
		((Control)textBoxDigitalSignature).Location = new Point(173, 276);
		((StiEditBase)textBoxDigitalSignature).MaxLength = 256;
		((Control)textBoxDigitalSignature).Name = "textBoxDigitalSignature";
		((Control)textBoxDigitalSignature).Size = new Size(128, 21);
		((Control)textBoxDigitalSignature).TabIndex = 10;
		((Control)checkBoxDigitalSignature).Anchor = (AnchorStyles)13;
		((Control)checkBoxDigitalSignature).AutoSize = true;
		((Control)checkBoxDigitalSignature).ForeColor = SystemColors.ControlText;
		((Control)checkBoxDigitalSignature).Location = new Point(15, 278);
		((Control)checkBoxDigitalSignature).Name = "checkBoxDigitalSignature";
		((Control)checkBoxDigitalSignature).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxDigitalSignature).Size = new Size(137, 17);
		((Control)checkBoxDigitalSignature).TabIndex = 9;
		((Control)checkBoxDigitalSignature).Text = "Use Digital Signature";
		((CheckBox)checkBoxDigitalSignature).CheckedChanged += CheckBoxDigitalSignature_CheckedChanged;
		((Control)comboBoxAllowEditable).BackColor = SystemColors.Window;
		comboBoxAllowEditable.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxAllowEditable).ForeColor = SystemColors.ControlText;
		((Control)comboBoxAllowEditable).Location = new Point(173, 20);
		((ComboBox)comboBoxAllowEditable).MaxDropDownItems = 20;
		((Control)comboBoxAllowEditable).Name = "comboBoxAllowEditable";
		((Control)comboBoxAllowEditable).Size = new Size(158, 21);
		((Control)comboBoxAllowEditable).TabIndex = 0;
		((Control)labelAllowEditable).AutoSize = true;
		((Control)labelAllowEditable).ForeColor = Color.DimGray;
		((Control)labelAllowEditable).Location = new Point(12, 23);
		((Control)labelAllowEditable).Name = "labelAllowEditable";
		((Control)labelAllowEditable).Padding = new Padding(0, 0, 12, 0);
		((Control)labelAllowEditable).Size = new Size(85, 13);
		((Control)labelAllowEditable).TabIndex = 22;
		((Control)labelAllowEditable).Text = "Allow Editable";
		((Label)labelAllowEditable).TextAlign = (ContentAlignment)16;
		((Control)comboBoxComplianceMode).BackColor = SystemColors.Window;
		comboBoxComplianceMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxComplianceMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxComplianceMode).Items.AddRange(new object[3] { "PDF/A-1", "PDF/A-2", "PDF/A-3" });
		((Control)comboBoxComplianceMode).Location = new Point(173, 185);
		((ComboBox)comboBoxComplianceMode).MaxDropDownItems = 20;
		((Control)comboBoxComplianceMode).Name = "comboBoxComplianceMode";
		((Control)comboBoxComplianceMode).Size = new Size(135, 21);
		((Control)comboBoxComplianceMode).TabIndex = 6;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(173, 152);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(135, 21);
		((Control)comboBoxImageQuality).TabIndex = 4;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(312, 155);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 19;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 155);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 20;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)checkBoxPdfACompliance).AutoSize = true;
		((Control)checkBoxPdfACompliance).ForeColor = SystemColors.ControlText;
		((Control)checkBoxPdfACompliance).Location = new Point(15, 188);
		((Control)checkBoxPdfACompliance).Name = "checkBoxPdfACompliance";
		((Control)checkBoxPdfACompliance).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxPdfACompliance).Size = new Size(129, 17);
		((Control)checkBoxPdfACompliance).TabIndex = 5;
		((Control)checkBoxPdfACompliance).Text = "PDF/A Compliance";
		((CheckBox)checkBoxPdfACompliance).CheckedChanged += CheckBoxPdfaCompliance_CheckedChanged;
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(312, 122);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 0;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)buttonDocumentSecurity).AccessibleRole = (AccessibleRole)43;
		buttonDocumentSecurity.AutoExpandOnClick = true;
		buttonDocumentSecurity.ColorTable = (eButtonColor)3;
		buttonDocumentSecurity.Is20Button = false;
		((Control)buttonDocumentSecurity).Location = new Point(172, 309);
		((PopupItemControl)buttonDocumentSecurity).LockClosePopup = false;
		((Control)buttonDocumentSecurity).Name = "buttonDocumentSecurity";
		((Control)buttonDocumentSecurity).Size = new Size(158, 24);
		buttonDocumentSecurity.SubItems.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)controlContainerItem1 });
		((Control)buttonDocumentSecurity).TabIndex = 12;
		((Control)buttonDocumentSecurity).Text = "Document Security";
		((PopupItemControl)buttonDocumentSecurity).ThemeAware = true;
		controlContainerItem1.AllowItemResize = false;
		controlContainerItem1.Control = (Control)(object)documentSecurity;
		((BaseItem)controlContainerItem1).GlobalItem = false;
		controlContainerItem1.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)controlContainerItem1).Name = "controlContainerItem1";
		((BaseItem)controlContainerItem1).ThemeAware = true;
		((Control)documentSecurity).BackColor = Color.FromArgb(250, 250, 250);
		((Control)documentSecurity).Location = new Point(6, 19);
		((Control)documentSecurity).Margin = new Padding(0);
		((Control)documentSecurity).Name = "documentSecurity";
		((Control)documentSecurity).Size = new Size(340, 188);
		((Control)documentSecurity).TabIndex = 10;
		((Control)comboBoxImageResolutionMode).BackColor = SystemColors.Window;
		comboBoxImageResolutionMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxImageResolutionMode).DropDownWidth = 120;
		((Control)comboBoxImageResolutionMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolutionMode).Items.AddRange(new object[3] { "Exactly", "No more than", "Auto" });
		((Control)comboBoxImageResolutionMode).Location = new Point(173, 86);
		((ComboBox)comboBoxImageResolutionMode).MaxDropDownItems = 20;
		((Control)comboBoxImageResolutionMode).Name = "comboBoxImageResolutionMode";
		((Control)comboBoxImageResolutionMode).Size = new Size(158, 21);
		((Control)comboBoxImageResolutionMode).TabIndex = 2;
		((Control)comboBoxImageCompressionMethod).BackColor = SystemColors.Window;
		comboBoxImageCompressionMethod.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxImageCompressionMethod).DropDownWidth = 120;
		((Control)comboBoxImageCompressionMethod).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageCompressionMethod).Items.AddRange(new object[6] { "Jpeg", "Flate", "Indexed", "Simple Monochrome", "Ordered Monochrome", "FloydSt. Monochrome" });
		((Control)comboBoxImageCompressionMethod).Location = new Point(173, 53);
		((ComboBox)comboBoxImageCompressionMethod).MaxDropDownItems = 20;
		((Control)comboBoxImageCompressionMethod).Name = "comboBoxImageCompressionMethod";
		((Control)comboBoxImageCompressionMethod).Size = new Size(158, 21);
		((Control)comboBoxImageCompressionMethod).TabIndex = 1;
		((ComboBox)comboBoxImageCompressionMethod).SelectedIndexChanged += ComboBoxImageCompressionMethod_SelectedIndexChanged;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "50", "75", "100", "200", "300", "400", "500", "600", "1200" });
		((Control)comboBoxImageResolution).Location = new Point(173, 119);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(135, 21);
		((Control)comboBoxImageResolution).TabIndex = 3;
		((Control)labelResolutionMode).AutoSize = true;
		((Control)labelResolutionMode).ForeColor = Color.DimGray;
		((Control)labelResolutionMode).Location = new Point(12, 89);
		((Control)labelResolutionMode).Name = "labelResolutionMode";
		((Control)labelResolutionMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolutionMode).Size = new Size(130, 13);
		((Control)labelResolutionMode).TabIndex = 1;
		((Control)labelResolutionMode).Text = "Image Resolution mode";
		((Label)labelResolutionMode).TextAlign = (ContentAlignment)16;
		((Control)labelCompressionMethod).AutoSize = true;
		((Control)labelCompressionMethod).ForeColor = Color.DimGray;
		((Control)labelCompressionMethod).Location = new Point(12, 56);
		((Control)labelCompressionMethod).Name = "labelCompressionMethod";
		((Control)labelCompressionMethod).Padding = new Padding(0, 0, 12, 0);
		((Control)labelCompressionMethod).Size = new Size(149, 13);
		((Control)labelCompressionMethod).TabIndex = 1;
		((Control)labelCompressionMethod).Text = "Image Compression method";
		((Label)labelCompressionMethod).TextAlign = (ContentAlignment)16;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 122);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(101, 13);
		((Control)labelResolution).TabIndex = 0;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)checkBoxEmbeddedFonts).AutoSize = true;
		((CheckBox)checkBoxEmbeddedFonts).Checked = true;
		((CheckBox)checkBoxEmbeddedFonts).CheckState = (CheckState)1;
		((Control)checkBoxEmbeddedFonts).ForeColor = SystemColors.ControlText;
		((Control)checkBoxEmbeddedFonts).Location = new Point(15, 218);
		((Control)checkBoxEmbeddedFonts).Name = "checkBoxEmbeddedFonts";
		((Control)checkBoxEmbeddedFonts).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxEmbeddedFonts).Size = new Size(115, 17);
		((Control)checkBoxEmbeddedFonts).TabIndex = 7;
		((Control)checkBoxEmbeddedFonts).Text = "Embedded fonts";
		((Control)checkBoxExportRtfTextAsImage).AutoSize = true;
		((Control)checkBoxExportRtfTextAsImage).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportRtfTextAsImage).Location = new Point(15, 248);
		((Control)checkBoxExportRtfTextAsImage).Name = "checkBoxExportRtfTextAsImage";
		((Control)checkBoxExportRtfTextAsImage).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportRtfTextAsImage).Size = new Size(155, 17);
		((Control)checkBoxExportRtfTextAsImage).TabIndex = 8;
		((Control)checkBoxExportRtfTextAsImage).Text = "Export Rtf Text as Image";
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 522);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)PagesRangeControl).Anchor = (AnchorStyles)13;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 8);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 610);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPdfExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiPdfExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)groupBoxExportSettings).PerformLayout();
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
