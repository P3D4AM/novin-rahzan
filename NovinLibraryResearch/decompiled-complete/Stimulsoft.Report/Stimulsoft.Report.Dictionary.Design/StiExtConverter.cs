using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Text;
using System.Xml;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiExtConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
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

	protected object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}

	protected string Encode(params string[] args)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string text in args)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				stringBuilder = ((stringBuilder.Length == 0) ? stringBuilder.Append(text) : stringBuilder.AppendFormat(", {0}", text));
			}
		}
		return stringBuilder.ToString();
	}

	protected string Encode(string key, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return key + "=" + XmlConvert.EncodeName(value);
		}
		return null;
	}

	protected string EncodeBool(string key, bool value, bool defaultValue)
	{
		if (value != defaultValue)
		{
			return key + "=" + (value ? "True" : "False");
		}
		return null;
	}

	protected string EncodeEnum(string key, Enum value, Enum defaultValue)
	{
		if (!object.Equals(value, defaultValue))
		{
			return $"{key}={value}";
		}
		return null;
	}

	protected string EncodeBytes(string key, byte[] value)
	{
		string text = StiPacker.PackAndEncryptToString(value);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return key + "=" + XmlConvert.EncodeName(text);
		}
		return null;
	}

	protected List<dynamic> DecodeElements(string value)
	{
		if (value == null)
		{
			return null;
		}
		string[] array = value.Split(new char[1] { ',' });
		List<object> list = new List<object>();
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split(new char[1] { '=' });
			if (array3.Length == 2)
			{
				list.Add(new
				{
					Key = array3[0].Trim().ToLowerInvariant(),
					Value = XmlConvert.DecodeName(array3[1])
				});
			}
		}
		return list;
	}

	protected byte[] DecodeBytes(string value)
	{
		if (value == null)
		{
			return null;
		}
		value = XmlConvert.DecodeName(value);
		return StiPacker.UnpackAndDecrypt(value);
	}

	protected TEnum DecodeEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}
		if (!Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
		{
			return defaultValue;
		}
		return result;
	}
}
