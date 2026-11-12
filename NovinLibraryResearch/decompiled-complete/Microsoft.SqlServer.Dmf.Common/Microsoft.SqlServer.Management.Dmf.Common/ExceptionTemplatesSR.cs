using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Dmf.Common;

[CompilerGenerated]
internal class ExceptionTemplatesSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(ExceptionTemplatesSR));

		private static CultureInfo _culture = null;

		public const string AdapterAlreadyExists = "AdapterAlreadyExists";

		public const string AdapterHasTooManyArguments = "AdapterHasTooManyArguments";

		public const string AssemblyAlreadyRegistered = "AssemblyAlreadyRegistered";

		public const string BadExpressionTree = "BadExpressionTree";

		public const string CannotConvertToType = "CannotConvertToType";

		public const string CannotMoveToElement = "CannotMoveToElement";

		public const string ConfigValueMismatch = "ConfigValueMismatch";

		public const string ConversionNotSupported = "ConversionNotSupported";

		public const string ExpressionNodeNotConfigurable = "ExpressionNodeNotConfigurable";

		public const string ExpressionNodeNotConfigurableGeneric = "ExpressionNodeNotConfigurableGeneric";

		public const string ExpressionNodeNotConfigurableOperators = "ExpressionNodeNotConfigurableOperators";

		public const string ExpressionTypeMistmatch = "ExpressionTypeMistmatch";

		public const string FailedOperation = "FailedOperation";

		public const string FunctionNotASmoObject = "FunctionNotASmoObject";

		public const string FunctionWrongArgumentsNumber = "FunctionWrongArgumentsNumber";

		public const string FunctionWrongArgumentType = "FunctionWrongArgumentType";

		public const string FunctionWrongReturnType = "FunctionWrongReturnType";

		public const string InvalidInOperator = "InvalidInOperator";

		public const string InvalidOperand = "InvalidOperand";

		public const string MissingProperty = "MissingProperty";

		public const string MissingTypeFacetAssociation = "MissingTypeFacetAssociation";

		public const string NonConfigurableReadOnlyProperty = "NonConfigurableReadOnlyProperty";

		public const string NonRetrievableProperty = "NonRetrievableProperty";

		public const string NoTargetSetEnabled = "NoTargetSetEnabled";

		public const string NotSupported = "NotSupported";

		public const string NullFacet = "NullFacet";

		public const string ObjectAlreadyExists = "ObjectAlreadyExists";

		public const string ObjectDoesntExist = "ObjectDoesntExist";

		public const string OperatorNotApplicable = "OperatorNotApplicable";

		public const string StringPropertyTooLong = "StringPropertyTooLong";

		public const string TargetSetCountMismatch = "TargetSetCountMismatch";

		public const string UnexpectedElement = "UnexpectedElement";

		public const string UnexpectedName = "UnexpectedName";

		public const string UnexpectedType = "UnexpectedType";

		public const string UnsupportedConstantType = "UnsupportedConstantType";

		public const string UnsupportedObjectType = "UnsupportedObjectType";

		public const string UnsupportedTargetSetForFacet = "UnsupportedTargetSetForFacet";

		public const string ValidationFailed = "ValidationFailed";

		public static CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				_culture = value;
			}
		}

		private Keys()
		{
		}

		public static string GetString(string key)
		{
			return resourceManager.GetString(key, _culture);
		}

		public static string GetString(string key, object arg0)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0);
		}

		public static string GetString(string key, object arg0, object arg1)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1);
		}

		public static string GetString(string key, object arg0, object arg1, object arg2)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2);
		}

		public static string GetString(string key, object arg0, object arg1, object arg2, object arg3)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2, arg3);
		}
	}

	public static CultureInfo Culture
	{
		get
		{
			return Keys.Culture;
		}
		set
		{
			Keys.Culture = value;
		}
	}

	public static string CannotMoveToElement => Keys.GetString("CannotMoveToElement");

	public static string ExpressionNodeNotConfigurableGeneric => Keys.GetString("ExpressionNodeNotConfigurableGeneric");

	protected ExceptionTemplatesSR()
	{
	}

	public static string AdapterAlreadyExists(string @interface, string @object)
	{
		return Keys.GetString("AdapterAlreadyExists", @interface, @object);
	}

	public static string AdapterHasTooManyArguments(string name)
	{
		return Keys.GetString("AdapterHasTooManyArguments", name);
	}

	public static string AssemblyAlreadyRegistered(string name)
	{
		return Keys.GetString("AssemblyAlreadyRegistered", name);
	}

	public static string BadExpressionTree(string reason)
	{
		return Keys.GetString("BadExpressionTree", reason);
	}

	public static string CannotConvertToType(string inputString, string typeName)
	{
		return Keys.GetString("CannotConvertToType", inputString, typeName);
	}

	public static string ConfigValueMismatch(string propertyName, string configValue, string runValue)
	{
		return Keys.GetString("ConfigValueMismatch", propertyName, configValue, runValue);
	}

	public static string ConversionNotSupported(string obj, string type)
	{
		return Keys.GetString("ConversionNotSupported", obj, type);
	}

	public static string ExpressionNodeNotConfigurable(string subtype)
	{
		return Keys.GetString("ExpressionNodeNotConfigurable", subtype);
	}

	public static string ExpressionNodeNotConfigurableOperators(string propertyName, string expression)
	{
		return Keys.GetString("ExpressionNodeNotConfigurableOperators", propertyName, expression);
	}

	public static string ExpressionTypeMistmatch(string typeLeft, string typeRight)
	{
		return Keys.GetString("ExpressionTypeMistmatch", typeLeft, typeRight);
	}

	public static string FailedOperation(string opName, string objType, string objName)
	{
		return Keys.GetString("FailedOperation", opName, objType, objName);
	}

	public static string FunctionNotASmoObject(string functionName, string targetType)
	{
		return Keys.GetString("FunctionNotASmoObject", functionName, targetType);
	}

	public static string FunctionWrongArgumentsNumber(string function, int argsReceived, int argsExpected)
	{
		return Keys.GetString("FunctionWrongArgumentsNumber", function, argsReceived, argsExpected);
	}

	public static string FunctionWrongArgumentType(string function, string typeReceived, string typeExpected)
	{
		return Keys.GetString("FunctionWrongArgumentType", function, typeReceived, typeExpected);
	}

	public static string FunctionWrongReturnType(string function, string typeReceived, string typeExpected)
	{
		return Keys.GetString("FunctionWrongReturnType", function, typeReceived, typeExpected);
	}

	public static string InvalidInOperator(string @operator)
	{
		return Keys.GetString("InvalidInOperator", @operator);
	}

	public static string InvalidOperand(string type, string operand)
	{
		return Keys.GetString("InvalidOperand", type, operand);
	}

	public static string MissingProperty(string name)
	{
		return Keys.GetString("MissingProperty", name);
	}

	public static string MissingTypeFacetAssociation(string typeName, string facet)
	{
		return Keys.GetString("MissingTypeFacetAssociation", typeName, facet);
	}

	public static string NonConfigurableReadOnlyProperty(string propertyName)
	{
		return Keys.GetString("NonConfigurableReadOnlyProperty", propertyName);
	}

	public static string NonRetrievableProperty(string name)
	{
		return Keys.GetString("NonRetrievableProperty", name);
	}

	public static string NoTargetSetEnabled(string objectSetName)
	{
		return Keys.GetString("NoTargetSetEnabled", objectSetName);
	}

	public static string NotSupported(string host_type, string host_name, string object_type, string object_name)
	{
		return Keys.GetString("NotSupported", host_type, host_name, object_type, object_name);
	}

	public static string NullFacet(string facet)
	{
		return Keys.GetString("NullFacet", facet);
	}

	public static string ObjectAlreadyExists(string @object, string name)
	{
		return Keys.GetString("ObjectAlreadyExists", @object, name);
	}

	public static string ObjectDoesntExist(string @object, string name)
	{
		return Keys.GetString("ObjectDoesntExist", @object, name);
	}

	public static string OperatorNotApplicable(string @operator, string type)
	{
		return Keys.GetString("OperatorNotApplicable", @operator, type);
	}

	public static string StringPropertyTooLong(string propertyName, int maxLength, int currentLength)
	{
		return Keys.GetString("StringPropertyTooLong", propertyName, maxLength, currentLength);
	}

	public static string TargetSetCountMismatch(string objectSetName, string facetName)
	{
		return Keys.GetString("TargetSetCountMismatch", objectSetName, facetName);
	}

	public static string UnexpectedElement(string nameRead, string nameExpected)
	{
		return Keys.GetString("UnexpectedElement", nameRead, nameExpected);
	}

	public static string UnexpectedName(string typeRead, string nameRead, string typeExpected, string nameExpected)
	{
		return Keys.GetString("UnexpectedName", typeRead, nameRead, typeExpected, nameExpected);
	}

	public static string UnexpectedType(string typeRead, string nameRead, string typeExpected)
	{
		return Keys.GetString("UnexpectedType", typeRead, nameRead, typeExpected);
	}

	public static string UnsupportedConstantType(string node, string typeName)
	{
		return Keys.GetString("UnsupportedConstantType", node, typeName);
	}

	public static string UnsupportedObjectType(string type, string host)
	{
		return Keys.GetString("UnsupportedObjectType", type, host);
	}

	public static string UnsupportedTargetSetForFacet(string targetSetSkeleton, string objectSetName, string facetName)
	{
		return Keys.GetString("UnsupportedTargetSetForFacet", targetSetSkeleton, objectSetName, facetName);
	}

	public static string ValidationFailed(string objectType, string objectName)
	{
		return Keys.GetString("ValidationFailed", objectType, objectName);
	}
}
