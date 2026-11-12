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
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table.Design;

public class StiDimensionColumnConverter : StiMeterConverter
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
			StiDimensionColumn stiDimensionColumn = value as StiDimensionColumn;
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
				typeof(string),
				typeof(bool),
				typeof(StiSummaryColumnType),
				typeof(StiHorAlignment),
				typeof(StiTableColumnSize)
			};
			object[] objs = new object[14]
			{
				stiDimensionColumn.Key, stiDimensionColumn.Expression, stiDimensionColumn.Label, stiDimensionColumn.HorAlignment, stiDimensionColumn.TextFormat, stiDimensionColumn.Visibility, stiDimensionColumn.VisibilityExpression, stiDimensionColumn.ForeColor, stiDimensionColumn.ShowHyperlink, stiDimensionColumn.HyperlinkPattern,
				stiDimensionColumn.ShowTotalSummary, stiDimensionColumn.SummaryType, stiDimensionColumn.SummaryAlignment, stiDimensionColumn.Size
			};
			ConstructorInfo constructor = typeof(StiDimensionColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
