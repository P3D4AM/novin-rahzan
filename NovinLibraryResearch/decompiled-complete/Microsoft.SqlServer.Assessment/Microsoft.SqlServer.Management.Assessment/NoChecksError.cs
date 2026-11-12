using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[DebuggerDisplay("Target unsupported warning")]
public class NoChecksError : IAssessmentError, IAssessmentResult
{
	public string TargetPath => Target.Urn;

	public SqlObjectType TargetType => Target.Type;

	public string Message => Resources.UnsupportedTargetCheckDisplayName;

	public ICheck Check => null;

	public string HelpLink { get; }

	public SeverityLevel Severity { get; }

	public IReadOnlyCollection<Exception> InnerExceptions => (IReadOnlyCollection<Exception>)(object)Array.Empty<Exception>();

	private ISqlObjectLocator Target { get; }

	public DateTimeOffset Timestamp { get; }

	public NoChecksError(ISqlObjectLocator target)
	{
		Severity = SeverityLevel.Warning;
		Target = target;
		Timestamp = DateTimeOffset.Now;
	}

	public override string ToString()
	{
		return Message + " " + Resources.StringTarget + ": " + TargetPath;
	}
}
