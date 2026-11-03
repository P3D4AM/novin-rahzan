using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiDifSylkExportSetupControl : UserControl
{
	private IContainer components;

	internal StiCheckBoxInfo checkBoxExportDataOnly;

	internal StiCheckBoxInfo checkBoxUseDefaultSystemEncoding;

	private StiLabelInfo labelEncoding;

	internal StiDropDownBox comboBoxEncoding;

	public bool ExportDataOnly => ((CheckBox)checkBoxExportDataOnly).Checked;

	public Encoding Encoding
	{
		get
		{
			Encoding result = Encoding.ASCII;
			if (!((CheckBox)checkBoxUseDefaultSystemEncoding).Checked)
			{
				try
				{
					result = Encoding.GetEncoding(int.Parse(((Control)comboBoxEncoding).Text));
				}
				catch
				{
				}
			}
			return result;
		}
	}

	public bool UseDefaultSystemEncoding => ((CheckBox)checkBoxUseDefaultSystemEncoding).Checked;

	public StiDifSylkExportSetupControl()
	{
		InitializeComponent();
		Localize();
		LocalizeSuperToolTip();
		comboBoxEncoding.DropDownWidth = StiScale.XXI((double)comboBoxEncoding.DropDownWidth);
	}

	private void CheckBoxUseDefaultSystemEncoding_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxUseDefaultSystemEncoding != null && labelEncoding != null && comboBoxEncoding != null)
		{
			((Control)labelEncoding).Enabled = !((CheckBox)checkBoxUseDefaultSystemEncoding).Checked;
			((Control)comboBoxEncoding).Enabled = !((CheckBox)checkBoxUseDefaultSystemEncoding).Checked;
		}
	}

	private void Localize()
	{
		((Control)checkBoxExportDataOnly).Text = StiLocalization.Get("Export", "ExportDataOnly");
		((Control)checkBoxUseDefaultSystemEncoding).Text = StiLocalization.Get("Export", "UseDefaultSystemEncoding");
		((Control)labelEncoding).Text = Loc.GetCleaned("Export", "Encoding");
	}

	private void LocalizeSuperToolTip()
	{
		labelEncoding.SetToolTip(StiLocalization.Get("HelpViewer", "EncodingData"));
		checkBoxExportDataOnly.SetToolTip(StiLocalization.Get("HelpViewer", "ExportDataOnly"));
		checkBoxUseDefaultSystemEncoding.SetToolTip(StiLocalization.Get("HelpViewer", "UseDefaultSystemEncoding"));
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
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		comboBoxEncoding = new StiDropDownBox();
		checkBoxExportDataOnly = new StiCheckBoxInfo();
		labelEncoding = new StiLabelInfo();
		checkBoxUseDefaultSystemEncoding = new StiCheckBoxInfo();
		((Control)this).SuspendLayout();
		((Control)comboBoxEncoding).Anchor = (AnchorStyles)13;
		((Control)comboBoxEncoding).BackColor = SystemColors.Window;
		((Control)comboBoxEncoding).Enabled = false;
		((Control)comboBoxEncoding).ForeColor = SystemColors.ControlText;
		comboBoxEncoding.Items.AddRange(new object[11]
		{
			"437", "850", "852", "857", "860", "861", "862", "863", "865", "866",
			"869"
		});
		((Control)comboBoxEncoding).Location = new Point(184, 0);
		((Control)comboBoxEncoding).Name = "comboBoxEncoding";
		((Control)comboBoxEncoding).Size = new Size(112, 21);
		((Control)comboBoxEncoding).TabIndex = 0;
		((Control)checkBoxExportDataOnly).AutoSize = true;
		((Control)checkBoxExportDataOnly).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportDataOnly).Location = new Point(15, 38);
		((Control)checkBoxExportDataOnly).Name = "checkBoxExportDataOnly";
		((Control)checkBoxExportDataOnly).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportDataOnly).Size = new Size(114, 17);
		((Control)checkBoxExportDataOnly).TabIndex = 1;
		((Control)checkBoxExportDataOnly).Text = "Export data only";
		((Control)labelEncoding).Enabled = false;
		((Control)labelEncoding).ForeColor = Color.DimGray;
		((Control)labelEncoding).Location = new Point(12, 0);
		((Control)labelEncoding).Name = "labelEncoding";
		((Control)labelEncoding).Padding = new Padding(0, 0, 12, 0);
		((Control)labelEncoding).Size = new Size(166, 21);
		((Control)labelEncoding).TabIndex = 3;
		((Control)labelEncoding).Text = "Encoding";
		((Label)labelEncoding).TextAlign = (ContentAlignment)16;
		((Control)checkBoxUseDefaultSystemEncoding).AutoSize = true;
		((CheckBox)checkBoxUseDefaultSystemEncoding).Checked = true;
		((CheckBox)checkBoxUseDefaultSystemEncoding).CheckState = (CheckState)1;
		((Control)checkBoxUseDefaultSystemEncoding).Location = new Point(15, 68);
		((Control)checkBoxUseDefaultSystemEncoding).Name = "checkBoxUseDefaultSystemEncoding";
		((Control)checkBoxUseDefaultSystemEncoding).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxUseDefaultSystemEncoding).Size = new Size(174, 17);
		((Control)checkBoxUseDefaultSystemEncoding).TabIndex = 2;
		((Control)checkBoxUseDefaultSystemEncoding).Text = "Use default system encoding";
		((CheckBox)checkBoxUseDefaultSystemEncoding).CheckedChanged += CheckBoxUseDefaultSystemEncoding_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)checkBoxExportDataOnly);
		((Control)this).Controls.Add((Control)(object)labelEncoding);
		((Control)this).Controls.Add((Control)(object)comboBoxEncoding);
		((Control)this).Controls.Add((Control)(object)checkBoxUseDefaultSystemEncoding);
		((Control)this).Name = "StiDifSylkExportSetupControl";
		((Control)this).Size = new Size(307, 98);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
