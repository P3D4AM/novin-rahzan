using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.ProviderBase;
using Microsoft.Data.Sql;

namespace Microsoft.Data.SqlClient;

public sealed class SqlDependency
{
	internal class IdentityUserNamePair
	{
		private DbConnectionPoolIdentity _identity;

		private string _userName;

		internal DbConnectionPoolIdentity Identity => _identity;

		internal string UserName => _userName;

		internal IdentityUserNamePair(DbConnectionPoolIdentity identity, string userName)
		{
			_identity = identity;
			_userName = userName;
		}

		public override bool Equals(object value)
		{
			IdentityUserNamePair identityUserNamePair = (IdentityUserNamePair)value;
			bool result = false;
			if (identityUserNamePair == null)
			{
				result = false;
			}
			else if (this == identityUserNamePair)
			{
				result = true;
			}
			else if (_identity != null)
			{
				if (_identity.Equals(identityUserNamePair._identity))
				{
					result = true;
				}
			}
			else if (_userName == identityUserNamePair._userName)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (_identity != null)
			{
				return _identity.GetHashCode();
			}
			return _userName.GetHashCode();
		}
	}

	private class DatabaseServicePair
	{
		private string _database;

		private string _service;

		internal string Database => _database;

		internal string Service => _service;

		internal DatabaseServicePair(string database, string service)
		{
			_database = database;
			_service = service;
		}

		public override bool Equals(object value)
		{
			DatabaseServicePair databaseServicePair = (DatabaseServicePair)value;
			bool result = false;
			if (databaseServicePair == null)
			{
				result = false;
			}
			else if (this == databaseServicePair)
			{
				result = true;
			}
			else if (_database == databaseServicePair._database)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return _database.GetHashCode();
		}
	}

	internal class EventContextPair
	{
		private OnChangeEventHandler _eventHandler;

		private ExecutionContext _context;

		private SqlDependency _dependency;

		private SqlNotificationEventArgs _args;

		private static ContextCallback _contextCallback = InvokeCallback;

		internal EventContextPair(OnChangeEventHandler eventHandler, SqlDependency dependency)
		{
			_eventHandler = eventHandler;
			_context = ExecutionContext.Capture();
			_dependency = dependency;
		}

		public override bool Equals(object value)
		{
			EventContextPair eventContextPair = (EventContextPair)value;
			bool result = false;
			if (eventContextPair == null)
			{
				result = false;
			}
			else if (this == eventContextPair)
			{
				result = true;
			}
			else if (_eventHandler == eventContextPair._eventHandler)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return _eventHandler.GetHashCode();
		}

		internal void Invoke(SqlNotificationEventArgs args)
		{
			_args = args;
			ExecutionContext.Run(_context, _contextCallback, this);
		}

		private static void InvokeCallback(object eventContextPair)
		{
			EventContextPair eventContextPair2 = (EventContextPair)eventContextPair;
			eventContextPair2._eventHandler(eventContextPair2._dependency, eventContextPair2._args);
		}
	}

