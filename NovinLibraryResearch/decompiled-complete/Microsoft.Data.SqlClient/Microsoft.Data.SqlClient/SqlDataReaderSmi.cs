using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Data.SqlTypes;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlDataReaderSmi : SqlDataReader
{
	internal enum PositionState
	{
		BeforeResults,
		BeforeRows,
		OnRow,
		AfterRows,
		AfterResults
	}

	private sealed class ReaderEventSink : SmiEventSink_Default
	{
		private readonly SqlDataReaderSmi reader;

		internal ReaderEventSink(SqlDataReaderSmi reader, SmiEventSink parent)
			: base(parent)
		{
			this.reader = reader;
		}

		internal override void MetaDataAvailable(SmiQueryMetaData[] md, bool nextEventIsRow)
		{
			int args = ((md != null) ? md.Length : (-1));
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.MetaDataAvailable|ADV> {0}, md.Length={1} nextEventIsRow={2}.", reader.ObjectID, args, nextEventIsRow);
			if (SqlClientEventSource.Log.IsAdvancedTraceOn() && md != null)
			{
				for (int i = 0; i < md.Length; i++)
				{
					SqlClientEventSource.Log.TraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.MetaDataAvailable|ADV> {0}, metaData[{1}] is {2}{3}", reader.ObjectID, i, md[i].GetType(), md[i].TraceString());
				}
			}
			reader.MetaDataAvailable(md, nextEventIsRow);
		}

		internal override void RowAvailable(ITypedGetters row)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> {0} (v2).", reader.ObjectID);
			reader.RowAvailable(row);
		}

		internal override void RowAvailable(ITypedGettersV3 row)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> {0} (ITypedGettersV3).", reader.ObjectID);
			reader.RowAvailable(row);
		}

		internal override void RowAvailable(SmiTypedGetterSetter rowData)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.RowAvailable|ADV> {0} (SmiTypedGetterSetter).", reader.ObjectID);
			reader.RowAvailable(rowData);
		}

		internal override void StatementCompleted(int recordsAffected)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.StatementCompleted|ADV> {0} recordsAffected= {1}.", reader.ObjectID, recordsAffected);
			base.StatementCompleted(recordsAffected);
			reader.StatementCompleted();
		}

		internal override void BatchCompleted()
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlDataReaderSmi.ReaderEventSink.BatchCompleted|ADV> {0}.", reader.ObjectID);
			base.BatchCompleted();
			reader.BatchCompleted();
		}
	}

	private PositionState _currentPosition;

	private bool _isOpen;

	private SmiQueryMetaData[] _currentMetaData;

	private int[] _indexMap;

	private int _visibleColumnCount;

	private DataTable _schemaTable;

	private ITypedGetters _currentColumnValues;

	private ITypedGettersV3 _currentColumnValuesV3;

	private bool _hasRows;

	private SmiEventStream _eventStream;

	private SmiRequestExecutor _requestExecutor;

	private SqlInternalConnectionSmi _currentConnection;

	private ReaderEventSink _readerEventSink;

	private FieldNameLookup _fieldNameLookup;

	private SqlSequentialStreamSmi _currentStream;

	private SqlSequentialTextReaderSmi _currentTextReader;

	public override int FieldCount
	{
		get
		{
			ThrowIfClosed("FieldCount");
			return InternalFieldCount;
		}
	}

	public override int VisibleFieldCount
	{
		get
		{
			ThrowIfClosed("VisibleFieldCount");
			if (FNotInResults())
			{
				return 0;
			}
			return _visibleColumnCount;
		}
	}

	public override int Depth
	{
		get
		{
			ThrowIfClosed("Depth");
			return 0;
		}
	}

	public override object this[int ordinal] => GetValue(ordinal);

	public override object this[string strName] => GetValue(GetOrdinal(strName));

	public override bool IsClosed => IsReallyClosed();

	public override int RecordsAffected => base.Command.InternalRecordsAffected;

	public override bool HasRows => _hasRows;

	private int InternalFieldCount
	{
		get
		{
			if (FNotInResults())
			{
				return 0;
			}
			return _currentMetaData.Length;
		}
	}

	public override string GetName(int ordinal)
	{
		EnsureCanGetMetaData("GetName");
		return _currentMetaData[ordinal].Name;
	}

	public override string GetDataTypeName(int ordinal)
	{
		EnsureCanGetMetaData("GetDataTypeName");
		SmiExtendedMetaData smiExtendedMetaData = _currentMetaData[ordinal];
		if (SqlDbType.Udt == smiExtendedMetaData.SqlDbType)
		{
			return smiExtendedMetaData.TypeSpecificNamePart1 + "." + smiExtendedMetaData.TypeSpecificNamePart2 + "." + smiExtendedMetaData.TypeSpecificNamePart3;
		}
		return smiExtendedMetaData.TypeName;
	}

	public override Type GetFieldType(int ordinal)
	{
		EnsureCanGetMetaData("GetFieldType");
		if (SqlDbType.Udt == _currentMetaData[ordinal].SqlDbType)
		{
			return _currentMetaData[ordinal].Type;
		}
		return MetaType.GetMetaTypeFromSqlDbType(_currentMetaData[ordinal].SqlDbType, _currentMetaData[ordinal].IsMultiValued).ClassType;
	}

	public override Type GetProviderSpecificFieldType(int ordinal)
	{
		EnsureCanGetMetaData("GetProviderSpecificFieldType");
		if (SqlDbType.Udt == _currentMetaData[ordinal].SqlDbType)
		{
			return _currentMetaData[ordinal].Type;
		}
		return MetaType.GetMetaTypeFromSqlDbType(_currentMetaData[ordinal].SqlDbType, _currentMetaData[ordinal].IsMultiValued).SqlType;
	}

	public override object GetValue(int ordinal)
	{
		EnsureCanGetCol("GetValue", ordinal);
		SmiQueryMetaData metaData = _currentMetaData[ordinal];
		if (_currentConnection.IsKatmaiOrNewer)
		{
			return ValueUtilsSmi.GetValue200(_readerEventSink, (SmiTypedGetterSetter)_currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
		}
		return ValueUtilsSmi.GetValue(_readerEventSink, _currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
	}

	public override T GetFieldValue<T>(int ordinal)
	{
		EnsureCanGetCol("GetFieldValue<T>", ordinal);
		SmiQueryMetaData metaData = _currentMetaData[ordinal];
		if (SqlDataReader._typeofINullable.IsAssignableFrom(typeof(T)))
		{
			if (_currentConnection.IsKatmaiOrNewer)
			{
				return (T)ValueUtilsSmi.GetSqlValue200(_readerEventSink, (SmiTypedGetterSetter)_currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
			}
			return (T)ValueUtilsSmi.GetSqlValue(_readerEventSink, _currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
		}
		if (_currentConnection.IsKatmaiOrNewer)
		{
			return (T)ValueUtilsSmi.GetValue200(_readerEventSink, (SmiTypedGetterSetter)_currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
		}
		return (T)ValueUtilsSmi.GetValue(_readerEventSink, _currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
	}

	public override Task<T> GetFieldValueAsync<T>(int ordinal, CancellationToken cancellationToken)
	{
		return ADP.CreatedTaskWithException<T>(ADP.ExceptionWithStackTrace(SQL.NotAvailableOnContextConnection()));
	}

	internal override SqlBuffer.StorageType GetVariantInternalStorageType(int ordinal)
	{
		if (IsDBNull(ordinal))
		{
			return SqlBuffer.StorageType.Empty;
		}
		SmiMetaData variantType = _currentColumnValuesV3.GetVariantType(_readerEventSink, ordinal);
		if (variantType == null)
		{
			return SqlBuffer.StorageType.Empty;
		}
		return ValueUtilsSmi.SqlDbTypeToStorageType(variantType.SqlDbType);
	}

	public override int GetValues(object[] values)
	{
		EnsureCanGetCol("GetValues", 0);
		if (values == null)
		{
			throw ADP.ArgumentNull("values");
		}
		int num = ((values.Length < _visibleColumnCount) ? values.Length : _visibleColumnCount);
		for (int i = 0; i < num; i++)
		{
			values[_indexMap[i]] = GetValue(i);
		}
		return num;
	}

	public override int GetOrdinal(string name)
	{
		EnsureCanGetMetaData("GetOrdinal");
		if (_fieldNameLookup == null)
		{
			_fieldNameLookup = new FieldNameLookup(this, -1);
		}
		return _fieldNameLookup.GetOrdinal(name);
	}

	public override bool IsDBNull(int ordinal)
	{
		EnsureCanGetCol("IsDBNull", ordinal);
		return ValueUtilsSmi.IsDBNull(_readerEventSink, _currentColumnValuesV3, ordinal);
	}

	public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
	{
		return ADP.CreatedTaskWithException<bool>(ADP.ExceptionWithStackTrace(SQL.NotAvailableOnContextConnection()));
	}

	public override bool GetBoolean(int ordinal)
	{
		EnsureCanGetCol("GetBoolean", ordinal);
		return ValueUtilsSmi.GetBoolean(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override byte GetByte(int ordinal)
	{
		EnsureCanGetCol("GetByte", ordinal);
		return ValueUtilsSmi.GetByte(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		EnsureCanGetCol("GetBytes", ordinal);
		return ValueUtilsSmi.GetBytes(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], fieldOffset, buffer, bufferOffset, length, throwOnNull: true);
	}

	internal override long GetBytesInternal(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		EnsureCanGetCol("GetBytes", ordinal);
		return ValueUtilsSmi.GetBytesInternal(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], fieldOffset, buffer, bufferOffset, length, throwOnNull: false);
	}

	public override char GetChar(int ordinal)
	{
		throw ADP.NotSupported();
	}

	public override long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		EnsureCanGetCol("GetChars", ordinal);
		SmiExtendedMetaData smiExtendedMetaData = _currentMetaData[ordinal];
		if (IsCommandBehavior(CommandBehavior.SequentialAccess) && smiExtendedMetaData.SqlDbType == SqlDbType.Xml)
		{
			return GetStreamingXmlChars(ordinal, fieldOffset, buffer, bufferOffset, length);
		}
		return ValueUtilsSmi.GetChars(_readerEventSink, _currentColumnValuesV3, ordinal, smiExtendedMetaData, fieldOffset, buffer, bufferOffset, length);
	}

	public override Guid GetGuid(int ordinal)
	{
		EnsureCanGetCol("GetGuid", ordinal);
		return ValueUtilsSmi.GetGuid(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override short GetInt16(int ordinal)
	{
		EnsureCanGetCol("GetInt16", ordinal);
		return ValueUtilsSmi.GetInt16(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override int GetInt32(int ordinal)
	{
		EnsureCanGetCol("GetInt32", ordinal);
		return ValueUtilsSmi.GetInt32(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override long GetInt64(int ordinal)
	{
		EnsureCanGetCol("GetInt64", ordinal);
		return ValueUtilsSmi.GetInt64(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override float GetFloat(int ordinal)
	{
		EnsureCanGetCol("GetFloat", ordinal);
		return ValueUtilsSmi.GetSingle(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override double GetDouble(int ordinal)
	{
		EnsureCanGetCol("GetDouble", ordinal);
		return ValueUtilsSmi.GetDouble(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override string GetString(int ordinal)
	{
		EnsureCanGetCol("GetString", ordinal);
		return ValueUtilsSmi.GetString(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override decimal GetDecimal(int ordinal)
	{
		EnsureCanGetCol("GetDecimal", ordinal);
		return ValueUtilsSmi.GetDecimal(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override DateTime GetDateTime(int ordinal)
	{
		EnsureCanGetCol("GetDateTime", ordinal);
		return ValueUtilsSmi.GetDateTime(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	internal override void CloseReaderFromConnection()
	{
		CloseInternal(closeConnection: false);
	}

	public override void Close()
	{
		CloseInternal(IsCommandBehavior(CommandBehavior.CloseConnection));
	}

	private void CloseInternal(bool closeConnection)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlDataReaderSmi.Close|API> {0}", ObjectID);
		bool flag = true;
		try
		{
			if (!IsClosed)
			{
				_hasRows = false;
				while (_eventStream.HasEvents)
				{
					_eventStream.ProcessEvent(_readerEventSink);
					_readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages: true);
				}
				_requestExecutor.Close(_readerEventSink);
				_readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages: true);
			}
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			if (flag)
			{
				_isOpen = false;
				if (closeConnection && base.Connection != null)
				{
					base.Connection.Close();
				}
				SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
			}
		}
	}

	public override bool NextResult()
	{
		ThrowIfClosed("NextResult");
		return InternalNextResult(ignoreNonFatalMessages: false);
	}

	public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
	{
		return ADP.CreatedTaskWithException<bool>(ADP.ExceptionWithStackTrace(SQL.NotAvailableOnContextConnection()));
	}

	internal bool InternalNextResult(bool ignoreNonFatalMessages)
	{
		long scopeId = SqlClientEventSource.Log.TryAdvancedScopeEnterEvent("<sc.SqlDataReaderSmi.InternalNextResult|ADV> {0}", ObjectID);
		try
		{
			_hasRows = false;
			if (PositionState.AfterResults != _currentPosition)
			{
				while (InternalRead(ignoreNonFatalMessages))
				{
				}
				ResetResultSet();
				while (_currentMetaData == null && _eventStream.HasEvents)
				{
					_eventStream.ProcessEvent(_readerEventSink);
					_readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalMessages);
				}
			}
			return PositionState.AfterResults != _currentPosition;
		}
		finally
		{
			SqlClientEventSource.Log.TryAdvanceScopeLeave(scopeId);
		}
	}

	public override bool Read()
	{
		ThrowIfClosed("Read");
		return InternalRead(ignoreNonFatalErrors: false);
	}

	public override Task<bool> ReadAsync(CancellationToken cancellationToken)
	{
		return ADP.CreatedTaskWithException<bool>(ADP.ExceptionWithStackTrace(SQL.NotAvailableOnContextConnection()));
	}

	internal bool InternalRead(bool ignoreNonFatalErrors)
	{
		long scopeId = SqlClientEventSource.Log.TryAdvancedScopeEnterEvent("<sc.SqlDataReaderSmi.InternalRead|ADV> {0}", ObjectID);
		try
		{
			if (FInResults())
			{
				_currentColumnValues = null;
				_currentColumnValuesV3 = null;
				if (_currentStream != null)
				{
					_currentStream.SetClosed();
					_currentStream = null;
				}
				if (_currentTextReader != null)
				{
					_currentTextReader.SetClosed();
					_currentTextReader = null;
				}
				while (_currentColumnValues == null && _currentColumnValuesV3 == null && FInResults() && PositionState.AfterRows != _currentPosition && _eventStream.HasEvents)
				{
					_eventStream.ProcessEvent(_readerEventSink);
					_readerEventSink.ProcessMessagesAndThrow(ignoreNonFatalErrors);
				}
			}
			return PositionState.OnRow == _currentPosition;
		}
		finally
		{
			SqlClientEventSource.Log.TryAdvanceScopeLeave(scopeId);
		}
	}

	public override DataTable GetSchemaTable()
	{
		ThrowIfClosed("GetSchemaTable");
		if (_schemaTable == null && FInResults())
		{
			DataTable dataTable = new DataTable("SchemaTable");
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.MinimumCapacity = InternalFieldCount;
			DataColumn column = new DataColumn(SchemaTableColumn.ColumnName, typeof(string));
			DataColumn dataColumn = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int));
			DataColumn column2 = new DataColumn(SchemaTableColumn.ColumnSize, typeof(int));
			DataColumn column3 = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short));
			DataColumn column4 = new DataColumn(SchemaTableColumn.NumericScale, typeof(short));
			DataColumn column5 = new DataColumn(SchemaTableColumn.DataType, typeof(Type));
			DataColumn column6 = new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type));
			DataColumn column7 = new DataColumn(SchemaTableColumn.ProviderType, typeof(int));
			DataColumn column8 = new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int));
			DataColumn dataColumn2 = new DataColumn(SchemaTableColumn.IsLong, typeof(bool));
			DataColumn column9 = new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool));
			DataColumn column10 = new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool));
			DataColumn column11 = new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool));
			DataColumn column12 = new DataColumn(SchemaTableColumn.IsUnique, typeof(bool));
			DataColumn column13 = new DataColumn(SchemaTableColumn.IsKey, typeof(bool));
			DataColumn column14 = new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool));
			DataColumn column15 = new DataColumn(SchemaTableOptionalColumn.IsHidden, typeof(bool));
			DataColumn column16 = new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string));
			DataColumn column17 = new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string));
			DataColumn column18 = new DataColumn(SchemaTableColumn.BaseTableName, typeof(string));
			DataColumn column19 = new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string));
			DataColumn column20 = new DataColumn(SchemaTableOptionalColumn.BaseServerName, typeof(string));
			DataColumn column21 = new DataColumn(SchemaTableColumn.IsAliased, typeof(bool));
			DataColumn column22 = new DataColumn(SchemaTableColumn.IsExpression, typeof(bool));
			DataColumn column23 = new DataColumn("IsIdentity", typeof(bool));
			DataColumn column24 = new DataColumn("DataTypeName", typeof(string));
			DataColumn column25 = new DataColumn("UdtAssemblyQualifiedName", typeof(string));
			DataColumn column26 = new DataColumn("XmlSchemaCollectionDatabase", typeof(string));
			DataColumn column27 = new DataColumn("XmlSchemaCollectionOwningSchema", typeof(string));
			DataColumn column28 = new DataColumn("XmlSchemaCollectionName", typeof(string));
			DataColumn column29 = new DataColumn("IsColumnSet", typeof(bool));
			dataColumn.DefaultValue = 0;
			dataColumn2.DefaultValue = false;
			DataColumnCollection columns = dataTable.Columns;
			columns.Add(column);
			columns.Add(dataColumn);
			columns.Add(column2);
			columns.Add(column3);
			columns.Add(column4);
			columns.Add(column12);
			columns.Add(column13);
			columns.Add(column20);
			columns.Add(column16);
			columns.Add(column19);
			columns.Add(column17);
			columns.Add(column18);
			columns.Add(column5);
			columns.Add(column9);
			columns.Add(column7);
			columns.Add(column21);
			columns.Add(column22);
			columns.Add(column23);
			columns.Add(column14);
			columns.Add(column11);
			columns.Add(column15);
			columns.Add(dataColumn2);
			columns.Add(column10);
			columns.Add(column6);
			columns.Add(column24);
			columns.Add(column26);
			columns.Add(column27);
			columns.Add(column28);
			columns.Add(column25);
			columns.Add(column8);
			columns.Add(column29);
			for (int i = 0; i < InternalFieldCount; i++)
			{
				SmiQueryMetaData smiQueryMetaData = _currentMetaData[i];
				long num = smiQueryMetaData.MaxLength;
				MetaType metaType = MetaType.GetMetaTypeFromSqlDbType(smiQueryMetaData.SqlDbType, smiQueryMetaData.IsMultiValued);
				if (-1 == num)
				{
					metaType = MetaType.GetMaxMetaTypeFromMetaType(metaType);
					num = ((metaType.IsSizeInCharacters && !metaType.IsPlp) ? 1073741823 : int.MaxValue);
				}
				DataRow dataRow = dataTable.NewRow();
				if (SqlDbType.Decimal == smiQueryMetaData.SqlDbType)
				{
					num = 17L;
				}
				else if (SqlDbType.Variant == smiQueryMetaData.SqlDbType)
				{
					num = 8009L;
				}
				dataRow[column] = smiQueryMetaData.Name;
				dataRow[dataColumn] = i;
				dataRow[column2] = num;
				dataRow[column7] = (int)smiQueryMetaData.SqlDbType;
				dataRow[column8] = (int)smiQueryMetaData.SqlDbType;
				if (smiQueryMetaData.SqlDbType != SqlDbType.Udt)
				{
					dataRow[column5] = metaType.ClassType;
					dataRow[column6] = metaType.SqlType;
				}
				else
				{
					dataRow[column25] = smiQueryMetaData.Type.AssemblyQualifiedName;
					dataRow[column5] = smiQueryMetaData.Type;
					dataRow[column6] = smiQueryMetaData.Type;
				}
				byte b = byte.MaxValue;
				switch (smiQueryMetaData.SqlDbType)
				{
				case SqlDbType.BigInt:
				case SqlDbType.DateTime:
				case SqlDbType.Decimal:
				case SqlDbType.Int:
				case SqlDbType.Money:
				case SqlDbType.SmallDateTime:
				case SqlDbType.SmallInt:
				case SqlDbType.SmallMoney:
				case SqlDbType.TinyInt:
					b = smiQueryMetaData.Precision;
					break;
				case SqlDbType.Float:
					b = 15;
					break;
				case SqlDbType.Real:
					b = 7;
					break;
				default:
					b = byte.MaxValue;
					break;
				}
				dataRow[column3] = b;
				if (SqlDbType.Decimal == smiQueryMetaData.SqlDbType || SqlDbType.Time == smiQueryMetaData.SqlDbType || SqlDbType.DateTime2 == smiQueryMetaData.SqlDbType || SqlDbType.DateTimeOffset == smiQueryMetaData.SqlDbType)
				{
					dataRow[column4] = smiQueryMetaData.Scale;
				}
				else
				{
					dataRow[column4] = MetaType.GetMetaTypeFromSqlDbType(smiQueryMetaData.SqlDbType, smiQueryMetaData.IsMultiValued).Scale;
				}
				dataRow[column9] = smiQueryMetaData.AllowsDBNull;
				if (!smiQueryMetaData.IsAliased.IsNull)
				{
					dataRow[column21] = smiQueryMetaData.IsAliased.Value;
				}
				if (!smiQueryMetaData.IsKey.IsNull)
				{
					dataRow[column13] = smiQueryMetaData.IsKey.Value;
				}
				if (!smiQueryMetaData.IsHidden.IsNull)
				{
					dataRow[column15] = smiQueryMetaData.IsHidden.Value;
				}
				if (!smiQueryMetaData.IsExpression.IsNull)
				{
					dataRow[column22] = smiQueryMetaData.IsExpression.Value;
				}
				dataRow[column10] = smiQueryMetaData.IsReadOnly;
				dataRow[column23] = smiQueryMetaData.IsIdentity;
				dataRow[column29] = smiQueryMetaData.IsColumnSet;
				dataRow[column14] = smiQueryMetaData.IsIdentity;
				dataRow[dataColumn2] = metaType.IsLong;
				if (SqlDbType.Timestamp == smiQueryMetaData.SqlDbType)
				{
					dataRow[column12] = true;
					dataRow[column11] = true;
				}
				else
				{
					dataRow[column12] = false;
					dataRow[column11] = false;
				}
				if (!ADP.IsEmpty(smiQueryMetaData.ColumnName))
				{
					dataRow[column19] = smiQueryMetaData.ColumnName;
				}
				else if (!ADP.IsEmpty(smiQueryMetaData.Name))
				{
					dataRow[column19] = smiQueryMetaData.Name;
				}
				if (!ADP.IsEmpty(smiQueryMetaData.TableName))
				{
					dataRow[column18] = smiQueryMetaData.TableName;
				}
				if (!ADP.IsEmpty(smiQueryMetaData.SchemaName))
				{
					dataRow[column17] = smiQueryMetaData.SchemaName;
				}
				if (!ADP.IsEmpty(smiQueryMetaData.CatalogName))
				{
					dataRow[column16] = smiQueryMetaData.CatalogName;
				}
				if (!ADP.IsEmpty(smiQueryMetaData.ServerName))
				{
					dataRow[column20] = smiQueryMetaData.ServerName;
				}
				if (SqlDbType.Udt == smiQueryMetaData.SqlDbType)
				{
					dataRow[column24] = smiQueryMetaData.TypeSpecificNamePart1 + "." + smiQueryMetaData.TypeSpecificNamePart2 + "." + smiQueryMetaData.TypeSpecificNamePart3;
				}
				else
				{
					dataRow[column24] = metaType.TypeName;
				}
				if (SqlDbType.Xml == smiQueryMetaData.SqlDbType)
				{
					dataRow[column26] = smiQueryMetaData.TypeSpecificNamePart1;
					dataRow[column27] = smiQueryMetaData.TypeSpecificNamePart2;
					dataRow[column28] = smiQueryMetaData.TypeSpecificNamePart3;
				}
				dataTable.Rows.Add(dataRow);
				dataRow.AcceptChanges();
			}
			foreach (DataColumn item in columns)
			{
				item.ReadOnly = true;
			}
			_schemaTable = dataTable;
		}
		return _schemaTable;
	}

	public override SqlBinary GetSqlBinary(int ordinal)
	{
		EnsureCanGetCol("GetSqlBinary", ordinal);
		return ValueUtilsSmi.GetSqlBinary(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlBoolean GetSqlBoolean(int ordinal)
	{
		EnsureCanGetCol("GetSqlBoolean", ordinal);
		return ValueUtilsSmi.GetSqlBoolean(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlByte GetSqlByte(int ordinal)
	{
		EnsureCanGetCol("GetSqlByte", ordinal);
		return ValueUtilsSmi.GetSqlByte(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlInt16 GetSqlInt16(int ordinal)
	{
		EnsureCanGetCol("GetSqlInt16", ordinal);
		return ValueUtilsSmi.GetSqlInt16(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlInt32 GetSqlInt32(int ordinal)
	{
		EnsureCanGetCol("GetSqlInt32", ordinal);
		return ValueUtilsSmi.GetSqlInt32(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlInt64 GetSqlInt64(int ordinal)
	{
		EnsureCanGetCol("GetSqlInt64", ordinal);
		return ValueUtilsSmi.GetSqlInt64(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlSingle GetSqlSingle(int ordinal)
	{
		EnsureCanGetCol("GetSqlSingle", ordinal);
		return ValueUtilsSmi.GetSqlSingle(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlDouble GetSqlDouble(int ordinal)
	{
		EnsureCanGetCol("GetSqlDouble", ordinal);
		return ValueUtilsSmi.GetSqlDouble(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlMoney GetSqlMoney(int ordinal)
	{
		EnsureCanGetCol("GetSqlMoney", ordinal);
		return ValueUtilsSmi.GetSqlMoney(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlDateTime GetSqlDateTime(int ordinal)
	{
		EnsureCanGetCol("GetSqlDateTime", ordinal);
		return ValueUtilsSmi.GetSqlDateTime(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlDecimal GetSqlDecimal(int ordinal)
	{
		EnsureCanGetCol("GetSqlDecimal", ordinal);
		return ValueUtilsSmi.GetSqlDecimal(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlString GetSqlString(int ordinal)
	{
		EnsureCanGetCol("GetSqlString", ordinal);
		return ValueUtilsSmi.GetSqlString(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlGuid GetSqlGuid(int ordinal)
	{
		EnsureCanGetCol("GetSqlGuid", ordinal);
		return ValueUtilsSmi.GetSqlGuid(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal]);
	}

	public override SqlChars GetSqlChars(int ordinal)
	{
		EnsureCanGetCol("GetSqlChars", ordinal);
		return ValueUtilsSmi.GetSqlChars(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], _currentConnection.InternalContext);
	}

	public override SqlBytes GetSqlBytes(int ordinal)
	{
		EnsureCanGetCol("GetSqlBytes", ordinal);
		return ValueUtilsSmi.GetSqlBytes(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], _currentConnection.InternalContext);
	}

	public override SqlXml GetSqlXml(int ordinal)
	{
		EnsureCanGetCol("GetSqlXml", ordinal);
		return ValueUtilsSmi.GetSqlXml(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], _currentConnection.InternalContext);
	}

	public override TimeSpan GetTimeSpan(int ordinal)
	{
		EnsureCanGetCol("GetTimeSpan", ordinal);
		return ValueUtilsSmi.GetTimeSpan(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], _currentConnection.IsKatmaiOrNewer);
	}

	public override DateTimeOffset GetDateTimeOffset(int ordinal)
	{
		EnsureCanGetCol("GetDateTimeOffset", ordinal);
		return ValueUtilsSmi.GetDateTimeOffset(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], _currentConnection.IsKatmaiOrNewer);
	}

	public override object GetSqlValue(int ordinal)
	{
		EnsureCanGetCol("GetSqlValue", ordinal);
		SmiMetaData metaData = _currentMetaData[ordinal];
		if (_currentConnection.IsKatmaiOrNewer)
		{
			return ValueUtilsSmi.GetSqlValue200(_readerEventSink, (SmiTypedGetterSetter)_currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
		}
		return ValueUtilsSmi.GetSqlValue(_readerEventSink, _currentColumnValuesV3, ordinal, metaData, _currentConnection.InternalContext);
	}

	public override int GetSqlValues(object[] values)
	{
		EnsureCanGetCol("GetSqlValues", 0);
		if (values == null)
		{
			throw ADP.ArgumentNull("values");
		}
		int num = ((values.Length < _visibleColumnCount) ? values.Length : _visibleColumnCount);
		for (int i = 0; i < num; i++)
		{
			values[_indexMap[i]] = GetSqlValue(i);
		}
		return num;
	}

	public override Stream GetStream(int ordinal)
	{
		EnsureCanGetCol("GetStream", ordinal);
		SmiQueryMetaData smiQueryMetaData = _currentMetaData[ordinal];
		if (smiQueryMetaData.SqlDbType != SqlDbType.Variant && IsCommandBehavior(CommandBehavior.SequentialAccess) && !ValueUtilsSmi.IsDBNull(_readerEventSink, _currentColumnValuesV3, ordinal))
		{
			if (HasActiveStreamOrTextReaderOnColumn(ordinal))
			{
				throw ADP.NonSequentialColumnAccess(ordinal, ordinal + 1);
			}
			_currentStream = ValueUtilsSmi.GetSequentialStream(_readerEventSink, _currentColumnValuesV3, ordinal, smiQueryMetaData);
			return _currentStream;
		}
		return ValueUtilsSmi.GetStream(_readerEventSink, _currentColumnValuesV3, ordinal, smiQueryMetaData);
	}

	public override TextReader GetTextReader(int ordinal)
	{
		EnsureCanGetCol("GetTextReader", ordinal);
		SmiQueryMetaData smiQueryMetaData = _currentMetaData[ordinal];
		if (smiQueryMetaData.SqlDbType != SqlDbType.Variant && IsCommandBehavior(CommandBehavior.SequentialAccess) && !ValueUtilsSmi.IsDBNull(_readerEventSink, _currentColumnValuesV3, ordinal))
		{
			if (HasActiveStreamOrTextReaderOnColumn(ordinal))
			{
				throw ADP.NonSequentialColumnAccess(ordinal, ordinal + 1);
			}
			_currentTextReader = ValueUtilsSmi.GetSequentialTextReader(_readerEventSink, _currentColumnValuesV3, ordinal, smiQueryMetaData);
			return _currentTextReader;
		}
		return ValueUtilsSmi.GetTextReader(_readerEventSink, _currentColumnValuesV3, ordinal, smiQueryMetaData);
	}

	public override XmlReader GetXmlReader(int ordinal)
	{
		EnsureCanGetCol("GetXmlReader", ordinal);
		if (_currentMetaData[ordinal].SqlDbType != SqlDbType.Xml)
		{
			throw ADP.InvalidCast();
		}
		Stream stream = null;
		if (IsCommandBehavior(CommandBehavior.SequentialAccess) && !ValueUtilsSmi.IsDBNull(_readerEventSink, _currentColumnValuesV3, ordinal))
		{
			if (HasActiveStreamOrTextReaderOnColumn(ordinal))
			{
				throw ADP.NonSequentialColumnAccess(ordinal, ordinal + 1);
			}
			_currentStream = ValueUtilsSmi.GetSequentialStream(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], bypassTypeCheck: true);
			stream = _currentStream;
		}
		else
		{
			stream = ValueUtilsSmi.GetStream(_readerEventSink, _currentColumnValuesV3, ordinal, _currentMetaData[ordinal], bypassTypeCheck: true);
		}
		return SqlTypeWorkarounds.SqlXmlCreateSqlXmlReader(stream);
	}

	internal SqlDataReaderSmi(SmiEventStream eventStream, SqlCommand parent, CommandBehavior behavior, SqlInternalConnectionSmi connection, SmiEventSink parentSink, SmiRequestExecutor requestExecutor)
		: base(parent, behavior)
	{
		_eventStream = eventStream;
		_currentConnection = connection;
		_readerEventSink = new ReaderEventSink(this, parentSink);
		_currentPosition = PositionState.BeforeResults;
		_isOpen = true;
		_indexMap = null;
		_visibleColumnCount = 0;
		_currentStream = null;
		_currentTextReader = null;
		_requestExecutor = requestExecutor;
	}

	internal override SmiExtendedMetaData[] GetInternalSmiMetaData()
	{
		if (_currentMetaData == null || _visibleColumnCount == InternalFieldCount)
		{
			return _currentMetaData;
		}
		SmiExtendedMetaData[] array = new SmiExtendedMetaData[_visibleColumnCount];
		for (int i = 0; i < _visibleColumnCount; i++)
		{
			array[i] = _currentMetaData[_indexMap[i]];
		}
		return array;
	}

	internal override int GetLocaleId(int ordinal)
	{
		EnsureCanGetMetaData("GetLocaleId");
		return (int)_currentMetaData[ordinal].LocaleId;
	}

	private bool IsReallyClosed()
	{
		return !_isOpen;
	}

	internal void ThrowIfClosed(string operationName)
	{
		if (IsClosed)
		{
			throw ADP.DataReaderClosed(operationName);
		}
	}

	private void EnsureCanGetCol(string operationName, int ordinal)
	{
		EnsureOnRow(operationName);
	}

	internal void EnsureOnRow(string operationName)
	{
		ThrowIfClosed(operationName);
		if (_currentPosition != PositionState.OnRow)
		{
			throw SQL.InvalidRead();
		}
	}

	internal void EnsureCanGetMetaData(string operationName)
	{
		ThrowIfClosed(operationName);
		if (FNotInResults())
		{
			throw SQL.InvalidRead();
		}
	}

	private bool FInResults()
	{
		return !FNotInResults();
	}

	private bool FNotInResults()
	{
		if (PositionState.AfterResults != _currentPosition)
		{
			return _currentPosition == PositionState.BeforeResults;
		}
		return true;
	}

	private void MetaDataAvailable(SmiQueryMetaData[] md, bool nextEventIsRow)
	{
		_currentMetaData = md;
		_hasRows = nextEventIsRow;
		_fieldNameLookup = null;
		_schemaTable = null;
		_currentPosition = PositionState.BeforeRows;
		_indexMap = new int[_currentMetaData.Length];
		int num = 0;
		for (int i = 0; i < _currentMetaData.Length; i++)
		{
			if (!_currentMetaData[i].IsHidden.IsTrue)
			{
				_indexMap[num] = i;
				num++;
			}
		}
		_visibleColumnCount = num;
	}

	private bool HasActiveStreamOrTextReaderOnColumn(int columnIndex)
	{
		bool flag = false;
		flag |= _currentStream != null && _currentStream.ColumnIndex == columnIndex;
		return flag | (_currentTextReader != null && _currentTextReader.ColumnIndex == columnIndex);
	}

	private void RowAvailable(ITypedGetters row)
	{
		_currentColumnValues = row;
		_currentPosition = PositionState.OnRow;
	}

	private void RowAvailable(ITypedGettersV3 row)
	{
		_currentColumnValuesV3 = row;
		_currentPosition = PositionState.OnRow;
	}

	private void StatementCompleted()
	{
		_currentPosition = PositionState.AfterRows;
	}

	private void ResetResultSet()
	{
		_currentMetaData = null;
		_visibleColumnCount = 0;
		_schemaTable = null;
	}

	private void BatchCompleted()
	{
		ResetResultSet();
		_currentPosition = PositionState.AfterResults;
		_eventStream.Close(_readerEventSink);
	}
}
