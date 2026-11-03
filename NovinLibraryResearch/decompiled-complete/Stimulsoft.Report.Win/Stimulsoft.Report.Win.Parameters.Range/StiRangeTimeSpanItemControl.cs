using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Win.Parameters.Controls;

namespace Stimulsoft.Report.Win.Parameters.Range;

[ToolboxItem(false)]
public class StiRangeTimeSpanItemControl : UserControl, IStiRangeItemControl
{
	private StiReport report;

	private StiVariable variable;

	private IContainer components;

	private Label labelTo;

	private Label labelFrom;

	private StiParameterDropDownButtonV2 dropDownButton;

	private StiTimeSpanPicker timeSpanPickerFrom;

	private StiTimeSpanPicker timeSpanPickerTo;

	public object Value => new TimeSpanRange(timeSpanPickerFrom.Value, timeSpanPickerTo.Value);

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue is StiDialogInfoRange stiDialogInfoRange)
		{
			timeSpanPickerFrom.Value = (TimeSpan)stiDialogInfoRange.From;
			timeSpanPickerTo.Value = (TimeSpan)stiDialogInfoRange.To;
			StiVariableHelper.SetVariableLabel(report, variable, e.Label);
		}
	}

	public StiRangeTimeSpanItemControl(StiVariable variable, StiReport report)
	{
		this.report = report;
		this.variable = variable;
		InitializeComponent();
		((Control)labelFrom).Text = StiLocalization.Get("PropertyMain", "RangeFrom");
		((Control)labelTo).Text = StiLocalization.Get("PropertyMain", "RangeTo");
		((Control)labelFrom).Width = TextRenderer.MeasureText(((Control)labelFrom).Text, ((Control)labelFrom).Font).Width + StiScale2.I2;
		((Control)labelTo).Width = TextRenderer.MeasureText(((Control)labelTo).Text, ((Control)labelTo).Font).Width + StiScale2.I2;
		dropDownButton.ShowFullBorder = true;
		List<StiDialogInfoRange> list = new List<StiDialogInfoRange>();
		List<StiDialogInfoItem> dialogInfoItems = variable.DialogInfo.GetDialogInfoItems(typeof(TimeSpanRange));
		foreach (StiDialogInfoItem item2 in dialogInfoItems)
		{
			StiTimeSpanRangeDialogInfoItem stiTimeSpanRangeDialogInfoItem = (StiTimeSpanRangeDialogInfoItem)item2;
			StiDialogInfoRange item = new StiDialogInfoRange(stiTimeSpanRangeDialogInfoItem.From, stiTimeSpanRangeDialogInfoItem.To, stiTimeSpanRangeDialogInfoItem.Value);
			list.Add(item);
		}
		dropDownButton.ListRanges = list;
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		((Control)timeSpanPickerFrom).Enabled = variable.DialogInfo.AllowUserValues;
		((Control)timeSpanPickerTo).Enabled = variable.DialogInfo.AllowUserValues;
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)timeSpanPickerTo).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)timeSpanPickerFrom).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)timeSpanPickerTo).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)this).Width = ((Control)labelFrom).Width + ((Control)labelTo).Width + ((Control)timeSpanPickerFrom).Width + ((Control)timeSpanPickerTo).Width;
		((Control)this).Height = ((Control)timeSpanPickerFrom).Height;
		if (((Control)dropDownButton).Visible)
		{
			((Control)this).Width = ((Control)this).Width + (((Control)dropDownButton).Width + StiScale2.I4);
		}
		if (variable.ValueObject is Stimulsoft.Report.Range { FromObject: var fromObject } range)
		{
			if (fromObject is TimeSpan value)
			{
				timeSpanPickerFrom.Value = value;
			}
			if (range.ToObject is TimeSpan value2)
			{
				timeSpanPickerTo.Value = value2;
			}
			return;
		}
		string text = variable.ValueObject as string;
		if (!string.IsNullOrEmpty(text) && text.Contains("<<|>>") && variable.InitBy == StiVariableInitBy.Expression && report[variable.Name] is DateTimeRange { FromObject: var fromObject2 } dateTimeRange)
		{
			if (fromObject2 is TimeSpan value3)
			{
				timeSpanPickerFrom.Value = value3;
			}
			if (dateTimeRange.ToObject is TimeSpan value4)
			{
				timeSpanPickerTo.Value = value4;
			}
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		labelTo = new Label();
		labelFrom = new Label();
		dropDownButton = new StiParameterDropDownButtonV2();
		timeSpanPickerFrom = new StiTimeSpanPicker();
		timeSpanPickerTo = new StiTimeSpanPicker();
		((Control)this).SuspendLayout();
		((Control)labelTo).Dock = (DockStyle)3;
		((Control)labelTo).Location = new Point(170, 0);
		((Control)labelTo).Name = "labelTo";
		((Control)labelTo).Size = new Size(20, 22);
		((Control)labelTo).TabIndex = 16;
		((Control)labelTo).Text = "To";
		labelTo.TextAlign = (ContentAlignment)16;
		((Control)labelFrom).Dock = (DockStyle)3;
		((Control)labelFrom).Location = new Point(0, 0);
		((Control)labelFrom).Name = "labelFrom";
		((Control)labelFrom).Size = new Size(30, 22);
		((Control)labelFrom).TabIndex = 15;
		((Control)labelFrom).Text = "From";
		labelFrom.TextAlign = (ContentAlignment)16;
		((Control)dropDownButton).Dock = (DockStyle)4;
		((StiToolButton)dropDownButton).Draw3DButton = false;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(340, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(19, 22);
		((Control)dropDownButton).TabIndex = 19;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((Control)timeSpanPickerFrom).Dock = (DockStyle)3;
		((Control)timeSpanPickerFrom).Location = new Point(30, 0);
		((Control)timeSpanPickerFrom).Name = "timeSpanPickerFrom";
		((Control)timeSpanPickerFrom).Padding = new Padding(2);
		timeSpanPickerFrom.ReadOnly = false;
		((Control)timeSpanPickerFrom).Size = new Size(140, 22);
		((Control)timeSpanPickerFrom).TabIndex = 22;
		timeSpanPickerFrom.Value = TimeSpan.Parse("00:00:00");
		((Control)timeSpanPickerTo).Dock = (DockStyle)3;
		((Control)timeSpanPickerTo).Location = new Point(190, 0);
		((Control)timeSpanPickerTo).Name = "timeSpanPickerTo";
		((Control)timeSpanPickerTo).Padding = new Padding(2);
		timeSpanPickerTo.ReadOnly = false;
		((Control)timeSpanPickerTo).Size = new Size(140, 22);
		((Control)timeSpanPickerTo).TabIndex = 23;
		timeSpanPickerTo.Value = TimeSpan.Parse("00:00:00");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)timeSpanPickerTo);
		((Control)this).Controls.Add((Control)(object)labelTo);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Controls.Add((Control)(object)timeSpanPickerFrom);
		((Control)this).Controls.Add((Control)(object)labelFrom);
		((Control)this).Name = "StiRangeTimeSpanItemControl";
		((Control)this).Size = new Size(359, 22);
		((Control)this).ResumeLayout(false);
	}
}
