using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Antlr.Runtime;

[Serializable]
public class UnwantedTokenException : MismatchedTokenException
{
	public virtual IToken UnexpectedToken => base.Token;

	public UnwantedTokenException()
	{
	}

	public UnwantedTokenException(string message)
		: base(message)
	{
	}

	public UnwantedTokenException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public UnwantedTokenException(int expecting, IIntStream input)
		: base(expecting, input)
	{
	}

	public UnwantedTokenException(int expecting, IIntStream input, IList<string> tokenNames)
		: base(expecting, input, tokenNames)
	{
	}

	public UnwantedTokenException(string message, int expecting, IIntStream input, IList<string> tokenNames)
		: base(message, expecting, input, tokenNames)
	{
	}

	public UnwantedTokenException(string message, int expecting, IIntStream input, IList<string> tokenNames, Exception innerException)
		: base(message, expecting, input, tokenNames, innerException)
	{
	}

	protected UnwantedTokenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override string ToString()
	{
		string text = ((base.TokenNames != null && base.Expecting >= 0 && base.Expecting < base.TokenNames.Count) ? base.TokenNames[base.Expecting] : base.Expecting.ToString());
		string text2 = ", expected " + text;
		if (base.Expecting == 0)
		{
			text2 = "";
		}
		if (base.Token == null)
		{
			return "UnwantedTokenException(found=" + text2 + ")";
		}
		return "UnwantedTokenException(found=" + base.Token.Text + text2 + ")";
	}
}
