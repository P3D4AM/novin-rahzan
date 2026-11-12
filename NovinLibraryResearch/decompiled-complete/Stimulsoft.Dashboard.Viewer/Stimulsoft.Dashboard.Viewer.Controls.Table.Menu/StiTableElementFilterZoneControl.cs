using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Viewer.Images;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table.Menu;

[ToolboxItem(false)]
public class StiTableElementFilterZoneControl : StiTableZoneControl
{
	private IStiDesignerBase designer;

	private Type columnType;

	private IContainer components;

	public CheckBox checkBoxExpression;

	public ComboBox comboBoxBoolean;

	private Label labelAnd;

	public DateTimePicker datePickerValue;

	public DateTimePicker datePickerValue2;

	public StiButtonEdit textBoxValue2;

	public StiButtonEdit textBoxValue;

	public ComboBox comboBoxCondition;

	public ComboBox comboBoxValue;

	public ComboBox comboBoxValue2;

	public StiTableElementFilterZoneControl()
		: this(null, null)
	{
	}

	public StiTableElementFilterZoneControl(IStiDesignerBase designer, Type columnType)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.designer = designer;
			this.columnType = columnType;
			Localize();
			DoScaling();
			((ListControl)comboBoxCondition).SelectedIndex = 0;
			((ListControl)comboBoxBoolean).SelectedIndex = 0;
		}
	}

	private void Localize()
	{
		((Control)labelAnd).Text = Loc.Get("Report", "FilterAnd").ToLowerInvariant();
		((Control)checkBoxExpression).Text = Loc.Get("PropertyEnum", "StiFilterDataTypeExpression");
		if (columnType != null)
		{
			comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionEqualTo"));
			comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
			if (columnType.IsStringType())
			{
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionContaining"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionNotContaining"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionEndingWith"));
			}
			if (columnType.IsStringType() || columnType.IsNumericType() || columnType.IsDateType())
			{
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionBetween"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionNotBetween"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionLessThan"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
			}
			if (columnType.IsStringType())
			{
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionIsBlank"));
				comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionIsNotBlank"));
			}
		}
		comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionIsNull"));
		comboBoxCondition.Items.Add((object)Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull"));
	}

	private void DoScaling()
	{
		textBoxValue.Image = StiDashboardViewerImages.Editors.EditSmall();
		textBoxValue2.Image = StiDashboardViewerImages.Editors.EditSmall();
		StiButtonEdit stiButtonEdit = textBoxValue;
		int width2 = (((Control)datePickerValue).Width = ((Control)comboBoxValue).Width);
		((Control)stiButtonEdit).Width = width2;
		StiButtonEdit stiButtonEdit2 = textBoxValue2;
		width2 = (((Control)datePickerValue2).Width = ((Control)comboBoxValue2).Width);
		((Control)stiButtonEdit2).Width = width2;
		StiButtonEdit stiButtonEdit3 = textBoxValue;
		bool fixedHeight = (textBoxValue2.FixedHeight = false);
		stiButtonEdit3.FixedHeight = fixedHeight;
		StiButtonEdit stiButtonEdit4 = textBoxValue;
		width2 = (((Control)textBoxValue2).Height = ((Control)comboBoxValue).Height);
		((Control)stiButtonEdit4).Height = width2;
		if (StiScale.Factor > 1.0)
		{
			((Control)comboBoxCondition).Top = (((Control)this).Height - ((Control)comboBoxCondition).Height) / 2;
			((Control)datePickerValue).Top = (((Control)this).Height - ((Control)datePickerValue).Height) / 2;
			((Control)datePickerValue2).Top = (((Control)this).Height - ((Control)datePickerValue2).Height) / 2;
			((Control)comboBoxValue).Top = (((Control)this).Height - ((Control)comboBoxValue).Height) / 2;
			((Control)comboBoxValue2).Top = (((Control)this).Height - ((Control)comboBoxValue2).Height) / 2;
			((Control)comboBoxBoolean).Top = (((Control)this).Height - ((Control)comboBoxBoolean).Height) / 2;
			((Control)textBoxValue).Top = (((Control)this).Height - ((Control)textBoxValue).Height) / 2;
			((Control)textBoxValue2).Top = (((Control)this).Height - ((Control)textBoxValue2).Height) / 2;
		}
		((Control)checkBoxExpression).Top = (((Control)this).Height - ((Control)checkBoxExpression).Height) / 2;
		comboBoxCondition.DropDownWidth = ((IEnumerable)comboBoxCondition.Items).Cast<string>().Max((string x) => TextRenderer.MeasureText(x, ((Control)comboBoxCondition).Font).Width);
	}

	public void FocusZoneControl()
	{
		((Control)this).Focus();
		if (((Control)textBoxValue).Visible)
		{
			((Control)textBoxValue).Focus();
		}
		else if (((Control)comboBoxValue).Visible)
		{
			((Control)comboBoxValue).Focus();
		}
		else if (((Control)datePickerValue).Visible)
		{
			((Control)datePickerValue).Focus();
		}
		else if (((Control)comboBoxBoolean).Visible)
		{
			((Control)comboBoxBoolean).Focus();
		}
	}

	private void Label_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
	}

	private void ComboBoxCondition_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool flag = ((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionBetween") || ((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionNotBetween");
		if (((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionIsNull") || ((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionIsNotNull") || ((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionIsBlank") || ((Control)comboBoxCondition).Text == Loc.Get("PropertyEnum", "StiFilterConditionIsNotBlank"))
		{
			((Control)textBoxValue).Visible = false;
			((Control)textBoxValue2).Visible = false;
			((Control)comboBoxValue).Visible = false;
			((Control)comboBoxValue2).Visible = false;
			((Control)labelAnd).Visible = false;
			((Control)datePickerValue).Visible = false;
			((Control)datePickerValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
			((Control)checkBoxExpression).Visible = false;
		}
		else if (columnType.IsDateType() && !checkBoxExpression.Checked)
		{
			((Control)datePickerValue).Visible = true;
			((Control)datePickerValue2).Visible = flag;
			((Control)labelAnd).Visible = flag;
			((Control)textBoxValue).Visible = false;
			((Control)textBoxValue2).Visible = false;
			((Control)comboBoxValue).Visible = false;
			((Control)comboBoxValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
			((Control)checkBoxExpression).Visible = true;
		}
		else if (columnType == typeof(bool) && !checkBoxExpression.Checked)
		{
			((Control)textBoxValue).Visible = false;
			((Control)textBoxValue2).Visible = false;
			((Control)comboBoxValue).Visible = false;
			((Control)comboBoxValue2).Visible = false;
			((Control)labelAnd).Visible = false;
			((Control)datePickerValue).Visible = false;
			((Control)datePickerValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = true;
			((Control)checkBoxExpression).Visible = true;
		}
		else
		{
			((Control)textBoxValue).Visible = checkBoxExpression.Checked;
			((Control)textBoxValue2).Visible = checkBoxExpression.Checked && flag;
			((Control)comboBoxValue).Visible = !checkBoxExpression.Checked;
			((Control)comboBoxValue2).Visible = !checkBoxExpression.Checked && flag;
			((Control)labelAnd).Visible = flag;
			((Control)datePickerValue).Visible = false;
			((Control)datePickerValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
			((Control)checkBoxExpression).Visible = true;
		}
		((Control)checkBoxExpression).Location = (flag ? new Point(((Control)textBoxValue2).Right + StiScale.I12, ((Control)checkBoxExpression).Location.Y) : new Point(((Control)textBoxValue).Right + StiScale.I12, ((Control)checkBoxExpression).Location.Y));
	}

	private void CheckBoxExpression_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxExpression.Checked)
		{
			if (string.IsNullOrEmpty(((Control)textBoxValue).Text))
			{
				((Control)textBoxValue).Text = ((Control)comboBoxValue).Text;
			}
			if (string.IsNullOrEmpty(((Control)textBoxValue2).Text))
			{
				((Control)textBoxValue2).Text = ((Control)comboBoxValue2).Text;
			}
		}
		ComboBoxCondition_SelectedIndexChanged(sender, e);
	}

	private void TextBoxValue_ButtonClick(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
		StiTextEditorForm stiTextEditorForm = new StiTextEditorForm(((Control)stiButtonEdit).Text);
		try
		{
			if ((int)((Form)stiTextEditorForm).ShowDialog() == 1)
			{
				((Control)stiButtonEdit).Text = stiTextEditorForm.TextValue;
			}
		}
		finally
		{
			((IDisposable)stiTextEditorForm)?.Dispose();
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTableElementFilterZoneControl));
		checkBoxExpression = new CheckBox();
		comboBoxBoolean = new ComboBox();
		labelAnd = new Label();
		datePickerValue = new DateTimePicker();
		datePickerValue2 = new DateTimePicker();
		textBoxValue2 = new StiButtonEdit();
		textBoxValue = new StiButtonEdit();
		comboBoxCondition = new ComboBox();
		comboBoxValue = new ComboBox();
		comboBoxValue2 = new ComboBox();
		((Control)this).SuspendLayout();
		((Control)checkBoxExpression).AutoSize = true;
		((Control)checkBoxExpression).BackColor = Color.Transparent;
		((ButtonBase)checkBoxExpression).FlatStyle = (FlatStyle)3;
		((Control)checkBoxExpression).Location = new Point(466, 9);
		((Control)checkBoxExpression).Name = "checkBoxExpression";
		((Control)checkBoxExpression).Size = new Size(83, 18);
		((Control)checkBoxExpression).TabIndex = 8;
		((Control)checkBoxExpression).Text = "Expression";
		((ButtonBase)checkBoxExpression).UseVisualStyleBackColor = false;
		checkBoxExpression.CheckedChanged += CheckBoxExpression_CheckedChanged;
		comboBoxBoolean.DropDownStyle = (ComboBoxStyle)2;
		comboBoxBoolean.Items.AddRange(new object[2] { "True", "False" });
		((Control)comboBoxBoolean).Location = new Point(120, 8);
		((Control)comboBoxBoolean).Name = "comboBoxBoolean";
		((Control)comboBoxBoolean).Size = new Size(152, 21);
		((Control)comboBoxBoolean).TabIndex = 6;
		((Control)comboBoxBoolean).Visible = false;
		((Control)labelAnd).BackColor = Color.Transparent;
		((Control)labelAnd).ForeColor = Color.DimGray;
		((Control)labelAnd).Location = new Point(273, 8);
		((Control)labelAnd).Name = "labelAnd";
		((Control)labelAnd).Size = new Size(32, 20);
		((Control)labelAnd).TabIndex = 38;
		((Control)labelAnd).Text = "and";
		labelAnd.TextAlign = (ContentAlignment)32;
		((Control)labelAnd).MouseDown += new MouseEventHandler(Label_MouseDown);
		datePickerValue.Format = (DateTimePickerFormat)2;
		((Control)datePickerValue).Location = new Point(120, 8);
		((Control)datePickerValue).Name = "datePickerValue";
		((Control)datePickerValue).Size = new Size(152, 20);
		((Control)datePickerValue).TabIndex = 4;
		datePickerValue2.Format = (DateTimePickerFormat)2;
		((Control)datePickerValue2).Location = new Point(305, 8);
		((Control)datePickerValue2).Name = "datePickerValue2";
		((Control)datePickerValue2).Size = new Size(152, 20);
		((Control)datePickerValue2).TabIndex = 5;
		((Control)textBoxValue2).BackColor = SystemColors.Window;
		textBoxValue2.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxValue2.ButtonIcon");
		textBoxValue2.Flat = false;
		((Control)textBoxValue2).ForeColor = SystemColors.ControlText;
		((Control)textBoxValue2).Location = new Point(305, 8);
		((Control)textBoxValue2).Name = "textBoxValue2";
		((Control)textBoxValue2).Size = new Size(153, 21);
		((Control)textBoxValue2).TabIndex = 7;
		textBoxValue2.ButtonClick += TextBoxValue_ButtonClick;
		((Control)textBoxValue).BackColor = SystemColors.Window;
		textBoxValue.ButtonIcon = (Icon)componentResourceManager.GetObject("textBoxValue.ButtonIcon");
		textBoxValue.Flat = false;
		((Control)textBoxValue).ForeColor = SystemColors.ControlText;
		((Control)textBoxValue).Location = new Point(120, 8);
		((Control)textBoxValue).Name = "textBoxValue";
		((Control)textBoxValue).Size = new Size(153, 21);
		((Control)textBoxValue).TabIndex = 6;
		textBoxValue.ButtonClick += TextBoxValue_ButtonClick;
		((Control)comboBoxCondition).BackColor = SystemColors.Window;
		comboBoxCondition.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxCondition).Location = new Point(8, 8);
		((Control)comboBoxCondition).Name = "comboBoxCondition";
		((Control)comboBoxCondition).Size = new Size(104, 21);
		((Control)comboBoxCondition).TabIndex = 1;
		comboBoxCondition.SelectedIndexChanged += ComboBoxCondition_SelectedIndexChanged;
		comboBoxValue.AutoCompleteMode = (AutoCompleteMode)1;
		comboBoxValue.AutoCompleteSource = (AutoCompleteSource)256;
		((Control)comboBoxValue).Location = new Point(120, 8);
		comboBoxValue.MaxDropDownItems = 15;
		((Control)comboBoxValue).Name = "comboBoxValue";
		((Control)comboBoxValue).Size = new Size(152, 21);
		((Control)comboBoxValue).TabIndex = 2;
		comboBoxValue2.AutoCompleteMode = (AutoCompleteMode)1;
		comboBoxValue2.AutoCompleteSource = (AutoCompleteSource)256;
		((Control)comboBoxValue2).Location = new Point(305, 8);
		comboBoxValue2.MaxDropDownItems = 15;
		((Control)comboBoxValue2).Name = "comboBoxValue2";
		((Control)comboBoxValue2).Size = new Size(152, 21);
		((Control)comboBoxValue2).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)comboBoxValue2);
		((Control)this).Controls.Add((Control)(object)comboBoxValue);
		((Control)this).Controls.Add((Control)(object)checkBoxExpression);
		((Control)this).Controls.Add((Control)(object)comboBoxBoolean);
		((Control)this).Controls.Add((Control)(object)labelAnd);
		((Control)this).Controls.Add((Control)(object)datePickerValue);
		((Control)this).Controls.Add((Control)(object)datePickerValue2);
		((Control)this).Controls.Add((Control)(object)textBoxValue2);
		((Control)this).Controls.Add((Control)(object)textBoxValue);
		((Control)this).Controls.Add((Control)(object)comboBoxCondition);
		((Control)this).Name = "StiTableElementFilterZoneControl";
		((Control)this).Size = new Size(550, 36);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
