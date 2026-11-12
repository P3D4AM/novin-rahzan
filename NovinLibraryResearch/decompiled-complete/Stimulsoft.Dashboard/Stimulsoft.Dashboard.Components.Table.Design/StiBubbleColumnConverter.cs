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

public class StiBubbleColumnConverter : StiMeterConverter
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
			StiBubbleColumn stiBubbleColumn = value as StiBubbleColumn;
			Type[] types = new Type[14]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiHorAlignment),
				typeof(StiFormatService),
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
			object[] objs = new object[14]
			{
				stiBubbleColumn.Key, stiBubbleColumn.Expression, stiBubbleColumn.Label, stiBubbleColumn.HorAlignment, stiBubbleColumn.TextFormat, stiBubbleColumn.Visibility, stiBubbleColumn.VisibilityExpression, stiBubbleColumn.ForeColor, stiBubbleColumn.ShowTotalSummary, stiBubbleColumn.SummaryType,
				stiBubbleColumn.SummaryAlignment, stiBubbleColumn.AllowCustomColors, stiBubbleColumn.PositiveColor, stiBubbleColumn.NegativeColor
			};
			ConstructorInfo constructor = typeof(StiBubbleColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
