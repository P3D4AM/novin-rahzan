using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.PivotTable.Design;

public class StiPivotSummaryConverter : StiMeterConverter
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
			StiPivotSummary stiPivotSummary = value as StiPivotSummary;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(StiHorAlignment),
				typeof(StiFormatService),
				typeof(StiTableColumnSize)
			};
			object[] objs = new object[5] { stiPivotSummary.Expression, stiPivotSummary.Label, stiPivotSummary.HorAlignment, stiPivotSummary.TextFormat, stiPivotSummary.Size };
			ConstructorInfo constructor = typeof(StiPivotSummary).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
