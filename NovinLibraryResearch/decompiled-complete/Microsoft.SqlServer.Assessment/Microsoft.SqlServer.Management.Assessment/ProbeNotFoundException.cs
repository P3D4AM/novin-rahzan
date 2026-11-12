using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class ProbeNotFoundException : Exception
{
	public ProbeNotFoundException()
	{
	}

	public ProbeNotFoundException(string message)
		: base(message)
	{
	}

	public ProbeNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ProbeNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
