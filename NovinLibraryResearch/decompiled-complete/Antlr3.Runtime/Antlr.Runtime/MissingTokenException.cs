using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class MissingTokenException : MismatchedTokenException
{
	private readonly object _inserted;

	public virtual int MissingType => base.Expecting;

	public MissingTokenException()
	{
	}

	public MissingTokenException(string message)
		: base(message)
	{
	}

	public MissingTokenException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MissingTokenException(int expecting, IIntStream input, object inserted)
		: this(expecting, input, inserted, null)
	{
	}

	public MissingTokenException(int expecting, IIntStream input, object inserted, IList<string> tokenNames)
		: base(expecting, input, tokenNames)
	{
		_inserted = inserted;
	}

	public MissingTokenException(string message, int expecting, IIntStream input, object inserted, IList<string> tokenNames)
		: base(message, expecting, input, tokenNames)
	{
		_inserted = inserted;
	}

	public MissingTokenException(string message, int expecting, IIntStream input, object inserted, IList<string> tokenNames, Exception innerException)
		: base(message, expecting, input, tokenNames, innerException)
	{
		_inserted = inserted;
	}

	protected MissingTokenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override string ToString()
	{
		if (_inserted != null && base.Token != null)
		{
			return string.Concat("MissingTokenException(inserted ", _inserted, " at ", base.Token.Text, ")");
		}
		if (base.Token != null)
		{
			return "MissingTokenException(at " + base.Token.Text + ")";
		}
		return "MissingTokenException";
	}
}
