using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Xml;

namespace Stimulsoft.Report.Design;

public class RangeConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public static string RangeToString(Range range)
	{
		RangeConverter rangeConverter = new RangeConverter();
		return rangeConverter.ConvertToString(range);
	}

	public static Range StringToRange(string str)
	{
		RangeConverter rangeConverter = new RangeConverter();
		return rangeConverter.ConvertFromString(str) as Range;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is Range)
			{
				Range range = value as Range;
				Type[] types = new Type[2] { range.RangeType, range.RangeType };
				ConstructorInfo constructor = typeof(Range).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[2] { range.FromObject, range.ToObject };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string))
		{
			if (value is CharRange charRange)
			{
				return "Char," + XmlConvert.EncodeName(charRange.From.ToString()) + "," + XmlConvert.EncodeName(charRange.To.ToString());
			}
			string name;
			string name2;
			if (value is DateTimeRange dateTimeRange)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					name = dateTimeRange.From.ToString();
					name2 = dateTimeRange.To.ToString();
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
				return "DateTime," + XmlConvert.EncodeName(name) + "," + XmlConvert.EncodeName(name2);
			}
			if (value is TimeSpanRange timeSpanRange)
			{
				CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					name = timeSpanRange.From.ToString();
					name2 = timeSpanRange.To.ToString();
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture2;
				}
				return "TimeSpan," + XmlConvert.EncodeName(name) + "," + XmlConvert.EncodeName(name2);
			}
			if (value is DecimalRange decimalRange)
			{
				return string.Format("Decimal,{0},{1}", XmlConvert.EncodeName(decimalRange.From.ToString().Replace(",", ".")), XmlConvert.EncodeName(decimalRange.To.ToString().Replace(",", ".")));
			}
			if (value is FloatRange floatRange)
			{
				return string.Format("Float,{0},{1}", XmlConvert.EncodeName(floatRange.From.ToString().Replace(",", ".")), XmlConvert.EncodeName(floatRange.To.ToString().Replace(",", ".")));
			}
			if (value is DoubleRange doubleRange)
			{
				return string.Format("Double,{0},{1}", XmlConvert.EncodeName(doubleRange.From.ToString().Replace(",", ".")), XmlConvert.EncodeName(doubleRange.To.ToString().Replace(",", ".")));
			}
			if (value is ByteRange byteRange)
			{
				return $"Byte,{byteRange.From},{byteRange.To}";
			}
			if (value is ShortRange shortRange)
			{
				return $"Short,{shortRange.From},{shortRange.To}";
			}
			if (value is IntRange intRange)
			{
				return $"Int,{intRange.From},{intRange.To}";
			}
			if (value is LongRange longRange)
			{
				return $"Long,{longRange.From},{longRange.To}";
			}
			if (value is GuidRange guidRange)
			{
				return $"Guid,{XmlConvert.EncodeName(guidRange.From.ToString())},{XmlConvert.EncodeName(guidRange.To.ToString())}";
			}
			if (value is StringRange stringRange)
			{
				return $"String,{XmlConvert.EncodeName(stringRange.From)},{XmlConvert.EncodeName(stringRange.To)}";
			}
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
			if (array[0] == "Char")
			{
				return new CharRange(XmlConvert.DecodeName(array[1])[0], XmlConvert.DecodeName(array[2])[0]);
			}
			if (array[0] == "DateTime")
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				DateTime? from;
				DateTime? to;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					string text2 = XmlConvert.DecodeName(array[1]);
					string text3 = XmlConvert.DecodeName(array[2]);
					from = ((!string.IsNullOrEmpty(text2.Trim())) ? new DateTime?(DateTime.Parse(text2)) : null);
					to = ((!string.IsNullOrEmpty(text3.Trim())) ? new DateTime?(DateTime.Parse(text3)) : null);
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
				return new DateTimeRange(from, to);
			}
			if (array[0] == "TimeSpan")
			{
				CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
				TimeSpan? from2;
				TimeSpan? to2;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					string text4 = XmlConvert.DecodeName(array[1]);
					string text5 = XmlConvert.DecodeName(array[2]);
					from2 = ((!string.IsNullOrEmpty(text4.Trim())) ? new TimeSpan?(TimeSpan.Parse(text4)) : null);
					to2 = ((!string.IsNullOrEmpty(text5.Trim())) ? new TimeSpan?(TimeSpan.Parse(text5)) : null);
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture2;
				}
				return new TimeSpanRange(from2, to2);
			}
			if (array[0] == "Decimal")
			{
				string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				decimal from3 = decimal.Parse(XmlConvert.DecodeName(array[1]).Replace(".", ",").Replace(",", numberDecimalSeparator));
				decimal to3 = decimal.Parse(XmlConvert.DecodeName(array[2]).Replace(".", ",").Replace(",", numberDecimalSeparator));
				return new DecimalRange(from3, to3);
			}
			if (array[0] == "Float")
			{
				string numberDecimalSeparator2 = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				float from4 = float.Parse(XmlConvert.DecodeName(array[1]).Replace(".", ",").Replace(",", numberDecimalSeparator2));
				float to4 = float.Parse(XmlConvert.DecodeName(array[2]).Replace(".", ",").Replace(",", numberDecimalSeparator2));
				return new FloatRange(from4, to4);
			}
			if (array[0] == "Double")
			{
				string numberDecimalSeparator3 = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				double from5 = double.Parse(XmlConvert.DecodeName(array[1]).Replace(".", ",").Replace(",", numberDecimalSeparator3));
				double to5 = double.Parse(XmlConvert.DecodeName(array[2]).Replace(".", ",").Replace(",", numberDecimalSeparator3));
				return new DoubleRange(from5, to5);
			}
			if (array[0] == "Byte")
			{
				byte from6 = byte.Parse(array[1]);
				byte to6 = byte.Parse(array[2]);
				return new ByteRange(from6, to6);
			}
			if (array[0] == "Short")
			{
				short from7 = short.Parse(array[1]);
				short to7 = short.Parse(array[2]);
				return new ShortRange(from7, to7);
			}
			if (array[0] == "Int")
			{
				int from8 = int.Parse(array[1]);
				int to8 = int.Parse(array[2]);
				return new IntRange(from8, to8);
			}
			if (array[0] == "Long")
			{
				long from9 = long.Parse(array[1]);
				long to9 = long.Parse(array[2]);
				return new LongRange(from9, to9);
			}
			if (array[0] == "Guid")
			{
				string text6 = XmlConvert.DecodeName(array[1]);
				string text7 = XmlConvert.DecodeName(array[2]);
				Guid from10 = (string.IsNullOrEmpty(text6) ? default(Guid) : new Guid(text6));
				Guid to10 = (string.IsNullOrEmpty(text7) ? default(Guid) : new Guid(text7));
				return new GuidRange(from10, to10);
			}
			if (array[0] == "String")
			{
				string from11 = XmlConvert.DecodeName(array[1]);
				string to11 = XmlConvert.DecodeName(array[2]);
				return new StringRange(from11, to11);
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
