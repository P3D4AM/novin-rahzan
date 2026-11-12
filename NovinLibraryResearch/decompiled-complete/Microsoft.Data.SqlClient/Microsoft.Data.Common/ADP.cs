using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Win32;

namespace Microsoft.Data.Common;

internal static class ADP
{
	internal enum ConnectionError
	{
		BeginGetConnectionReturnsNull,
		GetConnectionReturnsNull,
		ConnectionOptionsMissing,
		CouldNotSwitchToClosedPreviouslyOpenedState
	}

	internal enum InternalErrorCode
	{
		UnpooledObjectHasOwner = 0,
		UnpooledObjectHasWrongOwner = 1,
		PushingObjectSecondTime = 2,
		PooledObjectHasOwner = 3,
		PooledObjectInPoolMoreThanOnce = 4,
		CreateObjectReturnedNull = 5,
		NewObjectCannotBePooled = 6,
		NonPooledObjectUsedMoreThanOnce = 7,
		AttemptingToPoolOnRestrictedToken = 8,
		ConvertSidToStringSidWReturnedNull = 10,
		AttemptingToConstructReferenceCollectionOnStaticObject = 12,
		AttemptingToEnlistTwice = 13,
		CreateReferenceCollectionReturnedNull = 14,
		PooledObjectWithoutPool = 15,
		UnexpectedWaitAnyResult = 16,
		SynchronousConnectReturnedPending = 17,
		CompletedConnectReturnedPending = 18,
		NameValuePairNext = 20,
		InvalidParserState1 = 21,
		InvalidParserState2 = 22,
		InvalidParserState3 = 23,
		InvalidBuffer = 30,
		UnimplementedSMIMethod = 40,
		InvalidSmiCall = 41,
		SqlDependencyObtainProcessDispatcherFailureObjectHandle = 50,
		SqlDependencyProcessDispatcherFailureCreateInstance = 51,
		SqlDependencyProcessDispatcherFailureAppDomain = 52,
		SqlDependencyCommandHashIsNotAssociatedWithNotification = 53,
		UnknownTransactionFailure = 60
	}

	internal const CompareOptions DefaultCompareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

	private static Task<bool> _trueTask = null;

	private static Task<bool> _falseTask = null;

	private static readonly Type StackOverflowType = typeof(StackOverflowException);

	private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

	private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

	private static readonly Type NullReferenceType = typeof(NullReferenceException);

	private static readonly Type AccessViolationType = typeof(AccessViolationException);

	private static readonly Type SecurityType = typeof(SecurityException);

	internal const string Append = "Append";

	internal const string BeginExecuteNonQuery = "BeginExecuteNonQuery";

	internal const string BeginExecuteReader = "BeginExecuteReader";

	internal const string BeginTransaction = "BeginTransaction";

	internal const string BeginExecuteXmlReader = "BeginExecuteXmlReader";

	internal const string ChangeDatabase = "ChangeDatabase";

	internal const string Cancel = "Cancel";

	internal const string Clone = "Clone";

	internal const string ColumnEncryptionSystemProviderNamePrefix = "MSSQL_";

	internal const string CommitTransaction = "CommitTransaction";

	internal const string CommandTimeout = "CommandTimeout";

	internal const string Command = "Command";

	internal const string ConnectionString = "ConnectionString";

	internal const string Connection = "Connection";

	internal const string DataSetColumn = "DataSetColumn";

	internal const string DataSetTable = "DataSetTable";

	internal const string Delete = "Delete";

	internal const string DeleteCommand = "DeleteCommand";

	internal const string DeriveParameters = "DeriveParameters";

	internal const string EndExecuteNonQuery = "EndExecuteNonQuery";

	internal const string EndExecuteReader = "EndExecuteReader";

	internal const string EndExecuteXmlReader = "EndExecuteXmlReader";

	internal const string ExecuteReader = "ExecuteReader";

	internal const string ExecuteRow = "ExecuteRow";

	internal const string ExecuteNonQuery = "ExecuteNonQuery";

	internal const string ExecuteScalar = "ExecuteScalar";

	internal const string ExecuteSqlScalar = "ExecuteSqlScalar";

	internal const string ExecuteXmlReader = "ExecuteXmlReader";

	internal const string Fill = "Fill";

	internal const string FillPage = "FillPage";

	internal const string FillSchema = "FillSchema";

	internal const string GetBytes = "GetBytes";

	internal const string GetChars = "GetChars";

	internal const string GetOleDbSchemaTable = "GetOleDbSchemaTable";

	internal const string GetProperties = "GetProperties";

	internal const string GetSchema = "GetSchema";

	internal const string GetSchemaTable = "GetSchemaTable";

	internal const string GetServerTransactionLevel = "GetServerTransactionLevel";

	internal const string Insert = "Insert";

	internal const string Open = "Open";

	internal const string Parameter = "Parameter";

	internal const string ParameterBuffer = "buffer";

	internal const string ParameterCount = "count";

	internal const string ParameterDestinationType = "destinationType";

	internal const string ParameterIndex = "index";

	internal const string ParameterName = "ParameterName";

	internal const string ParameterOffset = "offset";

	internal const string ParameterSetPosition = "set_Position";

	internal const string ParameterService = "Service";

	internal const string ParameterTimeout = "Timeout";

	internal const string ParameterUserData = "UserData";

	internal const string Prepare = "Prepare";

	internal const string QuoteIdentifier = "QuoteIdentifier";

	internal const string Read = "Read";

	internal const string ReadAsync = "ReadAsync";

	internal const string Remove = "Remove";

	internal const string RollbackTransaction = "RollbackTransaction";

	internal const string SaveTransaction = "SaveTransaction";

	internal const string SetProperties = "SetProperties";

	internal const string SourceColumn = "SourceColumn";

	internal const string SourceVersion = "SourceVersion";

	internal const string SourceTable = "SourceTable";

	internal const string UnquoteIdentifier = "UnquoteIdentifier";

	internal const string Update = "Update";

	internal const string UpdateCommand = "UpdateCommand";

	internal const string UpdateRows = "UpdateRows";

	internal const CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

	internal const int DecimalMaxPrecision = 29;

	internal const int DecimalMaxPrecision28 = 28;

	internal const int DefaultCommandTimeout = 30;

	internal const int DefaultConnectionTimeout = 15;

	internal const int InfiniteConnectionTimeout = 0;

	internal const int MaxBufferAccessTokenExpiry = 600;

	internal const float FailoverTimeoutStep = 0.08f;

	internal const float FailoverTimeoutStepForTnir = 0.125f;

	internal const int MinimumTimeoutForTnirMs = 500;

	internal static readonly string StrEmpty = "";

	internal static readonly IntPtr PtrZero = new IntPtr(0);

	internal static readonly int PtrSize = IntPtr.Size;

	internal static readonly IntPtr InvalidPtr = new IntPtr(-1);

	internal static readonly IntPtr RecordsUnaffected = new IntPtr(-1);

	internal static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

	internal const int CharSize = 2;

	internal static readonly bool IsWindowsNT = PlatformID.Win32NT == Environment.OSVersion.Platform;

	internal static readonly bool IsPlatformNT5 = IsWindowsNT && Environment.OSVersion.Version.Major >= 5;

	private const string hexDigits = "0123456789abcdef";

	private static Version _systemDataVersion;

	internal static readonly string[] AzureSqlServerEndpoints = new string[4]
	{
		StringsHelper.GetString(Strings.AZURESQL_GenericEndpoint),
		StringsHelper.GetString(Strings.AZURESQL_GermanEndpoint),
		StringsHelper.GetString(Strings.AZURESQL_UsGovEndpoint),
		StringsHelper.GetString(Strings.AZURESQL_ChinaEndpoint)
	};

	internal static Task<bool> TrueTask
	{
		get
		{
			if (_trueTask == null)
			{
				_trueTask = Task.FromResult(result: true);
			}
			return _trueTask;
		}
	}

	internal static Task<bool> FalseTask
	{
		get
		{
			if (_falseTask == null)
			{
				_falseTask = Task.FromResult(result: false);
			}
			return _falseTask;
		}
	}

	internal static Task<T> CreatedTaskWithException<T>(Exception ex)
	{
		TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
		taskCompletionSource.SetException(ex);
		return taskCompletionSource.Task;
	}

	internal static Task<T> CreatedTaskWithCancellation<T>()
	{
		TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
		taskCompletionSource.SetCanceled();
		return taskCompletionSource.Task;
	}

	internal static Exception ExceptionWithStackTrace(Exception e)
	{
		try
		{
			throw e;
		}
		catch (Exception result)
		{
			return result;
		}
	}

	private static void TraceException(string trace, Exception e)
	{
		if (e != null)
		{
			SqlClientEventSource.Log.TryTraceEvent(trace, e);
		}
	}

