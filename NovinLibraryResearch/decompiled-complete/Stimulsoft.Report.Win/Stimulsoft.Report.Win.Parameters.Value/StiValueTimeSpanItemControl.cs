using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueTimeSpanItemControl : UserControl, IStiValueItemControl
{
	private CultureInfo culture;

	private IContainer components;

	internal CheckBox checkBoxNotAssigned;

	private StiParameterDropDownButtonV2 dropDownButton;

	private StiTimeSpanPicker timeSpanPicker;

	public object Value
	{
		get
		{
			if (((Control)checkBoxNotAssigned).Visible && checkBoxNotAssigned.Checked)
			{
				return null;
			}
			return timeSpanPicker.Value;
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
		HashSet<DateTime> hashSet = new HashSet<DateTime>();
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		foreach (StiDateTimeDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					list.Add(new StiDialogInfoValue(item.Key, item.Value, item2, culture));
				}
			}
		}
		dropDownButton.ListValues = list;
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue == null)
		{
			if (((Control)checkBoxNotAssigned).Visible)
			{
				checkBoxNotAssigned.Checked = true;
			}
		}
		else
		{
			timeSpanPicker.Value = (TimeSpan)e.NewValue;
			InvokeSelectedValue(e.NewValue);
		}
		StiVariableHelper.SetVariableLabel(Report, Variable, e.Label);
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		((Control)timeSpanPicker).Enabled = !checkBoxNotAssigned.Checked;
	}

	public StiValueTimeSpanItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		InitializeComponent();
		Variable = variable;
		Report = report;
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		if (!allowUserValues)
		{
			dropDownButton.IsKeyResult = false;
		}
		dropDownButton.Variable = variable;
		timeSpanPicker.ReadOnly = variable.ReadOnly;
		if (!string.IsNullOrEmpty(report.Culture))
		{
			try
			{
				culture = new CultureInfo(report.Culture);
			}
			catch
			{
			}
		}
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(TimeSpan), culture, null, report);
		andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		for (int i = 0; i < andParseDialogInfoItems.Count; i++)
		{
			StiDialogInfoValue stiDialogInfoValue = new StiDialogInfoValue
			{
				IsDateTime = true,
				Value = andParseDialogInfoItems[i].Value
			};
			stiDialogInfoValue.Key = ((StiTimeSpanDialogInfoItem)andParseDialogInfoItems[i]).Key;
			stiDialogInfoValue.Culture = culture;
			list.Add(stiDialogInfoValue);
		}
		dropDownButton.ListValues = list;
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		((Control)checkBoxNotAssigned).Visible = allowUserValues && variable.Type == typeof(TimeSpan?);
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)timeSpanPicker).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)timeSpanPicker).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		switch (variable.Selection)
		{
		case StiSelectionMode.First:
			if (list.Count > 0)
			{
				if (list[0].Key is TimeSpan value2)
				{
					timeSpanPicker.Value = value2;
				}
			}
			else
			{
				((Control)timeSpanPicker).Enabled = false;
			}
			break;
		case StiSelectionMode.FromVariable:
		{
			if (variable.ValueObject == null)
			{
				timeSpanPicker.Value = default(TimeSpan);
			}
			else if (variable.ValueObject is TimeSpan value)
			{
				timeSpanPicker.Value = value;
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
					object obj2 = StiParser.ParseTextValue("{" + variable.Value + "}", component);
					if (obj2 != null && TimeSpan.TryParse(obj2.ToString(), out var result))
					{
						timeSpanPicker.Value = result;
					}
				}
				catch
				{
					if (report[variable.Name] != null)
					{
						timeSpanPicker.Value = (TimeSpan)report[variable.Name];
					}
				}
			}
			if (((Control)checkBoxNotAssigned).Visible)
			{
				((Control)timeSpanPicker).Enabled = variable.ValueObject != null;
				checkBoxNotAssigned.Checked = variable.ValueObject == null || variable.Value == "null";
			}
			if (!variablesCurrentValues.ContainsKey(variable.Name))
			{
				break;
			}
			if (variablesCurrentValues[variable.Name] == null)
			{
				((Control)timeSpanPicker).Enabled = false;
				checkBoxNotAssigned.Checked = true;
				break;
			}
			((Control)timeSpanPicker).Enabled = true;
			checkBoxNotAssigned.Checked = false;
			string s = variablesCurrentValues[variable.Name].ToString();
			if (TimeSpan.TryParse(s, out var result2))
			{
				timeSpanPicker.Value = result2;
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
		checkBoxNotAssigned = new CheckBox();
		timeSpanPicker = new StiTimeSpanPicker();
		dropDownButton = new StiParameterDropDownButtonV2();
		((Control)this).SuspendLayout();
		((Control)checkBoxNotAssigned).AutoSize = true;
		((Control)checkBoxNotAssigned).Dock = (DockStyle)4;
		((Control)checkBoxNotAssigned).Location = new Point(127, 0);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Padding = new Padding(4, 0, 0, 0);
		((Control)checkBoxNotAssigned).Size = new Size(48, 21);
		((Control)checkBoxNotAssigned).TabIndex = 5;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		((Control)checkBoxNotAssigned).Visible = false;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)timeSpanPicker).Dock = (DockStyle)5;
		((Control)timeSpanPicker).Location = new Point(0, 0);
		((Control)timeSpanPicker).Name = "timeSpanPicker";
		((Control)timeSpanPicker).Size = new Size(107, 21);
		((Control)timeSpanPicker).TabIndex = 10;
		timeSpanPicker.Value = TimeSpan.Parse("00:00:00");
		((Control)dropDownButton).Dock = (DockStyle)4;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(107, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(20, 21);
		((Control)dropDownButton).TabIndex = 6;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)timeSpanPicker);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueTimeSpanItemControl";
		((Control)this).Size = new Size(175, 21);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
