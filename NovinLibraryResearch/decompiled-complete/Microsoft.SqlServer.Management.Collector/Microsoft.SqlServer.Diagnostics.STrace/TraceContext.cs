#define TRACE
using System;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.SqlServer.Diagnostics.STrace;

internal class TraceContext : IDisposable
{
	public static TraceContext GetTraceContext(string componentName, string s)
	{
		return new TraceContext();
	}

	public ActivityContext GetActivityContext(string s)
	{
		return new ActivityContext(s);
	}

	public MethodTraceContext GetMethodContext(string s)
	{
		return new MethodTraceContext(s);
	}

	public MethodTraceContext GetMethodContext(string methodName, TraceEventType traceEventType)
	{
		return new MethodTraceContext(methodName);
	}

	public void Assert(bool condition, string message)
	{
	}

	public void Dispose()
	{
	}

	public void TraceActivityStart(string s)
	{
		Trace.TraceInformation("TraceActivityStart - {0}", s);
	}

	public void TraceActivityEnd(string s)
	{
		Trace.TraceInformation("TraceActivityEnd - {0}", s);
	}

	public void TraceCatch(Exception ex)
	{
		Trace.TraceError("TraceCatch - {0}", ex);
	}

	public void TraceInformation(string format, params object[] info)
	{
		Trace.TraceInformation("TraceInformation - ", string.Format(format, info));
	}

	public Exception TraceThrow(Exception exception)
	{
		Trace.TraceInformation("TraceThrow - {0}", exception);
		return exception;
	}

	public Exception TraceThrow(Exception exception, TraceEventType traceEventType)
	{
		Trace.TraceInformation("TraceThrow - {0}:{1}", exception, traceEventType);
		return exception;
	}

	public void TraceVerbose(string message)
	{
		Trace.TraceInformation("TraceVerbose - {0}", message);
	}

	public void TraceVerbose(int id, string message)
	{
		Trace.TraceInformation("TraceVerbose - {0}:{1}", id, message);
	}

	public void TraceVerbose(string format, params object[] args)
	{
		Trace.TraceInformation("TraceVerbose - " + string.Format(format, args));
	}

	public void TraceVerbose(int id, string format, params object[] args)
	{
		Trace.TraceInformation("TraceVerbose - {0}" + string.Format(format, args), id);
	}

	public virtual void TraceParameterIn(string parameterName, object parameterValue)
	{
		Trace.TraceInformation("Parameter in:{0}:{1}", parameterName, parameterValue);
	}

	public virtual void TraceParameters(params object[] parameterValues)
	{
		Trace.TraceInformation(string.Format("TraceParameters - {0}", string.Join(",", parameterValues.Select((object traceValue) => traceValue ?? "").ToArray())));
	}

	protected void TraceParameters(TraceEventType traceEventType, params object[] parameterValues)
	{
		Trace.TraceInformation(string.Format("TraceParameters - {0}:{1}", traceEventType, string.Join(",", parameterValues.Select((object traceValue) => traceValue ?? "").ToArray())));
	}

	public void TraceMethodEnter(string methodName)
	{
		Trace.TraceInformation("MethodEnter - {0}", methodName);
	}

	public void TraceMethodExit(string methodName)
	{
		Trace.TraceInformation("MethodExit - {0}", methodName);
	}

	public void DebugAssert(bool test)
	{
	}

	public void DebugAssert(bool test, string message)
	{
	}

	public void DebugAssert(bool test, int id, string message)
	{
	}

	public void DebugAssert(bool test, string format, params object[] args)
	{
	}

	public void DebugAssert(bool test, int id, string format, params object[] args)
	{
	}

	public void TraceError(string message)
	{
		Trace.TraceError("TraceError - {0}", message);
	}

	public void TraceError(int id, string message)
	{
		Trace.TraceError("TraceError - {0}:{1}", id, message);
	}

	public void TraceError(string format, params object[] args)
	{
		Trace.TraceError("TraceError - " + string.Format(format, args));
	}

	public void TraceError(int id, string format, params object[] args)
	{
		Trace.TraceError("TraceError - {0}" + string.Format(format, args), id);
	}

	public virtual void TraceParameterOut(string parameterName, object parameterValue)
	{
		Trace.TraceInformation("TraceParameterOut - {0}:{1}", parameterName, parameterValue);
	}
}
