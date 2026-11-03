using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataColumnConverter : TypeConverter
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
			if (value is StiCalcDataColumn)
			{
				StiCalcDataColumn stiCalcDataColumn = (StiCalcDataColumn)value;
				Type[] types = new Type[5]
				{
					typeof(string),
					typeof(string),
					typeof(Type),
					typeof(string),
					typeof(string)
				};
				ConstructorInfo constructor = typeof(StiCalcDataColumn).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[5] { stiCalcDataColumn.Name, stiCalcDataColumn.Alias, stiCalcDataColumn.Type, stiCalcDataColumn.Value, stiCalcDataColumn.Key };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiDataTransformationColumn)
			{
				StiDataTransformationColumn stiDataTransformationColumn = (StiDataTransformationColumn)value;
				Type[] types2 = new Type[6]
				{
					typeof(string),
					typeof(string),
					typeof(Type),
					typeof(string),
					typeof(string),
					typeof(StiDataTransformationMode)
				};
				ConstructorInfo constructor2 = typeof(StiDataTransformationColumn).GetConstructor(types2);
				if (constructor2 != null)
				{
					object[] objs2 = new object[6] { stiDataTransformationColumn.Name, stiDataTransformationColumn.Alias, stiDataTransformationColumn.Type, stiDataTransformationColumn.Expression, stiDataTransformationColumn.Key, stiDataTransformationColumn.Mode };
					return CreateNewInstanceDescriptor(constructor2, objs2);
				}
			}
			else if (value is StiDataColumn)
			{
				StiDataColumn stiDataColumn = (StiDataColumn)value;
				Type[] types3 = new Type[5]
				{
					typeof(string),
					typeof(string),
					typeof(string),
					typeof(Type),
					typeof(string)
				};
				ConstructorInfo constructor3 = typeof(StiDataColumn).GetConstructor(types3);
				if (constructor3 != null)
				{
					object[] objs3 = new object[5] { stiDataColumn.NameInSource, stiDataColumn.Name, stiDataColumn.Alias, stiDataColumn.Type, stiDataColumn.Key };
					return CreateNewInstanceDescriptor(constructor3, objs3);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiDataColumn stiDataColumn2)
		{
			if (stiDataColumn2 is StiCalcDataColumn)
			{
				if (!string.IsNullOrEmpty(stiDataColumn2.Key))
				{
					StiCalcDataColumn stiCalcDataColumn2 = stiDataColumn2 as StiCalcDataColumn;
					return string.Format("{0},{1},{2},{3},{4},{5}", "CALC", XmlConvert.EncodeName(stiDataColumn2.Name), XmlConvert.EncodeName(stiDataColumn2.Alias), ConvertTypeToString(stiDataColumn2.Type), XmlConvert.EncodeName(stiCalcDataColumn2.Value), XmlConvert.EncodeName(stiDataColumn2.Key));
				}
				StiCalcDataColumn stiCalcDataColumn3 = stiDataColumn2 as StiCalcDataColumn;
				return $"{XmlConvert.EncodeName(stiDataColumn2.Name)},{XmlConvert.EncodeName(stiDataColumn2.Alias)},{ConvertTypeToString(stiDataColumn2.Type)},{XmlConvert.EncodeName(stiCalcDataColumn3.Value)}";
			}
			if (stiDataColumn2 is StiDataTransformationColumn)
			{
				StiDataTransformationColumn stiDataTransformationColumn2 = stiDataColumn2 as StiDataTransformationColumn;
				return string.Format("{0},{1},{2},{3},{4},{5},{6}", "TRANSFORM", XmlConvert.EncodeName(stiDataColumn2.Name), XmlConvert.EncodeName(stiDataColumn2.Alias), ConvertTypeToString(stiDataColumn2.Type), XmlConvert.EncodeName(stiDataTransformationColumn2.Expression), XmlConvert.EncodeName(stiDataColumn2.Key), stiDataTransformationColumn2.Mode);
			}
			if (!string.IsNullOrEmpty(stiDataColumn2.Key))
			{
				return string.Format("{0},{1},{2},{3},{4},{5}", "ORIGINAL", XmlConvert.EncodeName(stiDataColumn2.NameInSource), XmlConvert.EncodeName(stiDataColumn2.Name), XmlConvert.EncodeName(stiDataColumn2.Alias), ConvertTypeToString(stiDataColumn2.Type), XmlConvert.EncodeName(stiDataColumn2.Key));
			}
			if (stiDataColumn2.Name == stiDataColumn2.Alias && stiDataColumn2.Name == stiDataColumn2.NameInSource)
			{
				return $"{XmlConvert.EncodeName(stiDataColumn2.Name)},{ConvertTypeToString(stiDataColumn2.Type)}";
			}
			if (stiDataColumn2.Name == stiDataColumn2.NameInSource)
			{
				return $"{XmlConvert.EncodeName(stiDataColumn2.Name)},{XmlConvert.EncodeName(stiDataColumn2.Alias)},{ConvertTypeToString(stiDataColumn2.Type)}";
			}
			return string.Format("{0},{1},{2},{3},{4}", "ORIGINAL", XmlConvert.EncodeName(stiDataColumn2.NameInSource), XmlConvert.EncodeName(stiDataColumn2.Name), XmlConvert.EncodeName(stiDataColumn2.Alias), ConvertTypeToString(stiDataColumn2.Type));
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
			if (array.Length == 7 || array.Length == 6)
			{
				if (array[0] == "CALC")
				{
					Type type = StiTypeFinder.GetType(DecodeTypeName(array[3]));
					type = CheckType(DecodeTypeName(array[2]), type);
					return new StiCalcDataColumn(XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), type, XmlConvert.DecodeName(array[4]), XmlConvert.DecodeName(array[5]));
				}
				if (array[0] == "TRANSFORM")
				{
					Type type2 = StiTypeFinder.GetType(DecodeTypeName(array[3]));
					type2 = CheckType(DecodeTypeName(array[2]), type2);
					StiDataTransformationMode mode = ((array.Length != 6) ? ((StiDataTransformationMode)Enum.Parse(typeof(StiDataTransformationMode), XmlConvert.DecodeName(array[6]))) : StiDataTransformationMode.Dimension);
					return new StiDataTransformationColumn(XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), type2, XmlConvert.DecodeName(array[4]), XmlConvert.DecodeName(array[5]), mode);
				}
				Type type3 = StiTypeFinder.GetType(DecodeTypeName(array[4]));
				type3 = CheckType(DecodeTypeName(array[4]), type3);
				return new StiDataColumn(XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), type3, XmlConvert.DecodeName(array[5]));
			}
			if (array.Length == 5)
			{
				if (array[0] == "ORIGINAL")
				{
					Type type4 = StiTypeFinder.GetType(DecodeTypeName(array[4]));
					type4 = CheckType(DecodeTypeName(array[4]), type4);
					return new StiDataColumn(XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), type4);
				}
				Type type5 = StiTypeFinder.GetType(DecodeTypeName(array[2]));
				type5 = CheckType(DecodeTypeName(array[2]), type5);
				return new StiCalcDataColumn(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), type5, XmlConvert.DecodeName(array[4]));
			}
			if (array.Length == 4)
			{
				Type type6 = StiTypeFinder.GetType(DecodeTypeName(array[2]));
				type6 = CheckType(DecodeTypeName(array[2]), type6);
				return new StiCalcDataColumn(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), type6, XmlConvert.DecodeName(array[3]));
			}
			if (array.Length == 3)
			{
				Type type7 = StiTypeFinder.GetType(DecodeTypeName(array[2]));
				type7 = CheckType(DecodeTypeName(array[2]), type7);
				return new StiDataColumn(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), type7);
			}
			Type type8 = null;
			try
			{
				type8 = StiTypeFinder.GetType(DecodeTypeName(array[1]));
			}
			catch
			{
			}
			type8 = CheckType(DecodeTypeName(array[1]), type8);
			return new StiDataColumn(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[0]), type8);
		}
		return base.ConvertFrom(context, culture, value);
	}

	internal static Type CheckType(string typeName, Type type)
	{
		if (StiOptions.Engine.FullTrust && type == null && !string.IsNullOrEmpty(typeName))
		{
			type = CreateUndefinedType(typeName);
		}
		if (type == null)
		{
			type = typeof(object);
		}
		return type;
	}

	private static string EncodeTypeName(string typeName)
	{
		return typeName.Replace(",", "_x002c_");
	}

	private static string DecodeTypeName(string typeName)
	{
		return typeName.Replace("_x002c_", ",");
	}

	private static Type CreateUndefinedType(string typeName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		return (Type)new StiUndefinedType(typeName);
	}

	public static string ConvertTypeToString(Type type)
	{
		if (StiOptions.Engine.FullTrust)
		{
			return EncodeTypeName(ConvertTypeToStringFullTrust(type));
		}
		return EncodeTypeName(type.ToString());
	}

	private static string ConvertTypeToStringFullTrust(Type type)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (type == typeof(StiUndefinedType))
		{
			return ((StiUndefinedType)type).Type;
		}
		return type.ToString();
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
