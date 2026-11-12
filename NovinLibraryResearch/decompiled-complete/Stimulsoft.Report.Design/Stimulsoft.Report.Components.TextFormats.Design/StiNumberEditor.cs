using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

[ToolboxItem(false)]
public class StiNumberEditor : StiFormatEditor
{
	private StiNumberFormatService service;

	private bool locked;

	private IContainer components;

	private StiCheckBoxUX checkBoxUseGroupSeparator;

	private StiLabelUX labelDecimalSeparator;

	private StiLabelUX labelGroupSeparator;

	private StiLabelUX labelDecimalDigits;

	private StiNumericUpDown numericUpDownDecimalDigits;

	private StiDropDownBox comboBoxDecimalSeparator;

	private StiDropDownBox comboBoxGroupSeparator;

	private StiCheckBoxUX checkBoxUseLocalSetting;

	private StiLabelUX labelGroupSize;

	private StiNumericUpDown numericUpDownGroupSize;

	private StiComboBox comboBoxNegativePattern;

	private StiLabelUX labelNegativePattern;

	private StiGroupPanelUX groupLineSeparator;

	private StiCheckBoxUX checkBoxNegativePattern;

	private StiCheckBoxUX checkBoxGroupSize;

	private StiCheckBoxUX checkBoxGroupSeparator;

	private StiCheckBoxUX checkBoxDecimalSeparator;

	private StiCheckBoxUX checkBoxDecimalDigits;

	private StiCheckBoxUX checkBoxUseAbbreviation;

	private StiCheckBoxUX checkBoxNegativeInRed;

