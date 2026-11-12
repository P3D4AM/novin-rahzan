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

public class StiColorScaleColumnConverter : StiMeterConverter
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
			StiColorScaleColumn stiColorScaleColumn = value as StiColorScaleColumn;
			Type[] types = new Type[13]
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
				typeof(Color),
				typeof(Color)
			};
			object[] objs = new object[13]
			{
				stiColorScaleColumn.Key, stiColorScaleColumn.Expression, stiColorScaleColumn.Label, stiColorScaleColumn.HorAlignment, stiColorScaleColumn.TextFormat, stiColorScaleColumn.Visibility, stiColorScaleColumn.VisibilityExpression, stiColorScaleColumn.ForeColor, stiColorScaleColumn.ShowTotalSummary, stiColorScaleColumn.SummaryType,
				stiColorScaleColumn.SummaryAlignment, stiColorScaleColumn.MinimumColor, stiColorScaleColumn.MaximumColor
			};
			ConstructorInfo constructor = typeof(StiColorScaleColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
