using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentTypeMismatchException : SqlAssessmentEvaluationException
{
	public SqlAssessmentTypeMismatchException()
	{
	}

	public SqlAssessmentTypeMismatchException(string message)
		: base(message)
	{
	}

	public SqlAssessmentTypeMismatchException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentTypeMismatchException(Type expected, Type actual, object value)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalTypeMismatch, value, actual, expected))
	{
	}

	public SqlAssessmentTypeMismatchException(Type expected, Type actual, object value, Exception innerException)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalTypeMismatch, value, actual, expected), innerException)
	{
	}

	protected SqlAssessmentTypeMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
