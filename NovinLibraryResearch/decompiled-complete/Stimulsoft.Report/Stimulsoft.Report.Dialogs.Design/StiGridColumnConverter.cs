using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiGridColumnConverter : TypeConverter
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
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiGridColumn stiGridColumn = (StiGridColumn)value;
			Type[] types = new Type[6]
			{
				typeof(string),
				typeof(bool),
				typeof(int),
				typeof(HorizontalAlignment),
				typeof(string),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiGridColumn).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[6] { stiGridColumn.DataTextField, stiGridColumn.Visible, stiGridColumn.Width, stiGridColumn.Alignment, stiGridColumn.HeaderText, stiGridColumn.NullText };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		else if (destinationType == typeof(string) && value is StiGridColumn stiGridColumn2)
		{
			return $"{stiGridColumn2.DataTextField},{stiGridColumn2.Visible},{stiGridColumn2.Width},{stiGridColumn2.Alignment},{stiGridColumn2.HeaderText},{stiGridColumn2.NullText}";
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (value is string text)
		{
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			return new StiGridColumn(array[0], array[1] == "True", int.Parse(array[2]), (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), array[3]), array[4], array[4]);
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
