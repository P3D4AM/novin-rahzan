using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Editor;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors.PivotTable;

public class StiPivotTableElementConditionsEditorForm : Form, IStiElementConditionsEditorForm
{
	private List<StiPivotTableElementCondition> clonePivotTableConditions;

	private StiPivotTableElement pivotTableElement;

	private string key;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiZoneContainerControl panelConditions;

	private Button buttonAddCondition;

	public StiPivotTableElementConditionsEditorForm(IStiPivotTableElement pivotTableElement)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp && StiLocalization.CultureName == "ru")
			{
				((Form)this).HelpButton = true;
			}
			key = StiPivotTableToCrossTabCache.GetKey(pivotTableElement);
			this.pivotTableElement = (StiPivotTableElement)pivotTableElement;
			clonePivotTableConditions = this.pivotTableElement.PivotTableConditions.ConvertAll((IStiPivotTableElementCondition item) => (StiPivotTableElementCondition)item.Clone());
			clonePivotTableConditions.Reverse();
			Localize();
			Build();
			((Form)this).ShowIcon = false;
			panelConditions.EmptyImage = StiReportDesignImages.Actions.Conditions(StiImageSize.Double, allowCache: false);
			panelConditions.EmptyText = StiLocalization.Get("Chart", "NoConditions");
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Conditions");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddCondition).Text = StiLocalization.Get("Chart", "AddCondition");
	}

	private void Build()
	{
		((Control)panelConditions).Controls.Clear();
		foreach (StiPivotTableElementCondition clonePivotTableCondition in clonePivotTableConditions)
		{
			AddLevel(clonePivotTableCondition, isNew: false);
		}
		UpdateStateGroupLines();
	}

	private void AddLevel(StiPivotTableElementCondition condition, bool isNew)
	{
		StiPivotTableElementConditionsControl stiPivotTableElementConditionsControl = new StiPivotTableElementConditionsControl(pivotTableElement, condition, isNew);
		((Control)panelConditions).Controls.Add((Control)(object)stiPivotTableElementConditionsControl);
	}

	private void SaveConditions()
	{
		List<IStiPivotTableElementCondition> list = new List<IStiPivotTableElementCondition>();
		foreach (StiPivotTableElementConditionsControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			list.Add(item.GetPivotTableElementCondition());
		}
		pivotTableElement.PivotTableConditions = list;
		StiPivotTableToCrossTabCache.Remove(key);
	}

	private void UpdateStateGroupLines()
	{
		foreach (StiPivotTableElementConditionsControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			((Control)item.groupLine).Visible = ((ArrangedElementCollection)((Control)panelConditions).Controls).Count > 1;
		}
	}

	private void ButtonAdd_Click(object sender, EventArgs e)
	{
		StiPivotTableElementCondition condition = new StiPivotTableElementCondition();
		AddLevel(condition, isNew: true);
		UpdateStateGroupLines();
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
		SaveConditions();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_conditions.htm");
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		stiGroupLine1 = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		panelConditions = new StiZoneContainerControl();
		buttonAddCondition = new Button();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 777);
		((Control)stiGroupLine1).Margin = new Padding(6, 6, 6, 6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(1154, 88);
		((Control)stiGroupLine1).TabIndex = 70;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(980, 23);
		((Control)buttonCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 63;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(818, 23);
		((Control)buttonOk).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 62;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)panelConditions).AllowDrop = true;
		((Control)panelConditions).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelConditions).AutoScroll = true;
		((Control)panelConditions).BackColor = Color.White;
		panelConditions.EmptyImage = null;
		panelConditions.EmptyText = null;
		((Control)panelConditions).Location = new Point(24, 94);
		((Control)panelConditions).Margin = new Padding(4, 4, 4, 4);
		((Control)panelConditions).Name = "panelConditions";
		((Control)panelConditions).Padding = new Padding(4, 4, 4, 4);
		panelConditions.ShowBorder = true;
		((Control)panelConditions).Size = new Size(1108, 660);
		((Control)panelConditions).TabIndex = 72;
		((Control)buttonAddCondition).Anchor = (AnchorStyles)10;
		((Control)buttonAddCondition).Location = new Point(24, 23);
		((Control)buttonAddCondition).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonAddCondition).Name = "buttonAddCondition";
		((Control)buttonAddCondition).Size = new Size(226, 46);
		((Control)buttonAddCondition).TabIndex = 75;
		((Control)buttonAddCondition).Text = "Add Condition";
		((ButtonBase)buttonAddCondition).UseVisualStyleBackColor = true;
		((Control)buttonAddCondition).Click += ButtonAdd_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(1154, 865);
		((Control)this).Controls.Add((Control)(object)buttonAddCondition);
		((Control)this).Controls.Add((Control)(object)panelConditions);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPivotTableElementConditionsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Conditions";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	DialogResult IStiElementConditionsEditorForm.ShowDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}
}
