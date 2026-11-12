using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base.Drawing.Design;

public class StiCornerRadiusConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = ((string)value).Trim();
			char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			if (text.IndexOf(c) != -1)
			{
				string[] array = text.Split(new char[1] { c });
				return new StiCornerRadius(StiValueHelper.TryToFloat(array[0]), StiValueHelper.TryToFloat(array[1]), StiValueHelper.TryToFloat(array[2]), StiValueHelper.TryToFloat(array[3]));
			}
			string[] array2 = text.Split(new char[1] { ',' });
			if (array2.Length == 1)
			{
				return new StiCornerRadius(StiValueHelper.TryToFloat(array2[0]));
			}
			return new StiCornerRadius(StiValueHelper.TryToFloat(array2[0]), StiValueHelper.TryToFloat(array2[1]), StiValueHelper.TryToFloat(array2[2]), StiValueHelper.TryToFloat(array2[3]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		StiCornerRadius stiCornerRadius = (StiCornerRadius)value;
		if (destinationType == typeof(string))
		{
			char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			return $"{stiCornerRadius.TopLeft}{c}{stiCornerRadius.TopRight}{c}{stiCornerRadius.BottomRight}{c}{stiCornerRadius.BottomLeft}";
		}
		if (destinationType == typeof(InstanceDescriptor))
		{
			Type[] types = new Type[4]
			{
				typeof(float),
				typeof(float),
				typeof(float),
				typeof(float)
			};
			object[] objs = new object[4] { stiCornerRadius.TopLeft, stiCornerRadius.TopRight, stiCornerRadius.BottomRight, stiCornerRadius.BottomLeft };
			ConstructorInfo constructor = typeof(StiCornerRadius).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
