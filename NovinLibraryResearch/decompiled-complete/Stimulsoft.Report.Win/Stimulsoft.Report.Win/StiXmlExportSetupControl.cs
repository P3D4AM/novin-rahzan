using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiXmlExportSetupControl : UserControl
{
	private IContainer components;

	private StiLabelInfo labelDataExportMode;

	internal StiComboBox comboBoxDataExportMode;

	public int DataExportMode => ((ListControl)comboBoxDataExportMode).SelectedIndex;

	public StiXmlExportSetupControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			((ListControl)comboBoxDataExportMode).SelectedIndex = 0;
			LocalizeSuperToolTip();
			((ComboBox)comboBoxDataExportMode).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxDataExportMode).DropDownWidth);
		}
	}

	private void Localize()
	{
		((Control)labelDataExportMode).Text = Loc.GetCleaned("Export", "BandsFilter");
		((ComboBox)comboBoxDataExportMode).Items.Clear();
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataOnly"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataAndHeadersFooters"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "AllBands"));
	}

	private void LocalizeSuperToolTip()
	{
		labelDataExportMode.SetToolTip(StiLocalization.Get("HelpViewer", "ExportMode"));
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
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		comboBoxDataExportMode = new StiComboBox();
		labelDataExportMode = new StiLabelInfo();
		((Control)this).SuspendLayout();
		((Control)comboBoxDataExportMode).Anchor = (AnchorStyles)13;
		((Control)comboBoxDataExportMode).BackColor = SystemColors.Window;
		comboBoxDataExportMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDataExportMode).DropDownWidth = 134;
		((Control)comboBoxDataExportMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxDataExportMode).Items.AddRange(new object[3] { "Data only", "Data and Headers/Footers", "All bands" });
		((Control)comboBoxDataExportMode).Location = new Point(184, 0);
		((Control)comboBoxDataExportMode).Name = "cbDataExportMode";
		((Control)comboBoxDataExportMode).Size = new Size(111, 21);
		((Control)comboBoxDataExportMode).TabIndex = 2;
		((Control)labelDataExportMode).ForeColor = Color.DimGray;
		((Control)labelDataExportMode).Location = new Point(12, 0);
		((Control)labelDataExportMode).Name = "lbDataExportMode";
		((Control)labelDataExportMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelDataExportMode).Size = new Size(166, 21);
		((Control)labelDataExportMode).TabIndex = 12;
		((Control)labelDataExportMode).Text = "Bands filter";
		((Label)labelDataExportMode).TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)labelDataExportMode);
		((Control)this).Controls.Add((Control)(object)comboBoxDataExportMode);
		((Control)this).Name = "StiXmlExportSetupControl";
		((Control)this).Size = new Size(306, 34);
		((Control)this).ResumeLayout(false);
	}
}
