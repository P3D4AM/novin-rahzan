using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Assessment.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager resourceManager = new ResourceManager("Microsoft.SqlServer.Management.Assessment.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string ErrorEmtyString => ResourceManager.GetString("ErrorEmtyString", resourceCulture);

	internal static string ErrorCheckTargetChange => ResourceManager.GetString("ErrorCheckTargetChange", resourceCulture);

	internal static string ErrorInvalidCheckId => ResourceManager.GetString("ErrorInvalidCheckId", resourceCulture);

	internal static string ErrorEvalArity => ResourceManager.GetString("ErrorEvalArity", resourceCulture);

	internal static string ErrorEvalMissingData => ResourceManager.GetString("ErrorEvalMissingData", resourceCulture);

	internal static string ErrorEvalOverflow => ResourceManager.GetString("ErrorEvalOverflow", resourceCulture);

	internal static string ErrorEvalStringFormat => ResourceManager.GetString("ErrorEvalStringFormat", resourceCulture);

	internal static string ErrorEvalTypeMismatch => ResourceManager.GetString("ErrorEvalTypeMismatch", resourceCulture);

	internal static string ErrorIndexOutOfRange => ResourceManager.GetString("ErrorIndexOutOfRange", resourceCulture);

	internal static string ErrorJsonMissingProperty => ResourceManager.GetString("ErrorJsonMissingProperty", resourceCulture);

	internal static string ErrorJsonOperatorInstantiation => ResourceManager.GetString("ErrorJsonOperatorInstantiation", resourceCulture);

	internal static string ErrorJsonReader => ResourceManager.GetString("ErrorJsonReader", resourceCulture);

	internal static string ErrorJsonSerializationNotSupported => ResourceManager.GetString("ErrorJsonSerializationNotSupported", resourceCulture);

	internal static string ErrorJsonSingularRangeInclusive => ResourceManager.GetString("ErrorJsonSingularRangeInclusive", resourceCulture);

	internal static string ErrorJsonUnexpectedEndOfDocument => ResourceManager.GetString("ErrorJsonUnexpectedEndOfDocument", resourceCulture);

	internal static string ErrorJsonUnexpectedToken => ResourceManager.GetString("ErrorJsonUnexpectedToken", resourceCulture);

	internal static string ErrorJsonUnsupportedOperation => ResourceManager.GetString("ErrorJsonUnsupportedOperation", resourceCulture);

	internal static string ErrorJsonWrongToken => ResourceManager.GetString("ErrorJsonWrongToken", resourceCulture);

	internal static string ErrorJsonWrongValue => ResourceManager.GetString("ErrorJsonWrongValue", resourceCulture);

	internal static string ErrorValueEndsWithWhiteSpace => ResourceManager.GetString("ErrorValueEndsWithWhiteSpace", resourceCulture);

	internal static string ErrorValueIsAWhiteSpaceString => ResourceManager.GetString("ErrorValueIsAWhiteSpaceString", resourceCulture);

	internal static string ErrorValueIsAnEmptyString => ResourceManager.GetString("ErrorValueIsAnEmptyString", resourceCulture);

	internal static string ErrorValueStartsWithWhiteSpace => ResourceManager.GetString("ErrorValueStartsWithWhiteSpace", resourceCulture);

	internal static string ErrorEvalWrongProbeParameter => ResourceManager.GetString("ErrorEvalWrongProbeParameter", resourceCulture);

	internal static string ErrorStringOrObjectExpected => ResourceManager.GetString("ErrorStringOrObjectExpected", resourceCulture);

	internal static string ErrorStringArrayOrObjectExpected => ResourceManager.GetString("ErrorStringOrObjectExpected", resourceCulture);

	internal static string ErrorConverterCannotWriteValue => ResourceManager.GetString("ErrorConverterCannotWriteValue", resourceCulture);

	internal static string ErrorConverterCannotReadJSONValue => ResourceManager.GetString("ErrorConverterCannotReadJSONValue", resourceCulture);

	internal static string ErrorVersionFieldIsRequired => ResourceManager.GetString("ErrorVersionFieldIsRequired", resourceCulture);

	internal static string ErrorNameFieldIsRequired => ResourceManager.GetString("ErrorNameFieldIsRequired", resourceCulture);

	internal static string ErrorKeyNotFound => ResourceManager.GetString("ErrorKeyNotFound", resourceCulture);

	internal static string ErrorImplementationWasNotFound => ResourceManager.GetString("ErrorImplementationWasNotFound", resourceCulture);

	internal static string ErrorJsonSingularLeftRangeInclusive => ResourceManager.GetString("ErrorJsonSingularLeftRangeInclusive", resourceCulture);

	internal static string ErrorJsonSingularRightRangeInclusive => ResourceManager.GetString("ErrorJsonSingularRightRangeInclusive", resourceCulture);

	internal static string ErrorRecursionDetected => ResourceManager.GetString("ErrorRecursionDetected", resourceCulture);

	internal static string ErrorValueCannotBeNullOrWhiteSpace => ResourceManager.GetString("ErrorValueCannotBeNullOrWhiteSpace", resourceCulture);

	internal static string ErrorMultuallyExclusiveJsonProperties => ResourceManager.GetString("ErrorMultuallyExclusiveJsonProperties", resourceCulture);

	internal static string ErrorMissingJsonProperty => ResourceManager.GetString("ErrorMissingJsonProperty", resourceCulture);

	internal static string ErrorTargetFilerInDefinition => ResourceManager.GetString("ErrorTargetFilerInDefinition", resourceCulture);

	internal static string ErrorUnrecognizedChecksItemType => ResourceManager.GetString("ErrorUnrecognizedChecksItemType", resourceCulture);

	internal static string ErrorCheckModificationSelectorResuired => ResourceManager.GetString("Error_CheckModificationSelectorResuired", resourceCulture);

	internal static string ErrorEvalOther => ResourceManager.GetString("ErrorEvalOther", resourceCulture);

	internal static string UnsupportedTargetCheckMessage => ResourceManager.GetString("UnsupportedTargetCheckMessage", resourceCulture);

	internal static string UnsupportedTargetCheckDescription => ResourceManager.GetString("UnsupportedTargetCheckDescription", resourceCulture);

	internal static string UnsupportedTargetCheckDisplayName => ResourceManager.GetString("UnsupportedTargetCheckDisplayName", resourceCulture);

	internal static string ErrorFailedObtainProbeData => ResourceManager.GetString("ErrorFailedObtainProbeData", resourceCulture);

	internal static string ErrorCheckHasNoImplementation => ResourceManager.GetString("ErrorCheckHasNoImplementation", resourceCulture);

	internal static string ErrorAccessTarget => ResourceManager.GetString("ErrorAccessTarget", resourceCulture);

	internal static string StringCheckId => ResourceManager.GetString("StringCheckId", resourceCulture);

	internal static string StringTarget => ResourceManager.GetString("StringTarget", resourceCulture);

	internal static string ErrorConditionEvaluationFailed => ResourceManager.GetString("ErrorConditionEvaluationFailed", resourceCulture);

	internal static string ErrorJsonLocationWithLinePosition => ResourceManager.GetString("ErrorJsonLocationWithLinePosition", resourceCulture);

	internal static string ErrorProbeInitialize => ResourceManager.GetString("ErrorProbeInitialize", resourceCulture);

	internal static string ErrorLoadingAssembly => ResourceManager.GetString("ErrorLoadingAssembly", resourceCulture);

	internal static string NotEnoughPermissionsToRunAssessmentMessage => ResourceManager.GetString("NotEnoughPermissionsToRunAssessmentMessage", resourceCulture);

	internal static string ErrorProbeRequirements => ResourceManager.GetString("ErrorProbeRequirements", resourceCulture);

	internal static string GroupingColumnNameCannotBeNull => ResourceManager.GetString("GroupingColumnNameCannotBeNull", resourceCulture);

	internal static string Unsupported_PowerShell_parameter_type => ResourceManager.GetString("Unsupported_PowerShell_parameter_type", resourceCulture);

	internal static string WarningCheckSkipped1 => ResourceManager.GetString("WarningCheckSkipped1", resourceCulture);

	internal static string WarningCheckSkipped2 => ResourceManager.GetString("WarningCheckSkipped2", resourceCulture);

	internal static string WarningCheckSkipped3 => ResourceManager.GetString("WarningCheckSkipped3", resourceCulture);

	internal static string WarningCheckSkipped3Plus => ResourceManager.GetString("WarningCheckSkipped3Plus", resourceCulture);

	internal Resources()
	{
	}
}
