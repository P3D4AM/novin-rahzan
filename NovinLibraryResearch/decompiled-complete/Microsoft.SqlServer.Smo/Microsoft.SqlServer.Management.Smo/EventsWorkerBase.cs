using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Management;
using System.Text;
using Microsoft.SqlServer.Management.Diagnostics;

namespace Microsoft.SqlServer.Management.Smo;

internal abstract class EventsWorkerBase : IDisposable
{
	private sealed class EventSubscription : IDisposable
	{
		public readonly ManagementEventWatcher EventWatcher;

		public object EventHandlerKey;

		public EventSubscription(ManagementEventWatcher eventWatcher, object eventHandlerKey)
		{
			EventWatcher = eventWatcher;
			EventHandlerKey = eventHandlerKey;
		}

		public void Dispose()
		{
			if (EventWatcher != null)
			{
				EventWatcher.Stop();
				((Component)(object)EventWatcher).Dispose();
			}
		}
	}

	private EventHandlerList eventHandlers = new EventHandlerList();

	private Hashtable eventSubscriptions = new Hashtable();

	private Type eventEnumType;

	private EventSetBase events;

	private bool eventsStarted;

	private ManagementScope managementScope;

	private static readonly object defaultEventHandlerKey = new object();

	protected abstract SqlSmoObject Target { get; }

	internal EventsWorkerBase(SqlSmoObject target, Type eventSetType, Type eventEnumType)
	{
		Server serverObject = target.GetServerObject();
		if (serverObject.ServerVersion.Major < 9)
		{
			throw new InvalidVersionSmoOperationException(serverObject.ServerVersion);
		}
		this.eventEnumType = eventEnumType;
		events = (EventSetBase)Activator.CreateInstance(eventSetType);
		managementScope = serverObject.Events.ManagementScope;
	}

	public EventSetBase GetEventSelection()
	{
		return events.Copy();
	}

	public void AddDefaultEventHandler(ServerEventHandler eventHandler)
	{
		eventHandlers.AddHandler(defaultEventHandlerKey, eventHandler);
	}

	public void RemoveDefaultEventHandler(ServerEventHandler eventHandler)
	{
		eventHandlers.RemoveHandler(defaultEventHandlerKey, eventHandler);
	}

