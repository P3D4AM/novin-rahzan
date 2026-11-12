using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiChartFilterZoneControl : StiZoneControl
{
	private Label lbFieldIs;

	public TextBox tbValue;

	public ComboBox cbDataType;

	public ComboBox cbItem;

	public ComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbDataType;

	private StiHitTestVisibleLabel lbValue;

	public DateTimePicker dtpValue;

	private Button buttonExpression;

	private Container components;

	private StiReport report;

	public StiChartFilterZoneControl()
		: this(null)
	{
	}

	public StiChartFilterZoneControl(StiReport report)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			Localize();
			DoScale();
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartFilterZoneControl));
		lbFieldIs = new Label();
		tbValue = new TextBox();
		cbDataType = new ComboBox();
		cbItem = new ComboBox();
		cbCondition = new ComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbDataType = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		dtpValue = new DateTimePicker();
		buttonExpression = new Button();
		((Control)this).SuspendLayout();
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).Location = new Point(14, 12);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(104, 16);
		((Control)lbFieldIs).TabIndex = 24;
		((Control)lbFieldIs).Text = "Field Is";
		((Control)lbFieldIs).MouseDown += new MouseEventHandler(lbDataType_MouseDown);
		((Control)tbValue).Anchor = (AnchorStyles)13;
		((Control)tbValue).ForeColor = SystemColors.ControlText;
		((Control)tbValue).Location = new Point(362, 28);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(114, 20);
		((Control)tbValue).TabIndex = 3;
		((Control)cbDataType).BackColor = SystemColors.Window;
		cbDataType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbDataType).Location = new Point(130, 28);
		((Control)cbDataType).Name = "cbDataType";
		((Control)cbDataType).Size = new Size(104, 21);
		((Control)cbDataType).TabIndex = 1;
		cbDataType.SelectedIndexChanged += cbDataType_SelectedIndexChanged;
		((Control)cbItem).BackColor = SystemColors.Window;
		cbItem.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbItem).Location = new Point(14, 28);
		((Control)cbItem).Name = "cbItem";
		((Control)cbItem).Size = new Size(104, 21);
		((Control)cbItem).TabIndex = 0;
		cbItem.SelectedIndexChanged += cbItem_SelectedIndexChanged;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		cbCondition.DropDownWidth = 200;
		((Control)cbCondition).Location = new Point(246, 28);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(104, 21);
		((Control)cbCondition).TabIndex = 2;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).Location = new Point(246, 12);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(104, 16);
		((Control)lbCondition).TabIndex = 25;
		((Control)lbCondition).Text = "Condition";
		((Control)lbCondition).MouseDown += new MouseEventHandler(lbDataType_MouseDown);
		((Control)lbDataType).BackColor = Color.Transparent;
		((Control)lbDataType).Location = new Point(130, 12);
		((Control)lbDataType).Name = "lbDataType";
		((Control)lbDataType).Size = new Size(104, 16);
		((Control)lbDataType).TabIndex = 22;
		((Control)lbDataType).Text = "Data Type";
		((Control)lbDataType).MouseDown += new MouseEventHandler(lbDataType_MouseDown);
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).Location = new Point(363, 12);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(112, 16);
		((Control)lbValue).TabIndex = 23;
		((Control)lbValue).Text = "Value";
		((Control)lbValue).MouseDown += new MouseEventHandler(lbDataType_MouseDown);
		dtpValue.Format = (DateTimePickerFormat)2;
		((Control)dtpValue).Location = new Point(363, 28);
		((Control)dtpValue).Name = "dtpValue";
		((Control)dtpValue).Size = new Size(112, 20);
		((Control)dtpValue).TabIndex = 26;
		((Control)buttonExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(478, 27);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(25, 22);
		((Control)buttonExpression).TabIndex = 27;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Visible = false;
		((Control)buttonExpression).Click += buttonExpression_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).Controls.Add((Control)(object)buttonExpression);
		((Control)this).Controls.Add((Control)(object)dtpValue);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)tbValue);
		((Control)this).Controls.Add((Control)(object)cbDataType);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbDataType);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Name = "StiChartFilterZoneControl";
		((Control)this).Size = new Size(514, 62);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void Localize()
	{
		((Control)lbCondition).Text = StiLocalization.Get("PropertyMain", "Condition");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "FieldIs");
		((Control)lbDataType).Text = StiLocalization.Get("PropertyMain", "DataType");
		((Control)lbValue).Text = StiLocalization.Get("PropertyMain", "Value");
	}

	private void DoScale()
	{
		((ButtonBase)buttonExpression).Image = StiReportDesignImages.Actions.Expression();
		Button obj = buttonExpression;
		int height = (((Control)buttonExpression).Width = ((Control)cbCondition).Height + StiScale.I1);
		((Control)obj).Height = height;
	}

	private void lbDataType_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
	}

	private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)dtpValue).Visible = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime");
		((Control)tbValue).Visible = ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime");
		((Control)buttonExpression).Visible = ((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
		((Control)cbCondition).Enabled = !((Control)buttonExpression).Visible;
		((Control)lbCondition).Enabled = !((Control)buttonExpression).Visible;
		((Control)cbDataType).Enabled = !((Control)buttonExpression).Visible;
		((Control)lbDataType).Enabled = !((Control)buttonExpression).Visible;
		string text = ((Control)cbCondition).Text;
		cbCondition.Items.Clear();
		cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"))
		{
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
			cbCondition.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		}
		foreach (string item in cbCondition.Items)
		{
			if (item == text)
			{
				((Control)cbCondition).Text = text;
				return;
			}
		}
		((ListControl)cbCondition).SelectedIndex = 0;
	}

	private void buttonExpression_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		string text = ((Control)tbValue).Text;
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorCode);
		if ((int)val == 1)
		{
			((Control)tbValue).Text = text;
		}
	}

	private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		cbDataType_SelectedIndexChanged(sender, e);
	}
}
