using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.Dmf;

[CompilerGenerated]
internal class ExceptionTemplatesSR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(ExceptionTemplatesSR));

		private static CultureInfo _culture = null;

		public const string NoSuchCollection = "NoSuchCollection";

		public const string UnsupportedObjectType = "UnsupportedObjectType";

		public const string UnknownEnumeration = "UnknownEnumeration";

		public const string InvalidArgument = "InvalidArgument";

		public const string ObjectCannotBeCreated = "ObjectCannotBeCreated";

		public const string TooManyURNsReturned = "TooManyURNsReturned";

		public const string RootConditionFailed = "RootConditionFailed";

		public const string UnsupportedConnectionType = "UnsupportedConnectionType";

		public const string Policy = "Policy";

		public const string Condition = "Condition";

		public const string ObjectSet = "ObjectSet";

		public const string Category = "Category";

		public const string RootCondition = "RootCondition";

		public const string EvaluationMode = "EvaluationMode";

		public const string Schedule = "Schedule";

		public const string Enabled = "Enabled";

		public const string TargetSet = "TargetSet";

		public const string ManagementFacet = "ManagementFacet";

		public const string Property = "Property";

		public const string ObjectType = "ObjectType";

		public const string PolicyStore = "PolicyStore";

		public const string CategorySubscription = "CategorySubscription";

		public const string NotSupported = "NotSupported";

		public const string ObjectAlreadyExists = "ObjectAlreadyExists";

		public const string ObjectDoesntExist = "ObjectDoesntExist";

		public const string ArgumentNullOrEmpty = "ArgumentNullOrEmpty";

		public const string FailedOperation = "FailedOperation";

		public const string ConsistencyRepair = "ConsistencyRepair";

		public const string EraseSystemHealthPhantomRecords = "EraseSystemHealthPhantomRecords";

		public const string ValuePositive = "ValuePositive";

		public const string PurgeHealthState = "PurgeHealthState";

		public const string MarkSystemObject = "MarkSystemObject";

		public const string CannotMarkSystemObject = "CannotMarkSystemObject";

		public const string DefaultCategory = "DefaultCategory";

		public const string EvaluationModeNoneDescription = "EvaluationModeNoneDescription";

		public const string EvaluationModeEnforceDescription = "EvaluationModeEnforceDescription";

		public const string EvaluationModeCoCDescription = "EvaluationModeCoCDescription";

		public const string EvaluationModeCoSDescription = "EvaluationModeCoSDescription";

		public const string AdditionalHelpText = "AdditionalHelpText";

		public const string AdditionalHelpLink = "AdditionalHelpLink";

		public const string CheckMode = "CheckMode";

		public const string ConfigureMode = "ConfigureMode";

		public const string CheckSqlScriptAsProxyMode = "CheckSqlScriptAsProxyMode";

		public const string AdapterAlreadyExists = "AdapterAlreadyExists";

		public const string AdapterHasTooManyArguments = "AdapterHasTooManyArguments";

		public const string AssemblyAlreadyRegistered = "AssemblyAlreadyRegistered";

		public const string FailedMethodCall = "FailedMethodCall";

		public const string ConfigValueMismatch = "ConfigValueMismatch";

		public const string OperatorNotApplicable = "OperatorNotApplicable";

		public const string FunctionWrongArgumentsNumber = "FunctionWrongArgumentsNumber";

		public const string FunctionWrongArgumentType = "FunctionWrongArgumentType";

		public const string ExpressionTypeMistmatch = "ExpressionTypeMistmatch";

		public const string NullFacet = "NullFacet";

		public const string BadExpressionTree = "BadExpressionTree";

		public const string EvaluatingOperatorNone = "EvaluatingOperatorNone";

		public const string ExpressionNodeNotConfigurableGeneric = "ExpressionNodeNotConfigurableGeneric";

		public const string ExpressionNodeNotConfigurable = "ExpressionNodeNotConfigurable";

		public const string ExpressionNodeNotConfigurableOperators = "ExpressionNodeNotConfigurableOperators";

		public const string UnsupportedTargetFilter = "UnsupportedTargetFilter";

		public const string ParsingError = "ParsingError";

		public const string ParsingArgumentException = "ParsingArgumentException";

		public const string ParsingUnclosedBracketMsg = "ParsingUnclosedBracketMsg";

		public const string ParsingUnclosedParenthesisMsg = "ParsingUnclosedParenthesisMsg";

		public const string ParsingMissingQuotes = "ParsingMissingQuotes";

		public const string ParsingMissingElement = "ParsingMissingElement";

		public const string ParsingSyntaxError = "ParsingSyntaxError";

		public const string ParsingUnquotedDatePart = "ParsingUnquotedDatePart";

		public const string UnsupportedFunction = "UnsupportedFunction";

		public const string InvalidOperand = "InvalidOperand";

		public const string LeftOperand = "LeftOperand";

		public const string RightOperand = "RightOperand";

		public const string NonConfigurableReadOnlyProperty = "NonConfigurableReadOnlyProperty";

		public const string InvalidInOperator = "InvalidInOperator";

		public const string InvalidList = "InvalidList";

		public const string FunctionWrongReturnType = "FunctionWrongReturnType";

		public const string FunctionNotASmoObject = "FunctionNotASmoObject";

		public const string FunctionTooManyColumns = "FunctionTooManyColumns";

		public const string WmiException = "WmiException";

		public const string CannotProduceScript = "CannotProduceScript";

		public const string NoConfigureScriptForCompliantObject = "NoConfigureScriptForCompliantObject";

		public const string PropertyCannotBeSet = "PropertyCannotBeSet";

		public const string CannotMoveToElement = "CannotMoveToElement";

		public const string UnexpectedElement = "UnexpectedElement";

		public const string UnexpectedType = "UnexpectedType";

		public const string UnexpectedName = "UnexpectedName";

		public const string CannotConvertToType = "CannotConvertToType";

		public const string UnsupportedConstantType = "UnsupportedConstantType";

		public const string UnknownPolicy = "UnknownPolicy";

		public const string InvalidOrMissingXMLReader = "InvalidOrMissingXMLReader";

		public const string SinglePolicyDeserializationFailed = "SinglePolicyDeserializationFailed";

		public const string PolicyImportFileDoesNotHaveAnyPolicy = "PolicyImportFileDoesNotHaveAnyPolicy";

		public const string SinglePolicyExpected = "SinglePolicyExpected";

		public const string ReferencedObjectExpected = "ReferencedObjectExpected";

		public const string PolicyAndConditionNameMismatch = "PolicyAndConditionNameMismatch";

		public const string ObjectSetAndConditionFacetMismatch = "ObjectSetAndConditionFacetMismatch";

		public const string ConditionIsNull = "ConditionIsNull";

		public const string ObjectSetIsNull = "ObjectSetIsNull";

		public const string IncompatiblePolicyEvaluationMode = "IncompatiblePolicyEvaluationMode";

		public const string OnlyOneConditionExpression = "OnlyOneConditionExpression";

		public const string OnlyOneTarget = "OnlyOneTarget";

		public const string OnlyInSqlClr = "OnlyInSqlClr";

		public const string MissingPolicy = "MissingPolicy";

		public const string MissingProperty = "MissingProperty";

		public const string NonRetrievableProperty = "NonRetrievableProperty";

		public const string RollBack = "RollBack";

		public const string OnlyInMsdb = "OnlyInMsdb";

		public const string OnlyRoleMember = "OnlyRoleMember";

		public const string PolicyEvaluationFailed = "PolicyEvaluationFailed";

		public const string PolicyEvaluationFailedOnDelegate = "PolicyEvaluationFailedOnDelegate";

		public const string PolicyEvaluationFailedNoException = "PolicyEvaluationFailedNoException";

		public const string NullArgInPolicyEvaluation = "NullArgInPolicyEvaluation";

		public const string BadEventData = "BadEventData";

		public const string MissingTypeFacetAssociation = "MissingTypeFacetAssociation";

		public const string NoScriptInSqlClr = "NoScriptInSqlClr";

		public const string PolicyViolated = "PolicyViolated";

		public const string PolicyViolatedTarget = "PolicyViolatedTarget";

		public const string FailedToRetrieveImpersonationCookie = "FailedToRetrieveImpersonationCookie";

		public const string CantExecuteLongSqlScriptOn2000 = "CantExecuteLongSqlScriptOn2000";

		public const string PolicyCategoryAlreadySubscribed = "PolicyCategoryAlreadySubscribed";

		public const string PolicyCategoryNotSubscribed = "PolicyCategoryNotSubscribed";

		public const string Function = "Function";

		public const string Operator = "Operator";

		public const string ConversionNotSupported = "ConversionNotSupported";

		public const string TypeNotSupported = "TypeNotSupported";

		public const string PolicyWithNoFilters = "PolicyWithNoFilters";

		public const string ScheduleDoesntExist = "ScheduleDoesntExist";

		public const string ValidationFailed = "ValidationFailed";

		public const string MissingJobSchedule = "MissingJobSchedule";

		public const string StringPropertyTooLong = "StringPropertyTooLong";

		public const string InvalidHelpLinkMessage = "InvalidHelpLinkMessage";

		public const string ReferencedConditionsCannotContainScript = "ReferencedConditionsCannotContainScript";

		public const string FacetNotSupported = "FacetNotSupported";

		public const string UnknownTargetDatabase = "UnknownTargetDatabase";

		public const string UnsupportedTqe = "UnsupportedTqe";

		public const string UnsupportedKey = "UnsupportedKey";

		public const string UnsupportedTqeProp = "UnsupportedTqeProp";

		public const string EnumApplicablePolicies = "EnumApplicablePolicies";

		public const string CalculateHealthState = "CalculateHealthState";

		public const string GetAggregatedHealthState = "GetAggregatedHealthState";

		public const string UnsupportedCrudDependencyAction = "UnsupportedCrudDependencyAction";

		public const string InvalidUrnSkeleton = "InvalidUrnSkeleton";

		public const string TargetSetCountMismatch = "TargetSetCountMismatch";

		public const string UnsupportedTargetSetForFacet = "UnsupportedTargetSetForFacet";

		public const string NoTargetSetEnabled = "NoTargetSetEnabled";

		public const string ConditionCannotBeUsedForFiltering = "ConditionCannotBeUsedForFiltering";

		public const string CannotChangeFacet = "CannotChangeFacet";

		public const string CannotChangeFacetForObjectSet = "CannotChangeFacetForObjectSet";

		public const string ObjectSetAlreadyReferenced = "ObjectSetAlreadyReferenced";

		public const string VersionNotSupported = "VersionNotSupported";

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

		public static string GetString(string key, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2, arg3, arg4, arg5, arg6);
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

	public static string Policy => Keys.GetString("Policy");

	public static string Condition => Keys.GetString("Condition");

	public static string ObjectSet => Keys.GetString("ObjectSet");

	public static string Category => Keys.GetString("Category");

	public static string RootCondition => Keys.GetString("RootCondition");

	public static string EvaluationMode => Keys.GetString("EvaluationMode");

	public static string Schedule => Keys.GetString("Schedule");

	public static string Enabled => Keys.GetString("Enabled");

	public static string TargetSet => Keys.GetString("TargetSet");

	public static string ManagementFacet => Keys.GetString("ManagementFacet");

	public static string Property => Keys.GetString("Property");

	public static string ObjectType => Keys.GetString("ObjectType");

	public static string PolicyStore => Keys.GetString("PolicyStore");

	public static string CategorySubscription => Keys.GetString("CategorySubscription");

	public static string ConsistencyRepair => Keys.GetString("ConsistencyRepair");

	public static string EraseSystemHealthPhantomRecords => Keys.GetString("EraseSystemHealthPhantomRecords");

	public static string ValuePositive => Keys.GetString("ValuePositive");

	public static string PurgeHealthState => Keys.GetString("PurgeHealthState");

	public static string MarkSystemObject => Keys.GetString("MarkSystemObject");

	public static string DefaultCategory => Keys.GetString("DefaultCategory");

	public static string EvaluationModeNoneDescription => Keys.GetString("EvaluationModeNoneDescription");

	public static string EvaluationModeEnforceDescription => Keys.GetString("EvaluationModeEnforceDescription");

	public static string EvaluationModeCoCDescription => Keys.GetString("EvaluationModeCoCDescription");

	public static string EvaluationModeCoSDescription => Keys.GetString("EvaluationModeCoSDescription");

	public static string AdditionalHelpText => Keys.GetString("AdditionalHelpText");

	public static string AdditionalHelpLink => Keys.GetString("AdditionalHelpLink");

	public static string CheckMode => Keys.GetString("CheckMode");

	public static string ConfigureMode => Keys.GetString("ConfigureMode");

	public static string CheckSqlScriptAsProxyMode => Keys.GetString("CheckSqlScriptAsProxyMode");

	public static string EvaluatingOperatorNone => Keys.GetString("EvaluatingOperatorNone");

	public static string ExpressionNodeNotConfigurableGeneric => Keys.GetString("ExpressionNodeNotConfigurableGeneric");

	public static string ParsingUnclosedBracketMsg => Keys.GetString("ParsingUnclosedBracketMsg");

	public static string ParsingUnclosedParenthesisMsg => Keys.GetString("ParsingUnclosedParenthesisMsg");

	public static string ParsingMissingQuotes => Keys.GetString("ParsingMissingQuotes");

	public static string ParsingMissingElement => Keys.GetString("ParsingMissingElement");

	public static string ParsingSyntaxError => Keys.GetString("ParsingSyntaxError");

	public static string LeftOperand => Keys.GetString("LeftOperand");

	public static string RightOperand => Keys.GetString("RightOperand");

	public static string InvalidList => Keys.GetString("InvalidList");

	public static string FunctionTooManyColumns => Keys.GetString("FunctionTooManyColumns");

	public static string CannotProduceScript => Keys.GetString("CannotProduceScript");

	public static string NoConfigureScriptForCompliantObject => Keys.GetString("NoConfigureScriptForCompliantObject");

	public static string CannotMoveToElement => Keys.GetString("CannotMoveToElement");

	public static string UnknownPolicy => Keys.GetString("UnknownPolicy");

	public static string InvalidOrMissingXMLReader => Keys.GetString("InvalidOrMissingXMLReader");

	public static string SinglePolicyDeserializationFailed => Keys.GetString("SinglePolicyDeserializationFailed");

	public static string PolicyImportFileDoesNotHaveAnyPolicy => Keys.GetString("PolicyImportFileDoesNotHaveAnyPolicy");

	public static string ConditionIsNull => Keys.GetString("ConditionIsNull");

	public static string ObjectSetIsNull => Keys.GetString("ObjectSetIsNull");

	public static string IncompatiblePolicyEvaluationMode => Keys.GetString("IncompatiblePolicyEvaluationMode");

	public static string OnlyOneConditionExpression => Keys.GetString("OnlyOneConditionExpression");

	public static string OnlyInSqlClr => Keys.GetString("OnlyInSqlClr");

	public static string OnlyInMsdb => Keys.GetString("OnlyInMsdb");

	public static string NullArgInPolicyEvaluation => Keys.GetString("NullArgInPolicyEvaluation");

	public static string FailedToRetrieveImpersonationCookie => Keys.GetString("FailedToRetrieveImpersonationCookie");

	public static string CantExecuteLongSqlScriptOn2000 => Keys.GetString("CantExecuteLongSqlScriptOn2000");

	public static string PolicyCategoryAlreadySubscribed => Keys.GetString("PolicyCategoryAlreadySubscribed");

	public static string PolicyCategoryNotSubscribed => Keys.GetString("PolicyCategoryNotSubscribed");

	public static string Function => Keys.GetString("Function");

	public static string Operator => Keys.GetString("Operator");

	public static string PolicyWithNoFilters => Keys.GetString("PolicyWithNoFilters");

	public static string MissingJobSchedule => Keys.GetString("MissingJobSchedule");

	public static string InvalidHelpLinkMessage => Keys.GetString("InvalidHelpLinkMessage");

	public static string ReferencedConditionsCannotContainScript => Keys.GetString("ReferencedConditionsCannotContainScript");

	public static string FacetNotSupported => Keys.GetString("FacetNotSupported");

	public static string EnumApplicablePolicies => Keys.GetString("EnumApplicablePolicies");

	public static string CalculateHealthState => Keys.GetString("CalculateHealthState");

	public static string GetAggregatedHealthState => Keys.GetString("GetAggregatedHealthState");

	public static string CannotChangeFacet => Keys.GetString("CannotChangeFacet");

	public static string CannotChangeFacetForObjectSet => Keys.GetString("CannotChangeFacetForObjectSet");

	protected ExceptionTemplatesSR()
	{
	}

	public static string NoSuchCollection(string name)
	{
		return Keys.GetString("NoSuchCollection", name);
	}

	public static string UnsupportedObjectType(string type, string host)
	{
		return Keys.GetString("UnsupportedObjectType", type, host);
	}

	public static string UnknownEnumeration(string type)
	{
		return Keys.GetString("UnknownEnumeration", type);
	}

	public static string InvalidArgument(string arg)
	{
		return Keys.GetString("InvalidArgument", arg);
	}

	public static string ObjectCannotBeCreated(string obj)
	{
		return Keys.GetString("ObjectCannotBeCreated", obj);
	}

	public static string TooManyURNsReturned(string type)
	{
		return Keys.GetString("TooManyURNsReturned", type);
	}

	public static string RootConditionFailed(string type)
	{
		return Keys.GetString("RootConditionFailed", type);
	}

	public static string UnsupportedConnectionType(string type)
	{
		return Keys.GetString("UnsupportedConnectionType", type);
	}

	public static string NotSupported(string host_type, string host_name, string object_type, string object_name)
	{
		return Keys.GetString("NotSupported", host_type, host_name, object_type, object_name);
	}

	public static string ObjectAlreadyExists(string @object, string name)
	{
		return Keys.GetString("ObjectAlreadyExists", @object, name);
	}

	public static string ObjectDoesntExist(string @object, string name)
	{
		return Keys.GetString("ObjectDoesntExist", @object, name);
	}

	public static string ArgumentNullOrEmpty(string argument)
	{
		return Keys.GetString("ArgumentNullOrEmpty", argument);
	}

	public static string FailedOperation(string opName, string objType, string objName)
	{
		return Keys.GetString("FailedOperation", opName, objType, objName);
	}

	public static string CannotMarkSystemObject(string objType)
	{
		return Keys.GetString("CannotMarkSystemObject", objType);
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

	public static string FailedMethodCall(string methodName, string facetType, string targetName)
	{
		return Keys.GetString("FailedMethodCall", methodName, facetType, targetName);
	}

	public static string ConfigValueMismatch(string propertyName, string configValue, string runValue)
	{
		return Keys.GetString("ConfigValueMismatch", propertyName, configValue, runValue);
	}

	public static string OperatorNotApplicable(string @operator, string type)
	{
		return Keys.GetString("OperatorNotApplicable", @operator, type);
	}

	public static string FunctionWrongArgumentsNumber(string function, int argsReceived, int argsExpected)
	{
		return Keys.GetString("FunctionWrongArgumentsNumber", function, argsReceived, argsExpected);
	}

	public static string FunctionWrongArgumentType(string function, string typeReceived, string typeExpected)
	{
		return Keys.GetString("FunctionWrongArgumentType", function, typeReceived, typeExpected);
	}

	public static string ExpressionTypeMistmatch(string typeLeft, string typeRight)
	{
		return Keys.GetString("ExpressionTypeMistmatch", typeLeft, typeRight);
	}

	public static string NullFacet(string facet)
	{
		return Keys.GetString("NullFacet", facet);
	}

	public static string BadExpressionTree(string reason)
	{
		return Keys.GetString("BadExpressionTree", reason);
	}

	public static string ExpressionNodeNotConfigurable(string subtype)
	{
		return Keys.GetString("ExpressionNodeNotConfigurable", subtype);
	}

	public static string ExpressionNodeNotConfigurableOperators(string propertyName, string expression)
	{
		return Keys.GetString("ExpressionNodeNotConfigurableOperators", propertyName, expression);
	}

	public static string UnsupportedTargetFilter(string filter)
	{
		return Keys.GetString("UnsupportedTargetFilter", filter);
	}

	public static string ParsingError(string input)
	{
		return Keys.GetString("ParsingError", input);
	}

	public static string ParsingArgumentException(string pattern, string message)
	{
		return Keys.GetString("ParsingArgumentException", pattern, message);
	}

	public static string ParsingUnquotedDatePart(string input)
	{
		return Keys.GetString("ParsingUnquotedDatePart", input);
	}

	public static string UnsupportedFunction(string type)
	{
		return Keys.GetString("UnsupportedFunction", type);
	}

	public static string InvalidOperand(string type, string operand)
	{
		return Keys.GetString("InvalidOperand", type, operand);
	}

	public static string NonConfigurableReadOnlyProperty(string propertyName)
	{
		return Keys.GetString("NonConfigurableReadOnlyProperty", propertyName);
	}

	public static string InvalidInOperator(string @operator)
	{
		return Keys.GetString("InvalidInOperator", @operator);
	}

	public static string FunctionWrongReturnType(string function, string typeReceived, string typeExpected)
	{
		return Keys.GetString("FunctionWrongReturnType", function, typeReceived, typeExpected);
	}

	public static string FunctionNotASmoObject(string functionName, string targetType)
	{
		return Keys.GetString("FunctionNotASmoObject", functionName, targetType);
	}

	public static string WmiException(string message)
	{
		return Keys.GetString("WmiException", message);
	}

	public static string PropertyCannotBeSet(string prop)
	{
		return Keys.GetString("PropertyCannotBeSet", prop);
	}

	public static string UnexpectedElement(string nameRead, string nameExpected)
	{
		return Keys.GetString("UnexpectedElement", nameRead, nameExpected);
	}

	public static string UnexpectedType(string typeRead, string nameRead, string typeExpected)
	{
		return Keys.GetString("UnexpectedType", typeRead, nameRead, typeExpected);
	}

	public static string UnexpectedName(string typeRead, string nameRead, string typeExpected, string nameExpected)
	{
		return Keys.GetString("UnexpectedName", typeRead, nameRead, typeExpected, nameExpected);
	}

	public static string CannotConvertToType(string inputString, string typeName)
	{
		return Keys.GetString("CannotConvertToType", inputString, typeName);
	}

	public static string UnsupportedConstantType(string node, string typeName)
	{
		return Keys.GetString("UnsupportedConstantType", node, typeName);
	}

	public static string SinglePolicyExpected(string policyName, string policyName2)
	{
		return Keys.GetString("SinglePolicyExpected", policyName, policyName2);
	}

	public static string ReferencedObjectExpected(string objectType, string objectName)
	{
		return Keys.GetString("ReferencedObjectExpected", objectType, objectName);
	}

	public static string PolicyAndConditionNameMismatch(string policyName, string policyCondition, string conditionName)
	{
		return Keys.GetString("PolicyAndConditionNameMismatch", policyName, policyCondition, conditionName);
	}

	public static string ObjectSetAndConditionFacetMismatch(string objectSetName, string objectSetFacet, string conditionName, string conditionFacet)
	{
		return Keys.GetString("ObjectSetAndConditionFacetMismatch", objectSetName, objectSetFacet, conditionName, conditionFacet);
	}

	public static string OnlyOneTarget(string queryExpression)
	{
		return Keys.GetString("OnlyOneTarget", queryExpression);
	}

	public static string MissingPolicy(string name)
	{
		return Keys.GetString("MissingPolicy", name);
	}

	public static string MissingProperty(string name)
	{
		return Keys.GetString("MissingProperty", name);
	}

	public static string NonRetrievableProperty(string name)
	{
		return Keys.GetString("NonRetrievableProperty", name);
	}

	public static string RollBack(string policy, string objectQe, string condition, string description, string helpText, string helpLink, string statement)
	{
		return Keys.GetString("RollBack", policy, objectQe, condition, description, helpText, helpLink, statement);
	}

	public static string OnlyRoleMember(string role)
	{
		return Keys.GetString("OnlyRoleMember", role);
	}

	public static string PolicyEvaluationFailed(string policyName)
	{
		return Keys.GetString("PolicyEvaluationFailed", policyName);
	}

	public static string PolicyEvaluationFailedOnDelegate(string policyName)
	{
		return Keys.GetString("PolicyEvaluationFailedOnDelegate", policyName);
	}

	public static string PolicyEvaluationFailedNoException(string policyName)
	{
		return Keys.GetString("PolicyEvaluationFailedNoException", policyName);
	}

	public static string BadEventData(string element, string eventName)
	{
		return Keys.GetString("BadEventData", element, eventName);
	}

	public static string MissingTypeFacetAssociation(string typeName, string facet)
	{
		return Keys.GetString("MissingTypeFacetAssociation", typeName, facet);
	}

	public static string NoScriptInSqlClr(string policy)
	{
		return Keys.GetString("NoScriptInSqlClr", policy);
	}

	public static string PolicyViolated(string policy)
	{
		return Keys.GetString("PolicyViolated", policy);
	}

	public static string PolicyViolatedTarget(string policy, string targetName)
	{
		return Keys.GetString("PolicyViolatedTarget", policy, targetName);
	}

	public static string ConversionNotSupported(string obj, string type)
	{
		return Keys.GetString("ConversionNotSupported", obj, type);
	}

	public static string TypeNotSupported(string type)
	{
		return Keys.GetString("TypeNotSupported", type);
	}

	public static string ScheduleDoesntExist(string guid)
	{
		return Keys.GetString("ScheduleDoesntExist", guid);
	}

	public static string ValidationFailed(string objectType, string objectName)
	{
		return Keys.GetString("ValidationFailed", objectType, objectName);
	}

	public static string StringPropertyTooLong(string propertyName, int maxLength, int currentLength)
	{
		return Keys.GetString("StringPropertyTooLong", propertyName, maxLength, currentLength);
	}

	public static string UnknownTargetDatabase(string policy, string name)
	{
		return Keys.GetString("UnknownTargetDatabase", policy, name);
	}

	public static string UnsupportedTqe(string queryEx)
	{
		return Keys.GetString("UnsupportedTqe", queryEx);
	}

	public static string UnsupportedKey(string key)
	{
		return Keys.GetString("UnsupportedKey", key);
	}

	public static string UnsupportedTqeProp(string queryEx, string prop)
	{
		return Keys.GetString("UnsupportedTqeProp", queryEx, prop);
	}

	public static string UnsupportedCrudDependencyAction(string action)
	{
		return Keys.GetString("UnsupportedCrudDependencyAction", action);
	}

	public static string InvalidUrnSkeleton(string action)
	{
		return Keys.GetString("InvalidUrnSkeleton", action);
	}

	public static string TargetSetCountMismatch(string objectSetName, string facetName)
	{
		return Keys.GetString("TargetSetCountMismatch", objectSetName, facetName);
	}

	public static string UnsupportedTargetSetForFacet(string targetSetSkeleton, string objectSetName, string facetName)
	{
		return Keys.GetString("UnsupportedTargetSetForFacet", targetSetSkeleton, objectSetName, facetName);
	}

	public static string NoTargetSetEnabled(string objectSetName)
	{
		return Keys.GetString("NoTargetSetEnabled", objectSetName);
	}

	public static string ConditionCannotBeUsedForFiltering(string condition)
	{
		return Keys.GetString("ConditionCannotBeUsedForFiltering", condition);
	}

	public static string ObjectSetAlreadyReferenced(string objectSetName, string policyName)
	{
		return Keys.GetString("ObjectSetAlreadyReferenced", objectSetName, policyName);
	}

	public static string VersionNotSupported(string curVerison, string minVersion)
	{
		return Keys.GetString("VersionNotSupported", curVerison, minVersion);
	}
}
