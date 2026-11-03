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
public class StiRangeDateTimeItemControl : UserControl, IStiRangeItemControl
{
	private bool isDateTime;

	private bool isNotAssignedFrom;

	private bool isNotAssignedTo;

	private StiReport report;

	private StiVariable variable;

	private IContainer components;

	private Label labelTo;

	private Label labelFrom;

	private StiParameterDropDownButtonV2 dropDownButton;

	private DateTimePicker dateTimeFrom;

	private DateTimePicker dateTimeTo;

	public object Value
	{
		get
		{
			if (isDateTime)
			{
				DateTime? from = null;
				DateTime? to = null;
				if (!isNotAssignedFrom)
				{
					from = ((variable.DialogInfo.DateTimeType != 0) ? new DateTime?(dateTimeFrom.Value) : new DateTime?(dateTimeFrom.Value.Date));
				}
				if (!isNotAssignedTo)
				{
					to = ((variable.DialogInfo.DateTimeType != 0) ? new DateTime?(dateTimeTo.Value) : new DateTime?(dateTimeTo.Value.Date.AddDays(1.0).AddTicks(-1L)));
				}
				return new DateTimeRange(from, to);
			}
			TimeSpan? from2 = null;
			TimeSpan? to2 = null;
			if (!isNotAssignedFrom)
			{
				from2 = dateTimeFrom.Value.TimeOfDay;
			}
			if (!isNotAssignedTo)
			{
				to2 = dateTimeTo.Value.TimeOfDay;
			}
			return new TimeSpanRange(from2, to2);
		}
	}

	private DateTime ConvertTimeSpanToDateTime(object obj)
	{
		if (obj is TimeSpan timeSpan)
		{
			return new DateTime(2000, 1, Math.Max(1, timeSpan.Days), Math.Min(23, Math.Max(0, timeSpan.Hours)), Math.Min(59, Math.Max(0, timeSpan.Minutes)), Math.Min(59, Math.Max(0, timeSpan.Seconds)), Math.Min(999, Math.Max(0, timeSpan.Milliseconds)));
		}
		return DateTime.Now;
	}

	private void SetCustomFormatFrom(StiVariable variable)
	{
		if (isDateTime)
		{
			StiDateTimeFormatHelper.SetDateTimePickerFormat(dateTimeFrom, variable.DialogInfo.DateTimeType);
		}
		else
		{
			dateTimeFrom.Format = (DateTimePickerFormat)4;
		}
		isNotAssignedFrom = false;
	}

	private void SetCustomFormatTo(StiVariable variable)
	{
		if (isDateTime)
		{
			StiDateTimeFormatHelper.SetDateTimePickerFormat(dateTimeTo, variable.DialogInfo.DateTimeType);
		}
		else
		{
			dateTimeTo.Format = (DateTimePickerFormat)4;
		}
		isNotAssignedTo = false;
	}

	private void DropDownButton_ItemChanged(object sender, StiParameterInfoValueEventArgs e)
	{
		if (e.NewValue is StiDialogInfoRange stiDialogInfoRange)
		{
			if (isDateTime)
			{
				dateTimeFrom.Value = (DateTime)stiDialogInfoRange.From;
				dateTimeTo.Value = (DateTime)stiDialogInfoRange.To;
			}
			else
			{
				dateTimeFrom.Value = ConvertTimeSpanToDateTime(stiDialogInfoRange.From);
				dateTimeTo.Value = ConvertTimeSpanToDateTime(stiDialogInfoRange.To);
			}
			StiVariableHelper.SetVariableLabel(report, variable, e.Label);
		}
	}

	private void DateTimeFromPicker_ValueChanged(object sender, EventArgs e)
	{
		SetCustomFormatFrom(variable);
	}

	private void DateTimeToPicker_ValueChanged(object sender, EventArgs e)
	{
		SetCustomFormatTo(variable);
	}

