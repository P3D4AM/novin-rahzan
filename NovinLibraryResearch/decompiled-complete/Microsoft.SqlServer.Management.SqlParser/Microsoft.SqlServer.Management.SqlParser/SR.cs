using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Management.SqlParser;

[CompilerGenerated]
internal class SR
{
	[CompilerGenerated]
	public class Keys
	{
		private static ResourceManager resourceManager = new ResourceManager(typeof(SR));

		private static CultureInfo _culture = null;

		public const string UnclosedQuotationMark = "UnclosedQuotationMark";

		public const string IncorrectSyntaxNear = "IncorrectSyntaxNear";

		public const string IncorrectSyntaxNearKeyword = "IncorrectSyntaxNearKeyword";

		public const string IncorrectOptionValue = "IncorrectOptionValue";

		public const string IncorrectOptionOrder = "IncorrectOptionOrder";

		public const string OptionNotRecognized = "OptionNotRecognized";

		public const string OptionSpecifiedMultipleTimes = "OptionSpecifiedMultipleTimes";

		public const string MaximumPrecisionOutOfRange = "MaximumPrecisionOutOfRange";

		public const string MustBeOnlyStatementInBatch = "MustBeOnlyStatementInBatch";

		public const string StatementNotSupportedInIntelliSense = "StatementNotSupportedInIntelliSense";

		public const string IntegerValueOutOfRange = "IntegerValueOutOfRange";

		public const string MergeStatementMustTerminated = "MergeStatementMustTerminated";

		public const string CannotPerformAlterOnObject = "CannotPerformAlterOnObject";

		public const string TypeIsNotSystemType = "TypeIsNotSystemType";

		public const string ParseResultsShouldNotContainNullElement = "ParseResultsShouldNotContainNullElement";

		public const string SchemaNotExist = "SchemaNotExist";

		public const string ColumnSpecifiedMultipleTimes = "ColumnSpecifiedMultipleTimes";

		public const string InvalidObjectName = "InvalidObjectName";

		public const string TooManyArguments = "TooManyArguments";

		public const string InsufficientArguments = "InsufficientArguments";

		public const string ColumnPrefixMismatch = "ColumnPrefixMismatch";

		public const string VariableNameNotUnique = "VariableNameNotUnique";

		public const string ObjectNotExistOrIsInvalid = "ObjectNotExistOrIsInvalid";

		public const string CannotCreateSparseColumn = "CannotCreateSparseColumn";

		public const string CannotCreateDefaultConstraintOnSparseColumn = "CannotCreateDefaultConstraintOnSparseColumn";

		public const string ColumnIsInvalidForUseAsKeyColumnInIndex = "ColumnIsInvalidForUseAsKeyColumnInIndex";

		public const string CannotCreateMoreThanOneColumnSetOnTable = "CannotCreateMoreThanOneColumnSetOnTable";

		public const string CannotCreateSparseColumnSetOnTable = "CannotCreateSparseColumnSetOnTable";

		public const string CannotCreateGeneratedAlwaysColumnType = "CannotCreateGeneratedAlwaysColumnType";

		public const string CannotCreateGeneratedAlwaysColumnNullable = "CannotCreateGeneratedAlwaysColumnNullable";

		public const string CannotCreateMoreThanOneGeneratedAlwaysAsRowStartColumnOnTable = "CannotCreateMoreThanOneGeneratedAlwaysAsRowStartColumnOnTable";

		public const string CannotCreateMoreThanOneGeneratedAlwaysAsRowEndColumnOnTable = "CannotCreateMoreThanOneGeneratedAlwaysAsRowEndColumnOnTable";

		public const string CannotCreateMoreThanOneTemporalSystemTimePeriodOnTable = "CannotCreateMoreThanOneTemporalSystemTimePeriodOnTable";

		public const string GeneratedAlwaysAsRowStartColumnDefinitionMissing = "GeneratedAlwaysAsRowStartColumnDefinitionMissing";

		public const string GeneratedAlwaysAsRowEndColumnDefinitionMissing = "GeneratedAlwaysAsRowEndColumnDefinitionMissing";

		public const string GeneratedAlwaysAsRowStartColumnWrongName = "GeneratedAlwaysAsRowStartColumnWrongName";

		public const string GeneratedAlwaysAsRowEndColumnWrongName = "GeneratedAlwaysAsRowEndColumnWrongName";

		public const string TemporalSystemTimePeriodDefinitionMissing = "TemporalSystemTimePeriodDefinitionMissing";

		public const string InvalidBuildModeSqlNullStatement = "InvalidBuildModeSqlNullStatement";

		public const string InvalidBuildModeStatementCreateSchema = "InvalidBuildModeStatementCreateSchema";

		public const string InvalidBuildModeStatementCreateIndex = "InvalidBuildModeStatementCreateIndex";

		public const string InvalidBuildModeStatementCreateProcCursorParams = "InvalidBuildModeStatementCreateProcCursorParams";

		public const string InvalidBuildModeStatementCreateProcedureWithEncryption = "InvalidBuildModeStatementCreateProcedureWithEncryption";

		public const string InvalidBuildModeStatementCreateFunction = "InvalidBuildModeStatementCreateFunction";

		public const string InvalidBuildModeStatementCreateFunctionWithEncryption = "InvalidBuildModeStatementCreateFunctionWithEncryption";

		public const string InvalidBuildModeStatementCreateLogin = "InvalidBuildModeStatementCreateLogin";

		public const string InvalidBuildModeStatementCreateLoginWithDefaultDatabase = "InvalidBuildModeStatementCreateLoginWithDefaultDatabase";

		public const string InvalidBuildModeStatementCreateTriggerDdl = "InvalidBuildModeStatementCreateTriggerDdl";

		public const string InvalidBuildModeStatementCreateTriggerWithEncryption = "InvalidBuildModeStatementCreateTriggerWithEncryption";

		public const string InvalidBuildModeStatementCreateViewWithEncryption = "InvalidBuildModeStatementCreateViewWithEncryption";

		public const string InvalidBuildModeDataTypeUse = "InvalidBuildModeDataTypeUse";

		public const string InvalidBuildModeExecutionContextTypeSelf = "InvalidBuildModeExecutionContextTypeSelf";

		public const string UseDatabaseStatementNotAllowed = "UseDatabaseStatementNotAllowed";

		public const string InvalidExecuteAsOption = "InvalidExecuteAsOption";

		public const string ExtendedStoredProceduresNotSupported = "ExtendedStoredProceduresNotSupported";

		public const string StoredProceduresAlwaysReturnInt = "StoredProceduresAlwaysReturnInt";

		public const string AmbiguousColumnName = "AmbiguousColumnName";

		public const string InvalidColumnName = "InvalidColumnName";

		public const string MultiPartIdentifierBindingError = "MultiPartIdentifierBindingError";

		public const string InvalidColumnXmlNodeUse = "InvalidColumnXmlNodeUse";

		public const string TVFMethodMustBeAliased = "TVFMethodMustBeAliased";

		public const string DatabaseNotExist = "DatabaseNotExist";

		public const string DatabaseObjectExist = "DatabaseObjectExist";

		public const string UnrecognizedCreateTableOption = "UnrecognizedCreateTableOption";

		public const string MoreColumns = "MoreColumns";

		public const string FewerColumns = "FewerColumns";

		public const string IsTableValuedFunction = "IsTableValuedFunction";

		public const string CouldNotLocateDatabase = "CouldNotLocateDatabase";

		public const string CannotFindStoredProcedure = "CannotFindStoredProcedure";

		public const string MissingParameters = "MissingParameters";

		public const string InvalidParameter = "InvalidParameter";

		public const string ParameterSuppliedMultipleTimes = "ParameterSuppliedMultipleTimes";

		public const string OutputParameterMismatch = "OutputParameterMismatch";

		public const string MissingParameter = "MissingParameter";

		public const string InconsistentParameterFormat = "InconsistentParameterFormat";

		public const string InvalidExecuteOption = "InvalidExecuteOption";

		public const string MissingColumn = "MissingColumn";

		public const string InvalidUsageOfIndexOption = "InvalidUsageOfIndexOption";

		public const string InvalidUsageOfScopedConfiguration = "InvalidUsageOfScopedConfiguration";

		public const string UnrecognizedCursorOption = "UnrecognizedCursorOption";

		public const string InvalidUsageOfCursorOption = "InvalidUsageOfCursorOption";

		public const string MixingOldAndNewSyntaxForCursorOptionsNotAllowed = "MixingOldAndNewSyntaxForCursorOptionsNotAllowed";

		public const string ConflictingCursorOption = "ConflictingCursorOption";

		public const string IndexOrStatisticsExists = "IndexOrStatisticsExists";

		public const string ClusteredIndexExists = "ClusteredIndexExists";

		public const string CouldNotFindIndex = "CouldNotFindIndex";

		public const string CannotConvertXmlOrSpatialIndexToRelational = "CannotConvertXmlOrSpatialIndexToRelational";

