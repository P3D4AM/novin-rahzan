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
public class StiValueDateTimeItemControl : UserControl, IStiValueItemControl
{
	private CultureInfo culture;

	private StiDialogInfoValue lastInfoValue;

	private bool isNotAssigned;

	private IContainer components;

	private DateTimePicker dateTimePicker;

	private StiParameterDropDownButtonV2 dropDownButton;

	private StiTextBox textBoxDate;

	internal CheckBox checkBoxNotAssigned;

	private Panel panel1;

	private Panel panel2;

	public object Value
	{
		get
		{
			if (((Control)dateTimePicker).Visible && isNotAssigned)
			{
				if ((((Control)checkBoxNotAssigned).Visible && checkBoxNotAssigned.Checked) || isNotAssigned)
				{
					return null;
				}
				if (Variable.DialogInfo.DateTimeType == StiDateTimeType.Date)
				{
					return new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day);
				}
				return dateTimePicker.Value;
			}
			if (lastInfoValue != null)
			{
				return lastInfoValue.Key;
			}
			if (Variable.DialogInfo.DateTimeType == StiDateTimeType.Date)
			{
				return dateTimePicker.Value.Date;
			}
			return dateTimePicker.Value;
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
		if (((Control)dateTimePicker).Visible)
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
				dateTimePicker.Value = (DateTime)e.NewValue;
				InvokeSelectedValue(e.NewValue);
			}
		}
		else
		{
			lastInfoValue = (StiDialogInfoValue)e.NewValue;
			if (e.NewValue == null)
			{
				((Control)textBoxDate).Text = string.Empty;
			}
			else
			{
				((Control)textBoxDate).Text = lastInfoValue.Value;
				InvokeSelectedValue(lastInfoValue.Value);
			}
		}
		StiVariableHelper.SetVariableLabel(Report, Variable, e.Label);
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		((Control)dateTimePicker).Enabled = !checkBoxNotAssigned.Checked;
	}

	private void DateTimePicker_ValueChanged(object sender, EventArgs e)
	{
		SetCustomFormat(Variable);
	}

	private void SetCustomFormat(StiVariable variable)
	{
		StiDateTimeFormatHelper.SetDateTimePickerFormat(dateTimePicker, variable.DialogInfo.DateTimeType, culture);
		isNotAssigned = false;
	}

	public StiValueDateTimeItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
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
		((StiEditBase)textBoxDate).ReadOnly = true;
		((Control)textBoxDate).Visible = !allowUserValues;
		((Control)dateTimePicker).Visible = allowUserValues;
		((Control)this).Height = ((Control)textBoxDate).Height;
		if (((Control)textBoxDate).Visible)
		{
			((Control)textBoxDate).Dock = (DockStyle)5;
			((Control)textBoxDate).BringToFront();
		}
		if (((Control)dateTimePicker).Visible)
		{
			((Control)dateTimePicker).Dock = (DockStyle)0;
			((Control)dateTimePicker).Left = 0;
			((Control)dateTimePicker).Top = (((Control)this).Height - ((Control)dateTimePicker).Height) / 2;
			((Control)dateTimePicker).Width = ((Control)panel2).Width;
			((Control)dateTimePicker).Anchor = (AnchorStyles)15;
		}
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
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(DateTime), culture, null, report);
		andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		for (int i = 0; i < andParseDialogInfoItems.Count; i++)
		{
			StiDialogInfoValue stiDialogInfoValue = new StiDialogInfoValue
			{
				IsDateTime = true,
				Value = andParseDialogInfoItems[i].Value
			};
			stiDialogInfoValue.Key = ((StiDateTimeDialogInfoItem)andParseDialogInfoItems[i]).Key;
			stiDialogInfoValue.Culture = culture;
			list.Add(stiDialogInfoValue);
		}
		dropDownButton.ListValues = list;
		SetCustomFormat(variable);
		if (list.Count == 0)
		{
			StiParameterDropDownButtonV2 stiParameterDropDownButtonV = dropDownButton;
			bool visible = (((Control)panel1).Visible = false);
			((Control)stiParameterDropDownButtonV).Visible = visible;
		}
		((Control)checkBoxNotAssigned).Visible = allowUserValues && variable.Type == typeof(DateTime?);
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)dateTimePicker).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)dateTimePicker).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)textBoxDate).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		switch (variable.Selection)
		{
		case StiSelectionMode.First:
			if (list.Count > 0)
			{
				if (list[0].Key is DateTime value)
				{
					dateTimePicker.Value = value;
					string dateTimeFormat2 = StiDateTimeFormatHelper.GetDateTimeFormat(variable.DialogInfo.DateTimeType);
					((Control)textBoxDate).Text = value.ToString(dateTimeFormat2);
				}
			}
			else
			{
				((Control)dateTimePicker).Enabled = false;
			}
			break;
		case StiSelectionMode.FromVariable:
			if (variable.ValueObject == null)
			{
				dateTimePicker.Format = (DateTimePickerFormat)8;
				dateTimePicker.CustomFormat = " ";
				((Control)textBoxDate).Text = "";
				isNotAssigned = true;
			}
			else if (variable.ValueObject is DateTime)
			{
				dateTimePicker.Value = (DateTime)variable.ValueObject;
				string dateTimeFormat = StiDateTimeFormatHelper.GetDateTimeFormat(variable.DialogInfo.DateTimeType);
				((Control)textBoxDate).Text = ((DateTime)variable.ValueObject).ToString(dateTimeFormat);
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
					DateTime result = DateTime.Now;
					if (obj2 != null && DateTime.TryParse(obj2.ToString(), out result))
					{
						dateTimePicker.Value = result;
					}
				}
				catch
				{
					if (report[variable.Name] != null)
					{
						dateTimePicker.Value = (DateTime)report[variable.Name];
					}
				}
			}
			if (((Control)checkBoxNotAssigned).Visible)
			{
				((Control)dateTimePicker).Enabled = variable.ValueObject != null;
				checkBoxNotAssigned.Checked = variable.ValueObject == null || variable.Value == "null";
			}
			if (variablesCurrentValues.ContainsKey(variable.Name))
			{
				if (variablesCurrentValues[variable.Name] == null)
				{
					((Control)dateTimePicker).Enabled = false;
					checkBoxNotAssigned.Checked = true;
				}
				else
				{
					((Control)dateTimePicker).Enabled = true;
					checkBoxNotAssigned.Checked = false;
					dateTimePicker.Value = Convert.ToDateTime(variablesCurrentValues[variable.Name]);
				}
			}
			break;
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		dateTimePicker = new DateTimePicker();
		textBoxDate = new StiTextBox();
		checkBoxNotAssigned = new CheckBox();
		panel1 = new Panel();
		dropDownButton = new StiParameterDropDownButtonV2();
		panel2 = new Panel();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)dateTimePicker).Dock = (DockStyle)3;
		dateTimePicker.Format = (DateTimePickerFormat)8;
		((Control)dateTimePicker).Location = new Point(32, 0);
		((Control)dateTimePicker).Name = "dateTimePicker";
		((Control)dateTimePicker).Size = new Size(30, 20);
		((Control)dateTimePicker).TabIndex = 1;
		dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
		((Control)textBoxDate).BackColor = Color.White;
		((Control)textBoxDate).Dock = (DockStyle)3;
		((Control)textBoxDate).Location = new Point(0, 0);
		((Control)textBoxDate).Margin = new Padding(2);
		((Control)textBoxDate).Name = "textBoxDate";
		((Control)textBoxDate).Size = new Size(32, 21);
		((Control)textBoxDate).TabIndex = 3;
		((Control)checkBoxNotAssigned).AutoSize = true;
		((Control)checkBoxNotAssigned).Dock = (DockStyle)4;
		((Control)checkBoxNotAssigned).Location = new Point(127, 0);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Padding = new Padding(4, 0, 0, 0);
		((Control)checkBoxNotAssigned).Size = new Size(48, 21);
		((Control)checkBoxNotAssigned).TabIndex = 4;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		((Control)checkBoxNotAssigned).Visible = false;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)panel1).Dock = (DockStyle)4;
		((Control)panel1).Location = new Point(102, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(4, 21);
		((Control)panel1).TabIndex = 6;
		((Control)dropDownButton).Dock = (DockStyle)4;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).DropDownMenu = null;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(106, 0);
		((Control)dropDownButton).Margin = new Padding(0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(21, 21);
		((Control)dropDownButton).TabIndex = 2;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((Control)panel2).Controls.Add((Control)(object)dateTimePicker);
		((Control)panel2).Controls.Add((Control)(object)textBoxDate);
		((Control)panel2).Dock = (DockStyle)5;
		((Control)panel2).Location = new Point(0, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(102, 21);
		((Control)panel2).TabIndex = 7;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueDateTimeItemControl";
		((Control)this).Size = new Size(175, 21);
		((Control)panel2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
