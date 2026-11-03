using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Win.Parameters.Value;

[ToolboxItem(false)]
public class StiValueLongItemControl : UserControl, IStiValueItemControl
{
	private Type variableType;

	private StiTextBox textBox;

	private StiDropDownBox comboBox;

	private StiReport report;

	private bool firstCreation;

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
			if (variableType == typeof(short) || variableType == typeof(short?))
			{
				return (short)StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(int) || variableType == typeof(int?))
			{
				return (int)StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(long) || variableType == typeof(long?))
			{
				return StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(sbyte) || variableType == typeof(sbyte?))
			{
				return (sbyte)StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(ushort) || variableType == typeof(ushort?))
			{
				return (ushort)StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(uint) || variableType == typeof(uint?))
			{
				return (uint)StiValueHelper.TryToLong((object)text);
			}
			if (variableType == typeof(ulong) || variableType == typeof(ulong?))
			{
				return (ulong)StiValueHelper.TryToLong((object)text);
			}
			return (byte)StiValueHelper.TryToLong((object)text);
		}
	}

	public StiVariable Variable { get; }

	public event EventHandler SelectedValue;

	public void InvokeSelectedValue()
	{
		bool flag = StiVariableHelper.ApplyVariableValue(report, Variable, Value);
		if (comboBox != null)
		{
			StiDialogInfoValue stiDialogInfoValue = comboBox.SelectedItem as StiDialogInfoValue;
			if (stiDialogInfoValue != null && this.SelectedValue != null)
			{
				this.SelectedValue(flag ? null : stiDialogInfoValue.Key, EventArgs.Empty);
			}
			StiVariableHelper.SetVariableLabel(report, Variable, stiDialogInfoValue?.Value);
		}
	}

	public void FilterValues(object selectedObject)
	{
		StiVariableHelper.RefreshDialogInfo(report, Variable);
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(Variable.Type);
		HashSet<long> hashSet = new HashSet<long>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		if (comboBox == null)
		{
			CreateComboBox();
			if (textBox != null)
			{
				((Control)this).Controls.Remove((Control)(object)textBox);
				((Component)(object)textBox).Dispose();
				textBox = null;
			}
		}
		comboBox.BeginUpdate();
		comboBox.Items.Clear();
		string defaultValue = GetDefaultValue(null);
		int num = -1;
		foreach (StiLongDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					comboBox.Items.Add((object)new StiDialogInfoValue(item.Key, item.Value, item2));
					if (item.Key.ToString() == defaultValue)
					{
						num = comboBox.Items.Count - 1;
					}
				}
			}
		}
		if (firstCreation)
		{
			firstCreation = false;
		}
		else
		{
			num = -1;
		}
		if (num == -1 && comboBox.Items.Count > 0)
		{
			num = 0;
		}
		comboBox.SelectedIndex = num;
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
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
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
			val2.FormatFlags = (StringFormatFlags)4096;
			val2.Trimming = (StringTrimming)3;
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

	private void CreateTextBox()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		textBox = new StiTextBox();
		((Control)textBox).TabIndex = 0;
		((Control)textBox).Dock = (DockStyle)5;
		((Control)this).Controls.Add((Control)(object)textBox);
		((Control)textBox).BringToFront();
		((Control)this).Height = ((Control)textBox).Height;
	}

	private void CreateComboBox()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		comboBox = new StiDropDownBox();
		comboBox.MaxDropDownItems = 25;
		comboBox.FormattingEnabled = true;
		((Control)comboBox).TabIndex = 0;
		((Control)comboBox).Dock = (DockStyle)5;
		if (StiOptions.Viewer.RequestFromUserDropDownWidth.HasValue)
		{
			comboBox.DropDownWidth = StiOptions.Viewer.RequestFromUserDropDownWidth.GetValueOrDefault();
		}
		((Control)this).Controls.Add((Control)(object)comboBox);
		((Control)comboBox).BringToFront();
		firstCreation = true;
		comboBox.DrawMode = (DrawMode)1;
		comboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
		comboBox.SelectedValueChanged += delegate
		{
			InvokeSelectedValue();
		};
		if (!Variable.DialogInfo.AllowUserValues)
		{
			comboBox.DropDownStyle = (ComboBoxStyle)2;
		}
		((Control)this).Height = ((Control)comboBox).Height;
	}

	private string GetDefaultValue(Hashtable variablesCurrentValues)
	{
		string text = Variable.Value;
		if (Variable.ValueObject == null && ((Control)checkBoxNotAssigned).Visible)
		{
			checkBoxNotAssigned.Checked = true;
		}
		if (Variable.InitBy == StiVariableInitBy.Expression)
		{
			text = StiParameterHelper.ParseVariableExpression(Variable, report, text);
		}
		if (variablesCurrentValues != null && variablesCurrentValues.ContainsKey(Variable.Name))
		{
			text = variablesCurrentValues[Variable.Name].ToString();
		}
		return text;
	}

	public StiValueLongItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		InitializeComponent();
		variableType = variable.Type;
		Variable = variable;
		this.report = report;
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		string defaultValue = GetDefaultValue(variablesCurrentValues);
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(long), null, null, report);
		if (andParseDialogInfoItems.Count > 0)
		{
			andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
			CreateComboBox();
			int num = -1;
			foreach (StiLongDialogInfoItem item in andParseDialogInfoItems)
			{
				StiDialogInfoValue stiDialogInfoValue = new StiDialogInfoValue(item.Key, item.Value);
				comboBox.Items.Add((object)stiDialogInfoValue);
				if (item.Key.ToString() == defaultValue)
				{
					num = comboBox.Items.Count - 1;
				}
			}
			StiParameterHelper.CheckDropDownWidth(comboBox, andParseDialogInfoItems);
			switch (variable.Selection)
			{
			case StiSelectionMode.First:
				comboBox.SelectedIndex = 0;
				break;
			case StiSelectionMode.FromVariable:
				if (num != -1)
				{
					comboBox.SelectedIndex = num;
				}
				else if (allowUserValues)
				{
					comboBox.SelectedText = defaultValue;
				}
				break;
			}
		}
		else
		{
			CreateTextBox();
			((StiEditBase)textBox).ReadOnly = !allowUserValues;
			((Control)textBox).Text = defaultValue;
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
		((Control)checkBoxNotAssigned).Visible = allowUserValues && (variable.Type == typeof(short?) || variable.Type == typeof(int?) || variable.Type == typeof(long?) || variable.Type == typeof(sbyte?) || variable.Type == typeof(ushort?) || variable.Type == typeof(uint?) || variable.Type == typeof(ulong?) || variable.Type == typeof(byte?));
		((Control)checkBoxNotAssigned).Text = Loc.Get("Report", "Null");
		checkBoxNotAssigned.Checked = Variable.Value == null || Variable.Value == "null";
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
		((Control)checkBoxNotAssigned).Size = new Size(48, 21);
		((Control)checkBoxNotAssigned).TabIndex = 3;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		((Control)checkBoxNotAssigned).Visible = false;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueLongItemControl";
		((Control)this).Size = new Size(175, 21);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