		public const string CannotConvertClusteredIndexToNonclustered = "CannotConvertClusteredIndexToNonclustered";

		public const string CannotCreateIndexOnViewNotSchemaBound = "CannotCreateIndexOnViewNotSchemaBound";

		public const string CannotCreateIndexOnViewDoesNotHaveUniqueClusteredIndex = "CannotCreateIndexOnViewDoesNotHaveUniqueClusteredIndex";

		public const string CannotCreateNonuniqueClusteredIndexOnView = "CannotCreateNonuniqueClusteredIndexOnView";

		public const string InvalidIndexKeyColumnType = "InvalidIndexKeyColumnType";

		public const string InvalidIndexIncludedColumnType = "InvalidIndexIncludedColumnType";

		public const string CannotCreateIndexOnViewContainsInvalidColumns = "CannotCreateIndexOnViewContainsInvalidColumns";

		public const string OnlineOperationCannotBePerformedOnIndexInvalidColumns = "OnlineOperationCannotBePerformedOnIndexInvalidColumns";

		public const string CannotSpecifyIncludedColumnsForClusteredIndex = "CannotSpecifyIncludedColumnsForClusteredIndex";

		public const string InvalidFillFactorPercentage = "InvalidFillFactorPercentage";

		public const string IncorrectWhereClauseForFilteredIndex = "IncorrectWhereClauseForFilteredIndex";

		public const string CannotAssignDefaultValueToALocalVariable = "CannotAssignDefaultValueToALocalVariable";

		public const string CannotUseOutputOptionInDeclareOrCreateFunction = "CannotUseOutputOptionInDeclareOrCreateFunction";

		public const string ParamVarHasInvalidDataType = "ParamVarHasInvalidDataType";

		public const string ColumnHasInvalidDataType = "ColumnHasInvalidDataType";

		public const string OutOfRangeDegreeOfParallelism = "OutOfRangeDegreeOfParallelism";

		public const string ParameterCannotBeReadOnly = "ParameterCannotBeReadOnly";

		public const string TableValuedParameterMustBeReadOnly = "TableValuedParameterMustBeReadOnly";

		public const string OperandTypeClash = "OperandTypeClash";

		public const string ScalarVariableRequired = "ScalarVariableRequired";

		public const string ColumnNameNotUnique = "ColumnNameNotUnique";

		public const string MultiplePrimaryKey = "MultiplePrimaryKey";

		public const string CorrelationNameNotUnique = "CorrelationNameNotUnique";

		public const string InvalidCorrelationNameWithTable = "InvalidCorrelationNameWithTable";

		public const string InvalidCorrelationNamesInFrom = "InvalidCorrelationNamesInFrom";

		public const string ParametersNotSuppliedForFunction = "ParametersNotSuppliedForFunction";

		public const string TableIsAmbiguous = "TableIsAmbiguous";

		public const string ParametersSuppliedForNonFunction = "ParametersSuppliedForNonFunction";

		public const string FunctionCannotBeUsedToMatchTarget = "FunctionCannotBeUsedToMatchTarget";

		public const string ObjectCannotBeModified = "ObjectCannotBeModified";

		public const string TableVariableRequired = "TableVariableRequired";

		public const string OperatorNotSupported = "OperatorNotSupported";

		public const string BooleanConditionExpected = "BooleanConditionExpected";

		public const string DataTypeMissing = "DataTypeMissing";

		public const string ColumnConstraintNotUnique = "ColumnConstraintNotUnique";

		public const string PrimaryKeyNotUnique = "PrimaryKeyNotUnique";

		public const string CannotHaveDefaultsOnIdentity = "CannotHaveDefaultsOnIdentity";

		public const string CannotCreateIdentityOnNullable = "CannotCreateIdentityOnNullable";

		public const string CannotDefinePrimaryKeyOnNullable = "CannotDefinePrimaryKeyOnNullable";

		public const string RowguidcolDatatypeMismatch = "RowguidcolDatatypeMismatch";

		public const string ColumnHasUserDefinedTableType = "ColumnHasUserDefinedTableType";

		public const string InvalidSeed = "InvalidSeed";

		public const string InvalidIncrement = "InvalidIncrement";

		public const string IdentityColumnInvalidType = "IdentityColumnInvalidType";

		public const string ComputedColumnsConstraintCheckError = "ComputedColumnsConstraintCheckError";

		public const string ColumnNameNotInTargetTable = "ColumnNameNotInTargetTable";

		public const string ForeignKeyReferencesInvalidTable = "ForeignKeyReferencesInvalidTable";

		public const string ForeignKeyNumberOfRefColumnsDiffers = "ForeignKeyNumberOfRefColumnsDiffers";

		public const string NoPrimaryKeysInReferencedTable = "NoPrimaryKeysInReferencedTable";

		public const string ForeignKeyReferencesImplicitlyTableWithoutPrimaryKey = "ForeignKeyReferencesImplicitlyTableWithoutPrimaryKey";

		public const string ColumnIsNotSameTypeAsRefColumn = "ColumnIsNotSameTypeAsRefColumn";

		public const string ForeignKeyInvalidReferencingColumn = "ForeignKeyInvalidReferencingColumn";

		public const string ForeignKeyInvalidReferencedColumn = "ForeignKeyInvalidReferencedColumn";

		public const string NameOrAuthorizationKeywordRequired = "NameOrAuthorizationKeywordRequired";

		public const string InvalidOnClause = "InvalidOnClause";

		public const string InvalidOptionInCreateFunction = "InvalidOptionInCreateFunction";

		public const string DbNameIsNotAllowedForCreateAlterFunc = "DbNameIsNotAllowedForCreateAlterFunc";

		public const string ConflictingReturnsNullAndCalledOnNullInputOptions = "ConflictingReturnsNullAndCalledOnNullInputOptions";

		public const string LastStatementWithinFunctionMustBeReturn = "LastStatementWithinFunctionMustBeReturn";

		public const string ReturnStatementInScalarValuedFunctionMustIncludeArg = "ReturnStatementInScalarValuedFunctionMustIncludeArg";

		public const string UseReturnStatementWithValueCannotBeUsed = "UseReturnStatementWithValueCannotBeUsed";

		public const string InvalidUseOfSideEffectingOperatorWithinFunction = "InvalidUseOfSideEffectingOperatorWithinFunction";

		public const string SelectStatementWithinFunctionCannotReturnData = "SelectStatementWithinFunctionCannotReturnData";

		public const string TempFunctionNameIsNotAllowed = "TempFunctionNameIsNotAllowed";

		public const string ObjectNameIsMissingOrEmpty = "ObjectNameIsMissingOrEmpty";

		public const string InvalidConstantOutput = "InvalidConstantOutput";

		public const string ReadonlyCannotBeUsed = "ReadonlyCannotBeUsed";

		public const string InvalidOptionInCreateProcedure = "InvalidOptionInCreateProcedure";

		public const string InvalidProcedureNumberRange = "InvalidProcedureNumberRange";

		public const string DbNameIsNotAllowedForCreateAlterProc = "DbNameIsNotAllowedForCreateAlterProc";

		public const string DbNameIsNotAllowedForCreateSynonym = "DbNameIsNotAllowedForCreateSynonym";

		public const string DbNameIsNotAllowedForDropSynonym = "DbNameIsNotAllowedForDropSynonym";

		public const string InvalidOptionInCreateTrigger = "InvalidOptionInCreateTrigger";

		public const string InvalidTriggerEventTypes = "InvalidTriggerEventTypes";

		public const string InvalidTriggerEventType = "InvalidTriggerEventType";

		public const string InvalidTriggerEventTypeOnDatabase = "InvalidTriggerEventTypeOnDatabase";

		public const string DuplicateTriggerEventType = "DuplicateTriggerEventType";

		public const string DuplicateTriggerActionType = "DuplicateTriggerActionType";

		public const string InvalidTriggerSchema = "InvalidTriggerSchema";

		public const string TriggerDoesNotBelongToTarget = "TriggerDoesNotBelongToTarget";

		public const string RequiredInsteadOfTriggerOnView = "RequiredInsteadOfTriggerOnView";

		public const string DuplicateInsteadOfTrigger = "DuplicateInsteadOfTrigger";

		public const string CannotCreateTriggerOnViewWithCheckOption = "CannotCreateTriggerOnViewWithCheckOption";

		public const string CannotCreateInsteadOfTriggerOnTableWithCascade = "CannotCreateInsteadOfTriggerOnTableWithCascade";

		public const string InvalidLengthOrPrecision = "InvalidLengthOrPrecision";

		public const string InvalidScale = "InvalidScale";

		public const string ScalePrecisionMismatch = "ScalePrecisionMismatch";

		public const string MaximumSizeError = "MaximumSizeError";

		public const string MaximumSizeErrorForAnyType = "MaximumSizeErrorForAnyType";

		public const string TypeNameMaxPrefixError = "TypeNameMaxPrefixError";

