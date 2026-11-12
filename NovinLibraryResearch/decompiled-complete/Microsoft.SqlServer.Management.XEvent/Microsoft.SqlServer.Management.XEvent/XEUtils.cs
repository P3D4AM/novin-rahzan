using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public class XEUtils
{
	internal static void AppendAlterScripts<T>(StringBuilder addScript, StringBuilder dropScript, IEnumerable<T> coll, Session session) where T : ISessionObject
	{
		string separator = ", " + Environment.NewLine;
		using (TraceHelper.TraceContext.GetMethodContext("AppendAlterScripts"))
		{
			addScript.Append(string.Join(separator, (from obj in coll
				where obj.State == SfcObjectState.Pending || obj.State == SfcObjectState.Recreate || (obj.State == SfcObjectState.Existing && obj.IsDirty())
				select obj.GetCreateScript()).ToList()));
			dropScript.Append(string.Join(separator, (from obj in coll
				where obj.State == SfcObjectState.ToBeDropped || obj.State == SfcObjectState.Recreate || (obj.State == SfcObjectState.Existing && obj.IsDirty())
				select obj.GetDropScript()).ToList()));
		}
	}

	public static bool ToBeCreated(SfcObjectState state)
	{
		if (state != SfcObjectState.Pending && state != SfcObjectState.Existing)
		{
			return state == SfcObjectState.Recreate;
		}
		return true;
	}

	public static string ConvertToXsdEnumerationValue(Session.EventRetentionModeEnum retentionMode)
	{
		switch (retentionMode)
		{
		case Session.EventRetentionModeEnum.AllowMultipleEventLoss:
			return "allowMultipleEventLoss";
		case Session.EventRetentionModeEnum.AllowSingleEventLoss:
			return "allowSingleEventLoss";
		case Session.EventRetentionModeEnum.NoEventLoss:
			return "noEventLoss";
		default:
			TraceHelper.TraceContext.Assert(false, "Unknown EventRetentionMode");
			return string.Empty;
		}
	}

	public static string ConvertToXsdEnumerationValue(Session.MemoryPartitionModeEnum partitionMode)
	{
		switch (partitionMode)
		{
		case Session.MemoryPartitionModeEnum.None:
			return "none";
		case Session.MemoryPartitionModeEnum.PerCpu:
			return "perCpu";
		case Session.MemoryPartitionModeEnum.PerNode:
			return "perNode";
		default:
			TraceHelper.TraceContext.Assert(false, "Unknown MemoryPartitionMode");
			return string.Empty;
		}
	}
}
