using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueBoolItemControl : UserControl, IStiValueItemControl
{
	private IContainer components;

	private CheckBox checkBoxValue;

	private StiParameterDropDownButtonV2 dropDownButton;

	internal CheckBox checkBoxNotAssigned;

	public object Value
	{
		get
		{
			if (!checkBoxNotAssigned.Checked)
			{
				return checkBoxValue.Checked;
			}
			return null;
		}
	}

	public StiVariable Variable { get; }

	public StiReport Report { get; }

	public event EventHandler SelectedValue;

	public void InvokeSelectedValue()
	{
	}

	private void InvokeSelectedValue(object sender)
	{
		this.SelectedValue?.Invoke(sender, EventArgs.Empty);
	}

	public void FilterValues(object selectedObject)
	{
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(Variable.Type);
		HashSet<bool> hashSet = new HashSet<bool>();
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		foreach (StiBoolDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					list.Add(new StiDialogInfoValue(item.Key, item.Value, item2));
				}
			}
		}
		dropDownButton.ListValues = list;
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		if (Variable.DialogInfo.AllowUserValues)
		{
			((Control)checkBoxValue).Enabled = !checkBoxNotAssigned.Checked;
		}
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue is bool)
		{
			checkBoxValue.Checked = (bool)e.NewValue;
			InvokeSelectedValue(e.NewValue);
		}
		else if (((Control)checkBoxNotAssigned).Visible)
		{
			checkBoxNotAssigned.Checked = true;
		}
		StiVariableHelper.SetVariableLabel(Report, Variable, e.Label);
	}

	public StiValueBoolItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		Variable = variable;
		Report = report;
		InitializeComponent();
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(bool), null, null, report);
		andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		foreach (StiBoolDialogInfoItem item2 in andParseDialogInfoItems)
		{
			StiDialogInfoValue item = new StiDialogInfoValue(item2.Key, item2.Value);
			list.Add(item);
		}
		dropDownButton.ListValues = list;
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		((Control)checkBoxNotAssigned).Visible = allowUserValues && variable.Type == typeof(bool?);
		((Control)checkBoxValue).Enabled = allowUserValues;
		switch (variable.Selection)
		{
		case StiSelectionMode.Nothing:
			if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
			break;
		case StiSelectionMode.First:
			if (list.Count > 0 && list[0].Key is bool)
			{
				checkBoxValue.Checked = (bool)list[0].Key;
			}
			break;
		case StiSelectionMode.FromVariable:
		{
			bool @checked = false;
			if (variable.ValueObject is bool)
			{
				@checked = (bool)variable.ValueObject;
			}
			if (variable.InitBy == StiVariableInitBy.Expression)
			{
				try
				{
					StiText component = new StiText
					{
						Name = "**VariableRequestFromUser**",
						Page = report.Pages[0]
					};
					object obj = StiParser.ParseTextValue("{" + variable.Value + "}", component);
					if (obj is bool)
					{
						@checked = (bool)obj;
					}
				}
				catch
				{
					if (report[variable.Name] != null)
					{
						@checked = (bool)report[variable.Name];
					}
				}
			}
			if (variablesCurrentValues.ContainsKey(variable.Name))
			{
				@checked = Convert.ToBoolean(variablesCurrentValues[variable.Name]);
			}
			checkBoxValue.Checked = @checked;
			if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
			break;
		}
		}
		((Control)checkBoxNotAssigned).Text = Loc.Get("Report", "Null");
		dropDownButton.ShowFullBorder = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		checkBoxNotAssigned = new CheckBox();
		checkBoxValue = new CheckBox();
		dropDownButton = new StiParameterDropDownButtonV2();
		((Control)this).SuspendLayout();
		((Control)checkBoxNotAssigned).AutoSize = true;
		((Control)checkBoxNotAssigned).Dock = (DockStyle)3;
		((Control)checkBoxNotAssigned).Location = new Point(39, 0);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Padding = new Padding(4, 0, 0, 0);
		((Control)checkBoxNotAssigned).Size = new Size(48, 20);
		((Control)checkBoxNotAssigned).TabIndex = 3;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)checkBoxValue).AutoSize = true;
		((Control)checkBoxValue).Dock = (DockStyle)3;
		((Control)checkBoxValue).Location = new Point(0, 0);
		((Control)checkBoxValue).Name = "checkBoxValue";
		((Control)checkBoxValue).Padding = new Padding(0, 0, 4, 0);
		((Control)checkBoxValue).Size = new Size(19, 20);
		((Control)checkBoxValue).TabIndex = 1;
		((ButtonBase)checkBoxValue).UseVisualStyleBackColor = true;
		((Control)dropDownButton).Dock = (DockStyle)3;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).DropDownMenu = null;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(19, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(20, 20);
		((Control)dropDownButton).TabIndex = 2;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Controls.Add((Control)(object)checkBoxValue);
		((Control)this).Name = "StiValueBoolItemControl";
		((Control)this).Size = new Size(175, 20);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