		public const string XmlSchemaCollectionMaxPrefixError = "XmlSchemaCollectionMaxPrefixError";

		public const string SelectAssignmentError = "SelectAssignmentError";

		public const string SelectIntoMustBeFirstQuery = "SelectIntoMustBeFirstQuery";

		public const string InvalidTableHint = "InvalidTableHint";

		public const string TableConstraintHasNoColumnList = "TableConstraintHasNoColumnList";

		public const string DuplicateColumnNamesInIndex = "DuplicateColumnNamesInIndex";

		public const string InvalidOptionInCreateView = "InvalidOptionInCreateView";

		public const string DatabaseNameAsPrefixInCreateView = "DatabaseNameAsPrefixInCreateView";

		public const string NotRecognizedFunctionName = "NotRecognizedFunctionName";

		public const string CannotFindFunctionOrAggregate = "CannotFindFunctionOrAggregate";

		public const string RemoteFunctionRefIsNotAllowed = "RemoteFunctionRefIsNotAllowed";

		public const string RowTagOnlyInRawAndPath = "RowTagOnlyInRawAndPath";

		public const string XmlSchemaError = "XmlSchemaError";

		public const string ElementsError = "ElementsError";

		public const string Base64Error = "Base64Error";

		public const string TypeError = "TypeError";

		public const string IncludeNullValuesError = "IncludeNullValuesError";

		public const string WithoutArrayWrapperError = "WithoutArrayWrapperError";

		public const string AnonimousColumn = "AnonimousColumn";

		public const string InvalidGroupByOption = "InvalidGroupByOption";

		public const string CommaOr = "CommaOr";

		public const string Expecting = "Expecting";

		public const string EndOfFile = "EndOfFile";

		public const string Comma = "Comma";

		public const string Period = "Period";

		public const string CannotDropObject = "CannotDropObject";

		public const string CannotUseDrop = "CannotUseDrop";

		public const string CouldNotLocateEntryInSysdatabases = "CouldNotLocateEntryInSysdatabases";

		public const string FunctionRequiresNumberOfArguments = "FunctionRequiresNumberOfArguments";

		public const string FunctionRequiresRangeOfAruments = "FunctionRequiresRangeOfAruments";

		public const string FunctionRequiresZeroArguments = "FunctionRequiresZeroArguments";

		public const string FunctionRequiresAtLeastNumberOfArguments = "FunctionRequiresAtLeastNumberOfArguments";

		public const string InvalidParameterOne = "InvalidParameterOne";

		public const string NotRecognizedDatePartOption = "NotRecognizedDatePartOption";

		public const string InvalidAggregateFunction = "InvalidAggregateFunction";

		public const string DuplicateCteName = "DuplicateCteName";

		public const string RecursiveCteHasNoUnionAll = "RecursiveCteHasNoUnionAll";

		public const string NoAnchorMemberForRecursiveQuery = "NoAnchorMemberForRecursiveQuery";

		public const string RecursiveCteMemberHasMultipleRefs = "RecursiveCteMemberHasMultipleRefs";

		public const string AnchorMemberFoundInRecursiveQuery = "AnchorMemberFoundInRecursiveQuery";

		public const string PrefixedColumnsNotAllowedInPivot = "PrefixedColumnsNotAllowedInPivot";

		public const string ColumnNameConflictsInPivot = "ColumnNameConflictsInPivot";

		public const string PrefixedColumnsNotAllowedInUnpivot = "PrefixedColumnsNotAllowedInUnpivot";

		public const string ColumnNameConflictsInUnpivot = "ColumnNameConflictsInUnpivot";

		public const string ColumnSpecifiedMultipleTimesInUnpivot = "ColumnSpecifiedMultipleTimesInUnpivot";

		public const string OrderByItemContainsVariable = "OrderByItemContainsVariable";

		public const string OrderByPositionNumberIsOutOfRange = "OrderByPositionNumberIsOutOfRange";

		public const string OrderByListHasConstantExpression = "OrderByListHasConstantExpression";

		public const string SetClauseColumnSpecifiedMultipleTimes = "SetClauseColumnSpecifiedMultipleTimes";

		public const string OutputIntoTargetCannotBeViewOrCte = "OutputIntoTargetCannotBeViewOrCte";

		public const string NumberOfValuesDoesNotMatchTableDef = "NumberOfValuesDoesNotMatchTableDef";

		public const string SelectListOfInsertHasFewerItems = "SelectListOfInsertHasFewerItems";

		public const string SelectListOfInsertHasMoreItems = "SelectListOfInsertHasMoreItems";

		public const string InsertIntoIdentityColumnNotAllowed = "InsertIntoIdentityColumnNotAllowed";

		public const string NestedDmlMustHaveOutputClause = "NestedDmlMustHaveOutputClause";

		public const string SubqueriesNotAllowedInOutput = "SubqueriesNotAllowedInOutput";

		public const string AggregateNotAllowedInOutput = "AggregateNotAllowedInOutput";

		public const string FunctionNotAllowedInOutput = "FunctionNotAllowedInOutput";

		public const string ExplicitValueForIdentityColumn = "ExplicitValueForIdentityColumn";

		public const string NumberOfColumnsMustBeTheSame = "NumberOfColumnsMustBeTheSame";

		public const string CannotCallMethodsOnType = "CannotCallMethodsOnType";

		public const string UdtMemberIsNotStatic = "UdtMemberIsNotStatic";

		public const string UdtMemberIsStatic = "UdtMemberIsStatic";

		public const string UdtPropertyIsNotStatic = "UdtPropertyIsNotStatic";

		public const string UdtPropertyIsStatic = "UdtPropertyIsStatic";

		public const string CouldNotFindPropertyOrField = "CouldNotFindPropertyOrField";

		public const string CouldNotFindMethod = "CouldNotFindMethod";

		public const string NotValidFunctionOrProperty = "NotValidFunctionOrProperty";

		public const string IncorrectSyntaxToInvokeXmlMethod = "IncorrectSyntaxToInvokeXmlMethod";

		public const string LoginExist = "LoginExist";

		public const string UserExist = "UserExist";

		public const string CouldNotFindCredential = "CouldNotFindCredential";

		public const string CouldNotFindCertificate = "CouldNotFindCertificate";

		public const string CouldNotFindAsymmetricKey = "CouldNotFindAsymmetricKey";

		public const string CouldNotFindLogin = "CouldNotFindLogin";

		public const string UserDefinedTypeExist = "UserDefinedTypeExist";

		public const string InvalidBaseTypeForAlias = "InvalidBaseTypeForAlias";

		public const string CannotFindUser = "CannotFindUser";

		public const string UserGroupOrRoleExists = "UserGroupOrRoleExists";

		public const string RequiredParam = "RequiredParam";

		public const string DuplicateParam = "DuplicateParam";

		public const string InvalidCollation = "InvalidCollation";

		public const string ExpressionTypeInvalidForCollate = "ExpressionTypeInvalidForCollate";

		public const string CollateCannotBeUsedOnUddt = "CollateCannotBeUsedOnUddt";

		public const string ApplicationRole = "ApplicationRole";

		public const string AsymmetricKey = "AsymmetricKey";

		public const string BuiltinFunction = "BuiltinFunction";

		public const string Certificate = "Certificate";

		public const string Collation = "Collation";

		public const string Column = "Column";

		public const string ColumnAlias = "ColumnAlias";

		public const string CommonTableExpression = "CommonTableExpression";

		public const string Credential = "Credential";

		public const string Database = "Database";

		public const string DatabaseDdlTrigger = "DatabaseDdlTrigger";

		public const string DatabasePermission = "DatabasePermission";

		public const string DatabaseRole = "DatabaseRole";

		public const string DataType = "DataType";

		public const string Datepart = "Datepart";

		public const string DerivedTable = "DerivedTable";

		public const string DmlTableSource = "DmlTableSource";

		public const string DmlTrigger = "DmlTrigger";

		public const string ExtendedStoredProcedure = "ExtendedStoredProcedure";

		public const string GlobalVariable = "GlobalVariable";

		public const string LocalVariable = "LocalVariable";

		public const string Login = "Login";

		public const string Parameter = "Parameter";

		public const string PivotTable = "PivotTable";

		public const string ScalarValuedFunction = "ScalarValuedFunction";

		public const string Schema = "Schema";

		public const string Server = "Server";

		public const string ServerDdlTrigger = "ServerDdlTrigger";

		public const string StoredProcedure = "StoredProcedure";

		public const string Synonym = "Synonym";

		public const string SystemDataType = "SystemDataType";

		public const string Table = "Table";

		public const string TableAlias = "TableAlias";

		public const string TableValuedFunction = "TableValuedFunction";

		public const string TemporaryTable = "TemporaryTable";

		public const string UnpivotTable = "UnpivotTable";

		public const string User = "User";

		public const string UserDefinedAggregate = "UserDefinedAggregate";

		public const string UserDefinedClrType = "UserDefinedClrType";

