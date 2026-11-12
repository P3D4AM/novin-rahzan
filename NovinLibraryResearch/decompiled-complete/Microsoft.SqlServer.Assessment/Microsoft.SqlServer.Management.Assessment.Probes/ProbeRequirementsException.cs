using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

[Serializable]
public class ProbeRequirementsException : SqlAssessmentProbeException
{
	internal CharacteristicCollection MissingDependencies { get; set; }

	public ProbeRequirementsException(string message)
		: base(message)
	{
	}

	public ProbeRequirementsException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ProbeRequirementsException()
	{
	}

	protected ProbeRequirementsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
