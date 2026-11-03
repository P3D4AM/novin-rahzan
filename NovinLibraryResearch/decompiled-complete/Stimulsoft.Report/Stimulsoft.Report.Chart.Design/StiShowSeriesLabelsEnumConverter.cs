using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart.Design;

public class StiShowSeriesLabelsEnumConverter : StiEnumConverter
{
	public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		if (((StiEnumConverter)this).Values == null)
		{
			Array values = Enum.GetValues(((StiEnumConverter)this).Type);
			List<StiShowSeriesLabels> list = new List<StiShowSeriesLabels>();
			foreach (StiShowSeriesLabels item in values)
			{
				Type type = item.GetType();
				MemberInfo[] member = type.GetMember(item.ToString());
				object[] customAttributes = member[0].GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false);
				if (customAttributes.Length == 0)
				{
					list.Add(item);
				}
			}
			values = list.ToArray();
			IComparer comparer = ((StiEnumConverter)this).Comparer;
			if (comparer != null)
			{
				Array.Sort(values, 0, values.Length, comparer);
			}
			((StiEnumConverter)this).Values = new TypeConverter.StandardValuesCollection(values);
		}
		return ((StiEnumConverter)this).Values;
	}

	public StiShowSeriesLabelsEnumConverter(Type type)
	{
		((StiEnumConverter)this).Type = type;
	}

	public StiShowSeriesLabelsEnumConverter()
	{
	}
}
