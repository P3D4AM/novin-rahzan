using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime;

[Serializable]
public class MismatchedTokenException : RecognitionException
{
	private readonly int _expecting;

	private readonly ReadOnlyCollection<string> _tokenNames;

	public int Expecting => _expecting;

	public ReadOnlyCollection<string> TokenNames => _tokenNames;

	public MismatchedTokenException()
	{
	}

	public MismatchedTokenException(string message)
		: base(message)
	{
	}

	public MismatchedTokenException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public MismatchedTokenException(int expecting, IIntStream input)
		: this(expecting, input, null)
	{
	}

	public MismatchedTokenException(int expecting, IIntStream input, IList<string> tokenNames)
		: base(input)
	{
		_expecting = expecting;
		if (tokenNames != null)
		{
			_tokenNames = new ReadOnlyCollection<string>(new List<string>(tokenNames));
		}
	}

	public MismatchedTokenException(string message, int expecting, IIntStream input, IList<string> tokenNames)
		: base(message, input)
	{
		_expecting = expecting;
		if (tokenNames != null)
		{
			_tokenNames = new ReadOnlyCollection<string>(new List<string>(tokenNames));
		}
	}

	public MismatchedTokenException(string message, int expecting, IIntStream input, IList<string> tokenNames, Exception innerException)
		: base(message, input, innerException)
	{
		_expecting = expecting;
		if (tokenNames != null)
		{
			_tokenNames = new ReadOnlyCollection<string>(new List<string>(tokenNames));
		}
	}

	protected MismatchedTokenException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		_expecting = info.GetInt32("Expecting");
		_tokenNames = new ReadOnlyCollection<string>((string[])info.GetValue("TokenNames", typeof(string[])));
	}

	[SecurityCritical]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("Expecting", _expecting);
		info.AddValue("TokenNames", (_tokenNames != null) ? new List<string>(_tokenNames).ToArray() : null);
	}

	public override string ToString()
	{
		int unexpectedType = UnexpectedType;
		string text = ((TokenNames != null && unexpectedType >= 0 && unexpectedType < TokenNames.Count) ? TokenNames[unexpectedType] : unexpectedType.ToString());
		string text2 = ((TokenNames != null && Expecting >= 0 && Expecting < TokenNames.Count) ? TokenNames[Expecting] : Expecting.ToString());
		return "MismatchedTokenException(" + text + "!=" + text2 + ")";
	}
}
