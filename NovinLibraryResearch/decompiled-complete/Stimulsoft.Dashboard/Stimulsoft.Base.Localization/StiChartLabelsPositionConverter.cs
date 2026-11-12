using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Base.Localization;

public class StiChartLabelsPositionConverter : StiEnumConverter
{
	public static string ConvertPositionToString(StiChartLabelsPosition value)
	{
		return value switch
		{
			StiChartLabelsPosition.Center => Loc.Get("Chart", "LabelsCenter"), 
			StiChartLabelsPosition.InsideBase => Loc.Get("Chart", "LabelsInsideBase"), 
			StiChartLabelsPosition.InsideEnd => Loc.Get("Chart", "LabelsInsideEnd"), 
			StiChartLabelsPosition.Left => Loc.GetMain("Left"), 
			StiChartLabelsPosition.None => Loc.Get("Chart", "LabelsNone"), 
			StiChartLabelsPosition.Outside => Loc.Get("Chart", "LabelsOutside"), 
			StiChartLabelsPosition.OutsideBase => Loc.Get("Chart", "LabelsOutsideBase"), 
			StiChartLabelsPosition.OutsideEnd => Loc.Get("Chart", "LabelsOutsideEnd"), 
			StiChartLabelsPosition.OutsideLeft => Loc.GetEnum("StiLegendHorAlignmentLeftOutside"), 
			StiChartLabelsPosition.OutsideRight => Loc.GetEnum("StiLegendHorAlignmentRightOutside"), 
			StiChartLabelsPosition.Right => Loc.GetMain("Right"), 
			StiChartLabelsPosition.TwoColumns => Loc.Get("Chart", "LabelsTwoColumns"), 
			StiChartLabelsPosition.Value => Loc.GetMain("Value"), 
			StiChartLabelsPosition.Total => Loc.GetMain("Total"), 
			_ => Loc.Get("Chart", "LabelsNone"), 
		};
	}

	public static StiChartLabelsPosition ConvertStringToPosition(string value)
	{
		if (value == Loc.Get("Chart", "LabelsNone"))
		{
			return StiChartLabelsPosition.None;
		}
		if (value == Loc.Get("Chart", "LabelsCenter"))
		{
			return StiChartLabelsPosition.Center;
		}
		if (value == Loc.Get("Chart", "LabelsInsideBase"))
		{
			return StiChartLabelsPosition.InsideBase;
		}
		if (value == Loc.Get("Chart", "LabelsInsideEnd"))
		{
			return StiChartLabelsPosition.InsideEnd;
		}
		if (value == Loc.GetMain("Left"))
		{
			return StiChartLabelsPosition.Left;
		}
		if (value == Loc.Get("Chart", "LabelsOutside"))
		{
			return StiChartLabelsPosition.Outside;
		}
		if (value == Loc.Get("Chart", "LabelsOutsideBase"))
		{
			return StiChartLabelsPosition.OutsideBase;
		}
		if (value == Loc.Get("Chart", "LabelsOutsideEnd"))
		{
			return StiChartLabelsPosition.OutsideEnd;
		}
		if (value == Loc.GetEnum("StiLegendHorAlignmentLeftOutside"))
		{
			return StiChartLabelsPosition.OutsideLeft;
		}
		if (value == Loc.GetEnum("StiLegendHorAlignmentRightOutside"))
		{
			return StiChartLabelsPosition.OutsideRight;
		}
		if (value == Loc.GetMain("Right"))
		{
			return StiChartLabelsPosition.Right;
		}
		if (value == Loc.Get("Chart", "LabelsTwoColumns"))
		{
			return StiChartLabelsPosition.TwoColumns;
		}
		if (value == Loc.GetMain("Value"))
		{
			return StiChartLabelsPosition.Value;
		}
		if (value == Loc.GetMain("Total"))
		{
			return StiChartLabelsPosition.Total;
		}
		return StiChartLabelsPosition.None;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			try
			{
				if (StiPropertyGridOptions.Localizable)
				{
					return ConvertStringToPosition(value as string);
				}
				return base.ConvertFrom(context, culture, value);
			}
			catch
			{
				throw new Exception("ConvertInvalidPrimitive");
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(string) && value is StiChartLabelsPosition && StiPropertyGridOptions.Localizable)
		{
			return ConvertPositionToString((StiChartLabelsPosition)value);
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		if (base.Values == null)
		{
			Array values = Enum.GetValues(base.Type);
			IComparer comparer = Comparer;
			if (comparer != null)
			{
				Array.Sort(values, 0, values.Length, comparer);
			}
			base.Values = new StandardValuesCollection(values);
		}
		return base.Values;
	}

	public StiChartLabelsPositionConverter(Type type)
		: base(type)
	{
		base.Type = type;
	}

	public StiChartLabelsPositionConverter()
	{
	}
}
