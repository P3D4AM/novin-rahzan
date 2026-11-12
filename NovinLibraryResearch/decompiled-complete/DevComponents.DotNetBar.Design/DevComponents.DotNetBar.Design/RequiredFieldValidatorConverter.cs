using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design;

public class RequiredFieldValidatorConverter : ExpandableObjectConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if ((object)destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if ((object)destinationType == typeof(InstanceDescriptor) && value is RequiredFieldValidator)
		{
			RequiredFieldValidator requiredFieldValidator = (RequiredFieldValidator)value;
			Type[] array = null;
			MemberInfo memberInfo = null;
			object[] array2 = null;
			if (string.IsNullOrEmpty(requiredFieldValidator.OptionalValidationGroup))
			{
				array = new Type[1] { typeof(string) };
				memberInfo = typeof(RequiredFieldValidator).GetConstructor(array);
				array2 = new object[1] { requiredFieldValidator.ErrorMessage };
			}
			else
			{
				array = new Type[2]
				{
					typeof(string),
					typeof(string)
				};
				memberInfo = typeof(RequiredFieldValidator).GetConstructor(array);
				array2 = new object[2] { requiredFieldValidator.ErrorMessage, requiredFieldValidator.OptionalValidationGroup };
			}
			if ((object)memberInfo != null)
			{
				return new InstanceDescriptor(memberInfo, array2);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