	public StiNumberEditor(StiNumberFormatService service, StiDesignerControl designer)
		: base(designer)
	{
		this.service = service;
		locked = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((CheckBox)checkBoxDecimalDigits).Checked = (service.State & StiTextFormatState.DecimalDigits) > StiTextFormatState.None;
			((CheckBox)checkBoxDecimalSeparator).Checked = (service.State & StiTextFormatState.DecimalSeparator) > StiTextFormatState.None;
			((CheckBox)checkBoxGroupSeparator).Checked = (service.State & StiTextFormatState.GroupSeparator) > StiTextFormatState.None;
			((CheckBox)checkBoxGroupSize).Checked = (service.State & StiTextFormatState.GroupSize) > StiTextFormatState.None;
			((CheckBox)checkBoxNegativePattern).Checked = (service.State & StiTextFormatState.NegativePattern) > StiTextFormatState.None;
			((CheckBox)checkBoxUseAbbreviation).Checked = (service.State & StiTextFormatState.Abbreviation) > StiTextFormatState.None;
			((CheckBox)checkBoxNegativeInRed).Checked = (service.State & StiTextFormatState.NegativeInRed) > StiTextFormatState.None;
			Localize();
			locked = false;
			((CheckBox)checkBoxUseGroupSeparator).Checked = service.UseGroupSeparator;
			((CheckBox)checkBoxUseLocalSetting).Checked = service.UseLocalSetting;
			locked = true;
			((Control)comboBoxDecimalSeparator).Text = service.DecimalSeparator;
			numericUpDownDecimalDigits.Value = service.DecimalDigits;
			((Control)comboBoxGroupSeparator).Text = service.GroupSeparator;
			numericUpDownGroupSize.Value = service.GroupSize;
			((ListControl)comboBoxNegativePattern).SelectedIndex = service.NegativePattern;
			CheckVisibleState();
			DoScaling();
			locked = false;
		}
	}

	private void Localize()
	{
		((Control)checkBoxUseGroupSeparator).Text = StiLocalization.Get("FormFormatEditor", "UseGroupSeparator");
		((Control)checkBoxUseLocalSetting).Text = StiLocalization.Get("FormFormatEditor", "UseLocalSetting");
		((Control)checkBoxUseAbbreviation).Text = StiLocalization.Get("FormFormatEditor", "UseAbbreviation");
		((Control)checkBoxNegativeInRed).Text = StiLocalization.Get("FormFormatEditor", "NegativeInRed");
		((Control)labelDecimalDigits).Text = Loc.GetCleaned("FormFormatEditor", "DecimalDigits");
		((Control)labelDecimalSeparator).Text = Loc.GetCleaned("FormFormatEditor", "DecimalSeparator");
		((Control)labelDecimalSeparator).Text = Loc.GetCleaned("FormFormatEditor", "DecimalSeparator");
		((Control)labelGroupSeparator).Text = Loc.GetCleaned("FormFormatEditor", "GroupSeparator");
		((Control)labelGroupSize).Text = Loc.GetCleaned("FormFormatEditor", "GroupSize");
		((Control)labelNegativePattern).Text = Loc.GetCleaned("FormFormatEditor", "NegativePattern");
	}

	public override void ApplyChanges()
	{
		service.DecimalDigits = (int)numericUpDownDecimalDigits.Value;
		service.GroupSize = (int)numericUpDownGroupSize.Value;
		service.GroupSeparator = ((Control)comboBoxGroupSeparator).Text;
		service.DecimalSeparator = ((Control)comboBoxDecimalSeparator).Text;
	}

	private void CheckBoxUseGroupSeparator_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.UseGroupSeparator = ((CheckBox)checkBoxUseGroupSeparator).Checked;
			InvokeUpdateSample();
		}
	}

	private void NumericUpDownDecimalDigits_ValueChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.DecimalDigits = (int)numericUpDownDecimalDigits.Value;
			InvokeUpdateSample();
		}
	}

	private void NumericUpDownGroupSize_ValueChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.GroupSize = (int)numericUpDownGroupSize.Value;
			InvokeUpdateSample();
		}
	}

	private void CheckBoxUseLocalSetting_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			StiNumericUpDown stiNumericUpDown = numericUpDownDecimalDigits;
			StiDropDownBox stiDropDownBox = comboBoxDecimalSeparator;
			StiComboBox stiComboBox = comboBoxNegativePattern;
			StiDropDownBox stiDropDownBox2 = comboBoxGroupSeparator;
			bool flag2 = (((Control)numericUpDownGroupSize).Enabled = !((CheckBox)checkBoxUseLocalSetting).Checked);
			bool flag4 = (((Control)stiDropDownBox2).Enabled = flag2);
			bool flag6 = (((Control)stiComboBox).Enabled = flag4);
			bool enabled = (((Control)stiDropDownBox).Enabled = flag6);
			((Control)stiNumericUpDown).Enabled = enabled;
			CheckVisibleState();
			service.UseLocalSetting = ((CheckBox)checkBoxUseLocalSetting).Checked;
			InvokeUpdateSample();
			CheckEnabledState();
		}
	}

	private void ComboBoxGroupSeparator_TextChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.GroupSeparator = ((Control)comboBoxGroupSeparator).Text;
			InvokeUpdateSample();
		}
	}

	private void ComboBoxDecimalSeparator_TextChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.DecimalSeparator = ((Control)comboBoxDecimalSeparator).Text;
			InvokeUpdateSample();
		}
	}

	private void ComboBoxNegativePattern_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			service.NegativePattern = ((ListControl)comboBoxNegativePattern).SelectedIndex;
			InvokeUpdateSample();
		}
	}

	private void CheckBoxState_CheckedChanged(object sender, EventArgs e)
	{
		CheckEnabledState();
		InvokeUpdateSample();
	}

	private void DoScaling()
	{
		((Control)comboBoxNegativePattern).Width = ((Control)comboBoxDecimalSeparator).Width;
	}

	private void CheckVisibleState()
	{
		StiCheckBoxUX stiCheckBoxUX = checkBoxDecimalDigits;
		StiCheckBoxUX stiCheckBoxUX2 = checkBoxDecimalSeparator;
		StiCheckBoxUX stiCheckBoxUX3 = checkBoxGroupSeparator;
		StiCheckBoxUX stiCheckBoxUX4 = checkBoxGroupSize;
		bool flag = (((Control)checkBoxNegativePattern).Visible = ((CheckBox)checkBoxUseLocalSetting).Checked);
		bool flag3 = (((Control)stiCheckBoxUX4).Visible = flag);
		bool flag5 = (((Control)stiCheckBoxUX3).Visible = flag3);
		bool visible = (((Control)stiCheckBoxUX2).Visible = flag5);
		((Control)stiCheckBoxUX).Visible = visible;
	}

	private void CheckEnabledState()
	{
		if (!locked)
		{
			((Control)numericUpDownDecimalDigits).Enabled = ((CheckBox)checkBoxDecimalDigits).Checked || !service.UseLocalSetting;
			((Control)comboBoxDecimalSeparator).Enabled = ((CheckBox)checkBoxDecimalSeparator).Checked || !service.UseLocalSetting;
			((Control)comboBoxGroupSeparator).Enabled = ((CheckBox)checkBoxGroupSeparator).Checked || !service.UseLocalSetting;
			((Control)numericUpDownGroupSize).Enabled = ((CheckBox)checkBoxGroupSize).Checked || !service.UseLocalSetting;
			((Control)comboBoxNegativePattern).Enabled = ((CheckBox)checkBoxNegativePattern).Checked || !service.UseLocalSetting;
			StiTextFormatState stiTextFormatState = StiTextFormatState.None;
			if (((CheckBox)checkBoxDecimalDigits).Checked)
			{
				stiTextFormatState |= StiTextFormatState.DecimalDigits;
			}
			if (((CheckBox)checkBoxDecimalSeparator).Checked)
			{
				stiTextFormatState |= StiTextFormatState.DecimalSeparator;
			}
			if (((CheckBox)checkBoxGroupSeparator).Checked)
			{
				stiTextFormatState |= StiTextFormatState.GroupSeparator;
			}
			if (((CheckBox)checkBoxGroupSize).Checked)
			{
				stiTextFormatState |= StiTextFormatState.GroupSize;
			}
			if (((CheckBox)checkBoxNegativePattern).Checked)
			{
				stiTextFormatState |= StiTextFormatState.NegativePattern;
			}
			if (((CheckBox)checkBoxUseAbbreviation).Checked)
			{
				stiTextFormatState |= StiTextFormatState.Abbreviation;
			}
			if (((CheckBox)checkBoxNegativeInRed).Checked)
			{
				stiTextFormatState |= StiTextFormatState.NegativeInRed;
			}
			service.State = stiTextFormatState;
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
		//IL_0d9f: Unknown result type (might be due to invalid IL or missing references)
		checkBoxUseGroupSeparator = new StiCheckBoxUX();
		checkBoxUseLocalSetting = new StiCheckBoxUX();
		labelDecimalSeparator = new StiLabelUX();
		labelGroupSeparator = new StiLabelUX();
		labelGroupSize = new StiLabelUX();
		labelDecimalDigits = new StiLabelUX();
		numericUpDownGroupSize = new StiNumericUpDown();
		numericUpDownDecimalDigits = new StiNumericUpDown();
		comboBoxDecimalSeparator = new StiDropDownBox();
		comboBoxGroupSeparator = new StiDropDownBox();
		comboBoxNegativePattern = new StiComboBox();
		labelNegativePattern = new StiLabelUX();
		groupLineSeparator = new StiGroupPanelUX();
		checkBoxNegativePattern = new StiCheckBoxUX();
		checkBoxGroupSize = new StiCheckBoxUX();
		checkBoxGroupSeparator = new StiCheckBoxUX();
		checkBoxDecimalSeparator = new StiCheckBoxUX();
		checkBoxDecimalDigits = new StiCheckBoxUX();
		checkBoxUseAbbreviation = new StiCheckBoxUX();
		checkBoxNegativeInRed = new StiCheckBoxUX();
		((ISupportInitialize)numericUpDownGroupSize).BeginInit();
		((ISupportInitialize)numericUpDownDecimalDigits).BeginInit();
		((Control)this).SuspendLayout();
		((Control)checkBoxUseGroupSeparator).Location = new Point(0, 12);
		((Control)checkBoxUseGroupSeparator).Name = "checkBoxUseGroupSeparator";
		((Control)checkBoxUseGroupSeparator).Size = new Size(260, 24);
		((Control)checkBoxUseGroupSeparator).TabIndex = 0;
		((Control)checkBoxUseGroupSeparator).Text = "Use Group Separator";
		((CheckBox)checkBoxUseGroupSeparator).CheckedChanged += CheckBoxUseGroupSeparator_CheckedChanged;
		((Control)checkBoxUseLocalSetting).Location = new Point(0, 43);
		((Control)checkBoxUseLocalSetting).Name = "checkBoxUseLocalSetting";
		((Control)checkBoxUseLocalSetting).Size = new Size(258, 24);
		((Control)checkBoxUseLocalSetting).TabIndex = 1;
		((Control)checkBoxUseLocalSetting).Text = "Use Local Setting";
		((CheckBox)checkBoxUseLocalSetting).CheckedChanged += CheckBoxUseLocalSetting_CheckedChanged;
		((Control)labelDecimalSeparator).ForeColor = Color.DimGray;
		((Control)labelDecimalSeparator).Location = new Point(0, 187);
		((Control)labelDecimalSeparator).Name = "labelDecimalSeparator";
		((Control)labelDecimalSeparator).Size = new Size(144, 21);
		((Control)labelDecimalSeparator).TabIndex = 8;
		((Control)labelDecimalSeparator).Text = "DecimalSeparator";
		((Label)labelDecimalSeparator).TextAlign = (ContentAlignment)16;
		((Control)labelGroupSeparator).ForeColor = Color.DimGray;
		((Control)labelGroupSeparator).Location = new Point(0, 220);
		((Control)labelGroupSeparator).Name = "labelGroupSeparator";
		((Control)labelGroupSeparator).Size = new Size(144, 21);
		((Control)labelGroupSeparator).TabIndex = 11;
		((Control)labelGroupSeparator).Text = "GroupSeparator";
		((Label)labelGroupSeparator).TextAlign = (ContentAlignment)16;
		((Control)labelGroupSize).ForeColor = Color.DimGray;
		((Control)labelGroupSize).Location = new Point(0, 253);
		((Control)labelGroupSize).Name = "labelGroupSize";
		((Control)labelGroupSize).Size = new Size(144, 20);
		((Control)labelGroupSize).TabIndex = 14;
		((Control)labelGroupSize).Text = "GroupSize";
		((Label)labelGroupSize).TextAlign = (ContentAlignment)16;
		((Control)labelDecimalDigits).ForeColor = Color.DimGray;
		((Control)labelDecimalDigits).Location = new Point(0, 155);
		((Control)labelDecimalDigits).Name = "labelDecimalDigits";
		((Control)labelDecimalDigits).Size = new Size(144, 20);
		((Control)labelDecimalDigits).TabIndex = 5;
		((Control)labelDecimalDigits).Text = "DecimalDigits";
		((Label)labelDecimalDigits).TextAlign = (ContentAlignment)16;
		((Control)numericUpDownGroupSize).Anchor = (AnchorStyles)13;
		numericUpDownGroupSize.Minimum = 0m;
		numericUpDownGroupSize.Maximum = 9m;
		((Control)numericUpDownGroupSize).BackColor = SystemColors.Window;
		((Control)numericUpDownGroupSize).ForeColor = SystemColors.ControlText;
		((Control)numericUpDownGroupSize).Location = new Point(168, 253);
		((Control)numericUpDownGroupSize).Name = "numericUpDownGroupSize";
		((Control)numericUpDownGroupSize).Size = new Size(90, 21);
		((Control)numericUpDownGroupSize).TabIndex = 16;
		((Control)numericUpDownGroupSize).Text = "1";
		numericUpDownGroupSize.ValueChanged += NumericUpDownGroupSize_ValueChanged;
		((Control)numericUpDownGroupSize).TextChanged += NumericUpDownGroupSize_ValueChanged;
		((Control)numericUpDownDecimalDigits).Anchor = (AnchorStyles)13;
		numericUpDownDecimalDigits.Minimum = 0m;
		numericUpDownDecimalDigits.Maximum = 99m;
		((Control)numericUpDownDecimalDigits).BackColor = SystemColors.Window;
		((Control)numericUpDownDecimalDigits).ForeColor = SystemColors.ControlText;
		((Control)numericUpDownDecimalDigits).Location = new Point(168, 155);
		((Control)numericUpDownDecimalDigits).Name = "numericUpDownDecimalDigits";
		((Control)numericUpDownDecimalDigits).Size = new Size(90, 21);
		((Control)numericUpDownDecimalDigits).TabIndex = 7;
		((Control)numericUpDownDecimalDigits).Text = "1";
		numericUpDownDecimalDigits.ValueChanged += NumericUpDownDecimalDigits_ValueChanged;
		((Control)comboBoxDecimalSeparator).Anchor = (AnchorStyles)13;
		((Control)comboBoxDecimalSeparator).BackColor = SystemColors.Window;
		((Control)comboBoxDecimalSeparator).ForeColor = SystemColors.ControlText;
		comboBoxDecimalSeparator.Items.AddRange(new object[2] { ".", "," });
		((Control)comboBoxDecimalSeparator).Location = new Point(168, 187);
		((Control)comboBoxDecimalSeparator).Name = "comboBoxDecimalSeparator";
		((Control)comboBoxDecimalSeparator).Size = new Size(90, 21);
		((Control)comboBoxDecimalSeparator).TabIndex = 10;
		((Control)comboBoxDecimalSeparator).Text = "stiComboBox1";
		((Control)comboBoxDecimalSeparator).TextChanged += ComboBoxDecimalSeparator_TextChanged;
		((Control)comboBoxGroupSeparator).Anchor = (AnchorStyles)13;
		((Control)comboBoxGroupSeparator).BackColor = SystemColors.Window;
		((Control)comboBoxGroupSeparator).ForeColor = SystemColors.ControlText;
		comboBoxGroupSeparator.Items.AddRange(new object[3] { "", ".", "," });
		((Control)comboBoxGroupSeparator).Location = new Point(168, 220);
		((Control)comboBoxGroupSeparator).Name = "comboBoxGroupSeparator";
		((Control)comboBoxGroupSeparator).Size = new Size(90, 21);
		((Control)comboBoxGroupSeparator).TabIndex = 13;
		((Control)comboBoxGroupSeparator).Text = "stiComboBox2";
		((Control)comboBoxGroupSeparator).TextChanged += ComboBoxGroupSeparator_TextChanged;
		((Control)comboBoxNegativePattern).Anchor = (AnchorStyles)13;
		((Control)comboBoxNegativePattern).BackColor = SystemColors.Window;
		comboBoxNegativePattern.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxNegativePattern).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxNegativePattern).Items.AddRange(new object[5] { "(n)", "-n", "- n", "n-", "n -" });
		((Control)comboBoxNegativePattern).Location = new Point(168, 285);
		((Control)comboBoxNegativePattern).Name = "comboBoxNegativePattern";
		((Control)comboBoxNegativePattern).Size = new Size(90, 21);
		((Control)comboBoxNegativePattern).TabIndex = 19;
		((ComboBox)comboBoxNegativePattern).SelectedIndexChanged += ComboBoxNegativePattern_SelectedIndexChanged;
		((Control)comboBoxNegativePattern).TextChanged += ComboBoxNegativePattern_SelectedIndexChanged;
		((Control)labelNegativePattern).ForeColor = Color.DimGray;
		((Control)labelNegativePattern).Location = new Point(0, 285);
		((Control)labelNegativePattern).Name = "labelNegativePattern";
		((Control)labelNegativePattern).Size = new Size(144, 21);
		((Control)labelNegativePattern).TabIndex = 17;
		((Control)labelNegativePattern).Text = "NegativePattern";
		((Label)labelNegativePattern).TextAlign = (ContentAlignment)16;
		((Control)groupLineSeparator).Anchor = (AnchorStyles)13;
		((Control)groupLineSeparator).ForeColor = SystemColors.ControlText;
		groupLineSeparator.LinePosition = StiLinePosition.Top;
		groupLineSeparator.LineStyle = StiLineStyle.Dot;
		((Control)groupLineSeparator).Location = new Point(2, 140);
		((Control)groupLineSeparator).Name = "groupLineSeparator";
		((Control)groupLineSeparator).Size = new Size(258, 8);
		((Control)groupLineSeparator).TabIndex = 4;
		((Control)groupLineSeparator).TabStop = false;
		((Control)checkBoxNegativePattern).ForeColor = SystemColors.ControlText;
		((Control)checkBoxNegativePattern).Location = new Point(147, 285);
		((Control)checkBoxNegativePattern).Name = "checkBoxNegativePattern";
		((Control)checkBoxNegativePattern).Size = new Size(19, 21);
		((Control)checkBoxNegativePattern).TabIndex = 18;
		((Control)checkBoxNegativePattern).Text = " ";
		((CheckBox)checkBoxNegativePattern).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxGroupSize).ForeColor = SystemColors.ControlText;
		((Control)checkBoxGroupSize).Location = new Point(147, 253);
		((Control)checkBoxGroupSize).Name = "checkBoxGroupSize";
		((Control)checkBoxGroupSize).Size = new Size(19, 21);
		((Control)checkBoxGroupSize).TabIndex = 15;
		((Control)checkBoxGroupSize).Text = " ";
		((CheckBox)checkBoxGroupSize).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxGroupSeparator).ForeColor = SystemColors.ControlText;
		((Control)checkBoxGroupSeparator).Location = new Point(147, 220);
		((Control)checkBoxGroupSeparator).Name = "checkBoxGroupSeparator";
		((Control)checkBoxGroupSeparator).Size = new Size(19, 21);
		((Control)checkBoxGroupSeparator).TabIndex = 12;
		((Control)checkBoxGroupSeparator).Text = " ";
		((CheckBox)checkBoxGroupSeparator).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxDecimalSeparator).ForeColor = SystemColors.ControlText;
		((Control)checkBoxDecimalSeparator).Location = new Point(147, 187);
		((Control)checkBoxDecimalSeparator).Name = "checkBoxDecimalSeparator";
		((Control)checkBoxDecimalSeparator).Size = new Size(19, 21);
		((Control)checkBoxDecimalSeparator).TabIndex = 9;
		((Control)checkBoxDecimalSeparator).Text = " ";
		((CheckBox)checkBoxDecimalSeparator).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxDecimalDigits).ForeColor = SystemColors.ControlText;
		((Control)checkBoxDecimalDigits).Location = new Point(147, 155);
		((Control)checkBoxDecimalDigits).Name = "checkBoxDecimalDigits";
		((Control)checkBoxDecimalDigits).Size = new Size(19, 21);
		((Control)checkBoxDecimalDigits).TabIndex = 6;
		((Control)checkBoxDecimalDigits).Text = " ";
		((CheckBox)checkBoxDecimalDigits).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxUseAbbreviation).Location = new Point(0, 74);
		((Control)checkBoxUseAbbreviation).Name = "checkBoxUseAbbreviation";
		((Control)checkBoxUseAbbreviation).Size = new Size(258, 24);
		((Control)checkBoxUseAbbreviation).TabIndex = 2;
		((Control)checkBoxUseAbbreviation).Text = "Use Abbreviation";
		((CheckBox)checkBoxUseAbbreviation).CheckedChanged += CheckBoxState_CheckedChanged;
		((Control)checkBoxNegativeInRed).Location = new Point(0, 105);
		((Control)checkBoxNegativeInRed).Name = "checkBoxNegativeInRed";
		((Control)checkBoxNegativeInRed).Size = new Size(258, 24);
		((Control)checkBoxNegativeInRed).TabIndex = 3;
		((Control)checkBoxNegativeInRed).Text = "Negative in Red";
		((CheckBox)checkBoxNegativeInRed).CheckedChanged += CheckBoxState_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)checkBoxNegativeInRed);
		((Control)this).Controls.Add((Control)(object)checkBoxNegativePattern);
		((Control)this).Controls.Add((Control)(object)checkBoxGroupSize);
		((Control)this).Controls.Add((Control)(object)checkBoxGroupSeparator);
		((Control)this).Controls.Add((Control)(object)checkBoxDecimalSeparator);
		((Control)this).Controls.Add((Control)(object)checkBoxDecimalDigits);
		((Control)this).Controls.Add((Control)(object)groupLineSeparator);
		((Control)this).Controls.Add((Control)(object)comboBoxNegativePattern);
		((Control)this).Controls.Add((Control)(object)labelNegativePattern);
		((Control)this).Controls.Add((Control)(object)comboBoxGroupSeparator);
		((Control)this).Controls.Add((Control)(object)comboBoxDecimalSeparator);
		((Control)this).Controls.Add((Control)(object)numericUpDownDecimalDigits);
		((Control)this).Controls.Add((Control)(object)numericUpDownGroupSize);
		((Control)this).Controls.Add((Control)(object)labelDecimalDigits);
		((Control)this).Controls.Add((Control)(object)labelGroupSize);
		((Control)this).Controls.Add((Control)(object)labelGroupSeparator);
		((Control)this).Controls.Add((Control)(object)labelDecimalSeparator);
		((Control)this).Controls.Add((Control)(object)checkBoxUseAbbreviation);
		((Control)this).Controls.Add((Control)(object)checkBoxUseLocalSetting);
		((Control)this).Controls.Add((Control)(object)checkBoxUseGroupSeparator);
		((Control)this).Margin = new Padding(3);
		((Control)this).Name = "StiNumberEditor";
		((Control)this).Size = new Size(258, 318);
		((ISupportInitialize)numericUpDownGroupSize).EndInit();
		((ISupportInitialize)numericUpDownDecimalDigits).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
