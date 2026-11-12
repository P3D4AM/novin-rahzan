using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteEarlyExitException : RewriteCardinalityException
{
	public RewriteEarlyExitException()
	{
	}

	public RewriteEarlyExitException(string elementDescription)
		: base(elementDescription)
	{
	}

	public RewriteEarlyExitException(string elementDescription, Exception innerException)
		: base(elementDescription, innerException)
	{
	}

	public RewriteEarlyExitException(string message, string elementDescription)
		: base(message, elementDescription)
	{
	}

	public RewriteEarlyExitException(string message, string elementDescription, Exception innerException)
		: base(message, elementDescription, innerException)
	{
	}

	protected RewriteEarlyExitException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
