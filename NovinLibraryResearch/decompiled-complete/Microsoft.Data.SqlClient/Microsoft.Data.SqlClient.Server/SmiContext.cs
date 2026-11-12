using System;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Principal;
using System.Transactions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

internal abstract class SmiContext
{
	internal abstract SmiConnection ContextConnection { get; }

	internal abstract long ContextTransactionId { get; }

	internal abstract Transaction ContextTransaction { get; }

	internal abstract bool HasContextPipe { get; }

	internal abstract WindowsIdentity WindowsIdentity { get; }

	internal abstract event EventHandler OutOfScope;

	internal abstract SmiRecordBuffer CreateRecordBuffer(SmiExtendedMetaData[] columnMetaData, SmiEventSink eventSink);

	internal abstract SmiRequestExecutor CreateRequestExecutor(string commandText, CommandType commandType, SmiParameterMetaData[] parameterMetaData, SmiEventSink eventSink);

	internal abstract object GetContextValue(int key);

	internal abstract void GetTriggerInfo(SmiEventSink eventSink, out bool[] columnsUpdated, out TriggerAction action, out SqlXml eventInstanceData);

	internal abstract void SendMessageToPipe(string message, SmiEventSink eventSink);

	internal abstract void SendResultsStartToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

	internal abstract void SendResultsRowToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

	internal abstract void SendResultsEndToPipe(SmiRecordBuffer recordBuffer, SmiEventSink eventSink);

	internal abstract void SetContextValue(int key, object value);

	internal virtual SmiStream GetScratchStream(SmiEventSink sink)
	{
		ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		return null;
	}
}