	[DataContract]
	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.RemotingConfiguration)]
	private class SqlClientObjRef
	{
		[DataMember]
		private static ObjRef s_sqlObjRef;

		internal static IRemotingTypeInfo _typeInfo;

		private SqlClientObjRef()
		{
		}

		public SqlClientObjRef(SqlDependencyProcessDispatcher dispatcher)
		{
			s_sqlObjRef = RemotingServices.Marshal((MarshalByRefObject)dispatcher);
			_typeInfo = s_sqlObjRef.TypeInfo;
		}

		internal static bool CanCastToSqlDependencyProcessDispatcher()
		{
			return _typeInfo.CanCastTo(typeof(SqlDependencyProcessDispatcher), (object)s_sqlObjRef);
		}

		internal ObjRef GetObjRef()
		{
			return s_sqlObjRef;
		}
	}

	private readonly string _id = Guid.NewGuid().ToString() + ";" + _appDomainKey;

	private string _options;

	private int _timeout;

	private bool _dependencyFired;

	private List<EventContextPair> _eventList = new List<EventContextPair>();

	private object _eventHandlerLock = new object();

	private DateTime _expirationTime = DateTime.MaxValue;

	private List<string> _serverList = new List<string>();

	private static object _startStopLock = new object();

	private static readonly string _appDomainKey = Guid.NewGuid().ToString();

	private static Dictionary<string, Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>> _serverUserHash = new Dictionary<string, Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>>(StringComparer.OrdinalIgnoreCase);

	private static SqlDependencyProcessDispatcher _processDispatcher = null;

	private static readonly string _assemblyName = typeof(SqlDependencyProcessDispatcher).Assembly.FullName;

	private static readonly string _typeName = typeof(SqlDependencyProcessDispatcher).FullName;

	private readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private static int _objectTypeCount;

	internal int ObjectID => _objectID;

	[ResCategory("Data")]
	[ResDescription("Property to indicate if this dependency is invalid.")]
	public bool HasChanges => _dependencyFired;

	[ResCategory("Data")]
	[ResDescription("A string that uniquely identifies this dependency object.")]
	public string Id => _id;

	internal static string AppDomainKey => _appDomainKey;

	internal DateTime ExpirationTime => _expirationTime;

	internal string Options
	{
		get
		{
			string result = null;
			if (_options != null)
			{
				result = _options;
			}
			return result;
		}
	}

	internal static SqlDependencyProcessDispatcher ProcessDispatcher => _processDispatcher;

	internal int Timeout => _timeout;

	[ResCategory("Data")]
	[ResDescription("Event that can be used to subscribe for change notifications.")]
	public event OnChangeEventHandler OnChange
	{
		add
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.OnChange-Add|DEP> {0}", ObjectID);
			try
			{
				if (value == null)
				{
					return;
				}
				SqlNotificationEventArgs sqlNotificationEventArgs = null;
				lock (_eventHandlerLock)
				{
					if (_dependencyFired)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.OnChange-Add|DEP> Dependency already fired, firing new event.");
						sqlNotificationEventArgs = new SqlNotificationEventArgs(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
					}
					else
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.OnChange-Add|DEP> Dependency has not fired, adding new event.");
						EventContextPair item = new EventContextPair(value, this);
						if (_eventList.Contains(item))
						{
							throw SQL.SqlDependencyEventNoDuplicate();
						}
						_eventList.Add(item);
					}
				}
				if (sqlNotificationEventArgs != null)
				{
					value(this, sqlNotificationEventArgs);
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}
		remove
		{
			long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.OnChange-Remove|DEP> {0}", ObjectID);
			try
			{
				if (value == null)
				{
					return;
				}
				EventContextPair item = new EventContextPair(value, this);
				lock (_eventHandlerLock)
				{
					int num = _eventList.IndexOf(item);
					if (0 <= num)
					{
						_eventList.RemoveAt(num);
					}
				}
			}
			finally
			{
				SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
			}
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public SqlDependency()
		: this(null, null, 0)
	{
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public SqlDependency(SqlCommand command)
		: this(command, null, 0)
	{
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public SqlDependency(SqlCommand command, string options, int timeout)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency|DEP> {0}, options: '{1}', timeout: '{2}'", ObjectID, options, timeout);
		try
		{
			if (InOutOfProcHelper.InProc)
			{
				throw SQL.SqlDepCannotBeCreatedInProc();
			}
			if (timeout < 0)
			{
				throw SQL.InvalidSqlDependencyTimeout("timeout");
			}
			_timeout = timeout;
			if (options != null)
			{
				_options = options;
			}
			AddCommandInternal(command);
			SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddDependencyEntry(this);
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	[ResCategory("Data")]
	[ResDescription("To add a command to existing dependency object.")]
	public void AddCommandDependency(SqlCommand command)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.AddCommandDependency|DEP> {0}", ObjectID);
		try
		{
			if (command == null)
			{
				throw ADP.ArgumentNull("command");
			}
			AddCommandInternal(command);
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	private static ObjectHandle CreateProcessDispatcher(_AppDomain masterDomain)
	{
		return masterDomain.CreateInstance(_assemblyName, _typeName);
	}

	private static void ObtainProcessDispatcher()
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		byte[] data = SNINativeMethodWrapper.GetData();
		if (data == null)
		{
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP> nativeStorage null, obtaining dispatcher AppDomain and creating ProcessDispatcher.");
			_AppDomain defaultAppDomain = SNINativeMethodWrapper.GetDefaultAppDomain();
			if (defaultAppDomain != null)
			{
				ObjectHandle objectHandle = CreateProcessDispatcher(defaultAppDomain);
				if (objectHandle != null)
				{
					SqlDependencyProcessDispatcher sqlDependencyProcessDispatcher = (SqlDependencyProcessDispatcher)objectHandle.Unwrap();
					if (sqlDependencyProcessDispatcher != null)
					{
						_processDispatcher = sqlDependencyProcessDispatcher.SingletonProcessDispatcher;
						using MemoryStream memoryStream = new MemoryStream();
						SqlClientObjRef sqlClientObjRef = new SqlClientObjRef(_processDispatcher);
						DataContractSerializer serializer = new DataContractSerializer(sqlClientObjRef.GetType());
						GetSerializedObject(sqlClientObjRef, serializer, memoryStream);
						SNINativeMethodWrapper.SetData(memoryStream.ToArray());
						return;
					}
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - ObjectHandle.Unwrap returned null!");
					throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyObtainProcessDispatcherFailureObjectHandle);
				}
				SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - AppDomain.CreateInstance returned null!");
				throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyProcessDispatcherFailureCreateInstance);
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - unable to obtain default AppDomain!");
			throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyProcessDispatcherFailureAppDomain);
		}
		SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP> nativeStorage not null, obtaining existing dispatcher AppDomain and ProcessDispatcher.");
		using MemoryStream stream = new MemoryStream(data);
		DataContractSerializer serializer2 = new DataContractSerializer(typeof(SqlClientObjRef));
		if (SqlClientObjRef.CanCastToSqlDependencyProcessDispatcher())
		{
			_processDispatcher = GetDeserializedObject(serializer2, stream);
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ObtainProcessDispatcher|DEP> processDispatcher obtained, ID: {0}", _processDispatcher.ObjectID);
			return;
		}
		throw new ArgumentException(Strings.SqlDependency_UnexpectedValueOnDeserialize);
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
	private static void GetSerializedObject(SqlClientObjRef objRef, DataContractSerializer serializer, MemoryStream stream)
	{
		((XmlObjectSerializer)serializer).WriteObject((Stream)stream, (object)objRef);
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
	private static SqlDependencyProcessDispatcher GetDeserializedObject(DataContractSerializer serializer, MemoryStream stream)
	{
		object obj = ((XmlObjectSerializer)serializer).ReadObject((Stream)stream);
		object obj2 = RemotingServices.Unmarshal((obj as SqlClientObjRef).GetObjRef());
		return obj2 as SqlDependencyProcessDispatcher;
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public static bool Start(string connectionString)
	{
		return Start(connectionString, null, useDefaults: true);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public static bool Start(string connectionString, string queue)
	{
		return Start(connectionString, queue, useDefaults: false);
	}

	internal static bool Start(string connectionString, string queue, bool useDefaults)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.Start|DEP> AppDomainKey: '{0}', queue: '{1}'", AppDomainKey, queue);
		try
		{
			if (InOutOfProcHelper.InProc)
			{
				throw SQL.SqlDepCannotBeCreatedInProc();
			}
			if (ADP.IsEmpty(connectionString))
			{
				if (connectionString == null)
				{
					throw ADP.ArgumentNull("connectionString");
				}
				throw ADP.Argument("connectionString");
			}
			if (!useDefaults && ADP.IsEmpty(queue))
			{
				useDefaults = true;
				queue = null;
			}
			SqlConnectionString sqlConnectionString = new SqlConnectionString(connectionString);
			sqlConnectionString.DemandPermission();
			if (sqlConnectionString.LocalDBInstance != null)
			{
				LocalDBAPI.DemandLocalDBPermissions();
			}
			bool errorOccurred = false;
			bool flag = false;
			lock (_startStopLock)
			{
				try
				{
					if (_processDispatcher == null)
					{
						ObtainProcessDispatcher();
					}
					if (useDefaults)
					{
						string server = null;
						DbConnectionPoolIdentity identity = null;
						string user = null;
						string database = null;
						string service = null;
						bool appDomainStart = false;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							flag = _processDispatcher.StartWithDefault(connectionString, out server, out identity, out user, out database, ref service, _appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance, out errorOccurred, out appDomainStart);
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Start|DEP> Start (defaults) returned: '{0}', with service: '{1}', server: '{2}', database: '{3}'", flag, service, server, database);
						}
						finally
						{
							if (appDomainStart && !errorOccurred)
							{
								IdentityUserNamePair identityUser = new IdentityUserNamePair(identity, user);
								DatabaseServicePair databaseService = new DatabaseServicePair(database, service);
								if (!AddToServerUserHash(server, identityUser, databaseService))
								{
									try
									{
										Stop(connectionString, queue, useDefaults, startFailed: true);
									}
									catch (Exception e)
									{
										if (!ADP.IsCatchableExceptionType(e))
										{
											throw;
										}
										ADP.TraceExceptionWithoutRethrow(e);
										SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Start|DEP|ERR> Exception occurred from Stop() after duplicate was found on Start().");
									}
									throw SQL.SqlDependencyDuplicateStart();
								}
							}
						}
					}
					else
					{
						flag = _processDispatcher.Start(connectionString, queue, _appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance);
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Start|DEP> Start (user provided queue) returned: '{0}'", flag);
					}
				}
				catch (Exception e2)
				{
					if (!ADP.IsCatchableExceptionType(e2))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e2);
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Start|DEP|ERR> Exception occurred from _processDispatcher.Start(...), calling Invalidate(...).");
					throw;
				}
			}
			return flag;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public static bool Stop(string connectionString)
	{
		return Stop(connectionString, null, useDefaults: true, startFailed: false);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public static bool Stop(string connectionString, string queue)
	{
		return Stop(connectionString, queue, useDefaults: false, startFailed: false);
	}

	internal static bool Stop(string connectionString, string queue, bool useDefaults, bool startFailed)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.Stop|DEP> AppDomainKey: '{0}', queue: '{1}'", AppDomainKey, queue);
		try
		{
			if (InOutOfProcHelper.InProc)
			{
				throw SQL.SqlDepCannotBeCreatedInProc();
			}
			if (ADP.IsEmpty(connectionString))
			{
				if (connectionString == null)
				{
					throw ADP.ArgumentNull("connectionString");
				}
				throw ADP.Argument("connectionString");
			}
			if (!useDefaults && ADP.IsEmpty(queue))
			{
				useDefaults = true;
				queue = null;
			}
			SqlConnectionString sqlConnectionString = new SqlConnectionString(connectionString);
			sqlConnectionString.DemandPermission();
			if (sqlConnectionString.LocalDBInstance != null)
			{
				LocalDBAPI.DemandLocalDBPermissions();
			}
			bool result = false;
			lock (_startStopLock)
			{
				if (_processDispatcher != null)
				{
					try
					{
						string server = null;
						DbConnectionPoolIdentity identity = null;
						string user = null;
						string database = null;
						string queueService = null;
						if (useDefaults)
						{
							bool appDomainStop = false;
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
								result = _processDispatcher.Stop(connectionString, out server, out identity, out user, out database, ref queueService, _appDomainKey, out appDomainStop);
							}
							finally
							{
								if (appDomainStop && !startFailed)
								{
									IdentityUserNamePair identityUser = new IdentityUserNamePair(identity, user);
									DatabaseServicePair databaseService = new DatabaseServicePair(database, queueService);
									RemoveFromServerUserHash(server, identityUser, databaseService);
								}
							}
						}
						else
						{
							bool appDomainStop2 = false;
							result = _processDispatcher.Stop(connectionString, out server, out identity, out user, out database, ref queue, _appDomainKey, out appDomainStop2);
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
				}
			}
			return result;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private static bool AddToServerUserHash(string server, IdentityUserNamePair identityUser, DatabaseServicePair databaseService)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.AddToServerUserHash|DEP> server: '{0}', database: '{1}', service: '{2}'", server, databaseService.Database, databaseService.Service);
		try
		{
			bool result = false;
			lock (_serverUserHash)
			{
				Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary;
				if (!_serverUserHash.ContainsKey(server))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddToServerUserHash|DEP> Hash did not contain server, adding.");
					dictionary = new Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>();
					_serverUserHash.Add(server, dictionary);
				}
				else
				{
					dictionary = _serverUserHash[server];
				}
				List<DatabaseServicePair> list;
				if (!dictionary.ContainsKey(identityUser))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddToServerUserHash|DEP> Hash contained server but not user, adding user.");
					list = new List<DatabaseServicePair>();
					dictionary.Add(identityUser, list);
				}
				else
				{
					list = dictionary[identityUser];
				}
				if (!list.Contains(databaseService))
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddToServerUserHash|DEP> Adding database.");
					list.Add(databaseService);
					result = true;
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddToServerUserHash|DEP|ERR> ERROR - hash already contained server, user, and database - we will throw!.");
				}
			}
			return result;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private static void RemoveFromServerUserHash(string server, IdentityUserNamePair identityUser, DatabaseServicePair databaseService)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP> server: '{0}', database: '{1}', service: '{2}'", server, databaseService.Database, databaseService.Service);
		try
		{
			lock (_serverUserHash)
			{
				if (_serverUserHash.ContainsKey(server))
				{
					Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary = _serverUserHash[server];
					if (dictionary.ContainsKey(identityUser))
					{
						List<DatabaseServicePair> list = dictionary[identityUser];
						int num = list.IndexOf(databaseService);
						if (num >= 0)
						{
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP> Hash contained server, user, and database - removing database.");
							list.RemoveAt(num);
							if (list.Count == 0)
							{
								SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP> databaseServiceList count 0, removing the list for this server and user.");
								dictionary.Remove(identityUser);
								if (dictionary.Count == 0)
								{
									SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP> identityDatabaseHash count 0, removing the hash for this server.");
									_serverUserHash.Remove(server);
								}
							}
						}
						else
						{
							SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash contained server and user but not database!");
						}
					}
					else
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash contained server but not user!");
					}
				}
				else
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash did not contain server!");
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal static string GetDefaultComposedOptions(string server, string failoverServer, IdentityUserNamePair identityUser, string database)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP> server: '{0}', failoverServer: '{1}', database: '{2}'", server, failoverServer, database);
		try
		{
			string text2;
			lock (_serverUserHash)
			{
				if (!_serverUserHash.ContainsKey(server))
				{
					if (_serverUserHash.Count == 0)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - no start calls have been made, about to throw.");
						throw SQL.SqlDepDefaultOptionsButNoStart();
					}
					if (ADP.IsEmpty(failoverServer) || !_serverUserHash.ContainsKey(failoverServer))
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - not listening to this server, about to throw.");
						throw SQL.SqlDependencyNoMatchingServerStart();
					}
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP> using failover server instead\n");
					server = failoverServer;
				}
				Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary = _serverUserHash[server];
				List<DatabaseServicePair> list = null;
				if (!dictionary.ContainsKey(identityUser))
				{
					if (dictionary.Count > 1)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - not listening for this user, but listening to more than one other user, about to throw.");
						throw SQL.SqlDependencyNoMatchingServerStart();
					}
					using Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>.Enumerator enumerator = dictionary.GetEnumerator();
					if (enumerator.MoveNext())
					{
						list = enumerator.Current.Value;
					}
				}
				else
				{
					list = dictionary[identityUser];
				}
				DatabaseServicePair item = new DatabaseServicePair(database, null);
				DatabaseServicePair databaseServicePair = null;
				int num = list.IndexOf(item);
				if (num != -1)
				{
					databaseServicePair = list[num];
				}
				if (databaseServicePair != null)
				{
					database = FixupServiceOrDatabaseName(databaseServicePair.Database);
					string text = FixupServiceOrDatabaseName(databaseServicePair.Service);
					text2 = "Service=" + text + ";Local Database=" + database;
				}
				else
				{
					if (list.Count != 1)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - SqlDependency.Start called multiple times for this server/user, but no matching database.");
						throw SQL.SqlDependencyNoMatchingServerDatabaseStart();
					}
					object[] array = list.ToArray();
					object[] array2 = array;
					databaseServicePair = (DatabaseServicePair)array2[0];
					string text3 = FixupServiceOrDatabaseName(databaseServicePair.Database);
					string text4 = FixupServiceOrDatabaseName(databaseServicePair.Service);
					text2 = "Service=" + text4 + ";Local Database=" + text3;
				}
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.GetDefaultComposedOptions|DEP> resulting options: '{0}'.", text2);
			return text2;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void AddToServerList(string server)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.AddToServerList|DEP> {0}, server: '{1}'", ObjectID, server);
		try
		{
			lock (_serverList)
			{
				int num = _serverList.BinarySearch(server, StringComparer.OrdinalIgnoreCase);
				if (0 > num)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddToServerList|DEP> Server not present in hashtable, adding server: '{0}'.", server);
					num = ~num;
					_serverList.Insert(num, server);
				}
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal bool ContainsServer(string server)
	{
		lock (_serverList)
		{
			return _serverList.Contains(server);
		}
	}

	internal string ComputeHashAndAddToDispatcher(SqlCommand command)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.ComputeHashAndAddToDispatcher|DEP> {0}, SqlCommand: {1}", ObjectID, command.ObjectID);
		try
		{
			string commandHash = ComputeCommandHash(command.Connection.ConnectionString, command);
			string text = SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddCommandEntry(commandHash, this);
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ComputeHashAndAddToDispatcher|DEP> computed id string: '{0}'.", text);
			return text;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void Invalidate(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.Invalidate|DEP> {0}", ObjectID);
		try
		{
			List<EventContextPair> list = null;
			lock (_eventHandlerLock)
			{
				if (_dependencyFired && SqlNotificationInfo.AlreadyChanged != info && SqlNotificationSource.Client != source)
				{
					if (ExpirationTime < DateTime.UtcNow)
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Invalidate|DEP> ignore notification received after timeout!");
					}
					else
					{
						SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Invalidate|DEP|ERR> ERROR - notification received twice - we should never enter this state!");
					}
				}
				else
				{
					_dependencyFired = true;
					list = _eventList;
					_eventList = new List<EventContextPair>();
				}
			}
			if (list == null)
			{
				return;
			}
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.Invalidate|DEP> Firing events.");
			foreach (EventContextPair item in list)
			{
				item.Invoke(new SqlNotificationEventArgs(type, info, source));
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal void StartTimer(SqlNotificationRequest notificationRequest)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.StartTimer|DEP> {0}", ObjectID);
		try
		{
			if (_expirationTime == DateTime.MaxValue)
			{
				SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.StartTimer|DEP> We've timed out, executing logic.");
				int num = 432000;
				if (_timeout != 0)
				{
					num = _timeout;
				}
				if (notificationRequest != null && notificationRequest.Timeout < num && notificationRequest.Timeout != 0)
				{
					num = notificationRequest.Timeout;
				}
				_expirationTime = DateTime.UtcNow.AddSeconds(num);
				SqlDependencyPerAppDomainDispatcher.SingletonInstance.StartTimer(this);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private void AddCommandInternal(SqlCommand cmd)
	{
		if (cmd == null)
		{
			return;
		}
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.AddCommandInternal|DEP> {0}, SqlCommand: {1}", ObjectID, cmd.ObjectID);
		try
		{
			SqlConnection connection = cmd.Connection;
			if (cmd.Notification != null)
			{
				if (cmd._sqlDep == null || cmd._sqlDep != this)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddCommandInternal|DEP|ERR> ERROR - throwing command has existing SqlNotificationRequest exception.");
					throw SQL.SqlCommandHasExistingSqlNotificationRequest();
				}
				return;
			}
			bool flag = false;
			lock (_eventHandlerLock)
			{
				if (!_dependencyFired)
				{
					cmd.Notification = new SqlNotificationRequest();
					cmd.Notification.Timeout = _timeout;
					if (_options != null)
					{
						cmd.Notification.Options = _options;
					}
					cmd._sqlDep = this;
				}
				else if (_eventList.Count == 0)
				{
					SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.AddCommandInternal|DEP|ERR> ERROR - firing events, though it is unexpected we have events at this point.");
					flag = true;
				}
			}
			if (flag)
			{
				Invalidate(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
			}
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	private string ComputeCommandHash(string connectionString, SqlCommand command)
	{
		long scopeId = SqlClientEventSource.Log.TryNotificationScopeEnterEvent("<sc.SqlDependency.ComputeCommandHash|DEP> {0}, SqlCommand: {1}", ObjectID, command.ObjectID);
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0};{1}", connectionString, command.CommandText);
			for (int i = 0; i < command.Parameters.Count; i++)
			{
				object value = command.Parameters[i].Value;
				if (value == null || value == DBNull.Value)
				{
					stringBuilder.Append("; NULL");
					continue;
				}
				Type type = value.GetType();
				if (type == typeof(byte[]))
				{
					stringBuilder.Append(";");
					byte[] array = (byte[])value;
					for (int j = 0; j < array.Length; j++)
					{
						stringBuilder.Append(array[j].ToString("x2", CultureInfo.InvariantCulture));
					}
				}
				else if (type == typeof(char[]))
				{
					stringBuilder.Append((char[])value);
				}
				else if (type == typeof(XmlReader))
				{
					stringBuilder.Append(";");
					stringBuilder.Append(Guid.NewGuid().ToString());
				}
				else
				{
					stringBuilder.Append(";");
					stringBuilder.Append(value.ToString());
				}
			}
			string text = stringBuilder.ToString();
			SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.SqlDependency.ComputeCommandHash|DEP> ComputeCommandHash result: '{0}'.", text);
			return text;
		}
		finally
		{
			SqlClientEventSource.Log.TryNotificationScopeLeaveEvent(scopeId);
		}
	}

	internal static string FixupServiceOrDatabaseName(string name)
	{
		if (!ADP.IsEmpty(name))
		{
			return "\"" + name.Replace("\"", "\"\"") + "\"";
		}
		return name;
	}
}
