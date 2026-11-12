using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiValueChartMeterConverter : StiMeterConverter
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
			StiValueChartMeter stiValueChartMeter = value as StiValueChartMeter;
			Type[] types = new Type[9]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiChartSeriesType),
				typeof(StiSeriesYAxis),
				typeof(StiPenStyle),
				typeof(float),
				typeof(StiEmptyCellsAs),
				typeof(StiEmptyCellsAs)
			};
			object[] objs = new object[9] { stiValueChartMeter.Key, stiValueChartMeter.Expression, stiValueChartMeter.Label, stiValueChartMeter.SeriesType, stiValueChartMeter.YAxis, stiValueChartMeter.LineStyle, stiValueChartMeter.LineWidth, stiValueChartMeter.ShowZeros, stiValueChartMeter.ShowNulls };
			ConstructorInfo constructor = typeof(StiValueChartMeter).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
