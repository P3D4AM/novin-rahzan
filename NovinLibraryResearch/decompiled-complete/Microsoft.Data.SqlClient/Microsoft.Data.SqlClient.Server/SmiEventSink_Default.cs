namespace Microsoft.Data.SqlClient.Server;

internal class SmiEventSink_Default : SmiEventSink
{
	internal enum UnexpectedEventType
	{
		BatchCompleted,
		ColumnInfoAvailable,
		DefaultDatabaseChanged,
		MessagePosted,
		MetaDataAvailable,
		ParameterAvailable,
		ParametersAvailable,
		RowAvailable,
		StatementCompleted,
		TableNameAvailable,
		TransactionCommitted,
		TransactionDefected,
		TransactionEnlisted,
		TransactionEnded,
		TransactionRolledBack,
		TransactionStarted
	}

	private SmiEventSink _parent;

	private SqlErrorCollection _errors;

	private SqlErrorCollection _warnings;

	private SqlErrorCollection Errors
	{
		get
		{
			if (_errors == null)
			{
				_errors = new SqlErrorCollection();
			}
			return _errors;
		}
	}

	internal bool HasMessages => ((SmiEventSink_Default)_parent)?.HasMessages ?? (_errors != null || _warnings != null);

	internal virtual string ServerVersion => null;

	internal SmiEventSink Parent
	{
		get
		{
			return _parent;
		}
		set
		{
			_parent = value;
		}
	}

	private SqlErrorCollection Warnings
	{
		get
		{
			if (_warnings == null)
			{
				_warnings = new SqlErrorCollection();
			}
			return _warnings;
		}
	}

	protected virtual void DispatchMessages(bool ignoreNonFatalMessages)
	{
		SmiEventSink_Default smiEventSink_Default = (SmiEventSink_Default)_parent;
		if (smiEventSink_Default != null)
		{
			smiEventSink_Default.DispatchMessages(ignoreNonFatalMessages);
			return;
		}
		SqlException ex = ProcessMessages(ignoreWarnings: true, ignoreNonFatalMessages);
		if (ex == null)
		{
			return;
		}
		throw ex;
	}

	protected SqlException ProcessMessages(bool ignoreWarnings, bool ignoreNonFatalMessages)
	{
		SqlException result = null;
		SqlErrorCollection sqlErrorCollection = null;
		if (_errors != null)
		{
			if (ignoreNonFatalMessages)
			{
				sqlErrorCollection = new SqlErrorCollection();
				foreach (SqlError error2 in _errors)
				{
					if (error2.Class >= 20)
					{
						sqlErrorCollection.Add(error2);
					}
				}
				if (sqlErrorCollection.Count <= 0)
				{
					sqlErrorCollection = null;
				}
			}
			else
			{
				if (_warnings != null)
				{
					foreach (SqlError warning in _warnings)
					{
						_errors.Add(warning);
					}
				}
				sqlErrorCollection = _errors;
			}
			_errors = null;
			_warnings = null;
		}
		else
		{
			if (!ignoreWarnings)
			{
				sqlErrorCollection = _warnings;
			}
			_warnings = null;
		}
		if (sqlErrorCollection != null)
		{
			result = SqlException.CreateException(sqlErrorCollection, ServerVersion);
		}
		return result;
	}

	internal void CleanMessages()
	{
		SmiEventSink_Default smiEventSink_Default = (SmiEventSink_Default)_parent;
		if (smiEventSink_Default != null)
		{
			smiEventSink_Default.CleanMessages();
			return;
		}
		_errors = null;
		_warnings = null;
	}

	internal void ProcessMessagesAndThrow()
	{
		ProcessMessagesAndThrow(ignoreNonFatalMessages: false);
	}

	internal void ProcessMessagesAndThrow(bool ignoreNonFatalMessages)
	{
		if (HasMessages)
		{
			DispatchMessages(ignoreNonFatalMessages);
		}
	}

	internal SmiEventSink_Default()
	{
	}

	internal SmiEventSink_Default(SmiEventSink parent)
	{
		_parent = parent;
	}

	internal override void BatchCompleted()
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.BatchCompleted);
		}
		_parent.BatchCompleted();
	}

	internal override void ParametersAvailable(SmiParameterMetaData[] metaData, ITypedGettersV3 paramValues)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.ParametersAvailable);
		}
		_parent.ParametersAvailable(metaData, paramValues);
	}

	internal override void ParameterAvailable(SmiParameterMetaData metaData, SmiTypedGetterSetter paramValue, int ordinal)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.ParameterAvailable);
		}
		_parent.ParameterAvailable(metaData, paramValue, ordinal);
	}

	internal override void DefaultDatabaseChanged(string databaseName)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.DefaultDatabaseChanged);
		}
		_parent.DefaultDatabaseChanged(databaseName);
	}

	internal override void MessagePosted(int number, byte state, byte errorClass, string server, string message, string procedure, int lineNumber)
	{
		if (_parent == null)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SmiEventSink_Default.MessagePosted|ADV> {0}, number={1} state={2} errorClass={3} server='{4}' message='{5}' procedure='{6}' linenumber={7}.", 0, number, state, errorClass, server, message, procedure, lineNumber);
			SqlError sqlError = new SqlError(number, state, errorClass, server, message, procedure, lineNumber);
			if (sqlError.Class < 11)
			{
				Warnings.Add(sqlError);
			}
			else
			{
				Errors.Add(sqlError);
			}
		}
		else
		{
			_parent.MessagePosted(number, state, errorClass, server, message, procedure, lineNumber);
		}
	}

	internal override void MetaDataAvailable(SmiQueryMetaData[] metaData, bool nextEventIsRow)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.MetaDataAvailable);
		}
		_parent.MetaDataAvailable(metaData, nextEventIsRow);
	}

	internal override void RowAvailable(ITypedGetters rowData)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.RowAvailable);
		}
		_parent.RowAvailable(rowData);
	}

	internal override void RowAvailable(ITypedGettersV3 rowData)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.RowAvailable);
		}
		_parent.RowAvailable(rowData);
	}

	internal override void StatementCompleted(int rowsAffected)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.StatementCompleted);
		}
		_parent.StatementCompleted(rowsAffected);
	}

	internal override void TransactionCommitted(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionCommitted);
		}
		_parent.TransactionCommitted(transactionId);
	}

	internal override void TransactionDefected(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionDefected);
		}
		_parent.TransactionDefected(transactionId);
	}

	internal override void TransactionEnlisted(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionEnlisted);
		}
		_parent.TransactionEnlisted(transactionId);
	}

	internal override void TransactionEnded(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionEnded);
		}
		_parent.TransactionEnded(transactionId);
	}

	internal override void TransactionRolledBack(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionRolledBack);
		}
		_parent.TransactionRolledBack(transactionId);
	}

	internal override void TransactionStarted(long transactionId)
	{
		if (_parent == null)
		{
			throw SQL.UnexpectedSmiEvent(UnexpectedEventType.TransactionStarted);
		}
		_parent.TransactionStarted(transactionId);
	}
}
