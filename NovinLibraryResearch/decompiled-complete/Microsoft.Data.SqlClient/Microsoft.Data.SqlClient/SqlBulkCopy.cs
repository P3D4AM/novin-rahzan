using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlBulkCopy : IDisposable
{
	private enum TableNameComponents
	{
		Server,
		Catalog,
		Owner,
		TableName
	}

	private enum ValueSourceType
	{
		Unspecified,
		IDataReader,
		DataTable,
		RowArray,
		DbDataReader
	}

	private enum ValueMethod : byte
	{
		GetValue,
		SqlTypeSqlDecimal,
		SqlTypeSqlDouble,
		SqlTypeSqlSingle,
		DataFeedStream,
		DataFeedText,
		DataFeedXml
	}

	private readonly struct SourceColumnMetadata
	{
		public readonly ValueMethod Method;

		public readonly bool IsSqlType;

		public readonly bool IsDataFeed;

		public SourceColumnMetadata(ValueMethod method, bool isSqlType, bool isDataFeed)
		{
			Method = method;
			IsSqlType = isSqlType;
			IsDataFeed = isDataFeed;
		}
	}

	private const int TranCountResultId = 0;

	private const int TranCountRowId = 0;

	private const int TranCountValueId = 0;

	private const int MetaDataResultId = 1;

	private const int CollationResultId = 2;

	private const int ColIdId = 0;

	private const int NameId = 1;

	private const int Tds_CollationId = 2;

	private const int CollationId = 3;

	private const int MAX_LENGTH = int.MaxValue;

	private const int DefaultCommandTimeout = 30;

	private bool _enableStreaming;

	private int _batchSize;

	private readonly bool _ownConnection;

	private readonly SqlBulkCopyOptions _copyOptions;

	private int _timeout = 30;

	private string _destinationTableName;

	private int _rowsCopied;

	private int _notifyAfter;

	private int _rowsUntilNotification;

	private bool _insideRowsCopiedEvent;

	private object _rowSource;

	private SqlDataReader _sqlDataReaderRowSource;

	private bool _rowSourceIsSqlDataReaderSmi;

	private DbDataReader _dbDataReaderRowSource;

	private DataTable _dataTableSource;

	private SqlBulkCopyColumnMappingCollection _columnMappings;

	private SqlBulkCopyColumnMappingCollection _localColumnMappings;

	private SqlConnection _connection;

	private SqlTransaction _internalTransaction;

	private SqlTransaction _externalTransaction;

	private ValueSourceType _rowSourceType;

	private DataRow _currentRow;

	private int _currentRowLength;

	private DataRowState _rowStateToSkip;

	private IEnumerator _rowEnumerator;

	private TdsParser _parser;

	private TdsParserStateObject _stateObj;

	private List<_ColumnMapping> _sortedColumnMappings;

	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private int _savedBatchSize;

	private bool _hasMoreRowToCopy;

	private bool _isAsyncBulkCopy;

	private bool _isBulkCopyingInProgress;

	private SqlInternalConnectionTds.SyncAsyncLock _parserLock;

	private SourceColumnMetadata[] _currentRowMetadata;

	private int RowNumber
	{
		get
		{
			int num;
			switch (_rowSourceType)
			{
			case ValueSourceType.RowArray:
				num = _dataTableSource.Rows.IndexOf(_rowEnumerator.Current as DataRow);
				break;
			case ValueSourceType.DataTable:
				num = ((DataTable)_rowSource).Rows.IndexOf(_rowEnumerator.Current as DataRow);
				break;
			default:
				return -1;
			}
			return ++num;
		}
	}

	public int BatchSize
	{
		get
		{
			return _batchSize;
		}
		set
		{
			if (value >= 0)
			{
				_batchSize = value;
				return;
			}
			throw ADP.ArgumentOutOfRange("BatchSize");
		}
	}

	public int BulkCopyTimeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			if (value < 0)
			{
				throw SQL.BulkLoadInvalidTimeout(value);
			}
			_timeout = value;
		}
	}

	public bool EnableStreaming
	{
		get
		{
			return _enableStreaming;
		}
		set
		{
			_enableStreaming = value;
		}
	}

	public SqlBulkCopyColumnMappingCollection ColumnMappings => _columnMappings;

	public SqlBulkCopyColumnOrderHintCollection ColumnOrderHints { get; }

	public string DestinationTableName
	{
		get
		{
			return _destinationTableName;
		}
		set
		{
			if (value == null)
			{
				throw ADP.ArgumentNull("DestinationTableName");
			}
			if (value.Length == 0)
			{
				throw ADP.ArgumentOutOfRange("DestinationTableName");
			}
			_destinationTableName = value;
		}
	}

	public int NotifyAfter
	{
		get
		{
			return _notifyAfter;
		}
		set
		{
			if (value >= 0)
			{
				_notifyAfter = value;
				return;
			}
			throw ADP.ArgumentOutOfRange("NotifyAfter");
		}
	}

	internal int ObjectID => _objectID;

	public int RowsCopied => _rowsCopied;

	internal SqlStatistics Statistics
	{
		get
		{
			if (_connection != null && _connection.StatisticsEnabled)
			{
				return _connection.Statistics;
			}
			return null;
		}
	}

	public event SqlRowsCopiedEventHandler SqlRowsCopied;

	public SqlBulkCopy(SqlConnection connection)
	{
		if (connection == null)
		{
			throw ADP.ArgumentNull("connection");
		}
		_connection = connection;
		_columnMappings = new SqlBulkCopyColumnMappingCollection();
		ColumnOrderHints = new SqlBulkCopyColumnOrderHintCollection();
	}

	public SqlBulkCopy(SqlConnection connection, SqlBulkCopyOptions copyOptions, SqlTransaction externalTransaction)
		: this(connection)
	{
		_copyOptions = copyOptions;
		if (externalTransaction != null && IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
		{
			throw SQL.BulkLoadConflictingTransactionOption();
		}
		if (!IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
		{
			_externalTransaction = externalTransaction;
		}
	}

	public SqlBulkCopy(string connectionString)
	{
		if (connectionString == null)
		{
			throw ADP.ArgumentNull("connectionString");
		}
		_connection = new SqlConnection(connectionString);
		_columnMappings = new SqlBulkCopyColumnMappingCollection();
		ColumnOrderHints = new SqlBulkCopyColumnOrderHintCollection();
		_ownConnection = true;
	}

	public SqlBulkCopy(string connectionString, SqlBulkCopyOptions copyOptions)
		: this(connectionString)
	{
		_copyOptions = copyOptions;
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private bool IsCopyOption(SqlBulkCopyOptions copyOption)
	{
		return (_copyOptions & copyOption) == copyOption;
	}

	private string CreateInitialQuery()
	{
		string[] array;
		try
		{
			array = MultipartIdentifier.ParseMultipartIdentifier(DestinationTableName, "[\"", "]\"", Strings.SQL_BulkCopyDestinationTableName, ThrowOnEmptyMultipartName: true);
		}
		catch (Exception inner)
		{
			throw SQL.BulkLoadInvalidDestinationTable(DestinationTableName, inner);
		}
		if (ADP.IsEmpty(array[3]))
		{
			throw SQL.BulkLoadInvalidDestinationTable(DestinationTableName, null);
		}
		string text = "select @@trancount; SET FMTONLY ON select * from " + ADP.BuildMultiPartName(array) + " SET FMTONLY OFF ";
		if (_connection.IsShiloh)
		{
			string text2 = (_connection.IsKatmaiOrNewer ? "sp_tablecollations_100" : ((!_connection.IsYukonOrNewer) ? "sp_tablecollations" : "sp_tablecollations_90"));
			string text3 = array[3];
			bool flag = text3.Length > 0 && '#' == text3[0];
			if (!ADP.IsEmpty(text3))
			{
				text3 = SqlServerEscapeHelper.EscapeStringAsLiteral(text3);
				text3 = SqlServerEscapeHelper.EscapeIdentifier(text3);
			}
			string text4 = array[2];
			if (!ADP.IsEmpty(text4))
			{
				text4 = SqlServerEscapeHelper.EscapeStringAsLiteral(text4);
				text4 = SqlServerEscapeHelper.EscapeIdentifier(text4);
			}
			string text5 = array[1];
			if (flag && ADP.IsEmpty(text5))
			{
				text += $"exec tempdb..{text2} N'{text4}.{text3}'";
			}
			else
			{
				if (!ADP.IsEmpty(text5))
				{
					text5 = SqlServerEscapeHelper.EscapeIdentifier(text5);
				}
				text += $"exec {text5}..{text2} N'{text4}.{text3}'";
			}
		}
		return text;
	}

	private Task<BulkCopySimpleResultSet> CreateAndExecuteInitialQueryAsync(out BulkCopySimpleResultSet result)
	{
		string text = CreateInitialQuery();
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlBulkCopy.CreateAndExecuteInitialQueryAsync|INFO> Initial Query: '{0}'", text);
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlBulkCopy.CreateAndExecuteInitialQueryAsync|Info|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		Task task = _parser.TdsExecuteSQLBatch(text, BulkCopyTimeout, null, _stateObj, !_isAsyncBulkCopy, callerHasConnectionLock: true);
		if (task == null)
		{
			result = new BulkCopySimpleResultSet();
			RunParser(result);
			return null;
		}
		result = null;
		return task.ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				throw t.Exception.InnerException;
			}
			BulkCopySimpleResultSet bulkCopySimpleResultSet = new BulkCopySimpleResultSet();
			RunParserReliably(bulkCopySimpleResultSet);
			return bulkCopySimpleResultSet;
		}, TaskScheduler.Default);
	}

	private string AnalyzeTargetAndCreateUpdateBulkCommand(BulkCopySimpleResultSet internalResults)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_connection.IsShiloh && internalResults[2].Count == 0)
		{
			throw SQL.BulkLoadNoCollation();
		}
		string[] strings = MultipartIdentifier.ParseMultipartIdentifier(DestinationTableName, "[\"", "]\"", Strings.SQL_BulkCopyDestinationTableName, ThrowOnEmptyMultipartName: true);
		stringBuilder.AppendFormat("insert bulk {0} (", ADP.BuildMultiPartName(strings));
		int num = 0;
		int num2 = 0;
		bool flag = ((!_parser.IsYukonOrNewer) ? ((bool)(0 < (SqlInt32)internalResults[0][0][0])) : _connection.HasLocalTransaction);
		if (flag && _externalTransaction == null && _internalTransaction == null && _connection.Parser != null && _connection.Parser.CurrentTransaction != null && _connection.Parser.CurrentTransaction.IsLocal)
		{
			throw SQL.BulkLoadExistingTransaction();
		}
		HashSet<string> hashSet = new HashSet<string>();
		_SqlMetaDataSet metaData = internalResults[1].MetaData;
		_sortedColumnMappings = new List<_ColumnMapping>(metaData.Length);
		for (int i = 0; i < metaData.Length; i++)
		{
			_SqlMetaData sqlMetaData = metaData[i];
			bool flag2 = false;
			if (sqlMetaData.type == SqlDbType.Timestamp || (sqlMetaData.IsIdentity && !IsCopyOption(SqlBulkCopyOptions.KeepIdentity)))
			{
				metaData[i] = null;
				flag2 = true;
			}
			int j;
			for (j = 0; j < _localColumnMappings.Count; j++)
			{
				if (_localColumnMappings[j]._destinationColumnOrdinal != sqlMetaData.ordinal && !(UnquotedName(_localColumnMappings[j]._destinationColumnName) == sqlMetaData.column))
				{
					continue;
				}
				if (flag2)
				{
					num2++;
					break;
				}
				_sortedColumnMappings.Add(new _ColumnMapping(_localColumnMappings[j]._internalSourceColumnOrdinal, sqlMetaData));
				hashSet.Add(sqlMetaData.column);
				num++;
				if (num > 1)
				{
					stringBuilder.Append(", ");
				}
				if (sqlMetaData.type == SqlDbType.Variant)
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "sql_variant");
				}
				else if (sqlMetaData.type == SqlDbType.Udt)
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "varbinary");
				}
				else
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, typeof(SqlDbType).GetEnumName(sqlMetaData.type));
				}
				switch (sqlMetaData.metaType.NullableType)
				{
				case 106:
				case 108:
					stringBuilder.AppendFormat(null, "({0},{1})", sqlMetaData.precision, sqlMetaData.scale);
					break;
				case 240:
				{
					if (sqlMetaData.IsLargeUdt)
					{
						stringBuilder.Append("(max)");
						break;
					}
					int length = sqlMetaData.length;
					stringBuilder.AppendFormat(null, "({0})", length);
					break;
				}
				case 41:
				case 42:
				case 43:
					stringBuilder.AppendFormat(null, "({0})", sqlMetaData.scale);
					break;
				default:
					if (!sqlMetaData.metaType.IsFixed && !sqlMetaData.metaType.IsLong)
					{
						int num3 = sqlMetaData.length;
						byte nullableType = sqlMetaData.metaType.NullableType;
						if (nullableType == 99 || nullableType == 231 || nullableType == 239)
						{
							num3 /= 2;
						}
						stringBuilder.AppendFormat(null, "({0})", num3);
					}
					else if (sqlMetaData.metaType.IsPlp && sqlMetaData.metaType.SqlDbType != SqlDbType.Xml)
					{
						stringBuilder.Append("(max)");
					}
					break;
				}
				if (!_connection.IsShiloh)
				{
					break;
				}
				Result result = internalResults[2];
				object obj = result[i][3];
				bool flag3;
				switch (sqlMetaData.type)
				{
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					flag3 = true;
					break;
				default:
					flag3 = false;
					break;
				}
				if (!(obj != null && flag3))
				{
					break;
				}
				SqlString sqlString = (SqlString)obj;
				if (sqlString.IsNull)
				{
					break;
				}
				stringBuilder.Append(" COLLATE " + sqlString.Value);
				if (_sqlDataReaderRowSource == null || sqlMetaData.collation == null)
				{
					break;
				}
				int internalSourceColumnOrdinal = _localColumnMappings[j]._internalSourceColumnOrdinal;
				int lCID = sqlMetaData.collation.LCID;
				int localeId = _sqlDataReaderRowSource.GetLocaleId(internalSourceColumnOrdinal);
				if (localeId == lCID)
				{
					break;
				}
				throw SQL.BulkLoadLcidMismatch(localeId, _sqlDataReaderRowSource.GetName(internalSourceColumnOrdinal), lCID, sqlMetaData.column);
			}
			if (j == _localColumnMappings.Count)
			{
				metaData[i] = null;
			}
		}
		if (num + num2 != _localColumnMappings.Count)
		{
			throw SQL.BulkLoadNonMatchingColumnMapping();
		}
		stringBuilder.Append(")");
		if ((_copyOptions & (SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.AllowEncryptedValueModifications)) != 0 || ColumnOrderHints.Count > 0)
		{
			bool flag4 = false;
			stringBuilder.Append(" with (");
			if (IsCopyOption(SqlBulkCopyOptions.KeepNulls))
			{
				stringBuilder.Append("KEEP_NULLS");
				flag4 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.TableLock))
			{
				stringBuilder.Append((flag4 ? ", " : "") + "TABLOCK");
				flag4 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.CheckConstraints))
			{
				stringBuilder.Append((flag4 ? ", " : "") + "CHECK_CONSTRAINTS");
				flag4 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.FireTriggers))
			{
				stringBuilder.Append((flag4 ? ", " : "") + "FIRE_TRIGGERS");
				flag4 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.AllowEncryptedValueModifications))
			{
				stringBuilder.Append((flag4 ? ", " : "") + "ALLOW_ENCRYPTED_VALUE_MODIFICATIONS");
				flag4 = true;
			}
			if (ColumnOrderHints.Count > 0)
			{
				stringBuilder.Append((flag4 ? ", " : "") + TryGetOrderHintText(hashSet));
			}
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	private string TryGetOrderHintText(HashSet<string> destColumnNames)
	{
		StringBuilder stringBuilder = new StringBuilder("ORDER(");
		foreach (SqlBulkCopyColumnOrderHint columnOrderHint in ColumnOrderHints)
		{
			string column = columnOrderHint.Column;
			if (!destColumnNames.Contains(column))
			{
				throw SQL.BulkLoadOrderHintInvalidColumn(column);
			}
			if (!string.IsNullOrEmpty(column))
			{
				string text = SqlServerEscapeHelper.EscapeIdentifier(SqlServerEscapeHelper.EscapeStringAsLiteral(column));
				string text2 = ((columnOrderHint.SortOrder == SortOrder.Descending) ? "DESC" : "ASC");
				stringBuilder.Append(text + " " + text2 + ", ");
			}
		}
		stringBuilder.Length -= 2;
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	private Task SubmitUpdateBulkCommand(string TDSCommand)
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlBulkCopy.SubmitUpdateBulkCommand|Info|Correlation> ObjectID{0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		Task task = _parser.TdsExecuteSQLBatch(TDSCommand, BulkCopyTimeout, null, _stateObj, !_isAsyncBulkCopy, callerHasConnectionLock: true);
		if (task == null)
		{
			RunParser();
			return null;
		}
		return task.ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				throw t.Exception.InnerException;
			}
			RunParserReliably();
		}, TaskScheduler.Default);
	}

	private void WriteMetaData(BulkCopySimpleResultSet internalResults)
	{
		_stateObj.SetTimeoutSeconds(BulkCopyTimeout);
		_SqlMetaDataSet metaData = internalResults[1].MetaData;
		_stateObj._outputMessageType = 7;
		_parser.WriteBulkCopyMetaData(metaData, _sortedColumnMappings.Count, _stateObj);
	}

	public void Close()
	{
		if (_insideRowsCopiedEvent)
		{
			throw SQL.InvalidOperationInsideEvent();
		}
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		_columnMappings = null;
		_parser = null;
		try
		{
			if (_internalTransaction != null)
			{
				_internalTransaction.Rollback();
				_internalTransaction.Dispose();
				_internalTransaction = null;
			}
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			ADP.TraceExceptionWithoutRethrow(e);
		}
		finally
		{
			if (_connection != null)
			{
				if (_ownConnection)
				{
					_connection.Dispose();
				}
				_connection = null;
			}
		}
	}

	private object GetValueFromSourceRow(int destRowIndex, out bool isSqlType, out bool isDataFeed, out bool isNull)
	{
		_SqlMetaData metadata = _sortedColumnMappings[destRowIndex]._metadata;
		int sourceColumnOrdinal = _sortedColumnMappings[destRowIndex]._sourceColumnOrdinal;
		switch (_rowSourceType)
		{
		case ValueSourceType.IDataReader:
		case ValueSourceType.DbDataReader:
		{
			if (_currentRowMetadata[destRowIndex].IsDataFeed)
			{
				if (_dbDataReaderRowSource.IsDBNull(sourceColumnOrdinal))
				{
					isSqlType = false;
					isDataFeed = false;
					isNull = true;
					return DBNull.Value;
				}
				isSqlType = false;
				isDataFeed = true;
				isNull = false;
				switch (_currentRowMetadata[destRowIndex].Method)
				{
				case ValueMethod.DataFeedStream:
					return new StreamDataFeed(_dbDataReaderRowSource.GetStream(sourceColumnOrdinal));
				case ValueMethod.DataFeedText:
					return new TextDataFeed(_dbDataReaderRowSource.GetTextReader(sourceColumnOrdinal));
				case ValueMethod.DataFeedXml:
					return new XmlDataFeed(_sqlDataReaderRowSource.GetXmlReader(sourceColumnOrdinal));
				default:
				{
					isDataFeed = false;
					object value = _dbDataReaderRowSource.GetValue(sourceColumnOrdinal);
					ADP.IsNullOrSqlType(value, out isNull, out isSqlType);
					return value;
				}
				}
			}
			if (_sqlDataReaderRowSource != null)
			{
				if (_currentRowMetadata[destRowIndex].IsSqlType)
				{
					isSqlType = true;
					isDataFeed = false;
					INullable nullable = _currentRowMetadata[destRowIndex].Method switch
					{
						ValueMethod.SqlTypeSqlDecimal => _sqlDataReaderRowSource.GetSqlDecimal(sourceColumnOrdinal), 
						ValueMethod.SqlTypeSqlDouble => (SqlDecimal)_sqlDataReaderRowSource.GetSqlDouble(sourceColumnOrdinal), 
						ValueMethod.SqlTypeSqlSingle => (SqlDecimal)_sqlDataReaderRowSource.GetSqlSingle(sourceColumnOrdinal), 
						_ => (INullable)_sqlDataReaderRowSource.GetSqlValue(sourceColumnOrdinal), 
					};
					isNull = nullable.IsNull;
					return nullable;
				}
				isSqlType = false;
				isDataFeed = false;
				object value2 = _sqlDataReaderRowSource.GetValue(sourceColumnOrdinal);
				isNull = value2 == null || value2 == DBNull.Value;
				if (!isNull && metadata.type == SqlDbType.Udt)
				{
					isNull = (value2 as INullable)?.IsNull ?? false;
				}
				return value2;
			}
			isDataFeed = false;
			IDataReader dataReader = (IDataReader)_rowSource;
			if (_enableStreaming && _sqlDataReaderRowSource == null && dataReader.IsDBNull(sourceColumnOrdinal))
			{
				isSqlType = false;
				isNull = true;
				return DBNull.Value;
			}
			object value3 = dataReader.GetValue(sourceColumnOrdinal);
			ADP.IsNullOrSqlType(value3, out isNull, out isSqlType);
			return value3;
		}
		case ValueSourceType.DataTable:
		case ValueSourceType.RowArray:
		{
			isDataFeed = false;
			object obj = _currentRow[sourceColumnOrdinal];
			ADP.IsNullOrSqlType(obj, out isNull, out isSqlType);
			if (!isNull && _currentRowMetadata[destRowIndex].IsSqlType)
			{
				switch (_currentRowMetadata[destRowIndex].Method)
				{
				case ValueMethod.SqlTypeSqlSingle:
				{
					if (isSqlType)
					{
						return new SqlDecimal(((SqlSingle)obj).Value);
					}
					float num2 = (float)obj;
					if (!float.IsNaN(num2))
					{
						isSqlType = true;
						return new SqlDecimal(num2);
					}
					break;
				}
				case ValueMethod.SqlTypeSqlDouble:
				{
					if (isSqlType)
					{
						return new SqlDecimal(((SqlDouble)obj).Value);
					}
					double num = (double)obj;
					if (!double.IsNaN(num))
					{
						isSqlType = true;
						return new SqlDecimal(num);
					}
					break;
				}
				case ValueMethod.SqlTypeSqlDecimal:
					if (isSqlType)
					{
						return (SqlDecimal)obj;
					}
					isSqlType = true;
					return new SqlDecimal((decimal)obj);
				}
			}
			return obj;
		}
		default:
			throw ADP.NotSupported();
		}
	}

	private Task ReadFromRowSourceAsync(CancellationToken cts)
	{
		if (_isAsyncBulkCopy && _dbDataReaderRowSource != null)
		{
			return _dbDataReaderRowSource.ReadAsync(cts).ContinueWith(delegate(Task<bool> t)
			{
				if (t.Status == TaskStatus.RanToCompletion)
				{
					_hasMoreRowToCopy = t.Result;
				}
				return t;
			}, TaskScheduler.Default).Unwrap();
		}
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		bool canBeReleasedFromAnyThread = openTdsConnection._parserLock.CanBeReleasedFromAnyThread;
		openTdsConnection._parserLock.Release();
		_hasMoreRowToCopy = false;
		try
		{
			_hasMoreRowToCopy = ReadFromRowSource();
		}
		catch (Exception exception)
		{
			if (_isAsyncBulkCopy)
			{
				return Task.FromException<bool>(exception);
			}
			throw;
		}
		finally
		{
			openTdsConnection._parserLock.Wait(canBeReleasedFromAnyThread);
		}
		return null;
	}

	private bool ReadFromRowSource()
	{
		switch (_rowSourceType)
		{
		case ValueSourceType.IDataReader:
		case ValueSourceType.DbDataReader:
			return ((IDataReader)_rowSource).Read();
		case ValueSourceType.DataTable:
		case ValueSourceType.RowArray:
			do
			{
				if (!_rowEnumerator.MoveNext())
				{
					return false;
				}
				_currentRow = (DataRow)_rowEnumerator.Current;
			}
			while ((_currentRow.RowState & _rowStateToSkip) != 0);
			_currentRowLength = _currentRow.ItemArray.Length;
			return true;
		default:
			throw ADP.NotSupported();
		}
	}

	private SourceColumnMetadata GetColumnMetadata(int ordinal)
	{
		int sourceColumnOrdinal = _sortedColumnMappings[ordinal]._sourceColumnOrdinal;
		_SqlMetaData metadata = _sortedColumnMappings[ordinal]._metadata;
		bool isDataFeed;
		bool isSqlType;
		ValueMethod method;
		if ((_sqlDataReaderRowSource != null || _dataTableSource != null) && (metadata.metaType.NullableType == 106 || metadata.metaType.NullableType == 108))
		{
			isDataFeed = false;
			Type type;
			switch (_rowSourceType)
			{
			case ValueSourceType.IDataReader:
			case ValueSourceType.DbDataReader:
				type = _sqlDataReaderRowSource.GetFieldType(sourceColumnOrdinal);
				break;
			case ValueSourceType.DataTable:
			case ValueSourceType.RowArray:
				type = _dataTableSource.Columns[sourceColumnOrdinal].DataType;
				break;
			default:
				type = null;
				break;
			}
			if (typeof(SqlDecimal) == type || typeof(decimal) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlDecimal;
			}
			else if (typeof(SqlDouble) == type || typeof(double) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlDouble;
			}
			else if (typeof(SqlSingle) == type || typeof(float) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlSingle;
			}
			else
			{
				isSqlType = false;
				method = ValueMethod.GetValue;
			}
		}
		else if (_enableStreaming && metadata.length == int.MaxValue && !_rowSourceIsSqlDataReaderSmi)
		{
			isSqlType = false;
			if (_sqlDataReaderRowSource != null)
			{
				MetaType metaType = _sqlDataReaderRowSource.MetaData[sourceColumnOrdinal].metaType;
				if (metadata.type == SqlDbType.VarBinary && metaType.IsBinType && metaType.SqlDbType != SqlDbType.Timestamp && _sqlDataReaderRowSource.IsCommandBehavior(CommandBehavior.SequentialAccess))
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedStream;
				}
				else if ((metadata.type == SqlDbType.VarChar || metadata.type == SqlDbType.NVarChar) && metaType.IsCharType && metaType.SqlDbType != SqlDbType.Xml)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedText;
				}
				else if (metadata.type == SqlDbType.Xml && metaType.SqlDbType == SqlDbType.Xml)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedXml;
				}
				else
				{
					isDataFeed = false;
					method = ValueMethod.GetValue;
				}
			}
			else if (_dbDataReaderRowSource != null)
			{
				if (metadata.type == SqlDbType.VarBinary)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedStream;
				}
				else if (metadata.type == SqlDbType.VarChar || metadata.type == SqlDbType.NVarChar)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedText;
				}
				else
				{
					isDataFeed = false;
					method = ValueMethod.GetValue;
				}
			}
			else
			{
				isDataFeed = false;
				method = ValueMethod.GetValue;
			}
		}
		else
		{
			isSqlType = false;
			isDataFeed = false;
			method = ValueMethod.GetValue;
		}
		return new SourceColumnMetadata(method, isSqlType, isDataFeed);
	}

	private void CreateOrValidateConnection(string method)
	{
		if (_connection == null)
		{
			throw ADP.ConnectionRequired(method);
		}
		if (_connection.IsContextConnection)
		{
			throw SQL.NotAvailableOnContextConnection();
		}
		if (_ownConnection && _connection.State != ConnectionState.Open)
		{
			_connection.Open();
		}
		_connection.ValidateConnectionForExecute(method, null);
		if (_externalTransaction != null && _connection != _externalTransaction.Connection)
		{
			throw ADP.TransactionConnectionMismatch();
		}
	}

	private void RunParser(BulkCopySimpleResultSet bulkCopyHandler = null)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		openTdsConnection.ThreadHasParserLockForClose = true;
		try
		{
			_parser.Run(RunBehavior.UntilDone, null, null, bulkCopyHandler, _stateObj);
		}
		finally
		{
			openTdsConnection.ThreadHasParserLockForClose = false;
		}
	}

	private void RunParserReliably(BulkCopySimpleResultSet bulkCopyHandler = null)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		openTdsConnection.ThreadHasParserLockForClose = true;
		try
		{
			_parser.RunReliably(RunBehavior.UntilDone, null, null, bulkCopyHandler, _stateObj);
		}
		finally
		{
			openTdsConnection.ThreadHasParserLockForClose = false;
		}
	}

	private void CommitTransaction()
	{
		if (_internalTransaction != null)
		{
			SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_internalTransaction.Commit();
				_internalTransaction.Dispose();
				_internalTransaction = null;
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
		}
	}

	private void AbortTransaction()
	{
		if (_internalTransaction == null)
		{
			return;
		}
		if (!_internalTransaction.IsZombied)
		{
			SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_internalTransaction.Rollback();
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
		}
		_internalTransaction.Dispose();
		_internalTransaction = null;
	}

	private void AppendColumnNameAndTypeName(StringBuilder query, string columnName, string typeName)
	{
		SqlServerEscapeHelper.EscapeIdentifier(query, columnName);
		query.Append(" ");
		query.Append(typeName);
	}

	private string UnquotedName(string name)
	{
		if (ADP.IsEmpty(name))
		{
			return null;
		}
		if (name[0] == '[')
		{
			int length = name.Length;
			name = name.Substring(1, length - 2);
		}
		return name;
	}

	private object ValidateBulkCopyVariant(object value)
	{
		MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
		switch (metaTypeFromValue.TDSType)
		{
		case 36:
		case 40:
		case 41:
		case 42:
		case 43:
		case 48:
		case 50:
		case 52:
		case 56:
		case 59:
		case 60:
		case 61:
		case 62:
		case 108:
		case 127:
		case 165:
		case 167:
		case 231:
			if (value is INullable)
			{
				return MetaType.GetComValueFromSqlVariant(value);
			}
			return value;
		default:
			throw SQL.BulkLoadInvalidVariantValue();
		}
	}

	private object ConvertValue(object value, _SqlMetaData metadata, bool isNull, ref bool isSqlType, out bool coercedToDataFeed)
	{
		coercedToDataFeed = false;
		if (isNull)
		{
			if (!metadata.IsNullable)
			{
				throw SQL.BulkLoadBulkLoadNotAllowDBNull(metadata.column);
			}
			return value;
		}
		MetaType metaType = metadata.metaType;
		bool typeChanged = false;
		byte scale = metadata.scale;
		byte precision = metadata.precision;
		int length = metadata.length;
		if (metadata.isEncrypted)
		{
			metaType = metadata.baseTI.metaType;
			scale = metadata.baseTI.scale;
			precision = metadata.baseTI.precision;
			length = metadata.baseTI.length;
		}
		try
		{
			switch (metaType.NullableType)
			{
			case 106:
			case 108:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				SqlDecimal sqlDecimal = ((!isSqlType || typeChanged) ? new SqlDecimal((decimal)value) : ((SqlDecimal)value));
				if (sqlDecimal.Scale != scale)
				{
					sqlDecimal = TdsParser.AdjustSqlDecimalScale(sqlDecimal, scale);
				}
				if (sqlDecimal.Precision > precision)
				{
					try
					{
						sqlDecimal = SqlDecimal.ConvertToPrecScale(sqlDecimal, precision, sqlDecimal.Scale);
					}
					catch (SqlTruncateException)
					{
						throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaTypeFromSqlDbType, metadata.ordinal, RowNumber, metadata.isEncrypted, metadata.column, value.ToString(), ADP.ParameterValueOutOfRange(sqlDecimal));
					}
					catch (Exception e)
					{
						throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaTypeFromSqlDbType, metadata.ordinal, RowNumber, metadata.isEncrypted, metadata.column, value.ToString(), e);
					}
				}
				value = sqlDecimal;
				isSqlType = true;
				typeChanged = false;
				break;
			}
			case 34:
			case 35:
			case 36:
			case 38:
			case 40:
			case 41:
			case 42:
			case 43:
			case 50:
			case 58:
			case 59:
			case 61:
			case 62:
			case 104:
			case 109:
			case 110:
			case 111:
			case 165:
			case 167:
			case 173:
			case 175:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				break;
			}
			case 99:
			case 231:
			case 239:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				if (!coercedToDataFeed)
				{
					string text = ((isSqlType && !typeChanged) ? ((SqlString)value).Value : ((string)value));
					int num = length / 2;
					if (text.Length > num)
					{
						throw SQL.BulkLoadStringTooLong(truncatedValue: (!metadata.isEncrypted) ? text.Remove(Math.Min(num, 100)) : "<encrypted>", tableName: _destinationTableName, columnName: metadata.column);
					}
				}
				break;
			}
			case 98:
				value = ValidateBulkCopyVariant(value);
				typeChanged = true;
				break;
			case 240:
				if (!(value is byte[]))
				{
					value = _connection.GetBytes(value);
					typeChanged = true;
				}
				break;
			case 241:
				if (value is XmlReader)
				{
					value = new XmlDataFeed((XmlReader)value);
					typeChanged = true;
					coercedToDataFeed = true;
				}
				break;
			default:
				throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaType, metadata.ordinal, RowNumber, metadata.isEncrypted, metadata.column, value.ToString(), null);
			}
			if (typeChanged)
			{
				isSqlType = false;
			}
			return value;
		}
		catch (Exception e2)
		{
			if (!ADP.IsCatchableExceptionType(e2))
			{
				throw;
			}
			throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaType, metadata.ordinal, RowNumber, metadata.isEncrypted, metadata.column, value.ToString(), e2);
		}
	}

	public void WriteToServer(DbDataReader reader)
	{
		SqlConnection.ExecutePermission.Demand();
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_dbDataReaderRowSource = reader;
			_sqlDataReaderRowSource = reader as SqlDataReader;
			if (_sqlDataReaderRowSource != null)
			{
				_rowSourceIsSqlDataReaderSmi = _sqlDataReaderRowSource is SqlDataReaderSmi;
			}
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.DbDataReader;
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(reader.FieldCount, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(IDataReader reader)
	{
		SqlConnection.ExecutePermission.Demand();
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_sqlDataReaderRowSource = _rowSource as SqlDataReader;
			if (_sqlDataReaderRowSource != null)
			{
				_rowSourceIsSqlDataReaderSmi = _sqlDataReaderRowSource is SqlDataReaderSmi;
			}
			_dbDataReaderRowSource = _rowSource as DbDataReader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.IDataReader;
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(reader.FieldCount, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(DataTable table)
	{
		WriteToServer(table, (DataRowState)0);
	}

	public void WriteToServer(DataTable table, DataRowState rowState)
	{
		SqlConnection.ExecutePermission.Demand();
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowStateToSkip = ((rowState == (DataRowState)0 || rowState == DataRowState.Deleted) ? DataRowState.Deleted : (~rowState | DataRowState.Deleted));
			_rowSource = table;
			_dataTableSource = table;
			_sqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.DataTable;
			_rowEnumerator = table.Rows.GetEnumerator();
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(table.Columns.Count, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(DataRow[] rows)
	{
		SqlConnection.ExecutePermission.Demand();
		SqlStatistics statistics = Statistics;
		if (rows == null)
		{
			throw new ArgumentNullException("rows");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		if (rows.Length == 0)
		{
			return;
		}
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			DataTable table = rows[0].Table;
			_rowStateToSkip = DataRowState.Deleted;
			_rowSource = rows;
			_dataTableSource = table;
			_sqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.RowArray;
			_rowEnumerator = rows.GetEnumerator();
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(table.Columns.Count, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DataRow[] rows)
	{
		return WriteToServerAsync(rows, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataRow[] rows, CancellationToken cancellationToken)
	{
		Task task = null;
		SqlConnection.ExecutePermission.Demand();
		if (rows == null)
		{
			throw new ArgumentNullException("rows");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			if (rows.Length == 0)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				if (cancellationToken.IsCancellationRequested)
				{
					taskCompletionSource.SetCanceled();
				}
				else
				{
					taskCompletionSource.SetResult(null);
				}
				return taskCompletionSource.Task;
			}
			DataTable table = rows[0].Table;
			_rowStateToSkip = DataRowState.Deleted;
			_rowSource = rows;
			_dataTableSource = table;
			_sqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.RowArray;
			_rowEnumerator = rows.GetEnumerator();
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(table.Columns.Count, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DbDataReader reader)
	{
		return WriteToServerAsync(reader, CancellationToken.None);
	}

	public Task WriteToServerAsync(DbDataReader reader, CancellationToken cancellationToken)
	{
		Task task = null;
		SqlConnection.ExecutePermission.Demand();
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_sqlDataReaderRowSource = reader as SqlDataReader;
			_dbDataReaderRowSource = reader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.DbDataReader;
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(reader.FieldCount, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(IDataReader reader)
	{
		return WriteToServerAsync(reader, CancellationToken.None);
	}

	public Task WriteToServerAsync(IDataReader reader, CancellationToken cancellationToken)
	{
		Task task = null;
		SqlConnection.ExecutePermission.Demand();
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_sqlDataReaderRowSource = _rowSource as SqlDataReader;
			_dbDataReaderRowSource = _rowSource as DbDataReader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.IDataReader;
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(reader.FieldCount, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DataTable table)
	{
		return WriteToServerAsync(table, (DataRowState)0, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataTable table, CancellationToken cancellationToken)
	{
		return WriteToServerAsync(table, (DataRowState)0, cancellationToken);
	}

	public Task WriteToServerAsync(DataTable table, DataRowState rowState)
	{
		return WriteToServerAsync(table, rowState, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataTable table, DataRowState rowState, CancellationToken cancellationToken)
	{
		Task task = null;
		SqlConnection.ExecutePermission.Demand();
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowStateToSkip = ((rowState == (DataRowState)0 || rowState == DataRowState.Deleted) ? DataRowState.Deleted : (~rowState | DataRowState.Deleted));
			_rowSource = table;
			_sqlDataReaderRowSource = null;
			_dataTableSource = table;
			_rowSourceType = ValueSourceType.DataTable;
			_rowEnumerator = table.Rows.GetEnumerator();
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(table.Columns.Count, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private Task WriteRowSourceToServerAsync(int columnCount, CancellationToken ctoken)
	{
		if (ctoken.IsCancellationRequested)
		{
			return ADP.CreatedTaskWithCancellation<object>();
		}
		Task currentReconnectionTask = _connection._currentReconnectionTask;
		if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
		{
			if (_isAsyncBulkCopy)
			{
				TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
				currentReconnectionTask.ContinueWith(delegate
				{
					Task task2 = WriteRowSourceToServerAsync(columnCount, ctoken);
					if (task2 == null)
					{
						tcs.SetResult(null);
					}
					else
					{
						AsyncHelper.ContinueTask(task2, tcs, delegate
						{
							tcs.SetResult(null);
						});
					}
				}, ctoken);
				return tcs.Task;
			}
			AsyncHelper.WaitForCompletion(currentReconnectionTask, BulkCopyTimeout, delegate
			{
				throw SQL.CR_ReconnectTimeout();
			}, rethrowExceptions: false);
		}
		bool flag = true;
		_isBulkCopyingInProgress = true;
		CreateOrValidateConnection("WriteToServer");
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		_parserLock = openTdsConnection._parserLock;
		_parserLock.Wait(_isAsyncBulkCopy);
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(_connection);
			WriteRowSourceToServerCommon(columnCount);
			Task task = WriteToServerInternalAsync(ctoken);
			if (task != null)
			{
				flag = false;
				return task.ContinueWith(delegate(Task t)
				{
					try
					{
						AbortTransaction();
						return t;
					}
					finally
					{
						_isBulkCopyingInProgress = false;
						if (_parser != null)
						{
							_parser._asyncWrite = false;
						}
						if (_parserLock != null)
						{
							_parserLock.Release();
							_parserLock = null;
						}
					}
				}, TaskScheduler.Default).Unwrap();
			}
			return null;
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
		finally
		{
			_columnMappings.ReadOnly = false;
			if (flag)
			{
				try
				{
					AbortTransaction();
				}
				finally
				{
					_isBulkCopyingInProgress = false;
					if (_parser != null)
					{
						_parser._asyncWrite = false;
					}
					if (_parserLock != null)
					{
						_parserLock.Release();
						_parserLock = null;
					}
				}
			}
		}
	}

	private void WriteRowSourceToServerCommon(int columnCount)
	{
		bool flag = false;
		_columnMappings.ReadOnly = true;
		_localColumnMappings = _columnMappings;
		if (_localColumnMappings.Count > 0)
		{
			_localColumnMappings.ValidateCollection();
			foreach (SqlBulkCopyColumnMapping localColumnMapping in _localColumnMappings)
			{
				if (localColumnMapping._internalSourceColumnOrdinal == -1)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			_localColumnMappings = new SqlBulkCopyColumnMappingCollection();
			_localColumnMappings.CreateDefaultMapping(columnCount);
		}
		if (!flag)
		{
			return;
		}
		int num = -1;
		flag = false;
		if (_localColumnMappings.Count <= 0)
		{
			return;
		}
		foreach (SqlBulkCopyColumnMapping localColumnMapping2 in _localColumnMappings)
		{
			if (localColumnMapping2._internalSourceColumnOrdinal != -1)
			{
				continue;
			}
			string text = UnquotedName(localColumnMapping2.SourceColumn);
			switch (_rowSourceType)
			{
			case ValueSourceType.DataTable:
				num = ((DataTable)_rowSource).Columns.IndexOf(text);
				break;
			case ValueSourceType.RowArray:
				num = ((DataRow[])_rowSource)[0].Table.Columns.IndexOf(text);
				break;
			case ValueSourceType.IDataReader:
			case ValueSourceType.DbDataReader:
				try
				{
					num = ((IDataRecord)_rowSource).GetOrdinal(text);
				}
				catch (IndexOutOfRangeException e)
				{
					throw SQL.BulkLoadNonMatchingColumnName(text, e);
				}
				break;
			}
			if (num == -1)
			{
				throw SQL.BulkLoadNonMatchingColumnName(text);
			}
			localColumnMapping2._internalSourceColumnOrdinal = num;
		}
	}

	internal void OnConnectionClosed()
	{
		_stateObj?.OnConnectionClosed();
	}

	private bool FireRowsCopiedEvent(long rowsCopied)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		bool canBeReleasedFromAnyThread = openTdsConnection._parserLock.CanBeReleasedFromAnyThread;
		openTdsConnection._parserLock.Release();
		SqlRowsCopiedEventArgs sqlRowsCopiedEventArgs = new SqlRowsCopiedEventArgs(rowsCopied);
		try
		{
			_insideRowsCopiedEvent = true;
			this.SqlRowsCopied?.Invoke(this, sqlRowsCopiedEventArgs);
		}
		finally
		{
			_insideRowsCopiedEvent = false;
			openTdsConnection._parserLock.Wait(canBeReleasedFromAnyThread);
		}
		return sqlRowsCopiedEventArgs.Abort;
	}

	private Task ReadWriteColumnValueAsync(int col)
	{
		bool isSqlType;
		bool isDataFeed;
		bool isNull;
		object obj = GetValueFromSourceRow(col, out isSqlType, out isDataFeed, out isNull);
		_SqlMetaData metadata = _sortedColumnMappings[col]._metadata;
		if (!isDataFeed)
		{
			obj = ConvertValue(obj, metadata, isNull, ref isSqlType, out isDataFeed);
			if (!isNull && metadata.isEncrypted)
			{
				obj = _parser.EncryptColumnValue(obj, metadata, metadata.column, _stateObj, isDataFeed, isSqlType);
				isSqlType = false;
			}
		}
		Task result = null;
		if (metadata.type != SqlDbType.Variant)
		{
			result = _parser.WriteBulkCopyValue(obj, metadata, _stateObj, isSqlType, isDataFeed, isNull);
		}
		else
		{
			SqlBuffer.StorageType storageType = SqlBuffer.StorageType.Empty;
			if (_sqlDataReaderRowSource != null && _connection.IsKatmaiOrNewer)
			{
				storageType = _sqlDataReaderRowSource.GetVariantInternalStorageType(_sortedColumnMappings[col]._sourceColumnOrdinal);
			}
			switch (storageType)
			{
			case SqlBuffer.StorageType.DateTime2:
				_parser.WriteSqlVariantDateTime2((DateTime)obj, _stateObj);
				break;
			case SqlBuffer.StorageType.Date:
				_parser.WriteSqlVariantDate((DateTime)obj, _stateObj);
				break;
			default:
				result = _parser.WriteSqlVariantDataRowValue(obj, _stateObj);
				break;
			}
		}
		return result;
	}

	private void RegisterForConnectionCloseNotification<T>(ref Task<T> outterTask)
	{
		SqlConnection connection = _connection;
		if (connection == null)
		{
			throw ADP.ClosedConnectionError();
		}
		connection.RegisterForConnectionCloseNotification(ref outterTask, this, 3);
	}

	private Task CopyColumnsAsync(int col, TaskCompletionSource<object> source = null)
	{
		Task result = null;
		Task task = null;
		try
		{
			int i;
			for (i = col; i < _sortedColumnMappings.Count; i++)
			{
				task = ReadWriteColumnValueAsync(i);
				if (task != null)
				{
					break;
				}
			}
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
					result = source.Task;
				}
				CopyColumnsAsyncSetupContinuation(source, task, i);
				return result;
			}
			source?.SetResult(null);
		}
		catch (Exception exception)
		{
			if (source == null)
			{
				throw;
			}
			source.TrySetException(exception);
		}
		return result;
	}

	private void CopyColumnsAsyncSetupContinuation(TaskCompletionSource<object> source, Task task, int i)
	{
		AsyncHelper.ContinueTask(task, source, delegate
		{
			if (i + 1 < _sortedColumnMappings.Count)
			{
				CopyColumnsAsync(i + 1, source);
			}
			else
			{
				source.SetResult(null);
			}
		}, _connection.GetOpenTdsConnection());
	}

	private void CheckAndRaiseNotification()
	{
		bool flag = false;
		Exception ex = null;
		_rowsCopied++;
		if (_notifyAfter > 0 && _rowsUntilNotification > 0 && --_rowsUntilNotification == 0)
		{
			try
			{
				_stateObj.BcpLock = true;
				flag = FireRowsCopiedEvent(_rowsCopied);
				SqlClientEventSource.Log.TryTraceEvent("<sc.SqlBulkCopy.WriteToServerInternal|INFO>");
				if (ConnectionState.Open != _connection.State)
				{
					ex = ADP.OpenConnectionRequired("CheckAndRaiseNotification", _connection.State);
				}
			}
			catch (Exception ex2)
			{
				ex = (ADP.IsCatchableExceptionType(ex2) ? OperationAbortedException.Aborted(ex2) : ex2);
			}
			finally
			{
				_stateObj.BcpLock = false;
			}
			if (!flag)
			{
				_rowsUntilNotification = _notifyAfter;
			}
		}
		if (!flag && _rowsUntilNotification > _notifyAfter)
		{
			_rowsUntilNotification = _notifyAfter;
		}
		if (ex == null && flag)
		{
			ex = OperationAbortedException.Aborted(null);
		}
		if (_connection.State != ConnectionState.Open)
		{
			throw ADP.OpenConnectionRequired("WriteToServer", _connection.State);
		}
		if (ex != null)
		{
			_parser._asyncWrite = false;
			Task task = _parser.WriteBulkCopyDone(_stateObj);
			RunParser();
			AbortTransaction();
			throw ex;
		}
	}

	private Task CheckForCancellation(CancellationToken cts, TaskCompletionSource<object> tcs)
	{
		if (cts.IsCancellationRequested)
		{
			if (tcs == null)
			{
				tcs = new TaskCompletionSource<object>();
			}
			tcs.SetCanceled();
			return tcs.Task;
		}
		return null;
	}

	private TaskCompletionSource<object> ContinueTaskPend(Task task, TaskCompletionSource<object> source, Func<TaskCompletionSource<object>> action)
	{
		if (task == null)
		{
			return action();
		}
		AsyncHelper.ContinueTask(task, source, delegate
		{
			TaskCompletionSource<object> taskCompletionSource = action();
		});
		return null;
	}

	private Task CopyRowsAsync(int rowsSoFar, int totalRows, CancellationToken cts, TaskCompletionSource<object> source = null)
	{
		Task task = null;
		Task task2 = null;
		try
		{
			int i;
			for (i = rowsSoFar; totalRows <= 0 || i < totalRows; i++)
			{
				if (!_hasMoreRowToCopy)
				{
					break;
				}
				if (_isAsyncBulkCopy)
				{
					task = CheckForCancellation(cts, source);
					if (task != null)
					{
						return task;
					}
				}
				_stateObj.WriteByte(209);
				task2 = CopyColumnsAsync(0);
				if (task2 == null)
				{
					CheckAndRaiseNotification();
					Task task3 = ReadFromRowSourceAsync(cts);
					if (task3 != null)
					{
						if (source == null)
						{
							source = new TaskCompletionSource<object>();
						}
						task = source.Task;
						AsyncHelper.ContinueTask(task3, source, delegate
						{
							CopyRowsAsync(i + 1, totalRows, cts, source);
						}, _connection.GetOpenTdsConnection());
						return task;
					}
					continue;
				}
				source = source ?? new TaskCompletionSource<object>();
				task = source.Task;
				AsyncHelper.ContinueTask(task2, source, delegate
				{
					CheckAndRaiseNotification();
					Task task4 = ReadFromRowSourceAsync(cts);
					if (task4 == null)
					{
						CopyRowsAsync(i + 1, totalRows, cts, source);
					}
					else
					{
						AsyncHelper.ContinueTask(task4, source, delegate
						{
							CopyRowsAsync(i + 1, totalRows, cts, source);
						}, _connection.GetOpenTdsConnection());
					}
				}, _connection.GetOpenTdsConnection());
				return task;
			}
			if (source != null)
			{
				source.TrySetResult(null);
			}
		}
		catch (Exception exception)
		{
			if (source == null)
			{
				throw;
			}
			source.TrySetException(exception);
		}
		return task;
	}

	private Task CopyBatchesAsync(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source = null)
	{
		try
		{
			while (_hasMoreRowToCopy)
			{
				SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
				if (IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
				{
					openTdsConnection.ThreadHasParserLockForClose = true;
					try
					{
						_internalTransaction = _connection.BeginTransaction();
					}
					finally
					{
						openTdsConnection.ThreadHasParserLockForClose = false;
					}
				}
				Task task = SubmitUpdateBulkCommand(updateBulkCommandText);
				if (task == null)
				{
					Task task2 = CopyBatchesAsyncContinued(internalResults, updateBulkCommandText, cts, source);
					if (task2 != null)
					{
						return task2;
					}
					continue;
				}
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					Task task3 = CopyBatchesAsyncContinued(internalResults, updateBulkCommandText, cts, source);
					if (task3 == null)
					{
						CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
					}
				}, _connection.GetOpenTdsConnection());
				return source.Task;
			}
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
		if (source != null)
		{
			source.SetResult(null);
			return source.Task;
		}
		return null;
	}

	private Task CopyBatchesAsyncContinued(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source)
	{
		try
		{
			WriteMetaData(internalResults);
			_parser.LoadColumnEncryptionKeys(internalResults[1].MetaData, _connection);
			Task task = CopyRowsAsync(0, _savedBatchSize, cts);
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					Task task2 = CopyBatchesAsyncContinuedOnSuccess(internalResults, updateBulkCommandText, cts, source);
					if (task2 == null)
					{
						CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
					}
				}, _connection.GetOpenTdsConnection(), delegate
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: false);
				}, delegate
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: true);
				});
				return source.Task;
			}
			return CopyBatchesAsyncContinuedOnSuccess(internalResults, updateBulkCommandText, cts, source);
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
	}

	private Task CopyBatchesAsyncContinuedOnSuccess(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source)
	{
		try
		{
			Task task = _parser.WriteBulkCopyDone(_stateObj);
			if (task == null)
			{
				RunParser();
				CommitTransaction();
				return null;
			}
			if (source == null)
			{
				source = new TaskCompletionSource<object>();
			}
			AsyncHelper.ContinueTask(task, source, delegate
			{
				try
				{
					RunParser();
					CommitTransaction();
				}
				catch (Exception)
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: false);
					throw;
				}
				CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
			}, _connection.GetOpenTdsConnection(), delegate
			{
				CopyBatchesAsyncContinuedOnError(cleanupParser: false);
			});
			return source.Task;
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
	}

	private void CopyBatchesAsyncContinuedOnError(bool cleanupParser)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (cleanupParser && _parser != null && _stateObj != null)
			{
				_parser._asyncWrite = false;
				Task task = _parser.WriteBulkCopyDone(_stateObj);
				RunParser();
			}
			if (_stateObj != null)
			{
				CleanUpStateObject();
			}
		}
		catch (OutOfMemoryException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		AbortTransaction();
	}

	private void CleanUpStateObject(bool isCancelRequested = true)
	{
		if (_stateObj == null)
		{
			return;
		}
		_parser.Connection.ThreadHasParserLockForClose = true;
		try
		{
			_stateObj.ResetBuffer();
			_stateObj.ResetPacketCounters();
			if (isCancelRequested && (_parser.State == TdsParserState.OpenNotLoggedIn || _parser.State == TdsParserState.OpenLoggedIn))
			{
				_stateObj.CancelRequest();
			}
			_stateObj.SetTimeoutStateStopped();
			_stateObj.CloseSession();
			_stateObj._bulkCopyOpperationInProgress = false;
			_stateObj._bulkCopyWriteTimeout = false;
			_stateObj = null;
		}
		finally
		{
			_parser.Connection.ThreadHasParserLockForClose = false;
		}
	}

	private void WriteToServerInternalRestContinuedAsync(BulkCopySimpleResultSet internalResults, CancellationToken cts, TaskCompletionSource<object> source)
	{
		Task task = null;
		string text = null;
		try
		{
			text = AnalyzeTargetAndCreateUpdateBulkCommand(internalResults);
			if (_sortedColumnMappings.Count != 0)
			{
				_stateObj.SniContext = SniContext.Snix_SendRows;
				_savedBatchSize = _batchSize;
				_rowsUntilNotification = _notifyAfter;
				_rowsCopied = 0;
				_currentRowMetadata = new SourceColumnMetadata[_sortedColumnMappings.Count];
				for (int i = 0; i < _currentRowMetadata.Length; i++)
				{
					_currentRowMetadata[i] = GetColumnMetadata(i);
				}
				task = CopyBatchesAsync(internalResults, text, cts);
			}
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					if (task.IsCanceled)
					{
						_localColumnMappings = null;
						try
						{
							CleanUpStateObject();
							return;
						}
						finally
						{
							source.SetCanceled();
						}
					}
					if (task.Exception != null)
					{
						source.SetException(task.Exception.InnerException);
						return;
					}
					_localColumnMappings = null;
					try
					{
						CleanUpStateObject(isCancelRequested: false);
					}
					finally
					{
						if (source != null)
						{
							if (cts.IsCancellationRequested)
							{
								source.SetCanceled();
							}
							else
							{
								source.SetResult(null);
							}
						}
					}
				}, _connection.GetOpenTdsConnection());
			}
			else
			{
				_localColumnMappings = null;
				try
				{
					CleanUpStateObject(isCancelRequested: false);
				}
				catch (Exception)
				{
				}
				if (source != null)
				{
					source.SetResult(null);
				}
			}
		}
		catch (Exception exception)
		{
			_localColumnMappings = null;
			try
			{
				CleanUpStateObject();
			}
			catch (Exception)
			{
			}
			if (source != null)
			{
				source.TrySetException(exception);
				return;
			}
			throw;
		}
	}

	private void WriteToServerInternalRestAsync(CancellationToken cts, TaskCompletionSource<object> source)
	{
		_hasMoreRowToCopy = true;
		Task<BulkCopySimpleResultSet> internalResultsTask = null;
		BulkCopySimpleResultSet result = new BulkCopySimpleResultSet();
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		try
		{
			_parser = _connection.Parser;
			_parser._asyncWrite = _isAsyncBulkCopy;
			Task task;
			try
			{
				task = _connection.ValidateAndReconnect(delegate
				{
					if (_parserLock != null)
					{
						_parserLock.Release();
						_parserLock = null;
					}
				}, BulkCopyTimeout);
			}
			catch (SqlException inner)
			{
				throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner);
			}
			if (task != null)
			{
				if (_isAsyncBulkCopy)
				{
					CancellationTokenRegistration regReconnectCancel = default(CancellationTokenRegistration);
					TaskCompletionSource<object> cancellableReconnectTS = new TaskCompletionSource<object>();
					if (cts.CanBeCanceled)
					{
						regReconnectCancel = cts.Register(delegate
						{
							cancellableReconnectTS.TrySetCanceled();
						});
					}
					AsyncHelper.ContinueTask(task, cancellableReconnectTS, delegate
					{
						cancellableReconnectTS.SetResult(null);
					});
					AsyncHelper.SetTimeoutException(cancellableReconnectTS, BulkCopyTimeout, () => SQL.BulkLoadInvalidDestinationTable(_destinationTableName, SQL.CR_ReconnectTimeout()), CancellationToken.None);
					AsyncHelper.ContinueTask(cancellableReconnectTS.Task, source, delegate
					{
						regReconnectCancel.Dispose();
						if (_parserLock != null)
						{
							_parserLock.Release();
							_parserLock = null;
						}
						_parserLock = _connection.GetOpenTdsConnection()._parserLock;
						_parserLock.Wait(canReleaseFromAnyThread: true);
						WriteToServerInternalRestAsync(cts, source);
					}, null, delegate
					{
						regReconnectCancel.Dispose();
					}, delegate
					{
						regReconnectCancel.Dispose();
					}, (Exception ex) => SQL.BulkLoadInvalidDestinationTable(_destinationTableName, ex), _connection);
					return;
				}
				try
				{
					AsyncHelper.WaitForCompletion(task, BulkCopyTimeout, delegate
					{
						throw SQL.CR_ReconnectTimeout();
					});
				}
				catch (SqlException inner2)
				{
					throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner2);
				}
				_parserLock = _connection.GetOpenTdsConnection()._parserLock;
				_parserLock.Wait(canReleaseFromAnyThread: false);
				WriteToServerInternalRestAsync(cts, source);
				return;
			}
			if (_isAsyncBulkCopy)
			{
				_connection.AddWeakReference(this, 3);
			}
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_stateObj = _parser.GetSession(this);
				_stateObj._bulkCopyOpperationInProgress = true;
				_stateObj.StartSession(ObjectID);
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
			try
			{
				internalResultsTask = CreateAndExecuteInitialQueryAsync(out result);
			}
			catch (SqlException inner3)
			{
				throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner3);
			}
			if (internalResultsTask != null)
			{
				AsyncHelper.ContinueTask(internalResultsTask, source, delegate
				{
					WriteToServerInternalRestContinuedAsync(internalResultsTask.Result, cts, source);
				}, _connection.GetOpenTdsConnection());
			}
			else
			{
				WriteToServerInternalRestContinuedAsync(result, cts, source);
			}
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return;
			}
			throw;
		}
	}

	private Task WriteToServerInternalAsync(CancellationToken ctoken)
	{
		TaskCompletionSource<object> source = null;
		Task<object> outterTask = null;
		if (_isAsyncBulkCopy)
		{
			source = new TaskCompletionSource<object>();
			outterTask = source.Task;
			RegisterForConnectionCloseNotification(ref outterTask);
		}
		if (_destinationTableName == null)
		{
			if (source != null)
			{
				source.SetException(SQL.BulkLoadMissingDestinationTable());
				return outterTask;
			}
			throw SQL.BulkLoadMissingDestinationTable();
		}
		try
		{
			Task task = ReadFromRowSourceAsync(ctoken);
			if (task == null)
			{
				if (!_hasMoreRowToCopy)
				{
					if (source != null)
					{
						source.SetResult(null);
					}
					return outterTask;
				}
				WriteToServerInternalRestAsync(ctoken, source);
				return outterTask;
			}
			AsyncHelper.ContinueTask(task, source, delegate
			{
				if (!_hasMoreRowToCopy)
				{
					source.SetResult(null);
				}
				else
				{
					WriteToServerInternalRestAsync(ctoken, source);
				}
			}, _connection.GetOpenTdsConnection());
			return outterTask;
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return outterTask;
			}
			throw;
		}
	}
}
