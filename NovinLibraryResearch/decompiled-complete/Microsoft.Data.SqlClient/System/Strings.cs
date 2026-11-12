using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Strings
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
				ResourceManager resourceManager = new ResourceManager("SqlClient.Resources.Strings", typeof(Strings).Assembly);
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

	internal static string ADP_AdapterMappingExceptionMessage => ResourceManager.GetString("ADP_AdapterMappingExceptionMessage", resourceCulture);

	internal static string ADP_Ascending => ResourceManager.GetString("ADP_Ascending", resourceCulture);

	internal static string ADP_BadParameterName => ResourceManager.GetString("ADP_BadParameterName", resourceCulture);

	internal static string ADP_CalledTwice => ResourceManager.GetString("ADP_CalledTwice", resourceCulture);

	internal static string ADP_ClosedConnectionError => ResourceManager.GetString("ADP_ClosedConnectionError", resourceCulture);

	internal static string ADP_CollectionIndexInt32 => ResourceManager.GetString("ADP_CollectionIndexInt32", resourceCulture);

	internal static string ADP_CollectionIndexString => ResourceManager.GetString("ADP_CollectionIndexString", resourceCulture);

	internal static string ADP_CollectionInvalidType => ResourceManager.GetString("ADP_CollectionInvalidType", resourceCulture);

	internal static string ADP_CollectionIsNotParent => ResourceManager.GetString("ADP_CollectionIsNotParent", resourceCulture);

	internal static string ADP_CollectionIsParent => ResourceManager.GetString("ADP_CollectionIsParent", resourceCulture);

	internal static string ADP_CollectionNullValue => ResourceManager.GetString("ADP_CollectionNullValue", resourceCulture);

	internal static string ADP_CollectionRemoveInvalidObject => ResourceManager.GetString("ADP_CollectionRemoveInvalidObject", resourceCulture);

	internal static string ADP_CollectionUniqueValue => ResourceManager.GetString("ADP_CollectionUniqueValue", resourceCulture);

	internal static string ADP_ColumnSchemaExpression => ResourceManager.GetString("ADP_ColumnSchemaExpression", resourceCulture);

	internal static string ADP_ColumnSchemaMismatch => ResourceManager.GetString("ADP_ColumnSchemaMismatch", resourceCulture);

	internal static string ADP_ColumnSchemaMissing1 => ResourceManager.GetString("ADP_ColumnSchemaMissing1", resourceCulture);

	internal static string ADP_ColumnSchemaMissing2 => ResourceManager.GetString("ADP_ColumnSchemaMissing2", resourceCulture);

	internal static string ADP_CommandTextRequired => ResourceManager.GetString("ADP_CommandTextRequired", resourceCulture);

	internal static string ADP_ComputerNameEx => ResourceManager.GetString("ADP_ComputerNameEx", resourceCulture);

	internal static string ADP_ConnecitonRequired_UpdateRows => ResourceManager.GetString("ADP_ConnecitonRequired_UpdateRows", resourceCulture);

	internal static string ADP_ConnectionAlreadyOpen => ResourceManager.GetString("ADP_ConnectionAlreadyOpen", resourceCulture);

	internal static string ADP_ConnectionIsDisabled => ResourceManager.GetString("ADP_ConnectionIsDisabled", resourceCulture);

	internal static string ADP_ConnectionRequired => ResourceManager.GetString("ADP_ConnectionRequired", resourceCulture);

	internal static string ADP_ConnectionRequired_Batch => ResourceManager.GetString("ADP_ConnectionRequired_Batch", resourceCulture);

	internal static string ADP_ConnectionRequired_Clone => ResourceManager.GetString("ADP_ConnectionRequired_Clone", resourceCulture);

	internal static string ADP_ConnectionRequired_Delete => ResourceManager.GetString("ADP_ConnectionRequired_Delete", resourceCulture);

	internal static string ADP_ConnectionRequired_Fill => ResourceManager.GetString("ADP_ConnectionRequired_Fill", resourceCulture);

	internal static string ADP_ConnectionRequired_FillPage => ResourceManager.GetString("ADP_ConnectionRequired_FillPage", resourceCulture);

	internal static string ADP_ConnectionRequired_FillSchema => ResourceManager.GetString("ADP_ConnectionRequired_FillSchema", resourceCulture);

	internal static string ADP_ConnectionRequired_Insert => ResourceManager.GetString("ADP_ConnectionRequired_Insert", resourceCulture);

	internal static string ADP_ConnectionRequired_Update => ResourceManager.GetString("ADP_ConnectionRequired_Update", resourceCulture);

	internal static string ADP_ConnectionStateMsg => ResourceManager.GetString("ADP_ConnectionStateMsg", resourceCulture);

	internal static string ADP_ConnectionStateMsg_Closed => ResourceManager.GetString("ADP_ConnectionStateMsg_Closed", resourceCulture);

	internal static string ADP_ConnectionStateMsg_Connecting => ResourceManager.GetString("ADP_ConnectionStateMsg_Connecting", resourceCulture);

	internal static string ADP_ConnectionStateMsg_Open => ResourceManager.GetString("ADP_ConnectionStateMsg_Open", resourceCulture);

	internal static string ADP_ConnectionStateMsg_OpenExecuting => ResourceManager.GetString("ADP_ConnectionStateMsg_OpenExecuting", resourceCulture);

	internal static string ADP_ConnectionStateMsg_OpenFetching => ResourceManager.GetString("ADP_ConnectionStateMsg_OpenFetching", resourceCulture);

	internal static string ADP_ConnectionStringSyntax => ResourceManager.GetString("ADP_ConnectionStringSyntax", resourceCulture);

	internal static string ADP_DataAdapterExceptionMessage => ResourceManager.GetString("ADP_DataAdapterExceptionMessage", resourceCulture);

	internal static string ADP_DatabaseNameTooLong => ResourceManager.GetString("ADP_DatabaseNameTooLong", resourceCulture);

	internal static string ADP_DataReaderClosed => ResourceManager.GetString("ADP_DataReaderClosed", resourceCulture);

	internal static string ADP_DataReaderNoData => ResourceManager.GetString("ADP_DataReaderNoData", resourceCulture);

	internal static string ADP_DBConcurrencyExceptionMessage => ResourceManager.GetString("ADP_DBConcurrencyExceptionMessage", resourceCulture);

	internal static string ADP_DbDataUpdatableRecordReadOnly => ResourceManager.GetString("ADP_DbDataUpdatableRecordReadOnly", resourceCulture);

	internal static string ADP_DbRecordReadOnly => ResourceManager.GetString("ADP_DbRecordReadOnly", resourceCulture);

	internal static string ADP_DbTypeNotSupported => ResourceManager.GetString("ADP_DbTypeNotSupported", resourceCulture);

	internal static string ADP_DelegatedTransactionPresent => ResourceManager.GetString("ADP_DelegatedTransactionPresent", resourceCulture);

	internal static string ADP_DeriveParametersNotSupported => ResourceManager.GetString("ADP_DeriveParametersNotSupported", resourceCulture);

	internal static string ADP_Descending => ResourceManager.GetString("ADP_Descending", resourceCulture);

	internal static string ADP_DoubleValuedProperty => ResourceManager.GetString("ADP_DoubleValuedProperty", resourceCulture);

	internal static string ADP_DynamicSQLJoinUnsupported => ResourceManager.GetString("ADP_DynamicSQLJoinUnsupported", resourceCulture);

	internal static string ADP_DynamicSQLNestedQuote => ResourceManager.GetString("ADP_DynamicSQLNestedQuote", resourceCulture);

	internal static string ADP_DynamicSQLNoKeyInfoDelete => ResourceManager.GetString("ADP_DynamicSQLNoKeyInfoDelete", resourceCulture);

	internal static string ADP_DynamicSQLNoKeyInfoRowVersionDelete => ResourceManager.GetString("ADP_DynamicSQLNoKeyInfoRowVersionDelete", resourceCulture);

	internal static string ADP_DynamicSQLNoKeyInfoRowVersionUpdate => ResourceManager.GetString("ADP_DynamicSQLNoKeyInfoRowVersionUpdate", resourceCulture);

	internal static string ADP_DynamicSQLNoKeyInfoUpdate => ResourceManager.GetString("ADP_DynamicSQLNoKeyInfoUpdate", resourceCulture);

	internal static string ADP_DynamicSQLNoTableInfo => ResourceManager.GetString("ADP_DynamicSQLNoTableInfo", resourceCulture);

	internal static string ADP_EmptyArray => ResourceManager.GetString("ADP_EmptyArray", resourceCulture);

	internal static string ADP_EmptyDatabaseName => ResourceManager.GetString("ADP_EmptyDatabaseName", resourceCulture);

	internal static string ADP_EmptyString => ResourceManager.GetString("ADP_EmptyString", resourceCulture);

	internal static string ADP_EvenLengthLiteralValue => ResourceManager.GetString("ADP_EvenLengthLiteralValue", resourceCulture);

	internal static string ADP_FillChapterAutoIncrement => ResourceManager.GetString("ADP_FillChapterAutoIncrement", resourceCulture);

	internal static string ADP_FillRequiresSourceTableName => ResourceManager.GetString("ADP_FillRequiresSourceTableName", resourceCulture);

	internal static string ADP_FillSchemaRequiresSourceTableName => ResourceManager.GetString("ADP_FillSchemaRequiresSourceTableName", resourceCulture);

	internal static string ADP_HexDigitLiteralValue => ResourceManager.GetString("ADP_HexDigitLiteralValue", resourceCulture);

	internal static string ADP_IncorrectAsyncResult => ResourceManager.GetString("ADP_IncorrectAsyncResult", resourceCulture);

	internal static string ADP_InternalConnectionError => ResourceManager.GetString("ADP_InternalConnectionError", resourceCulture);

	internal static string ADP_InternalProviderError => ResourceManager.GetString("ADP_InternalProviderError", resourceCulture);

	internal static string ADP_InvalidArgumentLength => ResourceManager.GetString("ADP_InvalidArgumentLength", resourceCulture);

	internal static string ADP_InvalidArgumentValue => ResourceManager.GetString("ADP_InvalidArgumentValue", resourceCulture);

	internal static string ADP_InvalidBufferSizeOrIndex => ResourceManager.GetString("ADP_InvalidBufferSizeOrIndex", resourceCulture);

	internal static string ADP_InvalidCommandTimeout => ResourceManager.GetString("ADP_InvalidCommandTimeout", resourceCulture);

	internal static string ADP_InvalidConnectionOptionValue => ResourceManager.GetString("ADP_InvalidConnectionOptionValue", resourceCulture);

	internal static string ADP_InvalidConnectionOptionValueLength => ResourceManager.GetString("ADP_InvalidConnectionOptionValueLength", resourceCulture);

	internal static string ADP_InvalidConnectTimeoutValue => ResourceManager.GetString("ADP_InvalidConnectTimeoutValue", resourceCulture);

	internal static string ADP_InvalidDataDirectory => ResourceManager.GetString("ADP_InvalidDataDirectory", resourceCulture);

	internal static string ADP_InvalidDataLength => ResourceManager.GetString("ADP_InvalidDataLength", resourceCulture);

	internal static string ADP_InvalidDataLength2 => ResourceManager.GetString("ADP_InvalidDataLength2", resourceCulture);

	internal static string ADP_InvalidDataType => ResourceManager.GetString("ADP_InvalidDataType", resourceCulture);

	internal static string ADP_InvalidDateTimeDigits => ResourceManager.GetString("ADP_InvalidDateTimeDigits", resourceCulture);

	internal static string ADP_InvalidDestinationBufferIndex => ResourceManager.GetString("ADP_InvalidDestinationBufferIndex", resourceCulture);

	internal static string ADP_InvalidEnumerationValue => ResourceManager.GetString("ADP_InvalidEnumerationValue", resourceCulture);

	internal static string ADP_InvalidFormatValue => ResourceManager.GetString("ADP_InvalidFormatValue", resourceCulture);

	internal static string ADP_InvalidImplicitConversion => ResourceManager.GetString("ADP_InvalidImplicitConversion", resourceCulture);

	internal static string ADP_InvalidKey => ResourceManager.GetString("ADP_InvalidKey", resourceCulture);

	internal static string ADP_InvalidMaximumScale => ResourceManager.GetString("ADP_InvalidMaximumScale", resourceCulture);

	internal static string ADP_InvalidMaxRecords => ResourceManager.GetString("ADP_InvalidMaxRecords", resourceCulture);

	internal static string ADP_InvalidMetaDataValue => ResourceManager.GetString("ADP_InvalidMetaDataValue", resourceCulture);

	internal static string ADP_InvalidMinMaxPoolSizeValues => ResourceManager.GetString("ADP_InvalidMinMaxPoolSizeValues", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfAccessTokenAndAuthentication => ResourceManager.GetString("ADP_InvalidMixedUsageOfAccessTokenAndAuthentication", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfAccessTokenAndContextConnection => ResourceManager.GetString("ADP_InvalidMixedUsageOfAccessTokenAndContextConnection", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfAccessTokenAndCredential => ResourceManager.GetString("ADP_InvalidMixedUsageOfAccessTokenAndCredential", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfAccessTokenAndIntegratedSecurity => ResourceManager.GetString("ADP_InvalidMixedUsageOfAccessTokenAndIntegratedSecurity", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfAccessTokenAndUserIDPassword => ResourceManager.GetString("ADP_InvalidMixedUsageOfAccessTokenAndUserIDPassword", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfCredentialAndAccessToken => ResourceManager.GetString("ADP_InvalidMixedUsageOfCredentialAndAccessToken", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfSecureAndClearCredential => ResourceManager.GetString("ADP_InvalidMixedUsageOfSecureAndClearCredential", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfSecureCredentialAndContextConnection => ResourceManager.GetString("ADP_InvalidMixedUsageOfSecureCredentialAndContextConnection", resourceCulture);

	internal static string ADP_InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity => ResourceManager.GetString("ADP_InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity", resourceCulture);

	internal static string ADP_InvalidMultipartName => ResourceManager.GetString("ADP_InvalidMultipartName", resourceCulture);

	internal static string ADP_InvalidMultipartNameQuoteUsage => ResourceManager.GetString("ADP_InvalidMultipartNameQuoteUsage", resourceCulture);

	internal static string ADP_InvalidMultipartNameToManyParts => ResourceManager.GetString("ADP_InvalidMultipartNameToManyParts", resourceCulture);

	internal static string ADP_InvalidOffsetValue => ResourceManager.GetString("ADP_InvalidOffsetValue", resourceCulture);

	internal static string ADP_InvalidPrefixSuffix => ResourceManager.GetString("ADP_InvalidPrefixSuffix", resourceCulture);

	internal static string ADP_InvalidSeekOrigin => ResourceManager.GetString("ADP_InvalidSeekOrigin", resourceCulture);

	internal static string ADP_InvalidSizeValue => ResourceManager.GetString("ADP_InvalidSizeValue", resourceCulture);

	internal static string ADP_InvalidSourceBufferIndex => ResourceManager.GetString("ADP_InvalidSourceBufferIndex", resourceCulture);

	internal static string ADP_InvalidSourceColumn => ResourceManager.GetString("ADP_InvalidSourceColumn", resourceCulture);

	internal static string ADP_InvalidSourceTable => ResourceManager.GetString("ADP_InvalidSourceTable", resourceCulture);

	internal static string ADP_InvalidStartRecord => ResourceManager.GetString("ADP_InvalidStartRecord", resourceCulture);

	internal static string ADP_InvalidUDL => ResourceManager.GetString("ADP_InvalidUDL", resourceCulture);

	internal static string ADP_InvalidValue => ResourceManager.GetString("ADP_InvalidValue", resourceCulture);

	internal static string ADP_InvalidXMLBadVersion => ResourceManager.GetString("ADP_InvalidXMLBadVersion", resourceCulture);

	internal static string ADP_KeywordNotSupported => ResourceManager.GetString("ADP_KeywordNotSupported", resourceCulture);

	internal static string ADP_LiteralValueIsInvalid => ResourceManager.GetString("ADP_LiteralValueIsInvalid", resourceCulture);

	internal static string ADP_LocalTransactionPresent => ResourceManager.GetString("ADP_LocalTransactionPresent", resourceCulture);

	internal static string ADP_MismatchedAsyncResult => ResourceManager.GetString("ADP_MismatchedAsyncResult", resourceCulture);

	internal static string ADP_MissingColumnMapping => ResourceManager.GetString("ADP_MissingColumnMapping", resourceCulture);

	internal static string ADP_MissingConnectionOptionValue => ResourceManager.GetString("ADP_MissingConnectionOptionValue", resourceCulture);

	internal static string ADP_MissingDataReaderFieldType => ResourceManager.GetString("ADP_MissingDataReaderFieldType", resourceCulture);

	internal static string ADP_MissingSelectCommand => ResourceManager.GetString("ADP_MissingSelectCommand", resourceCulture);

	internal static string ADP_MissingSourceCommand => ResourceManager.GetString("ADP_MissingSourceCommand", resourceCulture);

	internal static string ADP_MissingSourceCommandConnection => ResourceManager.GetString("ADP_MissingSourceCommandConnection", resourceCulture);

	internal static string ADP_MissingTableMapping => ResourceManager.GetString("ADP_MissingTableMapping", resourceCulture);

	internal static string ADP_MissingTableMappingDestination => ResourceManager.GetString("ADP_MissingTableMappingDestination", resourceCulture);

	internal static string ADP_MissingTableSchema => ResourceManager.GetString("ADP_MissingTableSchema", resourceCulture);

	internal static string ADP_MultipleReturnValue => ResourceManager.GetString("ADP_MultipleReturnValue", resourceCulture);

	internal static string ADP_MustBeReadOnly => ResourceManager.GetString("ADP_MustBeReadOnly", resourceCulture);

	internal static string ADP_NegativeParameter => ResourceManager.GetString("ADP_NegativeParameter", resourceCulture);

	internal static string ADP_NoConnectionString => ResourceManager.GetString("ADP_NoConnectionString", resourceCulture);

	internal static string ADP_NonCLSException => ResourceManager.GetString("ADP_NonCLSException", resourceCulture);

	internal static string ADP_NonPooledOpenTimeout => ResourceManager.GetString("ADP_NonPooledOpenTimeout", resourceCulture);

	internal static string ADP_NonSeqByteAccess => ResourceManager.GetString("ADP_NonSeqByteAccess", resourceCulture);

	internal static string ADP_NonSequentialColumnAccess => ResourceManager.GetString("ADP_NonSequentialColumnAccess", resourceCulture);

	internal static string ADP_NoQuoteChange => ResourceManager.GetString("ADP_NoQuoteChange", resourceCulture);

	internal static string ADP_NoStoredProcedureExists => ResourceManager.GetString("ADP_NoStoredProcedureExists", resourceCulture);

	internal static string ADP_NotAPermissionElement => ResourceManager.GetString("ADP_NotAPermissionElement", resourceCulture);

	internal static string ADP_NotRowType => ResourceManager.GetString("ADP_NotRowType", resourceCulture);

	internal static string ADP_NotSupportedEnumerationValue => ResourceManager.GetString("ADP_NotSupportedEnumerationValue", resourceCulture);

	internal static string ADP_NullDataSet => ResourceManager.GetString("ADP_NullDataSet", resourceCulture);

	internal static string ADP_NullDataTable => ResourceManager.GetString("ADP_NullDataTable", resourceCulture);

	internal static string ADP_NumericToDecimalOverflow => ResourceManager.GetString("ADP_NumericToDecimalOverflow", resourceCulture);

	internal static string ADP_ObsoleteKeyword => ResourceManager.GetString("ADP_ObsoleteKeyword", resourceCulture);

	internal static string ADP_OdbcNoTypesFromProvider => ResourceManager.GetString("ADP_OdbcNoTypesFromProvider", resourceCulture);

	internal static string ADP_OffsetOutOfRangeException => ResourceManager.GetString("ADP_OffsetOutOfRangeException", resourceCulture);

	internal static string ADP_OnlyOneTableForStartRecordOrMaxRecords => ResourceManager.GetString("ADP_OnlyOneTableForStartRecordOrMaxRecords", resourceCulture);

	internal static string ADP_OpenConnectionPropertySet => ResourceManager.GetString("ADP_OpenConnectionPropertySet", resourceCulture);

	internal static string ADP_OpenConnectionRequired => ResourceManager.GetString("ADP_OpenConnectionRequired", resourceCulture);

	internal static string ADP_OpenConnectionRequired_Clone => ResourceManager.GetString("ADP_OpenConnectionRequired_Clone", resourceCulture);

	internal static string ADP_OpenConnectionRequired_Delete => ResourceManager.GetString("ADP_OpenConnectionRequired_Delete", resourceCulture);

	internal static string ADP_OpenConnectionRequired_Insert => ResourceManager.GetString("ADP_OpenConnectionRequired_Insert", resourceCulture);

	internal static string ADP_OpenConnectionRequired_Update => ResourceManager.GetString("ADP_OpenConnectionRequired_Update", resourceCulture);

	internal static string ADP_OpenReaderExists => ResourceManager.GetString("ADP_OpenReaderExists", resourceCulture);

	internal static string ADP_OpenResultSetExists => ResourceManager.GetString("ADP_OpenResultSetExists", resourceCulture);

	internal static string ADP_OperationAborted => ResourceManager.GetString("ADP_OperationAborted", resourceCulture);

	internal static string ADP_OperationAbortedExceptionMessage => ResourceManager.GetString("ADP_OperationAbortedExceptionMessage", resourceCulture);

	internal static string ADP_ParallelTransactionsNotSupported => ResourceManager.GetString("ADP_ParallelTransactionsNotSupported", resourceCulture);

	internal static string ADP_ParameterConversionFailed => ResourceManager.GetString("ADP_ParameterConversionFailed", resourceCulture);

	internal static string ADP_ParameterValueOutOfRange => ResourceManager.GetString("ADP_ParameterValueOutOfRange", resourceCulture);

	internal static string ADP_PendingAsyncOperation => ResourceManager.GetString("ADP_PendingAsyncOperation", resourceCulture);

	internal static string ADP_PermissionTypeMismatch => ResourceManager.GetString("ADP_PermissionTypeMismatch", resourceCulture);

	internal static string ADP_PooledOpenTimeout => ResourceManager.GetString("ADP_PooledOpenTimeout", resourceCulture);

	internal static string ADP_PrepareParameterScale => ResourceManager.GetString("ADP_PrepareParameterScale", resourceCulture);

	internal static string ADP_PrepareParameterSize => ResourceManager.GetString("ADP_PrepareParameterSize", resourceCulture);

	internal static string ADP_PrepareParameterType => ResourceManager.GetString("ADP_PrepareParameterType", resourceCulture);

	internal static string ADP_PropertyNotSupported => ResourceManager.GetString("ADP_PropertyNotSupported", resourceCulture);

	internal static string ADP_QuotePrefixNotSet => ResourceManager.GetString("ADP_QuotePrefixNotSet", resourceCulture);

	internal static string ADP_ResultsNotAllowedDuringBatch => ResourceManager.GetString("ADP_ResultsNotAllowedDuringBatch", resourceCulture);

	internal static string ADP_RowUpdatedErrors => ResourceManager.GetString("ADP_RowUpdatedErrors", resourceCulture);

	internal static string ADP_RowUpdatingErrors => ResourceManager.GetString("ADP_RowUpdatingErrors", resourceCulture);

	internal static string ADP_SingleValuedProperty => ResourceManager.GetString("ADP_SingleValuedProperty", resourceCulture);

	internal static string ADP_StreamClosed => ResourceManager.GetString("ADP_StreamClosed", resourceCulture);

	internal static string ADP_TransactionCompleted => ResourceManager.GetString("ADP_TransactionCompleted", resourceCulture);

	internal static string ADP_TransactionCompletedButNotDisposed => ResourceManager.GetString("ADP_TransactionCompletedButNotDisposed", resourceCulture);

	internal static string ADP_TransactionConnectionMismatch => ResourceManager.GetString("ADP_TransactionConnectionMismatch", resourceCulture);

	internal static string ADP_TransactionPresent => ResourceManager.GetString("ADP_TransactionPresent", resourceCulture);

	internal static string ADP_TransactionRequired => ResourceManager.GetString("ADP_TransactionRequired", resourceCulture);

	internal static string ADP_TransactionZombied => ResourceManager.GetString("ADP_TransactionZombied", resourceCulture);

	internal static string ADP_UdlFileError => ResourceManager.GetString("ADP_UdlFileError", resourceCulture);

	internal static string ADP_UnableToCreateBooleanLiteral => ResourceManager.GetString("ADP_UnableToCreateBooleanLiteral", resourceCulture);

	internal static string ADP_UninitializedParameterSize => ResourceManager.GetString("ADP_UninitializedParameterSize", resourceCulture);

	internal static string ADP_UnknownDataType => ResourceManager.GetString("ADP_UnknownDataType", resourceCulture);

	internal static string ADP_UnknownDataTypeCode => ResourceManager.GetString("ADP_UnknownDataTypeCode", resourceCulture);

	internal static string ADP_UnsupportedNativeDataTypeOleDb => ResourceManager.GetString("ADP_UnsupportedNativeDataTypeOleDb", resourceCulture);

	internal static string ADP_UnwantedStatementType => ResourceManager.GetString("ADP_UnwantedStatementType", resourceCulture);

	internal static string ADP_UpdateConcurrencyViolation_Batch => ResourceManager.GetString("ADP_UpdateConcurrencyViolation_Batch", resourceCulture);

	internal static string ADP_UpdateConcurrencyViolation_Delete => ResourceManager.GetString("ADP_UpdateConcurrencyViolation_Delete", resourceCulture);

	internal static string ADP_UpdateConcurrencyViolation_Update => ResourceManager.GetString("ADP_UpdateConcurrencyViolation_Update", resourceCulture);

	internal static string ADP_UpdateMismatchRowTable => ResourceManager.GetString("ADP_UpdateMismatchRowTable", resourceCulture);

	internal static string ADP_UpdateRequiresCommandClone => ResourceManager.GetString("ADP_UpdateRequiresCommandClone", resourceCulture);

	internal static string ADP_UpdateRequiresCommandDelete => ResourceManager.GetString("ADP_UpdateRequiresCommandDelete", resourceCulture);

	internal static string ADP_UpdateRequiresCommandInsert => ResourceManager.GetString("ADP_UpdateRequiresCommandInsert", resourceCulture);

	internal static string ADP_UpdateRequiresCommandSelect => ResourceManager.GetString("ADP_UpdateRequiresCommandSelect", resourceCulture);

	internal static string ADP_UpdateRequiresCommandUpdate => ResourceManager.GetString("ADP_UpdateRequiresCommandUpdate", resourceCulture);

	internal static string ADP_UpdateRequiresSourceTable => ResourceManager.GetString("ADP_UpdateRequiresSourceTable", resourceCulture);

	internal static string ADP_UpdateRequiresSourceTableName => ResourceManager.GetString("ADP_UpdateRequiresSourceTableName", resourceCulture);

	internal static string ADP_VersionDoesNotSupportDataType => ResourceManager.GetString("ADP_VersionDoesNotSupportDataType", resourceCulture);

	internal static string AttestationTokenSignatureValidationFailed => ResourceManager.GetString("AttestationTokenSignatureValidationFailed", resourceCulture);

	internal static string Arg_ArrayPlusOffTooSmall => ResourceManager.GetString("Arg_ArrayPlusOffTooSmall", resourceCulture);

	internal static string Arg_RankMultiDimNotSupported => ResourceManager.GetString("Arg_RankMultiDimNotSupported", resourceCulture);

	internal static string Arg_RemoveArgNotFound => ResourceManager.GetString("Arg_RemoveArgNotFound", resourceCulture);

	internal static string ArgumentOutOfRange_NeedNonNegNum => ResourceManager.GetString("ArgumentOutOfRange_NeedNonNegNum", resourceCulture);

	internal static string AZURESQL_ChinaEndpoint => ResourceManager.GetString("AZURESQL_ChinaEndpoint", resourceCulture);

	internal static string AZURESQL_GenericEndpoint => ResourceManager.GetString("AZURESQL_GenericEndpoint", resourceCulture);

	internal static string AZURESQL_GermanEndpoint => ResourceManager.GetString("AZURESQL_GermanEndpoint", resourceCulture);

	internal static string AZURESQL_UsGovEndpoint => ResourceManager.GetString("AZURESQL_UsGovEndpoint", resourceCulture);

	internal static string CodeGen_DuplicateTableName => ResourceManager.GetString("CodeGen_DuplicateTableName", resourceCulture);

	internal static string CodeGen_InvalidIdentifier => ResourceManager.GetString("CodeGen_InvalidIdentifier", resourceCulture);

	internal static string CodeGen_NoCtor0 => ResourceManager.GetString("CodeGen_NoCtor0", resourceCulture);

	internal static string CodeGen_NoCtor1 => ResourceManager.GetString("CodeGen_NoCtor1", resourceCulture);

	internal static string CodeGen_TypeCantBeNull => ResourceManager.GetString("CodeGen_TypeCantBeNull", resourceCulture);

	internal static string collectionChangedEventDescr => ResourceManager.GetString("collectionChangedEventDescr", resourceCulture);

	internal static string ConfigBaseElementsOnly => ResourceManager.GetString("ConfigBaseElementsOnly", resourceCulture);

	internal static string ConfigBaseNoChildNodes => ResourceManager.GetString("ConfigBaseNoChildNodes", resourceCulture);

	internal static string ConfigProviderInvalid => ResourceManager.GetString("ConfigProviderInvalid", resourceCulture);

	internal static string ConfigProviderMissing => ResourceManager.GetString("ConfigProviderMissing", resourceCulture);

	internal static string ConfigProviderNotFound => ResourceManager.GetString("ConfigProviderNotFound", resourceCulture);

	internal static string ConfigProviderNotInstalled => ResourceManager.GetString("ConfigProviderNotInstalled", resourceCulture);

	internal static string ConfigRequiredAttributeEmpty => ResourceManager.GetString("ConfigRequiredAttributeEmpty", resourceCulture);

	internal static string ConfigRequiredAttributeMissing => ResourceManager.GetString("ConfigRequiredAttributeMissing", resourceCulture);

	internal static string ConfigSectionsUnique => ResourceManager.GetString("ConfigSectionsUnique", resourceCulture);

	internal static string ConfigUnrecognizedAttributes => ResourceManager.GetString("ConfigUnrecognizedAttributes", resourceCulture);

	internal static string ConfigUnrecognizedElement => ResourceManager.GetString("ConfigUnrecognizedElement", resourceCulture);

	internal static string ConstraintNameDescr => ResourceManager.GetString("ConstraintNameDescr", resourceCulture);

	internal static string ConstraintTableDescr => ResourceManager.GetString("ConstraintTableDescr", resourceCulture);

	internal static string Data_ArgumentContainsNull => ResourceManager.GetString("Data_ArgumentContainsNull", resourceCulture);

	internal static string Data_ArgumentNull => ResourceManager.GetString("Data_ArgumentNull", resourceCulture);

	internal static string Data_ArgumentOutOfRange => ResourceManager.GetString("Data_ArgumentOutOfRange", resourceCulture);

	internal static string Data_CannotModifyCollection => ResourceManager.GetString("Data_CannotModifyCollection", resourceCulture);

	internal static string Data_CaseInsensitiveNameConflict => ResourceManager.GetString("Data_CaseInsensitiveNameConflict", resourceCulture);

	internal static string Data_EnforceConstraints => ResourceManager.GetString("Data_EnforceConstraints", resourceCulture);

	internal static string Data_InvalidOffsetLength => ResourceManager.GetString("Data_InvalidOffsetLength", resourceCulture);

	internal static string Data_NamespaceNameConflict => ResourceManager.GetString("Data_NamespaceNameConflict", resourceCulture);

	internal static string DataAdapter_AcceptChangesDuringFill => ResourceManager.GetString("DataAdapter_AcceptChangesDuringFill", resourceCulture);

	internal static string DataAdapter_AcceptChangesDuringUpdate => ResourceManager.GetString("DataAdapter_AcceptChangesDuringUpdate", resourceCulture);

	internal static string DataAdapter_ContinueUpdateOnError => ResourceManager.GetString("DataAdapter_ContinueUpdateOnError", resourceCulture);

	internal static string DataAdapter_FillError => ResourceManager.GetString("DataAdapter_FillError", resourceCulture);

	internal static string DataAdapter_FillLoadOption => ResourceManager.GetString("DataAdapter_FillLoadOption", resourceCulture);

	internal static string DataAdapter_MissingMappingAction => ResourceManager.GetString("DataAdapter_MissingMappingAction", resourceCulture);

	internal static string DataAdapter_MissingSchemaAction => ResourceManager.GetString("DataAdapter_MissingSchemaAction", resourceCulture);

	internal static string DataAdapter_ReturnProviderSpecificTypes => ResourceManager.GetString("DataAdapter_ReturnProviderSpecificTypes", resourceCulture);

	internal static string DataAdapter_TableMappings => ResourceManager.GetString("DataAdapter_TableMappings", resourceCulture);

	internal static string DataCategory_Action => ResourceManager.GetString("DataCategory_Action", resourceCulture);

	internal static string DataCategory_Advanced => ResourceManager.GetString("DataCategory_Advanced", resourceCulture);

	internal static string DataCategory_Behavior => ResourceManager.GetString("DataCategory_Behavior", resourceCulture);

	internal static string DataCategory_ConnectionResilency => ResourceManager.GetString("DataCategory_ConnectionResilency", resourceCulture);

	internal static string DataCategory_Context => ResourceManager.GetString("DataCategory_Context", resourceCulture);

	internal static string DataCategory_Data => ResourceManager.GetString("DataCategory_Data", resourceCulture);

	internal static string DataCategory_Fill => ResourceManager.GetString("DataCategory_Fill", resourceCulture);

	internal static string DataCategory_InfoMessage => ResourceManager.GetString("DataCategory_InfoMessage", resourceCulture);

	internal static string DataCategory_Initialization => ResourceManager.GetString("DataCategory_Initialization", resourceCulture);

	internal static string DataCategory_Mapping => ResourceManager.GetString("DataCategory_Mapping", resourceCulture);

	internal static string DataCategory_NamedConnectionString => ResourceManager.GetString("DataCategory_NamedConnectionString", resourceCulture);

	internal static string DataCategory_Notification => ResourceManager.GetString("DataCategory_Notification", resourceCulture);

	internal static string DataCategory_Pooling => ResourceManager.GetString("DataCategory_Pooling", resourceCulture);

	internal static string DataCategory_Replication => ResourceManager.GetString("DataCategory_Replication", resourceCulture);

	internal static string DataCategory_Schema => ResourceManager.GetString("DataCategory_Schema", resourceCulture);

	internal static string DataCategory_Security => ResourceManager.GetString("DataCategory_Security", resourceCulture);

	internal static string DataCategory_Source => ResourceManager.GetString("DataCategory_Source", resourceCulture);

	internal static string DataCategory_StateChange => ResourceManager.GetString("DataCategory_StateChange", resourceCulture);

	internal static string DataCategory_StatementCompleted => ResourceManager.GetString("DataCategory_StatementCompleted", resourceCulture);

	internal static string DataCategory_Udt => ResourceManager.GetString("DataCategory_Udt", resourceCulture);

	internal static string DataCategory_Update => ResourceManager.GetString("DataCategory_Update", resourceCulture);

	internal static string DataCategory_Xml => ResourceManager.GetString("DataCategory_Xml", resourceCulture);

	internal static string DataColumn_AutoIncrementAndDefaultValue => ResourceManager.GetString("DataColumn_AutoIncrementAndDefaultValue", resourceCulture);

	internal static string DataColumn_AutoIncrementAndExpression => ResourceManager.GetString("DataColumn_AutoIncrementAndExpression", resourceCulture);

	internal static string DataColumn_AutoIncrementCannotSetIfHasData => ResourceManager.GetString("DataColumn_AutoIncrementCannotSetIfHasData", resourceCulture);

	internal static string DataColumn_AutoIncrementSeed => ResourceManager.GetString("DataColumn_AutoIncrementSeed", resourceCulture);

	internal static string DataColumn_CannotChangeNamespace => ResourceManager.GetString("DataColumn_CannotChangeNamespace", resourceCulture);

	internal static string DataColumn_CannotSetDateTimeModeForNonDateTimeColumns => ResourceManager.GetString("DataColumn_CannotSetDateTimeModeForNonDateTimeColumns", resourceCulture);

	internal static string DataColumn_CannotSetMaxLength => ResourceManager.GetString("DataColumn_CannotSetMaxLength", resourceCulture);

	internal static string DataColumn_CannotSetMaxLength2 => ResourceManager.GetString("DataColumn_CannotSetMaxLength2", resourceCulture);

	internal static string DataColumn_CannotSetToNull => ResourceManager.GetString("DataColumn_CannotSetToNull", resourceCulture);

	internal static string DataColumn_CannotSimpleContent => ResourceManager.GetString("DataColumn_CannotSimpleContent", resourceCulture);

	internal static string DataColumn_CannotSimpleContentType => ResourceManager.GetString("DataColumn_CannotSimpleContentType", resourceCulture);

	internal static string DataColumn_ChangeDataType => ResourceManager.GetString("DataColumn_ChangeDataType", resourceCulture);

	internal static string DataColumn_DateTimeMode => ResourceManager.GetString("DataColumn_DateTimeMode", resourceCulture);

	internal static string DataColumn_DefaultValueAndAutoIncrement => ResourceManager.GetString("DataColumn_DefaultValueAndAutoIncrement", resourceCulture);

	internal static string DataColumn_DefaultValueColumnDataType => ResourceManager.GetString("DataColumn_DefaultValueColumnDataType", resourceCulture);

	internal static string DataColumn_DefaultValueDataType => ResourceManager.GetString("DataColumn_DefaultValueDataType", resourceCulture);

	internal static string DataColumn_DefaultValueDataType1 => ResourceManager.GetString("DataColumn_DefaultValueDataType1", resourceCulture);

	internal static string DataColumn_ExceedMaxLength => ResourceManager.GetString("DataColumn_ExceedMaxLength", resourceCulture);

	internal static string DataColumn_ExpressionAndConstraint => ResourceManager.GetString("DataColumn_ExpressionAndConstraint", resourceCulture);

	internal static string DataColumn_ExpressionAndReadOnly => ResourceManager.GetString("DataColumn_ExpressionAndReadOnly", resourceCulture);

	internal static string DataColumn_ExpressionAndUnique => ResourceManager.GetString("DataColumn_ExpressionAndUnique", resourceCulture);

	internal static string DataColumn_ExpressionCircular => ResourceManager.GetString("DataColumn_ExpressionCircular", resourceCulture);

	internal static string DataColumn_ExpressionInConstraint => ResourceManager.GetString("DataColumn_ExpressionInConstraint", resourceCulture);

	internal static string DataColumn_HasToBeStringType => ResourceManager.GetString("DataColumn_HasToBeStringType", resourceCulture);

	internal static string DataColumn_INullableUDTwithoutStaticNull => ResourceManager.GetString("DataColumn_INullableUDTwithoutStaticNull", resourceCulture);

	internal static string DataColumn_InvalidDataColumnMapping => ResourceManager.GetString("DataColumn_InvalidDataColumnMapping", resourceCulture);

	internal static string DataColumn_InvalidDateTimeMode => ResourceManager.GetString("DataColumn_InvalidDateTimeMode", resourceCulture);

	internal static string DataColumn_LongerThanMaxLength => ResourceManager.GetString("DataColumn_LongerThanMaxLength", resourceCulture);

	internal static string DataColumn_NameRequired => ResourceManager.GetString("DataColumn_NameRequired", resourceCulture);

	internal static string DataColumn_NonUniqueValues => ResourceManager.GetString("DataColumn_NonUniqueValues", resourceCulture);

	internal static string DataColumn_NotAllowDBNull => ResourceManager.GetString("DataColumn_NotAllowDBNull", resourceCulture);

	internal static string DataColumn_NotInAnyTable => ResourceManager.GetString("DataColumn_NotInAnyTable", resourceCulture);

	internal static string DataColumn_NotInTheTable => ResourceManager.GetString("DataColumn_NotInTheTable", resourceCulture);

	internal static string DataColumn_NotInTheUnderlyingTable => ResourceManager.GetString("DataColumn_NotInTheUnderlyingTable", resourceCulture);

	internal static string DataColumn_NullableTypesNotSupported => ResourceManager.GetString("DataColumn_NullableTypesNotSupported", resourceCulture);

	internal static string DataColumn_NullDataType => ResourceManager.GetString("DataColumn_NullDataType", resourceCulture);

	internal static string DataColumn_NullKeyValues => ResourceManager.GetString("DataColumn_NullKeyValues", resourceCulture);

	internal static string DataColumn_NullValues => ResourceManager.GetString("DataColumn_NullValues", resourceCulture);

	internal static string DataColumn_OrdinalExceedMaximun => ResourceManager.GetString("DataColumn_OrdinalExceedMaximun", resourceCulture);

	internal static string DataColumn_ReadOnly => ResourceManager.GetString("DataColumn_ReadOnly", resourceCulture);

	internal static string DataColumn_ReadOnlyAndExpression => ResourceManager.GetString("DataColumn_ReadOnlyAndExpression", resourceCulture);

	internal static string DataColumn_SetAddedAndModifiedCalledOnNonUnchanged => ResourceManager.GetString("DataColumn_SetAddedAndModifiedCalledOnNonUnchanged", resourceCulture);

	internal static string DataColumn_SetFailed => ResourceManager.GetString("DataColumn_SetFailed", resourceCulture);

	internal static string DataColumn_UDTImplementsIChangeTrackingButnotIRevertible => ResourceManager.GetString("DataColumn_UDTImplementsIChangeTrackingButnotIRevertible", resourceCulture);

	internal static string DataColumn_UniqueAndExpression => ResourceManager.GetString("DataColumn_UniqueAndExpression", resourceCulture);

	internal static string DataColumnAllowNullDescr => ResourceManager.GetString("DataColumnAllowNullDescr", resourceCulture);

	internal static string DataColumnAutoIncrementDescr => ResourceManager.GetString("DataColumnAutoIncrementDescr", resourceCulture);

	internal static string DataColumnAutoIncrementSeedDescr => ResourceManager.GetString("DataColumnAutoIncrementSeedDescr", resourceCulture);

	internal static string DataColumnAutoIncrementStepDescr => ResourceManager.GetString("DataColumnAutoIncrementStepDescr", resourceCulture);

	internal static string DataColumnCaptionDescr => ResourceManager.GetString("DataColumnCaptionDescr", resourceCulture);

	internal static string DataColumnColumnNameDescr => ResourceManager.GetString("DataColumnColumnNameDescr", resourceCulture);

	internal static string DataColumnDataTableDescr => ResourceManager.GetString("DataColumnDataTableDescr", resourceCulture);

	internal static string DataColumnDataTypeDescr => ResourceManager.GetString("DataColumnDataTypeDescr", resourceCulture);

	internal static string DataColumnDateTimeModeDescr => ResourceManager.GetString("DataColumnDateTimeModeDescr", resourceCulture);

	internal static string DataColumnDefaultValueDescr => ResourceManager.GetString("DataColumnDefaultValueDescr", resourceCulture);

	internal static string DataColumnExpressionDescr => ResourceManager.GetString("DataColumnExpressionDescr", resourceCulture);

	internal static string DataColumnMapping_DataSetColumn => ResourceManager.GetString("DataColumnMapping_DataSetColumn", resourceCulture);

	internal static string DataColumnMapping_SourceColumn => ResourceManager.GetString("DataColumnMapping_SourceColumn", resourceCulture);

	internal static string DataColumnMappingDescr => ResourceManager.GetString("DataColumnMappingDescr", resourceCulture);

	internal static string DataColumnMappings_Count => ResourceManager.GetString("DataColumnMappings_Count", resourceCulture);

	internal static string DataColumnMappings_Item => ResourceManager.GetString("DataColumnMappings_Item", resourceCulture);

	internal static string DataColumnMaxLengthDescr => ResourceManager.GetString("DataColumnMaxLengthDescr", resourceCulture);

	internal static string DataColumnNamespaceDescr => ResourceManager.GetString("DataColumnNamespaceDescr", resourceCulture);

	internal static string DataColumnOrdinalDescr => ResourceManager.GetString("DataColumnOrdinalDescr", resourceCulture);

	internal static string DataColumnPrefixDescr => ResourceManager.GetString("DataColumnPrefixDescr", resourceCulture);

	internal static string DataColumnReadOnlyDescr => ResourceManager.GetString("DataColumnReadOnlyDescr", resourceCulture);

	internal static string DataColumns_Add1 => ResourceManager.GetString("DataColumns_Add1", resourceCulture);

	internal static string DataColumns_Add2 => ResourceManager.GetString("DataColumns_Add2", resourceCulture);

	internal static string DataColumns_Add3 => ResourceManager.GetString("DataColumns_Add3", resourceCulture);

	internal static string DataColumns_Add4 => ResourceManager.GetString("DataColumns_Add4", resourceCulture);

	internal static string DataColumns_AddDuplicate => ResourceManager.GetString("DataColumns_AddDuplicate", resourceCulture);

	internal static string DataColumns_AddDuplicate2 => ResourceManager.GetString("DataColumns_AddDuplicate2", resourceCulture);

	internal static string DataColumns_AddDuplicate3 => ResourceManager.GetString("DataColumns_AddDuplicate3", resourceCulture);

	internal static string DataColumns_OutOfRange => ResourceManager.GetString("DataColumns_OutOfRange", resourceCulture);

	internal static string DataColumns_Remove => ResourceManager.GetString("DataColumns_Remove", resourceCulture);

	internal static string DataColumns_RemoveChildKey => ResourceManager.GetString("DataColumns_RemoveChildKey", resourceCulture);

	internal static string DataColumns_RemoveConstraint => ResourceManager.GetString("DataColumns_RemoveConstraint", resourceCulture);

	internal static string DataColumns_RemoveExpression => ResourceManager.GetString("DataColumns_RemoveExpression", resourceCulture);

	internal static string DataColumns_RemovePrimaryKey => ResourceManager.GetString("DataColumns_RemovePrimaryKey", resourceCulture);

	internal static string DataColumnUniqueDescr => ResourceManager.GetString("DataColumnUniqueDescr", resourceCulture);

	internal static string DataConstraint_AddFailed => ResourceManager.GetString("DataConstraint_AddFailed", resourceCulture);

	internal static string DataConstraint_AddPrimaryKeyConstraint => ResourceManager.GetString("DataConstraint_AddPrimaryKeyConstraint", resourceCulture);

	internal static string DataConstraint_BadObjectPropertyAccess => ResourceManager.GetString("DataConstraint_BadObjectPropertyAccess", resourceCulture);

	internal static string DataConstraint_CantAddConstraintToMultipleNestedTable => ResourceManager.GetString("DataConstraint_CantAddConstraintToMultipleNestedTable", resourceCulture);

	internal static string DataConstraint_CascadeDelete => ResourceManager.GetString("DataConstraint_CascadeDelete", resourceCulture);

	internal static string DataConstraint_CascadeUpdate => ResourceManager.GetString("DataConstraint_CascadeUpdate", resourceCulture);

	internal static string DataConstraint_ClearParentTable => ResourceManager.GetString("DataConstraint_ClearParentTable", resourceCulture);

	internal static string DataConstraint_Duplicate => ResourceManager.GetString("DataConstraint_Duplicate", resourceCulture);

	internal static string DataConstraint_DuplicateName => ResourceManager.GetString("DataConstraint_DuplicateName", resourceCulture);

	internal static string DataConstraint_ForeignKeyViolation => ResourceManager.GetString("DataConstraint_ForeignKeyViolation", resourceCulture);

	internal static string DataConstraint_ForeignTable => ResourceManager.GetString("DataConstraint_ForeignTable", resourceCulture);

	internal static string DataConstraint_NeededForForeignKeyConstraint => ResourceManager.GetString("DataConstraint_NeededForForeignKeyConstraint", resourceCulture);

	internal static string DataConstraint_NoName => ResourceManager.GetString("DataConstraint_NoName", resourceCulture);

	internal static string DataConstraint_NotInTheTable => ResourceManager.GetString("DataConstraint_NotInTheTable", resourceCulture);

	internal static string DataConstraint_OutOfRange => ResourceManager.GetString("DataConstraint_OutOfRange", resourceCulture);

	internal static string DataConstraint_ParentValues => ResourceManager.GetString("DataConstraint_ParentValues", resourceCulture);

	internal static string DataConstraint_RemoveFailed => ResourceManager.GetString("DataConstraint_RemoveFailed", resourceCulture);

	internal static string DataConstraint_RemoveParentRow => ResourceManager.GetString("DataConstraint_RemoveParentRow", resourceCulture);

	internal static string DataConstraint_UniqueViolation => ResourceManager.GetString("DataConstraint_UniqueViolation", resourceCulture);

	internal static string DataConstraint_Violation => ResourceManager.GetString("DataConstraint_Violation", resourceCulture);

	internal static string DataConstraint_ViolationValue => ResourceManager.GetString("DataConstraint_ViolationValue", resourceCulture);

	internal static string DataDom_CloneNode => ResourceManager.GetString("DataDom_CloneNode", resourceCulture);

	internal static string DataDom_ColumnMappingChange => ResourceManager.GetString("DataDom_ColumnMappingChange", resourceCulture);

	internal static string DataDom_ColumnNameChange => ResourceManager.GetString("DataDom_ColumnNameChange", resourceCulture);

	internal static string DataDom_ColumnNamespaceChange => ResourceManager.GetString("DataDom_ColumnNamespaceChange", resourceCulture);

	internal static string DataDom_DataSetNameChange => ResourceManager.GetString("DataDom_DataSetNameChange", resourceCulture);

	internal static string DataDom_DataSetNestedRelationsChange => ResourceManager.GetString("DataDom_DataSetNestedRelationsChange", resourceCulture);

	internal static string DataDom_DataSetNull => ResourceManager.GetString("DataDom_DataSetNull", resourceCulture);

	internal static string DataDom_DataSetTablesChange => ResourceManager.GetString("DataDom_DataSetTablesChange", resourceCulture);

	internal static string DataDom_EnforceConstraintsShouldBeOff => ResourceManager.GetString("DataDom_EnforceConstraintsShouldBeOff", resourceCulture);

	internal static string DataDom_Foliation => ResourceManager.GetString("DataDom_Foliation", resourceCulture);

	internal static string DataDom_MultipleDataSet => ResourceManager.GetString("DataDom_MultipleDataSet", resourceCulture);

	internal static string DataDom_MultipleLoad => ResourceManager.GetString("DataDom_MultipleLoad", resourceCulture);

	internal static string DataDom_NotSupport_Clear => ResourceManager.GetString("DataDom_NotSupport_Clear", resourceCulture);

	internal static string DataDom_NotSupport_EntRef => ResourceManager.GetString("DataDom_NotSupport_EntRef", resourceCulture);

	internal static string DataDom_NotSupport_GetElementById => ResourceManager.GetString("DataDom_NotSupport_GetElementById", resourceCulture);

	internal static string DataDom_TableColumnsChange => ResourceManager.GetString("DataDom_TableColumnsChange", resourceCulture);

	internal static string DataDom_TableNameChange => ResourceManager.GetString("DataDom_TableNameChange", resourceCulture);

	internal static string DataDom_TableNamespaceChange => ResourceManager.GetString("DataDom_TableNamespaceChange", resourceCulture);

	internal static string DataIndex_FindWithoutSortOrder => ResourceManager.GetString("DataIndex_FindWithoutSortOrder", resourceCulture);

	internal static string DataIndex_KeyLength => ResourceManager.GetString("DataIndex_KeyLength", resourceCulture);

	internal static string DataIndex_RecordStateRange => ResourceManager.GetString("DataIndex_RecordStateRange", resourceCulture);

	internal static string DataKey_DuplicateColumns => ResourceManager.GetString("DataKey_DuplicateColumns", resourceCulture);

	internal static string DataKey_NoColumns => ResourceManager.GetString("DataKey_NoColumns", resourceCulture);

	internal static string DataKey_RemovePrimaryKey => ResourceManager.GetString("DataKey_RemovePrimaryKey", resourceCulture);

	internal static string DataKey_RemovePrimaryKey1 => ResourceManager.GetString("DataKey_RemovePrimaryKey1", resourceCulture);

	internal static string DataKey_TableMismatch => ResourceManager.GetString("DataKey_TableMismatch", resourceCulture);

	internal static string DataKey_TooManyColumns => ResourceManager.GetString("DataKey_TooManyColumns", resourceCulture);

	internal static string DataMerge_DataTypeMismatch => ResourceManager.GetString("DataMerge_DataTypeMismatch", resourceCulture);

	internal static string DataMerge_MissingColumnDefinition => ResourceManager.GetString("DataMerge_MissingColumnDefinition", resourceCulture);

	internal static string DataMerge_MissingConstraint => ResourceManager.GetString("DataMerge_MissingConstraint", resourceCulture);

	internal static string DataMerge_MissingDefinition => ResourceManager.GetString("DataMerge_MissingDefinition", resourceCulture);

	internal static string DataMerge_MissingPrimaryKeyColumnInSource => ResourceManager.GetString("DataMerge_MissingPrimaryKeyColumnInSource", resourceCulture);

	internal static string DataMerge_PrimaryKeyColumnsMismatch => ResourceManager.GetString("DataMerge_PrimaryKeyColumnsMismatch", resourceCulture);

	internal static string DataMerge_PrimaryKeyMismatch => ResourceManager.GetString("DataMerge_PrimaryKeyMismatch", resourceCulture);

	internal static string DataMerge_ReltionKeyColumnsMismatch => ResourceManager.GetString("DataMerge_ReltionKeyColumnsMismatch", resourceCulture);

	internal static string DataRelation_AlreadyExists => ResourceManager.GetString("DataRelation_AlreadyExists", resourceCulture);

	internal static string DataRelation_AlreadyInOtherDataSet => ResourceManager.GetString("DataRelation_AlreadyInOtherDataSet", resourceCulture);

	internal static string DataRelation_AlreadyInTheDataSet => ResourceManager.GetString("DataRelation_AlreadyInTheDataSet", resourceCulture);

	internal static string DataRelation_CaseLocaleMismatch => ResourceManager.GetString("DataRelation_CaseLocaleMismatch", resourceCulture);

	internal static string DataRelation_ChildTableMismatch => ResourceManager.GetString("DataRelation_ChildTableMismatch", resourceCulture);

	internal static string DataRelation_ColumnsTypeMismatch => ResourceManager.GetString("DataRelation_ColumnsTypeMismatch", resourceCulture);

	internal static string DataRelation_DataSetMismatch => ResourceManager.GetString("DataRelation_DataSetMismatch", resourceCulture);

	internal static string DataRelation_DoesNotExist => ResourceManager.GetString("DataRelation_DoesNotExist", resourceCulture);

	internal static string DataRelation_DuplicateName => ResourceManager.GetString("DataRelation_DuplicateName", resourceCulture);

	internal static string DataRelation_ForeignDataSet => ResourceManager.GetString("DataRelation_ForeignDataSet", resourceCulture);

	internal static string DataRelation_ForeignRow => ResourceManager.GetString("DataRelation_ForeignRow", resourceCulture);

	internal static string DataRelation_ForeignTable => ResourceManager.GetString("DataRelation_ForeignTable", resourceCulture);

	internal static string DataRelation_GetParentRowTableMismatch => ResourceManager.GetString("DataRelation_GetParentRowTableMismatch", resourceCulture);

	internal static string DataRelation_InValidNamespaceInNestedRelation => ResourceManager.GetString("DataRelation_InValidNamespaceInNestedRelation", resourceCulture);

	internal static string DataRelation_InValidNestedRelation => ResourceManager.GetString("DataRelation_InValidNestedRelation", resourceCulture);

	internal static string DataRelation_KeyColumnsIdentical => ResourceManager.GetString("DataRelation_KeyColumnsIdentical", resourceCulture);

	internal static string DataRelation_KeyLengthMismatch => ResourceManager.GetString("DataRelation_KeyLengthMismatch", resourceCulture);

	internal static string DataRelation_KeyZeroLength => ResourceManager.GetString("DataRelation_KeyZeroLength", resourceCulture);

	internal static string DataRelation_LoopInNestedRelations => ResourceManager.GetString("DataRelation_LoopInNestedRelations", resourceCulture);

	internal static string DataRelation_NoName => ResourceManager.GetString("DataRelation_NoName", resourceCulture);

	internal static string DataRelation_NotInTheDataSet => ResourceManager.GetString("DataRelation_NotInTheDataSet", resourceCulture);

	internal static string DataRelation_OutOfRange => ResourceManager.GetString("DataRelation_OutOfRange", resourceCulture);

	internal static string DataRelation_ParentOrChildColumnsDoNotHaveDataSet => ResourceManager.GetString("DataRelation_ParentOrChildColumnsDoNotHaveDataSet", resourceCulture);

	internal static string DataRelation_ParentTableMismatch => ResourceManager.GetString("DataRelation_ParentTableMismatch", resourceCulture);

	internal static string DataRelation_RelationNestedReadOnly => ResourceManager.GetString("DataRelation_RelationNestedReadOnly", resourceCulture);

	internal static string DataRelation_SetParentRowTableMismatch => ResourceManager.GetString("DataRelation_SetParentRowTableMismatch", resourceCulture);

	internal static string DataRelation_TableCantBeNestedInTwoTables => ResourceManager.GetString("DataRelation_TableCantBeNestedInTwoTables", resourceCulture);

	internal static string DataRelation_TableNull => ResourceManager.GetString("DataRelation_TableNull", resourceCulture);

	internal static string DataRelation_TablesInDifferentSets => ResourceManager.GetString("DataRelation_TablesInDifferentSets", resourceCulture);

	internal static string DataRelation_TableWasRemoved => ResourceManager.GetString("DataRelation_TableWasRemoved", resourceCulture);

	internal static string DataRelationChildColumnsDescr => ResourceManager.GetString("DataRelationChildColumnsDescr", resourceCulture);

	internal static string DataRelationNested => ResourceManager.GetString("DataRelationNested", resourceCulture);

	internal static string DataRelationParentColumnsDescr => ResourceManager.GetString("DataRelationParentColumnsDescr", resourceCulture);

	internal static string DataRelationRelationNameDescr => ResourceManager.GetString("DataRelationRelationNameDescr", resourceCulture);

	internal static string DataRow_AlreadyDeleted => ResourceManager.GetString("DataRow_AlreadyDeleted", resourceCulture);

	internal static string DataRow_AlreadyInOtherCollection => ResourceManager.GetString("DataRow_AlreadyInOtherCollection", resourceCulture);

	internal static string DataRow_AlreadyInTheCollection => ResourceManager.GetString("DataRow_AlreadyInTheCollection", resourceCulture);

	internal static string DataRow_AlreadyRemoved => ResourceManager.GetString("DataRow_AlreadyRemoved", resourceCulture);

	internal static string DataRow_BeginEditInRowChanging => ResourceManager.GetString("DataRow_BeginEditInRowChanging", resourceCulture);

	internal static string DataRow_CancelEditInRowChanging => ResourceManager.GetString("DataRow_CancelEditInRowChanging", resourceCulture);

	internal static string DataRow_DeletedRowInaccessible => ResourceManager.GetString("DataRow_DeletedRowInaccessible", resourceCulture);

	internal static string DataRow_DeleteInRowDeleting => ResourceManager.GetString("DataRow_DeleteInRowDeleting", resourceCulture);

	internal static string DataRow_EditInRowChanging => ResourceManager.GetString("DataRow_EditInRowChanging", resourceCulture);

	internal static string DataRow_Empty => ResourceManager.GetString("DataRow_Empty", resourceCulture);

	internal static string DataRow_EndEditInRowChanging => ResourceManager.GetString("DataRow_EndEditInRowChanging", resourceCulture);

	internal static string DataRow_InvalidRowBitPattern => ResourceManager.GetString("DataRow_InvalidRowBitPattern", resourceCulture);

	internal static string DataRow_InvalidVersion => ResourceManager.GetString("DataRow_InvalidVersion", resourceCulture);

	internal static string DataRow_MultipleParents => ResourceManager.GetString("DataRow_MultipleParents", resourceCulture);

	internal static string DataRow_NoCurrentData => ResourceManager.GetString("DataRow_NoCurrentData", resourceCulture);

	internal static string DataRow_NoOriginalData => ResourceManager.GetString("DataRow_NoOriginalData", resourceCulture);

	internal static string DataRow_NoProposedData => ResourceManager.GetString("DataRow_NoProposedData", resourceCulture);

	internal static string DataRow_NotInTheDataSet => ResourceManager.GetString("DataRow_NotInTheDataSet", resourceCulture);

	internal static string DataRow_NotInTheTable => ResourceManager.GetString("DataRow_NotInTheTable", resourceCulture);

	internal static string DataRow_OutOfRange => ResourceManager.GetString("DataRow_OutOfRange", resourceCulture);

	internal static string DataRow_ParentRowNotInTheDataSet => ResourceManager.GetString("DataRow_ParentRowNotInTheDataSet", resourceCulture);

	internal static string DataRow_RemovedFromTheTable => ResourceManager.GetString("DataRow_RemovedFromTheTable", resourceCulture);

	internal static string DataRow_RowInsertMissing => ResourceManager.GetString("DataRow_RowInsertMissing", resourceCulture);

	internal static string DataRow_RowInsertOutOfRange => ResourceManager.GetString("DataRow_RowInsertOutOfRange", resourceCulture);

	internal static string DataRow_RowInsertTwice => ResourceManager.GetString("DataRow_RowInsertTwice", resourceCulture);

	internal static string DataRow_RowOutOfRange => ResourceManager.GetString("DataRow_RowOutOfRange", resourceCulture);

	internal static string DataRow_ValuesArrayLength => ResourceManager.GetString("DataRow_ValuesArrayLength", resourceCulture);

	internal static string DataROWView_PropertyNotFound => ResourceManager.GetString("DataROWView_PropertyNotFound", resourceCulture);

	internal static string DataSet_CannotChangeCaseLocale => ResourceManager.GetString("DataSet_CannotChangeCaseLocale", resourceCulture);

	internal static string DataSet_CannotChangeSchemaSerializationMode => ResourceManager.GetString("DataSet_CannotChangeSchemaSerializationMode", resourceCulture);

	internal static string DataSet_DefaultConstraintException => ResourceManager.GetString("DataSet_DefaultConstraintException", resourceCulture);

	internal static string DataSet_DefaultDataException => ResourceManager.GetString("DataSet_DefaultDataException", resourceCulture);

	internal static string DataSet_DefaultDeletedRowInaccessibleException => ResourceManager.GetString("DataSet_DefaultDeletedRowInaccessibleException", resourceCulture);

	internal static string DataSet_DefaultDuplicateNameException => ResourceManager.GetString("DataSet_DefaultDuplicateNameException", resourceCulture);

	internal static string DataSet_DefaultInRowChangingEventException => ResourceManager.GetString("DataSet_DefaultInRowChangingEventException", resourceCulture);

	internal static string DataSet_DefaultInvalidConstraintException => ResourceManager.GetString("DataSet_DefaultInvalidConstraintException", resourceCulture);

	internal static string DataSet_DefaultMissingPrimaryKeyException => ResourceManager.GetString("DataSet_DefaultMissingPrimaryKeyException", resourceCulture);

	internal static string DataSet_DefaultNoNullAllowedException => ResourceManager.GetString("DataSet_DefaultNoNullAllowedException", resourceCulture);

	internal static string DataSet_DefaultReadOnlyException => ResourceManager.GetString("DataSet_DefaultReadOnlyException", resourceCulture);

	internal static string DataSet_DefaultRowNotInTableException => ResourceManager.GetString("DataSet_DefaultRowNotInTableException", resourceCulture);

	internal static string DataSet_DefaultVersionNotFoundException => ResourceManager.GetString("DataSet_DefaultVersionNotFoundException", resourceCulture);

	internal static string DataSet_SetDataSetNameConflicting => ResourceManager.GetString("DataSet_SetDataSetNameConflicting", resourceCulture);

	internal static string DataSet_SetNameToEmpty => ResourceManager.GetString("DataSet_SetNameToEmpty", resourceCulture);

	internal static string DataSet_UnsupportedSchema => ResourceManager.GetString("DataSet_UnsupportedSchema", resourceCulture);

	internal static string DataSetCaseSensitiveDescr => ResourceManager.GetString("DataSetCaseSensitiveDescr", resourceCulture);

	internal static string DataSetDataSetNameDescr => ResourceManager.GetString("DataSetDataSetNameDescr", resourceCulture);

	internal static string DataSetDefaultViewDescr => ResourceManager.GetString("DataSetDefaultViewDescr", resourceCulture);

	internal static string DataSetDescr => ResourceManager.GetString("DataSetDescr", resourceCulture);

	internal static string DataSetEnforceConstraintsDescr => ResourceManager.GetString("DataSetEnforceConstraintsDescr", resourceCulture);

	internal static string DataSetHasErrorsDescr => ResourceManager.GetString("DataSetHasErrorsDescr", resourceCulture);

	internal static string DataSetInitializedDescr => ResourceManager.GetString("DataSetInitializedDescr", resourceCulture);

	internal static string DataSetLocaleDescr => ResourceManager.GetString("DataSetLocaleDescr", resourceCulture);

	internal static string DataSetMergeFailedDescr => ResourceManager.GetString("DataSetMergeFailedDescr", resourceCulture);

	internal static string DataSetNamespaceDescr => ResourceManager.GetString("DataSetNamespaceDescr", resourceCulture);

	internal static string DataSetPrefixDescr => ResourceManager.GetString("DataSetPrefixDescr", resourceCulture);

	internal static string DataSetRelationsDescr => ResourceManager.GetString("DataSetRelationsDescr", resourceCulture);

	internal static string DataSetTablesDescr => ResourceManager.GetString("DataSetTablesDescr", resourceCulture);

	internal static string DataStorage_AggregateException => ResourceManager.GetString("DataStorage_AggregateException", resourceCulture);

	internal static string DataStorage_IComparableNotDefined => ResourceManager.GetString("DataStorage_IComparableNotDefined", resourceCulture);

	internal static string DataStorage_InvalidStorageType => ResourceManager.GetString("DataStorage_InvalidStorageType", resourceCulture);

	internal static string DataStorage_ProblematicChars => ResourceManager.GetString("DataStorage_ProblematicChars", resourceCulture);

	internal static string DataStorage_SetInvalidDataType => ResourceManager.GetString("DataStorage_SetInvalidDataType", resourceCulture);

	internal static string DataTable_AlreadyInOtherDataSet => ResourceManager.GetString("DataTable_AlreadyInOtherDataSet", resourceCulture);

	internal static string DataTable_AlreadyInTheDataSet => ResourceManager.GetString("DataTable_AlreadyInTheDataSet", resourceCulture);

	internal static string DataTable_CannotAddToSimpleContent => ResourceManager.GetString("DataTable_CannotAddToSimpleContent", resourceCulture);

	internal static string DataTable_CanNotRemoteDataTable => ResourceManager.GetString("DataTable_CanNotRemoteDataTable", resourceCulture);

	internal static string DataTable_CanNotSerializeDataTableHierarchy => ResourceManager.GetString("DataTable_CanNotSerializeDataTableHierarchy", resourceCulture);

	internal static string DataTable_CanNotSerializeDataTableWithEmptyName => ResourceManager.GetString("DataTable_CanNotSerializeDataTableWithEmptyName", resourceCulture);

	internal static string DataTable_CanNotSetRemotingFormat => ResourceManager.GetString("DataTable_CanNotSetRemotingFormat", resourceCulture);

	internal static string DataTable_DatasetConflictingName => ResourceManager.GetString("DataTable_DatasetConflictingName", resourceCulture);

	internal static string DataTable_DuplicateName => ResourceManager.GetString("DataTable_DuplicateName", resourceCulture);

	internal static string DataTable_DuplicateName2 => ResourceManager.GetString("DataTable_DuplicateName2", resourceCulture);

	internal static string DataTable_ForeignPrimaryKey => ResourceManager.GetString("DataTable_ForeignPrimaryKey", resourceCulture);

	internal static string DataTable_InConstraint => ResourceManager.GetString("DataTable_InConstraint", resourceCulture);

	internal static string DataTable_InRelation => ResourceManager.GetString("DataTable_InRelation", resourceCulture);

	internal static string DataTable_InvalidSortString => ResourceManager.GetString("DataTable_InvalidSortString", resourceCulture);

	internal static string DataTable_MissingPrimaryKey => ResourceManager.GetString("DataTable_MissingPrimaryKey", resourceCulture);

	internal static string DataTable_MultipleSimpleContentColumns => ResourceManager.GetString("DataTable_MultipleSimpleContentColumns", resourceCulture);

	internal static string DataTable_NoName => ResourceManager.GetString("DataTable_NoName", resourceCulture);

	internal static string DataTable_NotInTheDataSet => ResourceManager.GetString("DataTable_NotInTheDataSet", resourceCulture);

	internal static string DataTable_OutOfRange => ResourceManager.GetString("DataTable_OutOfRange", resourceCulture);

	internal static string DataTable_SelfnestedDatasetConflictingName => ResourceManager.GetString("DataTable_SelfnestedDatasetConflictingName", resourceCulture);

	internal static string DataTable_TableNotFound => ResourceManager.GetString("DataTable_TableNotFound", resourceCulture);

	internal static string DataTableCaseSensitiveDescr => ResourceManager.GetString("DataTableCaseSensitiveDescr", resourceCulture);

	internal static string DataTableChildRelationsDescr => ResourceManager.GetString("DataTableChildRelationsDescr", resourceCulture);

	internal static string DataTableColumnChangedDescr => ResourceManager.GetString("DataTableColumnChangedDescr", resourceCulture);

	internal static string DataTableColumnChangingDescr => ResourceManager.GetString("DataTableColumnChangingDescr", resourceCulture);

	internal static string DataTableColumnsDescr => ResourceManager.GetString("DataTableColumnsDescr", resourceCulture);

	internal static string DataTableConstraintsDescr => ResourceManager.GetString("DataTableConstraintsDescr", resourceCulture);

	internal static string DataTableDataSetDescr => ResourceManager.GetString("DataTableDataSetDescr", resourceCulture);

	internal static string DataTableDefaultViewDescr => ResourceManager.GetString("DataTableDefaultViewDescr", resourceCulture);

	internal static string DataTableDisplayExpressionDescr => ResourceManager.GetString("DataTableDisplayExpressionDescr", resourceCulture);

	internal static string DataTableHasErrorsDescr => ResourceManager.GetString("DataTableHasErrorsDescr", resourceCulture);

	internal static string DataTableLocaleDescr => ResourceManager.GetString("DataTableLocaleDescr", resourceCulture);

	internal static string DataTableMapping_ColumnMappings => ResourceManager.GetString("DataTableMapping_ColumnMappings", resourceCulture);

	internal static string DataTableMapping_DataSetTable => ResourceManager.GetString("DataTableMapping_DataSetTable", resourceCulture);

	internal static string DataTableMapping_SourceTable => ResourceManager.GetString("DataTableMapping_SourceTable", resourceCulture);

	internal static string DataTableMappings_Count => ResourceManager.GetString("DataTableMappings_Count", resourceCulture);

	internal static string DataTableMappings_Item => ResourceManager.GetString("DataTableMappings_Item", resourceCulture);

	internal static string DataTableMinimumCapacityDescr => ResourceManager.GetString("DataTableMinimumCapacityDescr", resourceCulture);

	internal static string DataTableNamespaceDescr => ResourceManager.GetString("DataTableNamespaceDescr", resourceCulture);

	internal static string DataTableParentRelationsDescr => ResourceManager.GetString("DataTableParentRelationsDescr", resourceCulture);

	internal static string DataTablePrefixDescr => ResourceManager.GetString("DataTablePrefixDescr", resourceCulture);

	internal static string DataTablePrimaryKeyDescr => ResourceManager.GetString("DataTablePrimaryKeyDescr", resourceCulture);

	internal static string DataTableReader_ArgumentContainsNullValue => ResourceManager.GetString("DataTableReader_ArgumentContainsNullValue", resourceCulture);

	internal static string DataTableReader_CannotCreateDataReaderOnEmptyDataSet => ResourceManager.GetString("DataTableReader_CannotCreateDataReaderOnEmptyDataSet", resourceCulture);

	internal static string DataTableReader_DataTableCleared => ResourceManager.GetString("DataTableReader_DataTableCleared", resourceCulture);

	internal static string DataTableReader_DataTableReaderArgumentIsEmpty => ResourceManager.GetString("DataTableReader_DataTableReaderArgumentIsEmpty", resourceCulture);

	internal static string DataTableReader_InvalidDataTableReader => ResourceManager.GetString("DataTableReader_InvalidDataTableReader", resourceCulture);

	internal static string DataTableReader_InvalidRowInDataTableReader => ResourceManager.GetString("DataTableReader_InvalidRowInDataTableReader", resourceCulture);

	internal static string DataTableReader_SchemaInvalidDataTableReader => ResourceManager.GetString("DataTableReader_SchemaInvalidDataTableReader", resourceCulture);

	internal static string DataTableRowChangedDescr => ResourceManager.GetString("DataTableRowChangedDescr", resourceCulture);

	internal static string DataTableRowChangingDescr => ResourceManager.GetString("DataTableRowChangingDescr", resourceCulture);

	internal static string DataTableRowDeletedDescr => ResourceManager.GetString("DataTableRowDeletedDescr", resourceCulture);

	internal static string DataTableRowDeletingDescr => ResourceManager.GetString("DataTableRowDeletingDescr", resourceCulture);

	internal static string DataTableRowsClearedDescr => ResourceManager.GetString("DataTableRowsClearedDescr", resourceCulture);

	internal static string DataTableRowsClearingDescr => ResourceManager.GetString("DataTableRowsClearingDescr", resourceCulture);

	internal static string DataTableRowsDescr => ResourceManager.GetString("DataTableRowsDescr", resourceCulture);

	internal static string DataTableRowsNewRowDescr => ResourceManager.GetString("DataTableRowsNewRowDescr", resourceCulture);

	internal static string DataTableTableNameDescr => ResourceManager.GetString("DataTableTableNameDescr", resourceCulture);

	internal static string DataView_AddExternalObject => ResourceManager.GetString("DataView_AddExternalObject", resourceCulture);

	internal static string DataView_AddNewNotAllowNull => ResourceManager.GetString("DataView_AddNewNotAllowNull", resourceCulture);

	internal static string DataView_CanNotBindTable => ResourceManager.GetString("DataView_CanNotBindTable", resourceCulture);

	internal static string DataView_CanNotClear => ResourceManager.GetString("DataView_CanNotClear", resourceCulture);

	internal static string DataView_CanNotDelete => ResourceManager.GetString("DataView_CanNotDelete", resourceCulture);

	internal static string DataView_CanNotEdit => ResourceManager.GetString("DataView_CanNotEdit", resourceCulture);

	internal static string DataView_CanNotSetDataSet => ResourceManager.GetString("DataView_CanNotSetDataSet", resourceCulture);

	internal static string DataView_CanNotSetTable => ResourceManager.GetString("DataView_CanNotSetTable", resourceCulture);

	internal static string DataView_CanNotUse => ResourceManager.GetString("DataView_CanNotUse", resourceCulture);

	internal static string DataView_CanNotUseDataViewManager => ResourceManager.GetString("DataView_CanNotUseDataViewManager", resourceCulture);

	internal static string DataView_CreateChildView => ResourceManager.GetString("DataView_CreateChildView", resourceCulture);

	internal static string DataView_GetElementIndex => ResourceManager.GetString("DataView_GetElementIndex", resourceCulture);

	internal static string DataView_InsertExternalObject => ResourceManager.GetString("DataView_InsertExternalObject", resourceCulture);

	internal static string DataView_NotOpen => ResourceManager.GetString("DataView_NotOpen", resourceCulture);

	internal static string DataView_RemoveExternalObject => ResourceManager.GetString("DataView_RemoveExternalObject", resourceCulture);

	internal static string DataView_SetDataSetFailed => ResourceManager.GetString("DataView_SetDataSetFailed", resourceCulture);

	internal static string DataView_SetFailed => ResourceManager.GetString("DataView_SetFailed", resourceCulture);

	internal static string DataView_SetIListObject => ResourceManager.GetString("DataView_SetIListObject", resourceCulture);

	internal static string DataView_SetRowStateFilter => ResourceManager.GetString("DataView_SetRowStateFilter", resourceCulture);

	internal static string DataView_SetTable => ResourceManager.GetString("DataView_SetTable", resourceCulture);

	internal static string DataViewAllowDeleteDescr => ResourceManager.GetString("DataViewAllowDeleteDescr", resourceCulture);

	internal static string DataViewAllowEditDescr => ResourceManager.GetString("DataViewAllowEditDescr", resourceCulture);

	internal static string DataViewAllowNewDescr => ResourceManager.GetString("DataViewAllowNewDescr", resourceCulture);

	internal static string DataViewApplyDefaultSortDescr => ResourceManager.GetString("DataViewApplyDefaultSortDescr", resourceCulture);

	internal static string DataViewCountDescr => ResourceManager.GetString("DataViewCountDescr", resourceCulture);

	internal static string DataViewDataViewManagerDescr => ResourceManager.GetString("DataViewDataViewManagerDescr", resourceCulture);

	internal static string DataViewIsOpenDescr => ResourceManager.GetString("DataViewIsOpenDescr", resourceCulture);

	internal static string DataViewListChangedDescr => ResourceManager.GetString("DataViewListChangedDescr", resourceCulture);

	internal static string DataViewManagerDataSetDescr => ResourceManager.GetString("DataViewManagerDataSetDescr", resourceCulture);

	internal static string DataViewManagerTableSettingsDescr => ResourceManager.GetString("DataViewManagerTableSettingsDescr", resourceCulture);

	internal static string DataViewRowFilterDescr => ResourceManager.GetString("DataViewRowFilterDescr", resourceCulture);

	internal static string DataViewRowStateFilterDescr => ResourceManager.GetString("DataViewRowStateFilterDescr", resourceCulture);

	internal static string DataViewSortDescr => ResourceManager.GetString("DataViewSortDescr", resourceCulture);

	internal static string DataViewTableDescr => ResourceManager.GetString("DataViewTableDescr", resourceCulture);

	internal static string DbCommand_CommandText => ResourceManager.GetString("DbCommand_CommandText", resourceCulture);

	internal static string DbCommand_CommandTimeout => ResourceManager.GetString("DbCommand_CommandTimeout", resourceCulture);

	internal static string DbCommand_CommandType => ResourceManager.GetString("DbCommand_CommandType", resourceCulture);

	internal static string DbCommand_Connection => ResourceManager.GetString("DbCommand_Connection", resourceCulture);

	internal static string DbCommand_Parameters => ResourceManager.GetString("DbCommand_Parameters", resourceCulture);

	internal static string DbCommand_StatementCompleted => ResourceManager.GetString("DbCommand_StatementCompleted", resourceCulture);

	internal static string DbCommand_Transaction => ResourceManager.GetString("DbCommand_Transaction", resourceCulture);

	internal static string DbCommand_UpdatedRowSource => ResourceManager.GetString("DbCommand_UpdatedRowSource", resourceCulture);

	internal static string DbCommandBuilder_CatalogLocation => ResourceManager.GetString("DbCommandBuilder_CatalogLocation", resourceCulture);

	internal static string DbCommandBuilder_CatalogSeparator => ResourceManager.GetString("DbCommandBuilder_CatalogSeparator", resourceCulture);

	internal static string DbCommandBuilder_ConflictOption => ResourceManager.GetString("DbCommandBuilder_ConflictOption", resourceCulture);

	internal static string DbCommandBuilder_DataAdapter => ResourceManager.GetString("DbCommandBuilder_DataAdapter", resourceCulture);

	internal static string DbCommandBuilder_QuotePrefix => ResourceManager.GetString("DbCommandBuilder_QuotePrefix", resourceCulture);

	internal static string DbCommandBuilder_QuoteSuffix => ResourceManager.GetString("DbCommandBuilder_QuoteSuffix", resourceCulture);

	internal static string DbCommandBuilder_SchemaLocation => ResourceManager.GetString("DbCommandBuilder_SchemaLocation", resourceCulture);

	internal static string DbCommandBuilder_SchemaSeparator => ResourceManager.GetString("DbCommandBuilder_SchemaSeparator", resourceCulture);

	internal static string DbCommandBuilder_SetAllValues => ResourceManager.GetString("DbCommandBuilder_SetAllValues", resourceCulture);

	internal static string DbConnection_InfoMessage => ResourceManager.GetString("DbConnection_InfoMessage", resourceCulture);

	internal static string DbConnection_State => ResourceManager.GetString("DbConnection_State", resourceCulture);

	internal static string DbConnection_StateChange => ResourceManager.GetString("DbConnection_StateChange", resourceCulture);

	internal static string DbConnectionString_AdoNetPooler => ResourceManager.GetString("DbConnectionString_AdoNetPooler", resourceCulture);

	internal static string DbConnectionString_ApplicationIntent => ResourceManager.GetString("DbConnectionString_ApplicationIntent", resourceCulture);

	internal static string DbConnectionString_ApplicationName => ResourceManager.GetString("DbConnectionString_ApplicationName", resourceCulture);

	internal static string DbConnectionString_AsynchronousProcessing => ResourceManager.GetString("DbConnectionString_AsynchronousProcessing", resourceCulture);

	internal static string DbConnectionString_AttachDBFilename => ResourceManager.GetString("DbConnectionString_AttachDBFilename", resourceCulture);

	internal static string DbConnectionString_Authentication => ResourceManager.GetString("DbConnectionString_Authentication", resourceCulture);

	internal static string DbConnectionString_Certificate => ResourceManager.GetString("DbConnectionString_Certificate", resourceCulture);

	internal static string DbConnectionString_ConnectionReset => ResourceManager.GetString("DbConnectionString_ConnectionReset", resourceCulture);

	internal static string DbConnectionString_ConnectionString => ResourceManager.GetString("DbConnectionString_ConnectionString", resourceCulture);

	internal static string DbConnectionString_ConnectRetryCount => ResourceManager.GetString("DbConnectionString_ConnectRetryCount", resourceCulture);

	internal static string DbConnectionString_ConnectRetryInterval => ResourceManager.GetString("DbConnectionString_ConnectRetryInterval", resourceCulture);

	internal static string DbConnectionString_ConnectTimeout => ResourceManager.GetString("DbConnectionString_ConnectTimeout", resourceCulture);

	internal static string DbConnectionString_ContextConnection => ResourceManager.GetString("DbConnectionString_ContextConnection", resourceCulture);

	internal static string DbConnectionString_CurrentLanguage => ResourceManager.GetString("DbConnectionString_CurrentLanguage", resourceCulture);

	internal static string DbConnectionString_DataSource => ResourceManager.GetString("DbConnectionString_DataSource", resourceCulture);

	internal static string DbConnectionString_Driver => ResourceManager.GetString("DbConnectionString_Driver", resourceCulture);

	internal static string DbConnectionString_DSN => ResourceManager.GetString("DbConnectionString_DSN", resourceCulture);

	internal static string DbConnectionString_Encrypt => ResourceManager.GetString("DbConnectionString_Encrypt", resourceCulture);

	internal static string DbConnectionString_Enlist => ResourceManager.GetString("DbConnectionString_Enlist", resourceCulture);

	internal static string DbConnectionString_FailoverPartner => ResourceManager.GetString("DbConnectionString_FailoverPartner", resourceCulture);

	internal static string DbConnectionString_FileName => ResourceManager.GetString("DbConnectionString_FileName", resourceCulture);

	internal static string DbConnectionString_InitialCatalog => ResourceManager.GetString("DbConnectionString_InitialCatalog", resourceCulture);

	internal static string DbConnectionString_IntegratedSecurity => ResourceManager.GetString("DbConnectionString_IntegratedSecurity", resourceCulture);

	internal static string DbConnectionString_LoadBalanceTimeout => ResourceManager.GetString("DbConnectionString_LoadBalanceTimeout", resourceCulture);

	internal static string DbConnectionString_MaxPoolSize => ResourceManager.GetString("DbConnectionString_MaxPoolSize", resourceCulture);

	internal static string DbConnectionString_MinPoolSize => ResourceManager.GetString("DbConnectionString_MinPoolSize", resourceCulture);

	internal static string DbConnectionString_MultipleActiveResultSets => ResourceManager.GetString("DbConnectionString_MultipleActiveResultSets", resourceCulture);

	internal static string DbConnectionString_MultiSubnetFailover => ResourceManager.GetString("DbConnectionString_MultiSubnetFailover", resourceCulture);

	internal static string DbConnectionString_NetworkLibrary => ResourceManager.GetString("DbConnectionString_NetworkLibrary", resourceCulture);

	internal static string DbConnectionString_OleDbServices => ResourceManager.GetString("DbConnectionString_OleDbServices", resourceCulture);

	internal static string DbConnectionString_PacketSize => ResourceManager.GetString("DbConnectionString_PacketSize", resourceCulture);

	internal static string DbConnectionString_Password => ResourceManager.GetString("DbConnectionString_Password", resourceCulture);

	internal static string DbConnectionString_PersistSecurityInfo => ResourceManager.GetString("DbConnectionString_PersistSecurityInfo", resourceCulture);

	internal static string DbConnectionString_PoolBlockingPeriod => ResourceManager.GetString("DbConnectionString_PoolBlockingPeriod", resourceCulture);

	internal static string DbConnectionString_Pooling => ResourceManager.GetString("DbConnectionString_Pooling", resourceCulture);

	internal static string DbConnectionString_Provider => ResourceManager.GetString("DbConnectionString_Provider", resourceCulture);

	internal static string DbConnectionString_Replication => ResourceManager.GetString("DbConnectionString_Replication", resourceCulture);

	internal static string DbConnectionString_TransactionBinding => ResourceManager.GetString("DbConnectionString_TransactionBinding", resourceCulture);

	internal static string DbConnectionString_TransparentNetworkIPResolution => ResourceManager.GetString("DbConnectionString_TransparentNetworkIPResolution", resourceCulture);

	internal static string DbConnectionString_TrustServerCertificate => ResourceManager.GetString("DbConnectionString_TrustServerCertificate", resourceCulture);

	internal static string DbConnectionString_TypeSystemVersion => ResourceManager.GetString("DbConnectionString_TypeSystemVersion", resourceCulture);

	internal static string DbConnectionString_UserID => ResourceManager.GetString("DbConnectionString_UserID", resourceCulture);

	internal static string DbConnectionString_UserInstance => ResourceManager.GetString("DbConnectionString_UserInstance", resourceCulture);

	internal static string DbConnectionString_WorkstationID => ResourceManager.GetString("DbConnectionString_WorkstationID", resourceCulture);

	internal static string DbDataAdapter_DeleteCommand => ResourceManager.GetString("DbDataAdapter_DeleteCommand", resourceCulture);

	internal static string DbDataAdapter_InsertCommand => ResourceManager.GetString("DbDataAdapter_InsertCommand", resourceCulture);

	internal static string DbDataAdapter_RowUpdated => ResourceManager.GetString("DbDataAdapter_RowUpdated", resourceCulture);

	internal static string DbDataAdapter_RowUpdating => ResourceManager.GetString("DbDataAdapter_RowUpdating", resourceCulture);

	internal static string DbDataAdapter_SelectCommand => ResourceManager.GetString("DbDataAdapter_SelectCommand", resourceCulture);

	internal static string DbDataAdapter_UpdateBatchSize => ResourceManager.GetString("DbDataAdapter_UpdateBatchSize", resourceCulture);

	internal static string DbDataAdapter_UpdateCommand => ResourceManager.GetString("DbDataAdapter_UpdateCommand", resourceCulture);

	internal static string DbDataParameter_Precision => ResourceManager.GetString("DbDataParameter_Precision", resourceCulture);

	internal static string DbDataParameter_Scale => ResourceManager.GetString("DbDataParameter_Scale", resourceCulture);

	internal static string DbParameter_DbType => ResourceManager.GetString("DbParameter_DbType", resourceCulture);

	internal static string DbParameter_Direction => ResourceManager.GetString("DbParameter_Direction", resourceCulture);

	internal static string DbParameter_IsNullable => ResourceManager.GetString("DbParameter_IsNullable", resourceCulture);

	internal static string DbParameter_Offset => ResourceManager.GetString("DbParameter_Offset", resourceCulture);

	internal static string DbParameter_ParameterName => ResourceManager.GetString("DbParameter_ParameterName", resourceCulture);

	internal static string DbParameter_Size => ResourceManager.GetString("DbParameter_Size", resourceCulture);

	internal static string DbParameter_SourceColumn => ResourceManager.GetString("DbParameter_SourceColumn", resourceCulture);

	internal static string DbParameter_SourceColumnNullMapping => ResourceManager.GetString("DbParameter_SourceColumnNullMapping", resourceCulture);

	internal static string DbParameter_SourceVersion => ResourceManager.GetString("DbParameter_SourceVersion", resourceCulture);

	internal static string DbParameter_Value => ResourceManager.GetString("DbParameter_Value", resourceCulture);

	internal static string DbTable_ConflictDetection => ResourceManager.GetString("DbTable_ConflictDetection", resourceCulture);

	internal static string DbTable_Connection => ResourceManager.GetString("DbTable_Connection", resourceCulture);

	internal static string DbTable_DeleteCommand => ResourceManager.GetString("DbTable_DeleteCommand", resourceCulture);

	internal static string DbTable_InsertCommand => ResourceManager.GetString("DbTable_InsertCommand", resourceCulture);

	internal static string DbTable_ReturnProviderSpecificTypes => ResourceManager.GetString("DbTable_ReturnProviderSpecificTypes", resourceCulture);

	internal static string DbTable_SelectCommand => ResourceManager.GetString("DbTable_SelectCommand", resourceCulture);

	internal static string DbTable_TableMapping => ResourceManager.GetString("DbTable_TableMapping", resourceCulture);

	internal static string DbTable_UpdateBatchSize => ResourceManager.GetString("DbTable_UpdateBatchSize", resourceCulture);

	internal static string DbTable_UpdateCommand => ResourceManager.GetString("DbTable_UpdateCommand", resourceCulture);

	internal static string EnclaveRetrySleepInSecondsValueException => ResourceManager.GetString("EnclaveRetrySleepInSecondsValueException", resourceCulture);

	internal static string EnclaveSessionInvalidationFailed => ResourceManager.GetString("EnclaveSessionInvalidationFailed", resourceCulture);

	internal static string ExpiredAttestationToken => ResourceManager.GetString("ExpiredAttestationToken", resourceCulture);

	internal static string Expr_AggregateArgument => ResourceManager.GetString("Expr_AggregateArgument", resourceCulture);

	internal static string Expr_AggregateUnbound => ResourceManager.GetString("Expr_AggregateUnbound", resourceCulture);

	internal static string Expr_AmbiguousBinop => ResourceManager.GetString("Expr_AmbiguousBinop", resourceCulture);

	internal static string Expr_ArgumentOutofRange => ResourceManager.GetString("Expr_ArgumentOutofRange", resourceCulture);

	internal static string Expr_ArgumentType => ResourceManager.GetString("Expr_ArgumentType", resourceCulture);

	internal static string Expr_ArgumentTypeInteger => ResourceManager.GetString("Expr_ArgumentTypeInteger", resourceCulture);

	internal static string Expr_BindFailure => ResourceManager.GetString("Expr_BindFailure", resourceCulture);

	internal static string Expr_ComputeNotAggregate => ResourceManager.GetString("Expr_ComputeNotAggregate", resourceCulture);

	internal static string Expr_DatatypeConvertion => ResourceManager.GetString("Expr_DatatypeConvertion", resourceCulture);

	internal static string Expr_DatavalueConvertion => ResourceManager.GetString("Expr_DatavalueConvertion", resourceCulture);

	internal static string Expr_DivideByZero => ResourceManager.GetString("Expr_DivideByZero", resourceCulture);

	internal static string Expr_EvalNoContext => ResourceManager.GetString("Expr_EvalNoContext", resourceCulture);

	internal static string Expr_ExpressionTooComplex => ResourceManager.GetString("Expr_ExpressionTooComplex", resourceCulture);

	internal static string Expr_ExpressionUnbound => ResourceManager.GetString("Expr_ExpressionUnbound", resourceCulture);

	internal static string Expr_FilterConvertion => ResourceManager.GetString("Expr_FilterConvertion", resourceCulture);

	internal static string Expr_FunctionArgumentCount => ResourceManager.GetString("Expr_FunctionArgumentCount", resourceCulture);

	internal static string Expr_InvalidDate => ResourceManager.GetString("Expr_InvalidDate", resourceCulture);

	internal static string Expr_InvalidHoursArgument => ResourceManager.GetString("Expr_InvalidHoursArgument", resourceCulture);

	internal static string Expr_InvalidMinutesArgument => ResourceManager.GetString("Expr_InvalidMinutesArgument", resourceCulture);

	internal static string Expr_InvalidName => ResourceManager.GetString("Expr_InvalidName", resourceCulture);

	internal static string Expr_InvalidNameBracketing => ResourceManager.GetString("Expr_InvalidNameBracketing", resourceCulture);

	internal static string Expr_InvalidPattern => ResourceManager.GetString("Expr_InvalidPattern", resourceCulture);

	internal static string Expr_InvalidString => ResourceManager.GetString("Expr_InvalidString", resourceCulture);

	internal static string Expr_InvalidTimeZoneRange => ResourceManager.GetString("Expr_InvalidTimeZoneRange", resourceCulture);

	internal static string Expr_InvalidType => ResourceManager.GetString("Expr_InvalidType", resourceCulture);

	internal static string Expr_InvokeArgument => ResourceManager.GetString("Expr_InvokeArgument", resourceCulture);

	internal static string Expr_InWithoutList => ResourceManager.GetString("Expr_InWithoutList", resourceCulture);

	internal static string Expr_InWithoutParentheses => ResourceManager.GetString("Expr_InWithoutParentheses", resourceCulture);

	internal static string Expr_IsSyntax => ResourceManager.GetString("Expr_IsSyntax", resourceCulture);

	internal static string Expr_LookupArgument => ResourceManager.GetString("Expr_LookupArgument", resourceCulture);

	internal static string Expr_MismatchKindandTimeSpan => ResourceManager.GetString("Expr_MismatchKindandTimeSpan", resourceCulture);

	internal static string Expr_MissingOperand => ResourceManager.GetString("Expr_MissingOperand", resourceCulture);

	internal static string Expr_MissingOperandBefore => ResourceManager.GetString("Expr_MissingOperandBefore", resourceCulture);

	internal static string Expr_MissingRightParen => ResourceManager.GetString("Expr_MissingRightParen", resourceCulture);

	internal static string Expr_NonConstantArgument => ResourceManager.GetString("Expr_NonConstantArgument", resourceCulture);

	internal static string Expr_NYI => ResourceManager.GetString("Expr_NYI", resourceCulture);

	internal static string Expr_Overflow => ResourceManager.GetString("Expr_Overflow", resourceCulture);

	internal static string Expr_Syntax => ResourceManager.GetString("Expr_Syntax", resourceCulture);

	internal static string Expr_TooManyRightParentheses => ResourceManager.GetString("Expr_TooManyRightParentheses", resourceCulture);

	internal static string Expr_TypeMismatch => ResourceManager.GetString("Expr_TypeMismatch", resourceCulture);

	internal static string Expr_TypeMismatchInBinop => ResourceManager.GetString("Expr_TypeMismatchInBinop", resourceCulture);

	internal static string Expr_UnboundName => ResourceManager.GetString("Expr_UnboundName", resourceCulture);

	internal static string Expr_UndefinedFunction => ResourceManager.GetString("Expr_UndefinedFunction", resourceCulture);

	internal static string Expr_UnknownToken => ResourceManager.GetString("Expr_UnknownToken", resourceCulture);

	internal static string Expr_UnknownToken1 => ResourceManager.GetString("Expr_UnknownToken1", resourceCulture);

	internal static string Expr_UnresolvedRelation => ResourceManager.GetString("Expr_UnresolvedRelation", resourceCulture);

	internal static string Expr_UnsupportedOperator => ResourceManager.GetString("Expr_UnsupportedOperator", resourceCulture);

	internal static string Expr_UnsupportedType => ResourceManager.GetString("Expr_UnsupportedType", resourceCulture);

	internal static string ExtendedPropertiesDescr => ResourceManager.GetString("ExtendedPropertiesDescr", resourceCulture);

	internal static string FailToCreateEnclaveSession => ResourceManager.GetString("FailToCreateEnclaveSession", resourceCulture);

	internal static string FailToParseAttestationInfo => ResourceManager.GetString("FailToParseAttestationInfo", resourceCulture);

	internal static string FailToParseAttestationToken => ResourceManager.GetString("FailToParseAttestationToken", resourceCulture);

	internal static string ForeignKeyConstraintAcceptRejectRuleDescr => ResourceManager.GetString("ForeignKeyConstraintAcceptRejectRuleDescr", resourceCulture);

	internal static string ForeignKeyConstraintChildColumnsDescr => ResourceManager.GetString("ForeignKeyConstraintChildColumnsDescr", resourceCulture);

	internal static string ForeignKeyConstraintDeleteRuleDescr => ResourceManager.GetString("ForeignKeyConstraintDeleteRuleDescr", resourceCulture);

	internal static string ForeignKeyConstraintParentColumnsDescr => ResourceManager.GetString("ForeignKeyConstraintParentColumnsDescr", resourceCulture);

	internal static string ForeignKeyConstraintUpdateRuleDescr => ResourceManager.GetString("ForeignKeyConstraintUpdateRuleDescr", resourceCulture);

	internal static string ForeignKeyRelatedTableDescr => ResourceManager.GetString("ForeignKeyRelatedTableDescr", resourceCulture);

	internal static string GetAttestationSigningCertificateFailedInvalidCertificate => ResourceManager.GetString("GetAttestationSigningCertificateFailedInvalidCertificate", resourceCulture);

	internal static string GetAttestationSigningCertificateRequestFailedFormat => ResourceManager.GetString("GetAttestationSigningCertificateRequestFailedFormat", resourceCulture);

	internal static string GetAttestationTokenSigningKeysFailed => ResourceManager.GetString("GetAttestationTokenSigningKeysFailed", resourceCulture);

	internal static string GetSharedSecretFailed => ResourceManager.GetString("GetSharedSecretFailed", resourceCulture);

	internal static string GT_Disabled => ResourceManager.GetString("GT_Disabled", resourceCulture);

	internal static string GT_UnsupportedSysTxVersion => ResourceManager.GetString("GT_UnsupportedSysTxVersion", resourceCulture);

	internal static string IEnumerableOfSqlDataRecordHasNoRows => ResourceManager.GetString("IEnumerableOfSqlDataRecordHasNoRows", resourceCulture);

	internal static string InvalidArgumentToBase64UrlDecoder => ResourceManager.GetString("InvalidArgumentToBase64UrlDecoder", resourceCulture);

	internal static string InvalidArgumentToSHA256 => ResourceManager.GetString("InvalidArgumentToSHA256", resourceCulture);

	internal static string InvalidAttestationToken => ResourceManager.GetString("InvalidAttestationToken", resourceCulture);

	internal static string InvalidClaimInAttestationToken => ResourceManager.GetString("InvalidClaimInAttestationToken", resourceCulture);

	internal static string InvalidSchemaTableOrdinals => ResourceManager.GetString("InvalidSchemaTableOrdinals", resourceCulture);

	internal static string KeyConstraintColumnsDescr => ResourceManager.GetString("KeyConstraintColumnsDescr", resourceCulture);

	internal static string KeyConstraintIsPrimaryKeyDescr => ResourceManager.GetString("KeyConstraintIsPrimaryKeyDescr", resourceCulture);

	internal static string Load_ReadOnlyDataModified => ResourceManager.GetString("Load_ReadOnlyDataModified", resourceCulture);

	internal static string LocalDB_BadConfigSectionType => ResourceManager.GetString("LocalDB_BadConfigSectionType", resourceCulture);

	internal static string LocalDB_CreateFailed => ResourceManager.GetString("LocalDB_CreateFailed", resourceCulture);

	internal static string LocalDB_FailedGetDLLHandle => ResourceManager.GetString("LocalDB_FailedGetDLLHandle", resourceCulture);

	internal static string LocalDB_InvalidVersion => ResourceManager.GetString("LocalDB_InvalidVersion", resourceCulture);

	internal static string LocalDB_MethodNotFound => ResourceManager.GetString("LocalDB_MethodNotFound", resourceCulture);

	internal static string LocalDB_UnobtainableMessage => ResourceManager.GetString("LocalDB_UnobtainableMessage", resourceCulture);

	internal static string MDF_AmbiguousCollectionName => ResourceManager.GetString("MDF_AmbiguousCollectionName", resourceCulture);

	internal static string MDF_CollectionNameISNotUnique => ResourceManager.GetString("MDF_CollectionNameISNotUnique", resourceCulture);

	internal static string MDF_DataTableDoesNotExist => ResourceManager.GetString("MDF_DataTableDoesNotExist", resourceCulture);

	internal static string MDF_IncorrectNumberOfDataSourceInformationRows => ResourceManager.GetString("MDF_IncorrectNumberOfDataSourceInformationRows", resourceCulture);

	internal static string MDF_InvalidRestrictionValue => ResourceManager.GetString("MDF_InvalidRestrictionValue", resourceCulture);

	internal static string MDF_InvalidXml => ResourceManager.GetString("MDF_InvalidXml", resourceCulture);

	internal static string MDF_InvalidXmlInvalidValue => ResourceManager.GetString("MDF_InvalidXmlInvalidValue", resourceCulture);

	internal static string MDF_InvalidXmlMissingColumn => ResourceManager.GetString("MDF_InvalidXmlMissingColumn", resourceCulture);

	internal static string MDF_MissingDataSourceInformationColumn => ResourceManager.GetString("MDF_MissingDataSourceInformationColumn", resourceCulture);

	internal static string MDF_MissingRestrictionColumn => ResourceManager.GetString("MDF_MissingRestrictionColumn", resourceCulture);

	internal static string MDF_MissingRestrictionRow => ResourceManager.GetString("MDF_MissingRestrictionRow", resourceCulture);

	internal static string MDF_NoColumns => ResourceManager.GetString("MDF_NoColumns", resourceCulture);

	internal static string MDF_QueryFailed => ResourceManager.GetString("MDF_QueryFailed", resourceCulture);

	internal static string MDF_TooManyRestrictions => ResourceManager.GetString("MDF_TooManyRestrictions", resourceCulture);

	internal static string MDF_UnableToBuildCollection => ResourceManager.GetString("MDF_UnableToBuildCollection", resourceCulture);

	internal static string MDF_UndefinedCollection => ResourceManager.GetString("MDF_UndefinedCollection", resourceCulture);

	internal static string MDF_UndefinedPopulationMechanism => ResourceManager.GetString("MDF_UndefinedPopulationMechanism", resourceCulture);

	internal static string MDF_UnsupportedVersion => ResourceManager.GetString("MDF_UnsupportedVersion", resourceCulture);

	internal static string MetaType_SingleValuedStructNotSupported => ResourceManager.GetString("MetaType_SingleValuedStructNotSupported", resourceCulture);

	internal static string MissingClaimInAttestationToken => ResourceManager.GetString("MissingClaimInAttestationToken", resourceCulture);

	internal static string NamedSimpleType_InvalidDuplicateNamedSimpleTypeDelaration => ResourceManager.GetString("NamedSimpleType_InvalidDuplicateNamedSimpleTypeDelaration", resourceCulture);

	internal static string net_invalid_enum => ResourceManager.GetString("net_invalid_enum", resourceCulture);

	internal static string NullSchemaTableDataTypeNotSupported => ResourceManager.GetString("NullSchemaTableDataTypeNotSupported", resourceCulture);

	internal static string Odbc_CantAllocateEnvironmentHandle => ResourceManager.GetString("Odbc_CantAllocateEnvironmentHandle", resourceCulture);

	internal static string Odbc_CantEnableConnectionpooling => ResourceManager.GetString("Odbc_CantEnableConnectionpooling", resourceCulture);

	internal static string Odbc_CantSetPropertyOnOpenConnection => ResourceManager.GetString("Odbc_CantSetPropertyOnOpenConnection", resourceCulture);

	internal static string Odbc_ConnectionClosed => ResourceManager.GetString("Odbc_ConnectionClosed", resourceCulture);

	internal static string Odbc_ExceptionMessage => ResourceManager.GetString("Odbc_ExceptionMessage", resourceCulture);

	internal static string Odbc_ExceptionNoInfoMsg => ResourceManager.GetString("Odbc_ExceptionNoInfoMsg", resourceCulture);

	internal static string Odbc_FailedToGetDescriptorHandle => ResourceManager.GetString("Odbc_FailedToGetDescriptorHandle", resourceCulture);

	internal static string ODBC_GetSchemaRestrictionRequired => ResourceManager.GetString("ODBC_GetSchemaRestrictionRequired", resourceCulture);

	internal static string Odbc_GetTypeMapping_UnknownType => ResourceManager.GetString("Odbc_GetTypeMapping_UnknownType", resourceCulture);

	internal static string Odbc_MDACWrongVersion => ResourceManager.GetString("Odbc_MDACWrongVersion", resourceCulture);

	internal static string Odbc_NegativeArgument => ResourceManager.GetString("Odbc_NegativeArgument", resourceCulture);

	internal static string Odbc_NoMappingForSqlTransactionLevel => ResourceManager.GetString("Odbc_NoMappingForSqlTransactionLevel", resourceCulture);

	internal static string Odbc_NotInTransaction => ResourceManager.GetString("Odbc_NotInTransaction", resourceCulture);

	internal static string ODBC_NotSupportedEnumerationValue => ResourceManager.GetString("ODBC_NotSupportedEnumerationValue", resourceCulture);

	internal static string Odbc_NullData => ResourceManager.GetString("Odbc_NullData", resourceCulture);

	internal static string ODBC_ODBCCommandText => ResourceManager.GetString("ODBC_ODBCCommandText", resourceCulture);

	internal static string Odbc_OpenConnectionNoOwner => ResourceManager.GetString("Odbc_OpenConnectionNoOwner", resourceCulture);

	internal static string Odbc_UnknownOdbcType => ResourceManager.GetString("Odbc_UnknownOdbcType", resourceCulture);

	internal static string Odbc_UnknownSQLType => ResourceManager.GetString("Odbc_UnknownSQLType", resourceCulture);

	internal static string Odbc_UnknownURTType => ResourceManager.GetString("Odbc_UnknownURTType", resourceCulture);

	internal static string OdbcCommandBuilder_DataAdapter => ResourceManager.GetString("OdbcCommandBuilder_DataAdapter", resourceCulture);

	internal static string OdbcCommandBuilder_QuotePrefix => ResourceManager.GetString("OdbcCommandBuilder_QuotePrefix", resourceCulture);

	internal static string OdbcCommandBuilder_QuoteSuffix => ResourceManager.GetString("OdbcCommandBuilder_QuoteSuffix", resourceCulture);

	internal static string OdbcConnection_ConnectionString => ResourceManager.GetString("OdbcConnection_ConnectionString", resourceCulture);

	internal static string OdbcConnection_ConnectionStringTooLong => ResourceManager.GetString("OdbcConnection_ConnectionStringTooLong", resourceCulture);

	internal static string OdbcConnection_ConnectionTimeout => ResourceManager.GetString("OdbcConnection_ConnectionTimeout", resourceCulture);

	internal static string OdbcConnection_Database => ResourceManager.GetString("OdbcConnection_Database", resourceCulture);

	internal static string OdbcConnection_DataSource => ResourceManager.GetString("OdbcConnection_DataSource", resourceCulture);

	internal static string OdbcConnection_Driver => ResourceManager.GetString("OdbcConnection_Driver", resourceCulture);

	internal static string OdbcConnection_ServerVersion => ResourceManager.GetString("OdbcConnection_ServerVersion", resourceCulture);

	internal static string OdbcParameter_OdbcType => ResourceManager.GetString("OdbcParameter_OdbcType", resourceCulture);

	internal static string OleDb_AsynchronousNotSupported => ResourceManager.GetString("OleDb_AsynchronousNotSupported", resourceCulture);

	internal static string OleDb_BadAccessor => ResourceManager.GetString("OleDb_BadAccessor", resourceCulture);

	internal static string OleDb_BadStatus_ParamAcc => ResourceManager.GetString("OleDb_BadStatus_ParamAcc", resourceCulture);

	internal static string OleDb_BadStatusRowAccessor => ResourceManager.GetString("OleDb_BadStatusRowAccessor", resourceCulture);

	internal static string OleDb_CanNotDetermineDecimalSeparator => ResourceManager.GetString("OleDb_CanNotDetermineDecimalSeparator", resourceCulture);

	internal static string OleDb_CantConvertValue => ResourceManager.GetString("OleDb_CantConvertValue", resourceCulture);

	internal static string OleDb_CantCreate => ResourceManager.GetString("OleDb_CantCreate", resourceCulture);

	internal static string OleDb_CommandParameterBadAccessor => ResourceManager.GetString("OleDb_CommandParameterBadAccessor", resourceCulture);

	internal static string OleDb_CommandParameterCantConvertValue => ResourceManager.GetString("OleDb_CommandParameterCantConvertValue", resourceCulture);

	internal static string OleDb_CommandParameterDataOverflow => ResourceManager.GetString("OleDb_CommandParameterDataOverflow", resourceCulture);

	internal static string OleDb_CommandParameterDefault => ResourceManager.GetString("OleDb_CommandParameterDefault", resourceCulture);

	internal static string OleDb_CommandParameterError => ResourceManager.GetString("OleDb_CommandParameterError", resourceCulture);

	internal static string OleDb_CommandParameterSignMismatch => ResourceManager.GetString("OleDb_CommandParameterSignMismatch", resourceCulture);

	internal static string OleDb_CommandParameterUnavailable => ResourceManager.GetString("OleDb_CommandParameterUnavailable", resourceCulture);

	internal static string OleDb_CommandTextNotSupported => ResourceManager.GetString("OleDb_CommandTextNotSupported", resourceCulture);

	internal static string OleDb_ConfigUnableToLoadXmlMetaDataFile => ResourceManager.GetString("OleDb_ConfigUnableToLoadXmlMetaDataFile", resourceCulture);

	internal static string OleDb_ConfigWrongNumberOfValues => ResourceManager.GetString("OleDb_ConfigWrongNumberOfValues", resourceCulture);

	internal static string OleDb_ConnectionStringSyntax => ResourceManager.GetString("OleDb_ConnectionStringSyntax", resourceCulture);

	internal static string OleDb_DataOverflow => ResourceManager.GetString("OleDb_DataOverflow", resourceCulture);

	internal static string OleDb_DBBindingGetVector => ResourceManager.GetString("OleDb_DBBindingGetVector", resourceCulture);

	internal static string OleDb_FailedGetDescription => ResourceManager.GetString("OleDb_FailedGetDescription", resourceCulture);

	internal static string OleDb_FailedGetSource => ResourceManager.GetString("OleDb_FailedGetSource", resourceCulture);

	internal static string OleDb_Fill_EmptyRecord => ResourceManager.GetString("OleDb_Fill_EmptyRecord", resourceCulture);

	internal static string OleDb_Fill_EmptyRecordSet => ResourceManager.GetString("OleDb_Fill_EmptyRecordSet", resourceCulture);

	internal static string OleDb_Fill_NotADODB => ResourceManager.GetString("OleDb_Fill_NotADODB", resourceCulture);

	internal static string OleDb_GVtUnknown => ResourceManager.GetString("OleDb_GVtUnknown", resourceCulture);

	internal static string OleDb_IDBInfoNotSupported => ResourceManager.GetString("OleDb_IDBInfoNotSupported", resourceCulture);

	internal static string OleDb_InvalidProviderSpecified => ResourceManager.GetString("OleDb_InvalidProviderSpecified", resourceCulture);

	internal static string OleDb_InvalidRestrictionsDbInfoKeywords => ResourceManager.GetString("OleDb_InvalidRestrictionsDbInfoKeywords", resourceCulture);

	internal static string OleDb_InvalidRestrictionsDbInfoLiteral => ResourceManager.GetString("OleDb_InvalidRestrictionsDbInfoLiteral", resourceCulture);

	internal static string OleDb_InvalidRestrictionsSchemaGuids => ResourceManager.GetString("OleDb_InvalidRestrictionsSchemaGuids", resourceCulture);

	internal static string OleDb_ISourcesRowsetNotSupported => ResourceManager.GetString("OleDb_ISourcesRowsetNotSupported", resourceCulture);

	internal static string OleDb_MDACNotAvailable => ResourceManager.GetString("OleDb_MDACNotAvailable", resourceCulture);

	internal static string OleDb_MDACWrongVersion => ResourceManager.GetString("OleDb_MDACWrongVersion", resourceCulture);

	internal static string OleDb_MSDASQLNotSupported => ResourceManager.GetString("OleDb_MSDASQLNotSupported", resourceCulture);

	internal static string OleDb_NoErrorInformation => ResourceManager.GetString("OleDb_NoErrorInformation", resourceCulture);

	internal static string OleDb_NoErrorInformation2 => ResourceManager.GetString("OleDb_NoErrorInformation2", resourceCulture);

	internal static string OleDb_NoErrorMessage => ResourceManager.GetString("OleDb_NoErrorMessage", resourceCulture);

	internal static string OleDb_NoProviderSpecified => ResourceManager.GetString("OleDb_NoProviderSpecified", resourceCulture);

	internal static string OleDb_NoProviderSupportForParameters => ResourceManager.GetString("OleDb_NoProviderSupportForParameters", resourceCulture);

	internal static string OleDb_NoProviderSupportForSProcResetParameters => ResourceManager.GetString("OleDb_NoProviderSupportForSProcResetParameters", resourceCulture);

	internal static string OLEDB_NotSupportedEnumerationValue => ResourceManager.GetString("OLEDB_NotSupportedEnumerationValue", resourceCulture);

	internal static string OleDb_NotSupportedSchemaTable => ResourceManager.GetString("OleDb_NotSupportedSchemaTable", resourceCulture);

	internal static string OLEDB_OLEDBCommandText => ResourceManager.GetString("OLEDB_OLEDBCommandText", resourceCulture);

	internal static string OleDb_PossiblePromptNotUserInteractive => ResourceManager.GetString("OleDb_PossiblePromptNotUserInteractive", resourceCulture);

	internal static string OleDb_PropertyBadColumn => ResourceManager.GetString("OleDb_PropertyBadColumn", resourceCulture);

	internal static string OleDb_PropertyBadOption => ResourceManager.GetString("OleDb_PropertyBadOption", resourceCulture);

	internal static string OleDb_PropertyBadValue => ResourceManager.GetString("OleDb_PropertyBadValue", resourceCulture);

	internal static string OleDb_PropertyConflicting => ResourceManager.GetString("OleDb_PropertyConflicting", resourceCulture);

	internal static string OleDb_PropertyNotAllSettable => ResourceManager.GetString("OleDb_PropertyNotAllSettable", resourceCulture);

	internal static string OleDb_PropertyNotAvailable => ResourceManager.GetString("OleDb_PropertyNotAvailable", resourceCulture);

	internal static string OleDb_PropertyNotSet => ResourceManager.GetString("OleDb_PropertyNotSet", resourceCulture);

	internal static string OleDb_PropertyNotSettable => ResourceManager.GetString("OleDb_PropertyNotSettable", resourceCulture);

	internal static string OleDb_PropertyNotSupported => ResourceManager.GetString("OleDb_PropertyNotSupported", resourceCulture);

	internal static string OleDb_PropertyStatusUnknown => ResourceManager.GetString("OleDb_PropertyStatusUnknown", resourceCulture);

	internal static string OleDb_ProviderUnavailable => ResourceManager.GetString("OleDb_ProviderUnavailable", resourceCulture);

	internal static string OleDb_SchemaRowsetsNotSupported => ResourceManager.GetString("OleDb_SchemaRowsetsNotSupported", resourceCulture);

	internal static string OleDb_SignMismatch => ResourceManager.GetString("OleDb_SignMismatch", resourceCulture);

	internal static string OleDb_SVtUnknown => ResourceManager.GetString("OleDb_SVtUnknown", resourceCulture);

	internal static string OleDb_ThreadApartmentState => ResourceManager.GetString("OleDb_ThreadApartmentState", resourceCulture);

	internal static string OleDb_TransactionsNotSupported => ResourceManager.GetString("OleDb_TransactionsNotSupported", resourceCulture);

	internal static string OleDb_Unavailable => ResourceManager.GetString("OleDb_Unavailable", resourceCulture);

	internal static string OleDb_UnexpectedStatusValue => ResourceManager.GetString("OleDb_UnexpectedStatusValue", resourceCulture);

	internal static string OleDb_UninitializedParameters => ResourceManager.GetString("OleDb_UninitializedParameters", resourceCulture);

	internal static string OleDbCommandBuilder_DataAdapter => ResourceManager.GetString("OleDbCommandBuilder_DataAdapter", resourceCulture);

	internal static string OleDbCommandBuilder_DecimalSeparator => ResourceManager.GetString("OleDbCommandBuilder_DecimalSeparator", resourceCulture);

	internal static string OleDbCommandBuilder_QuotePrefix => ResourceManager.GetString("OleDbCommandBuilder_QuotePrefix", resourceCulture);

	internal static string OleDbCommandBuilder_QuoteSuffix => ResourceManager.GetString("OleDbCommandBuilder_QuoteSuffix", resourceCulture);

	internal static string OleDbConnection_ConnectionString => ResourceManager.GetString("OleDbConnection_ConnectionString", resourceCulture);

	internal static string OleDbConnection_ConnectionTimeout => ResourceManager.GetString("OleDbConnection_ConnectionTimeout", resourceCulture);

	internal static string OleDbConnection_Database => ResourceManager.GetString("OleDbConnection_Database", resourceCulture);

	internal static string OleDbConnection_DataSource => ResourceManager.GetString("OleDbConnection_DataSource", resourceCulture);

	internal static string OleDbConnection_Provider => ResourceManager.GetString("OleDbConnection_Provider", resourceCulture);

	internal static string OleDbConnection_ServerVersion => ResourceManager.GetString("OleDbConnection_ServerVersion", resourceCulture);

	internal static string OleDbParameter_OleDbType => ResourceManager.GetString("OleDbParameter_OleDbType", resourceCulture);

	internal static string propertyChangedEventDescr => ResourceManager.GetString("propertyChangedEventDescr", resourceCulture);

	internal static string Range_Argument => ResourceManager.GetString("Range_Argument", resourceCulture);

	internal static string Range_NullRange => ResourceManager.GetString("Range_NullRange", resourceCulture);

	internal static string RbTree_EnumerationBroken => ResourceManager.GetString("RbTree_EnumerationBroken", resourceCulture);

	internal static string RbTree_InvalidState => ResourceManager.GetString("RbTree_InvalidState", resourceCulture);

	internal static string RecordManager_MinimumCapacity => ResourceManager.GetString("RecordManager_MinimumCapacity", resourceCulture);

	internal static string SEC_ProtocolWarning => ResourceManager.GetString("SEC_ProtocolWarning", resourceCulture);

	internal static string SNI_ERROR_1 => ResourceManager.GetString("SNI_ERROR_1", resourceCulture);

	internal static string SNI_ERROR_10 => ResourceManager.GetString("SNI_ERROR_10", resourceCulture);

	internal static string SNI_ERROR_11 => ResourceManager.GetString("SNI_ERROR_11", resourceCulture);

	internal static string SNI_ERROR_12 => ResourceManager.GetString("SNI_ERROR_12", resourceCulture);

	internal static string SNI_ERROR_13 => ResourceManager.GetString("SNI_ERROR_13", resourceCulture);

	internal static string SNI_ERROR_14 => ResourceManager.GetString("SNI_ERROR_14", resourceCulture);

	internal static string SNI_ERROR_15 => ResourceManager.GetString("SNI_ERROR_15", resourceCulture);

	internal static string SNI_ERROR_16 => ResourceManager.GetString("SNI_ERROR_16", resourceCulture);

	internal static string SNI_ERROR_17 => ResourceManager.GetString("SNI_ERROR_17", resourceCulture);

	internal static string SNI_ERROR_18 => ResourceManager.GetString("SNI_ERROR_18", resourceCulture);

	internal static string SNI_ERROR_19 => ResourceManager.GetString("SNI_ERROR_19", resourceCulture);

	internal static string SNI_ERROR_2 => ResourceManager.GetString("SNI_ERROR_2", resourceCulture);

	internal static string SNI_ERROR_20 => ResourceManager.GetString("SNI_ERROR_20", resourceCulture);

	internal static string SNI_ERROR_21 => ResourceManager.GetString("SNI_ERROR_21", resourceCulture);

	internal static string SNI_ERROR_22 => ResourceManager.GetString("SNI_ERROR_22", resourceCulture);

	internal static string SNI_ERROR_23 => ResourceManager.GetString("SNI_ERROR_23", resourceCulture);

	internal static string SNI_ERROR_24 => ResourceManager.GetString("SNI_ERROR_24", resourceCulture);

	internal static string SNI_ERROR_25 => ResourceManager.GetString("SNI_ERROR_25", resourceCulture);

	internal static string SNI_ERROR_26 => ResourceManager.GetString("SNI_ERROR_26", resourceCulture);

	internal static string SNI_ERROR_27 => ResourceManager.GetString("SNI_ERROR_27", resourceCulture);

	internal static string SNI_ERROR_28 => ResourceManager.GetString("SNI_ERROR_28", resourceCulture);

	internal static string SNI_ERROR_29 => ResourceManager.GetString("SNI_ERROR_29", resourceCulture);

	internal static string SNI_ERROR_3 => ResourceManager.GetString("SNI_ERROR_3", resourceCulture);

	internal static string SNI_ERROR_30 => ResourceManager.GetString("SNI_ERROR_30", resourceCulture);

	internal static string SNI_ERROR_31 => ResourceManager.GetString("SNI_ERROR_31", resourceCulture);

	internal static string SNI_ERROR_32 => ResourceManager.GetString("SNI_ERROR_32", resourceCulture);

	internal static string SNI_ERROR_33 => ResourceManager.GetString("SNI_ERROR_33", resourceCulture);

	internal static string SNI_ERROR_34 => ResourceManager.GetString("SNI_ERROR_34", resourceCulture);

	internal static string SNI_ERROR_35 => ResourceManager.GetString("SNI_ERROR_35", resourceCulture);

	internal static string SNI_ERROR_36 => ResourceManager.GetString("SNI_ERROR_36", resourceCulture);

	internal static string SNI_ERROR_37 => ResourceManager.GetString("SNI_ERROR_37", resourceCulture);

	internal static string SNI_ERROR_38 => ResourceManager.GetString("SNI_ERROR_38", resourceCulture);

	internal static string SNI_ERROR_39 => ResourceManager.GetString("SNI_ERROR_39", resourceCulture);

	internal static string SNI_ERROR_4 => ResourceManager.GetString("SNI_ERROR_4", resourceCulture);

	internal static string SNI_ERROR_40 => ResourceManager.GetString("SNI_ERROR_40", resourceCulture);

	internal static string SNI_ERROR_41 => ResourceManager.GetString("SNI_ERROR_41", resourceCulture);

	internal static string SNI_ERROR_42 => ResourceManager.GetString("SNI_ERROR_42", resourceCulture);

	internal static string SNI_ERROR_43 => ResourceManager.GetString("SNI_ERROR_43", resourceCulture);

	internal static string SNI_ERROR_44 => ResourceManager.GetString("SNI_ERROR_44", resourceCulture);

	internal static string SNI_ERROR_47 => ResourceManager.GetString("SNI_ERROR_47", resourceCulture);

	internal static string SNI_ERROR_48 => ResourceManager.GetString("SNI_ERROR_48", resourceCulture);

	internal static string SNI_ERROR_49 => ResourceManager.GetString("SNI_ERROR_49", resourceCulture);

	internal static string SNI_ERROR_5 => ResourceManager.GetString("SNI_ERROR_5", resourceCulture);

	internal static string SNI_ERROR_50 => ResourceManager.GetString("SNI_ERROR_50", resourceCulture);

	internal static string SNI_ERROR_51 => ResourceManager.GetString("SNI_ERROR_51", resourceCulture);

	internal static string SNI_ERROR_52 => ResourceManager.GetString("SNI_ERROR_52", resourceCulture);

	internal static string SNI_ERROR_53 => ResourceManager.GetString("SNI_ERROR_53", resourceCulture);

	internal static string SNI_ERROR_54 => ResourceManager.GetString("SNI_ERROR_54", resourceCulture);

	internal static string SNI_ERROR_55 => ResourceManager.GetString("SNI_ERROR_55", resourceCulture);

	internal static string SNI_ERROR_56 => ResourceManager.GetString("SNI_ERROR_56", resourceCulture);

	internal static string SNI_ERROR_57 => ResourceManager.GetString("SNI_ERROR_57", resourceCulture);

	internal static string SNI_ERROR_6 => ResourceManager.GetString("SNI_ERROR_6", resourceCulture);

	internal static string SNI_ERROR_7 => ResourceManager.GetString("SNI_ERROR_7", resourceCulture);

	internal static string SNI_ERROR_8 => ResourceManager.GetString("SNI_ERROR_8", resourceCulture);

	internal static string SNI_ERROR_9 => ResourceManager.GetString("SNI_ERROR_9", resourceCulture);

	internal static string SNI_PN0 => ResourceManager.GetString("SNI_PN0", resourceCulture);

	internal static string SNI_PN1 => ResourceManager.GetString("SNI_PN1", resourceCulture);

	internal static string SNI_PN10 => ResourceManager.GetString("SNI_PN10", resourceCulture);

	internal static string SNI_PN11 => ResourceManager.GetString("SNI_PN11", resourceCulture);

	internal static string SNI_PN2 => ResourceManager.GetString("SNI_PN2", resourceCulture);

	internal static string SNI_PN3 => ResourceManager.GetString("SNI_PN3", resourceCulture);

	internal static string SNI_PN4 => ResourceManager.GetString("SNI_PN4", resourceCulture);

	internal static string SNI_PN5 => ResourceManager.GetString("SNI_PN5", resourceCulture);

	internal static string SNI_PN6 => ResourceManager.GetString("SNI_PN6", resourceCulture);

	internal static string SNI_PN7 => ResourceManager.GetString("SNI_PN7", resourceCulture);

	internal static string SNI_PN8 => ResourceManager.GetString("SNI_PN8", resourceCulture);

	internal static string SNI_PN9 => ResourceManager.GetString("SNI_PN9", resourceCulture);

	internal static string Snix_AutoEnlist => ResourceManager.GetString("Snix_AutoEnlist", resourceCulture);

	internal static string Snix_Close => ResourceManager.GetString("Snix_Close", resourceCulture);

	internal static string Snix_Connect => ResourceManager.GetString("Snix_Connect", resourceCulture);

	internal static string Snix_EnableMars => ResourceManager.GetString("Snix_EnableMars", resourceCulture);

	internal static string Snix_Execute => ResourceManager.GetString("Snix_Execute", resourceCulture);

	internal static string Snix_GetMarsSession => ResourceManager.GetString("Snix_GetMarsSession", resourceCulture);

	internal static string Snix_Login => ResourceManager.GetString("Snix_Login", resourceCulture);

	internal static string Snix_LoginSspi => ResourceManager.GetString("Snix_LoginSspi", resourceCulture);

	internal static string Snix_PreLogin => ResourceManager.GetString("Snix_PreLogin", resourceCulture);

	internal static string Snix_PreLoginBeforeSuccessfullWrite => ResourceManager.GetString("Snix_PreLoginBeforeSuccessfullWrite", resourceCulture);

	internal static string Snix_ProcessSspi => ResourceManager.GetString("Snix_ProcessSspi", resourceCulture);

	internal static string Snix_Read => ResourceManager.GetString("Snix_Read", resourceCulture);

	internal static string Snix_SendRows => ResourceManager.GetString("Snix_SendRows", resourceCulture);

	internal static string SQL_ArgumentLengthMismatch => ResourceManager.GetString("SQL_ArgumentLengthMismatch", resourceCulture);

	internal static string SQL_AsyncConnectionRequired => ResourceManager.GetString("SQL_AsyncConnectionRequired", resourceCulture);

	internal static string SQL_AsyncOperationCompleted => ResourceManager.GetString("SQL_AsyncOperationCompleted", resourceCulture);

	internal static string SQL_AuthenticationAndIntegratedSecurity => ResourceManager.GetString("SQL_AuthenticationAndIntegratedSecurity", resourceCulture);

	internal static string SQL_BatchedUpdatesNotAvailableOnContextConnection => ResourceManager.GetString("SQL_BatchedUpdatesNotAvailableOnContextConnection", resourceCulture);

	internal static string SQL_BulkCopyDestinationTableName => ResourceManager.GetString("SQL_BulkCopyDestinationTableName", resourceCulture);

	internal static string SQL_BulkLoadCannotConvertValue => ResourceManager.GetString("SQL_BulkLoadCannotConvertValue", resourceCulture);

	internal static string SQL_BulkLoadCannotConvertValueWithoutRowNo => ResourceManager.GetString("SQL_BulkLoadCannotConvertValueWithoutRowNo", resourceCulture);

	internal static string SQL_BulkLoadConflictingTransactionOption => ResourceManager.GetString("SQL_BulkLoadConflictingTransactionOption", resourceCulture);

	internal static string SQL_BulkLoadExistingTransaction => ResourceManager.GetString("SQL_BulkLoadExistingTransaction", resourceCulture);

	internal static string SQL_BulkLoadInvalidDestinationTable => ResourceManager.GetString("SQL_BulkLoadInvalidDestinationTable", resourceCulture);

	internal static string SQL_BulkLoadInvalidOperationInsideEvent => ResourceManager.GetString("SQL_BulkLoadInvalidOperationInsideEvent", resourceCulture);

	internal static string SQL_BulkLoadInvalidOrderHint => ResourceManager.GetString("SQL_BulkLoadInvalidOrderHint", resourceCulture);

	internal static string SQL_BulkLoadInvalidTimeout => ResourceManager.GetString("SQL_BulkLoadInvalidTimeout", resourceCulture);

	internal static string SQL_BulkLoadInvalidVariantValue => ResourceManager.GetString("SQL_BulkLoadInvalidVariantValue", resourceCulture);

	internal static string Sql_BulkLoadLcidMismatch => ResourceManager.GetString("Sql_BulkLoadLcidMismatch", resourceCulture);

	internal static string SQL_BulkLoadMappingInaccessible => ResourceManager.GetString("SQL_BulkLoadMappingInaccessible", resourceCulture);

	internal static string SQL_BulkLoadMappingsNamesOrOrdinalsOnly => ResourceManager.GetString("SQL_BulkLoadMappingsNamesOrOrdinalsOnly", resourceCulture);

	internal static string SQL_BulkLoadMissingDestinationTable => ResourceManager.GetString("SQL_BulkLoadMissingDestinationTable", resourceCulture);

	internal static string SQL_BulkLoadNoCollation => ResourceManager.GetString("SQL_BulkLoadNoCollation", resourceCulture);

	internal static string SQL_BulkLoadNonMatchingColumnMapping => ResourceManager.GetString("SQL_BulkLoadNonMatchingColumnMapping", resourceCulture);

	internal static string SQL_BulkLoadNonMatchingColumnName => ResourceManager.GetString("SQL_BulkLoadNonMatchingColumnName", resourceCulture);

	internal static string SQL_BulkLoadNotAllowDBNull => ResourceManager.GetString("SQL_BulkLoadNotAllowDBNull", resourceCulture);

	internal static string SQL_BulkLoadOrderHintDuplicateColumn => ResourceManager.GetString("SQL_BulkLoadOrderHintDuplicateColumn", resourceCulture);

	internal static string SQL_BulkLoadOrderHintInvalidColumn => ResourceManager.GetString("SQL_BulkLoadOrderHintInvalidColumn", resourceCulture);

	internal static string SQL_BulkLoadPendingOperation => ResourceManager.GetString("SQL_BulkLoadPendingOperation", resourceCulture);

	internal static string SQL_BulkLoadStringTooLong => ResourceManager.GetString("SQL_BulkLoadStringTooLong", resourceCulture);

	internal static string SQL_BulkLoadUnspecifiedSortOrder => ResourceManager.GetString("SQL_BulkLoadUnspecifiedSortOrder", resourceCulture);

	internal static string SQL_CannotCreateAuthInitializer => ResourceManager.GetString("SQL_CannotCreateAuthInitializer", resourceCulture);

	internal static string SQL_CannotCreateAuthProvider => ResourceManager.GetString("SQL_CannotCreateAuthProvider", resourceCulture);

	internal static string SQL_CannotFindAuthProvider => ResourceManager.GetString("SQL_CannotFindAuthProvider", resourceCulture);

	internal static string SQL_CannotGetAuthProviderConfig => ResourceManager.GetString("SQL_CannotGetAuthProviderConfig", resourceCulture);

	internal static string SQL_CannotGetDTCAddress => ResourceManager.GetString("SQL_CannotGetDTCAddress", resourceCulture);

	internal static string SQL_CannotInitializeAuthProvider => ResourceManager.GetString("SQL_CannotInitializeAuthProvider", resourceCulture);

	internal static string SQL_CannotModifyPropertyAsyncOperationInProgress => ResourceManager.GetString("SQL_CannotModifyPropertyAsyncOperationInProgress", resourceCulture);

	internal static string Sql_CanotCreateNormalizer => ResourceManager.GetString("Sql_CanotCreateNormalizer", resourceCulture);

	internal static string SQL_Certificate => ResourceManager.GetString("SQL_Certificate", resourceCulture);

	internal static string SQL_ChangePasswordArgumentMissing => ResourceManager.GetString("SQL_ChangePasswordArgumentMissing", resourceCulture);

	internal static string SQL_ChangePasswordConflictsWithSSPI => ResourceManager.GetString("SQL_ChangePasswordConflictsWithSSPI", resourceCulture);

	internal static string SQL_ChangePasswordRequiresYukon => ResourceManager.GetString("SQL_ChangePasswordRequiresYukon", resourceCulture);

	internal static string SQL_ChangePasswordUseOfUnallowedKey => ResourceManager.GetString("SQL_ChangePasswordUseOfUnallowedKey", resourceCulture);

	internal static string SQL_ConnectionDoomed => ResourceManager.GetString("SQL_ConnectionDoomed", resourceCulture);

	internal static string SQL_ConnectionLockedForBcpEvent => ResourceManager.GetString("SQL_ConnectionLockedForBcpEvent", resourceCulture);

	internal static string SQL_ContextAllowsLimitedKeywords => ResourceManager.GetString("SQL_ContextAllowsLimitedKeywords", resourceCulture);

	internal static string SQL_ContextAllowsOnlyTypeSystem2005 => ResourceManager.GetString("SQL_ContextAllowsOnlyTypeSystem2005", resourceCulture);

	internal static string SQL_ContextConnectionIsInUse => ResourceManager.GetString("SQL_ContextConnectionIsInUse", resourceCulture);

	internal static string SQL_ContextUnavailableOutOfProc => ResourceManager.GetString("SQL_ContextUnavailableOutOfProc", resourceCulture);

	internal static string SQL_ContextUnavailableWhileInProc => ResourceManager.GetString("SQL_ContextUnavailableWhileInProc", resourceCulture);

	internal static string SQL_CredentialsNotProvided => ResourceManager.GetString("SQL_CredentialsNotProvided", resourceCulture);

	internal static string SQL_CTAIPNotSupportedByServer => ResourceManager.GetString("SQL_CTAIPNotSupportedByServer", resourceCulture);

	internal static string SQL_CultureIdError => ResourceManager.GetString("SQL_CultureIdError", resourceCulture);

	internal static string SQL_DeviceFlowWithUsernamePassword => ResourceManager.GetString("SQL_DeviceFlowWithUsernamePassword", resourceCulture);

	internal static string SQL_Duration_Login_Begin => ResourceManager.GetString("SQL_Duration_Login_Begin", resourceCulture);

	internal static string SQL_Duration_Login_ProcessConnectionAuth => ResourceManager.GetString("SQL_Duration_Login_ProcessConnectionAuth", resourceCulture);

	internal static string SQL_Duration_PostLogin => ResourceManager.GetString("SQL_Duration_PostLogin", resourceCulture);

	internal static string SQL_Duration_PreLogin_Begin => ResourceManager.GetString("SQL_Duration_PreLogin_Begin", resourceCulture);

	internal static string SQL_Duration_PreLoginHandshake => ResourceManager.GetString("SQL_Duration_PreLoginHandshake", resourceCulture);

	internal static string SQL_EncryptionNotSupportedByClient => ResourceManager.GetString("SQL_EncryptionNotSupportedByClient", resourceCulture);

	internal static string SQL_EncryptionNotSupportedByServer => ResourceManager.GetString("SQL_EncryptionNotSupportedByServer", resourceCulture);

	internal static string SQL_EnumeratedRecordFieldCountChanged => ResourceManager.GetString("SQL_EnumeratedRecordFieldCountChanged", resourceCulture);

	internal static string SQL_EnumeratedRecordMetaDataChanged => ResourceManager.GetString("SQL_EnumeratedRecordMetaDataChanged", resourceCulture);

	internal static string SQL_ExceedsMaxDataLength => ResourceManager.GetString("SQL_ExceedsMaxDataLength", resourceCulture);

	internal static string SQL_ExClientConnectionId => ResourceManager.GetString("SQL_ExClientConnectionId", resourceCulture);

	internal static string SQL_ExErrorNumberStateClass => ResourceManager.GetString("SQL_ExErrorNumberStateClass", resourceCulture);

	internal static string SQL_ExOriginalClientConnectionId => ResourceManager.GetString("SQL_ExOriginalClientConnectionId", resourceCulture);

	internal static string SQL_ExRoutingDestination => ResourceManager.GetString("SQL_ExRoutingDestination", resourceCulture);

	internal static string SQL_FatalTimeout => ResourceManager.GetString("SQL_FatalTimeout", resourceCulture);

	internal static string SQL_InstanceFailure => ResourceManager.GetString("SQL_InstanceFailure", resourceCulture);

	internal static string SQL_IntegratedWithUserIDAndPassword => ResourceManager.GetString("SQL_IntegratedWithUserIDAndPassword", resourceCulture);

	internal static string SQL_InteractiveWithPassword => ResourceManager.GetString("SQL_InteractiveWithPassword", resourceCulture);

	internal static string Sql_InternalError => ResourceManager.GetString("Sql_InternalError", resourceCulture);

	internal static string SQL_InvalidBufferSizeOrIndex => ResourceManager.GetString("SQL_InvalidBufferSizeOrIndex", resourceCulture);

	internal static string SQL_InvalidDataLength => ResourceManager.GetString("SQL_InvalidDataLength", resourceCulture);

	internal static string SQL_InvalidInternalPacketSize => ResourceManager.GetString("SQL_InvalidInternalPacketSize", resourceCulture);

	internal static string SQL_InvalidOptionLength => ResourceManager.GetString("SQL_InvalidOptionLength", resourceCulture);

	internal static string SQL_InvalidPacketSizeValue => ResourceManager.GetString("SQL_InvalidPacketSizeValue", resourceCulture);

	internal static string SQL_InvalidParameterNameLength => ResourceManager.GetString("SQL_InvalidParameterNameLength", resourceCulture);

	internal static string SQL_InvalidParameterTypeNameFormat => ResourceManager.GetString("SQL_InvalidParameterTypeNameFormat", resourceCulture);

	internal static string SQL_InvalidPartnerConfiguration => ResourceManager.GetString("SQL_InvalidPartnerConfiguration", resourceCulture);

	internal static string SQL_InvalidRead => ResourceManager.GetString("SQL_InvalidRead", resourceCulture);

	internal static string SQL_InvalidServerCertificate => ResourceManager.GetString("SQL_InvalidServerCertificate", resourceCulture);

	internal static string SQL_InvalidSqlDbTypeWithOneAllowedType => ResourceManager.GetString("SQL_InvalidSqlDbTypeWithOneAllowedType", resourceCulture);

	internal static string SQL_InvalidSQLServerVersionUnknown => ResourceManager.GetString("SQL_InvalidSQLServerVersionUnknown", resourceCulture);

	internal static string SQL_InvalidSSPIPacketSize => ResourceManager.GetString("SQL_InvalidSSPIPacketSize", resourceCulture);

	internal static string SQL_InvalidTDSPacketSize => ResourceManager.GetString("SQL_InvalidTDSPacketSize", resourceCulture);

	internal static string SQL_InvalidTDSVersion => ResourceManager.GetString("SQL_InvalidTDSVersion", resourceCulture);

	internal static string SQL_InvalidUdt3PartNameFormat => ResourceManager.GetString("SQL_InvalidUdt3PartNameFormat", resourceCulture);

	internal static string SQL_NonInteractiveWithPassword => ResourceManager.GetString("SQL_NonInteractiveWithPassword", resourceCulture);

	internal static string SQL_MarsUnsupportedOnConnection => ResourceManager.GetString("SQL_MarsUnsupportedOnConnection", resourceCulture);

	internal static string Sql_MismatchedMetaDataDirectionArrayLengths => ResourceManager.GetString("Sql_MismatchedMetaDataDirectionArrayLengths", resourceCulture);

	internal static string SQL_MoneyOverflow => ResourceManager.GetString("SQL_MoneyOverflow", resourceCulture);

	internal static string SQL_MSALFailure => ResourceManager.GetString("SQL_MSALFailure", resourceCulture);

	internal static string SQL_MSALInnerException => ResourceManager.GetString("SQL_MSALInnerException", resourceCulture);

	internal static string SQL_NestedTransactionScopesNotSupported => ResourceManager.GetString("SQL_NestedTransactionScopesNotSupported", resourceCulture);

	internal static string SQL_NonBlobColumn => ResourceManager.GetString("SQL_NonBlobColumn", resourceCulture);

	internal static string SQL_NonCharColumn => ResourceManager.GetString("SQL_NonCharColumn", resourceCulture);

	internal static string SQL_NonLocalSSEInstance => ResourceManager.GetString("SQL_NonLocalSSEInstance", resourceCulture);

	internal static string SQL_NonXmlResult => ResourceManager.GetString("SQL_NonXmlResult", resourceCulture);

	internal static string SQL_NotAvailableOnContextConnection => ResourceManager.GetString("SQL_NotAvailableOnContextConnection", resourceCulture);

	internal static string SQL_NotificationsNotAvailableOnContextConnection => ResourceManager.GetString("SQL_NotificationsNotAvailableOnContextConnection", resourceCulture);

	internal static string SQL_NotificationsRequireYukon => ResourceManager.GetString("SQL_NotificationsRequireYukon", resourceCulture);

	internal static string SQL_NotSupportedEnumerationValue => ResourceManager.GetString("SQL_NotSupportedEnumerationValue", resourceCulture);

	internal static string Sql_NullCommandText => ResourceManager.GetString("Sql_NullCommandText", resourceCulture);

	internal static string SQL_NullEmptyTransactionName => ResourceManager.GetString("SQL_NullEmptyTransactionName", resourceCulture);

	internal static string SQL_OpenResultCountExceeded => ResourceManager.GetString("SQL_OpenResultCountExceeded", resourceCulture);

	internal static string SQL_OperationCancelled => ResourceManager.GetString("SQL_OperationCancelled", resourceCulture);

	internal static string SQL_ParameterCannotBeEmpty => ResourceManager.GetString("SQL_ParameterCannotBeEmpty", resourceCulture);

	internal static string SQL_ParameterInvalidVariant => ResourceManager.GetString("SQL_ParameterInvalidVariant", resourceCulture);

	internal static string SQL_ParameterTypeNameRequired => ResourceManager.GetString("SQL_ParameterTypeNameRequired", resourceCulture);

	internal static string SQL_ParsingError => ResourceManager.GetString("SQL_ParsingError", resourceCulture);

	internal static string SQL_ParsingErrorAuthLibraryType => ResourceManager.GetString("SQL_ParsingErrorAuthLibraryType", resourceCulture);

	internal static string SQL_ParsingErrorFeatureId => ResourceManager.GetString("SQL_ParsingErrorFeatureId", resourceCulture);

	internal static string SQL_ParsingErrorLength => ResourceManager.GetString("SQL_ParsingErrorLength", resourceCulture);

	internal static string SQL_ParsingErrorOffset => ResourceManager.GetString("SQL_ParsingErrorOffset", resourceCulture);

	internal static string SQL_ParsingErrorStatus => ResourceManager.GetString("SQL_ParsingErrorStatus", resourceCulture);

	internal static string SQL_ParsingErrorToken => ResourceManager.GetString("SQL_ParsingErrorToken", resourceCulture);

	internal static string SQL_ParsingErrorValue => ResourceManager.GetString("SQL_ParsingErrorValue", resourceCulture);

	internal static string SQL_ParsingErrorWithState => ResourceManager.GetString("SQL_ParsingErrorWithState", resourceCulture);

	internal static string SQL_PendingBeginXXXExists => ResourceManager.GetString("SQL_PendingBeginXXXExists", resourceCulture);

	internal static string SQL_PipeErrorRequiresSendEnd => ResourceManager.GetString("SQL_PipeErrorRequiresSendEnd", resourceCulture);

	internal static string SQL_PrecisionValueOutOfRange => ResourceManager.GetString("SQL_PrecisionValueOutOfRange", resourceCulture);

	internal static string SQL_ScaleValueOutOfRange => ResourceManager.GetString("SQL_ScaleValueOutOfRange", resourceCulture);

	internal static string SQL_SettingCredentialWithDeviceFlow => ResourceManager.GetString("SQL_SettingCredentialWithDeviceFlow", resourceCulture);

	internal static string SQL_SettingCredentialWithIntegrated => ResourceManager.GetString("SQL_SettingCredentialWithIntegrated", resourceCulture);

	internal static string SQL_SettingCredentialWithInteractive => ResourceManager.GetString("SQL_SettingCredentialWithInteractive", resourceCulture);

	internal static string SQL_SettingCredentialWithNonInteractive => ResourceManager.GetString("SQL_SettingCredentialWithNonInteractive", resourceCulture);

	internal static string SQL_SettingDeviceFlowWithCredential => ResourceManager.GetString("SQL_SettingDeviceFlowWithCredential", resourceCulture);

	internal static string SQL_SettingIntegratedWithCredential => ResourceManager.GetString("SQL_SettingIntegratedWithCredential", resourceCulture);

	internal static string SQL_SettingInteractiveWithCredential => ResourceManager.GetString("SQL_SettingInteractiveWithCredential", resourceCulture);

	internal static string SQL_SettingNonInteractiveWithCredential => ResourceManager.GetString("SQL_SettingNonInteractiveWithCredential", resourceCulture);

	internal static string SQL_SevereError => ResourceManager.GetString("SQL_SevereError", resourceCulture);

	internal static string SQL_SmallDateTimeOverflow => ResourceManager.GetString("SQL_SmallDateTimeOverflow", resourceCulture);

	internal static string SQL_SnapshotNotSupported => ResourceManager.GetString("SQL_SnapshotNotSupported", resourceCulture);

	internal static string SQL_SNIPacketAllocationFailure => ResourceManager.GetString("SQL_SNIPacketAllocationFailure", resourceCulture);

	internal static string SQL_SqlCommandCommandText => ResourceManager.GetString("SQL_SqlCommandCommandText", resourceCulture);

	internal static string SQL_SqlRecordReadOnly => ResourceManager.GetString("SQL_SqlRecordReadOnly", resourceCulture);

	internal static string SQL_SqlRecordReadOnly2 => ResourceManager.GetString("SQL_SqlRecordReadOnly2", resourceCulture);

	internal static string SQL_SqlResultSetClosed => ResourceManager.GetString("SQL_SqlResultSetClosed", resourceCulture);

	internal static string SQL_SqlResultSetClosed2 => ResourceManager.GetString("SQL_SqlResultSetClosed2", resourceCulture);

	internal static string SQL_SqlResultSetCommandNotInSameConnection => ResourceManager.GetString("SQL_SqlResultSetCommandNotInSameConnection", resourceCulture);

	internal static string SQL_SqlResultSetNoAcceptableCursor => ResourceManager.GetString("SQL_SqlResultSetNoAcceptableCursor", resourceCulture);

	internal static string SQL_SqlResultSetRowDeleted => ResourceManager.GetString("SQL_SqlResultSetRowDeleted", resourceCulture);

	internal static string SQL_SqlResultSetRowDeleted2 => ResourceManager.GetString("SQL_SqlResultSetRowDeleted2", resourceCulture);

	internal static string SQL_SqlUpdatableRecordReadOnly => ResourceManager.GetString("SQL_SqlUpdatableRecordReadOnly", resourceCulture);

	internal static string SQL_SSPIGenerateError => ResourceManager.GetString("SQL_SSPIGenerateError", resourceCulture);

	internal static string SQL_SSPIInitializeError => ResourceManager.GetString("SQL_SSPIInitializeError", resourceCulture);

	internal static string SQL_StreamNotSupportOnColumnType => ResourceManager.GetString("SQL_StreamNotSupportOnColumnType", resourceCulture);

	internal static string SQL_StreamReadNotSupported => ResourceManager.GetString("SQL_StreamReadNotSupported", resourceCulture);

	internal static string SQL_StreamSeekNotSupported => ResourceManager.GetString("SQL_StreamSeekNotSupported", resourceCulture);

	internal static string SQL_StreamWriteNotSupported => ResourceManager.GetString("SQL_StreamWriteNotSupported", resourceCulture);

	internal static string SQL_TDSParserTableName => ResourceManager.GetString("SQL_TDSParserTableName", resourceCulture);

	internal static string SQL_TextReaderNotSupportOnColumnType => ResourceManager.GetString("SQL_TextReaderNotSupportOnColumnType", resourceCulture);

	internal static string SQL_Timeout => ResourceManager.GetString("SQL_Timeout", resourceCulture);

	internal static string SQL_Timeout_Active_Directory_DeviceFlow_Authentication => ResourceManager.GetString("SQL_Timeout_Active_Directory_DeviceFlow_Authentication", resourceCulture);

	internal static string SQL_Timeout_Active_Directory_Interactive_Authentication => ResourceManager.GetString("SQL_Timeout_Active_Directory_Interactive_Authentication", resourceCulture);

	internal static string SQL_Timeout_Execution => ResourceManager.GetString("SQL_Timeout_Execution", resourceCulture);

	internal static string SQL_Timeout_FailoverInfo => ResourceManager.GetString("SQL_Timeout_FailoverInfo", resourceCulture);

	internal static string SQL_Timeout_Login_Begin => ResourceManager.GetString("SQL_Timeout_Login_Begin", resourceCulture);

	internal static string SQL_Timeout_Login_ProcessConnectionAuth => ResourceManager.GetString("SQL_Timeout_Login_ProcessConnectionAuth", resourceCulture);

	internal static string SQL_Timeout_PostLogin => ResourceManager.GetString("SQL_Timeout_PostLogin", resourceCulture);

	internal static string SQL_Timeout_PreLogin_Begin => ResourceManager.GetString("SQL_Timeout_PreLogin_Begin", resourceCulture);

	internal static string SQL_Timeout_PreLogin_ConsumeHandshake => ResourceManager.GetString("SQL_Timeout_PreLogin_ConsumeHandshake", resourceCulture);

	internal static string SQL_Timeout_PreLogin_InitializeConnection => ResourceManager.GetString("SQL_Timeout_PreLogin_InitializeConnection", resourceCulture);

	internal static string SQL_Timeout_PreLogin_SendHandshake => ResourceManager.GetString("SQL_Timeout_PreLogin_SendHandshake", resourceCulture);

	internal static string SQL_Timeout_RoutingDestinationInfo => ResourceManager.GetString("SQL_Timeout_RoutingDestinationInfo", resourceCulture);

	internal static string SQL_TimeOverflow => ResourceManager.GetString("SQL_TimeOverflow", resourceCulture);

	internal static string SQL_TimeScaleValueOutOfRange => ResourceManager.GetString("SQL_TimeScaleValueOutOfRange", resourceCulture);

	internal static string SQL_TooManyValues => ResourceManager.GetString("SQL_TooManyValues", resourceCulture);

	internal static string SQL_TypeName => ResourceManager.GetString("SQL_TypeName", resourceCulture);

	internal static string SQL_UDTTypeName => ResourceManager.GetString("SQL_UDTTypeName", resourceCulture);

	internal static string SQL_UnexpectedSmiEvent => ResourceManager.GetString("SQL_UnexpectedSmiEvent", resourceCulture);

	internal static string SQL_UnknownSysTxIsolationLevel => ResourceManager.GetString("SQL_UnknownSysTxIsolationLevel", resourceCulture);

	internal static string SQL_UnsupportedAuthentication => ResourceManager.GetString("SQL_UnsupportedAuthentication", resourceCulture);

	internal static string SQL_UnsupportedAuthenticationByProvider => ResourceManager.GetString("SQL_UnsupportedAuthenticationByProvider", resourceCulture);

	internal static string SQL_UnsupportedAuthenticationSpecified => ResourceManager.GetString("SQL_UnsupportedAuthenticationSpecified", resourceCulture);

	internal static string SQL_UnsupportedSqlAuthenticationMethod => ResourceManager.GetString("SQL_UnsupportedSqlAuthenticationMethod", resourceCulture);

	internal static string SQL_UserInstanceFailoverNotCompatible => ResourceManager.GetString("SQL_UserInstanceFailoverNotCompatible", resourceCulture);

	internal static string SQL_UserInstanceFailure => ResourceManager.GetString("SQL_UserInstanceFailure", resourceCulture);

	internal static string SQL_UserInstanceNotAvailableInProc => ResourceManager.GetString("SQL_UserInstanceNotAvailableInProc", resourceCulture);

	internal static string SQL_WrongType => ResourceManager.GetString("SQL_WrongType", resourceCulture);

	internal static string SQL_XmlReaderNotSupportOnColumnType => ResourceManager.GetString("SQL_XmlReaderNotSupportOnColumnType", resourceCulture);

	internal static string SqlCommand_Notification => ResourceManager.GetString("SqlCommand_Notification", resourceCulture);

	internal static string SqlCommand_NotificationAutoEnlist => ResourceManager.GetString("SqlCommand_NotificationAutoEnlist", resourceCulture);

	internal static string SqlCommandBuilder_DataAdapter => ResourceManager.GetString("SqlCommandBuilder_DataAdapter", resourceCulture);

	internal static string SqlCommandBuilder_DecimalSeparator => ResourceManager.GetString("SqlCommandBuilder_DecimalSeparator", resourceCulture);

	internal static string SqlCommandBuilder_QuotePrefix => ResourceManager.GetString("SqlCommandBuilder_QuotePrefix", resourceCulture);

	internal static string SqlCommandBuilder_QuoteSuffix => ResourceManager.GetString("SqlCommandBuilder_QuoteSuffix", resourceCulture);

	internal static string SqlConnection_AccessToken => ResourceManager.GetString("SqlConnection_AccessToken", resourceCulture);

	internal static string SqlConnection_Asynchronous => ResourceManager.GetString("SqlConnection_Asynchronous", resourceCulture);

	internal static string SqlConnection_ClientConnectionId => ResourceManager.GetString("SqlConnection_ClientConnectionId", resourceCulture);

	internal static string SqlConnection_ConnectionString => ResourceManager.GetString("SqlConnection_ConnectionString", resourceCulture);

	internal static string SqlConnection_ConnectionTimeout => ResourceManager.GetString("SqlConnection_ConnectionTimeout", resourceCulture);

	internal static string SqlConnection_Credential => ResourceManager.GetString("SqlConnection_Credential", resourceCulture);

	internal static string SqlConnection_CustomColumnEncryptionKeyStoreProviders => ResourceManager.GetString("SqlConnection_CustomColumnEncryptionKeyStoreProviders", resourceCulture);

	internal static string SqlConnection_Database => ResourceManager.GetString("SqlConnection_Database", resourceCulture);

	internal static string SqlConnection_DataSource => ResourceManager.GetString("SqlConnection_DataSource", resourceCulture);

	internal static string SqlConnection_PacketSize => ResourceManager.GetString("SqlConnection_PacketSize", resourceCulture);

	internal static string SqlConnection_Replication => ResourceManager.GetString("SqlConnection_Replication", resourceCulture);

	internal static string SqlConnection_ServerProcessId => ResourceManager.GetString("SqlConnection_ServerProcessId", resourceCulture);

	internal static string SqlConnection_ServerVersion => ResourceManager.GetString("SqlConnection_ServerVersion", resourceCulture);

	internal static string SqlConnection_StatisticsEnabled => ResourceManager.GetString("SqlConnection_StatisticsEnabled", resourceCulture);

	internal static string SqlConnection_WorkstationId => ResourceManager.GetString("SqlConnection_WorkstationId", resourceCulture);

	internal static string SqlConvert_ConvertFailed => ResourceManager.GetString("SqlConvert_ConvertFailed", resourceCulture);

	internal static string SQLCR_AllAttemptsFailed => ResourceManager.GetString("SQLCR_AllAttemptsFailed", resourceCulture);

	internal static string SQLCR_EncryptionChanged => ResourceManager.GetString("SQLCR_EncryptionChanged", resourceCulture);

	internal static string SQLCR_InvalidConnectRetryCountValue => ResourceManager.GetString("SQLCR_InvalidConnectRetryCountValue", resourceCulture);

	internal static string SQLCR_InvalidConnectRetryIntervalValue => ResourceManager.GetString("SQLCR_InvalidConnectRetryIntervalValue", resourceCulture);

	internal static string SQLCR_NextAttemptWillExceedQueryTimeout => ResourceManager.GetString("SQLCR_NextAttemptWillExceedQueryTimeout", resourceCulture);

	internal static string SQLCR_NoCRAckAtReconnection => ResourceManager.GetString("SQLCR_NoCRAckAtReconnection", resourceCulture);

	internal static string SQLCR_TDSVestionNotPreserved => ResourceManager.GetString("SQLCR_TDSVestionNotPreserved", resourceCulture);

	internal static string SQLCR_UnrecoverableClient => ResourceManager.GetString("SQLCR_UnrecoverableClient", resourceCulture);

	internal static string SQLCR_UnrecoverableServer => ResourceManager.GetString("SQLCR_UnrecoverableServer", resourceCulture);

	internal static string SqlDelegatedTransaction_PromotionFailed => ResourceManager.GetString("SqlDelegatedTransaction_PromotionFailed", resourceCulture);

	internal static string SqlDependency_AddCommandDependency => ResourceManager.GetString("SqlDependency_AddCommandDependency", resourceCulture);

	internal static string SqlDependency_DatabaseBrokerDisabled => ResourceManager.GetString("SqlDependency_DatabaseBrokerDisabled", resourceCulture);

	internal static string SqlDependency_DefaultOptionsButNoStart => ResourceManager.GetString("SqlDependency_DefaultOptionsButNoStart", resourceCulture);

	internal static string SqlDependency_Duplicate => ResourceManager.GetString("SqlDependency_Duplicate", resourceCulture);

	internal static string SqlDependency_DuplicateStart => ResourceManager.GetString("SqlDependency_DuplicateStart", resourceCulture);

	internal static string SqlDependency_EventNoDuplicate => ResourceManager.GetString("SqlDependency_EventNoDuplicate", resourceCulture);

	internal static string SqlDependency_HasChanges => ResourceManager.GetString("SqlDependency_HasChanges", resourceCulture);

	internal static string SqlDependency_Id => ResourceManager.GetString("SqlDependency_Id", resourceCulture);

	internal static string SqlDependency_IdMismatch => ResourceManager.GetString("SqlDependency_IdMismatch", resourceCulture);

	internal static string SqlDependency_InvalidTimeout => ResourceManager.GetString("SqlDependency_InvalidTimeout", resourceCulture);

	internal static string SqlDependency_NoMatchingServerDatabaseStart => ResourceManager.GetString("SqlDependency_NoMatchingServerDatabaseStart", resourceCulture);

	internal static string SqlDependency_NoMatchingServerStart => ResourceManager.GetString("SqlDependency_NoMatchingServerStart", resourceCulture);

	internal static string SqlDependency_OnChange => ResourceManager.GetString("SqlDependency_OnChange", resourceCulture);

	internal static string SqlDependency_SqlDependency => ResourceManager.GetString("SqlDependency_SqlDependency", resourceCulture);

	internal static string SqlDependency_UnexpectedValueOnDeserialize => ResourceManager.GetString("SqlDependency_UnexpectedValueOnDeserialize", resourceCulture);

	internal static string SqlFileStream_FileAlreadyInTransaction => ResourceManager.GetString("SqlFileStream_FileAlreadyInTransaction", resourceCulture);

	internal static string SqlFileStream_InvalidParameter => ResourceManager.GetString("SqlFileStream_InvalidParameter", resourceCulture);

	internal static string SqlFileStream_InvalidPath => ResourceManager.GetString("SqlFileStream_InvalidPath", resourceCulture);

	internal static string SqlFileStream_PathNotValidDiskResource => ResourceManager.GetString("SqlFileStream_PathNotValidDiskResource", resourceCulture);

	internal static string SqlMetaData_InvalidSqlDbTypeForConstructorFormat => ResourceManager.GetString("SqlMetaData_InvalidSqlDbTypeForConstructorFormat", resourceCulture);

	internal static string SqlMetaData_NameTooLong => ResourceManager.GetString("SqlMetaData_NameTooLong", resourceCulture);

	internal static string SqlMetaData_NoMetadata => ResourceManager.GetString("SqlMetaData_NoMetadata", resourceCulture);

	internal static string SqlMetaData_SpecifyBothSortOrderAndOrdinal => ResourceManager.GetString("SqlMetaData_SpecifyBothSortOrderAndOrdinal", resourceCulture);

	internal static string SqlMisc_AlreadyFilledMessage => ResourceManager.GetString("SqlMisc_AlreadyFilledMessage", resourceCulture);

	internal static string SqlMisc_ArithOverflowMessage => ResourceManager.GetString("SqlMisc_ArithOverflowMessage", resourceCulture);

	internal static string SqlMisc_BufferInsufficientMessage => ResourceManager.GetString("SqlMisc_BufferInsufficientMessage", resourceCulture);

	internal static string SqlMisc_ClosedXmlReaderMessage => ResourceManager.GetString("SqlMisc_ClosedXmlReaderMessage", resourceCulture);

	internal static string SqlMisc_CompareDiffCollationMessage => ResourceManager.GetString("SqlMisc_CompareDiffCollationMessage", resourceCulture);

	internal static string SqlMisc_ConcatDiffCollationMessage => ResourceManager.GetString("SqlMisc_ConcatDiffCollationMessage", resourceCulture);

	internal static string SqlMisc_ConversionOverflowMessage => ResourceManager.GetString("SqlMisc_ConversionOverflowMessage", resourceCulture);

	internal static string SqlMisc_DateTimeOverflowMessage => ResourceManager.GetString("SqlMisc_DateTimeOverflowMessage", resourceCulture);

	internal static string SqlMisc_DivideByZeroMessage => ResourceManager.GetString("SqlMisc_DivideByZeroMessage", resourceCulture);

	internal static string SqlMisc_FormatMessage => ResourceManager.GetString("SqlMisc_FormatMessage", resourceCulture);

	internal static string SqlMisc_InvalidArraySizeMessage => ResourceManager.GetString("SqlMisc_InvalidArraySizeMessage", resourceCulture);

	internal static string SqlMisc_InvalidDateTimeMessage => ResourceManager.GetString("SqlMisc_InvalidDateTimeMessage", resourceCulture);

	internal static string SqlMisc_InvalidFirstDayMessage => ResourceManager.GetString("SqlMisc_InvalidFirstDayMessage", resourceCulture);

	internal static string SqlMisc_InvalidFlagMessage => ResourceManager.GetString("SqlMisc_InvalidFlagMessage", resourceCulture);

	internal static string SqlMisc_InvalidOpStreamClosed => ResourceManager.GetString("SqlMisc_InvalidOpStreamClosed", resourceCulture);

	internal static string SqlMisc_InvalidOpStreamNonReadable => ResourceManager.GetString("SqlMisc_InvalidOpStreamNonReadable", resourceCulture);

	internal static string SqlMisc_InvalidOpStreamNonSeekable => ResourceManager.GetString("SqlMisc_InvalidOpStreamNonSeekable", resourceCulture);

	internal static string SqlMisc_InvalidOpStreamNonWritable => ResourceManager.GetString("SqlMisc_InvalidOpStreamNonWritable", resourceCulture);

	internal static string SqlMisc_InvalidPrecScaleMessage => ResourceManager.GetString("SqlMisc_InvalidPrecScaleMessage", resourceCulture);

	internal static string SqlMisc_LenTooLargeMessage => ResourceManager.GetString("SqlMisc_LenTooLargeMessage", resourceCulture);

	internal static string SqlMisc_MessageString => ResourceManager.GetString("SqlMisc_MessageString", resourceCulture);

	internal static string SqlMisc_NoBufferMessage => ResourceManager.GetString("SqlMisc_NoBufferMessage", resourceCulture);

	internal static string SqlMisc_NotFilledMessage => ResourceManager.GetString("SqlMisc_NotFilledMessage", resourceCulture);

	internal static string SqlMisc_NullString => ResourceManager.GetString("SqlMisc_NullString", resourceCulture);

	internal static string SqlMisc_NullValueMessage => ResourceManager.GetString("SqlMisc_NullValueMessage", resourceCulture);

	internal static string SqlMisc_NumeToDecOverflowMessage => ResourceManager.GetString("SqlMisc_NumeToDecOverflowMessage", resourceCulture);

	internal static string SqlMisc_SetNonZeroLenOnNullMessage => ResourceManager.GetString("SqlMisc_SetNonZeroLenOnNullMessage", resourceCulture);

	internal static string SqlMisc_SqlTypeMessage => ResourceManager.GetString("SqlMisc_SqlTypeMessage", resourceCulture);

	internal static string SqlMisc_StreamClosedMessage => ResourceManager.GetString("SqlMisc_StreamClosedMessage", resourceCulture);

	internal static string SqlMisc_StreamErrorMessage => ResourceManager.GetString("SqlMisc_StreamErrorMessage", resourceCulture);

	internal static string SqlMisc_SubclassMustOverride => ResourceManager.GetString("SqlMisc_SubclassMustOverride", resourceCulture);

	internal static string SqlMisc_TimeZoneSpecifiedMessage => ResourceManager.GetString("SqlMisc_TimeZoneSpecifiedMessage", resourceCulture);

	internal static string SqlMisc_TruncationMaxDataMessage => ResourceManager.GetString("SqlMisc_TruncationMaxDataMessage", resourceCulture);

	internal static string SqlMisc_TruncationMessage => ResourceManager.GetString("SqlMisc_TruncationMessage", resourceCulture);

	internal static string SqlMisc_WriteNonZeroOffsetOnNullMessage => ResourceManager.GetString("SqlMisc_WriteNonZeroOffsetOnNullMessage", resourceCulture);

	internal static string SqlMisc_WriteOffsetLargerThanLenMessage => ResourceManager.GetString("SqlMisc_WriteOffsetLargerThanLenMessage", resourceCulture);

	internal static string SQLMSF_FailoverPartnerNotSupported => ResourceManager.GetString("SQLMSF_FailoverPartnerNotSupported", resourceCulture);

	internal static string SQLNotify_AlreadyHasCommand => ResourceManager.GetString("SQLNotify_AlreadyHasCommand", resourceCulture);

	internal static string SQLNotify_ErrorFormat => ResourceManager.GetString("SQLNotify_ErrorFormat", resourceCulture);

	internal static string SqlNotify_SqlDepCannotBeCreatedInProc => ResourceManager.GetString("SqlNotify_SqlDepCannotBeCreatedInProc", resourceCulture);

	internal static string SqlParameter_DBNullNotSupportedForTVP => ResourceManager.GetString("SqlParameter_DBNullNotSupportedForTVP", resourceCulture);

	internal static string SqlParameter_InvalidTableDerivedPrecisionForTvp => ResourceManager.GetString("SqlParameter_InvalidTableDerivedPrecisionForTvp", resourceCulture);

	internal static string SqlParameter_Offset => ResourceManager.GetString("SqlParameter_Offset", resourceCulture);

	internal static string SqlParameter_ParameterName => ResourceManager.GetString("SqlParameter_ParameterName", resourceCulture);

	internal static string SqlParameter_SqlDbType => ResourceManager.GetString("SqlParameter_SqlDbType", resourceCulture);

	internal static string SqlParameter_TypeName => ResourceManager.GetString("SqlParameter_TypeName", resourceCulture);

	internal static string SqlParameter_UnexpectedTypeNameForNonStruct => ResourceManager.GetString("SqlParameter_UnexpectedTypeNameForNonStruct", resourceCulture);

	internal static string SqlParameter_UnsupportedTVPOutputParameter => ResourceManager.GetString("SqlParameter_UnsupportedTVPOutputParameter", resourceCulture);

	internal static string SqlParameter_XmlSchemaCollectionDatabase => ResourceManager.GetString("SqlParameter_XmlSchemaCollectionDatabase", resourceCulture);

	internal static string SqlParameter_XmlSchemaCollectionName => ResourceManager.GetString("SqlParameter_XmlSchemaCollectionName", resourceCulture);

	internal static string SqlParameter_XmlSchemaCollectionOwningSchema => ResourceManager.GetString("SqlParameter_XmlSchemaCollectionOwningSchema", resourceCulture);

	internal static string SqlPipe_AlreadyHasAnOpenResultSet => ResourceManager.GetString("SqlPipe_AlreadyHasAnOpenResultSet", resourceCulture);

	internal static string SqlPipe_CommandHookedUpToNonContextConnection => ResourceManager.GetString("SqlPipe_CommandHookedUpToNonContextConnection", resourceCulture);

	internal static string SqlPipe_DoesNotHaveAnOpenResultSet => ResourceManager.GetString("SqlPipe_DoesNotHaveAnOpenResultSet", resourceCulture);

	internal static string SqlPipe_IsBusy => ResourceManager.GetString("SqlPipe_IsBusy", resourceCulture);

	internal static string SqlPipe_MessageTooLong => ResourceManager.GetString("SqlPipe_MessageTooLong", resourceCulture);

	internal static string SqlProvider_DuplicateSortOrdinal => ResourceManager.GetString("SqlProvider_DuplicateSortOrdinal", resourceCulture);

	internal static string SqlProvider_InvalidDataColumnMaxLength => ResourceManager.GetString("SqlProvider_InvalidDataColumnMaxLength", resourceCulture);

	internal static string SqlProvider_InvalidDataColumnType => ResourceManager.GetString("SqlProvider_InvalidDataColumnType", resourceCulture);

	internal static string SqlProvider_MissingSortOrdinal => ResourceManager.GetString("SqlProvider_MissingSortOrdinal", resourceCulture);

	internal static string SqlProvider_NotEnoughColumnsInStructuredType => ResourceManager.GetString("SqlProvider_NotEnoughColumnsInStructuredType", resourceCulture);

	internal static string SqlProvider_SortOrdinalGreaterThanFieldCount => ResourceManager.GetString("SqlProvider_SortOrdinalGreaterThanFieldCount", resourceCulture);

	internal static string SqlRetryLogic_InvalidMinMaxPair => ResourceManager.GetString("SqlRetryLogic_InvalidMinMaxPair", resourceCulture);

	internal static string SqlRetryLogic_InvalidRange => ResourceManager.GetString("SqlRetryLogic_InvalidRange", resourceCulture);

	internal static string SqlRetryLogic_RetryCanceled => ResourceManager.GetString("SqlRetryLogic_RetryCanceled", resourceCulture);

	internal static string SqlRetryLogic_RetryExceeded => ResourceManager.GetString("SqlRetryLogic_RetryExceeded", resourceCulture);

	internal static string SQLROR_FailoverNotSupported => ResourceManager.GetString("SQLROR_FailoverNotSupported", resourceCulture);

	internal static string SQLROR_InvalidRoutingInfo => ResourceManager.GetString("SQLROR_InvalidRoutingInfo", resourceCulture);

	internal static string SQLROR_RecursiveRoutingNotSupported => ResourceManager.GetString("SQLROR_RecursiveRoutingNotSupported", resourceCulture);

	internal static string SQLROR_TimeoutAfterRoutingInfo => ResourceManager.GetString("SQLROR_TimeoutAfterRoutingInfo", resourceCulture);

	internal static string SQLROR_UnexpectedRoutingInfo => ResourceManager.GetString("SQLROR_UnexpectedRoutingInfo", resourceCulture);

	internal static string SQLTVP_TableTypeCanOnlyBeParameter => ResourceManager.GetString("SQLTVP_TableTypeCanOnlyBeParameter", resourceCulture);

	internal static string SQLUDT_CantLoadAssembly => ResourceManager.GetString("SQLUDT_CantLoadAssembly", resourceCulture);

	internal static string SQLUDT_InvalidDbId => ResourceManager.GetString("SQLUDT_InvalidDbId", resourceCulture);

	internal static string SQLUDT_InvalidSize => ResourceManager.GetString("SQLUDT_InvalidSize", resourceCulture);

	internal static string SQLUDT_InvalidSqlType => ResourceManager.GetString("SQLUDT_InvalidSqlType", resourceCulture);

	internal static string SqlUdt_InvalidUdtMessage => ResourceManager.GetString("SqlUdt_InvalidUdtMessage", resourceCulture);

	internal static string SQLUDT_InvalidUdtTypeName => ResourceManager.GetString("SQLUDT_InvalidUdtTypeName", resourceCulture);

	internal static string SQLUDT_InWhereClause => ResourceManager.GetString("SQLUDT_InWhereClause", resourceCulture);

	internal static string SQLUDT_MaxByteSizeValue => ResourceManager.GetString("SQLUDT_MaxByteSizeValue", resourceCulture);

	internal static string SQLUDT_Unexpected => ResourceManager.GetString("SQLUDT_Unexpected", resourceCulture);

	internal static string SQLUDT_UnexpectedUdtTypeName => ResourceManager.GetString("SQLUDT_UnexpectedUdtTypeName", resourceCulture);

	internal static string SqlUdtReason_CannotSupportNative => ResourceManager.GetString("SqlUdtReason_CannotSupportNative", resourceCulture);

	internal static string SqlUdtReason_CannotSupportUserDefined => ResourceManager.GetString("SqlUdtReason_CannotSupportUserDefined", resourceCulture);

	internal static string SqlUdtReason_MaplessNotYetSupported => ResourceManager.GetString("SqlUdtReason_MaplessNotYetSupported", resourceCulture);

	internal static string SqlUdtReason_MultipleSerFormats => ResourceManager.GetString("SqlUdtReason_MultipleSerFormats", resourceCulture);

	internal static string SqlUdtReason_MultivaluedAssemblyId => ResourceManager.GetString("SqlUdtReason_MultivaluedAssemblyId", resourceCulture);

	internal static string SqlUdtReason_NativeFormatExplictLayoutNotAllowed => ResourceManager.GetString("SqlUdtReason_NativeFormatExplictLayoutNotAllowed", resourceCulture);

	internal static string SqlUdtReason_NativeFormatNoFieldSupport => ResourceManager.GetString("SqlUdtReason_NativeFormatNoFieldSupport", resourceCulture);

	internal static string SqlUdtReason_NativeUdtMaxByteSize => ResourceManager.GetString("SqlUdtReason_NativeUdtMaxByteSize", resourceCulture);

	internal static string SqlUdtReason_NativeUdtNotSequentialLayout => ResourceManager.GetString("SqlUdtReason_NativeUdtNotSequentialLayout", resourceCulture);

	internal static string SqlUdtReason_NonSerializableField => ResourceManager.GetString("SqlUdtReason_NonSerializableField", resourceCulture);

	internal static string SqlUdtReason_NoPublicConstructor => ResourceManager.GetString("SqlUdtReason_NoPublicConstructor", resourceCulture);

	internal static string SqlUdtReason_NoPublicConstructors => ResourceManager.GetString("SqlUdtReason_NoPublicConstructors", resourceCulture);

	internal static string SqlUdtReason_NotNullable => ResourceManager.GetString("SqlUdtReason_NotNullable", resourceCulture);

	internal static string SqlUdtReason_NotSerializable => ResourceManager.GetString("SqlUdtReason_NotSerializable", resourceCulture);

	internal static string SqlUdtReason_NoUdtAttribute => ResourceManager.GetString("SqlUdtReason_NoUdtAttribute", resourceCulture);

	internal static string SqlUdtReason_NullPropertyMissing => ResourceManager.GetString("SqlUdtReason_NullPropertyMissing", resourceCulture);

	internal static string SqlUdtReason_ParseMethodMissing => ResourceManager.GetString("SqlUdtReason_ParseMethodMissing", resourceCulture);

	internal static string SqlUdtReason_ToStringMethodMissing => ResourceManager.GetString("SqlUdtReason_ToStringMethodMissing", resourceCulture);

	internal static string SqlUdtReason_TypeNotPublic => ResourceManager.GetString("SqlUdtReason_TypeNotPublic", resourceCulture);

	internal static string StrongTyping_CananotAccessDBNull => ResourceManager.GetString("StrongTyping_CananotAccessDBNull", resourceCulture);

	internal static string StrongTyping_CananotRemoveRelation => ResourceManager.GetString("StrongTyping_CananotRemoveRelation", resourceCulture);

	internal static string StrongTyping_CannotRemoveColumn => ResourceManager.GetString("StrongTyping_CannotRemoveColumn", resourceCulture);

	internal static string TCE_AttestationInfoNotReturnedFromSQLServer => ResourceManager.GetString("TCE_AttestationInfoNotReturnedFromSQLServer", resourceCulture);

	internal static string TCE_AttestationProtocolNotSpecifiedForGeneratingEnclavePackage => ResourceManager.GetString("TCE_AttestationProtocolNotSpecifiedForGeneratingEnclavePackage", resourceCulture);

	internal static string TCE_AttestationProtocolNotSupported => ResourceManager.GetString("TCE_AttestationProtocolNotSupported", resourceCulture);

	internal static string TCE_AttestationProtocolNotSupportEnclaveType => ResourceManager.GetString("TCE_AttestationProtocolNotSupportEnclaveType", resourceCulture);

	internal static string TCE_AttestationURLNotSupported => ResourceManager.GetString("TCE_AttestationURLNotSupported", resourceCulture);

	internal static string TCE_BatchedUpdateColumnEncryptionSettingMismatch => ResourceManager.GetString("TCE_BatchedUpdateColumnEncryptionSettingMismatch", resourceCulture);

	internal static string TCE_CannotCreateSqlColumnEncryptionEnclaveProvider => ResourceManager.GetString("TCE_CannotCreateSqlColumnEncryptionEnclaveProvider", resourceCulture);

	internal static string TCE_CannotGetSqlColumnEncryptionEnclaveProviderConfig => ResourceManager.GetString("TCE_CannotGetSqlColumnEncryptionEnclaveProviderConfig", resourceCulture);

	internal static string TCE_CanOnlyCallOnce => ResourceManager.GetString("TCE_CanOnlyCallOnce", resourceCulture);

	internal static string TCE_CertificateNotFound => ResourceManager.GetString("TCE_CertificateNotFound", resourceCulture);

	internal static string TCE_CertificateNotFoundSysErr => ResourceManager.GetString("TCE_CertificateNotFoundSysErr", resourceCulture);

	internal static string TCE_CertificateWithNoPrivateKey => ResourceManager.GetString("TCE_CertificateWithNoPrivateKey", resourceCulture);

	internal static string TCE_CertificateWithNoPrivateKeySysErr => ResourceManager.GetString("TCE_CertificateWithNoPrivateKeySysErr", resourceCulture);

	internal static string TCE_ColumnDecryptionFailed => ResourceManager.GetString("TCE_ColumnDecryptionFailed", resourceCulture);

	internal static string TCE_ColumnEncryptionKeysNotFound => ResourceManager.GetString("TCE_ColumnEncryptionKeysNotFound", resourceCulture);

	internal static string TCE_ColumnMasterKeySignatureNotFound => ResourceManager.GetString("TCE_ColumnMasterKeySignatureNotFound", resourceCulture);

	internal static string TCE_ColumnMasterKeySignatureVerificationFailed => ResourceManager.GetString("TCE_ColumnMasterKeySignatureVerificationFailed", resourceCulture);

	internal static string TCE_DbConnectionString_AttestationProtocol => ResourceManager.GetString("TCE_DbConnectionString_AttestationProtocol", resourceCulture);

	internal static string TCE_DbConnectionString_IPAddressPreference => ResourceManager.GetString("TCE_DbConnectionString_IPAddressPreference", resourceCulture);

	internal static string TCE_DbConnectionString_ColumnEncryptionSetting => ResourceManager.GetString("TCE_DbConnectionString_ColumnEncryptionSetting", resourceCulture);

	internal static string TCE_DbConnectionString_EnclaveAttestationUrl => ResourceManager.GetString("TCE_DbConnectionString_EnclaveAttestationUrl", resourceCulture);

	internal static string TCE_DecryptionFailed => ResourceManager.GetString("TCE_DecryptionFailed", resourceCulture);

	internal static string TCE_EmptyArgumentInConstructorInternal => ResourceManager.GetString("TCE_EmptyArgumentInConstructorInternal", resourceCulture);

	internal static string TCE_EmptyArgumentInternal => ResourceManager.GetString("TCE_EmptyArgumentInternal", resourceCulture);

	internal static string TCE_EmptyCertificateThumbprint => ResourceManager.GetString("TCE_EmptyCertificateThumbprint", resourceCulture);

	internal static string TCE_EmptyCertificateThumbprintSysErr => ResourceManager.GetString("TCE_EmptyCertificateThumbprintSysErr", resourceCulture);

	internal static string TCE_EmptyCngKeyId => ResourceManager.GetString("TCE_EmptyCngKeyId", resourceCulture);

	internal static string TCE_EmptyCngKeyIdSysErr => ResourceManager.GetString("TCE_EmptyCngKeyIdSysErr", resourceCulture);

	internal static string TCE_EmptyCngName => ResourceManager.GetString("TCE_EmptyCngName", resourceCulture);

	internal static string TCE_EmptyCngNameSysErr => ResourceManager.GetString("TCE_EmptyCngNameSysErr", resourceCulture);

	internal static string TCE_EmptyColumnEncryptionKey => ResourceManager.GetString("TCE_EmptyColumnEncryptionKey", resourceCulture);

	internal static string TCE_EmptyCspKeyId => ResourceManager.GetString("TCE_EmptyCspKeyId", resourceCulture);

	internal static string TCE_EmptyCspKeyIdSysErr => ResourceManager.GetString("TCE_EmptyCspKeyIdSysErr", resourceCulture);

	internal static string TCE_EmptyCspName => ResourceManager.GetString("TCE_EmptyCspName", resourceCulture);

	internal static string TCE_EmptyCspNameSysErr => ResourceManager.GetString("TCE_EmptyCspNameSysErr", resourceCulture);

	internal static string TCE_EmptyEncryptedColumnEncryptionKey => ResourceManager.GetString("TCE_EmptyEncryptedColumnEncryptionKey", resourceCulture);

	internal static string TCE_EmptyProviderName => ResourceManager.GetString("TCE_EmptyProviderName", resourceCulture);

	internal static string TCE_EnclaveComputationsNotSupported => ResourceManager.GetString("TCE_EnclaveComputationsNotSupported", resourceCulture);

	internal static string TCE_EnclaveProviderNotFound => ResourceManager.GetString("TCE_EnclaveProviderNotFound", resourceCulture);

	internal static string TCE_EnclaveProvidersNotConfiguredForEnclaveBasedQuery => ResourceManager.GetString("TCE_EnclaveProvidersNotConfiguredForEnclaveBasedQuery", resourceCulture);

	internal static string TCE_EnclaveTypeNotReturned => ResourceManager.GetString("TCE_EnclaveTypeNotReturned", resourceCulture);

	internal static string TCE_EnclaveTypeNotSupported => ResourceManager.GetString("TCE_EnclaveTypeNotSupported", resourceCulture);

	internal static string TCE_EnclaveTypeNullForEnclaveBasedQuery => ResourceManager.GetString("TCE_EnclaveTypeNullForEnclaveBasedQuery", resourceCulture);

	internal static string TCE_ExceptionWhenGeneratingEnclavePackage => ResourceManager.GetString("TCE_ExceptionWhenGeneratingEnclavePackage", resourceCulture);

	internal static string TCE_FailedToEncryptRegisterRulesBytePackage => ResourceManager.GetString("TCE_FailedToEncryptRegisterRulesBytePackage", resourceCulture);

	internal static string TCE_InsufficientBuffer => ResourceManager.GetString("TCE_InsufficientBuffer", resourceCulture);

	internal static string TCE_InvalidAlgorithmVersion => ResourceManager.GetString("TCE_InvalidAlgorithmVersion", resourceCulture);

	internal static string TCE_InvalidAlgorithmVersionInEncryptedCEK => ResourceManager.GetString("TCE_InvalidAlgorithmVersionInEncryptedCEK", resourceCulture);

	internal static string TCE_InvalidAttestationParameterUnableToConvertToUnsignedInt => ResourceManager.GetString("TCE_InvalidAttestationParameterUnableToConvertToUnsignedInt", resourceCulture);

	internal static string TCE_InvalidAuthenticationTag => ResourceManager.GetString("TCE_InvalidAuthenticationTag", resourceCulture);

	internal static string TCE_InvalidCertificateLocation => ResourceManager.GetString("TCE_InvalidCertificateLocation", resourceCulture);

	internal static string TCE_InvalidCertificateLocationSysErr => ResourceManager.GetString("TCE_InvalidCertificateLocationSysErr", resourceCulture);

	internal static string TCE_InvalidCertificatePath => ResourceManager.GetString("TCE_InvalidCertificatePath", resourceCulture);

	internal static string TCE_InvalidCertificatePathSysErr => ResourceManager.GetString("TCE_InvalidCertificatePathSysErr", resourceCulture);

	internal static string TCE_InvalidCertificateSignature => ResourceManager.GetString("TCE_InvalidCertificateSignature", resourceCulture);

	internal static string TCE_InvalidCertificateStore => ResourceManager.GetString("TCE_InvalidCertificateStore", resourceCulture);

	internal static string TCE_InvalidCertificateStoreSysErr => ResourceManager.GetString("TCE_InvalidCertificateStoreSysErr", resourceCulture);

	internal static string TCE_InvalidCiphertextLengthInEncryptedCEK => ResourceManager.GetString("TCE_InvalidCiphertextLengthInEncryptedCEK", resourceCulture);

	internal static string TCE_InvalidCiphertextLengthInEncryptedCEKCng => ResourceManager.GetString("TCE_InvalidCiphertextLengthInEncryptedCEKCng", resourceCulture);

	internal static string TCE_InvalidCiphertextLengthInEncryptedCEKCsp => ResourceManager.GetString("TCE_InvalidCiphertextLengthInEncryptedCEKCsp", resourceCulture);

	internal static string TCE_InvalidCipherTextSize => ResourceManager.GetString("TCE_InvalidCipherTextSize", resourceCulture);

	internal static string TCE_InvalidCngKey => ResourceManager.GetString("TCE_InvalidCngKey", resourceCulture);

	internal static string TCE_InvalidCngKeySysErr => ResourceManager.GetString("TCE_InvalidCngKeySysErr", resourceCulture);

	internal static string TCE_InvalidCngPath => ResourceManager.GetString("TCE_InvalidCngPath", resourceCulture);

	internal static string TCE_InvalidCngPathSysErr => ResourceManager.GetString("TCE_InvalidCngPathSysErr", resourceCulture);

	internal static string TCE_InvalidCspKeyId => ResourceManager.GetString("TCE_InvalidCspKeyId", resourceCulture);

	internal static string TCE_InvalidCspKeyIdSysErr => ResourceManager.GetString("TCE_InvalidCspKeyIdSysErr", resourceCulture);

	internal static string TCE_InvalidCspName => ResourceManager.GetString("TCE_InvalidCspName", resourceCulture);

	internal static string TCE_InvalidCspNameSysErr => ResourceManager.GetString("TCE_InvalidCspNameSysErr", resourceCulture);

	internal static string TCE_InvalidCspPath => ResourceManager.GetString("TCE_InvalidCspPath", resourceCulture);

	internal static string TCE_InvalidCspPathSysErr => ResourceManager.GetString("TCE_InvalidCspPathSysErr", resourceCulture);

	internal static string TCE_InvalidCustomKeyStoreProviderName => ResourceManager.GetString("TCE_InvalidCustomKeyStoreProviderName", resourceCulture);

	internal static string TCE_InvalidDatabaseIdUnableToCastToUnsignedInt => ResourceManager.GetString("TCE_InvalidDatabaseIdUnableToCastToUnsignedInt", resourceCulture);

	internal static string TCE_InvalidEncryptionKeyOrdinalEnclaveMetadata => ResourceManager.GetString("TCE_InvalidEncryptionKeyOrdinalEnclaveMetadata", resourceCulture);

	internal static string TCE_InvalidEncryptionKeyOrdinalParameterMetadata => ResourceManager.GetString("TCE_InvalidEncryptionKeyOrdinalParameterMetadata", resourceCulture);

	internal static string TCE_InvalidEncryptionType => ResourceManager.GetString("TCE_InvalidEncryptionType", resourceCulture);

	internal static string TCE_InvalidKeyEncryptionAlgorithm => ResourceManager.GetString("TCE_InvalidKeyEncryptionAlgorithm", resourceCulture);

	internal static string TCE_InvalidKeyEncryptionAlgorithmSysErr => ResourceManager.GetString("TCE_InvalidKeyEncryptionAlgorithmSysErr", resourceCulture);

	internal static string TCE_InvalidKeyIdUnableToCastToUnsignedShort => ResourceManager.GetString("TCE_InvalidKeyIdUnableToCastToUnsignedShort", resourceCulture);

	internal static string TCE_InvalidKeySize => ResourceManager.GetString("TCE_InvalidKeySize", resourceCulture);

	internal static string TCE_InvalidKeyStoreProviderName => ResourceManager.GetString("TCE_InvalidKeyStoreProviderName", resourceCulture);

	internal static string TCE_InvalidSignature => ResourceManager.GetString("TCE_InvalidSignature", resourceCulture);

	internal static string TCE_InvalidSignatureInEncryptedCEK => ResourceManager.GetString("TCE_InvalidSignatureInEncryptedCEK", resourceCulture);

	internal static string TCE_InvalidSignatureInEncryptedCEKCng => ResourceManager.GetString("TCE_InvalidSignatureInEncryptedCEKCng", resourceCulture);

	internal static string TCE_InvalidSignatureInEncryptedCEKCsp => ResourceManager.GetString("TCE_InvalidSignatureInEncryptedCEKCsp", resourceCulture);

	internal static string TCE_KeyDecryptionFailed => ResourceManager.GetString("TCE_KeyDecryptionFailed", resourceCulture);

	internal static string TCE_KeyDecryptionFailedCertStore => ResourceManager.GetString("TCE_KeyDecryptionFailedCertStore", resourceCulture);

	internal static string TCE_LargeCertificatePathLength => ResourceManager.GetString("TCE_LargeCertificatePathLength", resourceCulture);

	internal static string TCE_LargeCertificatePathLengthSysErr => ResourceManager.GetString("TCE_LargeCertificatePathLengthSysErr", resourceCulture);

	internal static string TCE_MultipleRowsReturnedForAttestationInfo => ResourceManager.GetString("TCE_MultipleRowsReturnedForAttestationInfo", resourceCulture);

	internal static string TCE_NoAttestationUrlSpecifiedForEnclaveBasedQueryGeneratingEnclavePackage => ResourceManager.GetString("TCE_NoAttestationUrlSpecifiedForEnclaveBasedQueryGeneratingEnclavePackage", resourceCulture);

	internal static string TCE_NoAttestationUrlSpecifiedForEnclaveBasedQuerySpDescribe => ResourceManager.GetString("TCE_NoAttestationUrlSpecifiedForEnclaveBasedQuerySpDescribe", resourceCulture);

	internal static string TCE_NotSupportedByServer => ResourceManager.GetString("TCE_NotSupportedByServer", resourceCulture);

	internal static string TCE_NullArgumentInConstructorInternal => ResourceManager.GetString("TCE_NullArgumentInConstructorInternal", resourceCulture);

	internal static string TCE_NullArgumentInternal => ResourceManager.GetString("TCE_NullArgumentInternal", resourceCulture);

	internal static string TCE_NullCertificatePath => ResourceManager.GetString("TCE_NullCertificatePath", resourceCulture);

	internal static string TCE_NullCertificatePathSysErr => ResourceManager.GetString("TCE_NullCertificatePathSysErr", resourceCulture);

	internal static string TCE_NullCipherText => ResourceManager.GetString("TCE_NullCipherText", resourceCulture);

	internal static string TCE_NullCngPath => ResourceManager.GetString("TCE_NullCngPath", resourceCulture);

	internal static string TCE_NullCngPathSysErr => ResourceManager.GetString("TCE_NullCngPathSysErr", resourceCulture);

	internal static string TCE_NullColumnEncryptionAlgorithm => ResourceManager.GetString("TCE_NullColumnEncryptionAlgorithm", resourceCulture);

	internal static string TCE_NullColumnEncryptionKey => ResourceManager.GetString("TCE_NullColumnEncryptionKey", resourceCulture);

	internal static string TCE_NullColumnEncryptionKeySysErr => ResourceManager.GetString("TCE_NullColumnEncryptionKeySysErr", resourceCulture);

	internal static string TCE_NullCspPath => ResourceManager.GetString("TCE_NullCspPath", resourceCulture);

	internal static string TCE_NullCspPathSysErr => ResourceManager.GetString("TCE_NullCspPathSysErr", resourceCulture);

	internal static string TCE_NullCustomKeyStoreProviderDictionary => ResourceManager.GetString("TCE_NullCustomKeyStoreProviderDictionary", resourceCulture);

	internal static string TCE_NullEnclavePackageForEnclaveBasedQuery => ResourceManager.GetString("TCE_NullEnclavePackageForEnclaveBasedQuery", resourceCulture);

	internal static string TCE_NullEnclaveSessionDuringQueryExecution => ResourceManager.GetString("TCE_NullEnclaveSessionDuringQueryExecution", resourceCulture);

	internal static string TCE_NullEnclaveSessionReturnedFromProvider => ResourceManager.GetString("TCE_NullEnclaveSessionReturnedFromProvider", resourceCulture);

	internal static string TCE_NullEncryptedColumnEncryptionKey => ResourceManager.GetString("TCE_NullEncryptedColumnEncryptionKey", resourceCulture);

	internal static string TCE_NullKeyEncryptionAlgorithm => ResourceManager.GetString("TCE_NullKeyEncryptionAlgorithm", resourceCulture);

	internal static string TCE_NullKeyEncryptionAlgorithmSysErr => ResourceManager.GetString("TCE_NullKeyEncryptionAlgorithmSysErr", resourceCulture);

	internal static string TCE_NullPlainText => ResourceManager.GetString("TCE_NullPlainText", resourceCulture);

	internal static string TCE_NullProviderValue => ResourceManager.GetString("TCE_NullProviderValue", resourceCulture);

	internal static string TCE_OffsetOutOfBounds => ResourceManager.GetString("TCE_OffsetOutOfBounds", resourceCulture);

	internal static string TCE_ParamDecryptionFailed => ResourceManager.GetString("TCE_ParamDecryptionFailed", resourceCulture);

	internal static string TCE_ParamEncryptionFailed => ResourceManager.GetString("TCE_ParamEncryptionFailed", resourceCulture);

	internal static string TCE_ParamEncryptionMetaDataMissing => ResourceManager.GetString("TCE_ParamEncryptionMetaDataMissing", resourceCulture);

	internal static string TCE_ParamInvalidForceColumnEncryptionSetting => ResourceManager.GetString("TCE_ParamInvalidForceColumnEncryptionSetting", resourceCulture);

	internal static string TCE_ParamUnExpectedEncryptionMetadata => ResourceManager.GetString("TCE_ParamUnExpectedEncryptionMetadata", resourceCulture);

	internal static string TCE_ProcEncryptionMetaDataMissing => ResourceManager.GetString("TCE_ProcEncryptionMetaDataMissing", resourceCulture);

	internal static string TCE_SequentialAccessNotSupportedOnEncryptedColumn => ResourceManager.GetString("TCE_SequentialAccessNotSupportedOnEncryptedColumn", resourceCulture);

	internal static string TCE_SqlColumnEncryptionEnclaveProviderNameCannotBeEmpty => ResourceManager.GetString("TCE_SqlColumnEncryptionEnclaveProviderNameCannotBeEmpty", resourceCulture);

	internal static string TCE_SqlCommand_ColumnEncryptionSetting => ResourceManager.GetString("TCE_SqlCommand_ColumnEncryptionSetting", resourceCulture);

	internal static string TCE_SqlConnection_ColumnEncryptionKeyCacheTtl => ResourceManager.GetString("TCE_SqlConnection_ColumnEncryptionKeyCacheTtl", resourceCulture);

	internal static string TCE_SqlConnection_ColumnEncryptionQueryMetadataCacheEnabled => ResourceManager.GetString("TCE_SqlConnection_ColumnEncryptionQueryMetadataCacheEnabled", resourceCulture);

	internal static string TCE_SqlConnection_TrustedColumnMasterKeyPaths => ResourceManager.GetString("TCE_SqlConnection_TrustedColumnMasterKeyPaths", resourceCulture);

	internal static string TCE_SqlParameter_ForceColumnEncryption => ResourceManager.GetString("TCE_SqlParameter_ForceColumnEncryption", resourceCulture);

	internal static string TCE_StreamNotSupportOnEncryptedColumn => ResourceManager.GetString("TCE_StreamNotSupportOnEncryptedColumn", resourceCulture);

	internal static string TCE_UnableToEstablishSecureChannel => ResourceManager.GetString("TCE_UnableToEstablishSecureChannel", resourceCulture);

	internal static string TCE_UnableToVerifyColumnMasterKeySignature => ResourceManager.GetString("TCE_UnableToVerifyColumnMasterKeySignature", resourceCulture);

	internal static string TCE_UnexpectedDescribeParamFormatAttestationInfo => ResourceManager.GetString("TCE_UnexpectedDescribeParamFormatAttestationInfo", resourceCulture);

	internal static string TCE_UnexpectedDescribeParamFormatParameterMetadata => ResourceManager.GetString("TCE_UnexpectedDescribeParamFormatParameterMetadata", resourceCulture);

	internal static string TCE_UnknownColumnEncryptionAlgorithm => ResourceManager.GetString("TCE_UnknownColumnEncryptionAlgorithm", resourceCulture);

	internal static string TCE_UnknownColumnEncryptionAlgorithmId => ResourceManager.GetString("TCE_UnknownColumnEncryptionAlgorithmId", resourceCulture);

	internal static string TCE_UnrecognizedKeyStoreProviderName => ResourceManager.GetString("TCE_UnrecognizedKeyStoreProviderName", resourceCulture);

	internal static string TCE_UnsupportedDatatype => ResourceManager.GetString("TCE_UnsupportedDatatype", resourceCulture);

	internal static string TCE_UnsupportedNormalizationVersion => ResourceManager.GetString("TCE_UnsupportedNormalizationVersion", resourceCulture);

	internal static string TCE_UntrustedKeyPath => ResourceManager.GetString("TCE_UntrustedKeyPath", resourceCulture);

	internal static string TCE_VeryLargeCiphertext => ResourceManager.GetString("TCE_VeryLargeCiphertext", resourceCulture);

	internal static string VerifyEnclaveDebuggable => ResourceManager.GetString("VerifyEnclaveDebuggable", resourceCulture);

	internal static string VerifyEnclavePolicyFailedFormat => ResourceManager.GetString("VerifyEnclavePolicyFailedFormat", resourceCulture);

	internal static string VerifyEnclaveReportFailed => ResourceManager.GetString("VerifyEnclaveReportFailed", resourceCulture);

	internal static string VerifyEnclaveReportFormatFailed => ResourceManager.GetString("VerifyEnclaveReportFormatFailed", resourceCulture);

	internal static string VerifyHealthCertificateChainFormat => ResourceManager.GetString("VerifyHealthCertificateChainFormat", resourceCulture);

	internal static string Xml_AttributeValues => ResourceManager.GetString("Xml_AttributeValues", resourceCulture);

	internal static string Xml_CannotConvert => ResourceManager.GetString("Xml_CannotConvert", resourceCulture);

	internal static string Xml_CanNotDeserializeObjectType => ResourceManager.GetString("Xml_CanNotDeserializeObjectType", resourceCulture);

	internal static string Xml_CannotInstantiateAbstract => ResourceManager.GetString("Xml_CannotInstantiateAbstract", resourceCulture);

	internal static string Xml_CircularComplexType => ResourceManager.GetString("Xml_CircularComplexType", resourceCulture);

	internal static string Xml_ColumnConflict => ResourceManager.GetString("Xml_ColumnConflict", resourceCulture);

	internal static string Xml_DataTableInferenceNotSupported => ResourceManager.GetString("Xml_DataTableInferenceNotSupported", resourceCulture);

	internal static string Xml_DatatypeNotDefined => ResourceManager.GetString("Xml_DatatypeNotDefined", resourceCulture);

	internal static string Xml_DuplicateConstraint => ResourceManager.GetString("Xml_DuplicateConstraint", resourceCulture);

	internal static string Xml_DynamicWithoutXmlSerializable => ResourceManager.GetString("Xml_DynamicWithoutXmlSerializable", resourceCulture);

	internal static string Xml_ElementTypeNotFound => ResourceManager.GetString("Xml_ElementTypeNotFound", resourceCulture);

	internal static string Xml_FoundEntity => ResourceManager.GetString("Xml_FoundEntity", resourceCulture);

	internal static string Xml_InvalidField => ResourceManager.GetString("Xml_InvalidField", resourceCulture);

	internal static string Xml_InvalidKey => ResourceManager.GetString("Xml_InvalidKey", resourceCulture);

	internal static string Xml_InvalidPrefix => ResourceManager.GetString("Xml_InvalidPrefix", resourceCulture);

	internal static string Xml_InvalidSelector => ResourceManager.GetString("Xml_InvalidSelector", resourceCulture);

	internal static string Xml_IsDataSetAttributeMissingInSchema => ResourceManager.GetString("Xml_IsDataSetAttributeMissingInSchema", resourceCulture);

	internal static string Xml_MergeDuplicateDeclaration => ResourceManager.GetString("Xml_MergeDuplicateDeclaration", resourceCulture);

	internal static string Xml_MismatchKeyLength => ResourceManager.GetString("Xml_MismatchKeyLength", resourceCulture);

	internal static string Xml_MissingAttribute => ResourceManager.GetString("Xml_MissingAttribute", resourceCulture);

	internal static string Xml_MissingRefer => ResourceManager.GetString("Xml_MissingRefer", resourceCulture);

	internal static string Xml_MissingSQL => ResourceManager.GetString("Xml_MissingSQL", resourceCulture);

	internal static string Xml_MissingTable => ResourceManager.GetString("Xml_MissingTable", resourceCulture);

	internal static string Xml_MultipleParentRows => ResourceManager.GetString("Xml_MultipleParentRows", resourceCulture);

	internal static string Xml_MultipleTargetConverterEmpty => ResourceManager.GetString("Xml_MultipleTargetConverterEmpty", resourceCulture);

	internal static string Xml_MultipleTargetConverterError => ResourceManager.GetString("Xml_MultipleTargetConverterError", resourceCulture);

	internal static string Xml_NestedCircular => ResourceManager.GetString("Xml_NestedCircular", resourceCulture);

	internal static string Xml_PolymorphismNotSupported => ResourceManager.GetString("Xml_PolymorphismNotSupported", resourceCulture);

	internal static string Xml_RelationChildKeyMissing => ResourceManager.GetString("Xml_RelationChildKeyMissing", resourceCulture);

	internal static string Xml_RelationChildNameMissing => ResourceManager.GetString("Xml_RelationChildNameMissing", resourceCulture);

	internal static string Xml_RelationParentNameMissing => ResourceManager.GetString("Xml_RelationParentNameMissing", resourceCulture);

	internal static string Xml_RelationTableKeyMissing => ResourceManager.GetString("Xml_RelationTableKeyMissing", resourceCulture);

	internal static string Xml_SimpleTypeNotSupported => ResourceManager.GetString("Xml_SimpleTypeNotSupported", resourceCulture);

	internal static string Xml_TooManyIsDataSetAtributeInSchema => ResourceManager.GetString("Xml_TooManyIsDataSetAtributeInSchema", resourceCulture);

	internal static string Xml_UndefinedDatatype => ResourceManager.GetString("Xml_UndefinedDatatype", resourceCulture);

	internal static string Xml_ValueOutOfRange => ResourceManager.GetString("Xml_ValueOutOfRange", resourceCulture);

	internal Strings()
	{
	}
}
