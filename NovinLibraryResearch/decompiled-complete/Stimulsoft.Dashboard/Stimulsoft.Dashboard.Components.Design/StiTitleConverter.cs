using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Design;

public class StiTitleConverter : StiOrderConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return "(" + Loc.Get("PropertyMain", "Title") + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTitle stiTitle = value as StiTitle;
			Type[] types = new Type[7]
			{
				typeof(string),
				typeof(Color),
				typeof(Color),
				typeof(Font),
				typeof(StiHorAlignment),
				typeof(bool),
				typeof(StiTextSizeMode)
			};
			ConstructorInfo constructor = typeof(StiTitle).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[7] { stiTitle.Text, stiTitle.ForeColor, stiTitle.BackColor, stiTitle.Font, stiTitle.HorAlignment, stiTitle.Visible, stiTitle.SizeMode };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
