using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Microsoft.Data.SqlClient;

[EventSource(Name = "Microsoft.Data.SqlClient.EventSource")]
internal class SqlClientEventSource : SqlClientEventSourceBase
{
	public class Keywords
	{
		internal const EventKeywords ExecutionTrace = (EventKeywords)1L;

		internal const EventKeywords Trace = (EventKeywords)2L;

		internal const EventKeywords Scope = (EventKeywords)4L;

		internal const EventKeywords NotificationTrace = (EventKeywords)8L;

		internal const EventKeywords NotificationScope = (EventKeywords)16L;

		internal const EventKeywords PoolerTrace = (EventKeywords)32L;

		internal const EventKeywords PoolerScope = (EventKeywords)64L;

		internal const EventKeywords AdvancedTrace = (EventKeywords)128L;

		internal const EventKeywords AdvancedTraceBin = (EventKeywords)256L;

		internal const EventKeywords CorrelationTrace = (EventKeywords)512L;

		internal const EventKeywords StateDump = (EventKeywords)1024L;

		internal const EventKeywords SNITrace = (EventKeywords)2048L;

		internal const EventKeywords SNIScope = (EventKeywords)4096L;
	}

	public static class Tasks
	{
		public const EventTask ExecuteCommand = (EventTask)1;

		public const EventTask Scope = (EventTask)2;

		public const EventTask PoolerScope = (EventTask)3;

		public const EventTask NotificationScope = (EventTask)4;

		public const EventTask SNIScope = (EventTask)5;
	}

	internal static readonly SqlClientEventSource Log = new SqlClientEventSource();

	private const string NullStr = "null";

	private const string SqlCommand_ClassName = "SqlCommand";

	private static long s_nextScopeId = 0L;

	private static long s_nextNotificationScopeId = 0L;

	private static long s_nextPoolerScopeId = 0L;

	private static long s_nextSNIScopeId = 0L;

	private const int BeginExecuteEventId = 1;

	private const int EndExecuteEventId = 2;

	private const int TraceEventId = 3;

	private const int ScopeEnterId = 4;

	private const int ScopeExitId = 5;

	private const int NotificationScopeEnterId = 6;

	private const int NotificationScopeExitId = 7;

	private const int NotificationTraceId = 8;

	private const int PoolerScopeEnterId = 9;

	private const int PoolerScopeExitId = 10;

	private const int PoolerTraceId = 11;

	private const int AdvancedTraceId = 12;

	private const int AdvancedScopeEnterId = 13;

	private const int AdvancedScopeExitId = 14;

	private const int AdvancedTraceBinId = 15;

	private const int AdvancedTraceErrorId = 16;

	private const int CorrelationTraceId = 17;

	private const int StateDumpEventId = 18;

	private const int SNITraceEventId = 19;

	private const int SNIScopeEnterId = 20;

	private const int SNIScopeExitId = 21;

