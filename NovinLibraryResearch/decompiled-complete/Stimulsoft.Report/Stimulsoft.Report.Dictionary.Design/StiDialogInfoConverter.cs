using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDialogInfoConverter : TypeConverter
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
			StiDialogInfo stiDialogInfo = (StiDialogInfo)value;
			if (stiDialogInfo.ItemsInitializationType == StiItemsInitializationType.Columns)
			{
				if (stiDialogInfo.BindingValue)
				{
					Type[] types = new Type[11]
					{
						typeof(StiDateTimeType),
						typeof(string),
						typeof(bool),
						typeof(string),
						typeof(string),
						typeof(string),
						typeof(bool),
						typeof(StiVariable),
						typeof(string),
						typeof(StiVariableSortDirection),
						typeof(StiVariableSortField)
					};
					ConstructorInfo constructor = typeof(StiDialogInfo).GetConstructor(types);
					if (constructor != null)
					{
						string text = stiDialogInfo.KeysColumn ?? string.Empty;
						string text2 = stiDialogInfo.ValuesColumn ?? string.Empty;
						string text3 = stiDialogInfo.CheckedColumn ?? string.Empty;
						string text4 = stiDialogInfo.BindingValuesColumn ?? string.Empty;
						object[] objs = new object[11]
						{
							stiDialogInfo.DateTimeType, stiDialogInfo.Mask, stiDialogInfo.AllowUserValues, text, text2, text3, stiDialogInfo.BindingValue, stiDialogInfo.BindingVariable, text4, stiDialogInfo.SortDirection,
							stiDialogInfo.SortField
						};
						return CreateNewInstanceDescriptor(constructor, objs);
					}
				}
				else
				{
					Type[] types2 = new Type[8]
					{
						typeof(StiDateTimeType),
						typeof(string),
						typeof(bool),
						typeof(string),
						typeof(string),
						typeof(string),
						typeof(StiVariableSortDirection),
						typeof(StiVariableSortField)
					};
					ConstructorInfo constructor2 = typeof(StiDialogInfo).GetConstructor(types2);
					if (constructor2 != null)
					{
						string text5 = stiDialogInfo.KeysColumn ?? string.Empty;
						string text6 = stiDialogInfo.ValuesColumn ?? string.Empty;
						string text7 = stiDialogInfo.CheckedColumn ?? string.Empty;
						object[] objs2 = new object[8] { stiDialogInfo.DateTimeType, stiDialogInfo.Mask, stiDialogInfo.AllowUserValues, text5, text6, text7, stiDialogInfo.SortDirection, stiDialogInfo.SortField };
						return CreateNewInstanceDescriptor(constructor2, objs2);
					}
				}
			}
			else
			{
				Type[] types3 = new Type[7]
				{
					typeof(StiDateTimeType),
					typeof(string),
					typeof(bool),
					typeof(StiVariableSortDirection),
					typeof(string[]),
					typeof(string[]),
					typeof(bool[])
				};
				ConstructorInfo constructor3 = typeof(StiDialogInfo).GetConstructor(types3);
				if (constructor3 != null)
				{
					string[] array = stiDialogInfo.Keys ?? new string[0];
					string[] array2 = stiDialogInfo.Values ?? new string[0];
					bool[] array3 = ((stiDialogInfo.CheckedStates != null && stiDialogInfo.CheckedStates.Length != 0) ? stiDialogInfo.CheckedStates : null);
					object[] objs3 = new object[7] { stiDialogInfo.DateTimeType, stiDialogInfo.Mask, stiDialogInfo.AllowUserValues, stiDialogInfo.SortDirection, array, array2, array3 };
					return CreateNewInstanceDescriptor(constructor3, objs3);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiDialogInfo stiDialogInfo2)
		{
			return StiObjectStateSaver.WriteObjectStateToString((object)stiDialogInfo2);
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
			StiDialogInfo stiDialogInfo = new StiDialogInfo();
			StiObjectStateSaver.ReadObjectStateFromString((object)stiDialogInfo, value as string);
			return stiDialogInfo;
		}
		return base.ConvertFrom(context, culture, value);
	}

	public static string ConvertDialogInfoToString(StiDialogInfo info)
	{
		if (info == null || info.IsDefault)
		{
			return "null";
		}
		StiDialogInfoConverter stiDialogInfoConverter = new StiDialogInfoConverter();
		return XmlConvert.EncodeName(stiDialogInfoConverter.ConvertToInvariantString(info));
	}

	public static StiDialogInfo ConvertFromStringToDialogInfo(string str)
	{
		if (str == "null" || string.IsNullOrWhiteSpace(str))
		{
			return new StiDialogInfo();
		}
		return new StiDialogInfoConverter().ConvertFromInvariantString(XmlConvert.DecodeName(str)) as StiDialogInfo;
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