		public const string UserDefinedDataType = "UserDefinedDataType";

		public const string UserDefinedTableType = "UserDefinedTableType";

		public const string View = "View";

		public const string XmlDataTypeMethod = "XmlDataTypeMethod";

		public const string DeserializationInvalidXmlElement1 = "DeserializationInvalidXmlElement1";

		public const string DeserializationInvalidXmlElement2 = "DeserializationInvalidXmlElement2";

		public const string DeserializationMissingXmlElement = "DeserializationMissingXmlElement";

		public const string DeserializationMissingXmlAttribute = "DeserializationMissingXmlAttribute";

		public const string DeserializationExpectedSingleXmlElement = "DeserializationExpectedSingleXmlElement";

		public const string DeserializationUnexpectedXmlContent = "DeserializationUnexpectedXmlContent";

		public const string DeserializationInvalidObjectType = "DeserializationInvalidObjectType";

		public const string DeserializationObjectMustHaveOneParent = "DeserializationObjectMustHaveOneParent";

		public const string DeserializationUnresolvedExternalReference = "DeserializationUnresolvedExternalReference";

		public const string InvalidRootTypes = "InvalidRootTypes";

		public const string SqlScriptMustHaveOnlyModuleStatement = "SqlScriptMustHaveOnlyModuleStatement";

		public const string SqlScriptMustHaveOnlyTriggerStatement = "SqlScriptMustHaveOnlyTriggerStatement";

		public const string SqlScriptMustHaveOnlyViewStatement = "SqlScriptMustHaveOnlyViewStatement";

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