	public void SubscribeToEvents(EventSetBase addEvents, ServerEventHandler eventHandler)
	{
		try
		{
			object eventHandlerKey = GetEventHandlerKey(eventHandler);
			for (int i = 0; i < addEvents.NumberOfElements; i++)
			{
				if (addEvents.GetBitAt(i))
				{
					string eventClass = GetEventClass(i);
					EventSubscription eventSubscription = (EventSubscription)eventSubscriptions[eventClass];
					if (eventSubscription != null)
					{
						TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Updating event handler for event " + eventClass + " on class " + Target.GetType().Name);
						eventSubscription.EventHandlerKey = eventHandlerKey;
					}
					else
					{
						TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Adding subscription for event " + eventClass + " on class " + Target.GetType().Name);
						CreateSubscription(i, eventClass, eventHandlerKey);
						events.SetBitAt(i, value: true);
					}
				}
			}
		}
		catch (Exception innerException)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.CannotSubscribe, this, innerException);
		}
	}

	public void UnsubscribeFromEvents(EventSetBase removeEvents)
	{
		for (int i = 0; i < removeEvents.NumberOfElements; i++)
		{
			if (removeEvents.GetBitAt(i))
			{
				string eventClass = GetEventClass(i);
				EventSubscription eventSubscription = (EventSubscription)eventSubscriptions[eventClass];
				if (eventSubscription != null)
				{
					TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Removing subscription for event " + eventClass + " on class " + Target.GetType().Name);
					eventSubscriptions.Remove(eventClass);
					eventSubscription.Dispose();
					events.SetBitAt(i, value: false);
				}
			}
		}
	}

	public void StartEvents()
	{
		if (eventsStarted)
		{
			return;
		}
		try
		{
			foreach (EventSubscription value in eventSubscriptions.Values)
			{
				TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Starting event subscription on: {0}, query: {1}", value.EventWatcher.Scope.Path, ((ManagementQuery)value.EventWatcher.Query).QueryString);
				value.EventWatcher.Start();
			}
			eventsStarted = true;
		}
		catch (Exception innerException)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.CannotStartSubscription, this, innerException);
		}
	}

	public void StopEvents()
	{
		if (!eventsStarted)
		{
			return;
		}
		foreach (EventSubscription value in eventSubscriptions.Values)
		{
			value.EventWatcher.Stop();
		}
		eventsStarted = false;
	}

	public void Dispose()
	{
		TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Removing all subscriptions");
		foreach (EventSubscription value in eventSubscriptions.Values)
		{
			value.Dispose();
		}
		eventSubscriptions.Clear();
		eventHandlers.Dispose();
	}

	protected abstract EventQuery CreateWqlQuery(string eventClass);

	private object GetEventHandlerKey(ServerEventHandler eventHandler)
	{
		if (eventHandler != null)
		{
			object obj = eventHandlers.FindHandler(eventHandler);
			if (obj == null)
			{
				obj = new object();
				eventHandlers.AddHandler(obj, eventHandler);
			}
			return obj;
		}
		return defaultEventHandlerKey;
	}

	protected virtual string GetEventClass(int eventID)
	{
		return ConvertToEventClass(new StringBuilder(Enum.GetName(eventEnumType, eventID)));
	}

	protected static string ConvertToEventClass(StringBuilder eventName)
	{
		eventName.Replace("DB", "Db");
		for (int i = 1; i < eventName.Length; i++)
		{
			if (char.IsUpper(eventName[i]) || char.IsDigit(eventName[i]))
			{
				eventName.Insert(i++, '_');
			}
		}
		eventName.Replace("Trace", "Trc");
		return eventName.ToString().ToUpper(CultureInfo.InvariantCulture);
	}

	protected static EventType ConvertFromEventClass(string eventClass)
	{
		StringBuilder stringBuilder = new StringBuilder(eventClass);
		for (int i = 1; i < stringBuilder.Length; i++)
		{
			if (stringBuilder[i] == '_')
			{
				stringBuilder.Remove(i, 1);
			}
		}
		stringBuilder.Replace("Trc", "Trace");
		return (EventType)Enum.Parse(typeof(EventType), stringBuilder.ToString(), ignoreCase: true);
	}

	private void CreateSubscription(int eventID, string eventClass, object eventHandlerKey)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		EventQuery val = CreateWqlQuery(eventClass);
		TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Subscription query {0}", ((ManagementQuery)val).QueryString);
		TryConnect();
		ManagementEventWatcher val2 = new ManagementEventWatcher(managementScope, val);
		val2.EventArrived += new EventArrivedEventHandler(OnEventArrived);
		EventSubscription value = new EventSubscription(val2, eventHandlerKey);
		if (eventsStarted)
		{
			val2.Start();
		}
		eventSubscriptions.Add(eventClass, value);
	}

	private void OnEventArrived(object sender, EventArrivedEventArgs args)
	{
		string text = args.NewEvent.ClassPath.ClassName;
		EventType eventType = ConvertFromEventClass(args.NewEvent.ClassPath.ClassName);
		EventSubscription eventSubscription = null;
		TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Received event {0} on class {1}", args.NewEvent.ClassPath.ClassName, Target.GetType().Name);
		while (text != null && text.Length > 0)
		{
			eventSubscription = (EventSubscription)eventSubscriptions[text];
			if (eventSubscription != null)
			{
				ServerEventHandler serverEventHandler = (ServerEventHandler)eventHandlers[eventSubscription.EventHandlerKey];
				if (serverEventHandler != null)
				{
					TraceHelper.Trace(SmoApplication.ModuleName, SmoApplication.trAlways, "Raising event {0} on subscription: {1}", args.NewEvent.ClassPath.ClassName, text);
					serverEventHandler(Target, new ServerEventArgs(eventType, args.NewEvent.Properties));
				}
			}
			text = null;
		}
	}

	private void TryConnect()
	{
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		if (managementScope.IsConnected)
		{
			return;
		}
		try
		{
			managementScope.Connect();
			ManagementPath val = new ManagementPath("Win32Provider.Name=\"MSSQL_ManagementProvider\"");
			new ManagementObject(managementScope, val, new ObjectGetOptions());
		}
		catch (ManagementException val2)
		{
			ManagementException val3 = val2;
			if ((int)val3.ErrorCode == -2147217394 || (int)val3.ErrorCode == -2147217389)
			{
				throw new SmoException(ExceptionTemplatesImpl.WMIProviderNotInstalled(Target.GetServerObject().Name), (Exception)(object)val3);
			}
			throw;
		}
	}

	internal static EventQuery CreateWqlQueryForServer(string eventClass)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return new EventQuery(string.Format(SmoApplication.DefaultCulture, "SELECT * FROM {0}", eventClass));
	}

	internal static EventQuery CreateWqlQueryForDatabase(string eventClass, string databaseName)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		return new EventQuery(string.Format(SmoApplication.DefaultCulture, "SELECT * FROM {0} WHERE DatabaseName = '{1}'", eventClass, EscapeWqlParameter(databaseName)));
	}

	internal static EventQuery CreateWqlQueryForDatabaseObject(string eventClass, string databaseName, string objectName, string objectType)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		return new EventQuery(string.Format(SmoApplication.DefaultCulture, "SELECT * FROM {0} WHERE DatabaseName = '{1}' AND ObjectName = '{2}' AND ObjectType = '{3}'", eventClass, EscapeWqlParameter(databaseName), EscapeWqlParameter(objectName), objectType));
	}

	internal static EventQuery CreateWqlQueryForTargetObject(string eventClass, string databaseName, string schemaName, string objectType, string targetObjectName, string targetObjectType)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		return new EventQuery(string.Format(SmoApplication.DefaultCulture, "SELECT * FROM {0} WHERE DatabaseName = '{1}' AND SchemaName = '{2}' AND ObjectType = '{3}' AND TargetObjectName = '{4}' AND TargetObjectType='{5}'", eventClass, EscapeWqlParameter(databaseName), EscapeWqlParameter(schemaName), objectType, EscapeWqlParameter(targetObjectName), targetObjectType));
	}

	internal static EventQuery CreateWqlQueryForSourceObject(string eventClass, string databaseName, string schemaName, string objectName, string objectType)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		return new EventQuery(string.Format(SmoApplication.DefaultCulture, "SELECT * FROM {0} WHERE DatabaseName = '{1}' AND SchemaName = '{2}' AND ObjectName = '{3}' AND ObjectType='{4}'", eventClass, EscapeWqlParameter(databaseName), EscapeWqlParameter(schemaName), EscapeWqlParameter(objectName), objectType));
	}

	private static string EscapeWqlParameter(string parameter)
	{
		return SqlSmoObject.EscapeString(SqlSmoObject.EscapeString(parameter, '\\'), '\'');
	}
}