	public StiRangeDateTimeItemControl(StiVariable variable, StiReport report)
	{
		this.report = report;
		this.variable = variable;
		isDateTime = variable.Type == typeof(DateTimeRange);
		InitializeComponent();
		((Control)labelFrom).Text = StiLocalization.Get("PropertyMain", "RangeFrom");
		((Control)labelTo).Text = StiLocalization.Get("PropertyMain", "RangeTo");
		((Control)labelFrom).Width = TextRenderer.MeasureText(((Control)labelFrom).Text, ((Control)labelFrom).Font).Width + StiScale2.I2;
		((Control)labelTo).Width = TextRenderer.MeasureText(((Control)labelTo).Text, ((Control)labelTo).Font).Width + StiScale2.I2;
		dropDownButton.ShowFullBorder = true;
		List<StiDialogInfoRange> list = new List<StiDialogInfoRange>();
		List<StiDialogInfoItem> list2 = (isDateTime ? variable.DialogInfo.GetDialogInfoItems(typeof(DateTimeRange)) : variable.DialogInfo.GetDialogInfoItems(typeof(TimeSpanRange)));
		foreach (StiDialogInfoItem item2 in list2)
		{
			StiDialogInfoRange item;
			if (isDateTime)
			{
				StiDateTimeRangeDialogInfoItem stiDateTimeRangeDialogInfoItem = (StiDateTimeRangeDialogInfoItem)item2;
				item = new StiDialogInfoRange(stiDateTimeRangeDialogInfoItem.From, stiDateTimeRangeDialogInfoItem.To, stiDateTimeRangeDialogInfoItem.Value);
			}
			else
			{
				StiTimeSpanRangeDialogInfoItem stiTimeSpanRangeDialogInfoItem = (StiTimeSpanRangeDialogInfoItem)item2;
				item = new StiDialogInfoRange(stiTimeSpanRangeDialogInfoItem.From, stiTimeSpanRangeDialogInfoItem.To, stiTimeSpanRangeDialogInfoItem.Value);
			}
			list.Add(item);
		}
		dropDownButton.ListRanges = list;
		SetCustomFormatFrom(variable);
		SetCustomFormatTo(variable);
		if (list.Count == 0)
		{
			((Control)dropDownButton).Visible = false;
		}
		((Control)dateTimeFrom).Enabled = variable.DialogInfo.AllowUserValues;
		((Control)dateTimeTo).Enabled = variable.DialogInfo.AllowUserValues;
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)dateTimeTo).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)dateTimeFrom).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)dateTimeTo).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)this).Width = ((Control)labelFrom).Width + ((Control)labelTo).Width + ((Control)dateTimeFrom).Width + ((Control)dateTimeTo).Width;
		((Control)this).Height = ((Control)dateTimeFrom).Height;
		if (((Control)dropDownButton).Visible)
		{
			((Control)this).Width = ((Control)this).Width + (((Control)dropDownButton).Width + StiScale2.I4);
		}
		if (variable.ValueObject is Stimulsoft.Report.Range range)
		{
			if (isDateTime)
			{
				dateTimeFrom.Value = ((range.FromObject is DateTime) ? ((DateTime)range.FromObject) : dateTimeFrom.Value);
				dateTimeTo.Value = ((range.ToObject is DateTime) ? ((DateTime)range.ToObject) : dateTimeTo.Value);
			}
			else
			{
				dateTimeFrom.Value = ConvertTimeSpanToDateTime(range.FromObject);
				dateTimeTo.Value = ConvertTimeSpanToDateTime(range.ToObject);
			}
			if (range.FromObject == null)
			{
				dateTimeFrom.Format = (DateTimePickerFormat)8;
				dateTimeFrom.CustomFormat = " ";
				isNotAssignedFrom = true;
			}
			if (range.ToObject == null)
			{
				dateTimeTo.Format = (DateTimePickerFormat)8;
				dateTimeTo.CustomFormat = " ";
				isNotAssignedTo = true;
			}
			return;
		}
		string text = variable.ValueObject as string;
		if (!string.IsNullOrEmpty(text) && text.Contains("<<|>>") && variable.InitBy == StiVariableInitBy.Expression && report[variable.Name] is DateTimeRange dateTimeRange)
		{
			if (isDateTime)
			{
				dateTimeFrom.Value = dateTimeRange.FromDate;
				dateTimeTo.Value = dateTimeRange.ToDate;
			}
			else
			{
				dateTimeFrom.Value = ConvertTimeSpanToDateTime(dateTimeRange.FromObject);
				dateTimeTo.Value = ConvertTimeSpanToDateTime(dateTimeRange.ToObject);
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
		labelTo = new Label();
		labelFrom = new Label();
		dropDownButton = new StiParameterDropDownButtonV2();
		dateTimeFrom = new DateTimePicker();
		dateTimeTo = new DateTimePicker();
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
		((StiToolButton)dropDownButton).DropDownMenu = null;
		((StiToolButton)dropDownButton).HotDragDrop = false;
		((Control)dropDownButton).Location = new Point(340, 0);
		((Control)dropDownButton).Name = "dropDownButton";
		((Control)dropDownButton).Size = new Size(19, 22);
		((Control)dropDownButton).TabIndex = 19;
		((ButtonBase)dropDownButton).UseVisualStyleBackColor = true;
		dropDownButton.ItemChanged += DropDownButton_ItemChanged;
		((Control)dateTimeFrom).Dock = (DockStyle)3;
		dateTimeFrom.Format = (DateTimePickerFormat)8;
		((Control)dateTimeFrom).Location = new Point(30, 0);
		((Control)dateTimeFrom).Name = "dateTimeFrom";
		((Control)dateTimeFrom).Size = new Size(140, 20);
		((Control)dateTimeFrom).TabIndex = 20;
		dateTimeFrom.ValueChanged += DateTimeFromPicker_ValueChanged;
		((Control)dateTimeTo).Dock = (DockStyle)3;
		dateTimeTo.Format = (DateTimePickerFormat)8;
		((Control)dateTimeTo).Location = new Point(190, 0);
		((Control)dateTimeTo).Name = "dateTimeTo";
		((Control)dateTimeTo).Size = new Size(140, 20);
		((Control)dateTimeTo).TabIndex = 21;
		dateTimeTo.ValueChanged += DateTimeToPicker_ValueChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)dateTimeTo);
		((Control)this).Controls.Add((Control)(object)labelTo);
		((Control)this).Controls.Add((Control)(object)dateTimeFrom);
		((Control)this).Controls.Add((Control)(object)labelFrom);
		((Control)this).Controls.Add((Control)(object)dropDownButton);
		((Control)this).Name = "StiRangeDateTimeItemControl";
		((Control)this).Size = new Size(359, 22);
		((Control)this).ResumeLayout(false);
	}
}
