using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table.Design;

public class StiSparklinesColumnConverter : StiMeterConverter
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
			StiSparklinesColumn stiSparklinesColumn = value as StiSparklinesColumn;
			Type[] types = new Type[17]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiHorAlignment),
				typeof(StiFormatService),
				typeof(StiSparklinesType),
				typeof(bool),
				typeof(bool),
				typeof(StiTableColumnVisibility),
				typeof(string),
				typeof(Color),
				typeof(bool),
				typeof(StiSummaryColumnType),
				typeof(StiHorAlignment),
				typeof(bool),
				typeof(Color),
				typeof(Color)
			};
			object[] objs = new object[17]
			{
				stiSparklinesColumn.Key, stiSparklinesColumn.Expression, stiSparklinesColumn.Label, stiSparklinesColumn.HorAlignment, stiSparklinesColumn.TextFormat, stiSparklinesColumn.Type, stiSparklinesColumn.ShowHighLowPoints, stiSparklinesColumn.ShowFirstLastPoints, stiSparklinesColumn.Visibility, stiSparklinesColumn.VisibilityExpression,
				stiSparklinesColumn.ForeColor, stiSparklinesColumn.ShowTotalSummary, stiSparklinesColumn.SummaryType, stiSparklinesColumn.SummaryAlignment, stiSparklinesColumn.AllowCustomColors, stiSparklinesColumn.PositiveColor, stiSparklinesColumn.NegativeColor
			};
			ConstructorInfo constructor = typeof(StiSparklinesColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
