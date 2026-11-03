using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiVariableConverter : TypeConverter
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
			StiVariable stiVariable = (StiVariable)value;
			Type[] types = new Type[13]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(Type),
				typeof(string),
				typeof(bool),
				typeof(StiVariableInitBy),
				typeof(bool),
				typeof(StiDialogInfo),
				typeof(string),
				typeof(bool),
				typeof(StiSelectionMode)
			};
			ConstructorInfo constructor = typeof(StiVariable).GetConstructor(types);
			if (constructor == null)
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			object[] objs = new object[13]
			{
				stiVariable.Category,
				stiVariable.Name,
				stiVariable.Alias,
				stiVariable.Description,
				stiVariable.Type,
				stiVariable.Value,
				stiVariable.ReadOnly,
				stiVariable.InitBy,
				stiVariable.RequestFromUser,
				(stiVariable.DialogInfo == null || stiVariable.DialogInfo.IsDefault) ? new StiDialogInfo() : stiVariable.DialogInfo,
				stiVariable.Key,
				stiVariable.AllowUseAsSqlParameter,
				stiVariable.Selection
			};
			return CreateNewInstanceDescriptor(constructor, objs);
		}
		if (destinationType == typeof(string) && value is StiVariable stiVariable2)
		{
			string text = XmlConvert.EncodeName(stiVariable2.Value);
			string text2 = XmlConvert.EncodeName(stiVariable2.Category);
			string text3 = XmlConvert.EncodeName(stiVariable2.Name);
			string text4 = XmlConvert.EncodeName(stiVariable2.Alias);
			string text5 = XmlConvert.EncodeName(stiVariable2.Description);
			if ((stiVariable2.Name == null && stiVariable2.Alias == null) || (stiVariable2.Name == "" && stiVariable2.Alias == ""))
			{
				if (!stiVariable2.Inherited)
				{
					return stiVariable2.Category;
				}
				return $"{stiVariable2.Category},{stiVariable2.Inherited}";
			}
			if ((stiVariable2.DialogInfo == null || stiVariable2.DialogInfo.IsDefault) && string.IsNullOrWhiteSpace(stiVariable2.Key) && !stiVariable2.AllowUseAsSqlParameter && stiVariable2.Selection == StiSelectionMode.FromVariable)
			{
				return $"{text2},{text3},{text4},{text5},{((stiVariable2.Type != null) ? stiVariable2.Type.ToString() : string.Empty)},{text},{stiVariable2.ReadOnly},{stiVariable2.InitBy == StiVariableInitBy.Expression},{stiVariable2.RequestFromUser},{stiVariable2.Inherited}";
			}
			if (string.IsNullOrWhiteSpace(stiVariable2.Key) && !stiVariable2.AllowUseAsSqlParameter && stiVariable2.Selection == StiSelectionMode.FromVariable)
			{
				return $"{text2},{text3},{text4},{text5},{((stiVariable2.Type != null) ? stiVariable2.Type.ToString() : string.Empty)},{text},{stiVariable2.ReadOnly},{stiVariable2.InitBy == StiVariableInitBy.Expression},{stiVariable2.RequestFromUser},{stiVariable2.Inherited},{((stiVariable2.DialogInfo != null && !stiVariable2.DialogInfo.IsDefault) ? StiDialogInfoConverter.ConvertDialogInfoToString(stiVariable2.DialogInfo) : string.Empty)}";
			}
			if (!stiVariable2.AllowUseAsSqlParameter && stiVariable2.Selection == StiSelectionMode.FromVariable)
			{
				return $"{text2},{text3},{text4},{text5},{((stiVariable2.Type != null) ? stiVariable2.Type.ToString() : string.Empty)},{text},{stiVariable2.ReadOnly},{stiVariable2.InitBy == StiVariableInitBy.Expression},{stiVariable2.RequestFromUser},{stiVariable2.Inherited},{((stiVariable2.DialogInfo != null && !stiVariable2.DialogInfo.IsDefault) ? StiDialogInfoConverter.ConvertDialogInfoToString(stiVariable2.DialogInfo) : string.Empty)},{((!string.IsNullOrEmpty(stiVariable2.Key)) ? XmlConvert.EncodeName(stiVariable2.Key) : string.Empty)}";
			}
			if (stiVariable2.Selection == StiSelectionMode.FromVariable)
			{
				return $"{text2},{text3},{text4},{text5},{((stiVariable2.Type != null) ? stiVariable2.Type.ToString() : string.Empty)},{text},{stiVariable2.ReadOnly},{stiVariable2.InitBy == StiVariableInitBy.Expression},{stiVariable2.RequestFromUser},{stiVariable2.Inherited},{((stiVariable2.DialogInfo != null && !stiVariable2.DialogInfo.IsDefault) ? StiDialogInfoConverter.ConvertDialogInfoToString(stiVariable2.DialogInfo) : string.Empty)},{((!string.IsNullOrEmpty(stiVariable2.Key)) ? XmlConvert.EncodeName(stiVariable2.Key) : string.Empty)},{stiVariable2.AllowUseAsSqlParameter}";
			}
			return $"{text2},{text3},{text4},{text5},{((stiVariable2.Type != null) ? stiVariable2.Type.ToString() : string.Empty)},{text},{stiVariable2.ReadOnly},{stiVariable2.InitBy == StiVariableInitBy.Expression},{stiVariable2.RequestFromUser},{stiVariable2.Inherited},{((stiVariable2.DialogInfo != null && !stiVariable2.DialogInfo.IsDefault) ? StiDialogInfoConverter.ConvertDialogInfoToString(stiVariable2.DialogInfo) : string.Empty)},{((!string.IsNullOrEmpty(stiVariable2.Key)) ? XmlConvert.EncodeName(stiVariable2.Key) : string.Empty)},{stiVariable2.AllowUseAsSqlParameter},{stiVariable2.Selection}";
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
			string[] array = text.Split(new char[1] { ',' });
			if (array.Length == 1)
			{
				return new StiVariable(array[0]);
			}
			if (array.Length == 2)
			{
				return new StiVariable(array[0])
				{
					Inherited = true
				};
			}
			if (array.Length == 5)
			{
				string text2 = XmlConvert.DecodeName(array[3]);
				Type type = ParseType(array[2]);
				StiVariableInitBy initBy;
				try
				{
					StiVariable.GetValue(text2, type);
					initBy = StiVariableInitBy.Value;
				}
				catch
				{
					initBy = StiVariableInitBy.Expression;
				}
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[1]), type, text2, array[4] == "True", initBy);
			}
			if (array.Length == 6)
			{
				string text3 = XmlConvert.DecodeName(array[4]);
				Type type2 = ParseType(array[3]);
				StiVariableInitBy initBy2;
				try
				{
					StiVariable.GetValue(text3, type2);
					initBy2 = StiVariableInitBy.Value;
				}
				catch
				{
					initBy2 = StiVariableInitBy.Expression;
				}
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), type2, text3, array[5] == "True", initBy2);
			}
			if (array.Length == 7)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), ParseType(array[3]), XmlConvert.DecodeName(array[4]), array[5] == "True", (array[6] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value);
			}
			if (array.Length == 8)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), ParseType(array[3]), XmlConvert.DecodeName(array[4]), array[5] == "True", (array[6] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value)
				{
					Inherited = (array[7] == "True")
				};
			}
			if (array.Length == 9)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True");
			}
			if (array.Length == 10)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True")
				{
					Inherited = (array[9] == "True")
				};
			}
			if (array.Length == 11)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True", StiDialogInfoConverter.ConvertFromStringToDialogInfo(array[10]))
				{
					Inherited = (array[9] == "True")
				};
			}
			if (array.Length == 12)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True", StiDialogInfoConverter.ConvertFromStringToDialogInfo(array[10]), string.IsNullOrWhiteSpace(array[11]) ? null : XmlConvert.DecodeName(array[11]))
				{
					Inherited = (array[9] == "True")
				};
			}
			if (array.Length == 13)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True", StiDialogInfoConverter.ConvertFromStringToDialogInfo(array[10]), string.IsNullOrWhiteSpace(array[11]) ? null : XmlConvert.DecodeName(array[11]), array[12] == "True")
				{
					Inherited = (array[9] == "True")
				};
			}
			if (array.Length == 14)
			{
				return new StiVariable(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), ParseType(array[4]), XmlConvert.DecodeName(array[5]), array[6] == "True", (array[7] == "True") ? StiVariableInitBy.Expression : StiVariableInitBy.Value, array[8] == "True", StiDialogInfoConverter.ConvertFromStringToDialogInfo(array[10]), string.IsNullOrWhiteSpace(array[11]) ? null : XmlConvert.DecodeName(array[11]), array[12] == "True", (StiSelectionMode)Enum.Parse(typeof(StiSelectionMode), array[13]))
				{
					Inherited = (array[9] == "True")
				};
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	private Type ParseType(string str)
	{
		if (!string.IsNullOrWhiteSpace(str) && !(str == "null"))
		{
			return StiTypeFinder.GetType(str);
		}
		return null;
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
