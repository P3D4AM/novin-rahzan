using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

[Serializable]
public class SqlAssessmentProbeException : Exception
{
	public SqlAssessmentProbeException()
	{
	}

	protected SqlAssessmentProbeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public SqlAssessmentProbeException(string message)
		: base(message)
	{
	}

	public SqlAssessmentProbeException(string message, object arg)
		: base(string.Format(CultureInfo.CurrentCulture, message, arg))
	{
	}

	public SqlAssessmentProbeException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentProbeException(Exception innerException, string message, params object[] args)
		: base(string.Format(CultureInfo.CurrentCulture, message, args), innerException)
	{
	}
}
