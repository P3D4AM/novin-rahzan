using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization;

public class StiEnumConverter : TypeConverter
{
	protected virtual IComparer Comparer => InvariantComparer.Default;

	protected Type EnumType => Type;

	protected StandardValuesCollection Values { get; set; }

	protected Type Type { get; set; }

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
			try
			{
				string text = (string)value;
				if (StiPropertyGridOptions.Localizable)
				{
					string[] names = Enum.GetNames(Type);
					string[] array = names;
					foreach (string text2 in array)
					{
						string text3 = StiLocalization.Get("PropertyEnum", Type.Name + text2, throwError: false);
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
					for (int j = 0; j < array2.Length; j++)
					{
						num |= Convert.ToInt64((Enum)Enum.Parse(Type, array2[j], ignoreCase: true));
					}
					return Enum.ToObject(Type, num);
				}
				return Enum.Parse(Type, text, ignoreCase: true);
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
		if (destinationType == typeof(string) && value != null)
		{
			Type underlyingType = Enum.GetUnderlyingType(Type);
			if (value is IConvertible && value.GetType() != underlyingType)
			{
				value = ((IConvertible)value).ToType(underlyingType, culture);
			}
			if (!Type.IsDefined(typeof(FlagsAttribute), inherit: false) && !Enum.IsDefined(Type, value))
			{
				throw new Exception("EnumConverterInvalidValue");
			}
			string text = Enum.Format(Type, value, "G");
			if (StiPropertyGridOptions.Localizable)
			{
				if (text.IndexOf(',') == -1)
				{
					string text2 = StiLocalization.Get("PropertyEnum", Type.Name + text, throwError: false);
					return text2 ?? text;
				}
				string[] array = text.Split(new char[1] { ',' });
				string text3 = "";
				string[] array2 = array;
				foreach (string text4 in array2)
				{
					string text5 = "";
					if (text3 != "")
					{
						text5 += ", ";
					}
					string text6 = StiLocalization.Get("PropertyEnum", Type.Name + text4.Trim(), throwError: false);
					text3 = ((text6 == null) ? (text3 + text5 + text4.Trim()) : (text3 + text5 + text6));
				}
				return text3;
			}
			return text;
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			string text7 = ConvertToInvariantString(context, value);
			if (Type.IsDefined(typeof(FlagsAttribute), inherit: false) && text7.IndexOf(',') != -1)
			{
				Type underlyingType2 = Enum.GetUnderlyingType(Type);
				if (!(value is IConvertible))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				object obj = ((IConvertible)value).ToType(underlyingType2, culture);
				Type[] types = new Type[2]
				{
					typeof(Type),
					underlyingType2
				};
				MethodInfo method = typeof(Enum).GetMethod("ToObject", types);
				if (method == null)
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				return new InstanceDescriptor(method, new object[2] { Type, obj });
			}
			FieldInfo field = Type.GetField(text7);
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
			Array values = Enum.GetValues(Type);
			IComparer comparer = Comparer;
			if (comparer != null)
			{
				Array.Sort(values, 0, values.Length, comparer);
			}
			Values = new StandardValuesCollection(values);
		}
		return Values;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return !Type.IsDefined(typeof(FlagsAttribute), inherit: false);
	}

	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool IsValid(ITypeDescriptorContext context, object value)
	{
		return Enum.IsDefined(Type, value);
	}

	public StiEnumConverter(Type type)
	{
		Type = type;
	}

	public StiEnumConverter()
	{
	}
}
