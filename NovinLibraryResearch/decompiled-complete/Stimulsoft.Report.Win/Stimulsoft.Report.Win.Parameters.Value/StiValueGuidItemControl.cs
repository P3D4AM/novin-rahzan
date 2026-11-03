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
public class StiValueGuidItemControl : UserControl, IStiValueItemControl
{
	private StiDropDownBox comboBox;

	private StiTextBox textBox;

	private IContainer components;

	private Button buttonNewGuid;

	internal CheckBox checkBoxNotAssigned;

	private Panel panel1;

	public object Value
	{
		get
		{
			if (checkBoxNotAssigned.Checked)
			{
				return null;
			}
			string text = ((comboBox == null) ? ((Control)textBox).Text : (Variable.DialogInfo.AllowUserValues ? ((Control)comboBox).Text : ((!(comboBox.SelectedItem is StiDialogInfoValue stiDialogInfoValue)) ? ((Control)comboBox).Text : stiDialogInfoValue.Key.ToString())));
			return StiValueHelper.TryToGuid((object)text);
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
		HashSet<Guid> hashSet = new HashSet<Guid>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		comboBox.BeginUpdate();
		comboBox.Items.Clear();
		foreach (StiGuidDialogInfoItem item in dialogInfoItems)
		{
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Contains(item.Key))
				{
					hashSet.Add(item.Key);
					comboBox.Items.Add((object)new StiDialogInfoValue(item.Key, item.Value, item2));
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
		if (buttonNewGuid != null)
		{
			((Control)buttonNewGuid).Enabled = !checkBoxNotAssigned.Checked;
		}
	}

	private void ButtonNewGuid_Click(object sender, EventArgs e)
	{
		if (textBox != null)
		{
			((Control)textBox).Text = Guid.NewGuid().ToString();
			return;
		}
		comboBox.SelectedValue = null;
		((Control)comboBox).Text = Guid.NewGuid().ToString();
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
		((Control)comboBox).TabIndex = 0;
		((Control)comboBox).Dock = (DockStyle)5;
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
		((StiEditBase)textBox).MaxLength = 1;
		((Control)textBox).TabIndex = 0;
		((Control)textBox).Dock = (DockStyle)5;
		((Control)this).Controls.Add((Control)(object)textBox);
		((Control)textBox).BringToFront();
		((Control)this).Height = ((Control)textBox).Height;
	}

	public StiValueGuidItemControl(StiVariable variable, StiReport report, Hashtable variablesCurrentValues)
	{
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		InitializeComponent();
		Variable = variable;
		Report = report;
		bool allowUserValues = variable.DialogInfo.AllowUserValues;
		((Control)checkBoxNotAssigned).Visible = allowUserValues && variable.Type == typeof(Guid?);
		string text = variable.Value;
		if ((string.IsNullOrEmpty(variable.Value) || variable.Value == "null") && ((Control)checkBoxNotAssigned).Visible)
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
		List<StiDialogInfoItem> andParseDialogInfoItems = variable.DialogInfo.GetAndParseDialogInfoItems(typeof(Guid), null, null, report);
		if (andParseDialogInfoItems.Count > 0)
		{
			andParseDialogInfoItems = variable.DialogInfo.OrderBy(andParseDialogInfoItems);
			CreateComboBox();
			int num = -1;
			foreach (StiGuidDialogInfoItem item in andParseDialogInfoItems)
			{
				StiDialogInfoValue stiDialogInfoValue = new StiDialogInfoValue(item.Key, item.Value);
				comboBox.Items.Add((object)stiDialogInfoValue);
				if (item.Key.ToString() == text)
				{
					num = comboBox.Items.Count - 1;
				}
			}
			StiParameterHelper.CheckDropDownWidth(comboBox, andParseDialogInfoItems);
			comboBox.DrawMode = (DrawMode)1;
			comboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
			comboBox.SelectedValueChanged += delegate
			{
				InvokeSelectedValue();
			};
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
			((StiEditBase)textBox).ReadOnly = !allowUserValues;
			((Control)textBox).Text = text;
		}
		Button obj = buttonNewGuid;
		bool visible = (((Control)panel1).Visible = allowUserValues);
		((Control)obj).Visible = visible;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		buttonNewGuid = new Button();
		checkBoxNotAssigned = new CheckBox();
		panel1 = new Panel();
		((Control)this).SuspendLayout();
		((Control)buttonNewGuid).Dock = (DockStyle)4;
		((Control)buttonNewGuid).Location = new Point(103, 0);
		((Control)buttonNewGuid).Name = "buttonNewGuid";
		((Control)buttonNewGuid).Size = new Size(25, 21);
		((Control)buttonNewGuid).TabIndex = 2;
		((Control)buttonNewGuid).Text = "...";
		((ButtonBase)buttonNewGuid).UseVisualStyleBackColor = true;
		((Control)buttonNewGuid).Click += ButtonNewGuid_Click;
		((Control)checkBoxNotAssigned).AutoSize = true;
		((Control)checkBoxNotAssigned).Dock = (DockStyle)4;
		((Control)checkBoxNotAssigned).Location = new Point(128, 0);
		((Control)checkBoxNotAssigned).Name = "checkBoxNotAssigned";
		((Control)checkBoxNotAssigned).Padding = new Padding(4, 0, 0, 0);
		((Control)checkBoxNotAssigned).Size = new Size(47, 21);
		((Control)checkBoxNotAssigned).TabIndex = 4;
		((Control)checkBoxNotAssigned).Text = "Null";
		((ButtonBase)checkBoxNotAssigned).UseCompatibleTextRendering = true;
		((ButtonBase)checkBoxNotAssigned).UseVisualStyleBackColor = true;
		checkBoxNotAssigned.CheckedChanged += CheckBoxNotAssigned_CheckedChanged;
		((Control)panel1).Dock = (DockStyle)4;
		((Control)panel1).Location = new Point(99, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(4, 21);
		((Control)panel1).TabIndex = 5;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)buttonNewGuid);
		((Control)this).Controls.Add((Control)(object)checkBoxNotAssigned);
		((Control)this).Name = "StiValueGuidItemControl";
		((Control)this).Size = new Size(175, 21);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
