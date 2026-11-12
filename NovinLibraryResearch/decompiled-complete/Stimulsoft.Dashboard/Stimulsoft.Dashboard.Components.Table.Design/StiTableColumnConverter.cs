using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table.Design;

public class StiTableColumnConverter : StiMeterConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTableColumn stiTableColumn = value as StiTableColumn;
			bool flag = stiTableColumn.DashboardInteraction != null && !stiTableColumn.DashboardInteraction.IsDefault;
			List<Type> list = new List<Type>();
			list.Add(typeof(string));
			list.Add(typeof(string));
			list.Add(typeof(string));
			list.Add(typeof(StiHorAlignment));
			list.Add(typeof(StiFormatService));
			list.Add(typeof(StiTableColumnVisibility));
			list.Add(typeof(string));
			list.Add(typeof(Color));
			list.Add(typeof(bool));
			list.Add(typeof(StiSummaryColumnType));
			list.Add(typeof(StiHorAlignment));
			List<Type> list2 = list;
			if (flag)
			{
				list2.Add(typeof(StiTableColumnDashboardInteraction));
			}
			List<object> list3 = new List<object>
			{
				stiTableColumn.Key, stiTableColumn.Expression, stiTableColumn.Label, stiTableColumn.HorAlignment, stiTableColumn.TextFormat, stiTableColumn.Visibility, stiTableColumn.VisibilityExpression, stiTableColumn.ForeColor, stiTableColumn.ShowTotalSummary, stiTableColumn.SummaryType,
				stiTableColumn.SummaryAlignment
			};
			if (flag)
			{
				list3.Add(stiTableColumn.DashboardInteraction);
			}
			ConstructorInfo constructor = typeof(StiTableColumn).GetConstructor(list2.ToArray());
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, list3.ToArray());
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
