using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentProbeParameterException : SqlAssessmentEvaluationException
{
	public string ParameterName { get; }

	public SqlAssessmentProbeParameterException()
	{
	}

	public SqlAssessmentProbeParameterException(string message)
		: base(message)
	{
	}

	public SqlAssessmentProbeParameterException(IProbe probe, string parameterName)
		: this(Resources.ErrorEvalWrongProbeParameter, probe, parameterName)
	{
	}

	public SqlAssessmentProbeParameterException(string message, IProbe probe, string parameterName)
		: base(string.Format(CultureInfo.CurrentCulture, message, probe?.FamilyName ?? string.Empty, parameterName))
	{
	}

	public SqlAssessmentProbeParameterException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentProbeParameterException(string message, string parameterName)
		: base(message)
	{
		ParameterName = parameterName;
	}

	protected SqlAssessmentProbeParameterException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
