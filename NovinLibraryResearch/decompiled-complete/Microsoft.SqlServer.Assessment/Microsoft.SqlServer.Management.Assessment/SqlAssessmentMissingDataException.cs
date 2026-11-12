using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[Serializable]
public class SqlAssessmentMissingDataException : SqlAssessmentEvaluationException
{
	public SqlAssessmentMissingDataException()
		: base(Resources.ErrorEvalMissingData)
	{
	}

	public SqlAssessmentMissingDataException(string dataItemName)
		: base(string.Format(CultureInfo.CurrentCulture, Resources.ErrorEvalMissingData, dataItemName))
	{
	}

	public SqlAssessmentMissingDataException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SqlAssessmentMissingDataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
