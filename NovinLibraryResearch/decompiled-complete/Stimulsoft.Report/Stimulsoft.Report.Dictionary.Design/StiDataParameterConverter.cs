using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataParameterConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiDataParameter)
			{
				StiDataParameter stiDataParameter = (StiDataParameter)value;
				if (StiOptions.Engine.ReconnectDataSourcesIfRequestFromUserVariableChanged)
				{
					Type[] types = new Type[5]
					{
						typeof(string),
						typeof(string),
						typeof(int),
						typeof(int),
						typeof(string)
					};
					ConstructorInfo constructor = typeof(StiDataParameter).GetConstructor(types);
					if (constructor != null)
					{
						object[] objs = new object[5] { stiDataParameter.Name, stiDataParameter.Value, stiDataParameter.Type, stiDataParameter.Size, stiDataParameter.Key };
						return CreateNewInstanceDescriptor(constructor, objs);
					}
				}
				else
				{
					Type[] types2 = new Type[4]
					{
						typeof(string),
						typeof(int),
						typeof(int),
						typeof(string)
					};
					ConstructorInfo constructor2 = typeof(StiDataParameter).GetConstructor(types2);
					if (constructor2 != null)
					{
						object[] objs2 = new object[4] { stiDataParameter.Name, stiDataParameter.Type, stiDataParameter.Size, stiDataParameter.Key };
						return CreateNewInstanceDescriptor(constructor2, objs2);
					}
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiDataParameter stiDataParameter2)
		{
			if (!string.IsNullOrEmpty(stiDataParameter2.Key))
			{
				return $"{XmlConvert.EncodeName(stiDataParameter2.Name)},{XmlConvert.EncodeName(stiDataParameter2.Value)},{stiDataParameter2.Type.ToString()},{stiDataParameter2.Size.ToString()},{stiDataParameter2.Key}";
			}
			return $"{XmlConvert.EncodeName(stiDataParameter2.Name)},{XmlConvert.EncodeName(stiDataParameter2.Value)},{stiDataParameter2.Type.ToString()},{stiDataParameter2.Size.ToString()}";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

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
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = value as string;
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			if (array.Length == 4)
			{
				return new StiDataParameter(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), int.Parse(array[2]), int.Parse(array[3]));
			}
			return new StiDataParameter(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), int.Parse(array[2]), int.Parse(array[3]), array[4]);
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
