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
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartStripsEditorForm : StiInplaceToolForm
{
	private StiChart chart;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiStripListBox listBoxItems;

	private Button buttonAddStrip;

	private StiReport Report => chart?.Report;

	private StiDesignerControl Designer { get; set; }

	private StiPropertyGrid PropertyGrid { get; set; }

	public StiChartStripsEditorForm(StiChart chart)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.chart = chart;
			Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
			PropertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(Designer);
			DoScaling();
			Build();
			Localize();
			((Form)this).ShowIcon = false;
			((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		}
	}

	private void Build()
	{
		if (chart.Strips == null)
		{
			return;
		}
		int num = 1;
		foreach (StiStrips strip in chart.Strips)
		{
			((ListBox)listBoxItems).Items.Add((object)strip);
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

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Chart", "StripsEditorForm");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddStrip).Text = StiLocalization.Get("Chart", "AddStrip");
		((Control)buttonAddStrip).Width = TextRenderer.MeasureText(((Control)buttonAddStrip).Text, ((Control)buttonAddStrip).Font).Width + StiScaleUI.I8;
	}

	private void ButtonAddStrip_Click(object sender, EventArgs e)
	{
		StiStrips stiStrips = new StiStrips();
		((ListBox)listBoxItems).Items.Add((object)stiStrips);
		((ListBox)listBoxItems).SelectedItem = stiStrips;
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		base.OnFormClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				((PropertyGrid)PropertyGrid).SelectedObject = chart;
				((PropertyGrid)PropertyGrid).PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
			}
		}
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
		Designer.UndoRedoSave(Loc.Get("PropertyMain", "Strips"), onlyCurrentPage: true);
		chart.Strips.Clear();
		StiStripsCollection strips = chart.Strips;
		IStiStrips[] values = ((IEnumerable)((ListBox)listBoxItems).Items).Cast<StiStrips>().ToArray();
		strips.AddRange(values);
		chart.ApplyStyle();
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
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		buttonAddStrip = new Button();
		listBoxItems = new StiStripListBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 424);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(306, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(219, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(138, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonAddStrip).Location = new Point(12, 12);
		((Control)buttonAddStrip).Name = "buttonAddStrip";
		((Control)buttonAddStrip).Size = new Size(113, 24);
		((Control)buttonAddStrip).TabIndex = 0;
		((Control)buttonAddStrip).Text = "Add Strip Line";
		((ButtonBase)buttonAddStrip).UseVisualStyleBackColor = true;
		((Control)buttonAddStrip).Click += ButtonAddStrip_Click;
		((Control)listBoxItems).AllowDrop = true;
		((Control)listBoxItems).Location = new Point(12, 49);
		((Control)listBoxItems).Margin = new Padding(3, 3, 3, 0);
		((Control)listBoxItems).Name = "listBoxItems";
		((Control)listBoxItems).Size = new Size(282, 359);
		((Control)listBoxItems).TabIndex = 1;
		((ListBox)listBoxItems).SelectedIndexChanged += ListBoxItems_SelectedIndexChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(306, 470);
		((Control)this).Controls.Add((Control)(object)buttonAddStrip);
		((Control)this).Controls.Add((Control)(object)listBoxItems);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(3);
		((Control)this).MinimumSize = new Size(290, 375);
		((Control)this).Name = "StiChartStripsEditorForm";
		((Control)this).Text = "Strips";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
