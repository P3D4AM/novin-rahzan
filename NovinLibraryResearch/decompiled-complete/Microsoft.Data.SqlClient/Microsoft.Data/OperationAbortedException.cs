using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft.Data.Common;

namespace Microsoft.Data;

[Serializable]
[TypeForwardedFrom("System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
public sealed class OperationAbortedException : SystemException
{
	private OperationAbortedException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232010;
	}

	private OperationAbortedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	internal static OperationAbortedException Aborted(Exception inner)
	{
		OperationAbortedException ex = ((inner != null) ? new OperationAbortedException(Strings.ADP_OperationAbortedExceptionMessage, inner) : new OperationAbortedException(Strings.ADP_OperationAborted, null));
		ADP.TraceExceptionAsReturnValue(ex);
		return ex;
	}
}
