using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

internal class RequirementsWarning : IAssessmentWarning, IAssessmentResult
{
	public string Message { get; }

	public ICheck Check { get; }

	public string TargetPath { get; }

	public SqlObjectType TargetType { get; }

	public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;


	public IReadOnlyList<string> CheckIds { get; }

	public RequirementsWarning(SqlObjectType targetType, string targetPath, string message, IReadOnlyList<string> checkIds)
	{
		CheckIds = checkIds;
		TargetType = targetType;
		TargetPath = targetPath;
		Message = message;
	}
}
