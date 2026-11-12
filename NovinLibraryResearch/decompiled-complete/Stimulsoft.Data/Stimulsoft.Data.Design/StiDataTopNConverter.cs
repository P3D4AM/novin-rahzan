using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Design;

public class StiDataTopNConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			IStiDataTopN stiDataTopN = context.Instance as IStiDataTopN;
			if (stiDataTopN?.TopN == null || stiDataTopN.TopN.IsDefault)
			{
				return "(" + Loc.Get("FormStyleDesigner", "NotSpecified") + ")";
			}
			return stiDataTopN.TopN.ToString();
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiDataTopN stiDataTopN2 = value as StiDataTopN;
			Type[] types = new Type[5]
			{
				typeof(StiDataTopNMode),
				typeof(int),
				typeof(bool),
				typeof(string),
				typeof(string)
			};
			object[] objs = new object[5] { stiDataTopN2.Mode, stiDataTopN2.Count, stiDataTopN2.ShowOthers, stiDataTopN2.OthersText, stiDataTopN2.MeasureField };
			ConstructorInfo constructor = typeof(StiDataTopN).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
