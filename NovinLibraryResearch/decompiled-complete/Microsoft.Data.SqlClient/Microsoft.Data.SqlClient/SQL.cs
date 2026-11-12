using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Transactions;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Data.SqlTypes;

namespace Microsoft.Data.SqlClient;

internal static class SQL
{
	internal static readonly byte[] AttentionHeader = new byte[8] { 6, 1, 0, 8, 0, 0, 0, 0 };

	internal const string WriteToServer = "WriteToServer";

	internal const int SqlDependencyTimeoutDefault = 0;

	internal const int SqlDependencyServerTimeout = 432000;

	internal const string SqlNotificationServiceDefault = "SqlQueryNotificationService";

	internal const string SqlNotificationStoredProcedureDefault = "SqlQueryNotificationStoredProcedure";

	internal const string Transaction = "Transaction";

	internal const string Connection = "Connection";

	internal static Exception CannotGetDTCAddress()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_CannotGetDTCAddress));
	}

	internal static Exception InvalidOptionLength(string key)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidOptionLength, key));
	}

	internal static Exception InvalidInternalPacketSize(string str)
	{
		return ADP.ArgumentOutOfRange(str);
	}

	internal static Exception InvalidPacketSize()
	{
		return ADP.ArgumentOutOfRange(StringsHelper.GetString(Strings.SQL_InvalidTDSPacketSize));
	}

	internal static Exception InvalidPacketSizeValue()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidPacketSizeValue));
	}

	internal static Exception InvalidSSPIPacketSize()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidSSPIPacketSize));
	}

	internal static Exception NullEmptyTransactionName()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_NullEmptyTransactionName));
	}

	internal static Exception SnapshotNotSupported(System.Data.IsolationLevel level)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_SnapshotNotSupported, typeof(System.Data.IsolationLevel), level.ToString()));
	}

	internal static Exception UserInstanceFailoverNotCompatible()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_UserInstanceFailoverNotCompatible));
	}

	internal static Exception CredentialsNotProvided(SqlAuthenticationMethod auth)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_CredentialsNotProvided, DbConnectionStringBuilderUtil.AuthenticationTypeToString(auth)));
	}

	internal static Exception InvalidCertAuth()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_Certificate));
	}

	internal static Exception AuthenticationAndIntegratedSecurity()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_AuthenticationAndIntegratedSecurity));
	}

	internal static Exception IntegratedWithUserIDAndPassword()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_IntegratedWithUserIDAndPassword));
	}

	internal static Exception InteractiveWithPassword()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InteractiveWithPassword));
	}

	internal static Exception DeviceFlowWithUsernamePassword()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_DeviceFlowWithUsernamePassword));
	}

	internal static Exception NonInteractiveWithPassword(string authenticationMode)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_NonInteractiveWithPassword, authenticationMode));
	}

	internal static Exception SettingIntegratedWithCredential()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingIntegratedWithCredential));
	}

	internal static Exception SettingInteractiveWithCredential()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingInteractiveWithCredential));
	}

	internal static Exception SettingDeviceFlowWithCredential()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingDeviceFlowWithCredential));
	}

	internal static Exception SettingNonInteractiveWithCredential(string authenticationMode)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingNonInteractiveWithCredential, authenticationMode));
	}

	internal static Exception SettingCredentialWithIntegratedArgument()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_SettingCredentialWithIntegrated));
	}

	internal static Exception SettingCredentialWithInteractiveArgument()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_SettingCredentialWithInteractive));
	}

	internal static Exception SettingCredentialWithDeviceFlowArgument()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_SettingCredentialWithDeviceFlow));
	}

	internal static Exception SettingCredentialWithNonInteractiveArgument(string authenticationMode)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_SettingCredentialWithNonInteractive, authenticationMode));
	}

	internal static Exception SettingCredentialWithIntegratedInvalid()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingCredentialWithIntegrated));
	}

	internal static Exception SettingCredentialWithInteractiveInvalid()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingCredentialWithInteractive));
	}

	internal static Exception SettingCredentialWithDeviceFlowInvalid()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingCredentialWithDeviceFlow));
	}

	internal static Exception SettingCredentialWithNonInteractiveInvalid(string authenticationMode)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SettingCredentialWithNonInteractive, authenticationMode));
	}

	internal static Exception InvalidSQLServerVersionUnknown()
	{
		return ADP.DataAdapter(StringsHelper.GetString(Strings.SQL_InvalidSQLServerVersionUnknown));
	}

	internal static Exception SynchronousCallMayNotPend()
	{
		return new Exception(StringsHelper.GetString(Strings.Sql_InternalError));
	}

	internal static Exception ConnectionLockedForBcpEvent()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ConnectionLockedForBcpEvent));
	}

	internal static Exception AsyncConnectionRequired()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_AsyncConnectionRequired));
	}

	internal static Exception FatalTimeout()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_FatalTimeout));
	}

	internal static Exception InstanceFailure()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_InstanceFailure));
	}

	internal static Exception ChangePasswordArgumentMissing(string argumentName)
	{
		return ADP.ArgumentNull(StringsHelper.GetString(Strings.SQL_ChangePasswordArgumentMissing, argumentName));
	}

	internal static Exception ChangePasswordConflictsWithSSPI()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_ChangePasswordConflictsWithSSPI));
	}

	internal static Exception ChangePasswordRequiresYukon()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ChangePasswordRequiresYukon));
	}

	internal static Exception UnknownSysTxIsolationLevel(System.Transactions.IsolationLevel isolationLevel)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_UnknownSysTxIsolationLevel, isolationLevel.ToString()));
	}

	internal static Exception ChangePasswordUseOfUnallowedKey(string key)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ChangePasswordUseOfUnallowedKey, key));
	}

	internal static Exception InvalidPartnerConfiguration(string server, string database)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_InvalidPartnerConfiguration, server, database));
	}

	internal static Exception BatchedUpdateColumnEncryptionSettingMismatch()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_BatchedUpdateColumnEncryptionSettingMismatch, "SqlCommandColumnEncryptionSetting", "SelectCommand", "InsertCommand", "UpdateCommand", "DeleteCommand"));
	}

	internal static Exception MARSUnsupportedOnConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_MarsUnsupportedOnConnection));
	}

	internal static Exception CannotModifyPropertyAsyncOperationInProgress(string property)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_CannotModifyPropertyAsyncOperationInProgress, property));
	}

	internal static Exception NonLocalSSEInstance()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_NonLocalSSEInstance));
	}

	internal static Exception UnsupportedAuthentication(string authentication)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_UnsupportedAuthentication, authentication));
	}

	internal static Exception UnsupportedSqlAuthenticationMethod(SqlAuthenticationMethod authentication)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_UnsupportedSqlAuthenticationMethod, authentication));
	}

	internal static Exception UnsupportedAuthenticationSpecified(SqlAuthenticationMethod authentication)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_UnsupportedAuthenticationSpecified, authentication));
	}

	internal static Exception CannotCreateAuthProvider(string authentication, string type, Exception e)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_CannotCreateAuthProvider, authentication, type), e);
	}

	internal static Exception CannotCreateSqlAuthInitializer(string type, Exception e)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_CannotCreateAuthInitializer, type), e);
	}

	internal static Exception CannotInitializeAuthProvider(string type, Exception e)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_CannotInitializeAuthProvider, type), e);
	}

	internal static Exception UnsupportedAuthenticationByProvider(string authentication, string type)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_UnsupportedAuthenticationByProvider, type, authentication));
	}

	internal static Exception CannotFindAuthProvider(string authentication)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_CannotFindAuthProvider, authentication));
	}

	internal static Exception CannotGetAuthProviderConfig(Exception e)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_CannotGetAuthProviderConfig), e);
	}

	internal static Exception ParameterCannotBeEmpty(string paramName)
	{
		return ADP.ArgumentNull(StringsHelper.GetString(Strings.SQL_ParameterCannotBeEmpty, paramName));
	}

	internal static Exception ActiveDirectoryInteractiveTimeout()
	{
		return ADP.TimeoutException(Strings.SQL_Timeout_Active_Directory_Interactive_Authentication);
	}

	internal static Exception ActiveDirectoryDeviceFlowTimeout()
	{
		return ADP.TimeoutException(Strings.SQL_Timeout_Active_Directory_DeviceFlow_Authentication);
	}

	internal static Exception NotificationsRequireYukon()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_NotificationsRequireYukon));
	}

	internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, int value)
	{
		return ADP.ArgumentOutOfRange(StringsHelper.GetString(Strings.SQL_NotSupportedEnumerationValue, type.Name, value.ToString(CultureInfo.InvariantCulture)), type.Name);
	}

	internal static ArgumentOutOfRangeException NotSupportedCommandType(CommandType value)
	{
		return NotSupportedEnumerationValue(typeof(CommandType), (int)value);
	}

	internal static ArgumentOutOfRangeException NotSupportedIsolationLevel(System.Data.IsolationLevel value)
	{
		return NotSupportedEnumerationValue(typeof(System.Data.IsolationLevel), (int)value);
	}

	internal static Exception OperationCancelled()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_OperationCancelled));
	}

	internal static Exception PendingBeginXXXExists()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_PendingBeginXXXExists));
	}

	internal static ArgumentOutOfRangeException InvalidSqlDependencyTimeout(string param)
	{
		return ADP.ArgumentOutOfRange(StringsHelper.GetString(Strings.SqlDependency_InvalidTimeout), param);
	}

	internal static Exception NonXmlResult()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_NonXmlResult));
	}

	internal static Exception InvalidUdt3PartNameFormat()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidUdt3PartNameFormat));
	}

	internal static Exception InvalidParameterTypeNameFormat()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidParameterTypeNameFormat));
	}

	internal static Exception InvalidParameterNameLength(string value)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidParameterNameLength, value));
	}

	internal static Exception PrecisionValueOutOfRange(byte precision)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_PrecisionValueOutOfRange, precision.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception ScaleValueOutOfRange(byte scale)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_ScaleValueOutOfRange, scale.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception TimeScaleValueOutOfRange(byte scale)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_TimeScaleValueOutOfRange, scale.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception InvalidSqlDbType(SqlDbType value)
	{
		return ADP.InvalidEnumerationValue(typeof(SqlDbType), (int)value);
	}

	internal static Exception UnsupportedTVPOutputParameter(ParameterDirection direction, string paramName)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SqlParameter_UnsupportedTVPOutputParameter, direction.ToString(), paramName));
	}

	internal static Exception DBNullNotSupportedForTVPValues(string paramName)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SqlParameter_DBNullNotSupportedForTVP, paramName));
	}

	internal static Exception InvalidTableDerivedPrecisionForTvp(string columnName, byte precision)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlParameter_InvalidTableDerivedPrecisionForTvp, precision, columnName, SqlDecimal.MaxPrecision));
	}

	internal static Exception UnexpectedTypeNameForNonStructParams(string paramName)
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SqlParameter_UnexpectedTypeNameForNonStruct, paramName));
	}

	internal static Exception SingleValuedStructNotSupported()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.MetaType_SingleValuedStructNotSupported));
	}

	internal static Exception ParameterInvalidVariant(string paramName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ParameterInvalidVariant, paramName));
	}

	internal static Exception MustSetTypeNameForParam(string paramType, string paramName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_ParameterTypeNameRequired, paramType, paramName));
	}

	internal static Exception NullSchemaTableDataTypeNotSupported(string columnName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.NullSchemaTableDataTypeNotSupported, columnName));
	}

	internal static Exception InvalidSchemaTableOrdinals()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.InvalidSchemaTableOrdinals));
	}

	internal static Exception EnumeratedRecordMetaDataChanged(string fieldName, int recordNumber)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_EnumeratedRecordMetaDataChanged, fieldName, recordNumber));
	}

	internal static Exception EnumeratedRecordFieldCountChanged(int recordNumber)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_EnumeratedRecordFieldCountChanged, recordNumber));
	}

	internal static Exception InvalidTDSVersion()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_InvalidTDSVersion));
	}

	internal static Exception ParsingError(ParsingErrorState state)
	{
		string sQL_ParsingErrorWithState = Strings.SQL_ParsingErrorWithState;
		object[] array = new object[1];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorWithState, array));
	}

	internal static Exception ParsingError(ParsingErrorState state, Exception innerException)
	{
		string sQL_ParsingErrorWithState = Strings.SQL_ParsingErrorWithState;
		object[] array = new object[1];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorWithState, array), innerException);
	}

	internal static Exception ParsingErrorValue(ParsingErrorState state, int value)
	{
		string sQL_ParsingErrorValue = Strings.SQL_ParsingErrorValue;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = value;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorValue, array));
	}

	internal static Exception ParsingErrorOffset(ParsingErrorState state, int offset)
	{
		string sQL_ParsingErrorOffset = Strings.SQL_ParsingErrorOffset;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = offset;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorOffset, array));
	}

	internal static Exception ParsingErrorFeatureId(ParsingErrorState state, int featureId)
	{
		string sQL_ParsingErrorFeatureId = Strings.SQL_ParsingErrorFeatureId;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = featureId;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorFeatureId, array));
	}

	internal static Exception ParsingErrorToken(ParsingErrorState state, int token)
	{
		string sQL_ParsingErrorToken = Strings.SQL_ParsingErrorToken;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = token;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorToken, array));
	}

	internal static Exception ParsingErrorLength(ParsingErrorState state, int length)
	{
		string sQL_ParsingErrorLength = Strings.SQL_ParsingErrorLength;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = length;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorLength, array));
	}

	internal static Exception ParsingErrorStatus(ParsingErrorState state, int status)
	{
		string sQL_ParsingErrorStatus = Strings.SQL_ParsingErrorStatus;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = status;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorStatus, array));
	}

	internal static Exception ParsingErrorLibraryType(ParsingErrorState state, int libraryType)
	{
		string sQL_ParsingErrorAuthLibraryType = Strings.SQL_ParsingErrorAuthLibraryType;
		object[] array = new object[2];
		int num = (int)state;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = libraryType;
		return ADP.InvalidOperation(StringsHelper.GetString(sQL_ParsingErrorAuthLibraryType, array));
	}

	internal static Exception MoneyOverflow(string moneyValue)
	{
		return ADP.Overflow(StringsHelper.GetString(Strings.SQL_MoneyOverflow, moneyValue));
	}

	internal static Exception SmallDateTimeOverflow(string datetime)
	{
		return ADP.Overflow(StringsHelper.GetString(Strings.SQL_SmallDateTimeOverflow, datetime));
	}

	internal static Exception SNIPacketAllocationFailure()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SNIPacketAllocationFailure));
	}

	internal static Exception TimeOverflow(string time)
	{
		return ADP.Overflow(StringsHelper.GetString(Strings.SQL_TimeOverflow, time));
	}

	internal static Exception InvalidServerCertificate()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_InvalidServerCertificate));
	}

	internal static Exception InvalidRead()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_InvalidRead));
	}

	internal static Exception NonBlobColumn(string columnName)
	{
		return ADP.InvalidCast(StringsHelper.GetString(Strings.SQL_NonBlobColumn, columnName));
	}

	internal static Exception NonCharColumn(string columnName)
	{
		return ADP.InvalidCast(StringsHelper.GetString(Strings.SQL_NonCharColumn, columnName));
	}

	internal static Exception StreamNotSupportOnColumnType(string columnName)
	{
		return ADP.InvalidCast(StringsHelper.GetString(Strings.SQL_StreamNotSupportOnColumnType, columnName));
	}

	internal static Exception StreamNotSupportOnEncryptedColumn(string columnName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_StreamNotSupportOnEncryptedColumn, columnName, "Stream"));
	}

	internal static Exception SequentialAccessNotSupportedOnEncryptedColumn(string columnName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_SequentialAccessNotSupportedOnEncryptedColumn, columnName, "CommandBehavior=SequentialAccess"));
	}

	internal static Exception TextReaderNotSupportOnColumnType(string columnName)
	{
		return ADP.InvalidCast(StringsHelper.GetString(Strings.SQL_TextReaderNotSupportOnColumnType, columnName));
	}

	internal static Exception XmlReaderNotSupportOnColumnType(string columnName)
	{
		return ADP.InvalidCast(StringsHelper.GetString(Strings.SQL_XmlReaderNotSupportOnColumnType, columnName));
	}

	internal static Exception UDTUnexpectedResult(string exceptionText)
	{
		return ADP.TypeLoad(StringsHelper.GetString(Strings.SQLUDT_Unexpected, exceptionText));
	}

	internal static Exception CannotCompleteDelegatedTransactionWithOpenResults(SqlInternalConnectionTds internalConnection, bool marsOn)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(-2, 0, 11, null, StringsHelper.GetString(Strings.ADP_OpenReaderExists, marsOn ? "Command" : "Connection"), "", 0, 258u));
		return SqlException.CreateException(sqlErrorCollection, null, internalConnection);
	}

	internal static TransactionPromotionException PromotionFailed(Exception inner)
	{
		TransactionPromotionException ex = new TransactionPromotionException(StringsHelper.GetString(Strings.SqlDelegatedTransaction_PromotionFailed), inner);
		ADP.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static Exception DateTimeOverflow()
	{
		return new OverflowException(SQLResource.DateTimeOverflowMessage);
	}

	internal static Exception SqlCommandHasExistingSqlNotificationRequest()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQLNotify_AlreadyHasCommand));
	}

	internal static Exception SqlDepCannotBeCreatedInProc()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlNotify_SqlDepCannotBeCreatedInProc));
	}

	internal static Exception SqlDepDefaultOptionsButNoStart()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_DefaultOptionsButNoStart));
	}

	internal static Exception SqlDependencyDatabaseBrokerDisabled()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_DatabaseBrokerDisabled));
	}

	internal static Exception SqlDependencyEventNoDuplicate()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_EventNoDuplicate));
	}

	internal static Exception SqlDependencyDuplicateStart()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_DuplicateStart));
	}

	internal static Exception SqlDependencyIdMismatch()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_IdMismatch));
	}

	internal static Exception SqlDependencyNoMatchingServerStart()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_NoMatchingServerStart));
	}

	internal static Exception SqlDependencyNoMatchingServerDatabaseStart()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlDependency_NoMatchingServerDatabaseStart));
	}

	internal static Exception SqlNotificationException(SqlNotificationEventArgs notify)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQLNotify_ErrorFormat, notify.Type, notify.Info, notify.Source));
	}

	internal static Exception SqlMetaDataNoMetaData()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlMetaData_NoMetadata));
	}

	internal static Exception MustSetUdtTypeNameForUdtParams()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQLUDT_InvalidUdtTypeName));
	}

	internal static Exception UnexpectedUdtTypeNameForNonUdtParams()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQLUDT_UnexpectedUdtTypeName));
	}

	internal static Exception UDTInvalidSqlType(string typeName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQLUDT_InvalidSqlType, typeName));
	}

	internal static Exception UDTInvalidSize(int maxSize, int maxSupportedSize)
	{
		throw ADP.ArgumentOutOfRange(StringsHelper.GetString(Strings.SQLUDT_InvalidSize, maxSize, maxSupportedSize));
	}

	internal static Exception InvalidSqlDbTypeForConstructor(SqlDbType type)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlMetaData_InvalidSqlDbTypeForConstructorFormat, type.ToString()));
	}

	internal static Exception NameTooLong(string parameterName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlMetaData_NameTooLong), parameterName);
	}

	internal static Exception InvalidSortOrder(SortOrder order)
	{
		return ADP.InvalidEnumerationValue(typeof(SortOrder), (int)order);
	}

	internal static Exception MustSpecifyBothSortOrderAndOrdinal(SortOrder order, int ordinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlMetaData_SpecifyBothSortOrderAndOrdinal, order.ToString(), ordinal));
	}

	internal static Exception TableTypeCanOnlyBeParameter()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQLTVP_TableTypeCanOnlyBeParameter));
	}

	internal static Exception UnsupportedColumnTypeForSqlProvider(string columnName, string typeName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlProvider_InvalidDataColumnType, columnName, typeName));
	}

	internal static Exception InvalidColumnMaxLength(string columnName, long maxLength)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlProvider_InvalidDataColumnMaxLength, columnName, maxLength));
	}

	internal static Exception InvalidColumnPrecScale()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlMisc_InvalidPrecScaleMessage));
	}

	internal static Exception NotEnoughColumnsInStructuredType()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlProvider_NotEnoughColumnsInStructuredType));
	}

	internal static Exception DuplicateSortOrdinal(int sortOrdinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlProvider_DuplicateSortOrdinal, sortOrdinal));
	}

	internal static Exception MissingSortOrdinal(int sortOrdinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlProvider_MissingSortOrdinal, sortOrdinal));
	}

	internal static Exception SortOrdinalGreaterThanFieldCount(int columnOrdinal, int sortOrdinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlProvider_SortOrdinalGreaterThanFieldCount, sortOrdinal, columnOrdinal));
	}

	internal static Exception IEnumerableOfSqlDataRecordHasNoRows()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.IEnumerableOfSqlDataRecordHasNoRows));
	}

	internal static Exception SqlPipeCommandHookedUpToNonContextConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlPipe_CommandHookedUpToNonContextConnection));
	}

	internal static Exception SqlPipeMessageTooLong(int messageLength)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SqlPipe_MessageTooLong, messageLength));
	}

	internal static Exception SqlPipeIsBusy()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlPipe_IsBusy));
	}

	internal static Exception SqlPipeAlreadyHasAnOpenResultSet(string methodName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlPipe_AlreadyHasAnOpenResultSet, methodName));
	}

	internal static Exception SqlPipeDoesNotHaveAnOpenResultSet(string methodName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlPipe_DoesNotHaveAnOpenResultSet, methodName));
	}

	internal static Exception SqlResultSetClosed(string methodname)
	{
		if (methodname == null)
		{
			return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetClosed2));
		}
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetClosed, methodname));
	}

	internal static Exception SqlResultSetNoData(string methodname)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.ADP_DataReaderNoData, methodname));
	}

	internal static Exception SqlRecordReadOnly(string methodname)
	{
		if (methodname == null)
		{
			return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlRecordReadOnly2));
		}
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlRecordReadOnly, methodname));
	}

	internal static Exception SqlResultSetRowDeleted(string methodname)
	{
		if (methodname == null)
		{
			return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetRowDeleted2));
		}
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetRowDeleted, methodname));
	}

	internal static Exception SqlResultSetCommandNotInSameConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetCommandNotInSameConnection));
	}

	internal static Exception SqlResultSetNoAcceptableCursor()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_SqlResultSetNoAcceptableCursor));
	}

	internal static Exception BulkLoadMappingInaccessible()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadMappingInaccessible));
	}

	internal static Exception BulkLoadMappingsNamesOrOrdinalsOnly()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadMappingsNamesOrOrdinalsOnly));
	}

	internal static Exception BulkLoadCannotConvertValue(Type sourcetype, MetaType metatype, int ordinal, int rowNumber, bool isEncrypted, string columnName, string value, Exception e)
	{
		string text = string.Empty;
		if (!isEncrypted)
		{
			text = $" '{((value.Length > 100) ? value.Substring(0, 100) : value)}'";
		}
		if (rowNumber == -1)
		{
			return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadCannotConvertValueWithoutRowNo, text, sourcetype.Name, metatype.TypeName, ordinal, columnName), e);
		}
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadCannotConvertValue, text, sourcetype.Name, metatype.TypeName, ordinal, columnName, rowNumber), e);
	}

	internal static Exception BulkLoadNonMatchingColumnMapping()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadNonMatchingColumnMapping));
	}

	internal static Exception BulkLoadNonMatchingColumnName(string columnName)
	{
		return BulkLoadNonMatchingColumnName(columnName, null);
	}

	internal static Exception BulkLoadNonMatchingColumnName(string columnName, Exception e)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadNonMatchingColumnName, columnName), e);
	}

	internal static Exception BulkLoadNullEmptyColumnName(string paramName)
	{
		return ADP.Argument(string.Format(StringsHelper.GetString(Strings.SQL_ParameterCannotBeEmpty), paramName));
	}

	internal static Exception BulkLoadUnspecifiedSortOrder()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_BulkLoadUnspecifiedSortOrder));
	}

	internal static Exception BulkLoadInvalidOrderHint()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_BulkLoadInvalidOrderHint));
	}

	internal static Exception BulkLoadOrderHintInvalidColumn(string columnName)
	{
		return ADP.InvalidOperation(string.Format(StringsHelper.GetString(Strings.SQL_BulkLoadOrderHintInvalidColumn), columnName));
	}

	internal static Exception BulkLoadOrderHintDuplicateColumn(string columnName)
	{
		return ADP.InvalidOperation(string.Format(StringsHelper.GetString(Strings.SQL_BulkLoadOrderHintDuplicateColumn), columnName));
	}

	internal static Exception BulkLoadStringTooLong(string tableName, string columnName, string truncatedValue)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadStringTooLong, tableName, columnName, truncatedValue));
	}

	internal static Exception BulkLoadInvalidVariantValue()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadInvalidVariantValue));
	}

	internal static Exception BulkLoadInvalidTimeout(int timeout)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_BulkLoadInvalidTimeout, timeout.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception BulkLoadExistingTransaction()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadExistingTransaction));
	}

	internal static Exception BulkLoadNoCollation()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadNoCollation));
	}

	internal static Exception BulkLoadConflictingTransactionOption()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_BulkLoadConflictingTransactionOption));
	}

	internal static Exception BulkLoadLcidMismatch(int sourceLcid, string sourceColumnName, int destinationLcid, string destinationColumnName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.Sql_BulkLoadLcidMismatch, sourceLcid, sourceColumnName, destinationLcid, destinationColumnName));
	}

	internal static Exception InvalidOperationInsideEvent()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadInvalidOperationInsideEvent));
	}

	internal static Exception BulkLoadMissingDestinationTable()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadMissingDestinationTable));
	}

	internal static Exception BulkLoadInvalidDestinationTable(string tableName, Exception inner)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadInvalidDestinationTable, tableName), inner);
	}

	internal static Exception BulkLoadBulkLoadNotAllowDBNull(string columnName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadNotAllowDBNull, columnName));
	}

	internal static Exception BulkLoadPendingOperation()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BulkLoadPendingOperation));
	}

	internal static Exception InvalidKeyEncryptionAlgorithm(string encryptionAlgorithm, string validEncryptionAlgorithm, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidKeyEncryptionAlgorithmSysErr, encryptionAlgorithm, validEncryptionAlgorithm), "encryptionAlgorithm");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidKeyEncryptionAlgorithm, encryptionAlgorithm, validEncryptionAlgorithm), "encryptionAlgorithm");
	}

	internal static Exception NullKeyEncryptionAlgorithm(bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.ArgumentNull("encryptionAlgorithm", StringsHelper.GetString(Strings.TCE_NullKeyEncryptionAlgorithmSysErr));
		}
		return ADP.ArgumentNull("encryptionAlgorithm", StringsHelper.GetString(Strings.TCE_NullKeyEncryptionAlgorithm));
	}

	internal static Exception EmptyColumnEncryptionKey()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyColumnEncryptionKey), "columnEncryptionKey");
	}

	internal static Exception NullColumnEncryptionKey()
	{
		return ADP.ArgumentNull("columnEncryptionKey", StringsHelper.GetString(Strings.TCE_NullColumnEncryptionKey));
	}

	internal static Exception EmptyEncryptedColumnEncryptionKey()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyEncryptedColumnEncryptionKey), "encryptedColumnEncryptionKey");
	}

	internal static Exception NullEncryptedColumnEncryptionKey()
	{
		return ADP.ArgumentNull("encryptedColumnEncryptionKey", StringsHelper.GetString(Strings.TCE_NullEncryptedColumnEncryptionKey));
	}

	internal static Exception LargeCertificatePathLength(int actualLength, int maxLength, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_LargeCertificatePathLengthSysErr, actualLength, maxLength), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_LargeCertificatePathLength, actualLength, maxLength), "masterKeyPath");
	}

	internal static Exception NullCertificatePath(string[] validLocations, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCertificatePathSysErr, validLocations[0], validLocations[1], "/"));
		}
		return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCertificatePath, validLocations[0], validLocations[1], "/"));
	}

	internal static Exception NullCspKeyPath(bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCspPathSysErr, "/"));
		}
		return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCspPath, "/"));
	}

	internal static Exception NullCngKeyPath(bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCngPathSysErr, "/"));
		}
		return ADP.ArgumentNull("masterKeyPath", StringsHelper.GetString(Strings.TCE_NullCngPath, "/"));
	}

	internal static Exception InvalidCertificatePath(string actualCertificatePath, string[] validLocations, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificatePathSysErr, actualCertificatePath, validLocations[0], validLocations[1], "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificatePath, actualCertificatePath, validLocations[0], validLocations[1], "/"), "masterKeyPath");
	}

	internal static Exception InvalidCspPath(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspPathSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspPath, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception InvalidCngPath(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCngPathSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCngPath, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception EmptyCspName(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCspNameSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCspName, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception EmptyCngName(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCngNameSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCngName, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception EmptyCspKeyId(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCspKeyIdSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCspKeyId, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception EmptyCngKeyId(string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCngKeyIdSysErr, masterKeyPath, "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCngKeyId, masterKeyPath, "/"), "masterKeyPath");
	}

	internal static Exception InvalidCspName(string cspName, string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspNameSysErr, cspName, masterKeyPath), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspName, cspName, masterKeyPath), "masterKeyPath");
	}

	internal static Exception InvalidCspKeyIdentifier(string keyIdentifier, string masterKeyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspKeyIdSysErr, keyIdentifier, masterKeyPath), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCspKeyId, keyIdentifier, masterKeyPath), "masterKeyPath");
	}

	internal static Exception InvalidCngKey(string masterKeyPath, string cngProviderName, string keyIdentifier, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCngKeySysErr, masterKeyPath, cngProviderName, keyIdentifier), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCngKey, masterKeyPath, cngProviderName, keyIdentifier), "masterKeyPath");
	}

	internal static Exception InvalidCertificateLocation(string certificateLocation, string certificatePath, string[] validLocations, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificateLocationSysErr, certificateLocation, certificatePath, validLocations[0], validLocations[1], "/"), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificateLocation, certificateLocation, certificatePath, validLocations[0], validLocations[1], "/"), "masterKeyPath");
	}

	internal static Exception InvalidCertificateStore(string certificateStore, string certificatePath, string validCertificateStore, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificateStoreSysErr, certificateStore, certificatePath, validCertificateStore), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificateStore, certificateStore, certificatePath, validCertificateStore), "masterKeyPath");
	}

	internal static Exception EmptyCertificateThumbprint(string certificatePath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCertificateThumbprintSysErr, certificatePath), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyCertificateThumbprint, certificatePath), "masterKeyPath");
	}

	internal static Exception CertificateNotFound(string thumbprint, string certificateLocation, string certificateStore, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_CertificateNotFoundSysErr, thumbprint, certificateLocation, certificateStore), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_CertificateNotFound, thumbprint, certificateLocation, certificateStore), "masterKeyPath");
	}

	internal static Exception InvalidAlgorithmVersionInEncryptedCEK(byte actual, byte expected)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidAlgorithmVersionInEncryptedCEK, actual.ToString("X2"), expected.ToString("X2")), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidCiphertextLengthInEncryptedCEK(int actual, int expected, string certificateName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCiphertextLengthInEncryptedCEK, actual, expected, certificateName), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidCiphertextLengthInEncryptedCEKCsp(int actual, int expected, string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCiphertextLengthInEncryptedCEKCsp, actual, expected, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidCiphertextLengthInEncryptedCEKCng(int actual, int expected, string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCiphertextLengthInEncryptedCEKCng, actual, expected, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidSignatureInEncryptedCEK(int actual, int expected, string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidSignatureInEncryptedCEK, actual, expected, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidSignatureInEncryptedCEKCsp(int actual, int expected, string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidSignatureInEncryptedCEKCsp, actual, expected, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidSignatureInEncryptedCEKCng(int actual, int expected, string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidSignatureInEncryptedCEKCng, actual, expected, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidCertificateSignature(string certificatePath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCertificateSignature, certificatePath), "encryptedColumnEncryptionKey");
	}

	internal static Exception InvalidSignature(string masterKeyPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidSignature, masterKeyPath), "encryptedColumnEncryptionKey");
	}

	internal static Exception CertificateWithNoPrivateKey(string keyPath, bool isSystemOp)
	{
		if (isSystemOp)
		{
			return ADP.Argument(StringsHelper.GetString(Strings.TCE_CertificateWithNoPrivateKeySysErr, keyPath), "masterKeyPath");
		}
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_CertificateWithNoPrivateKey, keyPath), "masterKeyPath");
	}

	internal static Exception NullColumnEncryptionKeySysErr()
	{
		return ADP.ArgumentNull("encryptionKey", StringsHelper.GetString(Strings.TCE_NullColumnEncryptionKeySysErr));
	}

	internal static Exception InvalidKeySize(string algorithmName, int actualKeylength, int expectedLength)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidKeySize, algorithmName, actualKeylength, expectedLength), "encryptionKey");
	}

	internal static Exception InvalidEncryptionType(string algorithmName, SqlClientEncryptionType encryptionType, params SqlClientEncryptionType[] validEncryptionTypes)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidEncryptionType, algorithmName, encryptionType.ToString(), string.Join(", ", validEncryptionTypes.Select((SqlClientEncryptionType validEncryptionType) => "'" + validEncryptionType.ToString() + "'"))), "encryptionType");
	}

	internal static Exception NullPlainText()
	{
		return ADP.ArgumentNull(StringsHelper.GetString(Strings.TCE_NullPlainText));
	}

	internal static Exception VeryLargeCiphertext(long cipherTextLength, long maxCipherTextSize, long plainTextLength)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_VeryLargeCiphertext, cipherTextLength, maxCipherTextSize, plainTextLength));
	}

	internal static Exception NullCipherText()
	{
		return ADP.ArgumentNull(StringsHelper.GetString(Strings.TCE_NullCipherText));
	}

	internal static Exception InvalidCipherTextSize(int actualSize, int minimumSize)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCipherTextSize, actualSize, minimumSize), "cipherText");
	}

	internal static Exception InvalidAlgorithmVersion(byte actual, byte expected)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidAlgorithmVersion, actual.ToString("X2"), expected.ToString("X2")), "cipherText");
	}

	internal static Exception InvalidAuthenticationTag()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidAuthenticationTag), "cipherText");
	}

	internal static Exception NullColumnEncryptionAlgorithm(string supportedAlgorithms)
	{
		return ADP.ArgumentNull("encryptionAlgorithm", StringsHelper.GetString(Strings.TCE_NullColumnEncryptionAlgorithm, supportedAlgorithms));
	}

	internal static Exception UnexpectedDescribeParamFormatParameterMetadata()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnexpectedDescribeParamFormatParameterMetadata, "sp_describe_parameter_encryption"));
	}

	internal static Exception UnexpectedDescribeParamFormatAttestationInfo(string enclaveType)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnexpectedDescribeParamFormatAttestationInfo, "sp_describe_parameter_encryption", enclaveType));
	}

	internal static Exception InvalidEncryptionKeyOrdinalEnclaveMetadata(int ordinal, int maxOrdinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_InvalidEncryptionKeyOrdinalEnclaveMetadata, ordinal, maxOrdinal));
	}

	internal static Exception InvalidEncryptionKeyOrdinalParameterMetadata(int ordinal, int maxOrdinal)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_InvalidEncryptionKeyOrdinalParameterMetadata, ordinal, maxOrdinal));
	}

	public static Exception MultipleRowsReturnedForAttestationInfo()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_MultipleRowsReturnedForAttestationInfo, "sp_describe_parameter_encryption"));
	}

	internal static Exception ParamEncryptionMetadataMissing(string paramName, string procedureName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_ParamEncryptionMetaDataMissing, "sp_describe_parameter_encryption", paramName, procedureName));
	}

	internal static Exception ParamInvalidForceColumnEncryptionSetting(string paramName, string procedureName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_ParamInvalidForceColumnEncryptionSetting, "ForceColumnEncryption(true)", paramName, procedureName, "SqlParameter"));
	}

	internal static Exception ParamUnExpectedEncryptionMetadata(string paramName, string procedureName)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_ParamUnExpectedEncryptionMetadata, paramName, procedureName, "ForceColumnEncryption(true)", "SqlParameter"));
	}

	internal static Exception ProcEncryptionMetadataMissing(string procedureName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_ProcEncryptionMetaDataMissing, "sp_describe_parameter_encryption", procedureName));
	}

	internal static Exception InvalidKeyStoreProviderName(string providerName, List<string> systemProviders, List<string> customProviders)
	{
		string text = string.Join(", ", systemProviders.Select((string provider) => "'" + provider + "'"));
		string text2 = string.Join(", ", customProviders.Select((string provider) => "'" + provider + "'"));
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidKeyStoreProviderName, providerName, text, text2));
	}

	internal static Exception UnableToVerifyColumnMasterKeySignature(Exception innerException)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_UnableToVerifyColumnMasterKeySignature, innerException.Message), innerException);
	}

	internal static Exception ColumnMasterKeySignatureVerificationFailed(string cmkPath)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_ColumnMasterKeySignatureVerificationFailed, cmkPath));
	}

	internal static Exception ColumnMasterKeySignatureNotFound(string cmkPath)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_ColumnMasterKeySignatureNotFound, cmkPath));
	}

	internal static Exception ExceptionWhenGeneratingEnclavePackage(Exception innerException)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_ExceptionWhenGeneratingEnclavePackage, innerException.Message), innerException);
	}

	internal static Exception FailedToEncryptRegisterRulesBytePackage(Exception innerException)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_FailedToEncryptRegisterRulesBytePackage, innerException.Message), innerException);
	}

	internal static Exception InvalidKeyIdUnableToCastToUnsignedShort(int keyId, Exception innerException)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidKeyIdUnableToCastToUnsignedShort, keyId, innerException.Message), innerException);
	}

	internal static Exception InvalidDatabaseIdUnableToCastToUnsignedInt(int databaseId, Exception innerException)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidDatabaseIdUnableToCastToUnsignedInt, databaseId, innerException.Message), innerException);
	}

	internal static Exception InvalidAttestationParameterUnableToConvertToUnsignedInt(string variableName, int intValue, string enclaveType, Exception innerException)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidAttestationParameterUnableToConvertToUnsignedInt, enclaveType, intValue, variableName, innerException.Message), innerException);
	}

	internal static Exception OffsetOutOfBounds(string argument, string type, string method)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_OffsetOutOfBounds, type, method));
	}

	internal static Exception InsufficientBuffer(string argument, string type, string method)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InsufficientBuffer, argument, type, method));
	}

	internal static Exception ColumnEncryptionKeysNotFound()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_ColumnEncryptionKeysNotFound));
	}

	internal static Exception AttestationInfoNotReturnedFromSqlServer(string enclaveType, string enclaveAttestationUrl)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_AttestationInfoNotReturnedFromSQLServer, enclaveType, enclaveAttestationUrl));
	}

	internal static Exception NullArgumentInConstructorInternal(string argumentName, string objectUnderConstruction)
	{
		return ADP.ArgumentNull(argumentName, StringsHelper.GetString(Strings.TCE_NullArgumentInConstructorInternal, argumentName, objectUnderConstruction));
	}

	internal static Exception EmptyArgumentInConstructorInternal(string argumentName, string objectUnderConstruction)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyArgumentInConstructorInternal, argumentName, objectUnderConstruction));
	}

	internal static Exception NullArgumentInternal(string argumentName, string type, string method)
	{
		return ADP.ArgumentNull(argumentName, StringsHelper.GetString(Strings.TCE_NullArgumentInternal, argumentName, type, method));
	}

	internal static Exception EmptyArgumentInternal(string argumentName, string type, string method)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_EmptyArgumentInternal, argumentName, type, method));
	}

	internal static Exception CannotGetSqlColumnEncryptionEnclaveProviderConfig(Exception innerException)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_CannotGetSqlColumnEncryptionEnclaveProviderConfig, innerException.Message), innerException);
	}

	internal static Exception CannotCreateSqlColumnEncryptionEnclaveProvider(string providerName, string type, Exception innerException)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_CannotCreateSqlColumnEncryptionEnclaveProvider, providerName, type, innerException.Message), innerException);
	}

	internal static Exception SqlColumnEncryptionEnclaveProviderNameCannotBeEmpty()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_SqlColumnEncryptionEnclaveProviderNameCannotBeEmpty));
	}

	internal static Exception NoAttestationUrlSpecifiedForEnclaveBasedQuerySpDescribe(string enclaveType)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_NoAttestationUrlSpecifiedForEnclaveBasedQuerySpDescribe, "sp_describe_parameter_encryption", enclaveType));
	}

	internal static Exception NoAttestationUrlSpecifiedForEnclaveBasedQueryGeneratingEnclavePackage(string enclaveType)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_NoAttestationUrlSpecifiedForEnclaveBasedQueryGeneratingEnclavePackage, enclaveType));
	}

	internal static Exception EnclaveTypeNullForEnclaveBasedQuery()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveTypeNullForEnclaveBasedQuery));
	}

	internal static Exception EnclaveProvidersNotConfiguredForEnclaveBasedQuery()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveProvidersNotConfiguredForEnclaveBasedQuery));
	}

	internal static Exception EnclaveProviderNotFound(string enclaveType, string attestationProtocol)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveProviderNotFound, enclaveType, attestationProtocol));
	}

	internal static Exception AttestationProtocolNotSpecifiedForGeneratingEnclavePackage()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_AttestationProtocolNotSpecifiedForGeneratingEnclavePackage));
	}

	internal static Exception NullEnclaveSessionReturnedFromProvider(string enclaveType, string attestationUrl)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_NullEnclaveSessionReturnedFromProvider, enclaveType, attestationUrl));
	}

	internal static Exception GetExceptionArray(string serverName, string errorMessage, Exception e)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		Exception innerException = ((e.InnerException != null) ? e.InnerException : e);
		sqlErrorCollection.Add(new SqlError(0, 0, 11, serverName, errorMessage, null, 0));
		if (e is SqlException)
		{
			SqlException ex = (SqlException)e;
			SqlErrorCollection errors = ex.Errors;
			for (int i = 0; i < ex.Errors.Count; i++)
			{
				sqlErrorCollection.Add(errors[i]);
			}
		}
		else
		{
			sqlErrorCollection.Add(new SqlError(0, 0, 11, serverName, e.Message, null, 0));
		}
		return SqlException.CreateException(sqlErrorCollection, "", null, innerException);
	}

	internal static Exception ParamEncryptionFailed(string paramName, string serverName, Exception e)
	{
		return GetExceptionArray(serverName, StringsHelper.GetString(Strings.TCE_ParamEncryptionFailed, paramName), e);
	}

	internal static Exception ParamDecryptionFailed(string paramName, string serverName, Exception e)
	{
		return GetExceptionArray(serverName, StringsHelper.GetString(Strings.TCE_ParamDecryptionFailed, paramName), e);
	}

	internal static Exception ColumnDecryptionFailed(string columnName, string serverName, Exception e)
	{
		return GetExceptionArray(serverName, StringsHelper.GetString(Strings.TCE_ColumnDecryptionFailed, columnName), e);
	}

	internal static Exception UnknownColumnEncryptionAlgorithm(string algorithmName, string supportedAlgorithms)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnknownColumnEncryptionAlgorithm, algorithmName, supportedAlgorithms));
	}

	internal static Exception UnknownColumnEncryptionAlgorithmId(int algoId, string supportAlgorithmIds)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnknownColumnEncryptionAlgorithmId, algoId, supportAlgorithmIds), "cipherAlgorithmId");
	}

	internal static Exception UnsupportedNormalizationVersion(byte version)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnsupportedNormalizationVersion, version, "'1'", "SQL Server"));
	}

	internal static Exception UnrecognizedKeyStoreProviderName(string providerName, List<string> systemProviders, List<string> customProviders)
	{
		string text = string.Join(", ", systemProviders.Select((string provider) => "'" + provider + "'"));
		string text2 = string.Join(", ", customProviders.Select((string provider) => "'" + provider + "'"));
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnrecognizedKeyStoreProviderName, providerName, text, text2));
	}

	internal static Exception InvalidDataTypeForEncryptedParameter(string parameterName, int actualDataType, int expectedDataType)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_NullProviderValue, parameterName, actualDataType, expectedDataType));
	}

	internal static Exception KeyDecryptionFailed(string providerName, string keyHex, Exception e)
	{
		if (providerName.Equals("MSSQL_CERTIFICATE_STORE"))
		{
			return GetExceptionArray(null, StringsHelper.GetString(Strings.TCE_KeyDecryptionFailedCertStore, providerName, keyHex), e);
		}
		return GetExceptionArray(null, StringsHelper.GetString(Strings.TCE_KeyDecryptionFailed, providerName, keyHex), e);
	}

	internal static Exception UntrustedKeyPath(string keyPath, string serverName)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UntrustedKeyPath, keyPath, serverName));
	}

	internal static Exception UnsupportedDatatypeEncryption(string dataType)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_UnsupportedDatatype, dataType));
	}

	internal static Exception ThrowDecryptionFailed(string keyStr, string valStr, Exception e)
	{
		return GetExceptionArray(null, StringsHelper.GetString(Strings.TCE_DecryptionFailed, keyStr, valStr), e);
	}

	internal static Exception NullEnclaveSessionDuringQueryExecution(string enclaveType, string enclaveAttestationUrl)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_NullEnclaveSessionDuringQueryExecution, enclaveType, enclaveAttestationUrl));
	}

	internal static Exception NullEnclavePackageForEnclaveBasedQuery(string enclaveType, string enclaveAttestationUrl)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_NullEnclavePackageForEnclaveBasedQuery, enclaveType, enclaveAttestationUrl));
	}

	internal static Exception TceNotSupported()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_NotSupportedByServer, "SQL Server"));
	}

	internal static Exception EnclaveComputationsNotSupported()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveComputationsNotSupported));
	}

	internal static Exception AttestationURLNotSupported()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_AttestationURLNotSupported));
	}

	internal static Exception AttestationProtocolNotSupported()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_AttestationProtocolNotSupported));
	}

	internal static Exception EnclaveTypeNotReturned()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveTypeNotReturned));
	}

	internal static Exception EnclaveTypeNotSupported(string enclaveType)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_EnclaveTypeNotSupported, enclaveType));
	}

	internal static Exception AttestationProtocolNotSupportEnclaveType(string attestationProtocolStr, string enclaveType)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_AttestationProtocolNotSupportEnclaveType, attestationProtocolStr, enclaveType));
	}

	internal static Exception CanOnlyCallOnce()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.TCE_CanOnlyCallOnce));
	}

	internal static Exception NullCustomKeyStoreProviderDictionary()
	{
		return ADP.ArgumentNull("clientKeyStoreProviders", StringsHelper.GetString(Strings.TCE_NullCustomKeyStoreProviderDictionary));
	}

	internal static Exception InvalidCustomKeyStoreProviderName(string providerName, string prefix)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.TCE_InvalidCustomKeyStoreProviderName, providerName, prefix), "clientKeyStoreProviders");
	}

	internal static Exception NullProviderValue(string providerName)
	{
		return ADP.ArgumentNull("clientKeyStoreProviders", StringsHelper.GetString(Strings.TCE_NullProviderValue, providerName));
	}

	internal static Exception EmptyProviderName()
	{
		return ADP.ArgumentNull("clientKeyStoreProviders", StringsHelper.GetString(Strings.TCE_EmptyProviderName));
	}

	internal static Exception ConnectionDoomed()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ConnectionDoomed));
	}

	internal static Exception OpenResultCountExceeded()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_OpenResultCountExceeded));
	}

	internal static Exception GlobalTransactionsNotEnabled()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.GT_Disabled));
	}

	internal static Exception UnsupportedSysTxForGlobalTransactions()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.GT_UnsupportedSysTxVersion));
	}

	internal static Exception MultiSubnetFailoverWithFailoverPartner(bool serverProvidedFailoverPartner, SqlInternalConnectionTds internalConnection)
	{
		string @string = StringsHelper.GetString(Strings.SQLMSF_FailoverPartnerNotSupported);
		if (serverProvidedFailoverPartner)
		{
			SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
			sqlErrorCollection.Add(new SqlError(0, 0, 20, null, @string, "", 0));
			SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
			ex._doNotReconnect = true;
			return ex;
		}
		return ADP.Argument(@string);
	}

	internal static Exception MultiSubnetFailoverWithMoreThan64IPs()
	{
		string sNIErrorMessage = GetSNIErrorMessage(47);
		return ADP.InvalidOperation(sNIErrorMessage);
	}

	internal static Exception MultiSubnetFailoverWithInstanceSpecified()
	{
		string sNIErrorMessage = GetSNIErrorMessage(48);
		return ADP.Argument(sNIErrorMessage);
	}

	internal static Exception MultiSubnetFailoverWithNonTcpProtocol()
	{
		string sNIErrorMessage = GetSNIErrorMessage(49);
		return ADP.Argument(sNIErrorMessage);
	}

	internal static Exception ROR_FailoverNotSupportedConnString()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQLROR_FailoverNotSupported));
	}

	internal static Exception ROR_FailoverNotSupportedServer(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLROR_FailoverNotSupported), "", 0));
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static Exception ROR_RecursiveRoutingNotSupported(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLROR_RecursiveRoutingNotSupported), "", 0));
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static Exception ROR_UnexpectedRoutingInfo(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLROR_UnexpectedRoutingInfo), "", 0));
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static Exception ROR_InvalidRoutingInfo(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLROR_InvalidRoutingInfo), "", 0));
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static Exception ROR_TimeoutAfterRoutingInfo(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLROR_TimeoutAfterRoutingInfo), "", 0));
		SqlException ex = SqlException.CreateException(sqlErrorCollection, null, internalConnection);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static SqlException CR_ReconnectTimeout()
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(-2, 0, 11, null, SQLMessage.Timeout(), "", 0, 258u));
		return SqlException.CreateException(sqlErrorCollection, "");
	}

	internal static SqlException CR_ReconnectionCancelled()
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 11, null, SQLMessage.OperationCancelled(), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "");
	}

	internal static Exception CR_NextAttemptWillExceedQueryTimeout(SqlException innerException, Guid connectionId)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 11, null, StringsHelper.GetString(Strings.SQLCR_NextAttemptWillExceedQueryTimeout), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", connectionId, innerException);
	}

	internal static Exception CR_EncryptionChanged(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLCR_EncryptionChanged), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", internalConnection);
	}

	internal static SqlException CR_AllAttemptsFailed(SqlException innerException, Guid connectionId)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 11, null, StringsHelper.GetString(Strings.SQLCR_AllAttemptsFailed), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", connectionId, innerException);
	}

	internal static SqlException CR_NoCRAckAtReconnection(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLCR_NoCRAckAtReconnection), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", internalConnection);
	}

	internal static SqlException CR_TDSVersionNotPreserved(SqlInternalConnectionTds internalConnection)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLCR_TDSVestionNotPreserved), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", internalConnection);
	}

	internal static SqlException CR_UnrecoverableServer(Guid connectionId)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLCR_UnrecoverableServer), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", connectionId);
	}

	internal static SqlException CR_UnrecoverableClient(Guid connectionId)
	{
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(new SqlError(0, 0, 20, null, StringsHelper.GetString(Strings.SQLCR_UnrecoverableClient), "", 0));
		return SqlException.CreateException(sqlErrorCollection, "", connectionId);
	}

	internal static Exception Azure_ManagedIdentityException(string msg)
	{
		SqlErrorCollection errorCollection = new SqlErrorCollection
		{
			new SqlError(0, 0, 20, null, msg, "", 0)
		};
		SqlException ex = SqlException.CreateException(errorCollection, null);
		ex._doNotReconnect = true;
		return ex;
	}

	internal static Exception BatchedUpdatesNotAvailableOnContextConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_BatchedUpdatesNotAvailableOnContextConnection));
	}

	internal static Exception ContextAllowsLimitedKeywords()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ContextAllowsLimitedKeywords));
	}

	internal static Exception ContextAllowsOnlyTypeSystem2005()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ContextAllowsOnlyTypeSystem2005));
	}

	internal static Exception ContextConnectionIsInUse()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ContextConnectionIsInUse));
	}

	internal static Exception ContextUnavailableOutOfProc()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ContextUnavailableOutOfProc));
	}

	internal static Exception ContextUnavailableWhileInProc()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_ContextUnavailableWhileInProc));
	}

	internal static Exception NestedTransactionScopesNotSupported()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_NestedTransactionScopesNotSupported));
	}

	internal static Exception NotAvailableOnContextConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_NotAvailableOnContextConnection));
	}

	internal static Exception NotificationsNotAvailableOnContextConnection()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_NotificationsNotAvailableOnContextConnection));
	}

	internal static Exception UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType eventType)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_UnexpectedSmiEvent, (int)eventType));
	}

	internal static Exception UserInstanceNotAvailableInProc()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_UserInstanceNotAvailableInProc));
	}

	internal static Exception ArgumentLengthMismatch(string arg1, string arg2)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_ArgumentLengthMismatch, arg1, arg2));
	}

	internal static Exception InvalidSqlDbTypeOneAllowedType(SqlDbType invalidType, string method, SqlDbType allowedType)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_InvalidSqlDbTypeWithOneAllowedType, invalidType, method, allowedType));
	}

	internal static Exception SqlPipeErrorRequiresSendEnd()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SQL_PipeErrorRequiresSendEnd));
	}

	internal static Exception TooManyValues(string arg)
	{
		return ADP.Argument(StringsHelper.GetString(Strings.SQL_TooManyValues), arg);
	}

	internal static Exception StreamWriteNotSupported()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_StreamWriteNotSupported));
	}

	internal static Exception StreamReadNotSupported()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_StreamReadNotSupported));
	}

	internal static Exception StreamSeekNotSupported()
	{
		return ADP.NotSupported(StringsHelper.GetString(Strings.SQL_StreamSeekNotSupported));
	}

	internal static SqlNullValueException SqlNullValue()
	{
		SqlNullValueException ex = new SqlNullValueException();
		ADP.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static Exception ParameterSizeRestrictionFailure(int index)
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.OleDb_CommandParameterError, index.ToString(CultureInfo.InvariantCulture), "SqlParameter.Size"));
	}

	internal static Exception SubclassMustOverride()
	{
		return ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlMisc_SubclassMustOverride));
	}

	internal static string GetSNIErrorMessage(int sniError)
	{
		string res = $"SNI_ERROR_{sniError}";
		return StringsHelper.GetString(res);
	}
}
