using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiCsvExportSetupControl : UserControl
{
	private IContainer components;

	private StiLabelInfo labelDataExportMode;

	internal StiComboBox comboBoxDataExportMode;

	internal StiCheckBoxInfo checkBoxSkipColumnHeaders;

	internal StiTextBox textBoxSeparator;

	private StiLabelInfo labelSeparator;

	private StiLabelInfo labelEncoding;

	internal StiComboBox comboBoxEncoding;

	public Encoding Encoding => StiEncodingHelper.GetEncodingFromName(((ComboBox)comboBoxEncoding).SelectedItem.ToString());

	public string Separator => ((Control)textBoxSeparator).Text;

	public bool SkipColumnHeaders => ((CheckBox)checkBoxSkipColumnHeaders).Checked;

	public int DataExportMode => ((ListControl)comboBoxDataExportMode).SelectedIndex;

	public StiCsvExportSetupControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			((Control)textBoxSeparator).Text = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
			((ComboBox)comboBoxEncoding).Items.Clear();
			ObjectCollection items = ((ComboBox)comboBoxEncoding).Items;
			object[] encodingNames = StiEncodingHelper.GetEncodingNames();
			items.AddRange(encodingNames);
			((ComboBox)comboBoxEncoding).SelectedItem = Encoding.Default.EncodingName;
			((ListControl)comboBoxDataExportMode).SelectedIndex = 0;
			LocalizeSuperToolTip();
			DoScaling();
			((ComboBox)comboBoxEncoding).DropDownWidth = StiScale.XXI(200.0);
			((ComboBox)comboBoxDataExportMode).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxDataExportMode).DropDownWidth);
		}
	}

	private void DoScaling()
	{
		((ComboBox)comboBoxEncoding).DropDownHeight = StiScaleUI.I(300);
	}

	private void Localize()
	{
		((Control)checkBoxSkipColumnHeaders).Text = StiLocalization.Get("Export", "SkipColumnHeaders").Replace("&", "_");
		((Control)labelEncoding).Text = Loc.GetCleaned("Export", "Encoding").Replace("&", "_");
		((Control)labelSeparator).Text = Loc.GetCleaned("Export", "Separator").Replace("&", "_");
		((Control)labelDataExportMode).Text = Loc.GetCleaned("Export", "BandsFilter");
		((ComboBox)comboBoxDataExportMode).Items.Clear();
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataOnly"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataAndHeadersFooters"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "AllBands"));
	}

	private void LocalizeSuperToolTip()
	{
		labelEncoding.SetToolTip(StiLocalization.Get("HelpViewer", "EncodingData"));
		labelSeparator.SetToolTip(StiLocalization.Get("HelpViewer", "Separator"));
		labelDataExportMode.SetToolTip(StiLocalization.Get("HelpViewer", "ExportMode"));
		checkBoxSkipColumnHeaders.SetToolTip(StiLocalization.Get("HelpViewer", "SkipColumnHeaders"));
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		comboBoxDataExportMode = new StiComboBox();
		textBoxSeparator = new StiTextBox();
		comboBoxEncoding = new StiComboBox();
		labelDataExportMode = new StiLabelInfo();
		checkBoxSkipColumnHeaders = new StiCheckBoxInfo();
		labelSeparator = new StiLabelInfo();
		labelEncoding = new StiLabelInfo();
		((Control)this).SuspendLayout();
		((Control)comboBoxDataExportMode).Anchor = (AnchorStyles)13;
		((Control)comboBoxDataExportMode).BackColor = SystemColors.Window;
		comboBoxDataExportMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDataExportMode).DropDownWidth = 134;
		((Control)comboBoxDataExportMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxDataExportMode).Items.AddRange(new object[3] { "Data only", "Data and Headers/Footers", "All bands" });
		((Control)comboBoxDataExportMode).Location = new Point(184, 65);
		((Control)comboBoxDataExportMode).Name = "cbDataExportMode";
		((Control)comboBoxDataExportMode).Size = new Size(111, 21);
		((Control)comboBoxDataExportMode).TabIndex = 2;
		((Control)textBoxSeparator).ForeColor = SystemColors.ControlText;
		((Control)textBoxSeparator).Location = new Point(184, 33);
		((Control)textBoxSeparator).Name = "textBoxSeparator";
		((Control)textBoxSeparator).Size = new Size(48, 20);
		((Control)textBoxSeparator).TabIndex = 1;
		((Control)textBoxSeparator).Text = ",";
		((Control)comboBoxEncoding).Anchor = (AnchorStyles)13;
		((Control)comboBoxEncoding).BackColor = SystemColors.Window;
		comboBoxEncoding.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxEncoding).DropDownWidth = 148;
		((Control)comboBoxEncoding).ForeColor = SystemColors.ControlText;
		((Control)comboBoxEncoding).Location = new Point(184, 0);
		((Control)comboBoxEncoding).Name = "comboBoxEncoding";
		((Control)comboBoxEncoding).Size = new Size(111, 21);
		((Control)comboBoxEncoding).TabIndex = 0;
		((Control)labelDataExportMode).ForeColor = Color.DimGray;
		((Control)labelDataExportMode).Location = new Point(12, 66);
		((Control)labelDataExportMode).Name = "lbDataExportMode";
		((Control)labelDataExportMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelDataExportMode).Size = new Size(166, 20);
		((Control)labelDataExportMode).TabIndex = 12;
		((Control)labelDataExportMode).Text = "Bands filter";
		((Label)labelDataExportMode).TextAlign = (ContentAlignment)16;
		((Control)checkBoxSkipColumnHeaders).AutoSize = true;
		((Control)checkBoxSkipColumnHeaders).Location = new Point(15, 102);
		((Control)checkBoxSkipColumnHeaders).Name = "checkBoxSkipColumnHeaders";
		((Control)checkBoxSkipColumnHeaders).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxSkipColumnHeaders).Size = new Size(140, 17);
		((Control)checkBoxSkipColumnHeaders).TabIndex = 3;
		((Control)checkBoxSkipColumnHeaders).Text = "Skip Column Headers";
		((Control)labelSeparator).ForeColor = Color.DimGray;
		((Control)labelSeparator).Location = new Point(12, 33);
		((Control)labelSeparator).Name = "labelSeparator";
		((Control)labelSeparator).Padding = new Padding(0, 0, 12, 0);
		((Control)labelSeparator).Size = new Size(166, 20);
		((Control)labelSeparator).TabIndex = 9;
		((Control)labelSeparator).Text = "Separator";
		((Label)labelSeparator).TextAlign = (ContentAlignment)16;
		((Control)labelEncoding).ForeColor = Color.DimGray;
		((Control)labelEncoding).Location = new Point(12, 0);
		((Control)labelEncoding).Name = "labelEncoding";
		((Control)labelEncoding).Padding = new Padding(0, 0, 12, 0);
		((Control)labelEncoding).Size = new Size(166, 20);
		((Control)labelEncoding).TabIndex = 7;
		((Control)labelEncoding).Text = "Encoding";
		((Label)labelEncoding).TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)labelDataExportMode);
		((Control)this).Controls.Add((Control)(object)comboBoxDataExportMode);
		((Control)this).Controls.Add((Control)(object)checkBoxSkipColumnHeaders);
		((Control)this).Controls.Add((Control)(object)textBoxSeparator);
		((Control)this).Controls.Add((Control)(object)labelSeparator);
		((Control)this).Controls.Add((Control)(object)labelEncoding);
		((Control)this).Controls.Add((Control)(object)comboBoxEncoding);
		((Control)this).Name = "StiCsvExportSetupControl";
		((Control)this).Size = new Size(306, 131);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
