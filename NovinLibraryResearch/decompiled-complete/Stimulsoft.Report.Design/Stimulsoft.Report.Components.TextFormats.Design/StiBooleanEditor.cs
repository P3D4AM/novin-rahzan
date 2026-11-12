using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

[ToolboxItem(false)]
public class StiBooleanEditor : StiFormatEditor
{
	private StiBooleanFormatService service;

	private bool locked;

	private IContainer components;

	private StiLabelUX labelFalseValue;

	private StiDropDownBox comboBoxFalseValue;

	private StiDropDownBox comboBoxTrueValue;

	private StiLabelUX labelTrueValue;

	private StiDropDownBox comboBoxFalseDisplay;

	private StiLabelUX labelFalseDisplay;

	private StiDropDownBox comboBoxTrueDisplay;

	private StiLabelUX labelTrueDisplay;

	private StiGroupBoxUX groupBoxTrue;

	private StiGroupBoxUX groupBoxFalse;

	public StiBooleanEditor(StiBooleanFormatService service, StiDesignerControl designer)
		: base(designer)
	{
		this.service = service;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			locked = true;
			Localize();
			comboBoxFalseValue.Items.Add((object)"False");
			comboBoxFalseValue.Items.Add((object)"True");
			comboBoxFalseValue.Items.Add((object)"No");
			comboBoxFalseValue.Items.Add((object)"Yes");
			comboBoxFalseValue.Items.Add((object)"Off");
			comboBoxFalseValue.Items.Add((object)"On");
			comboBoxTrueValue.Items.Add((object)"False");
			comboBoxTrueValue.Items.Add((object)"True");
			comboBoxTrueValue.Items.Add((object)"No");
			comboBoxTrueValue.Items.Add((object)"Yes");
			comboBoxTrueValue.Items.Add((object)"Off");
			comboBoxTrueValue.Items.Add((object)"On");
			((Control)comboBoxFalseDisplay).Text = service.FalseDisplay;
			((Control)comboBoxTrueDisplay).Text = service.TrueDisplay;
			((Control)comboBoxFalseValue).Text = service.FalseValue;
			((Control)comboBoxTrueValue).Text = service.TrueValue;
			locked = false;
		}
	}

	private void Localize()
	{
		((Control)groupBoxFalse).Text = StiLocalization.Get("FormFormatEditor", "nameFalse");
		((Control)labelFalseDisplay).Text = Loc.GetCleaned("FormFormatEditor", "BooleanDisplay");
		((Control)labelFalseValue).Text = Loc.GetCleaned("FormFormatEditor", "BooleanValue");
		((Control)groupBoxTrue).Text = StiLocalization.Get("FormFormatEditor", "nameTrue");
		((Control)labelTrueDisplay).Text = Loc.GetCleaned("FormFormatEditor", "BooleanDisplay");
		((Control)labelTrueValue).Text = Loc.GetCleaned("FormFormatEditor", "BooleanValue");
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameFalse"));
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameTrue"));
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameNo"));
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameYes"));
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameOff"));
		comboBoxFalseDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameOn"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameFalse"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameTrue"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameNo"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameYes"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameOff"));
		comboBoxTrueDisplay.Items.Add((object)StiLocalization.Get("FormFormatEditor", "nameOn"));
	}

	private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.FalseDisplay = ((Control)comboBoxFalseDisplay).Text;
			service.TrueDisplay = ((Control)comboBoxTrueDisplay).Text;
			service.FalseValue = ((Control)comboBoxFalseValue).Text;
			service.TrueValue = ((Control)comboBoxTrueValue).Text;
			InvokeUpdateSample();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		labelFalseValue = new StiLabelUX();
		comboBoxFalseValue = new StiDropDownBox();
		comboBoxTrueValue = new StiDropDownBox();
		labelTrueValue = new StiLabelUX();
		comboBoxFalseDisplay = new StiDropDownBox();
		labelFalseDisplay = new StiLabelUX();
		comboBoxTrueDisplay = new StiDropDownBox();
		labelTrueDisplay = new StiLabelUX();
		groupBoxFalse = new StiGroupBoxUX();
		groupBoxTrue = new StiGroupBoxUX();
		((Control)groupBoxFalse).SuspendLayout();
		((Control)groupBoxTrue).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelFalseValue).ForeColor = Color.DimGray;
		((Control)labelFalseValue).Location = new Point(12, 18);
		((Control)labelFalseValue).Name = "labelFalseValue";
		((Control)labelFalseValue).Size = new Size(123, 21);
		((Control)labelFalseValue).TabIndex = 2;
		((Control)labelFalseValue).Text = "False Value";
		((Label)labelFalseValue).TextAlign = (ContentAlignment)16;
		((Control)comboBoxFalseValue).Anchor = (AnchorStyles)13;
		((Control)comboBoxFalseValue).BackColor = SystemColors.Window;
		((Control)comboBoxFalseValue).ForeColor = SystemColors.ControlText;
		((Control)comboBoxFalseValue).Location = new Point(136, 18);
		((Control)comboBoxFalseValue).Name = "comboBoxFalseValue";
		((Control)comboBoxFalseValue).Size = new Size(101, 21);
		((Control)comboBoxFalseValue).TabIndex = 0;
		((Control)comboBoxFalseValue).Text = "stiComboBox1";
		((Control)comboBoxFalseValue).TextChanged += ComboBox_SelectedIndexChanged;
		((Control)comboBoxTrueValue).Anchor = (AnchorStyles)13;
		((Control)comboBoxTrueValue).BackColor = SystemColors.Window;
		((Control)comboBoxTrueValue).ForeColor = SystemColors.ControlText;
		((Control)comboBoxTrueValue).Location = new Point(136, 18);
		((Control)comboBoxTrueValue).Name = "comboBoxTrueValue";
		((Control)comboBoxTrueValue).Size = new Size(101, 21);
		((Control)comboBoxTrueValue).TabIndex = 0;
		((Control)comboBoxTrueValue).TextChanged += ComboBox_SelectedIndexChanged;
		((Control)labelTrueValue).ForeColor = Color.DimGray;
		((Control)labelTrueValue).Location = new Point(12, 18);
		((Control)labelTrueValue).Name = "labelTrueValue";
		((Control)labelTrueValue).Size = new Size(118, 21);
		((Control)labelTrueValue).TabIndex = 2;
		((Control)labelTrueValue).Text = "True Value";
		((Label)labelTrueValue).TextAlign = (ContentAlignment)16;
		((Control)comboBoxFalseDisplay).Anchor = (AnchorStyles)13;
		((Control)comboBoxFalseDisplay).BackColor = SystemColors.Window;
		((Control)comboBoxFalseDisplay).ForeColor = SystemColors.ControlText;
		((Control)comboBoxFalseDisplay).Location = new Point(136, 48);
		((Control)comboBoxFalseDisplay).Name = "comboBoxFalseDisplay";
		((Control)comboBoxFalseDisplay).Size = new Size(101, 21);
		((Control)comboBoxFalseDisplay).TabIndex = 1;
		((Control)comboBoxFalseDisplay).Text = "stiComboBox3";
		((Control)comboBoxFalseDisplay).TextChanged += ComboBox_SelectedIndexChanged;
		((Control)labelFalseDisplay).ForeColor = Color.DimGray;
		((Control)labelFalseDisplay).Location = new Point(12, 48);
		((Control)labelFalseDisplay).Name = "labelFalseDisplay";
		((Control)labelFalseDisplay).Size = new Size(123, 21);
		((Control)labelFalseDisplay).TabIndex = 3;
		((Control)labelFalseDisplay).Text = "False Display";
		((Label)labelFalseDisplay).TextAlign = (ContentAlignment)16;
		((Control)comboBoxTrueDisplay).Anchor = (AnchorStyles)13;
		((Control)comboBoxTrueDisplay).BackColor = SystemColors.Window;
		((Control)comboBoxTrueDisplay).ForeColor = SystemColors.ControlText;
		((Control)comboBoxTrueDisplay).Location = new Point(136, 48);
		((Control)comboBoxTrueDisplay).Name = "comboBoxTrueDisplay";
		((Control)comboBoxTrueDisplay).Size = new Size(101, 21);
		((Control)comboBoxTrueDisplay).TabIndex = 1;
		((Control)comboBoxTrueDisplay).Text = "stiComboBox4";
		((Control)comboBoxTrueDisplay).TextChanged += ComboBox_SelectedIndexChanged;
		((Control)labelTrueDisplay).ForeColor = Color.DimGray;
		((Control)labelTrueDisplay).Location = new Point(12, 48);
		((Control)labelTrueDisplay).Name = "labelTrueDisplay";
		((Control)labelTrueDisplay).Size = new Size(118, 21);
		((Control)labelTrueDisplay).TabIndex = 3;
		((Control)labelTrueDisplay).Text = "True Display";
		((Label)labelTrueDisplay).TextAlign = (ContentAlignment)16;
		((Control)groupBoxFalse).Anchor = (AnchorStyles)13;
		((Control)groupBoxFalse).Controls.Add((Control)(object)comboBoxFalseDisplay);
		((Control)groupBoxFalse).Controls.Add((Control)(object)comboBoxFalseValue);
		((Control)groupBoxFalse).Controls.Add((Control)(object)labelFalseValue);
		((Control)groupBoxFalse).Controls.Add((Control)(object)labelFalseDisplay);
		((Control)groupBoxFalse).Location = new Point(0, 12);
		((Control)groupBoxFalse).Name = "groupBoxFalse";
		((Control)groupBoxFalse).Size = new Size(248, 81);
		((Control)groupBoxFalse).TabIndex = 0;
		((GroupBox)groupBoxFalse).TabStop = false;
		((Control)groupBoxFalse).Text = "False";
		((Control)groupBoxTrue).Anchor = (AnchorStyles)13;
		((Control)groupBoxTrue).Controls.Add((Control)(object)labelTrueValue);
		((Control)groupBoxTrue).Controls.Add((Control)(object)comboBoxTrueDisplay);
		((Control)groupBoxTrue).Controls.Add((Control)(object)labelTrueDisplay);
		((Control)groupBoxTrue).Controls.Add((Control)(object)comboBoxTrueValue);
		((Control)groupBoxTrue).Location = new Point(0, 105);
		((Control)groupBoxTrue).Name = "groupBoxTrue";
		((Control)groupBoxTrue).Size = new Size(248, 81);
		((Control)groupBoxTrue).TabIndex = 1;
		((GroupBox)groupBoxTrue).TabStop = false;
		((Control)groupBoxTrue).Text = "True";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)groupBoxTrue);
		((Control)this).Controls.Add((Control)(object)groupBoxFalse);
		((Control)this).Name = "StiBooleanEditor";
		((Control)this).Padding = new Padding(4);
		((Control)this).Size = new Size(248, 198);
		((Control)groupBoxFalse).ResumeLayout(false);
		((Control)groupBoxTrue).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
