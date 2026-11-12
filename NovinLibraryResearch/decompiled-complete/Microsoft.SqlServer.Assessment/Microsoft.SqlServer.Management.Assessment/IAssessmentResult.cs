using System;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IAssessmentResult
{
	string Message { get; }

	ICheck Check { get; }

	string TargetPath { get; }

	SqlObjectType TargetType { get; }

	DateTimeOffset Timestamp { get; }
}
