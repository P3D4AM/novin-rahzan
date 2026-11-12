using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Design.DataTransformation.Editors;

public class StiDataTransformLimitForm : Form
{
	private List<StiDataActionRule> actionRules;

	private string columnKey;

	private string columnPath;

	private StiDataTransformationRestrictions restrictions;

	private IContainer components;

	private Label labelFirstIndex;

	private TextBox textBoxFirstIndex;

	private Label labelRowsCount;

	private StiGroupLine stiGroupLine1;

	private Button buttonOk;

	private Button buttonCancel;

	private ComboBox comboBoxRowsCount;

	private ComboBox comboBoxPriority;

	private Label labelPriority;

	private string LocSelectAll => Loc.Get("Dashboard", "SelectAll").Replace("&", "");

	public StiDataTransformLimitForm(List<StiDataActionRule> actionRules, string columnKey, string columnPath, StiDataTransformationRestrictions restrictions)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.actionRules = actionRules;
		this.columnKey = columnKey;
		this.columnPath = columnPath;
		this.restrictions = restrictions;
		Localize();
		ReadColumnActions();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Dashboard", "LimitRows");
		((Control)labelFirstIndex).Text = Loc.Get("Dashboard", "SkipFirstRows");
		((Control)labelRowsCount).Text = Loc.Get("Dashboard", "RowsCount");
		((Control)labelPriority).Text = Loc.Get("Dashboard", "Priority");
		comboBoxPriority.Items.Clear();
		comboBoxPriority.Items.Add((object)Loc.Get("Dashboard", "BeforeTransformation"));
		comboBoxPriority.Items.Add((object)Loc.Get("Dashboard", "AfterGroupingData"));
		comboBoxPriority.Items.Add((object)Loc.Get("Dashboard", "AfterSortingData"));
		((Control)comboBoxPriority).Text = (string)comboBoxPriority.Items[1];
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
		comboBoxRowsCount.Items[0] = LocSelectAll;
		((ListControl)comboBoxRowsCount).SelectedIndex = 0;
	}

	private void ReadColumnActions()
	{
		StiDataActionRule stiDataActionRule = actionRules.FirstOrDefault((StiDataActionRule a) => a.Type == StiDataActionType.Limit);
		if (stiDataActionRule != null)
		{
			((Control)textBoxFirstIndex).Text = stiDataActionRule.StartIndex.ToString();
			((ListControl)comboBoxRowsCount).SelectedIndex = -1;
			((Control)comboBoxRowsCount).Text = ((stiDataActionRule.RowsCount < 0) ? LocSelectAll : stiDataActionRule.RowsCount.ToString());
			((ListControl)comboBoxPriority).SelectedIndex = (int)stiDataActionRule.Priority;
		}
		if (!IsAllowed(StiDataTransformationRestrictions.Priority))
		{
			((Control)labelPriority).Enabled = false;
			((Control)comboBoxPriority).Enabled = false;
			((ListControl)comboBoxPriority).SelectedIndex = 1;
		}
	}

	private void WriteColumnActions()
	{
		int result;
		bool flag = int.TryParse(((Control)comboBoxRowsCount).Text, out result);
		StiDataActionRule stiDataActionRule = new StiDataActionRule(columnKey, columnPath, Math.Max(0, StiValueHelper.TryToInt(((Control)textBoxFirstIndex).Text)), (!flag || result < 0) ? (-1) : result, (StiDataActionPriority)((ListControl)comboBoxPriority).SelectedIndex);
		actionRules.RemoveAll((StiDataActionRule a) => a.Type == StiDataActionType.Limit);
		if (stiDataActionRule.StartIndex > 0 || stiDataActionRule.RowsCount >= 0)
		{
			actionRules.Add(stiDataActionRule);
		}
	}

	private bool IsAllowed(StiDataTransformationRestrictions restriction)
	{
		return (restrictions & restriction) != restriction;
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		((Control)textBoxFirstIndex).Focus();
	}

	private void TextBoxFirstIndex_Leave(object sender, EventArgs e)
	{
		((Control)textBoxFirstIndex).Text = Math.Max(0, StiValueHelper.TryToInt(((Control)textBoxFirstIndex).Text)).ToString();
	}

	private void ComboBoxRowsCount_Leave(object sender, EventArgs e)
	{
		int result;
		bool flag = int.TryParse(((Control)comboBoxRowsCount).Text, out result);
		((Control)comboBoxRowsCount).Text = ((!flag || result < 0) ? LocSelectAll : result.ToString());
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		WriteColumnActions();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		labelFirstIndex = new Label();
		textBoxFirstIndex = new TextBox();
		labelRowsCount = new Label();
		stiGroupLine1 = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		comboBoxRowsCount = new ComboBox();
		comboBoxPriority = new ComboBox();
		labelPriority = new Label();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelFirstIndex).ForeColor = Color.DimGray;
		((Control)labelFirstIndex).Location = new Point(12, 11);
		((Control)labelFirstIndex).Name = "labelFirstIndex";
		((Control)labelFirstIndex).Size = new Size(120, 20);
		((Control)labelFirstIndex).TabIndex = 4;
		((Control)labelFirstIndex).Text = "Skip First Rows";
		labelFirstIndex.TextAlign = (ContentAlignment)16;
		((Control)textBoxFirstIndex).Location = new Point(136, 12);
		((Control)textBoxFirstIndex).Name = "textBoxFirstIndex";
		((Control)textBoxFirstIndex).Size = new Size(218, 20);
		((Control)textBoxFirstIndex).TabIndex = 0;
		((Control)textBoxFirstIndex).Text = "0";
		((Control)textBoxFirstIndex).Leave += TextBoxFirstIndex_Leave;
		((Control)labelRowsCount).ForeColor = Color.DimGray;
		((Control)labelRowsCount).Location = new Point(12, 44);
		((Control)labelRowsCount).Name = "labelRowsCount";
		((Control)labelRowsCount).Size = new Size(115, 21);
		((Control)labelRowsCount).TabIndex = 5;
		((Control)labelRowsCount).Text = "Rows Count";
		labelRowsCount.TextAlign = (ContentAlignment)16;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 112);
		((Control)stiGroupLine1).Margin = new Padding(2, 2, 2, 2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(366, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(200, 12);
		((Control)buttonOk).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += ButtonOk_Click;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(279, 12);
		((Control)buttonCancel).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((ListControl)comboBoxRowsCount).FormattingEnabled = true;
		comboBoxRowsCount.Items.AddRange(new object[13]
		{
			"All", "10", "20", "50", "100", "200", "300", "500", "1000", "2000",
			"3000", "5000", "10000"
		});
		((Control)comboBoxRowsCount).Location = new Point(136, 44);
		((Control)comboBoxRowsCount).Name = "comboBoxRowsCount";
		((Control)comboBoxRowsCount).Size = new Size(218, 21);
		((Control)comboBoxRowsCount).TabIndex = 1;
		((Control)comboBoxRowsCount).Text = "All";
		((Control)comboBoxRowsCount).Leave += ComboBoxRowsCount_Leave;
		comboBoxPriority.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxPriority).FormattingEnabled = true;
		comboBoxPriority.Items.AddRange(new object[3] { "BeforeTransformation", "AfterGrouping", "AfterSorting" });
		((Control)comboBoxPriority).Location = new Point(136, 78);
		((Control)comboBoxPriority).Name = "comboBoxPriority";
		((Control)comboBoxPriority).Size = new Size(218, 21);
		((Control)comboBoxPriority).TabIndex = 6;
		((Control)labelPriority).ForeColor = Color.DimGray;
		((Control)labelPriority).Location = new Point(12, 77);
		((Control)labelPriority).Name = "labelPriority";
		((Control)labelPriority).Size = new Size(115, 21);
		((Control)labelPriority).TabIndex = 7;
		((Control)labelPriority).Text = "Priority";
		labelPriority.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(366, 158);
		((Control)this).Controls.Add((Control)(object)comboBoxPriority);
		((Control)this).Controls.Add((Control)(object)labelPriority);
		((Control)this).Controls.Add((Control)(object)comboBoxRowsCount);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)labelRowsCount);
		((Control)this).Controls.Add((Control)(object)textBoxFirstIndex);
		((Control)this).Controls.Add((Control)(object)labelFirstIndex);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataTransformLimitForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Limit Rows";
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
