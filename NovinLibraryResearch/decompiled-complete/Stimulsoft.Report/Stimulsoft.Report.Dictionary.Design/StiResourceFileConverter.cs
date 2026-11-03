using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiResourceFileConverter : StiExtConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiResource stiResource = value as StiResource;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(StiResourceType),
				typeof(byte[]),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiResource).GetConstructor(types);
			if (constructor == null)
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			object[] objs = new object[5] { stiResource.Name, stiResource.Alias, stiResource.Type, stiResource.Content, stiResource.AvailableInTheViewer };
			return CreateNewInstanceDescriptor(constructor, objs);
		}
		if (destinationType == typeof(string) && value is StiResource stiResource2)
		{
			string text = Encode("Name", stiResource2.Name);
			string text2 = Encode("Alias", stiResource2.Alias);
			string text3 = Encode("Key", stiResource2.Key);
			string text4 = EncodeBool("Inherited", stiResource2.Inherited, defaultValue: false);
			string text5 = EncodeEnum("Type", stiResource2.Type, StiResourceType.Image);
			string text6 = EncodeBool("AvailableInTheViewer", stiResource2.AvailableInTheViewer, defaultValue: false);
			string text7 = EncodeBytes("Content", stiResource2.Content);
			return Encode(text, text2, text6, text3, text4, text5, text7);
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			StiResource stiResource = new StiResource();
			List<object> list = DecodeElements(value as string);
			if (list != null)
			{
				foreach (dynamic item in list)
				{
					if (item.Key == "name")
					{
						stiResource.Name = item.Value;
					}
					else if (item.Key == "alias")
					{
						stiResource.Alias = item.Value;
					}
					else if (item.Key == "key")
					{
						stiResource.Key = item.Value;
					}
					else if (item.Key == "inherited")
					{
						stiResource.Inherited = item.Value == "True";
					}
					else if (item.Key == "type")
					{
						stiResource.Type = DecodeEnum<StiResourceType>(item.Value, StiResourceType.Image);
					}
					else if (item.Key == "availableintheviewer")
					{
						stiResource.AvailableInTheViewer = item.Value == "True";
					}
					else if (item.Key == "content")
					{
						stiResource.Content = DecodeBytes(item.Value);
					}
				}
			}
			return stiResource;
		}
		return base.ConvertFrom(context, culture, value);
	}
}
