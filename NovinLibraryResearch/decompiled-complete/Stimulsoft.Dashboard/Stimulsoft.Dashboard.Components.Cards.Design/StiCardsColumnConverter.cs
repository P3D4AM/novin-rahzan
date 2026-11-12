using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards.Design;

public class StiCardsColumnConverter : StiMeterConverter
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
			StiCardsColumn stiCardsColumn = value as StiCardsColumn;
			List<Type> list = new List<Type>();
			list.Add(typeof(string));
			list.Add(typeof(string));
			list.Add(typeof(string));
			list.Add(typeof(StiHorAlignment));
			list.Add(typeof(StiFormatService));
			list.Add(typeof(StiCardsColumnVisibility));
			list.Add(typeof(string));
			list.Add(typeof(Color));
			List<Type> list2 = list;
			List<object> list3 = new List<object> { stiCardsColumn.Key, stiCardsColumn.Expression, stiCardsColumn.Label, stiCardsColumn.HorAlignment, stiCardsColumn.TextFormat, stiCardsColumn.Visibility, stiCardsColumn.VisibilityExpression, stiCardsColumn.ForeColor };
			ConstructorInfo constructor = typeof(StiCardsColumn).GetConstructor(list2.ToArray());
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, list3.ToArray());
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
