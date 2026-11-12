using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization;

public class StiNullableEnumConverter : TypeConverter
{
	protected virtual IComparer Comparer => InvariantComparer.Default;

	protected Type NullableType { get; set; }

	protected StandardValuesCollection Values { get; set; }

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
			Type underlyingType = Nullable.GetUnderlyingType(NullableType);
			try
			{
				string text = (string)value;
				if (text.Length == 0)
				{
					return null;
				}
				if (StiPropertyGridOptions.Localizable)
				{
					string[] names = Enum.GetNames(underlyingType);
					string[] array = names;
					foreach (string text2 in array)
					{
						string text3 = StiLocalization.Get("PropertyEnum", underlyingType.Name + text2, throwError: false);
						if (text3 != null && text3 == text)
						{
							text = text2;
						}
					}
				}
				if (text.IndexOf(',') != -1)
				{
					long num = 0L;
					string[] array2 = text.Split(new char[1] { ',' });
					string[] array3 = array2;
					foreach (string value2 in array3)
					{
						num |= Convert.ToInt64((Enum)Enum.Parse(underlyingType, value2, ignoreCase: true));
					}
					return Enum.ToObject(underlyingType, num);
				}
				return Enum.Parse(underlyingType, text, ignoreCase: true);
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
		if (value == null)
		{
			return "null";
		}
		if (destinationType == typeof(string))
		{
			if (value is string && (string)value == "null")
			{
				return null;
			}
			Type underlyingType = Nullable.GetUnderlyingType(NullableType);
			Type underlyingType2 = Enum.GetUnderlyingType(underlyingType);
			if (value is IConvertible && value.GetType() != underlyingType2)
			{
				value = ((IConvertible)value).ToType(underlyingType2, culture);
			}
			if (!NullableType.IsDefined(typeof(FlagsAttribute), inherit: false) && !Enum.IsDefined(underlyingType, value))
			{
				throw new Exception("EnumConverterInvalidValue");
			}
			string text = Enum.Format(underlyingType, value, "G");
			if (StiPropertyGridOptions.Localizable)
			{
				if (text.IndexOf(',') == -1)
				{
					string text2 = StiLocalization.Get("PropertyEnum", underlyingType.Name + text, throwError: false);
					return text2 ?? text;
				}
				string[] array = text.Split(new char[1] { ',' });
				string text3 = string.Empty;
				string[] array2 = array;
				foreach (string text4 in array2)
				{
					string text5 = string.Empty;
					if (text3 != string.Empty)
					{
						text5 += ", ";
					}
					string text6 = StiLocalization.Get("PropertyEnum", underlyingType.Name + text4.Trim(), throwError: false);
					text3 = ((text6 == null) ? (text3 + text5 + text4.Trim()) : (text3 + text5 + text6));
				}
				return text3;
			}
			return text;
		}
		if (destinationType == typeof(InstanceDescriptor))
		{
			Type underlyingType3 = Nullable.GetUnderlyingType(NullableType);
			string text7 = ConvertToInvariantString(context, value);
			if (underlyingType3.IsDefined(typeof(FlagsAttribute), inherit: false) && text7.IndexOf(',') != -1)
			{
				Type underlyingType4 = Enum.GetUnderlyingType(underlyingType3);
				if (!(value is IConvertible))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				object obj = ((IConvertible)value).ToType(underlyingType4, culture);
				Type[] types = new Type[2]
				{
					typeof(Type),
					underlyingType4
				};
				MethodInfo method = typeof(Enum).GetMethod("ToObject", types);
				if (method == null)
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				return new InstanceDescriptor(method, new object[2] { underlyingType3, obj });
			}
			FieldInfo field = underlyingType3.GetField(text7);
			if (field != null)
			{
				return new InstanceDescriptor(field, null);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		if (Values == null)
		{
			Array values = Enum.GetValues(Nullable.GetUnderlyingType(NullableType));
			ArrayList values2 = new ArrayList(values) { "null" };
			Values = new StandardValuesCollection(values2);
		}
		return Values;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return !NullableType.IsDefined(typeof(FlagsAttribute), inherit: false);
	}

	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool IsValid(ITypeDescriptorContext context, object value)
	{
		return Enum.IsDefined(NullableType, value);
	}

	public StiNullableEnumConverter(Type type)
	{
		NullableType = type;
	}

	public StiNullableEnumConverter()
	{
	}
}
