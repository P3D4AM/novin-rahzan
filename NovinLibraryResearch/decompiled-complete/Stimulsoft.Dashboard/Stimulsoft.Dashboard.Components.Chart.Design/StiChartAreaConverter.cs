using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiChartAreaConverter : StiUniversalConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiChartArea stiChartArea = value as StiChartArea;
			Type[] types = new Type[7]
			{
				typeof(bool),
				typeof(bool),
				typeof(bool),
				typeof(StiHorChartGridLines),
				typeof(StiVertChartGridLines),
				typeof(StiChartInterlacing),
				typeof(StiChartInterlacing)
			};
			object[] arguments = new object[7] { stiChartArea.ColorEach, stiChartArea.ReverseHor, stiChartArea.ReverseVert, stiChartArea.GridLinesHor, stiChartArea.GridLinesVert, stiChartArea.InterlacingHor, stiChartArea.InterlacingVert };
			ConstructorInfo constructor = typeof(StiChartArea).GetConstructor(types);
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, arguments);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
