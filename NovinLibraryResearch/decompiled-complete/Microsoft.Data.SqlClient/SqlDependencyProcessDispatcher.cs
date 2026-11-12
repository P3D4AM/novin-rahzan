using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Xml;
using Microsoft.Data;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Data.SqlClient;

internal class SqlDependencyProcessDispatcher : MarshalByRefObject
{
	private class SqlConnectionContainer
	{
		private SqlConnection _con;

		private SqlCommand _com;

		private SqlParameter _conversationGuidParam;

		private SqlParameter _timeoutParam;

		private SqlConnectionContainerHashHelper _hashHelper;

		private WindowsIdentity _windowsIdentity;

		private string _queue;

		private string _receiveQuery;

		private string _beginConversationQuery;

		private string _endConversationQuery;

		private string _concatQuery;

		private readonly int _defaultWaitforTimeout = 60000;

		private string _escapedQueueName;

		private string _sprocName;

		private string _dialogHandle;

		private string _cachedServer;

		private string _cachedDatabase;

		private volatile bool _errorState;

		private volatile bool _stop;

		private volatile bool _stopped;

		private volatile bool _serviceQueueCreated;

		private int _startCount;

		private Timer _retryTimer;

		private Dictionary<string, int> _appDomainKeyHash;

		private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

		private static int _objectTypeCount;

		internal int ObjectID => _objectID;

		internal string Database
		{
			get
			{
				if (_cachedDatabase == null)
				{
					_cachedDatabase = _con.Database;
				}
				return _cachedDatabase;
			}
		}

		internal SqlConnectionContainerHashHelper HashHelper => _hashHelper;

		internal bool InErrorState => _errorState;

		internal string Queue => _queue;

		internal string Server => _cachedServer;

