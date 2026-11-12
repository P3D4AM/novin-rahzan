using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentFormatException : SqlAssessmentEvaluationException
{
	public SqlAssessmentFormatException()
	{
	}

	public SqlAssessmentFormatException(string message)
		: base(message)
	{
	}

	public SqlAssessmentFormatException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public SqlAssessmentFormatException(Type type, string value)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalStringFormat, value, type?.Name ?? string.Empty))
	{
	}

	public SqlAssessmentFormatException(Type type, string value, Exception innerException)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalStringFormat, value, type?.Name ?? string.Empty) + innerException?.Message, innerException)
	{
	}

	protected SqlAssessmentFormatException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
