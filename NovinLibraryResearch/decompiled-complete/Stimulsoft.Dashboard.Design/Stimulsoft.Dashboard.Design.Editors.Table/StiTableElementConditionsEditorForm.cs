using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Editor;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors.Table;

public class StiTableElementConditionsEditorForm : Form, IStiElementConditionsEditorForm
{
	private List<StiTableElementCondition> cloneTableConditions;

	private StiTableElement tableElement;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button buttonCancel;

	private Button buttonOk;

	private StiZoneContainerControl panelConditions;

	private Button buttonAddCondition;

	public StiTableElementConditionsEditorForm(IStiTableElement tableElement)
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
			this.tableElement = (StiTableElement)tableElement;
			cloneTableConditions = this.tableElement.TableConditions.ConvertAll((IStiTableElementCondition item) => (StiTableElementCondition)((ICloneable)item).Clone()).ToList();
			cloneTableConditions.Reverse();
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
		foreach (StiTableElementCondition cloneTableCondition in cloneTableConditions)
		{
			AddLevel(cloneTableCondition, isNew: false);
		}
		UpdateStateGroupLines();
	}

	private void AddLevel(StiTableElementCondition condition, bool isNew = true)
	{
		StiTableElementConditionsControl stiTableElementConditionsControl = new StiTableElementConditionsControl(tableElement, condition, isNew);
		((Control)panelConditions).Controls.Add((Control)(object)stiTableElementConditionsControl);
	}

	private void SaveConditions()
	{
		List<IStiTableElementCondition> list = new List<IStiTableElementCondition>();
		foreach (StiTableElementConditionsControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			list.Add(item.GetTableElementCondition());
		}
		tableElement.TableConditions = list;
	}

	private void UpdateStateGroupLines()
	{
		foreach (StiTableElementConditionsControl item in (ArrangedElementCollection)((Control)panelConditions).Controls)
		{
			((Control)item.groupLine).Visible = ((ArrangedElementCollection)((Control)panelConditions).Controls).Count > 1;
		}
	}

	private void ButtonAdd_Click(object sender, EventArgs e)
	{
		StiTableElementCondition condition = new StiTableElementCondition();
		AddLevel(condition);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		buttonAddCondition = new Button();
		buttonOk = new Button();
		buttonCancel = new Button();
		panelConditions = new StiZoneContainerControl();
		stiGroupLine1 = new StiGroupLine();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonAddCondition).Location = new Point(12, 12);
		((Control)buttonAddCondition).Name = "buttonAddCondition";
		((Control)buttonAddCondition).Size = new Size(113, 24);
		((Control)buttonAddCondition).TabIndex = 73;
		((Control)buttonAddCondition).Text = "Add Condition";
		((ButtonBase)buttonAddCondition).UseVisualStyleBackColor = true;
		((Control)buttonAddCondition).Click += ButtonAdd_Click;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(496, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 62;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(577, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 63;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelConditions).AllowDrop = true;
		((Control)panelConditions).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelConditions).AutoScroll = true;
		((Control)panelConditions).BackColor = Color.White;
		panelConditions.EmptyImage = null;
		panelConditions.EmptyText = null;
		((Control)panelConditions).Location = new Point(12, 49);
		((Control)panelConditions).Margin = new Padding(2, 2, 2, 2);
		((Control)panelConditions).Name = "panelConditions";
		((Control)panelConditions).Padding = new Padding(2);
		panelConditions.ShowBorder = true;
		((Control)panelConditions).Size = new Size(641, 278);
		((Control)panelConditions).TabIndex = 72;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 338);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(664, 46);
		((Control)stiGroupLine1).TabIndex = 70;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(664, 384);
		((Control)this).Controls.Add((Control)(object)buttonAddCondition);
		((Control)this).Controls.Add((Control)(object)panelConditions);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiTableElementConditionsEditorForm";
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
