using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.ComponentModel.Annotations;

namespace System;

internal static class SR
{
	private static ResourceManager s_resourceManager;

	private static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(ResourceType));

	internal static Type ResourceType { get; } = typeof(FxResources.System.ComponentModel.Annotations.SR);


	internal static string ArgumentIsNullOrWhitespace => GetResourceString("ArgumentIsNullOrWhitespace", null);

	internal static string AttributeStore_Unknown_Property => GetResourceString("AttributeStore_Unknown_Property", null);

	internal static string Common_PropertyNotFound => GetResourceString("Common_PropertyNotFound", null);

	internal static string CompareAttribute_MustMatch => GetResourceString("CompareAttribute_MustMatch", null);

	internal static string CompareAttribute_UnknownProperty => GetResourceString("CompareAttribute_UnknownProperty", null);

	internal static string CreditCardAttribute_Invalid => GetResourceString("CreditCardAttribute_Invalid", null);

	internal static string CustomValidationAttribute_Method_Must_Return_ValidationResult => GetResourceString("CustomValidationAttribute_Method_Must_Return_ValidationResult", null);

	internal static string CustomValidationAttribute_Method_Not_Found => GetResourceString("CustomValidationAttribute_Method_Not_Found", null);

	internal static string CustomValidationAttribute_Method_Required => GetResourceString("CustomValidationAttribute_Method_Required", null);

	internal static string CustomValidationAttribute_Method_Signature => GetResourceString("CustomValidationAttribute_Method_Signature", null);

	internal static string CustomValidationAttribute_Type_Conversion_Failed => GetResourceString("CustomValidationAttribute_Type_Conversion_Failed", null);

	internal static string CustomValidationAttribute_Type_Must_Be_Public => GetResourceString("CustomValidationAttribute_Type_Must_Be_Public", null);

	internal static string CustomValidationAttribute_ValidationError => GetResourceString("CustomValidationAttribute_ValidationError", null);

	internal static string CustomValidationAttribute_ValidatorType_Required => GetResourceString("CustomValidationAttribute_ValidatorType_Required", null);

	internal static string DataTypeAttribute_EmptyDataTypeString => GetResourceString("DataTypeAttribute_EmptyDataTypeString", null);

	internal static string DisplayAttribute_PropertyNotSet => GetResourceString("DisplayAttribute_PropertyNotSet", null);

	internal static string EmailAddressAttribute_Invalid => GetResourceString("EmailAddressAttribute_Invalid", null);

	internal static string EnumDataTypeAttribute_TypeCannotBeNull => GetResourceString("EnumDataTypeAttribute_TypeCannotBeNull", null);

	internal static string EnumDataTypeAttribute_TypeNeedsToBeAnEnum => GetResourceString("EnumDataTypeAttribute_TypeNeedsToBeAnEnum", null);

	internal static string FileExtensionsAttribute_Invalid => GetResourceString("FileExtensionsAttribute_Invalid", null);

	internal static string LocalizableString_LocalizationFailed => GetResourceString("LocalizableString_LocalizationFailed", null);

	internal static string MaxLengthAttribute_InvalidMaxLength => GetResourceString("MaxLengthAttribute_InvalidMaxLength", null);

	internal static string MaxLengthAttribute_ValidationError => GetResourceString("MaxLengthAttribute_ValidationError", null);

	internal static string MinLengthAttribute_InvalidMinLength => GetResourceString("MinLengthAttribute_InvalidMinLength", null);

	internal static string MinLengthAttribute_ValidationError => GetResourceString("MinLengthAttribute_ValidationError", null);

	internal static string LengthAttribute_InvalidValueType => GetResourceString("LengthAttribute_InvalidValueType", null);

	internal static string PhoneAttribute_Invalid => GetResourceString("PhoneAttribute_Invalid", null);

	internal static string RangeAttribute_ArbitraryTypeNotIComparable => GetResourceString("RangeAttribute_ArbitraryTypeNotIComparable", null);

	internal static string RangeAttribute_MinGreaterThanMax => GetResourceString("RangeAttribute_MinGreaterThanMax", null);

	internal static string RangeAttribute_Must_Set_Min_And_Max => GetResourceString("RangeAttribute_Must_Set_Min_And_Max", null);

	internal static string RangeAttribute_Must_Set_Operand_Type => GetResourceString("RangeAttribute_Must_Set_Operand_Type", null);

	internal static string RangeAttribute_ValidationError => GetResourceString("RangeAttribute_ValidationError", null);

	internal static string RegexAttribute_ValidationError => GetResourceString("RegexAttribute_ValidationError", null);

	internal static string RegularExpressionAttribute_Empty_Pattern => GetResourceString("RegularExpressionAttribute_Empty_Pattern", null);

	internal static string RequiredAttribute_ValidationError => GetResourceString("RequiredAttribute_ValidationError", null);

	internal static string StringLengthAttribute_InvalidMaxLength => GetResourceString("StringLengthAttribute_InvalidMaxLength", null);

	internal static string StringLengthAttribute_ValidationError => GetResourceString("StringLengthAttribute_ValidationError", null);

	internal static string StringLengthAttribute_ValidationErrorIncludingMinimum => GetResourceString("StringLengthAttribute_ValidationErrorIncludingMinimum", null);

	internal static string UIHintImplementation_ControlParameterKeyIsNotAString => GetResourceString("UIHintImplementation_ControlParameterKeyIsNotAString", null);

	internal static string UIHintImplementation_ControlParameterKeyIsNull => GetResourceString("UIHintImplementation_ControlParameterKeyIsNull", null);

	internal static string UIHintImplementation_ControlParameterKeyOccursMoreThanOnce => GetResourceString("UIHintImplementation_ControlParameterKeyOccursMoreThanOnce", null);

	internal static string UIHintImplementation_NeedEvenNumberOfControlParameters => GetResourceString("UIHintImplementation_NeedEvenNumberOfControlParameters", null);

	internal static string UrlAttribute_Invalid => GetResourceString("UrlAttribute_Invalid", null);

	internal static string ValidationAttribute_Cannot_Set_ErrorMessage_And_Resource => GetResourceString("ValidationAttribute_Cannot_Set_ErrorMessage_And_Resource", null);

	internal static string ValidationAttribute_IsValid_NotImplemented => GetResourceString("ValidationAttribute_IsValid_NotImplemented", null);

	internal static string ValidationAttribute_NeedBothResourceTypeAndResourceName => GetResourceString("ValidationAttribute_NeedBothResourceTypeAndResourceName", null);

	internal static string ValidationAttribute_ResourcePropertyNotStringType => GetResourceString("ValidationAttribute_ResourcePropertyNotStringType", null);

	internal static string ValidationAttribute_ResourceTypeDoesNotHaveProperty => GetResourceString("ValidationAttribute_ResourceTypeDoesNotHaveProperty", null);

	internal static string ValidationAttribute_ValidationError => GetResourceString("ValidationAttribute_ValidationError", null);

	internal static string Validator_InstanceMustMatchValidationContextInstance => GetResourceString("Validator_InstanceMustMatchValidationContextInstance", null);

	internal static string Validator_Property_Value_Wrong_Type => GetResourceString("Validator_Property_Value_Wrong_Type", null);

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool UsingResourceKeys()
	{
		return false;
	}

	internal static string GetResourceString(string resourceKey, string defaultString)
	{
		string text = null;
		try
		{
			text = ResourceManager.GetString(resourceKey);
		}
		catch (MissingManifestResourceException)
		{
		}
		if (defaultString != null && resourceKey.Equals(text, StringComparison.Ordinal))
		{
			return defaultString;
		}
		return text;
	}

	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}
		return resourceFormat;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(resourceFormat, p1, p2, p3);
	}
}
