using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

internal class ᬒ
{
	private static Type ᬓ;

	private static Type ᬔ;

	private static Type ᬕ;

	static ᬒ()
	{
		ᬓ = typeof(string);
		ᬕ = typeof(bool);
		ᬔ = typeof(CheckState);
	}

	internal static object ᬖ(object ԝ, Type ᬗ, TypeConverter ᬘ, TypeConverter ᬙ, string ᬚ, IFormatProvider ᬛ, object ᬜ, object ᬝ)
	{
		if (ᬤ(ԝ, ᬝ))
		{
			ԝ = DBNull.Value;
		}
		Type type = ᬗ;
		ᬗ = ᬢ(ᬗ);
		ᬘ = ᬢ(ᬘ);
		ᬙ = ᬢ(ᬙ);
		bool flag = (object)ᬗ != type;
		object obj = ᬟ(ԝ, ᬗ, ᬘ, ᬙ, ᬚ, ᬛ, ᬜ);
		if (type.IsValueType && obj == null && !flag)
		{
			throw new FormatException(ᬞ(ԝ, ᬗ));
		}
		return obj;
	}

	private static string ᬞ(object ԝ, Type ᬗ)
	{
		string format = ((ԝ == null) ? "Can't convert null value" : "Specified format cannot be converted");
		return string.Format(CultureInfo.CurrentCulture, format, new object[2] { ԝ, ᬗ.Name });
	}

	private static object ᬟ(object ԝ, Type ᬗ, TypeConverter ᬘ, TypeConverter ᬙ, string ᬚ, IFormatProvider ᬛ, object ᬜ)
	{
		if (ԝ == DBNull.Value || ԝ == null)
		{
			if (ᬜ != null)
			{
				return ᬜ;
			}
			if ((object)ᬗ == ᬓ)
			{
				return string.Empty;
			}
			if ((object)ᬗ == ᬔ)
			{
				return (object)(CheckState)2;
			}
			return null;
		}
		if ((object)ᬗ == ᬓ && ԝ is IFormattable && !string.IsNullOrEmpty(ᬚ))
		{
			return (ԝ as IFormattable).ToString(ᬚ, ᬛ);
		}
		Type type = ԝ.GetType();
		TypeConverter converter = TypeDescriptor.GetConverter(type);
		if (ᬘ != null && ᬘ != converter && ᬘ.CanConvertTo(ᬗ))
		{
			return ᬘ.ConvertTo(null, ᬠ(ᬛ), ԝ, ᬗ);
		}
		TypeConverter converter2 = TypeDescriptor.GetConverter(ᬗ);
		if (ᬙ != null && ᬙ != converter2 && ᬙ.CanConvertFrom(type))
		{
			return ᬙ.ConvertFrom(null, ᬠ(ᬛ), ԝ);
		}
		if ((object)ᬗ == ᬔ)
		{
			if ((object)type == ᬕ)
			{
				return (object)(CheckState)(((bool)ԝ) ? 1 : 0);
			}
			if (ᬘ == null)
			{
				ᬘ = converter;
			}
			if (ᬘ != null && ᬘ.CanConvertTo(ᬕ))
			{
				return (object)(CheckState)(((bool)ᬘ.ConvertTo(null, ᬠ(ᬛ), ԝ, ᬕ)) ? 1 : 0);
			}
		}
		if (ᬗ.IsAssignableFrom(type))
		{
			return ԝ;
		}
		if (ᬘ == null)
		{
			ᬘ = converter;
		}
		if (ᬙ == null)
		{
			ᬙ = converter2;
		}
		if (ᬘ != null && ᬘ.CanConvertTo(ᬗ))
		{
			return ᬘ.ConvertTo(null, ᬠ(ᬛ), ԝ, ᬗ);
		}
		if (ᬙ != null && ᬙ.CanConvertFrom(type))
		{
			return ᬙ.ConvertFrom(null, ᬠ(ᬛ), ԝ);
		}
		if (!(ԝ is IConvertible))
		{
			throw new FormatException(ᬞ(ԝ, ᬗ));
		}
		return ᬡ(ԝ, ᬗ, ᬛ);
	}

	private static CultureInfo ᬠ(IFormatProvider ᬛ)
	{
		if (ᬛ is CultureInfo)
		{
			return ᬛ as CultureInfo;
		}
		return CultureInfo.CurrentCulture;
	}

	private static object ᬡ(object ԝ, Type ᗧ, IFormatProvider ᬛ)
	{
		try
		{
			if (ᬛ == null)
			{
				ᬛ = CultureInfo.CurrentCulture;
			}
			return Convert.ChangeType(ԝ, ᗧ, ᬛ);
		}
		catch (InvalidCastException ex)
		{
			throw new FormatException(ex.Message, ex);
		}
	}

	private static Type ᬢ(Type ᗧ)
	{
		if ((object)ᗧ == ᬓ)
		{
			return ᬓ;
		}
		return Nullable.GetUnderlyingType(ᗧ) ?? ᗧ;
	}

	private static TypeConverter ᬢ(TypeConverter ᬣ)
	{
		if (!(ᬣ is NullableConverter nullableConverter))
		{
			return ᬣ;
		}
		return nullableConverter.UnderlyingTypeConverter;
	}

	internal static bool ᬤ(object ԝ, object ᬝ)
	{
		if (ԝ != null && ԝ != DBNull.Value)
		{
			return object.Equals(ԝ, ᬥ(ԝ.GetType(), ᬝ));
		}
		return true;
	}

	internal static object ᬥ(Type ᗧ, object ᬝ)
	{
		if (!ᗧ.IsGenericType || (object)ᗧ.GetGenericTypeDefinition() != typeof(Nullable<>))
		{
			return ᬝ;
		}
		if (ᬝ != null && ᬝ != DBNull.Value)
		{
			return ᬝ;
		}
		return null;
	}
}
