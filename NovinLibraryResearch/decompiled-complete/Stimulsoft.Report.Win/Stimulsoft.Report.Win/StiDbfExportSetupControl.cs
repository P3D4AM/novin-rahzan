using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiDbfExportSetupControl : UserControl
{
	private IContainer components;

	private StiLabelInfo labelEncoding;

	internal StiComboBox comboBoxEncoding;

	public Encoding Encoding => StiEncodingHelper.GetEncodingFromName(((ComboBox)comboBoxEncoding).SelectedItem.ToString());

	public int EncodingSelectedIndex => ((ListControl)comboBoxEncoding).SelectedIndex;

	public StiDbfExportSetupControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			((ComboBox)comboBoxEncoding).Items.Clear();
			ObjectCollection items = ((ComboBox)comboBoxEncoding).Items;
			object[] copePageNames = StiDbfExportService.copePageNames;
			items.AddRange(copePageNames);
			((ListControl)comboBoxEncoding).SelectedIndex = 0;
			LocalizeSuperToolTip();
			((ComboBox)comboBoxEncoding).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxEncoding).DropDownWidth);
		}
	}

	private void Localize()
	{
		((Control)labelEncoding).Text = Loc.GetCleaned("Export", "Encoding").Replace("&", "_");
	}

	private void LocalizeSuperToolTip()
	{
		labelEncoding.SetToolTip(StiLocalization.Get("HelpViewer", "EncodingData"));
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		labelEncoding = new StiLabelInfo();
		comboBoxEncoding = new StiComboBox();
		((Control)this).SuspendLayout();
		((Control)labelEncoding).ForeColor = Color.DimGray;
		((Control)labelEncoding).Location = new Point(12, 0);
		((Control)labelEncoding).Name = "labelEncoding";
		((Control)labelEncoding).Padding = new Padding(0, 0, 12, 0);
		((Control)labelEncoding).Size = new Size(166, 21);
		((Control)labelEncoding).TabIndex = 5;
		((Control)labelEncoding).Text = "Encoding";
		((Label)labelEncoding).TextAlign = (ContentAlignment)16;
		((Control)comboBoxEncoding).Anchor = (AnchorStyles)13;
		((Control)comboBoxEncoding).BackColor = SystemColors.Window;
		comboBoxEncoding.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxEncoding).DropDownWidth = 192;
		((Control)comboBoxEncoding).ForeColor = SystemColors.ControlText;
		((Control)comboBoxEncoding).Location = new Point(184, 0);
		((ComboBox)comboBoxEncoding).MaxDropDownItems = 15;
		((Control)comboBoxEncoding).Name = "comboBoxEncoding";
		((Control)comboBoxEncoding).Size = new Size(111, 21);
		((Control)comboBoxEncoding).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)labelEncoding);
		((Control)this).Controls.Add((Control)(object)comboBoxEncoding);
		((Control)this).Name = "StiDbfExportSetupControl";
		((Control)this).Size = new Size(306, 34);
		((Control)this).ResumeLayout(false);
	}
}
