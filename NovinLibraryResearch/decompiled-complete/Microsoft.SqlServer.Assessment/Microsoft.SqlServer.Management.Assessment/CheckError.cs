using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[DebuggerDisplay("Error {Message}")]
public class CheckError : IAssessmentError, IAssessmentResult
{
	public ICheck Check { get; }

	public string TargetPath { get; }

	public SqlObjectType TargetType { get; }

	public string Message { get; }

	public IReadOnlyCollection<Exception> InnerExceptions { get; }

	public DateTimeOffset Timestamp { get; }

	public CheckError(ICheck check, string message, SqlObjectType targetType, string targetPath)
	{
		Check = check ?? throw new ArgumentNullException("check");
		Message = message;
		TargetType = targetType;
		TargetPath = targetPath;
		Timestamp = DateTimeOffset.Now;
	}

	public CheckError(ICheck check, string message, Exception innerException, SqlObjectType targetType, string targetPath)
		: this(check, message, targetType, targetPath)
	{
		InnerExceptions = (IReadOnlyCollection<Exception>)(object)new Exception[1] { innerException };
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Message).Append(' ').Append(Resources.StringCheckId)
			.Append(": ")
			.Append(Check.Id)
			.Append(". ");
		if (TargetPath != null)
		{
			stringBuilder.Append(Resources.StringTarget).Append(": ").Append(TargetPath)
				.Append(". ");
		}
		foreach (Exception innerException in InnerExceptions)
		{
			stringBuilder.Append(innerException.Message).Append(' ');
		}
		stringBuilder.Length--;
		return stringBuilder.ToString();
	}
}
