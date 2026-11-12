namespace Microsoft.SqlServer.Management.Assessment;

public interface IAssessmentNote : IAssessmentResult
{
	string HelpLink { get; }

	SeverityLevel Severity { get; }
}
