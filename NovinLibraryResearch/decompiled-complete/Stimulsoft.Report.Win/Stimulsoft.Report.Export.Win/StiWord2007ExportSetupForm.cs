using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiWord2007ExportSetupForm : Form
{
	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox groupBoxExportSettings;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupLine glButtons;

	private StiCheckBoxInfo checkboxUsePageHeadersAndFooters;

	private StiComboBox comboBoxImageResolution;

	private StiLabelInfo labelResolution;

	private Label label6;

	private StiCheckBoxInfo checkboxRemoveEmptySpaceAtBottom;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	private StiComboBox comboBoxRestrictEditing;

	private StiLabelInfo labelRestrictEditing;

	private Panel panelOpenAfterExport;

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

	public float ImageQuality
	{
		get
		{
			int.TryParse(((Control)comboBoxImageQuality).Text, out var result);
			return (float)result / 100f;
		}
	}

	public bool UsePageHeadersAndFooters => ((CheckBox)checkboxUsePageHeadersAndFooters).Checked;

	public bool RemoveEmptySpaceAtBottom => ((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked;

	public StiWord2007RestrictEditing RestrictEditing => (StiWord2007RestrictEditing)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxRestrictEditing).SelectedItem).Tag;

	public StiWord2007ExportSetupForm()
	{
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
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
		((CheckBox)checkboxUsePageHeadersAndFooters).Checked = StiSettings.GetBool(((Control)this).Name, "UsePageHeadersAndFooters", ((CheckBox)checkboxUsePageHeadersAndFooters).Checked);
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked = StiSettings.GetBool(((Control)this).Name, "RemoveEmptySpaceAtBottom", StiOptions.Export.RichText.RemoveEmptySpaceAtBottom);
		((Control)comboBoxImageResolution).Handle.ToString();
		((ListControl)comboBoxImageResolution).SelectedIndex = 4;
		((ComboBox)comboBoxImageResolution).SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "75");
		int @int = StiSettings.GetInt(((Control)this).Name, "RestrictEditing", 1);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxRestrictEditing).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(@int))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxRestrictEditing).SelectedIndex = ((num < ((ComboBox)comboBoxRestrictEditing).Items.Count) ? num : 0);
		LocalizeSuperToolTip();
	}

	private void CheckboxUsePageHeadersAndFooters_CheckedChanged(object sender, EventArgs e)
	{
		if (((CheckBox)checkboxUsePageHeadersAndFooters).Checked)
		{
			((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked = true;
		}
		((Control)checkboxRemoveEmptySpaceAtBottom).Enabled = !((CheckBox)checkboxUsePageHeadersAndFooters).Checked;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "UsePageHeadersAndFooters", ((CheckBox)checkboxUsePageHeadersAndFooters).Checked);
		StiSettings.Set(((Control)this).Name, "RemoveEmptySpaceAtBottom", ((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text);
		StiSettings.Set(((Control)this).Name, "RestrictEditing", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxRestrictEditing).SelectedItem).Tag);
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

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_text_formats_word2007_2010.htm");
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeWord2007File").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkboxUsePageHeadersAndFooters).Text = StiLocalization.Get("Export", "UsePageHeadersAndFooters");
		((Control)checkboxRemoveEmptySpaceAtBottom).Text = StiLocalization.Get("Export", "RemoveEmptySpaceAtBottom");
		((Control)labelImageQuality).Text = StiLocalization.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = StiLocalization.GetCleaned("Export", "ImageResolution");
		((Control)labelRestrictEditing).Text = StiLocalization.GetCleaned("Export", "RestrictEditing");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQualityPdf"));
		labelRestrictEditing.SetToolTip(StiLocalization.Get("HelpViewer", "RestrictEditing"));
		checkboxUsePageHeadersAndFooters.SetToolTip(StiLocalization.Get("HelpViewer", "UsePageHeadersAndFooters"));
		checkboxRemoveEmptySpaceAtBottom.SetToolTip(StiLocalization.Get("HelpViewer", "RemoveEmptySpaceAtBottom"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
	}

	private void Build()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)new ComboBoxItem
		{
			Text = Loc.Get("Buttons", "No").Replace("&", ""),
			Tag = StiWord2007RestrictEditing.No
		});
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)new ComboBoxItem
		{
			Text = Loc.Get("Export", "ExceptEditableFields"),
			Tag = StiWord2007RestrictEditing.ExceptEditableFields
		});
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)new ComboBoxItem
		{
			Text = Loc.Get("Buttons", "Yes").Replace("&", ""),
			Tag = StiWord2007RestrictEditing.Yes
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Expected O, but got Unknown
		buttonOk = new Button();
		buttonCancel = new Button();
		glButtons = new StiGroupLine();
		groupBoxExportSettings = new StiGroupBox();
		comboBoxRestrictEditing = new StiComboBox();
		labelRestrictEditing = new StiLabelInfo();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		checkboxRemoveEmptySpaceAtBottom = new StiCheckBoxInfo();
		label6 = new Label();
		comboBoxImageResolution = new StiComboBox();
		labelResolution = new StiLabelInfo();
		checkboxUsePageHeadersAndFooters = new StiCheckBoxInfo();
		PagesRangeControl = new StiPagesRangeControl();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		panelOpenAfterExport = new Panel();
		((Control)glButtons).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(201, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(282, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 402);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxRestrictEditing);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelRestrictEditing);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkboxRemoveEmptySpaceAtBottom);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkboxUsePageHeadersAndFooters);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 182;
		((Control)groupBoxExportSettings).Size = new Size(345, 184);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)comboBoxRestrictEditing).BackColor = SystemColors.Window;
		comboBoxRestrictEditing.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxRestrictEditing).ForeColor = SystemColors.ControlText;
		((Control)comboBoxRestrictEditing).Location = new Point(200, 86);
		((ComboBox)comboBoxRestrictEditing).MaxDropDownItems = 20;
		((Control)comboBoxRestrictEditing).Name = "comboBoxRestrictEditing";
		((Control)comboBoxRestrictEditing).Size = new Size(133, 21);
		((Control)comboBoxRestrictEditing).TabIndex = 2;
		((Control)labelRestrictEditing).AutoSize = true;
		((Control)labelRestrictEditing).ForeColor = Color.DimGray;
		((Control)labelRestrictEditing).Location = new Point(12, 89);
		((Control)labelRestrictEditing).Name = "labelRestrictEditing";
		((Control)labelRestrictEditing).Padding = new Padding(0, 0, 12, 0);
		((Control)labelRestrictEditing).Size = new Size(90, 13);
		((Control)labelRestrictEditing).TabIndex = 25;
		((Control)labelRestrictEditing).Text = "Restrict Editing";
		((Label)labelRestrictEditing).TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(200, 53);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(98, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(304, 57);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 22;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 57);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 23;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)checkboxRemoveEmptySpaceAtBottom).AutoSize = true;
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked = true;
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).CheckState = (CheckState)1;
		((Control)checkboxRemoveEmptySpaceAtBottom).ForeColor = SystemColors.ControlText;
		((Control)checkboxRemoveEmptySpaceAtBottom).Location = new Point(15, 153);
		((Control)checkboxRemoveEmptySpaceAtBottom).Name = "checkboxRemoveEmptySpaceAtBottom";
		((Control)checkboxRemoveEmptySpaceAtBottom).Padding = new Padding(0, 0, 12, 0);
		((Control)checkboxRemoveEmptySpaceAtBottom).Size = new Size(245, 17);
		((Control)checkboxRemoveEmptySpaceAtBottom).TabIndex = 4;
		((Control)checkboxRemoveEmptySpaceAtBottom).Text = "Remove empty space at bottom of the page";
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(304, 24);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 2;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(200, 20);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(98, 21);
		((Control)comboBoxImageResolution).TabIndex = 0;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 24);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(101, 13);
		((Control)labelResolution).TabIndex = 0;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)checkboxUsePageHeadersAndFooters).AutoSize = true;
		((Control)checkboxUsePageHeadersAndFooters).ForeColor = SystemColors.ControlText;
		((Control)checkboxUsePageHeadersAndFooters).Location = new Point(15, 122);
		((Control)checkboxUsePageHeadersAndFooters).Name = "checkboxUsePageHeadersAndFooters";
		((Control)checkboxUsePageHeadersAndFooters).Padding = new Padding(0, 0, 12, 0);
		((Control)checkboxUsePageHeadersAndFooters).Size = new Size(181, 17);
		((Control)checkboxUsePageHeadersAndFooters).TabIndex = 3;
		((Control)checkboxUsePageHeadersAndFooters).Text = "Use page headers and footers";
		((CheckBox)checkboxUsePageHeadersAndFooters).CheckedChanged += CheckboxUsePageHeadersAndFooters_CheckedChanged;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 8);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 360);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 448);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiWord2007ExportSetupForm";
		((Control)this).Padding = new Padding(0, 3, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiWord2007ExportSetupForm";
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
