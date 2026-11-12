using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.ProviderBase;

internal abstract class DbConnectionPoolCounters
{
	private static class CreationData
	{
		internal static readonly CounterCreationData HardConnectsPerSecond = new CounterCreationData("HardConnectsPerSecond", "The number of actual connections per second that are being made to servers", (PerformanceCounterType)272696320);

		internal static readonly CounterCreationData HardDisconnectsPerSecond = new CounterCreationData("HardDisconnectsPerSecond", "The number of actual disconnects per second that are being made to servers", (PerformanceCounterType)272696320);

		internal static readonly CounterCreationData SoftConnectsPerSecond = new CounterCreationData("SoftConnectsPerSecond", "The number of connections we get from the pool per second", (PerformanceCounterType)272696320);

		internal static readonly CounterCreationData SoftDisconnectsPerSecond = new CounterCreationData("SoftDisconnectsPerSecond", "The number of connections we return to the pool per second", (PerformanceCounterType)272696320);

		internal static readonly CounterCreationData NumberOfNonPooledConnections = new CounterCreationData("NumberOfNonPooledConnections", "The number of connections that are not using connection pooling", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfPooledConnections = new CounterCreationData("NumberOfPooledConnections", "The number of connections that are managed by the connection pooler", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfActiveConnectionPoolGroups = new CounterCreationData("NumberOfActiveConnectionPoolGroups", "The number of unique connection strings", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfInactiveConnectionPoolGroups = new CounterCreationData("NumberOfInactiveConnectionPoolGroups", "The number of unique connection strings waiting for pruning", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfActiveConnectionPools = new CounterCreationData("NumberOfActiveConnectionPools", "The number of connection pools", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfInactiveConnectionPools = new CounterCreationData("NumberOfInactiveConnectionPools", "The number of connection pools", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfActiveConnections = new CounterCreationData("NumberOfActiveConnections", "The number of connections currently in-use", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfFreeConnections = new CounterCreationData("NumberOfFreeConnections", "The number of connections currently available for use", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfStasisConnections = new CounterCreationData("NumberOfStasisConnections", "The number of connections currently waiting to be made ready for use", (PerformanceCounterType)65536);

		internal static readonly CounterCreationData NumberOfReclaimedConnections = new CounterCreationData("NumberOfReclaimedConnections", "The number of connections we reclaim from GC'd external connections", (PerformanceCounterType)65536);
	}

	internal sealed class Counter
	{
		private PerformanceCounter _instance;

		internal Counter(string categoryName, string instanceName, string counterName, PerformanceCounterType counterType)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			if (!ADP.IsPlatformNT5)
			{
				return;
			}
			try
			{
				if (!ADP.IsEmpty(categoryName) && !ADP.IsEmpty(instanceName))
				{
					PerformanceCounter val = new PerformanceCounter();
					val.CategoryName = categoryName;
					val.CounterName = counterName;
					val.InstanceName = instanceName;
					val.InstanceLifetime = (PerformanceCounterInstanceLifetime)1;
					val.ReadOnly = false;
					val.RawValue = 0L;
					_instance = val;
				}
			}
			catch (InvalidOperationException e)
			{
				ADP.TraceExceptionWithoutRethrow(e);
			}
		}

		internal void Decrement()
		{
			PerformanceCounter instance = _instance;
			if (instance != null)
			{
				instance.Decrement();
			}
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void Dispose()
		{
			PerformanceCounter instance = _instance;
			_instance = null;
			if (instance != null)
			{
				instance.RemoveInstance();
			}
		}

		internal void Increment()
		{
			PerformanceCounter instance = _instance;
			if (instance != null)
			{
				instance.Increment();
			}
		}
	}

	private const int CounterInstanceNameMaxLength = 127;

	internal readonly Counter HardConnectsPerSecond;

	internal readonly Counter HardDisconnectsPerSecond;

	internal readonly Counter SoftConnectsPerSecond;

	internal readonly Counter SoftDisconnectsPerSecond;

	internal readonly Counter NumberOfNonPooledConnections;

	internal readonly Counter NumberOfPooledConnections;

	internal readonly Counter NumberOfActiveConnectionPoolGroups;

	internal readonly Counter NumberOfInactiveConnectionPoolGroups;

	internal readonly Counter NumberOfActiveConnectionPools;

	internal readonly Counter NumberOfInactiveConnectionPools;

	internal readonly Counter NumberOfActiveConnections;

	internal readonly Counter NumberOfFreeConnections;

	internal readonly Counter NumberOfStasisConnections;

	internal readonly Counter NumberOfReclaimedConnections;

	protected DbConnectionPoolCounters()
		: this(null, null)
	{
	}

	protected DbConnectionPoolCounters(string categoryName, string categoryHelp)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		AppDomain.CurrentDomain.DomainUnload += UnloadEventHandler;
		AppDomain.CurrentDomain.ProcessExit += ExitEventHandler;
		AppDomain.CurrentDomain.UnhandledException += ExceptionEventHandler;
		string instanceName = null;
		if (!ADP.IsEmpty(categoryName) && ADP.IsPlatformNT5)
		{
			instanceName = GetInstanceName();
		}
		HardConnectsPerSecond = new Counter(categoryName, instanceName, CreationData.HardConnectsPerSecond.CounterName, CreationData.HardConnectsPerSecond.CounterType);
		HardDisconnectsPerSecond = new Counter(categoryName, instanceName, CreationData.HardDisconnectsPerSecond.CounterName, CreationData.HardDisconnectsPerSecond.CounterType);
		NumberOfNonPooledConnections = new Counter(categoryName, instanceName, CreationData.NumberOfNonPooledConnections.CounterName, CreationData.NumberOfNonPooledConnections.CounterType);
		NumberOfPooledConnections = new Counter(categoryName, instanceName, CreationData.NumberOfPooledConnections.CounterName, CreationData.NumberOfPooledConnections.CounterType);
		NumberOfActiveConnectionPoolGroups = new Counter(categoryName, instanceName, CreationData.NumberOfActiveConnectionPoolGroups.CounterName, CreationData.NumberOfActiveConnectionPoolGroups.CounterType);
		NumberOfInactiveConnectionPoolGroups = new Counter(categoryName, instanceName, CreationData.NumberOfInactiveConnectionPoolGroups.CounterName, CreationData.NumberOfInactiveConnectionPoolGroups.CounterType);
		NumberOfActiveConnectionPools = new Counter(categoryName, instanceName, CreationData.NumberOfActiveConnectionPools.CounterName, CreationData.NumberOfActiveConnectionPools.CounterType);
		NumberOfInactiveConnectionPools = new Counter(categoryName, instanceName, CreationData.NumberOfInactiveConnectionPools.CounterName, CreationData.NumberOfInactiveConnectionPools.CounterType);
		NumberOfStasisConnections = new Counter(categoryName, instanceName, CreationData.NumberOfStasisConnections.CounterName, CreationData.NumberOfStasisConnections.CounterType);
		NumberOfReclaimedConnections = new Counter(categoryName, instanceName, CreationData.NumberOfReclaimedConnections.CounterName, CreationData.NumberOfReclaimedConnections.CounterType);
		string categoryName2 = null;
		if (!ADP.IsEmpty(categoryName))
		{
			TraceSwitch traceSwitch = new TraceSwitch("ConnectionPoolPerformanceCounterDetail", "level of detail to track with connection pool performance counters");
			if (TraceLevel.Verbose == traceSwitch.Level)
			{
				categoryName2 = categoryName;
			}
		}
		SoftConnectsPerSecond = new Counter(categoryName2, instanceName, CreationData.SoftConnectsPerSecond.CounterName, CreationData.SoftConnectsPerSecond.CounterType);
		SoftDisconnectsPerSecond = new Counter(categoryName2, instanceName, CreationData.SoftDisconnectsPerSecond.CounterName, CreationData.SoftDisconnectsPerSecond.CounterType);
		NumberOfActiveConnections = new Counter(categoryName2, instanceName, CreationData.NumberOfActiveConnections.CounterName, CreationData.NumberOfActiveConnections.CounterType);
		NumberOfFreeConnections = new Counter(categoryName2, instanceName, CreationData.NumberOfFreeConnections.CounterName, CreationData.NumberOfFreeConnections.CounterType);
	}

	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	private string GetAssemblyName()
	{
		string result = null;
		Assembly entryAssembly = Assembly.GetEntryAssembly();
		if (null != entryAssembly)
		{
			AssemblyName name = entryAssembly.GetName();
			if (name != null)
			{
				result = name.Name;
			}
		}
		return result;
	}

	private string GetInstanceName()
	{
		string text = null;
		string text2 = GetAssemblyName();
		if (ADP.IsEmpty(text2))
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			if (currentDomain != null)
			{
				text2 = currentDomain.FriendlyName;
			}
		}
		int currentProcessId = Microsoft.Data.Common.SafeNativeMethods.GetCurrentProcessId();
		text = string.Format(null, "{0}[{1}]", text2, currentProcessId);
		text = text.Replace('(', '[').Replace(')', ']').Replace('#', '_')
			.Replace('/', '_')
			.Replace('\\', '_');
		if (text.Length > 127)
		{
			int num = (127 - "[...]".Length) / 2;
			int num2 = 127 - num - "[...]".Length;
			text = string.Format(null, "{0}{1}{2}", text.Substring(0, num), "[...]", text.Substring(text.Length - num2, num2));
		}
		return text;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public void Dispose()
	{
		SafeDispose(HardConnectsPerSecond);
		SafeDispose(HardDisconnectsPerSecond);
		SafeDispose(SoftConnectsPerSecond);
		SafeDispose(SoftDisconnectsPerSecond);
		SafeDispose(NumberOfNonPooledConnections);
		SafeDispose(NumberOfPooledConnections);
		SafeDispose(NumberOfActiveConnectionPoolGroups);
		SafeDispose(NumberOfInactiveConnectionPoolGroups);
		SafeDispose(NumberOfActiveConnectionPools);
		SafeDispose(NumberOfActiveConnections);
		SafeDispose(NumberOfFreeConnections);
		SafeDispose(NumberOfStasisConnections);
		SafeDispose(NumberOfReclaimedConnections);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	private void SafeDispose(Counter counter)
	{
		counter?.Dispose();
	}

	[PrePrepareMethod]
	private void ExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
	{
		if (e != null && e.IsTerminating)
		{
			Dispose();
		}
	}

	[PrePrepareMethod]
	private void ExitEventHandler(object sender, EventArgs e)
	{
		Dispose();
	}

	[PrePrepareMethod]
	private void UnloadEventHandler(object sender, EventArgs e)
	{
		Dispose();
	}
}
