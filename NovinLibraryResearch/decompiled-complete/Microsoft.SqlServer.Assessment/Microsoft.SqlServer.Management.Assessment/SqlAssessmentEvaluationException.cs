using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentEvaluationException : Exception
{
	public SqlAssessmentEvaluationException()
	{
	}

	public SqlAssessmentEvaluationException(string message)
		: base(message)
	{
	}

	public SqlAssessmentEvaluationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SqlAssessmentEvaluationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
