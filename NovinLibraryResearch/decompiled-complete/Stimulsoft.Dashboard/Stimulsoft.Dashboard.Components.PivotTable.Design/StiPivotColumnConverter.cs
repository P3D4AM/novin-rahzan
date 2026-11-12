using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.PivotTable.Design;

public class StiPivotColumnConverter : StiMeterConverter
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
			StiPivotColumn stiPivotColumn = value as StiPivotColumn;
			Type[] types = new Type[9]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiHorAlignment),
				typeof(StiFormatService),
				typeof(StiDataTopN),
				typeof(bool),
				typeof(string),
				typeof(StiTableColumnSize)
			};
			object[] objs = new object[9] { stiPivotColumn.Key, stiPivotColumn.Expression, stiPivotColumn.Label, stiPivotColumn.HorAlignment, stiPivotColumn.TextFormat, stiPivotColumn.TopN, stiPivotColumn.ShowTotal, stiPivotColumn.TotalLabel, stiPivotColumn.Size };
			ConstructorInfo constructor = typeof(StiPivotColumn).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
