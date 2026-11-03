using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace DevComponents.Schedule;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public class InvalidTimeZoneException : Exception
{
	public InvalidTimeZoneException()
	{
	}

	public InvalidTimeZoneException(string message)
		: base(message)
	{
	}

	protected InvalidTimeZoneException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InvalidTimeZoneException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
