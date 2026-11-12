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

public class StiPivotRowConverter : StiMeterConverter
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
			StiPivotRow stiPivotRow = value as StiPivotRow;
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
			object[] objs = new object[9] { stiPivotRow.Key, stiPivotRow.Expression, stiPivotRow.Label, stiPivotRow.HorAlignment, stiPivotRow.TextFormat, stiPivotRow.TopN, stiPivotRow.ShowTotal, stiPivotRow.TotalLabel, stiPivotRow.Size };
			ConstructorInfo constructor = typeof(StiPivotRow).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
