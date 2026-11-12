using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Dashboard.Design.Editors.Chart;

public class StiChartConstantLinesEditorForm : Form
{
	private StiChartElement chart;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiChartConstantLineListBox listBoxItems;

	private Button buttonAddConstantLine;

	private StiReport Report => chart?.Report;

	private StiDesignerControl Designer => Report?.Designer?.DesignerControl as StiDesignerControl;

	private StiPropertyGrid PropertyGrid => StiPropertiesPanelHelper.GetPropertyGrid(Designer);

	public StiChartConstantLinesEditorForm(StiChartElement chart)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.chart = chart;
			DoScaling();
			Localize();
			Build();
			((Form)this).ShowIcon = false;
			((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		}
	}

	private void Build()
	{
		if (chart.ConstantLines == null)
		{
			return;
		}
		int num = 1;
		foreach (StiChartConstantLines constantLine in chart.ConstantLines)
		{
			((ListBox)listBoxItems).Items.Add((object)constantLine);
			num++;
		}
		if (((ListBox)listBoxItems).Items.Count > 0)
		{
			((ListControl)listBoxItems).SelectedIndex = 0;
		}
	}

	private void DoScaling()
	{
		((ListBox)listBoxItems).ItemHeight = StiScale.I(24);
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "ConstantLinesEditorForm");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddConstantLine).Text = StiLocalization.Get("Chart", "AddConstantLine");
		((Control)buttonAddConstantLine).Width = TextRenderer.MeasureText(((Control)buttonAddConstantLine).Text, ((Control)buttonAddConstantLine).Font).Width + StiScaleUI.I8;
	}

	private void FillConstantLines()
	{
		chart.ConstantLines.Clear();
		chart.ConstantLines.AddRange(((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiChartConstantLines>().ToArray());
	}

	private void ButtonAddConstantLine_Click(object sender, EventArgs e)
	{
		StiChartConstantLines stiChartConstantLines = new StiChartConstantLines();
		((ListBox)listBoxItems).Items.Add((object)stiChartConstantLines);
		((ListBox)listBoxItems).SelectedItem = stiChartConstantLines;
		((Control)listBoxItems).Focus();
	}

	private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxItems).SelectedItem != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, ((ListBox)listBoxItems).SelectedItem);
		}
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		((Control)listBoxItems).Invalidate();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		((Form)this).OnFormClosed(e);
		((PropertyGrid)PropertyGrid).SelectedObject = chart;
		((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Designer.UndoRedoSave(Loc.Get("PropertyMain", "ConstantLines"), onlyCurrentPage: true);
		FillConstantLines();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		buttonAddConstantLine = new Button();
		listBoxItems = new StiChartConstantLineListBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 816);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(612, 88);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(438, 23);
		((Control)buttonCancel).Margin = new Padding(6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(276, 23);
		((Control)buttonOk).Margin = new Padding(6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonAddConstantLine).Anchor = (AnchorStyles)10;
		((Control)buttonAddConstantLine).Location = new Point(24, 24);
		((Control)buttonAddConstantLine).Margin = new Padding(6);
		((Control)buttonAddConstantLine).Name = "buttonAddConstantLine";
		((Control)buttonAddConstantLine).Size = new Size(226, 46);
		((Control)buttonAddConstantLine).TabIndex = 0;
		((Control)buttonAddConstantLine).Text = "Add Constant Line";
		((ButtonBase)buttonAddConstantLine).UseVisualStyleBackColor = true;
		((Control)buttonAddConstantLine).Click += ButtonAddConstantLine_Click;
		((Control)listBoxItems).AllowDrop = true;
		((Control)listBoxItems).Location = new Point(24, 94);
		((Control)listBoxItems).Margin = new Padding(6, 6, 6, 0);
		((Control)listBoxItems).Name = "treeViewItems";
		((Control)listBoxItems).Size = new Size(564, 690);
		((Control)listBoxItems).TabIndex = 1;
		((ListBox)listBoxItems).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(612, 904);
		((Control)this).Controls.Add((Control)(object)buttonAddConstantLine);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(564, 686);
		((Control)this).Name = "StiChartConstantLinesEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Constants Lines";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
