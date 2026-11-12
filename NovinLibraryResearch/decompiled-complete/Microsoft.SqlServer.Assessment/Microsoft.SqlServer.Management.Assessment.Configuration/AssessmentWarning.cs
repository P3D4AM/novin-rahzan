using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class AssessmentWarning : IAssessmentWarning, IAssessmentResult
{
	public string Message { get; }

	public ICheck Check { get; }

	public string TargetPath { get; }

	public SqlObjectType TargetType { get; }

	public DateTimeOffset Timestamp { get; }

	public AssessmentWarning(Check check, SqlObjectType targetType, string targetPath, string message)
	{
		Check = check;
		TargetType = targetType;
		TargetPath = targetPath;
		Message = message;
		Timestamp = DateTimeOffset.Now;
	}
}