		public static string GetString(string key, object arg0, object arg1, object arg2, object arg3, object arg4)
		{
			return string.Format(CultureInfo.CurrentCulture, resourceManager.GetString(key, _culture), arg0, arg1, arg2, arg3, arg4);
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

	public static string MergeStatementMustTerminated => Keys.GetString("MergeStatementMustTerminated");

	public static string ParseResultsShouldNotContainNullElement => Keys.GetString("ParseResultsShouldNotContainNullElement");

	public static string CannotCreateMoreThanOneGeneratedAlwaysAsRowStartColumnOnTable => Keys.GetString("CannotCreateMoreThanOneGeneratedAlwaysAsRowStartColumnOnTable");

	public static string CannotCreateMoreThanOneGeneratedAlwaysAsRowEndColumnOnTable => Keys.GetString("CannotCreateMoreThanOneGeneratedAlwaysAsRowEndColumnOnTable");

	public static string CannotCreateMoreThanOneTemporalSystemTimePeriodOnTable => Keys.GetString("CannotCreateMoreThanOneTemporalSystemTimePeriodOnTable");

	public static string GeneratedAlwaysAsRowStartColumnDefinitionMissing => Keys.GetString("GeneratedAlwaysAsRowStartColumnDefinitionMissing");

	public static string GeneratedAlwaysAsRowEndColumnDefinitionMissing => Keys.GetString("GeneratedAlwaysAsRowEndColumnDefinitionMissing");

	public static string GeneratedAlwaysAsRowStartColumnWrongName => Keys.GetString("GeneratedAlwaysAsRowStartColumnWrongName");

	public static string GeneratedAlwaysAsRowEndColumnWrongName => Keys.GetString("GeneratedAlwaysAsRowEndColumnWrongName");

	public static string TemporalSystemTimePeriodDefinitionMissing => Keys.GetString("TemporalSystemTimePeriodDefinitionMissing");

	public static string InvalidBuildModeStatementCreateSchema => Keys.GetString("InvalidBuildModeStatementCreateSchema");

	public static string InvalidBuildModeStatementCreateIndex => Keys.GetString("InvalidBuildModeStatementCreateIndex");

	public static string InvalidBuildModeStatementCreateProcCursorParams => Keys.GetString("InvalidBuildModeStatementCreateProcCursorParams");

	public static string InvalidBuildModeStatementCreateProcedureWithEncryption => Keys.GetString("InvalidBuildModeStatementCreateProcedureWithEncryption");

	public static string InvalidBuildModeStatementCreateFunction => Keys.GetString("InvalidBuildModeStatementCreateFunction");

	public static string InvalidBuildModeStatementCreateFunctionWithEncryption => Keys.GetString("InvalidBuildModeStatementCreateFunctionWithEncryption");

	public static string InvalidBuildModeStatementCreateLogin => Keys.GetString("InvalidBuildModeStatementCreateLogin");

	public static string InvalidBuildModeStatementCreateLoginWithDefaultDatabase => Keys.GetString("InvalidBuildModeStatementCreateLoginWithDefaultDatabase");

	public static string InvalidBuildModeStatementCreateTriggerDdl => Keys.GetString("InvalidBuildModeStatementCreateTriggerDdl");

	public static string InvalidBuildModeStatementCreateTriggerWithEncryption => Keys.GetString("InvalidBuildModeStatementCreateTriggerWithEncryption");

	public static string InvalidBuildModeStatementCreateViewWithEncryption => Keys.GetString("InvalidBuildModeStatementCreateViewWithEncryption");

	public static string InvalidBuildModeExecutionContextTypeSelf => Keys.GetString("InvalidBuildModeExecutionContextTypeSelf");

	public static string UseDatabaseStatementNotAllowed => Keys.GetString("UseDatabaseStatementNotAllowed");

	public static string ExtendedStoredProceduresNotSupported => Keys.GetString("ExtendedStoredProceduresNotSupported");

	public static string StoredProceduresAlwaysReturnInt => Keys.GetString("StoredProceduresAlwaysReturnInt");

	public static string TVFMethodMustBeAliased => Keys.GetString("TVFMethodMustBeAliased");

	public static string InvalidExecuteOption => Keys.GetString("InvalidExecuteOption");

	public static string MixingOldAndNewSyntaxForCursorOptionsNotAllowed => Keys.GetString("MixingOldAndNewSyntaxForCursorOptionsNotAllowed");

	public static string CannotConvertClusteredIndexToNonclustered => Keys.GetString("CannotConvertClusteredIndexToNonclustered");

	public static string CannotSpecifyIncludedColumnsForClusteredIndex => Keys.GetString("CannotSpecifyIncludedColumnsForClusteredIndex");

	public static string CannotAssignDefaultValueToALocalVariable => Keys.GetString("CannotAssignDefaultValueToALocalVariable");

	public static string CannotUseOutputOptionInDeclareOrCreateFunction => Keys.GetString("CannotUseOutputOptionInDeclareOrCreateFunction");

	public static string BooleanConditionExpected => Keys.GetString("BooleanConditionExpected");

	public static string RowguidcolDatatypeMismatch => Keys.GetString("RowguidcolDatatypeMismatch");

	public static string ComputedColumnsConstraintCheckError => Keys.GetString("ComputedColumnsConstraintCheckError");

	public static string NameOrAuthorizationKeywordRequired => Keys.GetString("NameOrAuthorizationKeywordRequired");

	public static string InvalidOnClause => Keys.GetString("InvalidOnClause");

	public static string InvalidOptionInCreateFunction => Keys.GetString("InvalidOptionInCreateFunction");

	public static string DbNameIsNotAllowedForCreateAlterFunc => Keys.GetString("DbNameIsNotAllowedForCreateAlterFunc");

	public static string ConflictingReturnsNullAndCalledOnNullInputOptions => Keys.GetString("ConflictingReturnsNullAndCalledOnNullInputOptions");

	public static string LastStatementWithinFunctionMustBeReturn => Keys.GetString("LastStatementWithinFunctionMustBeReturn");

	public static string ReturnStatementInScalarValuedFunctionMustIncludeArg => Keys.GetString("ReturnStatementInScalarValuedFunctionMustIncludeArg");

	public static string UseReturnStatementWithValueCannotBeUsed => Keys.GetString("UseReturnStatementWithValueCannotBeUsed");

	public static string SelectStatementWithinFunctionCannotReturnData => Keys.GetString("SelectStatementWithinFunctionCannotReturnData");

	public static string TempFunctionNameIsNotAllowed => Keys.GetString("TempFunctionNameIsNotAllowed");

	public static string ObjectNameIsMissingOrEmpty => Keys.GetString("ObjectNameIsMissingOrEmpty");

	public static string InvalidConstantOutput => Keys.GetString("InvalidConstantOutput");

	public static string ReadonlyCannotBeUsed => Keys.GetString("ReadonlyCannotBeUsed");

	public static string InvalidOptionInCreateProcedure => Keys.GetString("InvalidOptionInCreateProcedure");

	public static string DbNameIsNotAllowedForCreateAlterProc => Keys.GetString("DbNameIsNotAllowedForCreateAlterProc");

	public static string DbNameIsNotAllowedForCreateSynonym => Keys.GetString("DbNameIsNotAllowedForCreateSynonym");

	public static string DbNameIsNotAllowedForDropSynonym => Keys.GetString("DbNameIsNotAllowedForDropSynonym");

	public static string InvalidOptionInCreateTrigger => Keys.GetString("InvalidOptionInCreateTrigger");

	public static string InvalidTriggerEventTypes => Keys.GetString("InvalidTriggerEventTypes");

	public static string ScalePrecisionMismatch => Keys.GetString("ScalePrecisionMismatch");

	public static string SelectAssignmentError => Keys.GetString("SelectAssignmentError");

	public static string SelectIntoMustBeFirstQuery => Keys.GetString("SelectIntoMustBeFirstQuery");

	public static string InvalidOptionInCreateView => Keys.GetString("InvalidOptionInCreateView");

	public static string DatabaseNameAsPrefixInCreateView => Keys.GetString("DatabaseNameAsPrefixInCreateView");

	public static string RowTagOnlyInRawAndPath => Keys.GetString("RowTagOnlyInRawAndPath");

	public static string XmlSchemaError => Keys.GetString("XmlSchemaError");

	public static string ElementsError => Keys.GetString("ElementsError");

	public static string Base64Error => Keys.GetString("Base64Error");

	public static string TypeError => Keys.GetString("TypeError");

	public static string IncludeNullValuesError => Keys.GetString("IncludeNullValuesError");

	public static string WithoutArrayWrapperError => Keys.GetString("WithoutArrayWrapperError");

	public static string CommaOr => Keys.GetString("CommaOr");

	public static string Expecting => Keys.GetString("Expecting");

	public static string EndOfFile => Keys.GetString("EndOfFile");

	public static string Comma => Keys.GetString("Comma");

	public static string Period => Keys.GetString("Period");

	public static string PrefixedColumnsNotAllowedInPivot => Keys.GetString("PrefixedColumnsNotAllowedInPivot");

	public static string PrefixedColumnsNotAllowedInUnpivot => Keys.GetString("PrefixedColumnsNotAllowedInUnpivot");

	public static string NumberOfValuesDoesNotMatchTableDef => Keys.GetString("NumberOfValuesDoesNotMatchTableDef");

	public static string SelectListOfInsertHasFewerItems => Keys.GetString("SelectListOfInsertHasFewerItems");

	public static string SelectListOfInsertHasMoreItems => Keys.GetString("SelectListOfInsertHasMoreItems");

	public static string InsertIntoIdentityColumnNotAllowed => Keys.GetString("InsertIntoIdentityColumnNotAllowed");

	public static string NestedDmlMustHaveOutputClause => Keys.GetString("NestedDmlMustHaveOutputClause");

	public static string SubqueriesNotAllowedInOutput => Keys.GetString("SubqueriesNotAllowedInOutput");

	public static string AggregateNotAllowedInOutput => Keys.GetString("AggregateNotAllowedInOutput");

	public static string NumberOfColumnsMustBeTheSame => Keys.GetString("NumberOfColumnsMustBeTheSame");

	public static string CollateCannotBeUsedOnUddt => Keys.GetString("CollateCannotBeUsedOnUddt");

	public static string ApplicationRole => Keys.GetString("ApplicationRole");

	public static string AsymmetricKey => Keys.GetString("AsymmetricKey");

	public static string BuiltinFunction => Keys.GetString("BuiltinFunction");

	public static string Certificate => Keys.GetString("Certificate");

	public static string Collation => Keys.GetString("Collation");

	public static string Column => Keys.GetString("Column");

	public static string ColumnAlias => Keys.GetString("ColumnAlias");

	public static string CommonTableExpression => Keys.GetString("CommonTableExpression");

	public static string Credential => Keys.GetString("Credential");

	public static string Database => Keys.GetString("Database");

	public static string DatabaseDdlTrigger => Keys.GetString("DatabaseDdlTrigger");

	public static string DatabasePermission => Keys.GetString("DatabasePermission");

	public static string DatabaseRole => Keys.GetString("DatabaseRole");

	public static string DataType => Keys.GetString("DataType");

	public static string Datepart => Keys.GetString("Datepart");

	public static string DerivedTable => Keys.GetString("DerivedTable");

	public static string DmlTableSource => Keys.GetString("DmlTableSource");

	public static string DmlTrigger => Keys.GetString("DmlTrigger");

	public static string ExtendedStoredProcedure => Keys.GetString("ExtendedStoredProcedure");

	public static string GlobalVariable => Keys.GetString("GlobalVariable");

	public static string LocalVariable => Keys.GetString("LocalVariable");

	public static string Login => Keys.GetString("Login");

	public static string Parameter => Keys.GetString("Parameter");

	public static string PivotTable => Keys.GetString("PivotTable");

	public static string ScalarValuedFunction => Keys.GetString("ScalarValuedFunction");

	public static string Schema => Keys.GetString("Schema");

	public static string Server => Keys.GetString("Server");

	public static string ServerDdlTrigger => Keys.GetString("ServerDdlTrigger");

	public static string StoredProcedure => Keys.GetString("StoredProcedure");

	public static string Synonym => Keys.GetString("Synonym");

	public static string SystemDataType => Keys.GetString("SystemDataType");

	public static string Table => Keys.GetString("Table");

	public static string TableAlias => Keys.GetString("TableAlias");

	public static string TableValuedFunction => Keys.GetString("TableValuedFunction");

	public static string TemporaryTable => Keys.GetString("TemporaryTable");

	public static string UnpivotTable => Keys.GetString("UnpivotTable");

	public static string User => Keys.GetString("User");

	public static string UserDefinedAggregate => Keys.GetString("UserDefinedAggregate");

	public static string UserDefinedClrType => Keys.GetString("UserDefinedClrType");

	public static string UserDefinedDataType => Keys.GetString("UserDefinedDataType");

	public static string UserDefinedTableType => Keys.GetString("UserDefinedTableType");

	public static string View => Keys.GetString("View");

	public static string XmlDataTypeMethod => Keys.GetString("XmlDataTypeMethod");

	public static string DeserializationExpectedSingleXmlElement => Keys.GetString("DeserializationExpectedSingleXmlElement");

	public static string DeserializationUnexpectedXmlContent => Keys.GetString("DeserializationUnexpectedXmlContent");

	public static string InvalidRootTypes => Keys.GetString("InvalidRootTypes");

	public static string SqlScriptMustHaveOnlyModuleStatement => Keys.GetString("SqlScriptMustHaveOnlyModuleStatement");

	public static string SqlScriptMustHaveOnlyTriggerStatement => Keys.GetString("SqlScriptMustHaveOnlyTriggerStatement");

	public static string SqlScriptMustHaveOnlyViewStatement => Keys.GetString("SqlScriptMustHaveOnlyViewStatement");

	protected SR()
	{
	}

	public static string UnclosedQuotationMark(string tokenText)
	{
		return Keys.GetString("UnclosedQuotationMark", tokenText);
	}

	public static string IncorrectSyntaxNear(string tokenNear)
	{
		return Keys.GetString("IncorrectSyntaxNear", tokenNear);
	}

	public static string IncorrectSyntaxNearKeyword(string keyword)
	{
		return Keys.GetString("IncorrectSyntaxNearKeyword", keyword);
	}

	public static string IncorrectOptionValue(string identifier, string value)
	{
		return Keys.GetString("IncorrectOptionValue", identifier, value);
	}

	public static string IncorrectOptionOrder(string identifier)
	{
		return Keys.GetString("IncorrectOptionOrder", identifier);
	}

	public static string OptionNotRecognized(string identifier)
	{
		return Keys.GetString("OptionNotRecognized", identifier);
	}

	public static string OptionSpecifiedMultipleTimes(string optionaName)
	{
		return Keys.GetString("OptionSpecifiedMultipleTimes", optionaName);
	}

	public static string MaximumPrecisionOutOfRange(string number)
	{
		return Keys.GetString("MaximumPrecisionOutOfRange", number);
	}

	public static string MustBeOnlyStatementInBatch(string statement)
	{
		return Keys.GetString("MustBeOnlyStatementInBatch", statement);
	}

	public static string StatementNotSupportedInIntelliSense(string statement)
	{
		return Keys.GetString("StatementNotSupportedInIntelliSense", statement);
	}

	public static string IntegerValueOutOfRange(string value)
	{
		return Keys.GetString("IntegerValueOutOfRange", value);
	}

	public static string CannotPerformAlterOnObject(string name)
	{
		return Keys.GetString("CannotPerformAlterOnObject", name);
	}

	public static string TypeIsNotSystemType(string name)
	{
		return Keys.GetString("TypeIsNotSystemType", name);
	}

	public static string SchemaNotExist(string schemaName)
	{
		return Keys.GetString("SchemaNotExist", schemaName);
	}

	public static string ColumnSpecifiedMultipleTimes(string columnName, string identifier)
	{
		return Keys.GetString("ColumnSpecifiedMultipleTimes", columnName, identifier);
	}

	public static string InvalidObjectName(string identifier)
	{
		return Keys.GetString("InvalidObjectName", identifier);
	}

	public static string TooManyArguments(string procName)
	{
		return Keys.GetString("TooManyArguments", procName);
	}

	public static string InsufficientArguments(string procName)
	{
		return Keys.GetString("InsufficientArguments", procName);
	}

	public static string ColumnPrefixMismatch(string prefixName)
	{
		return Keys.GetString("ColumnPrefixMismatch", prefixName);
	}

	public static string VariableNameNotUnique(string variableName)
	{
		return Keys.GetString("VariableNameNotUnique", variableName);
	}

	public static string ObjectNotExistOrIsInvalid(string identifier)
	{
		return Keys.GetString("ObjectNotExistOrIsInvalid", identifier);
	}

	public static string CannotCreateSparseColumn(string columnName, string tableName)
	{
		return Keys.GetString("CannotCreateSparseColumn", columnName, tableName);
	}

	public static string CannotCreateDefaultConstraintOnSparseColumn(string columnName, string tableName)
	{
		return Keys.GetString("CannotCreateDefaultConstraintOnSparseColumn", columnName, tableName);
	}

	public static string ColumnIsInvalidForUseAsKeyColumnInIndex(string columnName, string tableName)
	{
		return Keys.GetString("ColumnIsInvalidForUseAsKeyColumnInIndex", columnName, tableName);
	}

	public static string CannotCreateMoreThanOneColumnSetOnTable(string columnName, string tableName)
	{
		return Keys.GetString("CannotCreateMoreThanOneColumnSetOnTable", columnName, tableName);
	}

	public static string CannotCreateSparseColumnSetOnTable(string columnName, string tableName)
	{
		return Keys.GetString("CannotCreateSparseColumnSetOnTable", columnName, tableName);
	}

	public static string CannotCreateGeneratedAlwaysColumnType(string columnName)
	{
		return Keys.GetString("CannotCreateGeneratedAlwaysColumnType", columnName);
	}

	public static string CannotCreateGeneratedAlwaysColumnNullable(string columnName)
	{
		return Keys.GetString("CannotCreateGeneratedAlwaysColumnNullable", columnName);
	}

	public static string InvalidBuildModeSqlNullStatement(string statement)
	{
		return Keys.GetString("InvalidBuildModeSqlNullStatement", statement);
	}

	public static string InvalidBuildModeDataTypeUse(string dataType)
	{
		return Keys.GetString("InvalidBuildModeDataTypeUse", dataType);
	}

	public static string InvalidExecuteAsOption(string optionName)
	{
		return Keys.GetString("InvalidExecuteAsOption", optionName);
	}

	public static string AmbiguousColumnName(string columnName)
	{
		return Keys.GetString("AmbiguousColumnName", columnName);
	}

	public static string InvalidColumnName(string columnName)
	{
		return Keys.GetString("InvalidColumnName", columnName);
	}

	public static string MultiPartIdentifierBindingError(string identifier)
	{
		return Keys.GetString("MultiPartIdentifierBindingError", identifier);
	}

	public static string InvalidColumnXmlNodeUse(string columnName)
	{
		return Keys.GetString("InvalidColumnXmlNodeUse", columnName);
	}

	public static string DatabaseNotExist(string databaseName)
	{
		return Keys.GetString("DatabaseNotExist", databaseName);
	}

	public static string DatabaseObjectExist(string identifier)
	{
		return Keys.GetString("DatabaseObjectExist", identifier);
	}

	public static string UnrecognizedCreateTableOption(string phrase)
	{
		return Keys.GetString("UnrecognizedCreateTableOption", phrase);
	}

	public static string MoreColumns(string identifier)
	{
		return Keys.GetString("MoreColumns", identifier);
	}

	public static string FewerColumns(string identifier)
	{
		return Keys.GetString("FewerColumns", identifier);
	}

	public static string IsTableValuedFunction(string procName)
	{
		return Keys.GetString("IsTableValuedFunction", procName);
	}

	public static string CouldNotLocateDatabase(string dbName)
	{
		return Keys.GetString("CouldNotLocateDatabase", dbName);
	}

	public static string CannotFindStoredProcedure(string procName)
	{
		return Keys.GetString("CannotFindStoredProcedure", procName);
	}

	public static string MissingParameters(string procName)
	{
		return Keys.GetString("MissingParameters", procName);
	}

	public static string InvalidParameter(string parameterName, string procName)
	{
		return Keys.GetString("InvalidParameter", parameterName, procName);
	}

	public static string ParameterSuppliedMultipleTimes(string parameterName)
	{
		return Keys.GetString("ParameterSuppliedMultipleTimes", parameterName);
	}

	public static string OutputParameterMismatch(string parameterName)
	{
		return Keys.GetString("OutputParameterMismatch", parameterName);
	}

	public static string MissingParameter(string procedureName, string parameterName)
	{
		return Keys.GetString("MissingParameter", procedureName, parameterName);
	}

	public static string InconsistentParameterFormat(int paramNumber)
	{
		return Keys.GetString("InconsistentParameterFormat", paramNumber);
	}

	public static string MissingColumn(int order, string tableName)
	{
		return Keys.GetString("MissingColumn", order, tableName);
	}

	public static string InvalidUsageOfIndexOption(string option)
	{
		return Keys.GetString("InvalidUsageOfIndexOption", option);
	}

	public static string InvalidUsageOfScopedConfiguration(string option)
	{
		return Keys.GetString("InvalidUsageOfScopedConfiguration", option);
	}

	public static string UnrecognizedCursorOption(string option)
	{
		return Keys.GetString("UnrecognizedCursorOption", option);
	}

	public static string InvalidUsageOfCursorOption(string option)
	{
		return Keys.GetString("InvalidUsageOfCursorOption", option);
	}

	public static string ConflictingCursorOption(string option1, string option2)
	{
		return Keys.GetString("ConflictingCursorOption", option1, option2);
	}

	public static string IndexOrStatisticsExists(string name, string tableName)
	{
		return Keys.GetString("IndexOrStatisticsExists", name, tableName);
	}

	public static string ClusteredIndexExists(string tableName, string existingName)
	{
		return Keys.GetString("ClusteredIndexExists", tableName, existingName);
	}

	public static string CouldNotFindIndex(string indexName, string tableName)
	{
		return Keys.GetString("CouldNotFindIndex", indexName, tableName);
	}

	public static string CannotConvertXmlOrSpatialIndexToRelational(string indexName)
	{
		return Keys.GetString("CannotConvertXmlOrSpatialIndexToRelational", indexName);
	}

	public static string CannotCreateIndexOnViewNotSchemaBound(string viewName)
	{
		return Keys.GetString("CannotCreateIndexOnViewNotSchemaBound", viewName);
	}

	public static string CannotCreateIndexOnViewDoesNotHaveUniqueClusteredIndex(string viewName)
	{
		return Keys.GetString("CannotCreateIndexOnViewDoesNotHaveUniqueClusteredIndex", viewName);
	}

	public static string CannotCreateNonuniqueClusteredIndexOnView(string viewName)
	{
		return Keys.GetString("CannotCreateNonuniqueClusteredIndexOnView", viewName);
	}

	public static string InvalidIndexKeyColumnType(string columnName, string tableName)
	{
		return Keys.GetString("InvalidIndexKeyColumnType", columnName, tableName);
	}

	public static string InvalidIndexIncludedColumnType(string columnName, string tableName)
	{
		return Keys.GetString("InvalidIndexIncludedColumnType", columnName, tableName);
	}

	public static string CannotCreateIndexOnViewContainsInvalidColumns(string viewName)
	{
		return Keys.GetString("CannotCreateIndexOnViewContainsInvalidColumns", viewName);
	}

	public static string OnlineOperationCannotBePerformedOnIndexInvalidColumns(string indexName)
	{
		return Keys.GetString("OnlineOperationCannotBePerformedOnIndexInvalidColumns", indexName);
	}

	public static string InvalidFillFactorPercentage(string value)
	{
		return Keys.GetString("InvalidFillFactorPercentage", value);
	}

	public static string IncorrectWhereClauseForFilteredIndex(string name, string table)
	{
		return Keys.GetString("IncorrectWhereClauseForFilteredIndex", name, table);
	}

	public static string ParamVarHasInvalidDataType(string name)
	{
		return Keys.GetString("ParamVarHasInvalidDataType", name);
	}

	public static string ColumnHasInvalidDataType(string name)
	{
		return Keys.GetString("ColumnHasInvalidDataType", name);
	}

	public static string OutOfRangeDegreeOfParallelism(string degreeOfParallelism)
	{
		return Keys.GetString("OutOfRangeDegreeOfParallelism", degreeOfParallelism);
	}

	public static string ParameterCannotBeReadOnly(string paramName)
	{
		return Keys.GetString("ParameterCannotBeReadOnly", paramName);
	}

	public static string TableValuedParameterMustBeReadOnly(string paramName)
	{
		return Keys.GetString("TableValuedParameterMustBeReadOnly", paramName);
	}

	public static string OperandTypeClash(string type1, string type2)
	{
		return Keys.GetString("OperandTypeClash", type1, type2);
	}

	public static string ScalarVariableRequired(string scalarvariableName)
	{
		return Keys.GetString("ScalarVariableRequired", scalarvariableName);
	}

	public static string ColumnNameNotUnique(string columnName, string tableName)
	{
		return Keys.GetString("ColumnNameNotUnique", columnName, tableName);
	}

	public static string MultiplePrimaryKey(string tableName)
	{
		return Keys.GetString("MultiplePrimaryKey", tableName);
	}

	public static string CorrelationNameNotUnique(string correlationName)
	{
		return Keys.GetString("CorrelationNameNotUnique", correlationName);
	}

	public static string InvalidCorrelationNameWithTable(string correlationName, string tableName)
	{
		return Keys.GetString("InvalidCorrelationNameWithTable", correlationName, tableName);
	}

	public static string InvalidCorrelationNamesInFrom(string identifier1, string identifier2)
	{
		return Keys.GetString("InvalidCorrelationNamesInFrom", identifier1, identifier2);
	}

	public static string ParametersNotSuppliedForFunction(string name)
	{
		return Keys.GetString("ParametersNotSuppliedForFunction", name);
	}

	public static string TableIsAmbiguous(string name)
	{
		return Keys.GetString("TableIsAmbiguous", name);
	}

	public static string ParametersSuppliedForNonFunction(string name)
	{
		return Keys.GetString("ParametersSuppliedForNonFunction", name);
	}

	public static string FunctionCannotBeUsedToMatchTarget(string name)
	{
		return Keys.GetString("FunctionCannotBeUsedToMatchTarget", name);
	}

	public static string ObjectCannotBeModified(string name)
	{
		return Keys.GetString("ObjectCannotBeModified", name);
	}

	public static string TableVariableRequired(string tableVariableName)
	{
		return Keys.GetString("TableVariableRequired", tableVariableName);
	}

	public static string OperatorNotSupported(string operatorName)
	{
		return Keys.GetString("OperatorNotSupported", operatorName);
	}

	public static string DataTypeMissing(string columnName)
	{
		return Keys.GetString("DataTypeMissing", columnName);
	}

	public static string ColumnConstraintNotUnique(string constraintName, string columnName, string tableName)
	{
		return Keys.GetString("ColumnConstraintNotUnique", constraintName, columnName, tableName);
	}

	public static string PrimaryKeyNotUnique(string columnName, string tableName)
	{
		return Keys.GetString("PrimaryKeyNotUnique", columnName, tableName);
	}

	public static string CannotHaveDefaultsOnIdentity(string tableName, string columnName)
	{
		return Keys.GetString("CannotHaveDefaultsOnIdentity", tableName, columnName);
	}

	public static string CannotCreateIdentityOnNullable(string columnName, string tableName)
	{
		return Keys.GetString("CannotCreateIdentityOnNullable", columnName, tableName);
	}

	public static string CannotDefinePrimaryKeyOnNullable(string columnName)
	{
		return Keys.GetString("CannotDefinePrimaryKeyOnNullable", columnName);
	}

	public static string ColumnHasUserDefinedTableType(string columnName)
	{
		return Keys.GetString("ColumnHasUserDefinedTableType", columnName);
	}

	public static string InvalidSeed(string columnName)
	{
		return Keys.GetString("InvalidSeed", columnName);
	}

	public static string InvalidIncrement(string columnName)
	{
		return Keys.GetString("InvalidIncrement", columnName);
	}

	public static string IdentityColumnInvalidType(string columnName)
	{
		return Keys.GetString("IdentityColumnInvalidType", columnName);
	}

	public static string ColumnNameNotInTargetTable(string columnName)
	{
		return Keys.GetString("ColumnNameNotInTargetTable", columnName);
	}

	public static string ForeignKeyReferencesInvalidTable(string fkName, string tableName)
	{
		return Keys.GetString("ForeignKeyReferencesInvalidTable", fkName, tableName);
	}

	public static string ForeignKeyNumberOfRefColumnsDiffers(string tableName)
	{
		return Keys.GetString("ForeignKeyNumberOfRefColumnsDiffers", tableName);
	}

	public static string NoPrimaryKeysInReferencedTable(string fkName, string tableName)
	{
		return Keys.GetString("NoPrimaryKeysInReferencedTable", fkName, tableName);
	}

	public static string ForeignKeyReferencesImplicitlyTableWithoutPrimaryKey(string fkName, string tableName)
	{
		return Keys.GetString("ForeignKeyReferencesImplicitlyTableWithoutPrimaryKey", fkName, tableName);
	}

	public static string ColumnIsNotSameTypeAsRefColumn(string fkName, string tableName, string columnName, string refTableName, string refColumName)
	{
		return Keys.GetString("ColumnIsNotSameTypeAsRefColumn", fkName, tableName, columnName, refTableName, refColumName);
	}

	public static string ForeignKeyInvalidReferencingColumn(string fkName, string tableName, string columnName)
	{
		return Keys.GetString("ForeignKeyInvalidReferencingColumn", fkName, tableName, columnName);
	}

	public static string ForeignKeyInvalidReferencedColumn(string fkName, string tableName, string columnName)
	{
		return Keys.GetString("ForeignKeyInvalidReferencedColumn", fkName, tableName, columnName);
	}

	public static string InvalidUseOfSideEffectingOperatorWithinFunction(string name)
	{
		return Keys.GetString("InvalidUseOfSideEffectingOperatorWithinFunction", name);
	}

	public static string InvalidProcedureNumberRange(int procNumber)
	{
		return Keys.GetString("InvalidProcedureNumberRange", procNumber);
	}

	public static string InvalidTriggerEventType(string eventName)
	{
		return Keys.GetString("InvalidTriggerEventType", eventName);
	}

	public static string InvalidTriggerEventTypeOnDatabase(string eventName)
	{
		return Keys.GetString("InvalidTriggerEventTypeOnDatabase", eventName);
	}

	public static string DuplicateTriggerEventType(string eventName)
	{
		return Keys.GetString("DuplicateTriggerEventType", eventName);
	}

	public static string DuplicateTriggerActionType(string actionName)
	{
		return Keys.GetString("DuplicateTriggerActionType", actionName);
	}

	public static string InvalidTriggerSchema(string triggerName)
	{
		return Keys.GetString("InvalidTriggerSchema", triggerName);
	}

	public static string TriggerDoesNotBelongToTarget(string triggerName, string targetName)
	{
		return Keys.GetString("TriggerDoesNotBelongToTarget", triggerName, targetName);
	}

	public static string RequiredInsteadOfTriggerOnView(string triggerName, string targetName)
	{
		return Keys.GetString("RequiredInsteadOfTriggerOnView", triggerName, targetName);
	}

	public static string DuplicateInsteadOfTrigger(string triggerName, string targetName, string actionName)
	{
		return Keys.GetString("DuplicateInsteadOfTrigger", triggerName, targetName, actionName);
	}

	public static string CannotCreateTriggerOnViewWithCheckOption(string triggerName, string targetName)
	{
		return Keys.GetString("CannotCreateTriggerOnViewWithCheckOption", triggerName, targetName);
	}

	public static string CannotCreateInsteadOfTriggerOnTableWithCascade(string triggerName, string targetName, string actionName)
	{
		return Keys.GetString("CannotCreateInsteadOfTriggerOnTableWithCascade", triggerName, targetName, actionName);
	}

	public static string InvalidLengthOrPrecision(int length)
	{
		return Keys.GetString("InvalidLengthOrPrecision", length);
	}

	public static string InvalidScale(int scale)
	{
		return Keys.GetString("InvalidScale", scale);
	}

	public static string MaximumSizeError(int size, string typeName, int maximum)
	{
		return Keys.GetString("MaximumSizeError", size, typeName, maximum);
	}

	public static string MaximumSizeErrorForAnyType(int size, string typeName, int maximum)
	{
		return Keys.GetString("MaximumSizeErrorForAnyType", size, typeName, maximum);
	}

	public static string TypeNameMaxPrefixError(string name, int maximum)
	{
		return Keys.GetString("TypeNameMaxPrefixError", name, maximum);
	}

	public static string XmlSchemaCollectionMaxPrefixError(string name)
	{
		return Keys.GetString("XmlSchemaCollectionMaxPrefixError", name);
	}

	public static string InvalidTableHint(string tabelHint)
	{
		return Keys.GetString("InvalidTableHint", tabelHint);
	}

	public static string TableConstraintHasNoColumnList(string tableName)
	{
		return Keys.GetString("TableConstraintHasNoColumnList", tableName);
	}

	public static string DuplicateColumnNamesInIndex(string columnName)
	{
		return Keys.GetString("DuplicateColumnNamesInIndex", columnName);
	}

	public static string NotRecognizedFunctionName(string functionName)
	{
		return Keys.GetString("NotRecognizedFunctionName", functionName);
	}

	public static string CannotFindFunctionOrAggregate(string columnName, string aggregateName)
	{
		return Keys.GetString("CannotFindFunctionOrAggregate", columnName, aggregateName);
	}

	public static string RemoteFunctionRefIsNotAllowed(string functionName, string columnName)
	{
		return Keys.GetString("RemoteFunctionRefIsNotAllowed", functionName, columnName);
	}

	public static string AnonimousColumn(string name)
	{
		return Keys.GetString("AnonimousColumn", name);
	}

	public static string InvalidGroupByOption(string name)
	{
		return Keys.GetString("InvalidGroupByOption", name);
	}

	public static string CannotDropObject(string objectName, string objectType)
	{
		return Keys.GetString("CannotDropObject", objectName, objectType);
	}

	public static string CannotUseDrop(string incorrectStmt, string correctStmt, string objectName, string objectType)
	{
		return Keys.GetString("CannotUseDrop", incorrectStmt, correctStmt, objectName, objectType);
	}

	public static string CouldNotLocateEntryInSysdatabases(string name)
	{
		return Keys.GetString("CouldNotLocateEntryInSysdatabases", name);
	}

	public static string FunctionRequiresNumberOfArguments(string functionName, int numberOfArguments)
	{
		return Keys.GetString("FunctionRequiresNumberOfArguments", functionName, numberOfArguments);
	}

	public static string FunctionRequiresRangeOfAruments(string functionName, int rangeStart, int rangeEnd)
	{
		return Keys.GetString("FunctionRequiresRangeOfAruments", functionName, rangeStart, rangeEnd);
	}

	public static string FunctionRequiresZeroArguments(string functionName)
	{
		return Keys.GetString("FunctionRequiresZeroArguments", functionName);
	}

	public static string FunctionRequiresAtLeastNumberOfArguments(string functionName, int numberOfArguments)
	{
		return Keys.GetString("FunctionRequiresAtLeastNumberOfArguments", functionName, numberOfArguments);
	}

	public static string InvalidParameterOne(string functionName)
	{
		return Keys.GetString("InvalidParameterOne", functionName);
	}

	public static string NotRecognizedDatePartOption(string option, string functionName)
	{
		return Keys.GetString("NotRecognizedDatePartOption", option, functionName);
	}

	public static string InvalidAggregateFunction(string name)
	{
		return Keys.GetString("InvalidAggregateFunction", name);
	}

	public static string DuplicateCteName(string name)
	{
		return Keys.GetString("DuplicateCteName", name);
	}

	public static string RecursiveCteHasNoUnionAll(string name)
	{
		return Keys.GetString("RecursiveCteHasNoUnionAll", name);
	}

	public static string NoAnchorMemberForRecursiveQuery(string name)
	{
		return Keys.GetString("NoAnchorMemberForRecursiveQuery", name);
	}

	public static string RecursiveCteMemberHasMultipleRefs(string name)
	{
		return Keys.GetString("RecursiveCteMemberHasMultipleRefs", name);
	}

	public static string AnchorMemberFoundInRecursiveQuery(string name)
	{
		return Keys.GetString("AnchorMemberFoundInRecursiveQuery", name);
	}

	public static string ColumnNameConflictsInPivot(string columnName)
	{
		return Keys.GetString("ColumnNameConflictsInPivot", columnName);
	}

	public static string ColumnNameConflictsInUnpivot(string columnName)
	{
		return Keys.GetString("ColumnNameConflictsInUnpivot", columnName);
	}

	public static string ColumnSpecifiedMultipleTimesInUnpivot(string columnName)
	{
		return Keys.GetString("ColumnSpecifiedMultipleTimesInUnpivot", columnName);
	}

	public static string OrderByItemContainsVariable(int position)
	{
		return Keys.GetString("OrderByItemContainsVariable", position);
	}

	public static string OrderByPositionNumberIsOutOfRange(int position)
	{
		return Keys.GetString("OrderByPositionNumberIsOutOfRange", position);
	}

	public static string OrderByListHasConstantExpression(int position)
	{
		return Keys.GetString("OrderByListHasConstantExpression", position);
	}

	public static string SetClauseColumnSpecifiedMultipleTimes(string columnName)
	{
		return Keys.GetString("SetClauseColumnSpecifiedMultipleTimes", columnName);
	}

	public static string OutputIntoTargetCannotBeViewOrCte(string tableName)
	{
		return Keys.GetString("OutputIntoTargetCannotBeViewOrCte", tableName);
	}

	public static string FunctionNotAllowedInOutput(string name)
	{
		return Keys.GetString("FunctionNotAllowedInOutput", name);
	}

	public static string ExplicitValueForIdentityColumn(string name)
	{
		return Keys.GetString("ExplicitValueForIdentityColumn", name);
	}

	public static string CannotCallMethodsOnType(string typeName)
	{
		return Keys.GetString("CannotCallMethodsOnType", typeName);
	}

	public static string UdtMemberIsNotStatic(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("UdtMemberIsNotStatic", memberName, className, assemblyName);
	}

	public static string UdtMemberIsStatic(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("UdtMemberIsStatic", memberName, className, assemblyName);
	}

	public static string UdtPropertyIsNotStatic(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("UdtPropertyIsNotStatic", memberName, className, assemblyName);
	}

	public static string UdtPropertyIsStatic(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("UdtPropertyIsStatic", memberName, className, assemblyName);
	}

	public static string CouldNotFindPropertyOrField(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("CouldNotFindPropertyOrField", memberName, className, assemblyName);
	}

	public static string CouldNotFindMethod(string memberName, string className, string assemblyName)
	{
		return Keys.GetString("CouldNotFindMethod", memberName, className, assemblyName);
	}

	public static string NotValidFunctionOrProperty(string memberName)
	{
		return Keys.GetString("NotValidFunctionOrProperty", memberName);
	}

	public static string IncorrectSyntaxToInvokeXmlMethod(string methodName)
	{
		return Keys.GetString("IncorrectSyntaxToInvokeXmlMethod", methodName);
	}

	public static string LoginExist(string identifier)
	{
		return Keys.GetString("LoginExist", identifier);
	}

	public static string UserExist(string identifier)
	{
		return Keys.GetString("UserExist", identifier);
	}

	public static string CouldNotFindCredential(string name)
	{
		return Keys.GetString("CouldNotFindCredential", name);
	}

	public static string CouldNotFindCertificate(string name)
	{
		return Keys.GetString("CouldNotFindCertificate", name);
	}

	public static string CouldNotFindAsymmetricKey(string name)
	{
		return Keys.GetString("CouldNotFindAsymmetricKey", name);
	}

	public static string CouldNotFindLogin(string name)
	{
		return Keys.GetString("CouldNotFindLogin", name);
	}

	public static string UserDefinedTypeExist(string identifier)
	{
		return Keys.GetString("UserDefinedTypeExist", identifier);
	}

	public static string InvalidBaseTypeForAlias(string identifier)
	{
		return Keys.GetString("InvalidBaseTypeForAlias", identifier);
	}

	public static string CannotFindUser(string userName)
	{
		return Keys.GetString("CannotFindUser", userName);
	}

	public static string UserGroupOrRoleExists(string roleName)
	{
		return Keys.GetString("UserGroupOrRoleExists", roleName);
	}

	public static string RequiredParam(string roleName)
	{
		return Keys.GetString("RequiredParam", roleName);
	}

	public static string DuplicateParam(string roleName)
	{
		return Keys.GetString("DuplicateParam", roleName);
	}

	public static string InvalidCollation(string collationName)
	{
		return Keys.GetString("InvalidCollation", collationName);
	}

	public static string ExpressionTypeInvalidForCollate(string dataType)
	{
		return Keys.GetString("ExpressionTypeInvalidForCollate", dataType);
	}

	public static string DeserializationInvalidXmlElement1(string elementName)
	{
		return Keys.GetString("DeserializationInvalidXmlElement1", elementName);
	}

	public static string DeserializationInvalidXmlElement2(string elementName, string expectedName)
	{
		return Keys.GetString("DeserializationInvalidXmlElement2", elementName, expectedName);
	}

	public static string DeserializationMissingXmlElement(string elementName)
	{
		return Keys.GetString("DeserializationMissingXmlElement", elementName);
	}

	public static string DeserializationMissingXmlAttribute(string attributeName)
	{
		return Keys.GetString("DeserializationMissingXmlAttribute", attributeName);
	}

	public static string DeserializationInvalidObjectType(string typeName, string expectedName)
	{
		return Keys.GetString("DeserializationInvalidObjectType", typeName, expectedName);
	}

	public static string DeserializationObjectMustHaveOneParent(string objKey, string parentKey1, string parentKey2)
	{
		return Keys.GetString("DeserializationObjectMustHaveOneParent", objKey, parentKey1, parentKey2);
	}

	public static string DeserializationUnresolvedExternalReference(string objKey)
	{
		return Keys.GetString("DeserializationUnresolvedExternalReference", objKey);
	}
}
