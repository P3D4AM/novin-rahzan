using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiFilterZoneControl : StiZoneControl
{
	protected StiReport report;

	private IContainer components;

	private StiHitTestVisibleLabel lbFieldIs;

	public StiComboBox cbItem;

	public StiComboBox cbCondition;

	public StiButtonEdit tbExpression;

	private StiHitTestVisibleLabel lbColumn;

	public StiButtonEdit tbValue2;

	public StiButtonEdit tbValue1;

	public StiTreeViewBox cbColumn;

	private ImageList ilButton;

	public StiComboBox cbDataType;

	private StiHitTestVisibleLabel lbDataType;

	public DateTimePicker dtpValue1;

	public DateTimePicker dtpValue2;

	private StiHitTestVisibleLabel lbAnd;

	public StiComboBox comboBoxBoolean;

	public StiFilterZoneControl()
		: this(null)
	{
	}

	public StiFilterZoneControl(StiReport report)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			Localize();
			tbExpression.ButtonIcon = StiIconUtils.ToIcon(StiReportDesignImages.Actions.EditSmall());
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)lbColumn).Text = StiLocalization.Get("PropertyMain", "Column");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "FieldIs");
		((Control)lbDataType).Text = StiLocalization.Get("PropertyMain", "DataType");
		((Control)lbAnd).Text = StiLocalization.Get("Report", "FilterAnd");
	}

	private void DoScaling()
	{
		cbColumn.FixedHeight = false;
		tbValue1.FixedHeight = false;
		tbValue2.FixedHeight = false;
		tbExpression.FixedHeight = false;
		StiButtonEdit stiButtonEdit = tbExpression;
		StiTreeViewBox stiTreeViewBox = cbColumn;
		StiButtonEdit stiButtonEdit2 = tbValue1;
		int num = (((Control)tbValue2).Height = ((Control)cbDataType).Height);
		int num3 = (((Control)stiButtonEdit2).Height = num);
		int height2 = (((Control)stiTreeViewBox).Height = num3);
		((Control)stiButtonEdit).Height = height2;
		tbExpression.Image = StiReportDesignImages.Actions.EditSmall();
		tbValue1.Image = StiReportDesignImages.Actions.EditSmall();
		tbValue2.Image = StiReportDesignImages.Actions.EditSmall();
	}

	private void lbCondition_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
	}

	private void cbCondition_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool visible = ((Control)cbCondition).Text == StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween") || ((Control)cbCondition).Text == StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween");
		if (((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"))
		{
			((Control)cbDataType).Visible = false;
			((Control)lbDataType).Visible = false;
			((Control)tbValue1).Visible = false;
			((Control)tbValue2).Visible = false;
			((Control)dtpValue1).Visible = false;
			((Control)dtpValue2).Visible = false;
			((Control)lbAnd).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
			return;
		}
		((Control)cbDataType).Visible = true;
		((Control)lbDataType).Visible = true;
		if (((Control)cbCondition).Text == StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNull") || ((Control)cbCondition).Text == StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotNull"))
		{
			((Control)tbValue1).Visible = false;
			((Control)tbValue2).Visible = false;
			((Control)lbAnd).Visible = false;
			((Control)dtpValue1).Visible = false;
			((Control)dtpValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
		}
		else if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			((Control)dtpValue1).Visible = true;
			((Control)dtpValue2).Visible = visible;
			((Control)lbAnd).Visible = visible;
			((Control)tbValue1).Visible = false;
			((Control)tbValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
		}
		else if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			((Control)tbValue1).Visible = false;
			((Control)tbValue2).Visible = false;
			((Control)lbAnd).Visible = false;
			((Control)dtpValue1).Visible = false;
			((Control)dtpValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = true;
		}
		else
		{
			((Control)tbValue1).Visible = true;
			((Control)tbValue2).Visible = visible;
			((Control)lbAnd).Visible = visible;
			((Control)dtpValue1).Visible = false;
			((Control)dtpValue2).Visible = false;
			((Control)comboBoxBoolean).Visible = false;
		}
	}

	private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool flag = ((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
		((Control)tbExpression).Visible = !flag;
		((Control)tbValue1).Visible = flag;
		((Control)tbValue2).Visible = !flag;
		((Control)cbColumn).Visible = flag;
		((Control)cbCondition).Visible = flag;
		((Control)lbColumn).Visible = flag;
		cbCondition_SelectedIndexChanged(sender, e);
	}

	private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool flag = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeExpression");
		tbValue1.ShowButton = flag;
		tbValue2.ShowButton = flag;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") || flag)
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		}
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNull"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotNull"));
		foreach (string item in ((ComboBox)cbCondition).Items)
		{
			if (item == text)
			{
				((Control)cbCondition).Text = text;
				return;
			}
		}
		((ListControl)cbCondition).SelectedIndex = 0;
	}

	private void tbValue1_ButtonClick(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		if (report != null)
		{
			StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
			string text = ((Control)stiButtonEdit).Text;
			text = StiExpressionPacker.PackExpression(text, report.Designer, useBraces: false);
			DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorCode);
			if ((int)val == 1)
			{
				((Control)stiButtonEdit).Text = StiExpressionPacker.UnPackExpression(text, report.Designer, useBraces: false);
			}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiFilterZoneControl));
		ilButton = new ImageList(components);
		comboBoxBoolean = new StiComboBox();
		lbAnd = new StiHitTestVisibleLabel();
		dtpValue2 = new DateTimePicker();
		dtpValue1 = new DateTimePicker();
		cbDataType = new StiComboBox();
		lbDataType = new StiHitTestVisibleLabel();
		cbColumn = new StiTreeViewBox();
		tbExpression = new StiButtonEdit();
		tbValue2 = new StiButtonEdit();
		tbValue1 = new StiButtonEdit();
		lbFieldIs = new StiHitTestVisibleLabel();
		cbItem = new StiComboBox();
		cbCondition = new StiComboBox();
		lbColumn = new StiHitTestVisibleLabel();
		((Control)this).SuspendLayout();
		ilButton.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButton.ImageStream");
		ilButton.TransparentColor = Color.Magenta;
		ilButton.Images.SetKeyName(0, "");
		ilButton.Images.SetKeyName(1, "");
		((ComboBox)comboBoxBoolean).Items.AddRange(new object[2] { "true", "false" });
		((Control)comboBoxBoolean).Location = new Point(128, 65);
		((Control)comboBoxBoolean).Name = "comboBoxBoolean";
		((Control)comboBoxBoolean).Size = new Size(148, 21);
		((Control)comboBoxBoolean).TabIndex = 31;
		((Control)comboBoxBoolean).Visible = false;
		((Control)lbAnd).BackColor = Color.Transparent;
		((Control)lbAnd).ForeColor = Color.DimGray;
		((Control)lbAnd).Location = new Point(276, 65);
		((Control)lbAnd).Name = "lbAnd";
		((Control)lbAnd).Size = new Size(32, 20);
		((Control)lbAnd).TabIndex = 30;
		((Control)lbAnd).Text = "and";
		((Label)lbAnd).TextAlign = (ContentAlignment)32;
		dtpValue2.Format = (DateTimePickerFormat)2;
		((Control)dtpValue2).Location = new Point(308, 65);
		((Control)dtpValue2).Name = "dtpValue2";
		((Control)dtpValue2).Size = new Size(160, 20);
		((Control)dtpValue2).TabIndex = 7;
		dtpValue1.Format = (DateTimePickerFormat)2;
		((Control)dtpValue1).Location = new Point(128, 65);
		((Control)dtpValue1).Name = "dtpValue1";
		((Control)dtpValue1).Size = new Size(148, 20);
		((Control)dtpValue1).TabIndex = 6;
		((Control)cbDataType).BackColor = SystemColors.Window;
		cbDataType.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbDataType).DropDownWidth = 150;
		((Control)cbDataType).Location = new Point(128, 32);
		((Control)cbDataType).Name = "cbDataType";
		((Control)cbDataType).Size = new Size(112, 21);
		((Control)cbDataType).TabIndex = 1;
		((ComboBox)cbDataType).SelectedIndexChanged += cbDataType_SelectedIndexChanged;
		((Control)lbDataType).BackColor = Color.Transparent;
		((Control)lbDataType).ForeColor = Color.DimGray;
		((Control)lbDataType).Location = new Point(128, 12);
		((Control)lbDataType).Name = "lbDataType";
		((Control)lbDataType).Size = new Size(112, 16);
		((Control)lbDataType).TabIndex = 29;
		((Control)lbDataType).Text = "Data Type";
		((Control)lbDataType).MouseDown += new MouseEventHandler(lbCondition_MouseDown);
		((Control)cbColumn).Anchor = (AnchorStyles)13;
		((Control)cbColumn).BackColor = SystemColors.Window;
		cbColumn.DropDownWidth = 400;
		cbColumn.Flat = false;
		((Control)cbColumn).ForeColor = SystemColors.ControlText;
		((Control)cbColumn).Location = new Point(252, 32);
		cbColumn.MaxDropDownHeight = 300;
		((Control)cbColumn).Name = "cbColumn";
		((Control)cbColumn).Size = new Size(216, 21);
		((Control)cbColumn).TabIndex = 2;
		((Control)tbExpression).Anchor = (AnchorStyles)13;
		((Control)tbExpression).BackColor = SystemColors.Window;
		tbExpression.ButtonIcon = (Icon)componentResourceManager.GetObject("tbExpression.ButtonIcon");
		tbExpression.Flat = false;
		((Control)tbExpression).ForeColor = SystemColors.ControlText;
		((Control)tbExpression).Location = new Point(12, 65);
		((Control)tbExpression).Name = "tbExpression";
		((Control)tbExpression).Size = new Size(456, 21);
		((Control)tbExpression).TabIndex = 8;
		tbExpression.ButtonClick += tbValue1_ButtonClick;
		((Control)tbValue2).BackColor = SystemColors.Window;
		tbValue2.ButtonIcon = (Icon)componentResourceManager.GetObject("tbValue2.ButtonIcon");
		tbValue2.Flat = false;
		((Control)tbValue2).ForeColor = SystemColors.ControlText;
		((Control)tbValue2).Location = new Point(308, 65);
		((Control)tbValue2).Name = "tbValue2";
		((Control)tbValue2).Size = new Size(160, 21);
		((Control)tbValue2).TabIndex = 5;
		tbValue2.ButtonClick += tbValue1_ButtonClick;
		((Control)tbValue1).BackColor = SystemColors.Window;
		tbValue1.ButtonIcon = (Icon)componentResourceManager.GetObject("tbValue1.ButtonIcon");
		tbValue1.Flat = false;
		((Control)tbValue1).ForeColor = SystemColors.ControlText;
		((Control)tbValue1).Location = new Point(128, 65);
		((Control)tbValue1).Name = "tbValue1";
		((Control)tbValue1).Size = new Size(148, 21);
		((Control)tbValue1).TabIndex = 4;
		tbValue1.ButtonClick += tbValue1_ButtonClick;
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).ForeColor = Color.DimGray;
		((Control)lbFieldIs).Location = new Point(12, 12);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(104, 16);
		((Control)lbFieldIs).TabIndex = 16;
		((Control)lbFieldIs).Text = "Field Is";
		((Control)lbFieldIs).MouseDown += new MouseEventHandler(lbCondition_MouseDown);
		((Control)cbItem).BackColor = SystemColors.Window;
		cbItem.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbItem).Location = new Point(12, 32);
		((Control)cbItem).Name = "cbItem";
		((Control)cbItem).Size = new Size(104, 21);
		((Control)cbItem).TabIndex = 0;
		((ComboBox)cbItem).SelectedIndexChanged += cbItem_SelectedIndexChanged;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCondition).DropDownWidth = 200;
		((Control)cbCondition).Location = new Point(12, 65);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(104, 21);
		((Control)cbCondition).TabIndex = 3;
		((ComboBox)cbCondition).SelectedIndexChanged += cbCondition_SelectedIndexChanged;
		((Control)lbColumn).BackColor = Color.Transparent;
		((Control)lbColumn).ForeColor = Color.DimGray;
		((Control)lbColumn).Location = new Point(252, 12);
		((Control)lbColumn).Name = "lbColumn";
		((Control)lbColumn).Size = new Size(136, 16);
		((Control)lbColumn).TabIndex = 14;
		((Control)lbColumn).Text = "Column";
		((Control)lbColumn).MouseDown += new MouseEventHandler(lbCondition_MouseDown);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).Controls.Add((Control)(object)comboBoxBoolean);
		((Control)this).Controls.Add((Control)(object)lbAnd);
		((Control)this).Controls.Add((Control)(object)dtpValue2);
		((Control)this).Controls.Add((Control)(object)dtpValue1);
		((Control)this).Controls.Add((Control)(object)cbDataType);
		((Control)this).Controls.Add((Control)(object)lbDataType);
		((Control)this).Controls.Add((Control)(object)cbColumn);
		((Control)this).Controls.Add((Control)(object)tbExpression);
		((Control)this).Controls.Add((Control)(object)tbValue2);
		((Control)this).Controls.Add((Control)(object)tbValue1);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbColumn);
		((Control)this).Margin = new Padding(3);
		((Control)this).Name = "StiFilterZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(481, 101);
		((Control)this).ResumeLayout(false);
	}
}
