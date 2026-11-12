using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

[Serializable]
public class OperationException : Exception
{
	public OperationException()
	{
	}

	public OperationException([JetBrains.Annotations.NotNull] string message)
		: base(message)
	{
	}

	public OperationException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.CanBeNull] Exception innerException)
		: base(message, innerException)
	{
	}

	public OperationException([JetBrains.Annotations.NotNull] SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
