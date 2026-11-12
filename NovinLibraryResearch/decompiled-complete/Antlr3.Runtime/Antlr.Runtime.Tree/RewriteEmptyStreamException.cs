using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteEmptyStreamException : RewriteCardinalityException
{
	public RewriteEmptyStreamException()
	{
	}

	public RewriteEmptyStreamException(string elementDescription)
		: base(elementDescription)
	{
	}

	public RewriteEmptyStreamException(string elementDescription, Exception innerException)
		: base(elementDescription, innerException)
	{
	}

	public RewriteEmptyStreamException(string message, string elementDescription)
		: base(message, elementDescription)
	{
	}

	public RewriteEmptyStreamException(string message, string elementDescription, Exception innerException)
		: base(message, elementDescription, innerException)
	{
	}

	protected RewriteEmptyStreamException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
