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

public class StiDataBarsColumnConverter : StiMeterConverter
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
			StiDataBarsColumn stiDataBarsColumn = value as StiDataBarsColumn;
			Type[] types = new Type[11]
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
				typeof(StiHorAlignment)
			};
			object[] objs = new object[11]
			{
				stiDataBarsColumn.Key, stiDataBarsColumn.Expression, stiDataBarsColumn.Label, stiDataBarsColumn.HorAlignment, stiDataBarsColumn.TextFormat, stiDataBarsColumn.Visibility, stiDataBarsColumn.VisibilityExpression, stiDataBarsColumn.ForeColor, stiDataBarsColumn.ShowTotalSummary, stiDataBarsColumn.SummaryType,
				stiDataBarsColumn.SummaryAlignment
			};
			ConstructorInfo constructor = typeof(StiDataBarsColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
