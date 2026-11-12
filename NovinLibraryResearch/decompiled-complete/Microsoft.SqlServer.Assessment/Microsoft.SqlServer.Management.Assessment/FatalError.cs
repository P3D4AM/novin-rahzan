using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

public class FatalError : IAssessmentError, IAssessmentResult
{
	public string Message { get; }

	public ICheck Check { get; }

	public string TargetPath => Target.Urn;

	public SqlObjectType TargetType => Target.Type;

	public IReadOnlyCollection<Exception> InnerExceptions { get; }

	private ISqlObjectLocator Target { get; }

	public DateTimeOffset Timestamp { get; }

	internal FatalError(ISqlObjectLocator target, string message)
		: this(target, message, Array.Empty<Exception>())
	{
	}

	internal FatalError(ISqlObjectLocator target, string message, params Exception[] innerExceptions)
	{
		Target = target;
		Message = message;
		Check = null;
		Timestamp = DateTimeOffset.Now;
		Exception[] array = new Exception[innerExceptions.Length];
		innerExceptions.CopyTo(array, 0);
		InnerExceptions = (IReadOnlyCollection<Exception>)(object)array;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Message).Append(' ');
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
