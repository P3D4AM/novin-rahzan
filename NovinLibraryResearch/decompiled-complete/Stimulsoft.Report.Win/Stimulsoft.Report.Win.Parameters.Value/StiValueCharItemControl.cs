using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueCharItemControl : UserControl, IStiValueItemControl
{
	private StiDropDownBox comboBox;

	private StiTextBox textBox;

	private IContainer components;

	internal CheckBox checkBoxNotAssigned;

	public object Value
	{
		get
		{
			if (checkBoxNotAssigned.Checked)
			{
				return null;
			}
			string text = ((comboBox == null) ? ((Control)textBox).Text : (Variable.DialogInfo.AllowUserValues ? ((Control)comboBox).Text : ((!(comboBox.SelectedItem is StiDialogInfoValue stiDialogInfoValue)) ? ((Control)comboBox).Text : stiDialogInfoValue.Key.ToString())));
			return (text.Length > 0) ? text.ToCharArray()[0] : ' ';
		}
	}

	public StiVariable Variable { get; }

	public StiReport Report { get; }

	public event EventHandler SelectedValue;

	public void InvokeSelectedValue()
	{
		StiDialogInfoValue stiDialogInfoValue = comboBox.SelectedItem as StiDialogInfoValue;
		if (stiDialogInfoValue != null && this.SelectedValue != null)
		{
			this.SelectedValue(stiDialogInfoValue.Key, EventArgs.Empty);
		}
		StiVariableHelper.SetVariableLabel(Report, Variable, stiDialogInfoValue?.Value);
	}

	public void FilterValues(object selectedObject)
	{
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(Variable.Type);
		HashSet<char> hashSet = new HashSet<char>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		comboBox.BeginUpdate();
		comboBox.Items.Clear();
		foreach (StiCharDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					comboBox.Items.Add((object)new StiDialogInfoValue(item.Key, item.Value));
				}
			}
		}
		comboBox.SelectedIndex = ((comboBox.Items.Count <= 0) ? (-1) : 0);
		comboBox.EndUpdate();
	}

	private void CheckBoxNotAssigned_CheckedChanged(object sender, EventArgs e)
	{
		if (textBox != null)
		{
			((Control)textBox).Enabled = !checkBoxNotAssigned.Checked;
		}
		else if (comboBox != null)
		{
			((Control)comboBox).Enabled = !checkBoxNotAssigned.Checked;
		}
	}

	private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		if (e.Index == -1)
		{
			return;
		}
		DrawItemState val = e.State;
		if (comboBox.HotTrackingItemIndex == e.Index)
		{
			val = (DrawItemState)(val | 1);
		}
		Graphics graphics = e.Graphics;
		string @string = ((StiDialogInfoValue)comboBox.Items[e.Index]).GetString();
		graphics.FillRectangle(((val & 1) > 0) ? SystemBrushes.HotTrack : Brushes.White, e.Bounds);
		RectangleF rectangleF = new RectangleF(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
		StringFormat val2 = new StringFormat();
		try
		{
			val2.LineAlignment = (StringAlignment)1;
			if ((val & 1) == 1)
			{
				graphics.DrawString(@string, ((Control)comboBox).Font, SystemBrushes.HighlightText, rectangleF, val2);
			}
			else if (e.Index >= 0)
			{
				graphics.DrawString(@string, ((Control)comboBox).Font, SystemBrushes.ControlText, rectangleF, val2);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void CreateComboBox()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		comboBox = new StiDropDownBox();
		comboBox.MaxDropDownItems = 25;
		comboBox.FormattingEnabled = true;
		((Control)comboBox).Dock = (DockStyle)5;
		((Control)comboBox).TabIndex = 0;
		if (StiOptions.Viewer.RequestFromUserDropDownWidth.HasValue)
		{
			comboBox.DropDownWidth = StiOptions.Viewer.RequestFromUserDropDownWidth.GetValueOrDefault();
		}
		((Control)this).Controls.Add((Control)(object)comboBox);
		((Control)comboBox).BringToFront();
		((Control)this).Height = ((Control)comboBox).Height;
	}

	private void CreateTextBox()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		textBox = new StiTextBox();
		((Control)textBox).Dock = (DockStyle)5;
		((StiEditBase)textBox).MaxLength = 1;
		((Control)textBox).TabIndex = 0;
		((Control)this).Controls.Add((Control)(object)textBox);
		((Control)textBox).BringToFront();
		((Control)this).Height = ((Control)textBox).Height;
	}

	public StiValueCharItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		InitializeComponent();
		Variable = variable;
		Report = report;
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		string text = variable.Value.ToString();
		if (variable.ValueObject == null && ((Control)checkBoxNotAssigned).Visible)
		{
			checkBoxNotAssigned.Checked = true;
		}
		if (variable.InitBy == StiVariableInitBy.Expression)
		{
			text = StiParameterHelper.ParseVariableExpression(Variable, report, text);
		}
		if (variablesCurrentValues.ContainsKey(variable.Name))
		{
			text = variablesCurrentValues[variable.Name].ToString();
		}
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(char), null, null, report);
		if (andParseDialogInfoItems.Count > 0)
		{
			andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
			CreateComboBox();
			int num = -1;
			foreach (StiCharDialogInfoItem item in andParseDialogInfoItems)
			{
				StiDialogInfoValue stiDialogInfoValue = new StiDialogInfoValue(item.Key, item.Value, item.ValueBinding);
				comboBox.Items.Add((object)stiDialogInfoValue);
				if (item.Key.ToString() == text)
				{
					num = comboBox.Items.Count - 1;
				}
			}
			comboBox.DrawMode = (DrawMode)1;
			comboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
			comboBox.SelectedValueChanged += delegate
			{
				InvokeSelectedValue();
			};
			switch (variable.Selection)
			{
			case StiSelectionMode.First:
				if (comboBox.Items.Count > 0)
				{
					comboBox.SelectedIndex = 0;
				}
				break;
			case StiSelectionMode.FromVariable:
				if (num != -1)
				{
					comboBox.SelectedIndex = num;
				}
				else if (allowUserValues)
				{
					comboBox.SelectedText = text;
				}
				break;
			}
			if (!allowUserValues)
			{
				comboBox.DropDownStyle = (ComboBoxStyle)2;
			}
		}
		else
		{
			CreateTextBox();
			((Control)textBox).Text = text;
			((StiEditBase)textBox).ReadOnly = !allowUserValues;
		}
		if (comboBox != null)
		{
			StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)comboBox).Width);
			StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
			((Control)comboBox).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		}
		else if (textBox != null)
		{
			StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)textBox).Width);
			StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
			((Control)textBox).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		}
		((Control)checkBoxNotAssigned).Visible = allowUserValues && variable.Type == typeof(char?);
		((Control)checkBoxNotAssigned).Text = Loc.Get("Report", "Null");
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
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		checkBoxNotAssigned = new CheckBox();
		((Control)this).SuspendLayout();
		((Control)checkBoxNotAssigned).AutoSize = true;
		((Control)checkBoxNotAssigned).Dock = (DockStyle)4;
		((Control)checkBoxNotAssigned).Location = new Point(127, 0);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Padding = new Padding(4, 0, 0, 0);
		((Control)checkBoxNotAssigned).Size = new Size(48, 20);
		((Control)checkBoxNotAssigned).TabIndex = 3;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueCharItemControl";
		((Control)this).Size = new Size(175, 20);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
