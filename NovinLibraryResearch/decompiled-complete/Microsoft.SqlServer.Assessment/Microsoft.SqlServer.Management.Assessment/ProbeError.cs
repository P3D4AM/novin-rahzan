using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

[DebuggerDisplay("Error {Message}")]
public class ProbeError : IAssessmentError, IAssessmentResult
{
	public string Message { get; }

	public IReadOnlyDictionary<string, Exception> InnerProbeExceptions { get; }

	public ICheck Check { get; }

	public string TargetPath { get; }

	public SqlObjectType TargetType { get; }

	public IReadOnlyCollection<Exception> InnerExceptions => (IReadOnlyCollection<Exception>)(object)InnerProbeExceptions.Values.ToArray();

	public DateTimeOffset Timestamp { get; }

	protected ProbeError(ICheck check, string message, SqlObjectType targetType, string targetPath)
	{
		Check = check;
		Message = message;
		TargetType = targetType;
		TargetPath = targetPath;
		Timestamp = DateTimeOffset.Now;
	}

	protected ProbeError(ICheck check, string message, IEnumerable<KeyValuePair<string, Exception>> innerExceptions, SqlObjectType targetType, string targetPath)
		: this(check, message, targetType, targetPath)
	{
		InnerProbeExceptions = innerExceptions.ToDictionary((KeyValuePair<string, Exception> p) => p.Key, (KeyValuePair<string, Exception> p) => p.Value);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Message).Append(' ');
		if (Check != null)
		{
			stringBuilder.Append(Resources.StringCheckId).Append(": ").Append(Check.Id)
				.Append(". ");
		}
		if (TargetPath != null)
		{
			stringBuilder.Append(Resources.StringTarget).Append(": ").Append(TargetPath)
				.Append(". ");
		}
		foreach (KeyValuePair<string, Exception> innerProbeException in InnerProbeExceptions)
		{
			stringBuilder.Append(innerProbeException.Key).Append(": ").Append(innerProbeException.Value.Message)
				.Append(' ');
		}
		stringBuilder.Length--;
		return stringBuilder.ToString();
	}

	internal static ProbeError Create(Exception exception, Check check, string probeName, SqlObjectType targetType, string targetPath)
	{
		string message = string.Format(CultureInfo.CurrentCulture, Resources.ErrorFailedObtainProbeData, check.Id, probeName, exception.Message);
		KeyValuePair<string, Exception> keyValuePair = new KeyValuePair<string, Exception>(probeName, exception);
		if (exception is ProbeRequirementsException ex)
		{
			return new ProbeRequirementsError(check, message, new KeyValuePair<string, Exception>[1] { keyValuePair }, targetType, targetPath)
			{
				MissingDependencies = ex.MissingDependencies
			};
		}
		return new ProbeError(check, message, new KeyValuePair<string, Exception>[1] { keyValuePair }, targetType, targetPath);
	}
}
