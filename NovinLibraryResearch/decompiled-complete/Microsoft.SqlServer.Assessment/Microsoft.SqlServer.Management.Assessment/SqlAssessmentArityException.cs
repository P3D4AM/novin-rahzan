using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentArityException : SqlAssessmentEvaluationException
{
	public SqlAssessmentArityException()
	{
	}

	public SqlAssessmentArityException(string message)
		: base(message)
	{
	}

	public SqlAssessmentArityException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentArityException(int expectedArity, int actualArgumentsCount)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalArity, expectedArity, actualArgumentsCount))
	{
	}

	protected SqlAssessmentArityException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
