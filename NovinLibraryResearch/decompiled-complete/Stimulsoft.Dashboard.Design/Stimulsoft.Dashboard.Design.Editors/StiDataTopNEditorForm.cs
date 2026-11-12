using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiDataTopNEditorForm : StiElementEditorForm
{
	internal IStiDataTopN DataTopNElement;

	private JObject storedState;

	private bool isPivot;

	private IContainer components;

	private ToolTip toolTip;

	public StiComboBox comboBoxMode;

	private Label labelMode;

	private StiNumericUpDown numericUpDownCount;

	private Label labelCount;

	private StiTextBox textBoxOthersText;

	private Label labelOthersText;

	public CheckBox checkBoxShowOthers;

	private Label labelMeasure;

	public StiComboBox comboBoxMeasure;

	private StiGroupLine groupLineButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private Panel panelMode;

	private Panel panelMeasure;

	private Panel panelOtherText;

	private string LocModeNone => Loc.GetEnum("SelectionModeNone");

	private string LocModeTop => Loc.GetMain("Top");

	private string LocModeBottom => Loc.GetMain("Bottom");

	internal StiDataTopN TopN => DataTopNElement?.TopN;

	public override bool AllowResize => false;

	public StiDataTopNEditorForm()
		: this(null)
	{
	}

	public StiDataTopNEditorForm(StiElement element, IStiDataTopN topNElement = null)
		: base(element)
	{
		InitializeComponent();
		DataTopNElement = ((topNElement != null) ? topNElement : (base.Element as IStiDataTopN));
		isPivot = base.Element is StiPivotTableElement;
		LocalizeInternal();
		storedState = topNElement?.TopN.SaveToJsonObject(StiJsonSaveMode.Report);
		FillMeasure();
		RefreshUiPanel();
		ComboBoxMode_SelectedIndexChanged(null, null);
		CheckBoxShowOther_CheckedChanged(null, null);
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("PropertyMain", "TopN");
		((Control)labelMode).Text = Loc.GetMain("Mode");
		((Control)labelOthersText).Text = Loc.Get("PropertyMain", "OthersText");
		((Control)checkBoxShowOthers).Text = Loc.Get("PropertyMain", "ShowOthers");
		((Control)labelMeasure).Text = Loc.Get("Dashboard", "Measure");
		((Control)labelCount).Text = Loc.GetMain("Count");
		base.LockRefresh = true;
		((ComboBox)comboBoxMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxMode).SelectedIndex;
		((ComboBox)comboBoxMode).Items.Clear();
		((ComboBox)comboBoxMode).Items.Add((object)LocModeNone);
		((ComboBox)comboBoxMode).Items.Add((object)LocModeTop);
		((ComboBox)comboBoxMode).Items.Add((object)LocModeBottom);
		((ListControl)comboBoxMode).SelectedIndex = selectedIndex;
		if (((ListControl)comboBoxMode).SelectedIndex == -1)
		{
			((ListControl)comboBoxMode).SelectedIndex = 0;
		}
		((ComboBox)comboBoxMode).EndUpdate();
		base.LockRefresh = false;
	}

	private void DoScaling()
	{
		((Control)textBoxOthersText).AutoSize = false;
		((Control)textBoxOthersText).Size = new Size(((Control)textBoxOthersText).Width, StiScale.I(20));
	}

	private void FillMeasure()
	{
		((Control)panelMeasure).Visible = isPivot;
		((Control)groupLineButtons).Visible = isPivot;
		if (!isPivot)
		{
			return;
		}
		StiPivotTableElement stiPivotTableElement = base.Element as StiPivotTableElement;
		base.LockRefresh = true;
		((ComboBox)comboBoxMeasure).BeginUpdate();
		((ComboBox)comboBoxMeasure).Items.Clear();
		foreach (StiPivotSummary summary in stiPivotTableElement.Summaries)
		{
			((ComboBox)comboBoxMeasure).Items.Add((object)((!string.IsNullOrEmpty(summary.Label)) ? summary.Label : StiLabelHelper.GetLabel(summary)));
		}
		((ComboBox)comboBoxMeasure).EndUpdate();
		base.LockRefresh = false;
	}

	public override void WritePropertiesToElement()
	{
		RefreshElementView();
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			base.LockRefresh = true;
			switch (TopN.Mode)
			{
			case StiDataTopNMode.Top:
				((ComboBox)comboBoxMode).SelectedItem = LocModeTop;
				break;
			case StiDataTopNMode.Bottom:
				((ComboBox)comboBoxMode).SelectedItem = LocModeBottom;
				break;
			default:
				((ComboBox)comboBoxMode).SelectedItem = LocModeNone;
				break;
			}
			checkBoxShowOthers.Checked = TopN.ShowOthers;
			((Control)textBoxOthersText).Text = TopN.OthersText;
			numericUpDownCount.Value = TopN.Count;
			((ComboBox)comboBoxMeasure).SelectedItem = TopN.MeasureField;
			if (((ListControl)comboBoxMeasure).SelectedIndex == -1 && ((ComboBox)comboBoxMeasure).Items.Count > 0)
			{
				((ListControl)comboBoxMeasure).SelectedIndex = 0;
				TopN.MeasureField = ((Control)comboBoxMeasure).Text;
			}
			CalculateWindowHeight();
			base.LockRefresh = false;
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected override void CalculateWindowHeight()
	{
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + (from Control c in (IEnumerable)((Control)this).Controls
			where c.Visible
			select c.Height).Sum() + (((Control)groupLineButtons).Visible ? StiScale.YYI(12.0) : 0));
	}

	private void ComboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TopN != null)
		{
			if (((Control)comboBoxMode).Text == LocModeTop)
			{
				TopN.Mode = StiDataTopNMode.Top;
			}
			else if (((Control)comboBoxMode).Text == LocModeBottom)
			{
				TopN.Mode = StiDataTopNMode.Bottom;
			}
			else
			{
				TopN.Mode = StiDataTopNMode.None;
			}
			Label obj = labelCount;
			StiNumericUpDown stiNumericUpDown = numericUpDownCount;
			Label obj2 = labelOthersText;
			StiTextBox stiTextBox = textBoxOthersText;
			Label obj3 = labelMeasure;
			CheckBox obj4 = checkBoxShowOthers;
			bool flag2 = (((Control)comboBoxMeasure).Enabled = TopN.Mode != StiDataTopNMode.None);
			bool flag4 = (((Control)obj4).Enabled = flag2);
			bool flag6 = (((Control)obj3).Enabled = flag4);
			bool flag8 = (((Control)stiTextBox).Enabled = flag6);
			bool flag10 = (((Control)obj2).Enabled = flag8);
			bool enabled = (((Control)stiNumericUpDown).Enabled = flag10);
			((Control)obj).Enabled = enabled;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxMeasure_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TopN != null)
		{
			TopN.MeasureField = ((Control)comboBoxMeasure).Text;
		}
	}

	private void CheckBoxShowOther_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TopN != null)
		{
			TopN.ShowOthers = checkBoxShowOthers.Checked;
			Label obj = labelOthersText;
			bool enabled = (((Control)textBoxOthersText).Enabled = checkBoxShowOthers.Checked && TopN.Mode != StiDataTopNMode.None);
			((Control)obj).Enabled = enabled;
			if (checkBoxShowOthers.Checked)
			{
				((Control)textBoxOthersText).Focus();
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void NumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TopN != null)
		{
			TopN.Count = (int)numericUpDownCount.Value;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void TextBoxOtherText_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TopN != null)
		{
			TopN.OthersText = ((Control)textBoxOthersText).Text;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		if (!isPivot)
		{
			base.OnFormClosed(e);
		}
	}

	private void This_Cancel_Click(object sender, EventArgs e)
	{
		if (storedState != null)
		{
			DataTopNElement.TopN.LoadFromJsonObject(storedState);
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		checkBoxShowOthers = new CheckBox();
		textBoxOthersText = new StiTextBox();
		numericUpDownCount = new StiNumericUpDown();
		labelOthersText = new Label();
		labelCount = new Label();
		labelMode = new Label();
		comboBoxMode = new StiComboBox();
		labelMeasure = new Label();
		comboBoxMeasure = new StiComboBox();
		groupLineButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		panelMode = new Panel();
		panelMeasure = new Panel();
		panelOtherText = new Panel();
		((ISupportInitialize)numericUpDownCount).BeginInit();
		((Control)groupLineButtons).SuspendLayout();
		((Control)panelMode).SuspendLayout();
		((Control)panelMeasure).SuspendLayout();
		((Control)panelOtherText).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)checkBoxShowOthers).BackColor = Color.Transparent;
		((Control)checkBoxShowOthers).Location = new Point(252, 0);
		((Control)checkBoxShowOthers).Margin = new Padding(6, 6, 6, 6);
		((Control)checkBoxShowOthers).Name = "checkBoxShowOthers";
		((Control)checkBoxShowOthers).Size = new Size(384, 37);
		((Control)checkBoxShowOthers).TabIndex = 0;
		((Control)checkBoxShowOthers).Text = " Show Others";
		((ButtonBase)checkBoxShowOthers).UseVisualStyleBackColor = false;
		checkBoxShowOthers.CheckedChanged += CheckBoxShowOther_CheckedChanged;
		((Control)textBoxOthersText).Anchor = (AnchorStyles)13;
		((Control)textBoxOthersText).BackColor = Color.White;
		((Control)textBoxOthersText).Location = new Point(252, 60);
		((Control)textBoxOthersText).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxOthersText).Name = "textBoxOthersText";
		((Control)textBoxOthersText).Size = new Size(390, 42);
		((Control)textBoxOthersText).TabIndex = 1;
		((Control)textBoxOthersText).TextChanged += TextBoxOtherText_TextChanged;
		((Control)numericUpDownCount).BackColor = Color.White;
		numericUpDownCount.DecimalPlaces = 0;
		((Control)numericUpDownCount).Location = new Point(252, 63);
		((Control)numericUpDownCount).Margin = new Padding(4, 4, 4, 4);
		((Control)numericUpDownCount).Name = "numericUpDownCount";
		((Control)numericUpDownCount).Size = new Size(158, 42);
		((Control)numericUpDownCount).TabIndex = 1;
		((Control)numericUpDownCount).Text = "1";
		numericUpDownCount.ValueChanged += NumericUpDown_ValueChanged;
		((Control)labelOthersText).BackColor = Color.White;
		((Control)labelOthersText).ForeColor = Color.DimGray;
		((Control)labelOthersText).Location = new Point(0, 67);
		((Control)labelOthersText).Margin = new Padding(6, 0, 6, 0);
		((Control)labelOthersText).Name = "labelOthersText";
		((Control)labelOthersText).Size = new Size(236, 27);
		((Control)labelOthersText).TabIndex = 2;
		((Control)labelOthersText).Text = "Others Text";
		labelOthersText.TextAlign = (ContentAlignment)16;
		((Control)labelCount).BackColor = Color.White;
		((Control)labelCount).ForeColor = Color.DimGray;
		((Control)labelCount).Location = new Point(0, 67);
		((Control)labelCount).Margin = new Padding(6, 0, 6, 0);
		((Control)labelCount).Name = "labelCount";
		((Control)labelCount).Size = new Size(236, 31);
		((Control)labelCount).TabIndex = 3;
		((Control)labelCount).Text = "Count";
		labelCount.TextAlign = (ContentAlignment)16;
		((Control)labelMode).BackColor = Color.White;
		((Control)labelMode).ForeColor = Color.DimGray;
		((Control)labelMode).Location = new Point(0, 4);
		((Control)labelMode).Margin = new Padding(6, 0, 6, 0);
		((Control)labelMode).Name = "labelMode";
		((Control)labelMode).Size = new Size(236, 33);
		((Control)labelMode).TabIndex = 2;
		((Control)labelMode).Text = "Mode";
		labelMode.TextAlign = (ContentAlignment)16;
		((Control)comboBoxMode).Anchor = (AnchorStyles)13;
		((Control)comboBoxMode).BackColor = SystemColors.Window;
		comboBoxMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxMode).DropDownWidth = 200;
		((Control)comboBoxMode).Location = new Point(252, 0);
		((Control)comboBoxMode).Margin = new Padding(6, 6, 6, 6);
		((Control)comboBoxMode).Name = "comboBoxMode";
		((Control)comboBoxMode).Size = new Size(246, 42);
		((Control)comboBoxMode).TabIndex = 0;
		((ComboBox)comboBoxMode).SelectedIndexChanged += ComboBoxMode_SelectedIndexChanged;
		((Control)labelMeasure).BackColor = Color.White;
		((Control)labelMeasure).ForeColor = Color.DimGray;
		((Control)labelMeasure).Location = new Point(0, 6);
		((Control)labelMeasure).Margin = new Padding(6, 0, 6, 0);
		((Control)labelMeasure).Name = "labelMeasure";
		((Control)labelMeasure).Size = new Size(236, 33);
		((Control)labelMeasure).TabIndex = 1;
		((Control)labelMeasure).Text = "Measure";
		labelMeasure.TextAlign = (ContentAlignment)16;
		((Control)comboBoxMeasure).Anchor = (AnchorStyles)13;
		((Control)comboBoxMeasure).BackColor = SystemColors.Window;
		comboBoxMeasure.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxMeasure).DropDownWidth = 200;
		((Control)comboBoxMeasure).Location = new Point(252, 0);
		((Control)comboBoxMeasure).Margin = new Padding(6, 6, 6, 6);
		((Control)comboBoxMeasure).Name = "comboBoxMeasure";
		((Control)comboBoxMeasure).Size = new Size(386, 42);
		((Control)comboBoxMeasure).TabIndex = 0;
		((ComboBox)comboBoxMeasure).SelectedIndexChanged += ComboBoxMeasure_SelectedIndexChanged;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(24, 421);
		((Control)groupLineButtons).Margin = new Padding(6, 6, 6, 6);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(642, 69);
		((Control)groupLineButtons).TabIndex = 3;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(493, 23);
		((Control)buttonCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_Cancel_Click;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(329, 23);
		((Control)buttonOk).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)panelMode).BackColor = Color.White;
		((Control)panelMode).Controls.Add((Control)(object)comboBoxMode);
		((Control)panelMode).Controls.Add((Control)(object)labelCount);
		((Control)panelMode).Controls.Add((Control)(object)labelMode);
		((Control)panelMode).Controls.Add((Control)(object)numericUpDownCount);
		((Control)panelMode).Dock = (DockStyle)1;
		((Control)panelMode).Location = new Point(24, 23);
		((Control)panelMode).Margin = new Padding(4, 4, 4, 4);
		((Control)panelMode).Name = "panelMode";
		((Control)panelMode).Size = new Size(642, 127);
		((Control)panelMode).TabIndex = 0;
		((Control)panelMeasure).BackColor = Color.White;
		((Control)panelMeasure).Controls.Add((Control)(object)comboBoxMeasure);
		((Control)panelMeasure).Controls.Add((Control)(object)labelMeasure);
		((Control)panelMeasure).Dock = (DockStyle)1;
		((Control)panelMeasure).Location = new Point(24, 150);
		((Control)panelMeasure).Margin = new Padding(4, 4, 4, 4);
		((Control)panelMeasure).Name = "panelMeasure";
		((Control)panelMeasure).Size = new Size(642, 67);
		((Control)panelMeasure).TabIndex = 1;
		((Control)panelOtherText).BackColor = Color.White;
		((Control)panelOtherText).Controls.Add((Control)(object)checkBoxShowOthers);
		((Control)panelOtherText).Controls.Add((Control)(object)labelOthersText);
		((Control)panelOtherText).Controls.Add((Control)(object)textBoxOthersText);
		((Control)panelOtherText).Dock = (DockStyle)1;
		((Control)panelOtherText).Location = new Point(24, 217);
		((Control)panelOtherText).Margin = new Padding(4, 4, 4, 4);
		((Control)panelOtherText).Name = "panelOtherText";
		((Control)panelOtherText).Size = new Size(642, 125);
		((Control)panelOtherText).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(690, 513);
		((Control)this).Controls.Add((Control)(object)panelOtherText);
		((Control)this).Controls.Add((Control)(object)panelMeasure);
		((Control)this).Controls.Add((Control)(object)panelMode);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).Margin = new Padding(4, 4, 4, 4);
		((Control)this).Name = "StiDataTopNEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "TopN";
		((ISupportInitialize)numericUpDownCount).EndInit();
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)panelMode).ResumeLayout(false);
		((Control)panelMeasure).ResumeLayout(false);
		((Control)panelOtherText).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
