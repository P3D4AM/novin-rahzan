using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentOverflowException : SqlAssessmentEvaluationException
{
	public SqlAssessmentOverflowException()
	{
	}

	public SqlAssessmentOverflowException(string message)
		: base(message)
	{
	}

	public SqlAssessmentOverflowException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentOverflowException(Type type)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalOverflow, type))
	{
	}

	public SqlAssessmentOverflowException(Type type, Exception innerException)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalOverflow, type), innerException)
	{
	}

	protected SqlAssessmentOverflowException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
