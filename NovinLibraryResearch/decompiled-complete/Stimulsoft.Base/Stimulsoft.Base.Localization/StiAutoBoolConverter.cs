using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization;

public class StiAutoBoolConverter : TypeConverter
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
			string text = value as string;
			if (StiPropertyGridOptions.Localizable)
			{
				if (Loc.GetEnum("boolFalse") == text)
				{
					return StiAutoBool.False;
				}
				if (Loc.GetEnum("boolTrue") == text)
				{
					return StiAutoBool.True;
				}
				if (Loc.GetMain("Auto") == text)
				{
					return StiAutoBool.Auto;
				}
			}
			if (Enum.TryParse<StiAutoBool>(text, ignoreCase: true, out var result))
			{
				return result;
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
			StiAutoBool stiAutoBool = (StiAutoBool)value;
			if (StiPropertyGridOptions.Localizable)
			{
				switch (stiAutoBool)
				{
				case StiAutoBool.False:
					return Loc.GetEnum("boolFalse");
				case StiAutoBool.True:
					return Loc.GetEnum("boolTrue");
				case StiAutoBool.Auto:
					return Loc.GetMain("Auto");
				}
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

	public StiAutoBoolConverter(Type type)
	{
		Type = type;
	}

	public StiAutoBoolConverter()
	{
	}
}