		internal SqlConnectionContainer(SqlConnectionContainerHashHelper hashHelper, string appDomainKey, bool useDefaults)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer|DEP> {0}, queue: '{1}'", ObjectID, HashHelper?.Queue);
			bool flag = false;
			try
			{
				_hashHelper = hashHelper;
				string text = null;
				if (useDefaults)
				{
					text = Guid.NewGuid().ToString();
					_queue = "SqlQueryNotificationService-" + text;
					_hashHelper.ConnectionStringBuilder.ApplicationName = _queue;
				}
				else
				{
					_queue = _hashHelper.Queue;
				}
				_con = new SqlConnection(_hashHelper.ConnectionStringBuilder.ConnectionString);
				SqlConnectionString sqlConnectionString = (SqlConnectionString)_con.ConnectionOptions;
				sqlConnectionString.CreatePermissionSet().Assert();
				if (sqlConnectionString.LocalDBInstance != null)
				{
					LocalDBAPI.AssertLocalDBPermissions();
				}
				_con.Open();
				_cachedServer = _con.DataSource;
				if (!_con.IsYukonOrNewer)
				{
					throw SQL.NotificationsRequireYukon();
				}
				if (hashHelper.Identity != null)
				{
					_windowsIdentity = DbConnectionPoolIdentity.GetCurrentWindowsIdentity();
				}
				_escapedQueueName = SqlConnection.FixupDatabaseTransactionName(_queue);
				_appDomainKeyHash = new Dictionary<string, int>();
				_com = new SqlCommand();
				_com.Connection = _con;
				_com.CommandText = "select is_broker_enabled from sys.databases where database_id=db_id()";
				if (!(bool)_com.ExecuteScalar())
				{
					throw SQL.SqlDependencyDatabaseBrokerDisabled();
				}
				_conversationGuidParam = new SqlParameter("@p1", SqlDbType.UniqueIdentifier);
				_timeoutParam = new SqlParameter("@p2", SqlDbType.Int);
				_timeoutParam.Value = 0;
				_com.Parameters.Add(_timeoutParam);
				flag = true;
				_receiveQuery = "WAITFOR(RECEIVE TOP (1) message_type_name, conversation_handle, cast(message_body AS XML) as message_body from " + _escapedQueueName + "), TIMEOUT @p2;";
				if (useDefaults)
				{
					_sprocName = SqlConnection.FixupDatabaseTransactionName("SqlQueryNotificationStoredProcedure-" + text);
					CreateQueueAndService(restart: false);
				}
				else
				{
					_com.CommandText = _receiveQuery;
					_endConversationQuery = "END CONVERSATION @p1; ";
					_concatQuery = _endConversationQuery + _receiveQuery;
				}
				bool appDomainStart = false;
				IncrementStartCount(appDomainKey, out appDomainStart);
				SynchronouslyQueryServiceBrokerQueue();
				_timeoutParam.Value = _defaultWaitforTimeout;
				AsynchronouslyQueryServiceBrokerQueue();
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e);
				if (flag)
				{
					TearDownAndDispose();
				}
				else
				{
					if (_com != null)
					{
						_com.Dispose();
						_com = null;
					}
					if (_con != null)
					{
						_con.Dispose();
						_con = null;
					}
				}
				throw;
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		internal bool AppDomainUnload(string appDomainKey)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.AppDomainUnload|DEP> {0}, AppDomainKey: '{1}'", ObjectID, appDomainKey);
			try
			{
				lock (_appDomainKeyHash)
				{
					if (_appDomainKeyHash.ContainsKey(appDomainKey))
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash contained AppDomainKey: '{0}'.", appDomainKey);
						int num = _appDomainKeyHash[appDomainKey];
						SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash for AppDomainKey: '{0}' count: '{1}'.", appDomainKey, num);
						bool appDomainStop = false;
						while (num > 0)
						{
							Stop(appDomainKey, out appDomainStop);
							num--;
						}
						if (_appDomainKeyHash.ContainsKey(appDomainKey))
						{
							SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.AppDomainUnload|DEP|ERR> ERROR - after the Stop() loop, _appDomainKeyHash for AppDomainKey: '{0}' entry not removed from hash.  Count: {1}'", appDomainKey, _appDomainKeyHash[appDomainKey]);
						}
					}
					else
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash did not contain AppDomainKey: '{0}'.", appDomainKey);
					}
				}
				SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.AppDomainUnload|DEP> Exiting, _stopped: '{0}'.", _stopped);
				return _stopped;
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void AsynchronouslyQueryServiceBrokerQueue()
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.AsynchronouslyQueryServiceBrokerQueue|DEP> {0}", ObjectID);
			try
			{
				AsyncCallback callback = AsyncResultCallback;
				_com.BeginExecuteReader(callback, null);
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void AsyncResultCallback(IAsyncResult asyncResult)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.AsyncResultCallback|DEP> {0}", ObjectID);
			try
			{
				using (SqlDataReader reader = _com.EndExecuteReader(asyncResult))
				{
					ProcessNotificationResults(reader);
				}
				if (!_stop)
				{
					AsynchronouslyQueryServiceBrokerQueue();
				}
				else
				{
					TearDownAndDispose();
				}
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					_errorState = true;
					throw;
				}
				SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.AsyncResultCallback|DEP> Exception occurred.");
				if (!_stop)
				{
					ADP.TraceExceptionWithoutRethrow(e);
				}
				if (_stop)
				{
					TearDownAndDispose();
					return;
				}
				_errorState = true;
				Restart(null);
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void CreateQueueAndService(bool restart)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.CreateQueueAndService|DEP> {0}", ObjectID);
			try
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = _con;
				SqlTransaction sqlTransaction = null;
				try
				{
					sqlTransaction = (sqlCommand.Transaction = _con.BeginTransaction());
					string text = SqlServerEscapeHelper.MakeStringLiteral(_queue);
					sqlCommand.CommandText = "CREATE PROCEDURE " + _sprocName + " AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM " + _escapedQueueName + "; IF (SELECT COUNT(*) FROM " + _escapedQueueName + " WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = " + text + ") > 0)   DROP SERVICE " + _escapedQueueName + "; if (OBJECT_ID(" + text + ", 'SQ') IS NOT NULL)   DROP QUEUE " + _escapedQueueName + "; DROP PROCEDURE " + _sprocName + "; END COMMIT TRANSACTION; END";
					if (!restart)
					{
						sqlCommand.ExecuteNonQuery();
					}
					else
					{
						try
						{
							sqlCommand.ExecuteNonQuery();
						}
						catch (Exception e)
						{
							if (!ADP.IsCatchableExceptionType(e))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e);
							try
							{
								if (sqlTransaction != null)
								{
									sqlTransaction.Rollback();
									sqlTransaction = null;
								}
							}
							catch (Exception e2)
							{
								if (!ADP.IsCatchableExceptionType(e2))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e2);
							}
						}
						if (sqlTransaction == null)
						{
							sqlTransaction = (sqlCommand.Transaction = _con.BeginTransaction());
						}
					}
					sqlCommand.CommandText = "IF OBJECT_ID(" + text + ", 'SQ') IS NULL BEGIN CREATE QUEUE " + _escapedQueueName + " WITH ACTIVATION (PROCEDURE_NAME=" + _sprocName + ", MAX_QUEUE_READERS=1, EXECUTE AS OWNER); END; IF (SELECT COUNT(*) FROM sys.services WHERE NAME=" + text + ") = 0 BEGIN CREATE SERVICE " + _escapedQueueName + " ON QUEUE " + _escapedQueueName + " ([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]); IF (SELECT COUNT(*) FROM sys.database_principals WHERE name='sql_dependency_subscriber' AND type='R') <> 0 BEGIN GRANT SEND ON SERVICE::" + _escapedQueueName + " TO sql_dependency_subscriber; END;  END; BEGIN DIALOG @dialog_handle FROM SERVICE " + _escapedQueueName + " TO SERVICE " + text;
					SqlParameter sqlParameter = new SqlParameter();
					sqlParameter.ParameterName = "@dialog_handle";
					sqlParameter.DbType = DbType.Guid;
					sqlParameter.Direction = ParameterDirection.Output;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					_dialogHandle = ((Guid)sqlParameter.Value).ToString();
					_beginConversationQuery = "BEGIN CONVERSATION TIMER ('" + _dialogHandle + "') TIMEOUT = 120; " + _receiveQuery;
					_com.CommandText = _beginConversationQuery;
					_endConversationQuery = "END CONVERSATION @p1; ";
					_concatQuery = _endConversationQuery + _com.CommandText;
					sqlTransaction.Commit();
					sqlTransaction = null;
					_serviceQueueCreated = true;
				}
				finally
				{
					if (sqlTransaction != null)
					{
						try
						{
							sqlTransaction.Rollback();
							sqlTransaction = null;
						}
						catch (Exception e3)
						{
							if (!ADP.IsCatchableExceptionType(e3))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e3);
						}
					}
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		internal void IncrementStartCount(string appDomainKey, out bool appDomainStart)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.IncrementStartCount|DEP> {0}", ObjectID);
			try
			{
				appDomainStart = false;
				int args = Interlocked.Increment(ref _startCount);
				SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.IncrementStartCount|DEP> {0}, incremented _startCount: {1}", _staticInstance.ObjectID, args);
				lock (_appDomainKeyHash)
				{
					if (_appDomainKeyHash.ContainsKey(appDomainKey))
					{
						_appDomainKeyHash[appDomainKey] += 1;
						SqlClientEventSource.Log.TryNotificationTraceEvent("SqlConnectionContainer.IncrementStartCount|DEP> _appDomainKeyHash contained AppDomainKey: '{0}', incremented count: '{1}'.", appDomainKey, _appDomainKeyHash[appDomainKey]);
					}
					else
					{
						_appDomainKeyHash[appDomainKey] = 1;
						appDomainStart = true;
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.IncrementStartCount|DEP> _appDomainKeyHash did not contain AppDomainKey: '{0}', added to hashtable and value set to 1.", appDomainKey);
					}
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void ProcessNotificationResults(SqlDataReader reader)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> {0}", ObjectID);
			try
			{
				Guid guid = Guid.Empty;
				try
				{
					if (_stop)
					{
						return;
					}
					while (reader.Read())
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Row read.");
						string @string = reader.GetString(0);
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> msgType: '{0}'", @string);
						guid = reader.GetGuid(1);
						if (string.Compare(@string, "http://schemas.microsoft.com/SQL/Notifications/QueryNotification", StringComparison.OrdinalIgnoreCase) == 0)
						{
							SqlXml sqlXml = reader.GetSqlXml(2);
							if (sqlXml != null)
							{
								SqlNotification sqlNotification = SqlNotificationParser.ProcessMessage(sqlXml);
								if (sqlNotification != null)
								{
									string key = sqlNotification.Key;
									SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Key: '{0}'", key);
									int num = key.IndexOf(';');
									if (num >= 0)
									{
										string key2 = key.Substring(0, num);
										SqlDependencyPerAppDomainDispatcher sqlDependencyPerAppDomainDispatcher;
										lock (_staticInstance._sqlDependencyPerAppDomainDispatchers)
										{
											sqlDependencyPerAppDomainDispatcher = _staticInstance._sqlDependencyPerAppDomainDispatchers[key2];
										}
										if (sqlDependencyPerAppDomainDispatcher != null)
										{
											try
											{
												sqlDependencyPerAppDomainDispatcher.InvalidateCommandID(sqlNotification);
											}
											catch (Exception e)
											{
												if (!ADP.IsCatchableExceptionType(e))
												{
													throw;
												}
												ADP.TraceExceptionWithoutRethrow(e);
											}
										}
										else
										{
											SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Received notification but do not have an associated PerAppDomainDispatcher!");
										}
									}
									else
									{
										SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Unexpected ID format received!");
									}
								}
								else
								{
									SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Null notification returned from ProcessMessage!");
								}
							}
							else
							{
								SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Null payload for QN notification type!");
							}
						}
						else
						{
							guid = Guid.Empty;
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Unexpected message format received!");
						}
					}
				}
				finally
				{
					if (guid == Guid.Empty)
					{
						_com.CommandText = ((_beginConversationQuery != null) ? _beginConversationQuery : _receiveQuery);
						if (_com.Parameters.Count > 1)
						{
							_com.Parameters.Remove(_conversationGuidParam);
						}
					}
					else
					{
						_com.CommandText = _concatQuery;
						_conversationGuidParam.Value = guid;
						if (_com.Parameters.Count == 1)
						{
							_com.Parameters.Add(_conversationGuidParam);
						}
					}
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void Restart(object unused)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.Restart|DEP> {0}", ObjectID);
			try
			{
				lock (this)
				{
					if (!_stop)
					{
						try
						{
							_con.Close();
						}
						catch (Exception e)
						{
							if (!ADP.IsCatchableExceptionType(e))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e);
						}
					}
				}
				lock (this)
				{
					if (!_stop)
					{
						if (_hashHelper.Identity != null)
						{
							WindowsImpersonationContext val = null;
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
								val = _windowsIdentity.Impersonate();
								_con.Open();
							}
							finally
							{
								if (val != null)
								{
									val.Undo();
								}
							}
						}
						else
						{
							_con.Open();
						}
					}
				}
				lock (this)
				{
					if (!_stop && _serviceQueueCreated)
					{
						bool flag = false;
						try
						{
							CreateQueueAndService(restart: true);
						}
						catch (Exception e2)
						{
							if (!ADP.IsCatchableExceptionType(e2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e2);
							flag = true;
						}
						if (flag)
						{
							_staticInstance.Invalidate(Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
						}
					}
				}
				lock (this)
				{
					if (!_stop)
					{
						_timeoutParam.Value = 0;
						SynchronouslyQueryServiceBrokerQueue();
						_timeoutParam.Value = _defaultWaitforTimeout;
						AsynchronouslyQueryServiceBrokerQueue();
						_errorState = false;
						_retryTimer = null;
					}
				}
				if (_stop)
				{
					TearDownAndDispose();
				}
			}
			catch (Exception e3)
			{
				if (!ADP.IsCatchableExceptionType(e3))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e3);
				try
				{
					_staticInstance.Invalidate(Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
				}
				catch (Exception e4)
				{
					if (!ADP.IsCatchableExceptionType(e4))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e4);
				}
				try
				{
					_con.Close();
				}
				catch (Exception e5)
				{
					if (!ADP.IsCatchableExceptionType(e5))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e5);
				}
				if (!_stop)
				{
					_retryTimer = new Timer(Restart, null, _defaultWaitforTimeout, -1);
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		internal bool Stop(string appDomainKey, out bool appDomainStop)
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.Stop|DEP> {0}", ObjectID);
			try
			{
				appDomainStop = false;
				if (appDomainKey != null)
				{
					lock (_appDomainKeyHash)
					{
						if (_appDomainKeyHash.ContainsKey(appDomainKey))
						{
							int num = _appDomainKeyHash[appDomainKey];
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP> _appDomainKeyHash contained AppDomainKey: '{0}', pre-decrement Count: '{1}'.", appDomainKey, num);
							if (num > 0)
							{
								_appDomainKeyHash[appDomainKey] = num - 1;
							}
							else
							{
								SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP|ERR> ERROR pre-decremented count <= 0!");
							}
							if (1 == num)
							{
								_appDomainKeyHash.Remove(appDomainKey);
								appDomainStop = true;
							}
						}
						else
						{
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP|ERR> ERROR appDomainKey not null and not found in hash!");
						}
					}
				}
				if (Interlocked.Decrement(ref _startCount) == 0)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP> Reached 0 count, cancelling and waiting.");
					lock (this)
					{
						try
						{
							_com.Cancel();
						}
						catch (Exception e)
						{
							if (!ADP.IsCatchableExceptionType(e))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e);
						}
						_stop = true;
					}
					Stopwatch stopwatch = Stopwatch.StartNew();
					while (true)
					{
						lock (this)
						{
							if (!_stopped)
							{
								if (!_errorState && stopwatch.Elapsed.Seconds < 30)
								{
									goto IL_01ba;
								}
								SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP|ERR> forcing cleanup. elapsedSeconds: '{0}', _errorState: '{1}'.", stopwatch.Elapsed.Seconds, _errorState);
								Timer retryTimer = _retryTimer;
								_retryTimer = null;
								retryTimer?.Dispose();
								TearDownAndDispose();
							}
						}
						break;
						IL_01ba:
						Thread.Sleep(1);
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlConnectionContainer.Stop|DEP> _startCount not 0 after decrement.  _startCount: '{0}'.", _startCount);
				}
				return _stopped;
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void SynchronouslyQueryServiceBrokerQueue()
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.SynchronouslyQueryServiceBrokerQueue|DEP> {0}", ObjectID);
			try
			{
				using SqlDataReader reader = _com.ExecuteReader();
				ProcessNotificationResults(reader);
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}

		private void TearDownAndDispose()
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlConnectionContainer.TearDownAndDispose|DEP> {0}", ObjectID);
			try
			{
				lock (this)
				{
					try
					{
						if (_con.State == ConnectionState.Closed || ConnectionState.Broken == _con.State)
						{
							return;
						}
						if (_com.Parameters.Count > 1)
						{
							try
							{
								_com.CommandText = _endConversationQuery;
								_com.Parameters.Remove(_timeoutParam);
								_com.ExecuteNonQuery();
							}
							catch (Exception e)
							{
								if (!ADP.IsCatchableExceptionType(e))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e);
							}
						}
						if (!_serviceQueueCreated || _errorState)
						{
							return;
						}
						_com.CommandText = "BEGIN TRANSACTION; DROP SERVICE " + _escapedQueueName + "; DROP QUEUE " + _escapedQueueName + "; DROP PROCEDURE " + _sprocName + "; COMMIT TRANSACTION;";
						try
						{
							_com.ExecuteNonQuery();
						}
						catch (Exception e2)
						{
							if (!ADP.IsCatchableExceptionType(e2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e2);
						}
					}
					finally
					{
						_stopped = true;
						_con.Dispose();
						if (_windowsIdentity != null)
						{
							_windowsIdentity.Dispose();
						}
					}
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}
	}

	private class SqlNotificationParser
	{
		[Flags]
		private enum MessageAttributes
		{
			None = 0,
			Type = 1,
			Source = 2,
			Info = 4,
			All = 7
		}

		private const string RootNode = "QueryNotification";

		private const string MessageNode = "Message";

		private const string InfoAttribute = "info";

		private const string SourceAttribute = "source";

		private const string TypeAttribute = "type";

		internal static SqlNotification ProcessMessage(SqlXml xmlMessage)
		{
			using XmlReader xmlReader = xmlMessage.CreateReader();
			string empty = string.Empty;
			MessageAttributes messageAttributes = MessageAttributes.None;
			SqlNotificationType type = SqlNotificationType.Unknown;
			SqlNotificationInfo info = SqlNotificationInfo.Unknown;
			SqlNotificationSource source = SqlNotificationSource.Unknown;
			string key = string.Empty;
			xmlReader.Read();
			if (XmlNodeType.Element == xmlReader.NodeType && "QueryNotification" == xmlReader.LocalName && 3 <= xmlReader.AttributeCount)
			{
				while (MessageAttributes.All != messageAttributes && xmlReader.MoveToNextAttribute())
				{
					try
					{
						switch (xmlReader.LocalName)
						{
						case "type":
							try
							{
								SqlNotificationType sqlNotificationType = (SqlNotificationType)Enum.Parse(typeof(SqlNotificationType), xmlReader.Value, ignoreCase: true);
								if (Enum.IsDefined(typeof(SqlNotificationType), sqlNotificationType))
								{
									type = sqlNotificationType;
								}
							}
							catch (Exception e2)
							{
								if (!ADP.IsCatchableExceptionType(e2))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e2);
							}
							messageAttributes |= MessageAttributes.Type;
							break;
						case "source":
							try
							{
								SqlNotificationSource sqlNotificationSource = (SqlNotificationSource)Enum.Parse(typeof(SqlNotificationSource), xmlReader.Value, ignoreCase: true);
								if (Enum.IsDefined(typeof(SqlNotificationSource), sqlNotificationSource))
								{
									source = sqlNotificationSource;
								}
							}
							catch (Exception e3)
							{
								if (!ADP.IsCatchableExceptionType(e3))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e3);
							}
							messageAttributes |= MessageAttributes.Source;
							break;
						case "info":
							try
							{
								string value = xmlReader.Value;
								switch (value)
								{
								case "set options":
									info = SqlNotificationInfo.Options;
									break;
								case "previous invalid":
									info = SqlNotificationInfo.PreviousFire;
									break;
								case "query template limit":
									info = SqlNotificationInfo.TemplateLimit;
									break;
								default:
								{
									SqlNotificationInfo sqlNotificationInfo = (SqlNotificationInfo)Enum.Parse(typeof(SqlNotificationInfo), value, ignoreCase: true);
									if (Enum.IsDefined(typeof(SqlNotificationInfo), sqlNotificationInfo))
									{
										info = sqlNotificationInfo;
									}
									break;
								}
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
							messageAttributes |= MessageAttributes.Info;
							break;
						}
					}
					catch (ArgumentException e4)
					{
						ADP.TraceExceptionWithoutRethrow(e4);
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> Exception thrown - Enum.Parse failed to parse the value '{0}' of the attribute '{1}'.", xmlReader.Value, xmlReader.LocalName);
						return null;
					}
				}
				if (MessageAttributes.All != messageAttributes)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> Not all expected attributes in Message; messageAttributes = '{0}'.", (int)messageAttributes);
					return null;
				}
				if (!xmlReader.Read())
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<Sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
					return null;
				}
				if (XmlNodeType.Element != xmlReader.NodeType || string.Compare(xmlReader.LocalName, "Message", StringComparison.OrdinalIgnoreCase) != 0)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
					return null;
				}
				if (!xmlReader.Read())
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
					return null;
				}
				if (xmlReader.NodeType != XmlNodeType.Text)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
					return null;
				}
				using (XmlTextReader xmlTextReader = new XmlTextReader(xmlReader.Value, XmlNodeType.Element, null)
				{
					DtdProcessing = DtdProcessing.Prohibit
				})
				{
					if (!xmlTextReader.Read())
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
						return null;
					}
					if (xmlTextReader.NodeType != XmlNodeType.Text)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
						return null;
					}
					key = xmlTextReader.Value;
					xmlTextReader.Close();
				}
				return new SqlNotification(info, source, type, key);
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.");
			return null;
		}
	}

	private class SqlConnectionContainerHashHelper
	{
		private DbConnectionPoolIdentity _identity;

		private string _connectionString;

		private string _queue;

		private SqlConnectionStringBuilder _connectionStringBuilder;

		internal SqlConnectionStringBuilder ConnectionStringBuilder => _connectionStringBuilder;

		internal DbConnectionPoolIdentity Identity => _identity;

		internal string Queue => _queue;

		internal SqlConnectionContainerHashHelper(DbConnectionPoolIdentity identity, string connectionString, string queue, SqlConnectionStringBuilder connectionStringBuilder)
		{
			_identity = identity;
			_connectionString = connectionString;
			_queue = queue;
			_connectionStringBuilder = connectionStringBuilder;
		}

		public override bool Equals(object value)
		{
			SqlConnectionContainerHashHelper sqlConnectionContainerHashHelper = (SqlConnectionContainerHashHelper)value;
			bool flag = false;
			if (sqlConnectionContainerHashHelper == null)
			{
				return false;
			}
			if (this == sqlConnectionContainerHashHelper)
			{
				return true;
			}
			if ((_identity != null && sqlConnectionContainerHashHelper._identity == null) || (_identity == null && sqlConnectionContainerHashHelper._identity != null))
			{
				return false;
			}
			if (_identity == null && sqlConnectionContainerHashHelper._identity == null)
			{
				if (sqlConnectionContainerHashHelper._connectionString == _connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, _queue, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}
			if (sqlConnectionContainerHashHelper._identity.Equals(_identity) && sqlConnectionContainerHashHelper._connectionString == _connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, _queue, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (_identity != null)
			{
				num = _identity.GetHashCode();
			}
			if (_queue != null)
			{
				return _connectionString.GetHashCode() + _queue.GetHashCode() + num;
			}
			return _connectionString.GetHashCode() + num;
		}
	}

	private static SqlDependencyProcessDispatcher _staticInstance = new SqlDependencyProcessDispatcher(null);

	private Dictionary<SqlConnectionContainerHashHelper, SqlConnectionContainer> _connectionContainers;

	private Dictionary<string, SqlDependencyPerAppDomainDispatcher> _sqlDependencyPerAppDomainDispatchers;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private static int _objectTypeCount;

	internal int ObjectID => _objectID;

	internal SqlDependencyProcessDispatcher SingletonProcessDispatcher => _staticInstance;

	private SqlDependencyProcessDispatcher(object dummyVariable)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher|DEP> {0}", ObjectID);
		try
		{
			_connectionContainers = new Dictionary<SqlConnectionContainerHashHelper, SqlConnectionContainer>();
			_sqlDependencyPerAppDomainDispatchers = new Dictionary<string, SqlDependencyPerAppDomainDispatcher>();
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	public SqlDependencyProcessDispatcher()
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher|DEP> {0}", ObjectID);
		try
		{
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private static SqlConnectionContainerHashHelper GetHashHelper(string connectionString, out SqlConnectionStringBuilder connectionStringBuilder, out DbConnectionPoolIdentity identity, out string user, string queue)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher.GetHashString|DEP> {0}, queue: {1}", _staticInstance.ObjectID, queue);
		try
		{
			connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
			connectionStringBuilder.AsynchronousProcessing = true;
			connectionStringBuilder.Pooling = false;
			connectionStringBuilder.Enlist = false;
			connectionStringBuilder.ConnectRetryCount = 0;
			if (queue != null)
			{
				connectionStringBuilder.ApplicationName = queue;
			}
			if (connectionStringBuilder.IntegratedSecurity)
			{
				identity = DbConnectionPoolIdentity.GetCurrent();
				user = null;
			}
			else
			{
				identity = null;
				user = connectionStringBuilder.UserID;
			}
			return new SqlConnectionContainerHashHelper(identity, connectionStringBuilder.ConnectionString, queue, connectionStringBuilder);
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}

	private void Invalidate(string server, SqlNotification sqlNotification)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher.Invalidate|DEP> {0}, server: {1}", ObjectID, server);
		try
		{
			lock (_sqlDependencyPerAppDomainDispatchers)
			{
				foreach (KeyValuePair<string, SqlDependencyPerAppDomainDispatcher> sqlDependencyPerAppDomainDispatcher in _sqlDependencyPerAppDomainDispatchers)
				{
					SqlDependencyPerAppDomainDispatcher value = sqlDependencyPerAppDomainDispatcher.Value;
					try
					{
						value.InvalidateServer(server, sqlNotification);
					}
					catch (Exception e)
					{
						if (!ADP.IsCatchableExceptionType(e))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e);
					}
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void QueueAppDomainUnloading(string appDomainKey)
	{
		ThreadPool.QueueUserWorkItem(AppDomainUnloading, appDomainKey);
	}

	private void AppDomainUnloading(object state)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher.AppDomainUnloading|DEP> {0}", ObjectID);
		try
		{
			string text = (string)state;
			lock (_connectionContainers)
			{
				List<SqlConnectionContainerHashHelper> list = new List<SqlConnectionContainerHashHelper>();
				foreach (KeyValuePair<SqlConnectionContainerHashHelper, SqlConnectionContainer> connectionContainer in _connectionContainers)
				{
					SqlConnectionContainer value = connectionContainer.Value;
					if (value.AppDomainUnload(text))
					{
						list.Add(value.HashHelper);
					}
				}
				foreach (SqlConnectionContainerHashHelper item in list)
				{
					_connectionContainers.Remove(item);
				}
			}
			lock (_sqlDependencyPerAppDomainDispatchers)
			{
				_sqlDependencyPerAppDomainDispatchers.Remove(text);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal bool StartWithDefault(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string service, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart)
	{
		return Start(connectionString, out server, out identity, out user, out database, ref service, appDomainKey, dispatcher, out errorOccurred, out appDomainStart, useDefaults: true);
	}

	internal bool Start(string connectionString, string queue, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher)
	{
		string server = null;
		bool errorOccurred = false;
		DbConnectionPoolIdentity identity = null;
		return Start(connectionString, out server, out identity, out server, out server, ref queue, appDomainKey, dispatcher, out errorOccurred, out errorOccurred, useDefaults: false);
	}

	private bool Start(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart, bool useDefaults)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, queue: '{1}', appDomainKey: '{2}', perAppDomainDispatcher ID: '{3}'", ObjectID, queueService, appDomainKey, dispatcher.ObjectID);
		try
		{
			server = null;
			identity = null;
			user = null;
			database = null;
			errorOccurred = false;
			appDomainStart = false;
			lock (_sqlDependencyPerAppDomainDispatchers)
			{
				if (!_sqlDependencyPerAppDomainDispatchers.ContainsKey(appDomainKey))
				{
					_sqlDependencyPerAppDomainDispatchers[appDomainKey] = dispatcher;
				}
			}
			SqlConnectionStringBuilder connectionStringBuilder = null;
			SqlConnectionContainerHashHelper hashHelper = GetHashHelper(connectionString, out connectionStringBuilder, out identity, out user, queueService);
			bool flag = false;
			SqlConnectionContainer sqlConnectionContainer = null;
			lock (_connectionContainers)
			{
				if (!_connectionContainers.ContainsKey(hashHelper))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, hashtable miss, creating new container.", ObjectID);
					sqlConnectionContainer = new SqlConnectionContainer(hashHelper, appDomainKey, useDefaults);
					_connectionContainers.Add(hashHelper, sqlConnectionContainer);
					flag = true;
					appDomainStart = true;
				}
				else
				{
					sqlConnectionContainer = _connectionContainers[hashHelper];
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, hashtable hit, container: {1}", ObjectID, sqlConnectionContainer.ObjectID);
					if (sqlConnectionContainer.InErrorState)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, container: {1} is in error state!", ObjectID, sqlConnectionContainer.ObjectID);
						errorOccurred = true;
					}
					else
					{
						sqlConnectionContainer.IncrementStartCount(appDomainKey, out appDomainStart);
					}
				}
			}
			if (useDefaults && !errorOccurred)
			{
				server = sqlConnectionContainer.Server;
				database = sqlConnectionContainer.Database;
				queueService = sqlConnectionContainer.Queue;
				SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, default service: '{1}', server: '{2}', database: '{3}'", ObjectID, queueService, server, database);
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Start|DEP> {0}, started: {1}", ObjectID, flag);
			return flag;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal bool Stop(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, out bool appDomainStop)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependencyProcessDispatcher.Stop|DEP> {0}, queue: '{1}'", ObjectID, queueService);
		try
		{
			server = null;
			identity = null;
			user = null;
			database = null;
			appDomainStop = false;
			SqlConnectionStringBuilder connectionStringBuilder = null;
			SqlConnectionContainerHashHelper hashHelper = GetHashHelper(connectionString, out connectionStringBuilder, out identity, out user, queueService);
			bool flag = false;
			lock (_connectionContainers)
			{
				if (_connectionContainers.ContainsKey(hashHelper))
				{
					SqlConnectionContainer sqlConnectionContainer = _connectionContainers[hashHelper];
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Stop|DEP> {0}, hashtable hit, container: {1}", ObjectID, sqlConnectionContainer.ObjectID);
					server = sqlConnectionContainer.Server;
					database = sqlConnectionContainer.Database;
					queueService = sqlConnectionContainer.Queue;
					if (sqlConnectionContainer.Stop(appDomainKey, out appDomainStop))
					{
						flag = true;
						_connectionContainers.Remove(hashHelper);
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<Sc.SqlDependencyProcessDispatcher.Stop|DEP> {0}, hashtable miss.", ObjectID);
				}
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependencyProcessDispatcher.Stop|DEP> {0}, stopped: {1}", ObjectID, flag);
			return flag;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}
}