	[NonEvent]
	internal bool IsExecutionTraceEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)1L);
	}

	[NonEvent]
	internal bool IsTraceEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)2L);
	}

	[NonEvent]
	internal bool IsScopeEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)4L);
	}

	[NonEvent]
	internal bool IsNotificationTraceEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)8L);
	}

	[NonEvent]
	internal bool IsNotificationScopeEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)16L);
	}

	[NonEvent]
	internal bool IsPoolerTraceEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)32L);
	}

	[NonEvent]
	internal bool IsPoolerScopeEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)64L);
	}

	[NonEvent]
	internal bool IsAdvancedTraceOn()
	{
		return Log.IsEnabled(EventLevel.Verbose, (EventKeywords)128L);
	}

	[NonEvent]
	internal bool IsCorrelationEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)512L);
	}

	[NonEvent]
	internal bool IsStateDumpEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)1024L);
	}

	[NonEvent]
	internal bool IsSNITraceEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)2048L);
	}

	[NonEvent]
	internal bool IsSNIScopeEnabled()
	{
		return Log.IsEnabled(EventLevel.Informational, (EventKeywords)4096L);
	}

	private string GetFormattedMessage(string className, string memberName, string eventType, string message)
	{
		return new StringBuilder(className).Append(".").Append(memberName).Append(eventType)
			.Append(message)
			.ToString();
	}

	[NonEvent]
	internal void TraceEvent<T0, T1>(string message, T0 args0, T1 args1, [CallerMemberName] string memberName = "")
	{
		Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TryTraceEvent(string message)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(message);
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0>(string message, T0 args0)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0, T1, T2, T3, T4>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryTraceEvent<T0, T1, T2, T3, T4, T5>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
	{
		if (Log.IsTraceEnabled())
		{
			Trace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null", args5?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal long TryScopeEnterEvent(string className, [CallerMemberName] string memberName = "")
	{
		if (Log.IsScopeEnabled())
		{
			StringBuilder stringBuilder = new StringBuilder(className);
			stringBuilder.Append(".").Append(memberName).Append(" | INFO | SCOPE | Entering Scope {0}");
			return SNIScopeEnter(stringBuilder.ToString());
		}
		return 0L;
	}

	[NonEvent]
	internal long TryScopeEnterEvent<T0>(string message, T0 args0)
	{
		if (Log.IsScopeEnabled())
		{
			return ScopeEnter(string.Format(message, args0?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryScopeEnterEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsScopeEnabled())
		{
			return ScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryScopeEnterEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsScopeEnabled())
		{
			return ScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryScopeEnterEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsScopeEnabled())
		{
			return ScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal void TryScopeLeaveEvent(long scopeId)
	{
		if (Log.IsScopeEnabled())
		{
			ScopeLeave($"Exit Scope {scopeId}");
		}
	}

	[NonEvent]
	internal void TryBeginExecuteEvent(int objectId, Guid? connectionId, string commandText, [CallerMemberName] string memberName = "")
	{
		if (Log.IsExecutionTraceEnabled())
		{
			BeginExecute(GetFormattedMessage("SqlCommand", memberName, " | INFO | ", $"Object Id {objectId}, Client Connection Id {connectionId}, Command Text {commandText}"));
		}
	}

	[NonEvent]
	internal void TryEndExecuteEvent(int objectId, Guid? connectionId, int compositeState, int sqlExceptionNumber, [CallerMemberName] string memberName = "")
	{
		if (Log.IsExecutionTraceEnabled())
		{
			EndExecute(GetFormattedMessage("SqlCommand", memberName, " | INFO | ", $"Object Id {objectId}, Client Connection Id {connectionId}, Composite State {compositeState}, Sql Exception Number {sqlExceptionNumber}"));
		}
	}

	[NonEvent]
	internal void NotificationTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		NotificationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TryNotificationTraceEvent(string message)
	{
		if (Log.IsNotificationTraceEnabled())
		{
			NotificationTrace(message);
		}
	}

	[NonEvent]
	internal void TryNotificationTraceEvent<T0>(string message, T0 args0)
	{
		if (Log.IsNotificationTraceEnabled())
		{
			NotificationTrace(string.Format(message, args0?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryNotificationTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsNotificationTraceEnabled())
		{
			NotificationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryNotificationTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsNotificationTraceEnabled())
		{
			NotificationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryNotificationTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsNotificationTraceEnabled())
		{
			NotificationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal long TryNotificationScopeEnterEvent<T0>(string message, T0 args0)
	{
		if (Log.IsNotificationScopeEnabled())
		{
			return NotificationScopeEnter(string.Format(message, args0?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryNotificationScopeEnterEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsNotificationScopeEnabled())
		{
			return NotificationScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryNotificationScopeEnterEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsNotificationScopeEnabled())
		{
			return NotificationScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal long TryNotificationScopeEnterEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsNotificationScopeEnabled())
		{
			return NotificationScopeEnter(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal void TryNotificationScopeLeaveEvent(long scopeId)
	{
		if (Log.IsNotificationScopeEnabled())
		{
			NotificationScopeLeave($"Exit Notification Scope {scopeId}");
		}
	}

	[NonEvent]
	internal void TryPoolerTraceEvent<T0>(string message, T0 args0)
	{
		if (Log.IsPoolerTraceEnabled())
		{
			PoolerTrace(string.Format(message, args0?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryPoolerTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsPoolerTraceEnabled())
		{
			PoolerTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryPoolerTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsPoolerTraceEnabled())
		{
			PoolerTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryPoolerTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsPoolerTraceEnabled())
		{
			PoolerTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal long TryPoolerScopeEnterEvent<T0>(string message, T0 args0)
	{
		if (Log.IsPoolerScopeEnabled())
		{
			return PoolerScopeEnter(string.Format(message, args0?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal void TryPoolerScopeLeaveEvent(long scopeId)
	{
		if (Log.IsPoolerScopeEnabled())
		{
			PoolerScopeLeave($"Exit Pooler Scope {scopeId}");
		}
	}

	[NonEvent]
	internal void AdvancedTraceEvent<T0>(string message, T0 args0)
	{
		AdvancedTrace(string.Format(message, args0?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void AdvancedTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void AdvancedTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void AdvancedTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent(string message)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(message);
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0>(string message, T0 args0)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1, T2, T3, T4>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1, T2, T3, T4, T5>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null", args5?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceEvent<T0, T1, T2, T3, T4, T5, T6, T7>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5, T6 args6, T7 args7)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null", args5?.ToString() ?? "null", args6?.ToString() ?? "null", args7?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal long TryAdvancedScopeEnterEvent<T0>(string message, T0 args0)
	{
		if (Log.IsAdvancedTraceOn())
		{
			return AdvancedScopeEnter(string.Format(message, args0?.ToString() ?? "null"));
		}
		return 0L;
	}

	[NonEvent]
	internal void TryAdvanceScopeLeave(long scopeId)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedScopeLeave($"Exit Advanced Scope {scopeId}");
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceBinEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTraceBin(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryAdvancedTraceErrorEvent<T0, T1, T2, T3, T4>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4)
	{
		if (Log.IsAdvancedTraceOn())
		{
			AdvancedTraceError(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0>(string message, T0 args0)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0, T1, T2, T3, T4>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void TryCorrelationTraceEvent<T0, T1, T2, T3, T4, T5>(string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
	{
		if (Log.IsCorrelationEnabled())
		{
			CorrelationTrace(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null", args5?.ToString() ?? "null"));
		}
	}

	[NonEvent]
	internal void StateDumpEvent<T0, T1>(string message, T0 args0, T1 args1)
	{
		StateDump(string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null"));
	}

	[NonEvent]
	internal void TrySNITraceEvent(string className, string eventType, string message, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, message));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0>(string className, string eventType, string message, T0 args0, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0, T1>(string className, string eventType, string message, T0 args0, T1 args1, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0, T1, T2>(string className, string eventType, string message, T0 args0, T1 args1, T2 args2, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0, T1, T2, T3>(string className, string eventType, string message, T0 args0, T1 args1, T2 args2, T3 args3, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0, T1, T2, T3, T4>(string className, string eventType, string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal void TrySNITraceEvent<T0, T1, T2, T3, T4, T5>(string className, string eventType, string message, T0 args0, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNITraceEnabled())
		{
			SNITrace(GetFormattedMessage(className, memberName, eventType, string.Format(message, args0?.ToString() ?? "null", args1?.ToString() ?? "null", args2?.ToString() ?? "null", args3?.ToString() ?? "null", args4?.ToString() ?? "null", args5?.ToString() ?? "null")));
		}
	}

	[NonEvent]
	internal long TrySNIScopeEnterEvent(string className, [CallerMemberName] string memberName = "")
	{
		if (Log.IsSNIScopeEnabled())
		{
			StringBuilder stringBuilder = new StringBuilder(className);
			stringBuilder.Append(".").Append(memberName).Append(" | SNI | INFO | SCOPE | Entering Scope {0}");
			return SNIScopeEnter(stringBuilder.ToString());
		}
		return 0L;
	}

	[NonEvent]
	internal void TrySNIScopeLeaveEvent(long scopeId)
	{
		if (Log.IsSNIScopeEnabled())
		{
			SNIScopeLeave($"Exit SNI Scope {scopeId}");
		}
	}

	[Event(1, Keywords = (EventKeywords)1L, Task = (EventTask)1, Opcode = EventOpcode.Start)]
	internal void BeginExecute(string message)
	{
		WriteEvent(1, message);
	}

	[Event(2, Keywords = (EventKeywords)1L, Task = (EventTask)1, Opcode = EventOpcode.Stop)]
	internal void EndExecute(string message)
	{
		WriteEvent(2, message);
	}

	[Event(3, Level = EventLevel.Informational, Keywords = (EventKeywords)2L)]
	internal void Trace(string message)
	{
		WriteEvent(3, message);
	}

	[Event(4, Level = EventLevel.Informational, Task = (EventTask)2, Opcode = EventOpcode.Start, Keywords = (EventKeywords)4L)]
	internal long ScopeEnter(string message)
	{
		long num = Interlocked.Increment(ref s_nextScopeId);
		WriteEvent(4, string.Format(message, num));
		return num;
	}

	[Event(5, Level = EventLevel.Informational, Task = (EventTask)2, Opcode = EventOpcode.Stop, Keywords = (EventKeywords)4L)]
	internal void ScopeLeave(string message)
	{
		WriteEvent(5, message);
	}

	[Event(8, Level = EventLevel.Informational, Keywords = (EventKeywords)8L)]
	internal void NotificationTrace(string message)
	{
		WriteEvent(8, message);
	}

	[Event(6, Level = EventLevel.Informational, Task = (EventTask)4, Opcode = EventOpcode.Start, Keywords = (EventKeywords)16L)]
	internal long NotificationScopeEnter(string message)
	{
		long num = Interlocked.Increment(ref s_nextNotificationScopeId);
		WriteEvent(6, string.Format(message, num));
		return num;
	}

	[Event(7, Level = EventLevel.Informational, Task = (EventTask)4, Opcode = EventOpcode.Stop, Keywords = (EventKeywords)16L)]
	internal void NotificationScopeLeave(string message)
	{
		WriteEvent(7, message);
	}

	[Event(11, Level = EventLevel.Informational, Keywords = (EventKeywords)32L)]
	internal void PoolerTrace(string message)
	{
		WriteEvent(11, message);
	}

	[Event(9, Level = EventLevel.Informational, Task = (EventTask)3, Opcode = EventOpcode.Start, Keywords = (EventKeywords)64L)]
	internal long PoolerScopeEnter(string message)
	{
		long num = Interlocked.Increment(ref s_nextPoolerScopeId);
		WriteEvent(9, string.Format(message, num));
		return num;
	}

	[Event(10, Level = EventLevel.Informational, Task = (EventTask)3, Opcode = EventOpcode.Stop, Keywords = (EventKeywords)64L)]
	internal void PoolerScopeLeave(string message)
	{
		WriteEvent(10, message);
	}

	[Event(12, Level = EventLevel.Verbose, Keywords = (EventKeywords)128L)]
	internal void AdvancedTrace(string message)
	{
		WriteEvent(12, message);
	}

	[Event(13, Level = EventLevel.Verbose, Opcode = EventOpcode.Start, Keywords = (EventKeywords)128L)]
	internal long AdvancedScopeEnter(string message)
	{
		long num = Interlocked.Increment(ref s_nextScopeId);
		WriteEvent(13, string.Format(message, num));
		return num;
	}

	[Event(14, Level = EventLevel.Verbose, Opcode = EventOpcode.Stop, Keywords = (EventKeywords)128L)]
	internal void AdvancedScopeLeave(string message)
	{
		WriteEvent(14, message);
	}

	[Event(15, Level = EventLevel.Verbose, Keywords = (EventKeywords)256L)]
	internal void AdvancedTraceBin(string message)
	{
		WriteEvent(15, message);
	}

	[Event(16, Level = EventLevel.Error, Keywords = (EventKeywords)128L)]
	internal void AdvancedTraceError(string message)
	{
		WriteEvent(16, message);
	}

	[Event(17, Level = EventLevel.Informational, Keywords = (EventKeywords)512L)]
	internal void CorrelationTrace(string message)
	{
		WriteEvent(17, message);
	}

	[Event(18, Level = EventLevel.Verbose, Keywords = (EventKeywords)1024L)]
	internal void StateDump(string message)
	{
		WriteEvent(18, message);
	}

	[Event(19, Level = EventLevel.Informational, Keywords = (EventKeywords)2048L)]
	internal void SNITrace(string message)
	{
		WriteEvent(19, message);
	}

	[Event(20, Level = EventLevel.Informational, Task = (EventTask)5, Opcode = EventOpcode.Start, Keywords = (EventKeywords)4096L)]
	internal long SNIScopeEnter(string message)
	{
		long num = Interlocked.Increment(ref s_nextSNIScopeId);
		WriteEvent(20, string.Format(message, num));
		return num;
	}

	[Event(21, Level = EventLevel.Informational, Task = (EventTask)5, Opcode = EventOpcode.Stop, Keywords = (EventKeywords)4096L)]
	internal void SNIScopeLeave(string message)
	{
		WriteEvent(21, message);
	}
}
