using System;
using System.Data;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

internal abstract class SmiConnection : IDisposable
{
	internal abstract string GetCurrentDatabase(SmiEventSink eventSink);

	internal abstract void SetCurrentDatabase(string databaseName, SmiEventSink eventSink);

	public virtual void Dispose()
	{
		ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
	}

	public virtual void Close(SmiEventSink eventSink)
	{
		ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
	}

	internal abstract void BeginTransaction(string name, IsolationLevel level, SmiEventSink eventSink);

	internal abstract void CommitTransaction(long transactionId, SmiEventSink eventSink);

	internal abstract void CreateTransactionSavePoint(long transactionId, string name, SmiEventSink eventSink);

	internal abstract byte[] GetDTCAddress(SmiEventSink eventSink);

	internal abstract void EnlistTransaction(byte[] token, SmiEventSink eventSink);

	internal abstract byte[] PromoteTransaction(long transactionId, SmiEventSink eventSink);

	internal abstract void RollbackTransaction(long transactionId, string savePointName, SmiEventSink eventSink);
}
