using System;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment;

[DebuggerDisplay("Note for {Check.Id}")]
internal class AssessmentNote : IAssessmentNote, IAssessmentResult
{
	public string Message { get; }

	public ICheck Check { get; }

	public string TargetPath { get; }

	public SqlObjectType TargetType { get; }

	public string HelpLink { get; }

	public SeverityLevel Severity { get; }

	public DateTimeOffset Timestamp { get; }

	public AssessmentNote(ICheck check, string message, string targetPath, SqlObjectType targetType, string helpLink, SeverityLevel severity)
	{
		Check = check;
		Message = message;
		TargetPath = targetPath;
		TargetType = targetType;
		HelpLink = helpLink;
		Severity = severity;
		Timestamp = DateTimeOffset.Now;
	}
}
