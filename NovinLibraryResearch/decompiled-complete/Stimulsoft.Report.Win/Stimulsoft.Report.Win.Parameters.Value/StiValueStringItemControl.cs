using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueStringItemControl : UserControl, IStiValueItemControl
{
	private StiControlBase textBox;

	private IContainer components;

	private StiParameterDropDownButtonV2 dropDownButton;

	public object Value
	{
		get
		{
			if (dropDownButton.ListBox != null)
			{
				if (((ListBox)dropDownButton.ListBox).SelectedItem is StiDialogInfoValue stiDialogInfoValue && Variable.DialogInfo.AllowUserValues)
				{
					return stiDialogInfoValue.Key;
				}
				return ((Control)textBox).Text;
			}
			if (dropDownButton.ListValues != null && dropDownButton.ListValues.Count > 0 && Variable.Selection == StiSelectionMode.First && (!Variable.DialogInfo.AllowUserValues || dropDownButton.ListValues[0].Value == ((Control)textBox).Text))
			{
				return dropDownButton.ListValues[0].Key;
			}
			return ((Control)textBox).Text;
		}
	}

	public StiVariable Variable { get; }

	public StiReport Report { get; }

	public event EventHandler SelectedValue;

	public void FilterValues(object selectedObject)
	{
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(typeof(string));
		HashSet<string> hashSet = new HashSet<string>();
		List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		foreach (StiStringDialogInfoItem item in dialogInfoItems)
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

	public void InvokeSelectedValue()
	{
		if (dropDownButton.ListBox != null)
		{
			StiDialogInfoValue stiDialogInfoValue = ((ListBox)dropDownButton.ListBox).SelectedItem as StiDialogInfoValue;
			((Control)textBox).Text = stiDialogInfoValue.GetString();
			if (stiDialogInfoValue != null && this.SelectedValue != null)
			{
				this.SelectedValue(stiDialogInfoValue.Key, EventArgs.Empty);
			}
			StiVariableHelper.SetVariableLabel(Report, Variable, stiDialogInfoValue?.Value);
		}
	}

	private void CreateTextBox(StiVariable variable, bool allowUserValues)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		if (string.IsNullOrEmpty(variable.DialogInfo.Mask))
		{
			textBox = (StiControlBase)new StiTextBox();
			((StiEditBase)(StiTextBox)textBox).ReadOnly = !allowUserValues;
		}
		else
		{
			textBox = (StiControlBase)new StiMaskedTextBox();
			((StiMaskedTextBox)textBox).Mask = variable.DialogInfo.Mask;
			((StiMaskedEditBase)(StiMaskedTextBox)textBox).ReadOnly = !allowUserValues;
		}
		((Control)textBox).GotFocus += delegate
		{
			((Control)dropDownButton).Invalidate();
		};
		((Control)textBox).LostFocus += delegate
		{
			((Control)dropDownButton).Invalidate();
		};
		((Control)textBox).MouseEnter += delegate
		{
			((Control)dropDownButton).Invalidate();
		};
		((Control)textBox).MouseLeave += delegate
		{
			((Control)dropDownButton).Invalidate();
		};
		((Control)textBox).MouseDown += (MouseEventHandler)delegate
		{
			if (!allowUserValues)
			{
				dropDownButton.InvokeMouseDown();
			}
		};
		((Control)textBox).Dock = (DockStyle)5;
		((Control)textBox).TabIndex = 0;
		((Control)this).Controls.Add((Control)(object)textBox);
		((Control)this).Height = ((Control)textBox).Height;
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		InvokeSelectedValue();
	}

	private void TextBox_MouseLeave(object sender, EventArgs e)
	{
		((Control)dropDownButton).Invalidate();
	}

	private void TextBox_MouseEnter(object sender, EventArgs e)
	{
		((Control)dropDownButton).Invalidate();
	}

	public StiValueStringItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		InitializeComponent();
		Variable = variable;
		Report = report;
		((Control)dropDownButton).Width = dropDownButton.DropDownButtonWidth;
		((Control)dropDownButton).Dock = (DockStyle)4;
		string text = (string.IsNullOrEmpty(variable.Value) ? string.Empty : variable.Value);
		if (variable.InitBy == StiVariableInitBy.Expression)
		{
			text = StiParameterHelper.ParseVariableExpression(variable, report, text);
		}
		if (variablesCurrentValues.ContainsKey(variable.Name))
		{
			text = variablesCurrentValues[variable.Name].ToString();
		}
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		CreateTextBox(variable, allowUserValues);
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(string), null, null, report);
		if (andParseDialogInfoItems.Count > 0)
		{
			andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
			dropDownButton.ShowSearching = andParseDialogInfoItems.Count > 10;
			dropDownButton.ItemChanged += delegate
			{
				InvokeSelectedValue();
			};
			int num = -1;
			List<StiDialogInfoValue> list = new List<StiDialogInfoValue>();
			foreach (StiDialogInfoItem item2 in andParseDialogInfoItems)
			{
				StiStringDialogInfoItem stiStringDialogInfoItem = item2 as StiStringDialogInfoItem;
				if (item2 is StiExpressionDialogInfoItem stiExpressionDialogInfoItem)
				{
					stiStringDialogInfoItem = new StiStringDialogInfoItem
					{
						Checked = stiExpressionDialogInfoItem.Checked,
						Label = stiExpressionDialogInfoItem.Label,
						KeyObject = StiReportParser.Parse(stiExpressionDialogInfoItem.KeyObject as string, (IStiReport)(object)report, allowCache: false)
					};
				}
				if (item2 is StiExpressionRangeDialogInfoItem stiExpressionRangeDialogInfoItem)
				{
					stiStringDialogInfoItem = new StiStringDialogInfoItem
					{
						Checked = stiExpressionRangeDialogInfoItem.Checked,
						Label = stiExpressionRangeDialogInfoItem.Label,
						KeyObject = StiReportParser.Parse(stiExpressionRangeDialogInfoItem.KeyObject as string, (IStiReport)(object)report, allowCache: false),
						KeyObjectTo = StiReportParser.Parse(stiExpressionRangeDialogInfoItem.KeyObjectTo as string, (IStiReport)(object)report, allowCache: false)
					};
				}
				if (stiStringDialogInfoItem != null)
				{
					StiDialogInfoValue item = new StiDialogInfoValue(stiStringDialogInfoItem.Key, item2.Value);
					list.Add(item);
					if (stiStringDialogInfoItem.Key == text)
					{
						num = list.Count - 1;
					}
				}
			}
			dropDownButton.ListValues = list;
			switch (variable.Selection)
			{
			case StiSelectionMode.First:
				if (list.Count > 0)
				{
					((Control)textBox).Text = list[0].GetString();
				}
				break;
			case StiSelectionMode.FromVariable:
				((Control)textBox).Text = text;
				break;
			}
		}
		else
		{
			((Control)dropDownButton).Visible = false;
			((Control)textBox).Text = text;
		}
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, 0);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		if (textBox != null)
		{
			((Control)textBox).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		}
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		dropDownButton = new StiParameterDropDownButtonV2();
		((Control)this).SuspendLayout();
		((Control)dropDownButton).Dock = (DockStyle)4;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).DropDownMenu = null;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(156, 0);
		((Control)dropDownButton).Margin = new Padding(0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(19, 21);
		((Control)dropDownButton).TabIndex = 3;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Name = "StiValueStringItemControl";
		((Control)this).Size = new Size(175, 21);
		((Control)this).ResumeLayout(false);
	}
}
