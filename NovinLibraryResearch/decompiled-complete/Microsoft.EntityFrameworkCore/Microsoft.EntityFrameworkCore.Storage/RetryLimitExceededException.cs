using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

[Serializable]
public class RetryLimitExceededException : Exception
{
	public RetryLimitExceededException()
	{
	}

	public RetryLimitExceededException([JetBrains.Annotations.NotNull] string message)
		: base(message)
	{
	}

	public RetryLimitExceededException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.NotNull] Exception innerException)
		: base(message, innerException)
	{
	}

	public RetryLimitExceededException([JetBrains.Annotations.NotNull] SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