	internal static void TraceExceptionAsReturnValue(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|THROW> {0}", e);
	}

	internal static void TraceExceptionForCapture(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '{0}'", e);
	}

	internal static void TraceExceptionWithoutRethrow(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '{0}'", e);
	}

	internal static ArgumentException Argument(string error)
	{
		ArgumentException ex = new ArgumentException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException Argument(string error, Exception inner)
	{
		ArgumentException ex = new ArgumentException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException Argument(string error, string parameter)
	{
		ArgumentException ex = new ArgumentException(error, parameter);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException Argument(string error, string parameter, Exception inner)
	{
		ArgumentException ex = new ArgumentException(error, parameter, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentNullException ArgumentNull(string parameter)
	{
		ArgumentNullException ex = new ArgumentNullException(parameter);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentNullException ArgumentNull(string parameter, string error)
	{
		ArgumentNullException ex = new ArgumentNullException(parameter, error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string parameterName)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName, object value)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, value, message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ConfigurationException Configuration(string message)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		ConfigurationException val = (ConfigurationException)new ConfigurationErrorsException(message);
		TraceExceptionAsReturnValue((Exception)(object)val);
		return val;
	}

	internal static ConfigurationException Configuration(string message, XmlNode node)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		ConfigurationException val = (ConfigurationException)new ConfigurationErrorsException(message, node);
		TraceExceptionAsReturnValue((Exception)(object)val);
		return val;
	}

	internal static DataException Data(string message)
	{
		DataException ex = new DataException(message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IndexOutOfRangeException IndexOutOfRange(int value)
	{
		IndexOutOfRangeException ex = new IndexOutOfRangeException(value.ToString(CultureInfo.InvariantCulture));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IndexOutOfRangeException IndexOutOfRange(string error)
	{
		IndexOutOfRangeException ex = new IndexOutOfRangeException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IndexOutOfRangeException IndexOutOfRange()
	{
		IndexOutOfRangeException ex = new IndexOutOfRangeException();
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidCastException InvalidCast(string error)
	{
		return InvalidCast(error, null);
	}

	internal static InvalidCastException InvalidCast(string error, Exception inner)
	{
		InvalidCastException ex = new InvalidCastException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException InvalidOperation(string error)
	{
		InvalidOperationException ex = new InvalidOperationException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static TimeoutException TimeoutException(string error)
	{
		TimeoutException ex = new TimeoutException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException InvalidOperation(string error, Exception inner)
	{
		InvalidOperationException ex = new InvalidOperationException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static NotImplementedException NotImplemented(string error)
	{
		NotImplementedException ex = new NotImplementedException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static NotSupportedException NotSupported()
	{
		NotSupportedException ex = new NotSupportedException();
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static NotSupportedException NotSupported(string error)
	{
		NotSupportedException ex = new NotSupportedException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static OverflowException Overflow(string error)
	{
		return Overflow(error, null);
	}

	internal static OverflowException Overflow(string error, Exception inner)
	{
		OverflowException ex = new OverflowException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static PlatformNotSupportedException PropertyNotSupported(string property)
	{
		PlatformNotSupportedException ex = new PlatformNotSupportedException(StringsHelper.GetString(Strings.ADP_PropertyNotSupported, property));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static TypeLoadException TypeLoad(string error)
	{
		TypeLoadException ex = new TypeLoadException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidCastException InvalidCast()
	{
		InvalidCastException ex = new InvalidCastException();
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IOException IO(string error)
	{
		IOException ex = new IOException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IOException IO(string error, Exception inner)
	{
		IOException ex = new IOException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException DataAdapter(string error)
	{
		return InvalidOperation(error);
	}

	internal static InvalidOperationException DataAdapter(string error, Exception inner)
	{
		return InvalidOperation(error, inner);
	}

	private static InvalidOperationException Provider(string error)
	{
		return InvalidOperation(error);
	}

	internal static ObjectDisposedException ObjectDisposed(object instance)
	{
		ObjectDisposedException ex = new ObjectDisposedException(instance.GetType().Name);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException MethodCalledTwice(string method)
	{
		InvalidOperationException ex = new InvalidOperationException(StringsHelper.GetString(Strings.ADP_CalledTwice, method));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException IncorrectAsyncResult()
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_IncorrectAsyncResult), "AsyncResult");
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException SingleValuedProperty(string propertyName, string value)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_SingleValuedProperty, propertyName, value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException DoubleValuedProperty(string propertyName, string value1, string value2)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_DoubleValuedProperty, propertyName, value1, value2));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidPrefixSuffix()
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_InvalidPrefixSuffix));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidMultipartName(string property, string value)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_InvalidMultipartName, StringsHelper.GetString(property), value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidMultipartNameIncorrectUsageOfQuotes(string property, string value)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_InvalidMultipartNameQuoteUsage, StringsHelper.GetString(property), value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidMultipartNameToManyParts(string property, string value, int limit)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_InvalidMultipartNameToManyParts, StringsHelper.GetString(property), value, limit));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException BadParameterName(string parameterName)
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_BadParameterName, parameterName));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException MultipleReturnValue()
	{
		ArgumentException ex = new ArgumentException(StringsHelper.GetString(Strings.ADP_MultipleReturnValue));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static void CheckArgumentLength(string value, string parameterName)
	{
		CheckArgumentNull(value, parameterName);
		if (value.Length == 0)
		{
			throw Argument(StringsHelper.GetString(Strings.ADP_EmptyString, parameterName));
		}
	}

	internal static void CheckArgumentLength(Array value, string parameterName)
	{
		CheckArgumentNull(value, parameterName);
		if (value.Length == 0)
		{
			throw Argument(StringsHelper.GetString(Strings.ADP_EmptyArray, parameterName));
		}
	}

	internal static void CheckArgumentNull(object value, string parameterName)
	{
		if (value == null)
		{
			throw ArgumentNull(parameterName);
		}
	}

	internal static bool IsCatchableExceptionType(Exception e)
	{
		Type type = e.GetType();
		if (type != StackOverflowType && type != OutOfMemoryType && type != ThreadAbortType && type != NullReferenceType && type != AccessViolationType)
		{
			return !SecurityType.IsAssignableFrom(type);
		}
		return false;
	}

	internal static bool IsCatchableOrSecurityExceptionType(Exception e)
	{
		Type type = e.GetType();
		if (type != StackOverflowType && type != OutOfMemoryType && type != ThreadAbortType && type != NullReferenceType)
		{
			return type != AccessViolationType;
		}
		return false;
	}

	internal static ArgumentOutOfRangeException InvalidEnumerationValue(Type type, int value)
	{
		return ArgumentOutOfRange(StringsHelper.GetString(Strings.ADP_InvalidEnumerationValue, type.Name, value.ToString(CultureInfo.InvariantCulture)), type.Name);
	}

	internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, string value, string method)
	{
		return ArgumentOutOfRange(StringsHelper.GetString(Strings.ADP_NotSupportedEnumerationValue, type.Name, value, method), type.Name);
	}

	internal static ArgumentOutOfRangeException InvalidAcceptRejectRule(AcceptRejectRule value)
	{
		return InvalidEnumerationValue(typeof(AcceptRejectRule), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidCatalogLocation(CatalogLocation value)
	{
		return InvalidEnumerationValue(typeof(CatalogLocation), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidCommandBehavior(CommandBehavior value)
	{
		return InvalidEnumerationValue(typeof(CommandBehavior), (int)value);
	}

	internal static void ValidateCommandBehavior(CommandBehavior value)
	{
		if (value < CommandBehavior.Default || (CommandBehavior.SingleResult | CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection) < value)
		{
			throw InvalidCommandBehavior(value);
		}
	}

	internal static ArgumentException InvalidArgumentLength(string argumentName, int limit)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidArgumentLength, argumentName, limit));
	}

	internal static ArgumentException MustBeReadOnly(string argumentName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_MustBeReadOnly, argumentName));
	}

	internal static ArgumentOutOfRangeException InvalidCommandType(CommandType value)
	{
		return InvalidEnumerationValue(typeof(CommandType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidConflictOptions(ConflictOption value)
	{
		return InvalidEnumerationValue(typeof(ConflictOption), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidDataRowState(DataRowState value)
	{
		return InvalidEnumerationValue(typeof(DataRowState), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidDataRowVersion(DataRowVersion value)
	{
		return InvalidEnumerationValue(typeof(DataRowVersion), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidIsolationLevel(System.Data.IsolationLevel value)
	{
		return InvalidEnumerationValue(typeof(System.Data.IsolationLevel), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidKeyRestrictionBehavior(KeyRestrictionBehavior value)
	{
		return InvalidEnumerationValue(typeof(KeyRestrictionBehavior), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidLoadOption(LoadOption value)
	{
		return InvalidEnumerationValue(typeof(LoadOption), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidMissingMappingAction(MissingMappingAction value)
	{
		return InvalidEnumerationValue(typeof(MissingMappingAction), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidMissingSchemaAction(MissingSchemaAction value)
	{
		return InvalidEnumerationValue(typeof(MissingSchemaAction), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidParameterDirection(ParameterDirection value)
	{
		return InvalidEnumerationValue(typeof(ParameterDirection), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidPermissionState(PermissionState value)
	{
		return InvalidEnumerationValue(typeof(PermissionState), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidRule(Rule value)
	{
		return InvalidEnumerationValue(typeof(Rule), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidSchemaType(SchemaType value)
	{
		return InvalidEnumerationValue(typeof(SchemaType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidStatementType(StatementType value)
	{
		return InvalidEnumerationValue(typeof(StatementType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidUpdateRowSource(UpdateRowSource value)
	{
		return InvalidEnumerationValue(typeof(UpdateRowSource), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidUpdateStatus(UpdateStatus value)
	{
		return InvalidEnumerationValue(typeof(UpdateStatus), (int)value);
	}

	internal static ArgumentOutOfRangeException NotSupportedCommandBehavior(CommandBehavior value, string method)
	{
		return NotSupportedEnumerationValue(typeof(CommandBehavior), value.ToString(), method);
	}

	internal static ArgumentOutOfRangeException NotSupportedStatementType(StatementType value, string method)
	{
		return NotSupportedEnumerationValue(typeof(StatementType), value.ToString(), method);
	}

	internal static ArgumentOutOfRangeException InvalidUserDefinedTypeSerializationFormat(Format value)
	{
		return InvalidEnumerationValue(typeof(Format), (int)value);
	}

	internal static ArgumentOutOfRangeException NotSupportedUserDefinedTypeSerializationFormat(Format value, string method)
	{
		return NotSupportedEnumerationValue(typeof(Format), value.ToString(), method);
	}

	internal static ArgumentException ConfigProviderNotFound()
	{
		return Argument(StringsHelper.GetString(Strings.ConfigProviderNotFound));
	}

	internal static InvalidOperationException ConfigProviderInvalid()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ConfigProviderInvalid));
	}

	internal static ConfigurationException ConfigProviderNotInstalled()
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigProviderNotInstalled));
	}

	internal static ConfigurationException ConfigProviderMissing()
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigProviderMissing));
	}

	internal static ConfigurationException ConfigBaseNoChildNodes(XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigBaseNoChildNodes), node);
	}

	internal static ConfigurationException ConfigBaseElementsOnly(XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigBaseElementsOnly), node);
	}

	internal static ConfigurationException ConfigUnrecognizedAttributes(XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigUnrecognizedAttributes, node.Attributes[0].Name), node);
	}

	internal static ConfigurationException ConfigUnrecognizedElement(XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigUnrecognizedElement), node);
	}

	internal static ConfigurationException ConfigSectionsUnique(string sectionName)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigSectionsUnique, sectionName));
	}

	internal static ConfigurationException ConfigRequiredAttributeMissing(string name, XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigRequiredAttributeMissing, name), node);
	}

	internal static ConfigurationException ConfigRequiredAttributeEmpty(string name, XmlNode node)
	{
		return Configuration(StringsHelper.GetString(Strings.ConfigRequiredAttributeEmpty, name), node);
	}

	internal static ArgumentException ConnectionStringSyntax(int index)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_ConnectionStringSyntax, index));
	}

	internal static ArgumentException KeywordNotSupported(string keyword)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_KeywordNotSupported, keyword));
	}

	internal static ArgumentException UdlFileError(Exception inner)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_UdlFileError), inner);
	}

	internal static ArgumentException InvalidUDL()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidUDL));
	}

	internal static InvalidOperationException InvalidDataDirectory()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidDataDirectory));
	}

	internal static ArgumentException InvalidKeyname(string parameterName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidKey), parameterName);
	}

	internal static ArgumentException InvalidValue(string parameterName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidValue), parameterName);
	}

	internal static ArgumentException InvalidMinMaxPoolSizeValues()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMinMaxPoolSizeValues));
	}

	internal static ArgumentException ConvertFailed(Type fromType, Type toType, Exception innerException)
	{
		return Argument(StringsHelper.GetString(Strings.SqlConvert_ConvertFailed, fromType.FullName, toType.FullName), innerException);
	}

	internal static InvalidOperationException InvalidMixedUsageOfSecureAndClearCredential()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureAndClearCredential));
	}

	internal static ArgumentException InvalidMixedArgumentOfSecureAndClearCredential()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureAndClearCredential));
	}

	internal static InvalidOperationException InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity));
	}

	internal static ArgumentException InvalidMixedArgumentOfSecureCredentialAndIntegratedSecurity()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity));
	}

	internal static InvalidOperationException InvalidMixedUsageOfSecureCredentialAndContextConnection()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureCredentialAndContextConnection));
	}

	internal static ArgumentException InvalidMixedArgumentOfSecureCredentialAndContextConnection()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfSecureCredentialAndContextConnection));
	}

	internal static InvalidOperationException InvalidMixedUsageOfAccessTokenAndContextConnection()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfAccessTokenAndContextConnection));
	}

	internal static InvalidOperationException InvalidMixedUsageOfAccessTokenAndIntegratedSecurity()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfAccessTokenAndIntegratedSecurity));
	}

	internal static InvalidOperationException InvalidMixedUsageOfAccessTokenAndUserIDPassword()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfAccessTokenAndUserIDPassword));
	}

	internal static Exception InvalidMixedUsageOfAccessTokenAndCredential()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfAccessTokenAndCredential));
	}

	internal static Exception InvalidMixedUsageOfAccessTokenAndAuthentication()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfAccessTokenAndAuthentication));
	}

	internal static Exception InvalidMixedUsageOfCredentialAndAccessToken()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMixedUsageOfCredentialAndAccessToken));
	}

	internal static InvalidOperationException NoConnectionString()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NoConnectionString));
	}

	internal static NotImplementedException MethodNotImplemented(string methodName)
	{
		NotImplementedException ex = new NotImplementedException(methodName);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	private static string ConnectionStateMsg(ConnectionState state)
	{
		switch (state)
		{
		case ConnectionState.Closed:
		case ConnectionState.Connecting | ConnectionState.Broken:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg_Closed);
		case ConnectionState.Connecting:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg_Connecting);
		case ConnectionState.Open:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg_Open);
		case ConnectionState.Open | ConnectionState.Executing:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg_OpenExecuting);
		case ConnectionState.Open | ConnectionState.Fetching:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg_OpenFetching);
		default:
			return StringsHelper.GetString(Strings.ADP_ConnectionStateMsg, state.ToString());
		}
	}

	internal static ConfigurationException ConfigUnableToLoadXmlMetaDataFile(string settingName)
	{
		return Configuration(StringsHelper.GetString(Strings.OleDb_ConfigUnableToLoadXmlMetaDataFile, settingName));
	}

	internal static ConfigurationException ConfigWrongNumberOfValues(string settingName)
	{
		return Configuration(StringsHelper.GetString(Strings.OleDb_ConfigWrongNumberOfValues, settingName));
	}

	internal static Exception InvalidConnectionOptionValue(string key)
	{
		return InvalidConnectionOptionValue(key, null);
	}

	internal static Exception InvalidConnectionOptionValueLength(string key, int limit)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidConnectionOptionValueLength, key, limit));
	}

	internal static Exception InvalidConnectionOptionValue(string key, Exception inner)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidConnectionOptionValue, key), inner);
	}

	internal static Exception MissingConnectionOptionValue(string key, string requiredAdditionalKey)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_MissingConnectionOptionValue, key, requiredAdditionalKey));
	}

	internal static Exception InvalidXMLBadVersion()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidXMLBadVersion));
	}

	internal static Exception NotAPermissionElement()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_NotAPermissionElement));
	}

	internal static Exception PermissionTypeMismatch()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_PermissionTypeMismatch));
	}

	internal static Exception WrongType(Type got, Type expected)
	{
		return Argument(StringsHelper.GetString(Strings.SQL_WrongType, got.ToString(), expected.ToString()));
	}

	internal static Exception OdbcNoTypesFromProvider()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_OdbcNoTypesFromProvider));
	}

	internal static Exception PooledOpenTimeout()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_PooledOpenTimeout));
	}

	internal static Exception NonPooledOpenTimeout()
	{
		return TimeoutException(StringsHelper.GetString(Strings.ADP_NonPooledOpenTimeout));
	}

	internal static ArgumentException CollectionRemoveInvalidObject(Type itemType, ICollection collection)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_CollectionRemoveInvalidObject, itemType.Name, collection.GetType().Name));
	}

	internal static ArgumentNullException CollectionNullValue(string parameter, Type collection, Type itemType)
	{
		return ArgumentNull(parameter, StringsHelper.GetString(Strings.ADP_CollectionNullValue, collection.Name, itemType.Name));
	}

	internal static IndexOutOfRangeException CollectionIndexInt32(int index, Type collection, int count)
	{
		return IndexOutOfRange(StringsHelper.GetString(Strings.ADP_CollectionIndexInt32, index.ToString(CultureInfo.InvariantCulture), collection.Name, count.ToString(CultureInfo.InvariantCulture)));
	}

	internal static IndexOutOfRangeException CollectionIndexString(Type itemType, string propertyName, string propertyValue, Type collection)
	{
		return IndexOutOfRange(StringsHelper.GetString(Strings.ADP_CollectionIndexString, itemType.Name, propertyName, propertyValue, collection.Name));
	}

	internal static InvalidCastException CollectionInvalidType(Type collection, Type itemType, object invalidValue)
	{
		return InvalidCast(StringsHelper.GetString(Strings.ADP_CollectionInvalidType, collection.Name, itemType.Name, invalidValue.GetType().Name));
	}

	internal static Exception CollectionUniqueValue(Type itemType, string propertyName, string propertyValue)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_CollectionUniqueValue, itemType.Name, propertyName, propertyValue));
	}

	internal static ArgumentException ParametersIsNotParent(Type parameterType, ICollection collection)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_CollectionIsNotParent, parameterType.Name, collection.GetType().Name));
	}

	internal static ArgumentException ParametersIsParent(Type parameterType, ICollection collection)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_CollectionIsNotParent, parameterType.Name, collection.GetType().Name));
	}

	internal static InvalidOperationException TransactionConnectionMismatch()
	{
		return Provider(StringsHelper.GetString(Strings.ADP_TransactionConnectionMismatch));
	}

	internal static InvalidOperationException TransactionCompletedButNotDisposed()
	{
		return Provider(StringsHelper.GetString(Strings.ADP_TransactionCompletedButNotDisposed));
	}

	internal static InvalidOperationException TransactionRequired(string method)
	{
		return Provider(StringsHelper.GetString(Strings.ADP_TransactionRequired, method));
	}

	internal static InvalidOperationException MissingSelectCommand(string method)
	{
		return Provider(StringsHelper.GetString(Strings.ADP_MissingSelectCommand, method));
	}

	private static InvalidOperationException DataMapping(string error)
	{
		return InvalidOperation(error);
	}

	internal static InvalidOperationException ColumnSchemaExpression(string srcColumn, string cacheColumn)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_ColumnSchemaExpression, srcColumn, cacheColumn));
	}

	internal static InvalidOperationException ColumnSchemaMismatch(string srcColumn, Type srcType, DataColumn column)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_ColumnSchemaMismatch, srcColumn, srcType.Name, column.ColumnName, column.DataType.Name));
	}

	internal static InvalidOperationException ColumnSchemaMissing(string cacheColumn, string tableName, string srcColumn)
	{
		if (IsEmpty(tableName))
		{
			return InvalidOperation(StringsHelper.GetString(Strings.ADP_ColumnSchemaMissing1, cacheColumn, tableName, srcColumn));
		}
		return DataMapping(StringsHelper.GetString(Strings.ADP_ColumnSchemaMissing2, cacheColumn, tableName, srcColumn));
	}

	internal static InvalidOperationException MissingColumnMapping(string srcColumn)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_MissingColumnMapping, srcColumn));
	}

	internal static InvalidOperationException MissingTableSchema(string cacheTable, string srcTable)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_MissingTableSchema, cacheTable, srcTable));
	}

	internal static InvalidOperationException MissingTableMapping(string srcTable)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_MissingTableMapping, srcTable));
	}

	internal static InvalidOperationException MissingTableMappingDestination(string dstTable)
	{
		return DataMapping(StringsHelper.GetString(Strings.ADP_MissingTableMappingDestination, dstTable));
	}

	internal static Exception InvalidSourceColumn(string parameter)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidSourceColumn), parameter);
	}

	internal static Exception ColumnsAddNullAttempt(string parameter)
	{
		return CollectionNullValue(parameter, typeof(DataColumnMappingCollection), typeof(DataColumnMapping));
	}

	internal static Exception ColumnsDataSetColumn(string cacheColumn)
	{
		return CollectionIndexString(typeof(DataColumnMapping), "DataSetColumn", cacheColumn, typeof(DataColumnMappingCollection));
	}

	internal static Exception ColumnsIndexInt32(int index, IColumnMappingCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception ColumnsIndexSource(string srcColumn)
	{
		return CollectionIndexString(typeof(DataColumnMapping), "SourceColumn", srcColumn, typeof(DataColumnMappingCollection));
	}

	internal static Exception ColumnsIsNotParent(ICollection collection)
	{
		return ParametersIsNotParent(typeof(DataColumnMapping), collection);
	}

	internal static Exception ColumnsIsParent(ICollection collection)
	{
		return ParametersIsParent(typeof(DataColumnMapping), collection);
	}

	internal static Exception ColumnsUniqueSourceColumn(string srcColumn)
	{
		return CollectionUniqueValue(typeof(DataColumnMapping), "SourceColumn", srcColumn);
	}

	internal static Exception NotADataColumnMapping(object value)
	{
		return CollectionInvalidType(typeof(DataColumnMappingCollection), typeof(DataColumnMapping), value);
	}

	internal static Exception InvalidSourceTable(string parameter)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidSourceTable), parameter);
	}

	internal static Exception TablesAddNullAttempt(string parameter)
	{
		return CollectionNullValue(parameter, typeof(DataTableMappingCollection), typeof(DataTableMapping));
	}

	internal static Exception TablesDataSetTable(string cacheTable)
	{
		return CollectionIndexString(typeof(DataTableMapping), "DataSetTable", cacheTable, typeof(DataTableMappingCollection));
	}

	internal static Exception TablesIndexInt32(int index, ITableMappingCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception TablesIsNotParent(ICollection collection)
	{
		return ParametersIsNotParent(typeof(DataTableMapping), collection);
	}

	internal static Exception TablesIsParent(ICollection collection)
	{
		return ParametersIsParent(typeof(DataTableMapping), collection);
	}

	internal static Exception TablesSourceIndex(string srcTable)
	{
		return CollectionIndexString(typeof(DataTableMapping), "SourceTable", srcTable, typeof(DataTableMappingCollection));
	}

	internal static Exception TablesUniqueSourceTable(string srcTable)
	{
		return CollectionUniqueValue(typeof(DataTableMapping), "SourceTable", srcTable);
	}

	internal static Exception NotADataTableMapping(object value)
	{
		return CollectionInvalidType(typeof(DataTableMappingCollection), typeof(DataTableMapping), value);
	}

	internal static InvalidOperationException CommandAsyncOperationCompleted()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.SQL_AsyncOperationCompleted));
	}

	internal static Exception CommandTextRequired(string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_CommandTextRequired, method));
	}

	internal static InvalidOperationException ConnectionRequired(string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ConnectionRequired, method));
	}

	internal static InvalidOperationException OpenConnectionRequired(string method, ConnectionState state)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_OpenConnectionRequired, method, ConnectionStateMsg(state)));
	}

	internal static InvalidOperationException UpdateConnectionRequired(StatementType statementType, bool isRowUpdatingCommand)
	{
		string res;
		if (isRowUpdatingCommand)
		{
			res = Strings.ADP_ConnectionRequired_Clone;
		}
		else
		{
			switch (statementType)
			{
			case StatementType.Insert:
				res = Strings.ADP_ConnectionRequired_Insert;
				break;
			case StatementType.Update:
				res = Strings.ADP_ConnectionRequired_Update;
				break;
			case StatementType.Delete:
				res = Strings.ADP_ConnectionRequired_Delete;
				break;
			case StatementType.Batch:
				res = Strings.ADP_ConnectionRequired_Batch;
				goto default;
			default:
				throw InvalidStatementType(statementType);
			}
		}
		return InvalidOperation(StringsHelper.GetString(res));
	}

	internal static InvalidOperationException ConnectionRequired_Res(string method)
	{
		string res = "ADP_ConnectionRequired_" + method;
		return InvalidOperation(StringsHelper.GetString(res));
	}

	internal static InvalidOperationException UpdateOpenConnectionRequired(StatementType statementType, bool isRowUpdatingCommand, ConnectionState state)
	{
		string res = (isRowUpdatingCommand ? Strings.ADP_OpenConnectionRequired_Clone : (statementType switch
		{
			StatementType.Insert => Strings.ADP_OpenConnectionRequired_Insert, 
			StatementType.Update => Strings.ADP_OpenConnectionRequired_Update, 
			StatementType.Delete => Strings.ADP_OpenConnectionRequired_Delete, 
			_ => throw InvalidStatementType(statementType), 
		}));
		return InvalidOperation(StringsHelper.GetString(res, ConnectionStateMsg(state)));
	}

	internal static Exception NoStoredProcedureExists(string sproc)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NoStoredProcedureExists, sproc));
	}

	internal static Exception OpenReaderExists(bool marsOn)
	{
		return OpenReaderExists(null, marsOn);
	}

	internal static Exception OpenReaderExists(Exception e, bool marsOn)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_OpenReaderExists, marsOn ? "Command" : "Connection"), e);
	}

	internal static Exception TransactionCompleted()
	{
		return DataAdapter(StringsHelper.GetString(Strings.ADP_TransactionCompleted));
	}

	internal static Exception NonSeqByteAccess(long badIndex, long currIndex, string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NonSeqByteAccess, badIndex.ToString(CultureInfo.InvariantCulture), currIndex.ToString(CultureInfo.InvariantCulture), method));
	}

	internal static Exception NegativeParameter(string parameterName)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NegativeParameter, parameterName));
	}

	internal static Exception NumericToDecimalOverflow()
	{
		return InvalidCast(StringsHelper.GetString(Strings.ADP_NumericToDecimalOverflow));
	}

	internal static Exception ExceedsMaxDataLength(long specifiedLength, long maxLength)
	{
		return IndexOutOfRange(StringsHelper.GetString(Strings.SQL_ExceedsMaxDataLength, specifiedLength.ToString(CultureInfo.InvariantCulture), maxLength.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception InvalidSeekOrigin(string parameterName)
	{
		return ArgumentOutOfRange(StringsHelper.GetString(Strings.ADP_InvalidSeekOrigin), parameterName);
	}

	internal static Exception InvalidImplicitConversion(Type fromtype, string totype)
	{
		return InvalidCast(StringsHelper.GetString(Strings.ADP_InvalidImplicitConversion, fromtype.Name, totype));
	}

	internal static Exception InvalidMetaDataValue()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMetaDataValue));
	}

	internal static Exception NotRowType()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NotRowType));
	}

	internal static ArgumentException UnwantedStatementType(StatementType statementType)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_UnwantedStatementType, statementType.ToString()));
	}

	internal static InvalidOperationException NonSequentialColumnAccess(int badCol, int currCol)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NonSequentialColumnAccess, badCol.ToString(CultureInfo.InvariantCulture), currCol.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception FillSchemaRequiresSourceTableName(string parameter)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_FillSchemaRequiresSourceTableName), parameter);
	}

	internal static Exception InvalidMaxRecords(string parameter, int max)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidMaxRecords, max.ToString(CultureInfo.InvariantCulture)), parameter);
	}

	internal static Exception InvalidStartRecord(string parameter, int start)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidStartRecord, start.ToString(CultureInfo.InvariantCulture)), parameter);
	}

	internal static Exception FillRequires(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static Exception FillRequiresSourceTableName(string parameter)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_FillRequiresSourceTableName), parameter);
	}

	internal static Exception FillChapterAutoIncrement()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_FillChapterAutoIncrement));
	}

	internal static InvalidOperationException MissingDataReaderFieldType(int index)
	{
		return DataAdapter(StringsHelper.GetString(Strings.ADP_MissingDataReaderFieldType, index));
	}

	internal static InvalidOperationException OnlyOneTableForStartRecordOrMaxRecords()
	{
		return DataAdapter(StringsHelper.GetString(Strings.ADP_OnlyOneTableForStartRecordOrMaxRecords));
	}

	internal static ArgumentNullException UpdateRequiresNonNullDataSet(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static InvalidOperationException UpdateRequiresSourceTable(string defaultSrcTableName)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_UpdateRequiresSourceTable, defaultSrcTableName));
	}

	internal static InvalidOperationException UpdateRequiresSourceTableName(string srcTable)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_UpdateRequiresSourceTableName, srcTable));
	}

	internal static ArgumentNullException UpdateRequiresDataTable(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static Exception UpdateConcurrencyViolation(StatementType statementType, int affected, int expected, DataRow[] dataRows)
	{
		DBConcurrencyException ex = new DBConcurrencyException(StringsHelper.GetString(statementType switch
		{
			StatementType.Update => Strings.ADP_UpdateConcurrencyViolation_Update, 
			StatementType.Delete => Strings.ADP_UpdateConcurrencyViolation_Delete, 
			StatementType.Batch => Strings.ADP_UpdateConcurrencyViolation_Batch, 
			_ => throw InvalidStatementType(statementType), 
		}, affected.ToString(CultureInfo.InvariantCulture), expected.ToString(CultureInfo.InvariantCulture)), null, dataRows);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException UpdateRequiresCommand(StatementType statementType, bool isRowUpdatingCommand)
	{
		string res = (isRowUpdatingCommand ? Strings.ADP_UpdateRequiresCommandClone : (statementType switch
		{
			StatementType.Select => Strings.ADP_UpdateRequiresCommandSelect, 
			StatementType.Insert => Strings.ADP_UpdateRequiresCommandInsert, 
			StatementType.Update => Strings.ADP_UpdateRequiresCommandUpdate, 
			StatementType.Delete => Strings.ADP_UpdateRequiresCommandDelete, 
			_ => throw InvalidStatementType(statementType), 
		}));
		return InvalidOperation(StringsHelper.GetString(res));
	}

	internal static ArgumentException UpdateMismatchRowTable(int i)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_UpdateMismatchRowTable, i.ToString(CultureInfo.InvariantCulture)));
	}

	internal static DataException RowUpdatedErrors()
	{
		return Data(StringsHelper.GetString(Strings.ADP_RowUpdatedErrors));
	}

	internal static DataException RowUpdatingErrors()
	{
		return Data(StringsHelper.GetString(Strings.ADP_RowUpdatingErrors));
	}

	internal static InvalidOperationException ResultsNotAllowedDuringBatch()
	{
		return DataAdapter(StringsHelper.GetString(Strings.ADP_ResultsNotAllowedDuringBatch));
	}

	internal static Exception InvalidCommandTimeout(int value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidCommandTimeout, value.ToString(CultureInfo.InvariantCulture)), "CommandTimeout");
	}

	internal static Exception DeriveParametersNotSupported(IDbCommand value)
	{
		return DataAdapter(StringsHelper.GetString(Strings.ADP_DeriveParametersNotSupported, value.GetType().Name, value.CommandType.ToString()));
	}

	internal static Exception UninitializedParameterSize(int index, Type dataType)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_UninitializedParameterSize, index.ToString(CultureInfo.InvariantCulture), dataType.Name));
	}

	internal static Exception PrepareParameterType(IDbCommand cmd)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_PrepareParameterType, cmd.GetType().Name));
	}

	internal static Exception PrepareParameterSize(IDbCommand cmd)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_PrepareParameterSize, cmd.GetType().Name));
	}

	internal static Exception PrepareParameterScale(IDbCommand cmd, string type)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_PrepareParameterScale, cmd.GetType().Name, type));
	}

	internal static Exception MismatchedAsyncResult(string expectedMethod, string gotMethod)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_MismatchedAsyncResult, expectedMethod, gotMethod));
	}

	internal static Exception ConnectionIsDisabled(Exception InnerException)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ConnectionIsDisabled), InnerException);
	}

	internal static Exception ClosedConnectionError()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ClosedConnectionError));
	}

	internal static Exception ConnectionAlreadyOpen(ConnectionState state)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ConnectionAlreadyOpen, ConnectionStateMsg(state)));
	}

	internal static Exception DelegatedTransactionPresent()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DelegatedTransactionPresent));
	}

	internal static Exception TransactionPresent()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_TransactionPresent));
	}

	internal static Exception LocalTransactionPresent()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_LocalTransactionPresent));
	}

	internal static Exception OpenConnectionPropertySet(string property, ConnectionState state)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_OpenConnectionPropertySet, property, ConnectionStateMsg(state)));
	}

	internal static Exception EmptyDatabaseName()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_EmptyDatabaseName));
	}

	internal static Exception DatabaseNameTooLong()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_DatabaseNameTooLong));
	}

	internal static Exception InternalConnectionError(ConnectionError internalError)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InternalConnectionError, (int)internalError));
	}

	internal static Exception InternalError(InternalErrorCode internalError)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InternalProviderError, (int)internalError));
	}

	internal static Exception InternalError(InternalErrorCode internalError, Exception innerException)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InternalProviderError, (int)internalError), innerException);
	}

	internal static Exception InvalidConnectTimeoutValue()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidConnectTimeoutValue));
	}

	internal static Exception InvalidConnectRetryCountValue()
	{
		return Argument(StringsHelper.GetString(Strings.SQLCR_InvalidConnectRetryCountValue));
	}

	internal static Exception InvalidConnectRetryIntervalValue()
	{
		return Argument(StringsHelper.GetString(Strings.SQLCR_InvalidConnectRetryIntervalValue));
	}

	internal static Exception DataReaderNoData()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DataReaderNoData));
	}

	internal static Exception DataReaderClosed(string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DataReaderClosed, method));
	}

	internal static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
	{
		return ArgumentOutOfRange(StringsHelper.GetString(Strings.ADP_InvalidSourceBufferIndex, maxLen.ToString(CultureInfo.InvariantCulture), srcOffset.ToString(CultureInfo.InvariantCulture)), parameterName);
	}

	internal static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
	{
		return ArgumentOutOfRange(StringsHelper.GetString(Strings.ADP_InvalidDestinationBufferIndex, maxLen.ToString(CultureInfo.InvariantCulture), dstOffset.ToString(CultureInfo.InvariantCulture)), parameterName);
	}

	internal static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex)
	{
		return IndexOutOfRange(StringsHelper.GetString(Strings.SQL_InvalidBufferSizeOrIndex, numBytes.ToString(CultureInfo.InvariantCulture), bufferIndex.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception InvalidDataLength(long length)
	{
		return IndexOutOfRange(StringsHelper.GetString(Strings.SQL_InvalidDataLength, length.ToString(CultureInfo.InvariantCulture)));
	}

	internal static InvalidOperationException AsyncOperationPending()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_PendingAsyncOperation));
	}

	internal static Exception StreamClosed(string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_StreamClosed, method));
	}

	internal static IOException ErrorReadingFromStream(Exception internalException)
	{
		return IO(StringsHelper.GetString(Strings.SqlMisc_StreamErrorMessage), internalException);
	}

	internal static InvalidOperationException DynamicSQLJoinUnsupported()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLJoinUnsupported));
	}

	internal static InvalidOperationException DynamicSQLNoTableInfo()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNoTableInfo));
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoDelete()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNoKeyInfoDelete));
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoUpdate()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNoKeyInfoUpdate));
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionDelete()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNoKeyInfoRowVersionDelete));
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionUpdate()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNoKeyInfoRowVersionUpdate));
	}

	internal static InvalidOperationException DynamicSQLNestedQuote(string name, string quote)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DynamicSQLNestedQuote, name, quote));
	}

	internal static InvalidOperationException NoQuoteChange()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_NoQuoteChange));
	}

	internal static InvalidOperationException ComputerNameEx(int lastError)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ComputerNameEx, lastError));
	}

	internal static InvalidOperationException MissingSourceCommand()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_MissingSourceCommand));
	}

	internal static InvalidOperationException MissingSourceCommandConnection()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_MissingSourceCommandConnection));
	}

	internal static ArgumentException InvalidDataType(TypeCode typecode)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidDataType, typecode.ToString()));
	}

	internal static ArgumentException UnknownDataType(Type dataType)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_UnknownDataType, dataType.FullName));
	}

	internal static ArgumentException DbTypeNotSupported(DbType type, Type enumtype)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_DbTypeNotSupported, type.ToString(), enumtype.Name));
	}

	internal static ArgumentException UnknownDataTypeCode(Type dataType, TypeCode typeCode)
	{
		string aDP_UnknownDataTypeCode = Strings.ADP_UnknownDataTypeCode;
		object[] array = new object[2];
		int num = (int)typeCode;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = dataType.FullName;
		return Argument(StringsHelper.GetString(aDP_UnknownDataTypeCode, array));
	}

	internal static ArgumentException InvalidOffsetValue(int value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidOffsetValue, value.ToString(CultureInfo.InvariantCulture)));
	}

	internal static ArgumentException InvalidSizeValue(int value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidSizeValue, value.ToString(CultureInfo.InvariantCulture)));
	}

	internal static ArgumentException ParameterValueOutOfRange(decimal value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_ParameterValueOutOfRange, value.ToString((IFormatProvider?)null)));
	}

	internal static ArgumentException ParameterValueOutOfRange(SqlDecimal value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_ParameterValueOutOfRange, value.ToString()));
	}

	internal static ArgumentException ParameterValueOutOfRange(string value)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_ParameterValueOutOfRange, value));
	}

	internal static ArgumentException VersionDoesNotSupportDataType(string typeName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_VersionDoesNotSupportDataType, typeName));
	}

	internal static Exception ParameterConversionFailed(object value, Type destType, Exception inner)
	{
		string @string = StringsHelper.GetString(Strings.ADP_ParameterConversionFailed, value.GetType().Name, destType.Name);
		Exception ex = ((inner is ArgumentException) ? new ArgumentException(@string, inner) : ((inner is FormatException) ? new FormatException(@string, inner) : ((inner is InvalidCastException) ? new InvalidCastException(@string, inner) : ((!(inner is OverflowException)) ? inner : new OverflowException(@string, inner)))));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static Exception ParametersMappingIndex(int index, IDataParameterCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception ParametersSourceIndex(string parameterName, IDataParameterCollection collection, Type parameterType)
	{
		return CollectionIndexString(parameterType, "ParameterName", parameterName, collection.GetType());
	}

	internal static Exception ParameterNull(string parameter, IDataParameterCollection collection, Type parameterType)
	{
		return CollectionNullValue(parameter, collection.GetType(), parameterType);
	}

	internal static Exception InvalidParameterType(IDataParameterCollection collection, Type parameterType, object invalidValue)
	{
		return CollectionInvalidType(collection.GetType(), parameterType, invalidValue);
	}

	internal static Exception ParallelTransactionsNotSupported(IDbConnection obj)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_ParallelTransactionsNotSupported, obj.GetType().Name));
	}

	internal static Exception TransactionZombied(IDbTransaction obj)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_TransactionZombied, obj.GetType().Name));
	}

	internal static Exception DbRecordReadOnly(string methodname)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_DbRecordReadOnly, methodname));
	}

	internal static Exception OffsetOutOfRangeException()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_OffsetOutOfRangeException));
	}

	internal static Exception AmbiguousCollectionName(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_AmbiguousCollectionName, collectionName));
	}

	internal static Exception CollectionNameIsNotUnique(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_CollectionNameISNotUnique, collectionName));
	}

	internal static Exception DataTableDoesNotExist(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_DataTableDoesNotExist, collectionName));
	}

	internal static Exception IncorrectNumberOfDataSourceInformationRows()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_IncorrectNumberOfDataSourceInformationRows));
	}

	internal static ArgumentException InvalidRestrictionValue(string collectionName, string restrictionName, string restrictionValue)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_InvalidRestrictionValue, collectionName, restrictionName, restrictionValue));
	}

	internal static Exception InvalidXml()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_InvalidXml));
	}

	internal static Exception InvalidXmlMissingColumn(string collectionName, string columnName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_InvalidXmlMissingColumn, collectionName, columnName));
	}

	internal static Exception InvalidXmlInvalidValue(string collectionName, string columnName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_InvalidXmlInvalidValue, collectionName, columnName));
	}

	internal static Exception MissingDataSourceInformationColumn()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_MissingDataSourceInformationColumn));
	}

	internal static Exception MissingRestrictionColumn()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_MissingRestrictionColumn));
	}

	internal static Exception MissingRestrictionRow()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_MissingRestrictionRow));
	}

	internal static Exception NoColumns()
	{
		return Argument(StringsHelper.GetString(Strings.MDF_NoColumns));
	}

	internal static Exception QueryFailed(string collectionName, Exception e)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.MDF_QueryFailed, collectionName), e);
	}

	internal static Exception TooManyRestrictions(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_TooManyRestrictions, collectionName));
	}

	internal static Exception UnableToBuildCollection(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_UnableToBuildCollection, collectionName));
	}

	internal static Exception UndefinedCollection(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_UndefinedCollection, collectionName));
	}

	internal static Exception UndefinedPopulationMechanism(string populationMechanism)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_UndefinedPopulationMechanism, populationMechanism));
	}

	internal static Exception UnsupportedVersion(string collectionName)
	{
		return Argument(StringsHelper.GetString(Strings.MDF_UnsupportedVersion, collectionName));
	}

	internal static InvalidOperationException InvalidDateTimeDigits(string dataTypeName)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidDateTimeDigits, dataTypeName));
	}

	internal static Exception InvalidFormatValue()
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidFormatValue));
	}

	internal static InvalidOperationException InvalidMaximumScale(string dataTypeName)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_InvalidMaximumScale, dataTypeName));
	}

	internal static Exception LiteralValueIsInvalid(string dataTypeName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_LiteralValueIsInvalid, dataTypeName));
	}

	internal static Exception EvenLengthLiteralValue(string argumentName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_EvenLengthLiteralValue), argumentName);
	}

	internal static Exception HexDigitLiteralValue(string argumentName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_HexDigitLiteralValue), argumentName);
	}

	internal static InvalidOperationException QuotePrefixNotSet(string method)
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_QuotePrefixNotSet, method));
	}

	internal static InvalidOperationException UnableToCreateBooleanLiteral()
	{
		return InvalidOperation(StringsHelper.GetString(Strings.ADP_UnableToCreateBooleanLiteral));
	}

	internal static Exception UnsupportedNativeDataTypeOleDb(string dataTypeName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_UnsupportedNativeDataTypeOleDb, dataTypeName));
	}

	internal static Exception InvalidArgumentValue(string methodName)
	{
		return Argument(StringsHelper.GetString(Strings.ADP_InvalidArgumentValue, methodName));
	}

	internal static bool CompareInsensitiveInvariant(string strvalue, string strconst)
	{
		return CultureInfo.InvariantCulture.CompareInfo.Compare(strvalue, strconst, CompareOptions.IgnoreCase) == 0;
	}

	internal static Delegate FindBuilder(MulticastDelegate mcd)
	{
		if ((object)mcd != null)
		{
			Delegate[] invocationList = mcd.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				if (invocationList[i].Target is DbCommandBuilder)
				{
					return invocationList[i];
				}
			}
		}
		return null;
	}

	internal static Transaction GetCurrentTransaction()
	{
		return Transaction.Current;
	}

	internal static void SetCurrentTransaction(Transaction transaction)
	{
		Transaction.Current = transaction;
	}

	internal static IDtcTransaction GetOletxTransaction(Transaction transaction)
	{
		IDtcTransaction result = null;
		if (null != transaction)
		{
			result = TransactionInterop.GetDtcTransaction(transaction);
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool IsSysTxEqualSysEsTransaction()
	{
		return (!ContextUtil.IsInTransaction && null == Transaction.Current) || (ContextUtil.IsInTransaction && Transaction.Current == ContextUtil.SystemTransaction);
	}

	internal static bool NeedManualEnlistment()
	{
		if (IsWindowsNT)
		{
			bool flag = !InOutOfProcHelper.InProc;
			if ((flag && !IsSysTxEqualSysEsTransaction()) || (!flag && null != Transaction.Current))
			{
				return true;
			}
		}
		return false;
	}

	internal static long TimerCurrent()
	{
		return DateTime.UtcNow.ToFileTimeUtc();
	}

	internal static long TimerFromSeconds(int seconds)
	{
		checked
		{
			return unchecked((long)seconds) * 10000000L;
		}
	}

	internal static long TimerFromMilliseconds(long milliseconds)
	{
		return checked(milliseconds * 10000);
	}

	internal static bool TimerHasExpired(long timerExpire)
	{
		return TimerCurrent() > timerExpire;
	}

	internal static long TimerRemaining(long timerExpire)
	{
		long num = TimerCurrent();
		return checked(timerExpire - num);
	}

	internal static long TimerRemainingMilliseconds(long timerExpire)
	{
		return TimerToMilliseconds(TimerRemaining(timerExpire));
	}

	internal static long TimerRemainingSeconds(long timerExpire)
	{
		return TimerToSeconds(TimerRemaining(timerExpire));
	}

	internal static long TimerToMilliseconds(long timerValue)
	{
		return timerValue / 10000;
	}

	private static long TimerToSeconds(long timerValue)
	{
		return timerValue / 10000000;
	}

	[EnvironmentPermission(SecurityAction.Assert, Read = "COMPUTERNAME")]
	internal static string MachineName()
	{
		return Environment.MachineName;
	}

	internal static string BuildQuotedString(string quotePrefix, string quoteSuffix, string unQuotedString)
	{
		StringBuilder stringBuilder = new StringBuilder(unQuotedString.Length + quoteSuffix.Length + quoteSuffix.Length);
		AppendQuotedString(stringBuilder, quotePrefix, quoteSuffix, unQuotedString);
		return stringBuilder.ToString();
	}

	internal static string AppendQuotedString(StringBuilder buffer, string quotePrefix, string quoteSuffix, string unQuotedString)
	{
		if (!string.IsNullOrEmpty(quotePrefix))
		{
			buffer.Append(quotePrefix);
		}
		if (!string.IsNullOrEmpty(quoteSuffix))
		{
			int length = buffer.Length;
			buffer.Append(unQuotedString);
			buffer.Replace(quoteSuffix, quoteSuffix + quoteSuffix, length, unQuotedString.Length);
			buffer.Append(quoteSuffix);
		}
		else
		{
			buffer.Append(unQuotedString);
		}
		return buffer.ToString();
	}

	internal static string BuildMultiPartName(string[] strings)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < strings.Length; i++)
		{
			if (0 < stringBuilder.Length)
			{
				stringBuilder.Append('.');
			}
			if (strings[i] != null && strings[i].Length != 0)
			{
				stringBuilder.Append(BuildQuotedString("[", "]", strings[i]));
			}
		}
		return stringBuilder.ToString();
	}

	internal static byte[] ByteArrayFromString(string hexString, string dataTypeName)
	{
		if (((uint)hexString.Length & (true ? 1u : 0u)) != 0)
		{
			throw LiteralValueIsInvalid(dataTypeName);
		}
		char[] array = hexString.ToCharArray();
		byte[] array2 = new byte[hexString.Length / 2];
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		for (int i = 0; i < hexString.Length; i += 2)
		{
			int num = "0123456789abcdef".IndexOf(char.ToLower(array[i], invariantCulture));
			int num2 = "0123456789abcdef".IndexOf(char.ToLower(array[i + 1], invariantCulture));
			if (num < 0 || num2 < 0)
			{
				throw LiteralValueIsInvalid(dataTypeName);
			}
			array2[i / 2] = (byte)((num << 4) | num2);
		}
		return array2;
	}

	internal static void EscapeSpecialCharacters(string unescapedString, StringBuilder escapedString)
	{
		foreach (char value in unescapedString)
		{
			if (".$^{[(|)*+?\\]".IndexOf(value) >= 0)
			{
				escapedString.Append("\\");
			}
			escapedString.Append(value);
		}
	}

	internal static string FixUpDecimalSeparator(string numericString, bool formatLiteral, string decimalSeparator, char[] exponentSymbols)
	{
		if (numericString.IndexOfAny(exponentSymbols) == -1)
		{
			if (IsEmpty(decimalSeparator))
			{
				decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			}
			if (formatLiteral)
			{
				return numericString.Replace(".", decimalSeparator);
			}
			return numericString.Replace(decimalSeparator, ".");
		}
		return numericString;
	}

	internal static string GetFullPath(string filename)
	{
		return Path.GetFullPath(filename);
	}

	internal static string GetComputerNameDnsFullyQualified()
	{
		if (IsPlatformNT5)
		{
			int bufferSize = 0;
			int num = 0;
			if (SafeNativeMethods.GetComputerNameEx(3, null, ref bufferSize) == 0)
			{
				num = Marshal.GetLastWin32Error();
			}
			if ((num != 0 && num != 234) || bufferSize <= 0)
			{
				throw ComputerNameEx(num);
			}
			StringBuilder stringBuilder = new StringBuilder(bufferSize);
			bufferSize = stringBuilder.Capacity;
			if (SafeNativeMethods.GetComputerNameEx(3, stringBuilder, ref bufferSize) == 0)
			{
				throw ComputerNameEx(Marshal.GetLastWin32Error());
			}
			return stringBuilder.ToString();
		}
		return MachineName();
	}

	internal static Stream GetFileStream(string filename)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((CodeAccessPermission)new FileIOPermission((FileIOPermissionAccess)1, filename)).Assert();
		try
		{
			return new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		finally
		{
			CodeAccessPermission.RevertAssert();
		}
	}

	internal static FileVersionInfo GetVersionInfo(string filename)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		((CodeAccessPermission)new FileIOPermission((FileIOPermissionAccess)1, filename)).Assert();
		try
		{
			return FileVersionInfo.GetVersionInfo(filename);
		}
		finally
		{
			CodeAccessPermission.RevertAssert();
		}
	}

	internal static Stream GetXmlStreamFromValues(string[] values, string errorString)
	{
		if (values.Length != 1)
		{
			throw ConfigWrongNumberOfValues(errorString);
		}
		return GetXmlStream(values[0], errorString);
	}

	internal static Stream GetXmlStream(string value, string errorString)
	{
		string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
		if (runtimeDirectory == null)
		{
			throw ConfigUnableToLoadXmlMetaDataFile(errorString);
		}
		StringBuilder stringBuilder = new StringBuilder(runtimeDirectory.Length + "config\\".Length + value.Length);
		stringBuilder.Append(runtimeDirectory);
		stringBuilder.Append("config\\");
		stringBuilder.Append(value);
		string text = stringBuilder.ToString();
		if (GetFullPath(text) != text)
		{
			throw ConfigUnableToLoadXmlMetaDataFile(errorString);
		}
		try
		{
			return GetFileStream(text);
		}
		catch (Exception e)
		{
			if (!IsCatchableExceptionType(e))
			{
				throw;
			}
			throw ConfigUnableToLoadXmlMetaDataFile(errorString);
		}
	}

	internal static object ClassesRootRegistryValue(string subkey, string queryvalue)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((CodeAccessPermission)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_CLASSES_ROOT\\" + subkey)).Assert();
		try
		{
			RegistryKey val = Registry.ClassesRoot.OpenSubKey(subkey, false);
			try
			{
				return (val != null) ? val.GetValue(queryvalue) : null;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (SecurityException e)
		{
			TraceExceptionWithoutRethrow(e);
			return null;
		}
		finally
		{
			CodeAccessPermission.RevertAssert();
		}
	}

	internal static object LocalMachineRegistryValue(string subkey, string queryvalue)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((CodeAccessPermission)new RegistryPermission((RegistryPermissionAccess)1, "HKEY_LOCAL_MACHINE\\" + subkey)).Assert();
		try
		{
			RegistryKey val = Registry.LocalMachine.OpenSubKey(subkey, false);
			try
			{
				return (val != null) ? val.GetValue(queryvalue) : null;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (SecurityException e)
		{
			TraceExceptionWithoutRethrow(e);
			return null;
		}
		finally
		{
			CodeAccessPermission.RevertAssert();
		}
	}

	internal static bool RemoveStringQuotes(string quotePrefix, string quoteSuffix, string quotedString, out string unquotedString)
	{
		int num = quotePrefix?.Length ?? 0;
		int num2 = quoteSuffix?.Length ?? 0;
		if (num2 + num == 0)
		{
			unquotedString = quotedString;
			return true;
		}
		if (quotedString == null)
		{
			unquotedString = quotedString;
			return false;
		}
		int length = quotedString.Length;
		if (length < num + num2)
		{
			unquotedString = quotedString;
			return false;
		}
		if (num > 0 && !quotedString.StartsWith(quotePrefix, StringComparison.Ordinal))
		{
			unquotedString = quotedString;
			return false;
		}
		if (num2 > 0)
		{
			if (!quotedString.EndsWith(quoteSuffix, StringComparison.Ordinal))
			{
				unquotedString = quotedString;
				return false;
			}
			unquotedString = quotedString.Substring(num, length - (num + num2)).Replace(quoteSuffix + quoteSuffix, quoteSuffix);
		}
		else
		{
			unquotedString = quotedString.Substring(num, length - num);
		}
		return true;
	}

	internal static DataRow[] SelectAdapterRows(DataTable dataTable, bool sorted)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		DataRowCollection rows = dataTable.Rows;
		foreach (DataRow item in rows)
		{
			switch (item.RowState)
			{
			case DataRowState.Added:
				num++;
				break;
			case DataRowState.Deleted:
				num2++;
				break;
			case DataRowState.Modified:
				num3++;
				break;
			}
		}
		DataRow[] array = new DataRow[num + num2 + num3];
		if (sorted)
		{
			num3 = num + num2;
			num2 = num;
			num = 0;
			foreach (DataRow item2 in rows)
			{
				switch (item2.RowState)
				{
				case DataRowState.Added:
					array[num++] = item2;
					break;
				case DataRowState.Deleted:
					array[num2++] = item2;
					break;
				case DataRowState.Modified:
					array[num3++] = item2;
					break;
				}
			}
		}
		else
		{
			int num4 = 0;
			foreach (DataRow item3 in rows)
			{
				if ((item3.RowState & (DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)) != 0)
				{
					array[num4++] = item3;
					if (num4 == array.Length)
					{
						break;
					}
				}
			}
		}
		return array;
	}

	internal static int StringLength(string inputString)
	{
		return inputString?.Length ?? 0;
	}

	internal static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(columnNameArray.Length);
		int num = columnNameArray.Length;
		int num2 = columnNameArray.Length - 1;
		while (0 <= num2)
		{
			string text = columnNameArray[num2];
			if (text != null && 0 < text.Length)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				if (dictionary.TryGetValue(text, out var value))
				{
					num = Math.Min(num, value);
				}
				dictionary[text] = num2;
			}
			else
			{
				columnNameArray[num2] = StrEmpty;
				num = num2;
			}
			num2--;
		}
		int uniqueIndex = 1;
		for (int i = num; i < columnNameArray.Length; i++)
		{
			string text2 = columnNameArray[i];
			if (text2.Length == 0)
			{
				columnNameArray[i] = "Column";
				uniqueIndex = GenerateUniqueName(dictionary, ref columnNameArray[i], i, uniqueIndex);
				continue;
			}
			text2 = text2.ToLower(CultureInfo.InvariantCulture);
			if (i != dictionary[text2])
			{
				GenerateUniqueName(dictionary, ref columnNameArray[i], i, 1);
			}
		}
	}

	private static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
	{
		string text;
		string key;
		while (true)
		{
			text = columnName + uniqueIndex.ToString(CultureInfo.InvariantCulture);
			key = text.ToLower(CultureInfo.InvariantCulture);
			if (!hash.ContainsKey(key))
			{
				break;
			}
			uniqueIndex++;
		}
		columnName = text;
		hash.Add(key, index);
		return uniqueIndex;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static IntPtr IntPtrOffset(IntPtr pbase, int offset)
	{
		checked
		{
			if (4 == PtrSize)
			{
				return (IntPtr)(pbase.ToInt32() + offset);
			}
			return (IntPtr)(pbase.ToInt64() + offset);
		}
	}

	internal static int IntPtrToInt32(IntPtr value)
	{
		if (4 == PtrSize)
		{
			return (int)value;
		}
		long val = (long)value;
		val = Math.Min(2147483647L, val);
		val = Math.Max(-2147483648L, val);
		return (int)val;
	}

	internal static int SrcCompare(string strA, string strB)
	{
		if (!(strA == strB))
		{
			return 1;
		}
		return 0;
	}

	internal static int DstCompare(string strA, string strB)
	{
		return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
	}

	internal static bool IsDirection(IDataParameter value, ParameterDirection condition)
	{
		return condition == (condition & value.Direction);
	}

	internal static bool IsEmpty(string str)
	{
		if (str != null)
		{
			return str.Length == 0;
		}
		return true;
	}

	internal static bool IsEmptyArray(string[] array)
	{
		if (array != null)
		{
			return array.Length == 0;
		}
		return true;
	}

	internal static bool IsNull(object value)
	{
		if (value == null || DBNull.Value == value)
		{
			return true;
		}
		if (value is INullable nullable)
		{
			return nullable.IsNull;
		}
		return false;
	}

	internal static void IsNullOrSqlType(object value, out bool isNull, out bool isSqlType)
	{
		if (value == null || value == DBNull.Value)
		{
			isNull = true;
			isSqlType = false;
		}
		else if (value is INullable nullable)
		{
			isNull = nullable.IsNull;
			isSqlType = value is SqlBinary || value is SqlBoolean || value is SqlByte || value is SqlBytes || value is SqlChars || value is SqlDateTime || value is SqlDecimal || value is SqlDouble || value is SqlGuid || value is SqlInt16 || value is SqlInt32 || value is SqlInt64 || value is SqlMoney || value is SqlSingle || value is SqlString;
		}
		else
		{
			isNull = false;
			isSqlType = false;
		}
	}

	internal static Version GetAssemblyVersion()
	{
		if (_systemDataVersion == null)
		{
			_systemDataVersion = new Version("3.00.21159.1");
		}
		return _systemDataVersion;
	}

	internal static bool IsAzureSqlServerEndpoint(string dataSource)
	{
		int num = dataSource.LastIndexOf(',');
		if (num >= 0)
		{
			dataSource = dataSource.Substring(0, num);
		}
		num = dataSource.LastIndexOf('\\');
		if (num >= 0)
		{
			dataSource = dataSource.Substring(0, num);
		}
		dataSource = dataSource.Trim();
		for (num = 0; num < AzureSqlServerEndpoints.Length; num++)
		{
			if (dataSource.EndsWith(AzureSqlServerEndpoints[num], StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
