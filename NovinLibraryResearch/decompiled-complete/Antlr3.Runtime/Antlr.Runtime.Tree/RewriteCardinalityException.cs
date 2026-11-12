using System;
using System.Runtime.Serialization;

namespace Antlr.Runtime.Tree;

[Serializable]
public class RewriteCardinalityException : Exception
{
	private readonly string _elementDescription;

	public RewriteCardinalityException()
	{
	}

	public RewriteCardinalityException(string elementDescription)
		: this(elementDescription, elementDescription)
	{
		_elementDescription = elementDescription;
	}

	public RewriteCardinalityException(string elementDescription, Exception innerException)
		: this(elementDescription, elementDescription, innerException)
	{
	}

	public RewriteCardinalityException(string message, string elementDescription)
		: base(message)
	{
		_elementDescription = elementDescription;
	}

	public RewriteCardinalityException(string message, string elementDescription, Exception innerException)
		: base(message, innerException)
	{
		_elementDescription = elementDescription;
	}

	protected RewriteCardinalityException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_elementDescription = info.GetString("ElementDescription");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("ElementDescription", _elementDescription);
	}
}
