using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiHtmlSetupForm : Form
{
	private IContainer components;

	private Panel panelOpenAfterExport;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupLine glButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl pagesRange;

	private StiGroupBox groupBoxExportSettings;

	private Panel panelType;

	private StiLabelInfo labelType;

	private StiComboBox comboBoxType;

	private StiHtmlExportSetupControl htmlExportSetupControl;

	private StiHtml5ExportSetupControl html5ExportSetupControl;

	public StiExportFormat ExportFormat { get; set; } = StiExportFormat.Html;


	public StiPagesRange PagesRange => pagesRange.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

	public int CurrentPage
	{
		get
		{
			return pagesRange.CurrentPage;
		}
		set
		{
			pagesRange.CurrentPage = value;
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

	public bool UseEmbeddedImages => ((CheckBox)htmlExportSetupControl.checkBoxEmbeddedImageData).Checked;

	public bool ContinuousPages => html5ExportSetupControl.ContinuousPages;

	public float Resolution => html5ExportSetupControl.Resolution;

	public float ImageQuality => html5ExportSetupControl.ImageQuality;

	public bool AddPageBreaks => htmlExportSetupControl.AddPageBreaks;

	public float Zoom => htmlExportSetupControl.Zoom;

	public StiHtmlExportMode ExportMode => htmlExportSetupControl.ExportMode;

	public ImageFormat ImageFormat
	{
		get
		{
			if (ExportFormat != StiExportFormat.Html5)
			{
				return htmlExportSetupControl.ImageFormat;
			}
			return html5ExportSetupControl.ImageFormat;
		}
	}

	public bool CompressToArchive
	{
		get
		{
			if (ExportFormat != StiExportFormat.Html5)
			{
				return ((CheckBox)htmlExportSetupControl.checkBoxCompressToArchive).Checked;
			}
			return ((CheckBox)html5ExportSetupControl.checkBoxCompressToArchive).Checked;
		}
	}

	public StiHtmlSetupForm()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
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
		if (StiOptions.Viewer.Windows.Exports.ShowHtml)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Html",
				Tag = StiExportFormat.Html
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowHtml5)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Html5",
				Tag = StiExportFormat.Html5
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowMht)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Mht",
				Tag = StiExportFormat.Mht
			});
		}
		((Form)this).Closed += This_Closed;
		groupBoxExportSettings.CollapsedChange += GroupBoxExportSettings_CollapsedChange;
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((ListControl)comboBoxType).SelectedIndex = 0;
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		((Control)htmlExportSetupControl.comboBoxExportMode).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)htmlExportSetupControl.comboBoxScale).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)htmlExportSetupControl.comboBoxImageFormat).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)htmlExportSetupControl.labelPercent).Left = ((Control)comboBoxType).Right - StiScale.XXI(18.0);
		((Control)html5ExportSetupControl.comboBoxImageFormat).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)html5ExportSetupControl.comboBoxImageQuality).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)html5ExportSetupControl.comboBoxImageResolution).Width = ((Control)comboBoxType).Width - StiScale.XXI(27.0);
		((Control)html5ExportSetupControl.labelDpi).Left = ((Control)comboBoxType).Right - StiScale.XXI(18.0);
		((Control)html5ExportSetupControl.labelPercent).Left = ((Control)comboBoxType).Right - StiScale.XXI(18.0);
		((CheckBox)htmlExportSetupControl.checkBoxAddPageBreaks).Checked = StiSettings.GetBool(((Control)this).Name, "AddPageBreaks", ((CheckBox)htmlExportSetupControl.checkBoxAddPageBreaks).Checked);
		htmlExportSetupControl.comboBoxScale.SelectedIndex = StiSettings.GetInt(((Control)this).Name, "Scale", htmlExportSetupControl.comboBoxScale.SelectedIndex);
		((CheckBox)html5ExportSetupControl.checkBoxCompressToArchive).Checked = StiSettings.GetBool(((Control)this).Name, "CompressToArchive", nullValue: false);
		((CheckBox)htmlExportSetupControl.checkBoxCompressToArchive).Checked = StiSettings.GetBool(((Control)this).Name, "CompressToArchive", nullValue: false);
		((CheckBox)htmlExportSetupControl.checkBoxEmbeddedImageData).Checked = StiSettings.GetBool(((Control)this).Name, "UseEmbeddedImages", nullValue: false);
		((ListControl)htmlExportSetupControl.comboBoxImageFormat).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageFormat", ((ListControl)htmlExportSetupControl.comboBoxImageFormat).SelectedIndex);
		((ListControl)html5ExportSetupControl.comboBoxImageFormat).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageFormat", ((ListControl)htmlExportSetupControl.comboBoxImageFormat).SelectedIndex);
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings");
		((ListControl)htmlExportSetupControl.comboBoxExportMode).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ExportMode", ((ListControl)htmlExportSetupControl.comboBoxExportMode).SelectedIndex);
		((CheckBox)html5ExportSetupControl.checkBoxContinuousPages).Checked = StiSettings.GetBool(((Control)this).Name, "ContinuousPages", nullValue: false);
		((ListControl)html5ExportSetupControl.comboBoxImageResolution).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageResolution", ((ListControl)html5ExportSetupControl.comboBoxImageResolution).SelectedIndex);
		((Control)html5ExportSetupControl.comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "75");
		int @int = StiSettings.GetInt(((Control)this).Name, "Type", 32);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxType).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(@int))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = ((num < ((ComboBox)comboBoxType).Items.Count) ? num : 0);
		LocalizeSuperToolTip();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_web_documents.htm");
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ContinuousPages", ((CheckBox)html5ExportSetupControl.checkBoxContinuousPages).Checked);
		if (ExportFormat == StiExportFormat.Html5)
		{
			StiSettings.Set(((Control)this).Name, "ImageFormat", ((ListControl)html5ExportSetupControl.comboBoxImageFormat).SelectedIndex);
			StiSettings.Set(((Control)this).Name, "CompressToArchive", ((CheckBox)html5ExportSetupControl.checkBoxCompressToArchive).Checked);
		}
		else
		{
			StiSettings.Set(((Control)this).Name, "ImageFormat", ((ListControl)htmlExportSetupControl.comboBoxImageFormat).SelectedIndex);
			StiSettings.Set(((Control)this).Name, "CompressToArchive", ((CheckBox)htmlExportSetupControl.checkBoxCompressToArchive).Checked);
			StiSettings.Set(((Control)this).Name, "UseEmbeddedImages", ((CheckBox)htmlExportSetupControl.checkBoxEmbeddedImageData).Checked);
		}
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ListControl)html5ExportSetupControl.comboBoxImageResolution).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)html5ExportSetupControl.comboBoxImageQuality).Text);
		StiSettings.Set(((Control)this).Name, "AddPageBreaks", ((CheckBox)htmlExportSetupControl.checkBoxAddPageBreaks).Checked);
		StiSettings.Set(((Control)this).Name, "Type", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "Scale", htmlExportSetupControl.comboBoxScale.SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ExportMode", ((ListControl)htmlExportSetupControl.comboBoxExportMode).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Save();
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckState();
	}

	private void GroupBoxExportSettings_CollapsedChange(object sender, EventArgs e)
	{
		if (!groupBoxExportSettings.Collapsed)
		{
			CheckState();
		}
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeHtmlFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings").Replace("&", "_");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport").Replace("&", "_");
		((Control)labelType).Text = Loc.GetCleaned("Export", "Type");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelType.SetToolTip(StiLocalization.Get("HelpViewer", "TypeExport"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
	}

	private void CheckState()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		int num = ((Control)panelType).Height + StiScale.YYI(21.0);
		switch ((StiExportFormat)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag)
		{
		case StiExportFormat.Html:
			ExportFormat = StiExportFormat.Html;
			((Control)htmlExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)html5ExportSetupControl).Visible = false;
			htmlExportSetupControl.EmbeddedImageDataVisible = true;
			num += ((Control)htmlExportSetupControl).Height;
			break;
		case StiExportFormat.Html5:
			ExportFormat = StiExportFormat.Html5;
			((Control)htmlExportSetupControl).Visible = false;
			((Control)html5ExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			num += ((Control)html5ExportSetupControl).Height;
			break;
		case StiExportFormat.Mht:
			ExportFormat = StiExportFormat.Mht;
			((Control)htmlExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)html5ExportSetupControl).Visible = false;
			htmlExportSetupControl.EmbeddedImageDataVisible = false;
			num += ((Control)htmlExportSetupControl).Height;
			break;
		}
		if (groupBoxExportSettings.Collapsed)
		{
			num = groupBoxExportSettings.HeaderSize;
		}
		((Control)groupBoxExportSettings).Height = num;
		((Control)this).Height = ((Control)pagesRange).Height + num + ((Control)panelOpenAfterExport).Height + ((Control)glButtons).Height + StiScale.YYI(54.0);
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		buttonOk = new Button();
		buttonCancel = new Button();
		groupBoxExportSettings = new StiGroupBox();
		htmlExportSetupControl = new StiHtmlExportSetupControl();
		html5ExportSetupControl = new StiHtml5ExportSetupControl();
		panelType = new Panel();
		labelType = new StiLabelInfo();
		comboBoxType = new StiComboBox();
		glButtons = new StiGroupLine();
		pagesRange = new StiPagesRangeControl();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 608);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 3;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(198, 12);
		((Control)buttonOk).Name = "btOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(282, 12);
		((Control)buttonCancel).Name = "btCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Anchor = (AnchorStyles)13;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)htmlExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)html5ExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelType);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "gbExportSettings";
		groupBoxExportSettings.ResHeight = 415;
		((Control)groupBoxExportSettings).Size = new Size(345, 407);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)htmlExportSetupControl).BackColor = Color.White;
		((Control)htmlExportSetupControl).Dock = (DockStyle)1;
		((Control)htmlExportSetupControl).Location = new Point(3, 215);
		((Control)htmlExportSetupControl).Margin = new Padding(6);
		((Control)htmlExportSetupControl).Name = "htmlExportSetupControl";
		((Control)htmlExportSetupControl).Size = new Size(339, 186);
		((Control)htmlExportSetupControl).TabIndex = 2;
		((Control)html5ExportSetupControl).BackColor = Color.White;
		((Control)html5ExportSetupControl).Dock = (DockStyle)1;
		((Control)html5ExportSetupControl).Location = new Point(3, 57);
		((Control)html5ExportSetupControl).Margin = new Padding(6);
		((Control)html5ExportSetupControl).Name = "html5ExportSetupControl";
		((Control)html5ExportSetupControl).Size = new Size(339, 158);
		((Control)html5ExportSetupControl).TabIndex = 1;
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)comboBoxType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(3, 16);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(339, 41);
		((Control)panelType).TabIndex = 0;
		((Control)labelType).AutoSize = true;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 10);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelType).Size = new Size(43, 13);
		((Control)labelType).TabIndex = 2;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)comboBoxType).BackColor = SystemColors.Window;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxType).ForeColor = SystemColors.ControlText;
		((Control)comboBoxType).Location = new Point(184, 8);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(144, 21);
		((Control)comboBoxType).TabIndex = 0;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 650);
		((Control)glButtons).Margin = new Padding(0, 3, 3, 3);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)pagesRange).Anchor = (AnchorStyles)13;
		((Control)pagesRange).BackColor = Color.White;
		pagesRange.CurrentPage = 0;
		((Control)pagesRange).Location = new Point(12, 8);
		((Control)pagesRange).Name = "pagesRange";
		((Control)pagesRange).Size = new Size(345, 159);
		((Control)pagesRange).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 696);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)pagesRange);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiHtmlSetupForm";
		((Control)this).Padding = new Padding(0, 4, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiHtmlSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)panelType).ResumeLayout(false);
		((Control)panelType).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
