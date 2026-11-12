using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Viewer.Controls.DatePicker;
using Stimulsoft.Dashboard.Viewer.Elements.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.DatePicker;

[ToolboxItem(false)]
public class StiDatePickerElementControl : StiFilterElementControl
{
	private StiDateTimePicker DatePickerControl => base.Child as StiDateTimePicker;

	protected StiDatePickerElement DatePickerElement => base.Element as StiDatePickerElement;

	private string SelectionEndStr => StiDatePickerHelper.GetFormatedDate(DatePickerControl.SelectionEnd);

	private string SelectionStartStr => StiDatePickerHelper.GetFormatedDate(DatePickerControl.SelectionStart);

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && DatePickerControl != null)
		{
			DatePickerControl.Colors = GetColorCollection();
		}
	}

	public override void CreateChild()
	{
		StiDateTimePicker stiDateTimePicker = (StiDateTimePicker)(object)(base.Child = (Control)(object)new StiDateTimePicker(DatePickerElement.TextFormat, base.Element.Report)
		{
			Colors = GetColorCollection(),
			SelectionMode = DatePickerElement.SelectionMode,
			IsTransparent = IsTransparent
		});
		if (StiDatePickerHelper.IsVariableSpecifiedAsValue(DatePickerElement))
		{
			stiDateTimePicker.SelectionMode = (Stimulsoft.Report.Range.IsRangeType((StiDatePickerHelper.GetVariableSpecifiedAsValue(DatePickerElement) as StiVariable)?.Type) ? StiDateSelectionMode.Range : StiDateSelectionMode.Single);
		}
		if (base.Element is IStiControlElement)
		{
			base.Child.Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		StartRefreshingFilter();
		DatePickerControl.DateChanged += DatePickerControlDateChanged;
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.Child.Visible = true;
		if (StiDatePickerHelper.IsVariableSpecifiedAsValue(DatePickerElement))
		{
			if (Stimulsoft.Report.Range.IsRangeType((StiDatePickerHelper.GetVariableSpecifiedAsValue(DatePickerElement) is StiVariable stiVariable) ? stiVariable.Type : null) && DatePickerElement.InitialRangeSelectionSource == StiInitialDateRangeSelectionSource.Selection)
			{
				StiDatePickerHelper.CalculateRangeFromInitial(DatePickerElement.InitialRangeSelection, out var start, out var end);
				DatePickerControl.SelectionStart = start;
				DatePickerControl.SelectionEnd = end;
			}
			else
			{
				object defaultValue = StiDatePickerHelper.GetDefaultValue(DatePickerElement);
				if (defaultValue is DateTime || defaultValue is DateTime?)
				{
					DatePickerControl.SelectionStart = (DateTime)defaultValue;
					DatePickerControl.SelectionMode = StiDateSelectionMode.Single;
				}
				else if (defaultValue is DateTimeRange)
				{
					DatePickerControl.SelectionRange = defaultValue as DateTimeRange;
					DatePickerControl.SelectionMode = StiDateSelectionMode.Range;
				}
			}
		}
		else if (DatePickerElement.SelectionMode == StiDateSelectionMode.AutoRange)
		{
			IEnumerable<object> source = from r in dataTable.Rows
				select r[0] into d
				where d is DateTime
				select d;
			if (source.Any())
			{
				DatePickerControl.SelectionStart = source.Where((object d) => d is DateTime).Cast<DateTime>().Min();
				DatePickerControl.SelectionEnd = source.Where((object d) => d is DateTime).Cast<DateTime>().Max();
			}
		}
		else if (DatePickerElement.SelectionMode == StiDateSelectionMode.Range)
		{
			StiDatePickerHelper.CalculateRangeFromInitial(DatePickerElement.InitialRangeSelection, out var start2, out var end2);
			DatePickerControl.SelectionStart = start2;
			DatePickerControl.SelectionEnd = end2;
		}
		base.IsFirstInitialized = true;
	}

	protected override void RefreshFilter()
	{
		if (base.LockRefresh)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			if (StiDatePickerHelper.GetVariableSpecifiedAsValue(DatePickerElement) is StiVariable variable)
			{
				ApplyToVariable(variable);
			}
			else
			{
				string valueMeterExpression = StiDatePickerHelper.GetValueMeterExpression(DatePickerElement);
				if (valueMeterExpression == null)
				{
					return;
				}
				if (DatePickerElement.SelectionMode == StiDateSelectionMode.Single)
				{
					DatePickerElement.UserFilters = new StiDataFilterRule(valueMeterExpression, StiDatePickerHelper.GetCondition(DatePickerElement), SelectionStartStr).ToList();
				}
				else
				{
					DatePickerElement.UserFilters = new StiDataFilterRule("", valueMeterExpression, StiDataFilterCondition.Between, SelectionStartStr, SelectionEndStr, isEnabled: true, isExpression: false).ToList();
				}
			}
			RefreshViewerData();
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	protected override void ResetFilter()
	{
		DatePickerElement.UserFilters = new List<StiDataFilterRule>();
	}

	private void ApplyToVariable(StiVariable variable)
	{
		object obj = ((variable.Type == typeof(DateTimeRange)) ? DatePickerControl.SelectionRange : ((object)DatePickerControl.SelectionStart));
		StiVariableHelper.SetVariableValue(DatePickerElement.Report, variable, obj);
		if (variable.InitBy == StiVariableInitBy.Value)
		{
			variable.ValueObject = obj;
		}
		StiCacheCleaner.Clean(DatePickerElement.Report.Key);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && DatePickerControl != null)
		{
			DatePickerControl.DateChanged -= DatePickerControlDateChanged;
		}
		base.Dispose(disposing);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		base.Child.Width = ((Control)this).Width;
		base.Child.Top = (((Control)this).Height - base.Child.Height) / 2;
	}

	private void DatePickerControlDateChanged(object sender, EventArgs e)
	{
		StartRefreshingFilter();
	}

	public StiDatePickerElementControl(IStiElement element)
		: base(element)
	{
	}
}
