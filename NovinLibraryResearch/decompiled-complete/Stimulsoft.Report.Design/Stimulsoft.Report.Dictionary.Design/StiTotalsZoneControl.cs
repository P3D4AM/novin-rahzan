using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiTotalsZoneControl : StiZoneControl
{
	private string prevValue;

	private IContainer components;

	private ImageList ilButton;

	private StiHitTestVisibleLabel labelName;

	private StiHitTestVisibleLabel labelColumn;

	private StiHitTestVisibleLabel labelFunction;

	public StiTreeViewBox comboBoxColumn;

	public StiComboBox comboBoxFunction;

	public Stimulsoft.Controls.StiTextBox textBoxName;

	private string No => "[" + StiLocalization.Get("Report", "No") + "]";

	public StiTotalsZoneControl()
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
	}

	private void Localize()
	{
		((Control)labelColumn).Text = StiLocalization.Get("PropertyMain", "Column");
		((Control)labelFunction).Text = StiLocalization.Get("PropertyMain", "AggregateFunction");
		((Control)labelName).Text = StiLocalization.Get("PropertyMain", "Name");
	}

	private void UpdateName()
	{
		string empty = string.Empty;
		if (((Control)comboBoxColumn).Text != No && ((Control)comboBoxFunction).Text != No)
		{
			empty = $"{((Control)comboBoxColumn).Text}.{((Control)comboBoxFunction).Text}";
		}
		else if (((Control)comboBoxColumn).Text != No)
		{
			empty = ((Control)comboBoxColumn).Text;
		}
		else
		{
			if (!(((Control)comboBoxFunction).Text != No))
			{
				return;
			}
			empty = ((Control)comboBoxFunction).Text;
		}
		if (((Control)textBoxName).Text.Trim().Length == 0 || ((Control)textBoxName).Text == prevValue)
		{
			((Control)textBoxName).Text = empty;
			prevValue = empty;
		}
	}

	private void LbCondition_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
	}

	private void ComboBoxFunction_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateName();
	}

	private void ComboBoxColumn_DropUp(object sender, EventArgs e)
	{
		UpdateName();
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		labelName = new StiHitTestVisibleLabel();
		labelColumn = new StiHitTestVisibleLabel();
		comboBoxColumn = new StiTreeViewBox();
		ilButton = new ImageList(components);
		comboBoxFunction = new StiComboBox();
		labelFunction = new StiHitTestVisibleLabel();
		textBoxName = new Stimulsoft.Controls.StiTextBox();
		((Control)this).SuspendLayout();
		((Control)labelName).BackColor = Color.Transparent;
		((Control)labelName).Location = new Point(347, 8);
		((Control)labelName).Margin = new Padding(2, 0, 2, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(104, 17);
		((Control)labelName).TabIndex = 16;
		((Control)labelName).Text = "Name";
		((Control)labelName).MouseDown += new MouseEventHandler(LbCondition_MouseDown);
		((Control)labelColumn).BackColor = Color.Transparent;
		((Control)labelColumn).Location = new Point(8, 8);
		((Control)labelColumn).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumn).Name = "labelColumn";
		((Control)labelColumn).Size = new Size(136, 17);
		((Control)labelColumn).TabIndex = 14;
		((Control)labelColumn).Text = "Column";
		((Control)labelColumn).MouseDown += new MouseEventHandler(LbCondition_MouseDown);
		((Control)comboBoxColumn).BackColor = SystemColors.Window;
		comboBoxColumn.DropDownWidth = 400;
		comboBoxColumn.Flat = false;
		((Control)comboBoxColumn).ForeColor = SystemColors.ControlText;
		((Control)comboBoxColumn).Location = new Point(8, 25);
		((Control)comboBoxColumn).Margin = new Padding(2, 2, 2, 2);
		comboBoxColumn.MaxDropDownHeight = 300;
		((Control)comboBoxColumn).Name = "comboBoxColumn";
		((Control)comboBoxColumn).Size = new Size(210, 21);
		((Control)comboBoxColumn).TabIndex = 0;
		comboBoxColumn.DropUp += ComboBoxColumn_DropUp;
		ilButton.ColorDepth = (ColorDepth)24;
		ilButton.ImageSize = new Size(16, 16);
		ilButton.TransparentColor = Color.Magenta;
		((Control)comboBoxFunction).BackColor = SystemColors.Window;
		comboBoxFunction.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxFunction).DropDownWidth = 150;
		((Control)comboBoxFunction).Location = new Point(222, 25);
		((Control)comboBoxFunction).Margin = new Padding(2, 2, 2, 2);
		((ComboBox)comboBoxFunction).MaxDropDownItems = 20;
		((Control)comboBoxFunction).Name = "comboBoxFunction";
		((Control)comboBoxFunction).Size = new Size(114, 21);
		((Control)comboBoxFunction).TabIndex = 1;
		((ComboBox)comboBoxFunction).SelectedIndexChanged += ComboBoxFunction_SelectedIndexChanged;
		((Control)labelFunction).BackColor = Color.Transparent;
		((Control)labelFunction).Location = new Point(222, 8);
		((Control)labelFunction).Margin = new Padding(2, 0, 2, 0);
		((Control)labelFunction).Name = "labelFunction";
		((Control)labelFunction).Size = new Size(112, 17);
		((Control)labelFunction).TabIndex = 29;
		((Control)labelFunction).Text = "Function";
		((Control)labelFunction).MouseDown += new MouseEventHandler(LbCondition_MouseDown);
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).BackColor = Color.White;
		((Control)textBoxName).Location = new Point(339, 25);
		((Control)textBoxName).Margin = new Padding(2, 2, 2, 2);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(184, 21);
		((Control)textBoxName).TabIndex = 2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)textBoxName);
		((Control)this).Controls.Add((Control)(object)comboBoxFunction);
		((Control)this).Controls.Add((Control)(object)labelFunction);
		((Control)this).Controls.Add((Control)(object)comboBoxColumn);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)labelColumn);
		((Control)this).Margin = new Padding(1, 1, 1, 1);
		((Control)this).Name = "StiTotalsZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(528, 58);
		((Control)this).ResumeLayout(false);
	}
}
